namespace QWhale.Editor.CodeCompletion
{
    using QWhale.Common;
    using QWhale.Editor;
    using QWhale.Syntax;
    using QWhale.Syntax.CodeCompletion;
    using QWhale.Syntax.Lexer;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    [DesignTimeVisible(false), ToolboxItem(false)]
    public class CodeCompletionHint : CodeCompletionWindow, ICodeCompletionHint, ICodeCompletionWindow, IControl, ISyntaxPaint
    {
        private bool autoHide;
        private Point curPos;
        private Timer hideTimer;

        public event CustomDrawEvent CustomDraw
        {
            add
            {
                this.Hint.SyntaxPaint.CustomDraw += value;
            }
            remove
            {
                this.Hint.SyntaxPaint.CustomDraw -= value;
            }
        }

        public CodeCompletionHint(ISyntaxEdit owner) : base(owner)
        {
            this.Sizeable = false;
            base.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            this.curPos = new Point(0, 0);
            base.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.StandardDoubleClick | ControlStyles.StandardClick | ControlStyles.Opaque | ControlStyles.UserPaint, true);
            CompletionHint hint = this.Hint;
            hint.UpdateSize = (EventHandler) Delegate.Combine(hint.UpdateSize, new EventHandler(this.DoUpdateSize));
            this.Hint.MouseLeave += new EventHandler(this.CheckMouseLeave);
            this.CompletionFlags &= ~(CodeCompletionFlags.AcceptOnDelimiter | CodeCompletionFlags.AcceptOnDblClick | CodeCompletionFlags.AcceptOnEnter);
        }

        protected override void CheckPositionValid()
        {
        }

        protected override Control CreatePopupControl()
        {
            CompletionHint hint = new CompletionHint();
            hint.MouseWheel += new MouseEventHandler(this.DoMouseWheel);
            return hint;
        }

        protected override void DoHide()
        {
            base.DoHide();
            if (this.hideTimer != null)
            {
                this.hideTimer.Enabled = false;
            }
        }

        protected virtual void DoMouseWheel(object sender, MouseEventArgs e)
        {
            ISyntaxEdit syntaxEdit = base.GetSyntaxEdit();
            if (syntaxEdit != null)
            {
                syntaxEdit.Scrolling.MouseScroll(e.Delta);
                if ((this.DisplayPos.Y >= 0) && (this.DisplayPos.X >= 0))
                {
                    Point p = syntaxEdit.TextToScreen(this.DisplayPos);
                    p.Y = Math.Min(Math.Max((p.Y + syntaxEdit.Painter.FontHeight) + EditConsts.DefaultHintOffsetY, syntaxEdit.ClientRect.Top), syntaxEdit.ClientRect.Bottom - base.Height);
                    base.Top = syntaxEdit.PointToScreen(p).Y;
                }
            }
        }

        protected override void DoShow(Point position)
        {
            base.DoShow(position);
            if ((this.Provider != null) && (this.Provider.SelIndex >= 0))
            {
                this.Hint.SelectedIndex = this.Provider.SelIndex;
            }
            this.Hint.UpdateHint();
            base.Update();
            if (this.autoHide)
            {
                this.HideTimer.Enabled = true;
            }
        }

        protected void DoUpdateSize(object sender, EventArgs e)
        {
            base.Size = this.Hint.HintSize;
        }

        public virtual void DrawLine(int index, Point position, Rectangle clipRect)
        {
            this.Hint.SyntaxPaint.DrawLine(index, position, clipRect);
        }

        public virtual void DrawLine(int index, string line, short[] colorData, Point position, Rectangle clipRect)
        {
            this.Hint.SyntaxPaint.DrawLine(index, line, colorData, position, clipRect);
        }

        public virtual bool EqualStyles(int style1, int style2, bool useColors)
        {
            return this.Hint.SyntaxPaint.EqualStyles(style1, style2, useColors);
        }

        ~CodeCompletionHint()
        {
            if (this.hideTimer != null)
            {
                this.hideTimer.Dispose();
            }
        }

        public virtual Color GetFontColor(Color color, TextStyle textStyle)
        {
            return this.Hint.SyntaxPaint.GetFontColor(color, textStyle);
        }

