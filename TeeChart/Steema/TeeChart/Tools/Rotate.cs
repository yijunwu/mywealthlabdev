namespace Steema.TeeChart.Tools
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using Steema.TeeChart.Styles;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    [ToolboxBitmap(typeof(Rotate), "ToolsIcons.Rotate.bmp"), Description("Allows rotating Chart dragging with mouse button.")]
    public class Rotate : Steema.TeeChart.Tools.Tool
    {
        private MouseButtons button;
        private bool dragging;
        private double IdifX;
        private double IdifY;
        private bool IFirstTime;
        private int inertia;
        private bool inverted;
        private bool IOldRepaint;
        private int IOldX;
        private int IOldY;
        private int speed;
        private RotateStyles style;
        private Timer timer;

        public event RotatingEventHandler Rotating;

        public Rotate() : this(null)
        {
        }

        public Rotate(Chart c) : base(c)
        {
            this.button = MouseButtons.Left;
            this.speed = 50;
        }

        protected override void Assign(Steema.TeeChart.Tools.Tool t)
        {
            base.Assign(t);
            Rotate rotate = t as Rotate;
            rotate.Button = this.Button;
            rotate.Inverted = this.Inverted;
            rotate.Pen = this.Pen.Clone() as ChartPen;
            rotate.Style = this.Style;
            rotate.Speed = this.speed;
            rotate.Inertia = this.inertia;
        }

        private Point CalcPoint(Graphics3D g, int x, int y, int z)
        {
            return base.chart.parent.PointToScreen(g.Calc3DPoint(x, y, z));
        }

        protected internal override void ChartEvent(EventArgs e)
        {
            base.ChartEvent(e);
            if (this.inertia > 0)
            {
                if ((Math.Abs(this.IdifX) > 0.01) || (Math.Abs(this.IdifY) > 0.01))
                {
                    double num = (100.0 - this.inertia) * 0.001;
                    this.IdifX = (Math.Abs(this.IdifX) > 0.01) ? (this.IdifX -= (this.IdifX * num)) : 0.0;
                    this.IdifY = (Math.Abs(this.IdifY) > 0.01) ? (this.IdifY -= (this.IdifY * num)) : 0.0;
                }
                else if (this.timer != null)
                {
                    this.timer.Enabled = false;
                }
            }
        }

        private double CorrectAngle(double angle)
        {
            double num = angle;
            if (num > 360.0)
            {
                return (num - 360.0);
            }
            if (num < 0.0)
            {
                num += 360.0;
            }
            return num;
        }

        private void DoRotation(double tx, double ty)
        {
            Aspect aspect = base.chart.Aspect;
            aspect.Orthogonal = false;
            if (base.chart.graphics3D.SupportsFullRotation)
            {
                if ((this.style == RotateStyles.Rotation) || (this.style == RotateStyles.All))
                {
                    aspect.RotationFloat = this.CorrectAngle(aspect.RotationFloat + tx);
                }
                if ((this.style == RotateStyles.Elevation) || (this.style == RotateStyles.All))
                {
                    aspect.ElevationFloat = this.CorrectAngle(aspect.ElevationFloat + ty);
                }
            }
            else
            {
                if ((this.style == RotateStyles.Rotation) || (this.style == RotateStyles.All))
                {
                    aspect.RotationFloat = this.RotationChange(base.chart.graphics3D.SupportsFullRotation, aspect.RotationFloat, tx);
                }
                if ((this.style == RotateStyles.Elevation) || (this.style == RotateStyles.All))
                {
                    aspect.ElevationFloat = this.ElevationChange(base.chart.graphics3D.SupportsFullRotation, aspect.ElevationFloat, ty);
                }
            }
        }

        private void DoTick(object sender, EventArgs e)
        {
            this.timer.Enabled = false;
            if (!this.dragging)
            {
                if (base.chart != null)
                {
                    this.DoRotation(this.IdifX, this.IdifY);
                }
                this.timer.Enabled = true;
            }
        }

        private void DrawCubeOutline()
        {
            int z = base.chart.Aspect.Width3D;
            Graphics3D g = base.chart.graphics3D;
            Rectangle chartRect = base.chart.ChartRect;
            Point[] pointArray = new Point[] { this.CalcPoint(g, chartRect.Left, chartRect.Top, 0), this.CalcPoint(g, chartRect.Left, chartRect.Bottom, 0), this.CalcPoint(g, chartRect.Right, chartRect.Bottom, 0), this.CalcPoint(g, chartRect.Right, chartRect.Top, 0), this.CalcPoint(g, chartRect.Left, chartRect.Top, z), this.CalcPoint(g, chartRect.Left, chartRect.Bottom, z), this.CalcPoint(g, chartRect.Right, chartRect.Bottom, z), this.CalcPoint(g, chartRect.Right, chartRect.Top, z) };
            g.Pen = this.Pen;
            Color backColor = this.Pen.Color;
            ControlPaint.DrawReversibleLine(pointArray[0], pointArray[1], backColor);
            ControlPaint.DrawReversibleLine(pointArray[1], pointArray[2], backColor);
            ControlPaint.DrawReversibleLine(pointArray[2], pointArray[3], backColor);
            ControlPaint.DrawReversibleLine(pointArray[3], pointArray[0], backColor);
            ControlPaint.DrawReversibleLine(pointArray[4], pointArray[5], backColor);
            ControlPaint.DrawReversibleLine(pointArray[5], pointArray[6], backColor);
            ControlPaint.DrawReversibleLine(pointArray[6], pointArray[7], backColor);
            ControlPaint.DrawReversibleLine(pointArray[7], pointArray[4], backColor);
            ControlPaint.DrawReversibleLine(pointArray[0], pointArray[4], backColor);
            ControlPaint.DrawReversibleLine(pointArray[1], pointArray[5], backColor);
            ControlPaint.DrawReversibleLine(pointArray[2], pointArray[6], backColor);
            ControlPaint.DrawReversibleLine(pointArray[3], pointArray[7], backColor);
        }

        private double ElevationChange(bool fullrotation, double angle, double change)
        {
            double num = angle;
            if (change == 0.0)
            {
                return num;
            }
            if (change > 0.0)
            {
                return Math.Min((double) 360.0, (double) (angle + change));
            }
            double num2 = fullrotation ? 0.0 : 270.0;
            return Math.Max(num2, angle + change);
        }

        internal static Pie FirstSeriesPie(Chart c)
        {
            foreach (Series series in c.Series)
            {
                if ((series.GetType() == typeof(Pie)) && series.Active)
                {
                    return (Pie) series;
                }
            }
            return null;
        }

        protected internal override void MouseEvent(MouseEventKinds kind, MouseEventArgs e, ref Cursor c)
        {
            base.MouseEvent(kind, e, ref c);
            Point point = new Point(e.X, e.Y);
            switch (kind)
            {
                case MouseEventKinds.Down:
                    if (Utils.GetMouseButton(e) == this.button)
                    {
                        if (this.timer != null)
                        {
                            this.timer.Enabled = false;
                        }
                        this.dragging = true;
                        this.IOldX = point.X;
                        this.IOldY = point.Y;
                        this.IFirstTime = true;
                        base.chart.CancelMouse = true;
                    }
                    break;

                case MouseEventKinds.Move:
                    if (!this.dragging)
                    {
                        break;
                    }
                    this.MouseMove(point.X, point.Y);
                    return;

                case MouseEventKinds.Up:
                    if (!this.dragging)
                    {
                        break;
                    }
                    this.dragging = false;
                    if (this.Pen.Visible)
                    {
                        base.chart.AutoRepaint = this.IOldRepaint;
                        this.Invalidate();
                    }
                    if (this.inertia <= 0)
                    {
                        break;
                    }
                    if (this.timer == null)
                    {
                        this.timer = new Timer();
                        this.timer.Tick += new EventHandler(this.DoTick);
                    }
                    this.timer.Interval = 1;
                    this.timer.Enabled = true;
                    return;

                default:
                    return;
            }
        }

        private void MouseMove(int x, int y)
        {
            Aspect aspect = base.chart.Aspect;
            double num = 1.0 + ((this.Speed - 50) * 0.01);
            this.IdifX = num * ((90.0 * (x - this.IOldX)) / ((double) base.chart.Width));
            if (this.inverted)
            {
                this.IdifX = -this.IdifX;
            }
            this.IdifY = num * ((90.0 * (this.IOldY - y)) / ((double) base.chart.Height));
            if (this.inverted)
            {
                this.IdifY = -this.IdifY;
            }
            if (this.Pen.Visible)
            {
                if (this.IFirstTime)
                {
                    this.IOldRepaint = base.chart.AutoRepaint;
                    base.chart.AutoRepaint = false;
                    this.IFirstTime = false;
                }
                else
                {
                    this.DrawCubeOutline();
                }
            }
            aspect.view3D = true;
            this.DoRotation(this.IdifX, this.IdifY);
            this.IOldX = x;
            this.IOldY = y;
            base.chart.CancelMouse = true;
            if (this.Pen.Visible)
            {
                base.chart.graphics3D.aspect.orthogonal = false;
                base.chart.graphics3D.CalcTrigValues();
                base.chart.graphics3D.CalcPerspective(base.chart.ChartRect);
                this.DrawCubeOutline();
            }
            this.OnRotating();
        }

        public virtual void OnRotating()
        {
            if (this.Rotating != null)
            {
                this.Rotating(this, EventArgs.Empty);
            }
        }

        private double RotationChange(bool fullrotation, double angle, double change)
        {
            double num = angle;
            if (change != 0.0)
            {
                if (change > 0.0)
                {
                    num += change;
                    if (num > 360.0)
                    {
                        num -= 360.0;
                    }
                    return num;
                }
                if (fullrotation)
                {
                    return Math.Max((double) 0.0, (double) (angle + change));
                }
                num = angle + change;
                if (num < 0.0)
                {
                    num += 360.0;
                }
            }
            return num;
        }

        [DefaultValue(0x100000), Description("Defines which mousebutton activates the TTeeCustomTool.")]
        public MouseButtons Button
        {
            get
            {
                return this.button;
            }
            set
            {
                this.button = value;
            }
        }

        [Description("Gets descriptive text.")]
        public override string Description
        {
            get
            {
                return Texts.RotateTool;
            }
        }

        [DefaultValue(0), Description("Defines the rotate tool inertia factor.")]
        public int Inertia
        {
            get
            {
                return this.inertia;
            }
            set
            {
                base.SetIntegerProperty(ref this.inertia, value);
            }
        }

        [DefaultValue(false), Description("Inverts the direction of Rotation and Elevation.")]
        public bool Inverted
        {
            get
            {
                return this.inverted;
            }
            set
            {
                this.inverted = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Element Pen characteristics."), Category("Appearance")]
        public ChartPen Pen
        {
            get
            {
                if (base.pPen == null)
                {
                    base.pPen = new ChartPen(base.chart, Color.White);
                    base.pPen.bVisible = false;
                    base.pPen.defaultVisible = false;
                }
                return base.pPen;
            }
            set
            {
                base.pPen = value;
            }
        }

        [DefaultValue(50), Description("Sets the speed of the rotate tool.")]
        public int Speed
        {
            get
            {
                return this.speed;
            }
            set
            {
                this.speed = value;
            }
        }

        [DefaultValue(0), Description("Determines whether mouse action applies to Rotation, Elevation or Both.")]
        public RotateStyles Style
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

        [Description("Gets detailed descriptive text.")]
        public override string Summary
        {
            get
            {
                return Texts.RotateSummary;
            }
        }
    }
}

