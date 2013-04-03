using System;

internal class Class34
{
    private double double_0;
    private double double_1;
    private double double_2;
    private double double_3;
    private double double_4;
    private double double_5;
    private int int_0;

    public double method_0()
    {
        return this.double_0;
    }

    public void method_1(double double_6, double double_7)
    {
        this.double_2 += double_6;
        this.double_3 += double_7;
        this.double_1 += double_6 * double_7;
        this.double_4 += double_6 * double_6;
        this.double_5 += double_7 * double_7;
        this.int_0++;
    }

    public void method_2()
    {
        if (this.int_0 == 0)
        {
            this.double_0 = 0.0;
        }
        else
        {
            double num = this.double_1 - ((this.double_2 * this.double_3) / ((double) this.int_0));
            double num2 = this.double_4 - ((this.double_2 * this.double_2) / ((double) this.int_0));
            double num3 = this.double_5 - ((this.double_3 * this.double_3) / ((double) this.int_0));
            double d = num2 * num3;
            d = Math.Sqrt(d);
            if (d != 0.0)
            {
                this.double_0 = num / d;
            }
            else
            {
                this.double_0 = 0.0;
            }
        }
    }

    public void method_3()
    {
        this.double_2 = 0.0;
        this.double_3 = 0.0;
        this.double_1 = 0.0;
        this.int_0 = 0;
        this.double_4 = 0.0;
        this.double_5 = 0.0;
    }
}

