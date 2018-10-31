using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NetManager;
using System.Net;
using System.Runtime.InteropServices;
using System.IO;
using System.Threading;
using System.Xml.Linq;
using System.Collections;
using EEG;
using Migalka;

namespace EEG_migalka
{
    /// <summary>
    /// Главная форма приложения
    /// </summary>
    public partial class FormMain : Form
    {
        /// <summary>
        /// Делегат для обработки ошибок работы устройства
        /// </summary>
        /// <param name="error">код ошибки</param>
        /// <param name="type_error">тип ошибки 0 - перезагрузка не требуется, 1 - требуется перезагрузка</param>
        private delegate void delegate_func_user_error(uint error, uint type_error);

        /// <summary>
        /// функция задания функции обратного вызова обработки ошибок
        /// </summary>
        /// <param name="func_user_error">функция вида void func_user_error(uint error, uint type_error)</param>
        [DllImport("EEG4DLL.DLL")]
        private static extern void SetErrorFunction(delegate_func_user_error func_user_error);

        /// <summary>
        /// функция возвращающая сообщение об ошибке в виде текста
        /// </summary>
        /// <param name="error">код ошибки</param>
        /// <returns></returns>
        [DllImport("EEG4DLL.DLL")]
        private static extern string return_text_error(uint error);

        /// <summary>
        /// Включение прибора
        /// </summary>
        /// <param name="FPort">номер порта (в программе - 6)</param>
        /// <returns>Если в функции возникли ошибки - результат нулевой, если нет, то возвращается структура:
        /// struct
        ///{
        ///Smallint address;       // 1 для USB
        ///int  dma;             // handle для USB
        ///Byte irq;             // количество приборов на USB с этой версией versia (работает всегда только первый прибор)
        ///int *address_BufferDMA; // заводской номер для USB прибора
        ///int *address_driver;    // тип прибора для версии 6(ЭЭГ4М) для USB  или 0 для других версий
        ///int version;           // версия прибора прочитанная из самого прибора
        ///}
        ///</returns>
        [DllImport("EEG4DLL.DLL")]
        private static extern IntPtr SwitchOn(int FPort);

        /// <summary>
        /// Отключение прибора
        /// </summary>
        [DllImport("EEG4DLL.DLL")]
        private static extern void SwitchOff();

        /// <summary>
        /// Задает частоту дискретизации
        /// </summary>
        /// <param name="frec_descreta">0 для частоты дискретизации 1000 Гц;
        ///1 для частоты дискретизации 5000 Гц;</param>
        [DllImport("EEG4DLL.DLL")]
        private static extern void SetFrecEEG(int frec_descreta);

        /// <summary>
        /// Задает параметры каналов усиления E1 , E2 , E3 , E4.
        ///Данную функцию необходимо вызвать до включения режимов передачи ЭЭГ или ВП.
        /// </summary>
        /// <param name="e1_hf">фильтр для каналов E1</param>
        /// <param name="e2_hf">фильтр для каналов E2</param>
        /// <param name="e3_hf">фильтр для каналов E3</param>
        /// <param name="e4_hf">фильтр для каналов E4</param>
        /// <param name="e1_k">коэффициент усиления канала E1</param>
        /// <param name="e2_k">коэффициент усиления канала E2</param>
        /// <param name="e3_k">коэффициент усиления канала E3</param>
        /// <param name="e4_k">коэффициент усиления канала E4</param>
        /// <param name="channels">указывает какие каналы используются</param>
        [DllImport("EEG4DLL.DLL")]
        private static extern void value_param_amplifier_eeg4m(uint e1_hf, uint e2_hf, uint e3_hf, uint e4_hf, uint e1_k, uint e2_k, uint e3_k, uint e4_k, [MarshalAs(UnmanagedType.LPArray)] byte[] channels);

        /// <summary>
        /// делегат для функции обработки данных полученных из ЭЭГ
        /// </summary>
        /// <param name="buffer">Ссылка на двумерный массив</param>
        /// <param name="length">длина массива или количество отсчетов с интервалом 1 мс (частота отсчетов 1 кГц)   
        /// или с интервалом 0.2 мс (частота отсчетов 5 кГц) в зависимости от функции SetFrecEEG (по умолчанию задано 1000Гц);</param>
        /// <param name="time_on_bus">счетчик шины USB, значение которого указывает на время отсчета первого элемента в массиве;</param>
        private delegate void delegate_type_func_user_eeg([MarshalAs(UnmanagedType.LPArray, SizeConst = 60000)] int[] buffer, int length, int time_on_bus);

        /// <summary>
        /// Включает режим передачи ЭЭГ
        /// </summary>
        /// <param name="user_func_eeg">функция обратного вызова void func_user_eeg(type_buffer_eeg buffer, long length, int time_on_bus);</param>
        [DllImport("EEG4DLL.DLL")]
        private static extern void SetTransmitEEG(delegate_type_func_user_eeg user_func_eeg);

        /// <summary>
        /// функция останавливает передачу данных с ЭЭГ
        /// </summary>
        [DllImport("EEG4DLL.DLL")]
        private static extern void OnStopReceive();

