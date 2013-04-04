namespace Steema.TeeChart.Tools
{
    using Steema.TeeChart;
    using Steema.TeeChart.Styles;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;

    [ToolboxBitmap(typeof(SeriesHotspot), "ToolsIcons.SeriesHotspot.bmp"), Description("Displays \"tips\" or \"hints\" or provides drilldown capacity when the end-user moves or clicks the mouse over a series point.")]
    public class SeriesHotspot : ToolSeries, IHotspot
    {
        private HotspotHelperScripts helperScript;
        private int hotspotCanvasIndex;
        private Steema.TeeChart.Styles.MapAction mapAction;
        private string mapElements;
        private Steema.TeeChart.Styles.MapRegion mapRegion;
        private MarksStyles style;

        public event SeriesHotspotEventHandler GetHTMLMap;

        public SeriesHotspot() : this(null)
        {
        }

        public SeriesHotspot(Chart c) : base(c)
        {
            this.style = MarksStyles.Label;
            this.mapElements = "";
            this.hotspotCanvasIndex = 0x1f3;
            this.helperScript = HotspotHelperScripts.Annotation;
        }

        protected override void Assign(Tool t)
        {
            base.Assign(t);
            SeriesHotspot hotspot = t as SeriesHotspot;
            hotspot.HelperScript = this.HelperScript;
            hotspot.HotspotCanvasIndex = this.HotspotCanvasIndex;
            hotspot.MapAction = this.MapAction;
            hotspot.MapElements = this.MapElements;
        }

        protected internal void DoGetHTMLMap(Series s, PointPolygon p)
        {
            if (this.GetHTMLMap != null)
            {
                SeriesHotspotEventArgs e = new SeriesHotspotEventArgs(s, p);
                this.GetHTMLMap(this, e);
                p = e.PointPolygon;
            }
        }

        public string GenerateMap()
        {
            return this.GetMap(this.style, base.Chart.ChartRect);
        }

        public string GenerateMap(Rectangle bounds)
        {
            return this.GetMap(this.style, bounds);
        }

        public string GetMap(MarksStyles style, Rectangle bounds)
        {
            if (this.mapRegion == null)
            {
                this.mapRegion = new Steema.TeeChart.Styles.MapRegion(this);
            }
            return this.mapRegion.GetMap(style, bounds);
        }

        [Description("Gets descriptive text.")]
        public override string Description
        {
            get
            {
                return Texts.HotspotTool;
            }
        }

        public HotspotHelperScripts HelperScript
        {
            get
            {
                return this.helperScript;
            }
            set
            {
                if (value != this.helperScript)
                {
                    this.helperScript = value;
                }
            }
        }

        public int HotspotCanvasIndex
        {
            get
            {
                return this.hotspotCanvasIndex;
            }
            set
            {
                if (value != this.hotspotCanvasIndex)
                {
                    this.hotspotCanvasIndex = value;
                }
            }
        }

        [Browsable(false)]
        public Steema.TeeChart.Styles.MapAction MapAction
        {
            get
            {
                return this.mapAction;
            }
            set
            {
                this.mapAction = value;
            }
        }

        public string MapElements
        {
            get
            {
                return this.mapElements;
            }
            set
            {
                base.SetStringProperty(ref this.mapElements, value);
            }
        }

        [Browsable(false)]
        public Steema.TeeChart.Styles.MapRegion MapRegion
        {
            get
            {
                if (this.mapRegion == null)
                {
                    this.mapRegion = new Steema.TeeChart.Styles.MapRegion(this);
                }
                return this.mapRegion;
            }
        }

        [Browsable(false), DefaultValue(2), Description("Defines the text format of the Hotspot text.")]
        public MarksStyles Style
        {
            get
            {
                return this.style;
            }
            set
            {
                if (this.style != value)
                {
                    this.style = value;
                    this.Invalidate();
                }
            }
        }

        [Description("Gets detailed descriptive text.")]
        public override string Summary
        {
            get
            {
                return Texts.HotspotSummary;
            }
        }
    }
}

