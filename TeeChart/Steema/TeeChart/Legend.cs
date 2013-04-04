namespace Steema.TeeChart
{
    using Steema.TeeChart.Drawing;
    using Steema.TeeChart.Editors;
    using Steema.TeeChart.Styles;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Design;
    using System.Drawing.Drawing2D;

    [Editor(typeof(Legend.LegendEditor), typeof(UITypeEditor)), ToolboxItem(false)]
    public class Legend : TextShapePosition
    {
        private LegendAlignments alignment;
        private const int AllValues = -1;
        private bool checkBoxes;
        private const int CheckBoxSize = 11;
        public bool ColumnWidthAuto;
        public int[] ColumnWidths;
        private bool currentPage;
        private ChartPen dividingLines;
        internal int firstValue;
        private bool fontSeriesColor;
        private int FrameWidth;
        private int horizMargin;
        private int iColorWidth;
        private bool iDrawTitle;
        private string[,] iItems;
        internal int iLastValue;
        internal LegendStyles iLegendStyle;
        private int iManualHeight;
        private int iManualWidth;
        private bool IncPos;
        private bool inverted;
        internal int iSpaceWidth;
        private int ItemHeight;
        private LegendItems items;
        internal int iTotalItems;
        private const int LegendOff2 = 2;
        private const int LegendOff4 = 4;
        private LegendStyles legendStyle;
        private const int MaxLegendColumns = 2;
        private int maxNumRows;
        private int NumCols;
        private int NumRows;
        internal int posXLegend;
        internal int posYLegend;
        private bool resizeChart;
        private Steema.TeeChart.Styles.Series series;
        private LegendSymbol symbol;
        private LegendTextStyles textStyle;
        private LegendTitle title;
        private int tmpMaxWidth;
        private Steema.TeeChart.Styles.Series tmpSeries;
        private int tmpTotalWidth;
        private int topLeftPos;
        private int vertMargin;
        private int vertSpacing;
        private int XLegendBox;
        private int XLegendColor;
        private int XLegendText;

        public Legend() : this(null)
        {
        }

        public Legend(Chart c) : base(c)
        {
            this.resizeChart = true;
            this.textStyle = LegendTextStyles.LeftValue;
            this.currentPage = true;
            this.topLeftPos = 10;
            this.alignment = LegendAlignments.Right;
            this.maxNumRows = 10;
            this.ColumnWidthAuto = true;
            this.ColumnWidths = new int[10];
            this.iItems = new string[100, 10];
            base.Shadow.Brush.defaultColor = System.Drawing.Color.Black;
            base.shadow.Color = System.Drawing.Color.Black;
            base.shadow.Width = 3;
            base.shadow.Height = 3;
            base.shadow.Transparency = 0;
            base.shadow.defaultVisible = true;
            base.shadow.bVisible = base.shadow.defaultVisible;
            base.Pen.Color = System.Drawing.Color.Black;
            base.Pen.Visible = true;
            base.Pen.Style = DashStyle.Solid;
        }

        protected override void AssignShape(Steema.TeeChart.Drawing.Shape s)
        {
            base.AssignShape(s);
            Legend legend = s as Legend;
            if (legend != null)
            {
                legend.Alignment = this.Alignment;
                legend.CheckBoxes = this.CheckBoxes;
                legend.Color = this.Color;
                legend.CurrentPage = this.CurrentPage;
                legend.DividingLines = this.DividingLines.Clone() as ChartPen;
                legend.FirstValue = this.FirstValue;
                legend.FontSeriesColor = this.FontSeriesColor;
                legend.HorizMargin = this.HorizMargin;
                legend.Inverted = this.Inverted;
                legend.LegendStyle = this.LegendStyle;
                legend.MaxNumRows = this.MaxNumRows;
                legend.ResizeChart = this.ResizeChart;
                legend.Series = this.Series;
                legend.Symbol = this.Symbol;
                legend.Text = this.Text;
                legend.Title = this.Title;
                legend.TopLeftPos = this.TopLeftPos;
                legend.VertMargin = this.VertMargin;
                legend.VertSpacing = this.VertSpacing;
            }
        }

        private int CalcColumnsWidth(int numLegendValues)
        {
            if (this.ColumnWidthAuto)
            {
                Graphics3D graphicsd = base.chart.graphics3D;
                for (int j = 0; j < 2; j++)
                {
                    this.ColumnWidths[j] = 0;
                    for (int k = this.firstValue; k <= this.iLastValue; k++)
                    {
                        string text = null;
                        if (this.Items.Custom && (this.Items.Count > (k - this.firstValue)))
                        {
                            if (j == 0)
                            {
                                text = this.Items[k - this.firstValue].Text;
                            }
                            else
                            {
                                text = this.Items[k - this.firstValue].Text2;
                            }
                        }
                        else
                        {
                            text = this.iItems[k - this.firstValue, j];
                        }
                        if (text != null)
                        {
                            this.ColumnWidths[j] = Math.Max(this.ColumnWidths[j], Utils.Round(graphicsd.TextWidth(text)));
                        }
                    }
                }
            }
            int num3 = (this.iSpaceWidth * 2) - 1;
            for (int i = 0; i < 2; i++)
            {
                num3 += this.ColumnWidths[i];
            }
            return num3;
        }

        private void CalcHorizontalPositions()
        {
            int iLeft;
            int iManualWidth;
            if (base.AutoSize)
            {
                iManualWidth = (10 + ((this.tmpMaxWidth + this.iColorWidth) * this.NumCols)) + (6 * (this.NumCols - 1));
            }
            else
            {
                iManualWidth = this.iManualWidth;
            }
            if (this.HasCheckBoxes())
            {
                iManualWidth += 4 + (13 * this.NumCols);
                iManualWidth += 8;
            }
            iManualWidth = Math.Min(base.chart.Width, iManualWidth) / 2;
            if (!base.CustomPosition)
            {
                if (base.AutoSize)
                {
                    iLeft = Utils.Round((double) (((1.0 * this.topLeftPos) * ((base.chart.Right - base.chart.Left) - (2 * iManualWidth))) * 0.01));
                    base.iLeft = (base.chart.Graphics3D.ChartXCenter - iManualWidth) + iLeft;
                }
                else
                {
                    base.iLeft = base.chart.Graphics3D.ChartXCenter - iManualWidth;
                }
            }
            base.iRight = base.iLeft + (2 * iManualWidth);
            iLeft = base.iLeft;
            if (this.HasCheckBoxes())
            {
                this.XLegendBox = base.iLeft + 4;
                iLeft += 15;
            }
            this.CalcSymbolTextPos(iLeft);
            this.SetShapeWidthHeight();
        }

        internal int CalcItemHeight()
        {
            int fontHeight = base.chart.graphics3D.FontHeight;
            if (this.HasCheckBoxes())
            {
                fontHeight = Math.Max(0x11, fontHeight);
            }
            fontHeight += this.vertSpacing;
            if ((this.Vertical && (this.dividingLines != null)) && this.dividingLines.Visible)
            {
                fontHeight += Utils.Round((float) this.dividingLines.Width);
            }
            return fontHeight;
        }

        private void CalcLegendStyle()
        {
            if (this.legendStyle == LegendStyles.Auto)
            {
                if (this.CheckBoxes || (base.chart.CountActiveSeries() > 1))
                {
                    this.iLegendStyle = LegendStyles.Series;
                }
                else if (this.GetLegendSeries() is Custom3DPalette)
                {
                    this.iLegendStyle = LegendStyles.Palette;
                }
                else
                {
                    this.iLegendStyle = LegendStyles.Values;
                }
            }
            else
            {
                this.iLegendStyle = this.legendStyle;
            }
        }

        private int CalcMargin(int margin, int defaultMargin, int size)
        {
            if (margin != 0)
            {
                return margin;
            }
            return ((defaultMargin * size) / 100);
        }

        private int CalcMaxLegendValues(int yLegend, int a, int b, int c, int itemHeight)
        {
            if ((yLegend < a) && (itemHeight > 0))
            {
                return Math.Min(Utils.Round((float) ((((b - (2 * this.Frame.Width)) - yLegend) + c) / itemHeight)), this.iTotalItems);
            }
            return 0;
        }

        private void CalcSymbolTextPos(int leftPos)
        {
            int num = (leftPos + 2) + 4;
            if (this.Symbol.position == LegendSymbolPosition.Left)
            {
                this.XLegendColor = num;
                this.XLegendText = (this.XLegendColor + this.iColorWidth) + 4;
            }
            else
            {
                this.XLegendText = num;
                this.XLegendColor = this.XLegendText + this.tmpMaxWidth;
            }
        }

        private int CalcTotalItems()
        {
            int num = 0;
            if ((this.iLegendStyle == LegendStyles.Series) || (this.iLegendStyle == LegendStyles.LastValues))
            {
                foreach (Steema.TeeChart.Styles.Series series in base.chart.Series)
                {
                    if ((this.CheckBoxes || series.Active) && series.ShowInLegend)
                    {
                        num++;
                    }
                }
                num -= this.firstValue;
            }
            else
            {
                Steema.TeeChart.Styles.Series legendSeries = this.GetLegendSeries();
                if ((legendSeries != null) && legendSeries.ShowInLegend)
                {
                    num = legendSeries.CountLegendItems() - this.firstValue;
                }
            }
            return Math.Max(0, num);
        }

        private void CalcVerticalPositions()
        {
            int iLeft;
            if (base.CustomPosition || (this.Alignment == LegendAlignments.Left))
            {
                if (!base.CustomPosition && this.Frame.Visible)
                {
                    base.iLeft += this.FrameWidth + 1;
                }
                this.CalcWidths();
            }
            else
            {
                if (base.Shadow.Visible)
                {
                    base.iRight -= Math.Max(0, base.Shadow.Width);
                }
                if (this.Frame.Visible)
                {
                    base.iRight -= this.FrameWidth + 1;
                }
                this.CalcWidths();
                base.iLeft = base.iRight - this.tmpTotalWidth;
                if (this.HasCheckBoxes())
                {
                    base.iLeft -= 15;
                }
            }
            if (this.HasCheckBoxes())
            {
                this.XLegendBox = base.iLeft + 4;
                iLeft = this.XLegendBox + 11;
            }
            else
            {
                iLeft = base.iLeft;
            }
            base.iRight = iLeft + this.tmpTotalWidth;
            this.CalcSymbolTextPos(iLeft);
            this.SetShapeWidthHeight();
        }

        private void CalcWidths()
        {
            this.tmpMaxWidth = this.CalcColumnsWidth(this.NumRows);
            this.tmpTotalWidth = (8 + this.tmpMaxWidth) + 2;
            this.iColorWidth = this.Symbol.CalcWidth(this.tmpTotalWidth);
            if (base.AutoSize)
            {
                this.tmpTotalWidth += this.iColorWidth + 2;
                if (this.iDrawTitle)
                {
                    this.tmpTotalWidth = Math.Max(this.tmpTotalWidth, this.Title.TotalWidth());
                }
            }
            else
            {
                this.tmpTotalWidth = this.iManualWidth;
            }
            if (base.Chart.parent != null)
            {
                base.Chart.parent.DoGetLegendSize(this, ref this.tmpTotalWidth);
            }
        }

        public int Clicked(int x, int y)
        {
            int num4 = -1;
            if (this.GetLegendBounds().Contains(x, y))
            {
                base.chart.graphics3D.Font = base.Font;
                int tmpH = this.CalcItemHeight();
                if (this.Vertical)
                {
                    if (this.NumRows > 0)
                    {
                        num4 = this.ClickedRow(tmpH, y) + this.FirstValue;
                    }
                    return num4;
                }
                if (this.NumCols <= 0)
                {
                    return num4;
                }
                int num2 = (base.ShapeBounds.Right - base.ShapeBounds.Left) / this.NumCols;
                for (int i = 0; i < this.NumCols; i++)
                {
                    int num = (base.ShapeBounds.Left + 1) + (i * num2);
                    if ((x >= num) && (x <= (num + num2)))
                    {
                        num4 = this.ClickedRow(tmpH, y);
                        if (num4 != -1)
                        {
                            num4 = this.NumCols * num4;
                            if (this.Inverted)
                            {
                                num4 += (this.NumCols - i) - 1;
                                int num3 = ((this.iLastValue - this.FirstValue) + 1) % this.NumCols;
                                if (num3 > 0)
                                {
                                    num4 -= this.NumCols - num3;
                                }
                            }
                            else
                            {
                                num4 += i;
                            }
                            if (num4 > (this.iTotalItems - 1))
                            {
                                num4 = -1;
                            }
                            else
                            {
                                num4 += this.FirstValue;
                            }
                        }
                    }
                }
            }
            return num4;
        }

        private int ClickedRow(int tmpH, int y)
        {
            int num = -1;
            for (int i = 0; i < this.NumRows; i++)
            {
                int num3 = (this.FirstItemTop() + 1) + (i * tmpH);
                if ((y >= num3) && (y <= (num3 + tmpH)))
                {
                    num = i;
                    if (this.Inverted)
                    {
                        num = (this.NumRows - i) - 1;
                    }
                    return num;
                }
            }
            return num;
        }

        protected internal bool DoMouseDown(int x, int y)
        {
            bool flag = false;
            if (this.HasCheckBoxes())
            {
                int itemIndex = this.Clicked(x, y);
                if (itemIndex != -1)
                {
                    Steema.TeeChart.Styles.Series series = base.chart.SeriesLegend(itemIndex, false);
                    series.Active = !series.Active;
                    flag = true;
                }
            }
            return flag;
        }

        private void DrawItems()
        {
            this.tmpSeries = this.GetLegendSeries();
            if (!this.Items.Custom)
            {
                this.Items.Clear();
            }
            if (this.inverted)
            {
                for (int i = this.iLastValue; i >= this.firstValue; i--)
                {
                    this.DrawLegendItem(i, this.iLastValue - i);
                }
            }
            else
            {
                for (int j = this.firstValue; j <= this.iLastValue; j++)
                {
                    this.DrawLegendItem(j, j - this.firstValue);
                }
            }
        }

        private void DrawLegendItem(int ItemIndex, int ItemOrder)
        {
            Rectangle r = new Rectangle();
            if (ItemOrder < this.iTotalItems)
            {
                int num3;
                Graphics3D graphicsd = base.chart.graphics3D;
                this.PrepareSymbolPen();
                int xLegendText = this.XLegendText;
                int num5 = 0;
                this.posYLegend = this.FirstItemTop() + 1;
                int xLegendColor = this.XLegendColor;
                if (this.Vertical)
                {
                    num3 = ItemOrder;
                }
                else
                {
                    num3 = ItemOrder / this.NumCols;
                    num5 = ((this.tmpMaxWidth + this.iColorWidth) + 4) + 2;
                    if (this.HasCheckBoxes())
                    {
                        num5 += 15;
                    }
                    num5 *= ItemOrder % this.NumCols;
                    xLegendText += num5;
                    xLegendColor += num5;
                }
                this.posYLegend += (num3 * this.ItemHeight) + (this.vertSpacing / 2);
                if (base.chart.parent != null)
                {
                    base.chart.parent.DoGetLegendPos(this, ItemIndex, ref xLegendText, ref this.posYLegend, ref xLegendColor);
                }
                this.posXLegend = xLegendText;
                if (this.FontSeriesColor)
                {
                    if ((this.iLegendStyle == LegendStyles.Series) || (this.iLegendStyle == LegendStyles.LastValues))
                    {
                        graphicsd.Font.Color = base.chart.SeriesLegend(ItemIndex, !this.CheckBoxes).Color;
                    }
                    else
                    {
                        graphicsd.Font.Color = this.tmpSeries.LegendItemColor(ItemIndex);
                    }
                }
                for (int i = 0; i < 2; i++)
                {
                    string itemText = this.GetItemText(ItemOrder, i);
                    this.IncPos = true;
                    if (itemText != null)
                    {
                        if (this.iLegendStyle == LegendStyles.Series)
                        {
                            graphicsd.TextAlign = StringAlignment.Near;
                        }
                        else
                        {
                            switch (i)
                            {
                                case 0:
                                    if ((((this.textStyle == LegendTextStyles.XValue) || (this.textStyle == LegendTextStyles.Value)) || ((this.textStyle == LegendTextStyles.Percent) || (this.textStyle == LegendTextStyles.XAndValue))) || (((this.textStyle == LegendTextStyles.XAndPercent) || (this.textStyle == LegendTextStyles.LeftPercent)) || (this.textStyle == LegendTextStyles.LeftValue)))
                                    {
                                        this.SetRightAlign(i, true);
                                    }
                                    else
                                    {
                                        this.SetRightAlign(i, false);
                                    }
                                    break;

                                case 1:
                                    if (((this.textStyle == LegendTextStyles.RightValue) || (this.textStyle == LegendTextStyles.XAndValue)) || ((this.textStyle == LegendTextStyles.XAndPercent) || (this.textStyle == LegendTextStyles.RightPercent)))
                                    {
                                        this.SetRightAlign(i, true);
                                    }
                                    else
                                    {
                                        this.SetRightAlign(i, false);
                                    }
                                    break;
                            }
                        }
                        if (itemText.Length != 0)
                        {
                            LegendItem item;
                            int y = this.HasCheckBoxes() ? (this.posYLegend + 1) : this.posYLegend;
                            if (this.Items.Custom && (this.Items.Count > ItemIndex))
                            {
                                item = this.Items[ItemIndex];
                                if (i == 0)
                                {
                                    this.posXLegend = item.Left;
                                    y = item.Top;
                                    itemText = item.Text;
                                    graphicsd.TextAlign = item.Align;
                                }
                                else
                                {
                                    itemText = item.Text2;
                                }
                            }
                            else
                            {
                                item = this.Items[ItemIndex];
                                if (i == 0)
                                {
                                    item.Left = this.posXLegend;
                                    item.Top = y;
                                    item.FText = itemText;
                                    item.FText2 = this.GetItemText(ItemOrder, 1);
                                    item.Align = graphicsd.TextAlign;
                                }
                                this.Items[ItemIndex] = item;
                            }
                            graphicsd.TextOut(this.posXLegend, y, itemText);
                        }
                    }
                    if (this.IncPos)
                    {
                        this.posXLegend += this.ColumnWidths[i];
                    }
                    this.posXLegend += this.iSpaceWidth;
                }
                if (this.Items.Custom)
                {
                    r = this.Items[ItemIndex].SymbolRectangle;
                }
                else
                {
                    r.X = xLegendColor;
                    r.Width = this.iColorWidth;
                    r.Y = this.posYLegend;
                    r.Height = this.ItemHeight + 1;
                    if (!this.Symbol.continuous || (ItemOrder == 0))
                    {
                        r.Y += 2;
                        r.Height -= 2;
                    }
                    if (!this.Symbol.continuous || (ItemOrder == (this.iLastValue - this.firstValue)))
                    {
                        r.Height -= 3 + this.vertSpacing;
                    }
                    this.Items[ItemIndex].SymbolRectangle = r;
                }
                if ((this.iLegendStyle == LegendStyles.Series) || (this.iLegendStyle == LegendStyles.LastValues))
                {
                    if (this.CheckBoxes)
                    {
                        int xLegendBox = this.XLegendBox;
                        this.tmpSeries = base.chart.SeriesLegend(ItemIndex, false);
                        if (!this.Vertical)
                        {
                            xLegendBox += num5;
                        }
                        if (graphicsd.CanvasType == CanvasType.GDIplus)
                        {
                            Graphics g = ((Graphics3DGdiPlus) graphicsd).Graphics;
                            Utils.DrawCheckBox(xLegendBox, (this.posYLegend + (((this.ItemHeight - this.vertSpacing) - 11) / 2)) - 1, g, this.tmpSeries.Active, this.Color, true);
                        }
                        this.DrawSymbol(this.tmpSeries, -1, r);
                    }
                    else
                    {
                        this.DrawSymbol(base.chart.ActiveSeriesLegend(ItemIndex), -1, r);
                    }
                }
                else if (this.tmpSeries != null)
                {
                    this.DrawSymbol(this.tmpSeries, this.tmpSeries.LegendToValueIndex(ItemIndex), r);
                }
                else
                {
                    this.DrawSymbol(null, -1, r);
                }
                if (((ItemOrder > 0) && (this.dividingLines != null)) && this.dividingLines.Visible)
                {
                    graphicsd.Pen = this.dividingLines;
                    if (this.Vertical)
                    {
                        graphicsd.HorizontalLine(base.ShapeBounds.Left, base.ShapeBounds.Right, this.posYLegend - (this.vertSpacing / 2));
                    }
                    else
                    {
                        graphicsd.VerticalLine((base.ShapeBounds.Left + num5) + 2, base.ShapeBounds.Top, base.ShapeBounds.Bottom);
                    }
                }
            }
        }

        private void DrawSymbol(Steema.TeeChart.Styles.Series series, int index, Rectangle r)
        {
            if (this.iColorWidth > 0)
            {
                if (series != null)
                {
                    series.DrawLegend(index, r);
                }
                else
                {
                    this.Symbol.Draw(r);
                }
            }
            this.Symbol.DoOnSymbolDraw(this.Symbol, series, index, r);
        }

        private int FirstItemTop()
        {
            int top = base.ShapeBounds.Top;
            if (this.iDrawTitle)
            {
                top += this.Title.Height;
                if (!this.Title.Transparent)
                {
                    top += Math.Abs(this.Title.Shadow.Height);
                }
            }
            return top;
        }

        public string FormattedLegend(int seriesOrValueIndex)
        {
            Chart chart = base.chart;
            switch (this.iLegendStyle)
            {
                case LegendStyles.Series:
                    return chart.SeriesTitleLegend(seriesOrValueIndex, !this.CheckBoxes);

                case LegendStyles.Values:
                    return chart.FormattedValueLegend(this.GetLegendSeries(), seriesOrValueIndex);

                case LegendStyles.LastValues:
                    return chart.FormattedValueLegend(chart.Series[seriesOrValueIndex], chart.Series[seriesOrValueIndex].Count - 1);

                case LegendStyles.Palette:
                    return chart.FormattedValueLegend(this.GetLegendSeries(), seriesOrValueIndex);
            }
            return null;
        }

        public string FormattedValue(Steema.TeeChart.Styles.Series aSeries, int valueIndex)
        {
            if (valueIndex != -1)
            {
                string s = aSeries.LegendString(valueIndex, this.TextStyle);
                this.RemoveChar('\n', ref s);
                return s;
            }
            return "";
        }

        private void GetItems()
        {
            int num;
            this.iItems = new string[(this.iLastValue - this.firstValue) + 1, 10];
            if (this.Inverted)
            {
                for (num = this.iLastValue; num >= this.firstValue; num--)
                {
                    this.SetItem(this.iLastValue - num, num);
                }
            }
            else
            {
                for (num = this.firstValue; num <= this.iLastValue; num++)
                {
                    this.SetItem(num - this.firstValue, num);
                }
            }
        }

        private string GetItemText(int ItemOrder, int Column)
        {
            if (!this.Vertical && this.Inverted)
            {
                return this.iItems[((ItemOrder + this.iTotalItems) - this.iLastValue) - 1, Column];
            }
            return this.iItems[ItemOrder, Column];
        }

        private Rectangle GetLegendBounds()
        {
            int size = 0;
            Rectangle shapeBounds = base.ShapeBounds;
            if (base.Chart.parent != null)
            {
                base.Chart.parent.DoGetLegendSize(this, ref size);
                shapeBounds.Width -= size;
            }
            return shapeBounds;
        }

        private Steema.TeeChart.Styles.Series GetLegendSeries()
        {
            Steema.TeeChart.Styles.Series series = this.series;
            if (series == null)
            {
                return base.chart.GetASeries();
            }
            return series;
        }

        internal bool HasCheckBoxes()
        {
            return (this.CheckBoxes && (this.iLegendStyle != LegendStyles.Values));
        }

        private int MaxLegendValues(int yLegend, int itemHeight)
        {
            if (this.Vertical)
            {
                if (base.AutoSize)
                {
                    return this.CalcMaxLegendValues(yLegend, base.iBottom, base.iBottom - base.iTop, base.iTop, itemHeight);
                }
                return this.CalcMaxLegendValues(yLegend, base.iTop + this.iManualHeight, this.iManualHeight, base.iTop, itemHeight);
            }
            if (base.AutoSize)
            {
                return this.CalcMaxLegendValues(yLegend, base.iRight, base.iRight - base.iLeft, 0, itemHeight);
            }
            return this.CalcMaxLegendValues(yLegend, base.chart.Graphics3D.ChartXCenter - Utils.Round((double) (((double) this.iManualWidth) / 2.0)), this.iManualWidth, 0, itemHeight);
        }

        public override void Paint(Graphics3D g, Rectangle rect)
        {
            if (!base.AutoSize)
            {
                this.iManualWidth = base.iWidth;
                this.iManualHeight = base.iHeight;
            }
            if (!base.bCustomPosition)
            {
                base.ShapeBounds = rect;
            }
            else
            {
                base.ShapeBounds = new Rectangle(base.Left, base.Top, rect.Width, rect.Height);
            }
            this.CalcLegendStyle();
            bool flag = ((this.iLegendStyle == LegendStyles.Values) && this.currentPage) && (base.chart.Page.MaxPointsPerPage > 0);
            if (flag)
            {
                this.firstValue = (base.chart.Page.Current - 1) * base.chart.Page.MaxPointsPerPage;
            }
            this.iTotalItems = this.CalcTotalItems();
            if (flag)
            {
                this.iTotalItems = Math.Min(this.iTotalItems, base.chart.Page.MaxPointsPerPage);
            }
            g.Font = base.Font;
            this.iSpaceWidth = Utils.Round(g.TextWidth(" ")) - 1;
            this.ItemHeight = this.CalcItemHeight();
            this.iDrawTitle = this.Title.Visible && (this.Title.Text != "");
            if (this.iDrawTitle)
            {
                this.Title.CalcHeight();
            }
            if (this.Frame.Visible)
            {
                this.FrameWidth = Utils.Round((float) this.Frame.Width);
            }
            else
            {
                this.FrameWidth = 0;
            }
            if (base.Bevel.Inner != BevelStyles.None)
            {
                this.FrameWidth = base.Bevel.Width;
            }
            if (this.Vertical)
            {
                if (!base.bCustomPosition)
                {
                    base.iTop += Utils.Round((double) (((1.0 * this.topLeftPos) * (base.iBottom - base.iTop)) * 0.01));
                }
                this.NumCols = 1;
                this.NumRows = this.MaxLegendValues(this.FirstItemTop(), this.ItemHeight);
                this.iLastValue = (this.firstValue + Math.Min(this.iTotalItems, this.NumRows)) - 1;
                this.GetItems();
            }
            else
            {
                this.iLastValue = (this.firstValue + this.iTotalItems) - 1;
                this.GetItems();
                this.tmpMaxWidth = this.CalcColumnsWidth(-1);
                this.iColorWidth = this.Symbol.CalcWidth(this.tmpMaxWidth);
                if (!base.bCustomPosition)
                {
                    if (this.Alignment == LegendAlignments.Bottom)
                    {
                        int size = (base.Shadow.Height + this.FrameWidth) - 1;
                        if (base.Chart.parent != null)
                        {
                            base.Chart.parent.DoGetLegendSize(this, ref size);
                        }
                        base.iBottom = rect.Bottom - size;
                        if (base.Shadow.Visible)
                        {
                            base.iBottom -= base.Shadow.Height;
                        }
                    }
                    else
                    {
                        base.iTop = (rect.Y + this.FrameWidth) + 1;
                    }
                }
                int itemHeight = (this.tmpMaxWidth + this.iColorWidth) + 8;
                if (this.HasCheckBoxes())
                {
                    itemHeight += 0x1b;
                }
                this.NumCols = this.MaxLegendValues(8, itemHeight);
                if (this.NumCols > 0)
                {
                    if (base.AutoSize)
                    {
                        this.NumRows = this.iTotalItems / this.NumCols;
                        if ((this.iTotalItems % this.NumCols) > 0)
                        {
                            this.NumRows++;
                        }
                    }
                    else
                    {
                        this.NumRows = Utils.Round(Math.Floor((double) (((double) this.iManualHeight) / ((double) this.ItemHeight))));
                    }
                    this.NumRows = Math.Min(this.NumRows, this.MaxNumRows);
                }
                else
                {
                    this.NumRows = 0;
                }
                this.iLastValue = (this.firstValue + Math.Min(this.iTotalItems, this.NumCols * this.NumRows)) - 1;
            }
            if (this.iLastValue >= this.firstValue)
            {
                this.ResizeVertical();
                if (this.Vertical)
                {
                    this.CalcVerticalPositions();
                }
                else
                {
                    this.CalcHorizontalPositions();
                }
                Rectangle shapeBounds = base.ShapeBounds;
                if (base.chart.parent != null)
                {
                    base.chart.parent.DoGetLegendRectangle(this, ref shapeBounds);
                }
                base.ShapeBounds = shapeBounds;
                base.Paint(g, base.ShapeBounds);
                if (this.iDrawTitle)
                {
                    this.Title.InternalDraw(g, base.ShapeBounds);
                    g.Font = base.Font;
                }
                this.DrawItems();
            }
        }

        private void PrepareSymbolPen()
        {
            base.chart.legendPen = this.Symbol.DefaultPen ? null : this.Symbol.Pen;
        }

        private void RemoveChar(char c, ref string s)
        {
            int num;
            while ((num = s.IndexOf(c)) != -1)
            {
                s = s.Remove(num, 1);
            }
        }

        internal void ResizeChartRect(ref Rectangle rect)
        {
            int num;
            if (this.ResizeChart && !base.CustomPosition)
            {
                switch (this.alignment)
                {
                    case LegendAlignments.Left:
                        rect.X = base.iRight;
                        rect.Width -= rect.X;
                        break;

                    case LegendAlignments.Right:
                        rect.Width = base.iLeft - rect.X;
                        if (base.Shadow.Visible && (base.shadow.Width < 0))
                        {
                            rect.Width += base.shadow.Width;
                        }
                        break;

                    case LegendAlignments.Top:
                    {
                        int y = rect.Y;
                        rect.Y = base.iBottom;
                        if (base.Shadow.Visible)
                        {
                            rect.Y += Math.Max(0, base.Shadow.Height);
                        }
                        if (rect.Y > y)
                        {
                            rect.Height -= rect.Y - y;
                        }
                        break;
                    }
                    case LegendAlignments.Bottom:
                        rect.Height = base.iTop - rect.Y;
                        break;
                }
            }
            if (this.Vertical)
            {
                num = this.CalcMargin(this.HorizMargin, 3, base.chart.Width);
                if (this.Alignment == LegendAlignments.Left)
                {
                    rect.X += num;
                }
                else
                {
                    rect.Width -= num;
                }
            }
            else
            {
                num = this.CalcMargin(this.VertMargin, 4, base.chart.Height);
                if (this.Alignment == LegendAlignments.Top)
                {
                    rect.Y += num;
                    rect.Height -= num;
                }
                else
                {
                    rect.Height -= num;
                }
            }
        }

        private void ResizeVertical()
        {
            int num = 2 + (this.ItemHeight * this.NumRows);
            if (this.iDrawTitle)
            {
                num += this.Title.Height + 2;
                if (!this.Title.Transparent)
                {
                    num += Math.Abs(this.Title.Shadow.Height);
                }
            }
            if ((this.Alignment == LegendAlignments.Bottom) && !base.CustomPosition)
            {
                if (base.AutoSize)
                {
                    base.iTop = base.iBottom - num;
                }
                else
                {
                    base.iTop = base.iBottom - this.iManualHeight;
                }
            }
            else if (base.AutoSize)
            {
                base.iBottom = base.iTop + num;
            }
            else
            {
                base.iBottom = base.iTop + this.iManualHeight;
            }
        }

        private void SetItem(int index, int pos)
        {
            int num;
            string str = base.chart.FormattedLegend(pos);
            int num2 = 0;
            do
            {
                num = str.IndexOf('\x0006');
                if (num != -1)
                {
                    this.iItems[index, num2] = str.Substring(0, num);
                    str = str.Remove(0, num + 1);
                    num2++;
                }
            }
            while (((num != -1) && (str.Length != 0)) && (num2 <= 1));
            if ((str.Length != 0) && (num2 <= 1))
            {
                this.iItems[index, num2] = str;
            }
        }

        private void SetRightAlign(int column, bool isRight)
        {
            if (isRight)
            {
                base.chart.graphics3D.TextAlign = StringAlignment.Far;
                this.posXLegend += this.ColumnWidths[column];
                this.IncPos = false;
            }
            else
            {
                base.chart.graphics3D.TextAlign = StringAlignment.Near;
            }
        }

        private void SetShapeWidthHeight()
        {
            base.iHeight = base.iBottom - base.iTop;
            base.iWidth = base.iRight - base.iLeft;
        }

        [Description("Defines the Legend position."), DefaultValue(1)]
        public LegendAlignments Alignment
        {
            get
            {
                return this.alignment;
            }
            set
            {
                if (this.alignment != value)
                {
                    this.alignment = value;
                    this.Invalidate();
                }
            }
        }

        [Description("Enables/disables the display of Legend check boxes."), DefaultValue(false)]
        public bool CheckBoxes
        {
            get
            {
                return this.checkBoxes;
            }
            set
            {
                base.SetBooleanProperty(ref this.checkBoxes, value);
            }
        }

        [DefaultValue(typeof(System.Drawing.Color), "White"), Description("Changes the background color of the Legend box.")]
        public System.Drawing.Color Color
        {
            get
            {
                return base.Color;
            }
            set
            {
                base.Color = value;
            }
        }

        [Description("Legend shows only the current page items."), DefaultValue(true)]
        public bool CurrentPage
        {
            get
            {
                return this.currentPage;
            }
            set
            {
                base.SetBooleanProperty(ref this.currentPage, value);
            }
        }

        [Description("Pen used to draw lines separating Legend items."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ChartPen DividingLines
        {
            get
            {
                if (this.dividingLines == null)
                {
                    this.dividingLines = new ChartPen(base.chart, System.Drawing.Color.Black, false);
                }
                return this.dividingLines;
            }
            set
            {
                this.dividingLines = value;
            }
        }

        [DefaultValue(0), Description("Sets the first Legend item displayed")]
        public int FirstValue
        {
            get
            {
                return this.firstValue;
            }
            set
            {
                base.SetIntegerProperty(ref this.firstValue, value);
            }
        }

        [Description("Sets the legend text font color to that of the Series color."), DefaultValue(false)]
        public bool FontSeriesColor
        {
            get
            {
                return this.fontSeriesColor;
            }
            set
            {
                base.SetBooleanProperty(ref this.fontSeriesColor, value);
            }
        }

        private ChartPen Frame
        {
            get
            {
                return base.Pen;
            }
        }

        [DefaultValue(0), Description("Sets number of screen pixels between Legend and Chart rectangles.")]
        public int HorizMargin
        {
            get
            {
                return this.horizMargin;
            }
            set
            {
                base.SetIntegerProperty(ref this.horizMargin, value);
            }
        }

        [DefaultValue(false), Description("When True, draws the Legend items in opposite direction.")]
        public bool Inverted
        {
            get
            {
                return this.inverted;
            }
            set
            {
                base.SetBooleanProperty(ref this.inverted, value);
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Description(" List of LegendItem objects.")]
        public LegendItems Items
        {
            get
            {
                if (this.items == null)
                {
                    this.items = new LegendItems(this);
                }
                return this.items;
            }
            set
            {
                this.items = value;
            }
        }

        [DefaultValue(0), Description("Defines which items will be displayed in Chart Legend.")]
        public LegendStyles LegendStyle
        {
            get
            {
                return this.legendStyle;
            }
            set
            {
                if (this.legendStyle != value)
                {
                    this.legendStyle = value;
                    this.Invalidate();
                }
            }
        }

        [EditorBrowsable(EditorBrowsableState.Advanced), Browsable(false)]
        public string[] Lines
        {
            get
            {
                return base.Lines;
            }
        }

        [Description("Max number of Legend Rows displayed in a horizontal Legend."), DefaultValue(10)]
        public int MaxNumRows
        {
            get
            {
                return this.maxNumRows;
            }
            set
            {
                base.SetIntegerProperty(ref this.maxNumRows, value);
            }
        }

        [Description("Resizes Chart rectangle to prevent overlap with Legend."), DefaultValue(true)]
        public bool ResizeChart
        {
            get
            {
                return this.resizeChart;
            }
            set
            {
                base.SetBooleanProperty(ref this.resizeChart, value);
            }
        }

        [Browsable(false), Description("Determine the series used as data for the Legend entries."), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Steema.TeeChart.Styles.Series Series
        {
            get
            {
                return this.series;
            }
            set
            {
                this.series = value;
                this.Invalidate();
            }
        }

        [Description("Sets width and position of color rectangle associated to each Legend item."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public LegendSymbol Symbol
        {
            get
            {
                if (this.symbol == null)
                {
                    this.symbol = new LegendSymbol(this);
                }
                return this.symbol;
            }
            set
            {
                this.symbol = value;
            }
        }

        [Browsable(false), Description("Use the Text property to add text to the Legend."), EditorBrowsable(EditorBrowsableState.Advanced)]
        public string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                base.Text = value;
            }
        }

        [DefaultValue(1), Description("Determines how Legend text items will be formatted.")]
        public LegendTextStyles TextStyle
        {
            get
            {
                return this.textStyle;
            }
            set
            {
                if (this.textStyle != value)
                {
                    this.textStyle = value;
                    this.Invalidate();
                }
            }
        }

        [Localizable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Sets the Title text and its characteristics at the top of the legend.")]
        public LegendTitle Title
        {
            get
            {
                if (this.title == null)
                {
                    this.title = new LegendTitle(base.Chart, false);
                }
                return this.title;
            }
            set
            {
                this.title = value;
            }
        }

        [Description("Specifies the Legend's top position in percent of total chart height."), DefaultValue(10)]
        public int TopLeftPos
        {
            get
            {
                return this.topLeftPos;
            }
            set
            {
                base.SetIntegerProperty(ref this.topLeftPos, value);
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false), Description("Returns True only if the legend is left or right aligned.")]
        public bool Vertical
        {
            get
            {
                if (this.alignment != LegendAlignments.Left)
                {
                    return (this.alignment == LegendAlignments.Right);
                }
                return true;
            }
        }

        [DefaultValue(0), Description("Sets the vertical margin in pixels between Legend and Chart rectangle.")]
        public int VertMargin
        {
            get
            {
                return this.vertMargin;
            }
            set
            {
                base.SetIntegerProperty(ref this.vertMargin, value);
            }
        }

        [Description("Sets vertical spacing between Legend items (pixels)."), DefaultValue(0)]
        public int VertSpacing
        {
            get
            {
                return this.vertSpacing;
            }
            set
            {
                base.SetIntegerProperty(ref this.vertSpacing, value);
            }
        }

        internal class LegendEditor : UITypeEditor
        {
            public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
            {
                Legend legend = (Legend) value;
                bool flag = EditorUtils.ShowFormModal(new Steema.TeeChart.Editors.LegendEditor(legend.Chart, null));
                if ((context != null) && flag)
                {
                    context.OnComponentChanged();
                }
                return value;
            }

            public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
            {
                return UITypeEditorEditStyle.Modal;
            }
        }
    }
}

