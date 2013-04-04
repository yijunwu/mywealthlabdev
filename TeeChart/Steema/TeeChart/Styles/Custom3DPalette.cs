namespace Steema.TeeChart.Styles
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using Steema.TeeChart.Themes;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;

    public class Custom3DPalette : Custom3D
    {
        private double blueFactor;
        protected internal bool bUseColorRange;
        protected internal bool bUsePalette;
        private ArrayList custom3DPalette;
        private Color endColor;
        private double greenFactor;
        private Color iEnd;
        private Color iMid;
        protected internal int iPaletteSteps;
        private int iRangeA;
        private int iRangeB;
        private int iRangeG;
        private int iRangeMidA;
        private int iRangeMidB;
        private int iRangeMidG;
        private int iRangeMidR;
        private int iRangeR;
        private double iValueRangeInv;
        private Color midColor;
        private List<GridPalette> palette;
        private double paletteMin;
        private double paletteRange;
        private double paletteStep;
        private PaletteStyles paletteStyle;
        private ChartPen pen;
        private double redFactor;
        protected bool sameBrush;
        private Color startColor;
        private bool usePaletteMin;

        public event GetColorEventHandler GetValueColor;

        public Custom3DPalette() : this(null)
        {
        }

        public Custom3DPalette(Chart c) : base(c)
        {
            this.startColor = Color.Navy;
            this.endColor = Color.White;
            this.midColor = Color.Transparent;
            this.iPaletteSteps = 0x20;
            this.bUseColorRange = true;
            this.custom3DPalette = new ArrayList();
            this.palette = new List<GridPalette>();
            this.redFactor = 2.0;
            this.greenFactor = 1.0;
            this.blueFactor = 1.0;
            this.CalcColorRange();
        }

        public int AddPalette(double aValue, Color aColor)
        {
            GridPalette item = new GridPalette(aValue, aColor);
            for (int i = 0; i < this.palette.Count; i++)
            {
                if (aValue < this.palette[i].UpToValue)
                {
                    this.palette.Insert(i, item);
                    return i;
                }
            }
            this.palette.Add(item);
            return this.palette.IndexOf(item);
        }

        protected override void AddValues(Array source)
        {
            base.AddValues(source);
            this.CreateDefaultPalette(0);
        }

        protected internal void CalcColorRange()
        {
            this.iEnd = this.EndColor;
            if (this.MidColor != Color.Transparent)
            {
                this.iMid = this.MidColor;
                this.iRangeMidA = this.StartColor.A - this.iMid.A;
                this.iRangeMidR = this.StartColor.R - this.iMid.R;
                this.iRangeMidG = this.StartColor.G - this.iMid.G;
                this.iRangeMidB = this.StartColor.B - this.iMid.B;
                this.iRangeA = this.iMid.A - this.iEnd.A;
                this.iRangeR = this.iMid.R - this.iEnd.R;
                this.iRangeG = this.iMid.G - this.iEnd.G;
                this.iRangeB = this.iMid.B - this.iEnd.B;
            }
            else
            {
                this.iRangeA = this.StartColor.A - this.iEnd.A;
                this.iRangeR = this.StartColor.R - this.iEnd.R;
                this.iRangeG = this.StartColor.G - this.iEnd.G;
                this.iRangeB = this.StartColor.B - this.iEnd.B;
            }
        }

        private void CheckPaletteEmpty()
        {
            if ((base.Count > 0) && (this.palette.Count == 0))
            {
                this.CreateDefaultPalette(this.iPaletteSteps);
            }
        }

        public void ClearPalette()
        {
            if (this.palette != null)
            {
                this.palette.Clear();
            }
        }

        protected internal override int CountLegendItems()
        {
            if ((base.Count <= 0) || !this.sameBrush)
            {
                return base.CountLegendItems();
            }
            if (((base.Chart.Legend.LegendStyle == LegendStyles.Palette) || (base.Chart.Legend.LegendStyle == LegendStyles.Auto)) && (base.Chart.Legend.MaxNumRows <= this.palette.Count))
            {
                return base.Chart.Legend.MaxNumRows;
            }
            return this.palette.Count;
        }

        protected internal void CreateDefaultPalette(int numSteps)
        {
            double paletteStep;
            this.ClearPalette();
            double num = 3.1415926535897931 / ((double) numSteps);
            switch (this.paletteStyle)
            {
                case PaletteStyles.Pale:
                case PaletteStyles.Rainbow:
                    break;

                case PaletteStyles.Strong:
                    num = 2.0 * num;
                    break;

                default:
                    num = 255.0 / ((double) numSteps);
                    break;
            }
            if (this.paletteStep == 0.0)
            {
                if (this.paletteRange == 0.0)
                {
                    paletteStep = base.mandatory.Range / ((double) Math.Max(1, numSteps - 1));
                }
                else
                {
                    paletteStep = this.paletteRange / ((double) numSteps);
                }
            }
            else
            {
                paletteStep = this.paletteStep;
            }
            double num3 = this.usePaletteMin ? this.paletteMin : base.mandatory.Minimum;
            for (int i = 0; i < numSteps; i++)
            {
                Color color;
                double num6 = num * i;
                if (this.paletteStyle == PaletteStyles.Rainbow)
                {
                    color = Theme.RainbowPalette[i % Theme.RainbowPalette.Length];
                }
                else
                {
                    int num4;
                    if (this.paletteStyle == PaletteStyles.GrayScale)
                    {
                        num4 = Utils.Round(num6);
                        color = Utils.FromArgb(num4, num4, num4);
                    }
                    else if (this.paletteStyle == PaletteStyles.InvertedGray)
                    {
                        num4 = 0xff - Utils.Round(num6);
                        color = Utils.FromArgb(num4, num4, num4);
                    }
                    else
                    {
                        color = Utils.FromArgb(Utils.Round((double) (127.0 * (Math.Sin(num6 / this.RedFactor) + 1.0))), Utils.Round((double) (127.0 * (Math.Sin(num6 / this.GreenFactor) + 1.0))), Utils.Round((double) (127.0 * (Math.Cos(num6 / this.BlueFactor) + 1.0))));
                    }
                }
                this.AddPalette(num3 + (paletteStep * i), color);
            }
            this.Invalidate();
        }

        protected internal override void CreateSubGallery(Series.SubGalleryEventHandler AddSubChart)
        {
            base.CreateSubGallery(AddSubChart);
            AddSubChart(Texts.ColorRange);
        }

        protected internal override void DoBeforeDrawChart()
        {
            base.DoBeforeDrawChart();
            this.CheckPaletteEmpty();
            this.sameBrush = this.SetSameBrush();
            this.iValueRangeInv = base.mandatory.Range;
            if (this.iValueRangeInv != 0.0)
            {
                this.iValueRangeInv = 1.0 / this.iValueRangeInv;
            }
        }

        protected override void DrawLegendShape(Graphics3D g, int valueIndex, Rectangle rect)
        {
            if ((valueIndex == -1) && this.UseColorRange)
            {
                Gradient gradient;
                bool visible = g.Brush.Visible;
                if (visible)
                {
                    g.Brush.Visible = false;
                }
                g.Pen = new ChartPen(Color.Black);
                g.Rectangle(rect);
                try
                {
                    gradient = new Gradient {
                        StartColor = this.EndColor,
                        MiddleColor = this.MidColor,
                        EndColor = this.StartColor
                    };
                    rect.Offset(1, 1);
                    gradient.Draw(g, rect);
                }
                finally
                {
                    gradient = null;
                }
                g.Brush.Visible = visible;
            }
            else
            {
                base.DrawLegendShape(g, valueIndex, rect);
            }
        }

        public override void GalleryChanged3D(bool Is3D)
        {
            base.GalleryChanged3D(Is3D);
            base.chart.Aspect.Zoom = 70;
            base.chart.Aspect.VertOffset = -2;
        }

        private int GetComponentValue(int result)
        {
            if (result < 0)
            {
                return 0;
            }
            if (result <= 0xff)
            {
                return result;
            }
            return 0xff;
        }

        private Color GetSurfacePaletteColor(double Y)
        {
            foreach (GridPalette palette in this.palette)
            {
                if (palette.UpToValue >= Y)
                {
                    return palette.Color;
                }
            }
            return this.palette[this.palette.Count - 1].Color;
        }

        protected internal Color GetValueColorValue(double aValue)
        {
            if (this.bUseColorRange)
            {
                if (this.iValueRangeInv == 0.0)
                {
                    return this.EndColor;
                }
                double num = aValue - base.mandatory.Minimum;
                if (num < 0.0)
                {
                    return this.EndColor;
                }
                if (aValue > base.mandatory.Maximum)
                {
                    return this.StartColor;
                }
                return this.RangePercent(num * this.iValueRangeInv);
            }
            if (this.sameBrush)
            {
                return this.GetSurfacePaletteColor(aValue);
            }
            return base.Color;
        }

        protected internal override Color LegendItemColor(int legendIndex)
        {
            Color emptyColor = Utils.EmptyColor;
            if ((base.Count <= 0) || !this.sameBrush)
            {
                return base.LegendItemColor(legendIndex);
            }
            if ((base.Chart.Legend.LegendStyle == LegendStyles.Palette) || (base.Chart.Legend.LegendStyle == LegendStyles.Auto))
            {
                int valueIndex = this.CountLegendItems();
                int num2 = valueIndex - 1;
                if ((base.iColors != null) && (base.iColors.Count == base.Count))
                {
                    double num3 = (base.Count - 1.0) / (valueIndex - 1.0);
                    num3 *= num2 - legendIndex;
                    valueIndex = Utils.Round(num3);
                    if (legendIndex == 0)
                    {
                        return this.ValueColor(base.Count - 1);
                    }
                    if (legendIndex == num2)
                    {
                        return this.ValueColor(0);
                    }
                    return this.ValueColor(valueIndex);
                }
                valueIndex = (this.palette.Count - 1) / (valueIndex - 1);
                valueIndex *= num2 - legendIndex;
                if (legendIndex == 0)
                {
                    return this.GetValueColorValue(this.palette[this.palette.Count - 1].UpToValue);
                }
                if (legendIndex == num2)
                {
                    return this.GetValueColorValue(this.palette[0].UpToValue);
                }
                return this.GetValueColorValue(this.palette[valueIndex].UpToValue);
            }
            if ((base.iColors != null) && (base.iColors.Count == base.Count))
            {
                return this.ValueColor((base.Count - legendIndex) - 1);
            }
            return this.GetValueColorValue(this.palette[(this.palette.Count - legendIndex) - 1].UpToValue);
        }

        public override string LegendString(int legendIndex, LegendTextStyles legendTextStyle)
        {
            string str = "";
            if ((base.Count > 0) && this.sameBrush)
            {
                int num = this.CountLegendItems();
                if (((base.Chart.Legend.LegendStyle == LegendStyles.Palette) || (base.Chart.Legend.LegendStyle == LegendStyles.Auto)) && ((num > legendIndex) && (this.palette.Count > 0)))
                {
                    int num2 = num - 1;
                    num = (this.palette.Count - 1) / (num - 1);
                    num *= num2 - legendIndex;
                    if (legendIndex == 0)
                    {
                        return this.palette[this.palette.Count - 1].UpToValue.ToString(base.ValueFormat);
                    }
                    if (legendIndex == num2)
                    {
                        return this.palette[0].UpToValue.ToString(base.ValueFormat);
                    }
                    return this.palette[num].UpToValue.ToString(base.ValueFormat);
                }
                if ((base.Chart.Legend.LegendStyle == LegendStyles.LastValues) && (this.palette.Count > 0))
                {
                    return this.palette[0].UpToValue.ToString(base.ValueFormat);
                }
                if (this.sameBrush)
                {
                    str = this.palette[(this.palette.Count - legendIndex) - 1].UpToValue.ToString(base.ValueFormat);
                }
                return str;
            }
            return base.LegendString(legendIndex, legendTextStyle);
        }

        protected internal Color OriginalValueColor(int valueIndex)
        {
            Color valueColorValue = base.ValueColor(valueIndex);
            if ((valueColorValue != Color.Transparent) && !base.bColorEach)
            {
                if (this.bUseColorRange || this.bUsePalette)
                {
                    valueColorValue = this.GetValueColorValue(base.mandatory[valueIndex]);
                }
                else if (Utils.ColorIsEmpty(valueColorValue))
                {
                    valueColorValue = base.Color;
                }
            }
            if (this.GetValueColor != null)
            {
                GetColorEventArgs e = new GetColorEventArgs(valueIndex, valueColorValue);
                this.GetValueColor(this, e);
                valueColorValue = e.Color;
            }
            return valueColorValue;
        }

        public override void PrepareForGallery(bool isEnabled)
        {
            base.PrepareForGallery(isEnabled);
            this.UseColorRange = false;
            this.Pen.Color = isEnabled ? Color.Black : Color.Gray;
            this.UsePalette = isEnabled;
        }

        private Color RangePercent(double percent)
        {
            if (this.midColor == Color.Transparent)
            {
                return Color.FromArgb(this.iEnd.A, this.GetComponentValue(this.iEnd.R + Utils.Round((double) (percent * this.iRangeR))), this.GetComponentValue(this.iEnd.G + Utils.Round((double) (percent * this.iRangeG))), this.GetComponentValue(this.iEnd.B + Utils.Round((double) (percent * this.iRangeB))));
            }
            if (percent < 0.5)
            {
                return Color.FromArgb(this.iEnd.A, this.GetComponentValue(this.iEnd.R + Utils.Round((double) ((2.0 * percent) * this.iRangeR))), this.GetComponentValue(this.iEnd.G + Utils.Round((double) ((2.0 * percent) * this.iRangeG))), this.GetComponentValue(this.iEnd.B + Utils.Round((double) ((2.0 * percent) * this.iRangeB))));
            }
            return Color.FromArgb(this.iMid.A, this.GetComponentValue(this.iMid.R + Utils.Round((double) ((2.0 * (percent - 0.5)) * this.iRangeMidR))), this.GetComponentValue(this.iMid.G + Utils.Round((double) ((2.0 * (percent - 0.5)) * this.iRangeMidG))), this.GetComponentValue(this.iMid.B + Utils.Round((double) ((2.0 * (percent - 0.5)) * this.iRangeMidB))));
        }

        protected override void SetChart(Chart c)
        {
            base.SetChart(c);
            if (this.pen != null)
            {
                this.pen.Chart = c;
            }
        }

        private void SetPaletteSteps(int value)
        {
            this.iPaletteSteps = value;
            this.CreateDefaultPalette(this.iPaletteSteps);
        }

        private void SetPaletteStyle(PaletteStyles value)
        {
            if (this.paletteStyle != value)
            {
                this.paletteStyle = value;
                this.ClearPalette();
                this.Invalidate();
            }
        }

        protected virtual bool SetSameBrush()
        {
            if (!this.bUseColorRange && !this.bUsePalette)
            {
                return false;
            }
            return (this.Brush.Image == null);
        }

        public override void SetSubGallery(int index)
        {
            if (index == 1)
            {
                this.UseColorRange = true;
                this.UsePalette = false;
            }
            else
            {
                base.SetSubGallery(index);
            }
        }

        private void SetUseColorRange(bool value)
        {
            base.SetBooleanProperty(ref this.bUseColorRange, value);
            if (value)
            {
                base.ColorEach = false;
            }
        }

        private void SetUsePalette(bool value)
        {
            base.SetBooleanProperty(ref this.bUsePalette, value);
            if (value)
            {
                base.ColorEach = false;
                this.CheckPaletteEmpty();
            }
        }

        public override Color ValueColor(int valueIndex)
        {
            Color emptyColor;
            if ((base.iColors == null) || (base.iColors.Count <= valueIndex))
            {
                emptyColor = Utils.EmptyColor;
            }
            else if (base.bColorEach)
            {
                emptyColor = base.ValueColor(valueIndex);
            }
            else
            {
                emptyColor = base.iColors[valueIndex];
            }
            if ((Utils.ColorIsEmpty(emptyColor) && (emptyColor != Color.Transparent)) && !base.bColorEach)
            {
                if (this.sameBrush)
                {
                    emptyColor = this.GetValueColorValue(base.mandatory[valueIndex]);
                }
                else if (Utils.ColorIsEmpty(emptyColor) || !this.sameBrush)
                {
                    emptyColor = base.Color;
                }
            }
            if (this.GetValueColor != null)
            {
                GetColorEventArgs e = new GetColorEventArgs(valueIndex, emptyColor);
                this.GetValueColor(this, e);
                emptyColor = e.Color;
            }
            return emptyColor;
        }

        [DefaultValue((double) 1.0), Description("")]
        public double BlueFactor
        {
            get
            {
                return this.blueFactor;
            }
            set
            {
                base.SetDoubleProperty(ref this.blueFactor, value);
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Defines the Brush to fill polygon interiors."), Category("Appearance")]
        public ChartBrush Brush
        {
            get
            {
                return base.bBrush;
            }
            set
            {
                base.bBrush = value;
            }
        }

        [Description("Determines the last Range palette color."), DefaultValue(typeof(Color), "White")]
        public Color EndColor
        {
            get
            {
                return this.endColor;
            }
            set
            {
                base.SetColorProperty(ref this.endColor, value);
                this.CalcColorRange();
            }
        }

        [DefaultValue((double) 1.0), Description("")]
        public double GreenFactor
        {
            get
            {
                return this.greenFactor;
            }
            set
            {
                base.SetDoubleProperty(ref this.greenFactor, value);
            }
        }

        [DefaultValue(typeof(Color), "Transparent"), Description("Determines the middle Range palette color.")]
        public Color MidColor
        {
            get
            {
                return this.midColor;
            }
            set
            {
                base.SetColorProperty(ref this.midColor, value);
                this.CalcColorRange();
            }
        }

        public List<GridPalette> Palette
        {
            get
            {
                return this.palette;
            }
        }

        [DefaultValue(0)]
        public double PaletteMin
        {
            get
            {
                return this.paletteMin;
            }
            set
            {
                base.SetDoubleProperty(ref this.paletteMin, value);
            }
        }

        [DefaultValue((double) 0.0)]
        public double PaletteRange
        {
            get
            {
                return this.paletteRange;
            }
            set
            {
                base.SetDoubleProperty(ref this.paletteRange, value);
            }
        }

        [DefaultValue(0)]
        public double PaletteStep
        {
            get
            {
                return this.paletteStep;
            }
            set
            {
                base.SetDoubleProperty(ref this.paletteStep, value);
            }
        }

        [Description("Sets number of entries in the default color Palette."), DefaultValue(0x20)]
        public int PaletteSteps
        {
            get
            {
                return this.iPaletteSteps;
            }
            set
            {
                this.SetPaletteSteps(value);
            }
        }

        [Description("Selects Pale or Strong colour palette."), DefaultValue(1)]
        public PaletteStyles PaletteStyle
        {
            get
            {
                return this.paletteStyle;
            }
            set
            {
                this.SetPaletteStyle(value);
            }
        }

        [Category("Appearance"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Defines the Pen to draw the Chart.")]
        public ChartPen Pen
        {
            get
            {
                if (this.pen == null)
                {
                    this.pen = new ChartPen(base.chart, Color.Black);
                }
                return this.pen;
            }
            set
            {
                this.pen = value;
            }
        }

        [DefaultValue((double) 2.0), Description("")]
        public double RedFactor
        {
            get
            {
                return this.redFactor;
            }
            set
            {
                base.SetDoubleProperty(ref this.redFactor, value);
            }
        }

        [Description("Sets 1 of 3 Colors to create the palette to fill the Surface polygons."), DefaultValue(typeof(Color), "Navy")]
        public Color StartColor
        {
            get
            {
                return this.startColor;
            }
            set
            {
                base.SetColorProperty(ref this.startColor, value);
                this.CalcColorRange();
            }
        }

        [Description("Sets gradient color palette to fill Surface polygons."), DefaultValue(true)]
        public bool UseColorRange
        {
            get
            {
                return this.bUseColorRange;
            }
            set
            {
                base.SetBooleanProperty(ref this.bUseColorRange, value);
            }
        }

        [DefaultValue(false), Description("Sets multi-color palette to fill Surface polygons.")]
        public bool UsePalette
        {
            get
            {
                return this.bUsePalette;
            }
            set
            {
                base.SetBooleanProperty(ref this.bUsePalette, value);
            }
        }

        [DefaultValue(false)]
        public bool UsePaletteMin
        {
            get
            {
                return this.usePaletteMin;
            }
            set
            {
                base.SetBooleanProperty(ref this.usePaletteMin, value);
            }
        }

        public class GetColorEventArgs : EventArgs
        {
            private System.Drawing.Color color;
            private readonly int valueIndex;

            public GetColorEventArgs(int valueIndex, System.Drawing.Color color)
            {
                this.valueIndex = valueIndex;
                this.color = color;
            }

            public System.Drawing.Color Color
            {
                get
                {
                    return this.color;
                }
                set
                {
                    this.color = value;
                }
            }

            public int ValueIndex
            {
                get
                {
                    return this.valueIndex;
                }
            }
        }

        public delegate void GetColorEventHandler(Series sender, Custom3DPalette.GetColorEventArgs e);
    }
}

