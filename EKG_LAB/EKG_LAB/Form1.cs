using System;
using System.Windows.Forms;
using System.Net;
using NetManager;
using EEG;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading.Tasks;

using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.UI;

namespace Приемник
{
    public partial class Form1 : Form
    {
        #region Переменные и константы

        #region Переменные сервера
        private NMClient NMClient;

        private delegate void delegate_SetEnabled(Control Control, bool Enabled);

        private object m_lockObj = new object();
        #endregion

        #region Переменные и константы сигнала    
        int siteCount = 0;
        const int SizeEllipse = 5;

        int N;

        short[] t;
        int[][] g ;

        const int decim = 3;// децимация сигнала
        int dec = 0;
        const int dec_max = 4;// частота обработки полученных сигналов
        const int length_ = (24 / decim)+1;
        #endregion

        #region Переменные Вороного и его настройки
        Image<Bgr, Byte> Kadr;
        Color[] col;
        Color[] col2;

        Capture capweb = null;
        Point[] voron;

        Point frame_first_min = new Point(0, 0), frame_first_max = new Point(0, 0);
        Point min = new Point(0, 0), max = new Point(0, 0);            

        List<PointF> sites;     

        bool marker_user = false, marker_ends = false;

        bool Pause = false;
        bool tr = false;
        #endregion

        #endregion

        #region Форма

        #region Загрузка формы и кнопки
        public Form1()
        {
            InitializeComponent();

            NMClient = new NMClient(this);
            NMClient.OnDeleteClient += new EventHandler<EventClientArgs>(NMClient_OnDeleteClient);
            NMClient.OnError += new EventHandler<EventMsgArgs>(NMClient_OnError);
            NMClient.OnNewClient += new EventHandler<EventClientArgs>(NMClient_OnNewClient);
            NMClient.OnReseive += new EventHandler<EventClientMsgArgs>(NMClient_OnReseive);
            NMClient.OnStop += new EventHandler(NMClient_OnStop);                 
                      
        }    

