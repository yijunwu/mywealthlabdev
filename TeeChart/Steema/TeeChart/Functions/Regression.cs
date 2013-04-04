namespace Steema.TeeChart.Functions
{
    using System;
    using System.Runtime.InteropServices;

    public class Regression
    {
        public static void LinearRegression(int numpoints, double[] x, double[] y, double[] weights, out double[] coeffs)
        {
            coeffs = new double[2];
            double num = 0.0;
            double num2 = 0.0;
            double num3 = 0.0;
            double num4 = 0.0;
            double num5 = 0.0;
            if (weights != null)
            {
                for (int i = 0; i < numpoints; i++)
                {
                    num += weights[i];
                    num2 += x[i] * weights[i];
                    num3 += (x[i] * x[i]) * weights[i];
                    num4 += y[i] * weights[i];
                    num5 += (x[i] * y[i]) * weights[i];
                }
            }
            else
            {
                num = numpoints;
                for (int j = 0; j < numpoints; j++)
                {
                    num2 += x[j];
                    num4 += y[j];
                    num3 += x[j] * x[j];
                    num5 += x[j] * y[j];
                }
            }
            double num8 = (num * num3) - (num2 * num2);
            double num9 = num3 / num8;
            double num10 = -num2 / num8;
            double num11 = num / num8;
            coeffs[0] = (num9 * num4) + (num10 * num5);
            coeffs[1] = (num10 * num4) + (num11 * num5);
        }
    }
}

