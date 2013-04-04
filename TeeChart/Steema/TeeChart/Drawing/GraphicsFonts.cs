namespace Steema.TeeChart.Drawing
{
    using System;
    using System.Collections;

    public abstract class GraphicsFonts : ArrayList
    {
        protected GraphicsFonts()
        {
        }

        public abstract int Add(ChartFont font);
        public int FindFont(ChartFont font)
        {
            for (int i = 0; i < this.Count; i++)
            {
                if ((this[i] as GraphicsFont).Equals(font))
                {
                    return i;
                }
            }
            return this.Add(font);
        }
    }
}

