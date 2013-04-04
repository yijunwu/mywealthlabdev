namespace TASCIndicators
{
    using System;
    using System.Collections.Generic;
    using WealthLab;
    using WealthLab.Indicators;

    public class SemiCup
    {
        private int _boxLength = 4;
        private DataSeries _lnC;
        private DataSeries _mo;
        private IList<int> _pbList;
        public bool Active = true;
        internal int[] B = new int[6];
        public int BarInactive = -1;
        public List<int> FailureBars = new List<int>();
        internal double[] L = new double[6];
        public List<int> SemiCupBars = new List<int>();
        public int StartBar;
        internal CupStatus status;
        public Dictionary<int, CupStatus> Status;
        private WealthScript ws;

        internal SemiCup(WealthScript ww, int B0, int B5, DataSeries lnClose, DataSeries momentum, IList<int> pbL)
        {
            this.ws = ww;
            this._lnC = lnClose;
            this._mo = momentum;
            this._pbList = pbL;
            this.StartBar = B0;
            this.B[0] = B0;
            this.B[5] = B5;
            this.Status = new Dictionary<int, CupStatus>();
            this.UpdateGrid(B5);
            this.UpdateCup(B5);
        }

        internal double dX(int boxLengthMultiple)
        {
            int num = this.B[0];
            int num2 = this.B[2];
            if (boxLengthMultiple == 3)
            {
                num = this.B[2];
                num2 = this.B[5];
            }
            int num3 = num2 - num;
            double num4 = 0.0;
            double num5 = 0.0;
            for (int i = num + 1; i <= num2; i++)
            {
                if (this._mo[i] > 0.0)
                {
                    num4 += this._mo[i];
                }
                else
                {
                    num5 += Math.Abs(this._mo[i]);
                }
            }
            num4 /= (double) num3;
            num5 /= (double) num3;
            return ((100.0 * Math.Abs((double) (num4 - num5))) / (num4 + num5));
        }

        public double Level(int bar, int level)
        {
            double num = Lowest.Value(bar, this._lnC, bar - this.StartBar);
            double d = num;
            switch (level)
            {
                case 0:
                    break;

                case 1:
                case 2:
                case 3:
                case 4:
                {
                    double num3 = (this._lnC[this.StartBar] - num) / 5.0;
                    for (int i = 1; i <= level; i++)
                    {
                        d += num3;
                    }
                    break;
                }
                case 5:
                    d = this._lnC[this.StartBar];
                    break;

                default:
                    return 0.0;
            }
            return Math.Exp(d);
        }

        internal void UpdateCup(int bar)
        {
            if (this.Active && (bar >= this.B[5]))
            {
                switch (this.status)
                {
                    case CupStatus.Search:
                        if (this._lnC[bar] <= this.L[4])
                        {
                            this.UpdateGrid(bar);
                            if ((((this.dX(2) > 25.0) && (this.dX(3) < 25.0)) && ((Highest.Value(this.B[3], this._lnC, (this.B[3] - this.B[2]) + 1) < this.L[3]) && (Highest.Value(this.B[5], this._lnC, (this.B[5] - this.B[3]) + 1) < this.L[2]))) && (Highest.Value(this.B[2], this._lnC, (this.B[2] - this.B[1]) + 1) < this.L[4]))
                            {
                                this.status = CupStatus.SemiCupDetected;
                            }
                            break;
                        }
                        this.Active = false;
                        this.BarInactive = bar;
                        this.status = CupStatus.LevelExceeded;
                        break;

                    case CupStatus.SemiCupDetected:
                        if (this._lnC[bar] <= this.L[2])
                        {
                            if (this._lnC[bar] < this.L[0])
                            {
                                this.status = CupStatus.Failure;
                                this.FailureBars.Add(bar);
                            }
                            break;
                        }
                        this.Active = false;
                        this.BarInactive = bar;
                        this.status = CupStatus.FormingCup;
                        this.SemiCupBars.Add(bar);
                        break;
                }
                if (this.Status.ContainsKey(bar))
                {
                    this.Status.Remove(bar);
                }
                this.Status.Add(bar, this.status);
                if (this.status == CupStatus.Failure)
                {
                    this.status = CupStatus.Search;
                }
                if (this.status == CupStatus.SemiCupDetected)
                {
                    this.SemiCupBars.Add(bar);
                }
            }
        }

        private void UpdateGrid(int bar)
        {
            this.B[5] = bar;
            this._boxLength = (this.B[5] - this.B[0]) / 5;
            for (int i = 1; i < 5; i++)
            {
                this.B[i] = this.B[i - 1] + this._boxLength;
            }
            this.L[5] = this._lnC[this.B[0]];
            this.L[0] = Lowest.Value(bar, this._lnC, bar - this.B[0]);
            double num2 = (this.L[5] - this.L[0]) / 5.0;
            for (int j = 1; j <= 5; j++)
            {
                this.L[j] = this.L[j - 1] + num2;
            }
        }

        internal int BoxLength
        {
            get
            {
                return this._boxLength;
            }
        }
    }
}

