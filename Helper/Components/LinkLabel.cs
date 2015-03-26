using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace Helper
{
    public class LinkLabel : Label
    {
        public String Url { set; get; }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            Process.Start(Url);
        }
    }
}
