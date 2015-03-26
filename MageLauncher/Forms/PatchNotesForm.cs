using System;
using System.Windows.Forms;

namespace MageLauncher.Forms
{
    public partial class PatchNotesForm : Form
    {
        public PatchNotesForm()
        {
            InitializeComponent();
        }

        private void CloseFormButtonClick(object sender, EventArgs e)
        {
            Visible = false;
        }
    }
}
