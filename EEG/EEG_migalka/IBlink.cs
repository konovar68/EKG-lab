using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EEG_migalka
{
    /// <summary>
    /// Интерфейс работы мигалки
    /// </summary>
    interface IBlink
    {
        /// <summary>
        /// Частоты
        /// </summary>
        /// <param name="Index">Порядковый номер мигающего элемента</param>
        /// <returns></returns>
        double this[int Index]
        {
            get;
            set;
        }

        /// <summary>
        /// Число мигающих элементов
        /// </summary>
        int Count
        {
            get;
            set;
        }

        /// <summary>
        /// Запуск процесса
        /// </summary>
        void Start();

        /// <summary>
        /// Останов процесса
        /// </summary>
        void Stop();

        /// <summary>
        /// Выдает число, где единица в позиции в двоичной системе определяет, что соответствующий диод горит
        /// </summary>
        byte IsBlink
        {
            get;
        }
    }
}
