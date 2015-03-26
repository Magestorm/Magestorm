using System;
using System.Drawing;
using System.Windows.Forms;

namespace Helper
{
    public class ImageTextBox : Panel
    {
        public readonly TextBox TextBox;

        public new String Text
        {
            get
            {
                return TextBox.Text;
            }
            set
            {
                TextBox.Text = value;
            }
        }
        public Char PasswordChar
        {
            get
            {
                return TextBox.PasswordChar;
            }
            set
            {
                TextBox.PasswordChar = value;
            }
        }

        public ImageTextBox()
        {
            TextBox = new TextBox
            {
                Location = new Point(2, 1),
                BorderStyle = BorderStyle.None
            };

            TextBox.GotFocus += OnTextBoxGotFocus;
            TextBox.LostFocus += OnTextBoxLostFocus;

            Controls.Add(TextBox);
        }

        protected override void OnCreateControl()
        {
            TextBox.Size = Size - new Size(4, 0);

            base.OnCreateControl();
        }

        protected override void OnGotFocus(EventArgs e)
        {
            TextBox.Focus();

            base.OnGotFocus(e);
        }

        public void OnTextBoxGotFocus(object sender, EventArgs e)
        {
            Refresh();
        }

        public void OnTextBoxLostFocus(object sender, EventArgs e)
        {
            Refresh();
        }

        protected override void OnPaint(PaintEventArgs args)
        {
            SolidBrush bBrush = new SolidBrush(TextBox.Focused ? Color.Red : Color.LightGray);

            Pen bPen = new Pen(bBrush, 1.6f);

            args.Graphics.DrawLine(bPen, 0, Height - 1, Width, Height - 1);
            args.Graphics.DrawLine(bPen, 0, Height - 1, 0, 0);
            args.Graphics.DrawLine(bPen, Width - 1, Height - 1, Width - 1, 0);
            args.Graphics.DrawLine(bPen, 0, 0, Width, 0);

            base.OnPaint(args);
        }
    }
}
