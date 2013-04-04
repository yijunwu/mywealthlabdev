namespace Steema.TeeChart.Tools
{
    using Steema.TeeChart;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;

    public class SubChart
    {
        private TChart chart;
        private string data;
        private Tool iTool;

        public SubChart() : this((ChartCollection) null)
        {
        }

        public SubChart(Steema.TeeChart.Chart c) : this()
        {
        }

        public SubChart(ChartCollection collection)
        {
            if (collection != null)
            {
                this.iTool = collection.Owner;
                if (this.iTool != null)
                {
                    this.GetChart();
                }
            }
        }

        public Rectangle Bounds()
        {
            return this.Chart.Bounds;
        }

        private TChart GetChart()
        {
            if (this.chart == null)
            {
                this.chart = new InnerChart();
                (this.chart as InnerChart).IParent = this;
                this.GetData();
            }
            return this.chart;
        }

        private void GetData()
        {
            if (!Utils.IsNullOrEmpty(this.data))
            {
                Steema.TeeChart.Chart chart = this.chart.Chart;
                this.chart.Import.InternalLoadViewState(this.chart.Import.DecodeBase64(this.data), ref chart);
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public TChart Chart
        {
            get
            {
                return this.GetChart();
            }
            set
            {
                this.chart = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string Data
        {
            get
            {
                this.data = this.chart.Export.GetBase64((MemoryStream) this.chart.Export.InternalSaveViewState(this.chart.Chart));
                return this.data;
            }
            set
            {
                this.data = value;
            }
        }

        [DefaultValue(100), Description("Sets the Chart Height in pixels.")]
        public int Height
        {
            get
            {
                return Utils.Round((float) this.GetChart().Height);
            }
            set
            {
                this.GetChart().Height = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Tool ITool
        {
            get
            {
                return this.iTool;
            }
            set
            {
                this.iTool = value;
            }
        }

        [Description("Sets the Chart Left position in pixels."), DefaultValue(50)]
        public int Left
        {
            get
            {
                return this.GetChart().Left;
            }
            set
            {
                this.GetChart().Left = value;
            }
        }

        [DefaultValue(50), Description("Sets the Chart Top position in pixels.")]
        public int Top
        {
            get
            {
                return this.GetChart().Top;
            }
            set
            {
                this.GetChart().Top = value;
            }
        }

        [Description("Sets the Chart Width in pixels."), DefaultValue(150)]
        public int Width
        {
            get
            {
                return Utils.Round((float) this.GetChart().Width);
            }
            set
            {
                this.GetChart().Width = value;
            }
        }
    }
}

