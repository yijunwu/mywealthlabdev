namespace WealthLab.ChartDrawingObjects
{
    using Fidelity.Components;
    using System;
    using System.Drawing;
    using System.Windows.Forms;
    using WealthLab;
    using WealthLab.ChartControl;

    public class CDOHorizontalLine : CDOLineBased
    {
        private static DrawingObjectHelper drawingObjectHelper_0 = new HorizontalLineHelper();
        private LineSettings lineSettings_0;
        private Point point_0;

        public CDOHorizontalLine()
        {
        }

        public CDOHorizontalLine(ChartPane pane, DateTime dateTime_0, double value) : base(pane, dateTime_0, value)
        {
            base._startHandle.Visible = false;
            base._endHandle.Visible = false;
            base._mover.ShowPriceValue = true;
            base.Handles[0].HandleType = ChartDrawingObjectHandleType.Mover;
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

        protected override bool IsMouseOver(int int_1, int int_2)
        {
            return ((int_2 > (this.point_0.Y - Chart.PixelSensitivity)) && (int_2 < (this.point_0.Y + Chart.PixelSensitivity)));
        }

        protected override void OnSelected(int int_1, int int_2)
        {
            if ((int_2 > (this.point_0.Y - Chart.PixelSensitivity)) && (int_2 < (this.point_0.Y + Chart.PixelSensitivity)))
            {
                base._startHandle.Visible = false;
                base._endHandle.Visible = false;
                base._mover.X = int_1;
            }
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
                this.point_0.X = base._mover.X;
                this.point_0.Y = base._mover.Y;
                Pen pen = new Pen(base.Color, (float) base.Width);
                ChartRenderer.SetPenStyle(pen, base.Style);
                pen.Width = base.Width;
                graphics_0.DrawLine(pen, 0, base._mover.Y, base.ChartWidth, base._mover.Y);
                base._mover.ShowPriceValue = true;
                pen.Dispose();
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

