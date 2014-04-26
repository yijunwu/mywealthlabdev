using System;
using System.Drawing;

internal class Class33   ///WYJ note, TradePriceMarker
{
    private Brush brush;   ///WYJ fix, original name: brush_0
    private double price;   ///WYJ fix, original name: double_0
    private int barNum;   ///WYJ fix, original name: int_0

    ///WYJ fix, original signature: public Class33(int int_1, double double_1, Brush brush_1)
    public Class33(int barNum, double price, Brush brush)
    {
        this.barNum = barNum;
        this.price = price;
        this.brush = brush;
    }

    ///WYJ fix, original signature: public int method_0()
    public int getBarNum()
    {
        return this.barNum;
    }

    ///WYJ fir, original signature: public double method_1()
    public double getPrice()
    {
        return this.price;
    }

    ///WYJ fir, original signature: public Brush method_2()
    public Brush getBrush()
    {
        return this.brush;
    }
}

