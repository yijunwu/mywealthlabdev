namespace Steema.TeeChart.Drawing
{
    using Steema.TeeChart;
    using Steema.TeeChart.Editors;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Design;
    using System.Drawing.Drawing2D;
    using System.Windows.Forms;

    [Serializable, Editor(typeof(ChartPen.PenComponentEditor), typeof(UITypeEditor)), Description("Pen used to draw lines and borders.")]
    public class ChartPen : TeeBase, ICloneable
    {
        protected internal bool bVisible;
        protected System.Drawing.Color cColor;
        internal EventHandler ColorChanged;
        [NonSerialized]
        protected Pen custom;
        private System.Drawing.Drawing2D.DashCap dashCap;
        private float[] dashPattern;
        protected internal System.Drawing.Color defaultColor;
        protected internal LineCap defaultEndCap;
        internal DashStyle defaultStyle;
        protected internal bool defaultVisible;
        private LineCap endCap;
        [NonSerialized]
        protected Pen handle;
        private DashStyle style;
        private int transparency;
        private int width;

        public ChartPen()
        {
            this.defaultVisible = true;
            this.bVisible = true;
            this.width = 1;
        }

        public ChartPen(Chart c) : this(c, Utils.EmptyColor, true)
        {
        }

        public ChartPen(System.Drawing.Color c) : this(null, c)
        {
        }

        public ChartPen(Chart c, bool startVisible) : this(c, Utils.EmptyColor, startVisible)
        {
        }

        public ChartPen(Chart c, System.Drawing.Color startColor) : this(c, startColor, true)
        {
        }

        public ChartPen(Chart c, Pen pen) : base(c)
        {
            this.defaultVisible = true;
            this.bVisible = true;
            this.width = 1;
            this.custom = pen;
        }

        public ChartPen(Chart c, System.Drawing.Color startColor, bool startVisible) : base(c)
        {
            this.defaultVisible = true;
            this.bVisible = true;
            this.width = 1;
            this.defaultColor = startColor;
            this.cColor = this.defaultColor;
            this.defaultVisible = startVisible;
            this.bVisible = this.defaultVisible;
            this.handle = null;
            this.custom = null;
        }

        public ChartPen(Chart c, System.Drawing.Color startColor, bool startVisible, LineCap cap) : this(c, startColor, startVisible)
        {
            this.endCap = cap;
            this.defaultEndCap = cap;
        }

        public void Assign(ChartPen p)
        {
            this.style = p.style;
            this.width = p.width;
            this.cColor = p.cColor;
            this.endCap = p.endCap;
            this.dashCap = p.dashCap;
            this.bVisible = p.bVisible;
            this.transparency = p.transparency;
            this.dashPattern = p.dashPattern;
            this.SetNullHandle();
        }

        public void Assign(ChartPen p, System.Drawing.Color AColor)
        {
            this.style = p.style;
            this.width = p.width;
            this.cColor = AColor;
            this.endCap = p.endCap;
            this.dashCap = p.dashCap;
            this.bVisible = p.bVisible;
            this.transparency = p.transparency;
            this.dashPattern = p.dashPattern;
            this.SetNullHandle();
        }

        protected virtual void AssignPen(ChartPen p)
        {
            p.style = this.style;
            p.width = this.width;
            p.cColor = this.cColor;
            p.endCap = this.endCap;
            p.dashCap = this.dashCap;
            p.bVisible = this.bVisible;
            p.transparency = this.transparency;
            p.dashPattern = this.dashPattern;
            p.SetNullHandle();
        }

        public object Clone()
        {
            ChartPen p = CreateNewChartPen(base.Chart, base.GetType());
            this.AssignPen(p);
            return p;
        }

        public static ChartPen CreateNewChartPen(Chart chart, System.Type type)
        {
            ChartPen pen = NewFromType(type);
            pen.Chart = chart;
            return pen;
        }

        private System.Drawing.Color GetColor()
        {
            if (this.bVisible)
            {
                if (this.transparency == 0)
                {
                    return this.cColor;
                }
                return Graphics3D.TransparentColor(this.transparency, this.cColor);
            }
            return System.Drawing.Color.Transparent;
        }

        public override void Invalidate()
        {
            this.SetNullHandle();
            base.Invalidate();
        }

        public static ChartPen NewFromType(System.Type type)
        {
            return (ChartPen) Activator.CreateInstance(type);
        }

        private void SetDrawingPen()
        {
            this.handle = new Pen(this.GetColor());
            this.handle.DashStyle = this.style;
            if ((this.style == DashStyle.Custom) && (this.dashPattern != null))
            {
                this.handle.DashPattern = this.dashPattern;
            }
            this.handle.Width = this.width;
            this.handle.EndCap = this.endCap;
            this.handle.DashCap = this.dashCap;
        }

        private void SetNullHandle()
        {
            this.handle = null;
            this.custom = null;
            if ((base.chart != null) && (this == base.chart.graphics3D.Pen))
            {
                base.chart.graphics3D.Changed(base.chart.graphics3D.Pen);
            }
        }

        protected virtual bool ShouldSerializeColor()
        {
            return (this.cColor.ToArgb() != this.defaultColor.ToArgb());
        }

        protected bool ShouldSerializeDashPattern()
        {
            return (this.style == DashStyle.Custom);
        }

        protected bool ShouldSerializeEndCap()
        {
            return (this.endCap != this.defaultEndCap);
        }

        protected virtual bool ShouldSerializeStyle()
        {
            return (this.style != this.defaultStyle);
        }

        protected virtual bool ShouldSerializeVisible()
        {
            return (this.bVisible != this.defaultVisible);
        }

        [Description("Determines the color used by the pen to draw lines on the Drawing.")]
        public System.Drawing.Color Color
        {
            get
            {
                return this.cColor;
            }
            set
            {
                base.SetColorProperty(ref this.cColor, value);
            }
        }

        [DefaultValue(0), Description("Defines segment ending style of dashed lines.")]
        public System.Drawing.Drawing2D.DashCap DashCap
        {
            get
            {
                return this.dashCap;
            }
            set
            {
                if (this.dashCap != value)
                {
                    this.dashCap = value;
                    this.Invalidate();
                }
            }
        }

        public float[] DashPattern
        {
            get
            {
                return this.dashPattern;
            }
            set
            {
                if (this.dashPattern != value)
                {
                    this.dashPattern = value;
                    this.Invalidate();
                }
            }
        }

        [Description("Accesses the internal Drawing.Pen object."), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public Pen DrawingPen
        {
            get
            {
                if (this.custom == null)
                {
                    this.SetDrawingPen();
                    return this.handle;
                }
                return this.custom;
            }
        }

        [Description("Style of line endings.")]
        public LineCap EndCap
        {
            get
            {
                return this.endCap;
            }
            set
            {
                if (this.endCap != value)
                {
                    this.endCap = value;
                    this.Invalidate();
                }
            }
        }

        [Description("Determines the style in which the pen draw lines on the Drawing.")]
        public virtual DashStyle Style
        {
            get
            {
                return this.style;
            }
            set
            {
                if (this.style != value)
                {
                    this.style = value;
                    this.Invalidate();
                }
            }
        }

        [DefaultValue(0), Description("Sets Transparency level from 0 to 100%.")]
        public int Transparency
        {
            get
            {
                return this.transparency;
            }
            set
            {
                base.SetIntegerProperty(ref this.transparency, value);
            }
        }

        [Description("Determines if the pen will draw lines or not.")]
        public bool Visible
        {
            get
            {
                return this.bVisible;
            }
            set
            {
                base.SetBooleanProperty(ref this.bVisible, value);
            }
        }

        [Description("Determines the width of lines the pen draws."), DefaultValue(1)]
        public virtual int Width
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

        internal class PenComponentEditor : UITypeEditor
        {
            public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
            {
                using (PenEditor editor = new PenEditor((ChartPen) value))
                {
                    bool flag = editor.ShowDialog() == DialogResult.OK;
                    if ((context != null) && flag)
                    {
                        context.OnComponentChanged();
                    }
                    return flag;
                }
            }

            public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
            {
                return UITypeEditorEditStyle.Modal;
            }

            public override bool GetPaintValueSupported(ITypeDescriptorContext context)
            {
                return true;
            }

            public override void PaintValue(PaintValueEventArgs e)
            {
                base.PaintValue(e);
                int num = e.Bounds.Top + (e.Bounds.Height / 2);
                ChartPen pen = (ChartPen) e.Value;
                e.Graphics.DrawLine(pen.DrawingPen, e.Bounds.X, num, e.Bounds.Right - 1, num);
            }
        }
    }
}