        public virtual FontStyle GetFontStyle(FontStyle fontStyle, TextStyle textStyle)
        {
            return this.Hint.SyntaxPaint.GetFontStyle(fontStyle, textStyle);
        }

        public virtual ILexStyle GetLexStyle(int style, ref TextStyle textStyle)
        {
            return this.Hint.SyntaxPaint.GetLexStyle(style, ref textStyle);
        }

        protected override int GetSelectedIndex()
        {
            return this.Hint.SelectedIndex;
        }

        public virtual int MeasureLine(int index, int pos, int len)
        {
            return this.Hint.SyntaxPaint.MeasureLine(index, pos, len);
        }

        public virtual int MeasureLine(string line, short[] colorData, int pos, int len)
        {
            return this.Hint.SyntaxPaint.MeasureLine(line, colorData, pos, len);
        }

        public virtual int MeasureLine(int index, int pos, int len, int width, out int chars, bool exact)
        {
            return this.Hint.SyntaxPaint.MeasureLine(index, pos, len, width, out chars, exact);
        }

        public virtual int MeasureLine(string line, short[] colorData, int pos, int len, int width, out int chars, bool exact)
        {
            return this.Hint.SyntaxPaint.MeasureLine(line, colorData, pos, len, width, out chars, exact);
        }

        protected virtual void OnAutoHideChanged()
        {
            if (this.autoHide && base.Visible)
            {
                this.HideTimer.Enabled = true;
            }
        }

        public virtual bool OnCustomDraw(IPainter painter, Rectangle rect, DrawStage stage, DrawState state, IDrawInfo info)
        {
            return this.Hint.SyntaxPaint.OnCustomDraw(painter, rect, stage, state, info);
        }

        protected virtual void OnDisableColorPaintChanged()
        {
        }

        protected virtual void OnDisableSyntaxPaintChanged()
        {
        }

        protected void OnHideHint(object sender, EventArgs e)
        {
            this.Close(false);
        }

        protected virtual void OnIntervalChanged()
        {
        }

        protected virtual void OnLexerChanged()
        {
        }

        public virtual void PaintSyntax(IPainter painter, int startLine, int endLine, Point position, Rectangle rect, bool specialPaint)
        {
            this.Hint.SyntaxPaint.PaintSyntax(painter, startLine, endLine, position, rect, specialPaint);
        }

        protected void PositionChanged()
        {
            ISyntaxEdit syntaxEdit = base.GetSyntaxEdit();
            if (((syntaxEdit != null) && (this.StartPos.X >= 0)) && ((this.StartPos.Y >= 0) && !this.curPos.Equals(syntaxEdit.Position)))
            {
                this.curPos = syntaxEdit.Position;
                if (this.Provider is IParameterInfo)
                {
                    CodeCompletionArgs e = new CodeCompletionArgs {
                        CompletionType = CodeCompletionType.ParameterInfo,
                        NeedReparse = true
                    };
                    syntaxEdit.CodeCompletion(e);
                    if (!e.NeedShow)
                    {
                        e.Provider = null;
                    }
                    Point startPosition = e.StartPosition;
                    if (this.StartPos.Equals(startPosition))
                    {
                        if (!this.UpdateProviderContent(this.Provider, e.Provider))
                        {
                            this.Provider = e.Provider;
                        }
                    }
                    else if ((this.StartPos.Y > startPosition.Y) || ((this.StartPos.Y == startPosition.Y) && (this.StartPos.X > startPosition.X)))
                    {
                        this.Provider = e.Provider;
                        this.StartPos = startPosition;
                        this.DisplayPos = startPosition;
                        this.EndPos = e.EndPosition;
                    }
                    else
                    {
                        this.UpdateProviderParamIndex(this.Provider);
                    }
                }
                if ((this.Provider == null) || (this.Provider.Count == 0))
                {
                    this.Close(false);
                }
                else
                {
                    this.Hint.UpdateHint();
                }
            }
        }

