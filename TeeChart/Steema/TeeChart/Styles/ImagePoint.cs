namespace Steema.TeeChart.Styles
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using System;
    using System.Drawing;
    using System.IO;
    using System.Reflection;

    [ToolboxBitmap(typeof(ImagePoint), "SeriesIcons.ImagePoint.bmp")]
    public class ImagePoint : CustomImagePoint
    {
        public ImagePoint() : this(null)
        {
        }

        public ImagePoint(Chart c) : base(c)
        {
            string name = "Steema.TeeChart.Images.ImagePoint.TeeDefaultImage.bmp";
            Stream manifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(name);
            if (manifestResourceStream != null)
            {
                base.PointImage = Image.FromStream(manifestResourceStream);
            }
            base.Transparent = true;
        }

        protected override void DrawLegendShape(Graphics3D g, int valueIndex, Rectangle rect)
        {
            if (base.PointImage == null)
            {
                base.DrawLegendShape(g, valueIndex, rect);
            }
            else
            {
                g.Draw(rect, base.PointImage, base.Transparent);
            }
        }

        public override string Description
        {
            get
            {
                return Texts.ImagePointSeries;
            }
        }
    }
}

