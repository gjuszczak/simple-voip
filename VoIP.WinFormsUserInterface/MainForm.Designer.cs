namespace VoIP.WinFormsUserInterface
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.toolBar = new System.Windows.Forms.ToolStrip();
            this.btnSpeaker = new System.Windows.Forms.ToolStripSplitButton();
            this.btnMic = new System.Windows.Forms.ToolStripSplitButton();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.panelLayout = new System.Windows.Forms.Panel();
            this.toolBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolBar
            // 
            this.toolBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnSpeaker,
            this.btnMic,
            this.toolStripButton1});
            this.toolBar.Location = new System.Drawing.Point(0, 0);
            this.toolBar.Name = "toolBar";
            this.toolBar.Size = new System.Drawing.Size(384, 25);
            this.toolBar.Stretch = true;
            this.toolBar.TabIndex = 0;
            this.toolBar.Text = "toolStrip1";
            // 
            // btnSpeaker
            // 
            this.btnSpeaker.Image = global::VoIP.WinFormsUserInterface.Properties.Resources.SpeakerOn;
            this.btnSpeaker.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSpeaker.Name = "btnSpeaker";
            this.btnSpeaker.Size = new System.Drawing.Size(85, 22);
            this.btnSpeaker.Text = "Speakers";
            this.btnSpeaker.ButtonClick += new System.EventHandler(this.BtnSpeakerMic_ButtonClick);
            this.btnSpeaker.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.BtnSpeakerMic_DropDownItemClicked);
            // 
            // btnMic
            // 
            this.btnMic.Image = global::VoIP.WinFormsUserInterface.Properties.Resources.MicrophoneOn;
            this.btnMic.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnMic.Margin = new System.Windows.Forms.Padding(10, 1, 0, 2);
            this.btnMic.Name = "btnMic";
            this.btnMic.Size = new System.Drawing.Size(104, 22);
            this.btnMic.Text = "Microphone";
            this.btnMic.ButtonClick += new System.EventHandler(this.BtnSpeakerMic_ButtonClick);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.Image = global::VoIP.WinFormsUserInterface.Properties.Resources.Help;
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Margin = new System.Windows.Forms.Padding(10, 1, 0, 2);
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(52, 22);
            this.toolStripButton1.Text = "Help";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // panelLayout
            // 
            this.panelLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelLayout.Location = new System.Drawing.Point(0, 25);
            this.panelLayout.Name = "panelLayout";
            this.panelLayout.Size = new System.Drawing.Size(384, 65);
            this.panelLayout.TabIndex = 1;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 90);
            this.Controls.Add(this.panelLayout);
            this.Controls.Add(this.toolBar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.Text = "VoIP Grzegorz Juszczak";
            this.toolBar.ResumeLayout(false);
            this.toolBar.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolBar;
        private System.Windows.Forms.ToolStripSplitButton btnSpeaker;
        private System.Windows.Forms.ToolStripSplitButton btnMic;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.Panel panelLayout;
    }
}

