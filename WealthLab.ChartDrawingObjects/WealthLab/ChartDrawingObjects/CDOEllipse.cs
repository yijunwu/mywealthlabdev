namespace WealthLab.ChartDrawingObjects
{
    using System;
    using System.Drawing;
    using WealthLab;
    using WealthLab.ChartControl;

    public class CDOEllipse : CDORectangle
    {
        private static DrawingObjectHelper drawingObjectHelper_1 = new EllipseHelper();
        private Rectangle rectangle_1;

        public CDOEllipse()
        {
        }

        public CDOEllipse(ChartPane pane, DateTime dateTime_0, double value) : base(pane, dateTime_0, value)
        {
        }

        protected override bool IsMouseOver(int int_2, int int_3)
        {
            return ((((int_3 > (this.rectangle_1.Y - Chart.PixelSensitivity)) && (int_3 < (this.rectangle_1.Y + Chart.PixelSensitivity))) && ((int_2 > (this.rectangle_1.X - Chart.PixelSensitivity)) && (int_2 < (this.rectangle_1.X + (this.rectangle_1.Width + Chart.PixelSensitivity))))) || ((((int_3 > ((this.rectangle_1.Y + this.rectangle_1.Height) - Chart.PixelSensitivity)) && (int_3 < ((this.rectangle_1.Y + this.rectangle_1.Height) + Chart.PixelSensitivity))) && ((int_2 > (this.rectangle_1.X - Chart.PixelSensitivity)) && (int_2 < ((this.rectangle_1.X + this.rectangle_1.Width) + Chart.PixelSensitivity)))) || ((((int_2 > (this.rectangle_1.X - Chart.PixelSensitivity)) && (int_2 < (this.rectangle_1.X + Chart.PixelSensitivity))) && ((int_3 > (this.rectangle_1.Y - Chart.PixelSensitivity)) && (int_3 < (this.rectangle_1.Y + (this.rectangle_1.Height + Chart.PixelSensitivity))))) || (((int_2 > ((this.rectangle_1.X + this.rectangle_1.Width) - Chart.PixelSensitivity)) && (int_2 < ((this.rectangle_1.X + this.rectangle_1.Width) + Chart.PixelSensitivity))) && ((int_3 > (this.rectangle_1.Y - Chart.PixelSensitivity)) && (int_3 < (this.rectangle_1.Y + (this.rectangle_1.Height + Chart.PixelSensitivity))))))));
        }

        protected override void OnSelected(int int_2, int int_3)
        {
            if ((int_3 > (this.rectangle_1.Y - Chart.PixelSensitivity)) && (int_3 < (this.rectangle_1.Y + Chart.PixelSensitivity)))
            {
                base._mover.Y = this.rectangle_1.Y;
                base._mover.X = int_2;
            }
            else if ((int_3 > ((this.rectangle_1.Y + this.rectangle_1.Height) - Chart.PixelSensitivity)) && (int_3 < ((this.rectangle_1.Y + this.rectangle_1.Height) + Chart.PixelSensitivity)))
            {
                base._mover.Y = this.rectangle_1.Y + this.rectangle_1.Height;
                base._mover.X = int_2;
            }
            else if ((int_2 > (this.rectangle_1.X - Chart.PixelSensitivity)) && (int_2 < (this.rectangle_1.X + Chart.PixelSensitivity)))
            {
                base._mover.X = this.rectangle_1.X;
                base._mover.Y = int_3;
            }
            else if ((int_2 > ((this.rectangle_1.X + this.rectangle_1.Width) - Chart.PixelSensitivity)) && (int_2 < ((this.rectangle_1.X + this.rectangle_1.Width) + Chart.PixelSensitivity)))
            {
                base._mover.X = this.rectangle_1.X + this.rectangle_1.Width;
                base._mover.Y = int_3;
            }
        }

        protected override void Render(Graphics graphics_0)
        {
            if ((base.LeftHandle.Bar != -1) && (base.RightHandle.Bar != -1))
            {
                Pen pen = new Pen(base.Color, (float) base.Width);
                SolidBrush brush = new SolidBrush(base.ColorWithTransparency(base.Color, base.FillTransparency));
                ChartRenderer.SetPenStyle(pen, base.Style);
                pen.Width = base.Width;
                this.rectangle_1.X = base.LeftHandle.X;
                this.rectangle_1.Y = (base.RightHandle.Y > base.LeftHandle.Y) ? base.LeftHandle.Y : base.RightHandle.Y;
                this.rectangle_1.Width = Math.Abs((int) (base.RightHandle.X - base.LeftHandle.X));
                this.rectangle_1.Height = Math.Abs((int) (base.RightHandle.Y - base.LeftHandle.Y));
                graphics_0.DrawEllipse(pen, this.rectangle_1);
                graphics_0.FillEllipse(brush, this.rectangle_1);
                brush.Dispose();
                pen.Dispose();
            }
        }

        protected override DrawingObjectHelper Helper
        {
            get
            {
                return drawingObjectHelper_1;
            }
        }
    }
}

