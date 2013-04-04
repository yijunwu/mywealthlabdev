namespace Steema.TeeChart.Tools
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using Steema.TeeChart.Export;
    using Steema.TeeChart.Styles;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;

    [ToolboxBitmap(typeof(LegendPalette), "ToolsIcons.LegendPalette.bmp"), Description("Displays legend made with 3D series palette colors.")]
    public class LegendPalette : ToolSeries
    {
        internal PaletteChart fChart;
        private int height;
        private int left;
        private Steema.TeeChart.PositionUnits positionunits;
        private bool smooth;
        private int top;
        private bool vertical;
        private int width;

        public LegendPalette() : this(null)
        {
        }

        public LegendPalette(Chart c) : base(c)
        {
            this.vertical = true;
            this.smooth = true;
            this.top = 10;
            this.left = 10;
            this.width = 100;
            this.height = 200;
            this.positionunits = Steema.TeeChart.PositionUnits.Pixels;
            this.fChart = new PaletteChart();
            this.fChart.Axes.Bottom.Visible = false;
            this.fChart.Axes.Top.Visible = false;
            PaletteSeries s = new PaletteSeries {
                iTool = this,
                VertAxis = VerticalAxis.Both,
                HorizAxis = HorizontalAxis.Both
            };
            this.fChart.Series.Add(s);
        }

        protected override void Assign(Steema.TeeChart.Tools.Tool t)
        {
            base.Assign(t);
            LegendPalette palette = t as LegendPalette;
            palette.Width = this.Width;
            palette.Height = this.Height;
            palette.Smooth = this.Smooth;
            palette.Inverted = this.Inverted;
            palette.Left = this.Left;
            palette.Top = this.Top;
            palette.Pen = this.Pen.Clone() as ChartPen;
            palette.Gradient = this.Gradient.Clone() as Steema.TeeChart.Drawing.Gradient;
        }

        protected internal override void ChartEvent(EventArgs e)
        {
            base.ChartEvent(e);
            if (base.Active && (e is AfterDrawEventArgs))
            {
                this.fChart[0].BeginUpdate();
                this.fChart[0].Clear();
                if ((base.Series != null) && (base.Series is Custom3DPalette))
                {
                    double upToValue;
                    Color valueColorValue;
                    Custom3DPalette series = base.Series as Custom3DPalette;
                    if (base.Series is Contour)
                    {
                        for (int i = 0; i < (series as Contour).Levels.Count; i++)
                        {
                            upToValue = (series as Contour).Levels[i].UpToValue;
                            valueColorValue = (series as Contour).Levels[i].Color;
                            if (this.vertical)
                            {
                                this.fChart[0].Add((double) i, upToValue, valueColorValue);
                            }
                            else
                            {
                                this.fChart[0].Add(upToValue, (double) i, valueColorValue);
                            }
                        }
                    }
                    else
                    {
                        for (int j = 0; j < series.Palette.Count; j++)
                        {
                            upToValue = series.Palette[j].UpToValue;
                            valueColorValue = series.GetValueColorValue(upToValue);
                            if (this.vertical)
                            {
                                this.fChart[0].Add((double) j, upToValue, valueColorValue);
                            }
                            else
                            {
                                this.fChart[0].Add(upToValue, (double) j, valueColorValue);
                            }
                        }
                    }
                }
                base.Chart.Graphics3D.UnClip();
                (this.fChart[0] as PaletteSeries).Pen = this.Pen;
                int left = (this.positionunits == Steema.TeeChart.PositionUnits.Percent) ? Utils.Round((double) ((this.left * base.Chart.Width) * 0.01)) : this.left;
                int top = (this.positionunits == Steema.TeeChart.PositionUnits.Percent) ? Utils.Round((double) ((this.top * base.Chart.Height) * 0.01)) : this.top;
                Rectangle rectangle = Rectangle.FromLTRB(left, top, left + this.width, top + this.height);
                this.fChart[0].EndUpdate();
                MemoryStream stream = null;
                Bitmap image = null;
                try
                {
                    stream = new MemoryStream();
                    PNGFormat pNG = this.fChart.Export.Image.PNG;
                    pNG.Height = rectangle.Height;
                    pNG.Width = rectangle.Width;
                    pNG.Save(stream);
                    image = new Bitmap(stream);
                    base.Chart.Graphics3D.Draw(rectangle.X, rectangle.Y, image);
                }
                finally
                {
                    stream.Close();
                    image.Dispose();
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.fChart = null;
            }
            base.Dispose(disposing);
        }

        protected internal override void MouseEvent(MouseEventKinds kind, MouseEventArgs e, ref Cursor c)
        {
            base.MouseEvent(kind, e, ref c);
            Point point = new Point(e.X, e.Y);
            switch (kind)
            {
                case MouseEventKinds.Down:
                    this.fChart.Chart.DoMouseDown(e.Clicks > 1, e, Control.ModifierKeys);
                    break;

                case MouseEventKinds.Move:
                    this.fChart.Chart.DoMouseMove(point.X, point.Y, ref c);
                    break;

                case MouseEventKinds.Up:
                    this.fChart.Chart.DoMouseUp(e, Control.ModifierKeys);
                    break;
            }
            if (Rectangle.FromLTRB(this.left, this.top, this.left + this.width, this.top + this.height).Contains(point.X, point.Y))
            {
                base.Chart.CancelMouse = true;
            }
        }

        protected override void SetChart(Chart value)
        {
            base.SetChart(value);
            if ((this.fChart != null) && (value != null))
            {
                this.fChart.Chart.Parent = value.Parent;
            }
        }

        protected override void SetSeries(Series value)
        {
            base.SetSeries(value);
            this.Invalidate();
        }

        public LegendPaletteAxis Axis
        {
            get
            {
                switch (this.fChart[0].VertAxis)
                {
                    case VerticalAxis.Left:
                        return LegendPaletteAxis.laDefault;

                    case VerticalAxis.Right:
                        return LegendPaletteAxis.laOther;
                }
                return LegendPaletteAxis.laBoth;
            }
            set
            {
                switch (value)
                {
                    case LegendPaletteAxis.laDefault:
                        this.fChart[0].VertAxis = VerticalAxis.Left;
                        this.fChart[0].HorizAxis = HorizontalAxis.Bottom;
                        break;

                    case LegendPaletteAxis.laOther:
                        this.fChart[0].VertAxis = VerticalAxis.Right;
                        this.fChart[0].HorizAxis = HorizontalAxis.Top;
                        break;

                    default:
                        this.fChart[0].VertAxis = VerticalAxis.Both;
                        this.fChart[0].HorizAxis = HorizontalAxis.Both;
                        break;
                }
                this.Invalidate();
            }
        }

        [Description("Element Border Pen characteristics."), Category("Appearance")]
        public ChartPen Border
        {
            get
            {
                return this.fChart.Panel.Pen;
            }
            set
            {
                this.fChart.Panel.Pen = value;
            }
        }

        [Description("Gets descriptive text.")]
        public override string Description
        {
            get
            {
                return Texts.LegendPaletteTool;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("Appearance"), Description("Element Gradient characteristics.")]
        public Steema.TeeChart.Drawing.Gradient Gradient
        {
            get
            {
                return this.fChart.Panel.Gradient;
            }
            set
            {
                this.fChart.Panel.Gradient = value;
                this.Invalidate();
            }
        }

        [Description("Legend height."), DefaultValue(200)]
        public int Height
        {
            get
            {
                return this.height;
            }
            set
            {
                base.SetIntegerProperty(ref this.height, value);
            }
        }

        [DefaultValue(false), Description("Inverted legend.")]
        public bool Inverted
        {
            get
            {
                return this.fChart.Axes.Left.Inverted;
            }
            set
            {
                this.fChart.Axes.Left.Inverted = value;
                this.fChart.Axes.Right.Inverted = value;
                this.fChart.Axes.Top.Inverted = value;
                this.fChart.Axes.Bottom.Inverted = value;
            }
        }

        [DefaultValue(10), Description("Legend left coordinate.")]
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

        [Description("Element Pen characteristics."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("Appearance")]
        public ChartPen Pen
        {
            get
            {
                if (base.pPen == null)
                {
                    base.pPen = new ChartPen(this.fChart.Chart, Color.Black);
                }
                return base.pPen;
            }
            set
            {
                base.pPen = value;
            }
        }

        [Description("Legend position compared to chart."), DefaultValue(1)]
        public Steema.TeeChart.PositionUnits PositionUnits
        {
            get
            {
                return this.positionunits;
            }
            set
            {
                if (value != this.positionunits)
                {
                    this.positionunits = value;
                    this.Invalidate();
                }
            }
        }

        [Description("Smooth transition between levels."), DefaultValue(true)]
        public bool Smooth
        {
            get
            {
                return this.smooth;
            }
            set
            {
                base.SetBooleanProperty(ref this.smooth, value);
            }
        }

        [Description("Gets detailed descriptive text.")]
        public override string Summary
        {
            get
            {
                return Texts.LegendPaletteToolSummary;
            }
        }

        [Description("Legend top coordinate."), DefaultValue(10)]
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

        [DefaultValue(false), Description("Transparent Chart Panel.")]
        public bool Transparent
        {
            get
            {
                return this.fChart.Panel.Transparent;
            }
            set
            {
                this.fChart.Panel.Transparent = value;
            }
        }

        [DefaultValue(true), Description("Legend orientation")]
        public bool Vertical
        {
            get
            {
                return this.vertical;
            }
            set
            {
                if (this.vertical != value)
                {
                    this.vertical = value;
                    Utils.SwapInteger(ref this.width, ref this.height);
                    this.fChart.Axes.Bottom.Visible = !this.vertical;
                    this.fChart.Axes.Top.Visible = !this.vertical;
                    this.fChart.Axes.Left.Visible = this.vertical;
                    this.fChart.Axes.Right.Visible = this.vertical;
                }
            }
        }

        [DefaultValue(100), Description("Legend width.")]
        public int Width
        {
            get
            {
                return this.width;
            }
            set
            {
                base.SetIntegerProperty(ref this.width, value);
            }
        }
    }
}

