using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Net;

namespace NetManager
{
    public partial class FormServer : Form
    {
        private const string InitFileName = "Settings.xml";

        public FormServer()
        {
            InitializeComponent();

            //попытка считать настройки программы
            LoadInit();
            //определяется IP
            IPAddress[] IPAddressList = Dns.GetHostEntry(Dns.GetHostName()).AddressList;
            string[] Lines = new string[IPAddressList.Length];
            for (int I = 0; I < IPAddressList.Length; I++)
                Lines[I] = IPAddressList[I].AddressFamily.ToString() + ": " + IPAddressList[I].ToString();
            tbIP.Lines = Lines;
            //создается сервер
            Server = new NMServer(this);
            Server.OnError += new EventHandler<EventMsgArgs>(Server_OnError);
            Server.OnStop += new EventHandler(Server_OnStop);
            Server.OnRestart += new EventHandler(Server_OnRestart);
            Server.OnAddClient += new EventHandler<EventClientArgs>(Server_OnChangeClient);
            Server.OnDeleteClient += new EventHandler<EventClientArgs>(Server_OnChangeClient);
        }

        void Server_OnRestart(object sender, EventArgs e)
        {
            tbClients.Clear();
            lClients.Text = "Число клиентов (0)";
        }

        void Server_OnStop(object sender, EventArgs e)
        {
            nPort.Enabled = true;
            button1.Text = "Старт";
            startToolStripMenuItem.Text = "Старт";
            lClients.Text = "Число клиентов (0)";
            tbClients.Text = "";
        }

        void Server_OnChangeClient(object sender, EventClientArgs e)
        {
            lClients.Text = "Число клиентов (" + Server.Clients.Count.ToString() + ")";
            ClientSocket CS;
            string[] Strs = new string[Server.Clients.Count];
            for (int I = 0; I < Server.Clients.Count; I++)
            {
                CS = Server.Clients[I];
                Strs[I] = CS.Name + " (" + CS.Id.ToString() + ")";
            }
            tbClients.Lines = Strs;
        }

        void Server_OnError(object sender, EventMsgArgs e)
        {
            MessageBox.Show(e.Msg, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void LoadInit()
        {
            try
            {
                XDocument XmlDoc = XDocument.Load(InitFileName);
                Port = int.Parse(XmlDoc.Root.Element("NetManager").Element("Server").Element("Network").Attribute("Port").Value);
                nTestLive.Value = int.Parse(XmlDoc.Root.Element("NetManager").Element("Server").Attribute("liveTime").Value);
            }
            catch { }
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
                XmlDoc = new XDocument(new XElement("Settings"));
            }

            XElement NM = XmlDoc.Root.Element("NetManager");
            if (NM == null)
            {
                NM = new XElement("NetManager");
                XmlDoc.Root.Add(NM);
            }

            XElement El = NM.Element("Server");
            if (El == null)
            {
                El = new XElement("Server");
                NM.Add(El);
            }
            El.SetAttributeValue("liveTime", nTestLive.Value);

            NM = El.Element("Network");
            if (NM == null)
            {
                NM = new XElement("Network");
                El.Add(NM);
            }
            NM.SetAttributeValue("Port", Port);

            XmlDoc.Save(InitFileName);
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

        private void FormServer_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (Server.Running)
                Server.StopServer();
            SaveInit();
        }

        private NMServer Server;

        private void button1_Click(object sender, EventArgs e)
        {
            if (Server.Running)
            {
                Server.StopServer();//сервер остановили, а результат отлавдивается в OnStop
            }
            else
            {
                Server.Port = Port;
                Server.liveTime = (int) nTestLive.Value;
                Server.RunServer();
                nPort.Enabled = false;
                button1.Text = "Стоп";
                startToolStripMenuItem.Text = "Стоп";
            }
        }

        private void nTestLive_ValueChanged(object sender, EventArgs e)
        {
            Server.liveTime = (int)nTestLive.Value;
        }

        private void FormServer_Resize(object sender, EventArgs e)
        {
            SetView();
        }

        public void SetView()
        {
            ShowInTaskbar = WindowState != FormWindowState.Minimized;
            if (!ShowInTaskbar)
            {
                Hide();
                showToolStripMenuItem.Text = "Показать";
            }
            else
            {
                Show();
                showToolStripMenuItem.Text = "Скрыть";
            }
        }

        private void ShowHide()
        {
            if (WindowState == FormWindowState.Minimized)
            {
                WindowState = FormWindowState.Normal;
            }
            else
                WindowState = FormWindowState.Minimized;
            SetView();
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ShowHide();
        }

        private bool CanClose = false;

        private void CloseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CanClose = true;
            Close();
        }

        private void showToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowHide();
        }

        private void FormServer_Load(object sender, EventArgs e)
        {
            ShowHide();
            button1_Click(button1, new EventArgs());
        }

        private void FormServer_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!CanClose)
            {
                e.Cancel = true;
                if (WindowState != FormWindowState.Minimized)
                    ShowHide();
            }
        }
    }
}
