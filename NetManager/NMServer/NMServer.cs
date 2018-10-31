using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;
using System.Collections;

namespace NetManager
{
    /// <summary>
    /// класс сервер
    /// </summary>
    class NMServer
    {
        /// <summary>
        /// используется для синхронизации потоков при передаче событий. если null, то синхронизация не происходит
        /// </summary>
        public Control Control; 

        /// <summary>
        /// номер порта
        /// </summary>
        private int _Port;

        /// <summary>
        /// true - запущено
        /// </summary>
        private bool _Running;

        /// <summary>
        /// true - запущено
        /// </summary>
        public bool Running
        {
            get
            {
                return _Running;
            }
        }

        /// <summary>
        /// номер порта
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
                    throw new Exception("Нельзя присвоить номер порта не остановив сервер");
            }
        }

        /// <summary>
        /// конструктор создающий объект класса NMServer
        /// </summary>
        /// <param name="Control">визуальная форма, с потоком которой нужно синхронизировать вывод событий</param>
        public NMServer(Control Control)
        {
            _Running = false;
            _Clients = ArrayList.Synchronized(new ArrayList());
            this.Control = Control;

            SClientEdit = new Semaphore(1, 1);
        }

        /// <summary>
        /// конструктор создающий объект класса NMServer
        /// </summary>
        /// <param name="Port">номер порта</param>
        /// <param name="Control">визуальная форма, с потоком которой нужно синхронизировать вывод событий</param>
        public NMServer(int Port, Control Control)
        {
            this.Port = Port;
            _Running = false;
            _Clients = ArrayList.Synchronized(new ArrayList());
            this.Control = Control;

            SClientEdit = new Semaphore(1, 1);
        }

        /// <summary>
        /// деструктор, чтобы остановить потоки, если они были запущены
        /// </summary>
        ~NMServer()
        {
            if (Running)
                StopServer();
        }

        /// <summary>
        /// список клиентов
        /// </summary>
        private ArrayList _Clients;

        /// <summary>
        /// список клиентов
        /// </summary>
        public IList<ClientSocket> Clients
        {
            get
            {
                return Array.AsReadOnly<ClientSocket>((ClientSocket[])_Clients.ToArray(typeof(ClientSocket)));
            }
        }

        /// <summary>
        /// обработка ошибок
        /// </summary>
        public event EventHandler<EventMsgArgs> OnError;

        /// <summary>
        /// для синхронизации события обработки ошибок
        /// </summary>
        /// <param name="msg"></param>
        private delegate void DelegateErrorEvent(string msg);

        /// <summary>
        /// вывод события обработки ошибок
        /// </summary>
        /// <param name="msg">сообщение ошибки</param>
        private void ErrorEvent(string msg)
        {
            if ((Control != null) && Control.InvokeRequired)
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
        /// отправляет данные клиенту по указанному адресу
        /// </summary>
        /// <param name="Address">адрес</param>
        /// <param name="data">данные</param>
        private void Write(int Address, byte[] data)
        {
            int I = _Clients.Count - 1;
            try
            {
                while (Running && (I >= 0) && (((ClientSocket)_Clients[I]).Id != Address))
                    I--;
                if (Running && (I >= 0) && ((ClientSocket)_Clients[I]).Client.Connected)
                    ((ClientSocket)_Clients[I]).Write(data);
            }
            catch
            {
                (new delegateDeleteClient(DeleteClient)).BeginInvoke(((ClientSocket)_Clients[I]), null, null);
            }
        }

        /// <summary>
        /// время, по истечении которого идет проверка - жив ли клиент
        /// </summary>
        private int live_Time = 0;

        /// <summary>
        /// время, по истечении которого идет проверка - жив ли клиент
        /// </summary>
        public int liveTime
        {
            get
            {
                return live_Time;
            }
            set
            {
                live_Time = value;
            }
        }

        /// <summary>
        /// проверка, жив ли клиент
        /// </summary>
        private void TestLive()
        {
            while (Running)
            {
                if ((liveTime > 0) && (_Clients.Count > 0))
                {
                    if (Thread.CurrentThread.Priority != ThreadPriority.AboveNormal)
                        Thread.CurrentThread.Priority = ThreadPriority.AboveNormal;

                    byte[] buf = new byte[8];
                    buf[0] = 0;
                    buf[1] = 0;
                    buf[2] = 0;
                    buf[3] = 0;
                    buf[4] = 0xFF;
                    buf[5] = 0xFF;
                    buf[6] = 0xFF;
                    buf[7] = 0x7F;
                    for (int I = 0; I < _Clients.Count; I++)
                    {
                        ClientSocket CS = null;
                        try
                        {
                            CS = (ClientSocket)_Clients[I];
                            CS.Write(buf);
                        }
                        catch
                        {
                            if (CS == null)
                                (new delegateDeleteClient(DeleteClient)).BeginInvoke(CS, null, null);
                        }
                    }
                    Thread.Sleep(liveTime);
                }
                else
                {
                    if (Thread.CurrentThread.Priority != ThreadPriority.Lowest)
                        Thread.CurrentThread.Priority = ThreadPriority.Lowest;
                    Thread.Sleep(1000);
                }
            }
        }

        /// <summary>
        /// true - сервер перезагружается
        /// </summary>
        private bool Restarting = false;

        /// <summary>
        /// перезагрузка сервера
        /// </summary>
        private void Restart()
        {
            Restarting = true;
            try
            {
                StopServer();

                if (ThreadRunServer.IsAlive)
                    ThreadRunServer.Abort();
                if (ThreadTestLive.IsAlive)
                    ThreadTestLive.Abort();

                RunServer();
            }
            finally
            {
                Restarting = false;
            }
        }

        /// <summary>
        /// Перезапуск сервера
        /// </summary>
        private Thread ThreadRestart;

        /// <summary>
        /// Чтение данных из сетевого потока
        /// </summary>
        /// <param name="ns">Сетевой поток откуда происходит чтение</param>
        /// <param name="Size">Число байт, которое нужно прочтать</param>
        /// <returns></returns>
        private byte[] Read(NetworkStream ns, int Size)
        {
            byte[] tmp = new byte[Size];
            int N = ns.Read(tmp, 0, Size);
            int S = 0;
            while (N != Size)
            {
                S += N;
                Size = Size - N;
                N = ns.Read(tmp, S, Size); 
            }

            return tmp;
        }

        /// <summary>
        /// Работа клиента
        /// </summary>
        /// <param name="Client">текущий клиент</param>
        private void RunClient(ClientSocket Client)
        {
            NetworkStream ns = Client.Client.GetStream();
            try
            {
                while (Running && Client.Client.Connected)
                {
                    int N = BitConverter.ToInt32(Read(ns, sizeof(int)), 0);
                    if (Running && Client.Client.Connected)
                    {
                        if (N == 0) //данные для сервера
                        {
                            //считывается команда
                            int Com = BitConverter.ToInt32(Read(ns, sizeof(int)), 0);
                            if (Running && Client.Client.Connected)
                            {                                
                                switch (Com)
                                {
                                    case -1://команда закрыть соединение
                                        (new delegateDeleteClient(DeleteClient)).BeginInvoke(Client, null, null);
                                        break;
                                    case 0x7FFFFFFF://команда перезагрузиться
                                        if ((ThreadRestart == null) || (!ThreadRestart.IsAlive))
                                        {
                                            ThreadRestart = new Thread(Restart);
                                            ThreadRestart.IsBackground = true;
                                            ThreadRestart.Start();
                                        }
                                        break;
                                }
                            }
                        }
                        else//пересылка данных
                        {
                            int[] Addresses = new int[N];
                            byte[] buf = Read(ns, (N + 1) * sizeof(int));
                            if (Running && Client.Client.Connected)
                            {
                                for (int I = 0; I < N; I++)
                                    Addresses[I] = BitConverter.ToInt32(buf, I * sizeof(int));
                                int Size = BitConverter.ToInt32(buf, N * sizeof(int));
                                byte[] tmp = Read(ns, Size);
                                buf = new byte[Size + 2 * sizeof(int)];
                                Array.Copy(tmp, 0, buf, 2 * sizeof(int), Size);
                                if (Running && Client.Client.Connected)
                                {
                                    Array.Copy(BitConverter.GetBytes(Client.Id), 0, buf, 0, sizeof(int));
                                    Array.Copy(BitConverter.GetBytes(Size), 0, buf, sizeof(int), sizeof(int));
                                    int I = 0;
                                    while (Running && Client.Client.Connected && (I < N))
                                            Write(Addresses[I++], buf);
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
                (new delegateDeleteClient(DeleteClient)).BeginInvoke(Client, null, null);
            }
        }

        /// <summary>
        /// семафор для синхронизации вставки и удаления клиентов
        /// </summary>
        private Semaphore SClientEdit;
        
        /// <summary>
        /// делегат для синхронизации вызова события добавления и удаления клиента 
        /// с потоком где был создан Control
        /// </summary>
        /// <param name="Id">номер клиента</param>
        /// <param name="Name">имя клиента</param>
        private delegate void DelegateChangeClientEvent(int Id, string Name);

        /// <summary>
        /// вызов события добавления нового клиента
        /// </summary>
        /// <param name="Id">номер клиента</param>
        /// <param name="Name">имя клиента</param>
        private void AddClientEvent(int Id, string Name)
        {
            if ((Control != null) && Control.InvokeRequired)
            {
                DelegateChangeClientEvent Ev = new DelegateChangeClientEvent(AddClientEvent);
                Control.Invoke(Ev, Id, Name);
            }
            else
            {
                if (OnAddClient != null)
                    OnAddClient(this, new EventClientArgs(Id, Name));
            }
        }

        /// <summary>
        /// событие добавления нового клиента
        /// </summary>
        public event EventHandler<EventClientArgs> OnAddClient;

        /// <summary>
        /// копирование данных
        /// </summary>
        /// <param name="Source">откуда копируется</param>
        /// <param name="Destination">куда копируется</param>
        /// <param name="Index">номер последней свободной ячейки</param>
        private void CopyBytes(byte[] Source, byte[] Destination, ref int Index)
        {

            Array.Copy(Source, 0, Destination, Index, Source.Length);
            Index += Source.Length;
        }

        /// <summary>
        /// добавляет новый клиент
        /// </summary>
        /// <param name="CS">новый клиент</param>
        private void AddClient(ClientSocket CS)
        {
            SClientEdit.WaitOne();
            try
            {
                _Clients.Add(CS);
                AddClientEvent(CS.Id, CS.Name);

                byte[] data = new byte[4 * sizeof(int) + CS.Name.Length * sizeof(char)];
                byte[] buf;
                int Index = 0;
                CopyBytes(BitConverter.GetBytes((int)0), data, ref Index);
                CopyBytes(BitConverter.GetBytes((int)1), data, ref Index);
                CopyBytes(BitConverter.GetBytes(CS.Id), data, ref Index);
                CopyBytes(BitConverter.GetBytes(CS.Name.Length), data, ref Index);
                for (int J = 0; J < CS.Name.Length; J++)
                    CopyBytes(BitConverter.GetBytes(CS.Name[J]), data, ref Index);

                for (int I = 0; I < _Clients.Count; I++)
                    if (((ClientSocket)_Clients[I]).Id != CS.Id)
                        try
                        {
                            //передаем сообщение, что добавлен новый клиент
                            ((ClientSocket)_Clients[I]).Write(data);
                            //передаем сообщение о существующем клиенте новому клиенту
                            buf = new byte[4 * sizeof(int) + ((ClientSocket)_Clients[I]).Name.Length * sizeof(char)];
                            Index = 0;
                            CopyBytes(BitConverter.GetBytes((int)0), buf, ref Index);
                            CopyBytes(BitConverter.GetBytes((int)1), buf, ref Index);
                            CopyBytes(BitConverter.GetBytes(((ClientSocket)_Clients[I]).Id), buf, ref Index);
                            CopyBytes(BitConverter.GetBytes(((ClientSocket)_Clients[I]).Name.Length), buf, ref Index);
                            for (int J = 0; J < Clients[I].Name.Length; J++)
                                CopyBytes(BitConverter.GetBytes(((ClientSocket)_Clients[I]).Name[J]), buf, ref Index);
                            CS.Write(buf);
                        }
                        catch
                        {
                            (new delegateDeleteClient(DeleteClient)).BeginInvoke(Clients[I], null, null);
                        }
            }
            finally
            {
                SClientEdit.Release();
            }
        }

        /// <summary>
        /// событие удаления клиента
        /// </summary>
        public event EventHandler<EventClientArgs> OnDeleteClient;

        /// <summary>
        /// удаление клиента
        /// </summary>
        /// <param name="Id">номер клиента</param>
        /// <param name="Name">название клиента</param>
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
        /// делегат для асинхронного вызова удаления клиента (чтобы процессы не зависали, когда клиент удаляется)
        /// </summary>
        /// <param name="CS">клиент</param>
        private delegate void delegateDeleteClient(ClientSocket CS);

        /// <summary>
        /// удаление клиента
        /// </summary>
        /// <param name="CS">клиент</param>
        private void DeleteClient(ClientSocket CS)
        {
            SClientEdit.WaitOne();
            try
            {
                if (_Clients.IndexOf(CS) >= 0)
                {
                    CS.Client.Close();

                    _Clients.Remove(CS);

                    int I;
                    byte[] data = new byte[4 * sizeof(int) + CS.Name.Length * sizeof(char)];
                    int Index = 0;
                    CopyBytes(BitConverter.GetBytes((int)0), data, ref Index);
                    CopyBytes(BitConverter.GetBytes((int)-1), data, ref Index);
                    CopyBytes(BitConverter.GetBytes(CS.Id), data, ref Index);
                    CopyBytes(BitConverter.GetBytes(CS.Name.Length), data, ref Index);
                    for (I = 0; I < CS.Name.Length; I++)
                        CopyBytes(BitConverter.GetBytes(CS.Name[I]), data, ref Index);

                    for (I = 0; I < _Clients.Count; I++)
                        try
                        {
                            ((ClientSocket)_Clients[I]).Write(data);
                        }
                        catch
                        {
                            (new delegateDeleteClient(DeleteClient)).BeginInvoke((ClientSocket)_Clients[I], null, null);
                        }

                    DeleteClientEvent(CS.Id, CS.Name);
                }
            }
            finally
            {
                SClientEdit.Release();
            }
        }

        /// <summary>
        /// используется для вызова асинхронного потока добавления нового клиента
        /// </summary>
        /// <param name="Client">новый клиент</param>
        private delegate void DelegateNewClient(TcpClient Client);

        /// <summary>
        /// используется для запуска асинхронного потока работы клиента
        /// </summary>
        /// <param name="Client"></param>
        private delegate void ClientRuning(ClientSocket Client);

        /// <summary>
        /// добавляет нового клиента
        /// </summary>
        /// <param name="Client">новый клиент</param>
        private void NewClient(TcpClient Client)
        {
            ClientSocket CS = null;
            Client.ReceiveBufferSize = 0xFFFFFF;
            Client.SendBufferSize = 0xFFFFFF;
            try
            {
                NetworkStream ns = Client.GetStream();
                if (Running)
                {
                    int N = BitConverter.ToInt32(Read(ns, sizeof(int)), 0);

                    if (Running)
                    {
                        byte[] data = Read(ns, N * sizeof(char));
                        string name = "";
                        for (int I = 0; I < data.Length; I += 2)
                            name += BitConverter.ToChar(data, I);

                        CS = new ClientSocket(name, Client); 
                        ClientRuning CR = new ClientRuning(RunClient);
                        CR.BeginInvoke(CS, null, null);
                        AddClient(CS);
                    }
                }
            }
            catch 
            {
                if (CS != null)
                    (new delegateDeleteClient(DeleteClient)).BeginInvoke(CS, null, null);
            };
        }

        /// <summary>
        /// событие остановки клиента
        /// </summary>
        public event EventHandler OnStop;

        /// <summary>
        /// событие перезапуска клиента
        /// </summary>
        public event EventHandler OnRestart;

        /// <summary>
        /// делегат для синхронизации события остановки клиента с потоком в котором создан Control
        /// </summary>
        private delegate void DelegateStopEvent();

        /// <summary>
        /// вызов события остановки клиента
        /// </summary>
        private void StopEvent()
        {
            if ((Control != null) && Control.InvokeRequired)
            {
                DelegateStopEvent Ev = new DelegateStopEvent(StopEvent);
                Control.Invoke(Ev);
            }
            else
                if (!Restarting)
                {
                    if (OnStop != null)
                        OnStop(this, new EventArgs());
                }
                else
                    if (OnRestart != null)
                        OnRestart(this, new EventArgs());
        }
            
        /// <summary>
        /// остановка сервера
        /// </summary>
        private void StopListener()
        {
            byte[] buf = new byte[8];
            for (int I = 0; I < 7; I++)
                buf[I] = 0;
            //закрываются связи
            while (Clients.Count > 0)
            {
                try
                {
                    Clients[0].Write(buf);
                    Clients[0].Client.Close();
                }
                catch { };
                Clients.RemoveAt(0);
            }
            //останов
            Listener.Stop();
            StopEvent();
        }

        /// <summary>
        /// сервер
        /// </summary>
        TcpListener Listener = null;

        /// <summary>
        /// работа сервера
        /// </summary>
        private void Run_Server()
        {
            try
            {
                Listener = new TcpListener(IPAddress.Any, Port);
                Listener.Start();
                while (Running)
                {
                    if (Listener.Pending())
                    {
                        DelegateNewClient NC = new DelegateNewClient(NewClient);
                        NC.BeginInvoke(Listener.AcceptTcpClient(), null, null);
                    }
                    else
                        Thread.Sleep(1000);
                }
                StopListener();
            }
            catch (Exception e)
            {
                if (Listener != null)
                    StopListener();
                ErrorEvent(e.Message);
            }
        }

        /// <summary>
        /// Поток для запуска серверного процесса
        /// </summary>
        private Thread ThreadRunServer;

        /// <summary>
        /// Поток для проверки активности клиентов
        /// </summary>
        private Thread ThreadTestLive;

        /// <summary>
        /// запуск работы сервера
        /// </summary>
        public void RunServer()
        {
            if (!Running)
            {
                try
                {
                    //если есть "подвисшие" потоки, они прерываются
                    if ((ThreadRunServer != null) && ThreadRunServer.IsAlive)
                        try
                        {
                            ThreadRunServer.Abort();
                        }
                        catch { };
                    if ((ThreadTestLive != null) && ThreadTestLive.IsAlive)
                        try
                        {
                            ThreadTestLive.Abort();
                        }
                        catch { };

                    _Running = true;

                    ThreadRunServer = new Thread(Run_Server);
                    ThreadRunServer.IsBackground = true;
                    ThreadRunServer.Start();

                    ThreadTestLive = new Thread(TestLive);
                    ThreadTestLive.IsBackground = true;
                    ThreadTestLive.Start();
                }
                catch (Exception e)
                {
                    _Running = false;
                    
                    if ((ThreadRunServer != null) && ThreadRunServer.IsAlive)
                        try
                        {
                            ThreadRunServer.Abort();
                        }
                        catch { };
                    if ((ThreadTestLive != null) && ThreadTestLive.IsAlive)
                        try
                        {
                            ThreadTestLive.Abort();
                        }
                        catch { };

                    throw new Exception(e.Message);
                }
            }
            else
                throw new Exception("Сервер уже запущен");
        }

        /// <summary>
        /// остановка сервера
        /// </summary>
        public void StopServer()
        {
            if (Running)
            {
                _Running = false;
            }
            else
                throw new Exception("Сервер уже остановлен");

        }
    }
}
