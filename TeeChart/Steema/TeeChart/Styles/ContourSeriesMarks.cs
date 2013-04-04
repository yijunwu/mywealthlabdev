namespace Steema.TeeChart.Styles
{
    using Steema.TeeChart.Drawing;
    using System;
    using System.ComponentModel;

    public class ContourSeriesMarks : TeeBase
    {
        internal bool antioverlap;
        internal bool atsegments = true;
        internal bool colorlevel;
        internal int density = 4;
        internal Contour iSeries;
        internal int margin;

        public ContourSeriesMarks(Contour cs)
        {
            this.iSeries = cs;
        }

        [DefaultValue(false)]
        public bool AntiOverlap
        {
            get
            {
                return this.antioverlap;
            }
            set
            {
                if (this.antioverlap != value)
                {
                    this.antioverlap = value;
                    this.iSeries.Invalidate();
                }
            }
        }

        [DefaultValue(true)]
        public bool AtSegments
        {
            get
            {
                return this.atsegments;
            }
            set
            {
                if (this.atsegments != value)
                {
                    this.atsegments = value;
                    this.iSeries.Invalidate();
                }
            }
        }

        [DefaultValue(false)]
        public bool ColorLevel
        {
            get
            {
                return this.colorlevel;
            }
            set
            {
                if (this.colorlevel != value)
                {
                    this.colorlevel = value;
                    this.iSeries.Invalidate();
                }
            }
        }

        [DefaultValue(4)]
        public int Density
        {
            get
            {
                return this.density;
            }
            set
            {
                if (this.density != value)
                {
                    this.density = value;
                    this.iSeries.Invalidate();
                }
            }
        }

        [DefaultValue(0)]
        public int Margin
        {
            get
            {
                return this.margin;
            }
            set
            {
                if (this.margin != value)
                {
                    this.margin = value;
                    this.iSeries.Invalidate();
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool Visible
        {
            get
            {
                return this.iSeries.Marks.Visible;
            }
            set
            {
                this.iSeries.Marks.Visible = value;
            }
        }
    }
}

