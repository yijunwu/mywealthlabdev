namespace Steema.TeeChart.Tools
{
    using Steema.TeeChart;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Reflection;

    public sealed class ChartCollection : CollectionBase
    {
        private Tool owner;

        public ChartCollection(Tool tool)
        {
            this.owner = tool;
        }

        public SubChart Add(SubChart c)
        {
            if (base.List.IndexOf(c) == -1)
            {
                base.List.Add(c);
            }
            return c;
        }

        public TChart AddChart(string name)
        {
            SubChart chart = this.Add(new SubChart(this));
            chart.Chart.Name = name;
            chart.Chart.Header.Text = name;
            return chart.Chart;
        }

        protected override void OnInsertComplete(int index, object value)
        {
            base.OnInsertComplete(index, value);
            if (((this.Owner != null) && (this.Owner.Chart != null)) && (this.Owner.Chart.Parent != null))
            {
                (base.List[index] as SubChart).ITool = this.Owner;
                this.Owner.Chart.Parent.DoInvalidate();
            }
        }

        protected override void OnRemoveComplete(int index, object value)
        {
            base.OnRemoveComplete(index, value);
            this.Owner.Chart.Parent.DoInvalidate();
        }

        public SubChart this[int index]
        {
            get
            {
                return (SubChart) base.List[index];
            }
            set
            {
                base.List[index] = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Tool Owner
        {
            get
            {
                return this.owner;
            }
            set
            {
                this.owner = value;
            }
        }
    }
}

