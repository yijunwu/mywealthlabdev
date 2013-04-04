namespace Steema.TeeChart.Drawing
{
    using System;
    using System.Collections;
    using System.Drawing;
    using System.Reflection;

    public class GraphicsImages : ArrayList
    {
        public int Add(Image image)
        {
            GraphicsImage image2 = new GraphicsImage(this, image);
            return base.Add(image2);
        }

        public int FindImage(Image image)
        {
            for (int i = 0; i < this.Count; i++)
            {
                if (this[i].Equals(image))
                {
                    return i;
                }
            }
            return this.Add(image);
        }

        public GraphicsImage this[int index]
        {
            get
            {
                return (GraphicsImage) base[index];
            }
        }
    }
}

