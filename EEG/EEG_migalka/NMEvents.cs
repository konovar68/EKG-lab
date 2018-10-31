using System;

namespace NetManager
{
    /// <summary>
    /// аргумент для событий содержащий строковое сообщение
    /// </summary>
    class EventMsgArgs : EventArgs
    {
        /// <summary>
        /// сообщение
        /// </summary>
        public string Msg;

        /// <summary>
        /// конструктор аргумента события
        /// </summary>
        /// <param name="Message">Сообщение</param>
        public EventMsgArgs(string Message)
        {
            Msg = Message;
        }
    }

    /// <summary>
    /// класс для аргументов событий добавления и удаления клиентов
    /// </summary>
    class EventClientArgs : EventArgs
    {
        /// <summary>
        /// адрес клиента
        /// </summary>
        public int ClientId;

        /// <summary>
        /// название клиента
        /// </summary>
        public string Name;

        /// <summary>
        /// Конструктор класса для аргументов событий удаления и добавления клиентов
        /// </summary>
        /// <param name="Id">Адрес клиента</param>
        /// <param name="Name">Название клиента</param>
        public EventClientArgs(int Id, string Name)
        {
            ClientId = Id;
            this.Name = Name;
        }
    }

    /// <summary>
    /// Класс аргументов для события получения данных
    /// </summary>
    class EventClientMsgArgs : EventArgs
    {
        /// <summary>
        /// Адрес клиента отправителя
        /// </summary>
        public int ClientId;

        /// <summary>
        /// Имя клиента отправителя
        /// </summary>
        public string Name;

        /// <summary>
        /// Массив байт полученных данных
        /// </summary>
        public byte[] Msg;

        /// <summary>
        /// Констуктор класса аргументов для события получения данных
        /// </summary>
        /// <param name="Id">Адрес клиента отправителя</param>
        /// <param name="Name">Имя клиента отправителя</param>
        /// <param name="Data">Массив байт полученных данных</param>
        public EventClientMsgArgs(int Id, string Name, byte[] Data)
        {
            ClientId = Id;
            this.Name = Name;
            Msg = Data;
        }
    }
}
