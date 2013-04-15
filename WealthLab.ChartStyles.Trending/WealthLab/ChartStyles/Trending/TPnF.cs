namespace WealthLab.ChartStyles.Trending
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    //using WealthLab;

    public class TPnF
    {
        private Bars bars_0;
        internal bool bool_0;
        public IDictionary<int, PnF> Columns;
        private ControlPrice controlPrice_0;
        private const double double_0 = 1E-13;
        internal double double_1;
        internal int int_0;
        private int[] int_1;
        public List<PFTarget> PFTargets;
        public IDictionary<int, int> ReversalBars;
        public List<PnFTrendLine> TrendLines;

        public TPnF(Bars bars, ControlPrice priceField, double boxSize, int reversalBoxes, bool logMethod)
        {
            double volume = 0.0;
            int num8 = 0;
            int boxes = 1;
            int num10 = 0;
            bool oneStepBack = false;
            bool plotted = false;
            bool flag5 = priceField == ControlPrice.Close;
            StepBackState sbsNone = StepBackState.sbsNone;
            if (bars.Count >= 2)
            {
                double num5;
                double num6;
                this.bars_0 = bars;
                this.TrendLines = new List<PnFTrendLine>();
                this.Columns = new Dictionary<int, PnF>();
                this.ReversalBars = new Dictionary<int, int>();
                this.int_0 = Math.Max(reversalBoxes, 1);
                this.int_0 = Math.Min(this.int_0, 13);
                this.bool_0 = logMethod;
                this.controlPrice_0 = priceField;
                if (boxSize < 1E-06)
                {
                    this.double_1 = 1.0;
                }
                else
                {
                    this.double_1 = boxSize;
                }
                if (this.bool_0)
                {
                    if (this.double_1 < 0.01)
                    {
                        this.double_1 = 0.01;
                    }
                    this.double_1 = Math.Log(1.0 + (this.double_1 / 100.0));
                }
                if (flag5)
                {
                    num5 = bars.Close[0];
                    num6 = bars.Close[0];
                }
                else
                {
                    num5 = bars.High[0];
                    num6 = bars.Low[0];
                }
                if (this.bool_0)
                {
                    num5 = Math.Log(num5);
                    num6 = Math.Log(num6);
                }
                double low = this.CeilingToBox(num6);
                double high = this.FloorToBox(num5);
                bool flag = true;
                this.ReversalBars.Add(0, 0);
                bool directionUp = true;
                double reverseLevel = low;
                PnF nf = new PnF(0, num8, high, low, bars.Volume[0], reverseLevel, true, true, false, 1, false, false);
                this.Columns.Add(0, nf);
                for (int i = 1; i < bars.Count; i++)
                {
                    double num2;
                    flag = false;
                    if (flag5)
                    {
                        num5 = bars.Close[i];
                        num6 = num5;
                    }
                    else
                    {
                        num5 = bars.High[i];
                        num6 = bars.Low[i];
                    }
                    if (this.bool_0)
                    {
                        num5 = Math.Log(num5);
                        num6 = Math.Log(num6);
                    }
                    low = this.CeilingToBox(num6);
                    high = this.FloorToBox(num5);
                    if (nf.DirectionUp)
                    {
                        if (high > nf.High)
                        {
                            sbsNone = StepBackState.sbsNone;
                            boxes = this.Boxes(high, nf.Low);
                            reverseLevel = this.RoundToBox(high - (this.BoxSize * this.ReversalBoxes));
                            nf = new PnF(i, nf.Col, high, nf.Low, volume, reverseLevel, true, false, false, boxes, false, oneStepBack);
                        }
                        else
                        {
                            num2 = nf.High - (this.BoxSize * this.ReversalBoxes);
                            if ((flag = low <= num2) && (this.ReversalBoxes == 1))
                            {
                                bool flag6 = low > this.RoundToBox(nf.High - (this.BoxSize * 2.0));
                                switch (sbsNone)
                                {
                                    case StepBackState.sbsNone:
                                        if (flag6)
                                        {
                                            sbsNone = StepBackState.sbsProbationary;
                                        }
                                        ///goto  Label_03E9;  ///WYJ fix, simplify the flow
                                        break;

                                    case StepBackState.sbsProbationary:
                                        sbsNone = StepBackState.sbsConfirmed;
                                        ///goto  Label_03E9;
                                        break;

                                    default:
                                        if (flag6)
                                        {
                                            sbsNone = StepBackState.sbsProbationary;
                                        }
                                        else
                                        {
                                            sbsNone = StepBackState.sbsNone;
                                        }
                                        break;
                                }
                            }
                        }
                    }
                    else if (low < nf.Low)
                    {
                        sbsNone = StepBackState.sbsNone;
                        boxes = this.Boxes(nf.High, low);
                        reverseLevel = this.RoundToBox(low + (this.BoxSize * this.ReversalBoxes));
                        nf = new PnF(i, nf.Col, nf.High, low, volume, reverseLevel, false, false, false, boxes, false, oneStepBack);
                    }
                    else
                    {
                        num2 = nf.Low + (this.BoxSize * this.ReversalBoxes);
                        if ((flag = high >= num2) && (this.ReversalBoxes == 1))
                        {
                            bool flag7 = high < this.RoundToBox(nf.Low + (this.BoxSize * 2.0));
                            switch (sbsNone)
                            {
                                case StepBackState.sbsNone:
                                    if (flag7)
                                    {
                                        sbsNone = StepBackState.sbsProbationary;
                                    }
                                    ///goto  Label_03E9;  ///WYJ fix, simplify the flow
                                    break;

                                case StepBackState.sbsProbationary:
                                    sbsNone = StepBackState.sbsConfirmed;
                                    ///goto  Label_03E9;
                                    break;
                                
                                default:
                                    if (flag7)
                                    {
                                        sbsNone = StepBackState.sbsProbationary;
                                    }
                                    else
                                    {
                                        sbsNone = StepBackState.sbsNone;
                                    }
                                    break;
                            }
                            
                        }
                    }
                //Label_03E9:
                    if (!flag && (i == (num10 + 1)))
                    {
                        nf = this.method_2(nf, i, flag, sbsNone == StepBackState.sbsProbationary);
                    }
                    if (flag)
                    {
                        if ((this.ReversalBoxes <= 1) && (sbsNone == StepBackState.sbsConfirmed))
                        {
                            if (nf.DirectionUp)
                            {
                                directionUp = false;
                                high = this.RoundToBox(nf.High);
                                reverseLevel = this.RoundToBox(low + this.BoxSize);
                            }
                            else
                            {
                                directionUp = true;
                                low = this.RoundToBox(nf.Low);
                                reverseLevel = this.RoundToBox(high - this.BoxSize);
                            }
                            boxes = this.Boxes(high, low);
                            oneStepBack = true;
                            nf = new PnF(i, num8, high, low, volume, reverseLevel, directionUp, flag, plotted, boxes, false, true);
                            num10 = i;
                        }
                        else
                        {
                            int num12 = i - 1;
                            PnF nf2 = this.method_3(this.Columns[num12], num12);
                            this.Columns.Remove(num12);
                            this.Columns.Add(num12, nf2);
                            if (nf.DirectionUp)
                            {
                                directionUp = false;
                                high = nf.High;
                                reverseLevel = this.RoundToBox(low + (this.BoxSize * this.ReversalBoxes));
                            }
                            else
                            {
                                directionUp = true;
                                low = nf.Low;
                                reverseLevel = this.RoundToBox(high - (this.BoxSize * this.ReversalBoxes));
                            }
                            num8++;
                            this.ReversalBars.Add(num8, i);
                            boxes = this.Boxes(high, low);
                            oneStepBack = false;
                            nf = new PnF(i, num8, high, low, volume, reverseLevel, directionUp, flag, plotted, boxes, sbsNone == StepBackState.sbsProbationary, false);
                            num10 = i;
                        }
                    }
                    this.Columns.Add(i, nf);
                }
                int num13 = bars.Count - 1;
                PnF nf3 = this.method_3(this.Columns[num13], num13);
                this.Columns.Remove(num13);
                this.Columns.Add(num13, nf3);
                if (this.ReversalBoxes > 1)
                {
                    this.method_1();
                    this.method_0();
                }
            }
        }

        public int Boxes(double high, double double_2)
        {
            return (int) Math.Round((double) ((high - double_2) / this.double_1));
        }

        public double CeilingToBox(double double_2)
        {
            return (Math.Ceiling((double) (double_2 / this.double_1)) * this.double_1);
        }

        public double FloorToBox(double double_2)
        {
            return (Math.Floor((double) ((double_2 / this.double_1) + 1E-13)) * this.double_1);
        }

        internal void method_0()
        {
            if (this.ReversalBoxes >= 2)
            {
                if (this.PFTargets == null)
                {
                    this.PFTargets = new List<PFTarget>();
                }
                int boxes = this.Columns[0].Boxes;
                for (int i = 1; i < this.bars_0.Count; i++)
                {
                    PFTarget target;
                    PnF nf3 = this.Columns[i];
                    if (!nf3.Reversed)
                    {
                        goto Label_0234;
                    }
                    PnF nf = null;
                    int num5 = 0;
                    int num2 = i - 1;
                    while (num2 >= 0)
                    {
                        nf = this.Columns[num2];
                        if (nf.DirectionUp != nf3.DirectionUp)
                        {
                            goto Label_008C;
                        }
                        num2--;
                    }
                    goto Label_0092;
                Label_008C:
                    num5 = num2 - 1;
                Label_0092:
                    if (nf != null)
                    {
                        int col = nf.Col - 1;
                        bool flag3 = false;
                        bool flag = false;
                        if (nf.DirectionUp)
                        {
                            double num8 = 0.0;
                            double high = nf.High;
                            for (int j = num5; j >= 0; j--)
                            {
                                PnF nf2 = this.Columns[j];
                                if (!nf2.Plotted || (nf2.Col >= col))
                                {
                                    continue;
                                }
                                col = nf2.Col;
                                if (nf2.Low < high)
                                {
                                    high = nf2.Low;
                                }
                                if (nf2.DirectionUp == nf.DirectionUp)
                                {
                                    if (nf2.High > nf.High)
                                    {
                                        continue;
                                    }
                                    flag3 = true;
                                    num8 = nf2.High;
                                }
                                else if (flag3)
                                {
                                    flag = nf2.High > num8;
                                }
                                if (!flag)
                                {
                                    continue;
                                }
                                int num = (nf.Col - nf2.Col) + 1;
                                if (num < 6)
                                {
                                    continue;
                                }
                                bool flag2 = false;
                                using (List<PFTarget>.Enumerator enumerator2 = this.PFTargets.GetEnumerator())
                                {
                                    while (enumerator2.MoveNext())
                                    {
                                        PFTarget current = enumerator2.Current;
                                        if (current.EntryColumn == nf2.Col)
                                        {
                                            goto Label_019E;
                                        }
                                    }
                                    goto Label_01B1;
                                Label_019E:
                                    flag2 = true;
                                }
                            Label_01B1:
                                target = new PFTarget(this, i, nf.DirectionUp, nf2.Col, j, nf.Col, high, nf.High);
                                if (!flag2 && (target.TargetPrice >= nf.High))
                                {
                                    goto Label_0222;
                                }
                                if ((nf2.High - this.BoxSize) >= nf.High)
                                {
                                    break;
                                }
                                flag3 = false;
                                flag = false;
                                num8 = nf2.High;
                            }
                        }
                    }
                    continue;
                Label_0222:
                    this.PFTargets.Add(target);
                    continue;
                Label_0234:
                    if (nf3.Boxes > boxes)
                    {
                        foreach (PFTarget target2 in this.PFTargets)
                        {
                            if (target2.TargetIsUp)
                            {
                                if (nf3.DirectionUp)
                                {
                                    if (nf3.High > target2.ActivatePrice)
                                    {
                                        target2.BarActivated = i;
                                    }
                                }
                                else if (nf3.Low < target2.BasePrice)
                                {
                                    target2.BarCanceled = i;
                                }
                            }
                            else if (!nf3.DirectionUp)
                            {
                                if (nf3.Low < target2.ActivatePrice)
                                {
                                    target2.BarActivated = i;
                                }
                            }
                            else if (nf3.High > target2.BasePrice)
                            {
                                target2.BarCanceled = i;
                            }
                        }
                    }
                }
            }
        }

        ///WYJ note, code from Reflector, too many goto statements. But the version from ILSpy has some problems too. Will keep Reflector one.
        internal void method_1()
        {
            for (int i = 0; i < this.bars_0.Count; i++)
            {
                PnF nf = this.Columns[i];
                if (nf.Plotted)
                {
                    double low;
                    int num5 = i;
                    bool directionUp = nf.DirectionUp;
                    int col = nf.Col;
                    int boxes = nf.Boxes;
                    if (directionUp)
                    {
                        low = nf.Low;
                    }
                    else
                    {
                        low = nf.High;
                        boxes = -boxes;
                    }
                    PnFTrendLine item = new PnFTrendLine(this.bars_0, num5, low, num5, low, num5, col, PnFAngle.const_2, directionUp) {
                        int_0 = boxes
                    };
                    int num2 = 0;
                    bool flag = true;
                    foreach (PnFTrendLine line2 in this.TrendLines)
                    {
                        if (line2.EndColumn != -1)
                        {
                            continue;
                        }
                        line2.int_0 += boxes;
                        if (line2.IsRising == directionUp)
                        {
                            if (line2.int_0 == boxes)
                            {
                                flag = false;
                                line2.Reversals++;
                            }
                        }
                        else if (line2.IsRising != directionUp)
                        {
                            if (!line2.IsRising)
                            {
                                goto Label_0155;
                            }
                            switch (line2.Angle)
                            {
                                case PnFAngle.const_2:
                                    goto Label_0128;
                            }
                        }
                        goto Label_01A7;
                    Label_0128:
                        if (line2.int_0 < 2)
                        {
                            line2.EndColumn = col;
                            line2.EndBar = num5;
                        }
                        line2.int_0 -= 2;
                        goto Label_01A7;
                    Label_0155:
                        switch (line2.Angle)
                        {
                            case PnFAngle.const_2:
                                if (line2.int_0 > -2)
                                {
                                    line2.EndColumn = col;
                                    line2.EndBar = num5;
                                }
                                line2.int_0 += 2;
                                break;
                        }
                    Label_01A7:
                        if (directionUp && (boxes < line2.int_0))
                        {
                            num2++;
                        }
                        if (!directionUp && (boxes > line2.int_0))
                        {
                            num2++;
                        }
                    }
                    item.Accel = num2;
                    if (flag)
                    {
                        this.TrendLines.Add(item);
                    }
                }
            }
        }

        internal PnF method_2(PnF pnF_0, int int_2, bool bool_1, bool bool_2)
        {
            int col = pnF_0.Col;
            double high = pnF_0.High;
            double low = pnF_0.Low;
            double volume = pnF_0.Volume;
            double reverseLevel = pnF_0.ReverseLevel;
            bool directionUp = pnF_0.DirectionUp;
            bool plotted = pnF_0.Plotted;
            int boxes = pnF_0.Boxes;
            return new PnF(int_2, col, high, low, volume, reverseLevel, directionUp, bool_1, plotted, boxes, bool_2, pnF_0.OneStepBack);
        }

        internal PnF method_3(PnF pnF_0, int int_2)
        {
            int col = pnF_0.Col;
            double high = pnF_0.High;
            double low = pnF_0.Low;
            double volume = pnF_0.Volume;
            double reverseLevel = pnF_0.ReverseLevel;
            bool reversed = pnF_0.Reversed;
            bool directionUp = pnF_0.DirectionUp;
            int boxes = pnF_0.Boxes;
            bool oneStepBackProbationary = pnF_0.OneStepBackProbationary;
            return new PnF(int_2, col, high, low, volume, reverseLevel, directionUp, reversed, true, boxes, oneStepBackProbationary, pnF_0.OneStepBack);
        }

        public void PaintContributorBars(WealthScript wealthScript_0, Color XBarColor, Color OBarColor, Color NullBarColor)
        {
            int boxes = 1;
            wealthScript_0.SetBarColor(0, XBarColor);
            for (int i = 1; i < wealthScript_0.Bars.Count; i++)
            {
                PnF nf = this.Columns[i];
                if (nf.Reversed)
                {
                    boxes = nf.Boxes;
                    if (nf.DirectionUp)
                    {
                        wealthScript_0.SetBarColor(i, XBarColor);
                    }
                    else
                    {
                        wealthScript_0.SetBarColor(i, OBarColor);
                    }
                }
                else if (nf.Boxes > boxes)
                {
                    if (nf.DirectionUp)
                    {
                        wealthScript_0.SetBarColor(i, XBarColor);
                    }
                    else
                    {
                        wealthScript_0.SetBarColor(i, OBarColor);
                    }
                    boxes = nf.Boxes;
                }
                else
                {
                    wealthScript_0.SetBarColor(i, NullBarColor);
                }
            }
        }

        public double RoundToBox(double double_2)
        {
            return (Math.Round((double) (double_2 / this.double_1)) * this.double_1);
        }

        public double BoxSize
        {
            get
            {
                return this.double_1;
            }
            set
            {
                this.double_1 = value;
            }
        }

        public int[] DblTopDblBottom
        {
            get
            {
                if (this.int_1 == null)
                {
                    if (this.ReversalBars.Count < 2)
                    {
                        return null;
                    }
                    this.int_1 = new int[this.bars_0.Count];
                    bool flag = true;
                    int num3 = 0;
                    int num2 = 1;
                    int num4 = this.ReversalBars[1];
                    PnF nf = this.Columns[num4 - 1];
                    int boxes = nf.Boxes;
                    int num6 = boxes;
                    for (int i = num4; i < this.bars_0.Count; i++)
                    {
                        nf = this.Columns[i];
                        if (nf.Reversed)
                        {
                            num6 = boxes;
                            flag = true;
                        }
                        boxes = nf.Boxes;
                        this.int_1[i] = num3;
                        if ((boxes - num6) > 0)
                        {
                            if (nf.DirectionUp)
                            {
                                num2 = 1;
                            }
                            else
                            {
                                num2 = -1;
                            }
                            if (flag)
                            {
                                this.int_1[i] = num2 * 2;
                                flag = false;
                            }
                            num3 = num2;
                        }
                    }
                }
                return this.int_1;
            }
        }

        public bool LogMethod
        {
            get
            {
                return this.bool_0;
            }
            set
            {
                this.bool_0 = value;
            }
        }

        public ControlPrice PriceField
        {
            get
            {
                return this.controlPrice_0;
            }
            set
            {
                this.controlPrice_0 = value;
            }
        }

        public int ReversalBoxes
        {
            get
            {
                return this.int_0;
            }
            set
            {
                this.int_0 = value;
            }
        }
    }
}

