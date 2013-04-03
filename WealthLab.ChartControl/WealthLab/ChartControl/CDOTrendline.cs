namespace WealthLab.ChartControl
{
    using Fidelity.Components;
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.IO;
    using System.Windows.Forms;
    using WealthLab;
    using WealthLab.ChartControl.Properties;

    public class CDOTrendline : CDOLineBased
    {
        private bool bool_5;
        private bool bool_6;
        private static DrawingObjectHelper drawingObjectHelper_0 = new TrendlineHelper();
        private const int int_1 = 2;
        private int int_2;
        private TrendlineSettings trendlineSettings_0;

        public CDOTrendline()
        {
            this.bool_5 = true;
            this.int_2 = 1;
        }

        public CDOTrendline(ChartPane pane, DateTime dateTime_0, double value) : base(pane, dateTime_0, value)
        {
            this.bool_5 = true;
            this.int_2 = 1;
        }

        public override void ChangeSettings(UserControl userControl_0)
        {
            base.ChangeSettings(userControl_0);
            this.DisplayPercentageChange = (userControl_0 as TrendlineSettings).DisplayPercentageChange;
        }

        public void CreateParallelTrendline(object sender, EventArgs e)
        {
            CDOTrendline trendline = new CDOTrendline(base.Pane, base.Handles[0].Date, base.Handles[0].Value);
            if (base.Bars.DataScale.IsIntraday)
            {
                trendline.Handles[0].Value = base.Handles[0].Value * 0.999;
            }
            else
            {
                trendline.Handles[0].Value = base.Handles[0].Value * 0.99;
            }
            double num = base.Handles[0].Value - trendline.Handles[0].Value;
            trendline.Handles[0].Date = base.Handles[0].Date;
            trendline.Handles[1].Value = base.Handles[1].Value - num;
            trendline.Handles[1].Date = base.Handles[1].Date;
            trendline.Color = base.Color;
            trendline.Style = base.Style;
            trendline.Width = base.Width;
            trendline.ExtendLeft = base.ExtendLeft;
            trendline.ExtendRight = base.ExtendRight;
            trendline.SnapToValue = base.SnapToValue;
            trendline.DisplayPercentageChange = this.DisplayPercentageChange;
            this.AddDrawingObject(trendline);
        }

        public override UserControl GetSettingsUI()
        {
            if (this.trendlineSettings_0 == null)
            {
                this.trendlineSettings_0 = new TrendlineSettings();
            }
            this.trendlineSettings_0.DrawingObjectName = base.Name;
            this.trendlineSettings_0.Color = base.Color;
            this.trendlineSettings_0.DrawingObjectWidth = base.Width;
            this.trendlineSettings_0.Style = base.Style;
            this.trendlineSettings_0.ExtendLeft = base.ExtendLeft;
            this.trendlineSettings_0.ExtendRight = base.ExtendRight;
            this.trendlineSettings_0.SnapToValue = base.SnapToValue;
            this.trendlineSettings_0.DisplayPercentageChange = this.DisplayPercentageChange;
            return this.trendlineSettings_0;
        }

        protected internal override void OnEndDrag(ChartDrawingObjectHandle handle)
        {
            base.OnEndDrag(handle);
            if ((base._startHandle.Bar == base._endHandle.Bar) && this.bool_5)
            {
                this.bool_5 = false;
                base._endHandle.Bar += 5;
                if (base._endHandle.Bar >= base.Bars.Count)
                {
                    base._endHandle.Bar = base.Bars.Count - 1;
                }
            }
        }

        protected internal override void Read(BinaryReader binaryReader_0)
        {
            base.Read(binaryReader_0);
            try
            {
                int num = binaryReader_0.PeekChar();
                if ((num != -1) && (((ushort) num) == 0x56))
                {
                    binaryReader_0.ReadChar();
                    this.int_2 = binaryReader_0.ReadInt32();
                }
                if (this.int_2 == 2)
                {
                    this.DisplayPercentageChange = binaryReader_0.ReadBoolean();
                }
            }
            catch
            {
            }
        }

        public override void ReadSettings(ISettingsHost host)
        {
            base.ReadSettings(host);
            this.DisplayPercentageChange = host.Get(("DrawObj." + base.GetType().Name + ".") + "DisplayPercentageChange", false);
        }

        public override void RegisterExtendedBehaviors(ICDOBehavior behaviors)
        {
            behaviors.RegisterBehavior("Create Parallel Trendline|Delete Drawing Object", Resources.Parallel, new EventHandler(this.CreateParallelTrendline));
        }

        protected internal override void Render(Graphics graphics_0)
        {
            if ((base.LeftHandle.Bar != -1) && (base.RightHandle.Bar != -1))
            {
                base.Render(graphics_0);
                if (this.DisplayPercentageChange)
                {
                    using (Pen pen = new Pen(Color.Gray, 1f))
                    {
                        pen.DashStyle = DashStyle.Dash;
                        double num = base.Bars.Close[base.LeftHandle.Bar];
                        double num2 = base.Bars.Close[base.RightHandle.Bar];
                        int num3 = base.ConvertBarToX(base.LeftHandle.Bar);
                        int num4 = base.ConvertBarToX(base.RightHandle.Bar);
                        int num5 = base.Pane.ConvertValueToY(num);
                        int num6 = base.Pane.ConvertValueToY(num2);
                        graphics_0.DrawLine(pen, num3, num5, num4, num6);
                    }
                }
            }
        }

        protected internal override void RenderNotClipped(Graphics graphics_0)
        {
            base.RenderNotClipped(graphics_0);
            if (this.DisplayPercentageChange)
            {
                double num = base.Bars.Close[base.LeftHandle.Bar];
                double num2 = base.Bars.Close[base.RightHandle.Bar];
                int num3 = base.Renderer.Width - base.Renderer.MarginRightWidth;
                string text = ((((num2 - num) * 100.0) / num)).ToString("N2") + "%";
                SizeF ef = graphics_0.MeasureString(text, base.HandleFont);
                int num5 = base.ConvertBarToX(base.RightHandle.Bar);
                if (num3 > (num5 - (ef.Width / 2f)))
                {
                    base.ConvertBarToX(base.LeftHandle.Bar);
                    int num6 = base.Pane.ConvertValueToY(num2);
                    RectangleF rect = new RectangleF(num5 - (ef.Width / 2f), num6 - ef.Height, ef.Width, ef.Height);
                    graphics_0.FillRectangle(new SolidBrush(base.Pane.GetBackgroundColor(base.RightHandle.Bar)), rect);
                    graphics_0.DrawString(text, base.HandleFont, new SolidBrush(Color.Gray), rect);
                }
            }
        }

        protected internal override void Write(BinaryWriter binaryWriter_0)
        {
            base.Write(binaryWriter_0);
            binaryWriter_0.Write('V');
            binaryWriter_0.Write(2);
            binaryWriter_0.Write(this.DisplayPercentageChange);
        }

        public override void WriteSettings(ISettingsHost host)
        {
            base.WriteSettings(host);
            host.Set(("DrawObj." + base.GetType().Name + ".") + "DisplayPercentageChange", this.DisplayPercentageChange);
        }

        public bool DisplayPercentageChange
        {
            get
            {
                return this.bool_6;
            }
            set
            {
                this.bool_6 = value;
            }
        }

        protected internal override DrawingObjectHelper Helper
        {
            get
            {
                return drawingObjectHelper_0;
            }
        }
    }
}

