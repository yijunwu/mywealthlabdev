namespace Steema.TeeChart.Styles
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using System;
    using System.ComponentModel;
    using System.Drawing;

    [ToolboxBitmap(typeof(Calendar), "SeriesIcons.Kagi.bmp")]
    public class Kagi : Custom
    {
        private bool absreversal;
        private SeriesPointer buysymbol;
        private ChartPen downswing;
        private double reversalamount;
        private SeriesPointer sellsymbol;
        private ChartPen upswing;

        public Kagi() : this(null)
        {
        }

        public Kagi(Chart c) : base(c)
        {
            this.reversalamount = 0.03;
            base.Pointer.Draw3D = false;
            base.Pointer.defaultVisible = false;
            base.Pointer.Visible = false;
            base.AllowSinglePoint = false;
            base.vxValues.dateTime = false;
            this.upswing = new ChartPen(c, Color.Green);
            this.upswing.Width = 3;
            this.downswing = new ChartPen(c, Color.Red);
            this.downswing.Width = 1;
            this.buysymbol = new SeriesPointer(c, this);
            this.buysymbol.Style = PointerStyles.Triangle;
            this.buysymbol.Brush.Color = Color.Green;
            this.sellsymbol = new SeriesPointer(c, this);
            this.sellsymbol.Style = PointerStyles.DownTriangle;
            this.sellsymbol.Brush.Color = Color.Red;
        }

        private int CalcSegments(bool draw)
        {
            if (base.Count <= 1)
            {
                return 0;
            }
            Graphics3D graphicsd = base.Chart.Graphics3D;
            int num = 0;
            double[] history = new double[2];
            int index = 1;
            while ((this.CloseValues.Value[index] == this.BasePrice) && (index < base.Count))
            {
                index++;
            }
            if (index < base.Count)
            {
                if (draw)
                {
                    graphicsd.Pen = (this.CloseValues.Value[index] > this.BasePrice) ? this.upswing : this.downswing;
                    this.DrawVertLine((double) num, this.BasePrice, this.CloseValues[index]);
                }
                num++;
                history[0] = this.CloseValues[index];
                history[1] = this.BasePrice;
                double basePrice = this.BasePrice;
                for (int i = index + 1; i < base.Count; i++)
                {
                    if (this.Reversal(this.CloseValues[i], history))
                    {
                        if (draw)
                        {
                            this.DrawHorizLine(history[0], (double) (num - 1), (double) num);
                            bool flag = this.CloseValues.Value[i] > history[0];
                            if (flag && (this.CloseValues.Value[i] > basePrice))
                            {
                                this.DrawVertLine((double) num, history[0], basePrice);
                                if (this.buysymbol.Visible && (graphicsd.Pen == this.downswing))
                                {
                                    base.Pointer.Draw(base.CalcXPosValue((double) num), base.CalcYPosValue(basePrice), this.buysymbol.Brush.Color, this.buysymbol.Style);
                                }
                                graphicsd.Pen = this.upswing;
                                this.DrawVertLine((double) num, basePrice, this.CloseValues[i]);
                            }
                            else if (!flag && (this.CloseValues.Value[i] < basePrice))
                            {
                                this.DrawVertLine((double) num, history[0], basePrice);
                                if (this.sellsymbol.Visible && (graphicsd.Pen == this.upswing))
                                {
                                    base.Pointer.Draw(base.CalcXPosValue((double) num), base.CalcYPosValue(basePrice), this.sellsymbol.Color, this.sellsymbol.Style);
                                }
                                graphicsd.Pen = this.downswing;
                                this.DrawVertLine((double) num, basePrice, this.CloseValues[i]);
                            }
                            else
                            {
                                this.DrawVertLine((double) num, history[0], this.CloseValues[i]);
                            }
                        }
                        num++;
                        history[1] = history[0];
                        history[0] = this.CloseValues[i];
                        basePrice = this.CloseValues[i];
                    }
                    else if (((this.CloseValues[i] - history[0]) * (history[0] - history[1])) > 0.0)
                    {
                        if (draw)
                        {
                            this.DrawVertLine((double) (num - 1), history[0], this.CloseValues[i]);
                        }
                        history[1] = history[0];
                        history[0] = this.CloseValues[i];
                    }
                }
            }
            return num;
        }

        protected internal override int CountLegendItems()
        {
            return 2;
        }

        public override void Draw()
        {
            this.CalcSegments(true);
        }

        private void DrawHorizLine(double y, double fromvalue, double tovalue)
        {
            Graphics3D graphicsd = base.Chart.Graphics3D;
            int num = base.CalcYPosValue(y);
            graphicsd.MoveTo(base.CalcXPosValue(fromvalue), num, base.MiddleZ);
            graphicsd.LineTo(base.CalcXPosValue(tovalue), num, base.MiddleZ);
        }

        private void DrawVertLine(double x, double fromvalue, double tovalue)
        {
            Graphics3D graphicsd = base.Chart.Graphics3D;
            int num = base.CalcXPosValue(x);
            graphicsd.MoveTo(num, base.CalcYPosValue(fromvalue), base.MiddleZ);
            graphicsd.LineTo(num, base.CalcYPosValue(tovalue), base.MiddleZ);
        }

        protected internal override Color LegendItemColor(int index)
        {
            if (index != 0)
            {
                return this.downswing.Color;
            }
            return this.upswing.Color;
        }

        public override string LegendString(int legendIndex, LegendTextStyles legendTextStyle)
        {
            if (legendIndex != 0)
            {
                return Texts.Down;
            }
            return Texts.Up;
        }

        public override double MaxXValue()
        {
            return (double) (this.CalcSegments(false) - 1);
        }

        protected internal override int NumSampleValues()
        {
            return 10;
        }

        public override void PrepareForGallery(bool IsEnabled)
        {
            base.PrepareForGallery(IsEnabled);
            if (!IsEnabled)
            {
                this.buysymbol.Color = Color.Silver;
                this.buysymbol.Pen.Color = Color.Gray;
                this.sellsymbol.Color = Color.Silver;
                this.sellsymbol.Pen.Color = Color.Gray;
            }
        }

        private bool Reversal(double val, params double[] history)
        {
            double num = this.absreversal ? this.reversalamount : (history[0] * this.reversalamount);
            return ((Math.Abs((double) (val - history[0])) > num) & (((val - history[0]) * (history[0] - history[1])) < 0.0));
        }

        protected override void SetChart(Chart value)
        {
            base.SetChart(value);
            if (this.sellsymbol != null)
            {
                this.sellsymbol.Chart = value;
            }
            if (this.buysymbol != null)
            {
                this.buysymbol.Chart = value;
            }
            if (this.downswing != null)
            {
                this.downswing.Chart = value;
            }
            if (this.upswing != null)
            {
                this.upswing.Chart = value;
            }
        }

        [DefaultValue(false), Description("Defines if reversal amount is treated as absolute or relative value.")]
        public bool AbsoluteReversal
        {
            get
            {
                return this.absreversal;
            }
            set
            {
                base.SetBooleanProperty(ref this.absreversal, value);
            }
        }

        public double BasePrice
        {
            get
            {
                if (base.Count <= 0)
                {
                    return 0.0;
                }
                return this.CloseValues[0];
            }
        }

        public SeriesPointer BuySymbol
        {
            get
            {
                return this.buysymbol;
            }
        }

        [Description("Gets and sets all Stock market closing values.")]
        public ValueList CloseValues
        {
            get
            {
                return base.vyValues;
            }
        }

        public override string Description
        {
            get
            {
                return Texts.GalleryKagi;
            }
        }

        [DefaultValue((string) null), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Pen used to draw \"downswing\" lines."), Category("Appearance")]
        public ChartPen DownSwing
        {
            get
            {
                if (this.downswing == null)
                {
                    this.downswing = new ChartPen(base.Chart, Color.Red);
                }
                return this.downswing;
            }
        }

        [Description("Defines the reversal amount."), DefaultValue((double) 0.03)]
        public double ReversalAmount
        {
            get
            {
                return this.reversalamount;
            }
            set
            {
                base.SetDoubleProperty(ref this.reversalamount, value);
            }
        }

        public SeriesPointer SellSymbol
        {
            get
            {
                return this.sellsymbol;
            }
        }

        [Description("Pen used to draw \"upswing\" lines."), DefaultValue((string) null), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("Appearance")]
        public ChartPen UpSwing
        {
            get
            {
                if (this.upswing == null)
                {
                    this.upswing = new ChartPen(base.Chart, Color.Green);
                }
                return this.upswing;
            }
        }
    }
}

