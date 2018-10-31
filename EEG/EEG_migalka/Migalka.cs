using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Threading;
using EEG_migalka;

namespace Migalka
{
    /// <summary>
    /// Класс реализующий мигалку
    /// </summary>
    class Migalka : IBlink
    {
        /// <summary>
        /// Com-порт, для работы с устройством
        /// </summary>
        private SerialPort serialPort;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="SerialPort">Com-порт</param>
        public Migalka(SerialPort SerialPort)
        {
            this.serialPort = SerialPort;
        }

        /// <summary>
        /// частоты для мигания
        /// </summary>
        private double[] frequency = new double[] { 0, 0, 0, 0, 0, 0, 0, 0 };

        /// <summary>
        /// Индексатор для частот
        /// </summary>
        /// <param name="Index">Номер дода</param>
        /// <returns>Частота диода</returns>
        public double this[int Index]
        {
            get
            {
                return frequency[Index];
            }
            set
            {
                if (Running)
                {
                    string Error = "Нельзя задать частоту во время работы мигалки";
                    if (ErrorEvent != null)
                        ErrorEvent(Error);
                    else
                        throw new Exception(Error);
                }
                else
                    frequency[Index] = value;
            }
        }

        /// <summary>
        /// Число диодов
        /// </summary>
        public int Count
        {
            get
            {
                return frequency.Length;
            }
            set
            {
                throw new Exception("Данный параметр нельзя изменить");
            }
        }

        /// <summary>
        /// Класс для передачи аргументов потокам мигания
        /// </summary>
        private class BlinkArg
        {
            /// <summary>
            /// Частота
            /// </summary>
            public double Frequency;

            /// <summary>
            /// Номер
            /// </summary>
            public int Number;

            /// <summary>
            /// Конструтор с параметрами
            /// </summary>
            /// <param name="Frequency">Частота</param>
            /// <param name="Number">Номер</param>
            public BlinkArg(double Frequency, int Number)
            {
                this.Frequency = Frequency;
                this.Number = Number;
            }
        }

        /// <summary>
        /// Потоки, в которых просходит мигание
        /// </summary>
        private Thread[] Blinks;

        /// <summary>
        /// true - процесс запущен
        /// </summary>
        private bool Running = false;

        /// <summary>
        /// Делегат для обработки ошибок
        /// </summary>
        /// <param name="Msg"></param>
        public delegate void delegateErrorEvent(string Msg);

        /// <summary>
        /// Событие обработки ошибок
        /// </summary>
        public event delegateErrorEvent ErrorEvent;

        /// <summary>
        /// Для синхронизации работы с blink
        /// </summary>
        private object Lock = new object();

        /// <summary>
        /// Процедура для выполнения мигания
        /// </summary>
        /// <param name="Arg">Параметры мигания (BlinkArg)</param>
        private void BlinkProcess(object Arg)
        {
            try
            {
                int Time = (int)Math.Round(500 / (Arg as BlinkArg).Frequency);
                int Number = (Arg as BlinkArg).Number;
                byte f = (byte)(1 << (Number - 17));

                while (serialPort.IsOpen && Running)
                {
                    lock(Lock)
                    {
                        blink ^= f;
                    }
                    if ((blink & f) != 0)
                        serialPort.Write("$KE,WR," + Number.ToString() + ",1" + (char)13 + (char)10);
                    else
                        serialPort.Write("$KE,WR," + Number.ToString() + ",0" + (char)13 + (char)10);
                    Thread.Sleep(Time);
                }

                if (((blink & f) != 0) && serialPort.IsOpen)
                {
                    serialPort.Write("$KE,WR," + Number.ToString() + ",0" + (char)13 + (char)10);
                    blink ^= f;
                }
            }
            catch (Exception exp)
            {
                if (ErrorEvent != null)
                    ErrorEvent(exp.Message);
                else
                    throw new Exception(exp.Message);
            }
        }

        /// <summary>
        /// Процедура дожидается когда закончат работу все потоки мигания, после чего закрывает порт
        /// </summary>
        private void ClosePort()
        {
            Running = false;

            bool F;

            do
            {
                int I = 0;
                while ((I < Blinks.Length) && (Blinks[I] == null || !Blinks[I].IsAlive))
                    I++;
                F = I < Blinks.Length;
            } while (F);

            serialPort.Close();
        }

        /// <summary>
        /// Запуск процесса
        /// </summary>
        public void Start()
        {
            try
            {
                serialPort.Open();

                Running = true;

                Blinks = new Thread[frequency.Length];
                for (int I = 0; I < Blinks.Length; I++)
                    if (frequency[I] != 0)
                    {
                        Blinks[I] = new Thread(BlinkProcess);
                        Blinks[I].IsBackground = true;
                        Blinks[I].Priority = ThreadPriority.Highest;
                        Blinks[I].Start(new BlinkArg(frequency[I], 17 + I));
                    }
            }
            catch (Exception exp)
            {
                if (ErrorEvent != null)
                    ErrorEvent(exp.Message);
                else
                    throw new Exception(exp.Message);
            }
        }

        /// <summary>
        /// Останов процесса
        /// </summary>
        public void Stop()
        {
            ClosePort();
        }

        /// <summary>
        /// Определяет, включен или отключен диод
        /// </summary>
        private byte blink = 0;

        /// <summary>
        /// Выдает число, где единица в позиции в двоичной системе определяет, что соответствующий диод горит
        /// </summary>
        public byte IsBlink
        {
            get
            {
                return blink;
            }
        }
    }
}
