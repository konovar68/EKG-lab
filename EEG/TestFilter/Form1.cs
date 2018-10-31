using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace TestFilter
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        
            dgFilterCC.RowCount = (int)numberFilterCC.Value;
        }

        private void numberFilterCC_ValueChanged(object sender, EventArgs e)
        {
            dgFilterCC.RowCount = (int)numberFilterCC.Value;
        }

        private void chCoefCC_CheckedChanged(object sender, EventArgs e)
        {
            btnCoefCC.Enabled = chCoefCC.Checked;
            dgFilterCC.Enabled = chCoefCC.Checked;
        }

        private void btnCoefCC_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
                try
                {
                    StreamReader SR = new StreamReader(openFileDialog1.FileName);
                    for (int I = 0; I < numberFilterCC.Value; I++)
                        dgFilterCC.Rows[I].Cells[1].Value = Convert.ToInt16(SR.ReadLine());
                    SR.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
                tbInputFileName.Text = openFileDialog1.FileName;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                StreamReader SR = new StreamReader(tbInputFileName.Text);
                StreamWriter SW = new StreamWriter(tbOutputFileName.Text);

                short[] CC = new short[(int)numberFilterCC.Value];
                int CountCC = 0;
                int IndexCC = 0;
                short[] CoefCC = null;
                if (chCoefCC.Checked)
                {
                    CoefCC = new short[(int)numberFilterCC.Value];
                    for (int I = 0; I < CoefCC.Length; I++)
                        CoefCC[I] = Convert.ToInt16(dgFilterCC.Rows[I].Cells[1].Value);
                }

                while (!SR.EndOfStream)
                {
                    short x = short.Parse(SR.ReadLine());

                    CC[IndexCC] = x;
                    IndexCC = (IndexCC + 1) % CC.Length;
                    CountCC = Math.Min(CountCC + 1, CC.Length);
                    if (CountCC == CC.Length)
                    {
                        int s = 0;
                        if (chCoefCC.Checked)
                        {
                            int Ind = IndexCC;
                            for (int I = 0; I < CountCC; I++)
                            {
                                s += CC[Ind] * CoefCC[I];
                                Ind = (Ind + 1) % CountCC;
                            }
                            x = (short)(s / CC.Length);
                        }
                        else
                        {
                            for (int I = 0; I < CountCC; I++)
                                s += CC[I];
                            x = (short)(s / CC.Length);
                        }
                    }

                    if (CountCC == CC.Length)
                    {
                        SW.WriteLine(x);
                    }
                }
                SR.Close();
                SW.Close();

                MessageBox.Show("Все!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
                tbOutputFileName.Text = openFileDialog1.FileName;
        }

        private void dgFilterCC_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            for (int I = e.RowIndex; I < (e.RowIndex + e.RowCount); I++)
                dgFilterCC.Rows[I].Cells[0].Value = I + 1;
        }
    }
}
