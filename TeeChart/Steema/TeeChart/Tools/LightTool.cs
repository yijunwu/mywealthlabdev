namespace Steema.TeeChart.Tools
{
    using Steema.TeeChart;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    [Description("Contains a Chart Tool that displays a 2 dimensional \"light\" effect over the chart Canvas."), ToolboxBitmap(typeof(LightTool), "ToolsIcons.LightTool.bmp")]
    public class LightTool : Steema.TeeChart.Tools.Tool
    {
        private const int BytesPerPixel = 4;
        private double factor;
        private Bitmap fBuffer;
        private bool insideLighting;
        private int left;
        private bool mouse;
        private LightStyle style;
        private int top;

        public LightTool() : this(null)
        {
        }

        public LightTool(Chart c) : base(c)
        {
            this.factor = 10.0;
            this.style = LightStyle.Linear;
            this.left = -1;
            this.top = -1;
        }

        protected override void Assign(Steema.TeeChart.Tools.Tool t)
        {
            base.Assign(t);
            LightTool tool = t as LightTool;
            tool.Factor = this.Factor;
            tool.FollowMouse = this.FollowMouse;
            tool.Left = this.Left;
            tool.Style = this.Style;
            tool.Top = this.Top;
        }

        protected internal override void ChartEvent(EventArgs e)
        {
            if ((base.Active && (e is AfterDrawEventArgs)) && !this.insideLighting)
            {
                this.fBuffer = null;
                this.Iluminate();
            }
        }

        private void CheckBuffer()
        {
            if (((this.fBuffer == null) || (this.fBuffer.Width != base.chart.Width)) || (this.fBuffer.Height != base.chart.Height))
            {
                this.fBuffer = new Bitmap(base.chart.Width, base.chart.Height);
                base.chart.Draw(Graphics.FromImage(this.fBuffer), true);
            }
        }

        public void Iluminate()
        {
            if (!base.chart.Graphics3D.metafiling && !this.insideLighting)
            {
                this.insideLighting = true;
                try
                {
                    int left;
                    int top;
                    this.CheckBuffer();
                    if (this.left == -1)
                    {
                        left = base.chart.ChartBounds.Left + (base.chart.ChartBounds.Width / 2);
                    }
                    else
                    {
                        left = this.left;
                    }
                    if (this.top == -1)
                    {
                        top = base.chart.ChartBounds.Top + (base.chart.ChartBounds.Height / 2);
                    }
                    else
                    {
                        top = this.top;
                    }
                    Bitmap image = this.TeeLight(left, top);
                    base.chart.Graphics3D.Draw(0, 0, image);
                }
                finally
                {
                    this.insideLighting = false;
                }
            }
        }

        protected internal override void MouseEvent(MouseEventKinds kind, MouseEventArgs e, ref Cursor c)
        {
            if ((base.Active && this.mouse) && (kind == MouseEventKinds.Move))
            {
                Point point = new Point(e.X, e.Y);
                this.left = point.X;
                this.top = point.Y;
                this.Invalidate();
            }
        }

        private Bitmap TeeLight(int xx, int yy)
        {
            Bitmap bitmap = null;
            int height = this.fBuffer.Height;
            int width = this.fBuffer.Width;
            bitmap = new Bitmap(width, height);
            Rectangle rect = new Rectangle(0, 0, width, height);
            BitmapData bitmapdata = bitmap.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppPArgb);
            double num6 = 100.0 / Math.Sqrt((double) ((width * width) + (height * height)));
            BitmapData data2 = this.fBuffer.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppPArgb);
            IntPtr ptr = bitmapdata.Scan0;
            IntPtr ptr2 = data2.Scan0;
            int stride = data2.Stride;
            double num8 = this.Factor * 0.01;
            if (this.Style == LightStyle.Linear)
            {
                num8 = 20.0 * num8;
            }
            double num9 = num8 * num6;
            for (int i = 0; i < height; i++)
            {
                int num10 = Utils.Round(Utils.Sqr((double) (i - yy)));
                for (int j = 0; j < width; j++)
                {
                    int num;
                    double num7 = Math.Sqrt(Utils.Sqr((double) (j - xx)) + num10);
                    if (this.Style == LightStyle.Linear)
                    {
                        num = Utils.Round((double) (num9 * num7));
                    }
                    else
                    {
                        num = Utils.Round((double) (num8 * Utils.Sqr(num6 * (num7 + 1.0))));
                    }
                    int ofs = (i * stride) + (4 * j);
                    if (num > 0)
                    {
                        if (num >= 0xff)
                        {
                            Marshal.WriteByte(ptr, ofs + 2, 0);
                            Marshal.WriteByte(ptr, ofs + 1, 0);
                            Marshal.WriteByte(ptr, ofs, 0);
                            Marshal.WriteByte(ptr, ofs + 3, 0xff);
                        }
                        else
                        {
                            byte num11;
                            byte val = Marshal.ReadByte(ptr2, ofs + 2);
                            if (num >= val)
                            {
                                Marshal.WriteByte(ptr, ofs + 2, 0);
                            }
                            else
                            {
                                num11 = Convert.ToByte((int) (val - num));
                                Marshal.WriteByte(ptr, ofs + 2, num11);
                            }
                            val = Marshal.ReadByte(ptr2, ofs + 1);
                            if (num >= val)
                            {
                                Marshal.WriteByte(ptr, ofs + 1, 0);
                            }
                            else
                            {
                                num11 = Convert.ToByte((int) (val - num));
                                Marshal.WriteByte(ptr, ofs + 1, num11);
                            }
                            val = Marshal.ReadByte(ptr2, ofs);
                            if (num >= val)
                            {
                                Marshal.WriteByte(ptr, ofs, 0);
                            }
                            else
                            {
                                num11 = Convert.ToByte((int) (val - num));
                                Marshal.WriteByte(ptr, ofs, num11);
                            }
                            val = Marshal.ReadByte(ptr2, ofs + 3);
                            if ((val + num) <= 0xff)
                            {
                                num11 = Convert.ToByte((int) (val + num));
                                Marshal.WriteByte(ptr, ofs + 3, num11);
                            }
                            else
                            {
                                Marshal.WriteByte(ptr, ofs + 3, val);
                            }
                        }
                    }
                    else
                    {
                        Marshal.WriteByte(ptr, ofs + 2, Marshal.ReadByte(ptr2, ofs + 2));
                        Marshal.WriteByte(ptr, ofs + 1, Marshal.ReadByte(ptr2, ofs + 1));
                        Marshal.WriteByte(ptr, ofs, Marshal.ReadByte(ptr2, ofs));
                        Marshal.WriteByte(ptr, ofs + 3, Marshal.ReadByte(ptr2, ofs + 3));
                    }
                }
            }
            this.fBuffer.UnlockBits(data2);
            bitmap.UnlockBits(bitmapdata);
            return bitmap;
        }

        [Description("Gets descriptive text.")]
        public override string Description
        {
            get
            {
                return Texts.LightTool;
            }
        }

        [Browsable(true), DefaultValue(10)]
        public double Factor
        {
            get
            {
                return this.factor;
            }
            set
            {
                base.SetDoubleProperty(ref this.factor, value);
            }
        }

        [DefaultValue(false), Browsable(true)]
        public bool FollowMouse
        {
            get
            {
                return this.mouse;
            }
            set
            {
                this.mouse = value;
            }
        }

        [DefaultValue(-1), Browsable(true)]
        public int Left
        {
            get
            {
                return this.left;
            }
            set
            {
                base.SetIntegerProperty(ref this.left, value);
            }
        }

        [DefaultValue(0), Browsable(true)]
        public LightStyle Style
        {
            get
            {
                return this.style;
            }
            set
            {
                if (this.style != value)
                {
                    this.style = value;
                    this.Invalidate();
                }
            }
        }

        [Description("Gets detailed descriptive text.")]
        public override string Summary
        {
            get
            {
                return Texts.LightToolSummary;
            }
        }

        [DefaultValue(-1), Browsable(true)]
        public int Top
        {
            get
            {
                return this.top;
            }
            set
            {
                base.SetIntegerProperty(ref this.top, value);
            }
        }
    }
}

