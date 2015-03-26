using System;
using System.Drawing;
using System.Windows.Forms;

namespace Helper
{
    public class ImageButton : Label
    {
        public Image MouseEnterImage { set; get; }
        public Image MouseLeaveImage { set; get; }
        public Image MouseDownImage { set; get; }
        public Image DisabledImage { set; get; }

        public new Boolean Enabled
        {
            get
            {
                return base.Enabled;
            }
            set
            {
                base.Enabled = value;

                if (!base.Enabled)
                {
                    Image = DisabledImage;
                }
            }
        }

        protected override void OnCreateControl()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            BackColor = Color.Transparent;

            base.OnCreateControl();
        }
        protected override void OnMouseEnter(EventArgs e)
        {
            if (MouseEnterImage != null && Enabled) Image = MouseEnterImage;

            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            if (MouseLeaveImage != null && Enabled) Image = MouseLeaveImage;

            base.OnMouseLeave(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (MouseDownImage != null && Enabled) Image = MouseDownImage;

            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (MouseDownImage != null && Enabled) Image = MouseEnterImage;

            base.OnMouseUp(e);
        }
    }
}
