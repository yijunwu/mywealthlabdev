using System;
using WealthLab;
using WealthLab.Indicators;

internal class Class49
{
    private ADX adx_0;
    private ADXR adxr_0;
    private Bars bars_0;
    private DIMinus diminus_0;
    private DIPlus diplus_0;
    private DX dx_0;
    private int int_0;

    public Class49(Bars bars_1, int int_1)
    {
        this.bars_0 = bars_1;
        this.int_0 = int_1;
        string key = "ADXR(" + int_1 + ")";
        string str2 = "DIMinus(" + int_1 + ")";
        string str3 = "DIPlus(" + int_1 + ")";
        string str4 = "DX(" + int_1 + ")";
        string str5 = "ADX(" + int_1 + ")";
        if (((bars_1.Cache.ContainsKey(key) && bars_1.Cache.ContainsKey(str2)) && (bars_1.Cache.ContainsKey(str3) && bars_1.Cache.ContainsKey(str4))) && bars_1.Cache.ContainsKey(str5))
        {
            this.adxr_0 = (ADXR) bars_1.Cache[key];
            this.diminus_0 = (DIMinus) bars_1.Cache[str2];
            this.diplus_0 = (DIPlus) bars_1.Cache[str3];
            this.dx_0 = (DX) bars_1.Cache[str4];
            this.adx_0 = (ADX) bars_1.Cache[str5];
        }
        else
        {
            if (bars_1.Cache.ContainsKey(key))
            {
                bars_1.Cache.Remove(key);
            }
            if (bars_1.Cache.ContainsKey(str2))
            {
                bars_1.Cache.Remove(str2);
            }
            if (bars_1.Cache.ContainsKey(str3))
            {
                bars_1.Cache.Remove(str3);
            }
            if (bars_1.Cache.ContainsKey(str4))
            {
                bars_1.Cache.Remove(str4);
            }
            if (bars_1.Cache.ContainsKey(str5))
            {
                bars_1.Cache.Remove(str5);
            }
            this.adxr_0 = new ADXR(bars_1, int_1, key);
            bars_1.Cache.Add(key, this.adxr_0);
            this.diminus_0 = new DIMinus(bars_1, int_1, str2);
            bars_1.Cache.Add(str2, this.diminus_0);
            this.diplus_0 = new DIPlus(bars_1, int_1, str3);
            bars_1.Cache.Add(str3, this.diplus_0);
            this.dx_0 = new DX(bars_1, int_1, str4);
            bars_1.Cache.Add(str4, this.dx_0);
            this.adx_0 = new ADX(bars_1, int_1, str5);
            bars_1.Cache.Add(str5, this.adx_0);
            if (bars_1.Count != 0)
            {
                double num = 0.0;
                double num2 = 0.0;
                double num3 = 0.0;
                double num4 = 0.0;
                double num5 = 0.0;
                double num6 = 0.0;
                double num7 = 0.0;
                double num8 = 0.0;
                this.diplus_0[0] = 0.0;
                this.diminus_0[0] = 0.0;
                this.dx_0[0] = 0.0;
                for (int i = 1; i < int_1; i++)
                {
                    num8 = TrueRange.Value(i, bars_1);
                    num4 = bars_1.High[i] - bars_1.High[i - 1];
                    if (num4 < 0.0)
                    {
                        num4 = 0.0;
                    }
                    num5 = bars_1.Low[i - 1] - bars_1.Low[i];
                    if (num5 < 0.0)
                    {
                        num5 = 0.0;
                    }
                    if (num4 > num5)
                    {
                        num5 = 0.0;
                    }
                    else if (num5 > num4)
                    {
                        num4 = 0.0;
                    }
                    num += num4;
                    num2 += num5;
                    num3 += num8;
                    this.diplus_0[i] = 0.0;
                    this.diminus_0[i] = 0.0;
                    this.dx_0[i] = 0.0;
                }
                for (int j = int_1; j < bars_1.Count; j++)
                {
                    num4 = bars_1.High[j] - bars_1.High[j - 1];
                    if (num4 < 0.0)
                    {
                        num4 = 0.0;
                    }
                    num5 = bars_1.Low[j - 1] - bars_1.Low[j];
                    if (num5 < 0.0)
                    {
                        num5 = 0.0;
                    }
                    if (num4 > num5)
                    {
                        num5 = 0.0;
                    }
                    else if (num5 > num4)
                    {
                        num4 = 0.0;
                    }
                    num = (num - (num / ((double) int_1))) + num4;
                    num2 = (num2 - (num2 / ((double) int_1))) + num5;
                    num3 = (num3 - (num3 / ((double) int_1))) + TrueRange.Value(j, bars_1);
                    if (num3 != 0.0)
                    {
                        this.diplus_0[j] = Math.Round((double) ((num / num3) * 100.0));
                        this.diminus_0[j] = Math.Round((double) ((num2 / num3) * 100.0));
                    }
                    else
                    {
                        this.diplus_0[j] = 0.0;
                        this.diminus_0[j] = 0.0;
                    }
                    num6 = Math.Abs((double) (this.diplus_0[j] - this.diminus_0[j]));
                    num7 = this.diplus_0[j] + this.diminus_0[j];
                    if (num7 != 0.0)
                    {
                        this.dx_0[j] = Math.Round((double) ((num6 / num7) * 100.0));
                    }
                    else
                    {
                        this.dx_0[j] = 0.0;
                    }
                }
                WilderMA rma = WilderMA.Series(this.dx_0, int_1);
                for (int k = 0; k < bars_1.Count; k++)
                {
                    this.adx_0[k] = rma[k];
                }
                for (int m = 0; m < int_1; m++)
                {
                    this.adx_0[m] = 0.0;
                }
                for (int n = int_1; n < bars_1.Count; n++)
                {
                    this.adxr_0[n] = (this.adx_0[n] + this.adx_0[n - int_1]) / 2.0;
                }
                this.method_4().FirstValidValue = int_1;
                this.method_0().FirstValidValue = int_1;
                this.method_2().FirstValidValue = int_1;
                this.method_1().FirstValidValue = int_1;
                this.method_3().FirstValidValue = int_1;
            }
        }
    }

    public ADXR method_0()
    {
        return this.adxr_0;
    }

    public DIMinus method_1()
    {
        return this.diminus_0;
    }

    public DIPlus method_2()
    {
        return this.diplus_0;
    }

    public DX method_3()
    {
        return this.dx_0;
    }

    public ADX method_4()
    {
        return this.adx_0;
    }
}

