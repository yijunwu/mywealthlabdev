namespace QWhale.Editor.Dialogs
{
    using QWhale.Common;
    using QWhale.Editor;
    using QWhale.Editor.TextSource;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing;
    using System.Text.RegularExpressions;
    using System.Windows.Forms;

    public class DlgSearch : Form
    {
        public Button btClose;
        public Button btFindNext;
        public Button btFindOptions;
        public Button btMarkAll;
        public Button btPopup;
        public Button btReplace;
        public Button btReplaceAll;
        public Button btReplacePopup;
        public ComboBox cbFindWhat;
        private ComboBox cbLookIn;
        public ComboBox cbReplaceWith;
        public CheckBox chbMatchCase;
        public CheckBox chbMatchWholeWord;
        public CheckBox chbPromptOnReplace;
        public CheckBox chbSearchHiddenText;
        public CheckBox chbSearchUp;
        public CheckBox chbUseRegularExpressions;
        private bool clearBookmarks;
        public ContextMenu cmFind;
        private ContextMenu cmReplace;
        private IContainer components;
        private const int defaultButtonsHeigth = 0x24;
        private const int defaultButtonsReplaceHeigth = 0x41;
        private const int defaultOptionsClosedHeight = 12;
        private const int defaultOptionsHeight = 0x8e;
        private const int defaultOptionsReplaceTop = 0x7e;
        private const int defaultOptionsTop = 80;
        private bool findTextAtCursor;
        private bool firstSearch = true;
        private GroupBox gbFindOptions;
        private ImageList imSearch;
        private bool isOptionsVisible;
        private bool isReplace;
        private Label laFindOptions;
        public Label laFindWhat;
        public Label laLookIn;
        public Label laReplaceWith;
        public MenuItem menuItem10;
        public MenuItem menuItem4;
        public MenuItem miBeginLine;
        public MenuItem miEndLine;
        public MenuItem miEscape;
        private MenuItem miFindWhatText;
        private MenuItem miLineBreak;
        public MenuItem miOneCharInSet;
        public MenuItem miOneCharNotInSet;
        public MenuItem miOneOrMore;
        public MenuItem miOr;
        public MenuItem miSingleChar;
        public MenuItem miTag;
        private MenuItem miTaggedExpression1;
        private MenuItem miTaggedExpression2;
        private MenuItem miTaggedExpression3;
        private MenuItem miTaggedExpression4;
        private MenuItem miTaggedExpression5;
        private MenuItem miTaggedExpression6;
        private MenuItem miTaggedExpression7;
        private MenuItem miTaggedExpression8;
        private MenuItem miTaggedExpression9;
        public MenuItem miZeroOrMore;
        private SearchOptions options;
        private Panel pnButtons;
        private Panel pnFindOptions;
        private SearchOptions saveOptions;
        private string saveText = string.Empty;
        private ISearch search;
        private bool selectionEnabled;
        private TabControl tbSearch;
        private TabPage tbUseFind;
        private TabPage tbUseReplace;

        public DlgSearch()
        {
            this.InitializeComponent();
        }

        private void AddToHistory()
        {
            if (this.cbFindWhat.Text != string.Empty)
            {
                this.AddToHistory(this.cbFindWhat.Items, this.cbFindWhat.Text);
                this.cbFindWhat.SelectedIndex = 0;
            }
            if (this.isReplace && (this.cbReplaceWith.Text != string.Empty))
            {
                this.AddToHistory(this.cbReplaceWith.Items, this.cbReplaceWith.Text);
                this.cbReplaceWith.SelectedIndex = 0;
            }
        }

        private void AddToHistory(IList List, string s)
        {
            if (s != string.Empty)
            {
                for (int i = List.IndexOf(s); i >= 0; i = List.IndexOf(s))
                {
                    List.RemoveAt(i);
                }
                List.Insert(0, s);
            }
        }

        private void btClose_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void btFindNext_Click(object sender, EventArgs e)
        {
            this.AddToHistory();
            if (this.search != null)
            {
                bool flag;
                SearchOptions options = this.Options;
                if ((!this.search.FirstSearch && !this.firstSearch) && ((this.cbFindWhat.Text == this.saveText) && (options == this.saveOptions)))
                {
                    if ((options & SearchOptions.BackwardSearch) != SearchOptions.None)
                    {
                        flag = this.search.FindPrevious();
                    }
                    else
                    {
                        flag = this.search.FindNext();
                    }
                }
                else
                {
                    flag = this.search.Find(this.cbFindWhat.Text, options, this.GetExpression());
                }
                if (flag)
                {
                    this.TextFound();
                }
                else
                {
                    this.ShowNotFound();
                }
            }
        }

        private void btFindOptions_Click(object sender, EventArgs e)
        {
            this.isOptionsVisible = !this.isOptionsVisible;
            this.ResizeControls();
        }

        private void btMarkAll_Click(object sender, EventArgs e)
        {
            if (this.search != null)
            {
                this.search.MarkAll(this.cbFindWhat.Text, this.Options, this.GetExpression(), this.clearBookmarks);
            }
        }

        private void btPopup_Click(object sender, EventArgs e)
        {
            this.cmFind.Show((Control) sender, new Point(0, 0));
        }

        private void btReplace_Click(object sender, EventArgs e)
        {
            if (!this.isReplace)
            {
                this.IsReplace = true;
            }
            else
            {
                this.AddToHistory();
                if (this.search != null)
                {
                    Match match = null;
                    SearchOptions options = this.Options;
                    if ((!this.search.FirstSearch && (this.cbFindWhat.Text == this.saveText)) && this.search.NeedReplaceCurrent(out match))
                    {
                        if (this.search.ReplaceCurrent(this.cbReplaceWith.Text, options, match))
                        {
                            if ((this.Options & SearchOptions.BackwardSearch) != SearchOptions.None)
                            {
                                this.search.FindPrevious();
                            }
                            else
                            {
                                this.search.FindNext();
                            }
                        }
                    }
                    else if (!this.search.Find(this.cbFindWhat.Text, options, this.GetExpression()))
                    {
                        this.ShowNotFound();
                    }
                    else
                    {
                        this.TextFound();
                    }
                }
            }
        }

        private void btReplaceAll_Click(object sender, EventArgs e)
        {
            this.AddToHistory();
            if (this.search != null)
            {
                int num;
                Match match = null;
                SearchOptions options = this.Options;
                if ((!this.search.FirstSearch && (this.cbFindWhat.Text == this.saveText)) && this.search.NeedReplaceCurrent(out match))
                {
                    this.search.ReplaceCurrent(this.cbReplaceWith.Text, options, match);
                }
                if (!this.search.ReplaceAll(this.cbFindWhat.Text, this.cbReplaceWith.Text, this.Options, this.GetExpression(), out num))
                {
                    this.ShowNotFound();
                }
                else
                {
                    MessageBox.Show(string.Format(StringConsts.OccurencesReplaced, num), this.Text);
                }
            }
        }

        private void btReplacePopup_Click(object sender, EventArgs e)
        {
            this.cmReplace.Show((Control) sender, new Point(0, 0));
        }

        private void chbUseRegularExpressions_Click(object sender, EventArgs e)
        {
            this.btPopup.Enabled = this.chbUseRegularExpressions.Checked;
            this.btReplacePopup.Enabled = this.chbUseRegularExpressions.Checked;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void DlgSearch_Load(object sender, EventArgs e)
        {
            base.ActiveControl = this.cbFindWhat;
            this.LoadFromResource();
            this.ResizeControls();
        }

        private void FixCarriageReturn(ref string s)
        {
            this.ReplaceChar(ref s, 'r', string.Empty);
            this.ReplaceChar(ref s, 'n', @"\r\n");
        }

        private Regex GetExpression()
        {
            Regex regex = null;
            if (this.chbUseRegularExpressions.Checked)
            {
                try
                {
                    string text = this.cbFindWhat.Text;
                    this.FixCarriageReturn(ref text);
                    regex = new Regex(text, this.GetRegexOptions(this.cbFindWhat.Text));
                }
                catch (Exception exception)
                {
                    regex = null;
                    ErrorHandler.Error(exception);
                }
            }
            return regex;
        }

        private RegexOptions GetRegexOptions(string text)
        {
            return (((this.chbSearchUp.Checked ? RegexOptions.RightToLeft : RegexOptions.None) | (!this.chbMatchCase.Checked ? RegexOptions.IgnoreCase : RegexOptions.None)) | (((text != null) && ((text.IndexOf(@"\r") >= 0) || (text.IndexOf(@"\n") >= 0))) ? RegexOptions.Multiline : RegexOptions.None));
        }

        public void Init()
        {
            this.firstSearch = true;
            this.saveOptions = SearchOptions.None;
            this.saveText = string.Empty;
            this.UpdateSearch();
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            ComponentResourceManager manager = new ComponentResourceManager(typeof(DlgSearch));
            this.chbPromptOnReplace = new CheckBox();
            this.chbSearchUp = new CheckBox();
            this.chbSearchHiddenText = new CheckBox();
            this.chbMatchWholeWord = new CheckBox();
            this.btReplacePopup = new Button();
            this.chbMatchCase = new CheckBox();
            this.chbUseRegularExpressions = new CheckBox();
            this.cmReplace = new ContextMenu();
            this.miFindWhatText = new MenuItem();
            this.miTaggedExpression1 = new MenuItem();
            this.miTaggedExpression2 = new MenuItem();
            this.miTaggedExpression3 = new MenuItem();
            this.miTaggedExpression4 = new MenuItem();
            this.miTaggedExpression5 = new MenuItem();
            this.miTaggedExpression6 = new MenuItem();
            this.miTaggedExpression7 = new MenuItem();
            this.miTaggedExpression8 = new MenuItem();
            this.miTaggedExpression9 = new MenuItem();
            this.miTag = new MenuItem();
            this.laFindWhat = new Label();
            this.cbReplaceWith = new ComboBox();
            this.laReplaceWith = new Label();
            this.miEscape = new MenuItem();
            this.btReplaceAll = new Button();
            this.btMarkAll = new Button();
            this.btReplace = new Button();
            this.btFindNext = new Button();
            this.cmFind = new ContextMenu();
            this.miSingleChar = new MenuItem();
            this.miZeroOrMore = new MenuItem();
            this.miOneOrMore = new MenuItem();
            this.menuItem4 = new MenuItem();
            this.miBeginLine = new MenuItem();
            this.miEndLine = new MenuItem();
            this.miLineBreak = new MenuItem();
            this.menuItem10 = new MenuItem();
            this.miOneCharInSet = new MenuItem();
            this.miOneCharNotInSet = new MenuItem();
            this.miOr = new MenuItem();
            this.btPopup = new Button();
            this.cbFindWhat = new ComboBox();
            this.pnButtons = new Panel();
            this.btClose = new Button();
            this.gbFindOptions = new GroupBox();
            this.btFindOptions = new Button();
            this.cbLookIn = new ComboBox();
            this.laLookIn = new Label();
            this.laFindOptions = new Label();
            this.pnFindOptions = new Panel();
            this.tbSearch = new TabControl();
            this.tbUseFind = new TabPage();
            this.tbUseReplace = new TabPage();
            this.imSearch = new ImageList(this.components);
            this.pnButtons.SuspendLayout();
            this.gbFindOptions.SuspendLayout();
            this.tbSearch.SuspendLayout();
            base.SuspendLayout();
            this.chbPromptOnReplace.FlatStyle = FlatStyle.System;
            this.chbPromptOnReplace.Location = new Point(8, 0x74);
            this.chbPromptOnReplace.Name = "chbPromptOnReplace";
            this.chbPromptOnReplace.Size = new Size(0xd9, 0x19);
            this.chbPromptOnReplace.TabIndex = 0x10;
            this.chbPromptOnReplace.Text = "Prompt on replace";
            this.chbSearchUp.FlatStyle = FlatStyle.System;
            this.chbSearchUp.Location = new Point(8, 0x38);
            this.chbSearchUp.Name = "chbSearchUp";
            this.chbSearchUp.Size = new Size(0xd9, 0x19);
            this.chbSearchUp.TabIndex = 13;
            this.chbSearchUp.Text = "Search &up";
            this.chbSearchHiddenText.FlatStyle = FlatStyle.System;
            this.chbSearchHiddenText.Location = new Point(8, 0x4c);
            this.chbSearchHiddenText.Name = "chbSearchHiddenText";
            this.chbSearchHiddenText.Size = new Size(0xd9, 0x19);
            this.chbSearchHiddenText.TabIndex = 14;
            this.chbSearchHiddenText.Text = "Search &hidden text";
            this.chbMatchWholeWord.FlatStyle = FlatStyle.System;
            this.chbMatchWholeWord.Location = new Point(8, 0x24);
            this.chbMatchWholeWord.Name = "chbMatchWholeWord";
            this.chbMatchWholeWord.Size = new Size(0xd9, 0x19);
            this.chbMatchWholeWord.TabIndex = 12;
            this.chbMatchWholeWord.Text = "Match &whole word";
            this.btReplacePopup.BackColor = SystemColors.Control;
            this.btReplacePopup.FlatStyle = FlatStyle.System;
            this.btReplacePopup.ImageIndex = 0;
            this.btReplacePopup.Location = new Point(0xef, 90);
            this.btReplacePopup.Name = "btReplacePopup";
            this.btReplacePopup.Size = new Size(0x11, 20);
            this.btReplacePopup.TabIndex = 0x20;
            this.btReplacePopup.Text = ">";
            this.btReplacePopup.UseVisualStyleBackColor = false;
            this.btReplacePopup.Visible = false;
            this.btReplacePopup.Click += new EventHandler(this.btReplacePopup_Click);
            this.chbMatchCase.FlatStyle = FlatStyle.System;
            this.chbMatchCase.Location = new Point(8, 0x10);
            this.chbMatchCase.Name = "chbMatchCase";
            this.chbMatchCase.Size = new Size(0xd9, 0x19);
            this.chbMatchCase.TabIndex = 11;
            this.chbMatchCase.Text = "Match &case";
            this.chbUseRegularExpressions.FlatStyle = FlatStyle.System;
            this.chbUseRegularExpressions.Location = new Point(8, 0x60);
            this.chbUseRegularExpressions.Name = "chbUseRegularExpressions";
            this.chbUseRegularExpressions.Size = new Size(0xd9, 0x19);
            this.chbUseRegularExpressions.TabIndex = 15;
            this.chbUseRegularExpressions.Text = "Use &regular expressions";
            this.chbUseRegularExpressions.CheckedChanged += new EventHandler(this.chbUseRegularExpressions_Click);
            this.cmReplace.MenuItems.AddRange(new MenuItem[] { this.miFindWhatText, this.miTaggedExpression1, this.miTaggedExpression2, this.miTaggedExpression3, this.miTaggedExpression4, this.miTaggedExpression5, this.miTaggedExpression6, this.miTaggedExpression7, this.miTaggedExpression8, this.miTaggedExpression9 });
            this.miFindWhatText.Index = 0;
            this.miFindWhatText.Text = "$0 Find What Text";
            this.miFindWhatText.Click += new EventHandler(this.miFindWhatText_Click);
            this.miTaggedExpression1.Index = 1;
            this.miTaggedExpression1.Text = "$1 Tagged Expression 1";
            this.miTaggedExpression1.Click += new EventHandler(this.miFindWhatText_Click);
            this.miTaggedExpression2.Index = 2;
            this.miTaggedExpression2.Text = "$2 Tagged Expression 2";
            this.miTaggedExpression2.Click += new EventHandler(this.miFindWhatText_Click);
            this.miTaggedExpression3.Index = 3;
            this.miTaggedExpression3.Text = "$3 Tagged Expression 3";
            this.miTaggedExpression3.Click += new EventHandler(this.miFindWhatText_Click);
            this.miTaggedExpression4.Index = 4;
            this.miTaggedExpression4.Text = "$4 Tagged Expression 4";
            this.miTaggedExpression4.Click += new EventHandler(this.miFindWhatText_Click);
            this.miTaggedExpression5.Index = 5;
            this.miTaggedExpression5.Text = "$5 Tagged Expression 5";
            this.miTaggedExpression5.Click += new EventHandler(this.miFindWhatText_Click);
            this.miTaggedExpression6.Index = 6;
            this.miTaggedExpression6.Text = "$6 Tagged Expression 6";
            this.miTaggedExpression6.Click += new EventHandler(this.miFindWhatText_Click);
            this.miTaggedExpression7.Index = 7;
            this.miTaggedExpression7.Text = "$7 Tagged Expression 7";
            this.miTaggedExpression7.Click += new EventHandler(this.miFindWhatText_Click);
            this.miTaggedExpression8.Index = 8;
            this.miTaggedExpression8.Text = "$8 Tagged Expression 8";
            this.miTaggedExpression8.Click += new EventHandler(this.miFindWhatText_Click);
            this.miTaggedExpression9.Index = 9;
            this.miTaggedExpression9.Text = "$9 Tagged Expression 9";
            this.miTaggedExpression9.Click += new EventHandler(this.miFindWhatText_Click);
            this.miTag.Index = 12;
            this.miTag.Text = "{} Tag expression";
            this.miTag.Click += new EventHandler(this.miBeginWord_Click);
            this.laFindWhat.AutoSize = true;
            this.laFindWhat.FlatStyle = FlatStyle.System;
            this.laFindWhat.Location = new Point(8, 0x21);
            this.laFindWhat.Name = "laFindWhat";
            this.laFindWhat.Size = new Size(0x38, 13);
            this.laFindWhat.TabIndex = 20;
            this.laFindWhat.Text = "Fi&nd what:";
            this.cbReplaceWith.Location = new Point(8, 90);
            this.cbReplaceWith.Name = "cbReplaceWith";
            this.cbReplaceWith.Size = new Size(0xe1, 0x15);
            this.cbReplaceWith.TabIndex = 0x18;
            this.cbReplaceWith.Visible = false;
            this.laReplaceWith.AutoSize = true;
            this.laReplaceWith.FlatStyle = FlatStyle.System;
            this.laReplaceWith.Location = new Point(8, 0x4a);
            this.laReplaceWith.Name = "laReplaceWith";
            this.laReplaceWith.Size = new Size(0x48, 13);
            this.laReplaceWith.TabIndex = 0x16;
            this.laReplaceWith.Text = "Re&place with:";
            this.laReplaceWith.Visible = false;
            this.miEscape.Index = 11;
            this.miEscape.Text = @"\ Escape Special Character";
            this.miEscape.Click += new EventHandler(this.miBeginWord_Click);
            this.btReplaceAll.FlatStyle = FlatStyle.System;
            this.btReplaceAll.Location = new Point(0x9e, 0x23);
            this.btReplaceAll.Name = "btReplaceAll";
            this.btReplaceAll.Size = new Size(0x62, 0x17);
            this.btReplaceAll.TabIndex = 0x1b;
            this.btReplaceAll.Text = "Replace &All";
            this.btReplaceAll.Visible = false;
            this.btReplaceAll.Click += new EventHandler(this.btReplaceAll_Click);
            this.btMarkAll.FlatStyle = FlatStyle.System;
            this.btMarkAll.Location = new Point(0x9e, 8);
            this.btMarkAll.Name = "btMarkAll";
            this.btMarkAll.Size = new Size(0x62, 0x17);
            this.btMarkAll.TabIndex = 0x1c;
            this.btMarkAll.Text = "Bookmark All";
            this.btMarkAll.Click += new EventHandler(this.btMarkAll_Click);
            this.btReplace.FlatStyle = FlatStyle.System;
            this.btReplace.ImageAlign = ContentAlignment.MiddleRight;
            this.btReplace.Location = new Point(0x9e, 8);
            this.btReplace.Name = "btReplace";
            this.btReplace.Size = new Size(0x62, 0x17);
            this.btReplace.TabIndex = 0x1a;
            this.btReplace.Text = "&Replace";
            this.btReplace.Visible = false;
            this.btReplace.Click += new EventHandler(this.btReplace_Click);
            this.btFindNext.FlatStyle = FlatStyle.System;
            this.btFindNext.Location = new Point(0x36, 8);
            this.btFindNext.Name = "btFindNext";
            this.btFindNext.Size = new Size(0x62, 0x17);
            this.btFindNext.TabIndex = 0x19;
            this.btFindNext.Text = "&Find Next";
            this.btFindNext.Click += new EventHandler(this.btFindNext_Click);
            this.cmFind.MenuItems.AddRange(new MenuItem[] { this.miSingleChar, this.miZeroOrMore, this.miOneOrMore, this.menuItem4, this.miBeginLine, this.miEndLine, this.miLineBreak, this.menuItem10, this.miOneCharInSet, this.miOneCharNotInSet, this.miOr, this.miEscape, this.miTag });
            this.miSingleChar.Index = 0;
            this.miSingleChar.Text = ". Any single character";
            this.miSingleChar.Click += new EventHandler(this.miBeginWord_Click);
            this.miZeroOrMore.Index = 1;
            this.miZeroOrMore.Text = "* Zero or more";
            this.miZeroOrMore.Click += new EventHandler(this.miBeginWord_Click);
            this.miOneOrMore.Index = 2;
            this.miOneOrMore.Text = "+ One or more";
            this.miOneOrMore.Click += new EventHandler(this.miBeginWord_Click);
            this.menuItem4.Index = 3;
            this.menuItem4.Text = "-";
            this.miBeginLine.Index = 4;
            this.miBeginLine.Text = "^ Beginning of line";
            this.miBeginLine.Click += new EventHandler(this.miBeginWord_Click);
            this.miEndLine.Index = 5;
            this.miEndLine.Text = "$ End of line";
            this.miEndLine.Click += new EventHandler(this.miBeginWord_Click);
            this.miLineBreak.Index = 6;
            this.miLineBreak.Text = @"\n Line break";
            this.miLineBreak.Click += new EventHandler(this.miBeginWord_Click);
            this.menuItem10.Index = 7;
            this.menuItem10.Text = "-";
            this.miOneCharInSet.Index = 8;
            this.miOneCharInSet.Text = "[] Any one character in the set";
            this.miOneCharInSet.Click += new EventHandler(this.miBeginWord_Click);
            this.miOneCharNotInSet.Index = 9;
            this.miOneCharNotInSet.Text = "[^] Any one character not in the set";
            this.miOneCharNotInSet.Click += new EventHandler(this.miBeginWord_Click);
            this.miOr.Index = 10;
            this.miOr.Text = "| Or";
            this.miOr.Click += new EventHandler(this.miBeginWord_Click);
            this.btPopup.BackColor = SystemColors.Control;
            this.btPopup.FlatStyle = FlatStyle.System;
            this.btPopup.ImageIndex = 0;
            this.btPopup.Location = new Point(0xef, 50);
            this.btPopup.Name = "btPopup";
            this.btPopup.Size = new Size(0x11, 20);
            this.btPopup.TabIndex = 0x17;
            this.btPopup.Text = ">";
            this.btPopup.UseVisualStyleBackColor = false;
            this.btPopup.Click += new EventHandler(this.btPopup_Click);
            this.cbFindWhat.Location = new Point(8, 0x31);
            this.cbFindWhat.Name = "cbFindWhat";
            this.cbFindWhat.Size = new Size(0xe1, 0x15);
            this.cbFindWhat.TabIndex = 0x15;
            this.pnButtons.Controls.Add(this.btClose);
            this.pnButtons.Controls.Add(this.btFindNext);
            this.pnButtons.Controls.Add(this.btReplace);
            this.pnButtons.Controls.Add(this.btReplaceAll);
            this.pnButtons.Controls.Add(this.btMarkAll);
            this.pnButtons.Dock = DockStyle.Bottom;
            this.pnButtons.Location = new Point(0, 0x139);
            this.pnButtons.Name = "pnButtons";
            this.pnButtons.Size = new Size(0x10b, 0x41);
            this.pnButtons.TabIndex = 0x22;
            this.btClose.DialogResult = DialogResult.Cancel;
            this.btClose.FlatStyle = FlatStyle.System;
            this.btClose.Location = new Point(0xc0, 0xed);
            this.btClose.Name = "btClose";
            this.btClose.Size = new Size(0x62, 0x17);
            this.btClose.TabIndex = 0x1d;
            this.btClose.Text = "Close";
            this.btClose.Click += new EventHandler(this.btClose_Click);
            this.gbFindOptions.Controls.Add(this.chbPromptOnReplace);
            this.gbFindOptions.Controls.Add(this.chbMatchCase);
            this.gbFindOptions.Controls.Add(this.chbMatchWholeWord);
            this.gbFindOptions.Controls.Add(this.chbUseRegularExpressions);
            this.gbFindOptions.Controls.Add(this.chbSearchUp);
            this.gbFindOptions.Controls.Add(this.chbSearchHiddenText);
            this.gbFindOptions.Location = new Point(8, 0xa5);
            this.gbFindOptions.Name = "gbFindOptions";
            this.gbFindOptions.Size = new Size(0xf8, 0x8e);
            this.gbFindOptions.TabIndex = 0x23;
            this.gbFindOptions.TabStop = false;
            this.gbFindOptions.Text = "    Find Options";
            this.btFindOptions.BackColor = SystemColors.Control;
            this.btFindOptions.FlatStyle = FlatStyle.System;
            this.btFindOptions.Font = new Font("Symbol", 6.75f, FontStyle.Regular, GraphicsUnit.Point, 0xcc);
            this.btFindOptions.ImageIndex = 0;
            this.btFindOptions.Location = new Point(8, 0x9d);
            this.btFindOptions.Name = "btFindOptions";
            this.btFindOptions.Size = new Size(0x12, 0x12);
            this.btFindOptions.TabIndex = 0x27;
            this.btFindOptions.Text = "-";
            this.btFindOptions.UseVisualStyleBackColor = false;
            this.btFindOptions.Click += new EventHandler(this.btFindOptions_Click);
            this.cbLookIn.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cbLookIn.FormattingEnabled = true;
            this.cbLookIn.Location = new Point(8, 130);
            this.cbLookIn.Name = "cbLookIn";
            this.cbLookIn.Size = new Size(0xf8, 0x15);
            this.cbLookIn.TabIndex = 0x25;
            this.laLookIn.AutoSize = true;
            this.laLookIn.FlatStyle = FlatStyle.System;
            this.laLookIn.Location = new Point(8, 0x72);
            this.laLookIn.Name = "laLookIn";
            this.laLookIn.Size = new Size(0x2e, 13);
            this.laLookIn.TabIndex = 0x26;
            this.laLookIn.Text = "Look In:";
            this.laFindOptions.AutoSize = true;
            this.laFindOptions.Location = new Point(0x1a, 0x9a);
            this.laFindOptions.Name = "laFindOptions";
            this.laFindOptions.Size = new Size(0x42, 13);
            this.laFindOptions.TabIndex = 40;
            this.laFindOptions.Text = "Find Options";
            this.laFindOptions.Visible = false;
            this.pnFindOptions.BackColor = SystemColors.ScrollBar;
            this.pnFindOptions.Location = new Point(90, 0x9d);
            this.pnFindOptions.Name = "pnFindOptions";
            this.pnFindOptions.Size = new Size(0xa4, 1);
            this.pnFindOptions.TabIndex = 0x29;
            this.pnFindOptions.Visible = false;
            this.tbSearch.Appearance = TabAppearance.FlatButtons;
            this.tbSearch.Controls.Add(this.tbUseFind);
            this.tbSearch.Controls.Add(this.tbUseReplace);
            this.tbSearch.Dock = DockStyle.Top;
            this.tbSearch.ImageList = this.imSearch;
            this.tbSearch.ItemSize = new Size(80, 0x16);
            this.tbSearch.Location = new Point(0, 0);
            this.tbSearch.Name = "tbSearch";
            this.tbSearch.SelectedIndex = 0;
            this.tbSearch.Size = new Size(0x10b, 0x18);
            this.tbSearch.TabIndex = 0x11;
            this.tbSearch.Selected += new TabControlEventHandler(this.tbSearch_Selected);
            this.tbSearch.Enter += new EventHandler(this.tbSearch_Enter);
            this.tbUseFind.ImageIndex = 0;
            this.tbUseFind.Location = new Point(4, 0x1a);
            this.tbUseFind.Name = "tbUseFind";
            this.tbUseFind.Padding = new Padding(3);
            this.tbUseFind.Size = new Size(0x103, 0);
            this.tbUseFind.TabIndex = 0;
            this.tbUseFind.Text = "Find";
            this.tbUseFind.UseVisualStyleBackColor = true;
            this.tbUseReplace.ImageIndex = 1;
            this.tbUseReplace.Location = new Point(4, 0x1a);
            this.tbUseReplace.Name = "tbUseReplace";
            this.tbUseReplace.Padding = new Padding(3);
            this.tbUseReplace.Size = new Size(0x103, 0);
            this.tbUseReplace.TabIndex = 1;
            this.tbUseReplace.Text = "Replace";
            this.tbUseReplace.UseVisualStyleBackColor = true;
            this.tbUseReplace.Enter += new EventHandler(this.tbUseReplace_Enter);
            this.imSearch.ImageStream = (ImageListStreamer) manager.GetObject("imSearch.ImageStream");
            this.imSearch.TransparentColor = Color.Fuchsia;
            this.imSearch.Images.SetKeyName(0, "UseFind1.bmp");
            this.imSearch.Images.SetKeyName(1, "UseReplace1.bmp");
            base.AcceptButton = this.btFindNext;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.CancelButton = this.btClose;
            base.ClientSize = new Size(0x10b, 0x17a);
            base.Controls.Add(this.tbSearch);
            base.Controls.Add(this.pnFindOptions);
            base.Controls.Add(this.laFindOptions);
            base.Controls.Add(this.btFindOptions);
            base.Controls.Add(this.laLookIn);
            base.Controls.Add(this.cbLookIn);
            base.Controls.Add(this.gbFindOptions);
            base.Controls.Add(this.pnButtons);
            base.Controls.Add(this.btReplacePopup);
            base.Controls.Add(this.laFindWhat);
            base.Controls.Add(this.cbReplaceWith);
            base.Controls.Add(this.laReplaceWith);
            base.Controls.Add(this.btPopup);
            base.Controls.Add(this.cbFindWhat);
            base.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            base.Name = "DlgSearch";
            base.ShowInTaskbar = false;
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Find";
            base.Load += new EventHandler(this.DlgSearch_Load);
            this.pnButtons.ResumeLayout(false);
            this.gbFindOptions.ResumeLayout(false);
            this.tbSearch.ResumeLayout(false);
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private bool IsEscapeChar(string s, int index)
        {
            bool flag = false;
            while ((index > 0) && (s[index - 1] == '\\'))
            {
                if (s[index - 1] == '\\')
                {
                    flag = !flag;
                }
                index--;
            }
            return flag;
        }

        private void LoadFromResource()
        {
            this.laFindWhat.Text = StringConsts.FindWhatCaption;
            this.laReplaceWith.Text = StringConsts.ReplaceWithCaption;
            this.chbMatchCase.Text = StringConsts.MatchCaseCaption;
            this.chbMatchWholeWord.Text = StringConsts.MatchWholeWordCaption;
            this.chbSearchHiddenText.Text = StringConsts.SearchHiddenTextCaption;
            this.chbSearchUp.Text = StringConsts.SearchUpCaption;
            this.chbUseRegularExpressions.Text = StringConsts.UseRegularExpressionsCaption;
            this.chbPromptOnReplace.Text = StringConsts.PromptOnReplaceCaption;
            this.btFindNext.Text = StringConsts.FindNextCaption;
            this.btReplace.Text = StringConsts.ReplaceCaption + " ";
            this.tbUseFind.Text = StringConsts.DlgSearchCaption + " ";
            this.tbUseReplace.Text = StringConsts.ReplaceCaption;
            this.btReplaceAll.Text = StringConsts.ReplaceAllCaption;
            this.btMarkAll.Text = StringConsts.MarkAllCaption;
            this.gbFindOptions.Text = string.Format("    {0}", StringConsts.FindOptionsCaption);
            this.laFindOptions.Text = string.Format(StringConsts.FindOptionsCaption, new object[0]);
            this.laLookIn.Text = StringConsts.LookInCaption;
            this.Text = StringConsts.DlgSearchReplaceCaption;
            this.miSingleChar.Text = StringConsts.SingleCharCaption;
            this.miZeroOrMore.Text = StringConsts.ZeroOrMoreCaption;
            this.miOneOrMore.Text = StringConsts.OneOrMoreCaption;
            this.miBeginLine.Text = StringConsts.BeginLineCaption;
            this.miEndLine.Text = StringConsts.EndLineCaption;
            this.miLineBreak.Text = StringConsts.LineBreakCaption;
            this.miOneCharInSet.Text = StringConsts.OneCharInSetCaption;
            this.miOneCharNotInSet.Text = StringConsts.OneCharNotInSetCaption;
            this.miOr.Text = StringConsts.OrCaption;
            this.miEscape.Text = StringConsts.EscapeCaption;
            this.miTag.Text = StringConsts.TagCaption;
            this.miFindWhatText.Text = StringConsts.FindWhatTextCaption;
            this.miTaggedExpression1.Text = string.Format("${1} {0} {1}", StringConsts.TagExpressionCaption, 1);
            this.miTaggedExpression2.Text = string.Format("${1} {0} {1}", StringConsts.TagExpressionCaption, 2);
            this.miTaggedExpression3.Text = string.Format("${1} {0} {1}", StringConsts.TagExpressionCaption, 3);
            this.miTaggedExpression4.Text = string.Format("${1} {0} {1}", StringConsts.TagExpressionCaption, 4);
            this.miTaggedExpression5.Text = string.Format("${1} {0} {1}", StringConsts.TagExpressionCaption, 5);
            this.miTaggedExpression6.Text = string.Format("${1} {0} {1}", StringConsts.TagExpressionCaption, 6);
            this.miTaggedExpression7.Text = string.Format("${1} {0} {1}", StringConsts.TagExpressionCaption, 7);
            this.miTaggedExpression8.Text = string.Format("${1} {0} {1}", StringConsts.TagExpressionCaption, 8);
            this.miTaggedExpression9.Text = string.Format("${1} {0} {1}", StringConsts.TagExpressionCaption, 9);
        }

        private void miBeginWord_Click(object sender, EventArgs e)
        {
            string str = ((MenuItem) sender).Text.Trim();
            int index = str.IndexOf(' ');
            if (index > 0)
            {
                str = str.Substring(0, index);
            }
            this.cbFindWhat.Text = this.cbFindWhat.Text + str;
        }

        private void miFindWhatText_Click(object sender, EventArgs e)
        {
            string str = ((MenuItem) sender).Text.Trim();
            int index = str.IndexOf(' ');
            if (index > 0)
            {
                str = str.Substring(0, index);
            }
            this.cbReplaceWith.Text = this.cbReplaceWith.Text + str;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if ((this.cbFindWhat.Text == string.Empty) && (this.cbFindWhat.Items.Count > 0))
            {
                this.cbFindWhat.SelectedIndex = 0;
            }
        }

        private void RemoveChar(ref string s, char ch)
        {
            for (int i = s.Length - 1; i > 0; i--)
            {
                if ((s[i] == ch) && this.IsEscapeChar(s, i))
                {
                    s = s.Remove(i - 1, 2);
                }
            }
        }

        protected void ReplaceChanged()
        {
            this.laReplaceWith.Visible = this.isReplace;
            this.cbReplaceWith.Visible = this.isReplace;
            this.btReplacePopup.Visible = this.isReplace;
            this.btReplace.Visible = this.isReplace;
            this.btReplaceAll.Visible = this.isReplace;
            this.btMarkAll.Visible = !this.isReplace;
            this.btPopup.Enabled = this.chbUseRegularExpressions.Checked;
            this.btReplacePopup.Enabled = this.chbUseRegularExpressions.Checked;
            this.tbSearch.SelectedIndex = this.isReplace ? 1 : 0;
            this.ResizeControls();
        }

        private void ReplaceChar(ref string s, char ch, string replace)
        {
            for (int i = s.Length - 1; i > 0; i--)
            {
                if ((s[i] == ch) && this.IsEscapeChar(s, i))
                {
                    s = s.Remove(i - 1, 2);
                    if (replace != string.Empty)
                    {
                        s = s.Insert(i - 1, replace);
                    }
                }
            }
        }

        private void ResizeControls()
        {
            this.gbFindOptions.Visible = this.isOptionsVisible;
            this.laFindOptions.Visible = !this.isOptionsVisible;
            this.pnFindOptions.Visible = !this.isOptionsVisible;
            this.btFindOptions.Text = this.isOptionsVisible ? "-" : "+";
            this.laLookIn.Top = (this.IsReplace ? this.cbReplaceWith.Bottom : this.cbFindWhat.Bottom) + 4;
            this.cbLookIn.Top = (this.IsReplace ? this.cbReplaceWith.Bottom : this.cbFindWhat.Bottom) + 0x13;
            this.gbFindOptions.Top = this.cbLookIn.Bottom + 14;
            this.laFindOptions.Top = this.gbFindOptions.Top;
            this.pnFindOptions.Top = this.gbFindOptions.Top + 6;
            this.btFindOptions.Top = this.gbFindOptions.Top - 2;
            this.pnButtons.Height = this.IsReplace ? 0x41 : 0x24;
            base.Height = (((base.Height - base.ClientSize.Height) + (this.gbFindOptions.Visible ? this.gbFindOptions.Bottom : (this.gbFindOptions.Top + 12))) + this.pnButtons.Height) + 2;
        }

        protected void ShowNotFound()
        {
            this.search.ShowNotFound(this.Text);
            this.cbFindWhat.Focus();
        }

        private void tbSearch_Enter(object sender, EventArgs e)
        {
            if (this.cbFindWhat.CanFocus)
            {
                this.cbFindWhat.Focus();
            }
        }

        private void tbSearch_Selected(object sender, TabControlEventArgs e)
        {
            this.IsReplace = this.tbSearch.SelectedIndex == 1;
        }

        private void tbUseReplace_Enter(object sender, EventArgs e)
        {
            if (this.cbFindWhat.CanFocus)
            {
                this.cbFindWhat.Focus();
            }
        }

        private void TextFound()
        {
            this.firstSearch = false;
            this.saveOptions = this.Options;
            this.saveText = this.cbFindWhat.Text;
        }

        private void UpdateSearch()
        {
            this.cbLookIn.Items.Clear();
            this.cbLookIn.Items.Add(StringConsts.FromCursorCaption);
            this.cbLookIn.Items.Add(StringConsts.EntireScopeCaption.Replace("&", string.Empty));
            if (this.selectionEnabled)
            {
                this.cbLookIn.Items.Add(StringConsts.SelectionOnlyCaption.Replace("&", string.Empty));
            }
            this.cbLookIn.SelectedIndex = 0;
        }

        public bool ClearBookmarks
        {
            get
            {
                return this.clearBookmarks;
            }
            set
            {
                this.clearBookmarks = value;
            }
        }

        public bool IsReplace
        {
            get
            {
                return this.isReplace;
            }
            set
            {
                if (this.isReplace != value)
                {
                    this.isReplace = value;
                    this.ReplaceChanged();
                }
            }
        }

        public SearchOptions Options
        {
            get
            {
                return ((((((((((((this.chbMatchCase.Checked ? SearchOptions.CaseSensitive : SearchOptions.None) | (this.chbMatchWholeWord.Checked ? SearchOptions.WholeWordsOnly : SearchOptions.None)) | (this.chbSearchHiddenText.Checked ? SearchOptions.SearchHiddenText : SearchOptions.None)) | (this.chbSearchUp.Checked ? SearchOptions.BackwardSearch : SearchOptions.None)) | (this.chbUseRegularExpressions.Checked ? SearchOptions.RegularExpressions : SearchOptions.None)) | ((this.cbLookIn.SelectedIndex == 1) ? SearchOptions.EntireScope : SearchOptions.None)) | ((this.cbLookIn.SelectedIndex == 2) ? SearchOptions.SelectionOnly : SearchOptions.None)) | (this.findTextAtCursor ? SearchOptions.FindTextAtCursor : SearchOptions.None)) | (this.chbPromptOnReplace.Checked ? SearchOptions.PromptOnReplace : SearchOptions.None)) | (((this.options & SearchOptions.FindSelectedText) != SearchOptions.None) ? SearchOptions.FindSelectedText : SearchOptions.None)) | (((this.options & SearchOptions.CycledSearch) != SearchOptions.None) ? SearchOptions.CycledSearch : SearchOptions.None)) | (((this.options & SearchOptions.SilentSearch) != SearchOptions.None) ? SearchOptions.SilentSearch : SearchOptions.None));
            }
            set
            {
                this.options = value;
                this.UpdateSearch();
                this.chbMatchCase.Checked = (value & SearchOptions.CaseSensitive) != SearchOptions.None;
                this.chbMatchWholeWord.Checked = (value & SearchOptions.WholeWordsOnly) != SearchOptions.None;
                this.chbSearchHiddenText.Checked = (value & SearchOptions.SearchHiddenText) != SearchOptions.None;
                this.chbSearchUp.Checked = (value & SearchOptions.BackwardSearch) != SearchOptions.None;
                this.chbUseRegularExpressions.Checked = (value & SearchOptions.RegularExpressions) != SearchOptions.None;
                if ((value & SearchOptions.EntireScope) != SearchOptions.None)
                {
                    this.cbLookIn.SelectedIndex = 1;
                }
                if ((value & SearchOptions.SelectionOnly) != SearchOptions.None)
                {
                    this.cbLookIn.SelectedIndex = (this.cbLookIn.Items.Count > 2) ? 2 : 0;
                }
                this.btPopup.Enabled = this.chbUseRegularExpressions.Checked;
                this.findTextAtCursor = (value & SearchOptions.FindTextAtCursor) != SearchOptions.None;
                this.chbPromptOnReplace.Checked = (value & SearchOptions.PromptOnReplace) != SearchOptions.None;
            }
        }

        public ISearch Search
        {
            get
            {
                return this.search;
            }
            set
            {
                this.search = value;
            }
        }

        public bool SelectionEnabled
        {
            get
            {
                return this.selectionEnabled;
            }
            set
            {
                this.selectionEnabled = value;
            }
        }
    }
}

