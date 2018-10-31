namespace NetManager
{
    partial class FormClient
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.bRestart = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.tbName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbIPServer = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.nPort = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.lClients = new System.Windows.Forms.Label();
            this.chClients = new System.Windows.Forms.CheckedListBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.button2 = new System.Windows.Forms.Button();
            this.tbSend = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tbReseive = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nPort)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.bRestart);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.tbName);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.tbIPServer);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.nPort);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(635, 122);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = " Сетевые настройки ";
            // 
            // bRestart
            // 
            this.bRestart.Enabled = false;
            this.bRestart.Location = new System.Drawing.Point(269, 81);
            this.bRestart.Name = "bRestart";
            this.bRestart.Size = new System.Drawing.Size(163, 23);
            this.bRestart.TabIndex = 7;
            this.bRestart.Text = "Перезапустить сервер";
            this.bRestart.UseVisualStyleBackColor = true;
            this.bRestart.Click += new System.EventHandler(this.bRestart_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(148, 81);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(107, 23);
            this.button1.TabIndex = 6;
            this.button1.Text = "Подключиться";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // tbName
            // 
            this.tbName.Location = new System.Drawing.Point(12, 84);
            this.tbName.MaxLength = 10;
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(120, 20);
            this.tbName.TabIndex = 5;
            this.tbName.Text = "Client";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 68);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Название";
            // 
            // tbIPServer
            // 
            this.tbIPServer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbIPServer.Location = new System.Drawing.Point(140, 37);
            this.tbIPServer.Name = "tbIPServer";
            this.tbIPServer.Size = new System.Drawing.Size(484, 20);
            this.tbIPServer.TabIndex = 3;
            this.tbIPServer.Text = "192.0.0.1";
            this.tbIPServer.Leave += new System.EventHandler(this.tbIPServer_Leave);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(145, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "IP адрес сервера";
            // 
            // nPort
            // 
            this.nPort.Location = new System.Drawing.Point(12, 38);
            this.nPort.Maximum = new decimal(new int[] {
            65536,
            0,
            0,
            0});
            this.nPort.Minimum = new decimal(new int[] {
            1023,
            0,
            0,
            0});
            this.nPort.Name = "nPort";
            this.nPort.Size = new System.Drawing.Size(120, 20);
            this.nPort.TabIndex = 1;
            this.nPort.Value = new decimal(new int[] {
            8000,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Порт";
            // 
            // lClients
            // 
            this.lClients.AutoSize = true;
            this.lClients.Dock = System.Windows.Forms.DockStyle.Top;
            this.lClients.Location = new System.Drawing.Point(0, 122);
            this.lClients.Name = "lClients";
            this.lClients.Size = new System.Drawing.Size(66, 13);
            this.lClients.TabIndex = 1;
            this.lClients.Text = "Клиенты (0)";
            // 
            // chClients
            // 
            this.chClients.Dock = System.Windows.Forms.DockStyle.Top;
            this.chClients.FormattingEnabled = true;
            this.chClients.HorizontalScrollbar = true;
            this.chClients.Location = new System.Drawing.Point(0, 135);
            this.chClients.Name = "chClients";
            this.chClients.Size = new System.Drawing.Size(635, 109);
            this.chClients.TabIndex = 2;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 244);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.button2);
            this.splitContainer1.Panel1.Controls.Add(this.tbSend);
            this.splitContainer1.Panel1.Controls.Add(this.label4);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tbReseive);
            this.splitContainer1.Panel2.Controls.Add(this.label5);
            this.splitContainer1.Size = new System.Drawing.Size(635, 250);
            this.splitContainer1.SplitterDistance = 211;
            this.splitContainer1.TabIndex = 3;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(68, 215);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 2;
            this.button2.Text = "Ага";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // tbSend
            // 
            this.tbSend.Dock = System.Windows.Forms.DockStyle.Top;
            this.tbSend.Location = new System.Drawing.Point(0, 13);
            this.tbSend.Multiline = true;
            this.tbSend.Name = "tbSend";
            this.tbSend.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbSend.Size = new System.Drawing.Size(211, 189);
            this.tbSend.TabIndex = 1;
            this.tbSend.WordWrap = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Top;
            this.label4.Location = new System.Drawing.Point(0, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(61, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Отправить";
            // 
            // tbReseive
            // 
            this.tbReseive.BackColor = System.Drawing.SystemColors.Window;
            this.tbReseive.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbReseive.Location = new System.Drawing.Point(0, 13);
            this.tbReseive.Multiline = true;
            this.tbReseive.Name = "tbReseive";
            this.tbReseive.ReadOnly = true;
            this.tbReseive.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbReseive.Size = new System.Drawing.Size(420, 237);
            this.tbReseive.TabIndex = 1;
            this.tbReseive.WordWrap = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Dock = System.Windows.Forms.DockStyle.Top;
            this.label5.Location = new System.Drawing.Point(0, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(129, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "Полученные сообщения";
            // 
            // FormClient
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(635, 494);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.chClients);
            this.Controls.Add(this.lClients);
            this.Controls.Add(this.groupBox1);
            this.Name = "FormClient";
            this.Text = "NMClient";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormClient_FormClosed);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nPort)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.NumericUpDown nPort;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbIPServer;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox tbName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lClients;
        private System.Windows.Forms.CheckedListBox chClients;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox tbSend;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbReseive;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button bRestart;
    }
}

