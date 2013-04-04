namespace Steema.TeeChart.Styles
{
    using System;

    public class TagCloudDrawTagEventArgs : EventArgs
    {
        private int valueIndex;

        public TagCloudDrawTagEventArgs(int index)
        {
            this.valueIndex = index;
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

