using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace MageReboot
{
    static class Program
    {
        public static String BaseDirectory
        {
            get
            {
                return String.Format("{0}\\", Directory.GetCurrentDirectory());
            }
        }

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
