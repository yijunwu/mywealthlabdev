namespace Steema.TeeChart
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    [Description("Custom labels list")]
    public class AxisLabelsItems : List<AxisLabelItem>
    {
        internal Axis iAxis;

        public AxisLabelsItems(Axis a)
        {
            this.iAxis = a;
        }

        public void Add(AxisLabelItem item)
        {
            item.iAxisLabelsItems = this;
            base.Add(item);
        }

        public AxisLabelItem Add(double value)
        {
            AxisLabelItem item = new AxisLabelItem(this.iAxis.Chart) {
                iAxisLabelsItems = this,
                Transparent = true,
                Value = value
            };
            base.Add(item);
            return item;
        }

        public AxisLabelItem Add(double value, string text)
        {
            AxisLabelItem item = this.Add(value);
            item.Text = text;
            return item;
        }

        public void Clear()
        {
            foreach (AxisLabelItem item in this)
            {
                item.Dispose();
            }
            this.iAxis.Labels.labelPos.Clear();
            base.Clear();
            this.iAxis.Chart.Invalidate();
        }

        public void CopyFrom(AxisLabelsItems Source)
        {
            this.Clear();
            for (int i = 0; i < Source.Count; i++)
            {
                this.Add(Source[i].Value, Source[i].Text);
            }
        }
    }
}

