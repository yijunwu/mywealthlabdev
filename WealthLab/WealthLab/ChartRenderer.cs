namespace WealthLab
{
    using Fidelity.Components;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Text;
    using WealthLab.Properties;
    using WealthLab.WSDrawingObjects;

    [ToolboxBitmap(typeof(ChartRenderer), "ChartRenderer")]
    public class ChartRenderer : Component
    {
        private WealthLab.Bars bars_0;
        private BarScale barScale_0;
        private bool bool_0;
        private bool bool_1;
        private bool bool_10;
        private bool bool_11;
        private bool bool_12;
        private bool bool_13;
        private bool bool_14;
        private bool bool_2;
        private bool bool_3;
        private bool bool_4;
        private bool bool_5;
        private bool bool_6;
        private bool bool_7;
        private bool bool_8;
        private bool bool_9;
        private Brush brush_0;
        private Brush brush_1;
        private byte byte_0;
        private ChartPane chartPane_0;
        private ChartPane chartPane_1;
        private ChartPane chartPane_2;
        private ChartPane chartPane_3;
        private WealthLab.ChartStyle chartStyle_0;
        private Color color_0;
        private Color color_1;
        private Color[] color_10;
        private Color[] color_11;
        private Color color_12;
        private Color color_13;
        private Color color_2;
        private Color color_3;
        private Color color_4;
        private Color color_5;
        private Color color_6;
        private Color color_7;
        private Color color_8;
        private Color[] color_9;
        private Dictionary<int, int> dictionary_0;
        private Dictionary<int, int> dictionary_1;
        private Dictionary<string, int> dictionary_2;
        private Dictionary<string, int> dictionary_3;
        private Font font_0;
        private Font font_1;
        private FundamentalsLoader fundamentalsLoader_0;
        private GridLines gridLines_0;
        private IContainer icontainer_0;
        private int[] int_0;
        private int[] int_1;
        private int int_10;
        private int int_11;
        private int int_2;
        private int int_3;
        private int int_4;
        private int int_5;
        private int int_6;
        private int int_7;
        private int int_8;
        private int int_9;
        private List<ChartPane> list_0;
        private List<ChartGlyph> list_1;
        private List<Class33> list_2;
        private List<ChartPane> list_3;
        private List<Rectangle> list_4;
        private string string_0;
        private string string_1;
        private string string_2;
        private string string_3;
        private TradingSystemExecutor tradingSystemExecutor_0;
        private WealthLab.WealthScript wealthScript_0;

        public ChartRenderer()
        {
            this.int_0 = new int[0];
            this.int_1 = new int[0];
            this.int_2 = 4;
            this.color_0 = Color.White;
            this.color_1 = Color.Navy;
            this.color_2 = Color.Red;
            this.color_3 = Color.Teal;
            this.color_4 = Color.Teal;
            this.color_5 = Color.Gainsboro;
            this.color_6 = Color.Navy;
            this.color_7 = Color.Gainsboro;
            this.color_8 = Color.Black;
            this.list_0 = new List<ChartPane>();
            this.bool_0 = true;
            this.int_6 = 40;
            this.int_7 = 20;
            this.font_0 = new Font("Vrinda", 8f);
            this.font_1 = new Font("Verdana", 6f);
            this.bool_1 = true;
            this.bool_2 = true;
            this.bool_3 = true;
            this.bool_4 = true;
            this.bool_5 = true;
            this.bool_6 = true;
            this.list_1 = new List<ChartGlyph>();
            this.string_0 = "";
            this.bool_9 = true;
            this.dictionary_0 = new Dictionary<int, int>();
            this.dictionary_1 = new Dictionary<int, int>();
            this.list_2 = new List<Class33>();
            this.list_3 = new List<ChartPane>();
            this.string_1 = "";
            this.barScale_0 = BarScale.Yearly;
            this.dictionary_2 = new Dictionary<string, int>();
            this.dictionary_3 = new Dictionary<string, int>();
            this.bool_14 = true;
            this.gridLines_0 = new GridLines();
            this.list_4 = new List<Rectangle>();
            this.method_0();
        }

        public ChartRenderer(IContainer container)
        {
            this.int_0 = new int[0];
            this.int_1 = new int[0];
            this.int_2 = 4;
            this.color_0 = Color.White;
            this.color_1 = Color.Navy;
            this.color_2 = Color.Red;
            this.color_3 = Color.Teal;
            this.color_4 = Color.Teal;
            this.color_5 = Color.Gainsboro;
            this.color_6 = Color.Navy;
            this.color_7 = Color.Gainsboro;
            this.color_8 = Color.Black;
            this.list_0 = new List<ChartPane>();
            this.bool_0 = true;
            this.int_6 = 40;
            this.int_7 = 20;
            this.font_0 = new Font("Vrinda", 8f);
            this.font_1 = new Font("Verdana", 6f);
            this.bool_1 = true;
            this.bool_2 = true;
            this.bool_3 = true;
            this.bool_4 = true;
            this.bool_5 = true;
            this.bool_6 = true;
            this.list_1 = new List<ChartGlyph>();
            this.string_0 = "";
            this.bool_9 = true;
            this.dictionary_0 = new Dictionary<int, int>();
            this.dictionary_1 = new Dictionary<int, int>();
            this.list_2 = new List<Class33>();
            this.list_3 = new List<ChartPane>();
            this.string_1 = "";
            this.barScale_0 = BarScale.Yearly;
            this.dictionary_2 = new Dictionary<string, int>();
            this.dictionary_3 = new Dictionary<string, int>();
            this.bool_14 = true;
            this.gridLines_0 = new GridLines();
            this.list_4 = new List<Rectangle>();
            container.Add(this);
            this.method_0();
        }

        public void AdjustBarPositions()
        {
            this.int_1 = new int[this.bars_0.Count];
            this.int_0 = new int[this.bars_0.Count];
            for (int i = 0; i < this.bars_0.Count; i++)
            {
                this.int_0[i] = this.BarSpacing;
                this.int_1[i] = -1;
            }
            this.int_8 = this.method_7();
            this.int_9 = this.method_6();
            int num3 = this.RightPaddingBars - this.int_3;
            if (num3 < 0)
            {
                num3 = 0;
            }
            int num4 = ((this.int_4 - this.MarginRightWidth) - (this.int_0[this.int_8] / 2)) - (num3 * this.BarSpacing);
            for (int j = this.int_8; j >= this.int_9; j--)
            {
                this.int_1[j] = num4 - (this.int_0[j] / 2);
                num4 -= this.int_0[j];
            }
            if (this.int_9 < this.int_1.Length)
            {
                num4 = this.int_1[this.int_9];
                for (int k = this.int_9 - 1; k >= 0; k--)
                {
                    num4 -= this.int_0[k + 1];
                    this.int_1[k] = num4;
                }
            }
            if (this.int_8 < this.int_1.Length)
            {
                num4 = this.int_1[this.int_8];
                for (int m = this.int_8 + 1; m < this.bars_0.Count; m++)
                {
                    this.int_1[m] = num4 + this.int_0[m - 1];
                }
            }
        }

        public void AssignProperties(ChartRenderer chartRenderer_0)
        {
            this.BarSpacing = chartRenderer_0.BarSpacing;
            this.BackgroundColor = chartRenderer_0.BackgroundColor;
            this.UpBarColor = chartRenderer_0.UpBarColor;
            this.DownBarColor = chartRenderer_0.DownBarColor;
            this.UpBarVolumeColor = chartRenderer_0.UpBarVolumeColor;
            this.DownBarVolumeColor = chartRenderer_0.DownBarVolumeColor;
            this.GridlineColor = chartRenderer_0.GridlineColor;
            this.MarginBottomColor = chartRenderer_0.MarginBottomColor;
            this.MarginRightColor = chartRenderer_0.MarginRightColor;
            this.PaneSeparatorColor = chartRenderer_0.PaneSeparatorColor;
            this.HorizontalGridines = chartRenderer_0.HorizontalGridines;
            this.VerticalGridlines = chartRenderer_0.VerticalGridlines;
            this.PaneSeparatorVisible = chartRenderer_0.PaneSeparatorVisible;
            this.AxisFont = chartRenderer_0.AxisFont;
            this.TitleFont = chartRenderer_0.TitleFont;
            this.FundamentalGlyphs = chartRenderer_0.FundamentalGlyphs;
            this.PlotStops = chartRenderer_0.PlotStops;
            if (((this.VolumePane != null) && (this.VolumePane.PlottedIndicators.Count > 0)) && (this.Bars != null))
            {
                PlottedIndicator indicator = this.VolumePane.PlottedIndicators[0];
                for (int i = 0; i < this.Bars.Count; i++)
                {
                    if (this.Bars.Close[i] > this.Bars.Open[i])
                    {
                        indicator.SetBarColor(i, this.UpBarVolumeColor);
                    }
                    else
                    {
                        indicator.SetBarColor(i, this.DownBarVolumeColor);
                    }
                }
            }
        }

        public void ClearBarsObject()
        {
            this.bars_0 = null;
        }

        public void ClearDragDropIndicators()
        {
            foreach (ChartPane pane in this.Panes)
            {
                for (int i = pane.PlottedIndicators.Count - 1; i >= 0; i--)
                {
                    if (pane.PlottedIndicators[i].DragAndDrop)
                    {
                        pane.PlottedIndicators.RemoveAt(i);
                    }
                }
                if (pane.list_4 != null)
                {
                    pane.list_4.Clear();
                }
            }
        }

        public void ClipToPane(Graphics graphics_0, ChartPane pane)
        {
            if (pane != this.chartPane_2)
            {
                this.chartPane_2 = pane;
                if (pane == null)
                {
                    graphics_0.ResetClip();
                    graphics_0.SetClip(new Rectangle(0, 0, this.int_4, this.int_5));
                }
                else
                {
                    graphics_0.SetClip(new Rectangle(0, pane.Top, this.int_4 - this.int_6, pane.Height));
                }
            }
        }

        public void ClipToPane(Graphics graphics_0, ChartPane pane, bool includeMarginArea)
        {
            this.chartPane_2 = null;
            if (pane == null)
            {
                graphics_0.ResetClip();
                graphics_0.SetClip(new Rectangle(0, 0, this.int_4, this.int_5));
            }
            else
            {
                graphics_0.SetClip(new Rectangle(0, pane.Top, this.int_4, pane.Height));
            }
        }

        public int ConvertBarToX(int int_12)
        {
            return this.int_1[int_12];
        }

        public int ConvertXToBar(int int_12)
        {
            int num4;
            int num5;
            int num6;
            for (int i = this.int_9; i <= this.int_8; i++)
            {
                num4 = this.int_0[i] / 2;
                num5 = this.int_1[i] - num4;
                num6 = (this.int_1[i] + num4) + 1;
                if ((int_12 >= num5) && (int_12 <= num6))
                {
                    return i;
                }
            }
            if (int_12 < this.int_1[this.int_9])
            {
                for (int j = this.int_9; j >= 0; j--)
                {
                    num4 = this.int_0[j] / 2;
                    num5 = this.int_1[j] - num4;
                    num6 = (this.int_1[j] + num4) + 1;
                    if ((int_12 >= num5) && (int_12 <= num6))
                    {
                        return j;
                    }
                }
            }
            if (int_12 > this.int_1[this.int_8])
            {
                for (int k = this.int_8; k < this.Bars.Count; k++)
                {
                    num4 = this.int_0[k] / 2;
                    num5 = this.int_1[k] - num4;
                    num6 = (this.int_1[k] + num4) + 1;
                    if ((int_12 >= num5) && (int_12 <= num6))
                    {
                        return k;
                    }
                }
            }
            return -1;
        }

        public void CreateDefaultPanes()
        {
            this.list_0.Clear();
            this.list_2.Clear();
            this.chartPane_0 = new ChartPane(this, false);
            this.chartPane_0.Decimals = 2;
            this.chartPane_0.IsPricePane = true;
            this.chartPane_0.LogScale = this.LogScale;
            this.chartPane_0.Description = "P";
            if ((this.bool_13 && (this.dictionary_2.Count > 0)) && this.dictionary_2.ContainsKey("P"))
            {
                this.chartPane_0.RawHeight = this.dictionary_2["P"];
            }
            else
            {
                this.chartPane_0.RawHeight = 100;
            }
            this.chartPane_1 = new ChartPane(this, false);
            this.chartPane_1.Visible = this.bool_0;
            this.chartPane_1.Description = "V";
            if ((this.bool_13 && (this.dictionary_2.Count > 0)) && this.dictionary_2.ContainsKey("V"))
            {
                this.chartPane_1.RawHeight = this.dictionary_2["V"];
            }
            else
            {
                this.chartPane_1.RawHeight = 15;
            }
            if (this.method_5(this.chartPane_1.Description))
            {
                this.chartPane_1.Height = this.method_2(this.chartPane_1.Description);
                this.chartPane_1.Hidden = true;
            }
            if ((this.VolumePane.PlottedIndicators.Count == 0) && (this.bars_0 != null))
            {
                this.method_10();
            }
            this.bool_14 = true;
            this.color_9 = null;
            this.color_10 = null;
            this.byte_0 = (byte) (this.byte_0 + 1);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.icontainer_0 != null))
            {
                this.icontainer_0.Dispose();
            }
            base.Dispose(disposing);
        }

        public ChartPane FindPane(string description)
        {
            ChartPane pane2;
            using (IEnumerator<ChartPane> enumerator = this.Panes.GetEnumerator())
            {
                ChartPane current;
                while (enumerator.MoveNext())
                {
                    current = enumerator.Current;
                    if (current.Description == description)
                    {
                        goto Label_002E;
                    }
                }
                return null;
            Label_002E:
                pane2 = current;
            }
            return pane2;
        }

        public PlottedIndicator FindPlottedIndicator(string description)
        {
            foreach (ChartPane pane in this.Panes)
            {
                foreach (PlottedIndicator indicator in pane.PlottedIndicators)
                {
                    if (indicator.Series.Description == description)
                    {
                        return indicator;
                    }
                }
            }
            return null;
        }

        public PlottedIndicator FindPlottedIndicator(DataSeries dataSeries_0)
        {
            foreach (ChartPane pane in this.Panes)
            {
                foreach (PlottedIndicator indicator in pane.PlottedIndicators)
                {
                    if (indicator.Series == dataSeries_0)
                    {
                        return indicator;
                    }
                }
            }
            return null;
        }

        public Color GetBackgroundColor(int int_12)
        {
            if (this.color_9 == null)
            {
                return this.BackgroundColor;
            }
            if (this.color_9[int_12] == Color.Empty)
            {
                return this.BackgroundColor;
            }
            return this.color_9[int_12];
        }

        public int GetBarAnnotationCount(int int_12, bool above)
        {
            if (above)
            {
                if (this.dictionary_0.ContainsKey(int_12))
                {
                    return this.dictionary_0[int_12];
                }
                return 0;
            }
            if (this.dictionary_1.ContainsKey(int_12))
            {
                return this.dictionary_1[int_12];
            }
            return 0;
        }

        public Color GetBarColor(int int_12)
        {
            if (this.color_10 != null)
            {
                return this.color_10[int_12];
            }
            if (this.bars_0.Close[int_12] > this.bars_0.Open[int_12])
            {
                return this.UpBarColor;
            }
            return this.DownBarColor;
        }

        public Color GetBarColor(WealthLab.Bars bars, int int_12)
        {
            if ((this.color_10 != null) && (bars == this.bars_0))
            {
                return this.color_10[int_12];
            }
            if (bars.Close[int_12] > bars.Open[int_12])
            {
                return this.UpBarColor;
            }
            return this.DownBarColor;
        }

        public int GetBarWidth(int int_12)
        {
            return this.int_0[int_12];
        }

        public int GetResizedRawHeight(string description, int defHeight)
        {
            if (this.bool_13 && this.dictionary_2.ContainsKey(description))
            {
                return this.dictionary_2[description];
            }
            return defHeight;
        }

        public void HideDisplayPane(WealthLab.Bars bars, Graphics graphics_0, int width, int height, WealthLab.ChartStyle chartStyle, ChartPane pane)
        {
            int num = 0;
            pane.Hidden = !pane.Hidden;
            if (pane.Hidden)
            {
                num = pane.HiddenHeight - pane.Height;
            }
            else
            {
                num = (pane.HeightBeforeHidden - pane.HiddenHeight) + 10;
            }
            this.method_1(bars, graphics_0, width, height, chartStyle, true, num, pane.Description, true);
        }

        private void method_0()
        {
            this.icontainer_0 = new Container();
        }

        private void method_1(WealthLab.Bars bars_1, Graphics graphics_0, int int_12, int int_13, WealthLab.ChartStyle chartStyle_1, bool bool_15, int int_14, string string_4, bool bool_16)
        {
            if (!this.Executing)
            {
                Pen pen = new Pen(this.GridlineColor);
                Pen pen2 = new Pen(this.BackgroundColor);
                Pen pen3 = new Pen(ReverseColor(this.BackgroundColor));
                this.brush_0 = new SolidBrush(this.BackgroundColor);
                this.brush_1 = new SolidBrush(ReverseColor(this.BackgroundColor));
                this.list_1.Clear();
                this.dictionary_0.Clear();
                this.dictionary_1.Clear();
                if (chartStyle_1 != null)
                {
                    this.string_2 = chartStyle_1.GetType().Name;
                    this.chartStyle_0 = chartStyle_1;
                    this.chartStyle_0.Bars = bars_1;
                    this.chartStyle_0.Renderer = this;
                }
                graphics_0.Clear(this.BackgroundColor);
                if (((this.int_4 != int_12) || (this.int_5 != int_13)) || (this.string_3 != this.string_2))
                {
                    this.int_4 = int_12;
                    this.int_5 = int_13;
                    this.bool_14 = true;
                    this.string_3 = this.string_2;
                }
                if ((this.int_4 > this.MarginRightWidth) && (this.int_5 > this.MarginBottomHeight))
                {
                    if ((bars_1 != null) && (bars_1.Count != 0))
                    {
                        if ((this.bars_0 != bars_1) || (bars_1.Count != this.int_0.Length))
                        {
                            if ((this.bars_0 != bars_1) || (bars_1.Count != this.int_11))
                            {
                                if (((this.string_1 == bars_1.Symbol) && (this.barScale_0 == bars_1.Scale)) && (bars_1.Count == (this.int_11 + 1)))
                                {
                                    this.bool_12 = true;
                                    this.list_3.Clear();
                                    foreach (ChartPane pane15 in this.Panes)
                                    {
                                        if (pane15.Visible)
                                        {
                                            this.list_3.Add(pane15);
                                        }
                                    }
                                }
                                this.ChartStyle.Initialize();
                                this.int_11 = bars_1.Count;
                                this.barScale_0 = bars_1.Scale;
                                this.string_1 = bars_1.Symbol;
                            }
                            this.bool_14 = true;
                            if (this.bars_0 != bars_1)
                            {
                                this.Panes.Clear();
                            }
                        }
                        this.bars_0 = bars_1;
                        this.chartStyle_0 = chartStyle_1;
                        if (this.bool_14)
                        {
                            this.int_1 = new int[bars_1.Count];
                            this.int_0 = new int[bars_1.Count];
                            for (int i = 0; i < bars_1.Count; i++)
                            {
                                this.int_0[i] = this.BarSpacing;
                                this.int_1[i] = -1;
                            }
                            chartStyle_1.InitializeBarWidths();
                            this.int_8 = this.method_7();
                            this.int_9 = this.method_6();
                            int num31 = this.RightPaddingBars - this.int_3;
                            if (num31 < 0)
                            {
                                num31 = 0;
                            }
                            int num18 = ((int_12 - this.MarginRightWidth) - (this.int_0[this.int_8] / 2)) - (num31 * this.BarSpacing);
                            for (int j = this.int_8; j >= this.int_9; j--)
                            {
                                this.int_1[j] = num18 - (this.int_0[j] / 2);
                                num18 -= this.int_0[j];
                            }
                            if (this.int_9 < this.int_1.Length)
                            {
                                num18 = this.int_1[this.int_9];
                                for (int k = this.int_9 - 1; k >= 0; k--)
                                {
                                    num18 -= this.int_0[k + 1];
                                    this.int_1[k] = num18;
                                }
                            }
                            if (this.int_8 < this.int_1.Length)
                            {
                                num18 = this.int_1[this.int_8];
                                for (int m = this.int_8 + 1; m < bars_1.Count; m++)
                                {
                                    this.int_1[m] = num18 + this.int_0[m - 1];
                                }
                            }
                            this.bool_14 = false;
                        }
                        if (this.list_0.Count == 0)
                        {
                            this.CreateDefaultPanes();
                        }
                        double num = 0.0;
                        double num2 = 0.0;
                        foreach (ChartPane pane4 in this.list_0)
                        {
                            if (pane4.Visible)
                            {
                                if (pane4.Hidden)
                                {
                                    num2 += pane4.HiddenHeight;
                                }
                                else
                                {
                                    num += pane4.RawHeight;
                                }
                            }
                        }
                        if (num > 0.0)
                        {
                            int num3;
                            int num4;
                            DateTime time;
                            string str;
                            List<Rectangle>.Enumerator enumerator6;
                            int width;
                            bool flag;
                            double num10 = ((this.int_5 - num2) - this.MarginBottomHeight) / num;
                            num10 = (num10 > 0.0) ? num10 : 0.0;
                            int num34 = 0;
                            for (int n = 0; n < this.list_0.Count; n++)
                            {
                                ChartPane pricePane;
                                ChartPane pane2 = this.list_0[n];
                                if (!pane2.Visible)
                                {
                                    continue;
                                }
                                pane2.LowestValue = pane2.MinValue;
                                pane2.HighestValue = pane2.MaxValue;
                                if (!bool_15 || !(string_4 == pane2.Description))
                                {
                                    goto Label_0637;
                                }
                                int num12 = Math.Max(10, pane2.HiddenHeight);
                                if (bool_16)
                                {
                                    pricePane = this.PricePane;
                                }
                                else
                                {
                                    pricePane = pane2;
                                    int num15 = n;
                                    while (!pane2.AbovePricePane)
                                    {
                                        pricePane = this.Panes[--num15];
                                    Label_0548:
                                        if (pricePane.Hidden || !pricePane.Visible)
                                        {
                                            continue;
                                        }
                                        goto Label_057D;
                                    Label_055C:
                                        pricePane = this.Panes[++num15];
                                        goto Label_0548;
                                    }
                                    goto Label_055C;
                                }
                            Label_057D:
                                pane2.Height += int_14;
                                if (pane2.Height < num12)
                                {
                                    pane2.Height = num12;
                                }
                                int rawHeight = pane2.RawHeight;
                                pane2.RawHeight = (int) (((double) pane2.Height) / num10);
                                int num11 = pane2.RawHeight - rawHeight;
                                pricePane.RawHeight -= num11;
                                if ((pricePane.RawHeight * num10) < num12)
                                {
                                    int num13 = pricePane.RawHeight;
                                    pricePane.RawHeight = (int) (((double) num12) / num10);
                                    int num14 = pricePane.RawHeight - num13;
                                    pane2.RawHeight -= num14;
                                    pane2.Height = (int) (pane2.RawHeight * num10);
                                }
                                goto Label_06DA;
                            Label_0637:
                                if (this.bool_12 && (this.list_3.Count == this.list_0.Count))
                                {
                                    pane2.Height = this.list_3[n].Height;
                                    pane2.RawHeight = this.list_3[n].RawHeight;
                                    if (n == (this.list_0.Count - 1))
                                    {
                                        this.bool_12 = false;
                                        this.list_3.Clear();
                                    }
                                }
                                else if (pane2.Hidden)
                                {
                                    pane2.Height = pane2.HiddenHeight;
                                }
                                else
                                {
                                    pane2.Height = (int) (pane2.RawHeight * num10);
                                }
                            Label_06DA:
                                pane2.Top = num34;
                                num34 += pane2.Height;
                                foreach (PlottedIndicator indicator in pane2.PlottedIndicators)
                                {
                                    pane2.method_1(indicator.Series);
                                }
                                foreach (PlottedSymbol symbol in pane2.PlottedSymbols)
                                {
                                    pane2.method_1(symbol.Bars.High);
                                    pane2.method_1(symbol.Bars.Low);
                                }
                            }
                            if (bool_15)
                            {
                                this.dictionary_2.Clear();
                                foreach (ChartPane pane in this.list_0)
                                {
                                    this.dictionary_2.Add(pane.Description, pane.RawHeight);
                                }
                                this.bool_13 = true;
                            }
                            this.PricePane.method_1(bars_1.High);
                            this.PricePane.method_1(bars_1.Low);
                            if (!double.IsNaN(this.bars_0.High.PartialValue) && (this.int_8 == (bars_1.Count - 1)))
                            {
                                this.PricePane.method_2(bars_1.High.PartialValue);
                                this.PricePane.method_2(bars_1.Low.PartialValue);
                            }
                            if (!double.IsNaN(this.bars_0.Volume.PartialValue) && (this.int_8 == (bars_1.Count - 1)))
                            {
                                this.VolumePane.method_2(bars_1.Volume.PartialValue);
                            }
                            if (this.VolumePane.Visible)
                            {
                                this.VolumePane.method_1(bars_1.Volume);
                            }
                            if (this.VolumePane.LowestValue > 0.0)
                            {
                                this.VolumePane.LowestValue = 0.0;
                            }
                            foreach (ChartPane pane9 in this.Panes)
                            {
                                if (pane9.LowestValue == double.MaxValue)
                                {
                                    pane9.LowestValue = 0.0;
                                    pane9.HighestValue = 1.0;
                                }
                            }
                            foreach (ChartPane pane10 in this.Panes)
                            {
                                pane10.method_9();
                            }
                            if (this.WealthScript != null)
                            {
                                this.WealthScript.PaintHook(bars_1, graphics_0, this.chartStyle_0, PaintHookStage.AfterBackgroundRender);
                            }
                            if (this.color_9 != null)
                            {
                                for (int num35 = this.int_9; num35 <= this.int_8; num35++)
                                {
                                    if (this.color_9[num35] != Color.Empty)
                                    {
                                        num3 = this.int_1[num35] - (this.int_0[num35] / 2);
                                        graphics_0.FillRectangle(new SolidBrush(this.color_9[num35]), num3, 0, this.int_0[num35], int_13);
                                    }
                                }
                            }
                            foreach (ChartPane pane12 in this.Panes)
                            {
                                if (pane12.color_0 != null)
                                {
                                    for (int num38 = this.int_9; num38 <= this.int_8; num38++)
                                    {
                                        if (pane12.color_0[num38] != Color.Empty)
                                        {
                                            num3 = this.int_1[num38] - (this.int_0[num38] / 2);
                                            graphics_0.FillRectangle(new SolidBrush(pane12.color_0[num38]), num3, pane12.Top, this.int_0[num38], pane12.Height);
                                        }
                                    }
                                }
                            }
                            Brush brush5 = new SolidBrush(this.MarginRightColor);
                            graphics_0.FillRectangle(brush5, this.int_4 - this.MarginRightWidth, 0, this.MarginRightWidth, this.int_5);
                            Brush brush6 = new SolidBrush(this.MarginBottomColor);
                            graphics_0.FillRectangle(brush6, 0, this.int_5 - this.MarginBottomHeight, this.int_4, this.MarginBottomHeight);
                            if (this.HorizontalGridines)
                            {
                                Brush brush4 = new SolidBrush(ReverseColor(this.MarginRightColor));
                                foreach (ChartPane pane14 in this.list_0)
                                {
                                    if ((pane14.Visible && (pane14.Height > 0)) && (pane14.DisplayGrid && !pane14.Hidden))
                                    {
                                        this.gridLines_0.RangeMin = pane14.LowestValue;
                                        this.gridLines_0.RangeMax = pane14.HighestValue;
                                        this.gridLines_0.LinesDesired = pane14.Height / 20;
                                        this.gridLines_0.Decimals = this.Bars.SymbolInfo.Decimals;
                                        if (this.gridLines_0.GridIncrement > 0.0)
                                        {
                                            double gridFirstValue = this.gridLines_0.GridFirstValue;
                                            if (pane14.LogScale && (gridFirstValue == 0.0))
                                            {
                                                gridFirstValue += this.gridLines_0.GridIncrement;
                                            }
                                            num4 = pane14.ConvertValueToY(gridFirstValue);
                                            while (num4 >= pane14.Top)
                                            {
                                                if (num4 < (pane14.Top + pane14.Height))
                                                {
                                                    graphics_0.DrawLine(pen, 0, num4, this.int_4 - this.MarginRightWidth, num4);
                                                    if (num4 > (pane14.Top + this.AxisFont.Height))
                                                    {
                                                        int num42 = (this.int_4 - this.MarginRightWidth) + 2;
                                                        int num43 = num4 - ((int) (this.AxisFont.Height * 0.5));
                                                        graphics_0.DrawString(pane14.FormatChartValue(gridFirstValue), this.AxisFont, brush4, (float) num42, (float) num43);
                                                    }
                                                }
                                                gridFirstValue += this.gridLines_0.GridIncrement;
                                                num4 = pane14.ConvertValueToY(gridFirstValue);
                                                if ((num4 == 0) && (pane14.Top == 0))
                                                {
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            Brush brush3 = new SolidBrush(TextColorForBackground(this.MarginBottomColor));
                            int num37 = -2147483648;
                            pen.Width = 2f;
                            this.list_4.Clear();
                            if (bars_1.IsIntraday)
                            {
                                for (int num33 = this.int_9 + 1; num33 <= this.int_8; num33++)
                                {
                                    str = "";
                                    time = bars_1.Date[num33];
                                    time = bars_1.Date[num33 - 1];
                                    if (time.Day == time.Day)
                                    {
                                        continue;
                                    }
                                    time = bars_1.Date[num33];
                                    str = time.ToShortDateString();
                                    num3 = this.int_1[num33];
                                    width = (int) graphics_0.MeasureString(str, this.AxisFont).Width;
                                    Rectangle rectangle2 = new Rectangle {
                                        X = num3 - (width / 2),
                                        Width = width,
                                        Y = this.int_5 - this.MarginBottomHeight,
                                        Height = this.MarginBottomHeight
                                    };
                                    flag = true;
                                    using (enumerator6 = this.list_4.GetEnumerator())
                                    {
                                        while (enumerator6.MoveNext())
                                        {
                                            Rectangle current = enumerator6.Current;
                                            if (current.Contains(rectangle2.Left, (this.int_5 - this.MarginBottomHeight) + 1) || current.Contains(rectangle2.Left + width, (this.int_5 - this.MarginBottomHeight) + 1))
                                            {
                                                goto Label_0EB8;
                                            }
                                        }
                                        goto Label_0ECB;
                                    Label_0EB8:
                                        flag = false;
                                    }
                                Label_0ECB:
                                    if (flag)
                                    {
                                        graphics_0.DrawString(str, this.AxisFont, brush3, (float) rectangle2.Left, (float) rectangle2.Top);
                                        this.list_4.Add(rectangle2);
                                    }
                                    if (this.VerticalGridlines)
                                    {
                                        graphics_0.DrawLine(pen, num3, 0, num3, this.int_5 - this.MarginBottomHeight);
                                    }
                                }
                            }
                            pen.Width = 1f;
                            if (this.int_9 < this.int_1.Length)
                            {
                                num3 = this.int_1[this.int_9];
                                for (int num8 = this.int_9; num8 <= this.int_8; num8++)
                                {
                                    str = "";
                                    switch (bars_1.Scale)
                                    {
                                        case BarScale.Daily:
                                            if (num8 > 0)
                                            {
                                                time = bars_1.Date[num8];
                                                time = bars_1.Date[num8 - 1];
                                                if (time.Month != time.Month)
                                                {
                                                    time = bars_1.Date[num8];
                                                    str = time.ToShortDateString();
                                                }
                                            }
                                            break;

                                        case BarScale.Weekly:
                                            if (num8 > 0)
                                            {
                                                time = bars_1.Date[num8];
                                                time = bars_1.Date[num8 - 1];
                                                if (time.Month != time.Month)
                                                {
                                                    time = bars_1.Date[num8];
                                                    if (((time.Month + 2) % 3) == 0)
                                                    {
                                                        time = bars_1.Date[num8];
                                                        str = time.ToString("MMM-yy");
                                                    }
                                                }
                                            }
                                            break;

                                        case BarScale.Monthly:
                                        case BarScale.Quarterly:
                                        case BarScale.Yearly:
                                            if (num8 > 0)
                                            {
                                                time = bars_1.Date[num8];
                                                time = bars_1.Date[num8 - 1];
                                                if (time.Year != time.Year)
                                                {
                                                    time = bars_1.Date[num8];
                                                    str = time.Year.ToString();
                                                }
                                            }
                                            break;

                                        case BarScale.Minute:
                                            if (bars_1.BarInterval < 10)
                                            {
                                                time = bars_1.Date[num8];
                                                if (time.Minute == 0)
                                                {
                                                    str = bars_1.Date[num8].ToShortTimeString();
                                                }
                                            }
                                            break;
                                    }
                                    if (!(str != ""))
                                    {
                                        goto Label_120C;
                                    }
                                    if (this.VerticalGridlines)
                                    {
                                        graphics_0.DrawLine(pen, num3, 0, num3, this.int_5 - this.MarginBottomHeight);
                                    }
                                    width = (int) graphics_0.MeasureString(str, this.AxisFont).Width;
                                    int x = num3 - (width / 2);
                                    if (x <= num37)
                                    {
                                        goto Label_120C;
                                    }
                                    flag = true;
                                    using (enumerator6 = this.list_4.GetEnumerator())
                                    {
                                        while (enumerator6.MoveNext())
                                        {
                                            Rectangle rectangle3 = enumerator6.Current;
                                            if (rectangle3.Contains(x, (this.int_5 - this.MarginBottomHeight) + 1) || rectangle3.Contains(x + width, (this.int_5 - this.MarginBottomHeight) + 1))
                                            {
                                                goto Label_11CD;
                                            }
                                        }
                                        goto Label_11E0;
                                    Label_11CD:
                                        flag = false;
                                    }
                                Label_11E0:
                                    if (flag)
                                    {
                                        graphics_0.DrawString(str, this.AxisFont, brush3, (float) x, (float) (this.int_5 - this.MarginBottomHeight));
                                    }
                                    num37 = x + width;
                                Label_120C:
                                    num3 += this.int_0[num8];
                                }
                            }
                            if (this.WealthScript != null)
                            {
                                this.WealthScript.PaintHook(bars_1, graphics_0, this.chartStyle_0, PaintHookStage.BeforeBarsRender);
                            }
                            foreach (ChartPane pane6 in this.Panes)
                            {
                                this.ClipToPane(graphics_0, pane6);
                                this.method_9(pane6, pane6.list_2, graphics_0);
                            }
                            this.ClipToPane(graphics_0, null);
                            try
                            {
                                chartStyle_1.RenderBars(graphics_0);
                                foreach (ChartPane pane16 in this.Panes)
                                {
                                    if (pane16.Visible && !pane16.Hidden)
                                    {
                                        foreach (PlottedSymbol symbol2 in pane16.PlottedSymbols)
                                        {
                                            chartStyle_1.method_0(symbol2, pane16, graphics_0);
                                        }
                                    }
                                }
                            }
                            catch
                            {
                            }
                            foreach (ChartPane pane5 in this.Panes)
                            {
                                if (!pane5.Visible || pane5.Hidden)
                                {
                                    continue;
                                }
                                this.ClipToPane(graphics_0, pane5);
                                this.method_9(pane5, pane5.list_4, graphics_0);
                                using (IEnumerator<PlottedIndicator> enumerator5 = pane5.PlottedIndicators.GetEnumerator())
                                {
                                    int num26;
                                    int num27;
                                    Brush brush2;
                                    Color empty;
                                    int num29;
                                Label_1382:
                                    if (!enumerator5.MoveNext())
                                    {
                                        goto Label_1832;
                                    }
                                    PlottedIndicator indicator2 = enumerator5.Current;
                                    Pen pen4 = new Pen(indicator2.Color);
                                    switch (indicator2.Style)
                                    {
                                        case LineStyle.Histogram:
                                        {
                                            int top;
                                            int num24;
                                            int num25 = this.BarSpacing - 2;
                                            if (num25 < 1)
                                            {
                                                num25 = 1;
                                            }
                                            if (num25 > indicator2.Width)
                                            {
                                                num25 = indicator2.Width;
                                            }
                                            pen4.Width = num25;
                                            for (int num23 = this.int_9; num23 <= this.int_8; num23++)
                                            {
                                                if (num23 >= indicator2.Series.FirstValidValue)
                                                {
                                                    num3 = this.int_1[num23];
                                                    if (pane5.LogScale)
                                                    {
                                                        if (indicator2.Series[num23] > 0.0)
                                                        {
                                                            top = pane5.Top + pane5.Height;
                                                        }
                                                        else
                                                        {
                                                            top = pane5.Top;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        top = pane5.ConvertValueToY(0.0);
                                                    }
                                                    num24 = pane5.ConvertValueToY(indicator2.Series[num23]);
                                                    pen4.Color = indicator2.GetBarColor(num23);
                                                    graphics_0.DrawLine(pen4, num3, top, num3, num24);
                                                }
                                            }
                                            if (((indicator2.Series == this.bars_0.Volume) && !double.IsNaN(this.bars_0.Volume.PartialValue)) && (this.ScrollOffset == 0))
                                            {
                                                if (this.bars_0.Close.PartialValue > this.bars_0.Open.PartialValue)
                                                {
                                                    pen4.Color = this.UpBarVolumeColor;
                                                }
                                                else
                                                {
                                                    pen4.Color = this.DownBarVolumeColor;
                                                }
                                                num3 = (this.ChartWidth - this.MarginRightWidth) - (this.RightPaddingBars * this.BarSpacing);
                                                top = pane5.ConvertValueToY(0.0);
                                                num24 = pane5.ConvertValueToY(this.bars_0.Volume.PartialValue);
                                                graphics_0.DrawLine(pen4, num3, top, num3, num24);
                                            }
                                            goto Label_1795;
                                        }
                                        case LineStyle.Invisible:
                                            goto Label_1795;

                                        case LineStyle.Dots:
                                            empty = Color.Empty;
                                            brush2 = null;
                                            num29 = this.int_9 + 1;
                                            goto Label_177D;

                                        default:
                                            pen4.Width = indicator2.Width;
                                            pen4.StartCap = LineCap.Round;
                                            pen4.EndCap = pen4.StartCap;
                                            SetPenStyle(pen4, indicator2.Style);
                                            num27 = this.int_8;
                                            num26 = num27;
                                            break;
                                    }
                                Label_1402:
                                    if (((num26 < 1) || (num26 < this.int_9)) || (num27 <= 0))
                                    {
                                        goto Label_1795;
                                    }
                                    Color barColor = indicator2.GetBarColor(num27);
                                    num26 = num27 - 1;
                                    while (num26 >= 0)
                                    {
                                        if (!(indicator2.GetBarColor(num26) == barColor) || (num26 < this.int_9))
                                        {
                                            break;
                                        }
                                        num26--;
                                    }
                                    num26++;
                                    if (num26 == 0)
                                    {
                                        num26 = 1;
                                    }
                                    if (num27 == 0)
                                    {
                                        goto Label_1402;
                                    }
                                    int num20 = num26;
                                    if (num20 <= indicator2.Series.FirstValidValue)
                                    {
                                        num20 = indicator2.Series.FirstValidValue + 1;
                                    }
                                    if (num27 >= num20)
                                    {
                                        Point[] points = new Point[(num27 - num20) + 2];
                                        pen4.Color = barColor;
                                        int index = 0;
                                        for (int num19 = num27; num19 >= (num20 - 1); num19--)
                                        {
                                            points[index] = new Point(this.int_1[num19], pane5.ConvertValueToY(indicator2.Series[num19]));
                                            index++;
                                        }
                                        try
                                        {
                                            graphics_0.DrawLines(pen4, points);
                                        }
                                        catch (OverflowException)
                                        {
                                        }
                                    }
                                    goto Label_1819;
                                Label_16FD:
                                    if (num29 >= indicator2.Series.FirstValidValue)
                                    {
                                        Color color = indicator2.GetBarColor(num29);
                                        if (color != empty)
                                        {
                                            if (brush2 != null)
                                            {
                                                brush2.Dispose();
                                            }
                                            brush2 = new SolidBrush(color);
                                            empty = color;
                                        }
                                        num3 = this.int_1[num29];
                                        num4 = pane5.ConvertValueToY(indicator2.Series[num29]);
                                        graphics_0.FillEllipse(brush2, num3, num4, indicator2.Width, indicator2.Width);
                                    }
                                    num29++;
                                Label_177D:
                                    if (num29 <= this.int_8)
                                    {
                                        goto Label_16FD;
                                    }
                                    if (brush2 != null)
                                    {
                                        brush2.Dispose();
                                    }
                                Label_1795:
                                    if (indicator2.Selected)
                                    {
                                        Brush brush = new SolidBrush(indicator2.Color);
                                        for (int num22 = 0; num22 <= (int_12 - this.int_6); num22 += 20)
                                        {
                                            int num30 = this.ConvertXToBar(num22);
                                            if (num30 >= 0)
                                            {
                                                num3 = this.ConvertBarToX(num30);
                                                num4 = pane5.ConvertValueToY(indicator2.Series[num30]);
                                                graphics_0.FillEllipse(brush, num3 - 3, num4 - 3, 7, 7);
                                            }
                                        }
                                        brush.Dispose();
                                    }
                                    pen4.Dispose();
                                    goto Label_1382;
                                Label_1819:
                                    num27 = num26 - 1;
                                    goto Label_1402;
                                }
                            Label_1832:
                                this.ClipToPane(graphics_0, null);
                            }
                            if (this.tradingSystemExecutor_0 != null)
                            {
                                Pen pen5 = new Pen(Color.Blue, 2f);
                                Pen pen6 = new Pen(Color.Red, 2f);
                                foreach (Position position in this.tradingSystemExecutor_0.Performance.Results.Positions)
                                {
                                    if (position.Bars.Symbol == bars_1.Symbol)
                                    {
                                        Bitmap longEntry;
                                        int num45;
                                        int num46;
                                        string str4;
                                        object obj2;
                                        if ((position.EntryBar >= this.int_9) && (position.EntryBar <= this.int_8))
                                        {
                                            if (this.TradeArrowsVisible)
                                            {
                                                if (position.PositionType == PositionType.Long)
                                                {
                                                    str4 = "Buy ";
                                                    longEntry = Resources.LongEntry;
                                                }
                                                else
                                                {
                                                    str4 = "Short ";
                                                    longEntry = Resources.ShortEntry;
                                                }
                                                obj2 = str4;
                                                str4 = string.Concat(new object[] { obj2, position.Shares, " @", position.Bars.FormatValue(position.EntryPrice) });
                                                this.method_8(longEntry, position.EntryBar, str4, position.PositionType == PositionType.Short, Color.Black, false).Position = position;
                                            }
                                            if (this.TradeCirclesVisible)
                                            {
                                                num45 = this.PricePane.ConvertValueToY(position.EntryPrice);
                                                num46 = this.int_1[position.EntryBar];
                                                graphics_0.DrawEllipse(pen5, num46 - 3, num45 - 3, 6, 6);
                                            }
                                        }
                                        if ((!position.Active && (position.ExitBar >= this.int_9)) && ((position.ExitBar <= this.int_8) && this.TradeArrowsVisible))
                                        {
                                            Color blue;
                                            if (position.PositionType == PositionType.Long)
                                            {
                                                str4 = "Sell ";
                                                if (position.NetProfit > 0.0)
                                                {
                                                    longEntry = Resources.LongExitProfit;
                                                }
                                                else
                                                {
                                                    longEntry = Resources.LongExitLoss;
                                                }
                                            }
                                            else
                                            {
                                                str4 = "Cover ";
                                                if (position.NetProfit > 0.0)
                                                {
                                                    longEntry = Resources.ShortExitProfit;
                                                }
                                                else
                                                {
                                                    longEntry = Resources.ShortExitLoss;
                                                }
                                            }
                                            obj2 = str4;
                                            str4 = string.Concat(new object[] { obj2, position.Shares, " @", position.Bars.FormatValue(position.ExitPrice), "\n", position.NetProfit.ToString("C"), "\n", position.NetProfitPercent.ToString("N2"), "%" });
                                            if (position.NetProfit > 0.0)
                                            {
                                                blue = Color.Blue;
                                            }
                                            else
                                            {
                                                blue = Color.Red;
                                            }
                                            this.method_8(longEntry, position.ExitBar, str4, position.PositionType == PositionType.Long, blue, false).Position = position;
                                            if (this.TradeCirclesVisible)
                                            {
                                                num45 = this.PricePane.ConvertValueToY(position.ExitPrice);
                                                num46 = this.int_1[position.ExitBar];
                                                graphics_0.DrawEllipse(pen6, num46 - 3, num45 - 3, 6, 6);
                                            }
                                        }
                                    }
                                }
                                pen5.Dispose();
                                pen6.Dispose();
                            }
                            this.ClipToPane(graphics_0, this.PricePane);
                            foreach (Class33 class2 in this.list_2)
                            {
                                if ((class2.method_0() >= this.int_9) && (class2.method_0() <= this.int_8))
                                {
                                    try
                                    {
                                        num3 = this.ConvertBarToX(class2.method_0());
                                        num4 = this.PricePane.ConvertValueToY(class2.method_1());
                                        graphics_0.FillEllipse(class2.method_2(), num3, num4, 2, 2);
                                    }
                                    catch
                                    {
                                    }
                                }
                            }
                            this.ClipToPane(graphics_0, null);
                            if ((this.Fundamentals != null) && this.FundamentalsVisible)
                            {
                                foreach (string str3 in this.FundamentalGlyphs.Split(new char[] { ';' }))
                                {
                                    IList<FundamentalItem> list = this.Fundamentals.RequestSymbolItems(this.bars_0, this.bars_0.Symbol, str3);
                                    if (list != null)
                                    {
                                        foreach (FundamentalItem item in list)
                                        {
                                            if ((item.Bar >= this.int_9) && (item.Bar <= this.int_8))
                                            {
                                                this.method_8(item.Glyph, item.Bar, item.FormatValue(), true, Color.Black, true);
                                            }
                                        }
                                    }
                                }
                            }
                            foreach (ChartPane pane7 in this.Panes)
                            {
                                pane7.LabelOffset = 0;
                            }
                            if (bars_1.Symbol != "")
                            {
                                string text = bars_1.Symbol + " ";
                                if (bars_1.SecurityName != "")
                                {
                                    text = text + "(" + bars_1.SecurityName + ") ";
                                }
                                if (bars_1.IsIntraday)
                                {
                                    text = text + bars_1.BarInterval.ToString() + " ";
                                }
                                text = text + bars_1.Scale.ToString();
                                SizeF ef2 = graphics_0.MeasureString(text, this.font_1);
                                Rectangle rect = new Rectangle(2, this.PricePane.Top + 2, (int) (ef2.Width + 8f), (int) (ef2.Height + 4f));
                                graphics_0.FillRectangle(this.brush_0, rect);
                                graphics_0.DrawRectangle(pen3, rect);
                                graphics_0.DrawString(text, this.font_1, this.brush_1, 4f, (float) (this.PricePane.Top + 4));
                                this.PricePane.LabelOffset = (rect.Bottom + 2) - this.PricePane.Top;
                            }
                            if (this.IndicatorLabelsVisible)
                            {
                                foreach (ChartPane pane8 in this.Panes)
                                {
                                    if (pane8.Visible)
                                    {
                                        pane8.method_6(graphics_0);
                                    }
                                }
                            }
                            foreach (ChartPane pane13 in this.Panes)
                            {
                                if (pane13.list_3 != null)
                                {
                                    this.ClipToPane(graphics_0, pane13);
                                    this.method_9(pane13, pane13.list_3, graphics_0);
                                }
                            }
                            this.ClipToPane(graphics_0, null);
                            foreach (ChartPane pane11 in this.Panes)
                            {
                                if (pane11.Visible && !pane11.Hidden)
                                {
                                    foreach (PlottedIndicator indicator3 in pane11.PlottedIndicators)
                                    {
                                        if (((indicator3.Series == this.bars_0.Volume) && !double.IsNaN(this.bars_0.Volume.PartialValue)) && (this.int_8 == (this.bars_0.Count - 1)))
                                        {
                                            pane11.method_5(graphics_0, this.bars_0.Volume.PartialValue, indicator3.Color);
                                        }
                                        else
                                        {
                                            pane11.method_5(graphics_0, indicator3.Series[this.int_8], indicator3.Color);
                                        }
                                    }
                                }
                            }
                            double partialValue = this.Bars.Close[this.int_8];
                            if (!double.IsNaN(this.Bars.Close.PartialValue) && (this.int_8 == (this.Bars.Count - 1)))
                            {
                                partialValue = this.Bars.Close.PartialValue;
                            }
                            this.PricePane.method_5(graphics_0, partialValue, Color.Black);
                            if (this.WealthScript != null)
                            {
                                this.WealthScript.PaintHook(bars_1, graphics_0, this.chartStyle_0, PaintHookStage.AfterBarsRender);
                            }
                            foreach (ChartGlyph glyph in this.list_1)
                            {
                                graphics_0.DrawImage(glyph.Glyph, glyph.X, glyph.Y, glyph.Width, glyph.Height);
                            }
                            if (this.PaneSeparatorVisible)
                            {
                                Pen pen7 = new Pen(this.PaneSeparatorColor);
                                using (pen7)
                                {
                                    foreach (ChartPane pane17 in this.list_0)
                                    {
                                        if (pane17.Visible)
                                        {
                                            num4 = pane17.Top + pane17.Height;
                                            graphics_0.DrawLine(pen7, 0, num4, this.int_4, num4);
                                        }
                                    }
                                }
                            }
                            this.brush_0.Dispose();
                            this.brush_1.Dispose();
                            pen2.Dispose();
                            pen3.Dispose();
                        }
                    }
                    else
                    {
                        Font font = new Font("Arial", 12f);
                        graphics_0.DrawString("No Data Available", font, this.ReverseBackgroundBrush, (PointF) new Point(20, 20));
                    }
                }
            }
        }

        internal void method_10()
        {
            this.chartPane_1.PlottedIndicators.Clear();
            PlottedIndicator item = new PlottedIndicator(this, this.bars_0.Volume) {
                Style = LineStyle.Histogram,
                Width = 20,
                Color = this.UpBarVolumeColor
            };
            for (int i = 0; i < this.bars_0.Count; i++)
            {
                if (this.bars_0.Close[i] > this.bars_0.Open[i])
                {
                    item.SetBarColor(i, this.UpBarVolumeColor);
                }
                else
                {
                    item.SetBarColor(i, this.DownBarVolumeColor);
                }
            }
            this.chartPane_1.PlottedIndicators.Add(item);
        }

        internal void method_11(PlottedSymbol plottedSymbol_0, ChartPane chartPane_4)
        {
            this.color_11 = this.color_10;
            this.color_10 = null;
            this.color_12 = this.color_1;
            this.color_13 = this.color_2;
            this.color_1 = plottedSymbol_0.UpColor;
            this.color_2 = plottedSymbol_0.DownColor;
            this.chartPane_3 = this.chartPane_0;
            this.chartPane_0 = chartPane_4;
        }

        internal void method_12()
        {
            this.color_10 = this.color_11;
            this.color_11 = null;
            this.color_1 = this.color_12;
            this.color_2 = this.color_13;
            this.chartPane_0 = this.chartPane_3;
        }

        internal void method_13(int int_12, double double_0, TradeType tradeType_0)
        {
            Brush black = Brushes.Black;
            switch (tradeType_0)
            {
                case TradeType.Buy:
                    black = Brushes.Blue;
                    break;

                case TradeType.Sell:
                    black = Brushes.Red;
                    break;

                case TradeType.Short:
                    black = Brushes.Fuchsia;
                    break;

                case TradeType.Cover:
                    black = Brushes.Green;
                    break;
            }
            Class33 item = new Class33(int_12, double_0, black);
            this.list_2.Add(item);
        }

        internal int method_2(string string_4)
        {
            if (!string.IsNullOrEmpty(string_4) && this.dictionary_3.ContainsKey(string_4))
            {
                return this.dictionary_3[string_4];
            }
            return -1;
        }

        internal void method_3(string string_4, int int_12)
        {
            if (!string.IsNullOrEmpty(string_4))
            {
                if (this.dictionary_3.ContainsKey(string_4))
                {
                    this.dictionary_3[string_4] = int_12;
                }
                else
                {
                    this.dictionary_3.Add(string_4, int_12);
                }
            }
        }

        internal void method_4(string string_4)
        {
            if (!string.IsNullOrEmpty(string_4))
            {
                this.dictionary_3.Remove(string_4);
            }
        }

        internal bool method_5(string string_4)
        {
            return (!string.IsNullOrEmpty(string_4) && this.dictionary_3.ContainsKey(string_4));
        }

        private int method_6()
        {
            int num = this.RightPaddingBars - this.int_3;
            if (num < 0)
            {
                num = 0;
            }
            int num2 = (this.int_4 - this.MarginRightWidth) - (num * this.BarSpacing);
            int index = this.int_8;
            if (index < 0)
            {
                this.int_8 = 0;
                index = 0;
            }
            while (num2 >= 0)
            {
                num2 -= this.int_0[index];
                index--;
                if ((index <= 0) && (num2 >= 0))
                {
                    return 0;
                }
            }
            return (index + 1);
        }

        private int method_7()
        {
            if (this.bars_0 != null)
            {
                int index = ((this.bars_0.Count - this.ScrollOffset) - 1) + this.RightPaddingBars;
                if (index >= this.bars_0.Count)
                {
                    index = this.bars_0.Count - 1;
                }
                if (index >= 0)
                {
                    while (index >= 0)
                    {
                        if (this.int_0[index] != 0)
                        {
                            return index;
                        }
                        index--;
                    }
                    return index;
                }
                index = 0;
            }
            return 0;
        }

        private ChartGlyph method_8(Bitmap bitmap_0, int int_12, string string_4, bool bool_15, Color color_14, bool bool_16)
        {
            if (bitmap_0.Tag == null)
            {
                bitmap_0.Tag = "T";
                bitmap_0.MakeTransparent(Color.Fuchsia);
            }
            ChartGlyph item = new ChartGlyph(bitmap_0, string_4, color_14, int_12, bool_15) {
                X = this.int_1[int_12] - (bitmap_0.Width / 2)
            };
            if (bool_15)
            {
                item.Y = (this.PricePane.ConvertValueToY(this.bars_0.High[int_12]) - 4) - bitmap_0.Height;
                foreach (ChartGlyph glyph3 in this.list_1)
                {
                    if ((glyph3.Bar == int_12) && (glyph3.AboveBar == bool_15))
                    {
                        item.Y -= glyph3.Glyph.Height + 2;
                    }
                }
                if (bool_16 && (item.Y < this.PricePane.Top))
                {
                    item.AboveBar = false;
                    bool_15 = false;
                }
            }
            if (!bool_15)
            {
                item.Y = this.PricePane.ConvertValueToY(this.bars_0.Low[int_12]) + 4;
                foreach (ChartGlyph glyph2 in this.list_1)
                {
                    if ((glyph2.Bar == int_12) && (glyph2.AboveBar == bool_15))
                    {
                        item.Y += glyph2.Glyph.Height + 2;
                    }
                }
            }
            this.list_1.Add(item);
            return item;
        }

        private void method_9(ChartPane chartPane_4, List<WSDrawingObject> list_5, Graphics graphics_0)
        {
            if (list_5 != null)
            {
                foreach (WSDrawingObject obj2 in list_5)
                {
                    if (obj2 != null)
                    {
                        obj2.Render(graphics_0, chartPane_4, this);
                    }
                }
            }
        }

        public ChartPane PaneFromY(int int_12)
        {
            ChartPane pane2;
            using (IEnumerator<ChartPane> enumerator = this.Panes.GetEnumerator())
            {
                ChartPane current;
                while (enumerator.MoveNext())
                {
                    current = enumerator.Current;
                    if ((int_12 >= current.Top) && (int_12 < (current.Top + current.Height)))
                    {
                        goto Label_0039;
                    }
                }
                return null;
            Label_0039:
                pane2 = current;
            }
            return pane2;
        }

        public void PlotFundamentalItem(ChartPane pane, string itemName, Color color, LineStyle style, int width, bool dragDrop)
        {
            if (this.Bars != null)
            {
                this.PlotFundamentalItem(pane, this.Bars.Symbol, itemName, color, style, width, dragDrop);
            }
        }

        public void PlotFundamentalItem(ChartPane pane, string symbol, string itemName, Color color, LineStyle style, int width, bool dragDrop)
        {
            if (this.Fundamentals != null)
            {
                IList<FundamentalItem> items = this.Fundamentals.RequestSymbolItems(this.Bars, symbol, itemName);
                if (items == null)
                {
                    items = this.Fundamentals.RequestNonSymbolItems(this.Bars, itemName);
                }
                if (items != null)
                {
                    WSDPlottedFundamentalItem item = new WSDPlottedFundamentalItem(items, color, style, width);
                    if (dragDrop)
                    {
                        pane.method_3(item);
                    }
                    else
                    {
                        pane.method_4(item, true);
                    }
                }
            }
        }

        public void PlotSymbol(WealthLab.Bars bars, ChartPane pane, Color upColor, Color downColor)
        {
            PlottedSymbol item = new PlottedSymbol(bars, upColor, downColor);
            pane.PlottedSymbols.Add(item);
        }

        public void PlotWealthScriptObject(ChartPane pane, WSDrawingObject wsdrawingObject_0, bool fromDragDrop, bool behindBars)
        {
            if (fromDragDrop)
            {
                pane.method_3(wsdrawingObject_0);
            }
            else
            {
                pane.method_4(wsdrawingObject_0, behindBars);
            }
        }

        public void RemovePane(string paneDescription)
        {
            using (List<ChartPane>.Enumerator enumerator = this.list_0.GetEnumerator())
            {
                ChartPane current;
                while (enumerator.MoveNext())
                {
                    current = enumerator.Current;
                    if (current.Description == paneDescription)
                    {
                        goto Label_0030;
                    }
                }
                return;
            Label_0030:
                this.list_0.Remove(current);
                this.method_4(current.Description);
            }
        }

        public void Render(WealthLab.Bars bars, Graphics graphics_0, int width, int height, WealthLab.ChartStyle chartStyle)
        {
            this.Render(bars, graphics_0, width, height, chartStyle, false, 0, "");
        }

        public void Render(WealthLab.Bars bars, Graphics graphics_0, int width, int height, WealthLab.ChartStyle chartStyle, bool bPaneResizing, int delta, string paneDesc)
        {
            this.method_1(bars, graphics_0, width, height, chartStyle, bPaneResizing, delta, paneDesc, false);
        }

        public void RestoreHiddenPaneOrigHeights(string string_4)
        {
            if (!string.IsNullOrEmpty(string_4))
            {
                this.dictionary_3.Clear();
                foreach (string str in string_4.Split(new char[] { ';' }))
                {
                    int num2;
                    string[] strArray3 = str.Split(new char[] { '=' });
                    int.TryParse(strArray3[1], out num2);
                    this.dictionary_3.Add(strArray3[0], num2);
                }
            }
        }

        public void RestoreResizedPanes(string string_4)
        {
            if (!string.IsNullOrEmpty(string_4))
            {
                this.dictionary_2.Clear();
                foreach (string str in string_4.Split(new char[] { ';' }))
                {
                    string[] strArray3 = str.Split(new char[] { '=' });
                    this.dictionary_2[strArray3[0]] = int.Parse(strArray3[1]);
                }
                this.bool_13 = true;
            }
        }

        public static Color ReverseColor(Color color)
        {
            byte red = (byte) (0xff - color.R);
            byte green = (byte) (0xff - color.G);
            byte blue = (byte) (0xff - color.B);
            return Color.FromArgb(red, green, blue);
        }

        public string SaveHiddenPaneOrigHeights()
        {
            StringBuilder builder = new StringBuilder();
            bool flag = true;
            foreach (KeyValuePair<string, int> pair in this.dictionary_3)
            {
                if (!flag)
                {
                    builder.Append(";");
                }
                builder.Append(pair.Key);
                builder.Append("=");
                builder.Append(pair.Value.ToString());
                flag = false;
            }
            return builder.ToString();
        }

        public string SavePaneSizes()
        {
            StringBuilder builder = new StringBuilder();
            bool flag = true;
            foreach (KeyValuePair<string, int> pair in this.dictionary_2)
            {
                if (!flag)
                {
                    builder.Append(";");
                }
                builder.Append(pair.Key);
                builder.Append("=");
                builder.Append(pair.Value.ToString());
                flag = false;
            }
            return builder.ToString();
        }

        public void SetBackgroundColor(int int_12, Color color)
        {
            if (this.color_9 == null)
            {
                this.color_9 = new Color[this.bars_0.Count];
                for (int i = 0; i < this.bars_0.Count; i++)
                {
                    this.color_9[i] = Color.Empty;
                }
            }
            this.color_9[int_12] = color;
        }

        public void SetBarAnnotationCount(int int_12, bool above, int value)
        {
            if (above)
            {
                this.dictionary_0[int_12] = value;
            }
            else
            {
                this.dictionary_1[int_12] = value;
            }
        }

        public void SetBarColor(int int_12, Color color)
        {
            if (this.color_10 == null)
            {
                this.color_10 = new Color[this.bars_0.Count];
                for (int i = 0; i < this.bars_0.Count; i++)
                {
                    if (this.bars_0.Close[i] > this.bars_0.Open[i])
                    {
                        this.color_10[i] = this.UpBarColor;
                    }
                    else
                    {
                        this.color_10[i] = this.DownBarColor;
                    }
                }
            }
            this.color_10[int_12] = color;
        }

        public void SetBarWidth(int int_12, int width)
        {
            this.int_0[int_12] = width;
        }

        public static void SetPenStyle(Pen pen_0, LineStyle style)
        {
            if (style == LineStyle.Dotted)
            {
                pen_0.DashStyle = DashStyle.Dot;
            }
            else if (style == LineStyle.Dashed)
            {
                pen_0.DashStyle = DashStyle.Dash;
            }
        }

        public static Color TextColorForBackground(Color color)
        {
            Color white = Color.White;
            if (((color.R > 0) || (color.G > 0)) && (color.GetBrightness() >= 0.5))
            {
                white = Color.Black;
            }
            return white;
        }

        [Category("Margins")]
        public Font AxisFont
        {
            get
            {
                return this.font_0;
            }
            set
            {
                this.font_0 = value;
            }
        }

        internal Brush BackgroundBrush
        {
            get
            {
                return this.brush_0;
            }
        }

        [Category("Colors")]
        public Color BackgroundColor
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

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public WealthLab.Bars Bars
        {
            get
            {
                return this.bars_0;
            }
            set
            {
                this.bars_0 = value;
            }
        }

        [Category("Data Rendering")]
        public int BarSpacing
        {
            get
            {
                return this.int_2;
            }
            set
            {
                this.int_2 = value;
                if (this.int_2 < 1)
                {
                    this.int_2 = 1;
                }
                this.bool_14 = true;
            }
        }

        [Browsable(false)]
        public int ChartHeight
        {
            get
            {
                return this.int_5;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public WealthLab.ChartStyle ChartStyle
        {
            get
            {
                return this.chartStyle_0;
            }
        }

        [Browsable(false)]
        public int ChartWidth
        {
            get
            {
                return this.int_4;
            }
        }

        [Category("Colors")]
        public Color DownBarColor
        {
            get
            {
                return this.color_2;
            }
            set
            {
                this.color_2 = value;
            }
        }

        [Category("Colors")]
        public Color DownBarVolumeColor
        {
            get
            {
                return this.color_4;
            }
            set
            {
                this.color_4 = value;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool Executing
        {
            get
            {
                return this.bool_11;
            }
            set
            {
                this.bool_11 = value;
            }
        }

        [Category("Linked Components")]
        public TradingSystemExecutor Executor
        {
            get
            {
                return this.tradingSystemExecutor_0;
            }
            set
            {
                this.tradingSystemExecutor_0 = value;
            }
        }

        [Category("Visibility")]
        public string FundamentalGlyphs
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

        [Category("Linked Components")]
        public FundamentalsLoader Fundamentals
        {
            get
            {
                return this.fundamentalsLoader_0;
            }
            set
            {
                this.fundamentalsLoader_0 = value;
            }
        }

        [Category("Visibility")]
        public bool FundamentalsVisible
        {
            get
            {
                return this.bool_9;
            }
            set
            {
                this.bool_9 = value;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<ChartGlyph> Glyphs
        {
            get
            {
                return this.list_1;
            }
        }

        [Category("Colors")]
        public Color GridlineColor
        {
            get
            {
                return this.color_7;
            }
            set
            {
                this.color_7 = value;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int Height
        {
            get
            {
                return this.int_5;
            }
        }

        [Category("Visibility")]
        public bool HorizontalGridines
        {
            get
            {
                return this.bool_2;
            }
            set
            {
                this.bool_2 = value;
            }
        }

        public bool IndicatorLabelsVisible
        {
            get
            {
                return this.bool_8;
            }
            set
            {
                this.bool_8 = value;
            }
        }

        [Browsable(false)]
        public int LeftEdgeBar
        {
            get
            {
                return this.int_9;
            }
        }

        [Category("Data Rendering")]
        public bool LogScale
        {
            get
            {
                return this.bool_7;
            }
            set
            {
                this.bool_7 = value;
                if (this.PricePane != null)
                {
                    this.PricePane.LogScale = value;
                }
            }
        }

        [Category("Colors")]
        public Color MarginBottomColor
        {
            get
            {
                return this.color_6;
            }
            set
            {
                this.color_6 = value;
            }
        }

        [Category("Margins")]
        public int MarginBottomHeight
        {
            get
            {
                return this.int_7;
            }
            set
            {
                this.int_7 = value;
            }
        }

        [Category("Colors")]
        public Color MarginRightColor
        {
            get
            {
                return this.color_5;
            }
            set
            {
                this.color_5 = value;
            }
        }

        [Category("Margins")]
        public int MarginRightWidth
        {
            get
            {
                return this.int_6;
            }
            set
            {
                this.int_6 = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public byte PaneCreationCounter
        {
            get
            {
                return this.byte_0;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public IList<ChartPane> Panes
        {
            get
            {
                return this.list_0;
            }
        }

        [Category("Colors")]
        public Color PaneSeparatorColor
        {
            get
            {
                return this.color_8;
            }
            set
            {
                this.color_8 = value;
            }
        }

        [Category("Visibility")]
        public bool PaneSeparatorVisible
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

        public bool PlotStops
        {
            get
            {
                return this.bool_10;
            }
            set
            {
                this.bool_10 = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public ChartPane PricePane
        {
            get
            {
                return this.chartPane_0;
            }
        }

        internal Brush ReverseBackgroundBrush
        {
            get
            {
                return this.brush_1;
            }
        }

        [Browsable(false)]
        public int RightEdgeBar
        {
            get
            {
                return this.int_8;
            }
        }

        [Category("Data Rendering")]
        public int RightPaddingBars
        {
            get
            {
                return this.int_10;
            }
            set
            {
                this.int_10 = value;
            }
        }

        [Category("Data Rendering")]
        public int ScrollOffset
        {
            get
            {
                return this.int_3;
            }
            set
            {
                this.int_3 = value;
                if (this.int_3 < 0)
                {
                    this.int_3 = 0;
                }
                if ((this.bars_0 != null) && (this.int_3 > (this.bars_0.Count + this.RightPaddingBars)))
                {
                    this.int_3 = this.bars_0.Count + this.RightPaddingBars;
                }
                this.bool_14 = true;
            }
        }

        public Font TitleFont
        {
            get
            {
                return this.font_1;
            }
            set
            {
                this.font_1 = value;
            }
        }

        [Category("Visibility")]
        public bool TradeAnnotationsVisible
        {
            get
            {
                return this.bool_6;
            }
            set
            {
                this.bool_6 = value;
            }
        }

        [Category("Visibility")]
        public bool TradeArrowsVisible
        {
            get
            {
                return this.bool_4;
            }
            set
            {
                this.bool_4 = value;
            }
        }

        [Category("Visibility")]
        public bool TradeCirclesVisible
        {
            get
            {
                return this.bool_5;
            }
            set
            {
                this.bool_5 = value;
            }
        }

        [Category("Colors")]
        public Color UpBarColor
        {
            get
            {
                return this.color_1;
            }
            set
            {
                this.color_1 = value;
            }
        }

        [Category("Colors")]
        public Color UpBarVolumeColor
        {
            get
            {
                return this.color_3;
            }
            set
            {
                this.color_3 = value;
            }
        }

        [Category("Visibility")]
        public bool VerticalGridlines
        {
            get
            {
                return this.bool_3;
            }
            set
            {
                this.bool_3 = value;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ChartPane VolumePane
        {
            get
            {
                return this.chartPane_1;
            }
        }

        [Category("Visibility")]
        public bool VolumePaneVisible
        {
            get
            {
                return this.bool_0;
            }
            set
            {
                this.bool_0 = value;
                if (this.VolumePane != null)
                {
                    this.VolumePane.Visible = value;
                }
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public WealthLab.WealthScript WealthScript
        {
            get
            {
                return this.wealthScript_0;
            }
            set
            {
                this.wealthScript_0 = value;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int Width
        {
            get
            {
                return this.int_4;
            }
        }
    }
}

