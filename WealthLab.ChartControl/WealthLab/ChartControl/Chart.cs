namespace WealthLab.ChartControl
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Windows.Forms;
    using WealthLab;

    [ToolboxBitmap(typeof(Chart), "Chart")]
    public class Chart : Control
    {
        private WealthLab.Bars bars_0;
        private Bitmap bitmap_0;
        private bool bool_0;
        private bool bool_1 = true;
        private bool bool_2 = true;
        private bool bool_3 = true;
        private bool bool_4;
        private static bool displayCrossHair = false;
        [CompilerGenerated]
        private bool bool_6;
        private ChartMode chartMode_0;
        private ChartPane chartPane_0;
        private ChartRenderer chartRenderer_0;
        private WealthLab.ChartStyle chartStyle_0;
        private Class2 cursorPointData;
        private DrawingObjectManager drawingObjectManager_0;
        private Font font_0 = new Font("Vrinda", 8f);
        private GeneralToolTip generalToolTip_0 = new GeneralToolTip();
        private GlyphToolTip glyphToolTip_0 = new GlyphToolTip();
        private HScrollBar hscrollBar_0 = new HScrollBar();
        private IContainer icontainer_0;
        private IndicatorDragDropManager indicatorDragDropManager_0;
        private IndicatorToolTip indicatorToolTip_0 = new IndicatorToolTip();
        private int int_0 = -1;
        private int int_1 = -1;
        private int int_2;
        private int int_3;
        private object object_0 = new object();
        public static int PixelSensitivity = 3;
        private PlottedIndicator plottedIndicator_0;
        private Position position_0;
        private PriceToolTip priceToolTip_0 = new PriceToolTip();
        private ScaleSelector scaleSelector_0 = new ScaleSelector();
        private static System.Type type_0 = null;

        private EventHandler<BarNumberEventArgs> eventHandler_0;

        private EventHandler<EventArgs> eventHandler_1;

        private EventHandler<MouseEventArgs> eventHandler_2;

        private EventHandler<ExceptionEventArgs> eventHandler_3;

        private EventHandler<ScaleChangeEventArgs> eventHandler_4;


        public event EventHandler<ScaleChangeEventArgs> DataScaleChange
        {
            add
            {
                EventHandler<ScaleChangeEventArgs> eventHandler;
                EventHandler<ScaleChangeEventArgs> eventHandler4 = this.eventHandler_4;
                do
                {
                    eventHandler = eventHandler4;
                    EventHandler<ScaleChangeEventArgs> eventHandler1 = (EventHandler<ScaleChangeEventArgs>)Delegate.Combine(eventHandler, value);
                    eventHandler4 = Interlocked.CompareExchange<EventHandler<ScaleChangeEventArgs>>(ref this.eventHandler_4, eventHandler1, eventHandler);
                }
                while (eventHandler4 != eventHandler);
            }
            remove
            {
                EventHandler<ScaleChangeEventArgs> eventHandler;
                EventHandler<ScaleChangeEventArgs> eventHandler4 = this.eventHandler_4;
                do
                {
                    eventHandler = eventHandler4;
                    EventHandler<ScaleChangeEventArgs> eventHandler1 = (EventHandler<ScaleChangeEventArgs>)Delegate.Remove(eventHandler, value);
                    eventHandler4 = Interlocked.CompareExchange<EventHandler<ScaleChangeEventArgs>>(ref this.eventHandler_4, eventHandler1, eventHandler);
                }
                while (eventHandler4 != eventHandler);
            }
        }

        public event EventHandler<EventArgs> DrawingObjectOperationCompleted
        {
            add
            {
                EventHandler<EventArgs> eventHandler;
                EventHandler<EventArgs> eventHandler1 = this.eventHandler_1;
                do
                {
                    eventHandler = eventHandler1;
                    EventHandler<EventArgs> eventHandler2 = (EventHandler<EventArgs>)Delegate.Combine(eventHandler, value);
                    eventHandler1 = Interlocked.CompareExchange<EventHandler<EventArgs>>(ref this.eventHandler_1, eventHandler2, eventHandler);
                }
                while (eventHandler1 != eventHandler);
            }
            remove
            {
                EventHandler<EventArgs> eventHandler;
                EventHandler<EventArgs> eventHandler1 = this.eventHandler_1;
                do
                {
                    eventHandler = eventHandler1;
                    EventHandler<EventArgs> eventHandler2 = (EventHandler<EventArgs>)Delegate.Remove(eventHandler, value);
                    eventHandler1 = Interlocked.CompareExchange<EventHandler<EventArgs>>(ref this.eventHandler_1, eventHandler2, eventHandler);
                }
                while (eventHandler1 != eventHandler);
            }
        }

        public event EventHandler<BarNumberEventArgs> MouseMoveBarNumber
        {
            add
            {
                EventHandler<BarNumberEventArgs> eventHandler;
                EventHandler<BarNumberEventArgs> eventHandler0 = this.eventHandler_0;
                do
                {
                    eventHandler = eventHandler0;
                    EventHandler<BarNumberEventArgs> eventHandler1 = (EventHandler<BarNumberEventArgs>)Delegate.Combine(eventHandler, value);
                    eventHandler0 = Interlocked.CompareExchange<EventHandler<BarNumberEventArgs>>(ref this.eventHandler_0, eventHandler1, eventHandler);
                }
                while (eventHandler0 != eventHandler);
            }
            remove
            {
                EventHandler<BarNumberEventArgs> eventHandler;
                EventHandler<BarNumberEventArgs> eventHandler0 = this.eventHandler_0;
                do
                {
                    eventHandler = eventHandler0;
                    EventHandler<BarNumberEventArgs> eventHandler1 = (EventHandler<BarNumberEventArgs>)Delegate.Remove(eventHandler, value);
                    eventHandler0 = Interlocked.CompareExchange<EventHandler<BarNumberEventArgs>>(ref this.eventHandler_0, eventHandler1, eventHandler);
                }
                while (eventHandler0 != eventHandler);
            }
        }

        public event EventHandler<MouseEventArgs> MouseWheelMoved
        {
            add
            {
                EventHandler<MouseEventArgs> eventHandler;
                EventHandler<MouseEventArgs> eventHandler2 = this.eventHandler_2;
                do
                {
                    eventHandler = eventHandler2;
                    EventHandler<MouseEventArgs> eventHandler1 = (EventHandler<MouseEventArgs>)Delegate.Combine(eventHandler, value);
                    eventHandler2 = Interlocked.CompareExchange<EventHandler<MouseEventArgs>>(ref this.eventHandler_2, eventHandler1, eventHandler);
                }
                while (eventHandler2 != eventHandler);
            }
            remove
            {
                EventHandler<MouseEventArgs> eventHandler;
                EventHandler<MouseEventArgs> eventHandler2 = this.eventHandler_2;
                do
                {
                    eventHandler = eventHandler2;
                    EventHandler<MouseEventArgs> eventHandler1 = (EventHandler<MouseEventArgs>)Delegate.Remove(eventHandler, value);
                    eventHandler2 = Interlocked.CompareExchange<EventHandler<MouseEventArgs>>(ref this.eventHandler_2, eventHandler1, eventHandler);
                }
                while (eventHandler2 != eventHandler);
            }
        }

        public event EventHandler<ExceptionEventArgs> OnException
        {
            add
            {
                EventHandler<ExceptionEventArgs> eventHandler;
                EventHandler<ExceptionEventArgs> eventHandler3 = this.eventHandler_3;
                do
                {
                    eventHandler = eventHandler3;
                    EventHandler<ExceptionEventArgs> eventHandler1 = (EventHandler<ExceptionEventArgs>)Delegate.Combine(eventHandler, value);
                    eventHandler3 = Interlocked.CompareExchange<EventHandler<ExceptionEventArgs>>(ref this.eventHandler_3, eventHandler1, eventHandler);
                }
                while (eventHandler3 != eventHandler);
            }
            remove
            {
                EventHandler<ExceptionEventArgs> eventHandler;
                EventHandler<ExceptionEventArgs> eventHandler3 = this.eventHandler_3;
                do
                {
                    eventHandler = eventHandler3;
                    EventHandler<ExceptionEventArgs> eventHandler1 = (EventHandler<ExceptionEventArgs>)Delegate.Remove(eventHandler, value);
                    eventHandler3 = Interlocked.CompareExchange<EventHandler<ExceptionEventArgs>>(ref this.eventHandler_3, eventHandler1, eventHandler);
                }
                while (eventHandler3 != eventHandler);
            }
        }


        public Chart()
        {
            this.DoubleBuffered = true;
            this.method_6();
            this.hscrollBar_0.Left = 0;
            this.hscrollBar_0.LargeChange = 10;
            this.hscrollBar_0.Visible = this.bool_0;
            this.hscrollBar_0.Scroll += new ScrollEventHandler(this.hscrollBar_0_Scroll);
            base.Controls.Add(this.hscrollBar_0);
            this.priceToolTip_0.Visible = false;
            base.Controls.Add(this.priceToolTip_0);
            this.indicatorToolTip_0.Visible = false;
            base.Controls.Add(this.indicatorToolTip_0);
            this.generalToolTip_0.Visible = false;
            base.Controls.Add(this.generalToolTip_0);
            this.glyphToolTip_0.Visible = false;
            base.Controls.Add(this.glyphToolTip_0);
            this.scaleSelector_0.Visible = true;
            base.Controls.Add(this.scaleSelector_0);
            this.scaleSelector_0.ScaleChangeEvent += new EventHandler<ScaleChangeEventArgs>(this.method_4);
            this.MultiSymbolMode = false;

            //base.KeyDown += new KeyEventHandler(this.MainForm_KeyDown);
            //base.PreviewKeyDown += new PreviewKeyDownEventHandler(this.MainForm_PreviewKeyDown);
        }

        /// <summary>
        /// ///WYJ fix
        /// </summary>
        /// <param name="keyData"></param>
        /// <returns></returns>
        protected override bool IsInputKey(Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Right:
                case Keys.Left:
                case Keys.Up:
                case Keys.Down:
                    return true;
                case Keys.Control | Keys.Right:
                case Keys.Control | Keys.Left:
                case Keys.Control | Keys.Up:
                case Keys.Control | Keys.Down:
                    return true;
            }
            return base.IsInputKey(keyData);
        }

        /*
        protected override void OnKeyDown2(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            switch (e.KeyCode)
            {
                case Keys.Left:
                case Keys.Right:
                case Keys.Up:
                case Keys.Down:
                    if (e.Control)
                    {
                        return;
                    }
                    else
                    {
                        return;
                    }
                    break;
            }
        }

        private void MainForm_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Down:
                case Keys.Up:
                    e.IsInputKey = true;
                    break;
            }
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 0x1b)
            {
                //ChartForm activeChartWindow = this;
                //if (activeChartWindow != null)
                {
                //    activeChartWindow.PressEscape();
                    return;
                }
            }
        }
        */

        public void CancelScaleChange()
        {
            this.scaleSelector_0.ChartScale = this.Bars.DataScale;
        }

        public void CopyToClipboard()
        {
            try
            {
                Clipboard.SetDataObject(this.GetChartBitmap(), true, 2, 0x3e8);
            }
            catch (ExternalException)
            {
                MessageBox.Show("Copy to clipboard was blocked by another process.  Please try again", "ClipBoard Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.icontainer_0 != null))
            {
                this.icontainer_0.Dispose();
            }
            base.Dispose(disposing);
        }

        public void DoInvalidate()
        {
            if ((this.Renderer == null) || ((this.Renderer != null) && !this.Renderer.Executing))
            {
                base.Invalidate();
            }
        }

        public Bitmap GetChartBitmap()
        {
            Bitmap bitmap2;
            Graphics g = base.CreateGraphics();
            using (g)
            {
                int height = base.Height;
                if (this.hscrollBar_0.Visible)
                {
                    height -= this.hscrollBar_0.Height;
                }
                Bitmap image = new Bitmap(base.Width, height, g);
                Graphics graphics3 = Graphics.FromImage(image);
                using (graphics3)
                {
                    this.Renderer.Render(this.Bars, graphics3, base.Width, height, this.ChartStyle);
                    if (this.drawingObjectManager_0 != null)
                    {
                        this.drawingObjectManager_0.method_3(graphics3);
                    }
                    bitmap2 = image;
                }
            }
            return bitmap2;
        }

        public Bitmap GetChartBitmap(int width, int height)
        {
            Bitmap bitmap;
            Graphics g = base.CreateGraphics();
            using (g)
            {
                bitmap = new Bitmap(width, height, g);
                Graphics graphics3 = Graphics.FromImage(bitmap);
                using (graphics3)
                {
                    this.Renderer.Render(this.Bars, graphics3, width, height, this.ChartStyle);
                    if (this.drawingObjectManager_0 != null)
                    {
                        this.drawingObjectManager_0.method_3(graphics3);
                    }
                }
            }
            return bitmap;
        }

        private void hscrollBar_0_Scroll(object sender, ScrollEventArgs e)
        {
            if (((e.Type == ScrollEventType.EndScroll) && (this.bars_0 != null)) && (e.NewValue > this.bars_0.Count))
            {
                e.NewValue = this.bars_0.Count;
            }
            if (e.NewValue != this.int_1)
            {
                this.int_1 = e.NewValue;
                if (this.HasValidChart)
                {
                    this.chartRenderer_0.ScrollOffset = this.bars_0.Count - e.NewValue;
                    base.Invalidate();
                }
                this.priceToolTip_0.Reset();
                this.glyphToolTip_0.Reset();
                this.indicatorToolTip_0.Reset();
            }
        }

        private void method_0(UserControl userControl_0, int int_4, int int_5)
        {
            int num;
            int num2;
            if (int_4 > (base.Width / 2))
            {
                num = (int_4 - userControl_0.Width) - 8;
            }
            else
            {
                num = int_4 + 8;
            }
            if (int_5 > (base.Height / 2))
            {
                num2 = (int_5 - userControl_0.Height) - 8;
            }
            else
            {
                num2 = int_5 + 8;
            }
            if ((num + userControl_0.Width) > base.Width)
            {
                num = base.Width - userControl_0.Width;
            }
            if ((num2 + userControl_0.Height) > base.Height)
            {
                num2 = base.Height - userControl_0.Height;
            }
            if (num2 < 0)
            {
                num2 = 0;
            }
            if (num < 0)
            {
                num = 0;
            }
            userControl_0.Location = new Point(num, num2);
        }

        private void method_1(DragEventArgs dragEventArgs_0)
        {
            if ((this.DragDropManager != null) && this.HasValidChart)
            {
                DraggedIndicatorHelper data = (DraggedIndicatorHelper) dragEventArgs_0.Data.GetData(typeof(DraggedIndicatorHelper));
                DraggedFundamentalItem item = (DraggedFundamentalItem) dragEventArgs_0.Data.GetData(typeof(DraggedFundamentalItem));
                if ((data != null) || (item != null))
                {
                    dragEventArgs_0.Effect = DragDropEffects.Copy;
                    if (data != null)
                    {
                        this.Mode = ChartMode.DraggingIndicator;
                        this.bool_4 = data.Helper.CanDropOnIndicator;
                    }
                    else
                    {
                        this.Mode = ChartMode.DraggingFundamental;
                    }
                }
            }
        }

        private void method_2()
        {
            this.priceToolTip_0.Visible = false;
            this.indicatorToolTip_0.Visible = false;
            this.generalToolTip_0.Visible = false;
            this.glyphToolTip_0.Visible = false;
        }

        private void method_3()
        {
            if (!this.scaleSelector_0.ValidScale || this.MultiSymbolMode)
            {
                this.scaleSelector_0.Visible = false;
                return;
            }
            int num = this.Renderer.Height - this.Renderer.MarginBottomHeight;
            using (IEnumerator<ChartPane> enumerator = this.Renderer.Panes.GetEnumerator())
            {
                ChartPane current;
                while (enumerator.MoveNext())
                {
                    current = enumerator.Current;
                    if (current.IsPricePane)
                    {
                        goto Label_0060;
                    }
                }
                goto Label_007A;
            Label_0060:
                num = current.Top + current.Height;
            }
        Label_007A:
            this.scaleSelector_0.Top = num - this.scaleSelector_0.Height;
            this.scaleSelector_0.Left = 5;
            this.scaleSelector_0.Visible = true;
        }

        private void method_4(object sender, ScaleChangeEventArgs e)
        {
            EventHandler<ScaleChangeEventArgs> handler = this.eventHandler_4;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        
        ///WYJ fix, original signature: private void method_5(Graphics graphics_0)
        private void drawCrosshair(Graphics graphics_0)
        {
            if ((this.cursorPointData != null) && (this.cursorPointData.barNum < this.Bars.Count))
            {
                Pen pen = new Pen(ChartRenderer.ReverseColor(this.Renderer.BackgroundColor));
                using (pen)
                {
                    Brush brush = new SolidBrush(ChartRenderer.ReverseColor(this.Renderer.BackgroundColor));
                    Brush brush2 = new SolidBrush(this.Renderer.BackgroundColor);
                    int x = this.Renderer.ConvertBarToX(this.cursorPointData.barNum);
                    graphics_0.DrawLine(pen, x, 0, x, this.Renderer.ChartHeight);
                    graphics_0.DrawLine(pen, 0, this.cursorPointData.y, this.Renderer.ChartWidth, this.cursorPointData.y);
                    string text = this.Bars.Date[this.cursorPointData.barNum].ToString("d");
                    SizeF ef = graphics_0.MeasureString(text, this.Font);
                    RectangleF ef2 = new RectangleF((float) (x + 5), 0f, ef.Width, ef.Height);
                    graphics_0.DrawRectangle(pen, Rectangle.Ceiling(ef2));
                    graphics_0.FillRectangle(brush, Rectangle.Ceiling(ef2));
                    graphics_0.DrawString(text, this.Font, brush2, ef2);
                    string str2 = this.cursorPointData.doubleValue.ToString("F02");
                    ef = graphics_0.MeasureString(str2, this.Font);
                    RectangleF ef3 = new RectangleF(0f, (float) (this.cursorPointData.y - 15), ef.Width, ef.Height);
                    graphics_0.DrawRectangle(pen, Rectangle.Ceiling(ef3));
                    graphics_0.FillRectangle(brush, Rectangle.Ceiling(ef3));
                    graphics_0.DrawString(str2, this.Font, brush2, ef3);
                    brush2.Dispose();
                    brush.Dispose();
                }
            }
        }

        private void method_6()
        {
            base.SuspendLayout();
            base.ResumeLayout(false);
        }

        protected override void OnDragDrop(DragEventArgs drgevent)
        {
            if (this.DragDropManager != null)
            {
                DraggedIndicatorHelper data = (DraggedIndicatorHelper) drgevent.Data.GetData(typeof(DraggedIndicatorHelper));
                if (data != null)
                {
                    IndicatorHelper helper = data.Helper;
                    Point point = base.PointToClient(new Point(drgevent.X, drgevent.Y));
                    ChartPane pane = this.chartRenderer_0.PaneFromY(point.Y);
                    this.DragDropManager.ProcessDroppedIndicatorHelper(helper, pane);
                }
                else
                {
                    DraggedFundamentalItem draggedFundamental = (DraggedFundamentalItem) drgevent.Data.GetData(typeof(DraggedFundamentalItem));
                    if (draggedFundamental != null)
                    {
                        this.DragDropManager.ProcessDroppedFundamentalItem(draggedFundamental);
                    }
                }
            }
            this.Mode = ChartMode.Normal;
            base.OnDragDrop(drgevent);
        }

        protected override void OnDragEnter(DragEventArgs drgevent)
        {
            this.method_1(drgevent);
            base.OnDragEnter(drgevent);
        }

        protected override void OnDragLeave(EventArgs eventArgs_0)
        {
            this.Mode = ChartMode.Normal;
            base.OnDragLeave(eventArgs_0);
        }

        protected override void OnDragOver(DragEventArgs drgevent)
        {
            this.method_1(drgevent);
            Point point = base.PointToClient(new Point(drgevent.X, drgevent.Y));
            this.OnMouseMove(new MouseEventArgs(MouseButtons.None, 0, point.X, point.Y, 0));
            base.OnDragOver(drgevent);
        }

        protected override void OnKeyDown(KeyEventArgs keyEventArgs_0)
        {
            if (/*keyEventArgs_0.Control &&*/ (this.Bars != null))
            {
                if (keyEventArgs_0.KeyCode == Keys.Home && keyEventArgs_0.Control)
                {
                    this.ScrollToBar(0);
                    base.OnKeyDown(keyEventArgs_0);
                    return;
                }
                else if (keyEventArgs_0.KeyCode == Keys.End && keyEventArgs_0.Control)
                {
                    this.ScrollToBar(this.Bars.Count - 1);
                    base.OnKeyDown(keyEventArgs_0);
                    return;
                }
                int moveBy = 0;
                switch (keyEventArgs_0.KeyCode)
                {
                    case Keys.Left:
                        if (this.Mode == ChartMode.SetCrosshairLocation)
                        {
                            if (keyEventArgs_0.Control)
                                moveBy = 10;
                            else
                                moveBy = 1;

                            if (this.cursorPointData != null)
                            {
                                if (this.cursorPointData.barNum < this.chartRenderer_0.LeftEdgeBar + 1)
                                {
                                    this.cursorPointData.barNum = this.chartRenderer_0.LeftEdgeBar + 1;  
                                }
                                else
                                {
                                    this.cursorPointData.barNum -= moveBy;
                                    if (this.cursorPointData.barNum < 0)
                                        this.cursorPointData.barNum = 0;

                                    if (this.cursorPointData.barNum > this.chartRenderer_0.RightEdgeBar)
                                        this.cursorPointData.barNum = this.chartRenderer_0.RightEdgeBar;

                                    if (this.cursorPointData.barNum < this.chartRenderer_0.LeftEdgeBar + 1)
                                    {
                                        moveBy = this.chartRenderer_0.LeftEdgeBar + 1 - this.cursorPointData.barNum;
                                        this.ScrollBy(moveBy);
                                    }
                                }
                            }
                            else
                            {
                                int barNum = this.Renderer.ConvertXToBar(10000);
                                if (barNum == -1)
                                {
                                    barNum = this.Renderer.RightEdgeBar;
                                }

                                this.cursorPointData = new Chart.Class2(barNum, 0.0, 0);

                            }

                            double doubleValue = this.Bars.Close[this.cursorPointData.barNum];
                            int y = this.chartRenderer_0.PricePane.ConvertValueToY(doubleValue);
                            this.cursorPointData.doubleValue = doubleValue;
                            this.cursorPointData.y = y;

                            base.Invalidate();

                        }
                        else
                        {
                            if (keyEventArgs_0.Control)
                                moveBy = 40;
                            else
                                moveBy = 10;
                            this.ScrollBy(moveBy);
                        }
                        break;

                    case Keys.Right:
                        if (this.Mode == ChartMode.SetCrosshairLocation)
                        {
                            if (keyEventArgs_0.Control)
                                moveBy = 10;
                            else
                                moveBy = 1;

                            if (this.cursorPointData != null)
                            {
                                if (this.cursorPointData.barNum > this.chartRenderer_0.RightEdgeBar)
                                    this.cursorPointData.barNum = this.chartRenderer_0.RightEdgeBar;

                                else
                                {
                                    this.cursorPointData.barNum += moveBy;

                                    if (this.cursorPointData.barNum >= this.Bars.Count)
                                        this.cursorPointData.barNum = this.Bars.Count - 1;

                                    if (this.cursorPointData.barNum < this.chartRenderer_0.LeftEdgeBar)
                                        this.cursorPointData.barNum = this.chartRenderer_0.LeftEdgeBar;

                                    if (this.cursorPointData.barNum > this.chartRenderer_0.RightEdgeBar)
                                    {
                                        moveBy = this.cursorPointData.barNum - this.chartRenderer_0.RightEdgeBar;
                                        this.ScrollBy(-1 * moveBy);
                                    }
                                }
                            }
                            else
                            {
                                int barNum = this.Renderer.ConvertXToBar(10000);
                                if (barNum == -1)
                                {
                                    barNum = this.Renderer.LeftEdgeBar;
                                }
                                this.cursorPointData = new Chart.Class2(barNum, 0.0, 0);
                            }

                            double doubleValue = this.Bars.Close[this.cursorPointData.barNum];
                            int y = this.chartRenderer_0.PricePane.ConvertValueToY(doubleValue);
                            this.cursorPointData.doubleValue = doubleValue;
                            this.cursorPointData.y = y;

                            base.Invalidate();

                        }
                        else
                        {
                            if (keyEventArgs_0.Control)
                                moveBy = -40;
                            else
                                moveBy = -10;
                            this.ScrollBy(moveBy);///WYJ fix
                        }
                        break;
                }
                base.OnKeyDown(keyEventArgs_0);
            }

            
        }

        protected override void OnMouseClick(MouseEventArgs mouseEventArgs_0)
        {
            base.Focus();
            if ((mouseEventArgs_0.Button == MouseButtons.Left) && (this.Mode == ChartMode.Normal))
            {
                using (IEnumerator<ChartPane> enumerator = this.Renderer.Panes.GetEnumerator())
                {
                    ChartPane current;
                    while (enumerator.MoveNext())
                    {
                        current = enumerator.Current;
                        if (current.HideDisplayPaneButton(mouseEventArgs_0.X, mouseEventArgs_0.Y))
                        {
                            goto Label_005B;
                        }
                    }
                    goto Label_00D1;
                Label_005B:
                    this.chartPane_0 = current;
                    try
                    {
                        Graphics graphics = base.CreateGraphics();
                        this.Renderer.HideDisplayPane(this.Bars, graphics, base.Width, base.Height, this.ChartStyle, this.chartPane_0);
                        graphics.Dispose();
                        this.Refresh();
                        this.int_3 = 0;
                    }
                    catch (Exception exception)
                    {
                        if (this.eventHandler_3 != null)
                        {
                            this.eventHandler_3(this, new ExceptionEventArgs(exception));
                        }
                    }
                }
            }
        Label_00D1:
            base.OnMouseClick(mouseEventArgs_0);
        }

        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            if (!this.HasValidChart)
            {
                goto Label_029B;
            }
            bool flag = false;
            if (mevent.Button == MouseButtons.Left)
            {
                using (IEnumerator<ChartPane> enumerator = this.Renderer.Panes.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        ChartPane current = enumerator.Current;
                        if (current.HideDisplayPaneButton(mevent.X, mevent.Y))
                        {
                            goto Label_0055;
                        }
                    }
                    goto Label_0063;
                Label_0055:
                    flag = true;
                }
            }
        Label_0063:
            if (!flag)
            {
                if ((type_0 != null) && (this.drawingObjectManager_0 != null))
                {
                    int num = this.Renderer.ConvertXToBar(mevent.X);
                    if (num == -1)
                    {
                        type_0 = null;
                        if (this.eventHandler_1 != null)
                        {
                            this.eventHandler_1(this, EventArgs.Empty);
                        }
                        base.OnMouseDown(mevent);
                        return;
                    }
                    ChartPane pane = this.Renderer.PaneFromY(mevent.Y);
                    if (pane == null)
                    {
                        type_0 = null;
                        this.Mode = ChartMode.Normal;
                        if (this.eventHandler_1 != null)
                        {
                            this.eventHandler_1(this, EventArgs.Empty);
                        }
                        base.OnMouseDown(mevent);
                        return;
                    }
                    double num2 = pane.ConvertYToValue(mevent.Y);
                    ChartDrawingObject obj2 = this.drawingObjectManager_0.CreateDrawingObject(pane, type_0, this.Bars.Date[num], num2);
                    type_0 = null;
                    if (this.eventHandler_1 != null)
                    {
                        this.eventHandler_1(this, EventArgs.Empty);
                    }
                    if (obj2 != null)
                    {
                        if (obj2.Handles.Count > 0)
                        {
                            this.drawingObjectManager_0.SelectedHandle = obj2.Handles[0];
                            this.DrawingManager.SelectedHandle.Owner.OnBeginDrag(this.drawingObjectManager_0.SelectedHandle);
                            this.Refresh();
                            this.Mode = ChartMode.DraggingHandle;
                        }
                        else
                        {
                            this.Mode = ChartMode.Normal;
                        }
                        this.drawingObjectManager_0.SaveDrawingObjects(this.Bars);
                    }
                }
                else if (mevent.Button == MouseButtons.Left)
                {
                    if (displayCrossHair)
                    {
                        this.Mode = ChartMode.SetCrosshairLocation;
                    }
                    else if ((this.DrawingManager != null) && (this.DrawingManager.SelectedHandle != null))
                    {
                        this.DrawingManager.SelectedHandle.Owner.OnBeginDrag(this.DrawingManager.SelectedHandle);
                        this.Mode = ChartMode.DraggingHandle;
                    }
                    else if (this.Mode == ChartMode.ResizingPaneMode)
                    {
                        this.int_2 = mevent.Y;
                        this.int_3 = 0;
                        this.chartPane_0 = this.Renderer.PaneFromY(mevent.Y);
                    }
                    else
                    {
                        this.Mode = ChartMode.DragScrollChart;
                        this.int_0 = this.Renderer.ConvertXToBar(mevent.X);
                    }
                }
                else if (this.DrawingManager != null)
                {
                    this.DrawingManager.SelectedHandle = null;
                }
            }
        Label_029B:
            base.OnMouseDown(mevent);
        }

        protected override void OnMouseLeave(EventArgs eventArgs_0)
        {
            this.glyphToolTip_0.Visible = false;
            this.priceToolTip_0.Visible = false;
            this.indicatorToolTip_0.Visible = false;
            this.generalToolTip_0.Visible = false;
            base.OnMouseLeave(eventArgs_0);
        }

        ///WYJ fix: code from Reflector
        
        protected /*override*/ void OnMouseMove2(MouseEventArgs mevent)
        {
            try
            {
                bool flag = false;
                bool flag2 = false;
                bool flag3 = false;
                bool flag4 = false;
                PlottedIndicator indicator = null;
                PlottedIndicator indicator2 = null;
                Position position = null;
                int x = mevent.X;
                int y = mevent.Y;
                if ((this.Mode == ChartMode.SetCrosshairLocation) && !displayCrossHair)
                {
                    this.Mode = ChartMode.Normal;
                }
                lock (this.object_0)
                {
                    if (!this.HasValidChart)
                    {
                        goto Label_09B5;
                    }
                    int barNumber = this.chartRenderer_0.ConvertXToBar(x);
                    if (barNumber > this.chartRenderer_0.RightEdgeBar)
                    {
                        barNumber = this.chartRenderer_0.RightEdgeBar;
                    }
                    int num4 = barNumber;
                    if (this.Mode == ChartMode.DragScrollChart)
                    {
                        if (barNumber != this.int_0)
                        {
                            int num5 = barNumber - this.int_0;
                            int count = this.hscrollBar_0.Value - num5;
                            if (count < 0)
                            {
                                count = 0;
                            }
                            if (count > this.bars_0.Count)
                            {
                                count = this.bars_0.Count;
                            }
                            this.hscrollBar_0.Value = count;
                            this.hscrollBar_0_Scroll(this, new ScrollEventArgs(ScrollEventType.ThumbPosition, this.hscrollBar_0.Value));
                            base.Invalidate();
                        }
                        this.method_2();
                        base.OnMouseMove(mevent);
                        return;
                    }
                    ChartPane pane = null;
                    using (IEnumerator<ChartPane> enumerator = this.chartRenderer_0.Panes.GetEnumerator())
                    {
                        ChartPane current;
                        while (enumerator.MoveNext())
                        {
                            current = enumerator.Current;
                            if (this.Mode != ChartMode.SetCrosshairLocation)
                            {
                                if ((!current.IsPricePane && (mevent.Button == MouseButtons.None)) && ((((y > current.Top) && (y <= (current.Top + 4))) && !current.AbovePricePane) || (((y < (current.Top + current.Height)) && (y >= ((current.Top + current.Height) - 4))) && current.AbovePricePane)))
                                {
                                    this.Mode = ChartMode.ResizingPaneMode;
                                    this.Mode = ChartMode.ResizingPaneMode;
                                }
                                else if ((this.Mode == ChartMode.ResizingPaneMode) && (mevent.Button != MouseButtons.Left))
                                {
                                    this.Mode = ChartMode.Normal;
                                }
                            }
                            if (pane == null)
                            {
                                pane = current;
                            }
                            else if (current.Top > pane.Top)
                            {
                                pane = current;
                            }
                            if ((y >= current.Top) && (y < (current.Top + current.Height)))
                            {
                                goto Label_0224;
                            }
                        }
                        goto Label_0903;
                    Label_0224:
                        if (this.eventHandler_0 != null)
                        {
                            if (x > (base.Width - this.Renderer.MarginRightWidth))
                            {
                                num4 = -1;
                                barNumber = this.chartRenderer_0.RightEdgeBar;
                            }
                            this.eventHandler_0(this, new BarNumberEventArgs(num4, current.ConvertYToValue(y), current));
                        }
                        if (((barNumber >= 0) && (this.Mode == ChartMode.ResizingPaneMode)) && (mevent.Button == MouseButtons.Left))
                        {
                            if (!this.chartPane_0.AbovePricePane)
                            {
                                this.int_3 += this.int_2 - y;
                            }
                            else
                            {
                                this.int_3 += y - this.int_2;
                            }
                            this.int_2 = y;
                            base.Invalidate();
                            base.OnMouseMove(mevent);
                            return;
                        }
                        if ((this.Mode == ChartMode.SetCrosshairLocation) && (mevent.Button == MouseButtons.Left))
                        {
                            int cursorPointBar = this.Renderer.ConvertXToBar(x);
                            if (cursorPointBar == -1)
                            {
                                cursorPointBar = this.Renderer.RightEdgeBar;
                            }
                            double pointValue = current.ConvertYToValue(y);
                            if (this.cursorPointData != null)
                            {
                                this.cursorPointData.barNum = cursorPointBar;
                                this.cursorPointData.doubleValue = pointValue;
                                this.cursorPointData.y = y;
                            }
                            else
                            {
                                this.cursorPointData = new Class2(cursorPointBar, pointValue, y);
                            }
                            base.Invalidate();
                            base.OnMouseMove(mevent);
                            return;
                        }
                        if ((barNumber >= 0) && (this.Mode == ChartMode.DraggingHandle))
                        {
                            ChartDrawingObjectHandle selectedHandle = this.drawingObjectManager_0.SelectedHandle;
                            selectedHandle.Date = this.Bars.Date[barNumber];
                            ChartPane pane3 = selectedHandle.Owner.Pane;
                            double num7 = pane3.ConvertYToValue(y);
                            if (selectedHandle.SnapToValue)
                            {
                                if (pane3.IsPricePane)
                                {
                                    if ((num7 >= this.Bars.Low[barNumber]) && (num7 <= this.Bars.High[barNumber]))
                                    {
                                        if (Math.Abs((double) (num7 - this.Bars.Open[barNumber])) < Math.Abs((double) (num7 - this.Bars.Close[barNumber])))
                                        {
                                            selectedHandle.Value = this.Bars.Open[barNumber];
                                        }
                                        else
                                        {
                                            selectedHandle.Value = this.Bars.Close[barNumber];
                                        }
                                    }
                                    else if (num7 <= this.Bars.Low[barNumber])
                                    {
                                        selectedHandle.Value = this.Bars.Low[barNumber];
                                    }
                                    else
                                    {
                                        selectedHandle.Value = this.Bars.High[barNumber];
                                    }
                                }
                                else if (pane3.PlottedIndicators.Count == 0)
                                {
                                    selectedHandle.Value = num7;
                                }
                                else
                                {
                                    double maxValue = double.MaxValue;
                                    PlottedIndicator indicator4 = null;
                                    foreach (PlottedIndicator indicator3 in pane3.PlottedIndicators)
                                    {
                                        double num8 = Math.Abs((double) (indicator3.Series[barNumber] - num7));
                                        if (num8 < maxValue)
                                        {
                                            maxValue = num8;
                                            indicator4 = indicator3;
                                        }
                                    }
                                    if (indicator4 == null)
                                    {
                                        selectedHandle.Value = num7;
                                    }
                                    else
                                    {
                                        selectedHandle.Value = indicator4.Series[barNumber];
                                    }
                                }
                            }
                            else
                            {
                                selectedHandle.Value = num7;
                            }
                            selectedHandle.Owner.OnDrag(selectedHandle);
                            base.Invalidate();
                            this.method_2();
                            base.OnMouseMove(mevent);
                            return;
                        }
                        if (num4 < 0)
                        {
                            goto Label_0903;
                        }
                        if ((this.DrawingManager != null) && this.DrawingManager.method_4(current, mevent.X, mevent.Y))
                        {
                            base.Invalidate();
                        }
                        if (current.HideDisplayPaneButton(x, y))
                        {
                            this.method_0(this.generalToolTip_0, x, y);
                            this.generalToolTip_0.RenderValue(current.GetHashCode(), current.HideDisplayPaneTooltip);
                            flag3 = true;
                        }
                        if (flag3 || current.Hidden)
                        {
                            goto Label_0903;
                        }
                        using (IEnumerator<PlottedIndicator> enumerator3 = current.PlottedIndicators.GetEnumerator())
                        {
                            PlottedIndicator indicator5;
                            while (enumerator3.MoveNext())
                            {
                                indicator5 = enumerator3.Current;
                                DataSeries series = indicator5.Series;
                                if (series.FirstValidValue <= barNumber)
                                {
                                    int num16 = current.ConvertValueToY(series[barNumber]);
                                    if (Math.Abs((int) (y - num16)) <= PixelSensitivity)
                                    {
                                        goto Label_0687;
                                    }
                                }
                            }
                            goto Label_06E8;
                        Label_0687:
                            if (this.indicatorToolTip_0.RepositionRequired(indicator5, barNumber))
                            {
                                this.method_0(this.indicatorToolTip_0, x, y);
                                this.indicatorToolTip_0.RenderValue(indicator5, barNumber);
                            }
                            flag2 = true;
                            indicator2 = indicator5;
                            if ((this.Mode == ChartMode.DraggingIndicator) && this.bool_4)
                            {
                                indicator = indicator5;
                                indicator5.Selected = true;
                            }
                        }
                    Label_06E8:
                        if ((current == this.chartRenderer_0.PricePane) && !flag2)
                        {
                            int num13 = current.ConvertValueToY(this.bars_0.High[barNumber]);
                            int num11 = current.ConvertValueToY(this.bars_0.Low[barNumber]);
                            if ((y >= num13) && (y <= num11))
                            {
                                if (this.priceToolTip_0.RepositionRequired(this.bars_0, barNumber))
                                {
                                    this.method_0(this.priceToolTip_0, x, y);
                                    this.priceToolTip_0.RenderValues(this.bars_0, barNumber);
                                }
                                flag = true;
                            }
                            using (List<ChartGlyph>.Enumerator enumerator4 = this.chartRenderer_0.Glyphs.GetEnumerator())
                            {
                                ChartGlyph glyph;
                                while (enumerator4.MoveNext())
                                {
                                    glyph = enumerator4.Current;
                                    if ((!glyph.IsTrade || this.chartRenderer_0.TradeAnnotationsVisible) && (((x >= glyph.X) && (y >= glyph.Y)) && ((x <= (glyph.X + glyph.Width)) && (y <= (glyph.Y + glyph.Height)))))
                                    {
                                        goto Label_07F7;
                                    }
                                }
                                goto Label_083E;
                            Label_07F7:
                                if (this.glyphToolTip_0.RepositionRequired(glyph))
                                {
                                    this.glyphToolTip_0.Glyph = glyph;
                                    this.method_0(this.glyphToolTip_0, x, y);
                                }
                                flag4 = true;
                                position = glyph.Position;
                            }
                        }
                    Label_083E:
                        foreach (PlottedSymbol symbol in current.PlottedSymbols)
                        {
                            int num14 = current.ConvertValueToY(symbol.Bars.High[barNumber]);
                            int num15 = current.ConvertValueToY(symbol.Bars.Low[barNumber]);
                            if ((y >= num14) && (y <= num15))
                            {
                                if (this.priceToolTip_0.RepositionRequired(symbol.Bars, barNumber))
                                {
                                    this.method_0(this.priceToolTip_0, x, y);
                                    this.priceToolTip_0.RenderValues(symbol.Bars, barNumber);
                                }
                                flag = true;
                            }
                        }
                    }
                Label_0903:
                    if (this.indicatorDragDropManager_0 != null)
                    {
                        this.indicatorDragDropManager_0.SelectedIndicator = indicator2;
                    }
                    this.priceToolTip_0.Visible = (flag && (this.Mode == ChartMode.Normal)) && this.bool_1;
                    this.indicatorToolTip_0.Visible = (flag2 && (this.Mode == ChartMode.Normal)) && this.bool_2;
                    this.generalToolTip_0.Visible = flag3 && (this.Mode == ChartMode.Normal);
                    this.glyphToolTip_0.Visible = (flag4 && (this.Mode == ChartMode.Normal)) && this.bool_3;
                    if (this.position_0 != position)
                    {
                        this.position_0 = position;
                        base.Invalidate();
                    }
                Label_09B5:;
                }
                if (this.plottedIndicator_0 != indicator)
                {
                    if (this.plottedIndicator_0 != null)
                    {
                        this.plottedIndicator_0.Selected = false;
                    }
                    this.plottedIndicator_0 = indicator;
                    base.Invalidate();
                }
            }
            catch
            {
            }
            base.OnMouseMove(mevent);
        }

        ///WYJ fix, code from JustDecompile, protected override void OnMouseMove(MouseEventArgs mevent)
        protected override void OnMouseMove(MouseEventArgs mevent)
        {
            bool flag;
            bool flag1;
            bool flag2;
            bool flag3;
            try
            {
                bool flag4 = false;
                bool flag5 = false;
                bool flag6 = false;
                bool flag7 = false;
                PlottedIndicator plottedIndicator = null;
                PlottedIndicator plottedIndicator1 = null;
                Position position = null;
                int x = mevent.X;
                int y = mevent.Y;
                if (this.Mode == ChartMode.SetCrosshairLocation && !Chart.displayCrossHair)
                {
                    this.Mode = ChartMode.Normal;
                }
                lock (this.object_0)
                {
                    if (this.HasValidChart)
                    {
                        int bar = this.chartRenderer_0.ConvertXToBar(x);
                        if (bar > this.chartRenderer_0.RightEdgeBar)
                        {
                            bar = this.chartRenderer_0.RightEdgeBar;
                        }
                        int num = bar;
                        if (this.Mode == ChartMode.DragScrollChart)
                        {
                            if (bar != this.int_0)
                            {
                                int int0 = bar - this.int_0;
                                int count = this.hscrollBar_0.Value - int0;
                                if (count < 0)
                                {
                                    count = 0;
                                }
                                if (count > this.bars_0.Count)
                                {
                                    count = this.bars_0.Count;
                                }
                                this.hscrollBar_0.Value = count;
                                this.hscrollBar_0_Scroll(this, new ScrollEventArgs(ScrollEventType.ThumbPosition, this.hscrollBar_0.Value));
                                base.Invalidate();
                            }
                            this.method_2();
                            base.OnMouseMove(mevent);
                            return;
                        }
                        else //(this.Mode != ChartMode.DragScrollChart)
                        {
                            ChartPane chartPane = null;
                            IEnumerator<ChartPane> enumerator = this.chartRenderer_0.Panes.GetEnumerator();
                            using (enumerator)
                            {
                                while (enumerator.MoveNext())
                                {
                                    ChartPane current = enumerator.Current;
                                    if (this.Mode != ChartMode.SetCrosshairLocation)
                                    {
                                        if (current.IsPricePane || mevent.Button != MouseButtons.None || (y <= current.Top || y > current.Top + 4 || current.AbovePricePane) && (y >= current.Top + current.Height || y < current.Top + current.Height - 4 || !current.AbovePricePane))
                                        {
                                            if (this.Mode == ChartMode.ResizingPaneMode && mevent.Button != MouseButtons.Left)
                                            {
                                                this.Mode = ChartMode.Normal;
                                            }
                                        }
                                        else
                                        {
                                            this.Mode = ChartMode.ResizingPaneMode;
                                            this.Mode = ChartMode.ResizingPaneMode;
                                        }
                                    }
                                    if (chartPane != null)
                                    {
                                        if (current.Top > chartPane.Top)
                                        {
                                            chartPane = current;
                                        }
                                    }
                                    else
                                    {
                                        chartPane = current;
                                    }
                                    if (y >= current.Top && y < current.Top + current.Height)
                                    {
                                        if (this.eventHandler_0 != null)
                                        {
                                            if (x > base.Width - this.Renderer.MarginRightWidth)
                                            {
                                                num = -1;
                                                bar = this.chartRenderer_0.RightEdgeBar;
                                            }
                                            this.eventHandler_0(this, new BarNumberEventArgs(num, current.ConvertYToValue(y), current));
                                        }
                                        
                                        if (bar >= 0 && this.Mode == ChartMode.ResizingPaneMode && mevent.Button == MouseButtons.Left)
                                        {
                                            if (this.chartPane_0.AbovePricePane)
                                                this.int_3 += y - this.int_2;
                                            else
                                                this.int_3 += this.int_2 - y;

                                            this.int_2 = y;
                                            base.Invalidate();
                                            base.OnMouseMove(mevent);
                                            return;
                                        } 
                                        //if (bar < 0 || this.Mode != ChartMode.ResizingPaneMode || mevent.Button != MouseButtons.Left)
                                        else if (this.Mode == ChartMode.SetCrosshairLocation && mevent.Button == MouseButtons.Left)
                                        {
                                            int cursorPointBar = this.Renderer.ConvertXToBar(x);
                                            if (cursorPointBar == -1)
                                            {
                                                cursorPointBar = this.Renderer.RightEdgeBar;
                                            }
                                            double value1 = current.ConvertYToValue(y);
                                            if (this.cursorPointData == null)
                                            {
                                                this.cursorPointData = new Chart.Class2(cursorPointBar, value1, y);
                                            }
                                            else
                                            {
                                                this.cursorPointData.barNum = cursorPointBar;
                                                this.cursorPointData.doubleValue = value1;
                                                this.cursorPointData.y = y;
                                            }
                                            base.Invalidate();
                                            base.OnMouseMove(mevent);
                                            return;
                                        }
                                        //if (this.Mode != ChartMode.SetCrosshairLocation || mevent.Button != MouseButtons.Left)
                                        else if (bar >= 0 && this.Mode == ChartMode.DraggingHandle)
                                        {
                                            ChartDrawingObjectHandle selectedHandle = this.drawingObjectManager_0.SelectedHandle;
                                            selectedHandle.Date = this.Bars.Date[bar];
                                            ChartPane pane = selectedHandle.Owner.Pane;
                                            double value = pane.ConvertYToValue(y);
                                            if (!selectedHandle.SnapToValue)
                                            {
                                                selectedHandle.Value = value;
                                            }
                                            else
                                            {
                                                if (!pane.IsPricePane)
                                                {
                                                    if (pane.PlottedIndicators.Count != 0)
                                                    {
                                                        double num3 = double.MaxValue;
                                                        PlottedIndicator plottedIndicator2 = null;
                                                        foreach (PlottedIndicator plottedIndicator3 in pane.PlottedIndicators)
                                                        {
                                                            double num4 = Math.Abs(plottedIndicator3.Series[bar] - value);
                                                            if (num4 >= num3)
                                                            {
                                                                continue;
                                                            }
                                                            num3 = num4;
                                                            plottedIndicator2 = plottedIndicator3;
                                                        }
                                                        if (plottedIndicator2 != null)
                                                        {
                                                            selectedHandle.Value = plottedIndicator2.Series[bar];
                                                        }
                                                        else
                                                        {
                                                            selectedHandle.Value = value;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        selectedHandle.Value = value;
                                                    }
                                                }
                                                else
                                                {
                                                    if (value < this.Bars.Low[bar] || value > this.Bars.High[bar])
                                                    {
                                                        if (value > this.Bars.Low[bar])
                                                        {
                                                            selectedHandle.Value = this.Bars.High[bar];
                                                        }
                                                        else
                                                        {
                                                            selectedHandle.Value = this.Bars.Low[bar];
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (Math.Abs(value - this.Bars.Open[bar]) >= Math.Abs(value - this.Bars.Close[bar]))
                                                        {
                                                            selectedHandle.Value = this.Bars.Close[bar];
                                                        }
                                                        else
                                                        {
                                                            selectedHandle.Value = this.Bars.Open[bar];
                                                        }
                                                    }
                                                }
                                            }
                                            selectedHandle.Owner.OnDrag(selectedHandle);
                                            base.Invalidate();
                                            this.method_2();
                                            base.OnMouseMove(mevent);
                                            return;
                                        }
                                        else //if (bar < 0 || this.Mode != ChartMode.DraggingHandle)
                                        {
                                            if (num < 0)
                                            {
                                                break;
                                            }
                                            if (this.DrawingManager != null && this.DrawingManager.method_4(current, mevent.X, mevent.Y))
                                            {
                                                base.Invalidate();
                                            }
                                            if (current.HideDisplayPaneButton(x, y))
                                            {
                                                this.method_0(this.generalToolTip_0, x, y);
                                                this.generalToolTip_0.RenderValue(current.GetHashCode(), current.HideDisplayPaneTooltip);
                                                flag6 = true;
                                            }
                                            if (flag6 || current.Hidden)
                                            {
                                                break;
                                            }
                                            IEnumerator<PlottedIndicator> enumerator1 = current.PlottedIndicators.GetEnumerator();
                                            using (enumerator1)
                                            {
                                                while (enumerator1.MoveNext())
                                                {
                                                    PlottedIndicator current1 = enumerator1.Current;
                                                    DataSeries series = current1.Series;
                                                    if (series.FirstValidValue <= bar)
                                                    {
                                                        int y1 = current.ConvertValueToY(series[bar]);
                                                        if (Math.Abs(y - y1) <= Chart.PixelSensitivity)
                                                        {
                                                            if (this.indicatorToolTip_0.RepositionRequired(current1, bar))
                                                            {
                                                                this.method_0(this.indicatorToolTip_0, x, y);
                                                                this.indicatorToolTip_0.RenderValue(current1, bar);
                                                            }
                                                            flag5 = true;
                                                            plottedIndicator1 = current1;
                                                            if (this.Mode != ChartMode.DraggingIndicator || !this.bool_4)
                                                            {
                                                                break;
                                                            }
                                                            plottedIndicator = current1;
                                                            current1.Selected = true;
                                                            break;
                                                        }
                                                    }
                                                            
                                                }
                                            }
                                            if (current == this.chartRenderer_0.PricePane && !flag5)
                                            {
                                                int num1 = current.ConvertValueToY(this.bars_0.High[bar]);
                                                int y2 = current.ConvertValueToY(this.bars_0.Low[bar]);
                                                if (y >= num1 && y <= y2)
                                                {
                                                    if (this.priceToolTip_0.RepositionRequired(this.bars_0, bar))
                                                    {
                                                        this.method_0(this.priceToolTip_0, x, y);
                                                        this.priceToolTip_0.RenderValues(this.bars_0, bar);
                                                    }
                                                    flag4 = true;
                                                }
                                                List<ChartGlyph>.Enumerator enumerator2 = this.chartRenderer_0.Glyphs.GetEnumerator();
                                                try
                                                {
                                                    while (enumerator2.MoveNext())
                                                    {
                                                        ChartGlyph chartGlyph = enumerator2.Current;
                                                        if ((!chartGlyph.IsTrade || this.chartRenderer_0.TradeAnnotationsVisible) && x >= chartGlyph.X && y >= chartGlyph.Y && x <= chartGlyph.X + chartGlyph.Width && y <= chartGlyph.Y + chartGlyph.Height)
                                                        {
                                                            if (this.glyphToolTip_0.RepositionRequired(chartGlyph))
                                                            {
                                                                this.glyphToolTip_0.Glyph = chartGlyph;
                                                                this.method_0(this.glyphToolTip_0, x, y);
                                                            }
                                                            flag7 = true;
                                                            position = chartGlyph.Position;
                                                            break;
                                                        }
                                                    }
                                                }
                                                finally
                                                {
                                                    ((IDisposable)enumerator2).Dispose();
                                                }
                                            }
                                            IEnumerator<PlottedSymbol> enumerator3 = current.PlottedSymbols.GetEnumerator();
                                            using (enumerator3)
                                            {
                                                while (enumerator3.MoveNext())
                                                {
                                                    PlottedSymbol plottedSymbol = enumerator3.Current;
                                                    int num2 = current.ConvertValueToY(plottedSymbol.Bars.High[bar]);
                                                    int y3 = current.ConvertValueToY(plottedSymbol.Bars.Low[bar]);
                                                    if (y < num2 || y > y3)
                                                    {
                                                        continue;
                                                    }
                                                    if (this.priceToolTip_0.RepositionRequired(plottedSymbol.Bars, bar))
                                                    {
                                                        this.method_0(this.priceToolTip_0, x, y);
                                                        this.priceToolTip_0.RenderValues(plottedSymbol.Bars, bar);
                                                    }
                                                    flag4 = true;
                                                }
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                            if (this.indicatorDragDropManager_0 != null)
                            {
                                this.indicatorDragDropManager_0.SelectedIndicator = plottedIndicator1;
                            }
                            PriceToolTip priceToolTip0 = this.priceToolTip_0;
                            flag = (!flag4 || this.Mode != ChartMode.Normal ? false : this.bool_1);
                            priceToolTip0.Visible = flag;
                            IndicatorToolTip indicatorToolTip0 = this.indicatorToolTip_0;
                            flag1 = (!flag5 || this.Mode != ChartMode.Normal ? false : this.bool_2);
                            indicatorToolTip0.Visible = flag1;
                            GeneralToolTip generalToolTip0 = this.generalToolTip_0;
                            flag2 = (!flag6 ? false : this.Mode == ChartMode.Normal);
                            generalToolTip0.Visible = flag2;
                            GlyphToolTip glyphToolTip0 = this.glyphToolTip_0;
                            flag3 = (!flag7 || this.Mode != ChartMode.Normal ? false : this.bool_3);
                            glyphToolTip0.Visible = flag3;
                            if (this.position_0 != position)
                            {
                                this.position_0 = position;
                                base.Invalidate();
                            }
                        }
                        
                    }
                }
                if (this.plottedIndicator_0 != plottedIndicator)
                {
                    if (this.plottedIndicator_0 != null)
                    {
                        this.plottedIndicator_0.Selected = false;
                    }
                    this.plottedIndicator_0 = plottedIndicator;
                    base.Invalidate();
                }
                base.OnMouseMove(mevent);
                return;
            }
            catch
            {
                base.OnMouseMove(mevent);
                return;
            }
        }

        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            if (((this.Mode == ChartMode.ResizingPaneMode) && (mevent.Button == MouseButtons.Left)) && (this.int_3 != 0))
            {
                try
                {
                    bool bPaneResizing = true;
                    if (this.chartPane_0.Hidden)
                    {
                        bPaneResizing = false;
                    }
                    else
                    {
                        int num = this.chartPane_0.HiddenHeight - this.chartPane_0.Height;
                        this.int_3 = (this.int_3 > num) ? this.int_3 : num;
                    }
                    Graphics graphics = base.CreateGraphics();
                    this.Renderer.Render(this.Bars, graphics, base.Width, base.Height, this.ChartStyle, bPaneResizing, this.int_3, this.chartPane_0.Description);
                    graphics.Dispose();
                    this.Mode = ChartMode.Normal;
                    this.Refresh();
                    this.int_3 = 0;
                }
                catch (Exception exception)
                {
                    if (this.eventHandler_3 != null)
                    {
                        this.eventHandler_3(this, new ExceptionEventArgs(exception));
                    }
                }
            }
            if (this.Mode == ChartMode.DraggingHandle)
            {
                ChartDrawingObjectHandle selectedHandle = this.DrawingManager.SelectedHandle;
                if (selectedHandle == null)
                {
                    return;
                }
                selectedHandle.Owner.OnEndDrag(selectedHandle);
                this.drawingObjectManager_0.SaveDrawingObjects(this.Bars);
                this.Refresh();
            }
            if (!displayCrossHair)
            {
                this.Mode = ChartMode.Normal;
            }
            base.OnMouseUp(mevent);
        }

        protected override void OnMouseWheel(MouseEventArgs mouseEventArgs_0)
        {
            base.OnMouseWheel(mouseEventArgs_0);
            if (this.eventHandler_2 != null)
            {
                this.eventHandler_2(this, mouseEventArgs_0);
            }
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            Graphics graphics = pevent.Graphics;
            if ((!base.DesignMode && (this.Renderer != null)) && ((this.Bars != null) && (this.ChartStyle != null)))
            {
                int height = base.Height;
                if (this.hscrollBar_0.Visible)
                {
                    height -= this.hscrollBar_0.Height;
                }
                try
                {
                    this.Renderer.Render(this.Bars, graphics, base.Width, height, this.ChartStyle);
                    if (this.drawingObjectManager_0 != null)
                    {
                        this.drawingObjectManager_0.method_3(graphics);
                    }
                    this.method_3();
                    if (this.position_0 != null)
                    {
                        int num2;
                        int num4;
                        int num5 = this.Renderer.ConvertBarToX(this.position_0.EntryBar);
                        int num3 = this.Renderer.PricePane.ConvertValueToY(this.position_0.EntryPrice);
                        if (!this.position_0.Active)
                        {
                            num2 = this.Renderer.ConvertBarToX(this.position_0.ExitBar);
                            num4 = this.Renderer.PricePane.ConvertValueToY(this.position_0.ExitPrice);
                        }
                        else
                        {
                            num2 = this.Renderer.Width - this.Renderer.MarginRightWidth;
                            num4 = num3;
                        }
                        Pen pen = new Pen(ChartRenderer.ReverseColor(this.Renderer.BackgroundColor));
                        this.Renderer.ClipToPane(graphics, this.Renderer.PricePane);
                        using (pen)
                        {
                            graphics.DrawLine(pen, num5, num3, num2, num4);
                        }
                        this.Renderer.ClipToPane(graphics, null);
                    }
                    if (displayCrossHair)
                    {
                        this.Mode = ChartMode.SetCrosshairLocation;
                        this.drawCrosshair(graphics);
                    }
                    if ((this.Mode == ChartMode.ResizingPaneMode) && (this.int_3 != 0))
                    {
                        Pen pen3 = new Pen(ChartRenderer.ReverseColor(this.Renderer.BackgroundColor));
                        graphics.DrawLine(pen3, 0, this.int_2, this.Renderer.ChartWidth, this.int_2);
                        pen3.Dispose();
                    }
                }
                catch (Exception exception)
                {
                    if (this.eventHandler_3 != null)
                    {
                        this.eventHandler_3(this, new ExceptionEventArgs(exception));
                    }
                }
                base.OnPaint(pevent);
            }
            else
            {
                Color backgroundColor;
                if (this.Renderer != null)
                {
                    backgroundColor = this.Renderer.BackgroundColor;
                }
                else
                {
                    backgroundColor = this.BackColor;
                }
                graphics.Clear(backgroundColor);
                int num6 = 20;
                Brush brush = new SolidBrush(this.ForeColor);
                if (base.DesignMode)
                {
                    graphics.DrawString("Design Mode", this.Font, brush, 20f, (float) num6);
                }
                else
                {
                    if (this.Bars == null)
                    {
                        graphics.DrawString("Click a Symbol in the DataSets tree to the left, or type a symbol into the entry field and click Go", this.Font, brush, 20f, (float) (num6 += 20));
                    }
                    if (this.ChartStyle == null)
                    {
                        graphics.DrawString("No ChartStyle object assigned", this.Font, brush, 20f, (float) num6);
                    }
                }
                brush.Dispose();
                base.OnPaint(pevent);
            }
        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
        }

        protected override void OnResize(EventArgs eventArgs_0)
        {
            this.hscrollBar_0.Top = base.Height - this.hscrollBar_0.Height;
            this.hscrollBar_0.Width = base.Width;
            base.OnResize(eventArgs_0);
        }

        public void ScrollToBar(int int_4)
        {
            if (((int_4 < this.Bars.Count) && (int_4 >= 0)) && (this.chartRenderer_0 != null))
            {
                int num = base.Width / this.chartRenderer_0.BarSpacing;
                int newValue = (int_4 + (num / 2)) + this.hscrollBar_0.LargeChange;
                if (newValue > this.hscrollBar_0.Maximum)
                {
                    newValue = this.hscrollBar_0.Maximum;
                }
                this.hscrollBar_0.Value = newValue;
                this.hscrollBar_0_Scroll(this, new ScrollEventArgs(ScrollEventType.EndScroll, newValue));
            }
        }

        /// <summary>
        /// ///WYJ fix
        /// </summary>
        /// <param name="int_4"></param>
        public void ScrollBy(int int_4)
        {
            int count = this.hscrollBar_0.Value - int_4;
            if (count < 0)
            {
                count = 0;
            }
            if (count > this.bars_0.Count)
            {
                count = this.bars_0.Count;
            }
            
            this.hscrollBar_0.Value = count;
            this.hscrollBar_0_Scroll(this, new ScrollEventArgs(ScrollEventType.ThumbPosition, this.hscrollBar_0.Value));

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
                lock (this.object_0)
                {
                    if (this.Mode != ChartMode.Wait)
                    {
                        this.Mode = ChartMode.Normal;
                    }
                    this.bars_0 = value;
                    if (this.bars_0 != null)
                    {
                        this.hscrollBar_0.Maximum = (this.bars_0.Count + this.hscrollBar_0.LargeChange) - 1;
                        this.hscrollBar_0.Value = this.bars_0.Count;
                        if (this.chartRenderer_0 != null)
                        {
                            this.chartRenderer_0.ScrollOffset = 0;
                        }
                        this.int_1 = -1;
                    }
                    if ((base.Width > 0) && (base.Height > 0))
                    {
                        if ((this.bitmap_0 == null) || (this.bitmap_0.Size != base.Size))
                        {
                            this.bitmap_0 = new Bitmap(base.Width, base.Height);
                        }
                        Graphics graphics = Graphics.FromImage(this.bitmap_0);
                        using (graphics)
                        {
                            PaintEventArgs e = new PaintEventArgs(graphics, base.Bounds);
                            this.OnPaint(e);
                        }
                    }
                    if ((this.DragDropManager != null) && (this.Mode != ChartMode.Wait))
                    {
                        this.DragDropManager.CreateDragDropIndicators();
                    }
                    this.scaleSelector_0.ChartScale = this.Bars.DataScale;
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public WealthLab.ChartStyle ChartStyle
        {
            get
            {
                return this.chartStyle_0;
            }
            set
            {
                this.chartStyle_0 = value;
                this.Refresh();
            }
        }

        public static bool DisplayCrossHair
        {
            get
            {
                return displayCrossHair;
            }
            set
            {
                displayCrossHair = value;
            }
        }

        public IndicatorDragDropManager DragDropManager
        {
            get
            {
                return this.indicatorDragDropManager_0;
            }
            set
            {
                this.indicatorDragDropManager_0 = value;
                if (this.indicatorDragDropManager_0 != null)
                {
                    this.indicatorDragDropManager_0.chart_0 = this;
                }
            }
        }

        public DrawingObjectManager DrawingManager
        {
            get
            {
                return this.drawingObjectManager_0;
            }
            set
            {
                this.drawingObjectManager_0 = value;
                if (this.drawingObjectManager_0 != null)
                {
                    this.drawingObjectManager_0.Chart = this;
                }
            }
        }

        public bool FundamentalTooltipVisible
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

        public Font HandleFont
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

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public bool HasValidChart
        {
            get
            {
                return ((((this.chartRenderer_0 != null) && (this.bars_0 != null)) && (this.chartStyle_0 != null)) && (this.bars_0.Count > 0));
            }
        }

        public bool IndicatorTooltipVisible
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

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ChartMode Mode
        {
            get
            {
                return this.chartMode_0;
            }
            set
            {
                this.chartMode_0 = value;
                switch (this.chartMode_0)
                {
                    case ChartMode.DragScrollChart:
                        this.Cursor = Cursors.NoMoveHoriz;
                        return;

                    case ChartMode.DraggingHandle:
                        this.Cursor = Cursors.Cross;
                        return;

                    case ChartMode.Wait:
                        this.Cursor = Cursors.WaitCursor;
                        return;

                    case ChartMode.ResizingPaneMode:
                        this.Cursor = Cursors.SizeNS;
                        return;

                    case ChartMode.SetCrosshairLocation:
                        this.Cursor = Cursors.Cross;
                        return;
                }
                this.Cursor = Cursors.Default;
            }
        }

        public bool MultiSymbolMode
        {
            [CompilerGenerated]
            get
            {
                return this.bool_6;
            }
            [CompilerGenerated]
            set
            {
                this.bool_6 = value;
            }
        }

        public bool PriceTooltipVisible
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

        public ChartRenderer Renderer
        {
            get
            {
                return this.chartRenderer_0;
            }
            set
            {
                this.chartRenderer_0 = value;
                this.Refresh();
            }
        }

        public bool ScrollBarVisible
        {
            get
            {
                return this.bool_0;
            }
            set
            {
                this.hscrollBar_0.Visible = value;
                this.bool_0 = value;
                this.Refresh();
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public static System.Type TypeOfObjectToDraw
        {
            get
            {
                return type_0;
            }
            set
            {
                type_0 = value;
            }
        }

        private class Class2
        {
            public double doubleValue;
            public int barNum;
            public int y;

            public Class2(int bar, double pointValue, int y)
            {
                this.barNum = bar;
                this.doubleValue = pointValue;
                this.y = y;
            }
        }
    }
}

