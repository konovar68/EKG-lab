using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Globalization;
using System.Xml.Linq;

namespace NetManager
{
    public partial class FormClient : Form
    {
        private const string InitFileName = "Settings.xml"; 
    
        public FormClient()
        {
            InitializeComponent();

            LoadInit();

            Client = new NMClient(this);
            Client.OnStop += new EventHandler(Client_OnClose);
            Client.OnError += new EventHandler<EventMsgArgs>(Client_OnError);
            Client.OnNewClient += new EventHandler<EventClientArgs>(Client_OnNewClient);
            Client.OnDeleteClient += new EventHandler<EventClientArgs>(Client_OnDeleteClient);
            Client.OnReseive += new EventHandler<EventClientMsgArgs>(Client_OnReseive);
        }

        void Client_OnReseive(object sender, EventClientMsgArgs e)
        {
            string Msg = "";
            for (int I = 0; I < e.Msg.Length; I += sizeof(char))
                Msg += BitConverter.ToChar(e.Msg, I).ToString();
            tbReseive.Text += e.Name + " (" + e.ClientId.ToString() + "):" + Convert.ToChar(13).ToString() + Convert.ToChar(10).ToString() +
                Msg + Convert.ToChar(13).ToString() + Convert.ToChar(10).ToString() + Convert.ToChar(13).ToString() + Convert.ToChar(10).ToString();
        }

        void Client_OnClose(object sender, EventArgs e)
        {
            button1.Text = "Подключиться";
            tbIPServer.Enabled = true;
            tbName.Enabled = true;
            nPort.Enabled = true;
            chClients.Items.Clear();
            bRestart.Enabled = false;
        }

        void Client_OnDeleteClient(object sender, EventClientArgs e)
        {
            int I = chClients.Items.Count - 1;
            while ((I >= 0) && ((chClients.Items[I] as ClientAddress).Id != e.ClientId))
                I--;
            if (I >= 0)
                chClients.Items.RemoveAt(I);
        }

        private void Client_OnNewClient(object sender, EventClientArgs e)
        {
            chClients.Items.Add(new ClientAddress(e.ClientId, e.Name));
        }

        private void Client_OnError(object sender, EventMsgArgs e)
        {
            MessageBox.Show(e.Msg, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private int Port
        {
            get
            {
                return (int)nPort.Value;
            }
            set
            {
                nPort.Value = value;
            }
        }

        private IPAddress IPAddressServer
        {
            get
            {
                try
                {
                    return IPAddress.Parse(tbIPServer.Text);
                }
                catch
                {
                    tbIPServer.Focus();
                    throw new Exception("Неверно введен IP адрес!");
                }
            }
            set
            {
                tbIPServer.Text = value.ToString();
            }
        }

        private void tbIPServer_Leave(object sender, EventArgs e)
        {
            IPAddress IP;
            if (!IPAddress.TryParse(tbIPServer.Text, out IP))
            {
                MessageBox.Show("Неверно введен IP адрес!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                tbIPServer.Focus();
            }
            else
                tbIPServer.Text = IP.ToString();
        }

        private void LoadInit()
        {
            try
            {
                XDocument XmlDoc = XDocument.Load(InitFileName);
                Port = int.Parse(XmlDoc.Element("Settings").Element("NetManager").Element("Client").Element("Network").Attribute("Port").Value);
                IPAddressServer = IPAddress.Parse(XmlDoc.Element("Settings").Element("NetManager").Element("Client").Element("Network").Attribute("IP").Value);
                Name = XmlDoc.Element("Settings").Element("NetManager").Element("Client").Element("Network").Attribute("Name").Value;
            }
            catch
            {
            }
        }

        private void SaveInit()
        {
            XDocument XmlDoc;
            try
            {
                XmlDoc = XDocument.Load(InitFileName);
            }
            catch
            {
                XmlDoc = new XDocument();
            }

            XElement El = XmlDoc.Element("Settings");
            if (El == null)
            {
                El = new XElement("Settings");
                XmlDoc.Add(El);
            };

            XElement El2 = El.Element("NetManager");
            if (El2 == null)
            {
                El2 = new XElement("NetManager");
                El.Add(El2);
            };

            El = El2.Element("Client");
            if (El == null)
            {
                El = new XElement("Client");
                El2.Add(El);
            };

            El2 = El.Element("Network");
            if (El2 == null)
            {
                El2 = new XElement("Network");
                El.Add(El2);
            };
            
            El2.SetAttributeValue("Port", Port);
            El2.SetAttributeValue("IP", IPAddressServer);
            El2.SetAttributeValue("Name", tbName.Text);

            XmlDoc.Save(InitFileName);
        }

        private void FormClient_FormClosed(object sender, FormClosedEventArgs e)
        {
            SaveInit();
        }

        private NMClient Client;

        private void button1_Click(object sender, EventArgs e)
        {
            if (Client.Running)
            {
                Client.StopClient();//клиента остановили, а результат остановки отлавливается в OnStop
            }
            else
            {
                Client.IPServer = IPAddressServer;
                Client.Port = Port;
                Client.Name = tbName.Text;
                Client.RunClient();
                button1.Text = "Отключиться";
                tbIPServer.Enabled = false;
                nPort.Enabled = false;
                tbName.Enabled = false;
                bRestart.Enabled = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (chClients.CheckedItems.Count > 0)
            {
                int[] Addresses = new int[chClients.CheckedItems.Count];
                for (int I = 0; I < chClients.CheckedItems.Count; I++)
                    Addresses[I] = (chClients.CheckedItems[I] as ClientAddress).Id;
                byte[] Data = new byte[tbSend.Text.Length * sizeof(char)];
                byte[] tmp;
                for (int I = 0; I < tbSend.Text.Length; I++)
                {
                    tmp = BitConverter.GetBytes(tbSend.Text[I]);
                    for (int J = 0; J < tmp.Length; J++)
                        Data[I * sizeof(char) + J] = tmp[J];
                }
                Client.SendData(Addresses, Data);
            }
        }

        private void bRestart_Click(object sender, EventArgs e)
        {
            Client.SendRestartServer();
        }
    }
}
