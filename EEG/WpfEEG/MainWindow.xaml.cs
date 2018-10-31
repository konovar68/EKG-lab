using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Runtime.InteropServices;
using EEG;
using NetManager;
using System.Threading;
using System.Collections;
using System.Collections.ObjectModel;
using System.IO;
using System.ComponentModel;
using System.Net;
using System.Xml.Linq;

namespace WpfEEG
{
    /// <summary>
    /// Главная форма приложения
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        
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
        /// Поток для эмулятора, откуда считываются данные 
        /// </summary>
        private StreamReader SR;

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
                        if (MessageBox.Show(Str, "Ошибка", MessageBoxButton.OKCancel, MessageBoxImage.Error) == MessageBoxResult.OK)
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
                        MessageBox.Show(Str, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    Str += ". Программа будет закрыта";
                    MessageBox.Show(Str, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                }
            }
            else
                Log.WriteLine(DateTime.Now.ToString() + ": " + Str);
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

                WindowState = (WindowState)Enum.Parse(typeof(WindowState), XmlDoc.Element("Settings").Element("EEG").Element("Window").Attribute("WindowState").Value);
                Left = int.Parse(XmlDoc.Element("Settings").Element("EEG").Element("Window").Attribute("Left").Value);
                Top = int.Parse(XmlDoc.Element("Settings").Element("EEG").Element("Window").Attribute("Top").Value);
                Width = int.Parse(XmlDoc.Element("Settings").Element("EEG").Element("Window").Attribute("Width").Value);
                Height = int.Parse(XmlDoc.Element("Settings").Element("EEG").Element("Window").Attribute("Height").Value);
                chDrawDisable.IsChecked = bool.Parse(XmlDoc.Element("Settings").Element("EEG").Element("Window").Attribute("DrawDisable").Value);

                Port = int.Parse(XmlDoc.Element("Settings").Element("EEG").Element("Network").Attribute("Port").Value);
                IP = IPAddress.Parse(XmlDoc.Element("Settings").Element("EEG").Element("Network").Attribute("IP").Value);
                NetName = XmlDoc.Element("Settings").Element("EEG").Element("Network").Attribute("Name").Value;

                for (int I = 0; I < Filters.Count; I++)
                {
                    string Str = "E" + (I + 1).ToString();
                    Filters[I][1] = HighFrequency[int.Parse(XmlDoc.Element("Settings").Element("EEG").Element(Str).Attribute("FHF").Value)];
                    Filters[I][2] = LowFrequency[int.Parse(XmlDoc.Element("Settings").Element("EEG").Element(Str).Attribute("FLF").Value)];
                    Amplifier[I][1] = AmplifierValue[int.Parse(XmlDoc.Element("Settings").Element("EEG").Element(Str).Attribute("Amplification").Value)];
                }

                for (int I = 0; I < Channels.Count; I++)
                    Channels[I][1] = ChannelFunction[int.Parse(XmlDoc.Element("Settings").Element("EEG").Element("Channels").Attribute(Channels[I][0]).Value)];

                chSave.IsChecked = bool.Parse(XmlDoc.Element("Settings").Element("EEG").Element("Save").Attribute("Checked").Value);

                ScaleX = int.Parse(XmlDoc.Element("Settings").Element("EEG").Element("Scale").Attribute("CountX").Value);
                ScaleY = int.Parse(XmlDoc.Element("Settings").Element("EEG").Element("Scale").Attribute("CountY").Value);

                for (int I = 0; I < DrawChannels.Count; I++)
                {
                    DrawChannels[I].Visible = bool.Parse(XmlDoc.Element("Settings").Element("EEG").Element("Visible").Element("Channel" + I.ToString()).Attribute("View").Value);
                    int N = int.Parse(XmlDoc.Element("Settings").Element("EEG").Element("Visible").Element("Channel" + I.ToString()).Attribute("Color").Value);                   
                    DrawChannels[I].Color = Color.FromArgb((byte)((N >> 24) & 0xFF), (byte)((N >> 16) & 0xFF), (byte)((N >> 8) & 0xFF), (byte)(N & 0xFF));
                }
                
                chFilterCC.IsChecked = bool.Parse(XmlDoc.Element("Settings").Element("EEG").Element("FilterCC").Attribute("Enabled").Value);
                numberFilterCC.Value = double.Parse(XmlDoc.Element("Settings").Element("EEG").Element("FilterCC").Attribute("Count").Value);
                chCoefCC.IsChecked = bool.Parse(XmlDoc.Element("Settings").Element("EEG").Element("FilterCC").Element("Coef").Attribute("Enabled").Value);
                for (int I = 0; I < FilterCC.Count; I++)
                    FilterCC[I][0] = short.Parse(XmlDoc.Element("Settings").Element("EEG").Element("FilterCC").Element("Coef").Element("Coef" + (I + 1).ToString()).Attribute("Value").Value);

                chLoadFromFile.IsChecked = bool.Parse(XmlDoc.Element("Settings").Element("EEG").Element("Emulator").Attribute("Enabled").Value);
                tbLoadFromFile.Text = XmlDoc.Element("Settings").Element("EEG").Element("Emulator").Attribute("FileName").Value;
                chLoadFromFileDisconnect.IsChecked = bool.Parse(XmlDoc.Element("Settings").Element("EEG").Element("Emulator").Attribute("Disconnect").Value);
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
            El.SetAttributeValue("DrawDisable", chDrawDisable.IsChecked);

