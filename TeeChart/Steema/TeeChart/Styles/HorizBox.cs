namespace Steema.TeeChart.Styles
{
    using Steema.TeeChart;
    using System;
    using System.Drawing;

    [ToolboxBitmap(typeof(HorizBox), "SeriesIcons.HorizBox.bmp")]
    public class HorizBox : CustomBox
    {
        public HorizBox() : this(null)
        {
        }

        public HorizBox(Chart c) : base(c)
        {
            base.SetHorizontal();
            base.IVertical = false;
        }

        public override double MaxXValue()
        {
            double num = base.MaxXValue();
            if (base.UseCustomValues)
            {
                num = Math.Max(num, base.OuterFence3);
            }
            return num;
        }

        public override double MaxYValue()
        {
            return base.dPosition;
        }

        public override double MinXValue()
        {
            double num = base.MinXValue();
            if (base.UseCustomValues)
            {
                num = Math.Min(num, base.OuterFence1);
            }
            return num;
        }

        public override double MinYValue()
        {
            return base.dPosition;
        }

        public override string Description
        {
            get
            {
                return Texts.GalleryHorizBoxPlot;
            }
        }
    }
}

