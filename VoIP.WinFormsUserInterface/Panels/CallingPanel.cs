using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VoIP.TcpSignalizationLibrary;
using VoIP.WinFormsUserInterface.Helpers;
using System.Net.Sockets;

namespace VoIP.WinFormsUserInterface.Panels
{
    public partial class CallingPanel : PanelBase
    {
        public CallingPanel()
        {
            InitializeComponent();
        }
        
        private TcpSignalizationClient servicedClient;
        bool isCanceled = false;

        public CallingPanel(string hostname, int port)
            : this()
        {
            this.labTo.Text = string.Format("{0}:{1}", hostname, port);

            Task.Factory.StartNew(() =>
            {
                try
                {
                    TcpClient socket = new TcpClient();
                    socket.Connect(hostname, port);

                    this.BeginInvoke(new Action<TcpClient>(initalizeClient), socket);
                }
                catch { }
            });
        }

        void initalizeClient(TcpClient socket)
        {
            if (isCanceled)
            {
                socket.Close();
            }
            else
            {
                this.servicedClient = new TcpSignalizationClient(socket);
                this.servicedClient.OnNewPacketIncome += client_OnNewPacketIncome;
                this.servicedClient.Send(SignalizationCommand.Call);

                this.servicedClient.OnDispose += ServicedClient_OnDispose;

                this.labTo.Text = servicedClient.RemoteUri;
            }
        }

        private void ServicedClient_OnDispose(TcpSignalizationClient obj)
        {
            try
            {
                this.BeginInvoke(new Action(() =>
                {
                    this.labMessage.Text = "Połączenie zostało niespodziewanie przerwane. Proszę spróbować później.";
                    this.btnCancel.Text = "Zakończ";
                }));
            }
            catch { }
        }

        void client_OnNewPacketIncome(TcpSignalizationClient client, SignalizationPacket packet)
        {
            if (this.AfterPacketIncomeInvoke(client_OnNewPacketIncome, client, packet))
                return;

            if(this.isCanceled)
            {
                this.servicedClient.Send(SignalizationCommand.End);
            }
            if (packet.Command == SignalizationCommand.Answer)
            {
                if (OnCallingResult != null)
                    OnCallingResult(client, true);
            }
            else if (packet.Command == SignalizationCommand.Busy)
            {
                this.labMessage.Text = "Odbiorca jest aktualnie zajęty.";
                this.btnCancel.Text = "Zakończ";
            }
            else if (packet.Command == SignalizationCommand.Ringing)
            {
                this.labMessage.Text = "Dzwonienie...";
            }
        }

        public event Action<TcpSignalizationClient, bool> OnCallingResult;

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.isCanceled = true;

            if (this.servicedClient != null)
                this.servicedClient.Send(SignalizationCommand.End);

            if (OnCallingResult != null)
                OnCallingResult(null, false);
        }        
    }
}
