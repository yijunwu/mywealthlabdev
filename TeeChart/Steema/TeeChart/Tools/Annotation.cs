namespace Steema.TeeChart.Tools
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    [ToolboxBitmap(typeof(Annotation), "ToolsIcons.Annotation.bmp"), Description("Displays custom text at any location inside Chart.")]
    public class Annotation : Steema.TeeChart.Tools.Tool
    {
        private bool allowEdit;
        private bool bAutoSize;
        private AnnotationCallout callout;
        private bool cliptext;
        private System.Windows.Forms.Cursor cursor;
        private TextBox edit;
        protected bool isEditing;
        private AnnotationPositions position;
        private Steema.TeeChart.PositionUnits positionUnits;
        private TextShapePosition shape;
        private StringAlignment textAlign;
        protected int tmpX;
        protected int tmpY;

        public event MouseEventHandler Click;

        public event KeyEventHandler KeyDown;

        public Annotation() : this(null)
        {
        }

        public Annotation(Chart c) : base(c)
        {
            this.cliptext = true;
            this.Shape.iDrawText = false;
            this.shape.CustomPosition = false;
            this.shape.Shadow.Visible = true;
            this.Callout.Chart = base.chart;
            this.bAutoSize = true;
            this.allowEdit = false;
            this.positionUnits = Steema.TeeChart.PositionUnits.Pixels;
            this.position = AnnotationPositions.LeftTop;
            this.textAlign = StringAlignment.Near;
            this.cursor = Cursors.Default;
        }

        protected override void Assign(Steema.TeeChart.Tools.Tool t)
        {
            base.Assign(t);
            Annotation annotation = t as Annotation;
            TextShapePosition position = this.shape.Clone() as TextShapePosition;
            position.iDrawText = false;
            annotation.shape = position;
        }

        protected virtual void CalcTempPosition(out int x, out int y, int tmpW, int tmpH, int horizMargin, int vertMargin)
        {
            int num = (base.chart.Width - tmpW) - (2 * horizMargin);
            int num2 = (base.chart.Height - tmpH) - (4 * vertMargin);
            switch (this.position)
            {
                case AnnotationPositions.LeftTop:
                    x = 10;
                    y = 10;
                    return;

                case AnnotationPositions.LeftBottom:
                    x = 10;
                    y = num2;
                    return;

                case AnnotationPositions.RightTop:
                    x = num;
                    y = 10;
                    return;
            }
            x = num;
            y = num2;
        }

        protected virtual int CalcTempWidth(string tmp, out int NumLines)
        {
            return base.chart.MultiLineTextWidth(tmp, out NumLines);
        }

        protected virtual void CalcTextXY(ref int x, ref int y)
        {
        }

        protected internal override void ChartEvent(EventArgs e)
        {
            base.ChartEvent(e);
            if (e is AfterDrawEventArgs)
            {
                this.DrawText();
            }
        }

        protected internal bool Clicked(int x, int y)
        {
            return Rectangle.FromLTRB(this.Left, this.Top, this.Left + this.Width, this.Top + this.Height).Contains(x, y);
        }

        private void CreateAndShowEdit()
        {
            if (this.edit == null)
            {
                this.edit = new TextBox();
                this.edit.Hide();
                base.chart.Parent.GetControl().Controls.Add(this.edit);
            }
            if (this.KeyDown != null)
            {
                this.edit.KeyDown += this.KeyDown;
            }
            else
            {
                this.edit.KeyDown += new KeyEventHandler(this.EditorKey);
            }
            this.edit.Hide();
            this.edit.Multiline = true;
            this.edit.Font = this.Shape.Font.DrawingFont;
            this.edit.Left = this.Left;
            this.edit.Top = this.Top;
            this.edit.Show();
            this.edit.Text = this.Text;
            this.edit.Width = this.Width;
            this.edit.Height = this.Height;
            this.edit.Focus();
            this.isEditing = true;
        }

        protected virtual void DoDoubleClick(MouseEventArgs e)
        {
            this.CreateAndShowEdit();
        }

        protected virtual void DrawString(Graphics3D g, int x, int y, int t, int tmpHeight, string[] s)
        {
            g.TextOut(x, y + ((t - 1) * tmpHeight), s[t - 1]);
        }

        protected internal void DrawText()
        {
            int num2;
            int num3;
            int left;
            int num5;
            string text = this.Text;
            Graphics3D g = base.chart.graphics3D;
            g.Font = this.Shape.Font;
            int fontHeight = g.FontHeight;
            if (text.Length == 0)
            {
                text = " ";
            }
            Rectangle r = this.GetTextBounds(out num2, out num3, out left, out num5);
            this.shape.ShapeBounds = r;
            Rectangle shapeBounds = this.shape.ShapeBounds;
            if (this.shape.Visible)
            {
                this.shape.Paint();
            }
            if (this.cliptext)
            {
                r.Offset(base.chart.ChartBounds.Left, base.chart.ChartBounds.Top);
                g.ClearClipRegions();
                g.ClipRectangle(r);
            }
            int num6 = Utils.Round(g.TextWidth("W"));
            switch (this.TextAlign)
            {
                case StringAlignment.Near:
                    left = this.shape.ShapeBounds.Left;
                    break;

                case StringAlignment.Center:
                    left = Utils.Round((float) ((this.shape.ShapeBounds.Left + this.shape.ShapeBounds.Right) / 2)) - Utils.Round((float) (num3 / 2));
                    break;

                case StringAlignment.Far:
                    left = (this.shape.ShapeBounds.Right - num3) - Utils.Round((float) (num6 / g.Font.Size));
                    break;
            }
            string[] s = text.Split(new char[] { '\n' });
            for (int i = 1; i <= num2; i++)
            {
                this.CalcTextXY(ref left, ref num5);
                this.DrawString(g, left, num5, i, fontHeight, s);
            }
            if (this.cliptext)
            {
                g.UnClip();
            }
            if (this.callout.Visible || this.callout.Arrow.Visible)
            {
                Point p = new Point(this.callout.XPosition, this.callout.YPosition);
                Point aFrom = this.callout.CloserPoint(this.shape.ShapeBounds, p);
                if (this.callout.Distance != 0)
                {
                    p = Utils.PointAtDistance(aFrom, p, this.callout.Distance);
                }
                this.callout.Draw(Utils.EmptyColor, p, new Point(0, 0), aFrom, this.callout.ZPosition, false);
            }
            if (this.AutoSize)
            {
                left = this.shape.ShapeBounds.Right - this.shape.ShapeBounds.Left;
                num5 = this.shape.ShapeBounds.Bottom - this.shape.ShapeBounds.Top;
                r = Utils.FromLTRB(shapeBounds.Left, shapeBounds.Top, shapeBounds.Left + left, shapeBounds.Top + num5);
                this.shape.ShapeBounds = r;
            }
            else
            {
                this.shape.ShapeBounds = shapeBounds;
            }
        }

        public void EditorKey(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                case Keys.Down:
                case Keys.F2:
                case Keys.Escape:
                    this.EndEdit();
                    break;

                case Keys.Right:
                    break;

                default:
                    return;
            }
        }

        public void EndEdit()
        {
            if ((this.edit != null) && this.edit.Visible)
            {
                if (!Utils.IsNullOrEmpty(this.edit.Text))
                {
                    this.Text = this.edit.Text;
                }
                this.edit.Hide();
                this.isEditing = false;
                base.chart.Parent.GetControl().Focus();
            }
        }

        protected virtual string GetInnerText()
        {
            return this.shape.Text;
        }

        private Rectangle GetTextBounds(out int NumLines, out int TextWidth, out int x, out int y)
        {
            int height;
            int width;
            string text = this.Text;
            int horizMargin = 4;
            int vertMargin = 2;
            if (text.Length == 0)
            {
                text = " ";
            }
            Graphics3D graphicsd = base.chart.graphics3D;
            graphicsd.Font = this.shape.Font;
            int fontHeight = graphicsd.FontHeight;
            int tmpW = this.CalcTempWidth(text, out NumLines);
            int tmpH = NumLines * fontHeight;
            int num4 = this.shape.Pen.Visible ? ((1 + this.shape.Pen.Width) / 2) : 0;
            tmpH += num4;
            tmpW += num4;
            if (this.shape.CustomPosition)
            {
                if (this.positionUnits == Steema.TeeChart.PositionUnits.Pixels)
                {
                    x = this.shape.ShapeBounds.Left + horizMargin;
                    y = this.shape.ShapeBounds.Top + vertMargin;
                }
                else
                {
                    x = Utils.Round((float) ((this.shape.ShapeBounds.Left * 0.01f) * base.Chart.Width));
                    y = Utils.Round((float) ((this.shape.ShapeBounds.Top * 0.01f) * base.Chart.Height));
                }
            }
            else
            {
                this.CalcTempPosition(out x, out y, tmpW, tmpH, horizMargin, vertMargin);
            }
            if (this.AutoSize)
            {
                width = (tmpW + horizMargin) + 4;
                height = (tmpH + vertMargin) + 2;
            }
            else
            {
                width = this.shape.Width;
                height = this.shape.Height;
            }
            TextWidth = tmpW;
            x += num4 / 2;
            y += num4 / 2;
            return Utils.FromLTRB(x - horizMargin, y - vertMargin, (x - horizMargin) + width, (y - vertMargin) + height);
        }

        protected internal override void KeyEvent(KeyEventArgs e)
        {
            if (this.Clicked(this.tmpX, this.tmpY) && this.allowEdit)
            {
                this.CreateAndShowEdit();
            }
        }

        protected internal override void MouseEvent(MouseEventKinds kind, MouseEventArgs e, ref System.Windows.Forms.Cursor c)
        {
            if (kind == MouseEventKinds.Down)
            {
                if (e.Clicks == 2)
                {
                    this.OnDoubleClick(e);
                }
                else
                {
                    this.OnClick(e);
                }
            }
            else
            {
                Point point = new Point(e.X, e.Y);
                if ((kind == MouseEventKinds.Move) && (this.cursor != Cursors.Default))
                {
                    if (this.Clicked(point.X, point.Y))
                    {
                        c = this.cursor;
                        base.Chart.CancelMouse = true;
                    }
                }
                else if (kind == MouseEventKinds.Move)
                {
                    this.tmpX = point.X;
                    this.tmpY = point.Y;
                }
            }
        }

        protected virtual void OnClick(MouseEventArgs e)
        {
            Point point = new Point(e.X, e.Y);
            if ((this.Click != null) && this.Clicked(point.X, point.Y))
            {
                this.Click(this, e);
            }
        }

        protected virtual void OnDoubleClick(MouseEventArgs e)
        {
            Point point = new Point(e.X, e.Y);
            if (this.Clicked(point.X, point.Y) && this.allowEdit)
            {
                this.DoDoubleClick(e);
            }
        }

        protected override void SetChart(Chart c)
        {
            base.SetChart(c);
            this.Shape.Chart = base.chart;
            this.Callout.Chart = base.chart;
        }

        private bool ShouldSerializeBottom()
        {
            return !this.AutoSize;
        }

        protected bool ShouldSerializeHeight()
        {
            return !this.AutoSize;
        }

        private bool ShouldSerializeLeft()
        {
            return this.shape.CustomPosition;
        }

        private bool ShouldSerializeRight()
        {
            return !this.AutoSize;
        }

        private bool ShouldSerializeTop()
        {
            return this.shape.CustomPosition;
        }

        protected bool ShouldSerializeWidth()
        {
            return !this.AutoSize;
        }

        public void StartEdit()
        {
            this.CreateAndShowEdit();
        }

        [Description("Allows the Text of the Tool to be edited."), DefaultValue(false)]
        public bool AllowEdit
        {
            get
            {
                return this.allowEdit;
            }
            set
            {
                this.allowEdit = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), Description("Set to false to permit custom sizing of TextShape.")]
        public bool AutoSize
        {
            get
            {
                return this.bAutoSize;
            }
            set
            {
                base.SetBooleanProperty(ref this.bAutoSize, value);
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Description("The rectangular bounds of the annotation tool.")]
        public Rectangle Bounds
        {
            get
            {
                int num;
                int num2;
                int num3;
                int num4;
                return this.GetTextBounds(out num, out num2, out num3, out num4);
            }
            set
            {
                this.Shape.ShapeBounds = value;
            }
        }

        public AnnotationCallout Callout
        {
            get
            {
                if (this.callout == null)
                {
                    this.callout = new AnnotationCallout(null);
                }
                return this.callout;
            }
        }

        [DefaultValue(true), Description("Clip annotation text.")]
        public bool ClipText
        {
            get
            {
                return this.cliptext;
            }
            set
            {
                base.SetBooleanProperty(ref this.cliptext, value);
            }
        }

        [Category("Appearance"), DefaultValue(typeof(Cursors), "Default")]
        public System.Windows.Forms.Cursor Cursor
        {
            get
            {
                return this.cursor;
            }
            set
            {
                this.cursor = value;
            }
        }

        [Description("Gets descriptive text.")]
        public override string Description
        {
            get
            {
                return Texts.AnnotationTool;
            }
        }

        [Description("Sets a custom height.")]
        public int Height
        {
            get
            {
                return this.shape.Height;
            }
            set
            {
                this.shape.Height = value;
                this.AutoSize = false;
            }
        }

        [Description("Sets horizontal displacement in pixels of text box from Chart's left edge.")]
        public int Left
        {
            get
            {
                return this.shape.Left;
            }
            set
            {
                this.shape.Left = value;
                this.shape.CustomPosition = true;
            }
        }

        [DefaultValue(0), Description("Sets the position of Annotation Tool text box and text.")]
        public AnnotationPositions Position
        {
            get
            {
                return this.position;
            }
            set
            {
                if (this.position != value)
                {
                    this.position = value;
                    this.shape.CustomPosition = false;
                    this.Invalidate();
                }
            }
        }

        [DefaultValue(1), Description("Defines Annotation Position units (pixels or percentage).")]
        public Steema.TeeChart.PositionUnits PositionUnits
        {
            get
            {
                return this.positionUnits;
            }
            set
            {
                if (this.positionUnits != value)
                {
                    this.positionUnits = value;
                    this.Invalidate();
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Sets characteristics of the Annotation Tool text and text box Shape.")]
        public TextShapePosition Shape
        {
            get
            {
                if (this.shape == null)
                {
                    this.shape = new TextShapePosition(base.chart);
                }
                return this.shape;
            }
        }

        [Description("Gets detailed descriptive text.")]
        public override string Summary
        {
            get
            {
                return Texts.AnnotationSummary;
            }
        }

        [Description("Defines the text for the Annotation Tool."), DefaultValue(""), Localizable(true)]
        public string Text
        {
            get
            {
                return this.GetInnerText();
            }
            set
            {
                this.shape.Text = value;
            }
        }

        [DefaultValue(0), Description("Horizontal alignment of displayed text.")]
        public StringAlignment TextAlign
        {
            get
            {
                return this.textAlign;
            }
            set
            {
                if (this.textAlign != value)
                {
                    this.textAlign = value;
                    this.Invalidate();
                }
            }
        }

        [Description("Sets vertical displacement in pixels of text box from Chart's top edge.")]
        public int Top
        {
            get
            {
                return this.shape.Top;
            }
            set
            {
                this.shape.Top = value;
                this.shape.CustomPosition = true;
            }
        }

        [Description("Sets a custom width.")]
        public int Width
        {
            get
            {
                return this.shape.Width;
            }
            set
            {
                this.shape.Width = value;
                this.AutoSize = false;
            }
        }
    }
}

