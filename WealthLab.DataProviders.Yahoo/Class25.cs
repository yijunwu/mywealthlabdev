using System;
using System.Collections.Generic;
using WealthLab;

internal class Class25
{
    private Bars bars_0;
    private Bars bars_1;
    private Enum3 enum3_0;
    private List<DateTime> list_0 = new List<DateTime>();

    public Class25(Bars bars_2, Bars bars_3)
    {
        this.bars_0 = bars_2;
        this.bars_1 = bars_3;
        this.method_2();
    }

    public Enum3 method_0()
    {
        return this.enum3_0;
    }

    public List<DateTime> method_1()
    {
        return this.list_0;
    }

    private void method_2()
    {
        if (this.bars_0.Count != this.bars_1.Count)
        {
            if (this.bars_0.Count > this.bars_1.Count)
            {
                this.enum3_0 = Enum3.const_2;
                for (int i = 0; i < this.bars_0.Count; i++)
                {
                    if (!this.bars_1.Date.Contains(this.bars_0.Date[i]))
                    {
                        this.list_0.Add(this.bars_0.Date[i]);
                    }
                }
            }
            else
            {
                this.enum3_0 = Enum3.const_1;
                for (int j = 0; j < this.bars_1.Count; j++)
                {
                    if (!this.bars_0.Date.Contains(this.bars_1.Date[j]))
                    {
                        this.list_0.Add(this.bars_1.Date[j]);
                    }
                }
            }
        }
    }
}

