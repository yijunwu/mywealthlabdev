namespace Steema.TeeChart
{
    using Steema.TeeChart.Drawing;
    using Steema.TeeChart.Editors;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Design;
    using System.Drawing.Imaging;
    using System.Drawing.Printing;

    [Description("Properties to configure Chart printing."), Editor(typeof(Printer.ComponentEditor), typeof(UITypeEditor))]
    public sealed class Printer : TeeBase
    {
        private int chartHeight;
        private ArrayList chartList;
        private int chartWidth;
        private double chartWidthHeightRatio;
        private System.Drawing.Printing.PrintDocument document;
        private bool grayscale;
        internal bool isPartial;
        private bool landscape;
        private Steema.TeeChart.Margins margins;
        private PrintMarginUnits marginUnits;
        private bool printPanelBackground;
        private bool proportional;
        private int resolution;
        private bool useAntiAlias;

        public Printer(Chart c) : base(c)
        {
            this.proportional = true;
            this.marginUnits = PrintMarginUnits.HundredthsInch;
            this.margins = new Steema.TeeChart.Margins(100, 100, 100, 100);
            this.resolution = 1;
        }

        public void BeginPrint()
        {
            this.isPartial = true;
            this.chartList = new ArrayList();
        }

        private void DrawGrayedImage(Graphics g, Image image, Rectangle destRect, Rectangle srcRect)
        {
            ImageAttributes imageAttr = new ImageAttributes();
            ColorMatrix newColorMatrix = new ColorMatrix {
                Matrix00 = 0.3333333f,
                Matrix01 = 0.3333333f,
                Matrix02 = 0.3333333f,
                Matrix10 = 0.3333333f,
                Matrix11 = 0.3333333f,
                Matrix12 = 0.3333333f,
                Matrix20 = 0.3333333f,
                Matrix21 = 0.3333333f,
                Matrix22 = 0.3333333f
            };
            imageAttr.SetColorMatrix(newColorMatrix);
            g.DrawImage(image, destRect, srcRect.X, srcRect.Y, srcRect.Width, srcRect.Height, GraphicsUnit.Pixel, imageAttr);
        }

        public void EndPrint()
        {
            this.Document.Print();
            this.isPartial = false;
        }

        internal Rectangle FitToBounds(PrintPageEventArgs e)
        {
            int num3;
            int num4;
            int num = e.MarginBounds.Right - e.MarginBounds.Left;
            int num2 = e.MarginBounds.Bottom - e.MarginBounds.Top;
            if (!this.proportional)
            {
                return new Rectangle(e.MarginBounds.Left, e.MarginBounds.Top, e.MarginBounds.Right - e.MarginBounds.Left, e.MarginBounds.Bottom - e.MarginBounds.Top);
            }
            if (this.chartHeight > this.chartWidth)
            {
                num3 = num2;
                if ((num3 * this.chartWidthHeightRatio) < num)
                {
                    num4 = Utils.Round((double) (num3 * this.chartWidthHeightRatio));
                }
                else
                {
                    num4 = num;
                    num3 = Utils.Round((double) (((double) num) / this.chartWidthHeightRatio));
                }
            }
            else
            {
                num4 = num;
                if ((((double) num) / this.chartWidthHeightRatio) < num2)
                {
                    num3 = Utils.Round((double) (((double) num) / this.chartWidthHeightRatio));
                }
                else
                {
                    num3 = num2;
                    num4 = Utils.Round((double) (num3 * this.chartWidthHeightRatio));
                }
            }
            int x = e.MarginBounds.Left + (Utils.Round((float) (((float) (e.MarginBounds.Right - e.MarginBounds.Left)) / 2f)) - Utils.Round((float) (((float) num4) / 2f)));
            return new Rectangle(x, e.MarginBounds.Top + (Utils.Round((float) (((float) (e.MarginBounds.Bottom - e.MarginBounds.Top)) / 2f)) - Utils.Round((float) (((float) num3) / 2f))), num4, num3);
        }

        internal void GetOptions()
        {
            System.Drawing.Printing.Margins margins;
            this.Document.DefaultPageSettings.Landscape = this.landscape;
            if (this.marginUnits == PrintMarginUnits.Percent)
            {
                int height = this.Document.DefaultPageSettings.PaperSize.Height;
                int width = this.Document.DefaultPageSettings.PaperSize.Width;
                margins = new System.Drawing.Printing.Margins((this.margins.Left * width) / 100, (this.margins.Right * width) / 100, (this.margins.Top * height) / 100, (this.margins.Bottom * height) / 100);
            }
            else
            {
                margins = new System.Drawing.Printing.Margins(this.margins.Left, this.margins.Right, this.margins.Top, this.margins.Bottom);
            }
            this.Document.DefaultPageSettings.Margins = margins;
            Rectangle chartBounds = base.chart.chartBounds;
            this.chartWidth = chartBounds.Width;
            this.chartHeight = chartBounds.Height;
            this.chartWidthHeightRatio = ((double) this.chartWidth) / ((double) this.chartHeight);
            this.Document.DefaultPageSettings.Color = !this.grayscale;
        }

        private void GetPartialOptions()
        {
            this.Document.DefaultPageSettings.Color = !this.grayscale;
            this.Document.DefaultPageSettings.Landscape = this.landscape;
        }

        private void pd_PrintPage(object sender, PrintPageEventArgs e)
        {
            try
            {
                base.chart.printing = true;
                if (!this.isPartial)
                {
                    this.PrintToFullPage(sender, e);
                }
                else
                {
                    this.PrintToPartialPage(sender, e);
                }
            }
            finally
            {
                base.chart.printing = false;
            }
        }

        public void Preview()
        {
            this.GetOptions();
            PrintPreview.ShowModal(base.chart, this);
        }

        public void Print()
        {
            this.isPartial = false;
            this.GetOptions();
            this.Document.Print();
        }

        public void Print(bool landscape)
        {
            this.isPartial = false;
            this.GetOptions();
            this.Document.DefaultPageSettings.Landscape = landscape;
            this.Document.Print();
        }

        public void Print(Rectangle r)
        {
            this.GetPartialOptions();
            this.chartList.Add(new ChartPrintJob(base.Chart, r));
        }

        public void Print(Chart c, Rectangle r)
        {
            this.GetPartialOptions();
            this.chartList.Add(new ChartPrintJob(c, r));
        }

        private void PrintToFullPage(object sender, PrintPageEventArgs e)
        {
            Metafile metafile;
            Rectangle r = this.FitToBounds(e);
            if (this.resolution > 1)
            {
                int width = Convert.ToInt32((double) (r.Width * (1.0 + (((double) this.resolution) / 100.0))));
                metafile = base.chart.Metafile(base.chart, width, Convert.ToInt32((double) (r.Height * (1.0 + (((double) this.resolution) / 100.0)))));
                if (base.Chart.parent != null)
                {
                    base.Chart.parent.DoChartPrint(new ChartPrintJob(base.Chart, r), e);
                }
                if (this.Grayscale)
                {
                    this.DrawGrayedImage(e.Graphics, metafile, r, base.chart.chartBounds);
                }
                else
                {
                    e.Graphics.DrawImage(metafile, r, base.chart.chartBounds, GraphicsUnit.Pixel);
                }
            }
            else
            {
                metafile = base.chart.Metafile(base.chart, this.chartWidth, this.chartHeight);
                if (base.Chart.parent != null)
                {
                    base.Chart.parent.DoChartPrint(new ChartPrintJob(base.Chart, r), e);
                }
                if (this.Grayscale)
                {
                    this.DrawGrayedImage(e.Graphics, metafile, r, base.chart.chartBounds);
                }
                else
                {
                    e.Graphics.DrawImage(metafile, r, base.chart.chartBounds, GraphicsUnit.Pixel);
                }
            }
        }

        private void PrintToPartialPage(object sender, PrintPageEventArgs e)
        {
            foreach (ChartPrintJob job in this.chartList)
            {
                Metafile metafile;
                if (this.resolution > 1)
                {
                    int width = job.Chart.Width;
                    int height = job.Chart.Height;
                    job.Chart.Width = Convert.ToInt32((double) (job.ChartRect.Width * (1.0 + (((double) this.resolution) / 100.0))));
                    job.Chart.Height = Convert.ToInt32((double) (job.ChartRect.Height * (1.0 + (((double) this.resolution) / 100.0))));
                    metafile = base.chart.Metafile(job.Chart, Convert.ToInt32((double) (job.ChartRect.Width * (1.0 + (((double) this.resolution) / 100.0)))), Convert.ToInt32((double) (job.ChartRect.Height * (1.0 + (((double) this.resolution) / 100.0)))));
                    if (base.Chart.parent != null)
                    {
                        base.Chart.parent.DoChartPrint(job, e);
                    }
                    if (this.Grayscale)
                    {
                        this.DrawGrayedImage(e.Graphics, metafile, job.ChartRect, job.Chart.chartBounds);
                    }
                    else
                    {
                        e.Graphics.DrawImage(metafile, job.ChartRect, job.Chart.chartBounds, GraphicsUnit.Pixel);
                    }
                    job.Chart.Width = width;
                    job.Chart.Height = height;
                }
                else
                {
                    metafile = base.chart.Metafile(job.Chart, job.ChartRect.Width, job.ChartRect.Height);
                    if (base.Chart.parent != null)
                    {
                        base.Chart.parent.DoChartPrint(job, e);
                    }
                    if (this.Grayscale)
                    {
                        this.DrawGrayedImage(e.Graphics, metafile, job.ChartRect, base.chart.chartBounds);
                    }
                    else
                    {
                        e.Graphics.DrawImage(metafile, job.ChartRect, base.chart.chartBounds, GraphicsUnit.Pixel);
                    }
                }
            }
        }

        private bool ShouldSerializeMargins()
        {
            if (((this.margins.Left == 100) && (this.margins.Top == 100)) && (this.margins.Right == 100))
            {
                return (this.margins.Bottom != 100);
            }
            return true;
        }

        private System.Drawing.Printing.PrintDocument Document
        {
            get
            {
                if (this.document == null)
                {
                    this.document = new System.Drawing.Printing.PrintDocument();
                    this.document.PrintController = new StandardPrintController();
                    this.document.PrintPage += new PrintPageEventHandler(this.pd_PrintPage);
                }
                return this.document;
            }
        }

        [DefaultValue(false), Description("")]
        public bool Grayscale
        {
            get
            {
                return this.grayscale;
            }
            set
            {
                base.SetBooleanProperty(ref this.grayscale, value);
            }
        }

        [DefaultValue(false), Description("Sets the current Printer orientation to Landscape.")]
        public bool Landscape
        {
            get
            {
                return this.landscape;
            }
            set
            {
                base.SetBooleanProperty(ref this.landscape, value);
            }
        }

        [Description("Sets the four margins as a percentage of paper dimensions. ")]
        public Steema.TeeChart.Margins Margins
        {
            get
            {
                return this.margins;
            }
            set
            {
                this.margins = value;
            }
        }

        [DefaultValue(1), Description("Sets Printer Margin Units.")]
        public PrintMarginUnits MarginUnits
        {
            get
            {
                return this.marginUnits;
            }
            set
            {
                this.marginUnits = value;
            }
        }

        [Browsable(false), Description("Selects the Chart print job to be sent to the currently selected Printer.")]
        public System.Drawing.Printing.PrintDocument PrintDocument
        {
            get
            {
                return this.Document;
            }
            set
            {
                if (this.document != value)
                {
                    this.document = value;
                }
            }
        }

        [DefaultValue(false), Description("Prints transparent Chart Panel when false.")]
        public bool PrintPanelBackground
        {
            get
            {
                return this.printPanelBackground;
            }
            set
            {
                this.printPanelBackground = value;
            }
        }

        [Description("Sets print dimensions of Chart proportional to those onscreen."), DefaultValue(true)]
        public bool Proportional
        {
            get
            {
                return this.proportional;
            }
            set
            {
                base.SetBooleanProperty(ref this.proportional, value);
            }
        }

        [Description("Sets Chart printing detail resolution (from 1 to 100)"), DefaultValue(1)]
        public int Resolution
        {
            get
            {
                return this.resolution;
            }
            set
            {
                if ((value >= 1) || (value <= 100))
                {
                    this.resolution = value;
                }
            }
        }

        [Description(""), DefaultValue(false)]
        public bool UseAntiAlias
        {
            get
            {
                return this.useAntiAlias;
            }
            set
            {
                base.SetBooleanProperty(ref this.useAntiAlias, value);
            }
        }

        internal class ComponentEditor : UITypeEditor
        {
            public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
            {
                Printer printer = (Printer) value;
                PrintPreview.ShowModal(printer.Chart);
                if (context != null)
                {
                    context.OnComponentChanged();
                }
                return true;
            }

            public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
            {
                return UITypeEditorEditStyle.Modal;
            }
        }
    }
}

