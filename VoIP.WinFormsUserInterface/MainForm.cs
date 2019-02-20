using LumiSoft.Net.Media;
using LumiSoft.Net.Media.Codec.Audio;
using LumiSoft.Net.RTP;
using System;
using System.Collections.Generic;
using System.Net;
using System.Windows.Forms;
using VoIP.TcpSignalizationLibrary;
using VoIP.WinFormsUserInterface.Helpers;
using VoIP.WinFormsUserInterface.Panels;
using VoIP.WinFormsUserInterface.Properties;

namespace VoIP.WinFormsUserInterface
{
    public partial class MainForm : Form
    {
        private const string FORM_TITLE = "VoIP Grzegorz Juszczak";

        private bool micMute = false;
        private bool speakerMute = false;

        private TcpSignalizationListener listener;
        private List<TcpSignalizationClient> clients = new List<TcpSignalizationClient>();

        private AudioInDevice audioIn = null;
        private AudioOutDevice audioOut = null;

        private RTP_Session sessionRtp = null;
        private AudioIn_RTP audioInRtp = null;
        private AudioOut_RTP audioOutRtp = null;

        private bool isBusy = false;

        public MainForm()
        {
            InitializeComponent();
            InitializeAudioButtons();

            Text = string.Format("{0} - {1}", FORM_TITLE, Settings.Default.SignalizationPort);

            listener = new TcpSignalizationListener();
            listener.OnNewClientConnected += listener_OnNewClientConnected;
            listener.StartListening(Settings.Default.SignalizationPort);

            SetMainPanel();
        }

