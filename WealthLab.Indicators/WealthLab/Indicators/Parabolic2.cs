namespace WealthLab.Indicators
{
    using System;
    using WealthLab;

    public class Parabolic2 : DataSeries
    {
        private Bars bars_1;
        private bool bool_2;
        private double double_1;
        private double double_2;
        private double double_3;
        private double double_4;

        public Parabolic2(Bars bars, double accelUp, double accelDown, double accelMax, string description) : base(bars, description)
        {
            this.bool_2 = true;
            this.bars_1 = bars;
            base.FirstValidValue += 2;
            if (bars.Count >= 2)
            {
                this.double_1 = accelUp;
                this.double_2 = accelDown;
                this.double_3 = bars.Low[0];
                this.double_4 = Math.Max(bars.High[0], bars.High[1]);
                base[0] = this.double_3;
                base[1] = Math.Min(this.double_3, bars.Low[1]);
                for (int i = 2; i < bars.Count; i++)
                {
                    if (this.bool_2)
                    {
                        base[i] = base[i - 1] + (this.double_1 * (this.double_4 - base[i - 1]));
                    }
                    else
                    {
                        base[i] = base[i - 1] + (this.double_2 * (this.double_3 - base[i - 1]));
                    }
                    bool flag = false;
                    if (this.bool_2)
                    {
                        if (bars.Low[i] < base[i])
                        {
                            this.bool_2 = false;
                            flag = true;
                            base[i] = this.double_4;
                            this.double_3 = bars.Low[i];
                            this.double_2 = accelDown;
                        }
                    }
                    else if (bars.High[i] > base[i])
                    {
                        this.bool_2 = true;
                        flag = true;
                        base[i] = this.double_3;
                        this.double_4 = bars.High[i];
                        this.double_1 = accelUp;
                    }
                    if (!flag)
                    {
                        if (this.bool_2)
                        {
                            if (bars.High[i] > this.double_4)
                            {
                                this.double_4 = bars.High[i];
                                this.double_1 += accelUp;
                                if (this.double_1 > accelMax)
                                {
                                    this.double_1 = accelMax;
                                }
                            }
                            if (bars.Low[i - 1] < base[i])
                            {
                                base[i] = bars.Low[i - 1];
                            }
                            if (bars.Low[i - 2] < base[i])
                            {
                                base[i] = bars.Low[i - 2];
                            }
                        }
                        else
                        {
                            if (bars.Low[i] < this.double_3)
                            {
                                this.double_3 = bars.Low[i];
                                this.double_2 += accelDown;
                                if (this.double_2 > accelMax)
                                {
                                    this.double_2 = accelMax;
                                }
                            }
                            if (bars.High[i - 1] > base[i])
                            {
                                base[i] = bars.High[i - 1];
                            }
                            if (bars.High[i - 2] > base[i])
                            {
                                base[i] = bars.High[i - 2];
                            }
                        }
                    }
                }
            }
        }

        public override void CalculatePartialValue()
        {
            if (((this.bars_1.Count >= 2) && (this.bars_1.Low.PartialValue != double.NaN)) && (this.bars_1.High.PartialValue != double.NaN))
            {
                int count = this.bars_1.Count;
                bool flag = false;
                if (this.bool_2)
                {
                    base.PartialValue = base[count - 1] + (this.double_1 * (this.double_4 - base[count - 1]));
                    if (this.bars_1.Low.PartialValue < base.PartialValue)
                    {
                        base.PartialValue = this.double_4;
                        flag = true;
                    }
                }
                else
                {
                    base[count] = base[count - 1] + (this.double_2 * (this.double_3 - base[count - 1]));
                    if (this.bars_1.High.PartialValue > base.PartialValue)
                    {
                        base.PartialValue = this.double_3;
                        flag = true;
                    }
                }
                if (!flag)
                {
                    if (this.bool_2)
                    {
                        if (this.bars_1.Low[count - 1] < base.PartialValue)
                        {
                            base.PartialValue = this.bars_1.Low[count - 1];
                        }
                        if (this.bars_1.Low[count - 2] < base.PartialValue)
                        {
                            base.PartialValue = this.bars_1.Low[count - 2];
                        }
                    }
                    else
                    {
                        if (this.bars_1.High[count - 1] > base.PartialValue)
                        {
                            base.PartialValue = this.bars_1.High[count - 1];
                        }
                        if (this.bars_1.High[count - 2] > base.PartialValue)
                        {
                            base.PartialValue = this.bars_1.High[count - 2];
                        }
                    }
                }
            }
            else
            {
                base.PartialValue = double.NaN;
            }
        }

        public static Parabolic2 Series(Bars bars, double accelUp, double accelDown, double accelMax)
        {
            string key = string.Concat(new object[] { "Parabolic2(", accelUp, ", ", accelDown, ", ", accelMax, ")" });
            if (bars.Cache.ContainsKey(key))
            {
                return (Parabolic2) bars.Cache[key];
            }
            Parabolic2 parabolic = new Parabolic2(bars, accelUp, accelDown, accelMax, key);
            bars.Cache[key] = parabolic;
            return parabolic;
        }
    }
}

