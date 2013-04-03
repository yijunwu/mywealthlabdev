namespace WealthLab.ChartDrawingObjects
{
    using Fidelity.Components;
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Windows.Forms;
    using WealthLab;
    using WealthLab.ChartControl;

    public class CDOSpeedResistance : CDOLineBased
    {
        private static DrawingObjectHelper drawingObjectHelper_0 = new SpeedResistanceHelper();
        private LineSettings lineSettings_0;

        public CDOSpeedResistance()
        {
        }

        public CDOSpeedResistance(ChartPane pane, DateTime dateTime_0, double value) : base(pane, dateTime_0, value)
        {
        }

        public override void ChangeSettings(UserControl userControl_0)
        {
            LineSettings settings = userControl_0 as LineSettings;
            base.Color = settings.Color;
            base.Style = settings.Style;
            base.Width = settings.DrawingObjectWidth;
            base.SnapToValue = settings.SnapToValue;
        }

        public override UserControl GetSettingsUI()
        {
            if (this.lineSettings_0 == null)
            {
                this.lineSettings_0 = new LineSettings();
            }
            this.lineSettings_0.Color = base.Color;
            this.lineSettings_0.DrawingObjectWidth = base.Width;
            this.lineSettings_0.Style = base.Style;
            this.lineSettings_0.SnapToValue = base.SnapToValue;
            return this.lineSettings_0;
        }

        public override void ReadSettings(ISettingsHost host)
        {
            string str = "DrawObj." + base.GetType().Name + ".";
            base.Color = host.Get(str + "Color", Color.Red);
            base.Style = (LineStyle) Enum.Parse(typeof(LineStyle), host.Get(str + "Style", "Solid"));
            base.Width = host.Get(str + "Width", 1);
            base.SnapToValue = host.Get(str + "SnapToValue", false);
        }

        protected override void Render(Graphics graphics_0)
        {
            if ((base.LeftHandle.Bar != -1) && (base.RightHandle.Bar != -1))
            {
                SmoothingMode smoothingMode = graphics_0.SmoothingMode;
                graphics_0.SmoothingMode = SmoothingMode.AntiAlias;
                ChartDrawingObjectHandle handle = (base._startHandle.Date < base._endHandle.Date) ? base._startHandle : base._endHandle;
                ChartDrawingObjectHandle handle2 = (base._startHandle.Date >= base._endHandle.Date) ? base._startHandle : base._endHandle;
                try
                {
                    Pen pen = new Pen(base.Color, (float) base.Width);
                    ChartRenderer.SetPenStyle(pen, base.Style);
                    pen.Width = base.Width;
                    int x = handle.X;
                    int y = handle.Y;
                    int num3 = handle2.X;
                    int num4 = handle2.Y;
                    num4 = ChartDrawingObject.CalculateYIntercept(x, y, num3, num4, base.ChartWidth);
                    graphics_0.DrawLine(pen, x, y, base.ChartWidth, num4);
                    double num5 = handle2.Value;
                    double num6 = handle.Value;
                    double num7 = (num6 > num5) ? num5 : num6;
                    double num8 = Math.Abs((double) (num5 - num6)) / 3.0;
                    pen.Width = 1f;
                    double num9 = num7 + num8;
                    num4 = base.Pane.ConvertValueToY(num9);
                    num4 = ChartDrawingObject.CalculateYIntercept(x, y, num3, num4, base.ChartWidth);
                    graphics_0.DrawLine(pen, x, y, base.ChartWidth, num4);
                    num9 += num8;
                    num4 = base.Pane.ConvertValueToY(num9);
                    num4 = ChartDrawingObject.CalculateYIntercept(x, y, num3, num4, base.ChartWidth);
                    graphics_0.DrawLine(pen, x, y, base.ChartWidth, num4);
                    pen.Dispose();
                    graphics_0.SmoothingMode = smoothingMode;
                }
                catch (Exception)
                {
                }
            }
        }

        public override void WriteSettings(ISettingsHost host)
        {
            string str = "DrawObj." + base.GetType().Name + ".";
            host.Set(str + "Color", base.Color);
            host.Set(str + "Style", base.Style.ToString());
            host.Set(str + "Width", base.Width);
            host.Set(str + "SnapToValue", base.SnapToValue);
        }

        protected override DrawingObjectHelper Helper
        {
            get
            {
                return drawingObjectHelper_0;
            }
        }
    }
}

