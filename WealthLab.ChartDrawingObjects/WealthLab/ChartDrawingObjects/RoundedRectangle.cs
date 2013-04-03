namespace WealthLab.ChartDrawingObjects
{
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;

    public class RoundedRectangle
    {
        private GraphicsPath graphicsPath_0;
        private int int_0;
        private int int_1;
        private int int_2;
        private int int_3;
        private int int_4;
        private RectangleF rectangleF_0;

        public RoundedRectangle(Rectangle Rec, int CornerSize)
        {
            this.int_0 = Rec.X;
            this.int_1 = Rec.Y;
            this.int_2 = Rec.Width;
            this.int_3 = Rec.Height;
            this.int_4 = CornerSize;
        }

        public RectangleF InnerRectangle
        {
            get
            {
                return this.rectangleF_0;
            }
        }

        public GraphicsPath Path
        {
            get
            {
                int x = Convert.ToInt32((int) (this.int_0 + (this.int_2 - this.int_4)));
                int y = Convert.ToInt32((int) (this.int_3 + (this.int_1 - this.int_4)));
                Rectangle rect = new Rectangle(this.int_0, this.int_1, this.int_4, this.int_4);
                Rectangle rectangle2 = new Rectangle(x, this.int_1, this.int_4, this.int_4);
                Rectangle rectangle3 = new Rectangle(this.int_0, y, this.int_4, this.int_4);
                Rectangle rectangle4 = new Rectangle(x, y, this.int_4, this.int_4);
                this.rectangleF_0 = new RectangleF((float) this.int_0, this.int_1 + (this.int_4 * 0.5f), (float) this.int_2, (float) (this.int_3 - this.int_4));
                this.graphicsPath_0 = new GraphicsPath();
                this.graphicsPath_0.AddArc(rect, 180f, 90f);
                this.graphicsPath_0.AddArc(rectangle2, 270f, 90f);
                this.graphicsPath_0.AddArc(rectangle4, 360f, 90f);
                this.graphicsPath_0.AddArc(rectangle3, 90f, 90f);
                this.graphicsPath_0.CloseAllFigures();
                return this.graphicsPath_0;
            }
        }
    }
}

