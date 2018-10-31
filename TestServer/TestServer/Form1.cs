using System;
using System.Windows.Forms;
using System.Net;
using System.IO;
using NetManager;
using EEG;
using System.Threading;
using System.Collections.Generic;

namespace TestServer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            NMClient = new NMClient(this);
            NMClient.OnDeleteClient += new EventHandler<EventClientArgs>(NMClient_OnDeleteClient);
            NMClient.OnError += new EventHandler<EventMsgArgs>(NMClient_OnError);
            NMClient.OnNewClient += new EventHandler<EventClientArgs>(NMClient_OnNewClient);
            NMClient.OnReseive += new EventHandler<EventClientMsgArgs>(NMClient_OnReseive);
            NMClient.OnStop += new EventHandler(NMClient_OnStop);

            for (int I = 2; I < 500; I++)
                (dgSin.Columns[2] as DataGridViewComboBoxColumn).Items.Add((1000.0 / I).ToString());
            cbN.SelectedIndex = 0;

            m_A.Add(10000);
            m_F.Add(Convert.ToDouble((dgSin.Columns[2] as DataGridViewComboBoxColumn).Items[0]));
            dgSin.Rows.Add(dgSin.Rows.Count + 1, 10000, (dgSin.Columns[2] as DataGridViewComboBoxColumn).Items[0]);
        }

        void NMClient_OnStop(object sender, EventArgs e)
        {
            btnConnect.Text = "Подключить";
            btnSendFile.Enabled = false;
            btnSendSin.Enabled = false;
            chClients.Enabled = true;
            chClients.Items.Clear();
        }

        void NMClient_OnReseive(object sender, EventClientMsgArgs e)
        {
            //не предусмотрено
        }

        void NMClient_OnNewClient(object sender, EventClientArgs e)
        {
            chClients.Items.Add(new ClientAddress(e.ClientId, e.Name));
        }

        void NMClient_OnError(object sender, EventMsgArgs e)
        {
            MessageBox.Show(e.Msg, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        void NMClient_OnDeleteClient(object sender, EventClientArgs e)
        {
            ClientAddress Cl = new ClientAddress(e.ClientId, e.Name);
            int I = chClients.Items.Count - 1;
            while ((I >= 0) && (Cl.ToString() != chClients.Items[I].ToString()))
                I--;
            if (I >= 0)
                chClients.Items.RemoveAt(I);
        }

        private int Port
        {
            get
            {
                return Convert.ToInt32(tbPort.Text);
            }
            set
            {
                tbPort.Text = value.ToString();
            }
        }

        private delegate void delegateSetTime(DateTime time, int index);

        private void SetTime(DateTime time, int index)
        {
            if (dgFiles.InvokeRequired)
            {
                dgFiles.Invoke(new delegateSetTime(SetTime), time, index);
            }
            else
                dgFiles.Rows[index].Cells[2].Value = time;
        }
            

        private NMClient NMClient;

        private delegate void SendData();

        private void Send_Data()
        {
            if ((chClients.CheckedItems.Count > 0) && NMClient.Running)
            {
                Frame data = new Frame();
                int[] addresses = new int[chClients.CheckedItems.Count];

                for (int j = 0; j < chClients.CheckedItems.Count; j++)
                    addresses[j] = (chClients.CheckedItems[j] as ClientAddress).Id;

          //      Random Rnd = new Random();

                /* while (s.Connected && (N < 10078))
                 {
                     for (I = 260; I < 1460; I++)
                     {
                         if ((N == 3000) && ((I / 2) == 130) && ((I % 2) == 1))
                         {
                             data[I] = (byte)(I % 2);
                         }
                         else
                             data[I] = 0;
                     }
                     N += 20;
                     if (s.Connected)
                     ns.Write(data, 0, 1460);
                 };
                 MessageBox.Show("Все!");*/
                /* if (s.Connected)
                 {
                     SendData SD = new SendData(Send_Data);
                     SD.BeginInvoke(null, null);
                 }
                /* for (int K = 0; K < 1001; K++)
                 {
                     if (K != 500)
                     {
                         tmp = BitConverter.GetBytes((short)0);
                     }
                     else
                         tmp = BitConverter.GetBytes((short)1);
                     for (int J = 0; J < 30; J++)
                     {
                         data[I + J * 40] = tmp[0];
                         data[I + 1 + J * 40] = tmp[1];
                     };
                     I += 2;
                     if (I == 300)
                     {
                         I = 260;
                         ns.Write(data, 0, 1460);
                     }
                 }*/
                string str;
                for (int l = 0; l < dgFiles.Rows.Count; l++)
                {
                    try
                    {
                        StreamReader sr = new StreamReader(dgFiles.Rows[l].Cells[1].Value.ToString());
                        SetTime(DateTime.Now, l);
                        string[] strs;
                        int i = 0;
                        while (!sr.EndOfStream)
                        {
                            if (i == Frame.LengthData)
                            {
                                i = 0;
                                NMClient.SendData(addresses, data.GetBytes());
                                Thread.Sleep(4);
                            }
                            str = sr.ReadLine();
                            strs = str.Split(new char[] { ' ', (char)9, ';' });
                            byte f = 1;
                            byte code = 0;
                            for (int J = 0; J < strs[0].Length; J++)
                            {
                                if (strs[0][J] != '0')
                                    code += f;
                                f <<= 1;
                            }
                            data.reserved[i] = code;
                            for (int J = 0; J < Math.Min(strs.Length, Frame.CountChannels); J++)
                                data.Data[J * Frame.LengthData + i] = short.Parse(strs[J]);
                            i++;
                        }
                        sr.Close();
                    }
                    catch (Exception e)
                    {
                        dgFiles.Rows[l].Cells[2].Value = "Ошибка '" + e.Message + "'";
                    }
                }
            }
             MessageBox.Show("Все!");

            SetEnabled(btnSendFile, true);
            SetEnabled(btnSendSin, true);
            SetEnabled(chClients, true);
        }

        private delegate void delegate_SetEnabled(Control Control, bool Enabled);

        private void SetEnabled(Control Control, bool Enabled)
        {
            if (Control.InvokeRequired)
            {
                delegate_SetEnabled E = new delegate_SetEnabled(SetEnabled);
                Control.Invoke(E, new object[] {Control, Enabled});
            }
            else
                Control.Enabled = Enabled;
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (!NMClient.Running)
            {
                NMClient.IPServer = IPAddress.Parse(tbIP.Text);
                NMClient.Port = Int32.Parse(tbPort.Text);
                NMClient.Name = tbName.Text;
                NMClient.RunClient();
                btnConnect.Text = "Отключить";
                btnSendFile.Enabled = true;
                btnSendSin.Enabled = true;
            }
            else
                NMClient.StopClient();
        }

        private void btnSendFile_Click(object sender, EventArgs e)
        {
            btnSendFile.Enabled = false;
            btnSendSin.Enabled = false;
            chClients.Enabled = false;
            SendData SD = new SendData(Send_Data);
            SD.BeginInvoke(null, null);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (NMClient.Running)
                NMClient.StopClient();
        }

        private void nMinNoise_ValueChanged(object sender, EventArgs e)
        {
            nMaxNoise.Minimum = nMinNoise.Value;
            lock(m_lockObj)
            {
                m_MinNoise = (int)nMinNoise.Value;
            }
        }

        private void nMaxNoise_ValueChanged(object sender, EventArgs e)
        {
            nMinNoise.Maximum = nMaxNoise.Value;
            lock(m_lockObj)
            {
                m_MaxNoise = (int)nMaxNoise.Value;
            }
        }

        private void chNoise_CheckedChanged(object sender, EventArgs e)
        {
            nMaxNoise.Enabled = chNoise.Checked;
            nMinNoise.Enabled = chNoise.Checked;
            lock(m_lockObj)
            {
                m_checkNoise = chNoise.Checked;
            }
        }

        private void btnSaveSin_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                StreamWriter sw = new StreamWriter(saveFileDialog1.FileName);
                int N = Convert.ToInt32(cbN.SelectedItem);
                int minNoise = (int)nMinNoise.Value;
                int maxNoise = (int)nMaxNoise.Value;
                int count = (int)nCount.Value;
                Random r = new Random((int)DateTime.Now.Ticks);
                for (int i = 0; i < count; i++)
                {
                    double x = (short)(chNoise.Checked ? r.Next(minNoise, maxNoise) : 0);
                    for (int j = 0; j < dgSin.RowCount; j++)
                    {
                        int a = Convert.ToInt32(dgSin.Rows[j].Cells[1].Value);
                        double f = Convert.ToDouble(dgSin.Rows[j].Cells[2].Value);
                        x += a * Math.Sin(2 * Math.PI * i * f / N);
                    }
                    for (int j = 0; j < 22; j++)
                    {
                        sw.Write((short)x);
                        sw.Write((char)9);
                    }
                    sw.WriteLine();
                }
                sw.Close();
                MessageBox.Show("Сохранение данных в файл закончено");
            }            
        }

        private int m_Count = 1;
        private List<double> m_A = new List<double>();
        private List<double> m_F = new List<double>();
        private double m_N;
        private int m_MinNoise = 0;
        private int m_MaxNoise = 100;
        private bool m_checkNoise = false;
        private object m_lockObj = new object();
        private bool m_SendSin = false;

        private void Send_Sin()
        {
            if ((chClients.CheckedItems.Count > 0) && NMClient.Running)
            {
                Frame data = new Frame();
                int[] addresses = new int[chClients.CheckedItems.Count];

                for (int j = 0; j < chClients.CheckedItems.Count; j++)
                    addresses[j] = (chClients.CheckedItems[j] as ClientAddress).Id;

                Random Rnd = new Random((int)DateTime.Now.Ticks);

                int[] x = new int[m_Count];
                for (int j = 0; j < m_Count; j++)
                    x[j] = 0;
                int i = 0;
                DateTime t1 = DateTime.Now;
                double interval = Frame.LengthData / m_N * 1000;
                while (m_SendSin)
                {
                    double y;
                    lock (m_lockObj)
                    {
                        y = m_checkNoise ? Rnd.Next(m_MinNoise, m_MaxNoise) : 0;
                        for (int j = 0; j < m_Count; j++)
                        {
                            y += m_A[j] * Math.Sin(2 * Math.PI * x[j] * m_F[j] / m_N);
                            x[j] = (x[j] + 1) % (int)(m_N / m_F[j]);
                        }
                    }
                    for (int j = 0; j < Frame.CountChannels; j++)
                    {
                        data.reserved[i] = 0xFF;
                        data.Data[j * Frame.LengthData + i] = (short)y;
                    }
                    i = (i + 1) % Frame.LengthData;
                    if (i == 0)
                    {
                        NMClient.SendData(addresses, data.GetBytes());
                        double dt = (DateTime.Now - t1).TotalMilliseconds;
                        if (dt < interval)
                            Thread.Sleep((int)(interval - dt));
                        t1 = DateTime.Now;
                    }
                }
            }
            SetEnabled(btnSendFile, true);
            SetEnabled(chClients, true);
        }

        private void btnSendSin_Click(object sender, EventArgs e)
        {
            if (!m_SendSin)
            {
                m_SendSin = true;
                btnSendFile.Enabled = false;
                btnSendSin.Text = "Стоп";
                chClients.Enabled = false;
                SendData SD = new SendData(Send_Sin);
                SD.BeginInvoke(null, null);
            }
            else
            {
                m_SendSin = false;
                btnSendSin.Text = "Пуск";
            }
        }

        private void cbN_SelectedIndexChanged(object sender, EventArgs e)
        {
            lock(m_lockObj)
            {
                m_N = Convert.ToInt32(cbN.SelectedItem);
            }
        }

        private void nSinCount_ValueChanged(object sender, EventArgs e)
        {
            lock(m_lockObj)
            {                
                m_Count = (int)nSinCount.Value;
                while (m_Count < dgSin.RowCount)
                {
                    m_A.RemoveAt(m_A.Count - 1);
                    m_F.RemoveAt(m_F.Count - 1);
                    dgSin.Rows.RemoveAt(dgSin.Rows.Count - 1);
                }
                while (m_Count > dgSin.RowCount)
                {
                    m_A.Add(10000);
                    m_F.Add(Convert.ToDouble((dgSin.Columns[2] as DataGridViewComboBoxColumn).Items[0]));
                    dgSin.Rows.Add(dgSin.Rows.Count + 1, 10000, (dgSin.Columns[2] as DataGridViewComboBoxColumn).Items[0]);
                }
            }
        }

        private void dgSin_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dgSin.RowCount)
            {
                lock (m_lockObj)
                {
                    switch (e.ColumnIndex)
                    {
                        case 1:
                            m_A[e.RowIndex] = Convert.ToDouble(dgSin.Rows[e.RowIndex].Cells[1].Value);
                            break;
                        case 2:
                            m_F[e.RowIndex] = Convert.ToDouble(dgSin.Rows[e.RowIndex].Cells[2].Value);
                            break;
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dgFiles.Rows.Clear();
            btnSendFile.Enabled = dgFiles.RowCount > 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
                foreach (string file in openFileDialog1.FileNames)
                    dgFiles.Rows.Add(dgFiles.RowCount + 1, file, "");
            btnSendFile.Enabled = dgFiles.RowCount > 0;
        }

        private void удалитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgFiles.SelectedRows)
                dgFiles.Rows.Remove(row);
            btnSendFile.Enabled = dgFiles.RowCount > 0;
        }
    }
}
