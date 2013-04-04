namespace QWhale.Common
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    [ToolboxItem(false)]
    public class SpeedButton : Control
    {
        private bool allowCheck;
        private EditBorderStyle borderStyle;
        private const int DefaultButtonSize = 0x12;
        private bool hot;
        private int imageIndex;
        private System.Windows.Forms.ImageList images;
        private bool isChecked;
        private IPainter painter;

        [Description("Occurs when the value of the Checked property changes.")]
        public event EventHandler CheckedChanged;

        public SpeedButton()
        {
            this.imageIndex = -1;
            base.SetStyle(ControlStyles.DoubleBuffer, true);
            base.Width = 0x12;
            base.Height = 0x12;
            this.painter = new GdiPainter();
            base.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.StandardDoubleClick | ControlStyles.UserMouse | ControlStyles.Selectable | ControlStyles.StandardClick | ControlStyles.UserPaint, true);
            base.SetStyle(ControlStyles.Opaque, false);
            this.Cursor = Cursors.Default;
            this.BackColor = SystemColors.Control;
        }

        public SpeedButton(IContainer container) : this()
        {
            if (container != null)
            {
                container.Add(this);
            }
        }

        private Color GetBkColor()
        {
            switch (this.borderStyle)
            {
                case EditBorderStyle.None:
                case EditBorderStyle.FixedSingle:
                    if (!this.Pressed)
                    {
                        if (this.Hot)
                        {
                            return Consts.DefaultHotTrackBackColor;
                        }
                        break;
                    }
                    return Consts.DefaultPressedBackColor;
            }
            return this.BackColor;
        }

        protected virtual void OnAllowCheckChanged()
        {
        }

        protected virtual void OnBorderStyleChanged()
        {
            base.Invalidate();
        }

        public virtual void OnCheckedChanged(EventArgs args)
        {
            if (this.CheckedChanged != null)
            {
                this.CheckedChanged(this, args);
            }
        }

        protected virtual void OnImageIndexChanged()
        {
            base.Invalidate();
        }

        protected virtual void OnImagesChanged()
        {
            base.Invalidate();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (this.allowCheck && (e.Button == MouseButtons.Left))
            {
                this.Checked = !this.Checked;
            }
            base.Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            this.Hot = false;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            this.Hot = true;
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            base.Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Rectangle clientRectangle = base.ClientRectangle;
            this.painter.BeginPaint(e.Graphics);
            try
            {
                switch (this.borderStyle)
                {
                    case EditBorderStyle.None:
                        if (this.hot)
                        {
                            this.painter.BackColor = Consts.DefaultHotTrackColor;
                            this.painter.DrawRectangle(new Rectangle(clientRectangle.Left, clientRectangle.Top, clientRectangle.Width - 1, clientRectangle.Height - 1));
                            clientRectangle.Inflate(-1, -1);
                        }
                        goto Label_0138;

                    case EditBorderStyle.Fixed3D:
                    case EditBorderStyle.System:
                        if ((this.borderStyle != EditBorderStyle.System) || (XPThemes.CurrentTheme == XPThemeName.None))
                        {
                            break;
                        }
                        XPThemes.DrawPushButton(this.painter, clientRectangle, this.Pressed, this.Hot);
                        goto Label_0138;

                    case EditBorderStyle.FixedSingle:
                        this.painter.BackColor = Consts.DefaultWindowFrameColor;
                        this.painter.DrawRectangle(new Rectangle(clientRectangle.Left, clientRectangle.Top, clientRectangle.Width - 1, clientRectangle.Height - 1));
                        clientRectangle.Inflate(-1, -1);
                        goto Label_0138;

                    default:
                        goto Label_0138;
                }
                Rectangle rectangle2 = clientRectangle;
                this.painter.DrawEdge(ref clientRectangle, this.Pressed ? Border3DStyle.Sunken : Border3DStyle.Raised, Border3DSide.All);
                clientRectangle = rectangle2;
                clientRectangle.Inflate(-2, -2);
            Label_0138:
                this.painter.BackColor = this.GetBkColor();
                if (this.borderStyle != EditBorderStyle.System)
                {
                    this.painter.FillRectangle(clientRectangle);
                }
                if (this.images != null)
                {
                    Rectangle rect = new Rectangle(Point.Empty, this.images.ImageSize);
                    int x = base.ClientRectangle.Left + ((clientRectangle.Width - rect.Width) / 2);
                    int y = base.ClientRectangle.Top + ((clientRectangle.Height - rect.Height) / 2);
                    rect.Offset(x, y);
                    rect.Intersect(clientRectangle);
                    this.painter.DrawImage(this.images, this.imageIndex, rect);
                }
            }
            finally
            {
                this.painter.EndPaint();
            }
        }

        public bool AllowCheck
        {
            get
            {
                return this.allowCheck;
            }
            set
            {
                if (this.allowCheck != value)
                {
                    this.allowCheck = value;
                    this.OnAllowCheckChanged();
                }
            }
        }

        public EditBorderStyle BorderStyle
        {
            get
            {
                return this.borderStyle;
            }
            set
            {
                if (this.borderStyle != value)
                {
                    this.borderStyle = value;
                    this.OnBorderStyleChanged();
                }
            }
        }

        public bool Checked
        {
            get
            {
                return this.isChecked;
            }
            set
            {
                if (this.isChecked != value)
                {
                    this.isChecked = value;
                    this.OnCheckedChanged(new EventArgs());
                    base.Invalidate();
                }
            }
        }

        protected bool Hot
        {
            get
            {
                return this.hot;
            }
            set
            {
                if (this.hot != value)
                {
                    this.hot = value;
                    base.Invalidate();
                }
            }
        }

        public int ImageIndex
        {
            get
            {
                return this.imageIndex;
            }
            set
            {
                if (this.imageIndex != value)
                {
                    this.imageIndex = value;
                    this.OnImageIndexChanged();
                }
            }
        }

        public System.Windows.Forms.ImageList ImageList
        {
            get
            {
                return this.images;
            }
            set
            {
                if (this.images != value)
                {
                    this.images = value;
                    this.OnImagesChanged();
                }
            }
        }

        protected bool Pressed
        {
            get
            {
                if (this.allowCheck)
                {
                    return this.isChecked;
                }
                return (this.hot && (Control.MouseButtons == MouseButtons.Left));
            }
        }
    }
}

