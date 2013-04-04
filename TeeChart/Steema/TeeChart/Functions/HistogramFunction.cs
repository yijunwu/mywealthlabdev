namespace Steema.TeeChart.Functions
{
    using Steema.TeeChart;
    using Steema.TeeChart.Styles;
    using System;
    using System.ComponentModel;

    public class HistogramFunction : Function
    {
        private bool cumulative;
        private int numbins;

        public HistogramFunction() : this(null)
        {
        }

        public HistogramFunction(Chart c) : base(c)
        {
            this.numbins = 20;
            base.CanUsePeriod = false;
            base.SingleSource = true;
        }

        public override void AddPoints(Array source)
        {
            if ((!base.updating && (source != null)) && (source.Length > 0))
            {
                Series series = (Series) source.GetValue(0);
                base.Series.Clear();
                int count = series.Count;
                if (count > 0)
                {
                    double[] bins = new double[this.numbins];
                    double[] counts = new double[this.numbins];
                    double[] data = new double[series.mandatory.Value.Length];
                    for (int i = 0; i < data.Length; i++)
                    {
                        data[i] = series.mandatory[i];
                    }
                    Histogram(data, 0, count - 1, ref bins, ref counts, this.numbins, series.mandatory.Minimum, series.mandatory.Maximum);
                    if (this.cumulative)
                    {
                        for (int j = 1; j < this.numbins; j++)
                        {
                            counts[j] += counts[j - 1];
                        }
                    }
                    base.Series.notMandatory.Count = this.numbins;
                    base.Series.notMandatory.Value = bins;
                    base.Series.mandatory.Value = counts;
                    base.Series.mandatory.Count = this.numbins;
                }
            }
        }

        public override string Description()
        {
            return Texts.FunctionHistogram;
        }

        public static void Histogram(double[] data, int lb, int ub, ref double[] bins, ref double[] counts, int nbins, double min, double max)
        {
            if (min == max)
            {
                min = (min - Math.Floor((double) (0.5 * nbins))) - 0.5;
                max = (max + Math.Ceiling((double) (0.5 * nbins))) + 0.5;
            }
            double num = max - min;
            double num2 = num / ((double) nbins);
            double num3 = ((double) nbins) / num;
            for (int i = 0; i < nbins; i++)
            {
                bins[i] = (min + (0.5 * num2)) + (num2 * i);
                counts[i] = 0.0;
            }
            int num6 = 0;
            for (int j = lb; j <= ub; j++)
            {
                int index = (int) ((data[j] - min) * num3);
                if ((index >= 0) && (index < nbins))
                {
                    counts[index]++;
                }
                else if (index >= nbins)
                {
                    num6++;
                }
            }
            counts[nbins - 1] += num6;
        }

        [Description("Cumulative histogram")]
        public bool Cumulative
        {
            get
            {
                return this.cumulative;
            }
            set
            {
                base.SetBooleanProperty(ref this.cumulative, value);
            }
        }

        [Description("Defines number of bins."), DefaultValue(20)]
        public int NumBins
        {
            get
            {
                return this.numbins;
            }
            set
            {
                base.SetIntegerProperty(ref this.numbins, value);
            }
        }
    }
}

