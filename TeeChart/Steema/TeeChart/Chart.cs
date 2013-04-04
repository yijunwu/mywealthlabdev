namespace Steema.TeeChart
{
    using Steema.TeeChart.Data;
    using Steema.TeeChart.Drawing;
    using Steema.TeeChart.Editors;
    using Steema.TeeChart.Export;
    using Steema.TeeChart.Import;
    using Steema.TeeChart.Languages;
    using Steema.TeeChart.Styles;
    using Steema.TeeChart.Themes;
    using Steema.TeeChart.Tools;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Runtime.Serialization;
    using System.Security.Permissions;
    using System.Windows.Forms;

    [Serializable, LicenseProvider(typeof(FrAccessProvider))]
    public class Chart : TeeBase, ICloneable, ISerializable
    {
        internal Steema.TeeChart.Drawing.Aspect aspect;
        public bool AutoRepaint;
        internal Steema.TeeChart.Axes axes;
        public bool CancelMouse;
        internal Rectangle chartBounds;
        public Rectangle ChartRect;
        public bool ClipWhenMetafiling;
        public bool ClipWhenPrinting;
        private Exports export;
        private Steema.TeeChart.Footer footer;
        internal Steema.TeeChart.Drawing.Graphics3D graphics3D;
        private Steema.TeeChart.Header header;
        internal bool IClicked;
        private Imports import;
        internal bool InvertedRotation;
        public bool iOpenGL;
        protected internal Steema.TeeChart.Page iPage;
        public bool iWorldMaps;
        internal Steema.TeeChart.Legend legend;
        internal ChartPen legendPen;
        private FrAccess license;
        private List<ITeeEventListener> listeners;
        internal int maxZOrder;
        private Steema.TeeChart.Panel panel;
        private Scroll panning;
        internal IChart parent;
        private Steema.TeeChart.Printer printer;
        internal bool printing;
        internal bool redrawing;
        internal bool restoredAxisScales;
        private AllAxisSavedScales savedScales;
        private object serializedTag;
        internal SeriesCollection series;
        internal int seriesHeight3D;
        internal int seriesWidth3D;
        private Steema.TeeChart.Footer subFooter;
        private Steema.TeeChart.Header subHeader;
        private ToolsCollection tools;
        private ChartToolTip toolTip;
        private Steema.TeeChart.Walls walls;
        internal Steema.TeeChart.Zoom zoom;

        public Chart()
        {
            this.AutoRepaint = true;
            this.restoredAxisScales = true;
            this.ClipWhenMetafiling = true;
            this.chartBounds = new Rectangle(0, 0, 0, 0);
            this.ClipWhenPrinting = true;
            this.license = (FrAccess) LicenseManager.Validate(typeof(Chart), this);
            this.initVars();
        }

        public Chart(SerializationInfo info, StreamingContext context) : this()
        {
            this.import.DeserializeFrom(info, context);
        }

        public Steema.TeeChart.Styles.Series ActiveSeriesLegend(int itemIndex)
        {
            return this.SeriesLegend(itemIndex, true);
        }

        private bool ActiveSeriesUseAxis()
        {
            foreach (Steema.TeeChart.Styles.Series series in this.series)
            {
                if (series.Active && series.UseAxis)
                {
                    return true;
                }
            }
            return false;
        }

        internal bool AddToContainer(IComponent component)
        {
            bool isInternal = false;
            if (component is TeeBase)
            {
                isInternal = (component as TeeBase).InternalUse;
            }
            return this.AddToContainer(component, isInternal);
        }

        internal bool AddToContainer(IComponent component, bool isInternal)
        {
            bool flag = false;
            if (this.Parent != null)
            {
                flag = this.Parent.IsWebForm();
            }
            if (!flag)
            {
                flag = isInternal;
            }
            IContainer chartContainer = this.ChartContainer;
            if ((chartContainer != null) && !flag)
            {
                chartContainer.Add(component);
                return true;
            }
            return false;
        }

        private void ApplyMaxMinOffsets(Axis axis, Rectangle rect)
        {
            this.ApplyMaxMinOffsets(axis, rect.Left, rect.Top, rect.Right, rect.Bottom);
        }

        private void ApplyMaxMinOffsets(Axis axis, int left, int top, int right, int bottom)
        {
            bool flag = (axis.MinimumOffset != 0) || (axis.MaximumOffset != 0);
            int a = left;
            int b = top;
            int num3 = right;
            int num4 = bottom;
            if (flag)
            {
                if (axis.Horizontal)
                {
                    this.ApplyOffsets(axis, ref a, ref num3);
                }
                else
                {
                    this.ApplyOffsets(axis, ref num4, ref b);
                }
            }
            axis.AdjustMaxMinRect(a, b, num3, num4);
            axis.CalcRoundScales();
            if (flag)
            {
                axis.posTitle = axis.InflateAxisPos(axis.Labels.position, axis.SizeLabels());
            }
        }

        private void ApplyOffsets(Axis axis, ref int a, ref int b)
        {
            if (axis.Inverted)
            {
                b += axis.MinimumOffset;
                a += axis.MaximumOffset;
            }
            else
            {
                a += axis.MinimumOffset;
                b += axis.MaximumOffset;
            }
        }

        private void AxisRect(Axis a, ref Rectangle r)
        {
            if (this.IsAxisVisible(a))
            {
                Rectangle rectangle = r;
                if ((a != a.chart.axes.depthTop) && !a.IsCustom())
                {
                    a.CalcRect(ref rectangle, true);
                }
                else
                {
                    a.CalcRect(ref rectangle, false);
                }
                r.Intersect(rectangle);
            }
            else if (this.axes.AxisCalcPosLabels != null)
            {
                int num = this.axes.AxisCalcPosLabels(a, 0);
                a.InflateAxisRect(num, ref r);
            }
        }

        public System.Drawing.Bitmap Bitmap()
        {
            return this.Bitmap(this.Width, this.Height);
        }

        public System.Drawing.Bitmap Bitmap(int width, int height)
        {
            return this.Bitmap(width, height, PixelFormat.Undefined);
        }

        public System.Drawing.Bitmap Bitmap(int width, int height, PixelFormat pixelformat)
        {
            System.Drawing.Bitmap bitmap;
            if (pixelformat == PixelFormat.Undefined)
            {
                bitmap = new System.Drawing.Bitmap(width, height);
            }
            else
            {
                bitmap = new System.Drawing.Bitmap(width, height, pixelformat);
            }
            this.chartBounds.Width = width;
            this.chartBounds.Height = height;
            if (this.Graphics3D.UseBuffer)
            {
                this.Graphics3D.BackBuffer = this.Graphics3D.BackBufferContext.Allocate(Graphics.FromImage(bitmap), this.chartBounds);
                try
                {
                    this.Draw(this.Graphics3D.BackBuffer.Graphics);
                    this.Graphics3D.BackBuffer.Render();
                    return bitmap;
                }
                finally
                {
                    this.Graphics3D.BackBuffer.Dispose();
                    this.Graphics3D.BackBuffer = null;
                }
            }
            this.Draw(Graphics.FromImage(bitmap));
            return bitmap;
        }

        internal TeeEvent BroadcastEvent(TeeEvent Event)
        {
            TeeEvent event2 = Event;
            Event.sender = base.Chart;
            foreach (ITeeEventListener listener in this.Listeners)
            {
                listener.TeeEvent(Event);
                if ((Event is TeeMouseEvent) && base.chart.CancelMouse)
                {
                    return event2;
                }
            }
            return event2;
        }

        protected internal void BroadcastEvent(Steema.TeeChart.Styles.Series s, SeriesEventStyle e)
        {
            SeriesEvent event2 = new SeriesEvent {
                Event = e,
                Series = s
            };
            this.BroadcastEvent(event2);
        }

        protected internal virtual void BroadcastKeyEvent(KeyEventArgs e)
        {
            for (int i = 0; i < this.Tools.Count; i++)
            {
                if (this.Tools[i].Active)
                {
                    this.Tools[i].KeyEvent(e);
                }
            }
        }

        protected internal virtual void BroadcastMouseEvent(MouseEventKinds kind, MouseEventArgs e, Keys modKeys)
        {
            if ((this.parent != null) && !base.DesignMode)
            {
                if (this.Listeners.Count > 0)
                {
                    TeeMouseEvent event2 = new TeeMouseEvent();
                    try
                    {
                        event2.Event = kind;
                        event2.Button = Utils.GetMouseButton(e);
                        event2.ModifierKeys = modKeys;
                        event2.mArgs = new MouseEventArgs(Utils.GetMouseButton(e), 1, e.X, e.Y, 0);
                        this.BroadcastEvent(event2);
                    }
                    finally
                    {
                        event2 = null;
                    }
                }
                Cursor c = this.parent.GetCursor();
                if (c != null)
                {
                    this.BroadcastMouseEvent(kind, e, ref c);
                    this.parent.SetCursor(c);
                }
            }
        }

        protected internal virtual void BroadcastMouseEvent(MouseEventKinds kind, MouseEventArgs e, ref Cursor c)
        {
            for (int i = 0; i < this.Tools.Count; i++)
            {
                if (this.Tools[i].Active)
                {
                    this.Tools[i].MouseEvent(kind, e, ref c);
                    if (this.CancelMouse)
                    {
                        break;
                    }
                }
            }
            foreach (Steema.TeeChart.Styles.Series series in this.series)
            {
                if (series.Active)
                {
                    series.MouseEvent(kind, e, ref c);
                }
            }
        }

        protected internal virtual void BroadcastToolEvent(EventArgs e)
        {
            foreach (Steema.TeeChart.Tools.Tool tool in this.tools)
            {
                if (tool.Active)
                {
                    tool.ChartEvent(e);
                }
            }
        }

        private void CalcAxisRect()
        {
            Rectangle chartRect = this.ChartRect;
            this.axes.AdjustMaxMin();
            this.axes.InternalCalcPositions();
            this.AxisRect(this.axes.Left, ref chartRect);
            this.AxisRect(this.axes.Top, ref chartRect);
            this.AxisRect(this.axes.Right, ref chartRect);
            this.AxisRect(this.axes.Bottom, ref chartRect);
            this.AxisRect(this.axes.Depth, ref chartRect);
            this.AxisRect(this.axes.DepthTop, ref chartRect);
            foreach (Axis axis in this.axes.custom)
            {
                this.AxisRect(axis, ref chartRect);
            }
            this.RecalcWidthHeight(ref chartRect);
            this.ChartRect = chartRect;
            this.axes.InternalCalcPositions();
        }

        public void CalcClickedPart(Point Pos, out ChartClickedPart Part)
        {
            Part = this.CalcNeedClickedPart(Pos, false);
        }

        private void CalcInvertedRotation()
        {
            this.InvertedRotation = false;
            if (this.Aspect.View3D)
            {
                if (this.Aspect.Orthogonal)
                {
                    if (this.Aspect.OrthoAngle > 90)
                    {
                        this.InvertedRotation = true;
                    }
                }
                else if (this.Aspect.Rotation < 180)
                {
                    this.InvertedRotation = true;
                }
            }
        }

        private ChartClickedPart CalcNeedClickedPart(Point Pos, bool Needed)
        {
            ChartClickedPart result = new ChartClickedPart {
                Part = ChartClickedPartStyle.None,
                PointIndex = -1,
                ASeries = null,
                AAxis = null
            };
            if ((!this.Legend.Visible || !this.Legend.CustomPosition) || !this.ClickedLegend(Pos, ref result))
            {
                for (int i = this.Series.Count - 1; i >= 0; i--)
                {
                    Steema.TeeChart.Styles.Series series = this.Series[i];
                    bool flag = (this.parent != null) && this.parent.CheckClickSeries();
                    if (series.Active && ((!Needed || series.HasClickEvents()) || flag))
                    {
                        result.PointIndex = series.Clicked(Pos);
                        if (result.PointIndex != -1)
                        {
                            result.ASeries = this.Series[i];
                            result.Part = ChartClickedPartStyle.Series;
                            return result;
                        }
                        if (this.Series[i].Marks.Visible)
                        {
                            result.PointIndex = this.Series[i].Marks.Clicked(Pos);
                            if (result.PointIndex != -1)
                            {
                                result.ASeries = this.Series[i];
                                result.Part = ChartClickedPartStyle.SeriesMarks;
                                return result;
                            }
                        }
                    }
                }
                for (int j = 0; j < 5; j++)
                {
                    this.ClickedAxis(this.Axes[j], Pos, ref result);
                    if (result.Part == ChartClickedPartStyle.Axis)
                    {
                        return result;
                    }
                }
                for (int k = 0; k < this.Axes.Custom.Count; k++)
                {
                    this.ClickedAxis(this.Axes.Custom[k], Pos, ref result);
                    if (result.Part == ChartClickedPartStyle.Axis)
                    {
                        return result;
                    }
                }
                if (this.Legend.Visible && this.ClickedLegend(Pos, ref result))
                {
                    return result;
                }
                if (this.Header.Clicked(Pos))
                {
                    result.Part = ChartClickedPartStyle.Header;
                    return result;
                }
                if (this.SubHeader.Clicked(Pos))
                {
                    result.Part = ChartClickedPartStyle.SubHeader;
                    return result;
                }
                if (this.Footer.Clicked(Pos))
                {
                    result.Part = ChartClickedPartStyle.Foot;
                    return result;
                }
                if (this.SubFooter.Clicked(Pos))
                {
                    result.Part = ChartClickedPartStyle.SubFoot;
                    return result;
                }
                if (this.ChartRect.Contains(Pos.X, Pos.Y) && (this.CountActiveSeries() > 0))
                {
                    result.Part = ChartClickedPartStyle.ChartRect;
                }
            }
            return result;
        }

        private int CalcNumPages(Axis a)
        {
            int num = 1;
            int count = 0;
            bool flag = true;
            foreach (Steema.TeeChart.Styles.Series series in this.series)
            {
                if ((series.Active && series.AssociatedToAxis(a)) && (flag || (series.Count > count)))
                {
                    count = series.Count;
                    flag = false;
                }
                if (count > 0)
                {
                    num = count / this.iPage.MaxPointsPerPage;
                    if ((count % this.iPage.MaxPointsPerPage) > 0)
                    {
                        num++;
                    }
                }
            }
            return num;
        }

        private void CalcResult(double Value, bool IsMin, ref double Result, ref bool FirstTime)
        {
            if ((FirstTime || (IsMin && (Value < Result))) || (!IsMin && (Value > Result)))
            {
                Result = Value;
                FirstTime = false;
            }
        }

        private void CalcSeriesAxisRect(Axis axis)
        {
            int left = 0;
            int top = 0;
            int right = 0;
            int bottom = 0;
            int leftMargin = 0;
            int rightMargin = 0;
            foreach (Steema.TeeChart.Styles.Series series in this.series)
            {
                if (series.bActive && series.AssociatedToAxis(axis))
                {
                    if (axis.horizontal)
                    {
                        series.CalcHorizMargins(ref leftMargin, ref rightMargin);
                        axis.MinimumOffset = leftMargin;
                        axis.MaximumOffset = rightMargin;
                    }
                    else
                    {
                        series.CalcVerticalMargins(ref leftMargin, ref rightMargin);
                        axis.MinimumOffset = rightMargin;
                        axis.MaximumOffset = leftMargin;
                    }
                }
            }
            this.ApplyMaxMinOffsets(axis, left, top, right, bottom);
        }

        private void CalcSeriesRect()
        {
            for (int i = 0; i < this.axes.Count; i++)
            {
                this.CalcSeriesAxisRect(this.axes[i]);
            }
        }

        private void CalcSize3DWalls()
        {
            if (this.aspect.View3D)
            {
                double num = 0.001 * this.aspect.Chart3DPercent;
                if (!this.aspect.Orthogonal)
                {
                    num *= 2.0;
                }
                this.seriesWidth3D = Utils.Round((double) (num * this.chartBounds.Width));
                if (this.aspect.Orthogonal)
                {
                    double num2 = Math.Sin(this.aspect.OrthoAngle * 0.017453292519943295);
                    double num3 = Math.Cos(this.aspect.OrthoAngle * 0.017453292519943295);
                    num = num2 / num3;
                }
                else
                {
                    num = 1.0;
                }
                if (num > 1.0)
                {
                    this.seriesWidth3D = Utils.Round((double) (((double) this.seriesWidth3D) / num));
                }
                this.seriesHeight3D = Utils.Round((double) (this.seriesWidth3D * num));
                int num4 = this.aspect.ApplyZOrder ? Math.Max(1, this.maxZOrder + 1) : 1;
                this.aspect.Height3D = this.seriesHeight3D * num4;
                this.aspect.Width3D = this.seriesWidth3D * num4;
            }
            else
            {
                this.seriesWidth3D = 0;
                this.seriesHeight3D = 0;
                this.aspect.Width3D = 0;
                this.aspect.Height3D = 0;
            }
        }

        private int CalcString(int tmpResult, string St, ref int numLines)
        {
            tmpResult = Math.Max(tmpResult, Utils.Round(this.Graphics3D.TextWidth(St)));
            numLines++;
            return tmpResult;
        }

        private void CalcWallsRect(ref Rectangle r)
        {
            this.CalcSize3DWalls();
            if (this.aspect.View3D && this.aspect.Orthogonal)
            {
                int size;
                if (this.ActiveSeriesUseAxis())
                {
                    size = this.Walls.Back.Size;
                }
                else
                {
                    size = 0;
                }
                r.Width -= Math.Abs(this.aspect.Width3D) + size;
                int num2 = Math.Abs(this.aspect.Height3D) + size;
                r.Height -= num2;
                r.Y += num2;
                if (this.Walls.Right.Visible)
                {
                    r.Width -= this.Walls.Right.Size + 1;
                }
            }
            this.RecalcWidthHeight(ref r);
        }

        public bool CanClip()
        {
            if (this.graphics3D.SupportsFullRotation)
            {
                return false;
            }
            if ((this.printing || this.graphics3D.metafiling) && (!this.printing || !this.ClipWhenPrinting))
            {
                return (this.graphics3D.metafiling && this.ClipWhenMetafiling);
            }
            return true;
        }

        private bool CheckMouseSeries(ref Cursor c, int X, int Y)
        {
            foreach (Steema.TeeChart.Styles.Series series in this.series)
            {
                if (series.bActive && series.CheckMouse(ref c, X, Y))
                {
                    return true;
                }
            }
            return false;
        }

        private void CheckTitle(Steema.TeeChart.Title t, MouseEventArgs e, Keys Shift)
        {
            if (this.parent != null)
            {
                this.parent.CheckTitle(t, e, Shift);
            }
        }

        protected internal virtual void CheckZoomPanning(MouseEventArgs e, Keys Shift)
        {
            if (this.ActiveSeriesUseAxis())
            {
                Point point = new Point(e.X, e.Y);
                if ((this.Zoom.Allow && (Utils.GetMouseButton(e) == this.Zoom.MouseButton)) && ((this.Zoom.KeyShift == Keys.None) || (this.Zoom.KeyShift == Shift)))
                {
                    int x = e.X;
                    int y = e.Y;
                    if (this.Zoom.Direction == ZoomDirections.Vertical)
                    {
                        x = this.ChartRect.X;
                    }
                    if (this.Zoom.Direction == ZoomDirections.Horizontal)
                    {
                        y = this.ChartRect.Y;
                    }
                    this.Zoom.Activate(x, y);
                    if (this.Zoom.Direction == ZoomDirections.Vertical)
                    {
                        this.Zoom.x1 = this.ChartRect.Right;
                    }
                    if (this.Zoom.Direction == ZoomDirections.Horizontal)
                    {
                        this.Zoom.y1 = this.ChartRect.Bottom;
                    }
                    this.Zoom.Draw();
                    this.IClicked = true;
                }
                if (((this.Panning.Allow != ScrollModes.None) && (Utils.GetMouseButton(e) == this.Panning.MouseButton)) && ((this.Panning.KeyShift == Keys.None) || (this.Panning.KeyShift == Shift)))
                {
                    this.Panning.Activate(point.X, point.Y);
                    this.IClicked = true;
                }
            }
        }

        internal void Clear(IChart Parent)
        {
            this.Dispose(true);
            base.Chart = new Chart();
            this.parent = Parent;
            this.parent.SetChart(base.Chart);
            this.parent.DoSetControlStyle();
            this.parent.DoInvalidate();
        }

        private void ClickedAxis(Axis a, Point p, ref ChartClickedPart result)
        {
            if (a.Clicked(p))
            {
                result.Part = ChartClickedPartStyle.Axis;
                result.AAxis = a;
            }
        }

        private bool ClickedLegend(Point Pos, ref ChartClickedPart result)
        {
            result.PointIndex = this.Legend.Clicked(Pos.X, Pos.Y);
            if (result.PointIndex != -1)
            {
                result.Part = ChartClickedPartStyle.Legend;
                return true;
            }
            return false;
        }

        public object Clone()
        {
            MemoryStream stream = new MemoryStream();
            Chart chart = new Chart();
            this.Export.Theme.SaveWithBase64 = true;
            this.Export.Theme.Save(stream);
            stream.Position = 0L;
            return chart.Import.Theme.Load(stream);
        }

        public int CountActiveSeries()
        {
            int num = 0;
            foreach (Steema.TeeChart.Styles.Series series in this.series)
            {
                if (series.bActive)
                {
                    num++;
                }
            }
            return num;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.RemoveAllComponents();
                if (this.license != null)
                {
                    this.license.Dispose();
                    this.license = null;
                }
            }
            base.Dispose(disposing);
        }

        internal void DoDrawLegend(ref Rectangle tmp)
        {
            if (this.legend.Visible)
            {
                this.legend.Paint(this.graphics3D, tmp);
                if (this.legend.iLastValue >= this.legend.firstValue)
                {
                    this.legend.ResizeChartRect(ref tmp);
                }
            }
        }

        public virtual void DoKeyDown(KeyEventArgs e)
        {
            this.BroadcastKeyEvent(e);
        }

        public virtual void DoMouseDown(bool IsDoubleClick, MouseEventArgs e, Keys Shift)
        {
            if (this.CancelMouse)
            {
                return;
            }
            this.CancelMouse = false;
            this.BroadcastMouseEvent(MouseEventKinds.Down, e, Shift);
            if (this.CancelMouse)
            {
                goto Label_0222;
            }
            Point pos = new Point(e.X, e.Y);
            ChartClickedPart part = this.CalcNeedClickedPart(pos, true);
            this.IClicked = false;
            switch (part.Part)
            {
                case ChartClickedPartStyle.Legend:
                    if (!IsDoubleClick)
                    {
                        this.IClicked = this.legend.DoMouseDown(pos.X, pos.Y);
                        if (this.parent != null)
                        {
                            this.parent.DoClickLegend(this, e);
                        }
                    }
                    goto Label_0205;

                case ChartClickedPartStyle.Axis:
                    if (this.parent != null)
                    {
                        this.parent.DoClickAxis(part.AAxis, e);
                    }
                    this.IClicked = this.CancelMouse;
                    if (!this.IClicked)
                    {
                        this.CheckZoomPanning(e, Shift);
                    }
                    goto Label_0205;

                case ChartClickedPartStyle.Series:
                    this.CancelMouse = false;
                    if (!IsDoubleClick)
                    {
                        part.ASeries.OnClick(e);
                        break;
                    }
                    if (this.parent != null)
                    {
                        part.ASeries.OnDblClick(e);
                    }
                    break;

                case ChartClickedPartStyle.Header:
                    this.CheckTitle(this.header, e, Shift);
                    goto Label_0205;

                case ChartClickedPartStyle.Foot:
                    this.CheckTitle(this.footer, e, Shift);
                    goto Label_0205;

                case ChartClickedPartStyle.ChartRect:
                    if (this.parent != null)
                    {
                        this.parent.CheckBackground(this, e);
                    }
                    if (!this.IClicked)
                    {
                        this.CheckZoomPanning(e, Shift);
                    }
                    goto Label_0205;

                case ChartClickedPartStyle.SubHeader:
                    this.CheckTitle(this.subHeader, e, Shift);
                    goto Label_0205;

                case ChartClickedPartStyle.SubFoot:
                    this.CheckTitle(this.subFooter, e, Shift);
                    goto Label_0205;

                default:
                    goto Label_0205;
            }
            this.IClicked = this.CancelMouse;
            if (((this.parent != null) && this.parent.CheckClickSeries()) && !IsDoubleClick)
            {
                this.CancelMouse = true;
                if (this.parent != null)
                {
                    this.parent.DoClickSeries(this, part.ASeries, part.PointIndex, e);
                }
                this.IClicked = this.CancelMouse;
            }
            if (!this.IClicked)
            {
                this.CheckZoomPanning(e, Shift);
            }
        Label_0205:
            if (!this.IClicked && (this.parent != null))
            {
                this.parent.CheckBackground(this, e);
            }
        Label_0222:
            this.CancelMouse = false;
        }

        public virtual void DoMouseMove(int x, int y, ref Cursor c)
        {
            if (!this.CancelMouse)
            {
                if ((this.zoom != null) && this.zoom.Active)
                {
                    if (this.zoom.Direction == ZoomDirections.Vertical)
                    {
                        x = this.ChartRect.Right;
                    }
                    if (this.zoom.Direction == ZoomDirections.Horizontal)
                    {
                        y = this.ChartRect.Bottom;
                    }
                    if ((x != this.zoom.x1) || (y != this.zoom.y1))
                    {
                        this.zoom.Draw();
                        this.zoom.x1 = x;
                        this.zoom.y1 = y;
                        this.zoom.Draw();
                    }
                }
                else if ((this.panning != null) && this.panning.Active)
                {
                    if (!this.ChartRect.Contains(x, y))
                    {
                        this.panning.Active = false;
                    }
                    else if ((x != this.panning.x1) || (y != this.panning.y1))
                    {
                        bool panned = false;
                        if (this.restoredAxisScales)
                        {
                            this.savedScales = this.SaveScales();
                            this.restoredAxisScales = false;
                        }
                        this.PanAxis(true, x, this.panning.x1, ref panned);
                        this.PanAxis(false, y, this.panning.y1, ref panned);
                        this.panning.x1 = x;
                        this.panning.y1 = y;
                        if (panned)
                        {
                            if (this.parent != null)
                            {
                                this.parent.DoScroll(this, EventArgs.Empty);
                            }
                            this.Invalidate();
                        }
                    }
                }
                else
                {
                    this.CheckMouseSeries(ref c, x, y);
                }
            }
        }

        public virtual void DoMouseUp(MouseEventArgs e, Keys Shift)
        {
            this.CancelMouse = false;
            if ((this.Zoom.Active && (Utils.GetMouseButton(e) == this.Zoom.MouseButton)) && ((this.Zoom.KeyShift == Keys.None) || (this.Zoom.KeyShift == Shift)))
            {
                this.zoom.Active = false;
                this.zoom.Draw();
                Point point = new Point(e.X, e.Y);
                int x = point.X;
                int y = point.Y;
                if (this.zoom.Direction == ZoomDirections.Vertical)
                {
                    x = this.ChartRect.Right;
                }
                if (this.zoom.Direction == ZoomDirections.Horizontal)
                {
                    y = this.ChartRect.Bottom;
                }
                this.zoom.x1 = x;
                this.zoom.y1 = y;
                if ((Math.Abs((int) (this.zoom.x1 - this.zoom.x0)) > this.zoom.MinPixels) && (Math.Abs((int) (this.zoom.y1 - this.zoom.y0)) > this.zoom.MinPixels))
                {
                    if ((this.zoom.x1 > this.zoom.x0) && (this.zoom.y1 > this.zoom.y0))
                    {
                        this.zoom.CalcZoomPoints();
                    }
                    else
                    {
                        this.zoom.Undo();
                    }
                    this.Invalidate();
                }
            }
            if (this.panning != null)
            {
                this.panning.Active = false;
            }
            this.BroadcastMouseEvent(MouseEventKinds.Up, e, Shift);
        }

        public void DoPanelPaint(Graphics g, Rectangle r)
        {
            this.DoPanelPaint(g, r, false);
        }

        internal void DoPanelPaint(Graphics g, Rectangle r, bool noTools)
        {
            if (this.panel.BorderRound > 0)
            {
                r.Inflate(1, 1);
                if (this.panel.Pen.Visible)
                {
                    r.Inflate(-this.panel.Pen.Width, -this.panel.Pen.Width);
                }
            }
            else if (((this.panel.ImageBevel != null) && this.panel.ImageBevel.Visible) && this.panel.ImageBevel.Pen.Visible)
            {
                r.Offset(-1, -1);
                r.Inflate(-1, -1);
            }
            this.ChartRect = r;
            this.chartBounds = r;
            if (!noTools)
            {
                this.graphics3D.InitWindow(g, this.aspect, this.ChartRect, 100);
            }
            if (this.Parent != null)
            {
                this.Parent.DrawBackColor(this.graphics3D);
            }
            this.panel.Draw(this.graphics3D, ref this.ChartRect);
        }

        internal void DoZoom(double topi, double topf, double boti, double botf, double lefi, double leff, double rigi, double rigf)
        {
            if (this.restoredAxisScales)
            {
                this.savedScales = this.SaveScales();
                this.restoredAxisScales = false;
            }
            if (this.Zoom.Animated)
            {
                this.DoZoomAnimated(topi, topf, boti, botf, lefi, leff, rigi, rigf);
            }
            this.Axes.Left.SetMinMax(lefi, leff);
            this.Axes.Right.SetMinMax(rigi, rigf);
            this.Axes.Top.SetMinMax(topi, topf);
            this.Axes.Bottom.SetMinMax(boti, botf);
            this.zoom.Zoomed = true;
            if (this.parent != null)
            {
                this.parent.DoZoomed(this, EventArgs.Empty);
            }
        }

        private void DoZoomAnimated(double topi, double topf, double boti, double botf, double lefi, double leff, double rigi, double rigf)
        {
            for (int i = 1; i < this.Zoom.AnimatedSteps; i++)
            {
                this.ZoomAxis(this.Axes.Left, lefi, leff);
                this.ZoomAxis(this.Axes.Right, rigi, rigf);
                this.ZoomAxis(this.Axes.Top, topi, topf);
                this.ZoomAxis(this.Axes.Bottom, boti, botf);
                this.parent.RefreshControl();
            }
        }

        public void Draw(Graphics g)
        {
            this.Draw(g, new Rectangle(0, 0, this.chartBounds.Width, this.chartBounds.Height));
        }

        public void Draw(Graphics g, bool noTools)
        {
            this.Draw(g, new Rectangle(0, 0, this.chartBounds.Width, this.chartBounds.Height), noTools);
        }

        public void Draw(Graphics g, Rectangle r)
        {
            this.Draw(g, r, false);
        }

        public void Draw(Graphics g, Rectangle r, bool noTools)
        {
            bool autoRepaint = this.AutoRepaint;
            this.AutoRepaint = false;
            try
            {
                this.DoPanelPaint(g, r, noTools);
                if (this.parent != null)
                {
                    this.parent.DoBeforeDraw();
                }
                this.InternalDraw(g, noTools);
                if (this.parent != null)
                {
                    this.parent.DoAfterDraw();
                }
            }
            finally
            {
                this.graphics3D.ShowImage(g);
                this.AutoRepaint = autoRepaint;
            }
        }

        private void DrawAxisAfter(Axis axis)
        {
            if (this.IsAxisVisible(axis))
            {
                axis.iHideBackGrid = true;
                axis.Draw(false);
                axis.iHideBackGrid = false;
            }
        }

        private void DrawAxisGridAfter(Axis axis)
        {
            if (this.IsAxisVisible(axis))
            {
                axis.iHideSideGrid = true;
                axis.iHideSideGrid = false;
            }
        }

        internal bool DrawBackWallAfter(int Z)
        {
            return !Steema.TeeChart.Drawing.Graphics3D.Cull(this.graphics3D.FourPointsFromRect(this.ChartRect, Z));
        }

        internal bool DrawBottomWallFirst()
        {
            return this.DrawHorizWallFirst(base.chart.Height);
        }

        private bool DrawHorizWallFirst(int aPos)
        {
            Point[] p = new Point[4];
            p[0] = this.graphics3D.Calculate3DPosition(this.ChartRect.Left, aPos, 0);
            int x = this.ChartRect.Left + this.walls.CalcWallSize(this.axes.Left);
            p[1] = this.graphics3D.Calculate3DPosition(x, aPos, 0);
            p[2] = this.graphics3D.Calculate3DPosition(x, aPos, this.aspect.Width3D + this.walls.Back.Size);
            return Steema.TeeChart.Drawing.Graphics3D.Cull(p);
        }

        internal bool DrawLeftWallFirst()
        {
            return this.DrawVertWallFirst(base.chart.Left);
        }

        internal bool DrawRightWallAfter()
        {
            Point point = this.graphics3D.Calc3DPoint(this.ChartRect.Right, this.ChartRect.Y, 0);
            Point point2 = this.graphics3D.Calc3DPoint(this.ChartRect.Right, this.ChartRect.Bottom + this.walls.CalcWallSize(this.axes.Bottom), this.Aspect.Width3D + this.walls.Back.Size);
            return (point.X <= point2.X);
        }

        private void DrawTitleFoot(ref Rectangle rect, bool CustomOnly)
        {
            this.header.DoDraw(this.graphics3D, ref rect, CustomOnly);
            this.subHeader.DoDraw(this.graphics3D, ref rect, CustomOnly);
            this.footer.DoDraw(this.graphics3D, ref rect, CustomOnly);
            this.subFooter.DoDraw(this.graphics3D, ref rect, CustomOnly);
        }

        private void DrawTitlesAndLegend(Graphics g, ref Rectangle tmp, bool BeforeSeries)
        {
            this.Graphics3D.FrontPlaneBegin();
            if (BeforeSeries)
            {
                if (!this.legend.CustomPosition && this.ShouldDrawLegend())
                {
                    if (this.legend.Vertical)
                    {
                        this.DoDrawLegend(ref tmp);
                        this.DrawTitleFoot(ref tmp, false);
                    }
                    else
                    {
                        this.DrawTitleFoot(ref tmp, false);
                        this.DoDrawLegend(ref tmp);
                    }
                }
                else
                {
                    this.DrawTitleFoot(ref tmp, false);
                }
            }
            else
            {
                if (this.legend.CustomPosition && this.ShouldDrawLegend())
                {
                    this.DoDrawLegend(ref tmp);
                }
                this.DrawTitleFoot(ref tmp, true);
            }
            this.Graphics3D.FrontPlaneEnd();
            if ((!BeforeSeries && this.ActiveSeriesUseAxis()) && (this.Aspect.View3D && this.walls.View3D))
            {
                if (this.walls.Right.Visible && this.DrawRightWallAfter())
                {
                    if (this.walls.Right.ShouldDraw)
                    {
                        this.walls.Right.Paint(this.graphics3D, tmp);
                    }
                    this.DrawAxisAfter(this.axes.Right);
                }
                if ((this.walls.Left.Visible && this.DrawLeftWallFirst()) && (!this.Aspect.Orthogonal && (this.Aspect.Rotation < 270)))
                {
                    if (this.walls.Left.ShouldDraw)
                    {
                        this.walls.Left.Paint(this.graphics3D, tmp);
                    }
                    this.DrawAxisAfter(this.axes.Left);
                }
                if (this.walls.Back.visible && this.DrawBackWallAfter(this.aspect.Width3D))
                {
                    this.DrawAxisGridAfter(this.axes.Top);
                    this.DrawAxisGridAfter(this.axes.Bottom);
                    if (this.walls.Back.ShouldDraw)
                    {
                        this.walls.Back.Paint(this.graphics3D, tmp);
                    }
                }
                if ((this.walls.Bottom.Visible && this.DrawBottomWallFirst()) && !this.Aspect.Orthogonal)
                {
                    if (this.walls.Bottom.ShouldDraw)
                    {
                        this.walls.Bottom.Paint(this.graphics3D, tmp);
                    }
                    this.DrawAxisAfter(this.axes.Bottom);
                }
            }
        }

        private bool DrawVertWallFirst(int APos)
        {
            Point[] p = new Point[4];
            p[0] = this.graphics3D.Calculate3DPosition(APos, this.ChartRect.Top, 0);
            int y = this.ChartRect.Bottom + this.walls.CalcWallSize(this.axes.Bottom);
            p[1] = this.graphics3D.Calculate3DPosition(APos, y, 0);
            p[2] = this.graphics3D.Calculate3DPosition(APos, y, this.aspect.Width3D + this.walls.Back.Size);
            return Steema.TeeChart.Drawing.Graphics3D.Cull(p);
        }

        public string FormattedLegend(int seriesOrValueIndex)
        {
            string text = this.legend.FormattedLegend(seriesOrValueIndex);
            if (this.parent != null)
            {
                this.parent.DoGetLegendText(this, this.legend.iLegendStyle, seriesOrValueIndex, ref text);
            }
            return text;
        }

        public string FormattedValueLegend(Steema.TeeChart.Styles.Series aSeries, int valueIndex)
        {
            if (aSeries == null)
            {
                return "";
            }
            return this.legend.FormattedValue(aSeries, valueIndex);
        }

        public Color FreeSeriesColor(bool checkBackground)
        {
            int index = 0;
            do
            {
                Color defaultColor = Steema.TeeChart.Drawing.Graphics3D.GetDefaultColor(index);
                if (this.IsFreeSeriesColor(defaultColor, checkBackground))
                {
                    return defaultColor;
                }
                index++;
            }
            while (index < Steema.TeeChart.Drawing.Graphics3D.ColorPalette.Length);
            return Steema.TeeChart.Drawing.Graphics3D.ColorPalette[0];
        }

        public Steema.TeeChart.Styles.Series GetASeries()
        {
            foreach (Steema.TeeChart.Styles.Series series in this.series)
            {
                if (series.bActive)
                {
                    return series;
                }
            }
            return null;
        }

        public Steema.TeeChart.Styles.Series GetAxisSeries(Axis axis)
        {
            foreach (Steema.TeeChart.Styles.Series series in this.series)
            {
                if ((series.bActive || this.NoActiveSeries(axis)) && series.AssociatedToAxis(axis))
                {
                    return series;
                }
            }
            return null;
        }

        private Steema.TeeChart.Styles.Series GetAxisSeriesMaxPoints(Axis aAxis)
        {
            Steema.TeeChart.Styles.Series series2 = null;
            int count = -1;
            for (int i = 0; i < this.Series.Count; i++)
            {
                Steema.TeeChart.Styles.Series series = this.Series[i];
                if ((series.Active || this.NoActiveSeries(aAxis)) && (series.AssociatedToAxis(aAxis) && (series.Count > count)))
                {
                    count = series.Count;
                    series2 = series;
                }
            }
            return series2;
        }

        internal int GetMaxValuesCount()
        {
            int count = 0;
            bool flag = true;
            foreach (Steema.TeeChart.Styles.Series series in this.series)
            {
                if (series.bActive && (flag || ((series.Count > count) && (series.Function == null))))
                {
                    count = series.Count;
                    flag = false;
                }
            }
            return count;
        }

        [SecurityPermission(SecurityAction.Demand)]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            this.export.Template.Serialize(info, context);
        }

        protected void initVars()
        {
            ColorPalettes.ApplyPalette(this, 13);
            if (!English.TextsOk && !base.DesignMode)
            {
                DesignTimeOptions.InitLanguage(false);
            }
            this.listeners = new List<ITeeEventListener>();
            base.chart = this;
            this.series = new SeriesCollection(this);
            this.tools = new ToolsCollection(this);
            this.aspect = new Steema.TeeChart.Drawing.Aspect(this);
            this.graphics3D = new Graphics3DGdiPlus(this);
            this.panel = new Steema.TeeChart.Panel(this);
            this.legend = new Steema.TeeChart.Legend(this);
            this.header = new Steema.TeeChart.Header(this, true);
            this.header.defaultText = "TeeChart";
            this.header.Text = this.header.defaultText;
            this.subHeader = new Steema.TeeChart.Header(this, false);
            this.footer = new Steema.TeeChart.Footer(this, false);
            this.subFooter = new Steema.TeeChart.Footer(this, false);
            this.walls = new Steema.TeeChart.Walls(this);
            this.axes = new Steema.TeeChart.Axes(this);
            this.iPage = new Steema.TeeChart.Page(this);
            this.import = new Imports(this);
            this.export = new Exports(this);
            this.printer = new Steema.TeeChart.Printer(this);
        }

        internal void InternalDraw(Graphics g)
        {
            this.InternalDraw(g, false);
        }

        internal void InternalDraw(Graphics g, bool noTools)
        {
            Rectangle chartRect = this.ChartRect;
            if (!noTools)
            {
                this.BroadcastToolEvent(new BeforeDrawEventArgs());
            }
            this.redrawing = true;
            this.CalcInvertedRotation();
            foreach (Steema.TeeChart.Styles.Series series in this.series)
            {
                if (series.bActive)
                {
                    series.DoBeforeDrawChart();
                }
            }
            if (!this.Graphics3D.SupportsFullRotation)
            {
                this.DrawTitlesAndLegend(g, ref chartRect, true);
            }
            this.ChartRect = chartRect;
            this.SetSeriesZOrder();
            this.CalcWallsRect(ref chartRect);
            this.ChartRect = chartRect;
            this.CalcAxisRect();
            if (this.Parent != null)
            {
                this.Parent.DoGetAxesChartRect(this, ref this.ChartRect);
            }
            chartRect = this.ChartRect;
            this.SetSeriesZPositions();
            this.CalcSeriesRect();
            this.graphics3D.Projection(this.aspect.Width3D, this.ChartRect);
            if (this.Graphics3D.SupportsFullRotation)
            {
                Rectangle rectangle2 = chartRect;
                this.DrawTitlesAndLegend(g, ref chartRect, true);
                chartRect = rectangle2;
                this.ChartRect = rectangle2;
            }
            if (this.series.ActiveUseAxis() && this.walls.Visible)
            {
                this.walls.Paint(this.graphics3D, chartRect);
            }
            if (this.axes.DrawBehind)
            {
                if (!noTools)
                {
                    this.BroadcastToolEvent(new BeforeDrawAxesEventArgs());
                }
                this.axes.Draw(this.graphics3D);
            }
            if (!noTools)
            {
                this.BroadcastToolEvent(new BeforeDrawSeriesEventArgs());
            }
            if (this.parent != null)
            {
                this.parent.DoBeforeDrawSeries();
            }
            StringAlignment textAlign = this.graphics3D.TextAlign;
            if (this.axes.depth.inverted)
            {
                for (int i = this.series.Count - 1; i >= 0; i--)
                {
                    if (this.Series[i].Active)
                    {
                        this.Series[i].DrawSeries();
                    }
                }
            }
            else
            {
                for (int j = 0; j < this.series.Count; j++)
                {
                    if (this.series[j].bActive)
                    {
                        this.series[j].DrawSeries();
                    }
                }
            }
            if (!noTools)
            {
                this.BroadcastToolEvent(new AfterDrawSeriesEventsArgs());
            }
            this.graphics3D.TextAlign = textAlign;
            if (!this.axes.DrawBehind)
            {
                if (!noTools)
                {
                    this.BroadcastToolEvent(new BeforeDrawAxesEventArgs());
                }
                this.axes.Draw(this.graphics3D);
            }
            this.DrawTitlesAndLegend(g, ref chartRect, false);
            if (!noTools)
            {
                this.BroadcastToolEvent(new AfterDrawEventArgs());
            }
        }

        protected internal double InternalMinMax(Axis aAxis, bool isMin, bool isX)
        {
            bool flag;
            double num;
            double num2;
            ValueList list;
            int first = 0;
            int last = 0;
            if (aAxis.IsDepthAxis)
            {
                if (aAxis.CalcLabelStyle() == AxisLabelStyle.Value)
                {
                    num2 = 0.0;
                    flag = true;
                    foreach (Steema.TeeChart.Styles.Series series in this.series)
                    {
                        if (series.bActive)
                        {
                            num = isMin ? series.MinZValue() : series.MaxZValue();
                            if ((flag || (isMin && (num < num2))) || (!isMin && (num > num2)))
                            {
                                flag = false;
                                num2 = num;
                            }
                        }
                    }
                    return num2;
                }
                return (isMin ? -0.5 : (this.maxZOrder + 0.5));
            }
            num2 = 0.0;
            Steema.TeeChart.Styles.Series axisSeries = this.GetAxisSeries(aAxis);
            bool flag3 = (axisSeries != null) ? (axisSeries.yMandatory ? isX : !isX) : isX;
            if ((this.iPage.MaxPointsPerPage > 0) && flag3)
            {
                axisSeries = this.GetAxisSeriesMaxPoints(aAxis);
                if ((axisSeries == null) || (axisSeries.Count <= 0))
                {
                    return num2;
                }
                axisSeries.CalcFirstLastPage(ref first, ref last);
                list = isX ? axisSeries.XValues : axisSeries.YValues;
                if (isMin)
                {
                    return list[first];
                }
                num2 = list[last];
                if (this.iPage.ScaleLastPage)
                {
                    return num2;
                }
                int num6 = (last - first) + 1;
                if (num6 >= this.iPage.MaxPointsPerPage)
                {
                    return num2;
                }
                num = list[first];
                if (num == num2)
                {
                    return (num + (this.iPage.MaxPointsPerPage / num6));
                }
                return (num + ((this.iPage.MaxPointsPerPage * (num2 - num)) / ((double) num6)));
            }
            bool flag2 = (this.iPage.MaxPointsPerPage > 0) && this.iPage.AutoScale;
            flag = true;
            foreach (Steema.TeeChart.Styles.Series series3 in this.series)
            {
                if (((series3.bActive || this.NoActiveSeries(aAxis)) && (series3.Count > 0)) && ((isX && ((series3.HorizAxis == HorizontalAxis.Both) || (series3.GetHorizAxis == aAxis))) || (!isX && ((series3.VertAxis == VerticalAxis.Both) || (series3.GetVertAxis == aAxis)))))
                {
                    if (flag2)
                    {
                        int num3 = this.iPage.FirstValueIndex();
                        if (series3.Count > num3)
                        {
                            list = isX ? axisSeries.XValues : axisSeries.YValues;
                            num = list[num3];
                            for (int i = num3 + 1; i < Math.Min(num3 + this.iPage.MaxPointsPerPage, series3.Count); i++)
                            {
                                double num8 = list[i];
                                if (isMin)
                                {
                                    if (num8 < num)
                                    {
                                        num = num8;
                                    }
                                }
                                else if (num8 > num)
                                {
                                    num = num8;
                                }
                            }
                            this.CalcResult(num, isMin, ref num2, ref flag);
                        }
                    }
                    else
                    {
                        if (isMin)
                        {
                            num = isX ? series3.MinXValue() : series3.MinYValue();
                        }
                        else
                        {
                            num = isX ? series3.MaxXValue() : series3.MaxYValue();
                        }
                        this.CalcResult(num, isMin, ref num2, ref flag);
                    }
                }
            }
            return num2;
        }

        public bool IsAxisVisible(Axis a)
        {
            bool flag = this.axes.Visible && a.Visible;
            if (flag)
            {
                if (a.IsDepthAxis)
                {
                    return this.aspect.view3D;
                }
                foreach (Steema.TeeChart.Styles.Series series in this.series)
                {
                    if (series.bActive)
                    {
                        if (series.UseAxis)
                        {
                            flag = series.AssociatedToAxis(a);
                            if (flag)
                            {
                                return true;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            return flag;
        }

        public bool IsFreeSeriesColor(Color color, bool checkBackground)
        {
            bool flag = checkBackground && ((color == this.panel.Color) || (color == this.walls.Back.Color));
            foreach (Steema.TeeChart.Styles.Series series in this.series)
            {
                if ((series.Color == color) || flag)
                {
                    return false;
                }
            }
            return !flag;
        }

        protected internal bool IsValidDataSource(Steema.TeeChart.Styles.Series s, object source)
        {
            bool flag = (((s != source) && (source is Steema.TeeChart.Styles.Series)) && s.IsValidSourceOf((Steema.TeeChart.Styles.Series) source)) && s.IsValidSeriesSource((Steema.TeeChart.Styles.Series) source);
            if (!flag)
            {
                flag = DataSeriesSource.IsValidSource(source);
            }
            return flag;
        }

        public int MaxMarkWidth()
        {
            int num = 0;
            foreach (Steema.TeeChart.Styles.Series series in this.series)
            {
                if (series.Active)
                {
                    num = Math.Max(num, series.MaxMarkWidth());
                }
            }
            return num;
        }

        public int MaxTextWidth()
        {
            int num = 0;
            foreach (Steema.TeeChart.Styles.Series series in this.series)
            {
                if (series.Labels.Count > 0)
                {
                    for (int i = 0; i < series.Count; i++)
                    {
                        num = Math.Max(num, this.MultiLineTextWidth(series.sLabels[i]));
                    }
                }
            }
            return num;
        }

        public double MaxXValue(Axis axis)
        {
            return this.InternalMinMax(axis, false, true);
        }

        public double MaxYValue(Axis axis)
        {
            return this.InternalMinMax(axis, false, false);
        }

        public System.Drawing.Imaging.Metafile Metafile(Chart aChart, int width, int height)
        {
            return this.Metafile(new MemoryStream(), aChart, width, height);
        }

        public System.Drawing.Imaging.Metafile Metafile(Stream stream, Chart aChart, int width, int height)
        {
            return this.Metafile(stream, aChart, width, height, EmfType.EmfOnly);
        }

        public System.Drawing.Imaging.Metafile Metafile(Stream stream, Chart aChart, int width, int height, EmfType type)
        {
            System.Drawing.Bitmap image = new System.Drawing.Bitmap(1, 1, PixelFormat.Format24bppRgb);
            Graphics gRef = Graphics.FromImage(image);
            return this.Metafile(gRef, stream, aChart, width, height, type);
        }

        public System.Drawing.Imaging.Metafile Metafile(Graphics gRef, Stream stream, Chart aChart, int width, int height, EmfType type)
        {
            IntPtr hdc = gRef.GetHdc();
            Rectangle frameRect = new Rectangle(0, 0, width, height);
            System.Drawing.Imaging.Metafile image = new System.Drawing.Imaging.Metafile(stream, hdc, frameRect, MetafileFrameUnit.Pixel, type);
            gRef.ReleaseHdc(hdc);
            gRef.Dispose();
            gRef = Graphics.FromImage(image);
            aChart.graphics3D.metafiling = true;
            try
            {
                aChart.Draw(gRef, new Rectangle(0, 0, width, height));
            }
            finally
            {
                aChart.graphics3D.metafiling = false;
                gRef.Dispose();
            }
            return image;
        }

        public double MinXValue(Axis axis)
        {
            return this.InternalMinMax(axis, true, true);
        }

        public double MinYValue(Axis axis)
        {
            return this.InternalMinMax(axis, true, false);
        }

        protected internal int MultiLineTextWidth(string s)
        {
            int num;
            return this.MultiLineTextWidth(s, out num);
        }

        protected internal int MultiLineTextWidth(string s, out int numLines)
        {
            int index = s.IndexOf('\n');
            int num2 = 0;
            numLines = 0;
            while (index > 0)
            {
                num2 = Utils.Round(Math.Max((float) num2, this.graphics3D.TextWidth(s.Substring(1, index - 1))));
                numLines++;
                s = s.Remove(1, index);
                index = s.IndexOf('\n');
            }
            if (s != "")
            {
                num2 = Utils.Round(Math.Max((float) num2, this.graphics3D.TextWidth(s)));
                numLines++;
            }
            return num2;
        }

        private bool NoActiveSeries(Axis a)
        {
            for (int i = 0; i < this.series.Count; i++)
            {
                Steema.TeeChart.Styles.Series series = this.series[i];
                if (series.Active && series.AssociatedToAxis(a))
                {
                    return false;
                }
            }
            return true;
        }

        internal int NumPages()
        {
            if ((this.iPage.MaxPointsPerPage <= 0) || (this.Series.Count <= 0))
            {
                return 1;
            }
            if (this.Series[0].yMandatory)
            {
                return Math.Max(this.CalcNumPages(this.Axes.Top), this.CalcNumPages(this.Axes.Bottom));
            }
            return Math.Max(this.CalcNumPages(this.Axes.Left), this.CalcNumPages(this.Axes.Right));
        }

        private void PanAxis(bool AxisHorizontal, int Pos1, int Pos2, ref bool Panned)
        {
            ScrollModes modes = AxisHorizontal ? ScrollModes.Horizontal : ScrollModes.Vertical;
            if ((Pos1 != Pos2) && ((this.panning.Allow == modes) || (this.panning.Allow == ScrollModes.Both)))
            {
                if (AxisHorizontal)
                {
                    this.ProcessPanning(this.Axes.Top, Pos2, Pos1);
                    this.ProcessPanning(this.Axes.Bottom, Pos2, Pos1);
                }
                else
                {
                    this.ProcessPanning(this.Axes.Left, Pos2, Pos1);
                    this.ProcessPanning(this.Axes.Right, Pos2, Pos1);
                }
                for (int i = 0; i < this.Axes.Custom.Count; i++)
                {
                    Axis a = this.Axes.Custom[i];
                    if (!a.IsDepthAxis && ((AxisHorizontal && a.Horizontal) || (!AxisHorizontal && !a.Horizontal)))
                    {
                        this.ProcessPanning(a, Pos2, Pos1);
                    }
                }
                Panned = true;
            }
        }

        private void ProcessPanning(Axis a, int IniPos, int EndPos)
        {
            double delta = a.CalcPosPoint(IniPos) - a.CalcPosPoint(EndPos);
            double min = a.Minimum + delta;
            double max = a.Maximum + delta;
            if ((this.parent != null) && this.parent.DoAllowScroll(a, delta, ref min, ref max))
            {
                a.SetMinMax(min, max);
            }
        }

        internal void RecalcWidthHeight(ref Rectangle r)
        {
            int left = (r.Left < this.chartBounds.Left) ? this.chartBounds.Left : r.Left;
            int top = (r.Top < this.chartBounds.Top) ? this.chartBounds.Top : r.Top;
            int right = (r.Right < this.chartBounds.Left) ? (left + 1) : ((r.Right == this.chartBounds.Left) ? (left + this.chartBounds.Width) : r.Right);
            int bottom = (r.Bottom < this.chartBounds.Top) ? (top + 1) : ((r.Bottom == this.chartBounds.Top) ? (top + this.chartBounds.Height) : r.Bottom);
            r = Utils.FromLTRB(left, top, right, bottom);
            this.graphics3D.XCenter = (r.X + r.Right) / 2;
            this.graphics3D.YCenter = (r.Y + r.Bottom) / 2;
        }

        internal void RemoveAllComponents()
        {
            this.series.Clear();
            this.tools.Clear();
            this.axes.custom.Clear();
        }

        internal bool RemoveFromContainer(IComponent component)
        {
            bool isInternal = false;
            if (component is TeeBase)
            {
                isInternal = (component as TeeBase).InternalUse;
            }
            return this.RemoveFromContainer(component, isInternal);
        }

        internal bool RemoveFromContainer(IComponent component, bool isInternal)
        {
            bool flag = false;
            if (this.Parent != null)
            {
                flag = this.Parent.IsWebForm();
            }
            if (!flag)
            {
                flag = isInternal;
            }
            IContainer chartContainer = this.ChartContainer;
            if ((chartContainer != null) && !flag)
            {
                chartContainer.Remove(component);
                return true;
            }
            return false;
        }

        internal void RemoveListener(ITeeEventListener sender)
        {
            if (this.listeners != null)
            {
                this.listeners.Remove(sender);
            }
        }

        internal void RestoreAxisScales()
        {
            if (!this.restoredAxisScales)
            {
                this.RestoreScales(this.savedScales);
                this.restoredAxisScales = true;
            }
        }

        private void RestoreAxisScales(Axis a, AxisSavedScales tmp)
        {
            a.Automatic = tmp.Auto;
            a.AutomaticMinimum = tmp.AutoMin;
            a.AutomaticMaximum = tmp.AutoMax;
            if (!a.Automatic)
            {
                a.SetMinMax(tmp.Min, tmp.Max);
            }
        }

        private void RestoreScales(AllAxisSavedScales s)
        {
            this.RestoreAxisScales(this.Axes.Top, s.Top);
            this.RestoreAxisScales(this.Axes.Bottom, s.Bottom);
            this.RestoreAxisScales(this.Axes.Left, s.Left);
            this.RestoreAxisScales(this.Axes.Right, s.Right);
        }

        private void SaveAxisScales(Axis a, ref AxisSavedScales tmp)
        {
            tmp.Auto = a.Automatic;
            tmp.AutoMin = a.AutomaticMinimum;
            tmp.AutoMax = a.AutomaticMaximum;
            tmp.Min = a.Minimum;
            tmp.Max = a.Maximum;
        }

        private AllAxisSavedScales SaveScales()
        {
            AllAxisSavedScales scales = new AllAxisSavedScales();
            this.SaveAxisScales(this.Axes.Top, ref scales.Top);
            this.SaveAxisScales(this.Axes.Bottom, ref scales.Bottom);
            this.SaveAxisScales(this.Axes.Left, ref scales.Left);
            this.SaveAxisScales(this.Axes.Right, ref scales.Right);
            return scales;
        }

        public Steema.TeeChart.Styles.Series SeriesLegend(int itemIndex, bool onlyActive)
        {
            int num = 0;
            for (int i = 0; i < this.series.Count; i++)
            {
                Steema.TeeChart.Styles.Series series = this.series[i];
                if (series.ShowInLegend && (!onlyActive || series.bActive))
                {
                    if (num == itemIndex)
                    {
                        return series;
                    }
                    num++;
                }
            }
            return null;
        }

        public string SeriesTitleLegend(int seriesIndex, bool onlyActive)
        {
            Steema.TeeChart.Styles.Series series;
            if (onlyActive)
            {
                series = this.ActiveSeriesLegend(seriesIndex);
            }
            else
            {
                series = this.SeriesLegend(seriesIndex, false);
            }
            if (series == null)
            {
                return "";
            }
            return series.ToString();
        }

        internal void SetBrushCanvas(Color AColor, ChartBrush ABrush, Color ABackColor)
        {
            this.graphics3D.Brush = ABrush;
            this.graphics3D.Brush.Color = AColor;
        }

        private void SetSeriesZOrder()
        {
            this.maxZOrder = 0;
            bool flag = this.aspect.ApplyZOrder && this.aspect.View3D;
            if (flag)
            {
                this.maxZOrder = -1;
                foreach (Steema.TeeChart.Styles.Series series in this.series)
                {
                    if (series.Active)
                    {
                        series.CalcZOrder();
                    }
                }
            }
            if (!this.axes.Depth.inverted)
            {
                foreach (Steema.TeeChart.Styles.Series series2 in this.series)
                {
                    if (series2.bActive)
                    {
                        series2.iZOrder = flag ? (this.maxZOrder - series2.ZOrder) : 0;
                    }
                }
            }
        }

        private void SetSeriesZPositions()
        {
            for (int i = 0; i < this.series.Count; i++)
            {
                if (this.series[i].Active)
                {
                    this.series[i].CalcDepthPositions();
                }
            }
        }

        private bool ShouldDrawLegend()
        {
            if (!this.legend.Visible)
            {
                return false;
            }
            if (!this.legend.HasCheckBoxes())
            {
                return (this.CountActiveSeries() > 0);
            }
            return true;
        }

        protected virtual bool ShouldSerializeListeners()
        {
            return false;
        }

        [Obsolete("Please use tChart1.Zoom.Undo method."), EditorBrowsable(EditorBrowsableState.Never)]
        public void UndoZoom()
        {
            this.zoom.Undo();
        }

        private void ZoomAxis(Axis a, double tmpA, double tmpB)
        {
            a.SetMinMax((double) (a.Minimum + ((tmpA - a.Minimum) / Steema.TeeChart.Zoom.AnimatedFactor)), (double) (a.Maximum - ((a.Maximum - tmpB) / Steema.TeeChart.Zoom.AnimatedFactor)));
        }

        [Description("3D view parameters."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Steema.TeeChart.Drawing.Aspect Aspect
        {
            get
            {
                return this.aspect;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Collection of predefined and custom axis objects.")]
        public Steema.TeeChart.Axes Axes
        {
            get
            {
                return this.axes;
            }
        }

        [Browsable(false), Description("Read only. Used to get the four sides of the Chart.")]
        public Rectangle ChartBounds
        {
            get
            {
                return this.chartBounds;
            }
        }

        internal IContainer ChartContainer
        {
            get
            {
                if (this.parent != null)
                {
                    if (this.parent.GetContainer() != null)
                    {
                        return this.parent.GetContainer();
                    }
                    Form form = this.parent.FindParentForm() as Form;
                    if (form != null)
                    {
                        return form.Container;
                    }
                }
                return null;
            }
        }

        internal int ChartRectBottom
        {
            get
            {
                return this.ChartRect.Bottom;
            }
        }

        internal int ChartRectHeight
        {
            get
            {
                return this.ChartRect.Height;
            }
        }

        internal int ChartRectTop
        {
            get
            {
                return this.ChartRect.Top;
            }
        }

        internal int ChartRectWidth
        {
            get
            {
                return this.ChartRect.Width;
            }
        }

        [Description("Accesses Chart export properties and methods."), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public Exports Export
        {
            get
            {
                return this.export;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("Titles"), Description("Defines Text shown at the bottom of the Chart.")]
        public Steema.TeeChart.Footer Footer
        {
            get
            {
                return this.footer;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Description("Accesses TeeChart Draw properties and methods.")]
        public Steema.TeeChart.Drawing.Graphics3D Graphics3D
        {
            get
            {
                return this.graphics3D;
            }
            set
            {
                this.graphics3D = value;
                this.Invalidate();
            }
        }

        [Description("Defines Text shown at top of the Chart."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("Titles")]
        public Steema.TeeChart.Header Header
        {
            get
            {
                return this.header;
            }
        }

        [Description("Sets the Chart Height in pixels."), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int Height
        {
            get
            {
                return this.chartBounds.Height;
            }
            set
            {
                this.chartBounds.Height = value;
                this.Invalidate();
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false), Description("Accesses Chart import properties and methods.")]
        public Imports Import
        {
            get
            {
                return this.import;
            }
        }

        public Steema.TeeChart.Styles.Series this[int index]
        {
            get
            {
                return this.series[index];
            }
            set
            {
                this.series[index] = value;
            }
        }

        internal int Left
        {
            get
            {
                return this.chartBounds.Left;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Properties for Chart Legend.")]
        public Steema.TeeChart.Legend Legend
        {
            get
            {
                return this.legend;
            }
        }

        public List<ITeeEventListener> Listeners
        {
            get
            {
                return this.listeners;
            }
            set
            {
                this.listeners = value;
            }
        }

        [Description("Maximum Z order of all active Series."), Browsable(false)]
        public int MaxZOrder
        {
            get
            {
                return this.maxZOrder;
            }
        }

        [Description("Properties to define multiple pages."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Steema.TeeChart.Page Page
        {
            get
            {
                return this.iPage;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Background visible attributes.")]
        public Steema.TeeChart.Panel Panel
        {
            get
            {
                return this.panel;
            }
        }

        [Description("Sets the scrolling direction or denies scrolling."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Scroll Panning
        {
            get
            {
                if (this.panning == null)
                {
                    this.panning = new Scroll(this);
                }
                return this.panning;
            }
        }

        [Description("The IChart parent of the Chart."), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IChart Parent
        {
            get
            {
                return this.parent;
            }
            set
            {
                this.parent = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Printing related properties.")]
        public Steema.TeeChart.Printer Printer
        {
            get
            {
                return this.printer;
            }
        }

        internal int Right
        {
            get
            {
                return this.chartBounds.Right;
            }
        }

        [Browsable(false), DefaultValue((string) null), Description("Holds a custom tag object which is serialized with the chart.")]
        public object SerializedTag
        {
            get
            {
                return this.serializedTag;
            }
            set
            {
                this.serializedTag = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Collection of Series contained in this Chart.")]
        public SeriesCollection Series
        {
            get
            {
                return this.series;
            }
        }

        [Description("Defines Text shown directly above Footer."), Category("Titles"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Steema.TeeChart.Footer SubFooter
        {
            get
            {
                return this.subFooter;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("Titles"), Description("Defines Text shown directly below Header.")]
        public Steema.TeeChart.Header SubHeader
        {
            get
            {
                return this.subHeader;
            }
        }

        [Browsable(false), Obsolete("Please use SubHeader property."), EditorBrowsable(EditorBrowsableState.Never)]
        public Steema.TeeChart.Header SubTitle
        {
            get
            {
                return this.subHeader;
            }
        }

        [Obsolete("Please use Header property."), Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public Steema.TeeChart.Header Title
        {
            get
            {
                return this.header;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Collection of Tool components contained in this Chart.")]
        public ToolsCollection Tools
        {
            get
            {
                return this.tools;
            }
        }

        [Description("Displays a text box at the cursor."), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ChartToolTip ToolTip
        {
            get
            {
                if (this.toolTip == null)
                {
                    this.toolTip = new ChartToolTip(this);
                }
                return this.toolTip;
            }
        }

        internal int Top
        {
            get
            {
                return this.chartBounds.Top;
            }
        }

        [Description("Accesses wall characteristics of the Chart."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Steema.TeeChart.Walls Walls
        {
            get
            {
                return this.walls;
            }
        }

        [Description("Sets the Chart Width in pixels."), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int Width
        {
            get
            {
                return this.chartBounds.Width;
            }
            set
            {
                this.chartBounds.Width = value;
                this.Invalidate();
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Accesses the Zoom characteristics of the Chart.")]
        public Steema.TeeChart.Zoom Zoom
        {
            get
            {
                if (this.zoom == null)
                {
                    this.zoom = new Steema.TeeChart.Zoom(this);
                }
                return this.zoom;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct AllAxisSavedScales
        {
            public Chart.AxisSavedScales Left;
            public Chart.AxisSavedScales Top;
            public Chart.AxisSavedScales Right;
            public Chart.AxisSavedScales Bottom;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct AxisSavedScales
        {
            public bool Auto;
            public bool AutoMin;
            public bool AutoMax;
            public double Min;
            public double Max;
        }

        public sealed class ChartToolTip
        {
            private Chart chart;
            private Control chartControl;
            public string Text;
            private ToolTip toolTip;

            public ChartToolTip(Chart c)
            {
                this.chart = c;
                this.toolTip = new ToolTip();
                this.toolTip.InitialDelay = 500;
                this.toolTip.AutoPopDelay = 0x9c4;
            }

            public void Hide()
            {
                this.toolTip.Active = false;
            }

            public void Show()
            {
                this.toolTip.SetToolTip(this.ChartControl, this.Text);
                this.toolTip.Active = false;
                this.toolTip.Active = true;
            }

            [Description("Gets or sets the period of time the ToolTip remains visible if the pointer is stationary on a control with specified ToolTip text."), DefaultValue(0x9c4)]
            public int AutoPopDelay
            {
                get
                {
                    return this.toolTip.AutoPopDelay;
                }
                set
                {
                    this.toolTip.AutoPopDelay = value;
                }
            }

            private Control ChartControl
            {
                get
                {
                    if (this.chartControl == null)
                    {
                        this.chartControl = this.chart.parent.GetControl();
                    }
                    return this.chartControl;
                }
            }

            [DefaultValue(500), Description("Sets the time lag before the Tool Tip appears.")]
            public int InitialDelay
            {
                get
                {
                    return this.toolTip.InitialDelay;
                }
                set
                {
                    this.toolTip.InitialDelay = value;
                }
            }
        }
    }
}

