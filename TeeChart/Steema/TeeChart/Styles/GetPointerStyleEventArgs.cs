namespace Steema.TeeChart.Styles
{
    using System;
    using System.Drawing;

    public class GetPointerStyleEventArgs : EventArgs
    {
        private System.Drawing.Color color;
        private PointerStyles style;
        private readonly int valueIndex;

        public GetPointerStyleEventArgs(int valueIndex, PointerStyles style)
        {
            this.valueIndex = valueIndex;
            this.style = style;
        }

        public System.Drawing.Color Color
        {
            get
            {
                return this.color;
            }
            set
            {
                this.color = value;
            }
        }

        public PointerStyles Style
        {
            get
            {
                return this.style;
            }
            set
            {
                this.style = value;
            }
        }

        public int ValueIndex
        {
            get
            {
                return this.valueIndex;
            }
        }
    }
}

