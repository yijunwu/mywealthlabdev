namespace Steema.TeeChart.Tools
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Windows.Forms;

    [ToolboxBitmap(typeof(Magnify), "ToolsIcons.Magnify.bmp")]
    public class Magnify : RectangleTool
    {
        private bool circled;
        private bool followMouse;
        private double percent;
        private bool smooth;
        private int wheelZoom;

        public Magnify() : this(null)
        {
        }

        public Magnify(Chart c) : base(c)
        {
            this.circled = false;
            this.followMouse = false;
            this.percent = 50.0;
            this.smooth = false;
            this.wheelZoom = 5;
            base.Shape.Brush.Visible = false;
        }

        protected override void Assign(Steema.TeeChart.Tools.Tool t)
        {
            base.Assign(t);
            Magnify magnify = t as Magnify;
            magnify.circled = this.circled;
            magnify.followMouse = this.followMouse;
            magnify.percent = this.percent;
            magnify.smooth = this.smooth;
            magnify.wheelZoom = this.wheelZoom;
        }

        protected internal override void ChartEvent(EventArgs e)
        {
            if ((base.Active && (e is AfterDrawEventArgs)) && !base.Chart.Graphics3D.metafiling)
            {
                this.DrawLoupe();
            }
        }

        private void DoMouseWheel(MouseEventArgs e, ref Cursor c)
        {
            Point point = new Point(e.X, e.Y);
            if ((this.wheelZoom != 0) && base.Shape.ShapeBounds.Contains(point.X, point.Y))
            {
                if (e.Delta > 0)
                {
                    this.Percent = Math.Min((double) 100.0, (double) (this.Percent + Math.Abs(this.WheelZoom)));
                }
                else
                {
                    this.Percent = Math.Max((double) 0.001, (double) (this.Percent - Math.Abs(this.WheelZoom)));
                }
                this.Invalidate();
            }
        }

        private void DrawLoupe()
        {
            Rectangle bounds = base.Bounds;
            int width = Math.Abs(base.Width);
            int height = Math.Abs(base.Height);
            bounds.Width = width;
            bounds.Height = height;
            Bitmap image = new Bitmap(width, height);
            Graphics graphics = Graphics.FromImage(image);
            Chart chart = base.Chart.Clone() as Chart;
            for (int i = 0; i < chart.Tools.Count; i++)
            {
                if (chart.Tools[i] is Magnify)
                {
                    chart.Tools.RemoveAt(i);
                }
            }
            base.Shape.ShapeBounds = bounds;
            chart.Width = base.Chart.Width;
            chart.Height = base.Chart.Height;
            Bitmap bitmap2 = chart.Bitmap();
            graphics.FillRectangle(new SolidBrush(Color.White), new Rectangle(0, 0, width, height));
            graphics.DrawImage(bitmap2, new Rectangle(0, 0, width, height), bounds, GraphicsUnit.Pixel);
            new ZoomFilter { Smooth = this.Smooth, Percent = this.Percent }.Apply(ref image);
            if (this.circled)
            {
                Bitmap bitmap3 = new Bitmap(width, height);
                GraphicsPath path = new GraphicsPath();
                path.AddEllipse(new Rectangle(0, 0, width, height));
                graphics = Graphics.FromImage(bitmap3);
                graphics.SetClip(path);
                graphics.DrawImage(image, 0, 0);
                image = bitmap3;
            }
            Graphics3D graphicsd = base.Chart.Graphics3D;
            graphicsd.Brush = base.Shape.Brush;
            graphicsd.Pen = base.Shape.Pen;
            graphicsd.Draw(bounds.X, bounds.Y, image);
            if (this.circled)
            {
                graphicsd.Ellipse(bounds);
            }
            else
            {
                graphicsd.Rectangle(bounds);
            }
        }

        protected internal override void MouseEvent(MouseEventKinds kind, MouseEventArgs e, ref Cursor c)
        {
            if ((base.Active && this.FollowMouse) && (kind == MouseEventKinds.Move))
            {
                Point point = new Point(e.X, e.Y);
                base.Left = point.X - (base.Width / 2);
                base.Top = point.Y - (base.Height / 2);
            }
            else if (kind == MouseEventKinds.Wheel)
            {
                this.DoMouseWheel(e, ref c);
            }
            else
            {
                base.MouseEvent(kind, e, ref c);
            }
        }

        protected override void SetChart(Chart c)
        {
            base.SetChart(c);
            if ((c != null) && (c.Parent != null))
            {
                c.Parent.DoInvalidate();
            }
        }

        [DefaultValue(false), Description("Sets or returns whether the tool is drawn as a circle.")]
        public bool Circled
        {
            get
            {
                return this.circled;
            }
            set
            {
                this.circled = value;
                this.Invalidate();
            }
        }

        public override string Description
        {
            get
            {
                return Texts.Magnify;
            }
        }

        [DefaultValue(false), Description("Sets or returns whether the tool follows mouse movement.")]
        public bool FollowMouse
        {
            get
            {
                return this.followMouse;
            }
            set
            {
                this.followMouse = value;
                this.Invalidate();
            }
        }

        [DefaultValue(50), Description("Sets or returns the percentage by which the underlying image is magnified.")]
        public double Percent
        {
            get
            {
                return this.percent;
            }
            set
            {
                this.percent = value;
                this.Invalidate();
            }
        }

        [Description("Sets or returns whether the magnified image is smoothed."), DefaultValue(false)]
        public bool Smooth
        {
            get
            {
                return this.smooth;
            }
            set
            {
                this.smooth = value;
            }
        }

        public override string Summary
        {
            get
            {
                return Texts.MagnifySummary;
            }
        }

        [Description("Sets or returns the percentage by which the mouse wheel magnifies the image."), DefaultValue(5)]
        public int WheelZoom
        {
            get
            {
                return this.wheelZoom;
            }
            set
            {
                this.wheelZoom = value;
            }
        }
    }
}

