namespace Steema.TeeChart.Styles
{
    using Steema.TeeChart;
    using System;
    using System.ComponentModel;

    public class OrgItem
    {
        private OrgShape fFormat = new OrgShape();
        private int index = -1;
        private OrgItems owner;

        public OrgItem(OrgItems Collection)
        {
            this.owner = Collection;
            if (Collection != null)
            {
                this.fFormat.Chart = Collection.owner.Chart;
            }
        }

        public OrgItem AddBrother(string Text)
        {
            return this.Series[this.Series.Add(Text, this.Superior)];
        }

        public OrgItem AddChild(string Text)
        {
            return this.Series[this.Series.Add(Text, this.Index)];
        }

        public OrgShape Format
        {
            get
            {
                return this.fFormat;
            }
            set
            {
                this.fFormat = value;
            }
        }

        public int Index
        {
            get
            {
                return this.index;
            }
            set
            {
                this.index = value;
            }
        }

        public OrgSeries Series
        {
            get
            {
                return this.owner.owner;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int Superior
        {
            get
            {
                return Utils.Round(this.Series.mandatory[this.Index]);
            }
            set
            {
                this.Series.mandatory[this.Index] = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string Text
        {
            get
            {
                return this.Series.Labels[this.Index];
            }
            set
            {
                this.Series.Labels[this.Index] = value;
            }
        }
    }
}

