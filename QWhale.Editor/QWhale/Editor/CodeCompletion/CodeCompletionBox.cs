namespace QWhale.Editor.CodeCompletion
{
    using QWhale.Common;
    using QWhale.Editor;
    using QWhale.Syntax.CodeCompletion;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    [DesignTimeVisible(false), ToolboxItem(false)]
    public class CodeCompletionBox : CodeCompletionWindow, ICodeCompletionBox, ICodeCompletionWindow, IControl
    {
        private ICodeCompletionEdit codeEdit;
        private ICodeCompletionHint codeHint;
        private Timer codeHintTimer;
        private int dropDownCount;
        private bool showTabs;
        private bool sorted;
        private const int tabPadding = 6;
        private TabControl tabs;

        public event EventHandler SelectionChanged
        {
            add
            {
                this.ListBox.SelectedIndexChanged += value;
            }
            remove
            {
                this.ListBox.SelectedIndexChanged -= value;
            }
        }

        public CodeCompletionBox(ISyntaxEdit owner) : base(owner)
        {
            this.dropDownCount = EditConsts.DefaultDropDownCount;
            this.sorted = true;
            this.CompletionFlags &= ~CodeCompletionFlags.CloseOnMouseLeave;
            this.Sizeable = true;
            this.UpdateAutoSize(false, true);
            CompletionListBox listBox = this.ListBox;
            listBox.UpdateSize = (EventHandler) Delegate.Combine(listBox.UpdateSize, new EventHandler(this.DoUpdateSize));
            this.ListBox.TabStop = false;
            this.Font = new Font(EditConsts.DefaultCodeCompletionFont, EditConsts.DefaultCodeCompletionFontSize);
        }

        public virtual ICodeCompletionColumn AddColumn()
        {
            return this.ListBox.AddColumn();
        }

        protected virtual void AdjustTabWidth()
        {
            if ((this.tabs != null) && (this.tabs.TabCount > 0))
            {
                this.tabs.ItemSize = new Size((base.ClientRectangle.Width / this.tabs.TabCount) - (6 * this.tabs.TabCount), this.tabs.ItemSize.Height);
            }
        }

        public virtual void ClearColumns()
        {
            this.ListBox.ClearColumns();
        }

        protected override void CodeEditDisposed()
        {
            if (base.Visible)
            {
                this.Close(false);
            }
            this.codeEdit = null;
        }

        protected virtual void CodeEditDisposed(object sender, EventArgs e)
        {
            if (base.Visible)
            {
                this.Close(false);
            }
            this.codeEdit = null;
        }

        protected virtual void CodeEditKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.F1)
            {
                HelpEventArgs hevent = new HelpEventArgs(Point.Empty);
                this.OnHelpRequested(hevent);
            }
        }

        protected virtual void CodeEditKeyPress(object sender, KeyPressEventArgs e)
        {
            if (((e.KeyChar == '\b') && (sender is TextBox)) && (((TextBox) sender).SelectionStart == 0))
            {
                if (this.Provider.GetParent() != null)
                {
                    this.Provider = this.Provider.GetParent();
                    this.ListBox.SelectedIndex = this.Provider.SelIndex;
                }
                e.Handled = true;
            }
        }

        public override bool ContainsControl(Control control)
        {
            if (!base.ContainsControl(control))
            {
                if ((this.codeEdit != null) && (((control == this.codeEdit) || (control == this.codeEdit.Edit)) || ((control == this.codeEdit.Edit.EditBox) || (control == this.codeEdit.Edit.PathLabel))))
                {
                    return true;
                }
                if (this.tabs == null)
                {
                    return false;
                }
                if (control != this.tabs)
                {
                    return (control.Parent == this.tabs);
                }
            }
            return true;
        }

        protected override Control CreatePopupControl()
        {
            System.Windows.Forms.ListBox box = new CompletionListBox();
            box.SelectedIndexChanged += new EventHandler(this.DoSelectionChanged);
            box.MouseWheel += new MouseEventHandler(this.DoMouseWheel);
            box.KeyDown += new KeyEventHandler(this.DoKeyDown);
            return box;
        }

        protected override void DoHide()
        {
            base.DoHide();
            this.HideCodeHint();
            this.HideCodeEdit();
        }

        protected virtual void DoKeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Space) && ((this.CompletionFlags & CodeCompletionFlags.AcceptOnDelimiter) != CodeCompletionFlags.None))
            {
                e.Handled = true;
            }
        }

        protected virtual void DoMouseWheel(object sender, MouseEventArgs e)
        {
            this.HideCodeHint();
        }

        protected override void DoProcessKeyMessage(ref Message m)
        {
            if ((this.codeEdit == null) || !this.codeEdit.Visible)
            {
                base.DoProcessKeyMessage(ref m);
            }
        }

        protected virtual void DoSelectionChanged(object sender, EventArgs e)
        {
            this.DoShowCodeHint();
        }

        protected override void DoShow(Point position)
        {
            this.Filter = string.Empty;
            this.ListBox.Images = ((this.Provider != null) && (this.Provider.Images != null)) ? this.Provider.Images : this.Images;
            if (this.showTabs && (this.tabs != null))
            {
                int index = ((this.Provider.Count == 0) || (this.Provider.SelIndex >= 0)) ? 1 : this.tabs.SelectedIndex;
                if (this.tabs.SelectedIndex != index)
                {
                    this.tabs.SelectedIndex = index;
                }
                else
                {
                    this.OnTabsSelected(index);
                }
            }
            if ((this.Provider != null) && (this.Provider.SelIndex >= 0))
            {
                if (this.ListBox.GetIndex(this.Provider.SelIndex) == this.Provider.SelIndex)
                {
                    if (this.Provider.SelIndex < this.ListBox.Items.Count)
                    {
                        this.ListBox.SelectedIndex = this.Provider.SelIndex;
                    }
                }
                else
                {
                    for (int i = 0; i < this.ListBox.Items.Count; i++)
                    {
                        if (this.ListBox.GetIndex(i) == this.Provider.SelIndex)
                        {
                            this.ListBox.SelectedIndex = i;
                            break;
                        }
                    }
                }
            }
            else
            {
                this.ListBox.SelectedIndex = -1;
            }
            base.showing = true;
            try
            {
                this.ShowCodeEdit(ref position);
                this.DoShowCodeHint();
                base.DoShow(position);
                if ((this.codeEdit != null) && this.codeEdit.Visible)
                {
                    this.codeEdit.Focus();
                }
            }
            finally
            {
                base.showing = false;
            }
        }

        protected virtual void DoShowCodeHint()
        {
            this.HideCodeHint();
            this.CodeHintTimer.Enabled = false;
            this.CodeHintTimer.Enabled = true;
        }

        protected void DoUpdateSize(object sender, EventArgs e)
        {
            this.UpdateAutoSize();
        }

        ~CodeCompletionBox()
        {
            if (this.codeHintTimer != null)
            {
                this.codeHintTimer.Dispose();
            }
        }

        protected override int GetSelectedIndex()
        {
            return this.ListBox.GetIndex();
        }

        protected virtual void HideCodeEdit()
        {
            if (this.codeEdit != null)
            {
                this.codeEdit.Visible = false;
            }
        }

        protected virtual void HideCodeHint()
        {
            if (this.codeHint != null)
            {
                this.codeHint.Visible = false;
            }
        }

        private bool IndexMatches(string text, int index)
        {
            if ((index >= 0) && (index < this.ListBox.Items.Count))
            {
                string strB = this.ListBox.Items[index].ToString();
                strB = strB.Substring(0, Math.Min(strB.Length, text.Length));
                return (string.Compare(text, strB, true) == 0);
            }
            return false;
        }

        private int IndexOfString(string text)
        {
            if (this.IndexMatches(text, this.ListBox.SelectedIndex))
            {
                return this.ListBox.SelectedIndex;
            }
            for (int i = 0; i < this.ListBox.Items.Count; i++)
            {
                if (this.IndexMatches(text, i))
                {
                    return i;
                }
            }
            return -1;
        }

        public virtual ICodeCompletionColumn InsertColumn(int index)
        {
            return this.ListBox.InsertColumn(index);
        }

        public override bool IsFocused()
        {
            if (this.tabs != null)
            {
                if (this.tabs.Focused)
                {
                    return true;
                }
                foreach (TabPage page in this.tabs.TabPages)
                {
                    if (page.Focused)
                    {
                        return true;
                    }
                }
            }
            return base.IsFocused();
        }

        protected virtual void OnDropDownCountChanged()
        {
            this.UpdateAutoSize(false, true);
        }

        protected virtual void OnFilterChanged()
        {
        }

        protected virtual void OnFilteredChanged()
        {
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            this.AdjustTabWidth();
        }

        protected virtual void OnShowCodeHint(object sender, EventArgs e)
        {
            this.ShowCodeHint(this.ListBox.GetIndex());
            this.CodeHintTimer.Enabled = false;
        }

        protected virtual void OnShowTabsChanged()
        {
            if (this.showTabs)
            {
                this.Tabs.TabIndex = 0;
                this.AdjustTabWidth();
                this.OnTabsSelected(0);
            }
            else if (this.tabs != null)
            {
                this.tabs.Visible = false;
            }
        }

        protected virtual void OnTabsSelected(int index)
        {
            int num = (index == 0) ? 0 : -1;
            if (this.ListBox.Priority != num)
            {
                this.ListBox.Priority = num;
            }
            else
            {
                this.ListBox.ResetContent(0);
            }
        }

        protected virtual void PathLabelClick(object sender, EventArgs e)
        {
            if (this.Provider.GetParent() != null)
            {
                this.Provider = this.Provider.GetParent();
                this.ListBox.SelectedIndex = this.Provider.SelIndex;
            }
        }

        public override bool PerformSearch()
        {
            if (this.Provider == null)
            {
                return false;
            }
            string text = null;
            if ((this.codeEdit != null) && this.codeEdit.Visible)
            {
                text = this.codeEdit.EditText;
            }
            else
            {
                ISyntaxEdit syntaxEdit = base.GetSyntaxEdit();
                if (syntaxEdit != null)
                {
                    Point position = syntaxEdit.Position;
                    if ((position.Y == this.StartPos.Y) && (position.X > this.StartPos.X))
                    {
                        text = syntaxEdit.Lines[position.Y];
                        if (this.StartPos.X < text.Length)
                        {
                            text = (position.X < text.Length) ? text.Substring(this.StartPos.X, position.X - this.StartPos.X) : text.Substring(this.StartPos.X);
                        }
                    }
                }
            }
            if (text != null)
            {
                if (this.Filtered)
                {
                    this.Filter = text.Trim();
                }
                else
                {
                    int num = this.IndexOfString(text);
                    if (num >= 0)
                    {
                        this.ListBox.SelectedIndex = num;
                    }
                    else
                    {
                        this.ListBox.SelectedIndex = -1;
                    }
                }
            }
            else if (this.Filtered)
            {
                this.Filter = string.Empty;
            }
            else
            {
                this.ListBox.SelectedIndex = -1;
            }
            return (text != null);
        }

        protected override bool ProcessKeyPreview(ref Message m)
        {
            if ((m.Msg == 0x100) || (m.Msg == 260))
            {
                Keys keys = ((Keys) m.WParam.ToInt32()) & Keys.KeyCode;
                if ((((keys == Keys.Tab) && ((this.CompletionFlags & CodeCompletionFlags.AcceptOnTab) != CodeCompletionFlags.None)) || ((keys == Keys.Space) && ((this.CompletionFlags & CodeCompletionFlags.AcceptOnSpace) != CodeCompletionFlags.None))) && ((Control.ModifierKeys == Keys.None) || (Control.ModifierKeys == Keys.Shift)))
                {
                    this.Close(true);
                    return true;
                }
                if (Array.IndexOf<Keys>(this.ListBox.NavKeys, keys) >= 0)
                {
                    return false;
                }
            }
            if (this.OnProcessKeyPreview(ref m))
            {
                return true;
            }
            ISyntaxEdit syntaxEdit = base.GetSyntaxEdit();
            if (((m.Msg == 0x102) && (syntaxEdit != null)) && ((Control.ModifierKeys == Keys.None) || (Control.ModifierKeys == Keys.Shift)))
            {
                char ch = (char) m.WParam.ToInt32();
                if ((((this.CompletionFlags & CodeCompletionFlags.AcceptOnDelimiter) != CodeCompletionFlags.None) && (ch >= ' ')) && syntaxEdit.Lines.IsDelimiter(ch))
                {
                    this.Close(true);
                }
                if (((this.codeEdit != null) && this.codeEdit.Visible) && !this.codeEdit.Edit.EditBox.Focused)
                {
                    OSUtils.SendMessage(this.codeEdit.Edit.EditBox.Handle, m.Msg, m.WParam, m.LParam);
                }
            }
            return base.ProcessKeyPreview(ref m);
        }

        void IControl.add_Click(EventHandler handler1)
        {
            base.Click += handler1;
        }

        bool IControl.CanFocus
        {
            get { return base.CanFocus; }
        }

        Rectangle IControl.ClientRectangle
        {
            get { return base.ClientRectangle; }
        }

        bool IControl.Created
        {
            get { return base.Created; }
        }

        bool IControl.Enabled
        {
            get { return base.Enabled; }
            set { base.Enabled = value; }
        }

        int IControl.Height
        {
            get { return base.Height; }
            set { base.Height = value; }
        }

        bool IControl.IsHandleCreated
        {
            get { return base.IsHandleCreated; }
        }

        int IControl.Left
        {
            get { return base.Left; }
            set { base.Left = value; }
        }

        Point IControl.Location
        {
            get { return base.Location; }
            set { base.Location = value; }
        }

        Control IControl.Parent
        {
            get { return base.Parent; }
            set { base.Parent = value; }
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

        void IControl.remove_Click(EventHandler handler1)
        {
            base.Click -= handler1;
        }

        void IControl.set_Bounds(Rectangle rectangle1)
        {
            base.Bounds = rectangle1;
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

        void ICodeCompletionWindow.add_HelpRequested(HelpEventHandler handler1)
        {
            base.HelpRequested += handler1;
        }

        void ICodeCompletionWindow.remove_HelpRequested(HelpEventHandler handler1)
        {
            base.HelpRequested -= handler1;
        }

        public virtual void RemoveColumnAt(int index)
        {
            this.ListBox.RemoveColumnAt(index);
        }

        public override void ResetContent()
        {
            this.ListBox.ResetContent();
        }

        public virtual void ResetDropDownCount()
        {
            this.DropDownCount = EditConsts.DefaultDropDownCount;
        }

        public override void ResetSizeable()
        {
            this.Sizeable = true;
        }

        protected override void SetProvider(ICodeCompletionProvider provider)
        {
            this.ListBox.Sorted = (this.sorted && (provider != null)) && (provider.Count > 1);
            this.ListBox.Provider = provider;
            this.ListBox.Images = ((provider != null) && (provider.Images != null)) ? provider.Images : this.Images;
            this.HideCodeHint();
            Point location = base.Location;
            if (this.UpdateCodeEdit(ref location))
            {
                base.Location = location;
            }
        }

        protected virtual void ShowCodeEdit(ref Point position)
        {
            if (((this.Provider != null) && (this.Provider.EditField != null)) && (this.Provider.EditField != string.Empty))
            {
                ICodeCompletionEdit codeEdit = this.CodeEdit;
                codeEdit.PopupAt(new Point(position.X, position.Y - codeEdit.Height));
                this.UpdateCodeEdit(ref position);
            }
        }

        protected virtual void ShowCodeHint(int index)
        {
            if (((base.Visible && (index >= 0)) && ((this.Provider != null) && this.Provider.ShowDescriptions)) && (index < this.Provider.Count))
            {
                string str = this.Provider.Descriptions[index];
                if (str != string.Empty)
                {
                    ICodeCompletionHint codeHint = this.CodeHint;
                    ICodeCompletionProvider provider = codeHint.Provider;
                    provider.UseHtmlFormatting = this.Provider.UseHtmlFormatting;
                    codeHint.Provider = null;
                    ((IQuickInfo) provider).Text = str;
                    codeHint.Provider = provider;
                    codeHint.ResetContent();
                    Point position = base.PointToScreen(new Point(base.Width, (this.ListBox.SelectedIndex - this.ListBox.TopIndex) * this.ListBox.ItemHeight));
                    codeHint.EnsureVisible(ref position);
                    Rectangle rectangle = new Rectangle(position.X, position.Y, codeHint.Width, codeHint.Height);
                    rectangle.Intersect(base.Bounds);
                    if (!rectangle.IsEmpty)
                    {
                        position.X = base.Left - codeHint.Width;
                    }
                    codeHint.PopupAt(position);
                }
            }
            else
            {
                this.HideCodeHint();
            }
        }

        protected virtual void TabEnter(object sender, EventArgs e)
        {
            if (this.PopupControl.CanFocus)
            {
                this.PopupControl.Focus();
            }
        }

        protected virtual void TabsSelected(object sender, TabControlEventArgs e)
        {
            this.OnTabsSelected(e.TabPageIndex);
        }

        protected override void UpdateAutoSize()
        {
            this.UpdateAutoSize(true, true);
        }

        protected virtual void UpdateAutoSize(bool horz, bool vert)
        {
            if (this.AutoSize)
            {
                int width;
                int num2;
                if (vert)
                {
                    width = (this.ListBox.ItemHeight * Math.Min(this.ListBox.Items.Count, (this.dropDownCount == 0) ? EditConsts.MaxDropDownCount : this.dropDownCount)) + EditConsts.DefaultRowSeparator;
                    if (this.showTabs && (this.tabs != null))
                    {
                        width += this.tabs.Height;
                    }
                    width = Math.Max(width, EditConsts.DefaultMinListBoxHeight);
                }
                else
                {
                    width = base.ClientRectangle.Width;
                }
                if (horz)
                {
                    num2 = this.ListBox.ItemWidth + (EditConsts.DefaultColumnSeparator * 4);
                    if (this.ListBox.Images != null)
                    {
                        num2 += this.ListBox.Images.ImageSize.Width + EditConsts.DefaultColumnSeparator;
                    }
                    num2 += OSUtils.GetScrollSize(false);
                    num2 = Math.Max(num2, EditConsts.DefaultMinListBoxWidth);
                }
                else
                {
                    num2 = base.ClientRectangle.Width;
                }
                base.ClientSize = new Size(num2, width);
            }
        }

        protected virtual bool UpdateCodeEdit(ref Point position)
        {
            if (((this.Provider != null) && (this.Provider.EditField != null)) && (this.Provider.EditField != string.Empty))
            {
                ICodeCompletionEdit codeEdit = this.CodeEdit;
                codeEdit.Provider = this.Provider;
                codeEdit.EditField = this.Provider.EditField;
                codeEdit.EditPath = this.Provider.EditPath;
                codeEdit.EditText = string.Empty;
                position.X = codeEdit.Location.X + codeEdit.Edit.EditBox.Left;
                return true;
            }
            this.HideCodeEdit();
            return false;
        }

        protected virtual ICodeCompletionEdit CodeEdit
        {
            get
            {
                if (this.codeEdit == null)
                {
                    this.codeEdit = new CodeCompletionEdit(base.GetSyntaxEdit(), this);
                    this.codeEdit.Edit.EditBox.KeyPress += new KeyPressEventHandler(this.CodeEditKeyPress);
                    this.codeEdit.Edit.EditBox.KeyDown += new KeyEventHandler(this.CodeEditKeyDown);
                    this.codeEdit.Edit.EditBox.LostFocus += new EventHandler(this.DoLostFocus);
                    this.codeEdit.Edit.PathLabel.Click += new EventHandler(this.PathLabelClick);
                    this.codeEdit.Disposed += new EventHandler(this.CodeEditDisposed);
                }
                return this.codeEdit;
            }
        }

        protected virtual ICodeCompletionHint CodeHint
        {
            get
            {
                if (this.codeHint == null)
                {
                    this.codeHint = new CodeCompletionHint(base.GetSyntaxEdit());
                    this.codeHint.Enabled = false;
                    this.codeHint.Provider = new QuickInfo();
                    this.codeHint.CompletionFlags &= ~CodeCompletionFlags.CloseOnMouseLeave;
                }
                return this.codeHint;
            }
        }

        protected Timer CodeHintTimer
        {
            get
            {
                if (this.codeHintTimer == null)
                {
                    this.codeHintTimer = new Timer();
                    this.codeHintTimer.Enabled = false;
                    this.codeHintTimer.Interval = EditConsts.DefaultHintDelay;
                    this.codeHintTimer.Tick += new EventHandler(this.OnShowCodeHint);
                }
                return this.codeHintTimer;
            }
        }

        public virtual ICodeCompletionColumn[] Columns
        {
            get
            {
                ICodeCompletionColumn[] array = new ICodeCompletionColumn[this.ListBox.ColumnCount];
                this.ListBox.Columns.CopyTo(array, 0);
                return array;
            }
        }

        public virtual int DropDownCount
        {
            get
            {
                return this.dropDownCount;
            }
            set
            {
                if (this.dropDownCount != value)
                {
                    this.dropDownCount = value;
                    this.OnDropDownCountChanged();
                }
            }
        }

        public virtual string Filter
        {
            get
            {
                return this.ListBox.Filter;
            }
            set
            {
                if ((this.ListBox != null) && (this.ListBox.Filter != value))
                {
                    this.ListBox.Filter = value;
                    this.OnFilterChanged();
                }
            }
        }

        public virtual bool Filtered
        {
            get
            {
                return this.ListBox.Filtered;
            }
            set
            {
                if ((this.ListBox != null) && (this.ListBox.Filtered != value))
                {
                    this.ListBox.Filtered = value;
                    this.OnFilteredChanged();
                }
            }
        }

        protected CompletionListBox ListBox
        {
            get
            {
                return (CompletionListBox) this.PopupControl;
            }
        }

        public bool ShowTabs
        {
            get
            {
                return this.showTabs;
            }
            set
            {
                if (this.showTabs != value)
                {
                    this.showTabs = value;
                    this.OnShowTabsChanged();
                }
            }
        }

        public virtual bool Sorted
        {
            get
            {
                return this.sorted;
            }
            set
            {
                if ((this.sorted != value) || (this.ListBox.Sorted != value))
                {
                    this.sorted = value;
                    this.ListBox.Sorted = value;
                }
            }
        }

        protected virtual TabControl Tabs
        {
            get
            {
                if (this.tabs == null)
                {
                    this.tabs = new TabControl();
                    this.tabs.Dock = DockStyle.Bottom;
                    this.tabs.TabPages.Add(StringConsts.TabCommonText);
                    this.tabs.TabPages.Add(StringConsts.TabAllText);
                    this.tabs.Selected += new TabControlEventHandler(this.TabsSelected);
                    this.tabs.LostFocus += new EventHandler(this.DoLostFocus);
                    foreach (TabPage page in this.tabs.TabPages)
                    {
                        page.Enter += new EventHandler(this.TabEnter);
                    }
                    this.tabs.TabPages[0].Enter += new EventHandler(this.TabEnter);
                    this.tabs.TabPages[1].Enter += new EventHandler(this.TabEnter);
                    this.tabs.SizeMode = TabSizeMode.Fixed;
                    this.tabs.Appearance = TabAppearance.FlatButtons;
                    this.tabs.TabStop = false;
                    this.tabs.Parent = this;
                    this.tabs.Height = this.tabs.DisplayRectangle.Top;
                    this.tabs.SendToBack();
                }
                return this.tabs;
            }
        }
    }
}

