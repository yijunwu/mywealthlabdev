namespace Steema.TeeChart.Drawing
{
    using Steema.TeeChart;
    using Steema.TeeChart.Editors;
    using Steema.TeeChart.Themes;
    using System;
    using System.ComponentModel;
    using System.Drawing.Design;
    using System.Drawing.Drawing2D;
    using System.Drawing.Text;
    using System.Runtime.CompilerServices;

    [Editor(typeof(Aspect.AspectComponentEditor), typeof(UITypeEditor)), Description("Properties to define Chart 3D appearance.")]
    public sealed class Aspect : TeeBase
    {
        internal bool applyZOrder;
        private int chart3D;
        internal bool clipPoints;
        internal int colorPaletteIdx;
        private double elevation;
        internal TTeeView3DScrolled FOnScrolled;
        internal double FZOffset;
        public int Height3D;
        private double horizOffset;
        private int orthoAngle;
        internal bool orthogonal;
        private int perspective;
        internal double rotation;
        private int themeIndex;
        private int tilt;
        private double vertOffset;
        internal bool view3D;
        public int Width3D;
        private double zoom;
        private bool zoomText;

        public Aspect() : this(null)
        {
        }

        public Aspect(Chart c) : base(c)
        {
            this.applyZOrder = true;
            this.chart3D = 15;
            this.clipPoints = true;
            this.elevation = 345.0;
            this.orthoAngle = 0x2d;
            this.orthogonal = true;
            this.perspective = 15;
            this.rotation = 345.0;
            this.view3D = true;
            this.zoom = 100.0;
            this.zoomText = true;
            this.colorPaletteIdx = 13;
        }

        public void Assign(Aspect a)
        {
            this.applyZOrder = a.applyZOrder;
            this.chart3D = a.chart3D;
            this.clipPoints = a.clipPoints;
            this.elevation = a.elevation;
            this.horizOffset = a.horizOffset;
            this.orthoAngle = a.orthoAngle;
            this.orthogonal = a.orthogonal;
            this.perspective = a.perspective;
            this.rotation = a.rotation;
            this.tilt = a.tilt;
            this.vertOffset = a.vertOffset;
            this.view3D = a.view3D;
            this.zoom = a.zoom;
            this.zoomText = a.zoomText;
        }

        internal void SetZOffset(double Value)
        {
            if (this.FZOffset != Value)
            {
                this.FZOffset = Value;
                this.Invalidate();
                if (this.FOnScrolled != null)
                {
                    this.FOnScrolled(false);
                }
            }
        }

        [DefaultValue(true), Description("When True, multiple Series are displayed at a different 3D 'Z' (depth) position.")]
        public bool ApplyZOrder
        {
            get
            {
                return this.applyZOrder;
            }
            set
            {
                base.SetBooleanProperty(ref this.applyZOrder, value);
            }
        }

        [DefaultValue(15), Description("Percent from 0 to 100 of Z Depth.")]
        public int Chart3DPercent
        {
            get
            {
                return this.chart3D;
            }
            set
            {
                base.SetIntegerProperty(ref this.chart3D, value);
            }
        }

        [Description("When True, restricts Series points to display outside the Chart axes rectangle."), DefaultValue(true)]
        public bool ClipPoints
        {
            get
            {
                return this.clipPoints;
            }
            set
            {
                base.SetBooleanProperty(ref this.clipPoints, value);
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false), DefaultValue(13)]
        public int ColorPaletteIndex
        {
            get
            {
                return this.colorPaletteIdx;
            }
            set
            {
                base.SetIntegerProperty(ref this.colorPaletteIdx, value);
                if (this.colorPaletteIdx != -1)
                {
                    ColorPalettes.ApplyPalette(base.chart, this.colorPaletteIdx);
                }
            }
        }

        [Description("Gets or sets the angle in degrees of 3D elevation."), DefaultValue(0x159)]
        public int Elevation
        {
            get
            {
                return Convert.ToInt32(this.elevation);
            }
            set
            {
                base.SetDoubleProperty(ref this.elevation, Convert.ToDouble(value));
            }
        }

        [DefaultValue((double) 345.0), Description("Gets or sets the angle, as Float, in degrees of 3D elevation.")]
        public double ElevationFloat
        {
            get
            {
                return this.elevation;
            }
            set
            {
                base.SetDoubleProperty(ref this.elevation, value);
            }
        }

        [Description("Amount (postive or negative) in pixels of horizontal displacement."), DefaultValue(0)]
        public int HorizOffset
        {
            get
            {
                return Utils.Round(this.horizOffset);
            }
            set
            {
                base.SetDoubleProperty(ref this.horizOffset, (double) value);
            }
        }

        [Description("Amount (postive or negative) in pixels of horizontal displacement."), DefaultValue((double) 0.0)]
        public double HorizOffsetFloat
        {
            get
            {
                return this.horizOffset;
            }
            set
            {
                base.SetDoubleProperty(ref this.horizOffset, value);
            }
        }

        [DefaultValue(0x2d), Description("Angle in degrees, from 0 to 90, when displaying in Orthogonal mode.")]
        public int OrthoAngle
        {
            get
            {
                return this.orthoAngle;
            }
            set
            {
                base.SetIntegerProperty(ref this.orthoAngle, value);
            }
        }

        [DefaultValue(true), Description("When True, display in semi-3D mode.")]
        public bool Orthogonal
        {
            get
            {
                return this.orthogonal;
            }
            set
            {
                base.SetBooleanProperty(ref this.orthogonal, value);
            }
        }

        [DefaultValue(15), Description("Percent of 3D perspective.")]
        public int Perspective
        {
            get
            {
                return this.perspective;
            }
            set
            {
                base.SetIntegerProperty(ref this.perspective, value);
            }
        }

        [Description("Gets or sets the angle in degrees of 3D rotation."), DefaultValue(0x159)]
        public int Rotation
        {
            get
            {
                return Convert.ToInt32(this.rotation);
            }
            set
            {
                value = base.Chart.Graphics3D.CorrectAngle(value);
                base.SetDoubleProperty(ref this.rotation, Convert.ToDouble(value));
            }
        }

        [Description("Gets or sets the angle, as Float, in degrees of 3D rotation."), DefaultValue((double) 345.0)]
        public double RotationFloat
        {
            get
            {
                return this.rotation;
            }
            set
            {
                value = base.Chart.Graphics3D.CorrectAngle(value);
                base.SetDoubleProperty(ref this.rotation, Convert.ToDouble(value));
            }
        }

        [DefaultValue(2), Description("Chooses between speed or display quality.")]
        public System.Drawing.Drawing2D.SmoothingMode SmoothingMode
        {
            get
            {
                return base.chart.graphics3D.SmoothingMode;
            }
            set
            {
                base.chart.graphics3D.SmoothingMode = value;
            }
        }

        [DefaultValue(5)]
        public System.Drawing.Text.TextRenderingHint TextRenderingHint
        {
            get
            {
                return base.chart.graphics3D.TextRenderingHint;
            }
            set
            {
                base.chart.graphics3D.TextRenderingHint = value;
            }
        }

        [Browsable(false), DefaultValue(0), EditorBrowsable(EditorBrowsableState.Never)]
        public int ThemeIndex
        {
            get
            {
                return this.themeIndex;
            }
            set
            {
                base.SetIntegerProperty(ref this.themeIndex, value);
            }
        }

        [DefaultValue(0), Description("Gets or sets the angle in degrees of 3D tilt.")]
        public int Tilt
        {
            get
            {
                return this.tilt;
            }
            set
            {
                base.SetIntegerProperty(ref this.tilt, value);
            }
        }

        [DefaultValue(0), Description("Amount (postive or negative) in pixels of vertical displacement.")]
        public int VertOffset
        {
            get
            {
                return Utils.Round(this.vertOffset);
            }
            set
            {
                base.SetDoubleProperty(ref this.vertOffset, (double) value);
            }
        }

        [Description("Amount (postive or negative) in pixels of vertical displacement."), DefaultValue((double) 0.0)]
        public double VertOffsetFloat
        {
            get
            {
                return this.vertOffset;
            }
            set
            {
                base.SetDoubleProperty(ref this.vertOffset, value);
            }
        }

        [DefaultValue(true), Description("Gets or sets 3D display mode.")]
        public bool View3D
        {
            get
            {
                return this.view3D;
            }
            set
            {
                base.SetBooleanProperty(ref this.view3D, value);
                base.chart.BroadcastEvent(new View3DEvent());
            }
        }

        [Browsable(false)]
        public double ZOffset
        {
            get
            {
                return this.FZOffset;
            }
            set
            {
                this.SetZOffset(value);
            }
        }

        [Description("Percent of zoom in 3D mode."), DefaultValue(100)]
        public int Zoom
        {
            get
            {
                return Utils.Round(this.zoom);
            }
            set
            {
                base.SetDoubleProperty(ref this.zoom, (double) value);
            }
        }

        [Description("Percent of zoom, as Float, in 3D mode."), DefaultValue((double) 100.0)]
        public double ZoomFloat
        {
            get
            {
                return this.zoom;
            }
            set
            {
                base.SetDoubleProperty(ref this.zoom, value);
            }
        }

        [DefaultValue(true), Description("When True, all texts are resized according to Zoom property.")]
        public bool ZoomText
        {
            get
            {
                return this.zoomText;
            }
            set
            {
                base.SetBooleanProperty(ref this.zoomText, value);
            }
        }

        internal class AspectComponentEditor : UITypeEditor
        {
            public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
            {
                Aspect aspect = (Aspect) value;
                bool flag = EditorUtils.ShowFormModal(new AspectEditor(aspect.Chart, null));
                if ((context != null) && flag)
                {
                    context.OnComponentChanged();
                }
                return value;
            }

            public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
            {
                return UITypeEditorEditStyle.Modal;
            }
        }

        public delegate void TTeeView3DScrolled(bool IsHoriz);
    }
}

