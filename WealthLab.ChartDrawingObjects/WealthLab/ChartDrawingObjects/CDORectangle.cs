namespace WealthLab.ChartDrawingObjects
{
    using System;
    using System.Drawing;
    using WealthLab;
    using WealthLab.ChartControl;

    public class CDORectangle : CDOPolygonBased
    {
        private static DrawingObjectHelper drawingObjectHelper_0 = new RectangleHelper();
        private Rectangle rectangle_0;

        public CDORectangle()
        {
        }

        public CDORectangle(ChartPane pane, DateTime dateTime_0, double value) : base(pane, dateTime_0, value)
        {
        }

        protected override bool IsMouseOver(int int_2, int int_3)
        {
            return ((((int_3 > (this.rectangle_0.Y - Chart.PixelSensitivity)) && (int_3 < (this.rectangle_0.Y + Chart.PixelSensitivity))) && ((int_2 > (this.rectangle_0.X - Chart.PixelSensitivity)) && (int_2 < (this.rectangle_0.X + (this.rectangle_0.Width + Chart.PixelSensitivity))))) || ((((int_3 > ((this.rectangle_0.Y + this.rectangle_0.Height) - Chart.PixelSensitivity)) && (int_3 < ((this.rectangle_0.Y + this.rectangle_0.Height) + Chart.PixelSensitivity))) && ((int_2 > (this.rectangle_0.X - Chart.PixelSensitivity)) && (int_2 < ((this.rectangle_0.X + this.rectangle_0.Width) + Chart.PixelSensitivity)))) || ((((int_2 > (this.rectangle_0.X - Chart.PixelSensitivity)) && (int_2 < (this.rectangle_0.X + Chart.PixelSensitivity))) && ((int_3 > (this.rectangle_0.Y - Chart.PixelSensitivity)) && (int_3 < (this.rectangle_0.Y + (this.rectangle_0.Height + Chart.PixelSensitivity))))) || (((int_2 > ((this.rectangle_0.X + this.rectangle_0.Width) - Chart.PixelSensitivity)) && (int_2 < ((this.rectangle_0.X + this.rectangle_0.Width) + Chart.PixelSensitivity))) && ((int_3 > (this.rectangle_0.Y - Chart.PixelSensitivity)) && (int_3 < (this.rectangle_0.Y + (this.rectangle_0.Height + Chart.PixelSensitivity))))))));
        }

        protected override void OnSelected(int int_2, int int_3)
        {
            if ((int_3 > (this.rectangle_0.Y - Chart.PixelSensitivity)) && (int_3 < (this.rectangle_0.Y + Chart.PixelSensitivity)))
            {
                base._mover.Y = this.rectangle_0.Y;
                base._mover.X = int_2;
            }
            else if ((int_3 > ((this.rectangle_0.Y + this.rectangle_0.Height) - Chart.PixelSensitivity)) && (int_3 < ((this.rectangle_0.Y + this.rectangle_0.Height) + Chart.PixelSensitivity)))
            {
                base._mover.Y = this.rectangle_0.Y + this.rectangle_0.Height;
                base._mover.X = int_2;
            }
            else if ((int_2 > (this.rectangle_0.X - Chart.PixelSensitivity)) && (int_2 < (this.rectangle_0.X + Chart.PixelSensitivity)))
            {
                base._mover.X = this.rectangle_0.X;
                base._mover.Y = int_3;
            }
            else if ((int_2 > ((this.rectangle_0.X + this.rectangle_0.Width) - Chart.PixelSensitivity)) && (int_2 < ((this.rectangle_0.X + this.rectangle_0.Width) + Chart.PixelSensitivity)))
            {
                base._mover.X = this.rectangle_0.X + this.rectangle_0.Width;
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
                this.rectangle_0.X = base.LeftHandle.X;
                this.rectangle_0.Y = (base.RightHandle.Y > base.LeftHandle.Y) ? base.LeftHandle.Y : base.RightHandle.Y;
                this.rectangle_0.Width = Math.Abs((int) (base.RightHandle.X - base.LeftHandle.X));
                this.rectangle_0.Height = Math.Abs((int) (base.RightHandle.Y - base.LeftHandle.Y));
                graphics_0.DrawRectangle(pen, this.rectangle_0);
                graphics_0.FillRectangle(brush, this.rectangle_0);
                brush.Dispose();
                pen.Dispose();
            }
        }

        protected override DrawingObjectHelper Helper
        {
            get
            {
                return drawingObjectHelper_0;
            }
        }
    }
}

