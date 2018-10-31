using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
using System.Threading;

namespace NetManager
{
    /// <summary>
    /// Класс для подключения к серверу и передачи параметров
    /// </summary>
    class NMClient
    {
        /// <summary>
        /// для синхронизации потоков при вызове событий
        /// </summary>
        public Control Control;

        /// <summary>
        /// Здесь хранится номер порта
        /// </summary>
        private int _Port;

        /// <summary>
        /// Номер порта сервера, к которому идет подключение
        /// </summary>
        public int Port
        {
            get
            {
                return _Port;
            }

            set
            {
                if (!Running)
                {
                    _Port = value;
                }
                else
                    throw new Exception("Нельзя изменить номер порта у запущенного клиента");
            }
        }

        /// <summary>
        /// Здесь хранится IP адрес сервера
        /// </summary>
        private IPAddress _IPServer;

        /// <summary>
        /// IP адрес сервера
        /// </summary>
        public IPAddress IPServer
        {
            get
            {
                return _IPServer;
            }
            set
            {
                if (!Running)
                {
                    _IPServer = value;
                }
                else
                    throw new Exception("Нельзя изменить IP адрес сервера у запущенного клиента");
            }
        }

        /// <summary>
        /// Здесь хранится имя клиента
        /// </summary>
        private string _Name;

