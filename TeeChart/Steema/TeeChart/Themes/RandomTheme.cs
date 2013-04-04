namespace Steema.TeeChart.Themes
{
    using Steema.TeeChart;
    using System;
    using System.Drawing;

    public class RandomTheme : TeeChartTheme
    {
        private static Color randomColor = Utils.EmptyColor;

        public RandomTheme(Chart c) : base(c)
        {
        }

        public override string ToString()
        {
            return Texts.RandomTheme;
        }

        public static Color RandomColor
        {
            get
            {
                Color emptyColor = Utils.EmptyColor;
                do
                {
                    Random random = new Random();
                    int r = random.Next(1, 0xff);
                    int g = random.Next(1, 0xff);
                    int b = random.Next(1, 0xff);
                    emptyColor = Utils.FromArgb(r, g, b);
                }
                while (emptyColor.Equals(randomColor));
                randomColor = emptyColor;
                return randomColor;
            }
        }
    }
}