        private void listener_OnNewClientConnected(TcpSignalizationClient obj)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<TcpSignalizationClient>(listener_OnNewClientConnected), obj);
                return;
            }

            obj.OnNewPacketIncome += client_OnNewPacketIncome;
            obj.OnDispose += client_OnDispose;
            clients.Add(obj);
        }

        private void client_OnDispose(TcpSignalizationClient obj)
        {
            try
            {
                BeginInvoke(new Action(() =>
                {
                    clients.Remove(obj);
                }));
            }
            catch { }
        }

        private void client_OnNewPacketIncome(TcpSignalizationClient client, SignalizationPacket packet)
        {
            if (this.AfterPacketIncomeInvoke(client_OnNewPacketIncome, client, packet))
                return;

            if (!isBusy)
            {
                if (packet.Command == SignalizationCommand.Call)
                    SetAnswerPanel(client);
                else
                    client.Send(SignalizationCommand.Error);
            }
            else
            {
                client.Send(SignalizationCommand.Busy);
            }
        }

        private void mainPanel_BeginCall(string hostname, int port)
        {
            SetCallingPanel(hostname, port);
        }

        private void SetAnswerPanel(TcpSignalizationClient client)
        {
            client.OnNewPacketIncome -= client_OnNewPacketIncome;
            client.Send(SignalizationCommand.Ringing);
            isBusy = true;

            AnswerPanel answerPanel = new AnswerPanel(client);
            answerPanel.OnAnsweringResult += answerPanel_OnAnsweringResult;
            SetPanel(answerPanel);
        }

        private void SetCallingPanel(string hostname, int port)
        {
            isBusy = true;
            CallingPanel callingPanel = new CallingPanel(hostname, port);
            callingPanel.OnCallingResult += callingPanel_OnCallingResult;
            SetPanel(callingPanel);
        }

        private void SetConversationPanel(TcpSignalizationClient client)
        {
            ConversationPanel conversationPanel = new ConversationPanel(client);
            conversationPanel.OnConversationFinish += ConversationPanel_OnConversationFinish;
            SetPanel(conversationPanel);
        }

        private void ConversationPanel_OnConversationFinish(TcpSignalizationClient obj)
        {
            sessionRtp.Dispose();
            SetMainPanel();
        }

        private void SetMainPanel()
        {
            isBusy = false;
            MainPanel callPanel = new MainPanel();
            callPanel.BeginCall += mainPanel_BeginCall;
            SetPanel(callPanel);
        }

        private void callingPanel_OnCallingResult(TcpSignalizationClient client, bool success)
        {
            if (!success)
                SetMainPanel();
            else
            {
                StartRtpSession(client.RemoteIp, client.LocalIp, Settings.Default.RtpPort, Settings.Default.RtcpPort);
                SetConversationPanel(client);
            }
        }

        private void answerPanel_OnAnsweringResult(TcpSignalizationClient client, bool success)
        {
            if (!success)
                SetMainPanel();
            else
            {
                StartRtpSession(client.RemoteIp, client.LocalIp, Settings.Default.RtpPort, Settings.Default.RtcpPort);
                SetConversationPanel(client);
            }
        }

        private void StartRtpSession(IPAddress remoteIp, IPAddress localIp, int rtpPort, int rtcpPort)
        {
            Dictionary<int, AudioCodec> m_pAudioCodecs = new Dictionary<int, AudioCodec>
            {
                { 0, new PCMU() }
            };

            RTP_MultimediaSession rtpMultimediaSession = new RTP_MultimediaSession(RTP_Utils.GenerateCNAME());
            sessionRtp = rtpMultimediaSession.CreateSession(new RTP_Address(localIp, rtpPort, rtcpPort), new RTP_Clock(1, 8000));
            sessionRtp.AddTarget(new RTP_Address(remoteIp, rtpPort, rtcpPort));
            sessionRtp.Payload = 0;
            sessionRtp.StreamMode = RTP_StreamMode.SendReceive;

            sessionRtp.NewReceiveStream += delegate (object s, RTP_ReceiveStreamEventArgs e2)
            {
                if (audioOut != null)
                {
                    audioOutRtp = new AudioOut_RTP(audioOut, e2.Stream, m_pAudioCodecs);

                    if (speakerMute == false)
                        audioOutRtp.Start();
                }
            };

            if (audioIn != null)
            {
                audioInRtp = new AudioIn_RTP(audioIn, 10, m_pAudioCodecs, sessionRtp.CreateSendStream());

                if (micMute == false)
                    audioInRtp.Start();
            }

            sessionRtp.Start();
        }

        private void SetPanel(PanelBase panel)
        {
            foreach (IDisposable c in panelLayout.Controls)
                c.Dispose();

            panel.Dock = DockStyle.Fill;

            panelLayout.Controls.Clear();
            panelLayout.Controls.Add(panel);
        }

        private void InitializeAudioButtons()
        {
            // load list of output audio devices
            foreach (AudioOutDevice device in AudioOut.Devices)
            {
                ToolStripMenuItem item = new ToolStripMenuItem(device.Name);
                if (btnSpeaker.DropDownItems.Count == 0)
                {
                    item.Checked = true;
                    audioOut = device;
                }
                item.Tag = device;
                btnSpeaker.DropDownItems.Add(item);
            }

            // load list of input audio devices
            foreach (AudioInDevice device in AudioIn.Devices)
            {
                ToolStripMenuItem item = new ToolStripMenuItem(device.Name);
                if (btnMic.DropDownItems.Count == 0)
                {
                    item.Checked = true;
                    audioIn = device;
                }
                item.Tag = device;
                btnMic.DropDownItems.Add(item);
            }
        }

        private void BtnSpeakerMic_ButtonClick(object sender, EventArgs e)
        {
            if (sender == btnMic)
            {
                micMute = !micMute;
                btnMic.Image = micMute ? Resources.MicrophoneOff : Resources.MicrophoneOn;

                if (audioInRtp != null)
                {
                    if (micMute)
                        audioInRtp.Stop();
                    else
                        audioInRtp.Start();

                }
            }

            if (sender == btnSpeaker)
            {
                speakerMute = !speakerMute;
                btnSpeaker.Image = speakerMute ? Resources.SpeakerOff : Resources.SpeakerOn;

                if (audioOutRtp != null)
                {
                    if (speakerMute)
                        audioOutRtp.Stop();
                    else
                        audioOutRtp.Start();
                }
            }
        }

        private void BtnSpeakerMic_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            ToolStripSplitButton button = (ToolStripSplitButton)sender;

            foreach (ToolStripMenuItem item in button.DropDownItems)
            {
                item.Checked = (item == e.ClickedItem ? true : false);
                if (item.Checked)
                {
                    if (item.Tag is AudioInDevice)
                    {
                        audioIn = (AudioInDevice)item.Tag;
                        if (audioInRtp != null)
                            audioInRtp.AudioInDevice = audioIn;
                    }


                    if (item.Tag is AudioOutDevice)
                    {
                        audioOut = (AudioOutDevice)item.Tag;
                        if (audioOutRtp != null)
                            audioOutRtp.AudioOutDevice = audioOut;
                    }

                }
            }
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {

            listener.StopListening();
            listener.StartListening(Settings.Default.SignalizationPort);
            Text = string.Format("{0} - {1}", FORM_TITLE, Settings.Default.SignalizationPort);
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            using (HelpForm helpForm = new HelpForm())
            {
                helpForm.ShowDialog();
            }
        }
    }
}
