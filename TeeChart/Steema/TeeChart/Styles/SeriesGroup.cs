namespace Steema.TeeChart.Styles
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using System;

    public sealed class SeriesGroup : TeeBase
    {
        private string name;
        private SeriesCollection series;

        public SeriesGroup() : this(null)
        {
        }

        public SeriesGroup(Chart c) : base(c)
        {
            this.series = new SeriesCollection(c);
        }

        public void Hide()
        {
            this.Active = SeriesGroupActive.No;
        }

        public void Show()
        {
            this.Active = SeriesGroupActive.Yes;
        }

        public override string ToString()
        {
            return this.name;
        }

        public SeriesGroupActive Active
        {
            get
            {
                if (this.series.Count == 0)
                {
                    return SeriesGroupActive.Yes;
                }
                int num = 0;
                foreach (Steema.TeeChart.Styles.Series series in this.series)
                {
                    if (series.Active)
                    {
                        num++;
                    }
                }
                if (num == this.series.Count)
                {
                    return SeriesGroupActive.Yes;
                }
                if (num == 0)
                {
                    return SeriesGroupActive.No;
                }
                return SeriesGroupActive.Some;
            }
            set
            {
                if ((value != SeriesGroupActive.Some) && (this.Active != value))
                {
                    foreach (Steema.TeeChart.Styles.Series series in this.series)
                    {
                        if (value == SeriesGroupActive.Yes)
                        {
                            series.Active = true;
                        }
                        else if (value == SeriesGroupActive.No)
                        {
                            series.Active = false;
                        }
                    }
                }
            }
        }

        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
            }
        }

        public SeriesCollection Series
        {
            get
            {
                return this.series;
            }
            set
            {
                this.series.InternalClear();
                foreach (Steema.TeeChart.Styles.Series series in value)
                {
                    this.series.InternalAdd(series);
                }
            }
        }
    }
}

