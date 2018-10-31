namespace TestServer
{
    partial class Form1
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
            this.label3 = new System.Windows.Forms.Label();
            this.tbName = new System.Windows.Forms.TextBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.btnSendFile = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.cbN = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.nSinCount = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.dgSin = new System.Windows.Forms.DataGridView();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.btnSendSin = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnSaveSin = new System.Windows.Forms.Button();
            this.nCount = new System.Windows.Forms.NumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.nMaxNoise = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.nMinNoise = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.chNoise = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chClients = new System.Windows.Forms.CheckedListBox();
            this.tbIP = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tbPort = new System.Windows.Forms.TextBox();
            this.btnConnect = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.dgFiles = new System.Windows.Forms.DataGridView();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.удалитьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nSinCount)).BeginInit();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgSin)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nCount)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nMaxNoise)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nMinNoise)).BeginInit();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgFiles)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 92);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(32, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Имя:";
            // 
            // tbName
            // 
            this.tbName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbName.Location = new System.Drawing.Point(12, 108);
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(112, 20);
            this.tbName.TabIndex = 8;
            this.tbName.Text = "Test";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 173);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(454, 326);
            this.tabControl1.TabIndex = 13;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.button2);
            this.tabPage1.Controls.Add(this.button1);
            this.tabPage1.Controls.Add(this.dgFiles);
            this.tabPage1.Controls.Add(this.btnSendFile);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(446, 300);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Отправить данные из файла";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // btnSendFile
            // 
            this.btnSendFile.Enabled = false;
            this.btnSendFile.Location = new System.Drawing.Point(8, 18);
            this.btnSendFile.Name = "btnSendFile";
            this.btnSendFile.Size = new System.Drawing.Size(105, 23);
            this.btnSendFile.TabIndex = 3;
            this.btnSendFile.Text = "Пуск";
            this.btnSendFile.UseVisualStyleBackColor = true;
            this.btnSendFile.Click += new System.EventHandler(this.btnSendFile_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.cbN);
            this.tabPage2.Controls.Add(this.label6);
            this.tabPage2.Controls.Add(this.nSinCount);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Controls.Add(this.groupBox4);
            this.tabPage2.Controls.Add(this.btnSendSin);
            this.tabPage2.Controls.Add(this.groupBox3);
            this.tabPage2.Controls.Add(this.groupBox2);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(446, 300);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Сигнал A*Sin(2 * PI * x  * F / N)";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // cbN
            // 
            this.cbN.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbN.FormattingEnabled = true;
            this.cbN.Items.AddRange(new object[] {
            "5000",
            "1000"});
            this.cbN.Location = new System.Drawing.Point(309, 107);
            this.cbN.Name = "cbN";
            this.cbN.Size = new System.Drawing.Size(120, 21);
            this.cbN.TabIndex = 14;
            this.cbN.SelectedIndexChanged += new System.EventHandler(this.cbN_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(306, 90);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(90, 13);
            this.label6.TabIndex = 13;
            this.label6.Text = "Дискретизация:";
            // 
            // nSinCount
            // 
            this.nSinCount.Location = new System.Drawing.Point(309, 63);
            this.nSinCount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nSinCount.Name = "nSinCount";
            this.nSinCount.Size = new System.Drawing.Size(120, 20);
            this.nSinCount.TabIndex = 12;
            this.nSinCount.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nSinCount.ValueChanged += new System.EventHandler(this.nSinCount_ValueChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(306, 47);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(94, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Кол-во синусоид:";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.dgSin);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox4.Location = new System.Drawing.Point(3, 139);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(440, 158);
            this.groupBox4.TabIndex = 10;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Синусоиды";
            // 
            // dgSin
            // 
            this.dgSin.AllowUserToAddRows = false;
            this.dgSin.AllowUserToDeleteRows = false;
            this.dgSin.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgSin.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column3,
            this.Column1,
            this.Column2});
            this.dgSin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgSin.Location = new System.Drawing.Point(3, 16);
            this.dgSin.Name = "dgSin";
            this.dgSin.Size = new System.Drawing.Size(434, 139);
            this.dgSin.TabIndex = 0;
            this.dgSin.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgSin_CellValueChanged);
            // 
            // Column3
            // 
            this.Column3.HeaderText = "Номер";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "А";
            this.Column1.Name = "Column1";
            // 
            // Column2
            // 
            this.Column2.HeaderText = "F";
            this.Column2.Name = "Column2";
            this.Column2.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // btnSendSin
            // 
            this.btnSendSin.Enabled = false;
            this.btnSendSin.Location = new System.Drawing.Point(309, 14);
            this.btnSendSin.Name = "btnSendSin";
            this.btnSendSin.Size = new System.Drawing.Size(120, 23);
            this.btnSendSin.TabIndex = 9;
            this.btnSendSin.Text = "Пуск";
            this.btnSendSin.UseVisualStyleBackColor = true;
            this.btnSendSin.Click += new System.EventHandler(this.btnSendSin_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btnSaveSin);
            this.groupBox3.Controls.Add(this.nCount);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Location = new System.Drawing.Point(157, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(140, 121);
            this.groupBox3.TabIndex = 7;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Сохранить в файл";
            // 
            // btnSaveSin
            // 
            this.btnSaveSin.Location = new System.Drawing.Point(16, 84);
            this.btnSaveSin.Name = "btnSaveSin";
            this.btnSaveSin.Size = new System.Drawing.Size(97, 23);
            this.btnSaveSin.TabIndex = 2;
            this.btnSaveSin.Text = "Сохранить";
            this.btnSaveSin.UseVisualStyleBackColor = true;
            this.btnSaveSin.Click += new System.EventHandler(this.btnSaveSin_Click);
            // 
            // nCount
            // 
            this.nCount.Location = new System.Drawing.Point(9, 39);
            this.nCount.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.nCount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nCount.Name = "nCount";
            this.nCount.Size = new System.Drawing.Size(120, 20);
            this.nCount.TabIndex = 1;
            this.nCount.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(13, 23);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(100, 13);
            this.label10.TabIndex = 0;
            this.label10.Text = "Число элементов:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.nMaxNoise);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.nMinNoise);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.chNoise);
            this.groupBox2.Location = new System.Drawing.Point(7, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(146, 121);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "       Белый шум";
            // 
            // nMaxNoise
            // 
            this.nMaxNoise.Enabled = false;
            this.nMaxNoise.Location = new System.Drawing.Point(11, 87);
            this.nMaxNoise.Maximum = new decimal(new int[] {
            30000,
            0,
            0,
            0});
            this.nMaxNoise.Name = "nMaxNoise";
            this.nMaxNoise.Size = new System.Drawing.Size(120, 20);
            this.nMaxNoise.TabIndex = 11;
            this.nMaxNoise.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nMaxNoise.ValueChanged += new System.EventHandler(this.nMaxNoise_ValueChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(8, 71);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(27, 13);
            this.label9.TabIndex = 10;
            this.label9.Text = "Max";
            // 
            // nMinNoise
            // 
            this.nMinNoise.Enabled = false;
            this.nMinNoise.Location = new System.Drawing.Point(11, 39);
            this.nMinNoise.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nMinNoise.Name = "nMinNoise";
            this.nMinNoise.Size = new System.Drawing.Size(120, 20);
            this.nMinNoise.TabIndex = 9;
            this.nMinNoise.ValueChanged += new System.EventHandler(this.nMinNoise_ValueChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(8, 23);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(24, 13);
            this.label8.TabIndex = 8;
            this.label8.Text = "Min";
            // 
            // chNoise
            // 
            this.chNoise.AutoSize = true;
            this.chNoise.Location = new System.Drawing.Point(12, 0);
            this.chNoise.Name = "chNoise";
            this.chNoise.Size = new System.Drawing.Size(15, 14);
            this.chNoise.TabIndex = 7;
            this.chNoise.UseVisualStyleBackColor = true;
            this.chNoise.CheckedChanged += new System.EventHandler(this.chNoise_CheckedChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.tbIP);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.tbName);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.tbPort);
            this.panel1.Controls.Add(this.btnConnect);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(454, 173);
            this.panel1.TabIndex = 15;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.chClients);
            this.groupBox1.Location = new System.Drawing.Point(123, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(328, 167);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Список клиентов";
            // 
            // chClients
            // 
            this.chClients.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chClients.FormattingEnabled = true;
            this.chClients.Location = new System.Drawing.Point(3, 16);
            this.chClients.Name = "chClients";
            this.chClients.Size = new System.Drawing.Size(322, 148);
            this.chClients.TabIndex = 10;
            // 
            // tbIP
            // 
            this.tbIP.Location = new System.Drawing.Point(12, 63);
            this.tbIP.Name = "tbIP";
            this.tbIP.Size = new System.Drawing.Size(105, 20);
            this.tbIP.TabIndex = 6;
            this.tbIP.Text = "127.0.0.1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(20, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "IP:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Порт:";
            // 
            // tbPort
            // 
            this.tbPort.Location = new System.Drawing.Point(12, 19);
            this.tbPort.Name = "tbPort";
            this.tbPort.Size = new System.Drawing.Size(105, 20);
            this.tbPort.TabIndex = 2;
            this.tbPort.Text = "9000";
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(12, 139);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(105, 23);
            this.btnConnect.TabIndex = 0;
            this.btnConnect.Text = "Подключить";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.DefaultExt = "txt";
            this.saveFileDialog1.FileName = "data";
            // 
            // dgFiles
            // 
            this.dgFiles.AllowUserToAddRows = false;
            this.dgFiles.AllowUserToDeleteRows = false;
            this.dgFiles.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgFiles.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column4,
            this.Column5,
            this.Column6});
            this.dgFiles.ContextMenuStrip = this.contextMenuStrip1;
            this.dgFiles.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dgFiles.Location = new System.Drawing.Point(3, 63);
            this.dgFiles.Name = "dgFiles";
            this.dgFiles.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgFiles.Size = new System.Drawing.Size(440, 234);
            this.dgFiles.TabIndex = 4;
            // 
            // Column4
            // 
            this.Column4.HeaderText = "№";
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            this.Column4.Width = 40;
            // 
            // Column5
            // 
            this.Column5.HeaderText = "Файл";
            this.Column5.Name = "Column5";
            this.Column5.Width = 230;
            // 
            // Column6
            // 
            this.Column6.HeaderText = "Примечание";
            this.Column6.Name = "Column6";
            this.Column6.ReadOnly = true;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.удалитьToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(119, 26);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(122, 18);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(111, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "Добавить файлы";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(239, 18);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(102, 23);
            this.button2.TabIndex = 6;
            this.button2.Text = "Очистить";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // удалитьToolStripMenuItem
            // 
            this.удалитьToolStripMenuItem.Name = "удалитьToolStripMenuItem";
            this.удалитьToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.удалитьToolStripMenuItem.Text = "Удалить";
            this.удалитьToolStripMenuItem.Click += new System.EventHandler(this.удалитьToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(454, 499);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Тест (отправляет дельта-функцию длиной 1000)";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nSinCount)).EndInit();
            this.groupBox4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgSin)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nCount)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nMaxNoise)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nMinNoise)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgFiles)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbName;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.NumericUpDown nMinNoise;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.CheckBox chNoise;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.NumericUpDown nMaxNoise;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.NumericUpDown nCount;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button btnSaveSin;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckedListBox chClients;
        private System.Windows.Forms.TextBox tbIP;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSendFile;
        private System.Windows.Forms.TextBox tbPort;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button btnSendSin;
        private System.Windows.Forms.NumericUpDown nSinCount;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.DataGridView dgSin;
        private System.Windows.Forms.ComboBox cbN;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewComboBoxColumn Column2;
        private System.Windows.Forms.DataGridView dgFiles;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ToolStripMenuItem удалитьToolStripMenuItem;
    }
}

