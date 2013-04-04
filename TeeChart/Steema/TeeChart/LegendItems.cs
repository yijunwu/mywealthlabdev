namespace Steema.TeeChart
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Reflection;

    [Description("Specialized List class to hold Legend items of type LegendItem")]
    public sealed class LegendItems : List<LegendItem>
    {
        private bool custom;
        private Legend iLegend;

        public LegendItems(Legend legend)
        {
            this.iLegend = legend;
        }

        [DefaultValue(false)]
        public bool Custom
        {
            get
            {
                return this.custom;
            }
            set
            {
                this.custom = value;
            }
        }

        public LegendItem this[int index]
        {
            get
            {
                while (index > (base.Count - 1))
                {
                    base.Add(null);
                }
                LegendItem item = base[index];
                if (item == null)
                {
                    item = new LegendItem(this.iLegend);
                }
                return item;
            }
            set
            {
                base[index] = value;
            }
        }
    }
}

