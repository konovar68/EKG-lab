using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace NetManager
{
    static class Program
    {
        private static Mutex _syncObject;

        private const string _syncObjectName = "{AA9A954A-771D-43B7-A473-CAE14CDC4E36}";

        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            bool createdNew;
            _syncObject = new Mutex(true, _syncObjectName, out createdNew);
            if (!createdNew)
            {
                MessageBox.Show("Сервер уже запущен");
                return;
            }
            else
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new FormServer());
            }
        }
    }
}
