namespace WealthLab
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;

    public class PlottedIndicator
    {
        private bool bool_0;
        private bool bool_1;
        private System.Drawing.Color color_0;
        private System.Drawing.Color[] color_1;
        private DataSeries dataSeries_0;
        private int int_0;
        private LineStyle lineStyle_0;
        private string string_0 = "";

        public PlottedIndicator(ChartRenderer renderer, DataSeries series)
        {
            this.dataSeries_0 = series;
            string description = series.Description;
            int num = 1;
            while (true)
            {
                bool flag = true;
                foreach (ChartPane pane in renderer.Panes)
                {
                    using (IEnumerator<PlottedIndicator> enumerator2 = pane.PlottedIndicators.GetEnumerator())
                    {
                        while (enumerator2.MoveNext())
                        {
                            PlottedIndicator current = enumerator2.Current;
                            if ((current.Series != this.Series) && (current.Series.Description == series.Description))
                            {
                                ///goto  Label_0080; ///WYJ fix, simplify the flow
                                series.Description = description + "_" + num;
                                num++;
                                flag = false;
                                break;
                            }
                        }
                    }
                    if (!flag)
                    {
                        break;
                    }
                }
                if (flag)
                {
                    return;
                }
            }
        }

        public System.Drawing.Color GetBarColor(int int_1)
        {
            if (this.color_1 == null)
            {
                return this.Color;
            }
            if (int_1 >= this.color_1.Length)
            {
                return this.Color;
            }
            return this.color_1[int_1];
        }

        public void SetBarColor(int int_1, System.Drawing.Color color)
        {
            if (this.color_1 == null)
            {
                this.color_1 = new System.Drawing.Color[this.dataSeries_0.Count];
                for (int i = 0; i < this.dataSeries_0.Count; i++)
                {
                    this.color_1[i] = this.Color;
                }
            }
            this.color_1[int_1] = color;
        }

        public System.Drawing.Color Color
        {
            get
            {
                return this.color_0;
            }
            set
            {
                this.color_0 = value;
            }
        }

        public bool DragAndDrop
        {
            get
            {
                return this.bool_0;
            }
            set
            {
                this.bool_0 = value;
            }
        }

        public string FundamentalItemName
        {
            get
            {
                return this.string_0;
            }
            set
            {
                this.string_0 = value;
            }
        }

        public bool IsFundamental
        {
            get
            {
                return (this.string_0 != "");
            }
        }

        public bool Selected
        {
            get
            {
                return this.bool_1;
            }
            set
            {
                this.bool_1 = value;
            }
        }

        public DataSeries Series
        {
            get
            {
                return this.dataSeries_0;
            }
        }

        public LineStyle Style
        {
            get
            {
                return this.lineStyle_0;
            }
            set
            {
                this.lineStyle_0 = value;
            }
        }

        public int Width
        {
            get
            {
                return this.int_0;
            }
            set
            {
                this.int_0 = value;
            }
        }
    }
}

