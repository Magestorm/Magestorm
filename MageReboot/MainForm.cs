using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace MageReboot
{
    public partial class MainForm : Form
    {
        public static Boolean HasExited;

        public MainForm()
        {
            InitializeComponent();
            Visible = false;
        }

        private void ExitMenuItemClick(object sender, EventArgs e)
        {
            HasExited = true;

            Application.Exit();
        }

        private void MainFormShown(object sender, EventArgs e)
        {
            Visible = false;

            Thread workerThread = new Thread(WorkerThread);
            workerThread.Start();
        }

        private static void WorkerThread()
        {
	        Process childProcess = null;

            while (!HasExited)
            {
	            if (childProcess != null && !childProcess.HasExited && childProcess.Responding)
	            {
					Thread.Sleep(1000);
	            }
	            else
	            {
		            try
		            {
						if (childProcess != null && childProcess.HasExited != true)
						{
							childProcess.Kill();
						}
		            }
		            catch { }

					Thread.Sleep(500);

					childProcess = Process.Start(String.Format("{0}\\MageServer.exe", Program.BaseDirectory), "-restart");
	            }
            }
        }
    }
}