        private void Form1_Load(object sender, EventArgs e)
        {
            Video();            
            sites = new List<PointF>();
        }       
       
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (NMClient.Running)
                NMClient.StopClient();
            if (capweb != null)
                capweb.Dispose();
        }
        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (!NMClient.Running)
            {
                NMClient.IPServer = IPAddress.Parse(tbIP.Text);
                NMClient.Port = Int32.Parse(tbPort.Text);
                NMClient.Name = tbName.Text;
                NMClient.RunClient();
                btnConnect.Text = "Закрыть";
            }
            else
                NMClient.StopClient();
        }
        private void pause_Click(object sender, EventArgs e)
        {
            if (!Pause)
            {
                Application.Idle -= GetVideo;
                Pause = true;
            }
            else
            {
                Pause = false;
                Application.Idle += GetVideo;
            }
        }
        #endregion

        #region CheckBox
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                checkBox1.Checked = false;                            
            }
            marker_user = checkBox1.Checked;
            marker_ends = checkBox2.Checked;
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                checkBox2.Checked = false;                              
            }
            marker_user = checkBox1.Checked;
            marker_ends = checkBox2.Checked;
        }
        #endregion

        #region Мышь
        private void ib1_MouseDown(object sender, MouseEventArgs e)
        {
            if (marker_ends)
            {
                tr = true;
                frame_first_min.X = e.X;
                frame_first_min.Y = e.Y;                
            }
            else
            {
                if (marker_user)
                {                                         
                    if (comparison(new PointF(e.X, e.Y), min)&&
                        comparison(sum(max,min),new PointF(e.X, e.Y)))
                    {
                        PointF tmp = set_coef(new Point(e.X, e.Y),min,max);
                        bool us = true;
                        for (int i = 0; i < SizeEllipse; i++)
                            for (int j = 0; j < SizeEllipse; j++)
                                if (!(sites.IndexOf(new PointF(tmp.X - i, tmp.Y - j)) == -1))
                                {
                                    us = false;
                                    sites.Remove(new PointF(tmp.X - i, tmp.Y - j));
                                    i = SizeEllipse;
                                    j = SizeEllipse;
                                }
                        if (us) sites.Add(tmp);

                        Init( sites.Count);
                    }
                }
            }
            refresh();
        }
        private void ib1_MouseUp(object sender, MouseEventArgs e)
        {
            if (marker_ends)
            {
                if (comparison(frame_first_max, frame_first_min))
                {
                    
                    frame_first_max.X = e.X - frame_first_min.X;
                    frame_first_max.Y = e.Y - frame_first_min.Y;
                }
                tr = false;
            }            
        }
        private void ib1_MouseMove(object sender, MouseEventArgs e)
        {
            if(!Pause)
            if (marker_ends)
            {
                if (tr)
                {
                    if (comparison(new PointF(e.X,e.Y), frame_first_min))
                    {
                       frame_first_max.X = e.X - frame_first_min.X;
                       frame_first_max.Y = e.Y - frame_first_min.Y;
                    }
                }
            }            
        }
        #endregion

        #endregion

        #region Сервер
        void NMClient_OnStop(object sender, EventArgs e)
        {
            btnConnect.Text = "Connect";
            chClients.Enabled = true;
            chClients.Items.Clear();
        }
        void NMClient_OnReseive(object sender, EventClientMsgArgs e)
        {
            {
                N = BitConverter.ToInt32(e.Msg, 0);
                t = new Frame(e.Msg).Data;
                if (N == 6)
                {
                    for (int I = 0; I < siteCount; I++)
                    {
                        for (int j = 0; j < 24; j += decim)
                        {
                            g[I][(j / decim) +length_ * dec] = t[I * 24 + j];
                        }
                    }

                        if (dec == dec_max-1)
                        {
                        for (int I = 0; I < siteCount; I++)
                        {
                            g[I] = Smooth(g[I], 2);

                            int maximum = g[I][0];

                            for (int j = 1; j < g[I].Length; j++)
                            {
                                if(maximum< g[I][j])
                                    maximum = g[I][j];
                            }
                            voron[I].X = maximum;
                            voron[I].Y = I;
                        }
                        quicksort(voron, 0, voron.Length - 1);

                        for (int II = 0; II < siteCount; II++)
                        {
                            col2[II] = col[voron[II].Y];
                        }
                        dec = 0;
                        }
                        else dec++;
                    
                    
                }
            }
        }
        void NMClient_OnNewClient(object sender, EventClientArgs e)
        {
            chClients.Items.Add(new ClientAddress(e.ClientId, e.Name));
        }
        void NMClient_OnError(object sender, EventMsgArgs e)
        {
            MessageBox.Show(e.Msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        void NMClient_OnDeleteClient(object sender, EventClientArgs e)
        {
            ClientAddress Cl = new ClientAddress(e.ClientId, e.Name);
            int I = chClients.Items.Count - 1;
            while ((I >= 0) && (Cl.ToString() != chClients.Items[I].ToString()))
                I--;
            if (I >= 0)
                chClients.Items.RemoveAt(I);
        }
        private int Port
        {
            get
            {
                return Convert.ToInt32(tbPort.Text);
            }
            set
            {
                tbPort.Text = value.ToString();
            }
        }
        private void SetEnabled(Control Control, bool Enabled)
        {
            if (Control.InvokeRequired)
            {
                delegate_SetEnabled E = new delegate_SetEnabled(SetEnabled);
                Control.Invoke(E, new object[] { Control, Enabled });
            }
            else
                Control.Enabled = Enabled;
        }
        #endregion

        #region Видео и обработка Вороного
        void Video()
        {
            try
            {
                capweb = new Capture();
            }
            catch (NullReferenceException e)
            {
                MessageBox.Show("Камера не найдена");
            }
            Application.Idle += GetVideo;
        }
        private void GetVideo(object sender, EventArgs e)
        {

            Kadr = capweb.QueryFrame().Resize(300, 300, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
            Graphics g = Graphics.FromImage(Kadr.Bitmap);
                Image<Gray, Byte> gray = Kadr.Convert<Gray, Byte>().PyrDown().PyrUp();
                Gray cannyThreshold = new Gray(180);
                Gray cannyThresholdLinking = new Gray(120);
                Gray circleAccumulatorThreshold = new Gray(120);
                Image<Gray, Byte> cannyEdges = gray.Canny(cannyThreshold, cannyThresholdLinking);
                LineSegment2D[] lines = cannyEdges.HoughLinesBinary(
                    1,
                    Math.PI / 45.0,
                    20,
                    30,
                    10
                    )[0];

                List<MCvBox2D> boxList = new List<MCvBox2D>();

                using (MemStorage storage = new MemStorage())
                    for (Contour<Point> contours = cannyEdges.FindContours(); contours != null; contours = contours.HNext)
                    {
                        Contour<Point> currentContour = contours.ApproxPoly(contours.Perimeter * 0.05, storage);

                        if (contours.Area > 100)
                        {
                            if (currentContour.Total == 4)
                            {
                                bool isRectangle = true;
                                Point[] pts = currentContour.ToArray();
                                LineSegment2D[] edges = PointCollection.PolyLine(pts, true);

                                for (int i = 0; i < edges.Length; i++)
                                {
                                    double angle = Math.Abs(
                                       edges[(i + 1) % edges.Length].GetExteriorAngleDegree(edges[i]));
                                    if (angle < 0 || angle > 180)
                                    {
                                        isRectangle = false;
                                        break;
                                    }
                                }
                                MCvBox2D b = currentContour.GetMinAreaRect();
                                if (b.center.X > frame_first_min.X && b.center.X < frame_first_max.X && b.center.Y > frame_first_min.Y && b.center.Y < frame_first_max.Y)
                                    if (isRectangle) boxList.Add(b);
                            }
                        }
                    }

                if (boxList.Count > 2)
                {
                    min.X = Convert.ToInt32(boxList[0].center.X);
                    max.X = min.X;
                    min.Y = Convert.ToInt32(boxList[0].center.Y);
                    max.Y = min.Y;
                    for (int i = 0; i < boxList.Count; i++)
                    {
                        int a1 = Convert.ToInt32(boxList[i].center.X), a2 = Convert.ToInt32(boxList[i].center.Y);

                        if (min.X > a1) min.X = a1;
                        else if (max.X < a1) max.X = a1;

                        if (min.Y > a2) min.Y = a2;
                        else if (max.Y < a2) max.Y = a2;
                    }

                }        
                if(siteCount>0)
            Voronoi(min, max, Kadr);
            draw();

            ib1.Image = Kadr;

        }
        
        void Voronoi(PointF a1, PointF a2, Image<Bgr, Byte> bit)
        {
            Bitmap bitmap = bit.Bitmap;

            int[] sy = new int[Convert.ToInt32(a2.Y - a1.Y)];
            int[] sx = new int[Convert.ToInt32(a2.X - a1.X)];
            int n = 0;
            double dist2 = 0;

            int[,] col3 = new int[Convert.ToInt32(a2.X - a1.X), Convert.ToInt32(a2.Y - a1.Y)];

            for (int x = (int)a1.X; x < a2.X; x++)
            {
                for (int y = (int)a1.Y; y < a2.Y; y++)
                {
                    n = 0;
                    dist2 = Distance(get_coef(sites[0],min,max), new PointF(x, y));
                    for (int i = 1; i < siteCount; i++)
                    {
                        if (Distance(get_coef(sites[i], min, max), new PointF(x, y)) < dist2)
                        {
                            n = i;
                            dist2 = Distance(get_coef(sites[n], min, max), new PointF(x, y));
                        }
                    }
                    col3[(int)(x - a1.X), (int)(y - a1.Y)] = (int)col2[n].R;
                }
            }

            for (int x = 0; x < a2.X - a1.X; x++)
            {
                for (int y = 0; y < a2.Y - a1.Y; y++)
                    sy[y] = col3[x, y];
                sy = Smooth(sy, 30);
                for (int y = Convert.ToInt32(a1.Y); y < a2.Y; y++)
                    bitmap.SetPixel(Convert.ToInt32(x + a1.X), y, Color.FromArgb(sy[Convert.ToInt32(y - a1.Y)], 0, 0));
            }
            for (int y = 0; y < a2.Y - a1.Y; y++)
            {
                for (int x = 0; x < a2.X - a1.X; x++)
                    sx[x] = col3[x, y];
                sx = Smooth(sx, 30);
                for (int x = Convert.ToInt32(a1.X); x < a2.X; x++)
                    bitmap.SetPixel(x, Convert.ToInt32(y + a1.Y), Color.FromArgb(sx[Convert.ToInt32(x - a1.X)], 0, 0));
            }

        }

        /// <summary>
        /// Обновляет изображение
        /// </summary>
        void refresh()
        {
            Graphics g = Graphics.FromImage(Kadr.Bitmap);
            g.DrawRectangle(new Pen(Brushes.Red),
                frame_first_min.X,
                frame_first_min.Y,
                frame_first_max.X,
                frame_first_max.Y);
            
            foreach (PointF tr in sites)
            {
                PointF a = get_coef(tr, min, max);
                g.FillEllipse(Brushes.WhiteSmoke, a.X, a.Y, SizeEllipse, SizeEllipse);
            }
            ib1.Image = Kadr;
        }

        /// <summary>
        /// Рисует границы, границы вороного и сайты
        /// </summary>
        /// <param name="g">Graphics на котором рисуется</param>
        void draw()
        {
            Graphics g = Graphics.FromImage(Kadr.Bitmap);
            g.DrawRectangle(new Pen(Brushes.Red),
                    frame_first_min.X,
                    frame_first_min.Y,
                    frame_first_max.X,
                    frame_first_max.Y);
            if (siteCount < 1)
                g.FillRectangle(Brushes.Black,min.X,min.Y,max.X-min.X,max.Y-min.Y);

            foreach (PointF tr in sites)
            {
                PointF a = get_coef(tr, min, max);
                g.FillEllipse(Brushes.WhiteSmoke, a.X, a.Y, SizeEllipse, SizeEllipse);
            }
            ib1.Image = Kadr;
        }

        #endregion

        #region Быстрая сортировка
        int partition(Point[] array, int start, int end)
        {
            Point temp;//swap helper
            int marker = start;//divides left and right subarrays
            for (int i = start; i <= end; i++)
            {
                if (array[i].X < array[end].X) //array[end] is pivot
                {
                    temp = array[marker]; // swap
                    array[marker] = array[i];
                    array[i] = temp;
                    marker += 1;
                }
            }
            //put pivot(array[end]) between left and right subarrays
            temp = array[marker];
            array[marker] = array[end];
            array[end] = temp;
            return marker;
        }

        void quicksort(Point[] array, int start, int end)
        {
            if (start >= end)
            {
                return;
            }
            int pivot = partition(array, start, end);
            quicksort(array, start, pivot - 1);
            quicksort(array, pivot + 1, end);
        }
        #endregion

        #region Дополнительные функции
        /// <summary>
        /// Дистанция между точками
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <returns>double</returns>
        double Distance(PointF a1, PointF a2)
        {
            PointF a3 = minus(a1, a2);
            a3 = multiply(a3, a3);
            return a3.X+a3.Y;
        }

        /// <summary>
        /// инициализация пользовательских маркеров и цветов их отображения 
        /// </summary>
        /// <param name="i"></param>
        void Init(int i)
        {
            siteCount = i;
            g = new int[i][];
            for (int j = 0; j < i; j++)
                g[j] = new int[length_*dec_max];
            col = new Color[i];
            col2 = new Color[i];
            voron = new Point[i];
            int red = 250 / i;

            for (int j = 0; j < i; j++)
            {
                col[j] = Color.FromArgb(red * j, 0, 0);
            }
        }

        /// <summary>
        /// Функция скользящего среднего
        /// </summary>
        /// <param name="input">Входящий массив</param>
        /// <param name="window">Окно скольжения</param>
        /// <returns>Результирующий массив</returns>
        static int[] Smooth(int[] input, int window)
        {
            int[] output = new int[0];
            if (input.Length > 0)
            {
                output = new int[input.Length];
                int i, j, z, k1, k2, hw;
                double tmp;
                if (window % 2 == 0) window++;
                hw = (window - 1) / 2;
                output[0] = input[0];

                for (i = 1; i < input.Length; i++)
                {
                    tmp = 0;
                    if (i < hw)
                    {
                        k1 = 0;
                        k2 = 2 * i;
                        z = k2 + 1;
                    }
                    else if ((i + hw) > (input.Length - 1))
                    {
                        k1 = i - input.Length + i + 1;
                        k2 = input.Length - 1;
                        z = k2 - k1 + 1;
                    }
                    else
                    {
                        k1 = i - hw;
                        k2 = i + hw;
                        z = window;
                    }

                    for (j = k1; j < k2; j++)
                    {
                        if (j < input.Length)
                            tmp = tmp + input[j];
                    }
                    output[i] = (int)(tmp / z);

                }
            }
            return output;
        }

        #region Работа с коэффициентами
        /// <summary>
        /// Выводит коэфициент, из знания точки и границ
        /// </summary>
        /// <param name="e">точка</param>
        /// <param name="frame_start">начальная точка</param>
        /// <param name="frame_end">конечная точка</param>
        /// <returns>коэфициент</returns>
        PointF set_coef(PointF e, PointF frame_start,PointF frame_end)
        {
            return devide(minus(e, frame_start), minus(frame_end, frame_start));
        }
        /// <summary>
        /// Выводит динамическу точку из коэф. и границ
        /// </summary>
        /// <param name="coef">Коэффициент</param>
        /// <param name="frame_start">начальная точка</param>
        /// <param name="frame_end">конечная точка</param>
        /// <returns>преобразованная точка</returns>
        PointF get_coef(PointF coef, PointF frame_start, PointF frame_end)
        {
            return sum(multiply(minus(frame_end,frame_start), coef),frame_start);
        }
        #endregion

        /// <summary>
        /// Сравнивает Points выходит ли за максимальную границу
        /// </summary>
        /// <param name="a1">Первая точка</param>
        /// <param name="a2">Вторая точка</param>
        /// <returns>true Если первая точка больше(по оси Х или по оси Y)</returns>
        bool comparison(PointF a1,PointF a2)
        {
            return a1.X > a2.X || a1.Y > a2.Y;
        }

        #region + - * /
        /// <summary>
        /// Сумма Points
        /// </summary>
        /// <param name="a1">Point 1</param>
        /// <param name="a2">Point 2</param>
        /// <returns>Sum point 1 and 2</returns>
        PointF sum(PointF a1, PointF a2)
        { return new PointF(a1.X + a2.X, a1.Y + a2.Y); }

        /// <summary>
        /// Разность Points
        /// </summary>
        /// <param name="a1">Point 1</param>
        /// <param name="a2">Point 2</param>
        /// <returns>Point 1 - Point 2</returns>
        PointF minus(PointF a1, PointF a2)
        { return new PointF(a1.X - a2.X, a1.Y - a2.Y); }

        /// <summary>
        /// Делит Points
        /// </summary>
        /// <param name="a1">Point 1</param>
        /// <param name="a2">Point 2</param>
        /// <returns>(Point1.X/Point2.X,Point1.Y/Point2.Y)</returns>
        PointF devide(PointF a1, PointF a2)
        {
            return new PointF(a1.X / a2.X, a1.Y / a2.Y);
        }

        /// <summary>
        /// Умножает Points
        /// </summary>
        /// <param name="a1">Point 1</param>
        /// <param name="a2">Point 2</param>
        /// <returns>Point1.X*Point2.X,Point1.Y*Point2.Y</returns>
        PointF multiply(PointF a1, PointF a2)
        {
            return new PointF(a1.X * a2.X, a1.Y * a2.Y);
        }
        #endregion       
        #endregion

    }
}
