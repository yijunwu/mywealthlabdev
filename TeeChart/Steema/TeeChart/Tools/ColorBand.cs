namespace Steema.TeeChart.Tools
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    [Description("Displays a coloured rectangle (band) at the specified axis and position."), ToolboxBitmap(typeof(ColorBand), "ToolsIcons.ColorBand.bmp")]
    public class ColorBand : ToolAxis
    {
        private Rectangle BoundsRect;
        private bool drawBehind;
        private double end;
        private ColorLine FLineEnd;
        private ColorLine FLineStart;
        private double start;

        public event MouseEventHandler Click;

        public ColorBand() : this(null)
        {
        }

        public ColorBand(Chart c) : base(c)
        {
            this.drawBehind = true;
        }

        protected override void Assign(Steema.TeeChart.Tools.Tool t)
        {
            base.Assign(t);
            ColorBand band = t as ColorBand;
            band.Brush = this.Brush.Clone() as ChartBrush;
            band.Color = this.Color;
            band.DrawBehind = this.DrawBehind;
            band.End = this.End;
            band.EndLine = this.EndLine.Clone() as ColorLine;
            band.ResizeEnd = this.ResizeEnd;
            band.Start = this.Start;
            band.StartLine = this.StartLine.Clone() as ColorLine;
        }

        protected internal override void ChartEvent(EventArgs e)
        {
            base.ChartEvent(e);
            if (((e is BeforeDrawSeriesEventArgs) && this.drawBehind) || ((e is AfterDrawEventArgs) && !this.drawBehind))
            {
                this.PaintBand();
            }
        }

        public bool Clicked(int X, int Y)
        {
            return this.BoundsRect.Contains(X, Y);
        }

        private void DragLine(object sender, EventArgs e)
        {
            if ((sender as ColorLine).DragRepaint)
            {
                if (sender == this.FLineStart)
                {
                    this.Start = (sender as ColorLine).Value;
                }
                else
                {
                    this.End = (sender as ColorLine).Value;
                }
            }
        }

        private void EndDragLine(object sender)
        {
            if (!(sender as ColorLine).DragRepaint)
            {
                if (sender == this.FLineStart)
                {
                    this.Start = (sender as ColorLine).Value;
                }
                else
                {
                    this.End = (sender as ColorLine).Value;
                }
            }
        }

        protected internal override void MouseEvent(MouseEventKinds kind, MouseEventArgs e, ref Cursor c)
        {
            if (this.FLineStart.Active)
            {
                this.FLineStart.MouseEvent(kind, e, ref c);
            }
            if (this.FLineEnd.Active)
            {
                this.FLineEnd.MouseEvent(kind, e, ref c);
            }
            if (kind == MouseEventKinds.Down)
            {
                this.OnClick(e);
            }
        }

        private ColorLine NewColorLine(Chart c)
        {
            ColorLine line = new ColorLine {
                InternalUse = true,
                Chart = c,
                Active = false,
                DragRepaint = true,
                Draw3D = false
            };
            line.DragLine += new EventHandler(this.DragLine);
            line.EndDragLine += new ColorLineToolOnDragEventHandler(this.EndDragLine);
            line.DrawBehind = true;
            return line;
        }

        protected internal void OnClick(MouseEventArgs e)
        {
            if ((this.Click != null) && this.Clicked(e.X, e.Y))
            {
                this.Click(this, e);
            }
        }

        private void PaintBand()
        {
            if (base.iAxis != null)
            {
                bool flag;
                double num3;
                this.BoundsRect = base.chart.ChartRect;
                double start = this.start;
                double end = this.end;
                if (base.iAxis.Inverted)
                {
                    if (start < end)
                    {
                        num3 = start;
                        start = end;
                        end = num3;
                    }
                    flag = (end <= base.iAxis.Maximum) && (start >= base.iAxis.Minimum);
                }
                else
                {
                    if (start > end)
                    {
                        num3 = start;
                        start = end;
                        end = num3;
                    }
                    flag = (start <= base.iAxis.Maximum) && (end >= base.iAxis.Minimum);
                }
                if (flag)
                {
                    if (base.iAxis.Horizontal)
                    {
                        this.BoundsRect.X = Math.Max(base.iAxis.IStartPos, base.iAxis.CalcPosValue(start));
                        this.BoundsRect.Width = Math.Min(base.iAxis.IEndPos, base.iAxis.CalcPosValue(end)) - this.BoundsRect.X;
                        if (!base.Pen.Visible)
                        {
                            this.BoundsRect.Width++;
                        }
                    }
                    else
                    {
                        this.BoundsRect.Y = Math.Max(base.iAxis.IStartPos, base.iAxis.CalcPosValue(end));
                        this.BoundsRect.Height = Math.Min(base.iAxis.IEndPos, base.iAxis.CalcPosValue(start)) - this.BoundsRect.Y;
                        this.BoundsRect.X++;
                        if (!base.Pen.Visible)
                        {
                            this.BoundsRect.Height++;
                            this.BoundsRect.Width++;
                        }
                    }
                    Graphics3D graphicsd = base.chart.graphics3D;
                    graphicsd.Brush = this.Brush;
                    graphicsd.Pen = base.Pen;
                    if (base.chart.aspect.view3D && this.drawBehind)
                    {
                        graphicsd.Rectangle(this.BoundsRect, base.chart.aspect.Width3D);
                    }
                    else
                    {
                        graphicsd.Rectangle(this.BoundsRect);
                    }
                }
            }
        }

        protected internal override void SetAxis(Axis value)
        {
            base.SetAxis(value);
            this.FLineEnd.Axis = value;
            this.FLineStart.Axis = value;
        }

        protected override void SetChart(Chart value)
        {
            base.SetChart(value);
            if (base.chart != null)
            {
                if (this.FLineEnd == null)
                {
                    this.FLineEnd = this.NewColorLine(base.chart);
                }
                if (this.FLineStart == null)
                {
                    this.FLineStart = this.NewColorLine(base.chart);
                }
                this.SetLines();
            }
        }

        private void SetLines()
        {
            this.FLineEnd.Value = this.End;
            this.FLineStart.Value = this.Start;
            this.Invalidate();
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("Appearance"), Description("Element Brush characteristics.")]
        public ChartBrush Brush
        {
            get
            {
                if (base.bBrush == null)
                {
                    base.bBrush = new ChartBrush(base.chart);
                }
                return base.bBrush;
            }
            set
            {
                base.bBrush = value;
            }
        }

        [Category("Appearance"), Description("Sets Band Color."), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public System.Drawing.Color Color
        {
            get
            {
                return this.Brush.Color;
            }
            set
            {
                this.Brush.Color = value;
            }
        }

        [Description("Gets descriptive text.")]
        public override string Description
        {
            get
            {
                return Texts.ColorBandTool;
            }
        }

        [DefaultValue(true), Description("Draws the Colorband behind the series values.")]
        public bool DrawBehind
        {
            get
            {
                return this.drawBehind;
            }
            set
            {
                base.SetBooleanProperty(ref this.drawBehind, value);
                this.FLineEnd.DrawBehind = value;
                this.FLineStart.DrawBehind = value;
            }
        }

        [Description("Sets End Axis value of colorband.")]
        public double End
        {
            get
            {
                return this.end;
            }
            set
            {
                base.SetDoubleProperty(ref this.end, value);
                this.SetLines();
            }
        }

        [Description("Contains formatting properties for the automatic line used to drag the end value of the ColorBand tool at runtime."), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Category("Appearance")]
        public ColorLine EndLine
        {
            get
            {
                return this.FLineEnd;
            }
            set
            {
                this.FLineEnd = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Pen used to draw the ending line of the color band tool.")]
        public ChartPen EndLinePen
        {
            get
            {
                return this.EndLine.Pen;
            }
        }

        [Category("Appearance"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Description("Sets Band colour gradient.")]
        public Steema.TeeChart.Drawing.Gradient Gradient
        {
            get
            {
                return this.Brush.Gradient;
            }
        }

        [Description("Gets or sets if the ColorBand tool allows mouse dragging of the edge corresponding to the end value.")]
        public bool ResizeEnd
        {
            get
            {
                return this.EndLine.Active;
            }
            set
            {
                this.EndLine.Chart = base.Chart;
                this.EndLine.Active = value;
                this.Invalidate();
            }
        }

        [Description("Gets or sets if the ColorBand tool allows mouse dragging of the edge corresponding to the start value.")]
        public bool ResizeStart
        {
            get
            {
                return this.StartLine.Active;
            }
            set
            {
                this.StartLine.Chart = base.Chart;
                this.StartLine.Active = value;
                this.Invalidate();
            }
        }

        [Description("Sets Start Axis value of colorband.")]
        public double Start
        {
            get
            {
                return this.start;
            }
            set
            {
                base.SetDoubleProperty(ref this.start, value);
                this.SetLines();
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Description("Contains formatting properties for the automatic line used to drag the start value of the ColorBand tool at runtime."), Category("Appearance")]
        public ColorLine StartLine
        {
            get
            {
                return this.FLineStart;
            }
            set
            {
                this.FLineStart = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Pen used to draw the starting line of the color band tool.")]
        public ChartPen StartLinePen
        {
            get
            {
                return this.StartLine.Pen;
            }
        }

        [Description("Gets detailed descriptive text.")]
        public override string Summary
        {
            get
            {
                return Texts.ColorBandSummary;
            }
        }

        [Description("Sets the Transparency of ColorBand as percentage."), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(0), Category("Appearance")]
        public int Transparency
        {
            get
            {
                return this.Brush.Transparency;
            }
            set
            {
                this.Brush.Transparency = value;
            }
        }
    }
}

