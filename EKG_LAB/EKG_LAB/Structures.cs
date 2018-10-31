using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EEG
{
    /// <summary>
    /// структура передачи времени
    /// </summary>
    class SystemTime
    {
        public ushort wYear;
        public ushort wMonth;
        public ushort wDayOfWeek;
        public ushort wDay;
        public ushort wHour;
        public ushort wMinute;
        public ushort wSecond;
        public ushort wMilliseconds;

        public SystemTime()
        {
            DateTime d = DateTime.Now;
            wYear = (ushort)d.Year;
            wMonth = (ushort)d.Month;
            wDayOfWeek = (ushort)d.DayOfWeek;
            wDay = (ushort)d.Day;
            wHour = (ushort)d.Hour;
            wMinute = (ushort)d.Minute;
            wSecond = (ushort)d.Second;
            wMilliseconds = (ushort)d.Millisecond;
        }

        public SystemTime(DateTime d)
        {
            wYear = (ushort)d.Year;
            wMonth = (ushort)d.Month;
            wDayOfWeek = (ushort)d.DayOfWeek;
            wDay = (ushort)d.Day;
            wHour = (ushort)d.Hour;
            wMinute = (ushort)d.Minute;
            wSecond = (ushort)d.Second;
            wMilliseconds = (ushort)d.Millisecond;
        }
    }

    /// <summary>
    /// Структура для передачи данных
    /// </summary>
    class Frame
    {
        /// <summary>
        /// Код - всегда 6
        /// </summary>
        private int Code = 6;

        /// <summary>
        /// Время получения данных
        /// </summary>
        public SystemTime B_time;

        /// <summary>
        /// Время отправки данных
        /// </summary>
        public SystemTime E_time;

        /// <summary>
        /// Зарезервировано 28 байт
        /// </summary>
        public byte[] reserved;

        /// <summary>
        /// код частоты дискретизации
        /// </summary>
        public int frequency;

        /// <summary>
        /// данные ЭЭГ 29x24
        /// </summary>
        public short[] Data;

        /// <summary>
        /// конструктор, создающий фрейм с заданными параметрами
        /// </summary>
        public Frame()
        {
            B_time = new SystemTime();
            E_time = new SystemTime();
            reserved = new byte[28];
            frequency = 0;
            Data = new short[29 * 24];
        }

        /// <summary>
        /// Конструктор восстанавливающий структуру из массива байт
        /// </summary>
        /// <param name="Buffer">Массив байт с данными структуры</param>
        public Frame(byte[] Buffer)
        {
            if (Code == BitConverter.ToInt32(Buffer, 0))
            {
                B_time = new SystemTime();
                B_time.wYear = BitConverter.ToUInt16(Buffer, 4);
                B_time.wMonth = BitConverter.ToUInt16(Buffer, 6);
                B_time.wDayOfWeek = BitConverter.ToUInt16(Buffer, 8);
                B_time.wDay = BitConverter.ToUInt16(Buffer, 10);
                B_time.wHour = BitConverter.ToUInt16(Buffer, 12);
                B_time.wMinute = BitConverter.ToUInt16(Buffer, 14);
                B_time.wSecond = BitConverter.ToUInt16(Buffer, 16);
                B_time.wMilliseconds = BitConverter.ToUInt16(Buffer, 18);
                E_time = new SystemTime();
                E_time.wYear = BitConverter.ToUInt16(Buffer, 20);
                E_time.wMonth = BitConverter.ToUInt16(Buffer, 22);
                E_time.wDayOfWeek = BitConverter.ToUInt16(Buffer, 24);
                E_time.wDay = BitConverter.ToUInt16(Buffer, 26);
                E_time.wHour = BitConverter.ToUInt16(Buffer, 28);
                E_time.wMinute = BitConverter.ToUInt16(Buffer, 30);
                E_time.wSecond = BitConverter.ToUInt16(Buffer, 32);
                E_time.wMilliseconds = BitConverter.ToUInt16(Buffer, 34);
                reserved = new byte[28];
                for (int I = 0; I < reserved.Length; I++)
                    reserved[I] = Buffer[36 + I];
                frequency = BitConverter.ToInt32(Buffer, 64);
                Data = new short[29 * 24];
                for (int I = 0; I < Data.Length; I++)
                    Data[I] = BitConverter.ToInt16(Buffer, 68 + 2 * I);
            }
            else
                throw new Exception("Неверный формат данных");
        }

        /// <summary>
        /// Копирует биты в указанное место
        /// </summary>
        /// <param name="source">откуда копируется</param>
        /// <param name="destination">куда копируется</param>
        /// <param name="Index">номер первого элемента, с которого будет осуществлено копирование</param>
        private void PutBytes(byte[] source, byte[] destination, int Index)
        {
            for (int I = 0; I < source.Length; I++)
                destination[Index + I] = source[I];
        }

        /// <summary>
        /// Получает структуру в виде массива байт
        /// </summary>
        /// <returns>массив байт структуры</returns>
        public byte[] GetBytes()
        {
            byte[] res = new byte[Size];

            PutBytes(BitConverter.GetBytes(Code), res, 0);
            PutBytes(BitConverter.GetBytes(B_time.wYear), res, 4);
            PutBytes(BitConverter.GetBytes(B_time.wMonth), res, 6);
            PutBytes(BitConverter.GetBytes(B_time.wDayOfWeek), res, 8);
            PutBytes(BitConverter.GetBytes(B_time.wDay), res, 10);
            PutBytes(BitConverter.GetBytes(B_time.wHour), res, 12);
            PutBytes(BitConverter.GetBytes(B_time.wMinute), res, 14);
            PutBytes(BitConverter.GetBytes(B_time.wSecond), res, 16);
            PutBytes(BitConverter.GetBytes(B_time.wMilliseconds), res, 18);
            PutBytes(BitConverter.GetBytes(E_time.wYear), res, 20);
            PutBytes(BitConverter.GetBytes(E_time.wMonth), res, 22);
            PutBytes(BitConverter.GetBytes(E_time.wDayOfWeek), res, 24);
            PutBytes(BitConverter.GetBytes(E_time.wDay), res, 26);
            PutBytes(BitConverter.GetBytes(E_time.wHour), res, 28);
            PutBytes(BitConverter.GetBytes(E_time.wMinute), res, 30);
            PutBytes(BitConverter.GetBytes(E_time.wSecond), res, 32);
            PutBytes(BitConverter.GetBytes(E_time.wMilliseconds), res, 34);
            PutBytes(reserved, res, 36);
            PutBytes(BitConverter.GetBytes(frequency), res, 64);
            for (int I = 0; I < Data.Length; I++)
            {
                PutBytes(BitConverter.GetBytes(Data[I]), res, 68 + 2 * I);
            }

            return res;
        }

        /// <summary>
        /// количество каналов в массиве данных
        /// </summary>
        public static int CountChannels
        {
            get
            {
                return 29;
            }
        }

        /// <summary>
        /// число элементов в канале
        /// </summary>
        public static int LengthData
        {
            get
            {
                return 24;
            }
        }

        /// <summary>
        /// размер структуры
        /// </summary>
        public static int Size
        {
            get
            {
                return 1460;
            }
        }
    }
}
