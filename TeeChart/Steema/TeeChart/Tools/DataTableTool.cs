namespace Steema.TeeChart.Tools
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using Steema.TeeChart.Styles;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.InteropServices;

    [Description("Series Data Table"), ToolboxBitmap(typeof(DataTableTool), "ToolsIcons.DataTableTool.bmp")]
    public class DataTableTool : Tool
    {
        private bool autoposition;
        internal AxisCalcPosLabelsEventHandler AxisCalcPosLabelsDelegate;
        private bool cliptext;
        private ChartPen columnpen;
        private ChartFont font;
        private bool inverted;
        private int left;
        private Steema.TeeChart.Tools.TableLegend legend;
        private int top;

        public DataTableTool() : this(null)
        {
        }

        public DataTableTool(Chart c) : base(c)
        {
            this.autoposition = true;
            this.cliptext = true;
        }

        protected override void Assign(Tool t)
        {
            base.Assign(t);
            DataTableTool tool = t as DataTableTool;
            tool.RowPen = this.RowPen.Clone() as ChartPen;
            tool.ColumnPen = this.ColumnPen.Clone() as ChartPen;
            tool.Inverted = this.Inverted;
            tool.Left = this.Left;
            tool.Top = this.Top;
            tool.TableLegend = this.TableLegend.Clone() as Steema.TeeChart.Tools.TableLegend;
        }

        private void AssignPen(Graphics3D g, bool tablehorizontal)
        {
            if (tablehorizontal)
            {
                g.Pen = this.ColumnPen;
            }
            else
            {
                g.Pen = this.RowPen;
            }
        }

        private int AxisCalcPosLabels(Axis axis, int value)
        {
            int num = value;
            if ((base.Active && !axis.IsDepthAxis) && this.autoposition)
            {
                int num3;
                Rectangle chartBounds;
                Rectangle chartRect;
                Axis axis2 = this.GuessAxis();
                if (axis2 == null)
                {
                    return num;
                }
                if (axis.Equals(axis2))
                {
                    return (num + this.TableSize());
                }
                if (!this.TableLegend.Visible)
                {
                    return num;
                }
                int num2 = this.LegendWidth(out num3);
                if (axis2.Horizontal)
                {
                    if (!axis.Horizontal)
                    {
                        if (!this.TableLegend.OtherSide && !axis.OtherSide)
                        {
                            return Math.Max(value, num2);
                        }
                        if (this.TableLegend.OtherSide && axis.OtherSide)
                        {
                            chartBounds = base.Chart.ChartBounds;
                            chartRect = base.Chart.ChartRect;
                            num = Math.Max(value, num2 - (chartBounds.Right - chartRect.Right));
                        }
                    }
                    return num;
                }
                if (!axis.Horizontal)
                {
                    return num;
                }
                if (!this.TableLegend.OtherSide && axis.OtherSide)
                {
                    return Math.Max(value, num2);
                }
                if (this.TableLegend.OtherSide && !axis.OtherSide)
                {
                    chartBounds = base.Chart.ChartBounds;
                    chartRect = base.Chart.ChartRect;
                    num = Math.Max(value, num2 - (chartBounds.Bottom - chartRect.Bottom));
                }
            }
            return num;
        }

        private int CalcMaxSeriesCount()
        {
            int num = 0;
            foreach (Series series in base.Chart.Series)
            {
                if (!series.HasZValues && series.Visible)
                {
                    num = Math.Max(num, series.Count);
                }
            }
            return num;
        }

        private void CalcPositions(Axis axis, bool tablehorizontal, out int tmppos, out int tmpoffset)
        {
            if (this.autoposition)
            {
                tmppos = axis.Position;
                tmpoffset = 0;
                if (tablehorizontal)
                {
                    if (axis.OtherSide && base.Chart.Aspect.View3D)
                    {
                        tmppos -= base.Chart.seriesWidth3D;
                        tmpoffset = base.Chart.seriesWidth3D;
                    }
                    this.Top = tmppos;
                    this.Left = 0;
                }
                else
                {
                    if (axis.OtherSide && base.Chart.Aspect.View3D)
                    {
                        tmppos += base.Chart.seriesWidth3D;
                        tmpoffset = -base.Chart.seriesWidth3D;
                    }
                    this.Left = tmppos;
                    this.Top = 0;
                }
            }
            else if (tablehorizontal)
            {
                tmppos = this.Top;
                tmpoffset = this.Left;
            }
            else
            {
                tmppos = this.Left;
                tmpoffset = this.Top;
            }
        }

        protected internal override void ChartEvent(EventArgs e)
        {
            base.ChartEvent(e);
            if (e is AfterDrawEventArgs)
            {
                this.Draw();
            }
        }

        private void ColDivider(Axis axis, int tmppos, bool tablehorizontal, int tmpoffset, int tmpitemsize, int pos)
        {
            int num;
            int right = this.inverted ? this.ColRowPos(axis, tmpitemsize, tablehorizontal, tmppos, -1) : this.ColRowPos(axis, tmpitemsize, tablehorizontal, tmppos, base.Chart.Series.Count);
            if (this.autoposition)
            {
                num = tmppos;
            }
            else
            {
                num = this.inverted ? this.ColRowPos(axis, tmpitemsize, tablehorizontal, tmppos, base.Chart.Series.Count) : this.ColRowPos(axis, tmpitemsize, tablehorizontal, tmppos, -1);
            }
            if (tablehorizontal)
            {
                if (axis.OtherSide)
                {
                    base.Chart.Graphics3D.VerticalLine(tmpoffset + pos, num, right - 1);
                }
                else
                {
                    base.Chart.Graphics3D.VerticalLine(tmpoffset + pos, num, right + 1);
                }
            }
            else
            {
                base.Chart.Graphics3D.HorizontalLine(num, right, tmpoffset + pos);
            }
        }

        private int ColRowPos(Axis axis, int tmpitemsize, bool tablehorizontal, int tmppos, int index)
        {
            int num = 0;
            if (this.inverted)
            {
                for (int i = base.Chart.Series.Count - 1; i > index; i--)
                {
                    if (!base.Chart[i].HasZValues && base.Chart[i].Visible)
                    {
                        num++;
                    }
                }
            }
            else
            {
                for (int j = 0; j < index; j++)
                {
                    if (!base.Chart[j].HasZValues && base.Chart[j].Visible)
                    {
                        num++;
                    }
                }
            }
            num = 10 + (tmpitemsize * (num + 1));
            if (axis.OtherSide)
            {
                num *= -1;
            }
            if (!tablehorizontal)
            {
                return (tmppos - num);
            }
            return (tmppos + num);
        }

        private void DoText(Axis axis, bool tablehorizontal, int tmppos, int tmpitemsize, int tmpoffset, int pos, int textindex, int seriesindex)
        {
            Series series = base.Chart[seriesindex];
            if (series.Visible)
            {
                int index = (base.Chart.Page.MaxPointsPerPage > 0) ? ((series.LastVisibleIndex - textindex) + 1) : (this.CalcMaxSeriesCount() - textindex);
                if (((index >= 0) && (series.Count > index)) && !series.IsNull(index))
                {
                    string str;
                    double markValue = series.GetMarkValue(index);
                    if (!series.mandatory.dateTime)
                    {
                        str = markValue.ToString(series.ValueFormat);
                    }
                    else
                    {
                        str = Utils.DateTime(markValue).ToString();
                    }
                    index = this.ColRowPos(axis, tmpitemsize, tablehorizontal, tmppos, this.SeriesToPos(axis, tablehorizontal, seriesindex));
                    if (tablehorizontal)
                    {
                        base.Chart.Graphics3D.TextOut(tmpoffset + pos, index, str);
                    }
                    else
                    {
                        base.Chart.Graphics3D.TextOut(index + 4, (tmpoffset + pos) - (base.Chart.Graphics3D.FontHeight / 2), str);
                    }
                }
            }
        }

        private void Draw()
        {
            if (this.VisibleSeriesCount() > 0)
            {
                int fontHeight;
                int num4;
                int num5;
                Axis axis = this.GuessAxis();
                bool horizontal = axis.Horizontal;
                Graphics3D g = base.Chart.Graphics3D;
                g.Font = this.Font;
                if (horizontal)
                {
                    fontHeight = g.FontHeight;
                    if (this.TableLegend.Visible)
                    {
                        g.Font = this.TableLegend.Font;
                        fontHeight = Math.Max(fontHeight, g.FontHeight);
                        g.Font = this.Font;
                    }
                }
                else
                {
                    fontHeight = 0;
                    foreach (Series series in base.Chart.Series)
                    {
                        if (series.Visible)
                        {
                            string text = series.mandatory.Maximum.ToString(series.ValueFormat);
                            int num2 = ((int) base.Chart.Graphics3D.TextWidth(text)) + 8;
                            fontHeight = Math.Max(num2, fontHeight);
                        }
                    }
                }
                int num3 = this.CalcMaxSeriesCount();
                this.CalcPositions(axis, horizontal, out num4, out num5);
                if (axis.FAxisDraw.tmpTicks != null)
                {
                    int iStartPos;
                    int iEndPos;
                    bool flag2;
                    int[] tmpTicks = axis.FAxisDraw.tmpTicks;
                    int length = tmpTicks.Length;
                    if (length >= 2)
                    {
                        flag2 = tmpTicks[0] < tmpTicks[1];
                        if (!horizontal)
                        {
                            flag2 = !flag2;
                        }
                        if (axis.Inverted)
                        {
                            flag2 = !flag2;
                        }
                    }
                    else
                    {
                        flag2 = false;
                    }
                    this.AssignPen(g, horizontal);
                    if (horizontal)
                    {
                        g.TextAlign = StringAlignment.Center;
                    }
                    for (int i = 0; i < (length - 1); i++)
                    {
                        this.ColDivider(axis, num4, horizontal, num5, fontHeight, (tmpTicks[i + 1] + tmpTicks[i]) / 2);
                        for (int k = 0; k < base.Chart.Series.Count; k++)
                        {
                            if (flag2)
                            {
                                this.DoText(axis, horizontal, num4, fontHeight, num5, tmpTicks[i], num3 - i, k);
                            }
                            else
                            {
                                this.DoText(axis, horizontal, num4, fontHeight, num5, tmpTicks[i], i + 1, k);
                            }
                        }
                    }
                    if (length > 0)
                    {
                        for (int m = 0; m < base.Chart.Series.Count; m++)
                        {
                            if (flag2)
                            {
                                this.DoText(axis, horizontal, num4, fontHeight, num5, tmpTicks[length - 1], 1, m);
                            }
                            else
                            {
                                this.DoText(axis, horizontal, num4, fontHeight, num5, tmpTicks[length - 1], length, m);
                            }
                        }
                        if (length == 1)
                        {
                            iStartPos = axis.Horizontal ? axis.IStartPos : axis.IEndPos;
                        }
                        else
                        {
                            iStartPos = tmpTicks[length - 1] - ((tmpTicks[length - 2] - tmpTicks[length - 1]) / 2);
                            iStartPos = axis.Horizontal ? Math.Max(iStartPos, axis.IStartPos) : Math.Min(iStartPos, axis.IEndPos);
                        }
                        if (this.Font.Gradient.Visible)
                        {
                            this.AssignPen(g, horizontal);
                        }
                        this.ColDivider(axis, num4, horizontal, num5, fontHeight, iStartPos);
                        if (length == 1)
                        {
                            iEndPos = axis.Horizontal ? axis.IEndPos : axis.IStartPos;
                        }
                        else
                        {
                            iEndPos = tmpTicks[0] + ((tmpTicks[0] - tmpTicks[1]) / 2);
                            iEndPos = axis.Horizontal ? Math.Min(iEndPos, axis.IEndPos) : Math.Max(iEndPos, axis.IStartPos);
                        }
                        this.ColDivider(axis, num4, horizontal, num5, fontHeight, iEndPos);
                    }
                    else
                    {
                        iStartPos = axis.IStartPos;
                        iEndPos = axis.IEndPos;
                    }
                    iStartPos += num5;
                    iEndPos += num5;
                    if (this.legend.Visible)
                    {
                        int num12;
                        int num13;
                        if (flag2 && horizontal)
                        {
                            Utils.SwapInteger(ref iStartPos, ref iEndPos);
                        }
                        if (flag2 && !horizontal)
                        {
                            Utils.SwapInteger(ref iStartPos, ref iEndPos);
                        }
                        if (axis.Inverted)
                        {
                            Utils.SwapInteger(ref iStartPos, ref iEndPos);
                        }
                        num12 = this.LegendWidth(out num12);
                        if (horizontal)
                        {
                            iStartPos -= num12;
                        }
                        else
                        {
                            iEndPos -= num12;
                        }
                        if (this.legend.OtherSide)
                        {
                            if (horizontal)
                            {
                                num13 = iStartPos;
                                iStartPos = iEndPos;
                                iEndPos += num12;
                            }
                            else
                            {
                                num13 = iEndPos;
                                iEndPos = iStartPos;
                                iStartPos -= num12;
                            }
                        }
                        else
                        {
                            num13 = 0;
                        }
                        this.DrawLegend(axis, horizontal, num4, fontHeight, num5, g, iStartPos, iEndPos);
                        if (this.legend.OtherSide)
                        {
                            if (!horizontal)
                            {
                                iStartPos = iEndPos + num12;
                                iEndPos = num13 + num12;
                            }
                            else
                            {
                                iStartPos = num13 + num12;
                            }
                        }
                    }
                    if (horizontal)
                    {
                        g.Pen = this.RowPen;
                    }
                    else
                    {
                        g.Pen = this.ColumnPen;
                    }
                    for (int j = -1; j <= base.Chart.Series.Count; j++)
                    {
                        if (horizontal)
                        {
                            g.HorizontalLine(iStartPos, iEndPos, this.ColRowPos(axis, fontHeight, horizontal, num4, j));
                        }
                        else
                        {
                            g.VerticalLine(this.ColRowPos(axis, fontHeight, horizontal, num4, j), iStartPos, iEndPos);
                        }
                    }
                }
            }
        }

        private void DrawLegend(Axis axis, bool tablehorizontal, int tmppos, int tmpitemsize, int tmpoffset, Graphics3D g, int x0, int x1)
        {
            int num;
            int num2;
            g.Font = this.legend.Font;
            g.TextAlign = StringAlignment.Near;
            int w = this.LegendWidth(out num);
            if (tablehorizontal)
            {
                num2 = this.legend.OtherSide ? x1 : x0;
                g.VerticalLine(num2, this.ColRowPos(axis, tmpitemsize, tablehorizontal, tmppos, -1), this.ColRowPos(axis, tmpitemsize, tablehorizontal, tmppos, base.Chart.Series.Count));
            }
            else
            {
                num2 = x1;
                if (this.legend.OtherSide)
                {
                    num2 += w;
                }
                g.HorizontalLine(this.ColRowPos(axis, tmpitemsize, tablehorizontal, tmppos, -1), this.ColRowPos(axis, tmpitemsize, tablehorizontal, tmppos, base.Chart.Series.Count), num2);
            }
            if (this.inverted)
            {
                for (int i = base.Chart.Series.Count - 1; i >= 0; i--)
                {
                    this.DrawLegendItem(axis, tablehorizontal, tmppos, tmpitemsize, tmpoffset, g, x0, x1, w, num, i);
                }
            }
            else
            {
                for (int j = 0; j < base.Chart.Series.Count; j++)
                {
                    this.DrawLegendItem(axis, tablehorizontal, tmppos, tmpitemsize, tmpoffset, g, x0, x1, w, num, j);
                }
            }
        }

        private void DrawLegendItem(Axis axis, bool tablehorizontal, int tmppos, int tmpitemsize, int tmpoffset, Graphics3D g, int x0, int x1, int w, int tmpsymbolwidth, int index)
        {
            if (!base.Chart[index].HasZValues && base.Chart[index].Visible)
            {
                string text = base.Chart[index].ToString();
                int y = this.ColRowPos(axis, tmpitemsize, tablehorizontal, tmppos, this.SeriesToPos(axis, tablehorizontal, index));
                int x = x0 + 4;
                if (this.legend.Symbol.Visible && (this.legend.Symbol.Position == LegendSymbolPosition.Left))
                {
                    x = (x + tmpsymbolwidth) + 4;
                }
                Color color = g.Font.Color;
                if (this.legend.FontSeriesColor)
                {
                    g.Font.Color = base.Chart[index].Color;
                }
                if (tablehorizontal)
                {
                    g.TextOut(x, y, text);
                }
                else if (this.legend.Symbol.Visible && (this.legend.Symbol.Position == LegendSymbolPosition.Right))
                {
                    g.RotateLabel(y + 4, (x1 + w) - 4, text, 90.0);
                }
                else
                {
                    g.RotateLabel(y + 4, (((x1 + w) - 4) - tmpsymbolwidth) - 4, text, 90.0);
                }
                g.Font.Color = color;
                if (this.legend.Symbol.Visible)
                {
                    Rectangle rectangle;
                    if (tablehorizontal)
                    {
                        if (this.legend.Symbol.Position == LegendSymbolPosition.Left)
                        {
                            rectangle = Utils.FromLTRB(x0 + 4, y + 4, (x0 + 4) + tmpsymbolwidth, y + 12);
                        }
                        else
                        {
                            rectangle = Utils.FromLTRB(((x0 + w) - 4) - tmpsymbolwidth, y + 4, (x0 + w) - 4, y + 12);
                        }
                    }
                    else if (this.legend.Symbol.Position == LegendSymbolPosition.Left)
                    {
                        rectangle = Utils.FromLTRB(y + 6, ((x1 + w) - 4) - tmpsymbolwidth, (y + 6) + 12, (x1 + w) - 4);
                    }
                    else
                    {
                        rectangle = Utils.FromLTRB(y + 6, (x1 + 4) + tmpsymbolwidth, (y + 6) + tmpsymbolwidth, x1 + 4);
                    }
                    base.Chart[index].DrawLegend(-1, rectangle);
                }
            }
        }

        private Axis GuessAxis()
        {
            foreach (Series series in base.Chart.Series)
            {
                if (!series.HasZValues && series.Visible)
                {
                    return (series.yMandatory ? series.GetHorizAxis : series.GetVertAxis);
                }
            }
            return null;
        }

        private int LegendWidth(out int symbolwidth)
        {
            int num = 0;
            base.Chart.Graphics3D.Font = this.legend.Font;
            foreach (Series series in base.Chart.Series)
            {
                if (!series.HasZValues && series.Visible)
                {
                    num = Math.Max(num, (int) base.Chart.Graphics3D.TextWidth(series.ToString()));
                }
            }
            num += base.Chart.Graphics3D.FontHeight;
            symbolwidth = this.legend.Symbol.Visible ? this.legend.Symbol.CalcWidth(num) : 0;
            return (num + symbolwidth);
        }

        protected internal override void OnDisposing()
        {
            if (base.Chart != null)
            {
                Axes axes = base.Chart.Axes;
                axes.AxisCalcPosLabels = (AxisCalcPosLabelsEventHandler) Delegate.Remove(axes.AxisCalcPosLabels, this.AxisCalcPosLabelsDelegate);
            }
            base.OnDisposing();
        }

        private int SeriesToPos(Axis axis, bool tablehorizontal, int index)
        {
            int num = 0;
            if (tablehorizontal)
            {
                if (axis.OtherSide)
                {
                    return (this.inverted ? (index - 1) : (index + 1));
                }
                return index;
            }
            num = this.inverted ? (index - 1) : (index + 1);
            if (axis.OtherSide)
            {
                if (this.inverted)
                {
                    num++;
                    return num;
                }
                num--;
            }
            return num;
        }

        protected override void SetChart(Chart c)
        {
            if ((base.Chart != null) && (this.AxisCalcPosLabelsDelegate != null))
            {
                Axes axes = base.Chart.Axes;
                axes.AxisCalcPosLabels = (AxisCalcPosLabelsEventHandler) Delegate.Remove(axes.AxisCalcPosLabels, this.AxisCalcPosLabelsDelegate);
            }
            base.SetChart(c);
            this.TableLegend.Chart = c;
            if (base.Chart != null)
            {
                Axes axes2 = base.Chart.Axes;
                axes2.AxisCalcPosLabels = (AxisCalcPosLabelsEventHandler) Delegate.Combine(axes2.AxisCalcPosLabels, new AxisCalcPosLabelsEventHandler(this.AxisCalcPosLabels));
                this.AxisCalcPosLabelsDelegate = base.Chart.Axes.AxisCalcPosLabels;
                this.Invalidate();
            }
        }

        private int TableSize()
        {
            int fontHeight;
            int num2;
            base.Chart.Graphics3D.Font = this.Font;
            if (this.GuessAxis().Horizontal)
            {
                fontHeight = base.Chart.Graphics3D.FontHeight;
                if (this.legend.Visible)
                {
                    base.Chart.Graphics3D.Font = this.legend.Font;
                    fontHeight = Math.Max(fontHeight, base.Chart.Graphics3D.FontHeight);
                }
            }
            else
            {
                fontHeight = 0;
                foreach (Series series in base.Chart.Series)
                {
                    if (series.Visible)
                    {
                        string text = series.mandatory.Maximum.ToString(series.ValueFormat);
                        int num3 = ((int) base.Chart.Graphics3D.TextWidth(text)) + 8;
                        fontHeight = Math.Max(num3, fontHeight);
                    }
                }
            }
            if (this.GuessAxis().Horizontal)
            {
                num2 = 10;
            }
            else
            {
                num2 = 0x10;
            }
            return (num2 + (this.VisibleSeriesCount() * fontHeight));
        }

        private int VisibleSeriesCount()
        {
            int num = 0;
            foreach (Series series in base.Chart.Series)
            {
                if (!series.HasZValues && series.Visible)
                {
                    num++;
                }
            }
            return num;
        }

        [Description("Automatically position legend."), DefaultValue(true)]
        public bool AutoPosition
        {
            get
            {
                return this.autoposition;
            }
            set
            {
                base.SetBooleanProperty(ref this.autoposition, value);
            }
        }

        [DefaultValue(true), Description("Clip legend text.")]
        public bool ClipText
        {
            get
            {
                return this.cliptext;
            }
            set
            {
                base.SetBooleanProperty(ref this.cliptext, value);
            }
        }

        [Description("Column Pen characteristics."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("Appearance")]
        public ChartPen ColumnPen
        {
            get
            {
                if (this.columnpen == null)
                {
                    this.columnpen = new ChartPen(base.Chart);
                }
                return this.columnpen;
            }
            set
            {
                this.columnpen = value;
            }
        }

        [Description("Gets descriptive text.")]
        public override string Description
        {
            get
            {
                return Texts.DataTableTool;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("Appearance")]
        public ChartFont Font
        {
            get
            {
                if (this.font == null)
                {
                    this.font = new ChartFont(base.Chart);
                }
                return this.font;
            }
        }

        [DefaultValue(false), Description("Invert legend items.")]
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

        [Description("Legend left coordinate."), DefaultValue(0)]
        public int Left
        {
            get
            {
                return this.left;
            }
            set
            {
                base.SetIntegerProperty(ref this.left, value);
            }
        }

        [Category("Appearance"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Row Pen characteristics.")]
        public ChartPen RowPen
        {
            get
            {
                if (base.pPen == null)
                {
                    base.pPen = new ChartPen(base.Chart);
                }
                return base.pPen;
            }
            set
            {
                base.pPen = value;
            }
        }

        [Description("Gets detailed descriptive text.")]
        public override string Summary
        {
            get
            {
                return Texts.DataTableToolSummary;
            }
        }

        [Description("Table Legend characteristics.")]
        public Steema.TeeChart.Tools.TableLegend TableLegend
        {
            get
            {
                if (this.legend == null)
                {
                    this.legend = new Steema.TeeChart.Tools.TableLegend(base.Chart);
                }
                return this.legend;
            }
            set
            {
                this.legend = value;
            }
        }

        [Description("Legend top coordinate."), DefaultValue(0)]
        public int Top
        {
            get
            {
                return this.top;
            }
            set
            {
                base.SetIntegerProperty(ref this.top, value);
            }
        }
    }
}

