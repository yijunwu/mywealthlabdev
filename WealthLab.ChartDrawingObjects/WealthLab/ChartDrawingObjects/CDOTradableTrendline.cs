namespace WealthLab.ChartDrawingObjects
{
    using Fidelity.Components;
    using System;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;
    using WealthLab;
    using WealthLab.ChartControl;
    using WealthLab.ChartDrawingObjects.Properties;

    public class CDOTradableTrendline : CDOTrendline, ICustomSettings
    {
        private bool bool_7;
        private ISettingsHost isettingsHost_0;
        private TradableTrendlineHelper tradableTrendlineHelper_0;
        private TradableTrendlineSettings tradableTrendlineSettings_0;
        private TradeType tradeType_0;

        public CDOTradableTrendline()
        {
            this.tradableTrendlineHelper_0 = new TradableTrendlineHelper();
            this.bool_7 = true;
            base.ExtendRight = true;
        }

        public CDOTradableTrendline(ChartPane pane, DateTime dateTime_0, double value) : base(pane, dateTime_0, value)
        {
            this.tradableTrendlineHelper_0 = new TradableTrendlineHelper();
            this.bool_7 = true;
            base.ExtendRight = true;
        }

        public override void ChangeSettings(UserControl userControl_0)
        {
            base.ChangeSettings(userControl_0);
            this.AlertType = this.tradableTrendlineSettings_0.AlertType;
            this.AbovePrice = this.tradableTrendlineSettings_0.AbovePrice;
        }

        public void CreateParallelTradableTrendline(object sender, EventArgs e)
        {
            CDOTradableTrendline trendline = new CDOTradableTrendline(base.Pane, base.Handles[0].Date, base.Handles[0].Value);
            trendline.Handles[0].Value = base.Handles[0].Value * 0.99;
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
            trendline.DisplayPercentageChange = base.DisplayPercentageChange;
            this.AddDrawingObject(trendline);
        }

        public override UserControl GetSettingsUI()
        {
            if (this.tradableTrendlineSettings_0 == null)
            {
                this.tradableTrendlineSettings_0 = new TradableTrendlineSettings();
            }
            this.tradableTrendlineSettings_0.DrawingObjectName = base.Name;
            this.tradableTrendlineSettings_0.Color = base.Color;
            this.tradableTrendlineSettings_0.DrawingObjectWidth = base.Width;
            this.tradableTrendlineSettings_0.Style = base.Style;
            this.tradableTrendlineSettings_0.ExtendLeft = base.ExtendLeft;
            this.tradableTrendlineSettings_0.ExtendRight = base.ExtendRight;
            this.tradableTrendlineSettings_0.SnapToValue = base.SnapToValue;
            this.tradableTrendlineSettings_0.DisplayPercentageChange = base.DisplayPercentageChange;
            this.tradableTrendlineSettings_0.AlertType = this.AlertType;
            this.tradableTrendlineSettings_0.AbovePrice = this.AbovePrice;
            return this.tradableTrendlineSettings_0;
        }

        protected override void OnEndDrag(ChartDrawingObjectHandle handle)
        {
            base.OnEndDrag(handle);
            if ((this.isettingsHost_0 != null) && (this.tradableTrendlineSettings_0 == null))
            {
                int num = ChartDrawingObject.CalculateYIntercept(base.LeftHandle.X, base.LeftHandle.Y, base.RightHandle.X, base.RightHandle.Y, base.ConvertBarToX(base.Bars.Count - 1));
                double num2 = base.Pane.ConvertYToValue(num);
                double num3 = base.Bars.Close[base.Bars.Count - 1];
                this.AbovePrice = num2 > num3;
                base.PromptUserForSettings(this.isettingsHost_0, this.Helper);
            }
        }

        protected override void Read(BinaryReader binaryReader_0)
        {
            base.Read(binaryReader_0);
            try
            {
                int num = binaryReader_0.ReadInt32();
                if (Enum.IsDefined(typeof(TradeType), num))
                {
                    this.AlertType = (TradeType) num;
                }
                else
                {
                    this.AlertType = TradeType.Buy;
                }
                this.AbovePrice = binaryReader_0.ReadBoolean();
            }
            catch
            {
            }
        }

        public override void ReadSettings(ISettingsHost host)
        {
            base.ReadSettings(host);
            string str = "DrawObj." + base.GetType().Name + ".";
            this.AlertType = (TradeType) host.Get(str + "AlertType", 0);
            this.isettingsHost_0 = host;
        }

        public override void RegisterExtendedBehaviors(ICDOBehavior behaviors)
        {
            behaviors.RegisterBehavior("Create Parallel Trendline|Delete Drawing Object", Resources.Parallel, new EventHandler(this.CreateParallelTradableTrendline));
        }

        protected override void Render(Graphics graphics_0)
        {
            if ((base.LeftHandle.Bar != -1) && (base.RightHandle.Bar != -1))
            {
                base.Render(graphics_0);
                int x = base.LeftHandle.X;
                int y = base.LeftHandle.Y;
                int num3 = base.RightHandle.X;
                int num4 = base.RightHandle.Y;
                SolidBrush brush = new SolidBrush(base.Color);
                graphics_0.FillEllipse(brush, x - 3, y - 3, 7, 7);
                graphics_0.FillEllipse(brush, num3 - 3, num4 - 3, 7, 7);
            }
        }

        public override bool TriggerAlert(Bars bars, ref TradeType alertType, ref string signalName)
        {
            if ((bars.Count > 1) && base.ExtendRight)
            {
                bool flag;
                double num4 = ChartDrawingObject.CalculateYIntercept((double) base.LeftHandle.X, base.LeftHandle.Value, (double) base.RightHandle.X, base.RightHandle.Value, (double) base.ConvertBarToX(bars.Count - 1));
                double num2 = ChartDrawingObject.CalculateYIntercept((double) base.LeftHandle.X, base.LeftHandle.Value, (double) base.RightHandle.X, base.RightHandle.Value, (double) base.ConvertBarToX(bars.Count - 2));
                double num3 = bars.Close[bars.Count - 1];
                double num = bars.Close[bars.Count - 2];
                bool flag3 = (num3 > num4) && (num <= num2);
                bool flag2 = (num3 < num4) && (num >= num2);
                if (this.AbovePrice)
                {
                    flag = flag3;
                }
                else
                {
                    flag = flag2;
                }
                if (flag)
                {
                    alertType = this.AlertType;
                    signalName = "Tradable Trendline(" + base.Name + ")";
                    return true;
                }
            }
            return false;
        }

        protected override void Write(BinaryWriter binaryWriter_0)
        {
            base.Write(binaryWriter_0);
            binaryWriter_0.Write((int) this.AlertType);
            binaryWriter_0.Write(this.AbovePrice);
        }

        public override void WriteSettings(ISettingsHost host)
        {
            base.WriteSettings(host);
            string str = "DrawObj." + base.GetType().Name + ".";
            int alertType = (int) this.AlertType;
            host.Set(str + "AlertType", alertType);
        }

        public bool AbovePrice
        {
            get
            {
                return this.bool_7;
            }
            set
            {
                this.bool_7 = value;
            }
        }

        public TradeType AlertType
        {
            get
            {
                return this.tradeType_0;
            }
            set
            {
                this.tradeType_0 = value;
            }
        }

        public override bool CanTriggerAlerts
        {
            get
            {
                return true;
            }
        }

        public override bool ConfineToPricePane
        {
            get
            {
                return true;
            }
        }

        protected override DrawingObjectHelper Helper
        {
            get
            {
                return this.tradableTrendlineHelper_0;
            }
        }

        public override bool ShowToolTip
        {
            get
            {
                return true;
            }
        }

        public override string ToolTipText
        {
            get
            {
                return string.Concat(new object[] { this.AlertType, " ", base.Bars.Symbol, " when price crosses ", this.AbovePrice ? "above" : "below", " trendline ", base.Name });
            }
        }
    }
}

