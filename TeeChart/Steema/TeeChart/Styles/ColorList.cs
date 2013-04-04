namespace Steema.TeeChart.Styles
{
    using Steema.TeeChart;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Reflection;

    [Serializable]
    public sealed class ColorList : List<Color>
    {
        public ColorList()
        {
        }

        public ColorList(int capacity) : base(capacity)
        {
        }

        public ColorList(Color[] colors) : base(colors)
        {
        }

        internal void Exchange(int a, int b)
        {
            Color color = this[a];
            this[a] = this[b];
            this[b] = color;
        }

        internal void InsertColor(int index, Color color)
        {
            while (base.Count < index)
            {
                base.Add(Utils.EmptyColor);
            }
            base.Insert(index, color);
        }

        public Color this[int index]
        {
            get
            {
                if (index >= base.Count)
                {
                    return Utils.EmptyColor;
                }
                return base[index];
            }
            set
            {
                while (base.Count <= index)
                {
                    base.Add(Utils.EmptyColor);
                }
                base[index] = value;
            }
        }
    }
}

