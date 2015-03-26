using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using MageLauncher.Forms;
using MageLauncher.Properties;

namespace MageLauncher
{
    public static class Program
    {
        private const String User32 = "user32.dll";
        
        [DllImport(User32)]
        public static extern int SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);
        [DllImport(User32)]
        public static extern bool ReleaseCapture();

        public static String BaseDirectory
        {
            get
            {
                return String.Format("{0}\\", Directory.GetCurrentDirectory());
            }
        }

        public static LauncherForm MainForm;
        public static OptionsForm OptionForm;
        public static PatchNotesForm PatchNotesForm;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (IsGameRunning())
            {
                MessageBox.Show(Resources.MessageBox_Message_Game_Already_Running, Resources.MessageBox_Title_Error,MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Application.Exit();
                return;
            }

            OptionForm = new OptionsForm();
            MainForm = new LauncherForm();
            PatchNotesForm = new PatchNotesForm();

            Application.Run(MainForm);
        }

        public static Boolean IsGameRunning()
        {
            return Process.GetProcesses().Any(p => p.ProcessName == "Magestorm");
        }
    }
}
