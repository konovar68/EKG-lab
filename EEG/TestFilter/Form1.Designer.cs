namespace TestFilter
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.btnCoefCC = new System.Windows.Forms.Button();
            this.dgFilterCC = new System.Windows.Forms.DataGridView();
            this.Column7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.chCoefCC = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.numberFilterCC = new System.Windows.Forms.NumericUpDown();
            this.tbInputFileName = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.button2 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.tbOutputFileName = new System.Windows.Forms.TextBox();
            this.groupBox6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgFilterCC)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numberFilterCC)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.btnCoefCC);
            this.groupBox6.Controls.Add(this.dgFilterCC);
            this.groupBox6.Controls.Add(this.chCoefCC);
            this.groupBox6.Controls.Add(this.label7);
            this.groupBox6.Controls.Add(this.panel1);
            this.groupBox6.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox6.Location = new System.Drawing.Point(0, 0);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(164, 342);
            this.groupBox6.TabIndex = 5;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "       Фильтр свертка";
            // 
            // btnCoefCC
            // 
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
            this.dgFilterCC.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgFilterCC.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column7,
            this.Column8});
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.NullValue = "0";
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgFilterCC.DefaultCellStyle = dataGridViewCellStyle1;
            this.dgFilterCC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgFilterCC.Enabled = false;
            this.dgFilterCC.Location = new System.Drawing.Point(3, 87);
            this.dgFilterCC.Name = "dgFilterCC";
            this.dgFilterCC.RowHeadersVisible = false;
            this.dgFilterCC.Size = new System.Drawing.Size(158, 252);
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
            this.chCoefCC.Location = new System.Drawing.Point(6, 60);
            this.chCoefCC.Name = "chCoefCC";
            this.chCoefCC.Size = new System.Drawing.Size(104, 17);
            this.chCoefCC.TabIndex = 3;
            this.chCoefCC.Text = "Коэффициенты";
            this.chCoefCC.UseVisualStyleBackColor = true;
            this.chCoefCC.CheckedChanged += new System.EventHandler(this.chCoefCC_CheckedChanged);
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
            // panel1
            // 
            this.panel1.Controls.Add(this.numberFilterCC);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(3, 16);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(158, 71);
            this.panel1.TabIndex = 6;
            // 
            // numberFilterCC
            // 
            this.numberFilterCC.Location = new System.Drawing.Point(6, 18);
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
            // tbInputFileName
            // 
            this.tbInputFileName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbInputFileName.Location = new System.Drawing.Point(170, 25);
            this.tbInputFileName.Name = "tbInputFileName";
            this.tbInputFileName.Size = new System.Drawing.Size(375, 20);
            this.tbInputFileName.TabIndex = 6;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(551, 23);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 7;
            this.button1.Text = "...";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(170, 108);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 8;
            this.button2.Text = "OK";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(170, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Исходный файл:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(170, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "Выходной файл:";
            // 
            // button3
            // 
            this.button3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button3.Location = new System.Drawing.Point(551, 65);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 11;
            this.button3.Text = "...";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // tbOutputFileName
            // 
            this.tbOutputFileName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbOutputFileName.Location = new System.Drawing.Point(170, 67);
            this.tbOutputFileName.Name = "tbOutputFileName";
            this.tbOutputFileName.Size = new System.Drawing.Size(375, 20);
            this.tbOutputFileName.TabIndex = 10;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(638, 342);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.tbOutputFileName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.tbInputFileName);
            this.Controls.Add(this.groupBox6);
            this.Name = "Form1";
            this.Text = "Тест Фильтров";
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgFilterCC)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numberFilterCC)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Button btnCoefCC;
        private System.Windows.Forms.DataGridView dgFilterCC;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column7;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column8;
        private System.Windows.Forms.CheckBox chCoefCC;
        private System.Windows.Forms.NumericUpDown numberFilterCC;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox tbInputFileName;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.TextBox tbOutputFileName;
    }
}