        /// <summary>
        /// Название клиента, которое увидят все другие клиенты
        /// </summary>
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                if (!Running)
                {
                    _Name = value;
                }
                else
                    throw new Exception("Нельзя изменить название у запущенного клиента");
            }
        }

        /// <summary>
        /// true - клиент запущен
        /// </summary>
        private bool _Running;

        /// <summary>
        /// Сообщает о том, подключен ли клиент к серверу
        /// </summary>
        public bool Running
        {
            get
            {
                return _Running;
            }
        }

        /// <summary>
        /// Событие, возникающее при ошибках
        /// </summary>
        public event EventHandler<EventMsgArgs> OnError;

        /// <summary>
        /// Делегат для события обработки ошибок
        /// </summary>
        /// <param name="msg">Сообщение об ошибке</param>
        private delegate void DelegateErrorEvent(string msg);

        /// <summary>
        /// Обработка ошибки - процедура синхронизируется с потоком Control и вызывается событие
        /// </summary>
        /// <param name="msg">Сообщение об ошибке</param>
        private void ErrorEvent(string msg)
        {
            if (Control.InvokeRequired)
            {
                DelegateErrorEvent Ev = new DelegateErrorEvent(ErrorEvent);
                Control.Invoke(Ev, msg);
            }
            else
            {
                if (OnError != null)
                    OnError(this, new EventMsgArgs(msg));
            }
        }

        /// <summary>
        /// Событие возникающее когда к серверу добавляется новый клиент
        /// </summary>
        public event EventHandler<EventClientArgs> OnNewClient;

        /// <summary>
        /// Делегат обработки события добавления или удаления клиента
        /// </summary>
        /// <param name="Id">Адрес клиента</param>
        /// <param name="Name">Имя клиента</param>
        private delegate void DelegateChangeClientEvent(int Id, string Name);

        /// <summary>
        /// Обработка добавления нового клиента - процедура синхронизируется с потоком Control и вызывается событие
        /// </summary>
        /// <param name="Id">Адрес клиента</param>
        /// <param name="Name">Имя клиента</param>
        private void NewClientEvent(int Id, string Name)
        {
            if ((Control != null) && Control.InvokeRequired)
            {
                DelegateChangeClientEvent Ev = new DelegateChangeClientEvent(NewClientEvent);
                Control.Invoke(Ev, Id, Name);
            }
            else
                if (OnNewClient != null)
                OnNewClient(this, new EventClientArgs(Id, Name));
        }

        /// <summary>
        /// Событие, возникающее когда от сервера отключается клиент
        /// </summary>
        public event EventHandler<EventClientArgs> OnDeleteClient;

        /// <summary>
        /// Обработка удаления нового клиента - процедура синхронизируется с потоком Control и вызывается событие
        /// </summary>
        /// <param name="Id">Адрес клиента</param>
        /// <param name="Name">Имя клиента</param>
        private void DeleteClientEvent(int Id, string Name)
        {
            if ((Control != null) && Control.InvokeRequired)
            {
                DelegateChangeClientEvent Ev = new DelegateChangeClientEvent(DeleteClientEvent);
                Control.Invoke(Ev, Id, Name);
            }
            else
                if (OnDeleteClient != null)
                OnDeleteClient(this, new EventClientArgs(Id, Name));
        }

        /// <summary>
        /// Закрывается соединение с сервером
        /// </summary>
        /// <param name="Client">Tcp клиент, по которому было осуществлено подключение</param>
        private void CloseConnect(TcpClient Client)
        {
            try
            {
                byte[] buf = new byte[2 * sizeof(int)];
                byte[] tmp = BitConverter.GetBytes((int)0);
                for (int I = 0; I < tmp.Length; I++)
                    buf[I] = tmp[I];
                tmp = BitConverter.GetBytes((int)-1);
                for (int I = 0; I < tmp.Length; I++)
                    buf[sizeof(int) + I] = tmp[I];
                NetworkStream ns = Client.GetStream();
                ns.Write(buf, 0, buf.Length);
            }
            catch { };
            Client.Close();
            StopEvent();
        }

        /// <summary>
        /// Событие возникающее когда клиент останавливается
        /// </summary>
        public event EventHandler OnStop;

        /// <summary>
        /// Делегат для события отключения клиента от сервера
        /// </summary>
        private delegate void DelegateCloseEvent();

        /// <summary>
        /// Обработка отключения клиента - процедура синхронизируется с потоком Control и вызывается событие
        /// </summary>
        private void StopEvent()
        {
            if ((Control != null) && Control.InvokeRequired)
            {
                DelegateCloseEvent Ev = new DelegateCloseEvent(StopEvent);
                Control.Invoke(Ev);
            }
            else
                if (OnStop != null)
                OnStop(this, new EventArgs());
        }

        /// <summary>
        /// Здес хранится список подключенных клиентов
        /// </summary>
        private List<ClientAddress> _ClientAddresses;

        /// <summary>
        /// Список клиентов, подключенных к серверу
        /// </summary>
        public List<ClientAddress> ClientAddresses
        {
            get
            {
                return _ClientAddresses;
            }
        }

        /// <summary>
        /// Инструмент подключения к серверу по протоколу Tcp
        /// </summary>
        private TcpClient Client;

        /// <summary>
        /// Семафор для синхронизации получения данных
        /// </summary>
        private Semaphore SReseive;

        /// <summary>
        /// Событие, возникающее когда от сервера пришли данные
        /// </summary>
        public event EventHandler<EventClientMsgArgs> OnReseive;

        /// <summary>
        /// Делегат обработки события получения данных
        /// </summary>
        /// <param name="Id">Адрес источника</param>
        /// <param name="Msg">Массив байт данных полученных от источника</param>
        private delegate void DelegateReseiveEvent(int Id, byte[] Msg);

        /// <summary>
        /// Обработка получения данных - процедура синхронизируется с потоком Control и вызывается событие
        /// </summary>
        /// <param name="Id">Адрес источника</param>
        /// <param name="Msg">Массив байт данных полученных от источника</param>
        private void ReseiveEvent(int Id, byte[] Msg)
        {
            if ((Control != null) && Control.InvokeRequired)
            {
                DelegateReseiveEvent Ev = new DelegateReseiveEvent(ReseiveEvent);
                Control.Invoke(Ev, Id, Msg);
            }
            else
            {
                SReseive.WaitOne();
                int I = ClientAddresses.Count - 1;
                while ((I >= 0) && (ClientAddresses[I].Id != Id))
                    I--;
                if (I >= 0)
                {
                    string Name = ClientAddresses[I].Name;
                    if (OnReseive != null)
                        OnReseive(this, new EventClientMsgArgs(Id, Name, Msg));
                }
                SReseive.Release();
            }
        }

        /// <summary>
        /// Считывает данные по сети
        /// </summary>
        /// <param name="ns">Соединение по сети</param>
        /// <param name="Size">Размер считываемых данных</param>
        /// <returns></returns>
        private byte[] Read(NetworkStream ns, int Size)
        {
            byte[] buf = new byte[Size];
            int N = ns.Read(buf, 0, Size);
            int S = 0;
            while (N != Size)
            {
                S += N;
                Size -= N;
                N = ns.Read(buf, S, Size);
            }
            return buf;
        }

        /// <summary>
        /// Работа клиента (получение данных и команд от сервера)
        /// </summary>
        private void Run_Client()
        {
            try
            {
                Client = new TcpClient(IPServer.AddressFamily);
                Client.SendBufferSize = 8388608;
                Client.ReceiveBufferSize = 8388608;
                Client.Connect(new IPEndPoint(IPServer, Port));
                NetworkStream ns = Client.GetStream();
                ns.Write(BitConverter.GetBytes(Name.Length), 0, sizeof(int));
                //передается на сервер имя клиента
                for (int I = 0; I < Name.Length; I++)
                {
                    byte[] buf = BitConverter.GetBytes(Name[I]);
                    ns.Write(buf, 0, buf.Length);
                }
                while (Running && Client.Connected)
                {
                    int N = BitConverter.ToInt32(Read(ns, sizeof(int)), 0);
                    if (Running && Client.Connected)
                    {
                        if (N == 0)//обработка сообщений от сервера
                        {
                            N = BitConverter.ToInt32(Read(ns, sizeof(int)), 0);
                            if (Running && Client.Connected)
                            {
                                if (N != 0)//код команды
                                {
                                    if (N != (int)0x7FFFFFFF)
                                    {
                                        int Id = BitConverter.ToInt32(Read(ns, sizeof(int)), 0);
                                        if (Running && Client.Connected)
                                        {
                                            int Len = BitConverter.ToInt32(Read(ns, sizeof(int)), 0);
                                            if (Running && Client.Connected)
                                            {
                                                byte[] buf = Read(ns, Len * sizeof(char));
                                                if (Running && Client.Connected)
                                                {
                                                    string S = "";
                                                    for (int I = 0; I < buf.Length; I += sizeof(char))
                                                        S += BitConverter.ToChar(buf, I);
                                                    switch (N)
                                                    {
                                                        case 1:
                                                            ClientAddresses.Add(new ClientAddress(Id, S));
                                                            NewClientEvent(Id, S);
                                                            break;
                                                        case -1:
                                                            int I = ClientAddresses.Count - 1;
                                                            while ((I >= 0) && (ClientAddresses[I].Id != Id))
                                                                I--;
                                                            if (I >= 0)
                                                            {
                                                                ClientAddresses.RemoveAt(I);
                                                                DeleteClientEvent(Id, S);
                                                            }
                                                            break;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                else //отключение
                                {
                                    ClientAddresses.Clear();
                                    _Running = false;
                                    Client.Close();
                                    StopEvent();
                                    return;
                                }
                            }
                        }
                        else //получение сообщения
                        {
                            int Len = BitConverter.ToInt32(Read(ns, sizeof(int)), 0);
                            if (Running && Client.Connected)
                            {
                                byte[] buf = Read(ns, Len);
                                if (Running && Client.Connected)
                                    ReseiveEvent(N, buf);
                            }
                        }
                    }
                }
                CloseConnect(Client);
            }
            catch (Exception e)
            {
                if (_Running)
                {
                    _Running = false;
                    if (Client != null)
                        CloseConnect(Client);

                    ErrorEvent(e.Message);
                }
            }
        }

        /// <summary>
        /// Отправляет данные через сервер клиенту по указанному адресу
        /// <param name="Address">Адрес назначения</param>
        /// <param name="Data">Массив байт данных</param>
        /// </summary>
        public void SendData(int Address, byte[] Data)
        {
            int[] arr = new int[1];
            arr[0] = Address;
            SendData(arr, Data);
        }

        /// <summary>
        /// Отправляет данные через сервер клиентам по указанному адресу
        /// </summary>
        /// <param name="Addresses">Массив адресов назначения</param>
        /// <param name="Data">Массив байт данных</param>
        public void SendData(int[] Addresses, byte[] Data)
        {
            if (Running && Client.Connected && (Addresses.Length > 0) && (Data.Length > 0))
            {
                NetworkStream ns = Client.GetStream();
                if (Running && Client.Connected)
                {
                    byte[] tmp = new byte[8 + Addresses.Length * sizeof(int) + Data.Length];
                    Array.Copy(BitConverter.GetBytes(Addresses.Length), 0, tmp, 0, sizeof(int));
                    if (Running && Client.Connected)
                    {
                        int I = 0;
                        while ((I < Addresses.Length) && Running && Client.Connected)
                            Array.Copy(BitConverter.GetBytes(Addresses[I++]), 0, tmp, I * sizeof(int), sizeof(int));
                        Array.Copy(BitConverter.GetBytes(Data.Length), 0, tmp, (Addresses.Length + 1) * sizeof(int), sizeof(int));
                        Array.Copy(Data, 0, tmp, (Addresses.Length + 2) * sizeof(int), Data.Length);
                        if (Running && Client.Connected)
                            ns.Write(tmp, 0, tmp.Length);
                    }
                }
            }
        }

        /// <summary>
        /// для запуска процедуры Run_Client() в новом потоке
        /// </summary>
        private Thread ThreadRunClient;

        /// <summary>
        /// Подключение клиента к серверу
        /// </summary>
        public void RunClient()
        {
            if (!Running)
            {
                try
                {
                    _Running = true;

                    ThreadRunClient = new Thread(Run_Client);
                    ThreadRunClient.IsBackground = true;
                    ThreadRunClient.Start();
                }
                catch (Exception e)
                {
                    _Running = false;
                    throw e;
                }
            }
            else
                throw new Exception("Клиент уже запущен");
        }

        /// <summary>
        /// Отключение клиента от сервера
        /// </summary>
        public void StopClient()
        {
            if (Running)
            {
                _Running = false;
                CloseConnect(Client);
            }
            else
                throw new Exception("Клиент уже остановлен");
        }

        /// <summary>
        /// Заставляет сервер перезапуститься
        /// </summary>
        public void SendRestartServer()
        {
            if (Running)
            {
                byte[] data = new byte[8];
                NetworkStream ns = Client.GetStream();
                data[0] = 0x00;
                data[1] = 0x00;
                data[2] = 0x00;
                data[3] = 0x00;
                data[4] = 0xFF;
                data[5] = 0xFF;
                data[6] = 0xFF;
                data[7] = 0x7F;
                ns.Write(data, 0, 8);
            }
        }

        /// <summary>
        /// Конструктор клиента
        /// </summary>
        /// <param name="Control">Окно, внутри которого осуществляется управление клиентом.
        /// Используется для того, чтобы синхронизировать асинхронные процессы с процессом окна</param>
        public NMClient(Control Control)
        {
            _Running = false;
            _ClientAddresses = new List<ClientAddress>();
            SReseive = new Semaphore(1, 1);

            this.Control = Control;
        }

        /// <summary>
        /// Конструктор клиента
        /// </summary>
        /// <param name="IPServer">IP адрес сервера</param>
        /// <param name="Port">Номер порта сервера</param>
        /// <param name="Control">Окно, внутри которого осуществляется управление клиентом.
        /// Используется для того, чтобы синхронизировать асинхронные процессы с процессом окна</param>
        public NMClient(IPAddress IPServer, int Port, Control Control)
        {
            _Running = false;
            _ClientAddresses = new List<ClientAddress>();
            SReseive = new Semaphore(1, 1);

            this.IPServer = IPServer;
            this.Port = Port;
            this.Control = Control;
        }

        /// <summary>
        /// деструктор, в котором останавливается клиент, если он не был остановлен
        /// </summary>
        ~NMClient()
        {
            if (Running)
                StopClient();
        }
    }
}
