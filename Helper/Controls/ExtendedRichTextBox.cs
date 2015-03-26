using System;
using System.Security;
using System.Windows.Forms;

namespace Helper
{
    public class ExtendedRichTextBox : RichTextBox
    {
        private const String Msftedit = "msftedit.dll";

        protected override CreateParams CreateParams
        {
            [SecurityCritical]
            get
            {
                CreateParams param = base.CreateParams;
                if (NativeMethods.LoadLibraryW(Msftedit) != IntPtr.Zero)
                {
                    param.ClassName = @"RICHEDIT50W";
                }
                return param;
            }
        }

        public new void AppendText(String text)
        {
            Select(TextLength, 0);
            SelectedText = text;
        }
    }
}
