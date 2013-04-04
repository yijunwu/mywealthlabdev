namespace Steema.TeeChart.Styles
{
    using Steema.TeeChart.Drawing;
    using Steema.TeeChart.Tools;
    using System;
    using System.Collections;
    using System.Drawing;
    using System.Text;

    public class MapRegion : TeeBase
    {
        private ArrayList ListOfPolygonLists;
        private Steema.TeeChart.Styles.MapAction mapAction;
        private Series series;
        private SeriesHotspot seriesHotSpotTool;

        public MapRegion(Series s)
        {
            this.ListOfPolygonLists = new ArrayList();
            this.series = s;
            this.GetPolygons(s);
        }

        public MapRegion(SeriesHotspot h)
        {
            this.ListOfPolygonLists = new ArrayList();
            this.seriesHotSpotTool = h;
            base.chart = h.Chart;
            this.mapAction = h.MapAction;
            this.ListOfPolygonLists = new ArrayList();
            for (int i = base.chart.Series.Count - 1; i >= 0; i--)
            {
                this.ListOfPolygonLists.Add(this.GetPolygons(base.chart[i]));
            }
        }

        private void GetInChartPoints(PointPolygon p, Rectangle bounds, bool scrolling)
        {
            if (!scrolling)
            {
                bounds = base.Chart.ChartRect;
            }
            if (p.PointStyle == PolygonStyle.Poly)
            {
                if (p.Coordinates != null)
                {
                    bool flag = false;
                    bool flag2 = false;
                    bool flag3 = false;
                    bool flag4 = false;
                    for (int i = 0; i < p.Coordinates.GetLength(0); i += 2)
                    {
                        bool flag5 = false;
                        if (Math.IEEERemainder((double) i, 2.0) == 0.0)
                        {
                            if (scrolling)
                            {
                                if (p.Coordinates[i] > bounds.Right)
                                {
                                    flag = true;
                                }
                                else if (p.Coordinates[i] < bounds.Left)
                                {
                                    flag3 = true;
                                }
                                else
                                {
                                    flag5 = true;
                                }
                                if (p.Coordinates[i + 1] > bounds.Bottom)
                                {
                                    flag2 = true;
                                }
                                else if (p.Coordinates[i + 1] < bounds.Top)
                                {
                                    flag4 = true;
                                }
                                else if (flag5)
                                {
                                    p.IncludePolygon = true;
                                }
                            }
                            else
                            {
                                if (p.Coordinates[i] > bounds.Right)
                                {
                                    p.Coordinates[i] = bounds.Right;
                                    flag = true;
                                }
                                else if (p.Coordinates[i] < bounds.Left)
                                {
                                    p.Coordinates[i] = bounds.Left;
                                    flag3 = true;
                                }
                                else
                                {
                                    flag5 = true;
                                }
                                if (p.Coordinates[i + 1] > bounds.Bottom)
                                {
                                    p.Coordinates[i + 1] = bounds.Bottom;
                                    flag2 = true;
                                }
                                else if (p.Coordinates[i + 1] < bounds.Top)
                                {
                                    p.Coordinates[i + 1] = bounds.Top;
                                    flag4 = true;
                                }
                                else if (flag5)
                                {
                                    p.IncludePolygon = true;
                                }
                            }
                        }
                        if ((flag && flag3) && (flag2 && flag4))
                        {
                            p.IncludePolygon = true;
                        }
                    }
                }
            }
            else if ((((p.Coordinates != null) && (p.Coordinates[0] > bounds.Left)) && ((p.Coordinates[0] < bounds.Right) && (p.Coordinates[1] > bounds.Top))) && (p.Coordinates[1] < bounds.Bottom))
            {
                p.IncludePolygon = true;
            }
        }

        public string GetMap(MarksStyles style, Rectangle bounds)
        {
            bool flag = false;
            bool flag2 = false;
            bool scrolling = false;
            StringBuilder builder = new StringBuilder(1);
            foreach (Tool tool in base.Chart.Tools)
            {
                if (tool is ScrollTool)
                {
                    scrolling = true;
                    break;
                }
                if ((tool is ZoomTool) && base.Chart.Zoom.Zoomed)
                {
                    flag2 = true;
                    break;
                }
            }
            foreach (Steema.TeeChart.Styles.PointPolygonList list in this.ListOfPolygonLists)
            {
                MarksStyles styles = list.Series.Marks.Style;
                list.Series.Marks.Style = style;
                foreach (PointPolygon polygon in list)
                {
                    polygon.IncludePolygon = false;
                    if (flag2 || scrolling)
                    {
                        this.GetInChartPoints(polygon, bounds, scrolling);
                    }
                    else
                    {
                        polygon.IncludePolygon = true;
                    }
                    if ((polygon.Coordinates != null) && polygon.IncludePolygon)
                    {
                        switch (this.mapAction)
                        {
                            case Steema.TeeChart.Styles.MapAction.Mark:
                                polygon.Title = list.Series.ValueMarkText(polygon.ValueIndex);
                                this.seriesHotSpotTool.DoGetHTMLMap(list.Series, polygon);
                                if (polygon.IncludePolygon)
                                {
                                    builder.Append("<AREA shape=\"" + polygon.PointStyle.ToString() + "\"");
                                    builder.Append(" Title=\"" + polygon.Title + "\"");
                                }
                                break;

                            case Steema.TeeChart.Styles.MapAction.URL:
                                this.seriesHotSpotTool.DoGetHTMLMap(list.Series, polygon);
                                if (polygon.IncludePolygon)
                                {
                                    builder.Append("<AREA shape=\"" + polygon.PointStyle.ToString() + "\"");
                                    if ((polygon.Title != null) && (polygon.Title.Length > 0))
                                    {
                                        builder.Append(" Title=\"" + polygon.Title + "\"");
                                    }
                                    if ((polygon.HREF != null) && (polygon.HREF.Length > 0))
                                    {
                                        builder.Append(" href=\"" + polygon.HREF + "\" ");
                                    }
                                    if ((polygon.Attributes != null) && (polygon.Attributes.Length > 0))
                                    {
                                        builder.Append(" " + polygon.Attributes);
                                    }
                                }
                                break;

                            case Steema.TeeChart.Styles.MapAction.Script:
                                this.seriesHotSpotTool.DoGetHTMLMap(list.Series, polygon);
                                if (polygon.IncludePolygon)
                                {
                                    builder.Append("<AREA shape=\"" + polygon.PointStyle.ToString() + "\"");
                                    if ((polygon.Title != null) && (polygon.Title.Length > 0))
                                    {
                                        builder.Append(" Title=\"" + polygon.Title + "\"");
                                    }
                                    if ((polygon.Attributes != null) && (polygon.Attributes.Length > 0))
                                    {
                                        builder.Append(" " + polygon.Attributes);
                                    }
                                }
                                break;
                        }
                        if (polygon.IncludePolygon)
                        {
                            builder.Append(" coords=\" ");
                            for (int i = 0; i < polygon.Coordinates.GetLength(0); i++)
                            {
                                if (i > 0)
                                {
                                    builder.Append(",");
                                }
                                if (scrolling)
                                {
                                    if ((i == 0) || ((i % 2) == 0))
                                    {
                                        polygon.Coordinates[i] -= bounds.Left;
                                    }
                                    else
                                    {
                                        polygon.Coordinates[i] -= bounds.Top;
                                    }
                                }
                                builder.Append(polygon.Coordinates[i].ToString());
                                flag = true;
                            }
                            builder.Append("\">");
                        }
                    }
                }
                list.Series.Marks.Style = styles;
            }
            if (!flag)
            {
                return "";
            }
            return builder.ToString();
        }

        protected internal Steema.TeeChart.Styles.PointPolygonList GetPolygons(Series s)
        {
            Steema.TeeChart.Styles.PointPolygonList list = new Steema.TeeChart.Styles.PointPolygonList(s);
            PolygonStyle poly = PolygonStyle.Poly;
            Graphics3D graphicsd = base.chart.graphics3D;
            base.chart.Graphics3D = new Graphics3DHotSpot(base.chart);
            base.chart.Graphics3D.Projection(base.chart.aspect.Width3D, base.chart.ChartRect);
            base.chart.Graphics3D.InitWindow(graphicsd.g, base.chart.aspect, base.chart.ChartRect, 100);
            for (int i = s.Count - 1; i >= 0; i--)
            {
                int[] bounds = s.GetBounds(i, ref poly);
                list.Add(new PointPolygon(i, bounds, poly));
            }
            base.chart.Graphics3D = graphicsd;
            return list;
        }

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

        public Steema.TeeChart.Styles.PointPolygonList PointPolygonList
        {
            get
            {
                return this.GetPolygons(this.series);
            }
        }
    }
}

