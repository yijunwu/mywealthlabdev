namespace Steema.TeeChart
{
    using Microsoft.Win32;
    using Steema.TeeChart.Editors;
    using Steema.TeeChart.Styles;
    using Steema.TeeChart.Tools;
    using Steema.TeeChart.Functions;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Text;
    using System.Globalization;
    using System.IO;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public sealed class Utils
    {
        private static int[] aFunctionGalleryPage = new int[] { 
            0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 2, 2, 3, 0, 2, 2, 
            1, 1, 2, 3, 1, 1, 1, 1, 1, 1, 3, 3, 3, 3, 3, 3, 
            1, 1, 1, 1, 0, 0, 3, 2, 1
         };
        private static System.Type[] aFunctionTypesOf = new System.Type[] { 
            typeof(Add), typeof(Subtract), typeof(Multiply), typeof(Divide), typeof(High), typeof(Low), typeof(Average), typeof(Count), typeof(Momentum), typeof(MomentumDivision), typeof(Cumulative), typeof(ExpAverage), typeof(Smoothing), typeof(Steema.TeeChart.Functions.Custom), typeof(RootMeanSquare), typeof(StdDeviation), 
            typeof(Stochastic), typeof(ExpMovAverage), typeof(Performance), typeof(CrossPoints), typeof(CompressOHLC), typeof(CLVFunction), typeof(OBVFunction), typeof(CCIFunction), typeof(MovingAverage), typeof(PVOFunction), typeof(DownSampling), typeof(TrendFunction), typeof(CorrelationFunction), typeof(VarianceFunction), typeof(PerimeterFunction), typeof(PolyFitting), 
            typeof(Bollinger), typeof(MACDFunction), typeof(RSIFunction), typeof(ADXFunction), typeof(MedianFunction), typeof(ModeFunction), typeof(ExpTrendFunction), typeof(HistogramFunction), typeof(SARFunction)
         };
        private static int[] aSeriesGalleryCount = new int[] { 
            2, 2, 2, 2, 2, 2, 2, 1, 3, 1, 1, 1, 1, 1, 1, 2, 
            1, 2, 1, 2, 1, 1, 1, 1, 1, 2, 1, 1, 1, 2, 2, 1, 
            2, 2, 1, 1, 1, 1, 1, 1, 2, 1, 1, 1, 1, 1, 1, 2, 
            1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1
         };
        private static int[] aSeriesGalleryPage = new int[] { 
            0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 1, 3, 1, 7, 
            4, 3, 3, 3, 7, 7, 3, 4, 7, 7, 4, 4, 2, 2, 2, 4, 
            3, 3, 7, 2, 4, 2, 2, 2, 0, 4, 1, 5, 4, 2, 3, 7, 
            1, 1, 4, 1, 2, 3, 5, 5, 5, 5, 3, 7, 4
         };
        private static System.Type[] aSeriesTypesOf = new System.Type[] { 
            typeof(Line), typeof(Points), typeof(Area), typeof(FastLine), typeof(HorizLine), typeof(Bar), typeof(HorizBar), typeof(Pie), typeof(Shape), typeof(Arrow), typeof(Bubble), typeof(Gantt), typeof(Candle), typeof(Donut), typeof(Volume), typeof(Bar3D), 
            typeof(Points3D), typeof(Polar), typeof(PolarBar), typeof(Radar), typeof(Clock), typeof(WindRose), typeof(Pyramid), typeof(Surface), typeof(LinePoint), typeof(BarJoin), typeof(ColorGrid), typeof(Waterfall), typeof(Histogram), typeof(Error), typeof(ErrorBar), typeof(Contour), 
            typeof(Smith), typeof(Bezier), typeof(Steema.TeeChart.Styles.Calendar), typeof(HighLow), typeof(TriSurface), typeof(Funnel), typeof(Box), typeof(HorizBox), typeof(HorizArea), typeof(Tower), typeof(PointFigure), typeof(Gauges), typeof(Vector3D), typeof(HorizHistogram), typeof(Map), typeof(ImageBar), 
            typeof(Kagi), typeof(Renko), typeof(IsoSurface), typeof(Darvas), typeof(VolumePipe), typeof(ImagePoint), typeof(CircularGauge), typeof(LinearGauge), typeof(VerticalLinearGauge), typeof(NumericGauge), typeof(OrgSeries), typeof(TagCloud), typeof(PolarGrid)
         };
        private static System.Type[] aToolTypesOf = new System.Type[] { 
            typeof(Annotation), typeof(ChartImage), typeof(ExtraLegend), typeof(GridBand), typeof(GridTranspose), typeof(MarksTip), typeof(NearestPoint), typeof(PageNumber), typeof(PieTool), typeof(Rotate), typeof(LegendScrollBar), typeof(SeriesAnimation), typeof(SurfaceNearestTool), typeof(CursorTool), typeof(DragMarks), typeof(AxisArrow), 
            typeof(ColorLine), typeof(ColorBand), typeof(DrawLine), typeof(DragPoint), typeof(GanttTool), typeof(AxisScroll), typeof(SeriesHotspot), typeof(ZoomTool), typeof(ScrollTool), typeof(LightTool), typeof(FibonacciTool), typeof(SubChartTool), typeof(Marker), typeof(FaderTool), typeof(RectangleTool), typeof(Selector), 
            typeof(SeriesRegionTool), typeof(LegendPalette), typeof(SeriesStats), typeof(SeriesTranspose), typeof(DataTableTool), typeof(ClipSeries), typeof(BannerTool), typeof(Magnify), typeof(SeriesBandTool)
         };
        private static Bitmap bmp = null;
        public static double[] DateTimeStep = new double[] { 
            1.1574074074074074E-11, 1.1574074074074074E-08, 1.1574074074074073E-05, 5.7870370370370373E-05, 0.00011574074074074075, 0.00017361111111111112, 0.00034722222222222224, 0.00069444444444444447, 0.003472222222222222, 0.0069444444444444441, 0.010416666666666666, 0.020833333333333332, 0.041666666666666664, 0.083333333333333329, 0.25, 0.5, 
            1.0, 2.0, 3.0, 7.0, 15.0, 30.0, 60.0, 90.0, 120.0, 182.0, 365.0, 1.0
         };
        public static int DefaultNodeHeight;
        private const int extended = 3;
        private const int financial = 1;
        public static ArrayList FunctionGalleryPage = new ArrayList(aFunctionGalleryPage);
        public static ArrayList FunctionTypesOf = new ArrayList(aFunctionTypesOf);
        private const int gauges = 5;
        private const int maps = 6;
        private const int other = 7;
        public static double PiStep = 0.017453292519943295;
        private static PrivateFontCollection privateFonts;
        public static List<int> SeriesGalleryCount = new List<int>(aSeriesGalleryCount);
        public static List<int> SeriesGalleryPage = new List<int>(aSeriesGalleryPage);
        public static List<System.Type> SeriesTypesOf = new List<System.Type>(aSeriesTypesOf);
        private const int standard = 0;
        private const int stats = 2;
        public static string TeeChartKeyName = (TeeChartRoot + @"\" + TeeChartSubKey);
        public static string TeeChartRoot = "HKEY_LOCAL_MACHINE";
        public static string TeeChartSubKey = @"Software\Steema Software\TeeChart.NET";
        private const int threed = 4;
        public static List<System.Type> ToolTypesOf = new List<System.Type>(aToolTypesOf);

        private Utils()
        {
        }

        public static bool AutoLanguage()
        {
            object obj2 = Registry_GetValue(TeeChartKeyName, "AutoLanguage", "False");
            return ((obj2 != null) && bool.Parse(obj2.ToString()));
        }

        public static System.Drawing.Color CalcColorBlend(System.Drawing.Color start, System.Drawing.Color end, int percentage)
        {
            System.Drawing.Color color = start;
            Rectangle rect = new Rectangle(0, 0, 100, 1);
            LinearGradientBrush brush = new LinearGradientBrush(rect, start, end, LinearGradientMode.Horizontal);
            if (bmp == null)
            {
                bmp = new Bitmap(100, 1);
            }
            Graphics.FromImage(bmp).FillRectangle(brush, rect);
            return bmp.GetPixel(percentage, 0);
        }

        public static string[] ColorArrayToHexArray(System.Drawing.Color[] Colors)
        {
            string[] strArray = new string[Colors.Length];
            for (int i = 0; i < Colors.Length; i++)
            {
                strArray[i] = ColorToHex(Colors[i]);
            }
            return strArray;
        }

        public static bool ColorIsEmpty(System.Drawing.Color color)
        {
            if (!color.IsEmpty)
            {
                return color.Equals(System.Drawing.Color.FromArgb(0, 0, 0, 0));
            }
            return true;
        }

        public static string ColorToHex(System.Drawing.Color color)
        {
            return ColorToHex(color, true);
        }

        public static string ColorToHex(System.Drawing.Color color, bool ARGB)
        {
            string str = color.A.ToString("X2");
            string str2 = color.R.ToString("X2");
            string str3 = color.G.ToString("X2");
            string str4 = color.B.ToString("X2");
            string[] strArray = null;
            if (ARGB)
            {
                strArray = new string[] { str, str2, str3, str4 };
            }
            else
            {
                strArray = new string[] { str2, str3, str4 };
            }
            return string.Concat(strArray);
        }

        private static TOutput[] ConvertAll<TInput, TOutput>(TInput[] array, Converter<TInput, TOutput> converter)
        {
            return Array.ConvertAll<TInput, TOutput>(array, converter);
        }

        public static Array CopyToDoubleNullable(Array array)
        {
            Array array2 = null;
            if (array is double[])
            {
                return ConvertAll<double, double?>((double[]) array, new Converter<double, double?>(Utils.DoubleToDoubleNullable));
            }
            if (array is int[])
            {
                return ConvertAll<int, double?>((int[]) array, new Converter<int, double?>(Utils.Int32ToDoubleNullable));
            }
            if (array is int?[])
            {
                return ConvertAll<int?, double?>((int?[]) array, new Converter<int?, double?>(Utils.Int32NullableToDoubleNullable));
            }
            if (array is short[])
            {
                return ConvertAll<short, double?>((short[]) array, new Converter<short, double?>(Utils.Int16ToDoubleNullable));
            }
            if (array is short?[])
            {
                return ConvertAll<short?, double?>((short?[]) array, new Converter<short?, double?>(Utils.Int16NullableToDoubleNullable));
            }
            if (array is float[])
            {
                return ConvertAll<float, double?>((float[]) array, new Converter<float, double?>(Utils.FloatToDoubleNullable));
            }
            if (array is float?[])
            {
                return ConvertAll<float?, double?>((float?[]) array, new Converter<float?, double?>(Utils.FloatNullableToDoubleNullable));
            }
            if (array is decimal[])
            {
                return ConvertAll<decimal, double?>((decimal[]) array, new Converter<decimal, double?>(Utils.DecimalToDoubleNullable));
            }
            if (array is decimal?[])
            {
                array2 = ConvertAll<decimal?, double?>((decimal?[]) array, new Converter<decimal?, double?>(Utils.DecimalNullableToDoubleNullable));
            }
            return array2;
        }

        public static System.Drawing.Color DarkenColor(System.Drawing.Color AColor, int Percentage)
        {
            int red = ReduceByte(AColor.R, Percentage);
            int green = ReduceByte(AColor.G, Percentage);
            int blue = ReduceByte(AColor.B, Percentage);
            return System.Drawing.Color.FromArgb(AColor.A, red, green, blue);
        }

        public static double DateTime(System.DateTime value)
        {
            return value.ToOADate();
        }

        public static System.DateTime DateTime(double value)
        {
            return System.DateTime.FromOADate(value);
        }

        public static System.DateTime DateTime(double? value, double defaultNull)
        {
            return System.DateTime.FromOADate(value.GetValueOrDefault(defaultNull));
        }

        public static string DateTimeDefaultFormat(double tmpValue)
        {
            return DateTimeToStr(DateTime(tmpValue));
        }

        public static string DateTimeToStr(System.DateTime datetime)
        {
            return datetime.ToShortDateString();
        }

        public static string DateTimeToStr(double datetime)
        {
            return DateTime(datetime).ToShortDateString();
        }

        public static double? DecimalNullableToDoubleNullable(decimal? d)
        {
            decimal? nullable = d;
            if (!nullable.HasValue)
            {
                return null;
            }
            return new double?((double) nullable.GetValueOrDefault());
        }

        public static double? DecimalToDoubleNullable(decimal d)
        {
            return new double?((double) d);
        }

        internal static string DesignKeyV3()
        {
            return getRegistryString("DesignKeyV3", "");
        }

        public static string DesignTime()
        {
            return getRegistryString("DesignTime", "");
        }

        public static double? DoubleToDoubleNullable(double d)
        {
            return new double?(d);
        }

        public static void DrawCheckBox(int x, int y, Graphics g, bool drawChecked, System.Drawing.Color backColor, bool CheckBox)
        {
            ButtonState state = drawChecked ? ButtonState.Checked : ButtonState.Normal;
            if (CheckBox)
            {
                ControlPaint.DrawCheckBox(g, x, y, 14, 14, state | ButtonState.Flat);
            }
            else
            {
                ControlPaint.DrawRadioButton(g, x, y, 14, 14, state | ButtonState.Flat);
            }
        }

        public static void EnableControls(bool enable, Control[] controls)
        {
            foreach (Control control in controls)
            {
                control.Enabled = enable;
            }
        }

        public static bool ErrorMessage(string s)
        {
            return (MessageBox.Show(s, Texts.TeeChartExceptionString, MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1) == DialogResult.Yes);
        }

        public static double? FloatNullableToDoubleNullable(float? d)
        {
            float? nullable = d;
            if (!nullable.HasValue)
            {
                return null;
            }
            return new double?((double) nullable.GetValueOrDefault());
        }

        public static double? FloatToDoubleNullable(float d)
        {
            return new double?((double) d);
        }

        public static string FormatFloat(string format, double value)
        {
            return value.ToString(format);
        }

        public static System.Drawing.Color FromArgb(int alpha, System.Drawing.Color baseColor)
        {
            return System.Drawing.Color.FromArgb(alpha, baseColor);
        }

        public static System.Drawing.Color FromArgb(int r, int g, int b)
        {
            return System.Drawing.Color.FromArgb(r, g, b);
        }

        public static Rectangle FromLTRB(int left, int top, int right, int bottom)
        {
            return Rectangle.FromLTRB(left, top, right, bottom);
        }

        public static bool GalleryByType()
        {
            return (Convert.ToInt32(GalleryStyle()) == 1);
        }

        public static int GalleryStyle()
        {
            object obj2 = Registry_GetValue(TeeChartKeyName, "GalleryStyle", 0);
            if (obj2 != null)
            {
                return (int) obj2;
            }
            return 0;
        }

        internal static Image GetBitmapResource(string name)
        {
            Stream resource = GetResource(name);
            if (resource != null)
            {
                return new Bitmap(resource);
            }
            return null;
        }

        public static double GetDateTimeStep(DateTimeSteps value)
        {
            return DateTimeStep[(int) value];
        }

        internal static Icon GetIconResource(string name)
        {
            Stream resource = GetResource(name);
            if (resource != null)
            {
                return new Icon(resource);
            }
            return null;
        }

        public static MouseButtons GetMouseButton(MouseEventArgs e)
        {
            return e.Button;
        }

        private static string getRegistryString(string value, string defValue)
        {
            object obj2 = Registry_GetValue(TeeChartKeyName, value, defValue);
            if (obj2 != null)
            {
                return obj2.ToString();
            }
            return defValue;
        }

        internal static Stream GetResource(string name)
        {
            return Assembly.GetExecutingAssembly().GetManifestResourceStream(name);
        }

        public static System.Drawing.Color[] HexArrayToColorArray(string[] Colors)
        {
            System.Drawing.Color[] colorArray = new System.Drawing.Color[Colors.Length];
            for (int i = 0; i < Colors.Length; i++)
            {
                colorArray[i] = HexToColor(Colors[i]);
            }
            return colorArray;
        }

        public static System.Drawing.Color HexToColor(string color)
        {
            int alpha = int.Parse(color.Substring(0, 2), NumberStyles.AllowHexSpecifier);
            int red = int.Parse(color.Substring(2, 2), NumberStyles.AllowHexSpecifier);
            int green = int.Parse(color.Substring(4, 2), NumberStyles.AllowHexSpecifier);
            int blue = int.Parse(color.Substring(6, 2), NumberStyles.AllowHexSpecifier);
            return System.Drawing.Color.FromArgb(alpha, red, green, blue);
        }

        public static double Int(double d)
        {
            if (d > 0.0)
            {
                return Math.Floor(d);
            }
            return Math.Ceiling(d);
        }

        public static double? Int16NullableToDoubleNullable(short? d)
        {
            short? nullable = d;
            if (!nullable.HasValue)
            {
                return null;
            }
            return new double?((double) nullable.GetValueOrDefault());
        }

        public static double? Int16ToDoubleNullable(short d)
        {
            return new double?((double) d);
        }

        public static double? Int32NullableToDoubleNullable(int? d)
        {
            int? nullable = d;
            if (!nullable.HasValue)
            {
                return null;
            }
            return new double?((double) nullable.GetValueOrDefault());
        }

        public static double? Int32ToDoubleNullable(int d)
        {
            return new double?((double) d);
        }

        public static bool IsNullOrEmpty(string s)
        {
            return string.IsNullOrEmpty(s);
        }

        public static bool IsPrivateFont(string name)
        {
            FontFamily privateFontFamily = null;
            return IsPrivateFont(name, ref privateFontFamily);
        }

        public static bool IsPrivateFont(string name, ref FontFamily privateFontFamily)
        {
            bool flag = false;
            privateFontFamily = null;
            for (int i = 0; i < PrivateFonts.Families.Length; i++)
            {
                if (PrivateFonts.Families[i].Name == name)
                {
                    flag = true;
                    privateFontFamily = PrivateFonts.Families[i];
                    return flag;
                }
            }
            return flag;
        }

        public static int Language()
        {
            object obj2 = Registry_GetValue(TeeChartKeyName, "Language", 0);
            if (obj2 != null)
            {
                return (int) obj2;
            }
            return 0;
        }

        private static void LoadFont(Stream fontStream)
        {
            IntPtr destination = Marshal.AllocCoTaskMem((int) fontStream.Length);
            byte[] buffer = new byte[fontStream.Length];
            fontStream.Read(buffer, 0, (int) fontStream.Length);
            Marshal.Copy(buffer, 0, destination, (int) fontStream.Length);
            privateFonts.AddMemoryFont(destination, (int) fontStream.Length);
            fontStream.Close();
            Marshal.FreeCoTaskMem(destination);
        }

        public static int MulDiv(int Number, int Numerator, int Denominator)
        {
            if (Denominator == 0)
            {
                return -1;
            }
            return ((Number * Numerator) / Denominator);
        }

        public static bool Odd(int value)
        {
            return ((value % 2) != 0);
        }

        public static Point PointAtDistance(Point AFrom, Point ATo, int ADist)
        {
            Point point = new Point(ATo.X, ATo.Y);
            if (AFrom.X != ATo.X)
            {
                double num;
                double num2;
                SinCos(Math.Atan2((double) (ATo.Y - AFrom.Y), (double) (ATo.X - AFrom.X)), out num, out num2);
                point.X -= Round((double) (ADist * num2));
                point.Y -= Round((double) (ADist * num));
                return point;
            }
            if (ATo.Y < AFrom.Y)
            {
                point.Y += ADist;
                return point;
            }
            point.Y -= ADist;
            return point;
        }

        private static void PrivateSort(int l, int r, CompareEventHandler Compare, SwapEventHandler Swap)
        {
            int a = l;
            int b = r;
            int num3 = (a + b) / 2;
            while (a < b)
            {
                while (Compare(a, num3) < 0)
                {
                    a++;
                }
                while (Compare(num3, b) < 0)
                {
                    b--;
                }
                if (a < b)
                {
                    Swap(a, b);
                    if (a == num3)
                    {
                        num3 = b;
                    }
                    else if (b == num3)
                    {
                        num3 = a;
                    }
                }
                if (a <= b)
                {
                    a++;
                    b--;
                }
            }
            if (l < b)
            {
                PrivateSort(l, b, Compare, Swap);
            }
            if (a < r)
            {
                PrivateSort(a, r, Compare, Swap);
            }
        }

        private static int ReduceByte(byte Value, int APercentage)
        {
            int num = Value;
            if (Value != 0)
            {
                num = Convert.ToInt32((double) (Value * (((double) APercentage) / 100.0)));
            }
            return num;
        }

        public static void RegisterFunction(System.Type functionType, int galleryPage)
        {
            FunctionTypesOf.Add(functionType);
            FunctionGalleryPage.Add(galleryPage);
        }

        public static void RegisterSeries(System.Type seriesType, System.Type editorType, int galleryPage, int galleryCount)
        {
            SeriesTypesOf.Add(seriesType);
            SeriesGalleryPage.Add(galleryPage);
            SeriesGalleryCount.Add(galleryCount);
            EditorUtils.SeriesEditorsOf.Add(editorType);
        }

        public static void RegisterTool(System.Type toolType, System.Type editorType)
        {
            ToolTypesOf.Add(toolType);
            EditorUtils.ToolEditorsOf.Add(editorType);
        }

        public static object Registry_GetValue(string keyName, string valueName, object defaultValue)
        {
            return Registry.GetValue(keyName, valueName, defaultValue);
        }

        public static void Registry_SetValue(string keyName, string valueName, object value)
        {
            Registry.SetValue(keyName, valueName, value);
        }

        public static bool RememberEditor()
        {
            object obj2 = Registry_GetValue(TeeChartKeyName, "RememberEditor", "True");
            if (obj2 != null)
            {
                return bool.Parse(obj2.ToString());
            }
            return true;
        }

        public static int Round(double value)
        {
            return (int) Math.Round(value);
        }

        public static int Round(float value)
        {
            return (int) Math.Round((double) value);
        }

        public static int SeriesTypesIndex(Series s)
        {
            return SeriesTypesIndex(s.GetType());
        }

        public static int SeriesTypesIndex(System.Type seriesType)
        {
            for (int i = 0; i < SeriesTypesCount; i++)
            {
                if (SeriesTypesOf[i] == seriesType)
                {
                    return i;
                }
            }
            return -1;
        }

        public static Array SetLength(Array S, int NewLength, System.Type ElementType)
        {
            if ((S == null) || (S.Length == 0))
            {
                S = Array.CreateInstance(ElementType, NewLength);
                return S;
            }
            Array array = Array.CreateInstance(ElementType, S.Length);
            S.CopyTo(array, 0);
            S = Array.CreateInstance(ElementType, NewLength);
            Array.Copy(array, 0, S, 0, (NewLength <= array.Length) ? NewLength : array.Length);
            return S;
        }

        public static string ShareFolder()
        {
            return getRegistryString("ShareFolder", "");
        }

        public static void SinCos(double angle, out double resultSin, out double resultCos)
        {
            resultSin = Math.Sin(angle);
            resultCos = Math.Cos(angle);
        }

        public static void Sort(int startIndex, int endIndex, CompareEventHandler compareFunction, SwapEventHandler swap)
        {
            PrivateSort(startIndex, endIndex, compareFunction, swap);
        }

        public static double Sqr(double value)
        {
            return (value * value);
        }

        public static double StringToDouble(string text, double value)
        {
            if (text.Length == 0)
            {
                return value;
            }
            try
            {
                return double.Parse(text);
            }
            catch (ArgumentNullException)
            {
                return value;
            }
            catch (FormatException)
            {
                return value;
            }
            catch (OverflowException)
            {
                return value;
            }
        }

        public static int StringToInt(string text, int value)
        {
            if (text.Length == 0)
            {
                return value;
            }
            try
            {
                return int.Parse(text);
            }
            catch (ArgumentNullException)
            {
                return value;
            }
            catch (FormatException)
            {
                return value;
            }
            catch (OverflowException)
            {
                return value;
            }
        }

        public static void SwapDouble(ref double a, ref double b)
        {
            double num = a;
            a = b;
            b = num;
        }

        public static void SwapFloat(ref float a, ref float b)
        {
            float num = a;
            a = b;
            b = num;
        }

        public static void SwapInteger(ref int a, ref int b)
        {
            int num = a;
            a = b;
            b = num;
        }

        public static double TeeDistance(double x, double y)
        {
            return Math.Sqrt(Sqr(x) + Sqr(y));
        }

        public static string ThemeFolder()
        {
            return getRegistryString("ThemeFolder", "");
        }

        public static string TimeToStr(System.DateTime datetime)
        {
            return datetime.ToShortTimeString();
        }

        public static string TimeToStr(double datetime)
        {
            return DateTime(datetime).ToShortTimeString();
        }

        public static int ToolTypeIndex(Steema.TeeChart.Tools.Tool tool)
        {
            for (int i = 0; i < ToolTypesCount; i++)
            {
                if (ToolTypesOf[i] == tool.GetType())
                {
                    return i;
                }
            }
            return -1;
        }

        public static int ToolTypeIndex(System.Type tool)
        {
            for (int i = 0; i < ToolTypesCount; i++)
            {
                if (ToolTypesOf[i].Equals(tool))
                {
                    return i;
                }
            }
            return -1;
        }

        public static Rectangle ToRectangle(RectangleF rect)
        {
            return Rectangle.FromLTRB(Round(rect.Left), Round(rect.Top), Round(rect.Right), Round(rect.Bottom));
        }

        public static void UnRegisterSeries(System.Type seriesType)
        {
            int index = SeriesTypesOf.IndexOf(seriesType);
            if (index != -1)
            {
                SeriesTypesOf.RemoveAt(index);
                SeriesGalleryPage.RemoveAt(index);
                SeriesGalleryCount.RemoveAt(index);
                EditorUtils.SeriesEditorsOf.RemoveAt(index);
            }
        }

        public static string VirtualShare()
        {
            return getRegistryString("VirtualShare", "");
        }

        public static bool YesNo(string s)
        {
            return (MessageBox.Show(s, Texts.Confirm, MessageBoxButtons.YesNo) == DialogResult.Yes);
        }

        public static bool YesNoDelete(string s)
        {
            return YesNo(string.Format(Texts.SureToDelete, s));
        }

        public static System.Drawing.Color EmptyColor
        {
            get
            {
                return System.Drawing.Color.Empty;
            }
        }

        public static int FunctionTypesCount
        {
            get
            {
                return FunctionTypesOf.Count;
            }
        }

        public static PrivateFontCollection PrivateFonts
        {
            get
            {
                if (privateFonts == null)
                {
                    privateFonts = new PrivateFontCollection();
                    LoadFont(Assembly.GetExecutingAssembly().GetManifestResourceStream("Steema.TeeChart.Drawing.PrivateFonts.LCD-FONT.TTF"));
                    LoadFont(Assembly.GetExecutingAssembly().GetManifestResourceStream("Steema.TeeChart.Drawing.PrivateFonts.LED-FONT.TTF"));
                }
                return privateFonts;
            }
            set
            {
                privateFonts = value;
            }
        }

        public static int SeriesTypesCount
        {
            get
            {
                return SeriesTypesOf.Count;
            }
        }

        public static int ToolTypesCount
        {
            get
            {
                return ToolTypesOf.Count;
            }
        }

        public delegate int CompareEventHandler(int a, int b);

        public delegate void SwapEventHandler(int a, int b);
    }
}

