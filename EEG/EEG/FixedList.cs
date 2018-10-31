using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace EEG
{
    /// <summary>
    /// Класс с фиксированным числом элементов (на основе кольцевого буфера)
    /// </summary>
    class FixedList<T>
    {
        /// <summary>
        /// массив данных
        /// </summary>
        private T[] arr;

        /// <summary>
        /// номер первого элемента в кольцевом списке
        /// </summary>
        private int Ind;

        /// <summary>
        /// количество элементов в списке
        /// </summary>
        private int Count_;

        /// <summary>
        /// конструктор
        /// </summary>
        /// <param name="Capacity">количество элементов в списке</param>
        public FixedList(int Capacity)
        {
            arr = new T[Capacity];
            Ind = 0;
            Count_ = 0;
            Semaphore = new Semaphore(1, 1);
        }

        /// <summary>
        /// конструктор
        /// </summary>
        public FixedList() :
            this(1) { }

        /// <summary>
        /// индексатор для работы с элементами буфера
        /// </summary>
        /// <param name="Index">индекс элемента в буфере начиная с первого (первый имеет индекс 0)</param>
        /// <returns>возвращает элемент хранящийся в буфере по указанному индексу</returns>
        public T this[int Index]
        {
            get
            {
                if ((Index >= 0) && (Index < Count_))
                {
                    return arr[(Index + Ind) % arr.Length];
                }
                else
                    throw new Exception("Неверно задан индекс");
            }
            set
            {
                while (LockCount > 0)
                    Thread.Sleep(10);

                Semaphore.WaitOne();
                if ((Index >= 0) && (Index < Count_))
                {
                    arr[(Index + Ind) % arr.Length] = value;
                }
                else
                    throw new Exception("Неверно задан индекс");
                Semaphore.Release();
            }
        }

        /// <summary>
        /// размер кольцевого буфера (если количество элементов при добавлении больше этой емкости, то первый элемент выбывает)
        /// </summary>
        public int Capacity
        {
            get
            {
                return arr.Length;
            }
            set
            {
                while (LockCount > 0)
                    Thread.Sleep(10);

                Semaphore.WaitOne();
                T[] res = new T[value];

                for (int I = 0; I < Math.Min(Count_, value); I++)
                    res[I] = arr[(Ind + I) % arr.Length];
                Ind = 0;
                Count_ = Math.Min(Count_, value);
                arr = res;
                Semaphore.Release();
            }
        }

        /// <summary>
        /// количество элементов хранимых в буфере. Никогда не превышает Capacity
        /// </summary>
        public int Count
        {
            get
            {
                return Count_;
            }
        }

        /// <summary>
        /// добавляет новый элемент в конец списка
        /// </summary>
        /// <param name="value">добавляемый элемент</param>
        public void AddLast(T value)
        {
            while (LockCount > 0)
                Thread.Sleep(10);

            Semaphore.WaitOne();
            arr[(Ind + Count_) % arr.Length] = value;
            if (Count_ < arr.Length)
            {
                Count_++;
            }
            else
                Ind = (Ind + 1) % arr.Length;
            Semaphore.Release();
        }

        /// <summary>
        /// добавляет новый элемент в начало списка
        /// </summary>
        /// <param name="value">добавляемый элемент</param>
        public void AddFirst(T value)
        {
            while (LockCount > 0)
                Thread.Sleep(10);

            Semaphore.WaitOne();
            Ind = (arr.Length + Ind - 1) % arr.Length;
            arr[Ind] = value;
            if (Count_ < arr.Length)
            {
                Count_++;
            };
            Semaphore.Release();
        }

        /// <summary>
        /// удаляет первый элемент из списка
        /// </summary>
        public void RemoveFirst()
        {
            while (LockCount > 0)
                Thread.Sleep(10);

            Semaphore.WaitOne();
            if (Count_ > 0)
            {
                Ind = (Ind + 1) % arr.Length;
                Count_--;
            }
            Semaphore.Release();
        }

        /// <summary>
        /// удаляет последний элемент из списка
        /// </summary>
        public void RemoveLast()
        {
            while (LockCount > 0)
                Thread.Sleep(10);

            Semaphore.WaitOne();
            if (Count_ > 0)
                Count_--;
            Semaphore.Release();
        }

        /// <summary>
        /// очистка буфера
        /// </summary>
        public void Clear()
        {
            while (LockCount > 0)
                Thread.Sleep(10);

            Semaphore.WaitOne();
            Count_ = 0;
            Semaphore.Release();
        }

        /// <summary>
        /// семафор для синхронизации
        /// </summary>
        private Semaphore Semaphore;

        /// <summary>
        /// Если равно 0, то блокировки нет, иначе блокировка
        /// </summary>
        private uint LockCount = 0;

        /// <summary>
        /// блокирует изменение буфера. Процесс, который попытается изменить значение или довавить новый элемент будет заблокирован, пока не будет запущена процедура UnlockChange
        /// </summary>
        public void LockChanghe()
        {
            LockCount++;
        }

        /// <summary>
        /// снимает блокировку на изменение буфера.
        /// </summary>
        public void UnlockChange()
        {
            if (LockCount > 0)
                LockCount--;
        }

        ~FixedList()
        {
            LockCount = 0;
        }
    }
}
