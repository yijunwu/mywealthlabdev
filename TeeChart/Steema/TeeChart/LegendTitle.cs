namespace Steema.TeeChart
{
    using Steema.TeeChart.Drawing;
    using System;
    using System.Drawing;

    public sealed class LegendTitle : TextShape
    {
        private int FontH;
        private int tmpFrameWidth;
        private int tmpMargin;
        private int tmpXPosTitle;

        public LegendTitle(Chart c) : base(c)
        {
            base.Font.Color = Color.Black;
            base.Font.Bold = true;
            base.Font.defaultBold = true;
            base.Font.Brush.defaultColor = Color.Black;
            base.Pen.Visible = false;
            base.Pen.defaultVisible = false;
            base.defaultVisible = false;
        }

        public LegendTitle(Chart c, bool startVisible) : this(c)
        {
            base.Visible = startVisible;
            base.defaultVisible = base.Visible;
        }

        internal void CalcHeight()
        {
            base.Chart.Graphics3D.Font = base.Font;
            base.Height = Utils.Round((float) (base.Chart.Graphics3D.TextHeight("W") * base.Lines.Length));
            if (!base.Transparent)
            {
                base.Height += 2;
                if (base.Pen.Visible)
                {
                    base.Height += 2 * base.Pen.Width;
                }
            }
        }

        internal void CalcShapeBounds(Rectangle R)
        {
            base.ShapeBounds = Utils.FromLTRB(R.Left + 2, R.Top + 2, R.Right - 2, (R.Top + 4) + base.Height);
            if (!base.Transparent && base.Shadow.Visible)
            {
                if (base.Shadow.Width > 0)
                {
                    base.iRight -= base.Shadow.Width;
                }
                else
                {
                    base.iLeft -= base.Shadow.Width;
                }
                if (base.Shadow.Height < 0)
                {
                    base.iTop -= base.Shadow.Height;
                }
            }
        }

        internal void DrawLineTitle(int AIndex)
        {
            string text = base.Lines[AIndex];
            int y = base.ShapeBounds.Top + ((AIndex * this.FontH) + this.tmpFrameWidth);
            if (base.TextAlign == StringAlignment.Far)
            {
                this.tmpXPosTitle = (base.ShapeBounds.Right - Utils.Round(base.Chart.Graphics3D.TextWidth(text))) - (this.tmpMargin / 2);
            }
            else if (base.TextAlign == StringAlignment.Center)
            {
                this.tmpXPosTitle = Utils.Round((float) ((base.ShapeBounds.Left + base.ShapeBounds.Right) / 2)) - Utils.Round((float) (base.Chart.Graphics3D.TextWidth(text) / 2f));
            }
            base.Chart.Graphics3D.TextOut(this.tmpXPosTitle, y, text);
        }

        internal void DrawText()
        {
            if (base.Pen.Visible)
            {
                this.tmpFrameWidth = base.Pen.Width;
            }
            else
            {
                this.tmpFrameWidth = 1;
            }
            this.tmpMargin = Utils.Round(base.Chart.Graphics3D.TextWidth("W"));
            this.FontH = Utils.Round(base.Chart.Graphics3D.TextHeight("W"));
            if (base.TextAlign == StringAlignment.Near)
            {
                this.tmpXPosTitle = base.ShapeBounds.Left + (this.tmpMargin / 2);
            }
            if (base.TextFormat == TextFormat.Normal)
            {
                for (int i = 0; i < base.Lines.Length; i++)
                {
                    this.DrawLineTitle(i);
                }
            }
            else
            {
                base.Chart.Graphics3D.TextAlign = base.TextAlign;
                base.Chart.Graphics3D.TextOut(this.tmpXPosTitle, this.tmpFrameWidth + base.ShapeBounds.Top, base.Text);
            }
        }

        internal void InternalDraw(Graphics3D g, Rectangle Rect)
        {
            this.CalcShapeBounds(Rect);
            base.DrawRectRotated(g, base.ShapeBounds, 0, 0);
            this.DrawText();
        }

        internal int TotalWidth()
        {
            base.Chart.Graphics3D.Font = base.Font;
            int num = 0;
            for (int i = 0; i < base.Lines.Length; i++)
            {
                num = Math.Max(num, Utils.Round(base.Chart.Graphics3D.TextWidth(base.Lines[i])));
            }
            num += Utils.Round(base.Chart.Graphics3D.TextWidth("W"));
            if (!base.Transparent)
            {
                if (base.Pen.Visible)
                {
                    num += base.Pen.Width * 2;
                }
                if (base.Shadow.Visible)
                {
                    num += base.Shadow.Width;
                }
            }
            return num;
        }
    }
}

