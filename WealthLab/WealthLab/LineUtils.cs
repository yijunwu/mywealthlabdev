namespace WealthLab
{
    using System;

    public static class LineUtils
    {
        public static double Distance(double double_0, double double_1, double double_2, double double_3)
        {
            double num = double_2 - double_0;
            double num2 = double_3 - double_1;
            return Math.Sqrt((num * num) + (num2 * num2));
        }

        public static int Distance(int int_0, int int_1, int int_2, int int_3)
        {
            return (int) Distance((double) int_0, (double) int_1, (double) int_2, (double) int_3);
        }

        public static double SolveForX(double double_0, double double_1, double double_2, double double_3, double double_4)
        {
            double num;
            if (double_2 != double_0)
            {
                num = (double_3 - double_1) / (double_2 - double_0);
            }
            else
            {
                num = 0.0;
            }
            if (num == 0.0)
            {
                return double_0;
            }
            double num2 = ((num * double_0) - double_1) * -1.0;
            return ((double_4 - num2) / num);
        }

        public static int SolveForX(int int_0, int int_1, int int_2, int int_3, int int_4)
        {
            return (int) Math.Round(SolveForX((double) int_0, (double) int_1, (double) int_2, (double) int_3, (double) int_4));
        }

        public static double SolveForY(double double_0, double double_1, double double_2, double double_3, double double_4)
        {
            double num;
            if (double_2 != double_0)
            {
                num = (double_3 - double_1) / (double_2 - double_0);
            }
            else
            {
                num = 0.0;
            }
            double num2 = ((num * double_0) - double_1) * -1.0;
            return ((num * double_4) + num2);
        }

        public static int SolveForY(int int_0, int int_1, int int_2, int int_3, int int_4)
        {
            return (int) Math.Round(SolveForY((double) int_0, (double) int_1, (double) int_2, (double) int_3, (double) int_4));
        }
    }
}

