namespace Steema.TeeChart.Styles
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using System;
    using System.ComponentModel;
    using System.Drawing;

    [ToolboxBitmap(typeof(Darvas), "SeriesIcons.Darvas.bmp")]
    public class Darvas : OHLC
    {
        public Rectangle[] Boxes;
        public int NumBoxes;

        public Darvas() : this(null)
        {
        }

        public Darvas(Chart c) : base(c)
        {
            base.Transparency = 70;
        }

        private void CalculateBoxes()
        {
            int num4;
            this.NumBoxes = 0;
            this.Boxes = (Rectangle[]) Utils.SetLength(this.Boxes, base.Count, typeof(Rectangle));
            int num8 = 0;
            int index = 0;
            int num5 = -1;
            int num6 = -1;
            double minimum = base.HighValues.Minimum;
            double maximum = base.LowValues.Maximum;
            int num3 = 0;
        Label_0053:
            switch (num8)
            {
                case 0:
                    num3 = index;
                    num8 = 1;
                    goto Label_025B;

                case 1:
                    if (index >= base.Count)
                    {
                        num8 = 5;
                    }
                    else
                    {
                        minimum = this.GetHighValue(index);
                        num5 = index;
                        num6 = index;
                        index++;
                        if (minimum <= this.GetHighValue(index))
                        {
                            num8 = 1;
                        }
                        else
                        {
                            num8 = 2;
                        }
                    }
                    goto Label_025B;

                case 2:
                    if (index >= base.Count)
                    {
                        num8 = 5;
                    }
                    else
                    {
                        index++;
                        if (minimum <= this.GetHighValue(index))
                        {
                            num8 = 1;
                        }
                        else
                        {
                            num8 = 3;
                        }
                    }
                    goto Label_025B;

                case 3:
                    if (index >= base.Count)
                    {
                        num8 = 5;
                    }
                    else
                    {
                        maximum = this.GetLowValue(index);
                        num6 = index;
                        index++;
                        if (minimum <= this.GetHighValue(index))
                        {
                            num8 = 1;
                        }
                        else if (maximum >= this.GetLowValue(index))
                        {
                            num8 = 3;
                        }
                        else
                        {
                            num8 = 4;
                        }
                    }
                    goto Label_025B;

                case 4:
                    if (index >= base.Count)
                    {
                        num8 = 5;
                    }
                    else
                    {
                        index++;
                        if (minimum <= this.GetHighValue(index))
                        {
                            num8 = 1;
                        }
                        else if (maximum >= this.GetLowValue(index))
                        {
                            num8 = 3;
                        }
                        else
                        {
                            num8 = 5;
                        }
                    }
                    goto Label_025B;

                case 5:
                    if (index >= base.Count)
                    {
                        num8 = 6;
                    }
                    else
                    {
                        index++;
                        if ((minimum >= this.GetHighValue(index)) && (maximum <= this.GetLowValue(index)))
                        {
                            num8 = 5;
                        }
                        else
                        {
                            num8 = 6;
                        }
                    }
                    goto Label_025B;

                case 6:
                    if (index >= base.Count)
                    {
                        num4 = index - 1;
                        break;
                    }
                    num4 = index;
                    break;

                default:
                    goto Label_025B;
            }
            this.Boxes[this.NumBoxes] = Utils.FromLTRB(this.CalcXPos(num3), base.GetVertAxis.CalcYPosValue(this.GetHighValue(num5)), this.CalcXPos(num4), base.GetVertAxis.CalcYPosValue(this.GetLowValue(num6)));
            this.NumBoxes++;
            if (index < base.Count)
            {
                num8 = 0;
            }
            else
            {
                num8 = 7;
            }
        Label_025B:
            if (num8 < 7)
            {
                goto Label_0053;
            }
        }

        public override int Clicked(int x, int y)
        {
            int num = -1;
            if (this.Boxes != null)
            {
                if (base.chart != null)
                {
                    base.chart.Graphics3D.Calculate2DPosition(ref x, ref y, base.startZ);
                }
                for (int i = 0; i < this.Boxes.Length; i++)
                {
                    if (this.Boxes[i].Contains(x, y))
                    {
                        num = i;
                    }
                }
            }
            return num;
        }

        public override void Draw()
        {
            this.CalculateBoxes();
            Graphics3D graphicsd = base.chart.Graphics3D;
            graphicsd.Pen = base.LinePen.Clone() as ChartPen;
            graphicsd.Brush = base.Brush.Clone() as ChartBrush;
            for (int i = 0; i < this.NumBoxes; i++)
            {
                if (base.ColorEach)
                {
                    graphicsd.Brush.Color = Graphics3D.GetDefaultColor(i);
                    graphicsd.Brush.Transparency = base.Brush.Transparency;
                }
                if (base.chart.aspect.view3D)
                {
                    graphicsd.Rectangle(this.Boxes[i], base.StartZ);
                }
                else
                {
                    graphicsd.Rectangle(this.Boxes[i]);
                }
            }
        }

        private double GetHighValue(int index)
        {
            if (index < base.HighValues.Count)
            {
                return base.HighValues[index];
            }
            return 0.0;
        }

        private double GetLowValue(int index)
        {
            if (index < base.LowValues.Count)
            {
                return base.LowValues[index];
            }
            return 0.0;
        }

        [Description("Gets descriptive text.")]
        public override string Description
        {
            get
            {
                return Texts.GalleryDarvas;
            }
        }
    }
}

