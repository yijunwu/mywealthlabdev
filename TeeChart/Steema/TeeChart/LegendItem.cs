namespace Steema.TeeChart
{
    using Steema.TeeChart.Drawing;
    using System;
    using System.Drawing;

    [Serializable]
    public sealed class LegendItem : TeeBase
    {
        private StringAlignment align;
        internal string FText;
        internal string FText2;
        private Legend iLegend;
        private int left;
        private Rectangle symbolRect;
        private int top;

        public LegendItem(Legend legend)
        {
            this.iLegend = legend;
        }

        private void SetTextVariable(ref string variable, string value)
        {
            if (!string.Equals(variable, value))
            {
                variable = value;
                this.iLegend.Items.Custom = true;
                this.iLegend.Invalidate();
            }
        }

        public StringAlignment Align
        {
            get
            {
                return this.align;
            }
            set
            {
                this.align = value;
            }
        }

        public int Left
        {
            get
            {
                return this.left;
            }
            set
            {
                this.left = value;
            }
        }

        public Rectangle SymbolRectangle
        {
            get
            {
                return this.symbolRect;
            }
            set
            {
                this.symbolRect = value;
            }
        }

        public string Text
        {
            get
            {
                return this.FText;
            }
            set
            {
                this.SetTextVariable(ref this.FText, value);
            }
        }

        public string Text2
        {
            get
            {
                return this.FText2;
            }
            set
            {
                this.SetTextVariable(ref this.FText2, value);
            }
        }

        public int Top
        {
            get
            {
                return this.top;
            }
            set
            {
                this.top = value;
            }
        }
    }
}