        protected override bool ProcessKeyPreview(ref Message m)
        {
            if (this.OnProcessKeyPreview(ref m))
            {
                return true;
            }
            if ((m.Msg == 0x100) || (m.Msg == 260))
            {
                switch ((((Keys) m.WParam.ToInt32()) & Keys.KeyCode))
                {
                    case Keys.PageUp:
                    case Keys.Next:
                    case Keys.End:
                    case Keys.Home:
                        this.Close(false);
                        break;

                    case Keys.Up:
                        if (!this.Hint.NeedArrows)
                        {
                            this.Close(false);
                            break;
                        }
                        this.Hint.ChangeSelection(false);
                        return true;

                    case Keys.Down:
                        if (!this.Hint.NeedArrows)
                        {
                            this.Close(false);
                            break;
                        }
                        this.Hint.ChangeSelection(true);
                        return true;
                }
            }
            if (m.Msg == 0x102)
            {
                char ch = (char) m.WParam.ToInt32();
                ISyntaxEdit syntaxEdit = base.GetSyntaxEdit();
                if (((syntaxEdit != null) && (syntaxEdit.Lexer is ISyntaxParser)) && (Array.IndexOf<char>(((ISyntaxParser) syntaxEdit.Lexer).CodeCompletionStopChars, ch) >= 0))
                {
                    this.Close(false);
                }
            }
            bool flag = base.ProcessKeyPreview(ref m);
            this.PositionChanged();
            base.CheckPositionValid();
            return flag;
        }

        void IControl.add_Click(EventHandler handler1)
        {
            base.Click += handler1;
        }

        bool IControl.CanFocus
        {
            get
            {
                return base.CanFocus;
            }
        }

        Rectangle IControl.ClientRectangle
        {
            get
            {
                return base.ClientRectangle;
            }
        }

        bool IControl.Created
        {
            get
            {
                return base.Created;
            }
        }

        bool IControl.Enabled
        {
            get
            {
                return base.Enabled;
            }
            set
            {
                base.Enabled = value;
            }
        }

        int IControl.Height
        {
            get
            {
                return base.Height;
            }
            set
            {
                base.Height = value;
            }
        }

        bool IControl.IsHandleCreated
        {
            get 
            {
                return base.IsHandleCreated;
            }
        }
        

        int IControl.Left
        {
            get
            {
                return base.Left;
            }
            set
            {
                base.Left = value;
            }

        }

       

        Point IControl.Location
        {
            get
            {
                return base.Location;
            }
            set
            {
                base.Location = value;
            }
        }

        Control IControl.Parent
        {
            get
            {
                return base.Parent;
            }
            set
            { 
                base.Parent = value; 
            }

        }

        int IControl.Top
        {
            get
            {
                return base.Top;
            }
            set
            {
                base.Top = value;
            }
        }

        bool IControl.Visible
        {
            get
            {
                return base.Visible;
            }
            set
            {
                base.Visible = value;
            }

        }

        int IControl.Width
        {
            get
            {
                return base.Width;
            }
            set
            {
                base.Width = value;
            }
        }

        void IControl.remove_Click(EventHandler handler1)
        {
            base.Click -= handler1;
        }

        Rectangle IControl.Bounds
        {
            get
            {
                return base.Bounds;
            }
            set
            {
                base.Bounds = value;
            }
        }


        void ICodeCompletionWindow.add_HelpRequested(HelpEventHandler handler1)
        {
            base.HelpRequested += handler1;
        }

        void ICodeCompletionWindow.remove_HelpRequested(HelpEventHandler handler1)
        {
            base.HelpRequested -= handler1;
        }

        public virtual void ResetAutoHide()
        {
            this.AutoHide = false;
        }

        public virtual void ResetAutoHidePause()
        {
            this.AutoHidePause = EditConsts.DefaultHideHintDelay;
        }

        public override void ResetContent()
        {
            this.Hint.ResetContent();
        }

        public virtual void ResetDisableColorPaint()
        {
            this.Hint.SyntaxPaint.ResetDisableColorPaint();
        }

        public virtual void ResetDisableSyntaxPaint()
        {
            this.Hint.SyntaxPaint.ResetDisableSyntaxPaint();
        }

        protected override void SetProvider(ICodeCompletionProvider provider)
        {
            this.Hint.Provider = provider;
        }

