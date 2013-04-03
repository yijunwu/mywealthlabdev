namespace WealthLab.ChartStyles.Trending
{
    using System;

    public class PFTarget
    {
        public TPnF _tPnF;
        public double ActivatePrice;
        public int Bar;
        public int BarActivated;
        public int BarCanceled;
        public double BasePrice;
        public int EntryColumn;
        public int EntryColumnBar;
        public int ExitColumn;
        public int HorizontalCount;
        public bool TargetIsUp;
        public double TargetPrice;

        public PFTarget(TPnF tpnf, int barInitiate, bool upTarget, int entryColumn, int entryColBar, int exitColumn, double basePrice, double activePrice)
        {
            this._tPnF = tpnf;
            this.Bar = barInitiate;
            this.TargetIsUp = upTarget;
            this.EntryColumn = entryColumn;
            this.EntryColumnBar = entryColBar;
            this.ExitColumn = exitColumn;
            this.BarActivated = -1;
            this.BarCanceled = -1;
            this.HorizontalCount = (exitColumn - entryColumn) + 1;
            this.BasePrice = basePrice;
            this.ActivatePrice = activePrice;
            this.method_0();
        }

        private void method_0()
        {
            int num = -1;
            if (this.TargetIsUp)
            {
                num = 1;
            }
            if (this._tPnF.LogMethod)
            {
                double num2 = Math.Log(this.BasePrice);
                this.TargetPrice = Math.Pow(num2 + (num * ((this._tPnF.BoxSize * this._tPnF.ReversalBoxes) * this.HorizontalCount)), 2.7182818284590451);
            }
            else
            {
                this.TargetPrice = this.BasePrice + (num * ((this._tPnF.BoxSize * this._tPnF.ReversalBoxes) * this.HorizontalCount));
            }
        }
    }
}

