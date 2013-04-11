namespace QWhale.Editor.CodeCompletion
{
    using QWhale.Common;
    using QWhale.Editor;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    [DesignTimeVisible(false), ToolboxItem(false)]
    public class CodeCompletionEdit : CodeCompletionWindow, ICodeCompletionEdit, ICodeCompletionWindow, IControl
    {
        private ICodeCompletionBox parent;

        public CodeCompletionEdit(ISyntaxEdit owner, ICodeCompletionBox parent) : base(owner)
        {
            this.parent = parent;
            base.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            base.Width = 300;
            base.Height = this.Font.Height + 2;
            if (parent != null)
            {
                this.CompletionFlags = parent.CompletionFlags;
            }
            this.CompletionFlags &= ~(CodeCompletionFlags.AcceptOnDelimiter | CodeCompletionFlags.AcceptOnDblClick | CodeCompletionFlags.AcceptOnEnter | CodeCompletionFlags.CloseOnEscape);
            this.CompletionFlags |= CodeCompletionFlags.KeepActive;
            this.Edit.EditBox.TextChanged += new EventHandler(this.DoTextChanged);
        }

        public override bool ContainsControl(Control control)
        {
            if (!base.ContainsControl(control))
            {
                if (control == null)
                {
                    return false;
                }
                if (control != this.parent)
                {
                    return (control == this.parent.PopupControl);
                }
            }
            return true;
        }

        protected override Control CreatePopupControl()
        {
            return new CompletionEdit();
        }

        protected override void DoProcessKeyMessage(ref Message m)
        {
        }

        protected virtual void DoTextChanged(object sender, EventArgs e)
        {
            if (this.parent != null)
            {
                this.parent.PerformSearch();
            }
        }

        public override bool IsFocused()
        {
            if ((!this.Focused && !this.Edit.Focused) && !this.Edit.EditBox.Focused)
            {
                return this.Edit.PathLabel.Focused;
            }
            return true;
        }

        protected virtual void OnEditPathChanged()
        {
        }

        protected virtual void OnEditTextChanged()
        {
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
                    case Keys.Enter:
                    case Keys.Escape:
                    case Keys.PageUp:
                    case Keys.Next:
                    case Keys.Up:
                    case Keys.Down:
                        if ((this.parent != null) && this.parent.PopupControl.IsHandleCreated)
                        {
                            OSUtils.SendMessage(this.parent.PopupControl.Handle, m.Msg, m.WParam, m.LParam);
                            return true;
                        }
                        break;
                }
            }
            return base.ProcessKeyPreview(ref m);
        }

        event EventHandler IControl.Click
        {
            add
            {
                base.Click += value;
            }
            remove
            {
                base.Click -= value;
            }
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

        event HelpEventHandler ICodeCompletionWindow.HelpRequested
        {
            add
            {
                base.HelpRequested += value;
            }
            remove
            {
                base.HelpRequested -= value;
            }
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x85)
            {
                base.WndProc(ref m);
                if ((base.FormBorderStyle == FormBorderStyle.FixedToolWindow) && (XPThemes.CurrentTheme != XPThemeName.None))
                {
                    IntPtr windowDC = OSUtils.GetWindowDC(base.Handle);
                    try
                    {
                        OSUtils.ExcludeClipRect(windowDC, 2, 2, base.Width - 2, base.Height - 2);
                        XPThemes.DrawEditBorder(windowDC, new Rectangle(0, 0, base.Width, base.Height));
                    }
                    finally
                    {
                        OSUtils.ReleaseDC(base.Handle, windowDC);
                    }
                }
            }
            else
            {
                base.WndProc(ref m);
            }
        }

        public virtual ICompletionEdit Edit
        {
            get
            {
                return (this.PopupControl as ICompletionEdit);
            }
        }

        public virtual string EditField
        {
            get
            {
                return this.Edit.Label.Text;
            }
            set
            {
                if (this.Edit.Label.Text != value)
                {
                    this.Edit.Label.Text = value;
                    this.Edit.UpdateSize();
                }
            }
        }

        public string EditPath
        {
            get
            {
                return this.Edit.PathLabel.Text;
            }
            set
            {
                if (this.Edit.PathLabel.Text != value)
                {
                    this.Edit.PathLabel.Text = value;
                    this.OnEditPathChanged();
                }
            }
        }

        public string EditText
        {
            get
            {
                return this.Edit.EditBox.Text;
            }
            set
            {
                if (((this.Edit != null) && (this.Edit.EditBox != null)) && (this.Edit.EditBox.Text != value))
                {
                    this.Edit.EditBox.Text = value;
                    this.OnEditTextChanged();
                }
            }
        }
    }
}

