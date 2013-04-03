namespace WealthLab.ChartStyles
{
    using Fidelity.Components;
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Windows.Forms;
    using WealthLab;
    using WealthLab.ChartStyles.Properties;

    public class LineChartStyle : ChartStyle, ICustomSettings
    {
        private int int_0 = 1;
        private LineChartStyleSettings lineChartStyleSettings_0;

        public void ChangeSettings(UserControl userControl_0)
        {
            this.int_0 = (int) this.lineChartStyleSettings_0.numWidth.Value;
        }

        public UserControl GetSettingsUI()
        {
            if (this.lineChartStyleSettings_0 == null)
            {
                this.lineChartStyleSettings_0 = new LineChartStyleSettings();
                this.lineChartStyleSettings_0.numWidth.Value = this.int_0;
            }
            return this.lineChartStyleSettings_0;
        }

        public override void Initialize()
        {
        }

        protected override void InitializeBarWidths()
        {
        }

        public void ReadSettings(ISettingsHost host)
        {
            this.int_0 = host.Get("LineChartStyle.LineWidth", 1);
        }

        public override void RenderBars(Graphics graphics_0)
        {
            Pen pen;
            int ghostBarXPosition;
            int num5;
            int num = base.ConvertBarToX(base.LeftEdgeBar);
            int num2 = base.PricePane.ConvertValueToY(base.Bars.Close[base.LeftEdgeBar]);
            pen = new Pen(Color.Black) {
                Width = this.int_0,
                StartCap = LineCap.Round,
                EndCap = LineCap.Round
            };
            for (int i = base.LeftEdgeBar + 1; i <= base.RightEdgeBar; i++)
            {
                pen.Color = base.GetBarColor(i);
                ghostBarXPosition = base.ConvertBarToX(i);
                num5 = base.PricePane.ConvertValueToY(base.Bars.Close[i]);
                graphics_0.DrawLine(pen, num, num2, ghostBarXPosition, num5);
                num = ghostBarXPosition;
                num2 = num5;
            }
            if (base.ShouldDrawGhostBar)
            {
                ghostBarXPosition = base.GhostBarXPosition;
                num5 = base.PricePane.ConvertValueToY(base.Bars.Close.PartialValue);
                pen.Color = base.GhostBarColor;
                graphics_0.DrawLine(pen, num, num2, ghostBarXPosition, num5);
            }
            pen.Dispose();
        }

        public void WriteSettings(ISettingsHost host)
        {
            host.Set("LineChartStyle.LineWidth", this.int_0);
        }

        public override string FriendlyName
        {
            get
            {
                return "Line";
            }
        }

        public override Bitmap Glyph
        {
            get
            {
                return Resources.LineChartStyle;
            }
        }
    }
}

