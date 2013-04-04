namespace TASCIndicators
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using WealthLab;
    using WealthLab.Indicators;

    public class SemiCups
    {
        public DataSeries closeMo;
        public Dictionary<int, SemiCup> Cups = new Dictionary<int, SemiCup>();
        private string dnArrow = Convert.ToChar(0xea).ToString();
        private Font font = new Font("Wingdings", 8f, FontStyle.Bold);
        public DataSeries LnC;
        private int minBars;
        public IList<int> PkBarList = new List<int>();
        private string upArrow = Convert.ToChar(0xe9).ToString();
        private WealthScript ws;

        public SemiCups(WealthScript ws, int minBars, DataSeries pkBarSer)
        {
            this.ws = ws;
            this.minBars = minBars;
            this.LnC = new DataSeries(ws.Bars, "Log(Close)");
            for (int i = 0; i < ws.Bars.Count; i++)
            {
                this.LnC[i] = Math.Log(ws.Bars.Close[i]);
            }
            this.closeMo = this.LnC - (this.LnC >> 1);
            int num2 = -1;
            int num3 = minBars / 2;
            for (int j = minBars; j < (this.LnC.Count - minBars); j++)
            {
                int item = (int) pkBarSer[j];
                if (item != num2)
                {
                    if (num2 == -1)
                    {
                        this.PkBarList.Add(item);
                    }
                    else
                    {
                        int num6 = this.PkBarList[this.PkBarList.Count - 1];
                        if (item > (num6 + num3))
                        {
                            this.PkBarList.Add(item);
                        }
                        else if (this.LnC[num6] < this.LnC[item])
                        {
                            this.PkBarList.Remove(num6);
                            this.PkBarList.Add(item);
                        }
                    }
                    num2 = item;
                }
            }
            foreach (int num7 in this.PkBarList)
            {
                this.Cups.Add(num7, new SemiCup(ws, num7, Math.Max(num7 + minBars, num7), this.LnC, this.closeMo, this.PkBarList));
            }
            this.ProcessSemiCups();
        }

        public List<SemiCup> ActiveSemiCups(int bar)
        {
            List<SemiCup> list = new List<SemiCup>();
            foreach (KeyValuePair<int, SemiCup> pair in this.Cups)
            {
                SemiCup item = pair.Value;
                if (((item.SemiCupBars.Count != 0) && (bar >= item.SemiCupBars[0])) && (bar <= item.BarInactive))
                {
                    list.Add(item);
                }
            }
            return list;
        }

        public void Draw()
        {
            Color color = Color.FromArgb(60, Color.Gray);
            foreach (KeyValuePair<int, SemiCup> pair in this.Cups)
            {
                SemiCup cup = pair.Value;
                int count = cup.SemiCupBars.Count;
                if (((count >= 1) && (cup.status != CupStatus.Search)) && ((cup.status != CupStatus.LevelExceeded) && (cup.status != CupStatus.Duplicate)))
                {
                    Color black = Color.Black;
                    if (cup.status == CupStatus.Failure)
                    {
                        black = Color.Red;
                    }
                    double d = cup.L[5];
                    double num3 = cup.L[0];
                    double num4 = (Math.Exp(d) - Math.Exp(num3)) / Math.Pow((double) (cup.B[0] - cup.B[5]), 10.0);
                    double num5 = Math.Exp(d);
                    double num6 = 0.98 * Math.Exp(num3);
                    double num7 = -1.0;
                    for (int i = cup.B[0]; i <= cup.B[5]; i++)
                    {
                        double num9 = (num4 * Math.Pow((double) (i - cup.B[5]), 10.0)) + num6;
                        double num10 = Math.Min(num5, num9);
                        if (num7 > -1.0)
                        {
                            this.ws.DrawLine(this.ws.PricePane, i - 1, num7, i, num10, black, LineStyle.Solid, 2);
                        }
                        num7 = num10;
                    }
                    int num11 = cup.SemiCupBars[0];
                    double[] numArray = new double[6];
                    numArray[5] = this.LnC[cup.B[0]];
                    numArray[0] = Lowest.Value(num11, this.LnC, num11 - cup.B[0]);
                    double num12 = (numArray[5] - numArray[0]) / 5.0;
                    for (int j = 1; j < 5; j++)
                    {
                        numArray[j] = numArray[j - 1] + num12;
                    }
                    for (int k = 0; k <= 5; k++)
                    {
                        double num15 = Math.Exp(cup.L[k]);
                        this.ws.DrawLine(this.ws.PricePane, cup.B[0], num15, cup.B[5], num15, color, LineStyle.Dashed, 2);
                        this.ws.DrawLine(this.ws.PricePane, cup.B[k], Math.Exp(d), cup.B[k], Math.Exp(num3), Color.Gray, LineStyle.Dashed, 1);
                    }
                    for (int m = 0; m < cup.FailureBars.Count; m++)
                    {
                        this.ws.AnnotateBar(this.dnArrow, cup.FailureBars[m], true, Color.Red, Color.Transparent, this.font);
                    }
                    if (cup.status == CupStatus.FormingCup)
                    {
                        this.ws.AnnotateBar(this.upArrow, cup.SemiCupBars[count - 1], false, Color.Blue, Color.Transparent, this.font);
                    }
                }
            }
        }

        private void ProcessSemiCups()
        {
            foreach (KeyValuePair<int, SemiCup> pair in this.Cups)
            {
                SemiCup cup = pair.Value;
                for (int i = pair.Key; i < this.LnC.Count; i++)
                {
                    cup.UpdateCup(i);
                }
            }
        }
    }
}