            El = EEG.Element("Network");
            if (El == null)
            {
                El = new XElement("Network");
                EEG.Add(El);
            }
            El.SetAttributeValue("Port", Port);
            El.SetAttributeValue("IP", IP);
            El.SetAttributeValue("Name", NetName);

            for (int I = 0; I < Filters.Count; I++)
            {
                string Str = "E" + (I + 1).ToString();
                El = EEG.Element(Str);
                if (El == null)
                {
                    El = new XElement(Str);
                    EEG.Add(El);
                }
                El.SetAttributeValue("FHF", HighFrequency.IndexOf(Filters[I][1]));
                El.SetAttributeValue("FLF", LowFrequency.IndexOf(Filters[I][2]));
                El.SetAttributeValue("Amplification", AmplifierValue.IndexOf(Amplifier[I][1]));
            }

            El = EEG.Element("Channels");
            if (El == null)
            {
                El = new XElement("Channels");
                EEG.Add(El);
            }
            for (int I = 0; I < Channels.Count; I++)
                El.SetAttributeValue(Channels[I][0], ChannelFunction.IndexOf(Channels[I][1]));

            El = EEG.Element("Save");
            if (El == null)
            {
                El = new XElement("Save");
                EEG.Add(El);
            }
            El.SetAttributeValue("Checked", chSave.IsChecked);

            El = EEG.Element("Scale");
            if (El == null)
            {
                El = new XElement("Scale");
                EEG.Add(El);
            }
            El.SetAttributeValue("CountX", ScaleX);
            El.SetAttributeValue("CountY", ScaleY);

            El = EEG.Element("Visible");
            if (El == null)
            {
                El = new XElement("Visible");
                EEG.Add(El);
            }
            Root = El;
            for (int I = 0; I < DrawChannels.Count; I++)
            {
                El = Root.Element("Channel" + I.ToString());
                if (El == null)
                {
                    El = new XElement("Channel" + I.ToString());
                    Root.Add(El);
                }
                El.SetAttributeValue("View", DrawChannels[I].Visible);
                Color Color = DrawChannels[I].Color;
                El.SetAttributeValue("Color", Color.A * 0x1000000 + Color.R * 0x10000 + Color.G * 0x100 + Color.B);
            }
            
            Root = EEG.Element("FilterCC");
            if (Root == null)
            {
                Root = new XElement("FilterCC");
                EEG.Add(Root);
            }
            Root.SetAttributeValue("Enabled", chFilterCC.IsChecked);
            Root.SetAttributeValue("Count", numberFilterCC.Value);

            El = Root.Element("Coef");
            if (El == null)
            {
                El = new XElement("Coef");
                Root.Add(El);
            }
            El.SetAttributeValue("Enabled", chCoefCC.IsChecked);
            for (int I = 0; I < FilterCC.Count; I++)
            {
                string S = "Coef" + (I + 1).ToString();
                Root = El.Element(S);
                if (Root == null)
                {
                    Root = new XElement(S);
                    El.Add(Root);
                }
                Root.SetAttributeValue("Value", FilterCC[I][0]);
            }

            El = EEG.Element("Emulator");
            if (El == null)
            {
                El = new XElement("Emulator");
                EEG.Add(El);
            }
            El.SetAttributeValue("Enabled", chLoadFromFile.IsChecked);
            El.SetAttributeValue("FileName", tbLoadFromFile.Text);
            El.SetAttributeValue("Disconnect", chLoadFromFileDisconnect.IsChecked);

