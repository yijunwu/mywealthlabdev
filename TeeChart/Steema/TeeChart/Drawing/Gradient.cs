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

    [Description("Properties to configure a gradient filling."), Editor(typeof(Gradient.GradientComponentEditor), typeof(UITypeEditor))]
    public class Gradient : TeeBase, ICloneable
    {
        private double angle;
        private Point[] customTargetPolygon;
        private Rectangle customTargetRectangle;
        internal LinearGradientMode defaultDirection;
        internal Color defaultEndColor;
        internal Color defaultMiddleColor;
        internal Color defaultStartColor;
        internal bool defaultUseMiddle;
        internal bool defaultVisible;
        private LinearGradientMode direction;
        private Color endColor;
        private Color[] extendedColorPalette;
        private bool gammaCorrection;
        private Color middleColor;
        private bool sigma;
        private float sigmaFocus;
        private float sigmaScale;
        private Color startColor;
        private GradientStyle style;
        private int transparency;
        private bool useMiddle;
        internal bool visible;
        private System.Drawing.Drawing2D.WrapMode wrapMode;

        public Gradient() : this(null)
        {
        }

        public Gradient(Chart c) : base(c)
        {
            this.direction = LinearGradientMode.Vertical;
            this.startColor = Color.Gold;
            this.middleColor = Color.Gray;
            this.endColor = Color.White;
            this.sigmaFocus = 0.5f;
            this.sigmaScale = 1f;
            this.wrapMode = System.Drawing.Drawing2D.WrapMode.Clamp;
            this.customTargetRectangle = Rectangle.Empty;
            this.defaultDirection = LinearGradientMode.Vertical;
            this.defaultEndColor = Color.White;
            this.defaultMiddleColor = Color.Gray;
            this.defaultStartColor = Color.Gold;
            this.useMiddle = false;
            this.visible = false;
        }

        [Obsolete("Please use the Gradient.Clone() method")]
        public void Assign(Gradient value)
        {
            this.angle = value.angle;
            this.sigmaFocus = value.sigmaFocus;
            this.sigmaScale = value.sigmaScale;
            this.sigma = value.sigma;
            this.gammaCorrection = value.gammaCorrection;
            this.customTargetRectangle = value.customTargetRectangle;
            this.extendedColorPalette = value.extendedColorPalette;
            this.direction = value.direction;
            this.startColor = value.startColor;
            this.middleColor = value.middleColor;
            this.endColor = value.endColor;
            this.wrapMode = value.wrapMode;
            this.useMiddle = value.useMiddle;
            this.visible = value.visible;
            this.transparency = value.transparency;
            this.defaultVisible = value.defaultVisible;
        }

        private void AssignGradient(Gradient value)
        {
            value.direction = this.direction;
            value.startColor = this.startColor;
            value.middleColor = this.middleColor;
            value.endColor = this.endColor;
            value.angle = this.angle;
            value.sigmaFocus = this.sigmaFocus;
            value.sigmaScale = this.sigmaScale;
            value.sigma = this.sigma;
            value.gammaCorrection = this.gammaCorrection;
            if (this.style != null)
            {
                value.style = this.style.Clone() as GradientStyle;
            }
            value.wrapMode = this.wrapMode;
            value.useMiddle = this.useMiddle;
            value.visible = this.visible;
            value.transparency = this.transparency;
            value.defaultVisible = this.defaultVisible;
        }

        public object Clone()
        {
            Gradient gradient = new Gradient(base.Chart);
            this.AssignGradient(gradient);
            return gradient;
        }

        private Color CorrectColor(Color value)
        {
            if (this.transparency == 0)
            {
                return value;
            }
            return Graphics3D.TransparentColor(this.transparency, value);
        }

        private Color[] CorrectColors(Color[] value)
        {
            Color[] colorArray = new Color[value.Length];
            for (int i = 0; i < value.Length; i++)
            {
                colorArray[i] = this.CorrectColor(value[i]);
            }
            return colorArray;
        }

        public void Draw(Graphics3D g, params Point[] p)
        {
            g.Polygon(p);
            this.customTargetRectangle = Rectangle.Empty;
            this.extendedColorPalette = null;
            this.customTargetPolygon = null;
        }

        public void Draw(Graphics3D g, Rectangle rect)
        {
            g.Rectangle(rect);
            this.customTargetRectangle = Rectangle.Empty;
            this.extendedColorPalette = null;
            this.customTargetPolygon = null;
        }

        public void Draw(Graphics3D g, int left, int top, int right, int bottom)
        {
            this.Draw(g, Utils.FromLTRB(left, top, right, bottom));
        }

        public Brush DrawingBrush(RectangleF rect)
        {
            return this.DrawingBrush(Rectangle.Round(rect));
        }

        public Brush DrawingBrush(params PointDouble[] p)
        {
            return this.DrawingBrush(PointDouble.RoundF(p));
        }

        public Brush DrawingBrush(params Point[] p)
        {
            PathGradientBrush brush;
            if ((this.customTargetPolygon != null) && (this.customTargetPolygon.Length > 0))
            {
                brush = new PathGradientBrush(this.customTargetPolygon, this.wrapMode);
            }
            else
            {
                brush = new PathGradientBrush(p, this.wrapMode);
            }
            if (!this.Style.Visible)
            {
                if (this.useMiddle && !Utils.ColorIsEmpty(this.middleColor))
                {
                    brush.InterpolationColors = this.GetColorBlend(true);
                }
                else
                {
                    brush.InterpolationColors = this.GetColorBlend(false);
                }
            }
            else
            {
                switch (this.Style.Direction)
                {
                    case PathGradientMode.FromCenter:
                        if (!this.useMiddle || Utils.ColorIsEmpty(this.middleColor))
                        {
                            brush.InterpolationColors = this.GetColorBlend(false);
                            break;
                        }
                        brush.InterpolationColors = this.GetColorBlend(true);
                        break;

                    case PathGradientMode.Radial:
                        if (this.extendedColorPalette == null)
                        {
                            if (this.useMiddle && !this.middleColor.IsEmpty)
                            {
                                brush.CenterColor = this.CorrectColor(this.middleColor);
                                brush.SurroundColors = new Color[] { this.CorrectColor(this.startColor), this.CorrectColor(this.endColor) };
                            }
                            else
                            {
                                brush.CenterColor = this.CorrectColor(this.endColor);
                                brush.SurroundColors = new Color[] { this.CorrectColor(this.startColor) };
                            }
                            break;
                        }
                        brush.CenterColor = this.CorrectColor(this.extendedColorPalette[0]);
                        brush.SurroundColors = this.CorrectColors(this.extendedColorPalette);
                        break;
                }
                PointF centerPoint = brush.CenterPoint;
                centerPoint.X += this.Style.CenterXOffset;
                centerPoint.Y += this.Style.CenterYOffset;
                brush.CenterPoint = centerPoint;
            }
            if (this.sigma)
            {
                if (!this.Style.Visible)
                {
                    brush.SetSigmaBellShape(this.sigmaFocus, this.sigmaScale);
                    return brush;
                }
                switch (this.Style.Direction)
                {
                    case PathGradientMode.FromCenter:
                        brush.FocusScales = new PointF(this.sigmaFocus, this.sigmaScale);
                        return brush;

                    case PathGradientMode.Radial:
                        if (!this.useMiddle || this.middleColor.IsEmpty)
                        {
                            brush.FocusScales = new PointF(this.sigmaFocus, this.sigmaScale);
                            return brush;
                        }
                        brush.SetBlendTriangularShape(this.sigmaFocus, this.sigmaScale);
                        return brush;
                }
            }
            return brush;
        }

        public Brush DrawingBrush(params PointF[] p)
        {
            PathGradientBrush brush;
            if ((this.customTargetPolygon != null) && (this.customTargetPolygon.Length > 0))
            {
                brush = new PathGradientBrush(this.customTargetPolygon, this.wrapMode);
            }
            else
            {
                brush = new PathGradientBrush(p, this.wrapMode);
            }
            if (!this.Style.Visible)
            {
                if (this.useMiddle && !this.middleColor.IsEmpty)
                {
                    brush.InterpolationColors = this.GetColorBlend(true);
                }
                else
                {
                    brush.InterpolationColors = this.GetColorBlend(false);
                }
            }
            else
            {
                switch (this.Style.Direction)
                {
                    case PathGradientMode.FromCenter:
                        if (!this.useMiddle || this.middleColor.IsEmpty)
                        {
                            brush.InterpolationColors = this.GetColorBlend(false);
                            break;
                        }
                        brush.InterpolationColors = this.GetColorBlend(true);
                        break;

                    case PathGradientMode.Radial:
                        if (this.extendedColorPalette == null)
                        {
                            if (this.useMiddle && !this.middleColor.IsEmpty)
                            {
                                brush.CenterColor = this.CorrectColor(this.middleColor);
                                brush.SurroundColors = new Color[] { this.CorrectColor(this.startColor), this.CorrectColor(this.endColor) };
                            }
                            else
                            {
                                brush.CenterColor = this.CorrectColor(this.endColor);
                                brush.SurroundColors = new Color[] { this.CorrectColor(this.startColor) };
                            }
                            break;
                        }
                        brush.CenterColor = this.CorrectColor(this.extendedColorPalette[0]);
                        brush.SurroundColors = this.CorrectColors(this.extendedColorPalette);
                        break;
                }
                PointF centerPoint = brush.CenterPoint;
                centerPoint.X += this.Style.CenterXOffset;
                centerPoint.Y += this.Style.CenterYOffset;
                brush.CenterPoint = centerPoint;
            }
            if (this.sigma)
            {
                if (!this.Style.Visible)
                {
                    brush.SetSigmaBellShape(this.sigmaFocus, this.sigmaScale);
                    return brush;
                }
                switch (this.Style.Direction)
                {
                    case PathGradientMode.FromCenter:
                        brush.FocusScales = new PointF(this.sigmaFocus, this.sigmaScale);
                        return brush;

                    case PathGradientMode.Radial:
                        if (!this.useMiddle || this.middleColor.IsEmpty)
                        {
                            brush.FocusScales = new PointF(this.sigmaFocus, this.sigmaScale);
                            return brush;
                        }
                        brush.SetBlendTriangularShape(this.sigmaFocus, this.sigmaScale);
                        return brush;
                }
            }
            return brush;
        }

        public Brush DrawingBrush(Rectangle rect)
        {
            if (rect.Width < 1)
            {
                rect.Width = 1;
            }
            if (rect.Height < 1)
            {
                rect.Height = 1;
            }
            if (!this.Style.Visible)
            {
                LinearGradientBrush brush;
                if (this.angle == 0.0)
                {
                    if (this.CustomTargetRectangle == Rectangle.Empty)
                    {
                        brush = new LinearGradientBrush(rect, this.CorrectColor(this.startColor), this.CorrectColor(this.endColor), this.direction);
                    }
                    else
                    {
                        brush = new LinearGradientBrush(this.CustomTargetRectangle, this.CorrectColor(this.startColor), this.CorrectColor(this.endColor), this.direction);
                    }
                }
                else
                {
                    float angle = Convert.ToSingle(this.angle);
                    if (this.CustomTargetRectangle == Rectangle.Empty)
                    {
                        brush = new LinearGradientBrush(rect, this.CorrectColor(this.startColor), this.CorrectColor(this.endColor), angle);
                    }
                    else
                    {
                        brush = new LinearGradientBrush(this.CustomTargetRectangle, this.CorrectColor(this.startColor), this.CorrectColor(this.endColor), angle);
                    }
                }
                if ((this.useMiddle && !this.middleColor.IsEmpty) || (this.extendedColorPalette != null))
                {
                    brush.InterpolationColors = this.GetColorBlend(true);
                }
                if (this.sigma)
                {
                    brush.SetSigmaBellShape(this.sigmaFocus, this.sigmaScale);
                }
                brush.GammaCorrection = this.gammaCorrection;
                return brush;
            }
            if (this.CustomTargetRectangle == Rectangle.Empty)
            {
                Point[] pointArray = new Point[] { new Point(rect.Left, rect.Top), new Point(rect.Right, rect.Top), new Point(rect.Right, rect.Bottom), new Point(rect.Left, rect.Bottom) };
                return this.DrawingBrush(pointArray);
            }
            Point[] p = new Point[] { new Point(this.CustomTargetRectangle.Left, this.CustomTargetRectangle.Top), new Point(this.CustomTargetRectangle.Right, this.CustomTargetRectangle.Top), new Point(this.CustomTargetRectangle.Right, this.CustomTargetRectangle.Bottom), new Point(this.CustomTargetRectangle.Left, this.CustomTargetRectangle.Bottom) };
            return this.DrawingBrush(p);
        }

        public Brush DrawingBrush(Size size)
        {
            return this.DrawingBrush(new Rectangle(new Point(0, 0), size));
        }

        private ColorBlend GetColorBlend(bool hasMiddle)
        {
            if (this.extendedColorPalette != null)
            {
                ColorBlend blend = new ColorBlend(this.extendedColorPalette.Length);
                float[] numArray = new float[this.extendedColorPalette.Length];
                for (int i = 0; i < this.extendedColorPalette.Length; i++)
                {
                    numArray[i] = ((float) i) / ((float) (this.extendedColorPalette.Length - 1));
                }
                blend.Colors = this.CorrectColors(this.extendedColorPalette);
                blend.Positions = numArray;
                return blend;
            }
            if (hasMiddle)
            {
                ColorBlend blend2 = new ColorBlend(3);
                blend2.Colors[0] = this.CorrectColor(this.startColor);
                blend2.Colors[1] = this.CorrectColor(this.middleColor);
                blend2.Colors[2] = this.CorrectColor(this.endColor);
                blend2.Positions[0] = 0f;
                blend2.Positions[1] = 0.5f;
                blend2.Positions[2] = 1f;
                return blend2;
            }
            ColorBlend blend3 = new ColorBlend(2);
            blend3.Colors[0] = this.CorrectColor(this.startColor);
            blend3.Colors[1] = this.CorrectColor(this.endColor);
            blend3.Positions[0] = 0f;
            blend3.Positions[1] = 1f;
            return blend3;
        }

        protected virtual bool ShouldSerializeCustomTargetPolygon()
        {
            return ((this.customTargetPolygon != null) && (this.customTargetPolygon.Length > 0));
        }

        protected virtual bool ShouldSerializeCustomTargetRectangle()
        {
            return (this.customTargetRectangle != Rectangle.Empty);
        }

        protected virtual bool ShouldSerializeDirection()
        {
            return (this.direction != this.defaultDirection);
        }

        protected virtual bool ShouldSerializeEndColor()
        {
            return (this.endColor != this.defaultEndColor);
        }

        protected virtual bool ShouldSerializeExtendedColorPalette()
        {
            return (this.extendedColorPalette != null);
        }

        protected virtual bool ShouldSerializeMiddleColor()
        {
            return (this.middleColor != this.defaultMiddleColor);
        }

        protected virtual bool ShouldSerializeStartColor()
        {
            return (this.startColor != this.defaultStartColor);
        }

        protected virtual bool ShouldSerializeUseMiddle()
        {
            return (this.useMiddle != this.defaultUseMiddle);
        }

        protected virtual bool ShouldSerializeVisible()
        {
            return (this.visible != this.defaultVisible);
        }

        [DefaultValue((double) 0.0), Description("Angle of gradient filling. When zero, uses Direction property.")]
        public double Angle
        {
            get
            {
                return this.angle;
            }
            set
            {
                base.SetDoubleProperty(ref this.angle, value);
            }
        }

        [Description("Assign an array of points to this property to define a custom polygon to a Gradient.")]
        public Point[] CustomTargetPolygon
        {
            get
            {
                return this.customTargetPolygon;
            }
            set
            {
                this.customTargetPolygon = value;
            }
        }

        [Description("Assign a rectangle to this property to define a custom rectangle to a Gradient.")]
        public Rectangle CustomTargetRectangle
        {
            get
            {
                if (this.customTargetRectangle != Rectangle.Empty)
                {
                    if (this.customTargetRectangle.Width == 0)
                    {
                        this.customTargetRectangle.Width = 1;
                    }
                    if (this.customTargetRectangle.Height == 0)
                    {
                        this.customTargetRectangle.Height = 1;
                    }
                }
                return this.customTargetRectangle;
            }
            set
            {
                this.customTargetRectangle = value;
            }
        }

        [Description("Specifies the direction the gradient fill will be applied.")]
        public LinearGradientMode Direction
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

        [Description("One of the three Colors used to create the gradient fill.")]
        public Color EndColor
        {
            get
            {
                return this.endColor;
            }
            set
            {
                base.SetColorProperty(ref this.endColor, value);
            }
        }

        [Description("Assign a color array to this property to use more than three colors (end, start and middle) in a gradient.")]
        public Color[] ExtendedColorPalette
        {
            get
            {
                return this.extendedColorPalette;
            }
            set
            {
                this.extendedColorPalette = value;
            }
        }

        [Description("Enables fine-tuning of displayed Colors."), DefaultValue(false)]
        public bool GammaCorrection
        {
            get
            {
                return this.gammaCorrection;
            }
            set
            {
                base.SetBooleanProperty(ref this.gammaCorrection, value);
            }
        }

        [Description("One of the three Colors used to create the gradient fill.")]
        public Color MiddleColor
        {
            get
            {
                return this.middleColor;
            }
            set
            {
                if (this.middleColor != value)
                {
                    this.middleColor = value;
                    this.useMiddle = !Utils.ColorIsEmpty(this.middleColor);
                    this.Invalidate();
                }
            }
        }

        [Description("Enables use of SigmaFocus and SigmaScale."), DefaultValue(false)]
        public bool Sigma
        {
            get
            {
                return this.sigma;
            }
            set
            {
                base.SetBooleanProperty(ref this.sigma, value);
            }
        }

        [Description("Ratio between Start and End Colors."), DefaultValue((float) 0.5f)]
        public float SigmaFocus
        {
            get
            {
                return this.sigmaFocus;
            }
            set
            {
                if (this.sigmaFocus != value)
                {
                    this.sigmaFocus = value;
                    this.Invalidate();
                }
            }
        }

        [DefaultValue((float) 1f), Description("Ratio between Colors, from 0 to 1.")]
        public float SigmaScale
        {
            get
            {
                return this.sigmaScale;
            }
            set
            {
                if (this.sigmaScale != value)
                {
                    this.sigmaScale = value;
                    this.Invalidate();
                }
            }
        }

        [Description("One of the three Colors used to create the gradient fill.")]
        public Color StartColor
        {
            get
            {
                return this.startColor;
            }
            set
            {
                base.SetColorProperty(ref this.startColor, value);
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Specifies the style of the gradient fill.")]
        public GradientStyle Style
        {
            get
            {
                if (this.style == null)
                {
                    this.style = new GradientStyle(base.Chart);
                }
                return this.style;
            }
            set
            {
                this.style = value;
                this.Invalidate();
            }
        }

        [Description("Percentage of transparency."), DefaultValue(0)]
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

        [Description("Uses MiddleColor or not.")]
        public bool UseMiddle
        {
            get
            {
                return this.useMiddle;
            }
            set
            {
                base.SetBooleanProperty(ref this.useMiddle, value);
            }
        }

        [Description("Determines whether the gradient fill appears on screen.")]
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

        [Description("Used only for polygonal gradients, to repeat fillings."), DefaultValue(4)]
        public System.Drawing.Drawing2D.WrapMode WrapMode
        {
            get
            {
                return this.wrapMode;
            }
            set
            {
                if (this.wrapMode != value)
                {
                    this.wrapMode = value;
                    this.Invalidate();
                }
            }
        }

        internal class GradientComponentEditor : UITypeEditor
        {
            public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
            {
                using (GradientEditor editor = new GradientEditor((Gradient) value))
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
                e.Graphics.FillRectangle(((Gradient) e.Value).DrawingBrush(e.Bounds), e.Bounds);
            }
        }
    }
}

