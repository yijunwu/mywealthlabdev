using System;

///WYJ fix, original name: Class20
internal class SnDFactor   ///WYJ note: SnDFactor
{
    private DateTime dateTime;
    private double factor;
    private double factorForSplit;

    public SnDFactor()
    {
    }

    public SnDFactor(DateTime dateTime_1, double factor, double factorForSplit) : this()
    {
        this.dateTime = dateTime_1;
        this.factor = factor;
        this.factorForSplit = factorForSplit;
    }

    ///WYJ fix, original name: method_0
    public DateTime DateTime()
    {
        return this.dateTime;
    }

    ///WYJ fix, original name: method_1
    public void SetDateTime(DateTime dateTime_1)
    {
        this.dateTime = dateTime_1;
    }

    ///WYJ fix, original name: method_2
    public double FactorForSnD()
    {
        return this.factor;
    }

    ///WYJ fix, original name: method_3
    public void SetFactorForSnD(double double_2)
    {
        this.factor = double_2;
    }

    ///WYJ fix, original name: method_4
    public double FactorForSplit()
    {
        return this.factorForSplit;
    }

    public void SetFactorForSplit(double double_2)
    {
        this.factorForSplit = double_2;
    }
}