        /// <summary>
        /// Для обработки ошибок
        /// </summary>
        /// <param name="error">код ошибки</param>
        /// <param name="type_error">тип ошибки 0 - перезагрузка не требуется, 1 - требуется перезагрузка</param>
        private void func_user_error(uint error, uint type_error)
        {
            string Str = return_text_error(error & 0xFFFFFF);
            if (type_error == 1)
            {
                if (Started == 0)
                {
                    switch (error)
                    {
                        case 10:
                            Str += ".  Нажмите 'ОК' после того, как включите прибор, или отмена чтобы прервать запуск";
                            break;
                        case 20:
                            Str += ". Вытащите и вставьте снова USB разъем, после чего нажмите ОК";
                            break;
                    }

                    if ((error == 10) || (error == 20))
                    {
                        if (MessageBox.Show(Str, "Ошибка", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) == DialogResult.OK)
                        {
                            SwitchOff();
                            IntPtr res = SwitchOn(6);
                            if (res != IntPtr.Zero)
                                Started = 1;
                        }
                        else
                        {
                            SwitchOff();
                            Started = -1;
                        }
                    }
                    else
                        MessageBox.Show(Str, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    Str += ". Программа будет закрыта";
                    MessageBox.Show(Str, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                }
            }
            else
                Log.WriteLine(DateTime.Now.ToString() + ": " + Str);
        }

        /// <summary>
        /// Делегат, в котором хранится обработчик ошибок
        /// </summary>
        private delegate_func_user_error d_func_user_error;

        /// <summary>
        /// данные для отображения на графике
        /// </summary>
        private FixedList<short>[] DrawBuf;

        /// <summary>
        /// простейший делегат - без параметров и не возвращающий значение
        /// </summary>
        private delegate void SimpleDelegate();

        /// <summary>
        /// обновляет панель (для синхронизации потоков)
        /// </summary>
        private void Draw_Refresh()
        {
            if (panelDraw.InvokeRequired)
            {
                SimpleDelegate d = new SimpleDelegate(Draw_Refresh);
                panelDraw.Invoke(d);
            }
            else
                panelDraw.Refresh();
        }

        /// <summary>
        /// процесс перерисовки в потоке
        /// </summary>
        private void Draw_Proc()
        {
            while ((Started == 1) && (!chDrawDisable.Checked))
            {
                Draw_Refresh();
                Thread.Sleep(100);
            }
        }

        /// <summary>
        /// Список принятых но не обработанных данных
        /// </summary>
        private Queue[] Data;

        /// <summary>
        /// Предыдущий коэффициент (чтобы знать, было ли изменение)
        /// </summary>
        private byte oldCoef = 0;

        /// <summary>
        /// Функция обработки полученных данных от ЭЭГ
        /// </summary>
        /// <param name="buffer">массив данных от ЭЭГ</param>
        /// <param name="length">длина массива</param>
        /// <param name="time_on_bus">счетчик шины USB</param>
        void type_func_user_eeg([MarshalAs(UnmanagedType.LPArray, SizeConst = 60000)] int[] buffer, int length, int time_on_bus)
        {
            byte coef = Migalka.IsBlink;
            for (int I = 0; I < length; I++)
                for (int J = 0; J < Frame.CountChannels; J++)
                {
                    if (I == length / 2)
                        Data[J].Enqueue(new Point(buffer[2000 * J + I], (coef ^ oldCoef) & coef));
                    else
                        Data[J].Enqueue(new Point(buffer[2000 * J + I], 0));
                }
            oldCoef = coef;
        }

        /// <summary>
        /// Делегат, в котором хранится обработчик данных от ЭЭГ
        /// </summary>
        private delegate_type_func_user_eeg d_type_func_user_eeg;

        /// <summary>
        /// возвращает Id тех, кому нужно передать данные
        /// </summary>
        private int[] GetIdAddresses()
        {
            int[] res = new int[chDestinations.CheckedItems.Count];
            for (int I = 0; I < chDestinations.CheckedItems.Count; I++)
                res[I] = (chDestinations.CheckedItems[I] as ClientAddress).Id;
            return res;
        }

        /// <summary>
        /// Отправка данных по сети
        /// </summary>
        private void Send_Data(Frame Frame)
        {
            if (Client.Running)
                Client.SendData(GetIdAddresses(), Frame.GetBytes());
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public FormMain()
        {
            InitializeComponent();

            cbFrequensy.SelectedIndex = 1;

            dgChannelFilter.Columns[0].CellTemplate.Style.ForeColor = SystemColors.ControlText;
            dgChannelFilter.Columns[0].CellTemplate.Style.BackColor = SystemColors.ButtonHighlight;
            dgChannelFilter.Columns[0].CellTemplate.Style.SelectionForeColor = SystemColors.ControlText;
            dgChannelFilter.Columns[0].CellTemplate.Style.SelectionBackColor = SystemColors.ButtonHighlight;
            dgChannelFilter.Rows.Add("Е1");
            dgChannelFilter.Rows.Add("Е2");
            dgChannelFilter.Rows.Add("Е3");
            dgChannelFilter.Rows.Add("Е4");
            for (int I = 0; I < 4; I++)
            {
                dgChannelFilter.Rows[I].Cells[1].Value = (dgChannelFilter.Columns[1] as DataGridViewComboBoxColumn).Items[0];
                dgChannelFilter.Rows[I].Cells[2].Value = (dgChannelFilter.Columns[2] as DataGridViewComboBoxColumn).Items[0];
            }

            dgAmp.Columns[0].CellTemplate.Style.ForeColor = SystemColors.ControlText;
            dgAmp.Columns[0].CellTemplate.Style.BackColor = SystemColors.ButtonHighlight;
            dgAmp.Columns[0].CellTemplate.Style.SelectionForeColor = SystemColors.ControlText;
            dgAmp.Columns[0].CellTemplate.Style.SelectionBackColor = SystemColors.ButtonHighlight;
            dgAmp.Rows.Add("Е1");
            dgAmp.Rows.Add("Е2");
            dgAmp.Rows.Add("Е3");
            dgAmp.Rows.Add("Е4");
            for (int I = 0; I < 4; I++)
                dgAmp.Rows[I].Cells[1].Value = (dgAmp.Columns[1] as DataGridViewComboBoxColumn).Items[5];

            dgChannels.Columns[0].CellTemplate.Style.ForeColor = SystemColors.ControlText;
            dgChannels.Columns[0].CellTemplate.Style.BackColor = SystemColors.ButtonHighlight;
            dgChannels.Columns[0].CellTemplate.Style.SelectionForeColor = SystemColors.ControlText;
            dgChannels.Columns[0].CellTemplate.Style.SelectionBackColor = SystemColors.ButtonHighlight;
            dgChannels.Rows.Add("Fp1-A1");     //0
            dgChannels.Rows.Add("F3-A1");     //1
            dgChannels.Rows.Add("C3-A1");     //2
            dgChannels.Rows.Add("P3-A1");     //3
            dgChannels.Rows.Add("O1-A1");     //4
            dgChannels.Rows.Add("F7-A1");     //5
            dgChannels.Rows.Add("T3-A1");     //6
            dgChannels.Rows.Add("T5-A1");     //7
            dgChannels.Rows.Add("Fz-A1");     //8
            dgChannels.Rows.Add("PZ-A1");     //9
            dgChannels.Rows.Add("A1-A2");     //10
            dgChannels.Rows.Add("Fp2-A2");     //11
            dgChannels.Rows.Add("F4-A2");     //12
            dgChannels.Rows.Add("C4-A2");     //13
            dgChannels.Rows.Add("P4-A2");     //14
            dgChannels.Rows.Add("O2-A2");     //15
            dgChannels.Rows.Add("F8-A2");     //16
            dgChannels.Rows.Add("T4-A2");     //17
            dgChannels.Rows.Add("T6-A2");     //18
            dgChannels.Rows.Add("Fpz-A2");     //19
            dgChannels.Rows.Add("Cz-A2");     //20
            dgChannels.Rows.Add("Oz-A2");     //21
            dgChannels.Rows.Add("E1");     //22
            dgChannels.Rows.Add("E2");     //23
            dgChannels.Rows.Add("E3");     //24
            dgChannels.Rows.Add("E4");     //25
            dgChannels.Rows[0].Cells[1].Value = (dgChannels.Columns[1] as DataGridViewComboBoxColumn).Items[1];
            for (int I = 1; I < dgChannels.RowCount; I++)
                dgChannels.Rows[I].Cells[1].Value = (dgChannels.Columns[1] as DataGridViewComboBoxColumn).Items[0];

            (dgDrawChannels.Columns[0] as DataGridViewCheckBoxColumn).TrueValue = true;
            (dgDrawChannels.Columns[0] as DataGridViewCheckBoxColumn).FalseValue = false;

            dgDrawChannels.Rows.Add(true, "1. FP1-A1");
            dgDrawChannels.Rows[0].Cells[1].Style.BackColor = Color.FromArgb(255, 128, 128);
            dgDrawChannels.Rows[0].Cells[1].Style.SelectionBackColor = Color.FromArgb(255, 128, 128);
            dgDrawChannels.Rows.Add(true, "2. F3-A1");
            dgDrawChannels.Rows[1].Cells[1].Style.BackColor = Color.FromArgb(128, 255, 128);
            dgDrawChannels.Rows[1].Cells[1].Style.SelectionBackColor = Color.FromArgb(128, 255, 128);
            dgDrawChannels.Rows.Add(true, "3. C3-A1");
            dgDrawChannels.Rows[2].Cells[1].Style.BackColor = Color.FromArgb(128, 128, 255);
            dgDrawChannels.Rows[2].Cells[1].Style.SelectionBackColor = Color.FromArgb(128, 128, 255);
            dgDrawChannels.Rows.Add(true, "4. P3-A1");
            dgDrawChannels.Rows[3].Cells[1].Style.BackColor = Color.FromArgb(128, 128, 128);
            dgDrawChannels.Rows[3].Cells[1].Style.SelectionBackColor = Color.FromArgb(128, 128, 128);
            dgDrawChannels.Rows.Add(true, "5. O1-A1");
            dgDrawChannels.Rows[4].Cells[1].Style.BackColor = Color.FromArgb(255, 255, 128);
            dgDrawChannels.Rows[4].Cells[1].Style.SelectionBackColor = Color.FromArgb(255, 255, 128);
            dgDrawChannels.Rows.Add(true, "6. F7-A1");
            dgDrawChannels.Rows[5].Cells[1].Style.BackColor = Color.FromArgb(255, 128, 255);
            dgDrawChannels.Rows[5].Cells[1].Style.SelectionBackColor = Color.FromArgb(255, 128, 255);
            dgDrawChannels.Rows.Add(true, "7. T3-A1");
            dgDrawChannels.Rows[6].Cells[1].Style.BackColor = Color.FromArgb(128, 255, 255);
            dgDrawChannels.Rows[6].Cells[1].Style.SelectionBackColor = Color.FromArgb(128, 255, 255);
            dgDrawChannels.Rows.Add(true, "8. T5-A1");
            dgDrawChannels.Rows[7].Cells[1].Style.BackColor = Color.FromArgb(0, 255, 255);
            dgDrawChannels.Rows[7].Cells[1].Style.SelectionBackColor = Color.FromArgb(0, 255, 255);
            dgDrawChannels.Rows.Add(true, "9. FZ-A1");
            dgDrawChannels.Rows[8].Cells[1].Style.BackColor = Color.FromArgb(255, 0, 255);
            dgDrawChannels.Rows[8].Cells[1].Style.SelectionBackColor = Color.FromArgb(255, 0, 255);
            dgDrawChannels.Rows.Add(true, "10. PZ-A1");
            dgDrawChannels.Rows[9].Cells[1].Style.BackColor = Color.FromArgb(255, 255, 0);
            dgDrawChannels.Rows[9].Cells[1].Style.SelectionBackColor = Color.FromArgb(255, 255, 0);
            dgDrawChannels.Rows.Add(true, "11. A1-A2");
            dgDrawChannels.Rows[10].Cells[1].Style.BackColor = Color.FromArgb(255, 128, 0);
            dgDrawChannels.Rows[10].Cells[1].Style.SelectionBackColor = Color.FromArgb(255, 128, 0);
            dgDrawChannels.Rows.Add(true, "12. FP2-A2");
            dgDrawChannels.Rows[11].Cells[1].Style.BackColor = Color.FromArgb(255, 0, 128);
            dgDrawChannels.Rows[11].Cells[1].Style.SelectionBackColor = Color.FromArgb(255, 0, 128);
            dgDrawChannels.Rows.Add(true, "13. F4-A2");
            dgDrawChannels.Rows[12].Cells[1].Style.BackColor = Color.FromArgb(0, 255, 128);
            dgDrawChannels.Rows[12].Cells[1].Style.SelectionBackColor = Color.FromArgb(0, 255, 128);
            dgDrawChannels.Rows.Add(true, "14. C4-A2");
            dgDrawChannels.Rows[13].Cells[1].Style.BackColor = Color.FromArgb(0, 128, 255);
            dgDrawChannels.Rows[13].Cells[1].Style.SelectionBackColor = Color.FromArgb(0, 128, 255);
            dgDrawChannels.Rows.Add(true, "15. P4-A2");
            dgDrawChannels.Rows[14].Cells[1].Style.BackColor = Color.FromArgb(128, 0, 255);
            dgDrawChannels.Rows[14].Cells[1].Style.SelectionBackColor = Color.FromArgb(128, 0, 255);
            dgDrawChannels.Rows.Add(true, "16. O2-A2");
            dgDrawChannels.Rows[15].Cells[1].Style.BackColor = Color.FromArgb(128, 255, 0);
            dgDrawChannels.Rows[15].Cells[1].Style.SelectionBackColor = Color.FromArgb(128, 255, 0);
            dgDrawChannels.Rows.Add(true, "17. F8-A2");
            dgDrawChannels.Rows[16].Cells[1].Style.BackColor = Color.FromArgb(0, 255, 0);
            dgDrawChannels.Rows[16].Cells[1].Style.SelectionBackColor = Color.FromArgb(0, 255, 0);
            dgDrawChannels.Rows.Add(true, "18. T4-A2");
            dgDrawChannels.Rows[17].Cells[1].Style.BackColor = Color.FromArgb(255, 0, 0);
            dgDrawChannels.Rows[17].Cells[1].Style.SelectionBackColor = Color.FromArgb(255, 0, 0);
            dgDrawChannels.Rows.Add(true, "19. T6-A2");
            dgDrawChannels.Rows[18].Cells[1].Style.BackColor = Color.FromArgb(0, 0, 255);
            dgDrawChannels.Rows[18].Cells[1].Style.SelectionBackColor = Color.FromArgb(0, 0, 255);
            dgDrawChannels.Rows.Add(true, "20. FPZ-A2");
            dgDrawChannels.Rows[19].Cells[1].Style.BackColor = Color.FromArgb(0, 0, 128);
            dgDrawChannels.Rows[19].Cells[1].Style.SelectionBackColor = Color.FromArgb(0, 0, 128);
            dgDrawChannels.Rows.Add(true, "21. CZ-A2");
            dgDrawChannels.Rows[20].Cells[1].Style.BackColor = Color.FromArgb(0, 128, 0);
            dgDrawChannels.Rows[20].Cells[1].Style.SelectionBackColor = Color.FromArgb(0, 128, 0);
            dgDrawChannels.Rows.Add(true, "22. OZ-A2");
            dgDrawChannels.Rows[21].Cells[1].Style.BackColor = Color.FromArgb(128, 0, 0);
            dgDrawChannels.Rows[21].Cells[1].Style.SelectionBackColor = Color.FromArgb(128, 0, 0);
            dgDrawChannels.Rows.Add(true, "23. E1");
            dgDrawChannels.Rows[22].Cells[1].Style.BackColor = Color.FromArgb(128, 192, 0);
            dgDrawChannels.Rows[22].Cells[1].Style.SelectionBackColor = Color.FromArgb(128, 192, 0);
            dgDrawChannels.Rows.Add(true, "24. E2");
            dgDrawChannels.Rows[23].Cells[1].Style.BackColor = Color.FromArgb(128, 0, 192);
            dgDrawChannels.Rows[23].Cells[1].Style.SelectionBackColor = Color.FromArgb(128, 0, 192);
            dgDrawChannels.Rows.Add(true, "25. E3");
            dgDrawChannels.Rows[24].Cells[1].Style.BackColor = Color.FromArgb(192, 128, 0);
            dgDrawChannels.Rows[24].Cells[1].Style.SelectionBackColor = Color.FromArgb(192, 128, 0);
            dgDrawChannels.Rows.Add(true, "26. E3");
            dgDrawChannels.Rows[25].Cells[1].Style.BackColor = Color.FromArgb(192, 0, 128);
            dgDrawChannels.Rows[25].Cells[1].Style.SelectionBackColor = Color.FromArgb(192, 0, 128);
            dgDrawChannels.Rows.Add(true, "27. Не используется");
            dgDrawChannels.Rows[26].Cells[1].Style.BackColor = Color.FromArgb(0, 192, 128);
            dgDrawChannels.Rows[26].Cells[1].Style.SelectionBackColor = Color.FromArgb(0, 192, 128);
            dgDrawChannels.Rows.Add(true, "28. Дыхание");
            dgDrawChannels.Rows[27].Cells[1].Style.BackColor = Color.FromArgb(0, 128, 192);
            dgDrawChannels.Rows[27].Cells[1].Style.SelectionBackColor = Color.FromArgb(0, 128, 192);
            dgDrawChannels.Rows.Add(true, "29. Служебный канал");
            dgDrawChannels.Rows[28].Cells[1].Style.BackColor = Color.FromArgb(128, 128, 192);
            dgDrawChannels.Rows[28].Cells[1].Style.SelectionBackColor = Color.FromArgb(0, 128, 192);

            d_func_user_error = new delegate_func_user_error(func_user_error);
            SetErrorFunction(d_func_user_error);

            DrawBuf = new FixedList<short>[Frame.CountChannels];
            Data = new Queue[Frame.CountChannels];
            for (int I = 0; I < DrawBuf.Length; I++)
            {
                DrawBuf[I] = new FixedList<short>();
                DrawBuf[I].Capacity = (int)Math.Ceiling((panelDraw.ClientRectangle.Width - ShiftX - 10) / nScaleX.Value);
                Data[I] = Queue.Synchronized(new Queue());
            }

            dgFilterCC.RowCount = (int)numberFilterCC.Value;
            CC = new short[Frame.CountChannels][];
            CountCC = new int[Frame.CountChannels];
            IndexCC = new int[Frame.CountChannels];

            Client = new NMClient(this);
            Client.OnError += new EventHandler<EventMsgArgs>(Client_OnError);
            Client.OnStop += new EventHandler(Client_OnStop);
            Client.OnNewClient += new EventHandler<EventClientArgs>(Client_OnNewClient);
            Client.OnDeleteClient += new EventHandler<EventClientArgs>(Client_OnDeleteClient);
            Client.OnReseive += new EventHandler<EventClientMsgArgs>(Client_OnReseive);

            dgMigalka.Rows.Add(8);

            (dgMigalka.Columns[1] as DataGridViewComboBoxColumn).Items.Add("0");
            for (int I = 2; I < 500; I++)
                (dgMigalka.Columns[1] as DataGridViewComboBoxColumn).Items.Add((1000.0 / I).ToString());
            for (int I = 0; I < dgMigalka.Rows.Count; I++)
            {
                dgMigalka.Rows[I].Cells[0].Value = I + 1;
                dgMigalka.Rows[I].Cells[1].Value = "0";
            }

            Migalka = new Migalka.Migalka(serialPort1);

            LoadInit();
        }

        /// <summary>
        /// делегат для асинхроннрого вызова процесса обработки данных
        /// </summary>
        /// <param name="Frequency">Частота дискретизации</param>
        private delegate void d_Data_Processing(int Frequency);

        /// <summary>
        /// Обработка данных полученных с прибора и поставленных в очередь Data
        /// </summary>
        private void Data_Processing(int Frequency)
        {
            Frame Frame = new Frame();
            Frame.frequency = Frequency;
            int Data_Count = 0;

            if (chSave.Checked)
                SW = new StreamWriter(FileName);

            while (Started == 1)
            {
                if (Data_Count == Frame.LengthData)
                {
                    Frame.E_time = new SystemTime();
                    Send_Data(Frame);
                    Data_Count = 0;
                    Frame.B_time = new SystemTime();
                }

                short[] Datas = new short[Frame.CountChannels];

                if (chLoadFromFile.Checked)
                {
                    if (!SR.EndOfStream)
                    {
                        string Str;
                        string[] strs;
                        try
                        {
                            Str = SR.ReadLine();
                            strs = Str.Split(new char[] { ' ', (char)9, ';' });
                            for (int J = 0; (J < strs.Length) && (J < Frame.CountChannels); J++)
                                Datas[J] = short.Parse(strs[J]);
                        }
                        catch (Exception e)
                        {
                            if (chLoadFromFileDisconnect.Checked)
                                btnStart_Click(btnStart, new EventArgs());
                            MessageBox.Show("Ошибка чтения данных с файла: '" + e.Message + "'", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        if (chLoadFromFileDisconnect.Checked)
                            btnStart_Click(btnStart, new EventArgs());
                        MessageBox.Show("Достигнут конец файла", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }

                for (int J = 0; (J < Frame.CountChannels) && (Started == 1); J++)
                {
                    while ((Started == 1) && (Data[J].Count == 0))
                        Thread.Yield();
                    if (Started == 1)
                    {
                        Point p = (Point)Data[J].Dequeue();
                        short x =  (short)p.X;
                        if (J == 0) //одного достаточно
                            Frame.reserved[Data_Count] = (byte)p.Y;                        

                        if (chFilterCC.Checked)
                        {
                            CC[J][IndexCC[J]] = x;
                            IndexCC[J] = (IndexCC[J] + 1) % CC[J].Length;
                            CountCC[J] = Math.Min(CountCC[J] + 1, CC[J].Length);
                            if (CountCC[J] == CC[J].Length)
                            {
                                int s = 0;
                                if (chCoefCC.Enabled)
                                {
                                    int Ind = IndexCC[J];
                                    for (int I = 0; I < CountCC[J]; I++)
                                    {
                                        s += (short)(CC[J][Ind] * CoefCC[I]);
                                        Ind = (Ind + 1) % CC[J].Length;
                                    }
                                    x = (short)(s / CC[J].Length);
                                }
                                else
                                {
                                    for (int I = 0; I < CountCC[J]; I++)
                                        s += CC[J][I];
                                    x = (short)(s / CC[J].Length);
                                }
                            }
                        }

                        if (!chFilterCC.Enabled || (CountCC[J] == CC[J].Length))
                        {
                            if (!chDrawDisable.Checked)
                                DrawBuf[J].AddLast(x);

                            if (chLoadFromFile.Checked)
                            {
                                Frame.Data[Frame.LengthData * J + Data_Count] = Datas[J];
                            }
                            else
                                Frame.Data[Frame.LengthData * J + Data_Count] = x;
                            if (chSave.Checked)
                            {
                                if (J == 0)
                                {
                                    SW.Write(Frame.reserved[Data_Count]);
                                    SW.Write((char)9);
                                }

                                SW.Write(x);
                                if (J < Frame.CountChannels - 1)
                                    SW.Write((char)9);
                            }
                        }
                    }
                }
                Data_Count++;

                if (chSave.Checked)
                    SW.WriteLine();
            }

            if (chSave.Checked)
                SW.Close();
        }

        /// <summary>
        /// Обработка события получения данных по сети
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие</param>
        /// <param name="e">Параметры события</param>
        void Client_OnReseive(object sender, EventClientMsgArgs e)
        {
            int N = e.Msg[0] + e.Msg[1] * 0x100 + e.Msg[2] * 0x10000 + e.Msg[3] * 0x1000000;
            if ((Math.Abs(N) == 1) || (Math.Abs(N) == 2))
            {
                int I;
                if ((N == 1) || (N == -1))
                {
                    I = chDestinations.Items.Count - 1;
                    while ((I >= 0) && ((chDestinations.Items[I] as ClientAddress).Id != e.ClientId))
                        I--;
                }
                else
                    I = e.Msg[4] + e.Msg[5] * 0x100 + e.Msg[6] * 0x10000 + e.Msg[7] * 0x1000000;
                chDestinations.SetItemChecked(I, N > 0);
            }
        }

        /// <summary>
        /// Обработка события удаления клиента
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие</param>
        /// <param name="e">Параметры события</param>
        void Client_OnDeleteClient(object sender, EventClientArgs e)
        {
            ClientAddress Cl = new ClientAddress(e.ClientId, e.Name);
            int I = chDestinations.Items.Count - 1;
            while ((I >= 0) && (Cl.ToString() != chDestinations.Items[I].ToString()))
                I--;
            if (I >= 0)
                chDestinations.Items.RemoveAt(I);
        }

        /// <summary>
        /// Обработка события добавления клиента
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие</param>
        /// <param name="e">Параметры события</param>
        void Client_OnNewClient(object sender, EventClientArgs e)
        {
            chDestinations.Items.Add(new ClientAddress(e.ClientId, e.Name));
        }

        /// <summary>
        /// Обработка события остановки NMClient
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие</param>
        /// <param name="e">Параметры события</param>
        void Client_OnStop(object sender, EventArgs e)
        {
            tbIP.Enabled = true;
            tbName.Enabled = true;
            nPort.Enabled = true;
            chDestinations.Items.Clear();

            btnConnect.Text = "Подключить";
        }

        /// <summary>
        /// Обработка события ошибки NMClient
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие</param>
        /// <param name="e">Параметры события</param>
        void Client_OnError(object sender, EventMsgArgs e)
        {
            MessageBox.Show(e.Msg, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);

            if (!Client.Running)
            {
                tbIP.Enabled = true;
                tbName.Enabled = true;
                nPort.Enabled = true;
                chDestinations.Items.Clear();

                btnConnect.Text = "Подключить";
            }
        }

        /// <summary>
        /// Объект, с помощью которого идет сетевое подключение
        /// </summary>
        private NMClient Client;

        /// <summary>
        /// Обработчик нажатия кнопки подключения/отключения
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие</param>
        /// <param name="e">Параметры события</param>
        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (Client.Running)
            {
                Client.StopClient();
            }
            else
            {
                Client.IPServer = IPAddress.Parse(tbIP.Text);
                Client.Port = int.Parse(nPort.Text);
                Client.Name = tbName.Text;

                Client.RunClient();

                tbIP.Enabled = false;
                tbName.Enabled = false;
                nPort.Enabled = false;

                btnConnect.Text = "Отключить";
            }
        }

        /// <summary>
        /// Определяет, будет ли запущена программа
        /// </summary>
        private int Started = 0;

        /// <summary>
        /// Файл, куда сохраняется ЭЭГ
        /// </summary>
        private string FileName = "eeg.dat";

        /// <summary>
        /// Поток, записывающий данные в файл
        /// </summary>
        private StreamWriter SW;

        /// <summary>
        /// поток журнал работы программы
        /// </summary>
        private StreamWriter Log;

        /// <summary>
        /// окно для суммирования
        /// </summary>
        private short[][] CC;

        /// <summary>
        /// число элементов в окне суммирования (используется для заполнения окна)
        /// </summary>
        private int[] CountCC;

        /// <summary>
        /// индекс элемента в окне суммирования
        /// </summary>
        private int[] IndexCC;

        /// <summary>
        /// коэффициенты для окна суммирования
        /// </summary>
        private short[] CoefCC;

        /// <summary>
        /// Поток для эмулятора, откуда считываются данные 
        /// </summary>
        private StreamReader SR;

        private IBlink Migalka;

        /// <summary>
        /// Кнопка запуска процесса считывания с устройства
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие</param>
        /// <param name="e">Параметры события</param>
        private void btnStart_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                if (Started == 0)
                {
                    //проверки для эмулятора
                    if (chLoadFromFile.Checked)
                    {
                        if (!File.Exists(tbLoadFromFile.Text))
                        {
                            MessageBox.Show("Заданного файла не существует", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            tbLoadFromFile.Focus();
                            return;
                        }
                        else
                            SR = new StreamReader(tbLoadFromFile.Text);
                    }

                    Log = new StreamWriter("EEG.log");
                    IntPtr res = SwitchOn(6);
                    if ((Started != -1) || (res != IntPtr.Zero))
                    {
                        serialPort1.PortName = "COM" + nComPort.Value.ToString();
                        for (int I = 0; I < Migalka.Count; I++)
                            Migalka[I] = Convert.ToDouble(dgMigalka.Rows[I].Cells[1].Value);
                        Migalka.Start();

                        Started = 1;

                        uint[] hf = new uint[4];
                        uint[] k = new uint[4];
                        for (int I = 0; I < 4; I++)
                        {
                            hf[I] = (uint)((dgChannelFilter.Columns[1] as DataGridViewComboBoxColumn).Items.IndexOf(dgChannelFilter.Rows[I].Cells[1].Value) +
                                0x4 * (dgChannelFilter.Columns[2] as DataGridViewComboBoxColumn).Items.IndexOf(dgChannelFilter.Rows[I].Cells[2].Value));
                            k[I] = (uint)(dgAmp.Columns[1] as DataGridViewComboBoxColumn).Items.IndexOf(dgAmp.Rows[I].Cells[1].Value);
                        }

                        byte[] channels = new byte[26];
                        for (int I = 0; I < dgChannels.Rows.Count; I++)
                            channels[I] = (byte)(dgChannels.Columns[1] as DataGridViewComboBoxColumn).Items.IndexOf(dgChannels.Rows[I].Cells[1].Value);

                        value_param_amplifier_eeg4m(hf[0], hf[1], hf[2], hf[3], k[0], k[1], k[2], k[3], channels);
                        SetFrecEEG(cbFrequensy.SelectedIndex);

                        d_type_func_user_eeg = new delegate_type_func_user_eeg(type_func_user_eeg);
                        SetTransmitEEG(d_type_func_user_eeg);

                        if (chFilterCC.Checked)
                        {
                            for (int I = 0; I < CC.Length; I++)
                            {
                                CC[I] = new short[(int)numberFilterCC.Value];
                                CountCC[I] = 0;
                                IndexCC[I] = 0;
                            }
                            if (chCoefCC.Checked)
                            {
                                CoefCC = new short[(int)numberFilterCC.Value];
                                for (int I = 0; I < CoefCC.Length; I++)
                                    CoefCC[I] = Convert.ToInt16(dgFilterCC.Rows[I].Cells[1].Value);
                            }
                        }

                        if (!chDrawDisable.Checked)
                            (new SimpleDelegate(Draw_Proc)).BeginInvoke(null, null);

                        (new d_Data_Processing(Data_Processing)).BeginInvoke(cbFrequensy.SelectedIndex, null, null);

                        btnStart.Text = "Стоп";
                        dgAmp.Columns[1].ReadOnly = true;
                        dgChannelFilter.Columns[1].ReadOnly = true;
                        dgChannelFilter.Columns[2].ReadOnly = true;
                        dgChannels.Columns[1].ReadOnly = true;
                        chSave.Enabled = false;
                        chCoefCC.Enabled = false;
                        numberFilterCC.Enabled = false;
                        chFilterCC.Enabled = false;
                        btnCoefCC.Enabled = false;
                        dgFilterCC.Enabled = false;
                        tbLoadFromFile.Enabled = false;
                        btnLoadFromFile.Enabled = false;
                        chLoadFromFileDisconnect.Enabled = false;
                    }
                    else
                    {
                        Started = 0;
                        MessageBox.Show("Не удалось подключить прибор.", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {                    
                    OnStopReceive();
                    SwitchOff();
                    Migalka.Stop();

                    Log.Close();

                    if (chLoadFromFile.Checked)
                        SR.Close();

                    btnStart.Text = "Старт";
                    dgAmp.Columns[1].ReadOnly = false;
                    dgChannelFilter.Columns[1].ReadOnly = false;
                    dgChannelFilter.Columns[2].ReadOnly = false;
                    dgChannels.Columns[1].ReadOnly = false;
                    chSave.Enabled = true;
                    chCoefCC.Enabled = chFilterCC.Checked;
                    numberFilterCC.Enabled = chFilterCC.Checked;
                    chFilterCC.Enabled = true;
                    btnCoefCC.Enabled = chCoefCC.Checked;
                    dgFilterCC.Enabled = chCoefCC.Checked;
                    tbLoadFromFile.Enabled = chLoadFromFile.Checked;
                    btnLoadFromFile.Enabled = chLoadFromFile.Checked;
                    chLoadFromFileDisconnect.Enabled = chLoadFromFile.Checked;
                    Started = 0;

                    for (int I = 0; I < DrawBuf.Length; I++)
                        DrawBuf[I].Clear();

                    panelDraw.Refresh();
                }
            }
            finally
            {
                Cursor = Cursors.Default;
            }

        }

        /// <summary>
        /// Обработчик события нажатия кнопки задания цвета
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие</param>
        /// <param name="e">Параметры события</param>
        private void dgDrawChannels_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 1)
            {
                colorDialog1.Color = dgDrawChannels.Rows[e.RowIndex].Cells[1].Style.BackColor;
                if (colorDialog1.ShowDialog() == DialogResult.OK)
                {
                    dgDrawChannels.Rows[e.RowIndex].Cells[1].Style.BackColor = colorDialog1.Color;
                    dgDrawChannels.Rows[e.RowIndex].Cells[1].Style.SelectionBackColor = colorDialog1.Color;

                    panelDraw.Refresh();
                }
            };
        }

        /// <summary>
        /// смещение по X при отрисовке
        /// </summary>
        private int ShiftX = 130;

        /// <summary>
        /// Перерисовывает панель (рисуется график)
        /// </summary>
        /// <param name="sender">объект, вызвавший событие</param>
        /// <param name="e">параметры события</param>
        private void panelDraw_Paint(object sender, PaintEventArgs e)
        {
            ThreadPriority pr = Thread.CurrentThread.Priority;
            try
            {
                Thread.CurrentThread.Priority = ThreadPriority.Highest;
                int Count = 0;
                for (int I = 0; I < dgDrawChannels.RowCount; I++)
                    if ((bool)dgDrawChannels.Rows[I].Cells[0].Value)
                        Count++;
                if (Count > 0)
                {
                    float Height = (float)(panelDraw.ClientRectangle.Height - 20) / Count;
                    float Y = panelDraw.ClientRectangle.Top + 10;
                    float X = panelDraw.ClientRectangle.Left + ShiftX;
                    float ScaleX = (float)nScaleX.Value;
                    float ScaleY = (float)nScaleY.Value;
                    for (int I = 0; I < dgDrawChannels.RowCount; I++)
                        if ((bool)dgDrawChannels.Rows[I].Cells[0].Value)
                        {
                            string Str = (string)dgDrawChannels.Rows[I].Cells[1].Value;
                            SizeF F = e.Graphics.MeasureString(Str, panelDraw.Font);
                            e.Graphics.DrawLine(Pens.Black, X, Y + Height / 2, panelDraw.ClientRectangle.Right - 10, Y + Height / 2);
                            e.Graphics.DrawString(Str, panelDraw.Font, new SolidBrush(dgDrawChannels.Rows[I].Cells[1].Style.BackColor), X - 5 - F.Width, Y + (Height - F.Height) / 2);
                            Pen p = new Pen(dgDrawChannels.Rows[I].Cells[1].Style.BackColor);
                            DrawBuf[I].LockChanghe();
                            for (int J = 1; J < DrawBuf[I].Count; J++)
                                e.Graphics.DrawLine(p, X + (J - 1) * ScaleX, Y + Height / 2 - DrawBuf[I][J - 1] * ScaleY, ShiftX + J * ScaleX, Y + Height / 2 - DrawBuf[I][J] * ScaleY);
                            DrawBuf[I].UnlockChange();
                            Y += Height;
                        }
                }
            }
            finally
            {
                Thread.CurrentThread.Priority = pr;
            }
        }

        /// <summary>
        /// обработка события изменения масштаба по X
        /// </summary>
        /// <param name="sender">объект, вызвавший событие</param>
        /// <param name="e">параметры события</param>
        private void nScaleX_ValueChanged(object sender, EventArgs e)
        {
            for (int I = 0; I < DrawBuf.Length; I++)
                DrawBuf[I].Capacity = (int)Math.Ceiling((panelDraw.ClientRectangle.Width - ShiftX - 10) / nScaleX.Value);
        }

        /// <summary>
        /// обработка события изменения размера панели
        /// </summary>
        /// <param name="sender">объект, вызвавший событие</param>
        /// <param name="e">параметры события</param>
        private void panelDraw_SizeChanged(object sender, EventArgs e)
        {
            for (int I = 0; I < DrawBuf.Length; I++)
                DrawBuf[I].Capacity = (int)Math.Ceiling((panelDraw.ClientRectangle.Width - ShiftX - 10) / nScaleX.Value);

            panelDraw.Refresh();
        }

        /// <summary>
        /// обрабатываются процедуры необходимые при закрытии программы
        /// </summary>
        /// <param name="sender">объект, вызвавший событие</param>
        /// <param name="e">параметры события</param>
        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Started == 1)
            {
                OnStopReceive();
                SwitchOff();

                if (chSave.Checked)
                    SW.Close();
            }

            if (Client.Running)
                Client.StopClient();

            SaveInit();
        }

        /// <summary>
        /// обработка события изменения данных в ячейке таблицы dgDrawChannels
        /// </summary>
        /// <param name="sender">объект, вызвавший событие</param>
        /// <param name="e">параметры события</param>
        private void dgDrawChannels_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            panelDraw.Refresh();
        }

        /// <summary>
        /// обработка события значений в ячейке таблицы dgDrawChannels
        /// </summary>
        /// <param name="sender">объект, вызвавший событие</param>
        /// <param name="e">параметры события</param>
        private void dgDrawChannels_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            dgDrawChannels.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private string InitFileName = "Settings.xml";

        /// <summary>
        /// загрузка настроек из xml
        /// </summary>
        private void LoadInit()
        {
            try
            {
                XDocument XmlDoc = XDocument.Load(InitFileName);

                WindowState = (FormWindowState)Enum.Parse(typeof(FormWindowState), XmlDoc.Element("Settings").Element("EEG").Element("Window").Attribute("WindowState").Value);
                Left = int.Parse(XmlDoc.Element("Settings").Element("EEG").Element("Window").Attribute("Left").Value);
                Top = int.Parse(XmlDoc.Element("Settings").Element("EEG").Element("Window").Attribute("Top").Value);
                Width = int.Parse(XmlDoc.Element("Settings").Element("EEG").Element("Window").Attribute("Width").Value);
                Height = int.Parse(XmlDoc.Element("Settings").Element("EEG").Element("Window").Attribute("Height").Value);
                chDrawDisable.Checked = bool.Parse(XmlDoc.Element("Settings").Element("EEG").Element("Window").Attribute("DrawDisable").Value);

                nPort.Value = int.Parse(XmlDoc.Element("Settings").Element("EEG").Element("Network").Attribute("Port").Value);
                tbIP.Text = XmlDoc.Element("Settings").Element("EEG").Element("Network").Attribute("IP").Value;
                tbName.Text = XmlDoc.Element("Settings").Element("EEG").Element("Network").Attribute("Name").Value;

                for (int I = 0; I < dgChannelFilter.RowCount; I++)
                {
                    string Str = "E" + (I + 1).ToString();
                    dgChannelFilter.Rows[I].Cells[1].Value = (dgChannelFilter.Columns[1] as DataGridViewComboBoxColumn).Items[int.Parse(XmlDoc.Element("Settings").Element("EEG").Element(Str).Attribute("FHF").Value)];
                    dgChannelFilter.Rows[I].Cells[2].Value = (dgChannelFilter.Columns[2] as DataGridViewComboBoxColumn).Items[int.Parse(XmlDoc.Element("Settings").Element("EEG").Element(Str).Attribute("FLF").Value)];
                    dgAmp.Rows[I].Cells[1].Value = (dgAmp.Columns[1] as DataGridViewComboBoxColumn).Items[int.Parse(XmlDoc.Element("Settings").Element("EEG").Element(Str).Attribute("Amplification").Value)];
                }

                for (int I = 0; I < dgChannels.RowCount; I++)
                    dgChannels.Rows[I].Cells[1].Value = (dgChannels.Columns[1] as DataGridViewComboBoxColumn).Items[int.Parse(XmlDoc.Element("Settings").Element("EEG").Element("Channels").Attribute(dgChannels.Rows[I].Cells[0].Value.ToString()).Value)];

                chSave.Checked = bool.Parse(XmlDoc.Element("Settings").Element("EEG").Element("Save").Attribute("Checked").Value);

                nScaleX.Value = decimal.Parse(XmlDoc.Element("Settings").Element("EEG").Element("Scale").Attribute("X").Value);
                nScaleY.Value = decimal.Parse(XmlDoc.Element("Settings").Element("EEG").Element("Scale").Attribute("Y").Value);

                for (int I = 0; I < dgDrawChannels.RowCount; I++)
                {
                    dgDrawChannels.Rows[I].Cells[0].Value = bool.Parse(XmlDoc.Element("Settings").Element("EEG").Element("Visible").Element("Channel" + I.ToString()).Attribute("View").Value);
                    Color Color = Color.FromArgb(int.Parse(XmlDoc.Element("Settings").Element("EEG").Element("Visible").Element("Channel" + I.ToString()).Attribute("Color").Value));
                    dgDrawChannels.Rows[I].Cells[1].Style.BackColor = Color; 
                    dgDrawChannels.Rows[I].Cells[1].Style.SelectionBackColor = Color;
                }

                chFilterCC.Checked = bool.Parse(XmlDoc.Element("Settings").Element("EEG").Element("FilterCC").Attribute("Enabled").Value);
                numberFilterCC.Value = decimal.Parse(XmlDoc.Element("Settings").Element("EEG").Element("FilterCC").Attribute("Count").Value);
                chCoefCC.Checked = bool.Parse(XmlDoc.Element("Settings").Element("EEG").Element("FilterCC").Element("Coef").Attribute("Enabled").Value);
                for (int I = 0; I < dgFilterCC.RowCount; I++)
                    dgFilterCC.Rows[I].Cells[1].Value = XmlDoc.Element("Settings").Element("EEG").Element("FilterCC").Element("Coef").Element("Coef" + (I + 1).ToString()).Attribute("Value").Value;

                chLoadFromFile.Checked = bool.Parse(XmlDoc.Element("Settings").Element("EEG").Element("Emulator").Attribute("Enabled").Value);
                tbLoadFromFile.Text = XmlDoc.Element("Settings").Element("EEG").Element("Emulator").Attribute("FileName").Value;
                chLoadFromFileDisconnect.Checked = bool.Parse(XmlDoc.Element("Settings").Element("EEG").Element("Emulator").Attribute("Disconnect").Value);

                nComPort.Value = decimal.Parse(XmlDoc.Element("Settings").Element("Blink").Attribute("ComPort").Value);

                for (int I = 0; I < dgMigalka.Rows.Count; I++)
                    dgMigalka.Rows[I].Cells[1].Value = XmlDoc.Element("Settings").Element("Blink").Element("Blink" + I.ToString()).Attribute("Frequency").Value;
            }
            catch
            {
            }
        }

        /// <summary>
        /// сохранение настроек в xml
        /// </summary>
        private void SaveInit()
        {
            XDocument XmlDoc;
            try
            {
                XmlDoc = XDocument.Load(InitFileName);
            }
            catch
            {
                XmlDoc = new XDocument();
            }

            XElement Root = XmlDoc.Element("Settings");
            if (Root == null)
            {
                Root = new XElement("Settings");
                XmlDoc.Add(Root);
            }

            XElement Blink = Root.Element("Blink");
            if (Blink == null)
            {
                Blink = new XElement("Blink");
                Root.Add(Blink);
            }
            Blink.SetAttributeValue("ComPort", nComPort.Value);

            for (int I = 0; I < dgMigalka.Rows.Count; I++)
            {
                XElement El_blink = Blink.Element("Blink" + I.ToString());
                if (El_blink == null)
                {
                    El_blink = new XElement("Blink" + I.ToString());
                    Blink.Add(El_blink);
                }

                El_blink.SetAttributeValue("Frequency", dgMigalka.Rows[I].Cells[1].Value);
            }

            XElement EEG = Root.Element("EEG");
            if (EEG == null)
            {
                EEG = new XElement("EEG");
                Root.Add(EEG);
            }

            XElement El = EEG.Element("Window");
            if (El == null)
            {
                El = new XElement("Window");
                EEG.Add(El);
            }
            El.SetAttributeValue("WindowState", WindowState);
            El.SetAttributeValue("Left", Left);
            El.SetAttributeValue("Top", Top);
            El.SetAttributeValue("Width", Width);
            El.SetAttributeValue("Height", Height);
            El.SetAttributeValue("DrawDisable", chDrawDisable.Checked);

            El = EEG.Element("Network");
            if (El == null)
            {
                El = new XElement("Network");
                EEG.Add(El);
            }
            El.SetAttributeValue("Port", nPort.Value);
            El.SetAttributeValue("IP", tbIP.Text);
            El.SetAttributeValue("Name", tbName.Text);

            for (int I = 0; I < dgChannelFilter.RowCount; I++)
            {
                string Str = "E" + (I + 1).ToString();
                El = EEG.Element(Str);
                if (El == null)
                {
                    El = new XElement(Str);
                    EEG.Add(El);
                }
                El.SetAttributeValue("FHF", (dgChannelFilter.Columns[1] as DataGridViewComboBoxColumn).Items.IndexOf(dgChannelFilter.Rows[I].Cells[1].Value));
                El.SetAttributeValue("FLF", (dgChannelFilter.Columns[2] as DataGridViewComboBoxColumn).Items.IndexOf(dgChannelFilter.Rows[I].Cells[2].Value));
                El.SetAttributeValue("Amplification", (dgAmp.Columns[1] as DataGridViewComboBoxColumn).Items.IndexOf(dgAmp.Rows[I].Cells[1].Value));
            }

            El = EEG.Element("Channels");
            if (El == null)
            {
                El = new XElement("Channels");
                EEG.Add(El);
            }
            for (int I = 0; I < dgChannels.RowCount; I++)
                El.SetAttributeValue(dgChannels.Rows[I].Cells[0].Value.ToString(), (dgChannels.Columns[1] as DataGridViewComboBoxColumn).Items.IndexOf(dgChannels.Rows[I].Cells[1].Value));

            El = EEG.Element("Save");
            if (El == null)
            {
                El = new XElement("Save");
                EEG.Add(El);
            }
            El.SetAttributeValue("Checked", chSave.Checked);

            El = EEG.Element("Scale");
            if (El == null)
            {
                El = new XElement("Scale");
                EEG.Add(El);
            }
            El.SetAttributeValue("X", nScaleX.Value.ToString());
            El.SetAttributeValue("Y", nScaleY.Value.ToString());

            El = EEG.Element("Visible");
            if (El == null)
            {
                El = new XElement("Visible");
                EEG.Add(El);
            }
            Root = El;
            for (int I = 0; I < dgDrawChannels.RowCount; I++)
            {
                El = Root.Element("Channel" + I.ToString());
                if (El == null)
                {
                    El = new XElement("Channel" + I.ToString());
                    Root.Add(El);
                }
                El.SetAttributeValue("View", dgDrawChannels.Rows[I].Cells[0].Value);
                El.SetAttributeValue("Color", dgDrawChannels.Rows[I].Cells[1].Style.BackColor.ToArgb());
            }

            Root = EEG.Element("FilterCC");
            if (Root == null)
            {
                Root = new XElement("FilterCC");
                EEG.Add(Root);
            }
            Root.SetAttributeValue("Enabled", chFilterCC.Checked);
            Root.SetAttributeValue("Count", numberFilterCC.Value);

            El = Root.Element("Coef");
            if (El == null)
            {
                El = new XElement("Coef");
                Root.Add(El);
            }
            El.SetAttributeValue("Enabled", chCoefCC.Checked);
            for (int I = 0; I < dgFilterCC.RowCount; I++)
            {
                string S = "Coef" + (I + 1).ToString();
                Root = El.Element(S);
                if (Root == null)
                {
                    Root = new XElement(S);
                    El.Add(Root);
                }

                if (dgFilterCC.Rows[I].Cells[1].Value != null)
                    Root.SetAttributeValue("Value", dgFilterCC.Rows[I].Cells[1].Value);
                else
                    Root.SetAttributeValue("Value", 0);
            }

            El = EEG.Element("Emulator");
            if (El == null)
            {
                El = new XElement("Emulator");
                EEG.Add(El);
            }
            El.SetAttributeValue("Enabled", chLoadFromFile.Checked);
            El.SetAttributeValue("FileName", tbLoadFromFile.Text);
            El.SetAttributeValue("Disconnect", chLoadFromFileDisconnect.Checked);

            XmlDoc.Save(InitFileName);
        }

        /// <summary>
        /// включает или отключает фильтр свертки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chFilterCC_CheckedChanged(object sender, EventArgs e)
        {
            numberFilterCC.Enabled = chFilterCC.Checked;
            chCoefCC.Enabled = chFilterCC.Checked;
            btnCoefCC.Enabled = chFilterCC.Checked && chCoefCC.Checked;
            dgFilterCC.Enabled = chFilterCC.Checked && chCoefCC.Checked;
        }

        /// <summary>
        /// Задается длина окна свертки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void numberFilterCC_ValueChanged(object sender, EventArgs e)
        {
            dgFilterCC.RowCount = (int)numberFilterCC.Value;
        }

        /// <summary>
        /// Включает добавление коэффициентов
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chCoefCC_CheckedChanged(object sender, EventArgs e)
        {
            btnCoefCC.Enabled = chCoefCC.Checked;
            dgFilterCC.Enabled = chCoefCC.Checked;
        }

        /// <summary>
        /// Загрузка коэффициентов фильтра из файла
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCoefCC_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
                try
                {
                    StreamReader SR = new StreamReader(openFileDialog1.FileName);
                    for (int I = 0; I < numberFilterCC.Value; I++)
                        dgFilterCC.Rows[I].Cells[1].Value = Convert.ToInt16(SR.ReadLine());
                    SR.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
        }

        /// <summary>
        /// задается номер строки при добавлении строк в таблицу коэффициентов фильтра
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgFilterCC_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            for (int I = e.RowIndex; I < (e.RowIndex + e.RowCount); I++)
                dgFilterCC.Rows[I].Cells[0].Value = I + 1;
        }

        /// <summary>
        /// выбирается файл, из которого будут отправляться данные
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLoadFromFile_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                tbLoadFromFile.Text = openFileDialog1.FileName;
            }
        }

        /// <summary>
        /// Возникает когда включается или отключается загрузка из файла
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chLoadFromFile_CheckedChanged(object sender, EventArgs e)
        {
            tbLoadFromFile.Enabled = chLoadFromFile.Enabled;
            btnLoadFromFile.Enabled = chLoadFromFile.Enabled;
            chLoadFromFileDisconnect.Enabled = chLoadFromFile.Enabled;
        }

        /// <summary>
        /// отключается или включается отрисовка
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chDrawDisable_CheckedChanged(object sender, EventArgs e)
        {
            if (!chDrawDisable.Checked && (Started == 1))
                (new SimpleDelegate(Draw_Proc)).BeginInvoke(null, null);
            else
            {
                for (int I = 0; I < DrawBuf.Length; I++)
                    DrawBuf[I].Clear();
                Draw_Refresh();
            }
        }
    }
}
