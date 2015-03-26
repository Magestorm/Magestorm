using System;
using System.Windows.Forms;
using Helper;
using Helper.Timing;
using MageLauncher.Properties;

namespace MageLauncher
{
    public partial class LauncherForm : Form
    {
        public LauncherForm()
        {
            InitializeComponent();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (e.Button != MouseButtons.Left) return;

            Program.ReleaseCapture();
            Program.SendMessage(Handle, 0xA1, 0x2, 0);
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            TimeHelper.SpinApplication(1500);

            Patch.Start();
        }

        private void ExitButtonClick(object sender, EventArgs e)
        {
            if (Patch.Web.IsBusy)
            {
                DialogResult result = MessageBox.Show(Resources.MessageBox_Message_Exit_Confirm, Resources.MessageBox_Title_Launcher, MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                switch (result)
                {
                    case DialogResult.No:
                    {
                        return;
                    }
                }
            }

            Environment.Exit(0);
        }

        private void OptionsButtonClick(object sender, EventArgs e)
        {
            Program.OptionForm.Show();
        }
    }
}
