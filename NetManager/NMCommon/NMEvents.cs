using System;

namespace NetManager
{
    class EventMsgArgs : EventArgs
    {
        public string Msg;

        public EventMsgArgs(string Message)
        {
            Msg = Message;
        }
    }

    class EventClientArgs : EventArgs
    {
        public int ClientId;

        public string Name;

        public EventClientArgs(int Id, string Name)
        {
            ClientId = Id;
            this.Name = Name;
        }
    }

    class EventClientMsgArgs : EventArgs
    {
        public int ClientId;

        public string Name;

        public byte[] Msg;

        public EventClientMsgArgs(int Id, string Name, byte[] Data)
        {
            ClientId = Id;
            this.Name = Name;
            Msg = Data;
        }
    }
}
