namespace Steema.TeeChart
{
    using Steema.TeeChart.Styles;
    using System;
    using System.Drawing;

    public class SymbolDrawEventArgs : EventArgs
    {
        private Rectangle rect;
        private Steema.TeeChart.Styles.Series series;
        private int valueIndex;

        public SymbolDrawEventArgs(Steema.TeeChart.Styles.Series s, int valind, Rectangle r)
        {
            this.series = s;
            this.valueIndex = valind;
            this.rect = r;
        }

        public Rectangle Rect
        {
            get
            {
                return this.rect;
            }
            set
            {
                this.rect = value;
            }
        }

        public Steema.TeeChart.Styles.Series Series
        {
            get
            {
                return this.series;
            }
            set
            {
                this.series = value;
            }
        }

        public int ValueIndex
        {
            get
            {
                return this.valueIndex;
            }
            set
            {
                this.valueIndex = value;
            }
        }
    }
}