        protected bool UpdateProviderContent(ICodeCompletionProvider pr1, ICodeCompletionProvider pr2)
        {
            bool flag = (pr1 is IListMembers) && (pr2 is IListMembers);
            if (flag)
            {
                flag = pr1.Count == pr2.Count;
                if (flag)
                {
                    for (int i = 0; i < pr1.Count; i++)
                    {
                        IListMember member = (IListMember) pr1[i];
                        IListMember member2 = (IListMember) pr2[i];
                        flag = (member.Name == member2.Name) && (member.GetParamText(false) == member2.GetParamText(false));
                        if (!flag)
                        {
                            break;
                        }
                    }
                }
            }
            if (flag)
            {
                for (int j = 0; j < pr1.Count; j++)
                {
                    IListMember member3 = (IListMember) pr1[j];
                    IListMember member4 = (IListMember) pr2[j];
                    member3.CurrentParamIndex = member4.CurrentParamIndex;
                }
            }
            return flag;
        }

        protected void UpdateProviderParamIndex(ICodeCompletionProvider provider)
        {
            foreach (IListMember member in provider)
            {
                member.CurrentParamIndex = -1;
            }
        }

        public virtual bool AutoHide
        {
            get
            {
                return this.autoHide;
            }
            set
            {
                if (this.autoHide != value)
                {
                    this.autoHide = value;
                    this.OnAutoHideChanged();
                }
            }
        }

        public virtual int AutoHidePause
        {
            get
            {
                if (this.hideTimer == null)
                {
                    return EditConsts.DefaultHideHintDelay;
                }
                return this.hideTimer.Interval;
            }
            set
            {
                if ((this.HideTimer != null) && (this.HideTimer.Interval != value))
                {
                    this.HideTimer.Interval = value;
                    this.OnIntervalChanged();
                }
            }
        }

        protected override System.Windows.Forms.CreateParams CreateParams
        {
            get
            {
                OSUtils.InitCommonControls();
                System.Windows.Forms.CreateParams createParams = base.CreateParams;
                createParams.Parent = IntPtr.Zero;
                createParams.ClassName = "tooltips_class32";
                createParams.Style |= 1;
                createParams.ExStyle = 0;
                return createParams;
            }
        }

        public virtual bool DisableColorPaint
        {
            get
            {
                return this.Hint.SyntaxPaint.DisableColorPaint;
            }
            set
            {
                if (((this.Hint != null) && (this.Hint.SyntaxPaint != null)) && (this.Hint.SyntaxPaint.DisableColorPaint != value))
                {
                    this.Hint.SyntaxPaint.DisableColorPaint = value;
                    this.OnDisableColorPaintChanged();
                }
            }
        }

        public virtual bool DisableSyntaxPaint
        {
            get
            {
                return this.Hint.SyntaxPaint.DisableSyntaxPaint;
            }
            set
            {
                if (((this.Hint != null) && (this.Hint.SyntaxPaint != null)) && (this.Hint.SyntaxPaint.DisableSyntaxPaint != value))
                {
                    this.Hint.SyntaxPaint.DisableSyntaxPaint = value;
                    this.OnDisableSyntaxPaintChanged();
                }
            }
        }

        protected Timer HideTimer
        {
            get
            {
                if (this.hideTimer == null)
                {
                    this.hideTimer = new Timer();
                    this.hideTimer.Enabled = false;
                    this.hideTimer.Interval = EditConsts.DefaultHideHintDelay;
                    this.hideTimer.Tick += new EventHandler(this.OnHideHint);
                }
                return this.hideTimer;
            }
        }

        protected CompletionHint Hint
        {
            get
            {
                return (CompletionHint) this.PopupControl;
            }
        }

        public virtual ILexer Lexer
        {
            get
            {
                return this.Hint.SyntaxPaint.Lexer;
            }
            set
            {
                if (((this.Hint != null) && (this.Hint.SyntaxPaint != null)) && (this.Hint.SyntaxPaint.Lexer != value))
                {
                    this.Hint.SyntaxPaint.Lexer = value;
                    this.OnLexerChanged();
                }
            }
        }
    }
}

