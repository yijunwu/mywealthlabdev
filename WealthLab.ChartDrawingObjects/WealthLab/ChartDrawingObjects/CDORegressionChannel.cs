namespace WealthLab.ChartDrawingObjects
{
    using Fidelity.Components;
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Windows.Forms;
    using WealthLab;
    using WealthLab.ChartControl;

    public class CDORegressionChannel : CDOLineBased, ICustomSettings
    {
        private static DrawingObjectHelper drawingObjectHelper_0 = new RegressionChannelHelper();
        private LinearRegression linearRegression_0;
        private RegressionChannelSettings regressionChannelSettings_0;

        public CDORegressionChannel()
        {
            this.linearRegression_0 = new LinearRegression();
        }

        public CDORegressionChannel(ChartPane pane, DateTime dateTime_0, double value) : base(pane, dateTime_0, value)
        {
            this.linearRegression_0 = new LinearRegression();
            this.linearRegression_0 = new LinearRegression();
        }

        public override void ChangeSettings(UserControl userControl_0)
        {
            RegressionChannelSettings settings = userControl_0 as RegressionChannelSettings;
            base.Name = settings.DrawingObjectName;
            base.Color = settings.Color;
            base.Width = settings.DrawingObjectWidth;
            base.ExtendLeft = settings.ExtendLeft;
            base.ExtendRight = settings.ExtendRight;
        }

        public override UserControl GetSettingsUI()
        {
            if (this.regressionChannelSettings_0 == null)
            {
                this.regressionChannelSettings_0 = new RegressionChannelSettings();
            }
            this.regressionChannelSettings_0.DrawingObjectName = base.Name;
            this.regressionChannelSettings_0.Color = base.Color;
            this.regressionChannelSettings_0.DrawingObjectWidth = base.Width;
            this.regressionChannelSettings_0.ExtendLeft = base.ExtendLeft;
            this.regressionChannelSettings_0.ExtendRight = base.ExtendRight;
            return this.regressionChannelSettings_0;
        }

        private void method_0(ChartDrawingObjectHandle chartDrawingObjectHandle_0, ChartDrawingObjectHandle chartDrawingObjectHandle_1, ref int int_1, ref int int_2, ref int int_3, ref int int_4)
        {
            if (base.ExtendLeft)
            {
                int_2 = ChartDrawingObject.CalculateYIntercept(int_1, int_2, int_3, int_4, 0);
                int_1 = 0;
            }
            if (base.ExtendRight)
            {
                int_4 = ChartDrawingObject.CalculateYIntercept(int_1, int_2, int_3, int_4, base.ChartWidth);
                int_3 = base.ChartWidth;
            }
            if ((int_2 == -2147483648) || (int_4 == -2147483648))
            {
                int_1 = chartDrawingObjectHandle_0.X;
                int_2 = chartDrawingObjectHandle_0.Y;
                int_3 = chartDrawingObjectHandle_1.X;
                int_4 = chartDrawingObjectHandle_1.Y;
            }
        }

        public override void ReadSettings(ISettingsHost host)
        {
            string str = "DrawObj." + base.GetType().Name + ".";
            base.Color = host.Get(str + "Color", Color.Red);
            base.Width = host.Get(str + "Width", 1);
            base.ExtendLeft = host.Get(str + "ExtendLeft", false);
            base.ExtendRight = host.Get(str + "ExtendRight", false);
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
                    this.linearRegression_0.Init();
                    int bar = handle.Bar;
                    int num2 = handle2.Bar;
                    for (int i = bar; i <= num2; i++)
                    {
                        this.linearRegression_0.Add((double) i, base.Bars.Close[i]);
                    }
                    this.linearRegression_0.Complete();
                    double num5 = -2147483648.0;
                    for (int j = bar; j <= num2; j++)
                    {
                        double num7 = Math.Abs((double) (base.Bars.Close[j] - this.linearRegression_0.PredictY((double) j)));
                        if (num7 > num5)
                        {
                            num5 = num7;
                        }
                    }
                    double num8 = this.linearRegression_0.PredictY((double) bar);
                    double num4 = this.linearRegression_0.PredictY((double) num2);
                    if (num8 != double.NaN)
                    {
                        handle.Value = num8;
                    }
                    if (num4 != double.NaN)
                    {
                        handle2.Value = num4;
                    }
                    Pen pen = new Pen(base.Color, (float) base.Width);
                    ChartRenderer.SetPenStyle(pen, base.Style);
                    pen.Width = base.Width;
                    int x = handle.X;
                    int y = handle.Y;
                    int num11 = handle2.X;
                    int num12 = handle2.Y;
                    this.method_0(handle, handle2, ref x, ref y, ref num11, ref num12);
                    graphics_0.DrawLine(pen, x, y, num11, num12);
                    pen.Width = 1f;
                    ChartRenderer.SetPenStyle(pen, LineStyle.Dashed);
                    x = handle.X;
                    num11 = handle2.X;
                    y = base.Pane.ConvertValueToY(num8 - num5);
                    num12 = base.Pane.ConvertValueToY(num4 - num5);
                    this.method_0(handle, handle2, ref x, ref y, ref num11, ref num12);
                    graphics_0.DrawLine(pen, x, y, num11, num12);
                    x = handle.X;
                    num11 = handle2.X;
                    y = base.Pane.ConvertValueToY(num8 + num5);
                    num12 = base.Pane.ConvertValueToY(num4 + num5);
                    this.method_0(handle, handle2, ref x, ref y, ref num11, ref num12);
                    graphics_0.DrawLine(pen, x, y, num11, num12);
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
            host.Set(str + "Width", base.Width);
            host.Set(str + "ExtendLeft", base.ExtendLeft);
            host.Set(str + "ExtendRight", base.ExtendRight);
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

