namespace WealthLab
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using WealthLab.WSDrawingObjects;

    public sealed class ChartPane
    {
        private bool visible;
        private bool scaleUpdateNeeded;   ///WYJ fix, original name: bool_1
        private bool displayGrid;
        private bool isPricePane;
        private bool logScale;
        private bool hidden;
        private bool abovePricePane;
        private ChartRenderer chartRenderer;
        internal Color[] barBackgroundColors;
        private double highestValue;
        private double lowestValue;
        private double scale;   ///WYJ fix, original name: double_2; WYJ note, how much height per value
        private double bottomValue;   ///WYJ fix, original name: double_3
        private double topValue;   ///WYJ fix, original name: double_4
        private double minValue;
        private double maxValue;
        private int rawHeight;
        private int top;
        private int height;
        private int decimals;
        private int labelOffset;
        private int buttonHeight;   ///WYJ fix, original name: int_5
        private int buttonWidth;   ///WYJ fix, original name: int_6
        private int buttonY;   ///WYJ fix, original name: int_7
        private int buttonX;   ///WYJ fix, original name: int_8
        private int heightBeforeHidden;
        private List<PlottedIndicator> plottedIndicators;
        private List<PlottedSymbol> plottedSymbols;
        internal List<WSDrawingObject> wsdObjList_BehindBars;   ///WYJ fix, original name: list_2
        internal List<WSDrawingObject> wsdObjList_BeforeBars;   ///WYJ fix, original name: list_3
        internal List<WSDrawingObject> wsdObjList_DragDropped;   ///WYJ fix, original name: list_4
        private string description;

        public ChartPane()
        {
            this.visible = true;
            this.rawHeight = 100;
            this.scaleUpdateNeeded = true;
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
            this.scaleUpdateNeeded = true;
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
            this.scaleUpdateNeeded = true;
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
            if (renderer.hiddenPanesHeightsContain(this.Description))
            {
                this.Height = renderer.getHiddenPaneHeight(this.Description);
                this.Hidden = true;
            }
        }

        public ChartPane(ChartRenderer renderer, bool addToTop, int rawHeight, string description)
        {
            this.visible = true;
            this.rawHeight = 100;
            this.scaleUpdateNeeded = true;
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
            if (renderer.hiddenPanesHeightsContain(description))
            {
                this.Height = renderer.getHiddenPaneHeight(description);
                this.Hidden = true;
            }
        }

        public int ConvertValueToY(double value)
        {
            this.updateScale();
            if (this.LogScale && (this.bottomValue != 0.0))
            {
                value = Math.Log10(value);
            }
            return (((int) ((this.topValue - value) * this.scale)) + this.top);
        }

        public double ConvertYToValue(int y)
        {
            this.updateScale();
            double value = ((((double) (y - this.top)) / this.scale) - this.topValue) * -1.0;
            if (this.LogScale && (this.bottomValue != 0.0))
            {
                value = Math.Pow(10.0, value);
            }
            return value;
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

        ///WYJ fix, original signature: public bool HideDisplayPaneButton(int int_10, int int_11)
        public bool HideDisplayPaneButton(int x, int y)
        {
            if (this.IsPricePane || !this.Visible)
            {
                return false;
            }
            return (((x >= this.buttonX) && (x <= (this.buttonX + this.buttonWidth))) && ((y >= this.buttonY) && (y <= (this.buttonY + this.buttonHeight))));
        }

        ///WYJ fix, original signature: private void method_0()
        private void updateScale()
        {
            if (this.scaleUpdateNeeded)
            {
                double diff = this.HighestValue - this.LowestValue;
                double num2 = diff * 0.05;
                if (this.LogScale)
                {
                    num2 = 0.0;
                }
                this.topValue = this.HighestValue + num2;
                this.bottomValue = (this.LowestValue == 0.0) ? this.LowestValue : (this.LowestValue - num2);
                if (this.LogScale && (this.bottomValue != 0.0))
                {
                    if (this.bottomValue > 0.0)
                    {
                        this.bottomValue = Math.Log10(this.bottomValue);
                    }
                    if (this.topValue > 0.0)
                    {
                        this.topValue = Math.Log10(this.topValue);
                    }
                }
                if (this.topValue == this.bottomValue)
                {
                    this.scale = 1.0;
                }
                else if (this.LogScale)
                {
                    this.scale = ((double) (this.Height - 20)) / (this.topValue - this.bottomValue);
                }
                else
                {
                    this.scale = ((double) this.Height) / (this.topValue - this.bottomValue);
                }
                this.scaleUpdateNeeded = false;
            }
        }

        ///WYJ fix, original signature: internal void method_1(DataSeries dataSeries_0)
        internal void adjustRange(DataSeries dataSeries_0)
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
            this.scaleUpdateNeeded = true;
        }

        ///WYJ fix, original signature: internal void method_2(double double_7)
        internal void updateHighestLowest(double value)
        {
            if (value > this.HighestValue)
            {
                this.scaleUpdateNeeded = true;
                this.HighestValue = value;
            }
            if (value < this.LowestValue)
            {
                this.scaleUpdateNeeded = true;
                this.LowestValue = value;
            }
        }

        ///WYJ fix, original signature: internal void method_3(WSDrawingObject wsdrawingObject_0)
        internal void addDragDroppedWSDObject(WSDrawingObject wsdrawingObject_0)
        {
            if (this.wsdObjList_DragDropped == null)
            {
                this.wsdObjList_DragDropped = new List<WSDrawingObject>();
            }
            this.wsdObjList_DragDropped.Add(wsdrawingObject_0);
        }

        ///WYJ fix, original signature: internal void method_4(WSDrawingObject wsdrawingObject_0, bool bool_7)
        internal void addWSDObject(WSDrawingObject wsdrawingObject_0, bool behindBars)
        {
            if (behindBars)
            {
                if (this.wsdObjList_BehindBars == null)
                {
                    this.wsdObjList_BehindBars = new List<WSDrawingObject>();
                }
                this.wsdObjList_BehindBars.Add(wsdrawingObject_0);
            }
            else
            {
                if (this.wsdObjList_BeforeBars == null)
                {
                    this.wsdObjList_BeforeBars = new List<WSDrawingObject>();
                }
                this.wsdObjList_BeforeBars.Add(wsdrawingObject_0);
            }
        }

        ///WYJ fix, original signature: internal void method_5(Graphics graphics_0, double double_7, Color color_1)
        ///WYJ note, draw the last bar's value text, showing on the right side of the pane
        internal void drawLastBarValue(Graphics graphics, double value, Color bgrColor)
        {
            string text = this.FormatChartValue(value);
            Color color = ChartRenderer.TextColorForBackground(bgrColor);
            SizeF ef = graphics.MeasureString(text, this.chartRenderer.AxisFont);

            int x = (this.chartRenderer.Width - this.chartRenderer.MarginRightWidth) + 2;
            int y = this.ConvertValueToY(value) - ((int) (ef.Height / 2f));
            Brush bgrBrush = new SolidBrush(bgrColor);
            Brush brush = new SolidBrush(color);
            try
            {
                graphics.FillRectangle(bgrBrush, new RectangleF((float) x, (float) y, ef.Width, ef.Height));
                graphics.DrawString(text, this.chartRenderer.AxisFont, brush, (float) x, (float) (y + 1));
            }
            catch (OverflowException)
            {
            }
            bgrBrush.Dispose();
            brush.Dispose();
        }

        ///WYJ fix, original signature: internal void method_6(Graphics graphics_0)
        internal void drawAllLabels(Graphics graphics_0)
        {
            foreach (PlottedIndicator indicator in this.PlottedIndicators)
            {
                this.drawLabel(graphics_0, indicator.Series.Description, indicator.Color);
            }
            foreach (PlottedSymbol symbol in this.PlottedSymbols)
            {
                this.drawLabel(graphics_0, symbol.Bars.Symbol, symbol.UpColor);
            }
        }

        ///WYJ fix, original signature: internal void method_7(Graphics graphics_0, string string_1, Color color_1)
        ///WYJ note, draw the symbol or indicator name of the pane, along with the hide/show recangle
        internal void drawLabel(Graphics graphics, string labelStr, Color color)
        {
            bool notFirst = this.LabelOffset != 0;
            if (!this.Hidden || !notFirst)
            {
                string str;
                if (this.Hidden && ((this.PlottedIndicators.Count + this.PlottedSymbols.Count) > 1))
                {
                    str = labelStr + " ...";
                }
                else
                {
                    str = labelStr;
                }
                SizeF ef = graphics.MeasureString(str, this.chartRenderer.AxisFont);
                if (!this.IsPricePane && !notFirst)
                {
                    this.drawHideShowButton(graphics, color, (int) ef.Height, (this.Top + 2) + this.LabelOffset);
                }
                Rectangle rect = new Rectangle(6 + this.buttonWidth, (this.Top + 2) + this.LabelOffset, (int) ef.Width, (int) ef.Height);
                graphics.FillRectangle(this.chartRenderer.BackgroundBrush, rect);
                using (Brush brush = new SolidBrush(color))
                {
                    graphics.DrawString(str, this.chartRenderer.AxisFont, brush, (float) rect.Left, (float) rect.Top);
                    this.LabelOffset += rect.Height;
                }
            }
        }

        ///WYJ fix, original signature: private void method_8(Graphics graphics_0, Color color_1, int int_10, int int_11)
        ///WYJ note, draw the hide/show button(the +/- surrounded with rectangle)
        private void drawHideShowButton(Graphics graphics, Color color, int argWidth, int argY)
        {
            int width = ((argWidth % 2) == 0) ? (argWidth - 2) : (argWidth - 3);
            int x = 2;
            int rightX = 2 + width;
            int y = argY;
            int bottomY = y + width;
            this.buttonHeight = width;
            this.buttonWidth = width;
            this.buttonX = 2;
            this.buttonY = argY;
            using (Pen pen = new Pen(color))
            {
                graphics.FillRectangle(this.chartRenderer.BackgroundBrush, x, y, width, width);
                graphics.DrawLine(pen, new Point(x, y), new Point(x, bottomY));
                graphics.DrawLine(pen, new Point(rightX, y), new Point(rightX, bottomY));
                graphics.DrawLine(pen, new Point(x, y), new Point(rightX, y));
                graphics.DrawLine(pen, new Point(x, bottomY), new Point(rightX, bottomY));
                int centerY = y + (width / 2);
                graphics.DrawLine(pen, new Point(x + 2, centerY), new Point(rightX - 2, centerY));
                if (this.Hidden)
                {
                    int centerX = x + (width / 2);
                    graphics.DrawLine(pen, new Point(centerX, y + 2), new Point(centerX, bottomY - 2));
                }
            }
        }

        ///WYJ fix, original signature: internal void method_9()
        internal void linearScaleIfLogScaleNotApplicable()
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
                this.scaleUpdateNeeded = true;
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
                        this.Renderer.saveHiddenPaneHeight(this.Description, this.heightBeforeHidden);
                    }
                    else
                    {
                        this.Renderer.clearHiddenPaneHeight(this.Description);
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
                this.scaleUpdateNeeded = true;
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
                this.scaleUpdateNeeded = true;
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
                this.scaleUpdateNeeded = true;
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

