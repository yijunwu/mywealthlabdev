namespace WealthLab.Indicators
{
    using System;
    using WealthLab;

    public class Parabolic : DataSeries
    {
        private Bars bars_1;
        private bool bool_2;
        private bool bool_3;
        private double double_1;
        private double double_2;
        private double double_3;
        private double double_4;
        private double double_5;
        private double double_6;
        private double double_7;

        public Parabolic(Bars bars, double accelUp, double accelDown, double accelMax, string description) : base(bars, description)
        {
            this.bool_2 = true;
            this.bool_3 = true;
            this.bars_1 = bars;
            base.FirstValidValue++;
            this.double_2 = accelDown;
            this.double_3 = accelMax;
            this.double_1 = accelUp;
            this.double_4 = accelUp;
            this.double_5 = bars.Low[0];
            this.double_6 = bars.High[0];
            this.double_7 = bars.Low[0];
            for (int i = 1; i < bars.Count; i++)
            {
                double num;
                if (this.bool_2)
                {
                    if (bars.Low[i] < this.double_5)
                    {
                        this.bool_2 = false;
                        this.double_5 = this.double_6;
                        this.double_4 = accelDown;
                        this.double_7 = bars.Low[i];
                        this.bool_3 = true;
                    }
                    else
                    {
                        if (bars.High[i] > this.double_6)
                        {
                            this.double_6 = bars.High[i];
                            if (!this.bool_3)
                            {
                                this.double_4 += accelUp;
                                if (this.double_4 > accelMax)
                                {
                                    this.double_4 = accelMax;
                                }
                            }
                        }
                        this.bool_3 = false;
                        num = this.double_6 - this.double_5;
                        num *= this.double_4;
                        this.double_5 += num;
                        double num2 = Math.Min(bars.Low[i], bars.Low[i - 1]);
                        if (this.double_5 > num2)
                        {
                            this.double_5 = num2;
                        }
                    }
                }
                else if (bars.High[i] > this.double_5)
                {
                    this.bool_2 = true;
                    this.double_5 = this.double_7;
                    this.double_4 = accelUp;
                    this.double_6 = bars.High[i];
                    this.bool_3 = true;
                }
                else
                {
                    if (bars.Low[i] < this.double_7)
                    {
                        this.double_7 = bars.Low[i];
                        if (!this.bool_3)
                        {
                            this.double_4 += accelDown;
                            if (this.double_4 > accelMax)
                            {
                                this.double_4 = accelMax;
                            }
                        }
                    }
                    this.bool_3 = false;
                    num = this.double_7 - this.double_5;
                    num *= this.double_4;
                    this.double_5 += num;
                    double num3 = Math.Max(bars.High[i], bars.High[i - 1]);
                    if (this.double_5 < num3)
                    {
                        this.double_5 = num3;
                    }
                }
                base[i] = this.double_5;
            }
        }

        public override void CalculatePartialValue()
        {
            if (((this.bars_1.Count != 0) && (this.bars_1.Low.PartialValue != double.NaN)) && (this.bars_1.High.PartialValue != double.NaN))
            {
                double num;
                if (this.bool_2)
                {
                    if (this.bars_1.Low.PartialValue < this.double_5)
                    {
                        this.double_5 = this.double_6;
                    }
                    else
                    {
                        if (this.bars_1.High.PartialValue > this.double_6)
                        {
                            this.double_6 = this.bars_1.High.PartialValue;
                            if (!this.bool_3)
                            {
                                this.double_4 += this.double_1;
                                if (this.double_4 > this.double_3)
                                {
                                    this.double_4 = this.double_3;
                                }
                            }
                        }
                        this.bool_3 = false;
                        num = this.double_6 - this.double_5;
                        num *= this.double_4;
                        this.double_5 += num;
                        double num2 = Math.Min(this.bars_1.Low.PartialValue, this.bars_1.Low.PartialValue - 1.0);
                        if (this.double_5 > num2)
                        {
                            this.double_5 = num2;
                        }
                    }
                }
                else if (this.bars_1.High.PartialValue > this.double_5)
                {
                    this.double_5 = this.double_7;
                }
                else if (this.bars_1.Low.PartialValue < this.double_7)
                {
                    this.double_7 = this.bars_1.Low.PartialValue;
                    if (!this.bool_3)
                    {
                        this.double_4 += this.double_2;
                        if (this.double_4 > this.double_3)
                        {
                            this.double_4 = this.double_3;
                        }
                    }
                    this.bool_3 = false;
                    num = this.double_7 - this.double_5;
                    num *= this.double_4;
                    this.double_5 += num;
                    double num3 = Math.Max(this.bars_1.High.PartialValue, this.bars_1.High.PartialValue - 1.0);
                    if (this.double_5 < num3)
                    {
                        this.double_5 = num3;
                    }
                }
                base.PartialValue = this.double_5;
            }
            else
            {
                base.PartialValue = double.NaN;
            }
        }

        public static Parabolic Series(Bars bars, double accelUp, double accelDown, double accelMax)
        {
            string key = string.Concat(new object[] { "Parabolic(", accelUp, ", ", accelDown, ", ", accelMax, ")" });
            if (bars.Cache.ContainsKey(key))
            {
                return (Parabolic) bars.Cache[key];
            }
            Parabolic parabolic = new Parabolic(bars, accelUp, accelDown, accelMax, key);
            bars.Cache[key] = parabolic;
            return parabolic;
        }
    }
}

