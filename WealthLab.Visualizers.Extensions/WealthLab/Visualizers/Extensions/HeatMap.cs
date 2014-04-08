namespace WealthLab.Visualizers.Extensions
{
    using System;
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Forms;
    using System.Windows.Markup;
    using System.Windows.Media;
    using System.Windows.Shapes;
    //using ;
    using WealthLab;
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public class HeatMap : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector
    {
        private Bars _bars;
        private bool _contentLoaded;
        private double _max;
        private double _min;
        private SystemPerformance _performance;
        private List<Position> _positions;
        private List<WealthLab.Visualizers.Extensions.Range> _rangeList;
        private IVisualizerHost _visHost;
        internal System.Windows.Controls.Button btnLoss;
        internal System.Windows.Controls.Button btnProfit;
        internal System.Windows.Controls.Button btnSearch;
        internal System.Windows.Controls.ComboBox cmbView;
        private ColorDialog dialog = new ColorDialog();
        internal System.Windows.Controls.GroupBox gbColorKey;
        internal System.Windows.Controls.GroupBox gbKey;
        internal System.Windows.Controls.GroupBox gbSearch;
        internal System.Windows.Controls.GroupBox gbView;
        internal Grid gHeatMap;
        internal System.Windows.Controls.Label label1;
        internal System.Windows.Controls.Label label2;
        internal System.Windows.Controls.Label lbl_strategy;
        internal System.Windows.Controls.Label lbl_symbol;
        internal System.Windows.Controls.Label lblColor;
        internal System.Windows.Controls.Label lblKey;
        internal System.Windows.Controls.Label lblQty;
        internal System.Windows.Controls.Label lblSize;
        internal System.Windows.Shapes.Rectangle rectangle1;
        internal System.Windows.Controls.Primitives.StatusBar statusBar1;
        internal TreeMaps.Controls.SquarifiedTreeMapsPanel tmpHeatMap;
        internal TextBlock txtColor;
        internal TextBlock txtQty;
        internal System.Windows.Controls.TextBox txtSearch;
        internal TextBlock txtSize;

        public HeatMap()
        {
            this.InitializeComponent();
        }

        private void btn_search_Click(object sender, RoutedEventArgs e)
        {
            bool flag = false;
            foreach (System.Windows.Shapes.Rectangle rectangle in this.tmpHeatMap.Children)
            {
                string tag = rectangle.Tag as string;
                if (tag.ToUpper() == this.txtSearch.Text.ToUpper())
                {
                    rectangle.Stroke = System.Windows.Media.Brushes.Yellow;
                    rectangle.StrokeDashCap = PenLineCap.Square;
                    rectangle.StrokeThickness = 3.0;
                    flag = true;
                }
            }
            if (!flag)
            {
                System.Windows.MessageBox.Show("Could not find symbol " + this.txtSearch.Text);
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (this.dialog.ShowDialog() == DialogResult.OK)
            {
                BrushConverter converter = new BrushConverter();
                if (this.dialog.Color.IsKnownColor)
                {
                    this.btnProfit.Background = (System.Windows.Media.Brush) converter.ConvertFromString(this.dialog.Color.Name);
                }
                else
                {
                    this.btnProfit.Background = (System.Windows.Media.Brush) converter.ConvertFromString(this.dialog.Color.Name.Insert(0, "#"));
                }
            }
            this.FillColorImage();
            this.GenerateHeatMap();
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            if (this.dialog.ShowDialog() == DialogResult.OK)
            {
                BrushConverter converter = new BrushConverter();
                if (this.dialog.Color.IsKnownColor)
                {
                    this.btnLoss.Background = (System.Windows.Media.Brush) converter.ConvertFromString(this.dialog.Color.Name);
                }
                else
                {
                    this.btnLoss.Background = (System.Windows.Media.Brush) converter.ConvertFromString(this.dialog.Color.Name.Insert(0, "#"));
                }
            }
            this.FillColorImage();
            this.GenerateHeatMap();
        }

        // ///WYJ fix, code from Reflector
        /*
        public void CalculateAveragesMultipleStock()
        {
            this.txtSize.Text = "Number of trades for each stock.";
            this.txtColor.Text = "Average net profit percent of all trades.";
            if (CS$<>9__CachedAnonymousMethodDelegate3 == null)
            {
                CS$<>9__CachedAnonymousMethodDelegate3 = new Func<Position, string>(null, (IntPtr) <CalculateAveragesMultipleStock>b__2);
            }
            foreach (IGrouping<string, Position> grouping in this._positions.GroupBy<Position, string>(CS$<>9__CachedAnonymousMethodDelegate3))
            {
                double num = 0.0;
                double num2 = 0.0;
                foreach (Position position in grouping)
                {
                    num += position.NetProfitPercent;
                }
                num2 = num / ((double) grouping.Count<Position>());
                if (num2 > this._max)
                {
                    this._max = num2;
                }
                if (num2 < this._min)
                {
                    this._min = num2;
                }
            }
            this.CalculateRange();
        }*/

        // ///WYJ fix, code from Telerik JustCompile
        public void CalculateAveragesMultipleStock()
        {
            this.txtSize.Text = "Number of trades for each stock.";
            this.txtColor.Text = "Average net profit percent of all trades.";
            List<Position> positions = this._positions;
            IEnumerable<IGrouping<string, Position>> groupings = positions.GroupBy<Position, string>((Position p) => p.Bars.Symbol);
            foreach (IGrouping<string, Position> strs in groupings)
            {
                double netProfitPercent = 0;
                double num = 0;
                foreach (Position position in strs)
                {
                    netProfitPercent = netProfitPercent + position.NetProfitPercent;
                }
                num = netProfitPercent / (double)strs.Count<Position>();
                if (num > this._max)
                {
                    this._max = num;
                }
                if (num >= this._min)
                {
                    continue;
                }
                this._min = num;
            }
            this.CalculateRange();
        }

        // ///WYJ fix, code from Reflector
        /*
        public void CalculateAveragesSingleStock()
        {
            this.txtSize.Text = "Dollar Size of Trade";
            this.txtColor.Text = "Net profit percent. Out of overall profit.";
            if (CS$<>9__CachedAnonymousMethodDelegate5 == null)
            {
                CS$<>9__CachedAnonymousMethodDelegate5 = new Func<Position, string>(null, (IntPtr) <CalculateAveragesSingleStock>b__4);
            }
            foreach (IGrouping<string, Position> grouping in this._positions.GroupBy<Position, string>(CS$<>9__CachedAnonymousMethodDelegate5))
            {
                foreach (Position position in grouping)
                {
                    if (position.NetProfitPercent > this._max)
                    {
                        this._max = position.NetProfitPercent;
                    }
                    if (position.NetProfitPercent < this._min)
                    {
                        this._min = position.NetProfitPercent;
                    }
                }
            }
            this.CalculateRange();
        } */

        // ///WYJ fix, code from Telerik JustCompile
        public void CalculateAveragesSingleStock()
        {
            this.txtSize.Text = "Dollar Size of Trade";
            this.txtColor.Text = "Net profit percent. Out of overall profit.";
            List<Position> positions = this._positions;
            IEnumerable<IGrouping<string, Position>> groupings = positions.GroupBy<Position, string>((Position p) => p.Bars.Symbol);
            foreach (IGrouping<string, Position> strs in groupings)
            {
                foreach (Position position in strs)
                {
                    if (position.NetProfitPercent > this._max)
                    {
                        this._max = position.NetProfitPercent;
                    }
                    if (position.NetProfitPercent >= this._min)
                    {
                        continue;
                    }
                    this._min = position.NetProfitPercent;
                }
            }
            this.CalculateRange();
        }

        public void CalculateRange()
        {
            double num = 1.0;
            int num2 = 14;
            if ((this._min * -1.0) > this._max)
            {
                this._max = this._min * -1.0;
            }
            else
            {
                this._min = this._max * -1.0;
            }
            this._rangeList = new List<WealthLab.Visualizers.Extensions.Range>();
            double num3 = (this._max - this._min) / ((double) num2);
            for (int i = 1; i <= num2; i++)
            {
                double num7;
                double num5 = num3;
                num5 *= i;
                double num6 = this._min + num5;
                if (num == 0.0)
                {
                    num7 = num;
                    num = 1.0;
                }
                else
                {
                    num7 = num6 - num3;
                }
                if (num6 == 0.0)
                {
                    num = 0.0;
                }
                WealthLab.Visualizers.Extensions.Range item = new WealthLab.Visualizers.Extensions.Range {
                    High = num6,
                    Low = num7
                };
                this._rangeList.Add(item);
            }
        }

        private void cb_view_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.GenerateHeatMap();
        }

        public void DetermineColor(System.Windows.Shapes.Rectangle r, double number)
        {
            double num = Math.Round(this._max, MidpointRounding.AwayFromZero);
            double num2 = Math.Round(this._min, MidpointRounding.AwayFromZero);
            this.lblKey.Content = string.Concat(new object[] { num2, "% ", Math.Round((double) (num2 / 4.0)), "%  0%  +", Math.Round((double) (num / 4.0)), "% +", num, "%" });
            for (int i = 0; i < 14; i++)
            {
                System.Drawing.Color color;
                BrushConverter converter = new BrushConverter();
                System.Windows.Media.Color color2 = (System.Windows.Media.Color) System.Windows.Media.ColorConverter.ConvertFromString(this.btnLoss.Background.ToString());
                int num4 = color2.R;
                System.Windows.Media.Color color3 = (System.Windows.Media.Color) System.Windows.Media.ColorConverter.ConvertFromString(this.btnLoss.Background.ToString());
                int g = color3.G;
                System.Windows.Media.Color color4 = (System.Windows.Media.Color) System.Windows.Media.ColorConverter.ConvertFromString(this.btnLoss.Background.ToString());
                int b = color4.B;
                System.Windows.Media.Color color5 = (System.Windows.Media.Color) System.Windows.Media.ColorConverter.ConvertFromString(this.btnProfit.Background.ToString());
                int num7 = color5.R;
                System.Windows.Media.Color color6 = (System.Windows.Media.Color) System.Windows.Media.ColorConverter.ConvertFromString(this.btnProfit.Background.ToString());
                int num8 = color6.G;
                System.Windows.Media.Color color7 = (System.Windows.Media.Color) System.Windows.Media.ColorConverter.ConvertFromString(this.btnProfit.Background.ToString());
                int num9 = color7.B;
                if (number < this._rangeList[i].Low)
                {
                    continue;
                }
                if (number < this._rangeList[i].High)
                {
                    int num10;
                    int num11;
                    int num12;
                    switch (i)
                    {
                        case 0:
                            num10 = (num4 * 14) / 14;
                            num11 = (g * 14) / 14;
                            num12 = (b * 14) / 14;
                            color = System.Drawing.Color.FromArgb(num10, num11, num12);
                            if (!color.IsKnownColor)
                            {
                                goto Label_0253;
                            }
                            r.Fill = (System.Windows.Media.Brush) converter.ConvertFromString(color.Name);
                            break;

                        case 1:
                            num10 = ((num4 * 13) / 14) + (num7 / 14);
                            num11 = ((g * 13) / 14) + (num8 / 14);
                            num12 = ((b * 13) / 14) + (num9 / 14);
                            color = System.Drawing.Color.FromArgb(num10, num11, num12);
                            if (!color.IsKnownColor)
                            {
                                goto Label_02DF;
                            }
                            r.Fill = (System.Windows.Media.Brush) converter.ConvertFromString(color.Name);
                            break;

                        case 2:
                            num10 = ((num4 * 12) / 14) + ((num7 * 2) / 14);
                            num11 = ((g * 12) / 14) + ((num8 * 2) / 14);
                            num12 = ((b * 12) / 14) + ((num9 * 2) / 14);
                            color = System.Drawing.Color.FromArgb(num10, num11, num12);
                            if (!color.IsKnownColor)
                            {
                                goto Label_0371;
                            }
                            r.Fill = (System.Windows.Media.Brush) converter.ConvertFromString(color.Name);
                            break;

                        case 3:
                            num10 = ((num4 * 11) / 14) + ((num7 * 3) / 14);
                            num11 = ((g * 11) / 14) + ((num8 * 3) / 14);
                            num12 = ((b * 11) / 14) + ((num9 * 3) / 14);
                            color = System.Drawing.Color.FromArgb(num10, num11, num12);
                            if (!color.IsKnownColor)
                            {
                                goto Label_0403;
                            }
                            r.Fill = (System.Windows.Media.Brush) converter.ConvertFromString(color.Name);
                            break;

                        case 4:
                            num10 = ((num4 * 10) / 14) + ((num7 * 4) / 14);
                            num11 = ((g * 10) / 14) + ((num8 * 4) / 14);
                            num12 = ((b * 10) / 14) + ((num9 * 4) / 14);
                            color = System.Drawing.Color.FromArgb(num10, num11, num12);
                            if (!color.IsKnownColor)
                            {
                                goto Label_0495;
                            }
                            r.Fill = (System.Windows.Media.Brush) converter.ConvertFromString(color.Name);
                            break;

                        case 5:
                            num10 = ((num4 * 9) / 14) + ((num7 * 5) / 14);
                            num11 = ((g * 9) / 14) + ((num8 * 5) / 14);
                            num12 = ((b * 9) / 14) + ((num9 * 5) / 14);
                            color = System.Drawing.Color.FromArgb(num10, num11, num12);
                            if (!color.IsKnownColor)
                            {
                                goto Label_0527;
                            }
                            r.Fill = (System.Windows.Media.Brush) converter.ConvertFromString(color.Name);
                            break;

                        case 6:
                            num10 = ((num4 * 8) / 14) + ((num7 * 6) / 14);
                            num11 = ((g * 8) / 14) + ((num8 * 6) / 14);
                            num12 = ((b * 8) / 14) + ((num9 * 6) / 14);
                            color = System.Drawing.Color.FromArgb(num10, num11, num12);
                            if (!color.IsKnownColor)
                            {
                                goto Label_05B6;
                            }
                            r.Fill = (System.Windows.Media.Brush) converter.ConvertFromString(color.Name);
                            break;

                        case 7:
                            num10 = ((num4 * 7) / 14) + ((num7 * 7) / 14);
                            num11 = ((g * 7) / 14) + ((num8 * 7) / 14);
                            num12 = ((b * 7) / 14) + ((num9 * 7) / 14);
                            color = System.Drawing.Color.FromArgb(num10, num11, num12);
                            if (!color.IsKnownColor)
                            {
                                goto Label_0645;
                            }
                            r.Fill = (System.Windows.Media.Brush) converter.ConvertFromString(color.Name);
                            break;

                        case 8:
                            num10 = ((num4 * 6) / 14) + ((num7 * 8) / 14);
                            num11 = ((g * 6) / 14) + ((num8 * 8) / 14);
                            num12 = ((b * 6) / 14) + ((num9 * 8) / 14);
                            color = System.Drawing.Color.FromArgb(num10, num11, num12);
                            if (!color.IsKnownColor)
                            {
                                goto Label_06D4;
                            }
                            r.Fill = (System.Windows.Media.Brush) converter.ConvertFromString(color.Name);
                            break;

                        case 9:
                            num10 = ((num4 * 5) / 14) + ((num7 * 9) / 14);
                            num11 = ((g * 5) / 14) + ((num8 * 9) / 14);
                            num12 = ((b * 5) / 14) + ((num9 * 9) / 14);
                            color = System.Drawing.Color.FromArgb(num10, num11, num12);
                            if (!color.IsKnownColor)
                            {
                                goto Label_0766;
                            }
                            r.Fill = (System.Windows.Media.Brush) converter.ConvertFromString(color.Name);
                            break;

                        case 10:
                            num10 = ((num4 * 4) / 14) + ((num7 * 10) / 14);
                            num11 = ((g * 4) / 14) + ((num8 * 10) / 14);
                            num12 = ((b * 4) / 14) + ((num9 * 10) / 14);
                            color = System.Drawing.Color.FromArgb(num10, num11, num12);
                            if (!color.IsKnownColor)
                            {
                                goto Label_07F8;
                            }
                            r.Fill = (System.Windows.Media.Brush) converter.ConvertFromString(color.Name);
                            break;

                        case 11:
                            num10 = ((num4 * 3) / 14) + ((num7 * 11) / 14);
                            num11 = ((g * 3) / 14) + ((num8 * 11) / 14);
                            num12 = ((b * 3) / 14) + ((num9 * 11) / 14);
                            color = System.Drawing.Color.FromArgb(num10, num11, num12);
                            if (!color.IsKnownColor)
                            {
                                goto Label_088A;
                            }
                            r.Fill = (System.Windows.Media.Brush) converter.ConvertFromString(color.Name);
                            break;

                        case 12:
                            num10 = ((num4 * 2) / 14) + ((num7 * 12) / 14);
                            num11 = ((g * 2) / 14) + ((num8 * 12) / 14);
                            num12 = ((b * 2) / 14) + ((num9 * 12) / 14);
                            color = System.Drawing.Color.FromArgb(num10, num11, num12);
                            if (!color.IsKnownColor)
                            {
                                goto Label_091C;
                            }
                            r.Fill = (System.Windows.Media.Brush) converter.ConvertFromString(color.Name);
                            break;

                        case 13:
                            num10 = (num4 / 14) + ((num7 * 13) / 14);
                            num11 = (g / 14) + ((num8 * 13) / 14);
                            num12 = (b / 14) + ((num9 * 13) / 14);
                            color = System.Drawing.Color.FromArgb(num10, num11, num12);
                            if (!color.IsKnownColor)
                            {
                                goto Label_09A8;
                            }
                            r.Fill = (System.Windows.Media.Brush) converter.ConvertFromString(color.Name);
                            break;

                        case 14:
                            num10 = (num7 * 14) / 14;
                            num11 = (num8 * 14) / 14;
                            num12 = (num9 * 14) / 14;
                            color = System.Drawing.Color.FromArgb(num10, num11, num12);
                            if (!color.IsKnownColor)
                            {
                                goto Label_0A1D;
                            }
                            r.Fill = (System.Windows.Media.Brush) converter.ConvertFromString(color.Name);
                            break;
                    }
                }
                goto Label_0A41;
            Label_0253:
                r.Fill = (System.Windows.Media.Brush) converter.ConvertFromString(color.Name.Insert(0, "#"));
                goto Label_0A41;
            Label_02DF:
                r.Fill = (System.Windows.Media.Brush) converter.ConvertFromString(color.Name.Insert(0, "#"));
                goto Label_0A41;
            Label_0371:
                r.Fill = (System.Windows.Media.Brush) converter.ConvertFromString(color.Name.Insert(0, "#"));
                goto Label_0A41;
            Label_0403:
                r.Fill = (System.Windows.Media.Brush) converter.ConvertFromString(color.Name.Insert(0, "#"));
                goto Label_0A41;
            Label_0495:
                r.Fill = (System.Windows.Media.Brush) converter.ConvertFromString(color.Name.Insert(0, "#"));
                goto Label_0A41;
            Label_0527:
                r.Fill = (System.Windows.Media.Brush) converter.ConvertFromString(color.Name.Insert(0, "#"));
                goto Label_0A41;
            Label_05B6:
                r.Fill = (System.Windows.Media.Brush) converter.ConvertFromString(color.Name.Insert(0, "#"));
                goto Label_0A41;
            Label_0645:
                r.Fill = (System.Windows.Media.Brush) converter.ConvertFromString(color.Name.Insert(0, "#"));
                goto Label_0A41;
            Label_06D4:
                r.Fill = (System.Windows.Media.Brush) converter.ConvertFromString(color.Name.Insert(0, "#"));
                goto Label_0A41;
            Label_0766:
                r.Fill = (System.Windows.Media.Brush) converter.ConvertFromString(color.Name.Insert(0, "#"));
                goto Label_0A41;
            Label_07F8:
                r.Fill = (System.Windows.Media.Brush) converter.ConvertFromString(color.Name.Insert(0, "#"));
                goto Label_0A41;
            Label_088A:
                r.Fill = (System.Windows.Media.Brush) converter.ConvertFromString(color.Name.Insert(0, "#"));
                goto Label_0A41;
            Label_091C:
                r.Fill = (System.Windows.Media.Brush) converter.ConvertFromString(color.Name.Insert(0, "#"));
                goto Label_0A41;
            Label_09A8:
                r.Fill = (System.Windows.Media.Brush) converter.ConvertFromString(color.Name.Insert(0, "#"));
                goto Label_0A41;
            Label_0A1D:
                r.Fill = (System.Windows.Media.Brush) converter.ConvertFromString(color.Name.Insert(0, "#"));
            Label_0A41:
                if (number >= this._rangeList[i].High)
                {
                    r.Fill = this.btnProfit.Background;
                }
            }
        }

        private void FillColorImage()
        {
            LinearGradientBrush brush = new LinearGradientBrush {
                StartPoint = new System.Windows.Point(0.0, 0.0),
                EndPoint = new System.Windows.Point(1.0, 1.0)
            };
            brush.GradientStops.Add(new GradientStop((System.Windows.Media.Color) System.Windows.Media.ColorConverter.ConvertFromString(this.btnLoss.Background.ToString()), 0.0));
            brush.GradientStops.Add(new GradientStop((System.Windows.Media.Color) System.Windows.Media.ColorConverter.ConvertFromString(this.btnProfit.Background.ToString()), 1.0));
            this.rectangle1.Fill = brush;
        }

        /* ///WYJ fix, code from Reflector 
        public void GenerateHeatMap()
        {
            this.tmpHeatMap.Children.Clear();
            if (CS$<>9__CachedAnonymousMethodDelegate1 == null)
            {
                CS$<>9__CachedAnonymousMethodDelegate1 = new Func<Position, string>(null, (IntPtr) <GenerateHeatMap>b__0);
            }
            IEnumerable<IGrouping<string, Position>> source = this._positions.GroupBy<Position, string>(CS$<>9__CachedAnonymousMethodDelegate1);
            if (source.Count<IGrouping<string, Position>>() <= 1)
            {
                this.gbSearch.Visibility = Visibility.Hidden;
                this.btnSearch.Visibility = Visibility.Hidden;
                ComboBoxItem item = (ComboBoxItem) this.cmbView.Items[0];
                item.Content = "Trade Size";
                this.CalculateAveragesSingleStock();
                foreach (IGrouping<string, Position> grouping in source)
                {
                    foreach (Position position in grouping)
                    {
                        if (this.cmbView.SelectedIndex == 0)
                        {
                            this.SingleStock(grouping.Key, position.Bars.SecurityName, position.Shares * position.EntryPrice, position.NetProfit, position.NetProfitPercent, position.Shares);
                        }
                        if (this.cmbView.SelectedIndex == 1)
                        {
                            double netProfit = position.NetProfit;
                            if (position.NetProfit < 0.0)
                            {
                                netProfit = position.NetProfit * -1.0;
                            }
                            this.SingleStock(grouping.Key, position.Bars.SecurityName, netProfit, position.NetProfit, position.NetProfitPercent, position.Shares);
                            this.txtSize.Text = "Overall Profit or Loss for each individual trade.";
                            this.txtColor.Text = "Average Profit or Loss percentage of each individual trade.";
                        }
                    }
                }
            }
            else
            {
                this.gbSearch.Visibility = Visibility.Visible;
                this.btnSearch.Visibility = Visibility.Visible;
                ComboBoxItem item2 = (ComboBoxItem) this.cmbView.Items[0];
                item2.Content = "Trades";
                this.CalculateAveragesMultipleStock();
                foreach (IGrouping<string, Position> grouping2 in source)
                {
                    double num2 = 0.0;
                    double profit = 0.0;
                    string name = "";
                    foreach (Position position2 in grouping2)
                    {
                        num2 += position2.NetProfitPercent;
                        profit += position2.NetProfit;
                        name = position2.Bars.SecurityName;
                    }
                    if (this.cmbView.SelectedIndex == 0)
                    {
                        this.MultipleStock(grouping2.Key, name, (double) grouping2.Count<Position>(), num2 / ((double) grouping2.Count<Position>()), profit, grouping2.Count<Position>());
                    }
                    if (this.cmbView.SelectedIndex == 1)
                    {
                        if (profit < 0.0)
                        {
                            profit *= -1.0;
                        }
                        this.MultipleStock(grouping2.Key, name, profit, num2 / ((double) grouping2.Count<Position>()), profit, grouping2.Count<Position>());
                        this.txtSize.Text = "Overall Profit or Loss for each stock.";
                        this.txtColor.Text = "Average Profit or Loss percentage of all trades.";
                    }
                }
            }
        } */

        // ///WYJ fix, code for Telerik JustCompile
        public void GenerateHeatMap()
        {
            this.tmpHeatMap.Children.Clear();
            List<Position> positions = this._positions;
            IEnumerable<IGrouping<string, Position>> groupings = positions.GroupBy<Position, string>((Position p) => p.Bars.Symbol);
            if (groupings.Count<IGrouping<string, Position>>() > 1)
            {
                this.gbSearch.Visibility = Visibility.Visible;
                this.btnSearch.Visibility = Visibility.Visible;
                ComboBoxItem item = (ComboBoxItem)this.cmbView.Items[0];
                item.Content = "Trades";
                this.CalculateAveragesMultipleStock();
                foreach (IGrouping<string, Position> strs in groupings)
                {
                    double netProfitPercent = 0;
                    double netProfit = 0;
                    string securityName = "";
                    foreach (Position position in strs)
                    {
                        netProfitPercent = netProfitPercent + position.NetProfitPercent;
                        netProfit = netProfit + position.NetProfit;
                        securityName = position.Bars.SecurityName;
                    }
                    if (this.cmbView.SelectedIndex == 0)
                    {
                        this.MultipleStock(strs.Key, securityName, (double)strs.Count<Position>(), netProfitPercent / (double)strs.Count<Position>(), netProfit, strs.Count<Position>());
                    }
                    if (this.cmbView.SelectedIndex != 1)
                    {
                        continue;
                    }
                    if (netProfit < 0)
                    {
                        netProfit = netProfit * -1;
                    }
                    this.MultipleStock(strs.Key, securityName, netProfit, netProfitPercent / (double)strs.Count<Position>(), netProfit, strs.Count<Position>());
                    this.txtSize.Text = "Overall Profit or Loss for each stock.";
                    this.txtColor.Text = "Average Profit or Loss percentage of all trades.";
                }
            }
            else
            {
                this.gbSearch.Visibility = Visibility.Hidden;
                this.btnSearch.Visibility = Visibility.Hidden;
                ComboBoxItem comboBoxItem = (ComboBoxItem)this.cmbView.Items[0];
                comboBoxItem.Content = "Trade Size";
                this.CalculateAveragesSingleStock();
                foreach (IGrouping<string, Position> strs1 in groupings)
                {
                    foreach (Position position1 in strs1)
                    {
                        if (this.cmbView.SelectedIndex == 0)
                        {
                            this.SingleStock(strs1.Key, position1.Bars.SecurityName, position1.Shares * position1.EntryPrice, position1.NetProfit, position1.NetProfitPercent, position1.Shares);
                        }
                        if (this.cmbView.SelectedIndex != 1)
                        {
                            continue;
                        }
                        double num = position1.NetProfit;
                        if (position1.NetProfit < 0)
                        {
                            num = position1.NetProfit * -1;
                        }
                        this.SingleStock(strs1.Key, position1.Bars.SecurityName, num, position1.NetProfit, position1.NetProfitPercent, position1.Shares);
                        this.txtSize.Text = "Overall Profit or Loss for each individual trade.";
                        this.txtColor.Text = "Average Profit or Loss percentage of each individual trade.";
                    }
                }
            }
        }

        [DebuggerNonUserCode]
        public void InitializeComponent()
        {
            if (!this._contentLoaded)
            {
                this._contentLoaded = true;
                Uri resourceLocator = new Uri("/WealthLab.Visualizers.Extensions;component/heatmap.xaml", UriKind.Relative);
                System.Windows.Application.LoadComponent(this, resourceLocator);
            }
        }

        public void InitVariables(SystemPerformance performance, IVisualizerHost visHost)
        {
            this._performance = performance;
            this._visHost = visHost;
            this._bars = this._performance.Bars[0];
            this._positions = performance.Results.Positions.ToList<Position>();
            this._rangeList = new List<WealthLab.Visualizers.Extensions.Range>();
            this._max = double.MinValue;
            this._min = double.MaxValue;
            this.GenerateHeatMap();
            this.cmbView.SelectedIndex = 0;
            this.FillColorImage();
        }

        public void MultipleStock(string symbol, string name, double weight, double avg, double profit, int tradeCount)
        {
            System.Windows.Forms.DataObject printObject = new System.Windows.Forms.DataObject();
            this._visHost.StrategySummary(ref printObject);
            this.lbl_symbol.Content = "DataSet: " + printObject.GetData("Symbol");
            this.lbl_strategy.Content = "Strategy: " + printObject.GetData("Strategy");
            this.txtQty.Text = "Number of symbols. Each square represents a single symbol included in the backtest.";
            System.Windows.Shapes.Rectangle r = new System.Windows.Shapes.Rectangle();
            r.SetValue(TreeMaps.Controls.TreeMapsPanel.WeightProperty, weight);
            r.Tag = symbol;
            r.IsMouseDirectlyOverChanged += new DependencyPropertyChangedEventHandler(this.r_IsMouseDirectlyOverChanged);
            if (this.cmbView.SelectedIndex == 1)
            {
                if (avg < 0.0)
                {
                    r.ToolTip = symbol + " (" + name + ")\r\nAverage Profit %: " + avg.ToString("F2") + "\r\nLoss: ($" + weight.ToString("F2") + ")\r\nTrades: " + tradeCount.ToString();
                }
                else
                {
                    r.ToolTip = symbol + " (" + name + ")\r\nAverage Profit %: " + avg.ToString("F2") + "\r\nProfit: $" + weight.ToString("F2") + "\r\nTrades: " + tradeCount.ToString();
                }
            }
            else if (avg < 0.0)
            {
                r.ToolTip = string.Concat(new object[] { symbol, " (", name, ")\r\nAverage Profit %: ", avg.ToString("F2"), "\r\nLoss: ($", profit.ToString("F2"), ")\r\nTrades: ", weight });
            }
            else
            {
                r.ToolTip = string.Concat(new object[] { symbol, " (", name, ")\r\nAverage Profit %: ", avg.ToString("F2"), "\r\nProfit: $", profit.ToString("F2"), "\r\nTrades: ", weight });
            }
            this.DetermineColor(r, avg);
            this.tmpHeatMap.Children.Add(r);
        }

        private void r_IsMouseDirectlyOverChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            System.Windows.Shapes.Rectangle rectangle = (System.Windows.Shapes.Rectangle) sender;
            if (rectangle.IsMouseDirectlyOver)
            {
                rectangle.Stroke = System.Windows.Media.Brushes.Yellow;
                rectangle.StrokeDashCap = PenLineCap.Square;
                rectangle.StrokeThickness = 3.0;
            }
            else
            {
                rectangle.Stroke = null;
                rectangle.StrokeDashCap = PenLineCap.Flat;
                rectangle.StrokeThickness = 0.0;
            }
        }

        public void SingleStock(string symbol, string name, double weight, double netProfit, double netProfitPercent, double quantity)
        {
            System.Windows.Shapes.Rectangle r = new System.Windows.Shapes.Rectangle();
            System.Windows.Forms.DataObject printObject = new System.Windows.Forms.DataObject();
            this._visHost.StrategySummary(ref printObject);
            this.lbl_symbol.Content = "Symbol: " + symbol + " (" + name + ")";
            this.lbl_strategy.Content = "Strategy: " + printObject.GetData("Strategy");
            this.txtQty.Text = "Number of trades. Each square represents a single trade for the selected symbol.";
            r.SetValue(TreeMaps.Controls.TreeMapsPanel.WeightProperty, weight);
            r.IsMouseDirectlyOverChanged += new DependencyPropertyChangedEventHandler(this.r_IsMouseDirectlyOverChanged);
            if (this.cmbView.SelectedIndex == 0)
            {
                r.ToolTip = string.Concat(new object[] { symbol, " (", name, ")\r\nProfit %: ", netProfitPercent.ToString("F2"), "\r\nProfit: $", netProfit.ToString("F2"), "\r\n Size | Qty: $", weight, " | ", quantity });
            }
            if (this.cmbView.SelectedIndex == 1)
            {
                if (netProfitPercent < 0.0)
                {
                    r.ToolTip = string.Concat(new object[] { symbol, " (", name, ")\r\nProfit %: ", netProfitPercent.ToString("F2"), "\r\nLoss: ($", netProfit.ToString("F2"), ")\r\nQuantity: ", quantity });
                }
                else
                {
                    r.ToolTip = string.Concat(new object[] { symbol, " (", name, ")\r\nProfit %: ", netProfitPercent.ToString("F2"), "\r\nProfit: $", netProfit.ToString("F2"), "\r\nQuantity: ", quantity });
                }
            }
            this.DetermineColor(r, netProfitPercent);
            this.tmpHeatMap.Children.Add(r);
        }

        [EditorBrowsable(EditorBrowsableState.Never), DebuggerNonUserCode]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target)
        {
            switch (connectionId)
            {
                case 1:
                    this.gHeatMap = (Grid) target;
                    return;

                case 2:
                    this.tmpHeatMap = (TreeMaps.Controls.SquarifiedTreeMapsPanel)target;
                    return;

                case 3:
                    this.gbColorKey = (System.Windows.Controls.GroupBox) target;
                    return;

                case 4:
                    this.lblKey = (System.Windows.Controls.Label) target;
                    return;

                case 5:
                    this.gbView = (System.Windows.Controls.GroupBox) target;
                    return;

                case 6:
                    this.cmbView = (System.Windows.Controls.ComboBox) target;
                    this.cmbView.SelectionChanged += new SelectionChangedEventHandler(this.cb_view_SelectionChanged);
                    return;

                case 7:
                    this.gbSearch = (System.Windows.Controls.GroupBox) target;
                    return;

                case 8:
                    this.txtSearch = (System.Windows.Controls.TextBox) target;
                    return;

                case 9:
                    this.btnSearch = (System.Windows.Controls.Button) target;
                    this.btnSearch.Click += new RoutedEventHandler(this.btn_search_Click);
                    return;

                case 10:
                    this.statusBar1 = (System.Windows.Controls.Primitives.StatusBar) target;
                    return;

                case 11:
                    this.gbKey = (System.Windows.Controls.GroupBox) target;
                    return;

                case 12:
                    this.lblSize = (System.Windows.Controls.Label) target;
                    return;

                case 13:
                    this.lblColor = (System.Windows.Controls.Label) target;
                    return;

                case 14:
                    this.txtSize = (TextBlock) target;
                    return;

                case 15:
                    this.txtColor = (TextBlock) target;
                    return;

                case 0x10:
                    this.lblQty = (System.Windows.Controls.Label) target;
                    return;

                case 0x11:
                    this.txtQty = (TextBlock) target;
                    return;

                case 0x12:
                    this.lbl_symbol = (System.Windows.Controls.Label) target;
                    return;

                case 0x13:
                    this.lbl_strategy = (System.Windows.Controls.Label) target;
                    return;

                case 20:
                    this.rectangle1 = (System.Windows.Shapes.Rectangle) target;
                    return;

                case 0x15:
                    this.btnProfit = (System.Windows.Controls.Button) target;
                    this.btnProfit.Click += new RoutedEventHandler(this.button1_Click);
                    return;

                case 0x16:
                    this.btnLoss = (System.Windows.Controls.Button) target;
                    this.btnLoss.Click += new RoutedEventHandler(this.button2_Click);
                    return;

                case 0x17:
                    this.label1 = (System.Windows.Controls.Label) target;
                    return;

                case 0x18:
                    this.label2 = (System.Windows.Controls.Label) target;
                    return;
            }
            this._contentLoaded = true;
        }

        public System.Windows.Controls.GroupBox GroupKey
        {
            get
            {
                return this.gbKey;
            }
            set
            {
                this.gbKey = value;
            }
        }
    }
}

