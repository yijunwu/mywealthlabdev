namespace Steema.TeeChart.Styles
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Runtime.InteropServices;

    [ToolboxBitmap(typeof(ColorGrid), "SeriesIcons.ColorGrid.bmp")]
    public class ColorGrid : Custom3DGrid
    {
        private System.Drawing.Bitmap bitmap;
        private ChartBrush cBrush;
        private bool centered;
        private int tmpDec;
        private int xStep;
        private int zStep;

        public ColorGrid() : this(null)
        {
        }

        public ColorGrid(Chart c) : base(c)
        {
            this.xStep = 1;
            this.zStep = 1;
            base.Marks.Callout.Length = 0;
            base.calcVisiblePoints = true;
        }

        private Rectangle CalcDestRectangle(Rectangle tmpBounds)
        {
            int left = base.GetHorizAxis.CalcPosValue(Math.Max(this.MinXValue(), this.CalcMinValue((double) tmpBounds.Left)));
            int right = base.GetHorizAxis.CalcPosValue(Math.Min(this.MaxXValue(), this.CalcMaxValue((double) tmpBounds.Right))) + 1;
            int top = base.GetVertAxis.CalcPosValue(Math.Max(this.MinZValue(), this.CalcMinValue((double) tmpBounds.Top)));
            int bottom = base.GetVertAxis.CalcPosValue(Math.Min(this.MaxZValue(), this.CalcMaxValue((double) tmpBounds.Bottom))) + 1;
            top--;
            return Utils.FromLTRB(left, top, right, bottom);
        }

        internal override void CalcFirstLastVisibleIndex()
        {
            base.firstVisible = -1;
            base.lastVisible = -1;
            if (base.Count > 0)
            {
                base.firstVisible = this.Clicked(base.GetHorizAxis.IStartPos + 1, base.GetVertAxis.IEndPos - 1);
                base.lastVisible = this.Clicked(base.GetHorizAxis.IEndPos - 1, base.GetVertAxis.IStartPos + 1);
            }
            if ((base.firstVisible == -1) || (base.lastVisible == -1))
            {
                base.CalcFirstLastVisibleIndex();
            }
        }

        private double CalcMaxValue(double Value)
        {
            if (this.CenteredPoints)
            {
                double num = Value + 0.5;
                if (base.IrregularGrid)
                {
                    num--;
                }
                return num;
            }
            if (base.IrregularGrid)
            {
                return Value;
            }
            return (Value + 1.0);
        }

        private double CalcMinValue(double Value)
        {
            if (!this.CenteredPoints)
            {
                return Value;
            }
            return (Value - 0.5);
        }

        private Rectangle CellBounds(int x, int z)
        {
            int num2;
            int num4;
            int num5;
            int num6 = base[x, z];
            int left = base.GetHorizAxis.CalcPosValue(this.CalcMinValue(base.XValues[num6]));
            if (x < base.NumXValues)
            {
                num6 = base[x + 1, z];
                num2 = base.GetHorizAxis.CalcPosValue(this.CalcMinValue(base.XValues[num6]));
            }
            else
            {
                if (x > 1)
                {
                    num6 = base[x - 1, z];
                    num5 = left - base.GetHorizAxis.CalcPosValue(this.CalcMinValue(base.XValues[num6]));
                }
                else
                {
                    num5 = 1;
                }
                num2 = left + num5;
            }
            num6 = base[x, z];
            int top = base.GetVertAxis.CalcPosValue(this.CalcMinValue(base.ZValues[num6]));
            if (z < base.NumZValues)
            {
                num6 = base[x, z + 1];
                num4 = base.GetVertAxis.CalcPosValue(this.CalcMinValue(base.ZValues[num6]));
            }
            else
            {
                if (z > 1)
                {
                    num6 = base[x, z - 1];
                    num5 = top - base.GetVertAxis.CalcPosValue(this.CalcMinValue(base.ZValues[num6]));
                }
                else
                {
                    num5 = 1;
                }
                num4 = top + num5;
            }
            return Utils.FromLTRB(left, top, num2, num4);
        }

        public override int Clicked(int x, int y)
        {
            int num2;
            int num = base.Clicked(x, y);
            double num3 = base.GetHorizAxis.CalcPosPoint(x);
            double num4 = base.GetVertAxis.CalcPosPoint(y);
            int num5 = -1;
            double num8 = this.CenteredPoints ? 0.5 : 0.0;
            if (!base.IrregularGrid)
            {
                double d = (num4 + num8) - base.ZValues.Minimum;
                num2 = (d >= 0.0) ? ((int) Math.Truncate(d)) : -1;
                if ((num2 >= 0) && (num2 <= base.NumZValues))
                {
                    num5 = num2;
                }
                if (num5 != -1)
                {
                    double num6 = (num3 + num8) - base.XValues.Minimum;
                    num2 = (num6 >= 0.0) ? ((int) Math.Truncate(num6)) : -1;
                    if ((num2 >= 0) && (num2 <= base.NumXValues))
                    {
                        num = base[num2 + 1, num5 + 1];
                    }
                }
                return num;
            }
            num3 += num8;
            num4 += num8;
            if (base.Count <= 1)
            {
                if (base.Count > 0)
                {
                    num = 0;
                }
                return num;
            }
            for (num2 = 0; num2 < (base.NumZValues - 1); num2++)
            {
                if ((base.ZValues[num2] <= num4) && (base.ZValues[num2 + 1] > num4))
                {
                    num5 = num2;
                    break;
                }
            }
            if (num5 != -1)
            {
                for (num2 = 0; num2 < (base.NumXValues - 1); num2++)
                {
                    if ((base.XValues[num2 * base.NumZValues] <= num3) && (base.XValues[(num2 + 1) * base.NumZValues] > num3))
                    {
                        return (num5 + (num2 * base.NumZValues));
                    }
                }
            }
            return num;
        }

        protected internal override void CreateSubGallery(Series.SubGalleryEventHandler AddSubChart)
        {
            base.CreateSubGallery(AddSubChart);
            AddSubChart(Texts.NoGrid);
        }

        public override void Draw()
        {
            if (base.Count > 0)
            {
                this.tmpDec = (this.centered || !base.IrregularGrid) ? 0 : 1;
                int left = (int) base.GetHorizAxis.CalcPosPoint(base.GetHorizAxis.IStartPos);
                int right = (int) base.GetHorizAxis.CalcPosPoint(base.GetHorizAxis.IEndPos);
                if (base.GetHorizAxis.inverted)
                {
                    left++;
                }
                else
                {
                    right++;
                }
                int top = (int) base.GetVertAxis.CalcPosPoint(base.GetVertAxis.IStartPos);
                int bottom = (int) base.GetVertAxis.CalcPosPoint(base.GetVertAxis.IEndPos);
                if (base.GetVertAxis.inverted)
                {
                    bottom++;
                }
                else
                {
                    top++;
                }
                Rectangle rect = Utils.FromLTRB(left, top, right, bottom);
                base.chart.graphics3D.OrientRectangle(ref rect);
                if (base.IrregularGrid)
                {
                    rect.Height++;
                    rect.Width++;
                    if (this.MinXValue() < 0.0)
                    {
                        rect.X--;
                    }
                    if (this.MinYValue() < 0.0)
                    {
                        rect.Y--;
                    }
                }
                else
                {
                    rect.X -= Utils.Round(this.MinXValue());
                    rect.Y -= Utils.Round(this.MinZValue());
                    rect.Width -= Utils.Round(this.MinXValue());
                    rect.Height -= Utils.Round(this.MinZValue());
                    if (rect.X < 1)
                    {
                        rect.X = 1;
                    }
                    if (rect.Y < 1)
                    {
                        rect.Y = 1;
                    }
                    if (rect.Right > base.NumXValues)
                    {
                        rect.Width = base.NumXValues - rect.Left;
                    }
                    if (rect.Bottom > base.NumZValues)
                    {
                        rect.Height = base.NumZValues - rect.Top;
                    }
                }
                Rectangle r = new Rectangle(0, 0, 0, 0);
                if (base.IrregularGrid)
                {
                    this.DrawCellByCell(ref rect, ref r);
                }
                else
                {
                    this.DrawCellUsingBitmap(ref rect, ref r);
                }
                if (base.Pen.Visible)
                {
                    this.DrawGrid(r);
                }
            }
        }

        private void DrawBitmap(System.Drawing.Bitmap bitmap, ref Rectangle r)
        {
            Graphics3D graphicsd = base.chart.graphics3D;
            graphicsd.PrepareDrawImage();
            if (base.chart.Aspect.View3D)
            {
                graphicsd.Draw(graphicsd.CalcRect3D(r, base.MiddleZ), bitmap, false);
            }
            else
            {
                graphicsd.Draw(r, bitmap, false);
            }
        }

        private void DrawCellByCell(ref Rectangle tmpBounds, ref Rectangle R)
        {
            Rectangle r = new Rectangle(0, 0, 0, 0);
            Graphics3D graphicsd = base.chart.Graphics3D;
            Color color2 = graphicsd.Pen.Color;
            graphicsd.Pen.Color = Utils.EmptyColor;
            for (int i = 1; i < base.NumZValues; i++)
            {
                for (int j = 1; j < base.NumXValues; j++)
                {
                    int valueIndex = base[j, i];
                    if (valueIndex != -1)
                    {
                        Color color = this.ValueColor(valueIndex);
                        if (color != Utils.EmptyColor)
                        {
                            r = this.CellBounds(j, i);
                            this.CellBrush.Color = color;
                            graphicsd.Brush = this.CellBrush;
                            graphicsd.Rectangle(r, base.MiddleZ);
                        }
                    }
                }
            }
            graphicsd.Pen.Color = color2;
            R = this.CalcDestRectangle(tmpBounds);
        }

        private void DrawCellUsingBitmap(ref Rectangle tmpBounds, ref Rectangle R)
        {
            try
            {
                int width = Math.Abs((int) (((1 + tmpBounds.Right) - tmpBounds.Left) - this.tmpDec));
                int height = Math.Abs((int) (((1 + tmpBounds.Bottom) - tmpBounds.Top) - this.tmpDec));
                if ((width != 0) && (height != 0))
                {
                    if (((this.bitmap == null) || (this.bitmap.Width != width)) || (this.bitmap.Height != height))
                    {
                        if (this.bitmap != null)
                        {
                            this.bitmap.Dispose();
                        }
                        this.bitmap = new System.Drawing.Bitmap(width, height);
                    }
                }
                else if (this.bitmap == null)
                {
                    this.bitmap = new System.Drawing.Bitmap(1, 1);
                }
            }
            catch
            {
                throw new TeeChartException(Texts.ColorGridSeriesRangeExceed);
            }
            this.bitmap = this.FillBitmap(tmpBounds, this.bitmap);
            if (this.MinXValue() > 1.0)
            {
                tmpBounds.Width += Utils.Round(this.MinXValue());
                tmpBounds.X += Utils.Round(this.MinXValue());
            }
            if (this.MinZValue() > 1.0)
            {
                tmpBounds.Height += Utils.Round(this.MinZValue());
                tmpBounds.Y += Utils.Round(this.MinZValue());
            }
            R = this.CalcDestRectangle(tmpBounds);
            this.DrawBitmap(this.bitmap, ref R);
        }

        private void DrawGrid(Rectangle r)
        {
            int num;
            int num2;
            Graphics3D graphicsd = base.chart.graphics3D;
            double num3 = this.CenteredPoints ? 0.5 : 0.0;
            graphicsd.Pen = base.Pen;
            for (int i = 1; i <= (((base.NumZValues - this.tmpDec) - 1) / this.ZStep); i++)
            {
                num2 = 1 + (i * this.ZStep);
                num = base.GetVertAxis.CalcPosValue(base.ZValues[base[1, num2]] - num3);
                if (base.chart.aspect.view3D)
                {
                    graphicsd.HorizontalLine(r.X, r.Right, num, base.MiddleZ);
                }
                else
                {
                    graphicsd.HorizontalLine(r.X, r.Right, num);
                }
            }
            for (int j = 1; j <= (((base.NumXValues - this.tmpDec) - 1) / this.XStep); j++)
            {
                num2 = 1 + (j * this.XStep);
                num = base.GetHorizAxis.CalcPosValue(base.XValues[base[num2, 1]] - num3);
                if (base.chart.aspect.View3D)
                {
                    graphicsd.VerticalLine(num, r.Y, r.Bottom, base.MiddleZ);
                }
                else
                {
                    graphicsd.VerticalLine(num, r.Y, r.Bottom);
                }
            }
        }

        protected internal override void DrawMark(int valueIndex, string st, SeriesMarks.Position aPosition)
        {
            double num = (this.centered || base.IrregularGrid) ? 0.0 : 0.5;
            aPosition.LeftTop.Y = base.GetVertAxis.CalcPosValue(base.ZValues[valueIndex] + num) - (aPosition.Height / 2);
            aPosition.LeftTop.X = base.GetHorizAxis.CalcPosValue(base.XValues[valueIndex] + num) - (aPosition.Width / 2);
            base.DrawMark(valueIndex, st, aPosition);
        }

        private System.Drawing.Bitmap FillBitmap(Rectangle tmpBounds, System.Drawing.Bitmap bitmap)
        {
            int width = bitmap.Width;
            int height = bitmap.Height;
            BitmapData bitmapdata = bitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, PixelFormat.Format32bppRgb);
            IntPtr ptr = bitmapdata.Scan0;
            for (int i = tmpBounds.Top; i <= (tmpBounds.Bottom - this.tmpDec); i++)
            {
                for (int j = tmpBounds.Left; j <= (tmpBounds.Right - this.tmpDec); j++)
                {
                    Color white;
                    int valueIndex = base[j, i];
                    if (valueIndex != -1)
                    {
                        white = this.ValueColor(valueIndex);
                        if (white == Utils.EmptyColor)
                        {
                            white = Color.White;
                        }
                    }
                    else
                    {
                        white = Color.White;
                    }
                    int num6 = j - tmpBounds.Left;
                    int num7 = i - tmpBounds.Top;
                    Marshal.WriteInt32(ptr, (num6 + (num7 * width)) * 4, white.ToArgb());
                }
            }
            bitmap.UnlockBits(bitmapdata);
            return bitmap;
        }

        public override void GalleryChanged3D(bool Is3D)
        {
            base.chart.Aspect.View3D = false;
            base.chart.Aspect.ClipPoints = true;
        }

        public override double MaxXValue()
        {
            return this.CalcMaxValue(base.XValues.Maximum);
        }

        public override double MaxYValue()
        {
            return this.MaxZValue();
        }

        public override double MaxZValue()
        {
            return this.CalcMaxValue(base.ZValues.Maximum);
        }

        public override double MinXValue()
        {
            return this.CalcMinValue(base.XValues.Minimum);
        }

        public override double MinYValue()
        {
            return this.MinZValue();
        }

        public override double MinZValue()
        {
            return this.CalcMinValue(base.ZValues.Minimum);
        }

        private void SetBitmap(System.Drawing.Bitmap ABitmap)
        {
            this.bitmap = ABitmap;
            this.Clear();
            base.NumXValues = this.bitmap.Width;
            base.NumZValues = this.bitmap.Height;
            base.BeginUpdate();
            for (int i = 0; i < base.NumXValues; i++)
            {
                for (int j = 0; j < base.NumZValues; j++)
                {
                    Color pixel = this.bitmap.GetPixel(i, j);
                    base.Add((double) i, (double) pixel.ToArgb(), (double) (this.bitmap.Height - (j + 1)), "", pixel);
                }
            }
            base.EndUpdate();
        }

        public override void SetSubGallery(int index)
        {
            if (index == 2)
            {
                base.Pen.Visible = false;
            }
            else
            {
                base.SetSubGallery(index);
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Description("Bitmap property"), Browsable(false)]
        public System.Drawing.Bitmap Bitmap
        {
            get
            {
                return this.bitmap;
            }
            set
            {
                this.SetBitmap(value);
            }
        }

        private ChartBrush CellBrush
        {
            get
            {
                if (this.cBrush == null)
                {
                    this.cBrush = new ChartBrush(base.Chart);
                }
                return this.cBrush;
            }
            set
            {
                this.cBrush = value;
            }
        }

        [DefaultValue(false)]
        public bool CenteredPoints
        {
            get
            {
                return this.centered;
            }
            set
            {
                base.SetBooleanProperty(ref this.centered, value);
            }
        }

        public override string Description
        {
            get
            {
                return Texts.GalleryColorGrid;
            }
        }

        public int XStep
        {
            get
            {
                return this.xStep;
            }
            set
            {
                base.SetIntegerProperty(ref this.xStep, value);
            }
        }

        public int ZStep
        {
            get
            {
                return this.zStep;
            }
            set
            {
                base.SetIntegerProperty(ref this.zStep, value);
            }
        }
    }
}

