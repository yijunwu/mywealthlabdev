namespace Steema.TeeChart.Styles
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.InteropServices;

    public class Circular : Series
    {
        private Color circleBackColor;
        private bool circled;
        private Gradient circleGradient;
        private int customXRadius;
        private int customYRadius;
        private static Aspect IBack3D;
        protected int iCircleHeight;
        protected int iCircleWidth;
        protected int iCircleXCenter;
        protected int iCircleYCenter;
        protected int iXRadius;
        protected int iYRadius;
        public const double PiDegree = 0.017453292519943295;
        protected Rectangle rCircleRect;
        private int rotationAngle;
        protected double rotDegree;

        public Circular() : this(null)
        {
        }

        public Circular(Chart c) : base(c)
        {
            base.UseAxis = false;
            base.calcVisiblePoints = false;
            base.vxValues.Name = Texts.ValuesAngle;
        }

        private void AdjustCircleMarks()
        {
            int length = base.Marks.Callout.Length;
            if (base.Marks.Pen.Visible)
            {
                length += Utils.Round((float) (2 * base.Marks.Pen.Width));
            }
            base.chart.graphics3D.Font = base.Marks.Font;
            int num2 = base.chart.graphics3D.FontHeight + length;
            this.rCircleRect.Y += num2;
            this.rCircleRect.Height -= 2 * num2;
            int num3 = Utils.Round((float) ((base.MaxMarkWidth() + base.chart.graphics3D.TextWidth("W")) + length));
            this.rCircleRect.X += num3;
            this.rCircleRect.Width -= 2 * num3;
            this.AdjustCircleRect();
        }

        protected void AdjustCircleRect()
        {
            Rectangle rCircleRect = this.rCircleRect;
            if ((rCircleRect.Width % 2) == 1)
            {
                rCircleRect.Width--;
            }
            if ((rCircleRect.Height % 2) == 1)
            {
                rCircleRect.Height--;
            }
            if (rCircleRect.Width < 4)
            {
                rCircleRect.Width = 4;
            }
            if (rCircleRect.Height < 4)
            {
                rCircleRect.Height = 4;
            }
            this.iCircleWidth = rCircleRect.Width;
            this.iCircleHeight = rCircleRect.Height;
            Graphics3D.RectCenter(this.rCircleRect, out this.iCircleXCenter, out this.iCircleYCenter);
        }

        private double AdjustRatio(double ARatio, Graphics3D g)
        {
            int screenHeight = Graphics3D.ScreenHeight;
            int screenWidth = Graphics3D.ScreenWidth;
            double num3 = ARatio;
            if (screenHeight != 0)
            {
                double num4 = (1.0 * screenWidth) / ((double) screenHeight);
                if (num4 != 0.0)
                {
                    num3 = (1.0 * ARatio) / num4;
                }
            }
            return num3;
        }

        public virtual void AngleToPos(double angle, double aXRadius, double aYRadius, out int x, out int y)
        {
            double num;
            double num2;
            Utils.SinCos(this.rotDegree + angle, out num, out num2);
            x = this.iCircleXCenter + Utils.Round((double) (aXRadius * num2));
            y = this.iCircleYCenter - Utils.Round((double) (aYRadius * num));
        }

        public override void AssignFormat(Series source)
        {
            base.AssignFormat(source);
            if (source is Circular)
            {
                this.circled = (source as Circular).Circled;
                this.customXRadius = (source as Circular).CustomXRadius;
                this.customYRadius = (source as Circular).CustomYRadius;
                this.rotationAngle = (source as Circular).RotationAngle;
                this.circleBackColor = (source as Circular).CircleBackColor;
                this.circleGradient = (source as Circular).CircleGradient.Clone() as Gradient;
            }
        }

        protected internal override bool AssociatedToAxis(Axis a)
        {
            return true;
        }

        protected Color CalcCircleBackColor()
        {
            Color circleBackColor = this.circleBackColor;
            if (Utils.ColorIsEmpty(circleBackColor))
            {
                if (base.chart.printing)
                {
                    circleBackColor = Color.White;
                }
                else if (!base.chart.Walls.Back.Transparent)
                {
                    circleBackColor = base.Color;
                }
            }
            if (Utils.ColorIsEmpty(circleBackColor))
            {
                circleBackColor = base.chart.Panel.Color;
            }
            return circleBackColor;
        }

        private void CalcCircledRatio()
        {
            int num;
            double num2 = this.AdjustRatio((1.0 * Graphics3D.ScreenWidth) / ((double) Graphics3D.ScreenHeight), base.chart.graphics3D);
            this.CalcRadius();
            if (Utils.Round((double) (num2 * this.iYRadius)) < this.iXRadius)
            {
                num = this.iXRadius - Utils.Round((double) (num2 * this.iYRadius));
                this.rCircleRect.X += num;
                this.rCircleRect.Width -= 2 * num;
            }
            else
            {
                num = this.iYRadius - Utils.Round((double) ((1.0 * this.iXRadius) / num2));
                this.rCircleRect.Y += num;
                this.rCircleRect.Height -= 2 * num;
            }
            this.AdjustCircleRect();
        }

        protected Gradient CalcCircleGradient()
        {
            Gradient circleGradient = this.circleGradient;
            if (circleGradient == null)
            {
                return circleGradient;
            }
            if (circleGradient.Visible)
            {
                return circleGradient;
            }
            return null;
        }

        protected void CalcRadius()
        {
            if (this.customXRadius != 0)
            {
                this.iXRadius = this.customXRadius;
                this.iCircleWidth = 2 * this.iXRadius;
            }
            else
            {
                this.iXRadius = this.iCircleWidth / 2;
            }
            if (this.customYRadius != 0)
            {
                this.iYRadius = this.customYRadius;
                this.iCircleHeight = 2 * this.iYRadius;
            }
            else
            {
                this.iYRadius = this.iCircleHeight / 2;
            }
            this.rCircleRect.X = this.iCircleXCenter - this.iXRadius;
            this.rCircleRect.Width = 2 * this.iXRadius;
            this.rCircleRect.Y = this.iCircleYCenter - this.iYRadius;
            this.rCircleRect.Height = 2 * this.iYRadius;
        }

        protected internal override void DoBeforeDrawValues()
        {
            base.DoBeforeDrawValues();
            this.rCircleRect = base.chart.ChartRect;
            this.AdjustCircleRect();
            if (base.Marks.Visible)
            {
                this.AdjustCircleMarks();
            }
            if (this.circled)
            {
                this.CalcCircledRatio();
            }
            this.CalcRadius();
        }

        protected internal override void OnDisposing()
        {
            this.SetParentProperties(true);
        }

        public double PointToAngle(int x, int y)
        {
            double num;
            if ((x - this.iCircleXCenter) == 0)
            {
                if (y > this.iCircleYCenter)
                {
                    num = -1.5707963267948966;
                }
                else
                {
                    num = 1.5707963267948966;
                }
            }
            else if ((this.iYRadius == 0) || (this.iYRadius == 0))
            {
                num = 0.0;
            }
            else
            {
                num = Math.Atan2(((double) (this.iCircleYCenter - y)) / ((double) this.iYRadius), ((double) (x - this.iCircleXCenter)) / ((double) this.iXRadius));
            }
            if (num < 0.0)
            {
                num = 6.2831853071795862 + num;
            }
            num -= this.rotDegree;
            if (num < 0.0)
            {
                num = 6.2831853071795862 + num;
            }
            return num;
        }

        public double PointToRadius(int x, int y)
        {
            if (base.GetVertAxis == null)
            {
                return 0.0;
            }
            double num = base.GetVertAxis.Maximum - base.GetVertAxis.Minimum;
            if (num == 0.0)
            {
                return 0.0;
            }
            double num2 = x - this.iCircleXCenter;
            double num3 = y - this.iCircleYCenter;
            num2 *= num / ((double) this.iXRadius);
            num3 *= num / ((double) this.iYRadius);
            return (Math.Sqrt((num2 * num2) + (num3 * num3)) + base.GetVertAxis.Minimum);
        }

        protected override void PrepareLegendCanvas(Graphics3D g, int valueIndex, ref Color backColor, ref ChartBrush aBrush)
        {
            backColor = this.CalcCircleBackColor();
        }

        public void Rotate(int angle)
        {
            this.RotationAngle = (this.rotationAngle + angle) % 360;
        }

        protected override void SetActive(bool value)
        {
            base.SetActive(value);
            this.SetParentProperties(!base.bActive);
        }

        protected override void SetChart(Chart value)
        {
            base.SetChart(value);
            if (value == null)
            {
                this.SetParentProperties(true);
            }
            else
            {
                this.SetParentProperties(false);
            }
        }

        private void SetOtherCustomRadius(bool isXRadius, int value)
        {
            if (base.chart != null)
            {
                foreach (Series series in base.chart.Series)
                {
                    if (series.GetType().IsInstanceOfType(this))
                    {
                        if (isXRadius)
                        {
                            ((Circular) series).customXRadius = value;
                        }
                        else
                        {
                            ((Circular) series).customYRadius = value;
                        }
                    }
                }
            }
        }

        protected virtual void SetParentProperties(bool enableParentProps)
        {
            if ((base.chart != null) && !base.chart.graphics3D.SupportsFullRotation)
            {
                if (enableParentProps)
                {
                    if (IBack3D != null)
                    {
                        base.chart.aspect.Assign(IBack3D);
                    }
                    IBack3D = null;
                }
                else if (IBack3D == null)
                {
                    IBack3D = new Aspect();
                    Aspect a = base.chart.Aspect;
                    IBack3D.Assign(a);
                    if (a.Orthogonal)
                    {
                        a.Orthogonal = false;
                        a.Rotation = 360;
                        a.Elevation = 0x13b;
                        a.Perspective = 0;
                    }
                    a.Tilt = 0;
                }
            }
        }

        protected void SetRotationAngle(int value)
        {
            base.SetIntegerProperty(ref this.rotationAngle, value % 360);
            this.rotDegree = this.rotationAngle * 0.017453292519943295;
        }

        protected virtual bool ShouldSerializeCircleBackColor()
        {
            return !Utils.ColorIsEmpty(this.circleBackColor);
        }

        [Description("Determines the color to fill the ellipse.")]
        public Color CircleBackColor
        {
            get
            {
                return this.circleBackColor;
            }
            set
            {
                if (base.bBrush.Transparency != 0)
                {
                    this.circleBackColor = Graphics3D.TransparentColor(base.bBrush.Transparency, value);
                }
                else
                {
                    base.SetColorProperty(ref this.circleBackColor, value);
                }
            }
        }

        [DefaultValue(false), Description("Sets Circle series as elliptical or circular.")]
        public bool Circled
        {
            get
            {
                return this.circled;
            }
            set
            {
                base.SetBooleanProperty(ref this.circled, value);
                if (base.chart != null)
                {
                    foreach (Series series in base.chart.Series)
                    {
                        if (series.GetType().IsInstanceOfType(this))
                        {
                            ((Circular) series).circled = value;
                            ((Circular) series).customXRadius = 0;
                            ((Circular) series).customYRadius = 0;
                        }
                    }
                }
            }
        }

        [DefaultValue((string) null), Description("Determines the Gradient which fills the ellipse."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Gradient CircleGradient
        {
            get
            {
                if (this.circleGradient == null)
                {
                    this.circleGradient = new Gradient(base.chart);
                }
                return this.circleGradient;
            }
            set
            {
                this.circleGradient = value;
            }
        }

        [Description("Returns the height of the bounding Circle.")]
        public int CircleHeight
        {
            get
            {
                return this.iCircleHeight;
            }
        }

        public Rectangle CircleRect
        {
            get
            {
                return this.rCircleRect;
            }
        }

        [Description("Returns the width of the bounding Circle.")]
        public int CircleWidth
        {
            get
            {
                return this.iCircleWidth;
            }
        }

        [Description("Returns the exact horizontal position of ellipse's center in pixels.")]
        public int CircleXCenter
        {
            get
            {
                return this.iCircleXCenter;
            }
        }

        [Description("Returns the exact vertical position of ellipse's center in pixels.")]
        public int CircleYCenter
        {
            get
            {
                return this.iCircleYCenter;
            }
        }

        [DefaultValue(0), Description("Sets ellipse's horizontal radius in pixels.")]
        public int CustomXRadius
        {
            get
            {
                return this.customXRadius;
            }
            set
            {
                base.SetIntegerProperty(ref this.customXRadius, value);
                this.SetOtherCustomRadius(true, value);
            }
        }

        [Description("Sets ellipse's vertical radius in pixels."), DefaultValue(0)]
        public int CustomYRadius
        {
            get
            {
                return this.customYRadius;
            }
            set
            {
                base.SetIntegerProperty(ref this.customYRadius, value);
                this.SetOtherCustomRadius(false, value);
            }
        }

        [DefaultValue(0), Description("Sets angle of Chart rotation.")]
        public int RotationAngle
        {
            get
            {
                return this.rotationAngle;
            }
            set
            {
                this.SetRotationAngle(value);
            }
        }

        [Description("Returns the exact horizontal size of the ellipse's radius in pixels. ")]
        public int XRadius
        {
            get
            {
                return this.iXRadius;
            }
        }

        [Description("Returns the exact vertical size of the ellipse's radius in pixels.")]
        public int YRadius
        {
            get
            {
                return this.iYRadius;
            }
        }
    }
}

