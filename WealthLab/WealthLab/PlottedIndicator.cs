namespace WealthLab
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;

    public class PlottedIndicator
    {
        private bool dragAndDrop;
        private bool selected;
        private System.Drawing.Color color;
        private System.Drawing.Color[] barColors;
        private DataSeries dataSeries;
        private int width;
        private LineStyle lineStyle;
        private string fundamentalItemName = "";

        public PlottedIndicator(ChartRenderer renderer, DataSeries series)
        {
            this.dataSeries = series;
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
            if (this.barColors == null)
            {
                return this.Color;
            }
            if (int_1 >= this.barColors.Length)
            {
                return this.Color;
            }
            return this.barColors[int_1];
        }

        public void SetBarColor(int int_1, System.Drawing.Color color)
        {
            if (this.barColors == null)
            {
                this.barColors = new System.Drawing.Color[this.dataSeries.Count];
                for (int i = 0; i < this.dataSeries.Count; i++)
                {
                    this.barColors[i] = this.Color;
                }
            }
            this.barColors[int_1] = color;
        }

        public System.Drawing.Color Color
        {
            get
            {
                return this.color;
            }
            set
            {
                this.color = value;
            }
        }

        public bool DragAndDrop
        {
            get
            {
                return this.dragAndDrop;
            }
            set
            {
                this.dragAndDrop = value;
            }
        }

        public string FundamentalItemName
        {
            get
            {
                return this.fundamentalItemName;
            }
            set
            {
                this.fundamentalItemName = value;
            }
        }

        public bool IsFundamental
        {
            get
            {
                return (this.fundamentalItemName != "");
            }
        }

        public bool Selected
        {
            get
            {
                return this.selected;
            }
            set
            {
                this.selected = value;
            }
        }

        public DataSeries Series
        {
            get
            {
                return this.dataSeries;
            }
        }

        public LineStyle Style
        {
            get
            {
                return this.lineStyle;
            }
            set
            {
                this.lineStyle = value;
            }
        }

        public int Width
        {
            get
            {
                return this.width;
            }
            set
            {
                this.width = value;
            }
        }
    }
}

