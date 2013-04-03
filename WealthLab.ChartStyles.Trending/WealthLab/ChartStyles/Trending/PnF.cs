namespace WealthLab.ChartStyles.Trending
{
    using System;

    public class PnF : Column
    {
        private bool bool_3;
        private bool bool_4;
        private int int_2;

        public PnF(int int_3, int int_4, double high, double double_4, double volume, double reverseLevel, bool directionUp, bool reversed, bool plotted, int boxes, bool oneStepBackProbationary, bool oneStepBack) : base(int_3, int_4, high, double_4, volume, reverseLevel, directionUp, reversed, plotted)
        {
            this.int_2 = boxes;
            this.bool_3 = oneStepBackProbationary;
            this.bool_4 = oneStepBack;
        }

        public int Boxes
        {
            get
            {
                return this.int_2;
            }
        }

        public bool OneStepBack
        {
            get
            {
                return this.bool_4;
            }
        }

        public bool OneStepBackProbationary
        {
            get
            {
                return this.bool_3;
            }
        }
    }
}

