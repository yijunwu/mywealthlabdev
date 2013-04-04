namespace Steema.TeeChart.Drawing
{
    using Steema.TeeChart;
    using System;
    using System.ComponentModel;
    using System.Drawing;

    public sealed class ImageBevel : TeeBase, ICloneable
    {
        private ChartBrush brush;
        private ChartPen pen;
        private bool visible;
        private int width;

        public ImageBevel(Chart c) : base(c)
        {
            this.width = 10;
        }

        [Description("Assign all properties from a bevel to another."), Obsolete("Please use ImageBevel.Clone() method")]
        public void Assign(ImageBevel b)
        {
            if (b != null)
            {
                this.Pen = b.Pen;
                this.Brush = b.Brush;
                this.Visible = b.Visible;
            }
        }

        private void AssignImageBevel(ImageBevel b)
        {
            if (b != null)
            {
                b.Pen = this.Pen.Clone() as ChartPen;
                b.Brush = this.Brush.Clone() as ChartBrush;
                b.Visible = this.Visible;
            }
        }

        public object Clone()
        {
            ImageBevel b = new ImageBevel(base.Chart);
            this.AssignImageBevel(b);
            return b;
        }

        public void Draw(Graphics3D g, Rectangle rect, int borderRound)
        {
            if (this.Visible)
            {
                g.PaintImageBevel(rect, this.Width, this.Pen, this.Brush, borderRound);
            }
        }

        [Description("Sets ImageBevel Brush characteristics."), Category("Appearance"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ChartBrush Brush
        {
            get
            {
                if (this.brush == null)
                {
                    this.brush = new ChartBrush(base.chart, Color.LightGray);
                }
                return this.brush;
            }
            set
            {
                this.brush = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("Appearance"), Description("Sets ImageBevel Pen characteristics.")]
        public ChartPen Pen
        {
            get
            {
                if (this.pen == null)
                {
                    this.pen = new ChartPen(base.chart, Color.Black);
                }
                return this.pen;
            }
            set
            {
                this.pen = value;
            }
        }

        [Description("Sets the visibility of the Image Bevel."), DefaultValue(false)]
        public bool Visible
        {
            get
            {
                return this.visible;
            }
            set
            {
                base.SetBooleanProperty(ref this.visible, value);
            }
        }

        [Description("Draws an Image Bevel of the specified width."), DefaultValue(10)]
        public int Width
        {
            get
            {
                return this.width;
            }
            set
            {
                base.SetIntegerProperty(ref this.width, value);
            }
        }
    }
}

