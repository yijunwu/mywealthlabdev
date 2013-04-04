namespace Steema.TeeChart.Styles
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;

    public class CustomImagePoint : Points
    {
        private Image pointimage;
        private bool transparent;

        public event GetImageEvent GetImage;

        public CustomImagePoint() : this(null)
        {
        }

        public CustomImagePoint(Chart c) : base(c)
        {
        }

        public override void DrawValue(int valueIndex)
        {
            if (this.pointimage == null)
            {
                base.DrawValue(valueIndex);
            }
            else
            {
                if (this.GetImage != null)
                {
                    this.GetImage(this, valueIndex, this.pointimage);
                }
                Graphics3D graphicsd = base.Chart.Graphics3D;
                Rectangle r = new Rectangle(this.CalcXPos(valueIndex) - (base.Pointer.HorizSize / 2), this.CalcYPos(valueIndex) - (base.Pointer.VertSize / 2), base.Pointer.HorizSize, base.Pointer.VertSize);
                r = graphicsd.CalcRect3D(r, base.StartZ);
                graphicsd.Draw(r, this.pointimage, this.transparent);
            }
        }

        public override void PrepareForGallery(bool IsEnabled)
        {
            base.PrepareForGallery(IsEnabled);
            base.chart.Aspect.Orthogonal = true;
        }

        public Image PointImage
        {
            get
            {
                return this.pointimage;
            }
            set
            {
                this.pointimage = value;
                if (this.pointimage != null)
                {
                    if (this.pointimage.Width > 0)
                    {
                        base.Pointer.HorizSize = this.pointimage.Width;
                    }
                    if (this.pointimage.Height > 0)
                    {
                        base.Pointer.VertSize = this.pointimage.Height;
                    }
                }
                base.Repaint();
            }
        }

        [DefaultValue(false)]
        public bool Transparent
        {
            get
            {
                return this.transparent;
            }
            set
            {
                base.SetBooleanProperty(ref this.transparent, value);
            }
        }
    }
}

