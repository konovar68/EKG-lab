namespace NetManager
{
    partial class FormServer
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormServer));
            this.label1 = new System.Windows.Forms.Label();
            this.nPort = new System.Windows.Forms.NumericUpDown();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.nTestLive = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.tbIP = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbClients = new System.Windows.Forms.TextBox();
            this.lClients = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.showToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.startToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.CloseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.nPort)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nTestLive)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Номер порта:";
            this.toolTip1.SetToolTip(this.label1, "Номер порта, по которому будет проходить соединение с сервером");
            // 
            // nPort
            // 
            this.nPort.Location = new System.Drawing.Point(12, 35);
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
            this.toolTip1.SetToolTip(this.nPort, "Номер порта, по которому будет проходить соединение с сервером");
            this.nPort.Value = new decimal(new int[] {
            8000,
            0,
            0,
            0});
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.nTestLive);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.tbIP);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.nPort);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(498, 150);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = " Сетевые настройки ";
            // 
            // nTestLive
            // 
            this.nTestLive.Location = new System.Drawing.Point(145, 35);
            this.nTestLive.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.nTestLive.Name = "nTestLive";
            this.nTestLive.Size = new System.Drawing.Size(120, 20);
            this.nTestLive.TabIndex = 6;
            this.nTestLive.Value = new decimal(new int[] {
            3000,
            0,
            0,
            0});
            this.nTestLive.ValueChanged += new System.EventHandler(this.nTestLive_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(142, 19);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(117, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Время проверки (мс):";
            this.toolTip1.SetToolTip(this.label3, "Время, через которое будет проверяться активность клиента");
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(306, 32);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "Старт";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // tbIP
            // 
            this.tbIP.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbIP.BackColor = System.Drawing.SystemColors.Window;
            this.tbIP.Location = new System.Drawing.Point(12, 83);
            this.tbIP.Multiline = true;
            this.tbIP.Name = "tbIP";
            this.tbIP.ReadOnly = true;
            this.tbIP.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbIP.Size = new System.Drawing.Size(474, 54);
            this.tbIP.TabIndex = 3;
            this.toolTip1.SetToolTip(this.tbIP, "Список IP адресов локальной машины, на которой установлен сервер.\r\nИх можно испол" +
                    "ьзовать для соединений клиентов с сервером, если сервер не закрыт шлюзом.\r\n");
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "IP адреса:";
            this.toolTip1.SetToolTip(this.label2, "Список IP адресов локальной машины, на которой установлен сервер.\r\nИх можно испол" +
                    "ьзовать для соединений клиентов с сервером, если сервер не закрыт шлюзом.\r\n");
            // 
            // tbClients
            // 
            this.tbClients.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbClients.Location = new System.Drawing.Point(0, 0);
            this.tbClients.Multiline = true;
            this.tbClients.Name = "tbClients";
            this.tbClients.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbClients.Size = new System.Drawing.Size(498, 156);
            this.tbClients.TabIndex = 3;
            this.toolTip1.SetToolTip(this.tbClients, "Клиенты, которые поключены к серверу");
            // 
            // lClients
            // 
            this.lClients.AutoSize = true;
            this.lClients.Location = new System.Drawing.Point(0, 0);
            this.lClients.Name = "lClients";
            this.lClients.Size = new System.Drawing.Size(104, 13);
            this.lClients.TabIndex = 4;
            this.lClients.Text = "Число клиентов - 0";
            this.toolTip1.SetToolTip(this.lClients, "Клиенты, которые поключены к серверу");
            // 
            // panel1
            // 
            this.panel1.AutoSize = true;
            this.panel1.Controls.Add(this.lClients);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 150);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(498, 13);
            this.panel1.TabIndex = 5;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.tbClients);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 163);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(498, 156);
            this.panel2.TabIndex = 6;
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "NetManager";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showToolStripMenuItem,
            this.startToolStripMenuItem,
            this.toolStripMenuItem1,
            this.CloseToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(128, 76);
            // 
            // showToolStripMenuItem
            // 
            this.showToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.showToolStripMenuItem.Name = "showToolStripMenuItem";
            this.showToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.showToolStripMenuItem.Text = "Показать";
            this.showToolStripMenuItem.Click += new System.EventHandler(this.showToolStripMenuItem_Click);
            // 
            // startToolStripMenuItem
            // 
            this.startToolStripMenuItem.Name = "startToolStripMenuItem";
            this.startToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.startToolStripMenuItem.Text = "Старт";
            this.startToolStripMenuItem.Click += new System.EventHandler(this.button1_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(149, 6);
            // 
            // CloseToolStripMenuItem
            // 
            this.CloseToolStripMenuItem.Name = "CloseToolStripMenuItem";
            this.CloseToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.CloseToolStripMenuItem.Text = "Выход";
            this.CloseToolStripMenuItem.Click += new System.EventHandler(this.CloseToolStripMenuItem_Click);
            // 
            // FormServer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(498, 319);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormServer";
            this.Text = "NMServer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormServer_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormServer_FormClosed);
            this.Load += new System.EventHandler(this.FormServer_Load);
            this.Resize += new System.EventHandler(this.FormServer_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.nPort)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nTestLive)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown nPort;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox tbIP;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox tbClients;
        private System.Windows.Forms.Label lClients;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.NumericUpDown nTestLive;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem startToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem CloseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showToolStripMenuItem;
    }
}

