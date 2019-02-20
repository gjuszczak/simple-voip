using LumiSoft.Net.Media;
using LumiSoft.Net.Media.Codec.Audio;
using LumiSoft.Net.RTP;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Net;
using System.Text;
using System.Windows.Forms;
using VoIP.TcpSignalizationLibrary;
using VoIP.WinFormsUserInterface.Helpers;

namespace VoIP.WinFormsUserInterface.Panels
{
    public partial class AnswerPanel : VoIP.WinFormsUserInterface.Panels.PanelBase
    {
        TcpSignalizationClient servicedClient;

        public AnswerPanel(TcpSignalizationClient client)
            : this()
        {
            this.servicedClient = client;
            this.servicedClient.OnNewPacketIncome += servicedClient_OnNewPacketIncome;
            this.servicedClient.OnDispose += ServicedClient_OnDispose;
            this.labTo.Text = this.servicedClient.RemoteUri;
        }

        private void ServicedClient_OnDispose(TcpSignalizationClient obj)
        {
            try
            {
                this.BeginInvoke(new Action(() =>
                {
                    this.labMessage.Text = "Połączenie zostało niespodziewanie przerwane. Proszę spróbować później.";
                    this.btnAnswer.Visible = false;
                    this.btnReject.Text = "Zakończ.";
                    this.btnReject.Click -= btnReject_Click;
                    this.btnReject.Click += btnCancel_Click;
                }));
            }
            catch { }
        }

        public AnswerPanel()
        {
            InitializeComponent();
        }

        void servicedClient_OnNewPacketIncome(TcpSignalizationClient client, SignalizationPacket packet)
        {
            if (this.AfterPacketIncomeInvoke(servicedClient_OnNewPacketIncome, client, packet))
                return;

            if (packet.Command == SignalizationCommand.End)
            {
                this.labMessage.Text = "Nadawca anulował połączenie.";
                this.btnAnswer.Visible = false;
                this.btnReject.Text = "Zakończ.";

                this.servicedClient.OnDispose -= ServicedClient_OnDispose;
                this.servicedClient.Dispose();

                this.btnReject.Click -= btnReject_Click;
                this.btnReject.Click += btnCancel_Click;
            }

        }

        public event Action<TcpSignalizationClient, bool> OnAnsweringResult;

        private void btnAnswer_Click(object sender, EventArgs e)
        {
            this.servicedClient.Send(SignalizationCommand.Answer);
            this.servicedClient.OnNewPacketIncome -= servicedClient_OnNewPacketIncome;
            OnAnsweringResult(servicedClient, true);
        }

        private void btnReject_Click(object sender, EventArgs e)
        {
            this.servicedClient.Send(SignalizationCommand.Busy);
            this.servicedClient.OnNewPacketIncome -= servicedClient_OnNewPacketIncome;
            OnAnsweringResult(null, false);
        }

        private void btnCancel_Click(object arg1, EventArgs arg2)
        {
            this.servicedClient.OnNewPacketIncome -= servicedClient_OnNewPacketIncome;
            OnAnsweringResult(null, false);
        }
    }
}
