using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace VoIP.TcpSignalizationLibrary
{
    public class TcpSignalizationListener : IDisposable
    {
        private TcpListener listener;
        private CancellationTokenSource cancellationTokenSrc;
        private Task listenerTask;

        public event Action<TcpSignalizationClient> OnNewClientConnected;

        public void StartListening(int port)
        {
            if (listener != null)
                throw new InvalidOperationException("Tcp signalization listener is already started.");

            listener = new TcpListener(IPAddress.Any, port);
            listener.Start();

            cancellationTokenSrc = new CancellationTokenSource();

            listenerTask = Task.Factory.StartNew(listenTaskRoutine);           
        }

        private void listenTaskRoutine()
        {
            CancellationToken token = cancellationTokenSrc.Token;

            while (!token.IsCancellationRequested)
            {
                if (listener.Pending())
                {
                    TcpSignalizationClient client = new TcpSignalizationClient(listener.AcceptTcpClient());

                    OnNewClientConnected?.BeginInvoke(client, null, null);
                }
                else
                    Thread.Sleep(100);
            }
        }

        public void StopListening()
        {
            if (listener == null)
                return;

            cancellationTokenSrc.Cancel();
            listenerTask.Wait();
            listener.Stop();
            cancellationTokenSrc.Dispose();
            cancellationTokenSrc = null;
            listener = null;
            listenerTask = null;
        }

        public void Dispose()
        {
            StopListening();
        }
    }
}
