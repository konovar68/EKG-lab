namespace EEG
{
    partial class FormMain
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.nPort = new System.Windows.Forms.NumericUpDown();
            this.btnConnect = new System.Windows.Forms.Button();
            this.tbName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbIP = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.dgChannels = new System.Windows.Forms.DataGridView();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.btnStart = new System.Windows.Forms.Button();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.dgAmp = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.dgChannelFilter = new System.Windows.Forms.DataGridView();
            this.colFChannel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColFVCh = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.ColFNCh = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.panel5 = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.cbFrequensy = new System.Windows.Forms.ComboBox();
            this.chSave = new System.Windows.Forms.CheckBox();
            this.chDestinations = new System.Windows.Forms.CheckedListBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.dgDrawChannels = new System.Windows.Forms.DataGridView();
            this.Column5 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.nScaleY = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.nScaleX = new System.Windows.Forms.NumericUpDown();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panel1 = new System.Windows.Forms.Panel();
            this.chDrawDisable = new System.Windows.Forms.CheckBox();
            this.chLoadFromFileDisconnect = new System.Windows.Forms.CheckBox();
            this.btnLoadFromFile = new System.Windows.Forms.Button();
            this.tbLoadFromFile = new System.Windows.Forms.TextBox();
            this.chLoadFromFile = new System.Windows.Forms.CheckBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.btnCoefCC = new System.Windows.Forms.Button();
            this.dgFilterCC = new System.Windows.Forms.DataGridView();
            this.Column7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.chCoefCC = new System.Windows.Forms.CheckBox();
            this.numberFilterCC = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.chFilterCC = new System.Windows.Forms.CheckBox();
            this.panelDraw = new System.Windows.Forms.PictureBox();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nPort)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgChannels)).BeginInit();
            this.groupBox9.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgAmp)).BeginInit();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgChannelFilter)).BeginInit();
            this.panel5.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgDrawChannels)).BeginInit();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nScaleY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nScaleX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgFilterCC)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numberFilterCC)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelDraw)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.nPort);
            this.groupBox1.Controls.Add(this.btnConnect);
            this.groupBox1.Controls.Add(this.tbName);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.tbIP);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(221, 135);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Сеть";
            // 
            // nPort
            // 
            this.nPort.Location = new System.Drawing.Point(112, 32);
            this.nPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.nPort.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nPort.Name = "nPort";
            this.nPort.Size = new System.Drawing.Size(100, 20);
            this.nPort.TabIndex = 8;
            this.nPort.Value = new decimal(new int[] {
            9000,
            0,
            0,
            0});
            // 
            // btnConnect
            // 
            this.btnConnect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnConnect.Location = new System.Drawing.Point(60, 102);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(100, 23);
            this.btnConnect.TabIndex = 7;
            this.btnConnect.Text = "Подключить";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // tbName
            // 
            this.tbName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbName.Location = new System.Drawing.Point(6, 73);
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(206, 20);
            this.tbName.TabIndex = 5;
            this.tbName.Text = "ЭЭГ";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 57);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Название";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(109, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Порт";
            // 
            // tbIP
            // 
            this.tbIP.Location = new System.Drawing.Point(6, 32);
            this.tbIP.Name = "tbIP";
            this.tbIP.Size = new System.Drawing.Size(100, 20);
            this.tbIP.TabIndex = 1;
            this.tbIP.Text = "127.0.0.1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(17, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "IP";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.groupBox3);
            this.groupBox2.Controls.Add(this.btnStart);
            this.groupBox2.Controls.Add(this.groupBox9);
            this.groupBox2.Controls.Add(this.groupBox4);
            this.groupBox2.Controls.Add(this.panel5);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(227, 561);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Считывание данных";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.dgChannels);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(3, 284);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(221, 183);
            this.groupBox3.TabIndex = 7;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Назначение каналов";
            // 
            // dgChannels
            // 
            this.dgChannels.AllowUserToAddRows = false;
            this.dgChannels.AllowUserToDeleteRows = false;
            this.dgChannels.AllowUserToResizeColumns = false;
            this.dgChannels.AllowUserToResizeRows = false;
            this.dgChannels.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgChannels.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column3,
            this.Column4});
            this.dgChannels.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgChannels.Location = new System.Drawing.Point(3, 16);
            this.dgChannels.Name = "dgChannels";
            this.dgChannels.RowHeadersVisible = false;
            this.dgChannels.Size = new System.Drawing.Size(215, 164);
            this.dgChannels.TabIndex = 0;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "Назв-е";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            this.Column3.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Column3.Width = 50;
            // 
            // Column4
            // 
            this.Column4.HeaderText = "Назначение";
            this.Column4.Items.AddRange(new object[] {
            "не используется",
            "в нормальном режиме",
            "в качестве опорного",
            "в режиме калибровка",
            "в режиме контроля шума"});
            this.Column4.Name = "Column4";
            this.Column4.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Column4.Width = 140;
            // 
            // btnStart
            // 
            this.btnStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnStart.Location = new System.Drawing.Point(135, 526);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(74, 23);
            this.btnStart.TabIndex = 1;
            this.btnStart.Text = "Старт";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // groupBox9
            // 
            this.groupBox9.Controls.Add(this.dgAmp);
            this.groupBox9.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox9.Location = new System.Drawing.Point(3, 150);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Size = new System.Drawing.Size(221, 134);
            this.groupBox9.TabIndex = 6;
            this.groupBox9.TabStop = false;
            this.groupBox9.Text = "Усиление каналов";
            // 
            // dgAmp
            // 
            this.dgAmp.AllowUserToAddRows = false;
            this.dgAmp.AllowUserToDeleteRows = false;
            this.dgAmp.AllowUserToResizeColumns = false;
            this.dgAmp.AllowUserToResizeRows = false;
            this.dgAmp.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgAmp.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2});
            this.dgAmp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgAmp.Location = new System.Drawing.Point(3, 16);
            this.dgAmp.Name = "dgAmp";
            this.dgAmp.RowHeadersVisible = false;
            this.dgAmp.Size = new System.Drawing.Size(215, 115);
            this.dgAmp.TabIndex = 0;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "Канал";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Column1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column1.Width = 59;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "Размах";
            this.Column2.Items.AddRange(new object[] {
            "200 мкВ",
            "500 мкВ",
            "1 мВ",
            "2 мВ",
            "5 мВ",
            "10 мВ",
            "20 мВ",
            "50 мВ",
            "100 мВ"});
            this.Column2.Name = "Column2";
            this.Column2.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.dgChannelFilter);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox4.Location = new System.Drawing.Point(3, 16);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(221, 134);
            this.groupBox4.TabIndex = 4;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Фильтры";
            // 
            // dgChannelFilter
            // 
            this.dgChannelFilter.AllowUserToAddRows = false;
            this.dgChannelFilter.AllowUserToDeleteRows = false;
            this.dgChannelFilter.AllowUserToResizeColumns = false;
            this.dgChannelFilter.AllowUserToResizeRows = false;
            this.dgChannelFilter.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgChannelFilter.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colFChannel,
            this.ColFVCh,
            this.ColFNCh});
            this.dgChannelFilter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgChannelFilter.Location = new System.Drawing.Point(3, 16);
            this.dgChannelFilter.Name = "dgChannelFilter";
            this.dgChannelFilter.RowHeadersVisible = false;
            this.dgChannelFilter.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgChannelFilter.Size = new System.Drawing.Size(215, 115);
            this.dgChannelFilter.TabIndex = 8;
            // 
            // colFChannel
            // 
            this.colFChannel.Frozen = true;
            this.colFChannel.HeaderText = "Канал";
            this.colFChannel.Name = "colFChannel";
            this.colFChannel.ReadOnly = true;
            this.colFChannel.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.colFChannel.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colFChannel.Width = 50;
            // 
            // ColFVCh
            // 
            this.ColFVCh.HeaderText = "ВЧ";
            this.ColFVCh.Items.AddRange(new object[] {
            "0.212 Гц",
            "0.5 Гц",
            "160 Гц"});
            this.ColFVCh.Name = "ColFVCh";
            this.ColFVCh.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ColFVCh.Width = 80;
            // 
            // ColFNCh
            // 
            this.ColFNCh.HeaderText = "НЧ";
            this.ColFNCh.Items.AddRange(new object[] {
            "10000 Гц",
            "250 Гц"});
            this.ColFNCh.Name = "ColFNCh";
            this.ColFNCh.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ColFNCh.Width = 80;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.label5);
            this.panel5.Controls.Add(this.cbFrequensy);
            this.panel5.Controls.Add(this.chSave);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel5.Location = new System.Drawing.Point(3, 467);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(221, 91);
            this.panel5.TabIndex = 5;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 6);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(129, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "Частота дискретизации";
            // 
            // cbFrequensy
            // 
            this.cbFrequensy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbFrequensy.FormattingEnabled = true;
            this.cbFrequensy.Items.AddRange(new object[] {
            "1000 Гц",
            "5000 Гц"});
            this.cbFrequensy.Location = new System.Drawing.Point(12, 26);
            this.cbFrequensy.Name = "cbFrequensy";
            this.cbFrequensy.Size = new System.Drawing.Size(194, 21);
            this.cbFrequensy.TabIndex = 3;
            // 
            // chSave
            // 
            this.chSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chSave.AutoSize = true;
            this.chSave.Location = new System.Drawing.Point(9, 63);
            this.chSave.Name = "chSave";
            this.chSave.Size = new System.Drawing.Size(117, 17);
            this.chSave.TabIndex = 0;
            this.chSave.Text = "Сохранить в файл";
            this.chSave.UseVisualStyleBackColor = true;
            // 
            // chDestinations
            // 
            this.chDestinations.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chDestinations.FormattingEnabled = true;
            this.chDestinations.Location = new System.Drawing.Point(164, 135);
            this.chDestinations.MinimumSize = new System.Drawing.Size(4, 60);
            this.chDestinations.Name = "chDestinations";
            this.chDestinations.Size = new System.Drawing.Size(493, 88);
            this.chDestinations.TabIndex = 3;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.dgDrawChannels);
            this.panel4.Controls.Add(this.groupBox5);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(164, 334);
            this.panel4.TabIndex = 3;
            // 
            // dgDrawChannels
            // 
            this.dgDrawChannels.AllowUserToAddRows = false;
            this.dgDrawChannels.AllowUserToDeleteRows = false;
            this.dgDrawChannels.AllowUserToResizeColumns = false;
            this.dgDrawChannels.AllowUserToResizeRows = false;
            this.dgDrawChannels.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgDrawChannels.ColumnHeadersVisible = false;
            this.dgDrawChannels.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column5,
            this.Column6});
            this.dgDrawChannels.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgDrawChannels.Location = new System.Drawing.Point(0, 64);
            this.dgDrawChannels.Name = "dgDrawChannels";
            this.dgDrawChannels.RowHeadersVisible = false;
            this.dgDrawChannels.Size = new System.Drawing.Size(164, 270);
            this.dgDrawChannels.TabIndex = 3;
            this.dgDrawChannels.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgDrawChannels_CellDoubleClick);
            this.dgDrawChannels.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgDrawChannels_CellValueChanged);
            this.dgDrawChannels.CurrentCellDirtyStateChanged += new System.EventHandler(this.dgDrawChannels_CurrentCellDirtyStateChanged);
            // 
            // Column5
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.NullValue = false;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.White;
            this.Column5.DefaultCellStyle = dataGridViewCellStyle4;
            this.Column5.FalseValue = "false";
            this.Column5.HeaderText = "";
            this.Column5.Name = "Column5";
            this.Column5.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Column5.TrueValue = "true";
            this.Column5.Width = 20;
            // 
            // Column6
            // 
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.White;
            this.Column6.DefaultCellStyle = dataGridViewCellStyle5;
            this.Column6.HeaderText = "";
            this.Column6.Name = "Column6";
            this.Column6.ReadOnly = true;
            this.Column6.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Column6.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column6.Width = 120;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.label6);
            this.groupBox5.Controls.Add(this.nScaleY);
            this.groupBox5.Controls.Add(this.label4);
            this.groupBox5.Controls.Add(this.nScaleX);
            this.groupBox5.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox5.Location = new System.Drawing.Point(0, 0);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(164, 64);
            this.groupBox5.TabIndex = 2;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Масштаб";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(83, 16);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(17, 13);
            this.label6.TabIndex = 2;
            this.label6.Text = "Y:";
            // 
            // nScaleY
            // 
            this.nScaleY.DecimalPlaces = 5;
            this.nScaleY.Increment = new decimal(new int[] {
            1,
            0,
            0,
            262144});
            this.nScaleY.Location = new System.Drawing.Point(86, 32);
            this.nScaleY.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.nScaleY.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            327680});
            this.nScaleY.Name = "nScaleY";
            this.nScaleY.Size = new System.Drawing.Size(70, 20);
            this.nScaleY.TabIndex = 3;
            this.nScaleY.Value = new decimal(new int[] {
            5,
            0,
            0,
            262144});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(5, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(17, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "X:";
            // 
            // nScaleX
            // 
            this.nScaleX.DecimalPlaces = 2;
            this.nScaleX.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.nScaleX.Location = new System.Drawing.Point(8, 32);
            this.nScaleX.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.nScaleX.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.nScaleX.Name = "nScaleX";
            this.nScaleX.Size = new System.Drawing.Size(70, 20);
            this.nScaleX.TabIndex = 1;
            this.nScaleX.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.nScaleX.ValueChanged += new System.EventHandler(this.nScaleX_ValueChanged);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(227, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.AutoScroll = true;
            this.splitContainer1.Panel1.AutoScrollMinSize = new System.Drawing.Size(222, 0);
            this.splitContainer1.Panel1.Controls.Add(this.chDestinations);
            this.splitContainer1.Panel1.Controls.Add(this.panel1);
            this.splitContainer1.Panel1.Controls.Add(this.groupBox6);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.panelDraw);
            this.splitContainer1.Panel2.Controls.Add(this.panel4);
            this.splitContainer1.Size = new System.Drawing.Size(657, 561);
            this.splitContainer1.SplitterDistance = 223;
            this.splitContainer1.TabIndex = 4;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.chDrawDisable);
            this.panel1.Controls.Add(this.chLoadFromFileDisconnect);
            this.panel1.Controls.Add(this.btnLoadFromFile);
            this.panel1.Controls.Add(this.tbLoadFromFile);
            this.panel1.Controls.Add(this.chLoadFromFile);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(164, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(493, 135);
            this.panel1.TabIndex = 5;
            // 
            // chDrawDisable
            // 
            this.chDrawDisable.AutoSize = true;
            this.chDrawDisable.Location = new System.Drawing.Point(227, 80);
            this.chDrawDisable.Name = "chDrawDisable";
            this.chDrawDisable.Size = new System.Drawing.Size(136, 17);
            this.chDrawDisable.TabIndex = 5;
            this.chDrawDisable.Text = "Отключить отрисовку";
            this.chDrawDisable.UseVisualStyleBackColor = true;
            this.chDrawDisable.CheckedChanged += new System.EventHandler(this.chDrawDisable_CheckedChanged);
            // 
            // chLoadFromFileDisconnect
            // 
            this.chLoadFromFileDisconnect.AutoSize = true;
            this.chLoadFromFileDisconnect.Location = new System.Drawing.Point(227, 57);
            this.chLoadFromFileDisconnect.Name = "chLoadFromFileDisconnect";
            this.chLoadFromFileDisconnect.Size = new System.Drawing.Size(161, 17);
            this.chLoadFromFileDisconnect.TabIndex = 4;
            this.chLoadFromFileDisconnect.Text = "Отключить по завершении";
            this.chLoadFromFileDisconnect.UseVisualStyleBackColor = true;
            // 
            // btnLoadFromFile
            // 
            this.btnLoadFromFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLoadFromFile.Location = new System.Drawing.Point(458, 26);
            this.btnLoadFromFile.Name = "btnLoadFromFile";
            this.btnLoadFromFile.Size = new System.Drawing.Size(23, 23);
            this.btnLoadFromFile.TabIndex = 3;
            this.btnLoadFromFile.Text = "...";
            this.btnLoadFromFile.UseVisualStyleBackColor = true;
            this.btnLoadFromFile.Click += new System.EventHandler(this.btnLoadFromFile_Click);
            // 
            // tbLoadFromFile
            // 
            this.tbLoadFromFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbLoadFromFile.Location = new System.Drawing.Point(227, 28);
            this.tbLoadFromFile.Name = "tbLoadFromFile";
            this.tbLoadFromFile.Size = new System.Drawing.Size(230, 20);
            this.tbLoadFromFile.TabIndex = 2;
            // 
            // chLoadFromFile
            // 
            this.chLoadFromFile.AutoSize = true;
            this.chLoadFromFile.Location = new System.Drawing.Point(227, 8);
            this.chLoadFromFile.Name = "chLoadFromFile";
            this.chLoadFromFile.Size = new System.Drawing.Size(174, 17);
            this.chLoadFromFile.TabIndex = 1;
            this.chLoadFromFile.Text = "Отправлять данные с файла:";
            this.chLoadFromFile.UseVisualStyleBackColor = true;
            this.chLoadFromFile.CheckedChanged += new System.EventHandler(this.chLoadFromFile_CheckedChanged);
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.btnCoefCC);
            this.groupBox6.Controls.Add(this.dgFilterCC);
            this.groupBox6.Controls.Add(this.chCoefCC);
            this.groupBox6.Controls.Add(this.numberFilterCC);
            this.groupBox6.Controls.Add(this.label7);
            this.groupBox6.Controls.Add(this.chFilterCC);
            this.groupBox6.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox6.Location = new System.Drawing.Point(0, 0);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(164, 223);
            this.groupBox6.TabIndex = 4;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "       Фильтр свертка";
            // 
            // btnCoefCC
            // 
            this.btnCoefCC.Enabled = false;
            this.btnCoefCC.Location = new System.Drawing.Point(107, 56);
            this.btnCoefCC.Name = "btnCoefCC";
            this.btnCoefCC.Size = new System.Drawing.Size(33, 23);
            this.btnCoefCC.TabIndex = 5;
            this.btnCoefCC.Text = "...";
            this.btnCoefCC.UseVisualStyleBackColor = true;
            this.btnCoefCC.Click += new System.EventHandler(this.btnCoefCC_Click);
            // 
            // dgFilterCC
            // 
            this.dgFilterCC.AllowUserToAddRows = false;
            this.dgFilterCC.AllowUserToDeleteRows = false;
            this.dgFilterCC.AllowUserToResizeColumns = false;
            this.dgFilterCC.AllowUserToResizeRows = false;
            this.dgFilterCC.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgFilterCC.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgFilterCC.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column7,
            this.Column8});
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle6.NullValue = "0";
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgFilterCC.DefaultCellStyle = dataGridViewCellStyle6;
            this.dgFilterCC.Enabled = false;
            this.dgFilterCC.Location = new System.Drawing.Point(3, 85);
            this.dgFilterCC.Name = "dgFilterCC";
            this.dgFilterCC.RowHeadersVisible = false;
            this.dgFilterCC.Size = new System.Drawing.Size(158, 135);
            this.dgFilterCC.TabIndex = 4;
            this.dgFilterCC.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.dgFilterCC_RowsAdded);
            // 
            // Column7
            // 
            this.Column7.HeaderText = "№";
            this.Column7.Name = "Column7";
            this.Column7.ReadOnly = true;
            this.Column7.Width = 40;
            // 
            // Column8
            // 
            this.Column8.HeaderText = "Коэффициент";
            this.Column8.Name = "Column8";
            this.Column8.Width = 90;
            // 
            // chCoefCC
            // 
            this.chCoefCC.AutoSize = true;
            this.chCoefCC.Enabled = false;
            this.chCoefCC.Location = new System.Drawing.Point(6, 60);
            this.chCoefCC.Name = "chCoefCC";
            this.chCoefCC.Size = new System.Drawing.Size(104, 17);
            this.chCoefCC.TabIndex = 3;
            this.chCoefCC.Text = "Коэффициенты";
            this.chCoefCC.UseVisualStyleBackColor = true;
            this.chCoefCC.CheckedChanged += new System.EventHandler(this.chCoefCC_CheckedChanged);
            // 
            // numberFilterCC
            // 
            this.numberFilterCC.Enabled = false;
            this.numberFilterCC.Location = new System.Drawing.Point(6, 34);
            this.numberFilterCC.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.numberFilterCC.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numberFilterCC.Name = "numberFilterCC";
            this.numberFilterCC.Size = new System.Drawing.Size(86, 20);
            this.numberFilterCC.TabIndex = 2;
            this.numberFilterCC.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.numberFilterCC.ValueChanged += new System.EventHandler(this.numberFilterCC_ValueChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 18);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(70, 13);
            this.label7.TabIndex = 1;
            this.label7.Text = "Длина окна:";
            // 
            // chFilterCC
            // 
            this.chFilterCC.AutoSize = true;
            this.chFilterCC.Location = new System.Drawing.Point(12, 1);
            this.chFilterCC.Name = "chFilterCC";
            this.chFilterCC.Size = new System.Drawing.Size(15, 14);
            this.chFilterCC.TabIndex = 0;
            this.chFilterCC.UseVisualStyleBackColor = true;
            this.chFilterCC.CheckedChanged += new System.EventHandler(this.chFilterCC_CheckedChanged);
            // 
            // panelDraw
            // 
            this.panelDraw.BackColor = System.Drawing.Color.White;
            this.panelDraw.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelDraw.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelDraw.Location = new System.Drawing.Point(164, 0);
            this.panelDraw.MinimumSize = new System.Drawing.Size(150, 350);
            this.panelDraw.Name = "panelDraw";
            this.panelDraw.Size = new System.Drawing.Size(493, 350);
            this.panelDraw.TabIndex = 4;
            this.panelDraw.TabStop = false;
            this.panelDraw.SizeChanged += new System.EventHandler(this.panelDraw_SizeChanged);
            this.panelDraw.Paint += new System.Windows.Forms.PaintEventHandler(this.panelDraw_Paint);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 561);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.groupBox2);
            this.Location = new System.Drawing.Point(10, 10);
            this.MinimumSize = new System.Drawing.Size(720, 400);
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Считыватель ЭЭГ (02.04.2015)";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nPort)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgChannels)).EndInit();
            this.groupBox9.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgAmp)).EndInit();
            this.groupBox4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgChannelFilter)).EndInit();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgDrawChannels)).EndInit();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nScaleY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nScaleX)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgFilterCC)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numberFilterCC)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelDraw)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.TextBox tbName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbIP;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.CheckBox chSave;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.CheckedListBox chDestinations;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.NumericUpDown nPort;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.NumericUpDown nScaleX;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cbFrequensy;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.GroupBox groupBox9;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.DataGridView dgChannelFilter;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFChannel;
        private System.Windows.Forms.DataGridViewComboBoxColumn ColFVCh;
        private System.Windows.Forms.DataGridViewComboBoxColumn ColFNCh;
        private System.Windows.Forms.DataGridView dgAmp;
        private System.Windows.Forms.DataGridView dgChannels;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewComboBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewComboBoxColumn Column4;
        private System.Windows.Forms.DataGridView dgDrawChannels;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown nScaleY;
        private System.Windows.Forms.PictureBox panelDraw;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.NumericUpDown numberFilterCC;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox chFilterCC;
        private System.Windows.Forms.CheckBox chCoefCC;
        private System.Windows.Forms.DataGridView dgFilterCC;
        private System.Windows.Forms.Button btnCoefCC;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column7;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column8;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnLoadFromFile;
        private System.Windows.Forms.TextBox tbLoadFromFile;
        private System.Windows.Forms.CheckBox chLoadFromFile;
        private System.Windows.Forms.CheckBox chLoadFromFileDisconnect;
        private System.Windows.Forms.CheckBox chDrawDisable;
    }
}

