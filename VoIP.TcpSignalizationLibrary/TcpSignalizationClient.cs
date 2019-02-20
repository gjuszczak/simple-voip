using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace VoIP.TcpSignalizationLibrary
{
    public class TcpSignalizationClient : IDisposable
    {
        private const string END = "<<!END!>>";
        
        private object tcpReadWriteSyncObj;

        private TcpClient client;
        private NetworkStream stream;
        private CancellationTokenSource cancellationTokenSrc;
        private Task listenTask;

        private object packetsQueueSyncObj;

        private Queue<SignalizationPacket> packetsToSend = new Queue<SignalizationPacket>();
        private Task writerTask;

        private Task keepAliveTask;

        public TcpSignalizationClient(string ip, int port)
        {
            TcpClient client = new TcpClient();
            client.Connect(ip, port);
            initialize(client);
        }

        public TcpSignalizationClient(TcpClient client)
        {
            initialize(client);
        }

        private void initialize(TcpClient client)
        {
            this.client = client;
            this.stream = client.GetStream();

            tcpReadWriteSyncObj = new object();
            packetsQueueSyncObj = new object();

            cancellationTokenSrc = new CancellationTokenSource();

            listenTask = Task.Factory.StartNew(listenTaskRoutine);
            writerTask = Task.Factory.StartNew(writerTaskRoutine);
            keepAliveTask = Task.Factory.StartNew(keepAliveTaskRoutine);

        }

        public event Action<TcpSignalizationClient, SignalizationPacket> OnNewPacketIncome;
        public event Action<TcpSignalizationClient> OnDispose;

        public IPAddress RemoteIp
        {
            get
            {
                lock(tcpReadWriteSyncObj)
                {
                    IPEndPoint ep = (IPEndPoint)this.client.Client.RemoteEndPoint;
                    return ep.Address;
                }
            }
        }

        public IPAddress LocalIp
        {
            get
            {
                lock (tcpReadWriteSyncObj)
                {
                    IPEndPoint ep = (IPEndPoint)this.client.Client.LocalEndPoint;
                    return ep.Address;
                }
            }
        }

        public string RemoteUri
        {
            get
            {
                lock (tcpReadWriteSyncObj)
                {
                    IPEndPoint ep = (IPEndPoint)this.client.Client.RemoteEndPoint;
                    return string.Format("{0}:{1}", ep.Address, ep.Port);
                }
            }
        }

        private void listenTaskRoutine()
        {
            CancellationToken token = cancellationTokenSrc.Token;

            try
            {
                string buffer = string.Empty;
                using (BinaryReader reader = new BinaryReader(stream))
                {
                    while (!token.IsCancellationRequested)
                    {
                        bool wait = true;

                        lock (tcpReadWriteSyncObj)
                        {
                            if (stream.DataAvailable)
                            {
                                wait = false;

                                buffer += reader.ReadString();

                                int pos;
                                while ((pos = buffer.IndexOf(END)) != -1)
                                {
                                    var packet = SignalizationPacket.FromXml(buffer.Substring(0, pos));
                                    buffer = buffer.Remove(0, pos + END.Length);

                                    while (OnNewPacketIncome == null) Thread.Sleep(100);                                    
                                    OnNewPacketIncome(this, packet);
                                }

                            }
                        }

                        if (wait)
                            Thread.Sleep(100);
                    }
                }
            }
            catch { }
        }

        public void writerTaskRoutine()
        {
            CancellationToken token = cancellationTokenSrc.Token;

            try
            {
                using (BinaryWriter writer = new BinaryWriter(stream))
                {
                    while (!token.IsCancellationRequested)
                    {
                        SignalizationPacket packet;
                        lock (packetsQueueSyncObj)
                        {
                            packet = packetsToSend.Count > 0 ? packetsToSend.Peek() : null;
                        }

                        if (packet == null)
                        {
                            Thread.Sleep(100);
                            continue;
                        }

                        lock (tcpReadWriteSyncObj)
                        {
                            writer.Write(packet.ToXml());
                            writer.Write(END);
                            writer.Flush();
                            stream.Flush();
                        }

                        lock (packetsQueueSyncObj)
                        {
                            packetsToSend.Dequeue();
                        }
                    }
                }
            }
            catch { }
        }

        public void keepAliveTaskRoutine()
        {
            try
            {
                while (true)
                {
                    bool dispose = false;
                    lock (tcpReadWriteSyncObj)
                    {
                        dispose = !IsConnected(client.Client);
                    }

                    if (dispose)
                    {
                        this.Dispose();
                        return;
                    }
                    else
                        Thread.Sleep(1000);
                }
            }
            catch { }
        }

        public void Send(SignalizationCommand c)
        {
            var packet = new SignalizationPacket
            {
                Command = c
            };

            lock (packetsQueueSyncObj)
            {
                packetsToSend.Enqueue(packet);
            }
        }

        public void Dispose()
        {
            if (client == null)
                return;

            OnDispose?.Invoke(this);

            cancellationTokenSrc.Cancel();
            listenTask.Wait();
            writerTask.Wait();
            client.Close();
            cancellationTokenSrc.Dispose();
            cancellationTokenSrc = null;
            client = null;
            listenTask = null;
            stream = null;
            writerTask = null;
        }

        public static bool IsConnected(Socket socket)
        {
            try
            {
                return !(socket.Poll(1, SelectMode.SelectRead) && socket.Available == 0);
            }
            catch (SocketException) { return false; }
        }
    }
}
