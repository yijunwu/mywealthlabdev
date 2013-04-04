namespace QWhale.Editor.CodeCompletion
{
    using QWhale.Common;
    using QWhale.Editor;
    using QWhale.Syntax;
    using QWhale.Syntax.CodeCompletion;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    [DesignTimeVisible(false), ToolboxItem(false)]
    public class CodeCompletionWindow : Form, ICodeCompletionWindow, IControl
    {
        private CodeCompletionFlags completionFlags;
        private Point displayPos;
        private Point endPos;
        private ImageList images;
        private Control ownerControl;
        private Control popupControl;
        private Control prevFocused;
        private ICodeCompletionProvider provider;
        protected bool showing;
        private bool sizeable;
        private Point startPos;

        public event ClosePopupEvent ClosePopup;

        public event QWhale.Editor.KeyPreviewEvent KeyPreviewEvent;

        public event ShowPopupEvent ShowPopup;

        public CodeCompletionWindow()
        {
            this.completionFlags = EditConsts.DefaultCodeCompletionFlags;
            this.MinimumSize = new Size(this.Font.Height + 2, this.Font.Height + 2);
            base.SizeGripStyle = SizeGripStyle.Hide;
            base.StartPosition = FormStartPosition.Manual;
            base.FormBorderStyle = FormBorderStyle.FixedSingle;
            base.Width = 200;
            base.Height = 200;
            base.ShowInTaskbar = false;
            base.ControlBox = false;
            this.BackColor = Consts.DefaultControlBackColor;
            base.SetStyle(ControlStyles.ResizeRedraw, true);
            base.TopLevel = true;
            base.DoubleClick += new EventHandler(this.DoDoubleClick);
            base.Click += new EventHandler(this.DoClick);
            base.LostFocus += new EventHandler(this.DoLostFocus);
            this.popupControl = this.CreatePopupControl();
            if (this.popupControl != null)
            {
                this.popupControl.DoubleClick += new EventHandler(this.DoDoubleClick);
                this.popupControl.Click += new EventHandler(this.DoClick);
                this.popupControl.LostFocus += new EventHandler(this.DoLostFocus);
                base.Controls.Clear();
                base.Controls.Add(this.popupControl);
                this.popupControl.Dock = DockStyle.Fill;
            }
            this.AutoSize = true;
        }

        public CodeCompletionWindow(ISyntaxEdit owner) : this()
        {
            this.ownerControl = (Control) owner;
            base.Owner = this.GetParentForm((Control) owner);
        }

        protected virtual void CheckMouseLeave(object sender, EventArgs e)
        {
            if ((base.Visible && ((this.CompletionFlags & CodeCompletionFlags.CloseOnMouseLeave) != CodeCompletionFlags.None)) && (((this.ownerControl == null) || !this.ownerControl.Bounds.Contains(Control.MousePosition)) || ((sender == this.ownerControl) && !base.Bounds.Contains(Control.MousePosition))))
            {
                this.CloseDelayed(false);
            }
        }

        protected virtual void CheckPositionValid()
        {
            if (((this.CompletionFlags & CodeCompletionFlags.KeepActive) == CodeCompletionFlags.None) && !this.IsPositionValid())
            {
                this.Close(false);
            }
        }

        public virtual void Close(bool accept)
        {
            this.Close(accept, true);
        }

        public virtual void Close(bool accept, bool setFocus)
        {
            if (base.Visible)
            {
                if (this.Provider != null)
                {
                    this.Provider.SelIndex = this.GetSelectedIndex();
                }
                ClosingEventArgs args = new ClosingEventArgs(accept, this.Provider) {
                    UseIndent = (this.Provider != null) && this.Provider.UseIndent,
                    Text = (((this.Provider != null) && (this.Provider.ColumnCount != 0)) && ((this.Provider.SelIndex >= 0) && (this.Provider.SelIndex < this.Provider.Count))) ? this.Provider.GetText(this.Provider.SelIndex) : string.Empty
                };
                this.OnClosePopup(args);
                if (!args.Handled)
                {
                    PopupHookManager.PopupClosed(this);
                    this.DoHide();
                    if ((setFocus && (this.prevFocused != null)) && this.prevFocused.CanFocus)
                    {
                        this.prevFocused.Focus();
                    }
                }
            }
        }

        public virtual void CloseDelayed(bool accept)
        {
            if (base.IsHandleCreated)
            {
                OSUtils.PostMessage(base.Handle, 0x401, accept ? new IntPtr(1) : IntPtr.Zero, IntPtr.Zero);
            }
            else
            {
                this.Close(accept);
            }
        }

        protected virtual void CodeEditDisposed()
        {
        }

        public virtual bool ContainsControl(Control control)
        {
            return ((control != null) && base.Contains(control));
        }

        protected virtual Control CreatePopupControl()
        {
            return null;
        }

        protected void DoClick(object sender, EventArgs e)
        {
            if ((this.CompletionFlags & CodeCompletionFlags.AcceptOnClick) != CodeCompletionFlags.None)
            {
                this.Close(true);
            }
        }

        protected void DoDoubleClick(object sender, EventArgs e)
        {
            if ((this.CompletionFlags & CodeCompletionFlags.AcceptOnDblClick) != CodeCompletionFlags.None)
            {
                this.Close(true);
            }
        }

        protected virtual void DoHide()
        {
            base.Visible = false;
        }

        protected void DoLostFocus(object sender, EventArgs e)
        {
            if (!this.showing && this.NeedCloseOnLostFocus())
            {
                this.Close(false, false);
            }
        }

        protected virtual void DoProcessKeyMessage(ref Message m)
        {
            ISyntaxEdit syntaxEdit = this.GetSyntaxEdit();
            if (syntaxEdit != null)
            {
                Message msg = new Message {
                    HWnd = syntaxEdit.Handle,
                    LParam = m.LParam,
                    Msg = m.Msg,
                    Result = m.Result,
                    WParam = m.WParam
                };
                syntaxEdit.ProcessKeyMsg(ref msg);
                m.Result = msg.Result;
            }
        }

        protected virtual void DoShow(Point position)
        {
            this.prevFocused = null;
            if (this.ownerControl is ISyntaxEdit)
            {
                if ((this.ownerControl.Focused || ((ISyntaxEdit) this.ownerControl).CodeCompletionWindowFocused(out this.prevFocused)) && (this.prevFocused == null))
                {
                    this.prevFocused = this.ownerControl;
                }
            }
            else
            {
                this.prevFocused = ((this.ownerControl != null) && this.ownerControl.Focused) ? this.ownerControl : null;
            }
            base.Location = position;
            OSUtils.ShowWindowTopMost(base.Handle, position.X, position.Y, base.Width, base.Height);
            base.Visible = true;
            if (this.popupControl.CanFocus)
            {
                this.popupControl.Focus();
            }
        }

        public virtual void EnsureVisible(ref Point position)
        {
            Rectangle workingArea = Screen.GetWorkingArea(position);
            Rectangle rect = new Rectangle(position, base.Bounds.Size);
            if (!workingArea.Contains(rect))
            {
                if (rect.X < workingArea.Left)
                {
                    position.X = workingArea.Left;
                }
                else if (rect.Right > workingArea.Right)
                {
                    position.X = workingArea.Right - rect.Width;
                }
                if (rect.Y < workingArea.Top)
                {
                    position.Y = workingArea.Top;
                }
                else if (rect.Bottom > workingArea.Bottom)
                {
                    ISyntaxEdit syntaxEdit = this.GetSyntaxEdit();
                    int num = (syntaxEdit != null) ? syntaxEdit.Painter.FontHeight : base.FontHeight;
                    position.Y = Math.Min((int) ((position.Y - rect.Height) - num), (int) (workingArea.Bottom - rect.Height));
                }
            }
        }

        ~CodeCompletionWindow()
        {
            this.popupControl.DoubleClick -= new EventHandler(this.DoDoubleClick);
            this.popupControl.Click -= new EventHandler(this.DoClick);
            this.popupControl.LostFocus -= new EventHandler(this.DoLostFocus);
            base.Owner = null;
        }

        private Form GetParentForm(Control c)
        {
            while (c != null)
            {
                if (c is Form)
                {
                    return (Form) c;
                }
                c = c.Parent;
            }
            return null;
        }

        protected virtual int GetSelectedIndex()
        {
            return -1;
        }

        protected ISyntaxEdit GetSyntaxEdit()
        {
            if (this.ownerControl is ISyntaxEdit)
            {
                return (ISyntaxEdit) this.ownerControl;
            }
            return null;
        }

        public virtual bool IsFocused()
        {
            if (!this.Focused)
            {
                return this.popupControl.Focused;
            }
            return true;
        }

        protected virtual bool IsPopupControlFocused()
        {
            return this.PopupControl.Focused;
        }

        private bool IsPositionValid()
        {
            bool flag = true;
            ISyntaxEdit syntaxEdit = this.GetSyntaxEdit();
            if (syntaxEdit != null)
            {
                Point position = syntaxEdit.Position;
                if (this.startPos.Y >= 0)
                {
                    flag = (position.Y > this.startPos.Y) || ((position.Y == this.startPos.Y) && (position.X >= this.startPos.X));
                }
                if (flag && (this.endPos.Y >= 0))
                {
                    flag = (position.Y < this.endPos.Y) || ((position.Y == this.endPos.Y) && (position.X <= this.endPos.X));
                }
            }
            return flag;
        }

        protected virtual bool NeedCloseOnLostFocus()
        {
            return PopupHookManager.NeedCloseOnLostFocus(this);
        }

        protected virtual void OnAutoSizeChanged()
        {
            if (this.AutoSize)
            {
                this.UpdateAutoSize();
            }
        }

        protected virtual void OnClosePopup(ClosingEventArgs args)
        {
            args.StartPosition = this.startPos;
            args.EndPosition = this.endPos;
            if (this.Provider != null)
            {
                this.Provider.OnClosePopup(this, args);
                if (args.Accepted && args.Handled)
                {
                    this.Provider = args.Provider;
                    return;
                }
            }
            if (this.ClosePopup != null)
            {
                this.ClosePopup(this, args);
            }
        }

        protected virtual void OnCompletionFlagsChanged()
        {
        }

        protected virtual void OnDisplayPosChanged()
        {
        }

        protected virtual void OnEndPosChanged()
        {
        }

        protected virtual void OnImagesChanged()
        {
        }

        protected virtual void OnOwnerControlChanged()
        {
        }

        protected virtual bool OnProcessKeyPreview(ref Message m)
        {
            if (this.KeyPreviewEvent != null)
            {
                KeyPreviewEventArgs e = new KeyPreviewEventArgs(m);
                this.KeyPreviewEvent(this, e);
                return e.Handled;
            }
            return false;
        }

        protected virtual void OnShowPopup(ShowingEventArgs args)
        {
            if (this.ShowPopup != null)
            {
                this.ShowPopup(this, args);
            }
        }

        protected virtual void OnSizeableChanged()
        {
            if (this.sizeable)
            {
                base.FormBorderStyle = FormBorderStyle.Sizable;
            }
            else
            {
                base.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            }
        }

        protected virtual void OnStartPosChanged()
        {
        }

        public virtual bool PerformSearch()
        {
            return false;
        }

        public virtual void Popup()
        {
            this.PopupAt(Control.MousePosition);
        }

        public virtual void PopupAt(Point position)
        {
            if (this.Provider != null)
            {
                ICodeCompletionProvider provider = this.Provider;
                ShowingEventArgs e = new ShowingEventArgs(this.Provider);
                provider.OnShowPopup(this, e);
                this.OnShowPopup(e);
                if (!e.NeedShow)
                {
                    return;
                }
            }
            this.showing = true;
            try
            {
                if ((this.completionFlags & CodeCompletionFlags.FeetToScreen) > CodeCompletionFlags.None)
                {
                    this.EnsureVisible(ref position);
                }
                PopupHookManager.PopupClosed(this);
                PopupHookManager.PopupShowing(this);
                this.DoShow(position);
            }
            finally
            {
                this.showing = false;
            }
        }

        public virtual void PopupAt(int x, int y)
        {
            this.PopupAt(new Point(x, y));
        }

        public virtual void PositionChanged(int x, int y, int deltaX, int deltaY)
        {
            if (base.Visible)
            {
                Range.UpdatePos(x, y, deltaX, deltaY, ref this.endPos, true);
            }
        }

        protected override bool ProcessKeyPreview(ref Message m)
        {
            Keys none = Keys.None;
            if ((m.Msg == 0x100) || (m.Msg == 260))
            {
                none = ((Keys) m.WParam.ToInt32()) & Keys.KeyCode;
                switch (none)
                {
                    case Keys.Escape:
                        if ((this.CompletionFlags & CodeCompletionFlags.CloseOnEscape) == CodeCompletionFlags.None)
                        {
                            break;
                        }
                        this.Close(false);
                        return true;

                    case Keys.Space:
                        if ((this.CompletionFlags & CodeCompletionFlags.AcceptOnSpace) != CodeCompletionFlags.None)
                        {
                            this.Close(true);
                            return true;
                        }
                        break;

                    case Keys.Tab:
                        if ((this.CompletionFlags & CodeCompletionFlags.AcceptOnTab) != CodeCompletionFlags.None)
                        {
                            this.Close(true);
                            return true;
                        }
                        break;

                    case Keys.Enter:
                        if ((this.CompletionFlags & CodeCompletionFlags.AcceptOnEnter) == CodeCompletionFlags.None)
                        {
                            break;
                        }
                        this.Close(true);
                        return true;
                }
            }
            if (((m.Msg == 0x102) && (m.WParam.ToInt32() == 0x20)) && ((this.CompletionFlags & CodeCompletionFlags.AcceptOnSpace) != CodeCompletionFlags.None))
            {
                this.Close(true);
                return true;
            }
            this.DoProcessKeyMessage(ref m);
            this.CheckPositionValid();
            return ((((((m.Msg == 0x102) && (m.WParam.ToInt32() != 0x1b)) && (m.WParam.ToInt32() != 13)) || (none == Keys.Back)) && this.PerformSearch()) || base.ProcessKeyPreview(ref m));
        }

        void IControl.add_Click(EventHandler handler1)
        {
            base.Click += handler1;
        }

        void IControl.BringToFront()
        {
            base.BringToFront();
        }

        Graphics IControl.CreateGraphics()
        {
            return base.CreateGraphics();
        }

        Form IControl.FindForm()
        {
            return base.FindForm();
        }

        bool IControl.Focus()
        {
            return base.Focus();
        }

        bool IControl.get_CanFocus()
        {
            return base.CanFocus;
        }

        Rectangle IControl.get_ClientRectangle()
        {
            return base.ClientRectangle;
        }

        bool IControl.get_Created()
        {
            return base.Created;
        }

        bool IControl.get_Enabled()
        {
            return base.Enabled;
        }

        int IControl.get_Height()
        {
            return base.Height;
        }

        bool IControl.get_IsHandleCreated()
        {
            return base.IsHandleCreated;
        }

        int IControl.get_Left()
        {
            return base.Left;
        }

        Point IControl.get_Location()
        {
            return base.Location;
        }

        Control IControl.get_Parent()
        {
            return base.Parent;
        }

        int IControl.get_Top()
        {
            return base.Top;
        }

        bool IControl.get_Visible()
        {
            return base.Visible;
        }

        int IControl.get_Width()
        {
            return base.Width;
        }

        void IControl.Invalidate()
        {
            base.Invalidate();
        }

        void IControl.Invalidate(Rectangle rectangle1)
        {
            base.Invalidate(rectangle1);
        }

        void IControl.Invalidate(Region region1)
        {
            base.Invalidate(region1);
        }

        void IControl.Invalidate(Region region1, bool flag1)
        {
            base.Invalidate(region1, flag1);
        }

        Point IControl.PointToClient(Point point1)
        {
            return base.PointToClient(point1);
        }

        Point IControl.PointToScreen(Point point1)
        {
            return base.PointToScreen(point1);
        }

        void IControl.remove_Click(EventHandler handler1)
        {
            base.Click -= handler1;
        }

        void IControl.set_Bounds(Rectangle rectangle1)
        {
            base.Bounds = rectangle1;
        }

        void IControl.set_Enabled(bool flag1)
        {
            base.Enabled = flag1;
        }

        void IControl.set_Height(int num1)
        {
            base.Height = num1;
        }

        void IControl.set_Left(int num1)
        {
            base.Left = num1;
        }

        void IControl.set_Location(Point point1)
        {
            base.Location = point1;
        }

        void IControl.set_Parent(Control control1)
        {
            base.Parent = control1;
        }

        void IControl.set_Top(int num1)
        {
            base.Top = num1;
        }

        void IControl.set_Visible(bool flag1)
        {
            base.Visible = flag1;
        }

        void IControl.set_Width(int num1)
        {
            base.Width = num1;
        }

        void IControl.Update()
        {
            base.Update();
        }

        void ICodeCompletionWindow.add_HelpRequested(HelpEventHandler handler1)
        {
            base.HelpRequested += handler1;
        }

        void ICodeCompletionWindow.remove_HelpRequested(HelpEventHandler handler1)
        {
            base.HelpRequested -= handler1;
        }

        public virtual void ResetAutoSize()
        {
            this.AutoSize = true;
        }

        public virtual void ResetCodeCompletionFlags()
        {
            this.CompletionFlags = EditConsts.DefaultCodeCompletionFlags;
        }

        public virtual void ResetContent()
        {
        }

        public virtual void ResetSizeable()
        {
            this.Sizeable = false;
        }

        protected virtual void SetProvider(ICodeCompletionProvider provider)
        {
        }

        protected virtual void UpdateAutoSize()
        {
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x401)
            {
                this.Close(m.WParam != IntPtr.Zero, false);
            }
            base.WndProc(ref m);
        }

        public override bool AutoSize
        {
            get
            {
                return base.AutoSize;
            }
            set
            {
                if (base.AutoSize != value)
                {
                    base.AutoSize = value;
                    this.OnAutoSizeChanged();
                }
            }
        }

        public virtual CodeCompletionFlags CompletionFlags
        {
            get
            {
                return this.completionFlags;
            }
            set
            {
                if (this.completionFlags != value)
                {
                    this.completionFlags = value;
                    this.OnCompletionFlagsChanged();
                }
            }
        }

        protected override System.Windows.Forms.CreateParams CreateParams
        {
            get
            {
                uint num = 0x80000000;
                System.Windows.Forms.CreateParams createParams = base.CreateParams;
                createParams.Style |= (int) num;
                return createParams;
            }
        }

        public virtual Point DisplayPos
        {
            get
            {
                return this.displayPos;
            }
            set
            {
                if (this.displayPos != value)
                {
                    this.displayPos = value;
                    this.OnDisplayPosChanged();
                }
            }
        }

        public virtual Point EndPos
        {
            get
            {
                return this.endPos;
            }
            set
            {
                if (this.endPos != value)
                {
                    this.endPos = value;
                    this.OnEndPosChanged();
                }
            }
        }

        public virtual ImageList Images
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

        public virtual Control OwnerControl
        {
            get
            {
                return this.ownerControl;
            }
            set
            {
                if (this.ownerControl != value)
                {
                    this.ownerControl = value;
                    this.OnOwnerControlChanged();
                }
            }
        }

        public virtual Control PopupControl
        {
            get
            {
                return this.popupControl;
            }
        }

        public virtual ICodeCompletionProvider Provider
        {
            get
            {
                return this.provider;
            }
            set
            {
                if (this.provider != value)
                {
                    this.provider = value;
                    this.SetProvider(value);
                }
            }
        }

        public virtual bool Sizeable
        {
            get
            {
                return this.sizeable;
            }
            set
            {
                if (this.sizeable != value)
                {
                    this.sizeable = value;
                    this.OnSizeableChanged();
                }
            }
        }

        public virtual Point StartPos
        {
            get
            {
                return this.startPos;
            }
            set
            {
                if (this.startPos != value)
                {
                    this.startPos = value;
                    this.OnStartPosChanged();
                }
            }
        }

        internal class PopupHookManager
        {
            private static bool inMouseHook = false;
            private static IntPtr mouseHookHandle = IntPtr.Zero;
            private static HookHandler mouseHookProc;
            private static Point mousePos = Control.MousePosition;
            private static IList<CodeCompletionWindow.PopupWnd> Popups = new List<CodeCompletionWindow.PopupWnd>();

            internal static void CheckMouse(Control control, Point mousePos, bool mouseMove)
            {
                for (int i = Popups.Count - 1; i >= 0; i--)
                {
                    CodeCompletionWindow.PopupWnd wnd = Popups[i];
                    ICodeCompletionWindow popup = (CodeCompletionWindow) wnd.Popup;
                    ICodeCompletionWindow window2 = wnd.Popup;
                    if (((window2 != null) && popup.Created) && popup.Visible)
                    {
                        Control control2 = popup.FindForm();
                        if ((((control2 != null) && !control2.Contains(control)) && ((control2 != control) && (popup != control))) && !popup.ContainsControl(control))
                        {
                            if ((mouseMove && (popup.OwnerControl != null)) && ((popup.CompletionFlags & CodeCompletionFlags.CloseOnMouseLeave) != CodeCompletionFlags.None))
                            {
                                Control ownerControl = popup.OwnerControl;
                                Rectangle rectangle = new Rectangle(0, 0, ownerControl.Width, ownerControl.Height);
                                if (!popup.Bounds.Contains(mousePos) && !rectangle.Contains(ownerControl.PointToClient(mousePos)))
                                {
                                    window2.CloseDelayed(false);
                                }
                            }
                            else if (!mouseMove && !popup.Bounds.Contains(mousePos))
                            {
                                window2.CloseDelayed(false);
                            }
                        }
                    }
                }
            }

            internal static void FilterMouseMessage(int msg, IntPtr hWnd)
            {
                Point mousePosition = Control.MousePosition;
                if (OSUtils.IsMouseMsg(msg) || ((msg == 0x200) && IsSignificantMouseMove(mousePosition)))
                {
                    if (msg == 0x200)
                    {
                        mousePos = mousePosition;
                    }
                    CheckMouse(Control.FromHandle(hWnd), mousePosition, msg == 0x200);
                }
            }

            internal static void InstallHook()
            {
                mouseHookProc = new HookHandler(CodeCompletionWindow.PopupHookManager.MouseHook);
                mouseHookHandle = OSUtils.SetMouseHook(mouseHookProc);
                Application.ApplicationExit += new EventHandler(CodeCompletionWindow.PopupHookManager.OnApplicationExit);
            }

            internal static bool IsSignificantMouseMove(Point pt)
            {
                if (Math.Abs((int) (pt.X - mousePos.X)) <= 5)
                {
                    return (Math.Abs((int) (pt.Y - mousePos.Y)) > 5);
                }
                return true;
            }

            private static IntPtr MouseHook(int ncode, IntPtr wParam, IntPtr lParam)
            {
                if (ncode == 0)
                {
                    if (!inMouseHook && (lParam != IntPtr.Zero))
                    {
                        Point point;
                        IntPtr mouseHookHandle = OSUtils.GetMouseHookHandle(lParam, out point);
                        try
                        {
                            inMouseHook = true;
                            FilterMouseMessage(wParam.ToInt32(), mouseHookHandle);
                            goto Label_004A;
                        }
                        finally
                        {
                            inMouseHook = false;
                        }
                    }
                    return OSUtils.CallNextHook(CodeCompletionWindow.PopupHookManager.mouseHookHandle, ncode, wParam, lParam);
                }
            Label_004A:
                return OSUtils.CallNextHook(CodeCompletionWindow.PopupHookManager.mouseHookHandle, ncode, wParam, lParam);
            }

            internal static bool NeedCloseOnLostFocus(ICodeCompletionWindow wnd)
            {
                if ((wnd.CompletionFlags & CodeCompletionFlags.CloseOnLostFocus) == CodeCompletionFlags.None)
                {
                    return false;
                }
                foreach (CodeCompletionWindow.PopupWnd wnd2 in Popups)
                {
                    ICodeCompletionWindow popup = wnd2.Popup;
                    if (((popup != null) && popup.Visible) && popup.IsFocused())
                    {
                        return false;
                    }
                }
                return true;
            }

            internal static void OnApplicationExit(object sender, EventArgs e)
            {
                RemoveHook();
            }

            internal static void PopupClosed(ICodeCompletionWindow popup)
            {
                for (int i = Popups.Count - 1; i >= 0; i--)
                {
                    CodeCompletionWindow.PopupWnd wnd = Popups[i];
                    if (wnd.Popup == popup)
                    {
                        wnd.Release();
                        Popups.RemoveAt(i);
                        break;
                    }
                }
                if (Popups.Count == 0)
                {
                    RemoveHook();
                }
            }

            internal static void PopupShowing(ICodeCompletionWindow popup)
            {
                Popups.Add(new CodeCompletionWindow.PopupWnd(popup, popup.OwnerControl.FindForm()));
                if (mouseHookHandle == IntPtr.Zero)
                {
                    InstallHook();
                }
            }

            internal static void RemoveHook()
            {
                if (mouseHookHandle != IntPtr.Zero)
                {
                    Application.ApplicationExit -= new EventHandler(CodeCompletionWindow.PopupHookManager.OnApplicationExit);
                    OSUtils.ReleaseHook(mouseHookHandle);
                    mouseHookHandle = IntPtr.Zero;
                    mouseHookProc = null;
                }
            }

            internal static int PopupCount
            {
                get
                {
                    return Popups.Count;
                }
            }
        }

        internal class PopupNativeWindow : NativeWindow
        {
            private IList<ICodeCompletionWindow> popups = new List<ICodeCompletionWindow>();

            public PopupNativeWindow(IntPtr handle)
            {
                base.AssignHandle(handle);
            }

            public void Add(ICodeCompletionWindow popup)
            {
                this.popups.Add(popup);
            }

            ~PopupNativeWindow()
            {
                this.popups.Clear();
            }

            public void Remove(ICodeCompletionWindow popup)
            {
                this.popups.Remove(popup);
            }

            protected override void WndProc(ref Message m)
            {
                if ((m.Msg == 0x86) && (m.WParam == IntPtr.Zero))
                {
                    foreach (ICodeCompletionWindow window in this.popups)
                    {
                        if (window.Visible)
                        {
                            m.Result = (IntPtr) 1;
                            return;
                        }
                    }
                }
                base.WndProc(ref m);
            }
        }

        internal class PopupWnd
        {
            private CodeCompletionWindow.PopupNativeWindow mdiNativeWindow;
            private CodeCompletionWindow.PopupNativeWindow nativeWindow;
            private ICodeCompletionWindow popup;
            private Control popupParent;

            public PopupWnd(ICodeCompletionWindow popup, Control popupParent)
            {
                this.popup = popup;
                this.popupParent = popupParent;
                if ((popupParent != null) && popupParent.IsHandleCreated)
                {
                    this.nativeWindow = this.GetNativeWindow(popupParent.Handle);
                    this.nativeWindow.Add(this.Popup);
                }
                if ((popupParent is Form) && ((Form) popupParent).IsMdiChild)
                {
                    Form mdiParent = ((Form) popupParent).MdiParent;
                    if ((mdiParent != null) && mdiParent.IsHandleCreated)
                    {
                        this.mdiNativeWindow = this.GetNativeWindow(mdiParent.Handle);
                        this.mdiNativeWindow.Add(this.Popup);
                    }
                }
            }

            private CodeCompletionWindow.PopupNativeWindow GetNativeWindow(IntPtr handle)
            {
                NativeWindow window = NativeWindow.FromHandle(handle);
                if (window is CodeCompletionWindow.PopupNativeWindow)
                {
                    return (CodeCompletionWindow.PopupNativeWindow) window;
                }
                return new CodeCompletionWindow.PopupNativeWindow(handle);
            }

            public void Release()
            {
                if (this.nativeWindow != null)
                {
                    this.nativeWindow.Remove(this.Popup);
                }
                this.nativeWindow = null;
                if (this.mdiNativeWindow != null)
                {
                    this.mdiNativeWindow.Remove(this.Popup);
                }
                this.mdiNativeWindow = null;
            }

            public ICodeCompletionWindow Popup
            {
                get
                {
                    return this.popup;
                }
            }
        }
    }
}