            XmlDoc.Save(InitFileName);
        }

        /// <summary>
        /// Делегат, в котором хранится обработчик ошибок
        /// </summary>
        private delegate_func_user_error d_func_user_error;

        /// <summary>
        /// простейший делегат - без параметров и не возвращающий значение
        /// </summary>
        private delegate void SimpleDelegate();

        /// <summary>
        /// Список принятых но не обработанных данных
        /// </summary>
        private Queue[] Data;

        /// <summary>
        /// Функция обработки полученных данных от ЭЭГ
        /// </summary>
        /// <param name="buffer">массив данных от ЭЭГ</param>
        /// <param name="length">длина массива</param>
        /// <param name="time_on_bus">счетчик шины USB</param>
        void type_func_user_eeg([MarshalAs(UnmanagedType.LPArray, SizeConst = 60000)] int[] buffer, int length, int time_on_bus)
        {
            for (int I = 0; I < length; I++)
                for (int J = 0; J < EEG.Frame.CountChannels; J++)
                    Data[J].Enqueue(buffer[2000 * J + I]);
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
            List<int> res = new List<int>();
            foreach (CheckedListItem<ClientAddress> Item in Destinations)
            {
                if (Item.IsChecked)
                    res.Add((Item.Item as ClientAddress).Id);
            }
            return res.ToArray();
        }

        /// <summary>
        /// Отправка данных по сети
        /// </summary>
        private void Send_Data(EEG.Frame Frame)
        {
            if (Client.Running)
                Client.SendData(GetIdAddresses(), Frame.GetBytes());
        }
        
        /// <summary>
        /// Список потенциальных клиентов для отправки данных
        /// </summary>
        public ObservableCollection<CheckedListItem<ClientAddress>> Destinations { get; set; }

        /// <summary>
        /// Фильтры
        /// </summary>
        private List<string[]> Filters;

        /// <summary>
        /// Фильтры ВЧ
        /// </summary>
        private ReadOnlyCollection<string> HighFrequency;

        /// <summary>
        /// Фильтры НЧ
        /// </summary>
        private ReadOnlyCollection<string> LowFrequency;

        /// <summary>
        /// Усиление каналов
        /// </summary>
        private List<string[]> Amplifier;

        /// <summary>
        /// Параметры для усиления каналов
        /// </summary>
        private ReadOnlyCollection<string> AmplifierValue;

        /// <summary>
        /// Назначение каналов
        /// </summary>
        private List<string[]> Channels;

        /// <summary>
        /// Назначение каналов
        /// </summary>
        private ReadOnlyCollection<string> ChannelFunction;

        /// <summary>
        /// Фильтр
        /// </summary>
        private List<short[]> FilterCC;

        /// <summary>
        /// Данные для отрисовки каналов
        /// </summary>
        private ReadOnlyCollection<DrawChannel> DrawChannels;

        /// <summary>
        /// Конструктор
        /// </summary>
        public MainWindow()
        {
            Client = new NMClient(this);
            Client.Port = 9000;
            Client.IPServer = IPAddress.Parse("127.0.0.1");
            Client.Name = "ЭЭГ";
            Client.OnError += new EventHandler<EventMsgArgs>(Client_OnError);
            Client.OnStop += new EventHandler(Client_OnStop);
            Client.OnNewClient += new EventHandler<EventClientArgs>(Client_OnNewClient);
            Client.OnDeleteClient += new EventHandler<EventClientArgs>(Client_OnDeleteClient);
            Client.OnReseive += new EventHandler<EventClientMsgArgs>(Client_OnReseive);

            FilterCC = new List<short[]>();

            InitializeComponent();

            Destinations = new ObservableCollection<CheckedListItem<ClientAddress>>();

            DataContext = this; 
            
            cbFrequensy.SelectedIndex = 1;

            Filters = new List<string[]>(4);
            dgChannelFilter.ItemsSource = new ListCollectionView(Filters);
            HighFrequency = Array.AsReadOnly<string>(new string[] { "0.212 Гц", "0.5 Гц", "160 Гц" });
            (dgChannelFilter.Columns[1] as DataGridComboBoxColumn).ItemsSource = HighFrequency;
            LowFrequency = Array.AsReadOnly<string>(new string[] { "10000 Гц", "250 Гц" });
            (dgChannelFilter.Columns[2] as DataGridComboBoxColumn).ItemsSource = LowFrequency;
            for (int I = 0; I < 4; I++)
                Filters.Add(new string[] { "E" + (I + 1).ToString(), HighFrequency[0], LowFrequency[0] });

            Amplifier = new List<string[]>(4);
            dgAmp.ItemsSource = new ListCollectionView(Amplifier);
            AmplifierValue = Array.AsReadOnly<string>(new string[] { "200 мкВ", "500 мкВ", "1 мВ", "2 мВ", "5 мВ", "10 мВ", "20 мВ", "50 мВ", "100 мВ" });
            (dgAmp.Columns[1] as DataGridComboBoxColumn).ItemsSource = AmplifierValue;
            for (int I = 0; I < 4; I++)
                Amplifier.Add(new string[] { "E" + (I + 1).ToString(), AmplifierValue[5] });

            Channels = new List<string[]>(26);
            dgChannels.ItemsSource = new ListCollectionView(Channels);
            ChannelFunction = Array.AsReadOnly<string>(new string[] { "не используется", "в нормальном режиме", "в качестве опорного", "в режиме калибровка", "в режиме контроля шума" });
            (dgChannels.Columns[1] as DataGridComboBoxColumn).ItemsSource = ChannelFunction;
            Channels.Add(new string[] { "Fp1-A1", ChannelFunction[1] });     //0
            Channels.Add(new string[] { "F3-A1", ChannelFunction[1] });     //1
            Channels.Add(new string[] { "C3-A1", ChannelFunction[1] });     //2
            Channels.Add(new string[] { "P3-A1", ChannelFunction[1] });     //3
            Channels.Add(new string[] { "O1-A1", ChannelFunction[1] });     //4
            Channels.Add(new string[] { "F7-A1", ChannelFunction[1] });     //5
            Channels.Add(new string[] { "T3-A1", ChannelFunction[1] });     //6
            Channels.Add(new string[] { "T5-A1", ChannelFunction[1] });     //7
            Channels.Add(new string[] { "Fz-A1", ChannelFunction[1] });     //8
            Channels.Add(new string[] { "PZ-A1", ChannelFunction[1] });     //9
            Channels.Add(new string[] { "A1-A2", ChannelFunction[1] });     //10
            Channels.Add(new string[] { "Fp2-A2", ChannelFunction[1] });     //11
            Channels.Add(new string[] { "F4-A2", ChannelFunction[1] });     //12
            Channels.Add(new string[] { "C4-A2", ChannelFunction[1] });     //13
            Channels.Add(new string[] { "P4-A2", ChannelFunction[1] });     //14
            Channels.Add(new string[] { "O2-A2", ChannelFunction[1] });     //15
            Channels.Add(new string[] { "F8-A2", ChannelFunction[1] });     //16
            Channels.Add(new string[] { "T4-A2", ChannelFunction[1] });     //17
            Channels.Add(new string[] { "T6-A2", ChannelFunction[1] });     //18
            Channels.Add(new string[] { "Fpz-A2", ChannelFunction[1] });     //19
            Channels.Add(new string[] { "Cz-A2", ChannelFunction[1] });     //20
            Channels.Add(new string[] { "Oz-A2", ChannelFunction[1] });     //21
            Channels.Add(new string[] { "E1", ChannelFunction[1] });     //22
            Channels.Add(new string[] { "E2", ChannelFunction[1] });     //23
            Channels.Add(new string[] { "E3", ChannelFunction[1] });     //24
            Channels.Add(new string[] { "E4", ChannelFunction[1] });     //25

            DrawChannel[] DrawChannels = new DrawChannel[29];
            DrawChannels[0] = new DrawChannel() { Name = "1. FP1-A1", Color = Color.FromArgb(255, 255, 128, 128) };
            DrawChannels[1] = new DrawChannel() { Name = "2. F3-A1", Color = Color.FromArgb(255, 128, 255, 128) };
            DrawChannels[2] = new DrawChannel() { Name = "3. C3-A1", Color = Color.FromArgb(255, 128, 128, 255) };
            DrawChannels[3] = new DrawChannel() { Name = "4. P3-A1", Color = Color.FromArgb(255, 128, 128, 128) };
            DrawChannels[4] = new DrawChannel() { Name = "5. O1-A1", Color = Color.FromArgb(255, 255, 255, 128) };
            DrawChannels[5] = new DrawChannel() { Name = "6. F7-A1", Color = Color.FromArgb(255, 255, 128, 255) };
            DrawChannels[6] = new DrawChannel() { Name = "7. T3-A1", Color = Color.FromArgb(255, 128, 255, 255) };
            DrawChannels[7] = new DrawChannel() { Name = "8. T5-A1", Color = Color.FromArgb(255, 0, 255, 255) };
            DrawChannels[8] = new DrawChannel() { Name = "9. FZ-A1", Color = Color.FromArgb(255, 255, 0, 255) };
            DrawChannels[9] = new DrawChannel() { Name = "10. PZ-A1", Color = Color.FromArgb(255, 255, 255, 0) };
            DrawChannels[10] = new DrawChannel() { Name = "11. A1-A2", Color = Color.FromArgb(255, 255, 128, 0) };
            DrawChannels[11] = new DrawChannel() { Name = "12. FP2-A2", Color = Color.FromArgb(255, 255, 0, 128) };
            DrawChannels[12] = new DrawChannel() { Name = "13. F4-A2", Color = Color.FromArgb(255, 0, 255, 128) };
            DrawChannels[13] = new DrawChannel() { Name = "14. C4-A2", Color = Color.FromArgb(255, 0, 128, 255) };
            DrawChannels[14] = new DrawChannel() { Name = "15. P4-A2", Color = Color.FromArgb(255, 128, 0, 255) };
            DrawChannels[15] = new DrawChannel() { Name = "16. O2-A2", Color = Color.FromArgb(255, 128, 255, 0) };
            DrawChannels[16] = new DrawChannel() { Name = "17. F8-A2", Color = Color.FromArgb(255, 0, 255, 0) };
            DrawChannels[17] = new DrawChannel() { Name = "18. T4-A2", Color = Color.FromArgb(255, 255, 0, 0) };
            DrawChannels[18] = new DrawChannel() { Name = "19. T6-A2", Color = Color.FromArgb(255, 0, 0, 255) };
            DrawChannels[19] = new DrawChannel() { Name = "20. FPZ-A2", Color = Color.FromArgb(255, 0, 0, 128) };
            DrawChannels[20] = new DrawChannel() { Name = "21. CZ-A2", Color = Color.FromArgb(255, 0, 128, 0) };
            DrawChannels[21] = new DrawChannel() { Name = "22. OZ-A2", Color = Color.FromArgb(255, 128, 0, 0) };
            DrawChannels[22] = new DrawChannel() { Name = "23. E1", Color = Color.FromArgb(255, 128, 192, 0) };
            DrawChannels[23] = new DrawChannel() { Name = "24. E2", Color = Color.FromArgb(255, 128, 0, 192) };
            DrawChannels[24] = new DrawChannel() { Name = "25. E3", Color = Color.FromArgb(255, 192, 128, 0) };
            DrawChannels[25] = new DrawChannel() { Name = "26. E3", Color = Color.FromArgb(255, 192, 0, 128) };
            DrawChannels[26] = new DrawChannel() { Name = "27. Не используется", Color = Color.FromArgb(255, 0, 192, 128) };
            DrawChannels[27] = new DrawChannel() { Name = "28. Дыхание", Color = Color.FromArgb(255, 0, 128, 192) };
            DrawChannels[28] = new DrawChannel() { Name = "29. Служебный канал", Color = Color.FromArgb(255, 0, 128, 192) };
            double Height = drawPanel.Height / 29;
            for (int I = 0; I < DrawChannels.Length; I++)
            {
                drawGrid.Children.Add(DrawChannels[I].Label);
                Grid.SetColumn(DrawChannels[I].Label, 0);
                
                drawPanel.Children.Add(DrawChannels[I].Line);
                DrawChannels[I].Line.X1 = 0;
                DrawChannels[I].Line.Y1 = Height * (I + 0.5);
                DrawChannels[I].Line.X2 = drawPanel.Width;
                DrawChannels[I].Line.Y2 = Height * (I + 0.5);

                drawPanel.Children.Add(DrawChannels[I].Polyline);
                DrawChannels[I].ScaleX = ScaleX;
                DrawChannels[I].ScaleY = ScaleY;

                DrawChannels[I].Visible = true;
                DrawChannels[I].PropertyChanged += new PropertyChangedEventHandler(MainWindow_PropertyChanged);
            }
            this.DrawChannels =  Array.AsReadOnly<DrawChannel>(DrawChannels);
            dgDrawChannels.ItemsSource = new ListCollectionView(this.DrawChannels);

            Data = new Queue[EEG.Frame.CountChannels];
            for (int I = 0; I < Data.Length; I++)
                Data[I] = Queue.Synchronized(new Queue());

            CC = new short[EEG.Frame.CountChannels][];
            CountCC = new int[EEG.Frame.CountChannels];
            IndexCC = new int[EEG.Frame.CountChannels];

            LoadInit();
        }

        /// <summary>
        /// Обработка событий изменений свойств у DrawChannel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void MainWindow_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Visible")
                UpdateDrawChannels();
        }

        /// <summary>
        /// Выбор цвета графика
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgDrawChannels_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dgDrawChannels.CurrentCell.Column.DisplayIndex == 1)
            {
                WPFColorPickerLib.ColorDialog colorDialog = new WPFColorPickerLib.ColorDialog((dgDrawChannels.SelectedItem as DrawChannel).Color);
                colorDialog.Owner = this;
                if (colorDialog.ShowDialog() == true)
                    (dgDrawChannels.SelectedItem as DrawChannel).Color = colorDialog.SelectedColor;
            }; 
        }

        /// <summary>
        /// Подключение обработчика ошибок
        /// </summary>
        /// <param name="obj"></param>
        private void _SetErrorFunction(object obj)
        {
            d_func_user_error = new delegate_func_user_error(func_user_error);
            SetErrorFunction(d_func_user_error);
        }

        /// <summary>
        /// Нумерация строк в таблице задания фильтра
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgFilterCC_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1;
        }
        
        /// <summary>
        /// делегат для асинхроннрого вызова процесса обработки данных
        /// </summary>
        /// <param name="Frequency">Частота дискретизации</param>
        private delegate void d_Data_Processing(int Frequency);

        /// <summary>
        /// Определяет сохрять ли данные
        /// </summary>
        public bool IsSave { get; set; }

        /// <summary>
        /// Определяет выполнять ли загрузку данных из файла
        /// </summary>
        public bool IsLoadFromFile { get; set; }

        /// <summary>
        /// Определяет прерывать ли работу по окончании загрузки данных из файла
        /// </summary>
        public bool IsLoadFromFileDisconnect { get; set; }

        /// <summary>
        /// Определяет применять ли фильтрацию
        /// </summary>
        public bool IsFilterCC { get; set; }

        /// <summary>
        /// Определяет использовать ли коэффициенты в фильтре
        /// </summary>
        public bool IsCoefCC { get; set; }

        /// <summary>
        /// Определяет отключать ли отприсовку
        /// </summary>
        public bool IsDrawDisable { get; set; }

        /// <summary>
        /// Обработка данных полученных с прибора и поставленных в очередь Data
        /// </summary>
        private void Data_Processing(int Frequency)
        {
            EEG.Frame Frame = new EEG.Frame();
            Frame.frequency = Frequency;
            int Data_Count = 0;
            short x;

            if (IsSave)
                SW = new StreamWriter(FileName);

            while (Started == 1)
            {
                if (Data_Count == EEG.Frame.LengthData)
                {
                    Frame.E_time = new SystemTime();
                    Send_Data(Frame);
                    Data_Count = 0;
                    Frame.B_time = new SystemTime();
                }

                short[] Datas = new short[EEG.Frame.CountChannels];

                if (IsLoadFromFile)
                {
                    if (!SR.EndOfStream)
                    {
                        string Str;
                        string[] strs;
                        try
                        {
                            Str = SR.ReadLine();
                            strs = Str.Split(new char[] { ' ', (char)9, ';' });
                            for (int J = 0; (J < strs.Length) && (J < EEG.Frame.CountChannels); J++)
                                Datas[J] = short.Parse(strs[J]);
                        }
                        catch (Exception e)
                        {
                            if (IsLoadFromFileDisconnect)
                                btnStart_Click(btnStart, new RoutedEventArgs());
                            MessageBox.Show("Ошибка чтения данных с файла: '" + e.Message + "'", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    else
                    {
                        if (IsLoadFromFileDisconnect)
                            btnStart_Click(btnStart, new RoutedEventArgs());
                        MessageBox.Show("Достигнут конец файла", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }

                for (int J = 0; (J < EEG.Frame.CountChannels) && (Started == 1); J++)
                {
                    while ((Started == 1) && (Data[J].Count == 0))
                        Thread.Sleep(10);
                    if (Started == 1)
                    {
                        x = (short)(int)Data[J].Dequeue();

                        if (IsFilterCC)
                        {
                            CC[J][IndexCC[J]] = x;
                            IndexCC[J] = (IndexCC[J] + 1) % CC[J].Length;
                            CountCC[J] = Math.Min(CountCC[J] + 1, CC[J].Length);
                            if (CountCC[J] == CC[J].Length)
                            {
                                int s = 0;
                                if (IsCoefCC)
                                {
                                    int Ind = IndexCC[J];
                                    for (int I = 0; I < CountCC[J]; I++)
                                    {
                                        s += (short)(CC[J][Ind] * FilterCC[I][0]);
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

                        if ((!IsFilterCC) || (CountCC[J] == CC[J].Length))
                        {
                            if ((!IsDrawDisable) && DrawChannels[J].Visible)
                                Dispatcher.Invoke(new ThreadStart(delegate { DrawChannels[J].PutValue(x); }));

                            if (IsLoadFromFile)
                            {
                                Frame.Data[EEG.Frame.LengthData * J + Data_Count] = Datas[J];
                            }
                            else
                                Frame.Data[EEG.Frame.LengthData * J + Data_Count] = x;
                            if (IsSave)
                            {
                                SW.Write(x);
                                if (J < EEG.Frame.CountChannels - 1)
                                    SW.Write((char)9);
                            }
                        }
                    }
                }
                Data_Count++;

                if (IsSave)
                    SW.WriteLine();
            }

            if (IsSave)
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
                int I, M;
                if (Math.Abs(N) == 2)
                    M = e.Msg[4] + e.Msg[5] * 0x100 + e.Msg[6] * 0x10000 + e.Msg[7] * 0x1000000;
                else
                    M = e.ClientId;
                I = Destinations.Count - 1;
                while ((I >= 0) && (Destinations[I].Item.Id != M))
                    I--;
                if (I >= 0) 
                    Destinations[I].IsChecked = N > 0;
            }
        }

        /// <summary>
        /// Обработка события удаления клиента
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие</param>
        /// <param name="e">Параметры события</param>
        void Client_OnDeleteClient(object sender, EventClientArgs e)
        {
            Dispatcher.Invoke(new ThreadStart(delegate
                {
                    ClientAddress Cl = new ClientAddress(e.ClientId, e.Name);
                    int I = Destinations.Count - 1;
                    while ((I >= 0) && (Cl.ToString() != Destinations[I].Item.ToString()))
                        I--;
                    if (I >= 0)
                        Destinations.RemoveAt(I);
                }));
        }

        /// <summary>
        /// Обработка события добавления клиента
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие</param>
        /// <param name="e">Параметры события</param>
        void Client_OnNewClient(object sender, EventClientArgs e)
        {
            Dispatcher.Invoke(new ThreadStart(delegate
                {
                    Destinations.Add(new CheckedListItem<ClientAddress>(new ClientAddress(e.ClientId, e.Name)));
                }));
        }

        /// <summary>
        /// Обработка события остановки NMClient
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие</param>
        /// <param name="e">Параметры события</param>
        void Client_OnStop(object sender, EventArgs e)
        {
            Dispatcher.Invoke(new ThreadStart(delegate
                {
                    tbIP.IsEnabled = true;
                    tbName.IsEnabled = true;
                    tbPort.IsEnabled = true;
                    Destinations.Clear();

                    btnConnect.Content = "Подключить";
                }));
        }

        /// <summary>
        /// Обработка события ошибки NMClient
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие</param>
        /// <param name="e">Параметры события</param>
        void Client_OnError(object sender, EventMsgArgs e)
        {
            MessageBox.Show(e.Msg, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

            if (!Client.Running)
            {
                Dispatcher.Invoke(new ThreadStart(delegate
                    {
                        tbIP.IsEnabled = true;
                        tbName.IsEnabled = true;
                        tbPort.IsEnabled = true;
                        Destinations.Clear();

                        btnConnect.Content = "Подключить";
                    }));
            }
        }

        /// <summary>
        /// Объект, с помощью которого идет сетевое подключение
        /// </summary>
        private NMClient Client;

        /// <summary>
        /// Номер порта
        /// </summary>
        public int Port
        {
            get
            {
                return Client.Port;
            }
            set
            {
                Client.Port = value;
                
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Port"));
            }
        }

        /// <summary>
        /// IP адрес
        /// </summary>
        public IPAddress IP
        {
            get
            {
                return Client.IPServer;
            }
            set
            {
                Client.IPServer = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("IP"));
            }
        }

        /// <summary>
        /// Сетевое имя
        /// </summary>
        public string NetName
        {
            get
            {
                return Client.Name;
            }
            set
            {
                Client.Name = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("NetName"));
            }
        }
        
        /// <summary>
        /// Обработчик нажатия кнопки подключения/отключения
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConnect_Click(object sender, RoutedEventArgs e)
        {
            if (Client.Running)
            {
                Client.StopClient();
            }
            else
            { 
                Client.RunClient();

                tbIP.IsEnabled = false;
                tbName.IsEnabled = false;
                tbPort.IsEnabled = false;

                btnConnect.Content = "Отключить";
            }
        }

        /// <summary>
        /// Название загружаемого файла
        /// </summary>
        public string LoadFromFileName { get; set; }

        /// <summary>
        /// Индекс выбранной частоты
        /// </summary>
        public int FrequensyIndex { get; set; }

        /// <summary>
        /// Запуск процесса считывания данных
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            (new SynchronizationContext()).Post(new SendOrPostCallback(delegate
                {
                    Dispatcher.Invoke(new ThreadStart(delegate
                        {
                            Cursor = Cursors.Wait;
                            IsEnabled = false;
                        }));
                    try
                    {
                        if (Started == 0)
                        {
                            //проверки для эмулятора
                            if (IsLoadFromFile)
                            {
                                if (!File.Exists(LoadFromFileName))
                                {
                                    MessageBox.Show("Заданного файла не существует", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                                    tbLoadFromFile.Focus();
                                    return;
                                }
                                else
                                    SR = new StreamReader(LoadFromFileName);
                            }

                            Log = new StreamWriter("EEG.log");

                            d_func_user_error = new delegate_func_user_error(func_user_error);
                            SetErrorFunction(d_func_user_error);

                            IntPtr res = SwitchOn(6);
                            if ((Started != -1) || (res != IntPtr.Zero))
                            {
                                Started = 1;

                                uint[] hf = new uint[4];
                                uint[] k = new uint[4];
                                for (int I = 0; I < 4; I++)
                                {
                                    hf[I] = (uint)(HighFrequency.IndexOf(Filters[I][1]) + 0x4 * LowFrequency.IndexOf(Filters[I][2]));
                                    k[I] = (uint)AmplifierValue.IndexOf(Amplifier[I][1]);
                                }

                                byte[] channels = new byte[26];
                                for (int I = 0; I < channels.Length; I++)
                                    channels[I] = (byte)ChannelFunction.IndexOf(Channels[I][1]);

                                value_param_amplifier_eeg4m(hf[0], hf[1], hf[2], hf[3], k[0], k[1], k[2], k[3], channels);
                                SetFrecEEG(FrequensyIndex);

                                for (int I = 0; I < Data.Length; I++)
                                    Data[I].Clear();

                                d_type_func_user_eeg = new delegate_type_func_user_eeg(type_func_user_eeg);
                                SetTransmitEEG(d_type_func_user_eeg);

                                if (IsFilterCC)
                                {
                                    for (int I = 0; I < CC.Length; I++)
                                    {
                                        CC[I] = new short[(int)numberFilterCC.Value];
                                        CountCC[I] = 0;
                                        IndexCC[I] = 0;
                                    }
                                }

                                (new d_Data_Processing(Data_Processing)).BeginInvoke(FrequensyIndex, null, null);

                                Dispatcher.Invoke(new ThreadStart(delegate
                                    {
                                        btnStart.Content = "Стоп";
                                        dgAmp.Columns[1].IsReadOnly = true;
                                        dgChannelFilter.Columns[1].IsReadOnly = true;
                                        dgChannelFilter.Columns[2].IsReadOnly = true;
                                        dgChannels.Columns[1].IsReadOnly = true;
                                        chSave.IsEnabled = false;
                                        chCoefCC.IsEnabled = false;
                                        numberFilterCC.IsEnabled = false;
                                        chFilterCC.IsEnabled = false;
                                        btnCoefCC.IsEnabled = false;
                                        dgFilterCC.IsEnabled = false;
                                        tbLoadFromFile.IsEnabled = false;
                                        btnLoadFromFile.IsEnabled = false;
                                        chLoadFromFileDisconnect.IsEnabled = false;
                                    }));
                            }
                            else
                            {
                                Started = 0;
                                MessageBox.Show("Не удалось подключить прибор.", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Warning);
                            }
                        }
                        else
                        {
                            OnStopReceive();
                            SwitchOff();

                            Log.Close();

                            if (IsLoadFromFile)
                                SR.Close();

                            Dispatcher.Invoke(new ThreadStart(delegate
                                {
                                    btnStart.Content = "Старт";
                                    dgAmp.Columns[1].IsReadOnly = false;
                                    dgChannelFilter.Columns[1].IsReadOnly = false;
                                    dgChannelFilter.Columns[2].IsReadOnly = false;
                                    dgChannels.Columns[1].IsReadOnly = false;
                                    chSave.IsEnabled = true;
                                    chCoefCC.IsEnabled = chFilterCC.IsChecked == true;
                                    numberFilterCC.IsEnabled = chFilterCC.IsChecked == true;
                                    chFilterCC.IsEnabled = true;
                                    btnCoefCC.IsEnabled = chCoefCC.IsChecked == true;
                                    dgFilterCC.IsEnabled = chCoefCC.IsChecked == true;
                                    tbLoadFromFile.IsEnabled = chLoadFromFile.IsChecked == true;
                                    btnLoadFromFile.IsEnabled = chLoadFromFile.IsChecked == true;
                                    chLoadFromFileDisconnect.IsEnabled = chLoadFromFile.IsChecked == true;
                                    Started = 0;
                                }));
                        }
                    }
                    finally
                    {
                        Dispatcher.Invoke(new ThreadStart(delegate
                        {
                            Cursor = Cursors.Arrow;
                            IsEnabled = true;
                        }));
                    }
                }), null);
        }

        /// <summary>
        /// Выполняются необходимые действия перед закрытием
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WindowEEG_Closing(object sender, CancelEventArgs e)
        {
            if (Started == 1)
            {
                OnStopReceive();
                SwitchOff();

                if (chSave.IsChecked == true)
                    SW.Close();
            }

            if (Client.Running)
                Client.StopClient();

            SaveInit();
        }

        /// <summary>
        /// Включает и отключает фильтр
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chFilterCC_Checked(object sender, RoutedEventArgs e)
        {
            numberFilterCC.IsEnabled = chFilterCC.IsChecked == true;
            chCoefCC.IsEnabled = chFilterCC.IsChecked == true;
            btnCoefCC.IsEnabled = (chFilterCC.IsChecked == true) && (chCoefCC.IsChecked == true);
            dgFilterCC.IsEnabled = (chFilterCC.IsChecked == true) && (chCoefCC.IsChecked == true);
        }

        /// <summary>
        /// Меняет число элементов в фильтре
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void numberFilterCC_ValueChanged(object sender, RoutedEventArgs e)
        {
            if (dgFilterCC != null)
            {
                if (dgFilterCC.ItemsSource == null)
                    dgFilterCC.ItemsSource = new ListCollectionView(this.FilterCC);
                
                ListCollectionView FilterCC = (dgFilterCC.ItemsSource as ListCollectionView);
                
                while (FilterCC.Count < numberFilterCC.Value)
                    FilterCC.AddNewItem(new short[] { 1 });
                while (FilterCC.Count > numberFilterCC.Value)
                    FilterCC.RemoveAt(FilterCC.Count - 1);
                FilterCC.CommitNew();
            }
        }

        /// <summary>
        /// Включает или отключает использоавние коэффициентов для фильтра
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chCoefCC_Checked(object sender, RoutedEventArgs e)
        {
            btnCoefCC.IsEnabled = chCoefCC.IsChecked == true;
            dgFilterCC.IsEnabled = chCoefCC.IsChecked == true;
        }

        /// <summary>
        /// Загрузка коэффициентов фильтра из файла
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCoefCC_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog Dlg = new Microsoft.Win32.OpenFileDialog();
            if (Dlg.ShowDialog() == true)
                try
                {
                    StreamReader SR = new StreamReader(Dlg.FileName);
                    for (int I = 0; I < numberFilterCC.Value; I++)
                        FilterCC[I][0] = Convert.ToInt16(SR.ReadLine());
                    SR.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
        }

        /// <summary>
        /// Выбор файла для отправки данных
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLoadFromFile_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog Dlg = new Microsoft.Win32.OpenFileDialog();
            if (Dlg.ShowDialog() == true)
                tbLoadFromFile.Text = Dlg.FileName;
        }

        /// <summary>
        /// Включает и отклюает загрузку данных для отправки из файла
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chLoadFromFile_Checked(object sender, RoutedEventArgs e)
        {
            tbLoadFromFile.IsEnabled = chLoadFromFile.IsChecked == true;
            btnLoadFromFile.IsEnabled = chLoadFromFile.IsChecked == true;
            chLoadFromFileDisconnect.IsEnabled = chLoadFromFile.IsChecked == true;
        }

        /// <summary>
        /// Обновляются местоположения каналов
        /// </summary>
        private void UpdateDrawChannels()
        {
            int Count = 0;
            for (int I = 0; I < DrawChannels.Count; I++)
                if (DrawChannels[I].Visible)
                    Count++;
            if (Count > 0)
            {
                double Height = drawGrid.ActualHeight / Count;
                double HeightForLine = drawPanel.Height / Count;
                Count = 0;
                for (int I = 0; I < DrawChannels.Count; I++)
                    if (DrawChannels[I].Visible)
                    {
                        DrawChannels[I].Label.Margin = new Thickness(0, Height * Count, 0, drawGrid.ActualHeight - Height * (Count + 1));
                        DrawChannels[I].AxeY = HeightForLine * (Count + 0.5);
                        Count++;
                    }
            }
        }

        /// <summary>
        /// Обрабатывается изменение размеров панели прорисовки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void drawGrid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateDrawChannels();
        }

        private double _ScaleX = 1;

        /// <summary>
        /// Меняется масштаб по горизонтали
        /// </summary>
        public double ScaleX
        {
            get
            {
                return _ScaleX;
            }
            set
            {
                _ScaleX = value;
                if (DrawChannels != null)
                    for (int I = 0; I < DrawChannels.Count; I++)
                        DrawChannels[I].ScaleX = ScaleX;
            }
        }

        private double _ScaleY = 1;

        /// <summary>
        /// Меняется масштаб по вертикали
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public double ScaleY
        {
            get
            {
                return _ScaleY;
            }
            set
            {
                _ScaleY = value;
                if (DrawChannels != null)
                    for (int I = 0; I < DrawChannels.Count; I++)
                        DrawChannels[I].ScaleY = ScaleY;
            }
        }
    }
}
