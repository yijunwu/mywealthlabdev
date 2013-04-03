using System;
using System.Globalization;

internal class Class3
{
    public static string smethod_0()
    {
        string str = "|";
        NumberFormatInfo numberFormat = CultureInfo.CurrentCulture.NumberFormat;
        return (Convert.ToString((double) 61.8, (IFormatProvider) numberFormat) + str + Convert.ToString((double) 50.0, (IFormatProvider) numberFormat) + str + Convert.ToString((double) 38.2, (IFormatProvider) numberFormat));
    }

    public static float smethod_1(string string_0)
    {
        try
        {
            return Convert.ToSingle(string_0, CultureInfo.CurrentCulture.NumberFormat);
        }
        catch (Exception)
        {
            return Convert.ToSingle(38.2, CultureInfo.CurrentCulture.NumberFormat);
        }
    }
}

