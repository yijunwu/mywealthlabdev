namespace Steema.TeeChart.Drawing
{
    using Steema.TeeChart;
    using System;
    using System.ComponentModel;
    using System.Drawing;

    public sealed class Bevel : TeeBase, ICloneable
    {
        private Color colorOne;
        private Color colorTwo;
        internal BevelStyles defaultOuter;
        internal BevelStyles inner;
        internal BevelStyles outer;
        private int width;

        public Bevel(Chart c) : base(c)
        {
            this.width = 1;
            this.colorOne = Color.White;
            this.colorTwo = Color.Gray;
        }

        [Obsolete("Please use the Bevel.Clone() method"), Description("Assign all properties from a bevel to another.")]
        public void Assign(Bevel b)
        {
            if (b != null)
            {
                this.colorOne = b.colorOne;
                this.colorTwo = b.colorTwo;
                this.inner = b.inner;
                this.outer = b.outer;
                this.width = b.width;
            }
        }

        private void AssignBevel(Bevel b)
        {
            if (b != null)
            {
                b.colorOne = this.colorOne;
                b.colorTwo = this.colorTwo;
                b.inner = this.inner;
                b.outer = this.outer;
                b.width = this.width;
            }
        }

        public object Clone()
        {
            Bevel b = new Bevel(base.Chart);
            this.AssignBevel(b);
            return b;
        }

        public void Draw(Graphics3D g, Rectangle rect)
        {
            if (this.inner != BevelStyles.None)
            {
                g.PaintBevel(this.inner, rect, this.width, this.colorOne, this.colorTwo);
                rect.Inflate(-this.width, -this.width);
            }
            if (this.outer != BevelStyles.None)
            {
                g.PaintBevel(this.outer, rect, this.width, this.colorOne, this.colorTwo);
            }
        }

        private bool ShouldSerializeOuter()
        {
            return (this.outer != this.defaultOuter);
        }

        [Description("Color of left and top sides of bevels."), DefaultValue(typeof(Color), "White")]
        public Color ColorOne
        {
            get
            {
                return this.colorOne;
            }
            set
            {
                base.SetColorProperty(ref this.colorOne, value);
            }
        }

        [DefaultValue(typeof(Color), "Gray"), Description("Color of right and bottom sides of bevels.")]
        public Color ColorTwo
        {
            get
            {
                return this.colorTwo;
            }
            set
            {
                base.SetColorProperty(ref this.colorTwo, value);
            }
        }

        [DefaultValue(0), Description("Style for inner bevel.")]
        public BevelStyles Inner
        {
            get
            {
                return this.inner;
            }
            set
            {
                if (this.inner != value)
                {
                    this.inner = value;
                    this.Invalidate();
                }
            }
        }

        [Description("Style for outer bevel.")]
        public BevelStyles Outer
        {
            get
            {
                return this.outer;
            }
            set
            {
                if (this.outer != value)
                {
                    this.outer = value;
                    this.Invalidate();
                }
            }
        }

        [DefaultValue(1), Description("Size in pixels for bevels.")]
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

