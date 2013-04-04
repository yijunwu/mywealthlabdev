namespace Steema.TeeChart.Styles
{
    using Steema.TeeChart.Drawing;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Reflection;

    public class MarksItems : List<MarksItem>
    {
        internal bool ILoadingCustom;
        internal TextShape IMarks;

        public MarksItems(SeriesMarks s)
        {
            this.IMarks = s;
        }

        public void Clear()
        {
            base.Clear();
            this.IMarks.Invalidate();
        }

        public MarksItem this[int index]
        {
            get
            {
                while (index > (base.Count - 1))
                {
                    base.Add(null);
                }
                if (base[index] == null)
                {
                    MarksItem item = new MarksItem(this.IMarks.Chart) {
                        Color = MarksItem.ChartMarkColor
                    };
                    ((SeriesMarks) this.IMarks).Shadow.Width = 1;
                    ((SeriesMarks) this.IMarks).Shadow.Height = 1;
                    ((SeriesMarks) this.IMarks).Shadow.Color = Color.Gray;
                    base[index] = item;
                }
                return base[index];
            }
        }
    }
}

