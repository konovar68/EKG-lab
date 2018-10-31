using System;
using System.Net.Sockets;
using System.Threading;
using NetManager;

namespace NetManager
{
    class ClientSocket
    {
        private static int _Id = 0;
        
        public string Name = "";//название

        private int Id_;

        public int Id
        {
            get
            {
                return Id_;
            }
        }

        public TcpClient Client;//сокет подключения

        private Semaphore SWrite;

        private int c = 0;

        public void Write(byte[] data)
        {
            SWrite.WaitOne();
            try
            {
                c++;
                NetworkStream ns = Client.GetStream();
                ns.Write(data, 0, data.Length);
                c--;
            }
            finally
            {
                SWrite.Release();
            }
        }
        
        public ClientSocket() 
        {
            _Id++;
            Id_ = _Id;
            SWrite = new Semaphore(1, 1);
        }

        public ClientSocket(string Name, TcpClient Client)
        {
            _Id++;
            Id_ = _Id;
            SWrite = new Semaphore(1, 1);

            this.Name = Name;
            this.Client = Client;
        }

        public override string ToString()
        {
            return Name + " (" + Id.ToString() + ")";
        }
    }
}