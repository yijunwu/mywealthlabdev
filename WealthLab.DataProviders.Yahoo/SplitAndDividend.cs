using System;
using System.Collections.Generic;
using WealthLab;

///WYJ fix, original name: Class28
internal class SplitAndDividend  ///WYJ note, class for dividend and split
{
    private List<FundamentalItem> dividend = new List<FundamentalItem>();
    private List<FundamentalItem> split = new List<FundamentalItem>();

    public List<FundamentalItem> getDividend()
    {
        return this.dividend;
    }

    public void setDividend(List<FundamentalItem> dividend)
    {
        this.dividend = dividend;
    }

    public List<FundamentalItem> getSplit()
    {
        return this.split;
    }

    public void setSplit(List<FundamentalItem> split)
    {
        this.split = split;
    }
}

