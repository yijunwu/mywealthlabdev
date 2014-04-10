namespace WealthLab.Indicators
{
    using System;
    using WealthLab;

    public class PeakTroughCalculator
    {
        private DataSeries dataSeries_0;
        internal static DataSeries dataSeries_1;
        private double double_0;
        private double double_1;
        private double double_2;
        internal static double double_3;
        internal static double double_4;
        internal static double double_5;
        internal static double double_6;
        internal static double double_7;
        internal static int int_0;
        internal static int int_1;
        private WealthLab.Indicators.Peak peak;
        private WealthLab.Indicators.PeakBar peakBar;
        private PeakTroughMode peakTroughMode_0;
        internal static PeakTroughMode peakTroughMode_1;
        private WealthLab.Indicators.Trough trough;
        private WealthLab.Indicators.TroughBar troughBar;

        public PeakTroughCalculator(DataSeries source, double reversalAmount, PeakTroughMode mode)
        {
            string key = string.Concat(new object[] { "Peak(", source.Description, ",", reversalAmount, ",", mode, ")" });
            string str2 = string.Concat(new object[] { "Trough(", source.Description, ",", reversalAmount, ",", mode, ")" });
            string str3 = string.Concat(new object[] { "PeakBar(", source.Description, ",", reversalAmount, ",", mode, ")" });
            string str4 = string.Concat(new object[] { "TroughBar(", source.Description, ",", reversalAmount, ",", mode, ")" });
            if (source.Cache.ContainsKey(key))
            {
                this.peak = (WealthLab.Indicators.Peak) source.Cache[key];
                this.trough = (WealthLab.Indicators.Trough) source.Cache[str2];
                this.peakBar = (WealthLab.Indicators.PeakBar) source.Cache[str3];
                this.troughBar = (WealthLab.Indicators.TroughBar) source.Cache[str4];
            }
            else
            {
                this.peak = new WealthLab.Indicators.Peak(source, key);
                source.Cache.Add(key, this.peak);
                this.trough = new WealthLab.Indicators.Trough(source, str2);
                source.Cache.Add(str2, this.trough);
                this.peakBar = new WealthLab.Indicators.PeakBar(source, str3);
                source.Cache.Add(str3, this.peakBar);
                this.troughBar = new WealthLab.Indicators.TroughBar(source, str4);
                source.Cache.Add(str4, this.troughBar);
                if (source.Count != 0)
                {
                    this.peakTroughMode_0 = mode;
                    this.dataSeries_0 = source;
                    this.double_2 = reversalAmount;
                    bool flag = true;
                    bool flag2 = true;
                    double num = source[0];
                    double num2 = source[0];
                    this.double_0 = source[0];
                    this.double_1 = source[0];
                    int num3 = -1;
                    int num4 = -1;
                    int num5 = -1;
                    int num6 = -1;
                    for (int i = 0; i < source.Count; i++)
                    {
                        if (flag)
                        {
                            if (source[i] > this.double_1)
                            {
                                this.double_1 = source[i];
                                num5 = i;
                            }
                            if (this.method_0(i))
                            {
                                flag = false;
                                flag2 = true;
                                this.double_0 = source[i];
                                num = this.double_1;
                                num3 = num5;
                                num6 = i;
                            }
                        }
                        if (flag2)
                        {
                            if (source[i] < this.double_0)
                            {
                                this.double_0 = source[i];
                                num6 = i;
                            }
                            if (this.method_1(i))
                            {
                                flag2 = false;
                                flag = true;
                                this.double_1 = source[i];
                                num2 = this.double_0;
                                num4 = num6;
                                num5 = i;
                            }
                        }
                        this.peak[i] = num;
                        this.peakBar[i] = num3;
                        this.trough[i] = num2;
                        this.troughBar[i] = num4;
                    }
                }
            }
        }

        private bool method_0(int int_2)
        {
            if (this.peakTroughMode_0 == PeakTroughMode.Value)
            {
                return (this.dataSeries_0[int_2] <= (this.double_1 - this.double_2));
            }
            if (this.dataSeries_0[int_2] == this.double_1)
            {
                return false;
            }
            double num = ((this.double_1 - this.dataSeries_0[int_2]) * 100.0) / this.double_1;
            if (this.double_1 <= 0.0)
            {
                num = -num;
            }
            return (num >= this.double_2);
        }

        private bool method_1(int int_2)
        {
            if (this.peakTroughMode_0 == PeakTroughMode.Value)
            {
                return (this.dataSeries_0[int_2] >= (this.double_0 + this.double_2));
            }
            if (this.dataSeries_0[int_2] == this.double_0)
            {
                return false;
            }
            double num = ((this.dataSeries_0[int_2] - this.double_0) * 100.0) / this.double_0;
            if (this.double_0 <= 0.0)
            {
                num = -num;
            }
            return (num >= this.double_2);
        }

        internal static void smethod_0(int int_2, DataSeries dataSeries_2, double double_8, PeakTroughMode peakTroughMode_2)
        {
            double_3 = 0.0;
            double_4 = 0.0;
            int_0 = -1;
            int_1 = -1;
            if (dataSeries_2.Count != 0)
            {
                peakTroughMode_1 = peakTroughMode_2;
                dataSeries_1 = dataSeries_2;
                double_7 = double_8;
                bool flag2 = true;
                bool flag = true;
                double_3 = dataSeries_2[0];
                double_4 = dataSeries_2[0];
                double_6 = dataSeries_2[0];
                double_5 = dataSeries_2[0];
                int num2 = -1;
                int num3 = -1;
                for (int i = 0; i <= int_2; i++)
                {
                    if (flag2)
                    {
                        if (dataSeries_2[i] > double_5)
                        {
                            double_5 = dataSeries_2[i];
                            num2 = i;
                        }
                        if (smethod_1(i))
                        {
                            flag2 = false;
                            flag = true;
                            double_6 = dataSeries_2[i];
                            double_3 = double_5;
                            int_0 = num2;
                            num3 = i;
                        }
                    }
                    if (flag)
                    {
                        if (dataSeries_2[i] < double_6)
                        {
                            double_6 = dataSeries_2[i];
                            num3 = i;
                        }
                        if (smethod_2(i))
                        {
                            flag = false;
                            flag2 = true;
                            double_5 = dataSeries_2[i];
                            double_4 = double_6;
                            int_1 = num3;
                            num2 = i;
                        }
                    }
                }
            }
        }

        private static bool smethod_1(int int_2)
        {
            if (peakTroughMode_1 == PeakTroughMode.Value)
            {
                return (dataSeries_1[int_2] <= (double_5 - double_7));
            }
            if (dataSeries_1[int_2] == double_5)
            {
                return false;
            }
            double num = ((double_5 - dataSeries_1[int_2]) * 100.0) / double_5;
            if (double_5 <= 0.0)
            {
                num = -num;
            }
            return (num >= double_7);
        }

        private static bool smethod_2(int int_2)
        {
            if (peakTroughMode_1 == PeakTroughMode.Value)
            {
                return (dataSeries_1[int_2] >= (double_6 + double_7));
            }
            if (dataSeries_1[int_2] == double_6)
            {
                return false;
            }
            double num = ((dataSeries_1[int_2] - double_6) * 100.0) / double_6;
            if (double_6 <= 0.0)
            {
                num = -num;
            }
            return (num >= double_7);
        }

        public WealthLab.Indicators.Peak Peak
        {
            get
            {
                return this.peak;
            }
        }

        public WealthLab.Indicators.PeakBar PeakBar
        {
            get
            {
                return this.peakBar;
            }
        }

        public WealthLab.Indicators.Trough Trough
        {
            get
            {
                return this.trough;
            }
        }

        public WealthLab.Indicators.TroughBar TroughBar
        {
            get
            {
                return this.troughBar;
            }
        }
    }
}

