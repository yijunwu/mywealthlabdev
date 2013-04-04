namespace Steema.TeeChart.Themes
{
    using System;
    using System.IO;
    using System.Xml;
    using System.Xml.Serialization;

    [XmlRoot("ThemeProperties", Namespace="http://www.steema.com", IsNullable=false)]
    public class ThemeProperties : IThemeProperties
    {
        private string aspectSmoothingMode;
        private string aspectTextRenderingHint;
        private bool axesBottomGridCentered;
        private bool axesBottomGridVisible;
        private bool axesTopGridVisible;
        private string axisAxisPenColor;
        private int axisAxisPenWidth;
        private bool axisGridCentered;
        private string axisGridColor;
        private string axisGridStyle;
        private bool axisGridVisible;
        private int axisGridWidth;
        private bool axisLabelsFontBold;
        private string axisLabelsFontColor;
        private string axisLabelsFontName;
        private int axisLabelsFontSize;
        private string axisMinorGridColor;
        private bool axisMinorGridVisible;
        private int axisMinorTickCount;
        private int axisMinorTicksLength;
        private bool axisMinorTicksVisible;
        private string axisTicksColor;
        private string axisTicksInnerColor;
        private int axisTicksInnerLength;
        private bool axisTicksInnerVisible;
        private int axisTicksLength;
        private string axisTitleFontColor;
        private string axisTitleFontName;
        private string base64;
        private string headerColor;
        private bool headerFontBold;
        private string headerFontColor;
        private string headerFontName;
        private int headerFontSize;
        private string headerGradientDirection;
        private string headerGradientEndColor;
        private string headerGradientMiddleColor;
        private bool headerGradientSigma;
        private float headerGradientSigmaFocus;
        private float headerGradientSigmaScale;
        private string headerGradientStartColor;
        private bool headerGradientVisible;
        private string headerPenColor;
        private bool headerPenVisible;
        private int headerPenWidth;
        private int headerShadowSize;
        private int headerShadowTransparency;
        private int headerTransparency;
        private static ThemeProperties instance;
        private string legendDividingLinesColor;
        private bool legendDividingLinesVisible;
        private string legendFontColor;
        private string legendFontName;
        private int legendFontSize;
        private string legendGradientDirection;
        private string legendGradientEndColor;
        private string legendGradientMiddleColor;
        private bool legendGradientSigma;
        private float legendGradientSigmaFocus;
        private float legendGradientSigmaScale;
        private string legendGradientStartColor;
        private bool legendGradientVisible;
        private string legendPenColor;
        private string legendPenStyle;
        private bool legendPenVisible;
        private int legendPenWidth;
        private string legendShadowColor;
        private int legendShadowHeight;
        private int legendShadowTransparency;
        private int legendShadowWidth;
        private bool legendSymbolDefaultPen;
        private bool legendSymbolPenVisible;
        private bool legendSymbolSquared;
        private bool legendTransparent;
        private string[] palette;
        private string panelBevelInner;
        private string panelBevelOuter;
        private int panelBevelWidth;
        private int panelBorderRound;
        private string panelColor;
        private string panelGradientDirection;
        private string panelGradientEndColor;
        private string panelGradientMiddleColor;
        private bool panelGradientSigma;
        private float panelGradientSigmaFocus;
        private float panelGradientSigmaScale;
        private string panelGradientStartColor;
        private bool panelGradientVisible;
        private string panelPenColor;
        private string panelPenStyle;
        private bool panelPenVisible;
        private int panelPenWidth;
        private string panelShadowColor;
        private int panelShadowSize;
        private bool panelShadowVisible;
        private string seriesMarksArrowColor;
        private string seriesMarksFontColor;
        private string seriesMarksFontName;
        private int seriesMarksFontSize;
        private string seriesMarksGradientDirection;
        private string seriesMarksGradientEndColor;
        private string seriesMarksGradientMiddleColor;
        private bool seriesMarksGradientSigma;
        private float seriesMarksGradientSigmaFocus;
        private float seriesMarksGradientSigmaScale;
        private string seriesMarksGradientStartColor;
        private bool seriesMarksGradientVisible;
        private bool seriesMarksTransparent;
        private static object syncRoot = new object();
        private bool wallApplyDark;
        private string wallGradientDirection;
        private string wallGradientEndColor;
        private string wallGradientMiddleColor;
        private bool wallGradientSigma;
        private float wallGradientSigmaFocus;
        private float wallGradientSigmaScale;
        private string wallGradientStartColor;
        private bool wallGradientVisible;
        private string wallPenColor;
        private string wallPenStyle;
        private bool wallPenVisible;
        private int wallPenWidth;
        private string wallsBackColor;
        private string wallsBackGradientDirection;
        private string wallsBackGradientEndColor;
        private string wallsBackGradientMiddleColor;
        private bool wallsBackGradientSigma;
        private float wallsBackGradientSigmaFocus;
        private float wallsBackGradientSigmaScale;
        private string wallsBackGradientStartColor;
        private bool wallsBackGradientVisible;
        private bool wallsBackTransparent;
        private string wallsBottomColor;
        private int wallSize;
        private string wallsLeftColor;
        private string wallsRightColor;

        public void Read(Stream stream)
        {
            this.ReadTheme(stream);
        }

        public void Read(string fileName)
        {
            this.ReadTheme(fileName);
        }

        private void ReadTheme(object FileOrStream)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ThemeProperties));
            serializer.UnknownNode += new XmlNodeEventHandler(this.serializer_UnknownNode);
            serializer.UnknownAttribute += new XmlAttributeEventHandler(this.serializer_UnknownAttribute);
            if (FileOrStream is string)
            {
                FileStream stream = new FileStream(FileOrStream as string, FileMode.Open) {
                    Position = 0L
                };
                instance = (ThemeProperties) serializer.Deserialize(stream);
                stream.Close();
            }
            else if (FileOrStream is Stream)
            {
                if ((FileOrStream as Stream).CanRead)
                {
                    (FileOrStream as Stream).Position = 0L;
                }
                instance = (ThemeProperties) serializer.Deserialize(FileOrStream as Stream);
            }
        }

        protected void serializer_UnknownAttribute(object sender, XmlAttributeEventArgs e)
        {
            XmlAttribute attr = e.Attr;
            Console.WriteLine("Unknown attribute " + attr.Name + "='" + attr.Value + "'");
        }

        protected void serializer_UnknownNode(object sender, XmlNodeEventArgs e)
        {
            Console.WriteLine("Unknown Node:" + e.Name + "\t" + e.Text);
        }

        public void Write(Stream stream)
        {
            this.WriteTheme(stream);
        }

        public void Write(string fileName)
        {
            this.WriteTheme(fileName);
        }

        private void WriteTheme(object FileOrStream)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ThemeProperties));
            TextWriter textWriter = null;
            if (FileOrStream is string)
            {
                textWriter = new StreamWriter(FileOrStream as string);
            }
            else if (FileOrStream is Stream)
            {
                textWriter = new StreamWriter(FileOrStream as Stream);
            }
            serializer.Serialize(textWriter, Instance);
        }

        public string AspectSmoothingMode
        {
            get
            {
                return this.aspectSmoothingMode;
            }
            set
            {
                this.aspectSmoothingMode = value;
            }
        }

        public string AspectTextRenderingHint
        {
            get
            {
                return this.aspectTextRenderingHint;
            }
            set
            {
                this.aspectTextRenderingHint = value;
            }
        }

        public bool AxesBottomGridCentered
        {
            get
            {
                return this.axesBottomGridCentered;
            }
            set
            {
                this.axesBottomGridCentered = value;
            }
        }

        public bool AxesBottomGridVisible
        {
            get
            {
                return this.axesBottomGridVisible;
            }
            set
            {
                this.axesBottomGridVisible = value;
            }
        }

        public bool AxesTopGridVisible
        {
            get
            {
                return this.axesTopGridVisible;
            }
            set
            {
                this.axesTopGridVisible = value;
            }
        }

        public string AxisAxisPenColor
        {
            get
            {
                return this.axisAxisPenColor;
            }
            set
            {
                this.axisAxisPenColor = value;
            }
        }

        public int AxisAxisPenWidth
        {
            get
            {
                return this.axisAxisPenWidth;
            }
            set
            {
                this.axisAxisPenWidth = value;
            }
        }

        public bool AxisGridCentered
        {
            get
            {
                return this.axisGridCentered;
            }
            set
            {
                this.axisGridCentered = value;
            }
        }

        public string AxisGridColor
        {
            get
            {
                return this.axisGridColor;
            }
            set
            {
                this.axisGridColor = value;
            }
        }

        public string AxisGridStyle
        {
            get
            {
                return this.axisGridStyle;
            }
            set
            {
                this.axisGridStyle = value;
            }
        }

        public bool AxisGridVisible
        {
            get
            {
                return this.axisGridVisible;
            }
            set
            {
                this.axisGridVisible = value;
            }
        }

        public int AxisGridWidth
        {
            get
            {
                return this.axisGridWidth;
            }
            set
            {
                this.axisGridWidth = value;
            }
        }

        public bool AxisLabelsFontBold
        {
            get
            {
                return this.axisLabelsFontBold;
            }
            set
            {
                this.axisLabelsFontBold = value;
            }
        }

        public string AxisLabelsFontColor
        {
            get
            {
                return this.axisLabelsFontColor;
            }
            set
            {
                this.axisLabelsFontColor = value;
            }
        }

        public string AxisLabelsFontName
        {
            get
            {
                return this.axisLabelsFontName;
            }
            set
            {
                this.axisLabelsFontName = value;
            }
        }

        public int AxisLabelsFontSize
        {
            get
            {
                return this.axisLabelsFontSize;
            }
            set
            {
                this.axisLabelsFontSize = value;
            }
        }

        public string AxisMinorGridColor
        {
            get
            {
                return this.axisMinorGridColor;
            }
            set
            {
                this.axisMinorGridColor = value;
            }
        }

        public bool AxisMinorGridVisible
        {
            get
            {
                return this.axisMinorGridVisible;
            }
            set
            {
                this.axisMinorGridVisible = value;
            }
        }

        public int AxisMinorTickCount
        {
            get
            {
                return this.axisMinorTickCount;
            }
            set
            {
                this.axisMinorTickCount = value;
            }
        }

        public int AxisMinorTicksLength
        {
            get
            {
                return this.axisMinorTicksLength;
            }
            set
            {
                this.axisMinorTicksLength = value;
            }
        }

        public bool AxisMinorTicksVisible
        {
            get
            {
                return this.axisMinorTicksVisible;
            }
            set
            {
                this.axisMinorTicksVisible = value;
            }
        }

        public string AxisTicksColor
        {
            get
            {
                return this.axisTicksColor;
            }
            set
            {
                this.axisTicksColor = value;
            }
        }

        public string AxisTicksInnerColor
        {
            get
            {
                return this.axisTicksInnerColor;
            }
            set
            {
                this.axisTicksInnerColor = value;
            }
        }

        public int AxisTicksInnerLength
        {
            get
            {
                return this.axisTicksInnerLength;
            }
            set
            {
                this.axisTicksInnerLength = value;
            }
        }

        public bool AxisTicksInnerVisible
        {
            get
            {
                return this.axisTicksInnerVisible;
            }
            set
            {
                this.axisTicksInnerVisible = value;
            }
        }

        public int AxisTicksLength
        {
            get
            {
                return this.axisTicksLength;
            }
            set
            {
                this.axisTicksLength = value;
            }
        }

        public string AxisTitleFontColor
        {
            get
            {
                return this.axisTitleFontColor;
            }
            set
            {
                this.axisTitleFontColor = value;
            }
        }

        public string AxisTitleFontName
        {
            get
            {
                return this.axisTitleFontName;
            }
            set
            {
                this.axisTitleFontName = value;
            }
        }

        public string Base64
        {
            get
            {
                return this.base64;
            }
            set
            {
                this.base64 = value;
            }
        }

        public string HeaderColor
        {
            get
            {
                return this.headerColor;
            }
            set
            {
                this.headerColor = value;
            }
        }

        public bool HeaderFontBold
        {
            get
            {
                return this.headerFontBold;
            }
            set
            {
                this.headerFontBold = value;
            }
        }

        public string HeaderFontColor
        {
            get
            {
                return this.headerFontColor;
            }
            set
            {
                this.headerFontColor = value;
            }
        }

        public string HeaderFontName
        {
            get
            {
                return this.headerFontName;
            }
            set
            {
                this.headerFontName = value;
            }
        }

        public int HeaderFontSize
        {
            get
            {
                return this.headerFontSize;
            }
            set
            {
                this.headerFontSize = value;
            }
        }

        public string HeaderGradientDirection
        {
            get
            {
                return this.headerGradientDirection;
            }
            set
            {
                this.headerGradientDirection = value;
            }
        }

        public string HeaderGradientEndColor
        {
            get
            {
                return this.headerGradientEndColor;
            }
            set
            {
                this.headerGradientEndColor = value;
            }
        }

        public string HeaderGradientMiddleColor
        {
            get
            {
                return this.headerGradientMiddleColor;
            }
            set
            {
                this.headerGradientMiddleColor = value;
            }
        }

        public bool HeaderGradientSigma
        {
            get
            {
                return this.headerGradientSigma;
            }
            set
            {
                this.headerGradientSigma = value;
            }
        }

        public float HeaderGradientSigmaFocus
        {
            get
            {
                return this.headerGradientSigmaFocus;
            }
            set
            {
                this.headerGradientSigmaFocus = value;
            }
        }

        public float HeaderGradientSigmaScale
        {
            get
            {
                return this.headerGradientSigmaScale;
            }
            set
            {
                this.headerGradientSigmaScale = value;
            }
        }

        public string HeaderGradientStartColor
        {
            get
            {
                return this.headerGradientStartColor;
            }
            set
            {
                this.headerGradientStartColor = value;
            }
        }

        public bool HeaderGradientVisible
        {
            get
            {
                return this.headerGradientVisible;
            }
            set
            {
                this.headerGradientVisible = value;
            }
        }

        public string HeaderPenColor
        {
            get
            {
                return this.headerPenColor;
            }
            set
            {
                this.headerPenColor = value;
            }
        }

        public bool HeaderPenVisible
        {
            get
            {
                return this.headerPenVisible;
            }
            set
            {
                this.headerPenVisible = value;
            }
        }

        public int HeaderPenWidth
        {
            get
            {
                return this.headerPenWidth;
            }
            set
            {
                this.headerPenWidth = value;
            }
        }

        public int HeaderShadowSize
        {
            get
            {
                return this.headerShadowSize;
            }
            set
            {
                this.headerShadowSize = value;
            }
        }

        public int HeaderShadowTransparency
        {
            get
            {
                return this.headerShadowTransparency;
            }
            set
            {
                this.headerShadowTransparency = value;
            }
        }

        public int HeaderTransparency
        {
            get
            {
                return this.headerTransparency;
            }
            set
            {
                this.headerTransparency = value;
            }
        }

        public static ThemeProperties Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new ThemeProperties();
                        }
                    }
                }
                return instance;
            }
        }

        public string LegendDividingLinesColor
        {
            get
            {
                return this.legendDividingLinesColor;
            }
            set
            {
                this.legendDividingLinesColor = value;
            }
        }

        public bool LegendDividingLinesVisible
        {
            get
            {
                return this.legendDividingLinesVisible;
            }
            set
            {
                this.legendDividingLinesVisible = value;
            }
        }

        public string LegendFontColor
        {
            get
            {
                return this.legendFontColor;
            }
            set
            {
                this.legendFontColor = value;
            }
        }

        public string LegendFontName
        {
            get
            {
                return this.legendFontName;
            }
            set
            {
                this.legendFontName = value;
            }
        }

        public int LegendFontSize
        {
            get
            {
                return this.legendFontSize;
            }
            set
            {
                this.legendFontSize = value;
            }
        }

        public string LegendGradientDirection
        {
            get
            {
                return this.legendGradientDirection;
            }
            set
            {
                this.legendGradientDirection = value;
            }
        }

        public string LegendGradientEndColor
        {
            get
            {
                return this.legendGradientEndColor;
            }
            set
            {
                this.legendGradientEndColor = value;
            }
        }

        public string LegendGradientMiddleColor
        {
            get
            {
                return this.legendGradientMiddleColor;
            }
            set
            {
                this.legendGradientMiddleColor = value;
            }
        }

        public bool LegendGradientSigma
        {
            get
            {
                return this.legendGradientSigma;
            }
            set
            {
                this.legendGradientSigma = value;
            }
        }

        public float LegendGradientSigmaFocus
        {
            get
            {
                return this.legendGradientSigmaFocus;
            }
            set
            {
                this.legendGradientSigmaFocus = value;
            }
        }

        public float LegendGradientSigmaScale
        {
            get
            {
                return this.legendGradientSigmaScale;
            }
            set
            {
                this.legendGradientSigmaScale = value;
            }
        }

        public string LegendGradientStartColor
        {
            get
            {
                return this.legendGradientStartColor;
            }
            set
            {
                this.legendGradientStartColor = value;
            }
        }

        public bool LegendGradientVisible
        {
            get
            {
                return this.legendGradientVisible;
            }
            set
            {
                this.legendGradientVisible = value;
            }
        }

        public string LegendPenColor
        {
            get
            {
                return this.legendPenColor;
            }
            set
            {
                this.legendPenColor = value;
            }
        }

        public string LegendPenStyle
        {
            get
            {
                return this.legendPenStyle;
            }
            set
            {
                this.legendPenStyle = value;
            }
        }

        public bool LegendPenVisible
        {
            get
            {
                return this.legendPenVisible;
            }
            set
            {
                this.legendPenVisible = value;
            }
        }

        public int LegendPenWidth
        {
            get
            {
                return this.legendPenWidth;
            }
            set
            {
                this.legendPenWidth = value;
            }
        }

        public string LegendShadowColor
        {
            get
            {
                return this.legendShadowColor;
            }
            set
            {
                this.legendShadowColor = value;
            }
        }

        public int LegendShadowHeight
        {
            get
            {
                return this.legendShadowHeight;
            }
            set
            {
                this.legendShadowHeight = value;
            }
        }

        public int LegendShadowTransparency
        {
            get
            {
                return this.legendShadowTransparency;
            }
            set
            {
                this.legendShadowTransparency = value;
            }
        }

        public int LegendShadowWidth
        {
            get
            {
                return this.legendShadowWidth;
            }
            set
            {
                this.legendShadowWidth = value;
            }
        }

        public bool LegendSymbolDefaultPen
        {
            get
            {
                return this.legendSymbolDefaultPen;
            }
            set
            {
                this.legendSymbolDefaultPen = value;
            }
        }

        public bool LegendSymbolPenVisible
        {
            get
            {
                return this.legendSymbolPenVisible;
            }
            set
            {
                this.legendSymbolPenVisible = value;
            }
        }

        public bool LegendSymbolSquared
        {
            get
            {
                return this.legendSymbolSquared;
            }
            set
            {
                this.legendSymbolSquared = value;
            }
        }

        public bool LegendTransparent
        {
            get
            {
                return this.legendTransparent;
            }
            set
            {
                this.legendTransparent = value;
            }
        }

        public string[] Palette
        {
            get
            {
                return this.palette;
            }
            set
            {
                this.palette = value;
            }
        }

        public string PanelBevelInner
        {
            get
            {
                return this.panelBevelInner;
            }
            set
            {
                this.panelBevelInner = value;
            }
        }

        public string PanelBevelOuter
        {
            get
            {
                return this.panelBevelOuter;
            }
            set
            {
                this.panelBevelOuter = value;
            }
        }

        public int PanelBevelWidth
        {
            get
            {
                return this.panelBevelWidth;
            }
            set
            {
                this.panelBevelWidth = value;
            }
        }

        public int PanelBorderRound
        {
            get
            {
                return this.panelBorderRound;
            }
            set
            {
                this.panelBorderRound = value;
            }
        }

        public string PanelColor
        {
            get
            {
                return this.panelColor;
            }
            set
            {
                this.panelColor = value;
            }
        }

        public string PanelGradientDirection
        {
            get
            {
                return this.panelGradientDirection;
            }
            set
            {
                this.panelGradientDirection = value;
            }
        }

        public string PanelGradientEndColor
        {
            get
            {
                return this.panelGradientEndColor;
            }
            set
            {
                this.panelGradientEndColor = value;
            }
        }

        public string PanelGradientMiddleColor
        {
            get
            {
                return this.panelGradientMiddleColor;
            }
            set
            {
                this.panelGradientMiddleColor = value;
            }
        }

        public bool PanelGradientSigma
        {
            get
            {
                return this.panelGradientSigma;
            }
            set
            {
                this.panelGradientSigma = value;
            }
        }

        public float PanelGradientSigmaFocus
        {
            get
            {
                return this.panelGradientSigmaFocus;
            }
            set
            {
                this.panelGradientSigmaFocus = value;
            }
        }

        public float PanelGradientSigmaScale
        {
            get
            {
                return this.panelGradientSigmaScale;
            }
            set
            {
                this.panelGradientSigmaScale = value;
            }
        }

        public string PanelGradientStartColor
        {
            get
            {
                return this.panelGradientStartColor;
            }
            set
            {
                this.panelGradientStartColor = value;
            }
        }

        public bool PanelGradientVisible
        {
            get
            {
                return this.panelGradientVisible;
            }
            set
            {
                this.panelGradientVisible = value;
            }
        }

        public string PanelPenColor
        {
            get
            {
                return this.panelPenColor;
            }
            set
            {
                this.panelPenColor = value;
            }
        }

        public string PanelPenStyle
        {
            get
            {
                return this.panelPenStyle;
            }
            set
            {
                this.panelPenStyle = value;
            }
        }

        public bool PanelPenVisible
        {
            get
            {
                return this.panelPenVisible;
            }
            set
            {
                this.panelPenVisible = value;
            }
        }

        public int PanelPenWidth
        {
            get
            {
                return this.panelPenWidth;
            }
            set
            {
                this.panelPenWidth = value;
            }
        }

        public string PanelShadowColor
        {
            get
            {
                return this.panelShadowColor;
            }
            set
            {
                this.panelShadowColor = value;
            }
        }

        public int PanelShadowSize
        {
            get
            {
                return this.panelShadowSize;
            }
            set
            {
                this.panelShadowSize = value;
            }
        }

        public bool PanelShadowVisible
        {
            get
            {
                return this.panelShadowVisible;
            }
            set
            {
                this.panelShadowVisible = value;
            }
        }

        public string SeriesMarksArrowColor
        {
            get
            {
                return this.seriesMarksArrowColor;
            }
            set
            {
                this.seriesMarksArrowColor = value;
            }
        }

        public string SeriesMarksFontColor
        {
            get
            {
                return this.seriesMarksFontColor;
            }
            set
            {
                this.seriesMarksFontColor = value;
            }
        }

        public string SeriesMarksFontName
        {
            get
            {
                return this.seriesMarksFontName;
            }
            set
            {
                this.seriesMarksFontName = value;
            }
        }

        public int SeriesMarksFontSize
        {
            get
            {
                return this.seriesMarksFontSize;
            }
            set
            {
                this.seriesMarksFontSize = value;
            }
        }

        public string SeriesMarksGradientDirection
        {
            get
            {
                return this.seriesMarksGradientDirection;
            }
            set
            {
                this.seriesMarksGradientDirection = value;
            }
        }

        public string SeriesMarksGradientEndColor
        {
            get
            {
                return this.seriesMarksGradientEndColor;
            }
            set
            {
                this.seriesMarksGradientEndColor = value;
            }
        }

        public string SeriesMarksGradientMiddleColor
        {
            get
            {
                return this.seriesMarksGradientMiddleColor;
            }
            set
            {
                this.seriesMarksGradientMiddleColor = value;
            }
        }

        public bool SeriesMarksGradientSigma
        {
            get
            {
                return this.seriesMarksGradientSigma;
            }
            set
            {
                this.seriesMarksGradientSigma = value;
            }
        }

        public float SeriesMarksGradientSigmaFocus
        {
            get
            {
                return this.seriesMarksGradientSigmaFocus;
            }
            set
            {
                this.seriesMarksGradientSigmaFocus = value;
            }
        }

        public float SeriesMarksGradientSigmaScale
        {
            get
            {
                return this.seriesMarksGradientSigmaScale;
            }
            set
            {
                this.seriesMarksGradientSigmaScale = value;
            }
        }

        public string SeriesMarksGradientStartColor
        {
            get
            {
                return this.seriesMarksGradientStartColor;
            }
            set
            {
                this.seriesMarksGradientStartColor = value;
            }
        }

        public bool SeriesMarksGradientVisible
        {
            get
            {
                return this.seriesMarksGradientVisible;
            }
            set
            {
                this.seriesMarksGradientVisible = value;
            }
        }

        public bool SeriesMarksTransparent
        {
            get
            {
                return this.seriesMarksTransparent;
            }
            set
            {
                this.seriesMarksTransparent = value;
            }
        }

        public bool WallApplyDark
        {
            get
            {
                return this.wallApplyDark;
            }
            set
            {
                this.wallApplyDark = value;
            }
        }

        public string WallGradientDirection
        {
            get
            {
                return this.wallGradientDirection;
            }
            set
            {
                this.wallGradientDirection = value;
            }
        }

        public string WallGradientEndColor
        {
            get
            {
                return this.wallGradientEndColor;
            }
            set
            {
                this.wallGradientEndColor = value;
            }
        }

        public string WallGradientMiddleColor
        {
            get
            {
                return this.wallGradientMiddleColor;
            }
            set
            {
                this.wallGradientMiddleColor = value;
            }
        }

        public bool WallGradientSigma
        {
            get
            {
                return this.wallGradientSigma;
            }
            set
            {
                this.wallGradientSigma = value;
            }
        }

        public float WallGradientSigmaFocus
        {
            get
            {
                return this.wallGradientSigmaFocus;
            }
            set
            {
                this.wallGradientSigmaFocus = value;
            }
        }

        public float WallGradientSigmaScale
        {
            get
            {
                return this.wallGradientSigmaScale;
            }
            set
            {
                this.wallGradientSigmaScale = value;
            }
        }

        public string WallGradientStartColor
        {
            get
            {
                return this.wallGradientStartColor;
            }
            set
            {
                this.wallGradientStartColor = value;
            }
        }

        public bool WallGradientVisible
        {
            get
            {
                return this.wallGradientVisible;
            }
            set
            {
                this.wallGradientVisible = value;
            }
        }

        public string WallPenColor
        {
            get
            {
                return this.wallPenColor;
            }
            set
            {
                this.wallPenColor = value;
            }
        }

        public string WallPenStyle
        {
            get
            {
                return this.wallPenStyle;
            }
            set
            {
                this.wallPenStyle = value;
            }
        }

        public bool WallPenVisible
        {
            get
            {
                return this.wallPenVisible;
            }
            set
            {
                this.wallPenVisible = value;
            }
        }

        public int WallPenWidth
        {
            get
            {
                return this.wallPenWidth;
            }
            set
            {
                this.wallPenWidth = value;
            }
        }

        public string WallsBackColor
        {
            get
            {
                return this.wallsBackColor;
            }
            set
            {
                this.wallsBackColor = value;
            }
        }

        public string WallsBackGradientDirection
        {
            get
            {
                return this.wallsBackGradientDirection;
            }
            set
            {
                this.wallsBackGradientDirection = value;
            }
        }

        public string WallsBackGradientEndColor
        {
            get
            {
                return this.wallsBackGradientEndColor;
            }
            set
            {
                this.wallsBackGradientEndColor = value;
            }
        }

        public string WallsBackGradientMiddleColor
        {
            get
            {
                return this.wallsBackGradientMiddleColor;
            }
            set
            {
                this.wallsBackGradientMiddleColor = value;
            }
        }

        public bool WallsBackGradientSigma
        {
            get
            {
                return this.wallsBackGradientSigma;
            }
            set
            {
                this.wallsBackGradientSigma = value;
            }
        }

        public float WallsBackGradientSigmaFocus
        {
            get
            {
                return this.wallsBackGradientSigmaFocus;
            }
            set
            {
                this.wallsBackGradientSigmaFocus = value;
            }
        }

        public float WallsBackGradientSigmaScale
        {
            get
            {
                return this.wallsBackGradientSigmaScale;
            }
            set
            {
                this.wallsBackGradientSigmaScale = value;
            }
        }

        public string WallsBackGradientStartColor
        {
            get
            {
                return this.wallsBackGradientStartColor;
            }
            set
            {
                this.wallsBackGradientStartColor = value;
            }
        }

        public bool WallsBackGradientVisible
        {
            get
            {
                return this.wallsBackGradientVisible;
            }
            set
            {
                this.wallsBackGradientVisible = value;
            }
        }

        public bool WallsBackTransparent
        {
            get
            {
                return this.wallsBackTransparent;
            }
            set
            {
                this.wallsBackTransparent = value;
            }
        }

        public string WallsBottomColor
        {
            get
            {
                return this.wallsBottomColor;
            }
            set
            {
                this.wallsBottomColor = value;
            }
        }

        public int WallSize
        {
            get
            {
                return this.wallSize;
            }
            set
            {
                this.wallSize = value;
            }
        }

        public string WallsLeftColor
        {
            get
            {
                return this.wallsLeftColor;
            }
            set
            {
                this.wallsLeftColor = value;
            }
        }

        public string WallsRightColor
        {
            get
            {
                return this.wallsRightColor;
            }
            set
            {
                this.wallsRightColor = value;
            }
        }
    }
}

