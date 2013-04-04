namespace Steema.TeeChart.Tools
{
    using Steema.TeeChart.Drawing;
    using System;
    using System.ComponentModel;
    using System.Drawing;

    [Serializable]
    public class DrawLineItem
    {
        private PointDouble endPos;
        private ChartPen pen;
        private PointDouble startPos;
        private DrawLineStyle style;
        private DrawLine tool;

        public DrawLineItem() : this(null)
        {
        }

        public DrawLineItem(DrawLine owner)
        {
            this.SetOwner(owner);
        }

        public void DrawHandles()
        {
            this.DrawHandles(this.tool.chart.graphics3D);
        }

        public void DrawHandles(Graphics3D g)
        {
            bool visible = g.Brush.Visible;
            bool flag2 = g.Pen.Visible;
            bool solid = g.Brush.Solid;
            Color color = g.Brush.Color;
            g.Brush.Visible = true;
            g.Brush.Solid = true;
            g.Brush.Color = (this.tool.chart.Panel.Color == Color.Black) ? Color.Silver : Color.Black;
            g.Pen.Visible = false;
            if (this.tool.InternalGetAxisRect().Contains(this.StartHandle))
            {
                g.Rectangle(this.StartHandle, 0);
            }
            if (this.tool.InternalGetAxisRect().Contains(this.EndHandle))
            {
                g.Rectangle(this.EndHandle, 0);
            }
            g.Brush.Visible = visible;
            g.Brush.Solid = solid;
            g.Brush.Color = color;
            g.Pen.Visible = flag2;
        }

        private Rectangle RectangleFromPoint(Point p)
        {
            return new Rectangle(p.X - 3, p.Y - 3, 6, 6);
        }

        internal void SetOwner(DrawLine owner)
        {
            this.tool = owner;
            if (this.tool != null)
            {
                this.Pen = this.tool.Pen.Clone() as ChartPen;
                this.tool.Lines.Add(this);
            }
        }

        [Description("Returns Rect of the DrawLine end handle.")]
        public Rectangle EndHandle
        {
            get
            {
                return this.RectangleFromPoint(this.tool.AxisPoint(this.EndPos));
            }
        }

        public PointDouble EndPos
        {
            get
            {
                return this.endPos;
            }
            set
            {
                this.endPos = value;
            }
        }

        [Description("Gets/Sets the pen of the DrawLine.")]
        public ChartPen Pen
        {
            get
            {
                if (this.pen == null)
                {
                    this.pen = new ChartPen(this.tool.Chart, Color.Black);
                }
                return this.pen;
            }
            set
            {
                this.pen = value;
            }
        }

        [Description("Returns Rect of the DrawLine start handle.")]
        public Rectangle StartHandle
        {
            get
            {
                return this.RectangleFromPoint(this.tool.AxisPoint(this.StartPos));
            }
        }

        public PointDouble StartPos
        {
            get
            {
                return this.startPos;
            }
            set
            {
                this.startPos = value;
            }
        }

        public DrawLineStyle Style
        {
            get
            {
                return this.style;
            }
            set
            {
                this.style = value;
            }
        }
    }
}

