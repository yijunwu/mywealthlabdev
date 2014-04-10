namespace WealthLab
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using WealthLab.WSDrawingObjects;

    public sealed class ChartPane
    {
        private bool visible;
        private bool bool_1;
        private bool displayGrid;
        private bool isPricePane;
        private bool logScale;
        private bool hidden;
        private bool abovePricePane;
        private ChartRenderer chartRenderer;
        internal Color[] barBackgroundColors;
        private double highestValue;
        private double lowestValue;
        private double double_2;
        private double double_3;
        private double double_4;
        private double minValue;
        private double maxValue;
        private int rawHeight;
        private int top;
        private int height;
        private int decimals;
        private int labelOffset;
        private int int_5;
        private int int_6;
        private int int_7;
        private int int_8;
        private int heightBeforeHidden;
        private List<PlottedIndicator> plottedIndicators;
        private List<PlottedSymbol> plottedSymbols;
        internal List<WSDrawingObject> list_2;
        internal List<WSDrawingObject> list_3;
        internal List<WSDrawingObject> list_4;
        private string description;

        public ChartPane()
        {
            this.visible = true;
            this.rawHeight = 100;
            this.bool_1 = true;
            this.decimals = 2;
            this.plottedIndicators = new List<PlottedIndicator>();
            this.displayGrid = true;
            this.plottedSymbols = new List<PlottedSymbol>();
            this.minValue = double.MaxValue;
            this.maxValue = double.MinValue;
        }

        public ChartPane(ChartRenderer renderer, bool addToTop)
        {
            this.visible = true;
            this.rawHeight = 100;
            this.bool_1 = true;
            this.decimals = 2;
            this.plottedIndicators = new List<PlottedIndicator>();
            this.displayGrid = true;
            this.plottedSymbols = new List<PlottedSymbol>();
            this.minValue = double.MaxValue;
            this.maxValue = double.MinValue;
            this.chartRenderer = renderer;
            if (addToTop)
            {
                renderer.Panes.Insert(0, this);
                this.abovePricePane = true;
            }
            else
            {
                renderer.Panes.Add(this);
                this.abovePricePane = false;
            }
        }

        public ChartPane(ChartRenderer renderer, bool addToTop, int rawHeight, bool fromWealthScript)
        {
            string str;
            this.visible = true;
            this.rawHeight = 100;
            this.bool_1 = true;
            this.decimals = 2;
            this.plottedIndicators = new List<PlottedIndicator>();
            this.displayGrid = true;
            this.plottedSymbols = new List<PlottedSymbol>();
            this.minValue = double.MaxValue;
            this.maxValue = double.MinValue;
            this.chartRenderer = renderer;
            this.rawHeight = rawHeight;
            if (fromWealthScript)
            {
                str = "W";
            }
            else
            {
                str = "D";
            }
            int num = 1;
            do
            {
                this.description = str + num;
                num++;
            }
            while (this.chartRenderer.FindPane(this.description) != null);
            if (addToTop)
            {
                renderer.Panes.Insert(0, this);
                this.abovePricePane = true;
            }
            else
            {
                renderer.Panes.Add(this);
                this.abovePricePane = false;
            }
            if (renderer.method_5(this.Description))
            {
                this.Height = renderer.method_2(this.Description);
                this.Hidden = true;
            }
        }

        public ChartPane(ChartRenderer renderer, bool addToTop, int rawHeight, string description)
        {
            this.visible = true;
            this.rawHeight = 100;
            this.bool_1 = true;
            this.decimals = 2;
            this.plottedIndicators = new List<PlottedIndicator>();
            this.displayGrid = true;
            this.plottedSymbols = new List<PlottedSymbol>();
            this.minValue = double.MaxValue;
            this.maxValue = double.MinValue;
            this.chartRenderer = renderer;
            this.rawHeight = rawHeight;
            this.description = description;
            if (addToTop)
            {
                renderer.Panes.Insert(0, this);
                this.abovePricePane = true;
            }
            else
            {
                renderer.Panes.Add(this);
                this.abovePricePane = false;
            }
            if (renderer.method_5(description))
            {
                this.Height = renderer.method_2(description);
                this.Hidden = true;
            }
        }

        public int ConvertValueToY(double value)
        {
            this.method_0();
            if (this.LogScale && (this.double_3 != 0.0))
            {
                value = Math.Log10(value);
            }
            return (((int) ((this.double_4 - value) * this.double_2)) + this.top);
        }

        public double ConvertYToValue(int int_10)
        {
            this.method_0();
            double y = ((((double) (int_10 - this.top)) / this.double_2) - this.double_4) * -1.0;
            if (this.LogScale && (this.double_3 != 0.0))
            {
                y = Math.Pow(10.0, y);
            }
            return y;
        }

        public string FormatChartValue(double value)
        {
            string str;
            if (this.IsPricePane)
            {
                str = "N" + this.Renderer.Bars.SymbolInfo.Decimals;
            }
            else if (this.Description == "V")
            {
                str = "N" + this.Decimals;
            }
            else
            {
                str = "N" + DecimalsManager.Instance.Indicator;
            }
            if (!this.IsPricePane)
            {
                double num = Math.Abs(value);
                if (num >= 1000000000000)
                {
                    value /= 1000000000000;
                    return (value.ToString(str) + "T");
                }
                if (num >= 1000000000.0)
                {
                    value /= 1000000000.0;
                    return (value.ToString(str) + "B");
                }
                if (num >= 1000000.0)
                {
                    value /= 1000000.0;
                    return (value.ToString(str) + "M");
                }
                if (num >= 10000.0)
                {
                    value /= 1000.0;
                    return (value.ToString(str) + "K");
                }
            }
            return value.ToString(str);
        }

        public Color GetBackgroundColor(int int_10)
        {
            if (this.barBackgroundColors == null)
            {
                if (this.chartRenderer == null)
                {
                    return Color.Empty;
                }
                return this.chartRenderer.BackgroundColor;
            }
            if (this.barBackgroundColors[int_10] == Color.Empty)
            {
                return this.chartRenderer.BackgroundColor;
            }
            return this.barBackgroundColors[int_10];
        }

        public bool HideDisplayPaneButton(int int_10, int int_11)
        {
            if (this.IsPricePane || !this.Visible)
            {
                return false;
            }
            return (((int_10 >= this.int_8) && (int_10 <= (this.int_8 + this.int_6))) && ((int_11 >= this.int_7) && (int_11 <= (this.int_7 + this.int_5))));
        }

        private void method_0()
        {
            if (this.bool_1)
            {
                double num = this.HighestValue - this.LowestValue;
                double num2 = num * 0.05;
                if (this.LogScale)
                {
                    num2 = 0.0;
                }
                this.double_4 = this.HighestValue + num2;
                this.double_3 = (this.LowestValue == 0.0) ? this.LowestValue : (this.LowestValue - num2);
                if (this.LogScale && (this.double_3 != 0.0))
                {
                    if (this.double_3 > 0.0)
                    {
                        this.double_3 = Math.Log10(this.double_3);
                    }
                    if (this.double_4 > 0.0)
                    {
                        this.double_4 = Math.Log10(this.double_4);
                    }
                }
                if (this.double_4 == this.double_3)
                {
                    this.double_2 = 1.0;
                }
                else if (this.LogScale)
                {
                    this.double_2 = ((double) (this.Height - 20)) / (this.double_4 - this.double_3);
                }
                else
                {
                    this.double_2 = ((double) this.Height) / (this.double_4 - this.double_3);
                }
                this.bool_1 = false;
            }
        }

        internal void method_1(DataSeries dataSeries_0)
        {
            for (int i = this.Renderer.RightEdgeBar; i >= this.Renderer.LeftEdgeBar; i--)
            {
                if (i >= dataSeries_0.FirstValidValue)
                {
                    double d = dataSeries_0[i];
                    if (!double.IsInfinity(d))
                    {
                        if (d > this.HighestValue)
                        {
                            this.HighestValue = d;
                        }
                        if (d < this.LowestValue)
                        {
                            this.LowestValue = d;
                        }
                    }
                }
            }
            if ((this.HighestValue <= this.LowestValue) && (this.HighestValue != double.MinValue))
            {
                this.HighestValue = this.LowestValue + 0.1;
            }
            this.bool_1 = true;
        }

        internal void method_2(double double_7)
        {
            if (double_7 > this.HighestValue)
            {
                this.bool_1 = true;
                this.HighestValue = double_7;
            }
            if (double_7 < this.LowestValue)
            {
                this.bool_1 = true;
                this.LowestValue = double_7;
            }
        }

        internal void method_3(WSDrawingObject wsdrawingObject_0)
        {
            if (this.list_4 == null)
            {
                this.list_4 = new List<WSDrawingObject>();
            }
            this.list_4.Add(wsdrawingObject_0);
        }

        internal void method_4(WSDrawingObject wsdrawingObject_0, bool bool_7)
        {
            if (bool_7)
            {
                if (this.list_2 == null)
                {
                    this.list_2 = new List<WSDrawingObject>();
                }
                this.list_2.Add(wsdrawingObject_0);
            }
            else
            {
                if (this.list_3 == null)
                {
                    this.list_3 = new List<WSDrawingObject>();
                }
                this.list_3.Add(wsdrawingObject_0);
            }
        }

        internal void method_5(Graphics graphics_0, double double_7, Color color_1)
        {
            string text = this.FormatChartValue(double_7);
            Color color = ChartRenderer.TextColorForBackground(color_1);
            SizeF ef = graphics_0.MeasureString(text, this.chartRenderer.AxisFont);
            int num = (this.chartRenderer.Width - this.chartRenderer.MarginRightWidth) + 2;
            int num2 = this.ConvertValueToY(double_7) - ((int) (ef.Height / 2f));
            Brush brush = new SolidBrush(color_1);
            Brush brush2 = new SolidBrush(color);
            try
            {
                graphics_0.FillRectangle(brush, new RectangleF((float) num, (float) num2, ef.Width, ef.Height));
                graphics_0.DrawString(text, this.chartRenderer.AxisFont, brush2, (float) num, (float) (num2 + 1));
            }
            catch (OverflowException)
            {
            }
            brush.Dispose();
            brush2.Dispose();
        }

        internal void method_6(Graphics graphics_0)
        {
            foreach (PlottedIndicator indicator in this.PlottedIndicators)
            {
                this.method_7(graphics_0, indicator.Series.Description, indicator.Color);
            }
            foreach (PlottedSymbol symbol in this.PlottedSymbols)
            {
                this.method_7(graphics_0, symbol.Bars.Symbol, symbol.UpColor);
            }
        }

        internal void method_7(Graphics graphics_0, string string_1, Color color_1)
        {
            bool flag = this.LabelOffset != 0;
            if (!this.Hidden || !flag)
            {
                string str;
                if (this.Hidden && ((this.PlottedIndicators.Count + this.PlottedSymbols.Count) > 1))
                {
                    str = string_1 + " ...";
                }
                else
                {
                    str = string_1;
                }
                SizeF ef = graphics_0.MeasureString(str, this.chartRenderer.AxisFont);
                if (!this.IsPricePane && !flag)
                {
                    this.method_8(graphics_0, color_1, (int) ef.Height, (this.Top + 2) + this.LabelOffset);
                }
                Rectangle rect = new Rectangle(6 + this.int_6, (this.Top + 2) + this.LabelOffset, (int) ef.Width, (int) ef.Height);
                graphics_0.FillRectangle(this.chartRenderer.BackgroundBrush, rect);
                using (Brush brush = new SolidBrush(color_1))
                {
                    graphics_0.DrawString(str, this.chartRenderer.AxisFont, brush, (float) rect.Left, (float) rect.Top);
                    this.LabelOffset += rect.Height;
                }
            }
        }

        private void method_8(Graphics graphics_0, Color color_1, int int_10, int int_11)
        {
            int width = ((int_10 % 2) == 0) ? (int_10 - 2) : (int_10 - 3);
            int x = 2;
            int num5 = 2 + width;
            int y = int_11;
            int num4 = y + width;
            this.int_5 = width;
            this.int_6 = width;
            this.int_8 = 2;
            this.int_7 = int_11;
            using (Pen pen = new Pen(color_1))
            {
                graphics_0.FillRectangle(this.chartRenderer.BackgroundBrush, x, y, width, width);
                graphics_0.DrawLine(pen, new Point(x, y), new Point(x, num4));
                graphics_0.DrawLine(pen, new Point(num5, y), new Point(num5, num4));
                graphics_0.DrawLine(pen, new Point(x, y), new Point(num5, y));
                graphics_0.DrawLine(pen, new Point(x, num4), new Point(num5, num4));
                int num6 = y + (width / 2);
                graphics_0.DrawLine(pen, new Point(x + 2, num6), new Point(num5 - 2, num6));
                if (this.Hidden)
                {
                    int num7 = x + (width / 2);
                    graphics_0.DrawLine(pen, new Point(num7, y + 2), new Point(num7, num4 - 2));
                }
            }
        }

        internal void method_9()
        {
            if (this.LogScale)
            {
                if ((this.LowestValue <= 0.0) && (this.HighestValue > 0.0))
                {
                    this.LogScale = false;
                }
                if ((this.LowestValue < 0.0) && (this.HighestValue >= 0.0))
                {
                    this.LogScale = false;
                }
            }
        }

        public void SetBackgroundColor(int bar, Color color)
        {
            if (this.chartRenderer != null)
            {
                if (this.barBackgroundColors == null)
                {
                    this.barBackgroundColors = new Color[this.chartRenderer.Bars.Count];
                    for (int i = 0; i < this.chartRenderer.Bars.Count; i++)
                    {
                        this.barBackgroundColors[i] = Color.Empty;
                    }
                }
                this.barBackgroundColors[bar] = color;
            }
        }

        public bool AbovePricePane
        {
            get
            {
                return this.abovePricePane;
            }
        }

        public int BarInterval
        {
            get
            {
                return this.Renderer.Bars.BarInterval;
            }
        }

        public int Decimals
        {
            get
            {
                return this.decimals;
            }
            set
            {
                this.decimals = value;
            }
        }

        public string Description
        {
            get
            {
                return this.description;
            }
            internal set
            {
                this.description = value;
            }
        }

        public bool DisplayGrid
        {
            get
            {
                return this.displayGrid;
            }
            internal set
            {
                this.displayGrid = value;
            }
        }

        public int Height
        {
            get
            {
                return this.height;
            }
            internal set
            {
                this.height = value;
                this.bool_1 = true;
            }
        }

        internal int HeightBeforeHidden
        {
            get
            {
                return this.heightBeforeHidden;
            }
        }

        public bool Hidden
        {
            get
            {
                return this.hidden;
            }
            internal set
            {
                if (!this.IsPricePane)
                {
                    this.hidden = value;
                    if (this.hidden)
                    {
                        this.heightBeforeHidden = this.Height;
                        this.Renderer.method_3(this.Description, this.heightBeforeHidden);
                    }
                    else
                    {
                        this.Renderer.method_4(this.Description);
                    }
                }
            }
        }

        public int HiddenHeight
        {
            get
            {
                return (this.Renderer.AxisFont.Height + 4);
            }
        }

        public string HideDisplayPaneTooltip
        {
            get
            {
                if (this.Hidden)
                {
                    return "Display the Pane";
                }
                return "Hide the Pane";
            }
        }

        internal double HighestValue
        {
            get
            {
                return this.highestValue;
            }
            set
            {
                this.highestValue = value;
            }
        }

        public bool IsPricePane
        {
            get
            {
                return this.isPricePane;
            }
            internal set
            {
                this.isPricePane = value;
            }
        }

        internal int LabelOffset
        {
            get
            {
                return this.labelOffset;
            }
            set
            {
                this.labelOffset = value;
            }
        }

        public bool LogScale
        {
            get
            {
                return this.logScale;
            }
            set
            {
                this.logScale = value;
                this.bool_1 = true;
            }
        }

        internal double LowestValue
        {
            get
            {
                return this.lowestValue;
            }
            set
            {
                this.lowestValue = value;
            }
        }

        public double MaxValue
        {
            get
            {
                return this.maxValue;
            }
            set
            {
                this.maxValue = value;
            }
        }

        public double MinValue
        {
            get
            {
                return this.minValue;
            }
            set
            {
                this.minValue = value;
            }
        }

        public IList<PlottedIndicator> PlottedIndicators
        {
            get
            {
                return this.plottedIndicators;
            }
        }

        public IList<PlottedSymbol> PlottedSymbols
        {
            get
            {
                return this.plottedSymbols;
            }
        }

        internal int RawHeight
        {
            get
            {
                return this.rawHeight;
            }
            set
            {
                this.rawHeight = value;
                this.bool_1 = true;
            }
        }

        internal ChartRenderer Renderer
        {
            get
            {
                return this.chartRenderer;
            }
        }

        public BarScale Scale
        {
            get
            {
                return this.Renderer.Bars.Scale;
            }
        }

        public PlottedIndicator SelectedIndicator
        {
            get
            {
                PlottedIndicator indicator2;
                using (IEnumerator<PlottedIndicator> enumerator = this.PlottedIndicators.GetEnumerator())
                {
                    PlottedIndicator current;
                    while (enumerator.MoveNext())
                    {
                        current = enumerator.Current;
                        if (current.Selected)
                        {
                            ///goto  Label_0028;  ///WYJ fix, simplify the flow
                            indicator2 = current;
                            return indicator2;
                        }
                    }
                    return null;
                }
            }
        }

        public int Top
        {
            get
            {
                return this.top;
            }
            internal set
            {
                this.top = value;
                this.bool_1 = true;
            }
        }

        public bool Visible
        {
            get
            {
                return this.visible;
            }
            internal set
            {
                this.visible = value;
            }
        }
    }
}

