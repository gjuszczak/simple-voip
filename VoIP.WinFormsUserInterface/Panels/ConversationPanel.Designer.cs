namespace VoIP.WinFormsUserInterface.Panels
{
    partial class ConversationPanel
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
            this.components = new System.ComponentModel.Container();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.labTimer = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.labMessage = new System.Windows.Forms.Label();
            this.labTo = new System.Windows.Forms.Label();
            this.ConversationTimer = new System.Windows.Forms.Timer(this.components);
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 123F));
            this.tableLayoutPanel1.Controls.Add(this.labTimer, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnCancel, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.labMessage, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.labTo, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(5);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(384, 65);
            this.tableLayoutPanel1.TabIndex = 5;
            // 
            // labTimer
            // 
            this.labTimer.AutoSize = true;
            this.labTimer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labTimer.Location = new System.Drawing.Point(156, 0);
            this.labTimer.Margin = new System.Windows.Forms.Padding(0);
            this.labTimer.Name = "labTimer";
            this.labTimer.Size = new System.Drawing.Size(104, 26);
            this.labTimer.TabIndex = 4;
            this.labTimer.Text = "00:00:00";
            this.labTimer.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(263, 8);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(3, 3, 10, 3);
            this.btnCancel.Name = "btnCancel";
            this.tableLayoutPanel1.SetRowSpan(this.btnCancel, 2);
            this.btnCancel.Size = new System.Drawing.Size(111, 49);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Stop";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // labMessage
            // 
            this.labMessage.AutoSize = true;
            this.labMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labMessage.Location = new System.Drawing.Point(10, 0);
            this.labMessage.Margin = new System.Windows.Forms.Padding(10, 0, 3, 0);
            this.labMessage.Name = "labMessage";
            this.labMessage.Size = new System.Drawing.Size(143, 26);
            this.labMessage.TabIndex = 2;
            this.labMessage.Text = "Connected";
            this.labMessage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labTo
            // 
            this.labTo.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.labTo, 2);
            this.labTo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labTo.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labTo.Location = new System.Drawing.Point(3, 26);
            this.labTo.Name = "labTo";
            this.labTo.Size = new System.Drawing.Size(254, 39);
            this.labTo.TabIndex = 3;
            this.labTo.Text = "localhost:1234";
            this.labTo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ConversationTimer
            // 
            this.ConversationTimer.Enabled = true;
            this.ConversationTimer.Interval = 1000;
            this.ConversationTimer.Tick += new System.EventHandler(this.ConversationTimer_Tick);
            // 
            // ConversationPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "ConversationPanel";
            this.Size = new System.Drawing.Size(384, 65);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label labMessage;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label labTo;
        private System.Windows.Forms.Label labTimer;
        private System.Windows.Forms.Timer ConversationTimer;
    }
}
