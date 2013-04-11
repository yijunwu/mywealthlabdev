namespace Steema.TeeChart
{
    using Steema.TeeChart.Drawing;
    using Steema.TeeChart.Editors;
    using Steema.TeeChart.Editors.Export;
    using Steema.TeeChart.Export;
    using Steema.TeeChart.Import;
    using Steema.TeeChart.Styles;
    using Steema.TeeChart.Tools;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.Design;
    using System.Diagnostics;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Imaging;
    using System.Drawing.Printing;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Runtime.Serialization;
    using System.Security.Permissions;
    using System.Windows.Forms;
    using System.Windows.Forms.Design;

    [Serializable, ToolboxBitmap(typeof(TChart), "Images.TChart.bmp"), LicenseProvider(typeof(FrAccessProvider)), Editor(typeof(TChart.ChartComponentEditor), typeof(System.ComponentModel.ComponentEditor)), DefaultProperty("Series"), Designer(typeof(TChart.ChartDesigner)), Guid("DA648F29-0B1B-33FE-96F6-24346FBF88AF")]
    public class TChart : Control, ISerializable, IChart
    {
        private Steema.TeeChart.Chart chart;
        private Container components;
        private bool iRedrawNeeded;
        private FrAccess license;
        private MouseButtons mouseButton;
        private bool refreshing;
        private Timer timer;

        public event PaintChartEventHandler AfterDraw;

        public event AllowScrollEventHandler AllowScroll;

        public event PaintChartEventHandler BeforeDraw;

        public event PaintChartEventHandler BeforeDrawAxes;

        public event PaintChartEventHandler BeforeDrawSeries;

        public event ChartPrintEventHandler ChartPrint;

        public event MouseEventHandler ClickAxis;

        public event MouseEventHandler ClickBackground;

        public event MouseEventHandler ClickLegend;

        public event SeriesEventHandler ClickSeries;

        public event MouseEventHandler ClickTitle;

        public event GetAxesChartRectEventHandler GetAxesChartRect;

        public event GetAxisLabelEventHandler GetAxisLabel;

        public event GetLegendPosEventHandler GetLegendPos;

        public event GetLegendRectEventHandler GetLegendRect;

        internal event GetLegendSizeEventHandler GetLegendSize;

        public event GetLegendTextEventHandler GetLegendText;

        public event GetNextAxisLabelEventHandler GetNextAxisLabel;

        public event EventHandler Scroll;

        public event EventHandler UndoneZoom;

        public event EventHandler Zoomed;

        public TChart()
        {
            this.iRedrawNeeded = true;
            this.InitializeComponent();
            this.chart = new Steema.TeeChart.Chart();
            this.chart.parent = this;
            this.license = (FrAccess) LicenseManager.Validate(typeof(TChart), this);
            this.initVars();
        }

        public TChart(SerializationInfo info, StreamingContext context) : this()
        {
            this.chart.Import.DeserializeFrom(info, context);
            base.Width = this.chart.Width;
            base.Height = this.chart.Height;
        }

        [EditorBrowsable(EditorBrowsableState.Advanced), Obsolete("Use Steema.TeeChart.Editors.AboutBox.ShowModal() method")]
        public void AboutBox()
        {
            Steema.TeeChart.Editors.AboutBox.ShowModal();
        }

        [EditorBrowsable(EditorBrowsableState.Never), Description("Adds a new Series into Chart."), Obsolete("Use Series.Add method instead.")]
        public Steema.TeeChart.Styles.Series AddSeries(Steema.TeeChart.Styles.Series s)
        {
            return this.chart.Series.Add(s);
        }

        private void CheckTimer()
        {
            if (this.timer == null)
            {
                this.timer = new Timer();
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Clear()
        {
            base.Controls.Clear();
            this.chart.Clear(this);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.components != null)
                {
                    this.components.Dispose();
                }
                if (this.license != null)
                {
                    this.license.Dispose();
                    this.license = null;
                }
                if (this.Chart.Graphics3D.BackBuffer != null)
                {
                    this.Chart.Graphics3D.BackBuffer.Dispose();
                    this.Chart.Graphics3D.BackBuffer = null;
                }
                this.chart.AutoRepaint = false;
                this.chart.RemoveAllComponents();
                this.chart.Dispose();
            }
            base.Dispose(disposing);
        }

        public virtual void DoInvalidate()
        {
            if (!this.refreshing)
            {
                this.refreshing = true;
                try
                {
                    if (this.Site != null)
                    {
                        IComponentChangeService service = (IComponentChangeService) this.Site.GetService(typeof(IComponentChangeService));
                        if (service != null)
                        {
                            service.OnComponentChanged(this, null, null, null);
                        }
                    }
                }
                finally
                {
                    this.refreshing = false;
                    base.Invalidate();
                }
            }
        }

        public void Draw()
        {
            System.Drawing.Bitmap bitmap = null;
            try
            {
                bitmap = this.Bitmap;
            }
            finally
            {
                bitmap.Dispose();
                bitmap = null;
            }
        }

        public virtual void Draw(Graphics g)
        {
            bool autoRepaint = this.chart.AutoRepaint;
            this.chart.AutoRepaint = false;
            this.chart.Graphics3D.oldRegion = new Region(new Rectangle(0, 0, 0, 0));
            this.chart.Graphics3D.hasClipRegion = false;
            try
            {
                if (this.Panel.BorderRound > 0)
                {
                    byte[] buffer;
                    Rectangle rect = Rectangle.FromLTRB(base.ClientRectangle.Left, base.ClientRectangle.Top, base.ClientRectangle.Right, base.ClientRectangle.Bottom);
                    rect.Inflate(-1, -1);
                    GraphicsPath path = new GraphicsPath(PointDouble.RoundF(this.chart.Graphics3D.GetClipRoundRectangle(out buffer, rect, this.Panel.BorderRound, this.Panel.BorderRound, 0)), buffer);
                    Region region = new Region(path);
                    base.Region = region;
                }
                else
                {
                    base.Region = null;
                }
                this.chart.DoPanelPaint(g, base.ClientRectangle);
                if (this.BackgroundImage != null)
                {
                    g.DrawImage(this.BackgroundImage, this.chart.ChartRect);
                }
                ((IChart) this).DoBeforeDraw();
                this.chart.InternalDraw(g);
                if ((this.chart.zoom != null) && this.chart.zoom.Active)
                {
                    this.chart.zoom.Draw();
                }
                ((IChart) this).DoAfterDraw();
            }
            finally
            {
                this.chart.graphics3D.ShowImage(g);
                this.chart.AutoRepaint = autoRepaint;
            }
        }

        [Obsolete("Use TChart.Series.Exchange() method instead."), EditorBrowsable(EditorBrowsableState.Never)]
        public void ExchangeSeries(int series1, int series2)
        {
            this.chart.Series.Exchange(series1, series2);
        }

        [SecurityPermission(SecurityAction.Demand)]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            this.chart.Export.Template.Serialize(info, context);
        }

        private void InitializeComponent()
        {
        }

        protected void initVars()
        {
            this.Text = this.Text;
            ((IChart) this).DoSetControlStyle();
        }

        protected override void OnDoubleClick(EventArgs e)
        {
            if (!base.DesignMode)
            {
                Point point = base.PointToClient(new Point(Control.MousePosition.X, Control.MousePosition.Y));
                MouseEventArgs args = new MouseEventArgs(this.mouseButton, 2, point.X, point.Y, 0);
                this.PrepareGraphics();
                this.chart.DoMouseDown(true, args, Control.ModifierKeys);
            }
            if (!this.chart.CancelMouse)
            {
                base.OnDoubleClick(e);
            }
        }

        protected override void OnInvalidated(InvalidateEventArgs e)
        {
            this.iRedrawNeeded = true;
            base.OnInvalidated(e);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.Focus();
            if (!base.DesignMode)
            {
                this.PrepareGraphics();
                this.chart.DoKeyDown(e);
            }
            base.OnKeyDown(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            this.chart.CancelMouse = false;
            base.Focus();
            if (!base.DesignMode)
            {
                this.PrepareGraphics();
                this.mouseButton = e.Button;
                this.chart.DoMouseDown(false, e, Control.ModifierKeys);
            }
            if (!this.chart.CancelMouse)
            {
                base.OnMouseDown(e);
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            if (!base.DesignMode)
            {
                this.chart.Panning.Active = false;
            }
            base.OnMouseLeave(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            this.chart.CancelMouse = false;
            if (!base.DesignMode)
            {
                this.PrepareGraphics();
                Cursor c = this.Cursor;
                this.chart.BroadcastMouseEvent(MouseEventKinds.Move, e, Control.ModifierKeys);
                c = this.Cursor;
                this.chart.BroadcastMouseEvent(MouseEventKinds.Move, e, ref c);
                this.chart.DoMouseMove(e.X, e.Y, ref c);
                Cursor.Current = c;
            }
            if (!this.chart.CancelMouse)
            {
                base.OnMouseMove(e);
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (!base.DesignMode)
            {
                this.PrepareGraphics();
                this.chart.DoMouseUp(e, Control.ModifierKeys);
            }
            if (!this.chart.CancelMouse)
            {
                base.OnMouseUp(e);
            }
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            this.chart.CancelMouse = false;
            if (!base.DesignMode)
            {
                this.PrepareGraphics();
                Cursor c = this.Cursor;
                this.chart.BroadcastMouseEvent(MouseEventKinds.Wheel, e, Control.ModifierKeys);
                this.chart.BroadcastMouseEvent(MouseEventKinds.Wheel, e, ref c);
                Cursor.Current = c;
            }
            if (!this.chart.CancelMouse)
            {
                base.OnMouseWheel(e);
            }
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            if (this.Chart.Graphics3D.UseBuffer)
            {
                bool iRedrawNeeded = this.iRedrawNeeded;
                if (this.Chart.Graphics3D.BackBuffer == null)
                {
                    this.Chart.Graphics3D.BackBuffer = this.Chart.Graphics3D.BackBufferContext.Allocate(base.CreateGraphics(), base.ClientRectangle);
                    iRedrawNeeded = true;
                }
                if (iRedrawNeeded)
                {
                    if (!base.GetStyle(ControlStyles.Opaque))
                    {
                        PaintEventArgs pevent = new PaintEventArgs(this.Chart.Graphics3D.BackBuffer.Graphics, base.ClientRectangle);
                        base.OnPaintBackground(pevent);
                    }
                    this.Draw(this.Chart.Graphics3D.BackBuffer.Graphics);
                    this.iRedrawNeeded = false;
                }
                this.chart.redrawing = true;
                this.Chart.Graphics3D.BackBuffer.Render();
            }
            else
            {
                this.Draw(pe.Graphics);
            }
        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            if (!this.chart.Graphics3D.UseBuffer)
            {
                base.OnPaintBackground(pevent);
            }
        }

        protected override void OnResize(EventArgs e)
        {
            if (this.Chart.Graphics3D.BackBuffer != null)
            {
                this.Chart.Graphics3D.BackBuffer.Dispose();
                this.Chart.Graphics3D.BackBuffer = null;
            }
            base.OnResize(e);
        }

        private void PrepareGraphics()
        {
            this.chart.graphics3D.g = base.CreateGraphics();
        }

        [EditorBrowsable(EditorBrowsableState.Never), Obsolete("Use TChart.Series.Clear() method instead.")]
        public void RemoveAllSeries()
        {
            this.chart.Series.Clear();
        }

        private bool ShouldSerializeText()
        {
            return false;
        }

        [Description("Shows the TeeChart editor dialog.")]
        public bool ShowEditor()
        {
            return this.ShowEditor(null);
        }

        [EditorBrowsable(EditorBrowsableState.Never), Description("Shows the TeeChart editor dialog.")]
        public bool ShowEditor(ITypeDescriptorContext context)
        {
            bool flag = new ChartComponentEditor().EditComponent(context, this);
            this.chart.CancelMouse = true;
            return flag;
        }

        void IChart.CheckBackground(object sender, MouseEventArgs e)
        {
            if (this.ClickBackground != null)
            {
                this.chart.CancelMouse = true;
                this.ClickBackground(sender, e);
                this.chart.IClicked = this.chart.CancelMouse;
            }
        }

        bool IChart.CheckClickSeries()
        {
            return (this.ClickSeries != null);
        }

        bool IChart.CheckGetAxisLabelAssigned()
        {
            return (this.GetAxisLabel != null);
        }

        void IChart.CheckTitle(Title ATitle, MouseEventArgs e, Keys Shift)
        {
            if (this.ClickTitle != null)
            {
                this.chart.CancelMouse = true;
                this.ClickTitle(ATitle, e);
                this.chart.IClicked = this.chart.CancelMouse;
            }
            if (!this.chart.IClicked)
            {
                this.chart.CheckZoomPanning(e, Shift);
            }
        }

        void IChart.DoAfterDraw()
        {
            if (this.AfterDraw != null)
            {
                this.chart.graphics3D.DisposeObj();
                this.AfterDraw(this, this.chart.graphics3D);
            }
        }

        bool IChart.DoAllowScroll(Axis a, double Delta, ref double Min, ref double Max)
        {
            if (this.AllowScroll != null)
            {
                bool allowScroll = true;
                AllowScrollEventArgs e = new AllowScrollEventArgs(Min, Max, allowScroll);
                this.AllowScroll(a, e);
                Min = e.Min;
                Max = e.Max;
                return e.AllowScroll;
            }
            return true;
        }

        void IChart.DoBeforeDraw()
        {
            if (this.BeforeDraw != null)
            {
                this.chart.graphics3D.DisposeObj();
                this.BeforeDraw(this, this.chart.graphics3D);
            }
        }

        void IChart.DoBeforeDrawAxes()
        {
            if (this.BeforeDrawAxes != null)
            {
                this.chart.graphics3D.DisposeObj();
                this.BeforeDrawAxes(this, this.chart.graphics3D);
            }
        }

        void IChart.DoBeforeDrawSeries()
        {
            if (this.BeforeDrawSeries != null)
            {
                this.chart.graphics3D.DisposeObj();
                this.BeforeDrawSeries(this, this.chart.graphics3D);
            }
        }

        void IChart.DoChartPrint(object sender, PrintPageEventArgs e)
        {
            if (this.ChartPrint != null)
            {
                this.ChartPrint((ChartPrintJob) sender, e);
            }
        }

        void IChart.DoClickAxis(Axis a, MouseEventArgs e)
        {
            if (this.ClickAxis != null)
            {
                this.chart.CancelMouse = true;
                this.ClickAxis(a, e);
            }
        }

        void IChart.DoClickLegend(object sender, MouseEventArgs e)
        {
            if (this.ClickLegend != null)
            {
                this.chart.CancelMouse = true;
                this.ClickLegend(sender, e);
                this.chart.IClicked = this.chart.CancelMouse;
            }
        }

        void IChart.DoClickSeries(object sender, Steema.TeeChart.Styles.Series s, int valueIndex, MouseEventArgs e)
        {
            if (this.ClickSeries != null)
            {
                this.ClickSeries(sender, s, valueIndex, e);
            }
        }

        void IChart.DoGetAxesChartRect(object sender, ref Rectangle rect)
        {
            if (this.GetAxesChartRect != null)
            {
                GetAxesChartRectEventArgs e = new GetAxesChartRectEventArgs(rect);
                this.GetAxesChartRect(sender, e);
                rect = e.AxesChartRect;
            }
        }

        void IChart.DoGetAxisLabel(object sender, Steema.TeeChart.Styles.Series s, int valueIndex, ref string labelText)
        {
            if (this.GetAxisLabel != null)
            {
                GetAxisLabelEventArgs e = new GetAxisLabelEventArgs(s, valueIndex, labelText);
                this.GetAxisLabel(sender, e);
                labelText = e.LabelText;
            }
        }

        void IChart.DoGetLegendPos(object sender, int index, ref int X, ref int Y, ref int XColor)
        {
            if (this.GetLegendPos != null)
            {
                GetLegendPosEventArgs e = new GetLegendPosEventArgs(index, X, Y, XColor);
                this.GetLegendPos(sender, e);
                X = e.X;
                Y = e.Y;
                XColor = e.XColor;
            }
        }

        void IChart.DoGetLegendRectangle(object sender, ref Rectangle rect)
        {
            if (this.GetLegendRect != null)
            {
                GetLegendRectEventArgs e = new GetLegendRectEventArgs(rect);
                this.GetLegendRect(sender, e);
                rect = e.Rectangle;
            }
        }

        void IChart.DoGetLegendSize(object sender, ref int size)
        {
            if (this.GetLegendSize != null)
            {
                GetLegendSizeEventArgs e = new GetLegendSizeEventArgs(size);
                this.GetLegendSize(sender, e);
                size = e.Size;
            }
        }

        void IChart.DoGetLegendText(object sender, LegendStyles LegendStyle, int Index, ref string Text)
        {
            if (this.GetLegendText != null)
            {
                GetLegendTextEventArgs e = new GetLegendTextEventArgs(LegendStyle, Index, Text);
                this.GetLegendText(sender, e);
                Text = e.Text;
            }
        }

        void IChart.DoGetNextAxisLabel(object sender, int labelIndex, ref double labelValue, ref bool Stop)
        {
            if (this.GetNextAxisLabel != null)
            {
                GetNextAxisLabelEventArgs e = new GetNextAxisLabelEventArgs(labelIndex, labelValue, Stop);
                this.GetNextAxisLabel(sender, e);
                labelValue = e.LabelValue;
                Stop = e.Stop;
            }
        }

        void IChart.DoScroll(object sender, EventArgs e)
        {
            if (this.Scroll != null)
            {
                this.Scroll(sender, e);
            }
        }

        void IChart.DoSetControlStyle()
        {
            if (((base.BackColor.A != this.Panel.Color.A) || (base.BackColor.R != this.Panel.Color.R)) || ((base.BackColor.G != this.Panel.Color.G) || (base.BackColor.B != this.Panel.Color.B)))
            {
                base.BackColor = Color.Transparent;
            }
            ControlStyles flag = ControlStyles.AllPaintingInWmPaint | ControlStyles.StandardDoubleClick | ControlStyles.SupportsTransparentBackColor | ControlStyles.Selectable | ControlStyles.StandardClick | ControlStyles.ResizeRedraw | ControlStyles.UserPaint;
            base.SetStyle(flag, true);
            base.SetStyle(ControlStyles.OptimizedDoubleBuffer, false);
        }

        void IChart.DoUndoneZoom(object sender, EventArgs e)
        {
            if (this.UndoneZoom != null)
            {
                this.UndoneZoom(this, e);
            }
        }

        void IChart.DoZoomed(object sender, EventArgs e)
        {
            if (this.Zoomed != null)
            {
                this.Zoomed(this, e);
            }
        }

        void IChart.DrawBackColor(Steema.TeeChart.Drawing.Graphics3D g)
        {
        }

        object IChart.FindParentForm()
        {
            return base.FindForm();
        }

        IContainer IChart.GetContainer()
        {
            return base.Container;
        }

        Control IChart.GetControl()
        {
            return this;
        }

        Cursor IChart.GetCursor()
        {
            return this.Cursor;
        }

        bool IChart.IsWebForm()
        {
            return false;
        }

        Point IChart.PointToScreen(Point p)
        {
            return base.PointToScreen(p);
        }

        void IChart.RefreshControl()
        {
            this.Refresh();
        }

        void IChart.SetChart(Steema.TeeChart.Chart c)
        {
            List<ITeeEventListener> listeners = this.chart.Listeners;
            this.chart = c;
            this.chart.parent = this;
            if (listeners.Count > 0)
            {
                this.chart.Listeners = listeners;
            }
        }

        void IChart.SetCursor(Cursor c)
        {
            this.Cursor = c;
        }

        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false), DefaultValue(true), Obsolete("Please use Series.ApplyZOrder instead.")]
        public bool ApplyZOrder
        {
            get
            {
                return this.chart.Series.ApplyZOrder;
            }
            set
            {
                this.chart.Series.ApplyZOrder = value;
            }
        }

        [Description("3D view parameters."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Steema.TeeChart.Drawing.Aspect Aspect
        {
            get
            {
                return this.chart.aspect;
            }
        }

        [Description("Enables/Disables Repainting of Chart."), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false), DefaultValue(true)]
        public bool AutoRepaint
        {
            get
            {
                return this.chart.AutoRepaint;
            }
            set
            {
                this.chart.AutoRepaint = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Collection of predefined and custom axis objects.")]
        public Steema.TeeChart.Axes Axes
        {
            get
            {
                return this.chart.Axes;
            }
        }

        [DefaultValue(typeof(Color), "Control"), Description("Sets the color the Chart rectangle is filled with.")]
        public Color BackColor
        {
            get
            {
                return base.BackColor;
            }
            set
            {
                this.chart.Panel.Color = value;
                base.BackColor = value;
                ((IChart) this).DoSetControlStyle();
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public System.Drawing.Bitmap Bitmap
        {
            get
            {
                return this.chart.Bitmap(base.Width, base.Height);
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Obsolete("Please use Graphics3D property."), Description("Accesses TeeChart Draw properties and methods.")]
        public Steema.TeeChart.Drawing.Graphics3D Canvas
        {
            get
            {
                return this.Graphics3D;
            }
            set
            {
                this.Graphics3D = value;
                base.Invalidate();
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Description("Defines the Chart component to display on a TChart.")]
        public Steema.TeeChart.Chart Chart
        {
            get
            {
                return this.chart;
            }
            set
            {
                ((IChart) this).SetChart(value);
            }
        }

        [Obsolete("Please use Aspect.ClipPoints instead."), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false), DefaultValue(true)]
        public bool ClipPoints
        {
            get
            {
                return this.chart.Aspect.clipPoints;
            }
            set
            {
                this.chart.Aspect.ClipPoints = value;
            }
        }

        protected override Size DefaultSize
        {
            get
            {
                return new Size(400, 250);
            }
        }

        [Description("Accesses Chart export properties and methods."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Browsable(false)]
        public Exports Export
        {
            get
            {
                return this.chart.Export;
            }
        }

        [Browsable(false), Localizable(false), EditorBrowsable(EditorBrowsableState.Never), Description("Determines Font characteristics.")]
        public System.Drawing.Font Font
        {
            get
            {
                return base.Font;
            }
        }

        [Localizable(true), Description("Defines Text shown at the bottom of the Chart."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("Titles")]
        public Steema.TeeChart.Footer Footer
        {
            get
            {
                return this.chart.Footer;
            }
        }

        [Browsable(false), Description(""), EditorBrowsable(EditorBrowsableState.Never)]
        public Color ForeColor
        {
            get
            {
                return base.ForeColor;
            }
        }

        [Obsolete("Use TChart.Walls.Back.Pen property instead."), Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public ChartPen Frame
        {
            get
            {
                return this.chart.Walls.Back.Pen;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never), Obsolete("Please use MousePosition property")]
        public Point GetCursorPos
        {
            get
            {
                return Control.MousePosition;
            }
        }

        [Description("Use the Graphics3D property to access TeeChart Draw properties and methods."), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public Steema.TeeChart.Drawing.Graphics3D Graphics3D
        {
            get
            {
                return this.chart.graphics3D;
            }
            set
            {
                this.chart.Graphics3D = value;
            }
        }

        [Localizable(true), Description("Defines Text shown at top of the Chart."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("Titles")]
        public Steema.TeeChart.Header Header
        {
            get
            {
                return this.chart.Header;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Browsable(false), Description("Accesses Chart import properties and methods.")]
        public Imports Import
        {
            get
            {
                return this.chart.Import;
            }
        }

        public Steema.TeeChart.Styles.Series this[int index]
        {
            get
            {
                return this.chart.Series[index];
            }
            set
            {
                this.chart.Series[index] = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Steema.TeeChart.Legend Legend
        {
            get
            {
                return this.chart.Legend;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Description("")]
        public System.Drawing.Imaging.Metafile Metafile
        {
            get
            {
                return this.chart.Metafile(this.chart, base.Width, base.Height);
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Description("True when the Chart is internally drawing into a Metafile image.")]
        public bool Metafiling
        {
            get
            {
                return this.chart.graphics3D.metafiling;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Accesses multiple page characteristics of the Chart.")]
        public Steema.TeeChart.Page Page
        {
            get
            {
                return this.chart.Page;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Background visible attributes.")]
        public Steema.TeeChart.Panel Panel
        {
            get
            {
                return this.chart.Panel;
            }
        }

        [Description("Accesses Panning characteristics."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Steema.TeeChart.Scroll Panning
        {
            get
            {
                return this.chart.Panning;
            }
        }

        [Description("Printing related properties."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Steema.TeeChart.Printer Printer
        {
            get
            {
                return this.chart.Printer;
            }
        }

        [Description("Determines when Chart is being printed."), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public bool Printing
        {
            get
            {
                return this.chart.printing;
            }
            set
            {
                this.chart.printing = value;
                base.Invalidate();
            }
        }

        [Description("Collection of Series contained in this Chart."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public SeriesCollection Series
        {
            get
            {
                return this.chart.series;
            }
        }

        [Obsolete("Use TChart.Series.Count property instead."), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public int SeriesCount
        {
            get
            {
                return this.chart.Series.Count;
            }
        }

        public override ISite Site
        {
            get
            {
                return base.Site;
            }
            set
            {
                base.Site = value;
                if (base.Site != null)
                {
                    DesignTimeOptions.InitLanguage(base.Site.DesignMode, true);
                }
            }
        }

        [Category("Titles"), Localizable(true), Description("Defines Text shown directly above Footer."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Steema.TeeChart.Footer SubFooter
        {
            get
            {
                return this.chart.SubFooter;
            }
        }

        [Description("Defines Text shown directly below Header."), Category("Titles"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Localizable(true)]
        public Steema.TeeChart.Header SubHeader
        {
            get
            {
                return this.chart.SubHeader;
            }
        }

        [Description("Sets the text for the Header."), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override string Text
        {
            get
            {
                if (this.chart != null)
                {
                    return this.chart.Header.Text;
                }
                return "";
            }
            set
            {
                if (this.chart != null)
                {
                    this.chart.Header.Text = value;
                }
            }
        }

        [DefaultValue(false), Obsolete("Please use a standalone Timer component."), Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public bool TimerEnabled
        {
            get
            {
                this.CheckTimer();
                return this.timer.Enabled;
            }
            set
            {
                this.CheckTimer();
                this.timer.Enabled = value;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false), DefaultValue(100), Obsolete("Please use a standalone Timer component.")]
        public int TimerInterval
        {
            get
            {
                this.CheckTimer();
                return this.timer.Interval;
            }
            set
            {
                this.CheckTimer();
                this.timer.Interval = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Collection of Tool components contained in this Chart.")]
        public ToolsCollection Tools
        {
            get
            {
                return this.chart.Tools;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Accesses wall characteristics of the Chart.")]
        public Steema.TeeChart.Walls Walls
        {
            get
            {
                return this.chart.Walls;
            }
        }

        [Description("Accesses the Zoom characteristics of the Chart."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Steema.TeeChart.Zoom Zoom
        {
            get
            {
                return this.chart.Zoom;
            }
        }

        public class AllowScrollEventArgs : EventArgs
        {
            public bool AllowScroll;
            public double Max;
            public double Min;

            public AllowScrollEventArgs(double min, double max, bool allowScroll)
            {
                this.Min = min;
                this.Max = max;
                this.AllowScroll = allowScroll;
            }
        }

        public delegate void AllowScrollEventHandler(Axis a, TChart.AllowScrollEventArgs e);

        internal class ChartComponentEditor : System.ComponentModel.ComponentEditor
        {
            public override bool EditComponent(ITypeDescriptorContext context, object aobject)
            {
                bool flag = ChartEditor.ShowModal(((TChart) aobject).chart);
                if ((context != null) && flag)
                {
                    context.OnComponentChanged();
                }
                return flag;
            }
        }

        internal class ChartDesigner : ControlDesigner
        {
            internal static int language = -1;

            public ChartDesigner()
            {
                this.AddVerbs();
            }

            private void AddVerbs()
            {
                DesignTimeOptions.InitLanguage(true, false);
                this.Verbs.Clear();
                this.Verbs.Add(new DesignerVerb(Texts.About, new EventHandler(this.OnAbout)));
                this.Verbs.Add(new DesignerVerb(Texts.Edit, new EventHandler(this.OnEdit)));
                this.Verbs.Add(new DesignerVerb(Texts.Clear, new EventHandler(this.OnClear)));
                this.Verbs.Add(new DesignerVerb(Texts.ExportChart, new EventHandler(this.OnExport)));
                this.Verbs.Add(new DesignerVerb(Texts.ImportChart, new EventHandler(this.OnImport)));
                this.Verbs.Add(new DesignerVerb(Texts.PrintPreview, new EventHandler(this.OnPreview)));
                this.Verbs.Add(new DesignerVerb(Texts.Options, new EventHandler(this.OnOptions)));
                this.Verbs.Add(new DesignerVerb(Texts.OnlineSupport, new EventHandler(this.OnSupport)));
                base.RaiseComponentChanged(null, null, null);
            }

            private void OnAbout(object sender, EventArgs e)
            {
                ///WYJ fix, original: AboutBox.ShowModal();
                Steema.TeeChart.Editors.AboutBox.ShowModal();
            }

            private void OnClear(object sender, EventArgs e)
            {
                if (Utils.YesNo(Texts.ClearMessage))
                {
                    this.Chart.Clear();
                }
            }

            protected override void OnContextMenu(int x, int y)
            {
                this.AddVerbs();
                base.OnContextMenu(x, y);
            }

            private void OnEdit(object sender, EventArgs e)
            {
                if (this.Chart.ShowEditor((ITypeDescriptorContext) this.GetService(typeof(ITypeDescriptorContext))))
                {
                    base.RaiseComponentChanged(null, null, null);
                }
            }

            private void OnExport(object sender, EventArgs e)
            {
                ExportEditor.ShowModal(this.Chart.Chart);
            }

            private void OnImport(object sender, EventArgs e)
            {
                ImportEditor.ShowModal(this.Chart.Chart);
            }

            private void OnOptions(object sender, EventArgs e)
            {
                using (DesignTimeOptions options = DesignTimeOptions.GetInstance())
                {
                    if (EditorUtils.ShowFormModal(options))
                    {
                        options.StoreSettings();
                    }
                }
            }

            private void OnPreview(object sender, EventArgs e)
            {
                PrintPreview.ShowModal(this.Chart.Chart);
                base.RaiseComponentChanged(null, null, null);
            }

            private void OnSupport(object sender, EventArgs e)
            {
                Process.Start(Texts.SupportURL);
            }

            private TChart Chart
            {
                get
                {
                    return (TChart) base.Component;
                }
            }
        }

        public delegate void SeriesEventHandler(object sender, Series s, int valueIndex, MouseEventArgs e);
    }
}

