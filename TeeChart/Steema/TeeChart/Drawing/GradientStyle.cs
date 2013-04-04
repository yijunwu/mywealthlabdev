namespace Steema.TeeChart.Drawing
{
    using Steema.TeeChart;
    using System;
    using System.ComponentModel;

    public class GradientStyle : TeeBase, ICloneable
    {
        private int centerXOffset;
        private int centerYOffset;
        private PathGradientMode direction;
        private bool visible;

        public GradientStyle() : this(null)
        {
        }

        public GradientStyle(Chart c) : base(c)
        {
            this.direction = PathGradientMode.FromCenter;
            this.visible = false;
            this.centerXOffset = 0;
            this.centerYOffset = 0;
        }

        private void Assign(GradientStyle value)
        {
            value.centerXOffset = this.centerXOffset;
            value.centerYOffset = this.centerYOffset;
            value.direction = this.direction;
            value.visible = this.visible;
        }

        public object Clone()
        {
            GradientStyle style = new GradientStyle(base.Chart);
            this.Assign(style);
            return style;
        }

        [DefaultValue(0), Description("The X pixel offset from the center of the non-linear gradient.")]
        public int CenterXOffset
        {
            get
            {
                return this.centerXOffset;
            }
            set
            {
                if (this.centerXOffset != value)
                {
                    this.centerXOffset = value;
                    this.Invalidate();
                }
            }
        }

        [Description("The Y pixel offset from the center of the non-linear gradient."), DefaultValue(0)]
        public int CenterYOffset
        {
            get
            {
                return this.centerYOffset;
            }
            set
            {
                if (this.centerYOffset != value)
                {
                    this.centerYOffset = value;
                    this.Invalidate();
                }
            }
        }

        [DefaultValue(0), Description("Specifies the direction the non-linear gradient fill will be applied.")]
        public PathGradientMode Direction
        {
            get
            {
                return this.direction;
            }
            set
            {
                if (this.direction != value)
                {
                    this.direction = value;
                    this.Invalidate();
                }
            }
        }

        [Description("When true, applies the specified non-linear gradient style."), DefaultValue(false)]
        public bool Visible
        {
            get
            {
                return this.visible;
            }
            set
            {
                if (this.visible != value)
                {
                    this.visible = value;
                    this.Invalidate();
                }
            }
        }
    }
}

