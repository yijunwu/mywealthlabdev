namespace Steema.TeeChart.Styles
{
    using Steema.TeeChart;
    using System;
    using System.Drawing;

    [ToolboxBitmap(typeof(Box), "SeriesIcons.Box.bmp")]
    public class Box : CustomBox
    {
        public Box() : this(null)
        {
        }

        public Box(Chart c) : base(c)
        {
        }

        public override double MaxXValue()
        {
            return base.dPosition;
        }

        public override double MaxYValue()
        {
            double num = base.MaxYValue();
            if (base.UseCustomValues)
            {
                num = Math.Max(num, base.OuterFence3);
            }
            return num;
        }

        public override double MinXValue()
        {
            return base.dPosition;
        }

        public override double MinYValue()
        {
            double num = base.MinYValue();
            if (base.UseCustomValues)
            {
                num = Math.Min(num, base.OuterFence1);
            }
            return num;
        }

        public override string Description
        {
            get
            {
                return Texts.GalleryBoxPlot;
            }
        }
    }
}

