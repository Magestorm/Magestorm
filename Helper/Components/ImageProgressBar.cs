using System;
using System.Drawing;
using System.Windows.Forms;

namespace Helper
{
    public class ImageProgressBar : PictureBox
    {
        public PictureBox ProgressBox;
        public PictureBox ProgressBorderBox;

        public Int64 Min { get; set; }
        public Int64 Max { get; set; }
        public Int64 Step { get; set; }
        private Int64 _value;
        public Int64 Value
        {
            get
            {
                return _value;
            }
            set
            {
                if (value >= Max)
                {
                    value = Max;
                    if (ProgressBox != null)
                    {
                        ProgressBox.Enabled = false;
                    }
                }
                else
                {
                    if (ProgressBox != null)
                    {
                        ProgressBox.Enabled = true;
                    }
                }

                _value = value;
            }
        }

        public override String Text
        {
            get
            {
                return String.Format(@"{0:0.}%", (100.0f / Max) * Value);
            }
        }

        public Int32 ImageBorderPixelSize { get; set; }
        public Image ProgressImage { set; get; }
        public Image ProgressBorderImage { set; get; }
        public Boolean ShowPercentage { set; get; }

        protected override void OnHandleCreated(EventArgs e)
        {
            ProgressBox = new PictureBox
            {
                Image = ProgressImage,
                Location = new Point(ImageBorderPixelSize, ImageBorderPixelSize),
                Size = new Size(0, Size.Height - (ImageBorderPixelSize * 2))
            };

            ProgressBox.Paint += ProgressBoxPaintEx;

            Controls.Add(ProgressBox);

            ProgressBorderBox = new PictureBox
            {
                BackgroundImage = ProgressBorderImage,
                Location = new Point(0, 0),
                Size = new Size(0, Size.Height)
            };

            Controls.Add(ProgressBorderBox);

            base.OnHandleCreated(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Int32 iSize = 0;

            try
            {
				iSize = (Int32)System.Math.Floor(((double)Size.Width / Max) * Value);
            }
            catch (Exception) { }

            ProgressBox.Size = Value < Max ? new Size(iSize - ImageBorderPixelSize, ProgressBox.Size.Height) : new Size(iSize - ((ImageBorderPixelSize * 2)), ProgressBox.Size.Height);
            ProgressBorderBox.Size = new Size(iSize, ProgressBorderBox.Size.Height);

            if (ShowPercentage) e.Graphics.DrawString(Text, Font, new SolidBrush(Color.FromArgb(255, 225, 35, 35)), ((float)Size.Width / 2) - (Text.Length * 3), ((float)Size.Height / 2) - (ImageBorderPixelSize * 4));

            base.OnPaint(e);
        }

        public void ProgressBoxPaintEx(object sender, PaintEventArgs e)
        {
            if (ShowPercentage) e.Graphics.DrawString(Text, Font, new SolidBrush(Color.FromArgb(255, 200, 200, 200)), ((float)Size.Width / 2) - (Text.Length * 3), ((float)Size.Height / 2) - (ImageBorderPixelSize * 5));
        }

        public void DoStep()
        {
            Value += Step;

            Refresh();
        }

        public void DoFullStep()
        {
            if (Max <= 0) Max = 1;

            Value = Max;

            Refresh();
        }

        public void Reset()
        {
            Value = 0;
            Max = 1;

            Refresh();
        }
    }
}
