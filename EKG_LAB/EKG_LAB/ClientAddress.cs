using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetManager
{
    /// <summary>
    /// Класс содержащий адрес и имя клиента
    /// </summary>
    class ClientAddress
    {
        /// <summary>
        /// Адрес клиента
        /// </summary>
        public int Id;

        /// <summary>
        /// Имя клиента
        /// </summary>
        public string Name;

        /// <summary>
        /// Конструктор класса содержащего адрес и имя клиента
        /// </summary>
        /// <param name="Id">Адрес клиента</param>
        /// <param name="Name">Имя клиента</param>
        public ClientAddress(int Id, string Name)
        {
            this.Id = Id;
            this.Name = Name;
        }

        /// <summary>
        /// Конструктор класса содержащего адрес и имя клиента
        /// </summary>
        public ClientAddress() { }

        public override string ToString()
        {
            return Name + " (" + Id.ToString() + ")";
        }
    }
}
