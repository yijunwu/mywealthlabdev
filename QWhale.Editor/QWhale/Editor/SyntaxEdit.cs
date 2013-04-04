namespace QWhale.Editor
{
    using QWhale.Common;
    using QWhale.Design;
    using QWhale.Editor.CodeCompletion;
    using QWhale.Editor.Dialogs;
    using QWhale.Editor.Serialization;
    using QWhale.Editor.TextSource;
    using QWhale.Syntax;
    using QWhale.Syntax.CodeCompletion;
    using QWhale.Syntax.Lexer;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Design;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Windows.Forms;
    using System.Xml.Serialization;

    [ToolboxBitmap(typeof(SyntaxEdit), "Images.SyntaxEdit.bmp")]
    public class SyntaxEdit : Control, ISyntaxEdit, ISearch, ITextSearch, IEditNotifier, INotifier, ICaret, IEditNavigate, INavigate, IEdit, IWordWrap, ITextExport, IExport, ITextImport, IImport, ICodeCompletion, IRecordPlayBack, ISplitView, IAutoCorrect, IControl
    {
        private bool acceptReturns;
        private bool acceptTabs;
        private char[] autoCorrectDelimiters;
        private AutoCorrectEventArgs autoCorrectEventArgs;
        private bool autoCorrection;
        private Color borderColor;
        private EditBorderStyle borderStyle;
        private IEditBraceMatching braces;
        private CodeCompletionArgs codeCompletionArgs;
        private ICodeCompletionBox codeCompletionBox;
        private char[] codeCompletionChars;
        private ICodeCompletionHint codeCompletionHint;
        private Timer codeCompletionTimer;
        private Container components;
        private ContextMenuStrip defaultMenu;
        private IDisplayStrings displayLines;
        private bool dragCaret;
        private bool dragMargin;
        private IEditorSettingsDialog editorSettingsDialog;
        private bool firstSearch;
        private IGotoLineDialog gotoLineDialog;
        private IGutter gutter;
        private bool hideCaret;
        private Point hideCaretPoint;
        private Cursor hideWhiteSpaceCursor;
        private ISyntaxEdit horzSplitEdit;
        private Splitter horzSplitter;
        private IEditHyperText hyperText;
        private Cursor incrementalSearchCursor;
        private bool incrSearchFlag;
        private Point incrSearchPosition;
        private Point incrStartSearchPosition;
        private Point infoTipPos;
        private bool inIncrementalSearch;
        private ITextSource innerTextSource;
        private bool keepCaretOnLostFocus;
        private IKeyList keyList;
        private bool keyProcessed;
        private int keyState;
        private char lastKey;
        private int lbuttonClicks;
        private Cursor leftArrowCursor;
        private ILineSeparator lineSeparator;
        private IEditLineStyles lineStyles;
        private bool macroRecording;
        private IMacroKeyList macroRecords;
        private bool macroSuspended;
        private int macroUpdateCount;
        private IMargin margin;
        private ToolStripMenuItem miCopy;
        private ToolStripMenuItem miCut;
        private ToolStripMenuItem miDelete;
        private ToolStripMenuItem miPaste;
        private ToolStripMenuItem miRedo;
        private ToolStripMenuItem miSelectAll;
        private ToolStripMenuItem miUndo;
        private IBookMark mouseBookMark;
        private Point mouseBookMarkPt;
        private ISyntaxError mouseError;
        private IOutlineRange mouseRange;
        private string mouseUrl;
        private Point mouseUrlPoint;
        private bool needStartDrag;
        private NotifyEventArgs notifyEventArgs;
        private Point oldDragPoint;
        private int oldLine;
        private IOutlining outlining;
        private IEditPages pages;
        private IPainter painter;
        private IPrinting printing;
        private Cursor reverseIncrementalSearchCursor;
        private Point saveDragPos;
        private IScrolling scrolling;
        private bool searchCycled;
        private ISearchDialog searchDialog;
        private Regex searchExpression;
        private int searchLen;
        private Match searchMatch;
        private QWhale.Editor.TextSource.SearchOptions searchOptions;
        private Point searchPos;
        private Rectangle searchSelRect;
        private SelectionType searchSelType;
        private Point searchStartPos;
        private string searchText;
        private int searchUpdateCount;
        private ISelection selection;
        private Cursor showWhiteSpaceCursor;
        private ICodeSnippetRange snippetRange;
        private ITextSource source;
        private IEditSpelling spelling;
        private Point startDragPos;
        private IEditSyntaxPaint syntaxPaint;
        private ISyntaxSettings syntaxSettings;
        private bool transparent;
        private Timer tripleClickTimer;
        private bool urlAtCursor;
        private bool useDefaultMenu;
        private bool vertNavigate;
        private int vertNavigateX;
        private ISyntaxEdit vertSplitEdit;
        private Splitter vertSplitter;
        private IWhiteSpace whiteSpace;

        [Description("Occurs when control tries to auto correct word being typed."), Category("SyntaxEdit")]
        public event AutoCorrectEvent AutoCorrect;

        [Category("SyntaxEdit"), Description("Occurs when a control needs checking whether some string represents hypertext.")]
        public event HyperTextEvent CheckHyperText
        {
            add
            {
                this.hyperText.HyperText += value;
            }
            remove
            {
                this.hyperText.HyperText -= value;
            }
        }

        [Category("SyntaxEdit"), Description("Occurs when control draws its content.")]
        public event CustomDrawEvent CustomDraw
        {
            add
            {
                this.syntaxPaint.CustomDraw += value;
            }
            remove
            {
                this.syntaxPaint.CustomDraw -= value;
            }
        }

        [Category("SyntaxEdit"), Description("Occurs when header or footer part of each page is drawing.")]
        public event DrawHeaderEvent DrawHeader
        {
            add
            {
                this.pages.DrawHeader += value;
            }
            remove
            {
                this.pages.DrawHeader -= value;
            }
        }

        [Description("Occurs when user margin part of each line is drawing."), Category("SyntaxEdit")]
        public event DrawUserMarginEvent DrawUserMargin
        {
            add
            {
                this.gutter.DrawUserMargin += value;
            }
            remove
            {
                this.gutter.DrawUserMargin -= value;
            }
        }

        [Description("Occurs when the gutter part of Edit control is clicked."), Category("SyntaxEdit")]
        public event EventHandler GutterClick
        {
            add
            {
                this.gutter.Click += value;
            }
            remove
            {
                this.gutter.Click -= value;
            }
        }

        [Category("SyntaxEdit"), Description("Occurs when the gutter part of Edit control is double-clicked.")]
        public event EventHandler GutterDblClick
        {
            add
            {
                this.gutter.DoubleClick += value;
            }
            remove
            {
                this.gutter.DoubleClick -= value;
            }
        }

        [Category("SyntaxEdit"), Description("Occurs when control scrolls its content in horizonal direction. This can be caused by dragging horizonal scroll thumb, or caret moving.")]
        public event EventHandler HorizontalScroll
        {
            add
            {
                this.scrolling.HorizontalScroll += value;
            }
            remove
            {
                this.scrolling.HorizontalScroll -= value;
            }
        }

        [Description("Occurs when user attempts to jump to url."), Category("SyntaxEdit")]
        public event UrlJumpEvent JumpToUrl
        {
            add
            {
                this.hyperText.JumpToUrl += value;
            }
            remove
            {
                this.hyperText.JumpToUrl -= value;
            }
        }

        [Category("SyntaxEdit"), Description("Occurs when modified state is changed.")]
        public event EventHandler ModifiedChanged;

        [Category("SyntaxEdit"), Description("Occurs when code completion window is to be displayed.")]
        public event CodeCompletionEvent NeedCodeCompletion;

        [Description("Occurs when Edit control should paint its background in transparent mode."), Category("SyntaxEdit")]
        public event PaintEventHandler PaintBackground;

        [Description("Occurs when replace dialog prompts on replace action."), Category("SyntaxEdit")]
        public event PromptOnReplaceEvent PromptOnReplace;

        [Description("Occurs when some scrolling button is clicked."), Category("SyntaxEdit")]
        public event EventHandler ScrollButtonClick
        {
            add
            {
                this.scrolling.ScrollButtonClick += value;
            }
            remove
            {
                this.scrolling.ScrollButtonClick -= value;
            }
        }

        [Category("SyntaxEdit"), Description("Occurs when selection bounds are changed.")]
        public event EventHandler SelectionChanged
        {
            add
            {
                this.selection.SelectionChanged += value;
            }
            remove
            {
                this.selection.SelectionChanged -= value;
            }
        }

        [Category("SyntaxEdit"), Description("Occurs when text Source's state is changed, for example when caret position moved, text edited, amount of lines changed, lexer changed, etc.")]
        public event NotifyEvent SourceStateChanged;

        [Category("SyntaxEdit"), Description("Occurs when user splits Edit control horizontally.")]
        public event EventHandler SplitHorz;

        [Description("Occurs when user splits Edit control vertically."), Category("SyntaxEdit")]
        public event EventHandler SplitVert;

        [Description("Occurs when undo/redo operation performed."), Category("SyntaxEdit")]
        public event QWhale.Editor.TextSource.UndoEvent UndoEvent
        {
            add
            {
                this.Source.UndoEvent += value;
            }
            remove
            {
                this.Source.UndoEvent -= value;
            }
        }

        [Category("SyntaxEdit"), Description("Occurs when horizontal split view is removed.")]
        public event EventHandler UnsplitHorz;

        [Category("SyntaxEdit"), Description("Occurs when vertical split view is removed.")]
        public event EventHandler UnsplitVert;

        [Description("Occurs when control scrolls its content in vertical direction. This can be caused by dragging vertical scroll thumb, or caret moving."), Category("SyntaxEdit")]
        public event EventHandler VerticalScroll
        {
            add
            {
                this.scrolling.VerticalScroll += value;
            }
            remove
            {
                this.scrolling.VerticalScroll -= value;
            }
        }

        [Description("Occurs when spelling of some word within the text needs checking."), Category("SyntaxEdit")]
        public event WordSpellEvent WordSpell
        {
            add
            {
                this.spelling.WordSpell += value;
            }
            remove
            {
                this.spelling.WordSpell -= value;
            }
        }

        public SyntaxEdit()
        {
            this.infoTipPos = new Point(-1, -1);
            this.mouseUrl = string.Empty;
            this.acceptReturns = true;
            this.acceptTabs = true;
            this.borderStyle = EditBorderStyle.Fixed3D;
            this.borderColor = Color.Empty;
            this.useDefaultMenu = true;
            this.searchOptions = EditConsts.DefaultSearchOptions;
            this.firstSearch = true;
            this.hideCaretPoint = new Point(-100, -100);
            this.oldDragPoint = new Point(-100, -100);
            this.oldLine = -1;
            this.codeCompletionChars = EditConsts.DefaultCodeCompletionChars.ToCharArray();
            this.codeCompletionArgs = new CodeCompletionArgs();
            this.autoCorrectDelimiters = EditConsts.DefaultAutoCorrectDelimiters.ToCharArray();
            this.components = new Container();
            this.innerTextSource = new InnerTextSource();
            this.Source = this.innerTextSource;
            this.source = null;
            this.painter = new GdiPainter();
            this.gutter = new QWhale.Editor.Gutter(this);
            this.displayLines = new DisplayStrings(this, this.Lines);
            this.margin = new Margin(this);
            this.selection = new QWhale.Editor.Selection(this);
            this.scrolling = new QWhale.Editor.Scrolling(this);
            this.printing = new QWhale.Editor.Printing(this);
            this.pages = new EditPages(this);
            this.pages.Add();
            this.whiteSpace = new QWhale.Editor.WhiteSpace(this);
            this.lineSeparator = new QWhale.Editor.LineSeparator(this);
            this.lineStyles = new EditLineStyles(this);
            this.braces = new QWhale.Editor.TextSource.Braces(this);
            this.hyperText = new EditHyperText(this);
            this.outlining = new QWhale.Editor.Outlining(this);
            this.spelling = new EditSpelling(this);
            this.keyList = new QWhale.Editor.KeyList(this);
            this.macroRecords = new MacroKeyList();
            this.syntaxPaint = new EditSyntaxPaint(this.painter, this);
            this.syntaxSettings = new QWhale.Editor.Dialogs.SyntaxSettings();
            this.notifyEventArgs = new NotifyEventArgs();
            this.autoCorrectEventArgs = new AutoCorrectEventArgs();
            base.QueryContinueDrag += new QueryContinueDragEventHandler(this.QueryEndDrag);
            this.Cursor = Cursors.IBeam;
            this.Font = new Font(FontFamily.GenericMonospace, 10f);
            this.painter.Font = this.Font;
            base.SetStyle(ControlStyles.StandardDoubleClick | ControlStyles.UserMouse | ControlStyles.Selectable | ControlStyles.StandardClick | ControlStyles.Opaque | ControlStyles.UserPaint, true);
            this.BackColor = Consts.DefaultControlBackColor;
            base.Width = 100;
            base.Height = 0x60;
            TrialVersion.CheckTrialVersion();
        }

        public SyntaxEdit(IContainer container) : this()
        {
            container.Add(this);
        }

        public virtual void Assign(ISyntaxEdit source)
        {
            ISerializationInfo serializationInfo = source.SerializationInfo;
            serializationInfo.Load();
            if (source.Source == this.Source)
            {
                ((XmlSyntaxEditInfo) serializationInfo).TextSource = null;
            }
            this.SerializationInfo = serializationInfo;
        }

        public virtual bool BreakLine()
        {
            return this.Source.BreakLine();
        }

        protected void CancelDragging()
        {
            this.dragMargin = false;
            this.pages.CancelDragging();
            this.margin.CancelDragging();
        }

        public virtual bool CanFindNext()
        {
            return !this.firstSearch;
        }

        public virtual bool CanFindNextSelected()
        {
            if (!this.selection.IsEmpty)
            {
                return true;
            }
            if (this.GetTextToSearchAtCursor().Trim() != string.Empty)
            {
                this.FirstSearch = true;
                Point position = this.Position;
                this.selection.SelectWord();
                if (!this.selection.IsEmpty)
                {
                    this.selection.BeginUpdate();
                    try
                    {
                        this.Position = position;
                    }
                    finally
                    {
                        this.selection.EndUpdate();
                    }
                }
            }
            return !this.selection.IsEmpty;
        }

        public virtual bool CanFindPrevious()
        {
            return !this.firstSearch;
        }

        public virtual bool CanFindPreviousSelected()
        {
            if (!this.selection.IsEmpty)
            {
                return true;
            }
            if (this.GetTextToSearchAtCursor().Trim() != string.Empty)
            {
                this.FirstSearch = true;
                Point position = this.Position;
                this.selection.SelectWord();
                if (!this.selection.IsEmpty)
                {
                    this.selection.BeginUpdate();
                    try
                    {
                        this.Position = position;
                    }
                    finally
                    {
                        this.selection.EndUpdate();
                    }
                }
            }
            return !this.selection.IsEmpty;
        }

        public virtual bool CanSearchSelection(out string selectedText)
        {
            bool flag = !this.selection.IsEmpty;
            selectedText = flag ? this.selection.SelectedText : string.Empty;
            return flag;
        }

        protected bool CanSplit(bool vert)
        {
            Splitter splitter = this.GetSplitter(vert);
            if (!base.IsHandleCreated || (this.Dock != DockStyle.Fill))
            {
                return false;
            }
            if (splitter != null)
            {
                return !splitter.Visible;
            }
            return true;
        }

        public virtual bool CanSplitHorz()
        {
            return this.CanSplit(false);
        }

        public virtual bool CanSplitVert()
        {
            return this.CanSplit(true);
        }

        protected bool CanUnsplit(bool vert)
        {
            Splitter splitter = this.GetSplitter(vert);
            return ((splitter != null) && splitter.Visible);
        }

        public virtual bool CanUnsplitHorz()
        {
            return this.CanUnsplit(false);
        }

        public virtual bool CanUnsplitVert()
        {
            return this.CanUnsplit(true);
        }

        protected virtual void CheckAutoCorrect()
        {
            if (this.lastKey != '\0')
            {
                if (this.autoCorrection && (Array.IndexOf<char>(this.autoCorrectDelimiters, this.lastKey) >= 0))
                {
                    int num;
                    int num2;
                    Point prevPosition = this.Source.PrevPosition;
                    string s = this.Lines[prevPosition.Y];
                    if (this.Lines.GetWord(s, Math.Max(prevPosition.X - 1, 0), out num, out num2, this.Source.SpellTable))
                    {
                        this.CheckAutoCorrect(new Point(num, prevPosition.Y), s.Substring(num, (num2 - num) + 1));
                    }
                }
                this.lastKey = '\0';
            }
        }

        protected virtual void CheckAutoCorrect(Point pos, string word)
        {
            string str;
            if (((word != string.Empty) && !this.spelling.IsWordCorrect(word)) && this.HasAutoCorrection(word, out str))
            {
                this.DoAutoCorrection(pos, word, str);
            }
        }

        private bool CheckCursor(Point pt)
        {
            Cursor arrow = null;
            IHitTestInfo hitTestInfo = new QWhale.Editor.HitTestInfo();
            this.GetHitTest(pt, hitTestInfo);
            if ((((hitTestInfo.HitTest & HitTest.Left) != HitTest.None) || ((hitTestInfo.HitTest & HitTest.Right) != HitTest.None)) || (((hitTestInfo.HitTest & HitTest.Above) != HitTest.None) || ((hitTestInfo.HitTest & HitTest.Below) != HitTest.None)))
            {
                arrow = Cursors.Arrow;
            }
            else if ((hitTestInfo.HitTest & HitTest.Gutter) != HitTest.None)
            {
                if ((((hitTestInfo.HitTest & HitTest.OutlineArea) != HitTest.None) && ((hitTestInfo.HitTest & HitTest.OutlineImage) == HitTest.None)) || (((hitTestInfo.HitTest & HitTest.LineNumber) != HitTest.None) && ((this.gutter.Options & GutterOptions.PaintLinesOnGutter) == GutterOptions.None)))
                {
                    arrow = this.LeftArrowCursor;
                }
                else
                {
                    arrow = Cursors.Arrow;
                }
            }
            else if ((((hitTestInfo.HitTest & HitTest.Margin) != HitTest.None) && this.margin.AllowDrag) && ((Control.ModifierKeys & Keys.Control) != Keys.None))
            {
                arrow = Cursors.SizeWE;
            }
            else if (((hitTestInfo.HitTest & HitTest.HyperText) != HitTest.None) && ((Control.ModifierKeys & Keys.Control) != Keys.None))
            {
                arrow = Cursors.Hand;
            }
            else if (this.InIncrementalSearch)
            {
                if ((this.searchOptions & QWhale.Editor.TextSource.SearchOptions.BackwardSearch) != QWhale.Editor.TextSource.SearchOptions.None)
                {
                    arrow = this.ReverseIncrementalSearchCursor;
                }
                else
                {
                    arrow = this.IncrementalSearchCursor;
                }
            }
            else if ((hitTestInfo.HitTest & HitTest.Selection) != HitTest.None)
            {
                arrow = Cursors.Arrow;
            }
            else if ((hitTestInfo.HitTest & HitTest.PageWhiteSpace) != HitTest.None)
            {
                if (this.pages.DisplayWhiteSpace)
                {
                    arrow = this.HideWhiteSpaceCursor;
                }
                else
                {
                    arrow = this.ShowWhiteSpaceCursor;
                }
            }
            if (arrow != null)
            {
                OSUtils.SetCursor(arrow.Handle);
            }
            return (arrow != null);
        }

        private void CheckIncrementalSeacrh()
        {
            if (!this.incrSearchFlag)
            {
                this.FinishIncrementalSearch();
            }
        }

        protected void ClearSelection()
        {
            if (((this.selection.UpdateCount == 0) && !this.selection.IsEmpty) && ((this.selection.Options & SelectionOptions.PersistentBlocks) == SelectionOptions.None))
            {
                this.selection.Clear();
            }
        }

        protected void CloseCodeCompletionBox(object sender, ClosingEventArgs e)
        {
            if ((e.Accepted && (e.Text != null)) && (e.Text != string.Empty))
            {
                if (e.Provider is ICodeSnippets)
                {
                    if ((e.Provider.SelIndex >= 0) && (e.Provider.SelIndex < e.Provider.Count))
                    {
                        this.InsertCodeSnippet(((ICodeSnippets) e.Provider)[e.Provider.SelIndex], e.StartPosition, e.EndPosition, e.UseIndent);
                    }
                }
                else
                {
                    this.InsertTextFromProvider(e.Provider, e.Text, e.StartPosition, e.EndPosition, e.UseIndent, e.UseFormat);
                }
            }
        }

        protected void CloseCodeCompletionHint(object sender, ClosingEventArgs e)
        {
            if ((e.Accepted && (e.Text != null)) && (e.Text != string.Empty))
            {
                this.InsertTextFromProvider(e.Provider, e.Text, e.StartPosition, e.EndPosition, e.UseIndent, e.UseFormat);
            }
        }

        protected void ClosePopupWindow()
        {
            if ((this.codeCompletionBox != null) && this.codeCompletionBox.Visible)
            {
                this.codeCompletionBox.Close(false);
            }
        }

        public virtual void CodeCompletion(CodeCompletionArgs e)
        {
            if (this.Lexer is ISyntaxParser)
            {
                this.DoCodeCompletion((ISyntaxParser) this.Lexer, this.Position, e);
            }
            if (this.NeedCodeCompletion != null)
            {
                this.NeedCodeCompletion(this, e);
            }
        }

        protected bool CodeCompletion(char ch, byte style, bool isValidText)
        {
            this.DisableCodeCompletionTimer();
            bool flag = false;
            if (this.Source.NeedCodeCompletion() && ((this.codeCompletionBox == null) || !this.codeCompletionBox.Visible))
            {
                ISyntaxParser lexer = this.Lexer as ISyntaxParser;
                int defaultCompletionStartDelay = EditConsts.DefaultCompletionStartDelay;
                if (lexer.IsCodeCompletionChar(ch, style, ref defaultCompletionStartDelay))
                {
                    this.codeCompletionArgs.Init(CodeCompletionType.None, this.Position);
                    this.codeCompletionArgs.KeyChar = ch;
                    this.codeCompletionArgs.Interval = defaultCompletionStartDelay;
                    this.DoCodeCompletion();
                    flag = true;
                }
            }
            if (!isValidText || (Array.IndexOf<char>(this.codeCompletionChars, ch) < 0))
            {
                return flag;
            }
            if (!flag)
            {
                this.codeCompletionArgs.Init(CodeCompletionType.None, this.Position);
                this.codeCompletionArgs.KeyChar = ch;
                this.DoCodeCompletion();
            }
            return true;
        }

        public virtual bool CodeCompletionWindowFocused(out Control control)
        {
            if ((this.codeCompletionBox != null) && this.codeCompletionBox.IsFocused())
            {
                control = (Control) this.codeCompletionBox;
                return true;
            }
            if ((this.codeCompletionHint != null) && this.codeCompletionHint.IsFocused())
            {
                control = (Control) this.codeCompletionHint;
                return true;
            }
            control = null;
            return false;
        }

        public virtual void CodeSnippets()
        {
            this.CodeSnippets(this.codeCompletionArgs);
            this.OnNeedCompletion(this.codeCompletionArgs);
        }

        protected virtual void CodeSnippets(CodeCompletionArgs e)
        {
            e.Init(CodeCompletionType.CodeSnippets, this.Position);
            if (this.Source.NeedCodeCompletion())
            {
                this.DoCodeCompletion(this.Lexer as ISyntaxParser, this.Position, e);
            }
        }

        public virtual void CompleteWord()
        {
            if (this.IsValidText(this.Position))
            {
                this.ListMembers(this.codeCompletionArgs, CodeCompletionType.CompleteWord);
                this.OnNeedCompletion(this.codeCompletionArgs);
            }
        }

        protected void CompleteWord(CodeCompletionArgs e)
        {
            this.ListMembers(e, CodeCompletionType.CompleteWord);
        }

        public virtual void CreateCaret()
        {
            if (!this.hideCaret && base.IsHandleCreated)
            {
                Size caretSize = this.GetCaretSize(this.Position);
                OSUtils.CreateCaret(base.Handle, caretSize.Width, caretSize.Height);
                OSUtils.ShowCaret(base.Handle);
            }
        }

        protected virtual ICodeCompletionBox CreateCodeCompletionBox()
        {
            return new QWhale.Editor.CodeCompletion.CodeCompletionBox(this);
        }

        protected virtual ICodeCompletionHint CreateCodeCompletionHint()
        {
            return new QWhale.Editor.CodeCompletion.CodeCompletionHint(this);
        }

        protected override void CreateHandle()
        {
            base.CreateHandle();
            this.scrolling.UpdateFlat();
            this.scrolling.UpdateScroll(true);
            this.pages.DisplayRulers();
        }

        protected virtual ISyntaxEdit CreateSplitEdit(bool vert)
        {
            ISyntaxEdit edit = new SyntaxEdit {
                Visible = false,
                Parent = base.Parent
            };
            edit.BringToFront();
            edit.Source = this.Source;
            edit.Location = base.Location;
            edit.Dock = vert ? DockStyle.Left : DockStyle.Top;
            if (vert)
            {
                edit.Width = 0;
            }
            else
            {
                edit.Height = 0;
            }
            this.components.Add((Component) edit);
            if (vert)
            {
                this.vertSplitEdit = edit;
                return edit;
            }
            this.horzSplitEdit = edit;
            return edit;
        }

        protected virtual Splitter CreateSplitter(bool vert)
        {
            Splitter component = new Splitter {
                Visible = false,
                Parent = base.Parent
            };
            component.BringToFront();
            component.Dock = vert ? DockStyle.Left : DockStyle.Top;
            component.Location = base.Location;
            component.MinSize = 0;
            component.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            component.SplitterMoved += new SplitterEventHandler(this.SplitterMoved);
            this.components.Add(component);
            if (vert)
            {
                this.vertSplitter = component;
                return component;
            }
            this.horzSplitter = component;
            return component;
        }

        public virtual bool DeleteBlock(Rectangle rect)
        {
            return this.Source.DeleteBlock(rect);
        }

        public virtual bool DeleteLeft(int len)
        {
            return this.Source.DeleteLeft(len);
        }

        public virtual bool DeleteRight(int len)
        {
            return this.Source.DeleteRight(len);
        }

        public virtual void DestroyCaret()
        {
            OSUtils.DestroyCaret();
        }

        private void DisableCodeCompletionTimer()
        {
            if (this.codeCompletionTimer != null)
            {
                this.codeCompletionTimer.Enabled = false;
            }
        }

        public virtual void DisablePositionUpdate()
        {
            this.Source.DisablePositionUpdate();
        }

        protected void DisplayCaretNowhere()
        {
            OSUtils.SetCaretPos(this.hideCaretPoint.X, this.hideCaretPoint.Y);
        }

        public virtual void DisplayDragCaret()
        {
            if (!this.dragCaret)
            {
                if ((!this.hideCaret && base.IsHandleCreated) && this.IsFocused)
                {
                    this.DisplayCaretNowhere();
                }
                this.dragCaret = true;
                this.UpdateCaret();
            }
        }

        public virtual DialogResult DisplayEditorSettingsDialog()
        {
            return this.DisplayEditorSettingsDialog(EditorSettingsTab.None, null);
        }

        public virtual DialogResult DisplayEditorSettingsDialog(EditorSettingsTab hiddenTabs)
        {
            return this.DisplayEditorSettingsDialog(hiddenTabs, null);
        }

        public virtual DialogResult DisplayEditorSettingsDialog(EditorSettingsTab hiddenTabs, IWin32Window owner)
        {
            if (this.EditorSettingsDialog == null)
            {
                return DialogResult.None;
            }
            this.syntaxSettings.LoadFromEdit(this);
            this.EditorSettingsDialog.SyntaxSettings = this.syntaxSettings;
            DialogResult result = this.EditorSettingsDialog.Execute(hiddenTabs, owner);
            if (result == DialogResult.OK)
            {
                this.syntaxSettings.Assign(this.EditorSettingsDialog.SyntaxSettings);
                this.syntaxSettings.ApplyToEdit(this);
            }
            return result;
        }

        public virtual DialogResult DisplayGotoLineDialog()
        {
            return this.DisplayGotoLineDialog(null);
        }

        public virtual DialogResult DisplayGotoLineDialog(IWin32Window owner)
        {
            int y = this.Position.Y;
            DialogResult result = (this.GotoLineDialog != null) ? this.GotoLineDialog.Execute(this, this.Lines.Count, ref y, owner) : DialogResult.None;
            if (result == DialogResult.OK)
            {
                this.Source.BeginUpdate();
                try
                {
                    ITextSource source = this.Source;
                    source.State |= NotifyState.CenterLine;
                    this.MoveToLine(y);
                }
                finally
                {
                    this.Source.EndUpdate();
                }
            }
            return result;
        }

        public virtual DialogResult DisplayReplaceDialog()
        {
            return this.DisplayReplaceDialog(null);
        }

        public virtual DialogResult DisplayReplaceDialog(IWin32Window owner)
        {
            if (this.SearchDialog == null)
            {
                return DialogResult.None;
            }
            return this.SearchDialog.Execute(this, false, true, owner);
        }

        public virtual DialogResult DisplaySearchDialog()
        {
            return this.DisplaySearchDialog(null);
        }

        public virtual DialogResult DisplaySearchDialog(IWin32Window owner)
        {
            if (this.SearchDialog == null)
            {
                return DialogResult.None;
            }
            return this.SearchDialog.Execute(this, false, false, owner);
        }

        public virtual Point DisplayToScreen(int x, int y)
        {
            return this.DisplayToScreen(x, y, false);
        }

        public virtual Point DisplayToScreen(int x, int y, bool average)
        {
            int num2;
            int num = average ? (this.painter.FontWidth * x) : this.syntaxPaint.MeasureLine(y, 0, x);
            if (this.pages.PageType == PageType.PageLayout)
            {
                IEditPage pageAt = this.pages.GetPageAt(x, y);
                Rectangle clientRect = pageAt.ClientRect;
                if (num != 0x7fffffff)
                {
                    num += clientRect.Left + this.gutter.DisplayWidth;
                }
                num2 = (y - pageAt.StartLine) * this.painter.FontHeight;
                num2 += clientRect.Top;
            }
            else
            {
                Rectangle rectangle2 = this.ClientRect;
                if (this.scrolling.ScrollByPixels)
                {
                    num2 = ((y * this.painter.FontHeight) - this.scrolling.WindowOriginY) + rectangle2.Top;
                    if (num != 0x7fffffff)
                    {
                        num = ((num - this.scrolling.WindowOriginX) + this.gutter.DisplayWidth) + rectangle2.Left;
                    }
                }
                else
                {
                    num2 = ((y - this.scrolling.WindowOriginY) * this.painter.FontHeight) + rectangle2.Top;
                    if (num != 0x7fffffff)
                    {
                        num = ((num - (this.painter.FontWidth * this.scrolling.WindowOriginX)) + this.gutter.DisplayWidth) + rectangle2.Left;
                    }
                }
            }
            return new Point(num, num2);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        protected void DisposeCodeCompletionBox(object sender, EventArgs e)
        {
            if (sender == this.codeCompletionBox)
            {
                this.codeCompletionBox = null;
            }
            else if (sender == this.codeCompletionHint)
            {
                this.codeCompletionHint = null;
            }
        }

        protected virtual void DoAutoCorrection(Point pos, string word, string correctWord)
        {
            this.Source.BeginUpdate(UpdateReason.Insert);
            int index = this.Source.StorePosition(this.Position);
            try
            {
                this.Source.Position = pos;
                this.Source.DeleteRight(word.Length);
                this.Source.Insert(correctWord);
            }
            finally
            {
                this.Position = this.Source.RestorePosition(index);
                this.Source.EndUpdate();
            }
        }

        protected void DoCodeCompletion()
        {
            this.DisableCodeCompletionTimer();
            if (this.codeCompletionArgs.Interval == 0)
            {
                this.OnCodeCompletion(this, EventArgs.Empty);
            }
            else
            {
                this.CodeCompletionTimer.Interval = this.codeCompletionArgs.Interval;
                this.CodeCompletionTimer.Enabled = true;
            }
        }

        protected void DoCodeCompletion(ISyntaxParser parser, Point position, CodeCompletionArgs e)
        {
            IStringItem item = this.Lines.GetItem(position.Y);
            if (item != null)
            {
                parser.Strings = this.Lines;
                parser.CodeCompletion(item.String, item.TextData, position, e);
            }
        }

        protected void DoCodeToolTip(string s, int x, int y, bool useHtmlFormatting)
        {
            this.codeCompletionArgs.CompletionType = CodeCompletionType.QuickInfo;
            this.codeCompletionArgs.Interval = EditConsts.DefaultHintDelay;
            this.codeCompletionArgs.NeedShow = true;
            this.codeCompletionArgs.ToolTip = true;
            this.codeCompletionArgs.Provider = this.GetQuickInfo(s, useHtmlFormatting);
            this.codeCompletionArgs.DisplayPosition = this.ScreenToText(x, y);
            this.DoCodeCompletion();
        }

        protected void DoCopy(object sender, EventArgs e)
        {
            if (this.Selection.CanCopy())
            {
                this.Selection.Copy();
            }
        }

        protected void DoCut(object sender, EventArgs e)
        {
            if (this.Selection.CanCut())
            {
                this.Selection.Cut();
            }
        }

        protected void DoDelete(object sender, EventArgs e)
        {
            this.Selection.DeleteRight();
        }

        protected virtual bool DoFind(string str, QWhale.Editor.TextSource.SearchOptions options, Regex expression, bool silent)
        {
            bool flag = ((options & QWhale.Editor.TextSource.SearchOptions.SelectionOnly) != QWhale.Editor.TextSource.SearchOptions.None) && (!this.Selection.IsEmpty || (!this.FirstSearch && (this.searchSelType != SelectionType.None)));
            if (flag && !this.FirstSearch)
            {
                this.searchUpdateCount++;
                try
                {
                    this.Selection.SetSelection(this.searchSelType, this.searchSelRect);
                }
                finally
                {
                    this.searchUpdateCount--;
                }
            }
            if (flag && this.Selection.IsEmpty)
            {
                return false;
            }
            this.searchMatch = null;
            if (this.FirstSearch)
            {
                this.searchText = str;
                this.searchOptions = options;
                this.searchExpression = expression;
                this.searchSelType = flag ? this.Selection.SelectionType : SelectionType.None;
                this.searchSelRect = flag ? this.Selection.SelectionRect : Rectangle.Empty;
                this.searchStartPos = this.Position;
                this.searchCycled = false;
                this.InitSearchPos(options, false);
            }
            else if (!this.selection.IsEmpty && (this.selection.UpdateCount == 0))
            {
                if ((options & QWhale.Editor.TextSource.SearchOptions.BackwardSearch) == QWhale.Editor.TextSource.SearchOptions.None)
                {
                    if (flag)
                    {
                        this.searchPos = this.Source.Position;
                    }
                    else
                    {
                        this.searchPos = new Point(this.selection.SelectionRect.Left, this.selection.SelectionRect.Top);
                        if (this.searchPos.X < this.Lines.GetLength(this.searchPos.Y))
                        {
                            this.searchPos.X++;
                        }
                        else
                        {
                            this.searchPos.X = 0;
                            this.searchPos.Y++;
                        }
                    }
                }
                else if (flag)
                {
                    this.searchPos = this.Source.Position;
                }
                else
                {
                    this.searchPos = new Point(this.selection.SelectionRect.Right, this.selection.SelectionRect.Bottom);
                    if (this.searchPos.X > 0)
                    {
                        this.searchPos.X--;
                    }
                    else
                    {
                        this.searchPos.Y--;
                        this.searchPos.X = this.Lines.GetLength(this.searchPos.Y);
                    }
                }
            }
            else
            {
                this.searchPos = this.Source.Position;
            }
            bool flag2 = this.PerformCycledSearch(str, options, expression, ref this.searchPos);
            if (!flag2 && !this.searchCycled)
            {
                flag2 = this.InitCycledSearch(options) && this.PerformCycledSearch(str, options, expression, ref this.searchPos);
            }
            if (flag2)
            {
                this.TextFound(options, this.searchPos, this.searchLen, silent, (expression != null) && ((expression.Options & RegexOptions.Multiline) != RegexOptions.None));
                this.FirstSearch = false;
                return flag2;
            }
            if (this.searchCycled)
            {
                this.searchStartPos = this.Position;
                this.searchCycled = false;
            }
            return flag2;
        }

        protected void DoFontChanged()
        {
            if (!this.gutter.InvalidateLineNumberArea() || !this.WordWrap)
            {
                this.UpdateWordWrap();
            }
            this.painter.Clear();
            this.painter.Font = this.Font;
            this.UpdateMonospaced();
            this.UpdateView();
        }

        protected int DoMarkAll(string str, QWhale.Editor.TextSource.SearchOptions options, Regex expr, bool clearPrevious)
        {
            this.FirstSearch = true;
            int num = 0;
            IList<IBookMark> list = new List<IBookMark>();
            this.Selection.BeginUpdate();
            this.Source.BeginUpdate(UpdateReason.Other);
            try
            {
                if (clearPrevious)
                {
                    this.Source.BookMarks.ClearAllUnnumberedBookmarks();
                }
                while (this.DoFind(str, options, expr, true))
                {
                    bool flag = false;
                    if (this.Source.BookMarks.GetBookMarks(new Point(0, this.searchPos.Y), new Point(0x7fffffff, this.searchPos.Y), list) > 0)
                    {
                        foreach (IBookMark mark in list)
                        {
                            if (mark.Index == 0x7fffffff)
                            {
                                flag = true;
                                break;
                            }
                        }
                    }
                    if (this.searchLen == 0)
                    {
                        if ((options & QWhale.Editor.TextSource.SearchOptions.BackwardSearch) == QWhale.Editor.TextSource.SearchOptions.None)
                        {
                            if (this.searchPos.X < this.Lines.GetLength(this.searchPos.Y))
                            {
                                this.searchPos.X++;
                            }
                            else
                            {
                                if (this.searchPos.Y == (this.Lines.Count - 1))
                                {
                                    return num;
                                }
                                this.searchPos.X = 0;
                                this.searchPos.Y++;
                            }
                        }
                        else if (this.searchPos.X > 0)
                        {
                            this.searchPos.X--;
                        }
                        else
                        {
                            if (this.searchPos.Y == 0)
                            {
                                return num;
                            }
                            this.searchPos.Y--;
                            this.searchPos.X = this.Lines.GetLength(this.searchPos.Y);
                        }
                        this.Source.Position = this.searchPos;
                    }
                    if (!flag)
                    {
                        this.Source.BookMarks.SetBookMark(new Point(0, this.searchPos.Y), 0x7fffffff);
                    }
                    num++;
                }
            }
            finally
            {
                this.Source.EndUpdate();
                this.Selection.EndUpdate();
            }
            return num;
        }

        protected void DoPaste(object sender, EventArgs e)
        {
            if (this.Selection.CanPaste())
            {
                this.Selection.Paste();
            }
        }

        protected virtual DialogResult DoPromptOnReplace(string text, ref bool yesToAll)
        {
            DialogResult yes = DialogResult.Yes;
            PromptReplaceEventArgs e = new PromptReplaceEventArgs(text, false, yes, yesToAll) {
                Text = text,
                Handled = false,
                DialogResult = DialogResult.Yes,
                YesToAll = false
            };
            if (this.PromptOnReplace != null)
            {
                this.PromptOnReplace(this, e);
                if (e.Handled)
                {
                    yes = e.DialogResult;
                    yesToAll = e.YesToAll;
                }
            }
            if (!e.Handled)
            {
                yes = MessageBox.Show(string.Format(StringConsts.ConfirmReplaceText, text), StringConsts.ConfirmReplace, MessageBoxButtons.YesNoCancel);
            }
            return yes;
        }

        protected void DoRedo(object sender, EventArgs e)
        {
            if (this.Source.CanRedo())
            {
                this.Source.Redo();
            }
        }

        protected DialogResult DoReplace(string text, string replaceWith, QWhale.Editor.TextSource.SearchOptions options, Match match)
        {
            bool yesToAll = false;
            return this.DoReplace(text, replaceWith, options, match, ref yesToAll);
        }

        protected virtual DialogResult DoReplace(string text, string replaceWith, QWhale.Editor.TextSource.SearchOptions options, Match match, ref bool yesToAll)
        {
            DialogResult yes = DialogResult.Yes;
            if ((QWhale.Editor.TextSource.SearchOptions.PromptOnReplace & options) != QWhale.Editor.TextSource.SearchOptions.None)
            {
                yes = this.DoPromptOnReplace(text, ref yesToAll);
            }
            if (yes == DialogResult.Yes)
            {
                this.searchUpdateCount++;
                this.Source.BeginUpdate(UpdateReason.Insert);
                try
                {
                    this.Selection.Delete();
                    Point position = this.Position;
                    string replaceString = this.GetReplaceString(replaceWith, match);
                    if ((replaceString.IndexOf('\r') >= 0) || (replaceString.IndexOf('\n') >= 0))
                    {
                        this.Source.InsertBlock(replaceString);
                    }
                    else if ((replaceString.IndexOf(@"\r") >= 0) || (replaceString.IndexOf(@"\n") >= 0))
                    {
                        string[] strings = replaceString.Replace(@"\r\n", @"\n").Replace(@"\r", @"\n").Split(new string[] { @"\n" }, StringSplitOptions.None);
                        this.Source.InsertBlock(strings);
                    }
                    else
                    {
                        this.Source.Insert(replaceString);
                    }
                    if ((QWhale.Editor.TextSource.SearchOptions.BackwardSearch & options) != QWhale.Editor.TextSource.SearchOptions.None)
                    {
                        this.Position = position;
                    }
                }
                finally
                {
                    this.Source.EndUpdate();
                    this.searchUpdateCount--;
                }
            }
            return yes;
        }

        protected bool DoReplace(string str, string replaceWith, QWhale.Editor.TextSource.SearchOptions options, Regex expr, bool silent)
        {
            return ((!this.Source.Readonly && this.DoFind(str, options, expr, silent)) && (this.DoReplace(str, replaceWith, options, this.searchMatch) == DialogResult.Yes));
        }

        protected bool DoReplaceAll(string str, string replaceWith, QWhale.Editor.TextSource.SearchOptions options, Regex expr, out int count)
        {
            bool flag;
            return this.DoReplaceAll(str, replaceWith, options, expr, out count, out flag);
        }

        protected bool DoReplaceAll(string str, string replaceWith, QWhale.Editor.TextSource.SearchOptions options, Regex expr, out int count, out bool abort)
        {
            abort = false;
            this.FirstSearch = true;
            count = 0;
            bool flag = false;
            bool flag2 = (options & QWhale.Editor.TextSource.SearchOptions.SelectionOnly) != QWhale.Editor.TextSource.SearchOptions.None;
            bool silent = (options & QWhale.Editor.TextSource.SearchOptions.PromptOnReplace) == QWhale.Editor.TextSource.SearchOptions.None;
            this.searchUpdateCount++;
            if (silent)
            {
                this.selection.Invalidate();
                this.Source.BeginUpdate(UpdateReason.Other);
            }
            int p = this.StorePositionWithUndo(this.Position);
            try
            {
                bool yesToAll = false;
                while (this.DoFind(str, options, expr, silent))
                {
                    flag = true;
                    if (this.Readonly)
                    {
                        return flag;
                    }
                    int index = this.StorePosition(this.searchStartPos);
                    try
                    {
                        DialogResult result = this.DoReplace(str, replaceWith, yesToAll ? ((options & ~QWhale.Editor.TextSource.SearchOptions.CycledSearch) & ~QWhale.Editor.TextSource.SearchOptions.PromptOnReplace) : (options & ~QWhale.Editor.TextSource.SearchOptions.CycledSearch), this.searchMatch, ref yesToAll);
                        if (this.searchLen == 0)
                        {
                            if ((options & QWhale.Editor.TextSource.SearchOptions.BackwardSearch) == QWhale.Editor.TextSource.SearchOptions.None)
                            {
                                if (this.searchPos.X < this.Lines.GetLength(this.searchPos.Y))
                                {
                                    this.searchPos.X += replaceWith.Length + 1;
                                }
                                else
                                {
                                    if (this.searchPos.Y == (this.Lines.Count - 1))
                                    {
                                        return flag;
                                    }
                                    this.searchPos.X += replaceWith.Length;
                                    this.searchPos.Y++;
                                }
                            }
                            else if (this.searchPos.X > replaceWith.Length)
                            {
                                this.searchPos.X -= replaceWith.Length + 1;
                            }
                            else
                            {
                                if (this.searchPos.Y == 0)
                                {
                                    return flag;
                                }
                                this.searchPos.Y--;
                                this.searchPos.X = this.Lines.GetLength(this.searchPos.Y) - replaceWith.Length;
                            }
                            this.Source.Position = this.searchPos;
                        }
                        switch (result)
                        {
                            case DialogResult.Cancel:
                                abort = true;
                                return flag;

                            case DialogResult.Yes:
                                count++;
                                break;
                        }
                        continue;
                    }
                    finally
                    {
                        this.searchStartPos = this.RestorePosition(index);
                    }
                }
            }
            finally
            {
                this.RestorePositionWithUndo(p);
                if (silent)
                {
                    this.Source.EndUpdate();
                }
                if (flag2)
                {
                    this.Selection.SetSelection(this.searchSelType, this.searchSelRect);
                }
                else
                {
                    this.Selection.Clear();
                }
                this.searchUpdateCount--;
            }
            return flag;
        }

        protected void DoSelectAll(object sender, EventArgs e)
        {
            this.Selection.SelectAll();
        }

        protected void DoTripleClick(object sender, EventArgs e)
        {
            this.StopTripleClickTimer();
        }

        protected void DoUndo(object sender, EventArgs e)
        {
            if (this.Source.CanUndo())
            {
                this.Source.Undo();
            }
        }

        public virtual void EnablePositionUpdate()
        {
            this.Source.EnablePositionUpdate();
        }

        ~SyntaxEdit()
        {
            this.Source = null;
        }

        public virtual bool Find(string text)
        {
            return this.Find(text, QWhale.Editor.TextSource.SearchOptions.None, null);
        }

        public virtual bool Find(string text, QWhale.Editor.TextSource.SearchOptions options)
        {
            return this.Find(text, options, null);
        }

        public virtual bool Find(string text, QWhale.Editor.TextSource.SearchOptions options, Regex expression)
        {
            this.FirstSearch = true;
            return this.DoFind(text, options, expression, false);
        }

        public virtual bool Find(string text, QWhale.Editor.TextSource.SearchOptions options, Regex expression, IList<IRange> ranges)
        {
            int num;
            Match match;
            Point position = new Point(0, 0);
            if ((options & QWhale.Editor.TextSource.SearchOptions.SelectionOnly) == QWhale.Editor.TextSource.SearchOptions.None)
            {
                while (this.Find(text, options, expression, ref position, out num, out match))
                {
                    ranges.Add(new Range(position.X, position.Y, position.X + num, position.Y));
                    if ((QWhale.Editor.TextSource.SearchOptions.BackwardSearch & options) == QWhale.Editor.TextSource.SearchOptions.None)
                    {
                        position.X += num;
                    }
                }
            }
            else if (!this.Selection.IsEmpty)
            {
                if ((QWhale.Editor.TextSource.SearchOptions.BackwardSearch & options) != QWhale.Editor.TextSource.SearchOptions.None)
                {
                    position.Offset(this.Selection.SelectionRect.Width, this.Selection.SelectionRect.Height);
                }
                ITextStrings strings = new TextStrings(null) {
                    Text = this.Selection.SelectedText
                };
                Point point2 = position;
                while (strings.Find(text, options, expression, ref position, out num, out match))
                {
                    point2 = this.Selection.SelectionToTextPoint(position);
                    ranges.Add(new Range(point2.X, point2.Y, point2.X + num, point2.Y));
                    if ((QWhale.Editor.TextSource.SearchOptions.BackwardSearch & options) == QWhale.Editor.TextSource.SearchOptions.None)
                    {
                        position.X += num;
                    }
                }
            }
            return (ranges.Count > 0);
        }

        public virtual bool Find(string s, QWhale.Editor.TextSource.SearchOptions options, Regex expression, ref Point position, out int len, out Match match)
        {
            return this.Source.Lines.Find(s, options, expression, ref position, out len, out match);
        }

        public virtual bool FindNext()
        {
            return (this.CanFindNext() && this.DoFind(this.searchText, this.searchOptions & ~QWhale.Editor.TextSource.SearchOptions.BackwardSearch, this.searchExpression, false));
        }

        public virtual bool FindNextSelected()
        {
            return (this.CanFindNextSelected() && this.DoFind(this.selection.SelectedText, this.searchOptions & ~QWhale.Editor.TextSource.SearchOptions.BackwardSearch, this.searchExpression, false));
        }

        public virtual bool FindPrevious()
        {
            return (this.CanFindPrevious() && this.DoFind(this.searchText, this.searchOptions | QWhale.Editor.TextSource.SearchOptions.BackwardSearch, this.searchExpression, false));
        }

        public virtual bool FindPreviousSelected()
        {
            return (this.CanFindPreviousSelected() && this.DoFind(this.selection.SelectedText, this.searchOptions | QWhale.Editor.TextSource.SearchOptions.BackwardSearch, this.searchExpression, false));
        }

        public virtual void FinishIncrementalSearch()
        {
            if (this.inIncrementalSearch)
            {
                this.inIncrementalSearch = false;
                this.Source.BeginUpdate(UpdateReason.Other);
                try
                {
                    ITextSource source = this.Source;
                    source.State |= NotifyState.IncrementalSearchChanged;
                }
                finally
                {
                    this.Source.EndUpdate();
                }
                if (base.IsHandleCreated)
                {
                    OSUtils.SendMessage(base.Handle, 0x20, IntPtr.Zero, IntPtr.Zero);
                }
            }
        }

        public virtual Size GetCaretSize(Point position)
        {
            if (this.Source.Overwrite)
            {
                return new Size(this.painter.FontWidth, this.painter.FontHeight);
            }
            return new Size(EditConsts.DefaultCaretWidth, this.painter.FontHeight);
        }

        public virtual int GetCharsInWidth(int width)
        {
            int fontWidth = this.painter.FontWidth;
            if (fontWidth == 0)
            {
                return 0;
            }
            return (width / fontWidth);
        }

        protected int GetCharsInWidth(int width, bool exact)
        {
            int fontWidth = this.painter.FontWidth;
            if (fontWidth == 0)
            {
                return 0;
            }
            int num2 = width / fontWidth;
            if (!exact && ((width % fontWidth) != 0))
            {
                num2++;
            }
            return num2;
        }

        protected Rectangle GetClientRect(bool excludeNonClient)
        {
            Rectangle clientRectangle = base.ClientRectangle;
            if (!excludeNonClient || (this.pages.PageType != PageType.PageLayout))
            {
                if ((this.pages.Rulers & EditRulers.Horizonal) != EditRulers.None)
                {
                    int height = this.pages.HorzRuler.Height;
                    clientRectangle.Y += height;
                    clientRectangle.Height -= height;
                }
                if ((this.pages.Rulers & EditRulers.Vertical) != EditRulers.None)
                {
                    int width = this.pages.VertRuler.Width;
                    clientRectangle.X += width;
                    clientRectangle.Width -= width;
                }
            }
            if (this.scrolling.HasVScrollBar)
            {
                clientRectangle.Width -= this.scrolling.VScrollBar.Width;
            }
            if (this.scrolling.HasHScrollBar)
            {
                clientRectangle.Height -= this.scrolling.HScrollBar.Height;
            }
            return clientRectangle;
        }

        protected int GetClientWidth(bool charWidth)
        {
            return (this.GetClientRect(true).Width - (charWidth ? this.gutter.DisplayArea : this.gutter.DisplayWidth));
        }

        public virtual void GetHitTest(Point position, IHitTestInfo hitTestInfo)
        {
            this.GetHitTest(position.X, position.Y, hitTestInfo);
        }

        public virtual void GetHitTest(int x, int y, IHitTestInfo hitTestInfo)
        {
            Rectangle clientRect = this.ClientRect;
            if (x < clientRect.Left)
            {
                hitTestInfo.HitTest |= HitTest.Left;
            }
            if (x > clientRect.Right)
            {
                hitTestInfo.HitTest |= HitTest.Right;
            }
            if (y < clientRect.Top)
            {
                hitTestInfo.HitTest |= HitTest.Above;
            }
            if (y > clientRect.Bottom)
            {
                hitTestInfo.HitTest |= HitTest.Below;
            }
            if (this.pages.PageType != PageType.Normal)
            {
                hitTestInfo.Page = this.Pages.GetPageIndexAtPoint(x, y);
                if (hitTestInfo.Page >= 0)
                {
                    hitTestInfo.HitTest |= HitTest.Page;
                    EditPage page = (EditPage) this.Pages[hitTestInfo.Page];
                    if (page.WhiteSpaceTopRect.Contains(x, y) || page.WhiteSpaceBottomRect.Contains(x, y))
                    {
                        hitTestInfo.HitTest |= HitTest.PageWhiteSpace;
                    }
                    if (this.margin.Visible && this.margin.Contains(x, y))
                    {
                        hitTestInfo.HitTest |= HitTest.Margin;
                    }
                }
            }
            else if (this.margin.Visible && this.margin.Contains(x, y))
            {
                hitTestInfo.HitTest |= HitTest.Margin;
            }
            if ((hitTestInfo.HitTest & HitTest.PageWhiteSpace) == HitTest.None)
            {
                this.gutter.GetHitTest(x, y, hitTestInfo);
                if ((hitTestInfo.HitTest & HitTest.Gutter) == HitTest.None)
                {
                    if (this.ScreenToDisplay(x, y).Y >= this.displayLines.Count)
                    {
                        hitTestInfo.HitTest |= HitTest.BeyondEof;
                    }
                    else
                    {
                        this.GetHitTestAtTextPoint(this.ScreenToText(x, y), hitTestInfo);
                    }
                }
            }
        }

        public virtual void GetHitTestAtTextPoint(Point position, IHitTestInfo hitTestInfo)
        {
            this.GetHitTestAtTextPoint(position.X, position.Y, hitTestInfo);
        }

        public virtual void GetHitTestAtTextPoint(int x, int y, IHitTestInfo hitTestInfo)
        {
            if ((y >= 0) && (y < this.Lines.Count))
            {
                hitTestInfo.Item = this.Lines.GetItem(y);
                hitTestInfo.String = this.Lines[y];
                hitTestInfo.Line = y;
                hitTestInfo.Pos = x;
                if ((x >= 0) && (x < hitTestInfo.String.Length))
                {
                    string str;
                    hitTestInfo.HitTest |= HitTest.Text;
                    hitTestInfo.Style = (byte) hitTestInfo.Item.TextData[x];
                    hitTestInfo.TextStyle = hitTestInfo.Item.TextStyleAt(x);
                    hitTestInfo.Word = this.Lines.GetTextAt(x, y);
                    if (this.hyperText.IsUrlAtTextPoint(x, y, out str))
                    {
                        hitTestInfo.HitTest |= HitTest.HyperText;
                        hitTestInfo.Url = str;
                    }
                }
                else
                {
                    hitTestInfo.HitTest |= HitTest.BeyondEol;
                }
            }
            else
            {
                hitTestInfo.HitTest |= HitTest.BeyondEof;
            }
            if (this.selection.IsPosInSelection(x, y))
            {
                hitTestInfo.HitTest |= HitTest.Selection;
            }
        }

        public virtual int GetLinesInHeight(int height)
        {
            int fontHeight = this.painter.FontHeight;
            if (fontHeight == 0)
            {
                return 0;
            }
            return (height / fontHeight);
        }

        private ICodeCompletionProvider GetQuickInfo(string text, bool useHtmlFormatting)
        {
            return new QWhale.Syntax.CodeCompletion.QuickInfo { Text = text, UseHtmlFormatting = useHtmlFormatting };
        }

        protected string GetReplaceString(string replaceWith, Match match)
        {
            string str = replaceWith;
            if ((match != null) && match.Success)
            {
                str = match.Result(replaceWith);
            }
            return str;
        }

        protected ISyntaxEdit GetSplitEdit(bool vert)
        {
            if (!vert)
            {
                return this.horzSplitEdit;
            }
            return this.vertSplitEdit;
        }

        protected Splitter GetSplitter(bool vert)
        {
            if (!vert)
            {
                return this.horzSplitter;
            }
            return this.vertSplitter;
        }

        public virtual string GetTextAtCursor()
        {
            return this.Lines.GetTextAt(this.Position);
        }

        public virtual string GetTextToSearchAtCursor()
        {
            string s = this.Lines[this.Position.Y];
            if (((this.Position.X > 0) && (this.Position.X < s.Length)) && (this.Lines.IsDelimiter(s, this.Position.X) && !this.Lines.IsDelimiter(s, this.Position.X - 1)))
            {
                return this.Lines.GetTextAt(this.Position.X - 1, this.Position.Y);
            }
            return this.Lines.GetTextAt(this.Position);
        }

        public virtual int GetWrapMargin()
        {
            return this.displayLines.GetWrapMargin();
        }

        public virtual bool HasAutoCorrection(string word, out string correctWord)
        {
            if (this.AutoCorrect != null)
            {
                this.autoCorrectEventArgs.Word = word;
                this.autoCorrectEventArgs.CorrectWord = word;
                this.autoCorrectEventArgs.HasCorrection = false;
                this.AutoCorrect(this, this.autoCorrectEventArgs);
                correctWord = this.autoCorrectEventArgs.CorrectWord;
                return this.autoCorrectEventArgs.HasCorrection;
            }
            correctWord = word;
            return false;
        }

        public virtual void HideDragCaret()
        {
            if (this.dragCaret)
            {
                this.PaintDragCaret(this.oldDragPoint, true);
                this.oldDragPoint = this.hideCaretPoint;
                this.dragCaret = false;
                this.UpdateCaret();
            }
        }

        public virtual void HideScrollHint()
        {
            if ((this.codeCompletionHint != null) && this.codeCompletionHint.Visible)
            {
                this.codeCompletionHint.Close(false);
            }
        }

        public virtual bool IncrementalSearch(string key, bool deleteLast)
        {
            bool flag = false;
            string searchText = this.searchText;
            this.incrSearchFlag = true;
            try
            {
                if (deleteLast)
                {
                    this.searchText = this.searchText.Remove(this.searchText.Length - 1, 1);
                }
                else
                {
                    this.searchText = this.searchText + key;
                }
                this.searchPos = this.incrStartSearchPosition;
                flag = this.PerformCycledSearch(this.searchText, this.searchOptions, this.searchExpression, ref this.searchPos);
                if (!flag && !this.searchCycled)
                {
                    flag = this.InitCycledSearch(this.searchOptions);
                    if (flag)
                    {
                        this.incrStartSearchPosition = this.searchPos;
                        flag = this.PerformCycledSearch(this.searchText, this.searchOptions, this.searchExpression, ref this.searchPos);
                    }
                }
                if (flag)
                {
                    Point searchPos = this.searchPos;
                    int searchLen = this.searchLen;
                    this.Source.BeginUpdate(UpdateReason.Other);
                    try
                    {
                        flag = true;
                        this.Position = new Point(searchPos.X + searchLen, searchPos.Y);
                        ITextSource source = this.Source;
                        source.State |= NotifyState.IncrementalSearchChanged;
                    }
                    finally
                    {
                        this.Source.EndUpdate();
                    }
                    this.FirstSearch = false;
                    this.selection.SetSelection(SelectionType.Stream, new Rectangle(searchPos.X, searchPos.Y, searchLen, 0));
                    return flag;
                }
                if (deleteLast)
                {
                    if (this.searchText == string.Empty)
                    {
                        this.selection.Clear();
                    }
                    this.Source.BeginUpdate(UpdateReason.Other);
                    try
                    {
                        if (this.searchText == string.Empty)
                        {
                            this.MoveTo(this.incrSearchPosition);
                        }
                        else
                        {
                            this.selection.SetSelection(SelectionType.Stream, new Rectangle(this.Position.X, this.Position.Y, 0, 0));
                        }
                        ITextSource source2 = this.Source;
                        source2.State |= NotifyState.IncrementalSearchChanged;
                        return flag;
                    }
                    finally
                    {
                        this.Source.EndUpdate();
                    }
                }
                this.searchText = searchText;
            }
            finally
            {
                this.incrSearchFlag = false;
            }
            return flag;
        }

        public virtual void IndentLine()
        {
            this.Source.IndentLine();
        }

        protected bool InitCycledSearch(QWhale.Editor.TextSource.SearchOptions options)
        {
            if ((options & QWhale.Editor.TextSource.SearchOptions.CycledSearch) != QWhale.Editor.TextSource.SearchOptions.None)
            {
                this.InitSearchPos(options | QWhale.Editor.TextSource.SearchOptions.EntireScope, false);
                this.searchCycled = true;
                return true;
            }
            return false;
        }

        protected virtual void InitDefaultMenu()
        {
            this.defaultMenu = new ContextMenuStrip();
            this.miUndo = new ToolStripMenuItem(StringConsts.MenuUndoCaption, null, new EventHandler(this.DoUndo));
            this.miRedo = new ToolStripMenuItem(StringConsts.MenuRedoCaption, null, new EventHandler(this.DoRedo));
            this.miCut = new ToolStripMenuItem(StringConsts.MenuCutCaption, null, new EventHandler(this.DoCut));
            this.miCopy = new ToolStripMenuItem(StringConsts.MenuCopyCaption, null, new EventHandler(this.DoCopy));
            this.miPaste = new ToolStripMenuItem(StringConsts.MenuPasteCaption, null, new EventHandler(this.DoPaste));
            this.miDelete = new ToolStripMenuItem(StringConsts.MenuDeleteCaption, null, new EventHandler(this.DoDelete));
            this.miSelectAll = new ToolStripMenuItem(StringConsts.MenuSelectAllCaption, null, new EventHandler(this.DoSelectAll));
            this.defaultMenu.Items.Add(this.miUndo);
            this.defaultMenu.Items.Add(this.miRedo);
            this.defaultMenu.Items.Add("-");
            this.defaultMenu.Items.Add(this.miCut);
            this.defaultMenu.Items.Add(this.miCopy);
            this.defaultMenu.Items.Add(this.miPaste);
            this.defaultMenu.Items.Add(this.miDelete);
            this.defaultMenu.Items.Add("-");
            this.defaultMenu.Items.Add(this.miSelectAll);
        }

        protected void InitSearchPos(QWhale.Editor.TextSource.SearchOptions options, bool update)
        {
            if ((options & QWhale.Editor.TextSource.SearchOptions.EntireScope) != QWhale.Editor.TextSource.SearchOptions.None)
            {
                if ((options & QWhale.Editor.TextSource.SearchOptions.BackwardSearch) != QWhale.Editor.TextSource.SearchOptions.None)
                {
                    if (((options & QWhale.Editor.TextSource.SearchOptions.SelectionOnly) != QWhale.Editor.TextSource.SearchOptions.None) && !this.Selection.IsEmpty)
                    {
                        this.searchPos = new Point(this.Selection.SelectionRect.Right, this.Selection.SelectionRect.Bottom);
                    }
                    else
                    {
                        this.searchPos = new Point(this.Lines[this.Lines.Count - 1].Length, this.Lines.Count - 1);
                    }
                }
                else if (((options & QWhale.Editor.TextSource.SearchOptions.SelectionOnly) != QWhale.Editor.TextSource.SearchOptions.None) && !this.Selection.IsEmpty)
                {
                    this.searchPos = this.Selection.SelectionRect.Location;
                }
                else
                {
                    this.searchPos = new Point(0, 0);
                }
            }
            else if ((update && ((options & QWhale.Editor.TextSource.SearchOptions.SelectionOnly) != QWhale.Editor.TextSource.SearchOptions.None)) && !this.Selection.IsEmpty)
            {
                if ((options & QWhale.Editor.TextSource.SearchOptions.BackwardSearch) != QWhale.Editor.TextSource.SearchOptions.None)
                {
                    this.searchPos = new Point(this.Selection.SelectionRect.Right, this.Selection.SelectionRect.Bottom);
                }
                else
                {
                    this.searchPos = this.Selection.SelectionRect.Location;
                }
            }
            else
            {
                this.searchPos = this.Source.Position;
            }
        }

        public virtual bool Insert(string text)
        {
            return this.Source.Insert(text);
        }

        public virtual bool InsertBlock(string[] strings)
        {
            return this.Source.InsertBlock(strings);
        }

        public virtual bool InsertBlock(ITextStrings strings)
        {
            return this.Source.InsertBlock(strings);
        }

        public virtual bool InsertBlock(string text)
        {
            return this.Source.InsertBlock(text);
        }

        public virtual bool InsertBlock(string[] strings, bool select)
        {
            return this.Source.InsertBlock(strings, select);
        }

        protected void InsertCodeSnippet(ICodeSnippet snippet, Point startPos, Point endPos, bool useIndent)
        {
            this.selection.BeginUpdate();
            this.Source.BeginUpdate(UpdateReason.InsertBlock);
            try
            {
                Point point;
                if (!this.selection.IsEmpty)
                {
                    this.selection.SelectionType = SelectionType.Stream;
                }
                string str = this.selection.IsEmpty ? string.Empty : this.selection.SelectedText;
                if (!this.selection.IsEmpty)
                {
                    int index = startPos.Equals(this.selection.SelectionRect.Location) ? -1 : this.Source.StorePosition(startPos);
                    int num2 = endPos.Equals(this.selection.SelectionRect.Location) ? -1 : this.Source.StorePosition(endPos);
                    try
                    {
                        this.selection.Delete();
                    }
                    finally
                    {
                        if (num2 >= 0)
                        {
                            endPos = this.Source.RestorePosition(num2);
                        }
                        if (index >= 0)
                        {
                            startPos = this.Source.RestorePosition(index);
                        }
                    }
                }
                IList<ICodeSnippetLiteral> list = new List<ICodeSnippetLiteral>();
                ICodeSnippetRanges ranges = new CodeSnippetRanges();
                foreach (ICodeSnippetDeclaration declaration in snippet.Declarations)
                {
                    foreach (ICodeSnippetLiteral literal in declaration.Literals)
                    {
                        list.Add(literal);
                    }
                    foreach (ICodeSnippetObject obj2 in declaration.Objects)
                    {
                        list.Add(obj2);
                    }
                }
                string code = snippet.Code.Code;
                string delimiter = snippet.Code.Delimiter;
                Regex regex = new Regex(@"\" + delimiter + EditConsts.DefaultSnippetPattern + @"\" + delimiter, RegexOptions.Multiline);
                bool flag = false;
                int num3 = 0;
                foreach (Match match in regex.Matches(code))
                {
                    if (!match.Success)
                    {
                        continue;
                    }
                    ICodeSnippetLiteral literal2 = null;
                    bool flag2 = match.Value == (delimiter + EditConsts.DefaultSnippetSelectedPattern + delimiter);
                    bool flag3 = match.Value == (delimiter + EditConsts.DefaultSnippetEndPattern + delimiter);
                    flag |= flag3;
                    if (!flag2)
                    {
                        foreach (ICodeSnippetLiteral literal3 in list)
                        {
                            if (match.Value == (delimiter + literal3.ID + delimiter))
                            {
                                literal2 = literal3;
                                break;
                            }
                        }
                    }
                    string str4 = flag3 ? SyntaxConsts.DefaultCaretSymbol.ToString() : (flag2 ? str : ((literal2 != null) ? literal2.Default : string.Empty));
                    code = code.Remove(match.Index - num3, match.Length).Insert(match.Index - num3, str4);
                    if ((literal2 != null) && literal2.Editable)
                    {
                        ranges.Add(new CodeSnippetRange(new Point(match.Index - num3, 0), new Point((match.Index - num3) + literal2.Default.Length, 0), literal2.ID, literal2.ToolTip, literal2.ID != string.Empty));
                    }
                    else if (flag3)
                    {
                        ranges.Add(new CodeSnippetRange(new Point(match.Index - num3, 0), new Point(match.Index - num3, 0), "$" + EditConsts.DefaultSnippetEndPattern + "$", string.Empty, false));
                    }
                    num3 += match.Length - str4.Length;
                }
                if (ranges.Count != 0)
                {
                    ranges.Add(new CodeSnippetRange(new Point(code.Length, 0), new Point(code.Length, 0), string.Empty, string.Empty, false));
                }
                startPos.X = Math.Min(startPos.X, this.Lines.GetLength(startPos.Y));
                this.InsertTextFromProvider(null, code, startPos, endPos, useIndent, out point, str == string.Empty, false);
                ITextSource source = this.Source;
                foreach (IRange range in ranges)
                {
                    range.StartPoint = this.PointToBlockPoint(range.StartPoint.X, startPos, code);
                    range.EndPoint = this.PointToBlockPoint(range.EndPoint.X, startPos, code);
                }
                source.CodeSnippets = ranges;
                source.BeginUpdateSnippet();
                try
                {
                    this.selection.SetSelection(SelectionType.Stream, startPos, this.Position);
                    this.selection.SmartFormat();
                    this.selection.Clear();
                }
                finally
                {
                    source.EndUpdateSnippet();
                }
                if ((!this.SelectFirstSnippet() && flag) && ((point.X >= 0) && (point.Y >= 0)))
                {
                    this.MoveTo(point);
                }
            }
            finally
            {
                this.Source.EndUpdate();
                this.selection.EndUpdate();
            }
        }

        public virtual bool InsertFromFile(string fileName)
        {
            return this.Source.InsertFromFile(fileName);
        }

        protected void InsertTextFromProvider(ICodeCompletionProvider provider, string text, Point startPos, Point endPos, bool useIndent, bool format)
        {
            this.selection.BeginUpdate();
            this.Source.BeginUpdate(UpdateReason.Insert);
            try
            {
                Point point;
                this.InsertTextFromProvider(provider, text, startPos, endPos, useIndent, out point, true, format);
                if ((point.X >= 0) && (point.Y >= 0))
                {
                    this.MoveTo(point);
                }
            }
            finally
            {
                this.Source.EndUpdate();
                this.selection.EndUpdate();
            }
        }

        protected void InsertTextFromProvider(ICodeCompletionProvider provider, string text, Point startPos, Point endPos, bool useIndent, out Point curPos, bool needSelect, bool format)
        {
            ITextSource source = this.Source;
            source.BeginUpdate(UpdateReason.Insert);
            try
            {
                if ((provider != null) && provider.UseHtmlFormatting)
                {
                    text = text.Replace("&lt;", "<").Replace("&gt;", ">").Replace("&amp;", "@").Replace("&quot;", "\"").Replace("&nbsp;", " ");
                }
                this.MoveTo(startPos);
                string s = this.Lines[this.Position.Y];
                if (needSelect)
                {
                    if (((startPos.X >= 0) && (startPos.Y >= 0)) && ((endPos.X >= 0) && (endPos.Y >= 0)))
                    {
                        this.selection.SetSelection(SelectionType.Stream, startPos, endPos);
                    }
                    else if (((this.Position.X >= 0) && (this.Position.X < s.Length)) && !this.Lines.IsDelimiter(s, this.Position.X))
                    {
                        this.selection.SelectWord();
                        if (!this.selection.IsEmpty)
                        {
                            this.selection.SetSelection(this.Selection.SelectionType, startPos, new Point(this.Selection.SelectionRect.Right, this.Selection.SelectionRect.Bottom));
                        }
                    }
                }
                string[] strings = StringItem.Split(text);
                curPos = new Point(-1, -1);
                int index = strings[0].IndexOf(SyntaxConsts.DefaultCaretSymbol);
                if (index >= 0)
                {
                    curPos.X = startPos.X + index;
                    curPos.Y = startPos.Y;
                    strings[0] = strings[0].Remove(index, 1);
                }
                string str2 = (useIndent && (strings.Length > 1)) ? this.Lines.GetIndentString(this.Lines.TabPosToPos(this.Lines[this.Position.Y], startPos.X), 0) : string.Empty;
                for (int i = 1; i < strings.Length; i++)
                {
                    strings[i] = str2 + strings[i];
                    index = strings[i].IndexOf(SyntaxConsts.DefaultCaretSymbol);
                    if (index >= 0)
                    {
                        strings[i] = strings[i].Remove(index, 1);
                        curPos.X = index;
                        curPos.Y = this.Position.Y + i;
                    }
                }
                if (!this.selection.IsEmpty)
                {
                    this.selection.Delete();
                }
                Point position = source.Position;
                source.InsertBlock(strings);
                if (format)
                {
                    int num3 = source.StorePosition(curPos);
                    try
                    {
                        this.selection.SetSelection(SelectionType.Stream, position, this.Position);
                        this.selection.SmartFormat();
                    }
                    finally
                    {
                        curPos = source.RestorePosition(num3);
                    }
                }
                this.selection.Clear();
            }
            finally
            {
                source.EndUpdate();
            }
        }

        protected void InternalProcessKey(char ch)
        {
            byte style = this.TextStyleAt(new Point(Math.Max(this.Position.X - 1, 0), this.Position.Y));
            bool isValidText = (style == 0) || this.IsValidText((byte) (style - 1));
            if ((!this.CodeCompletion(ch, style, isValidText) && isValidText) && !this.Selection.SmartFormat(ch))
            {
                this.Selection.SmartIndent(ch);
            }
        }

        private void InvalidateLine(int index)
        {
            if (index >= 0)
            {
                Point point = this.DisplayToScreen(0, index);
                int displayWidth = this.gutter.DisplayWidth;
                int right = this.ClientRect.Right;
                base.Invalidate(new Rectangle(displayWidth, point.Y - 1, (right - displayWidth) + 1, this.painter.FontHeight + 2));
            }
        }

        private void InvalidateWindow(int first, int last, bool invalidateGutter)
        {
            Point point2;
            Point position = this.TextToScreen(new Point(0, first), false);
            if (last == 0x7fffffff)
            {
                point2 = new Point(this.ClientRect.Right, this.ClientRect.Bottom);
            }
            else
            {
                point2 = this.TextToScreen(new Point(0x7fffffff, last), true);
                point2.Y += this.painter.FontHeight;
            }
            if (this.Pages.PageType == PageType.PageLayout)
            {
                position.X = this.Pages.GetPageAtPoint(position).BoundsRect.Left;
                point2.X = this.Pages.GetPageAtPoint(point2).BoundsRect.Right;
            }
            else if (invalidateGutter && (this.gutter.DisplayWidth > 0))
            {
                position.X = this.ClientRect.Left;
                point2.X = this.ClientRect.Right;
            }
            point2.X = this.ClientRect.Right;
            if (invalidateGutter)
            {
                if (position.Y > 0)
                {
                    position.Y--;
                }
                point2.Y++;
            }
            base.Invalidate(new Rectangle(position.X, position.Y, (point2.X - position.X) + 1, (point2.Y - position.Y) + 1));
        }

        protected override bool IsInputChar(char charCode)
        {
            return ((charCode != '\t') && base.IsInputChar(charCode));
        }

        protected override bool IsInputKey(Keys keyData)
        {
            Keys keys = keyData & Keys.KeyCode;
            switch (keys)
            {
                case Keys.Enter:
                    return this.AcceptReturns;

                case Keys.Tab:
                    return this.AcceptTabs;
            }
            return ((Array.IndexOf<Keys>(EditConsts.NavKeys, keys) >= 0) || base.IsInputKey(keyData));
        }

        protected bool IsMouseOnPageWhiteSpace(int x, int y)
        {
            if (this.pages.PageType == PageType.Normal)
            {
                return false;
            }
            IEditPage pageAtPoint = this.Pages.GetPageAtPoint(x, y);
            if (pageAtPoint == null)
            {
                return false;
            }
            if (!pageAtPoint.WhiteSpaceTopRect.Contains(x, y))
            {
                return pageAtPoint.WhiteSpaceBottomRect.Contains(x, y);
            }
            return true;
        }

        protected bool IsMouseOnSyntaxError(int x, int y, out ISyntaxError err)
        {
            err = null;
            Point point = this.ScreenToText(x, y);
            if ((point.Y >= 0) && (point.Y < this.Lines.Count))
            {
                IStringItem item = this.Lines.GetItem(point.Y);
                if (point.X >= 0)
                {
                    TextStyle none = TextStyle.None;
                    if (point.X < item.String.Length)
                    {
                        none = item.TextStyleAt(point.X);
                    }
                    else if (point.X == item.String.Length)
                    {
                        none = TextStyle.WaveLine;
                    }
                    if ((none & TextStyle.WaveLine) != TextStyle.None)
                    {
                        err = this.Source.GetSyntaxErrorAt(point.X, point.Y);
                    }
                }
            }
            return (err != null);
        }

        protected bool IsSignificantMouseMove(Point pt1, Point pt2)
        {
            if (Math.Abs((int) (pt1.X - pt2.X)) <= 1)
            {
                return (Math.Abs((int) (pt1.Y - pt2.Y)) > 1);
            }
            return true;
        }

        protected virtual bool IsValidText(byte style)
        {
            return ((this.Lexer == null) || !this.Lexer.Scheme.IsPlainText(style));
        }

        public virtual bool IsValidText(Point position)
        {
            byte num = this.TextStyleAt(position);
            return ((num <= 0) || this.IsValidText((byte) (num - 1)));
        }

        public virtual bool LineIsReadonly(int index)
        {
            return this.Source.LineIsReadonly(index);
        }

        public virtual void ListMembers()
        {
            if (this.IsValidText(this.Position))
            {
                this.ListMembers(this.codeCompletionArgs, CodeCompletionType.ListMembers);
                this.OnNeedCompletion(this.codeCompletionArgs);
            }
        }

        protected void ListMembers(CodeCompletionArgs e)
        {
            this.ListMembers(e, CodeCompletionType.ListMembers);
        }

        protected virtual void ListMembers(CodeCompletionArgs e, CodeCompletionType completionType)
        {
            e.Init(completionType, this.Position);
            if (this.Source.NeedCodeCompletion())
            {
                this.DoCodeCompletion(this.Lexer as ISyntaxParser, this.Position, e);
            }
        }

        public virtual bool LoadFile(string fileName)
        {
            return this.Lines.LoadFile(fileName);
        }

        public virtual bool LoadFile(string fileName, IStringImport importer)
        {
            return this.Lines.LoadFile(fileName, importer);
        }

        public virtual bool LoadFile(string fileName, Encoding encoding)
        {
            return this.Lines.LoadFile(fileName, encoding);
        }

        public virtual bool LoadFile(string fileName, IStringImport importer, Encoding encoding)
        {
            return this.Lines.LoadFile(fileName, importer, encoding);
        }

        public virtual void LoadMacros(Stream stream)
        {
            StreamReader reader = new StreamReader(stream);
            try
            {
                this.LoadMacros(reader);
            }
            finally
            {
                reader.Close();
            }
        }

        public virtual void LoadMacros(TextReader reader)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(XmlMacroKeysDataInfo));
            try
            {
                this.MacroRecords.SerializationInfo = (ISerializationInfo) serializer.Deserialize(reader);
                this.MacroRecords.LinkMacros(this, this.KeyList.Handlers);
            }
            catch (Exception exception)
            {
                ErrorHandler.Error(exception);
            }
        }

        public virtual void LoadMacros(string fileName)
        {
            Stream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            try
            {
                this.LoadMacros(stream);
            }
            finally
            {
                stream.Close();
            }
        }

        public virtual bool LoadStream(Stream stream)
        {
            return this.Lines.LoadStream(stream);
        }

        public virtual bool LoadStream(TextReader reader)
        {
            return this.Lines.LoadStream(reader);
        }

        public virtual bool LoadStream(Stream stream, IStringImport importer)
        {
            return this.Lines.LoadStream(stream, importer);
        }

        public virtual bool LoadStream(Stream stream, Encoding encoding)
        {
            return this.Lines.LoadStream(stream, encoding);
        }

        public virtual bool LoadStream(TextReader reader, IStringImport importer)
        {
            return this.Lines.LoadStream(reader, importer);
        }

        public virtual bool LoadStream(Stream stream, IStringImport importer, Encoding encoding)
        {
            return this.Lines.LoadStream(stream, importer, encoding);
        }

        public virtual void MakeVisible(Point position)
        {
            this.MakeVisible(position, false);
        }

        public virtual void MakeVisible(Point position, bool centerLine)
        {
            this.Outlining.EnsureExpanded(position);
            this.ScrollTo(position, centerLine);
        }

        public virtual int MarkAll(string text, bool clearPrevious)
        {
            return this.DoMarkAll(text, QWhale.Editor.TextSource.SearchOptions.None, null, clearPrevious);
        }

        public virtual int MarkAll(string text, QWhale.Editor.TextSource.SearchOptions options, bool clearPrevious)
        {
            return this.DoMarkAll(text, options, null, clearPrevious);
        }

        public virtual int MarkAll(string text, QWhale.Editor.TextSource.SearchOptions options, Regex expression, bool clearPrevious)
        {
            return this.DoMarkAll(text, options, expression, clearPrevious);
        }

        public virtual void MoveCaretOnDrag()
        {
            Point pt = base.PointToClient(Cursor.Position);
            if (!this.selection.ScrollIfNeeded(pt))
            {
                this.selection.BeginUpdate();
                this.displayLines.DisableUpdate();
                try
                {
                    bool lineEnd = false;
                    pt = this.ScreenToText(pt.X, pt.Y, ref lineEnd);
                    this.displayLines.LineEnd = lineEnd;
                    this.Position = pt;
                }
                finally
                {
                    this.displayLines.EnableUpdate();
                    this.selection.EndUpdate();
                }
            }
        }

        public virtual void MoveCharLeft()
        {
            bool flag = false;
            Point point = this.displayLines.PointToDisplayPoint(this.Position);
            if ((point.X == 0) && ((QWhale.Editor.TextSource.NavigateOptions.UpAtLineBegin & this.NavigateOptions) != QWhale.Editor.TextSource.NavigateOptions.None))
            {
                point.X = this.displayLines[point.Y - 1].Length;
                point.Y--;
                this.displayLines.LineEnd = true;
                flag = true;
            }
            else if (point.X > 0)
            {
                point.X--;
            }
            if (flag)
            {
                this.displayLines.DisableUpdate();
                this.Source.BeginUpdate(UpdateReason.Navigate);
                try
                {
                    ITextSource source = this.Source;
                    source.State |= NotifyState.PositionChanged;
                    this.MoveTo(this.displayLines.DisplayPointToPoint(point.X, point.Y, true, true, false));
                }
                finally
                {
                    this.Source.EndUpdate();
                    this.displayLines.EnableUpdate();
                }
            }
            else
            {
                this.MoveTo(this.displayLines.DisplayPointToPoint(point.X, point.Y, true, true, false));
            }
            this.ClearSelection();
        }

        public virtual void MoveCharRight()
        {
            bool flag = false;
            Point point = this.displayLines.PointToDisplayPoint(this.Position);
            if ((point.X >= this.displayLines[point.Y].Length) && ((this.NavigateOptions & QWhale.Editor.TextSource.NavigateOptions.BeyondEol) == QWhale.Editor.TextSource.NavigateOptions.None))
            {
                if (((this.NavigateOptions & QWhale.Editor.TextSource.NavigateOptions.DownAtLineEnd) != QWhale.Editor.TextSource.NavigateOptions.None) && (((this.NavigateOptions & QWhale.Editor.TextSource.NavigateOptions.BeyondEof) != QWhale.Editor.TextSource.NavigateOptions.None) || (point.Y < (this.displayLines.DisplayCount - 1))))
                {
                    point.Y++;
                    point.X = 0;
                    this.displayLines.LineEnd = false;
                    flag = true;
                }
            }
            else
            {
                point.X++;
            }
            if (flag)
            {
                this.Source.BeginUpdate(UpdateReason.Navigate);
                try
                {
                    ITextSource source = this.Source;
                    source.State |= NotifyState.PositionChanged;
                    this.MoveTo(this.displayLines.DisplayPointToPoint(point.X, point.Y, false, false, true));
                }
                finally
                {
                    this.Source.EndUpdate();
                }
            }
            else
            {
                this.MoveTo(this.displayLines.DisplayPointToPoint(point.X, point.Y, false, false, true));
            }
            this.ClearSelection();
        }

        public virtual void MoveFileBegin()
        {
            this.MoveTo(0, 0);
            this.ClearSelection();
        }

        public virtual void MoveFileEnd()
        {
            if (this.Lines.Count == 0)
            {
                this.MoveTo(0, 0);
            }
            else
            {
                this.MoveTo(this.Lines.GetLength(this.Lines.Count - 1), this.Lines.Count - 1);
            }
            this.ClearSelection();
        }

        public virtual void MoveLineBegin()
        {
            Point position = this.displayLines.PointToDisplayPoint(this.Position);
            position.X = 0;
            this.MoveTo(this.displayLines.DisplayPointToPoint(position));
            this.ClearSelection();
        }

        public virtual void MoveLineBeginCycled()
        {
            Point position = this.displayLines.PointToDisplayPoint(this.Position);
            string str = this.displayLines[position.Y];
            int num = str.Length - str.TrimStart(new char[0]).Length;
            position.X = (position.X == num) ? 0 : num;
            this.MoveTo(this.displayLines.DisplayPointToPoint(position));
            this.ClearSelection();
        }

        public virtual void MoveLineDown()
        {
            this.displayLines.DisableUpdate();
            try
            {
                Point point = this.displayLines.PointToDisplayPoint(this.Position);
                if (this.scrolling.ScrollByPixels)
                {
                    point.X = this.DisplayToScreen(point.X, point.Y).X;
                }
                if (this.vertNavigate)
                {
                    point.X = this.vertNavigateX;
                }
                point.Y++;
                int x = this.scrolling.ScrollByPixels ? this.ScreenToDisplayX(point.X, point.Y) : point.X;
                bool lineEnd = false;
                Point position = this.displayLines.DisplayPointToPoint(x, point.Y, ref lineEnd);
                this.displayLines.LineEnd = lineEnd;
                this.MoveTo(position);
                this.vertNavigate = true;
                this.vertNavigateX = point.X;
            }
            finally
            {
                this.displayLines.EnableUpdate();
            }
            this.ClearSelection();
        }

        public virtual void MoveLineEnd()
        {
            Point position = this.displayLines.PointToDisplayPoint(this.Position);
            position.X = this.displayLines[position.Y].Length;
            this.displayLines.DisableUpdate();
            try
            {
                this.displayLines.LineEnd = true;
                this.MoveTo(this.displayLines.DisplayPointToPoint(position));
            }
            finally
            {
                this.displayLines.EnableUpdate();
            }
            this.ClearSelection();
        }

        public virtual void MoveLineEndCycled()
        {
            Point position = this.displayLines.PointToDisplayPoint(this.Position);
            string str = this.displayLines[position.Y];
            int length = str.TrimEnd(new char[0]).Length;
            position.X = (position.X == length) ? str.Length : length;
            this.MoveTo(this.displayLines.DisplayPointToPoint(position));
            this.ClearSelection();
        }

        public virtual void MoveLineUp()
        {
            this.displayLines.DisableUpdate();
            try
            {
                Point point = this.displayLines.PointToDisplayPoint(this.Position);
                if (point.Y > 0)
                {
                    point.Y--;
                }
                if (this.scrolling.ScrollByPixels)
                {
                    point.X = this.DisplayToScreen(point.X, point.Y).X;
                }
                if (this.vertNavigate)
                {
                    point.X = this.vertNavigateX;
                }
                int x = this.scrolling.ScrollByPixels ? this.ScreenToDisplayX(point.X, point.Y) : point.X;
                bool lineEnd = false;
                Point position = this.displayLines.DisplayPointToPoint(x, point.Y, ref lineEnd);
                this.displayLines.LineEnd = lineEnd;
                this.MoveTo(position);
                this.vertNavigate = true;
                this.vertNavigateX = point.X;
            }
            finally
            {
                this.displayLines.EnableUpdate();
            }
            this.ClearSelection();
        }

        private void MovePage(bool direction)
        {
            this.displayLines.DisableUpdate();
            try
            {
                Point position = this.scrolling.ScrollByPixels ? this.TextToScreen(this.Position) : this.displayLines.PointToDisplayPoint(this.Position);
                if (this.vertNavigate)
                {
                    position.X = this.vertNavigateX;
                }
                int x = position.X;
                if (!this.scrolling.ScrollByPixels)
                {
                    position = this.DisplayToScreen(position.X, position.Y);
                }
                int num2 = this.scrolling.ScrollByPixels ? this.ClientRect.Height : Math.Max(this.LinesInHeight - 1, 0);
                if (direction)
                {
                    num2 += this.scrolling.WindowOriginY;
                }
                else
                {
                    num2 = this.scrolling.WindowOriginY - num2;
                }
                if (num2 < 0)
                {
                    this.scrolling.WindowOriginY = 0;
                    this.MoveToLine(0);
                }
                else
                {
                    this.scrolling.DisableUpdate();
                    try
                    {
                        int windowOriginY = this.scrolling.WindowOriginY;
                        this.scrolling.WindowOriginY = num2;
                        bool lineEnd = false;
                        position = this.ScreenToText(position.X, position.Y, ref lineEnd);
                        this.displayLines.LineEnd = lineEnd;
                        this.scrolling.WindowOriginY = windowOriginY;
                    }
                    finally
                    {
                        this.scrolling.EnableUpdate();
                    }
                    this.scrolling.WindowOriginY = num2;
                    this.MoveTo(position);
                }
                this.vertNavigate = true;
                this.vertNavigateX = x;
            }
            finally
            {
                this.displayLines.EnableUpdate();
            }
            this.ClearSelection();
        }

        public virtual void MovePageDown()
        {
            this.MovePage(true);
        }

        public virtual void MovePageUp()
        {
            this.MovePage(false);
        }

        public virtual void MoveScreenBottom()
        {
            this.displayLines.DisableUpdate();
            try
            {
                Point position = this.displayLines.PointToDisplayPoint(this.Position);
                int x = position.X;
                if (this.scrolling.ScrollByPixels)
                {
                    x = this.DisplayToScreen(position.X, position.Y).X;
                }
                if (this.vertNavigate)
                {
                    x = this.vertNavigateX;
                }
                Point point2 = this.scrolling.ScrollByPixels ? this.ScreenToDisplay(x, this.ClientRect.Bottom - this.painter.FontHeight) : new Point(x, this.scrolling.WindowOriginY + Math.Max(this.LinesInHeight - 1, 0));
                bool lineEnd = false;
                position = this.displayLines.DisplayPointToPoint(point2.X, point2.Y, ref lineEnd);
                this.displayLines.LineEnd = lineEnd;
                this.MoveTo(position);
                this.vertNavigate = true;
                this.vertNavigateX = x;
            }
            finally
            {
                this.displayLines.EnableUpdate();
            }
            this.ClearSelection();
        }

        public virtual void MoveScreenTop()
        {
            this.displayLines.DisableUpdate();
            try
            {
                Point position = this.displayLines.PointToDisplayPoint(this.Position);
                int x = position.X;
                if (this.scrolling.ScrollByPixels)
                {
                    x = this.DisplayToScreen(position.X, position.Y).X;
                }
                if (this.vertNavigate)
                {
                    x = this.vertNavigateX;
                }
                Point point2 = this.scrolling.ScrollByPixels ? this.ScreenToDisplay(x, this.ClientRect.Top) : new Point(x, this.scrolling.WindowOriginY);
                bool lineEnd = false;
                position = this.displayLines.DisplayPointToPoint(point2.X, point2.Y, ref lineEnd);
                this.displayLines.LineEnd = lineEnd;
                this.MoveTo(position);
                this.vertNavigate = true;
                this.vertNavigateX = x;
            }
            finally
            {
                this.displayLines.EnableUpdate();
            }
            this.ClearSelection();
        }

        public virtual void MoveTo(Point position)
        {
            this.Source.MoveTo(position);
        }

        public virtual void MoveTo(int x, int y)
        {
            this.Source.MoveTo(x, y);
        }

        public virtual void MoveToBrace()
        {
            Point position = this.Position;
            if (Array.IndexOf<char>(this.braces.OpenBraces, this.Lines.GetCharAt(position)) >= 0)
            {
                position.X++;
                if (this.braces.FindClosingBrace(ref position))
                {
                    this.Position = position;
                }
            }
            else if (Array.IndexOf<char>(this.braces.ClosingBraces, this.Lines.GetCharAt(position)) >= 0)
            {
                if (this.braces.FindOpenBrace(ref position))
                {
                    this.Position = position;
                }
            }
            else if ((position.X > 0) && (Array.IndexOf<char>(this.braces.OpenBraces, this.Lines.GetCharAt(new Point(position.X - 1, position.Y))) >= 0))
            {
                if (this.braces.FindClosingBrace(ref position))
                {
                    this.Position = position;
                }
            }
            else if ((position.X > 0) && (Array.IndexOf<char>(this.braces.ClosingBraces, this.Lines.GetCharAt(new Point(position.X - 1, position.Y))) >= 0))
            {
                position.X--;
                if (this.braces.FindOpenBrace(ref position))
                {
                    this.Position = position;
                }
            }
        }

        public virtual void MoveToChar(int x)
        {
            this.Source.MoveToChar(x);
        }

        public virtual void MoveToCloseBrace()
        {
            Point position = this.Position;
            if (Array.IndexOf<char>(this.braces.OpenBraces, this.Lines.GetCharAt(this.Position)) >= 0)
            {
                position.X++;
                if (this.braces.FindClosingBrace(ref position))
                {
                    this.Position = position;
                }
            }
        }

        public virtual void MoveToLine(int y)
        {
            this.Source.MoveToLine(y);
        }

        public virtual void MoveToLine(int y, int linesAbove)
        {
            this.Source.MoveToLine(y, linesAbove);
        }

        public virtual void MoveToOpenBrace()
        {
            Point position = this.Position;
            if ((Array.IndexOf<char>(this.braces.ClosingBraces, this.Lines.GetCharAt(position)) >= 0) && this.braces.FindOpenBrace(ref position))
            {
                this.Position = position;
            }
        }

        public virtual void MoveWordLeft()
        {
            Point point = this.displayLines.PointToDisplayPoint(this.Position);
            string s = this.displayLines[point.Y];
            if (point.X == 0)
            {
                if (point.Y > 0)
                {
                    point.Y--;
                    s = this.displayLines[point.Y];
                    point.X = s.Length;
                }
                else
                {
                    point = new Point(0, 0);
                }
            }
            int x = Math.Min(point.X, s.Length);
            if (x > 0)
            {
                bool flag = (s[x - 1] == ' ') || (s[x - 1] == '\t');
                bool flag2 = this.displayLines.IsDelimiter(s, x - 1);
                if (flag)
                {
                    while ((x > 0) && ((s[x - 1] == ' ') || (s[x - 1] == '\t')))
                    {
                        x--;
                    }
                }
                if (!flag2 || flag)
                {
                    while ((x > 0) && !this.displayLines.IsDelimiter(s, x - 1))
                    {
                        x--;
                    }
                }
                else
                {
                    x--;
                }
            }
            this.MoveTo(this.displayLines.DisplayPointToPoint(x, point.Y, false, true, false));
        }

        public virtual void MoveWordRight()
        {
            Point position = this.displayLines.PointToDisplayPoint(this.Position);
            string s = this.displayLines[position.Y];
            int length = s.Length;
            if (position.X >= length)
            {
                if ((position.Y + 1) < this.displayLines.DisplayCount)
                {
                    position.Y++;
                    s = this.displayLines[position.Y];
                    position.X = s.Length - s.TrimStart(new char[0]).Length;
                }
                else
                {
                    position.X = length;
                }
                this.MoveTo(this.displayLines.DisplayPointToPoint(position));
            }
            else
            {
                int x = position.X;
                bool flag = this.displayLines.IsDelimiter(s, x);
                while ((x < length) && (this.displayLines.IsDelimiter(s, x) == flag))
                {
                    x++;
                }
                if (!flag)
                {
                    while ((x < length) && ((s[x] == ' ') || (s[x] == '\t')))
                    {
                        x++;
                    }
                }
                if (x == position.X)
                {
                    x++;
                }
                this.MoveTo(this.displayLines.DisplayPointToPoint(x, position.Y, false, false, true));
            }
        }

        public virtual void Navigate(int deltaX, int deltaY)
        {
            this.Source.Navigate(deltaX, deltaY);
        }

        protected bool NeedImeComposition()
        {
            return (((base.IsHandleCreated && (base.ImeMode != ImeMode.Disable)) && (base.ImeMode != ImeMode.NoControl)) && (base.ImeMode != ImeMode.Off));
        }

        private bool NeedIncrementalSearch(char key)
        {
            bool inIncrementalSearch = this.inIncrementalSearch;
            if (inIncrementalSearch)
            {
                this.IncrementalSearch(key.ToString(), false);
            }
            return inIncrementalSearch;
        }

        public bool NeedReplaceCurrent()
        {
            return ((!this.firstSearch && !this.Selection.IsEmpty) && this.SelectionMatchesSearchText());
        }

        public virtual bool NeedReplaceCurrent(out Match match)
        {
            match = this.searchMatch;
            return this.NeedReplaceCurrent();
        }

        protected bool NeedResizeRedraw()
        {
            return (((this.scrolling.HasVScrollBar && this.scrolling.HasHScrollBar) && ((this.scrolling.Options & ScrollingOptions.SystemScrollbars) == ScrollingOptions.None)) || ((((this.selection.Options & SelectionOptions.DrawBorder) != SelectionOptions.None) && ((this.selection.Options & SelectionOptions.SelectBeyondEol) != SelectionOptions.None)) && (this.selection.BorderColor != Color.Empty)));
        }

        public virtual bool NewLine()
        {
            return this.Source.NewLine();
        }

        public virtual bool NewLineAbove()
        {
            return this.Source.NewLineAbove();
        }

        public virtual bool NewLineBelow()
        {
            return this.Source.NewLineBelow();
        }

        public virtual void Notification(object sender, EventArgs e)
        {
            if (sender is ITextSource)
            {
                ITextSource source = (ITextSource) sender;
                NotifyState state = source.State;
                if (e is PositionChangedEventArgs)
                {
                    PositionChangedEventArgs args = (PositionChangedEventArgs) e;
                    this.PositionChanged(args.Reason, args.DeltaX, args.DeltaY);
                }
                else if (e is BlockDeletingEventArgs)
                {
                    this.displayLines.BlockDeleting(((BlockDeletingEventArgs) e).Rect);
                }
                else
                {
                    bool flag = (state & NotifyState.BookMarkChanged) != NotifyState.None;
                    bool flag2 = (state & NotifyState.Edit) != NotifyState.None;
                    bool flag3 = (state & NotifyState.ModifiedChanged) != NotifyState.None;
                    bool flag4 = (state & NotifyState.ReadonlyChanged) != NotifyState.None;
                    if ((state & NotifyState.OverWriteChanged) != NotifyState.None)
                    {
                        this.UpdateCaretMode();
                    }
                    if ((state & NotifyState.Undo) != NotifyState.None)
                    {
                        this.ClosePopupWindow();
                    }
                    if ((flag2 || flag) || ((state & NotifyState.BlockChanged) != NotifyState.None))
                    {
                        if (flag2)
                        {
                            this.RescanLines(source.FirstChanged, source.LastChanged);
                            this.OnTextChanged(EventArgs.Empty);
                        }
                        this.InvalidateWindow(source.FirstChanged, source.LastChanged, ((flag || this.outlining.AllowOutlining) || (((this.gutter.Options & GutterOptions.PaintLineNumbers) != GutterOptions.None) || ((this.gutter.Options & GutterOptions.PaintLineModificators) != GutterOptions.None))) || ((this.gutter.Options & GutterOptions.PaintUserMargin) != GutterOptions.None));
                    }
                    if (flag4)
                    {
                        base.Invalidate();
                    }
                    else if (flag3 && ((this.gutter.Options & GutterOptions.PaintLineModificators) != GutterOptions.None))
                    {
                        base.Invalidate(this.Gutter.Rect);
                    }
                    if ((((state & NotifyState.PositionChanged) != NotifyState.None) || ((state & NotifyState.CenterLine) != NotifyState.None)) || ((state & NotifyState.CountChanged) != NotifyState.None))
                    {
                        this.CheckIncrementalSeacrh();
                        if (source.ActiveEdit == this)
                        {
                            source.CurrentSnippet = source.GetCodeSnippetRangeAt(this.Position);
                            this.lineSeparator.TempUnhighlightLine();
                            if (((((this.selection.Options & SelectionOptions.SmartFormat) != SelectionOptions.None) && source.NeedReparseTextOnLineChange()) && (((state & NotifyState.TextParsed) != NotifyState.None) && ((state & NotifyState.Undo) == NotifyState.None))) && ((source.PrevPosition.Y != source.Position.Y) || ((state & NotifyState.CountChanged) != NotifyState.None)))
                            {
                                this.selection.SmartFormat(source.PrevPosition.Y);
                            }
                            if (this.displayLines.UpdateCount == 0)
                            {
                                this.displayLines.LineEnd = false;
                            }
                            this.MakeVisible(this.Position, (state & NotifyState.CenterLine) != NotifyState.None);
                            this.UpdateCaret();
                            this.ClearSelection();
                            this.UpdateStartSearchPos();
                            this.vertNavigate = false;
                        }
                    }
                    if ((((state & NotifyState.SelectBlock) != NotifyState.None) && (source.ActiveEdit == this)) && ((this.selection.UpdateCount == 0) && ((this.selection.Options & SelectionOptions.PersistentBlocks) == SelectionOptions.None)))
                    {
                        this.selection.SetSelection(SelectionType.Stream, source.SelectBlockRect);
                    }
                    if ((state & NotifyState.CountChanged) != NotifyState.None)
                    {
                        if (this.gutter.InvalidateLineNumberArea() && !flag4)
                        {
                            base.Invalidate();
                        }
                        this.scrolling.UpdateScroll(false);
                    }
                    if ((state & NotifyState.SyntaxChanged) != NotifyState.None)
                    {
                        this.OnSyntaxChanged();
                    }
                    if ((state & NotifyState.Outline) != NotifyState.None)
                    {
                        this.OnOutlineChanged(false);
                    }
                    this.OnSourceStateChanged(state, source.FirstChanged, source.LastChanged);
                    if ((state & NotifyState.PositionChanged) != NotifyState.None)
                    {
                        this.selection.OnSelectionChanged();
                    }
                    if (flag3)
                    {
                        this.OnModifiedChanged();
                    }
                }
            }
            else if (sender == this.displayLines)
            {
                NotifyEventArgs args2 = (NotifyEventArgs) e;
                if (((args2.State & NotifyState.Outline) != NotifyState.None) && this.displayLines.IsPointVisible(this.Position))
                {
                    IRange range = null;
                    if (!this.displayLines.IsPointCollapsed(this.Position, out range))
                    {
                        this.ScrollTo(this.Position, false);
                    }
                    else if (range != null)
                    {
                        this.ScrollTo(range.StartPoint, false);
                    }
                }
                if (args2.Update || !this.WordWrap)
                {
                    this.UpdatePages(args2.FirstChanged);
                }
                if ((args2.FirstChanged == 0) && (args2.LastChanged == 0x7fffffff))
                {
                    base.Invalidate();
                }
                else
                {
                    this.InvalidateWindow(args2.FirstChanged, args2.LastChanged, (args2.State & NotifyState.Outline) != NotifyState.None);
                }
                if (args2.Update)
                {
                    this.scrolling.UpdateScroll(false);
                    this.UpdateCaret();
                }
                this.OnSourceStateChanged(args2.State, args2.FirstChanged, args2.LastChanged);
            }
        }

        protected virtual void OnAcceptReturnsChanged()
        {
        }

        protected virtual void OnAcceptTabsChanged()
        {
        }

        protected virtual void OnAutoCorrectDelimitersChanged()
        {
        }

        protected virtual void OnAutoCorrectionChanged()
        {
        }

        protected virtual void OnBorderColorChanged()
        {
            base.RecreateHandle();
        }

        protected virtual void OnBorderStyleChanged()
        {
            base.RecreateHandle();
        }

        protected virtual void OnBracesChanged()
        {
        }

        protected void OnCodeCompletion(object source, EventArgs e)
        {
            this.DisableCodeCompletionTimer();
            if (this.codeCompletionArgs.Provider != null)
            {
                if (this.codeCompletionArgs.CompletionType == CodeCompletionType.QuickInfo)
                {
                    if (this.snippetRange != null)
                    {
                        Point pt = base.PointToScreen(this.TextToScreen(this.snippetRange.StartPoint, false));
                        pt.Y += this.painter.FontHeight + EditConsts.DefaultHintOffsetY;
                        pt.X = Cursor.Position.X + EditConsts.DefaultHintOffsetX;
                        this.ShowCodeCompletionHint(this.codeCompletionArgs.Provider, pt, this.codeCompletionArgs.StartPosition, this.codeCompletionArgs.EndPosition, this.codeCompletionArgs.DisplayPosition, true, false, null);
                        return;
                    }
                    if (this.mouseRange != null)
                    {
                        Point point2 = base.PointToScreen(this.TextToScreen(this.mouseRange.StartPoint, false));
                        point2.Y += this.painter.FontHeight + EditConsts.DefaultHintOffsetY;
                        point2.X = Cursor.Position.X + EditConsts.DefaultHintOffsetX;
                        this.ShowCodeCompletionHint(this.codeCompletionArgs.Provider, point2, this.codeCompletionArgs.StartPosition, this.codeCompletionArgs.EndPosition, this.codeCompletionArgs.DisplayPosition, true, false, this.Lexer);
                        return;
                    }
                    if ((this.mouseUrl != string.Empty) && (this.mouseUrl != null))
                    {
                        Point point3 = base.PointToScreen(this.DisplayToScreen(this.mouseUrlPoint.X, this.mouseUrlPoint.Y));
                        point3.Y += this.painter.FontHeight + EditConsts.DefaultHintOffsetY;
                        point3.X = Cursor.Position.X + EditConsts.DefaultHintOffsetX;
                        this.ShowCodeCompletionHint(this.codeCompletionArgs.Provider, point3, this.codeCompletionArgs.StartPosition, this.codeCompletionArgs.EndPosition, this.codeCompletionArgs.DisplayPosition, true, false, null);
                        return;
                    }
                    if (this.mouseError != null)
                    {
                        Point point4 = base.PointToScreen(this.TextToScreen(this.mouseError.Position, false));
                        point4.Y += this.painter.FontHeight + EditConsts.DefaultHintOffsetY;
                        point4.X = Cursor.Position.X + EditConsts.DefaultHintOffsetX;
                        this.ShowCodeCompletionHint(this.codeCompletionArgs.Provider, point4, this.codeCompletionArgs.StartPosition, this.codeCompletionArgs.EndPosition, this.codeCompletionArgs.DisplayPosition, true, false, null);
                        return;
                    }
                    if (this.mouseBookMark != null)
                    {
                        Point point5 = base.PointToScreen(this.mouseBookMarkPt);
                        point5.Y += this.painter.FontHeight + EditConsts.DefaultHintOffsetY;
                        point5.X = Cursor.Position.X + EditConsts.DefaultHintOffsetX;
                        this.ShowCodeCompletionHint(this.codeCompletionArgs.Provider, point5, this.codeCompletionArgs.StartPosition, this.codeCompletionArgs.EndPosition, this.codeCompletionArgs.DisplayPosition, true, false, null);
                        return;
                    }
                    if (this.dragMargin)
                    {
                        if (!this.margin.IsDragging)
                        {
                            Point point6 = base.PointToClient(Control.MousePosition);
                            point6 = base.PointToScreen(this.DisplayToScreen(this.margin.Position, this.ScreenToDisplay(point6.X, point6.Y).Y));
                            point6.Y = Control.MousePosition.Y + EditConsts.DefaultHintOffsetY;
                            point6.X += EditConsts.DefaultHintOffsetX;
                            this.ShowCodeCompletionHint(this.codeCompletionArgs.Provider, point6, this.codeCompletionArgs.StartPosition, this.codeCompletionArgs.EndPosition, this.codeCompletionArgs.DisplayPosition, true, false, null);
                        }
                        return;
                    }
                }
                if (this.codeCompletionArgs.ToolTip)
                {
                    this.ShowCodeCompletionHint(this.codeCompletionArgs.Provider);
                }
                else
                {
                    this.ShowCodeCompletionBox(this.codeCompletionArgs.Provider);
                }
            }
            else if (this.codeCompletionArgs.KeyChar != '\0')
            {
                if (this.Source.NeedCodeCompletion())
                {
                    ISyntaxParser lexer = this.Lexer as ISyntaxParser;
                    this.DoCodeCompletion(lexer, this.Position, this.codeCompletionArgs);
                }
                this.OnNeedCompletion(this.codeCompletionArgs);
            }
        }

        protected virtual void OnCodeCompletionCharsChanged()
        {
        }

        protected virtual void OnDefaultMenuChanged()
        {
        }

        protected override void OnDragDrop(DragEventArgs drgevent)
        {
            base.OnDragDrop(drgevent);
            this.HideDragCaret();
            if (drgevent.Effect != DragDropEffects.None)
            {
                this.needStartDrag = false;
                if (this.selection.SelectionState == SelectionState.Drag)
                {
                    if (!this.selection.Move(this.Position, (Control.ModifierKeys & Keys.Control) == Keys.None))
                    {
                        this.selection.Clear();
                    }
                    this.selection.SelectionState = SelectionState.None;
                    drgevent.Effect = DragDropEffects.None;
                }
                else
                {
                    object data = drgevent.Data.GetData(DataFormats.UnicodeText);
                    if (data == null)
                    {
                        data = drgevent.Data.GetData(DataFormats.Text);
                    }
                    if (data != null)
                    {
                        if ((this.selection.Options & SelectionOptions.ClearOnDrag) != SelectionOptions.None)
                        {
                            this.selection.Delete();
                        }
                        else
                        {
                            this.selection.Clear();
                        }
                        this.selection.SelectedText = (string) data;
                    }
                }
            }
        }

        protected override void OnDragEnter(DragEventArgs drgevent)
        {
            base.OnDragEnter(drgevent);
            this.DisplayDragCaret();
        }

        protected override void OnDragLeave(EventArgs e)
        {
            base.OnDragLeave(e);
            this.HideDragCaret();
        }

        protected override void OnDragOver(DragEventArgs drgevent)
        {
            drgevent.Effect = DragDropEffects.None;
            base.OnDragOver(drgevent);
            this.DisplayDragCaret();
            if (drgevent.Effect == DragDropEffects.None)
            {
                if ((!this.Source.Readonly && (drgevent.Data != null)) && (drgevent.Data.GetDataPresent(DataFormats.Text) || drgevent.Data.GetDataPresent(DataFormats.UnicodeText)))
                {
                    if ((Control.ModifierKeys & Keys.Control) != Keys.None)
                    {
                        drgevent.Effect = DragDropEffects.Copy;
                    }
                    else
                    {
                        drgevent.Effect = DragDropEffects.Move;
                    }
                    this.MoveCaretOnDrag();
                }
                else
                {
                    drgevent.Effect = DragDropEffects.None;
                }
            }
        }

        protected virtual void OnEditMarginChanged()
        {
        }

        protected virtual void OnEditorSettingsDialogChanged()
        {
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);
            base.Invalidate();
        }

        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            this.DoFontChanged();
        }

        protected override void OnForeColorChanged(EventArgs e)
        {
            base.OnForeColorChanged(e);
        }

        protected override void OnGotFocus(EventArgs e)
        {
            this.Source.ActiveEdit = this;
            if (!this.hideCaret)
            {
                this.CreateCaret();
                this.UpdateCaret();
            }
            this.selection.UpdateSelection();
            base.OnGotFocus(e);
        }

        protected virtual void OnGotoLineDialogChanged()
        {
        }

        protected virtual void OnGutterChanged()
        {
        }

        protected virtual void OnHideCaretChanged()
        {
            if (this.hideCaret)
            {
                this.DestroyCaret();
            }
        }

        protected virtual void OnHyperTextChanged()
        {
        }

        protected virtual void OnKeepCaretOnLostFocusChanged()
        {
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.CancelDragging();
            }
            this.keyProcessed = false;
            base.OnKeyDown(e);
            if (!e.Handled)
            {
                switch (e.KeyData)
                {
                    case Keys.Tab:
                        this.lastKey = '\t';
                        break;

                    case Keys.Enter:
                        this.lastKey = '\r';
                        break;
                }
                if (this.ProcessKey(e.KeyData))
                {
                    this.keyProcessed = true;
                    e.Handled = true;
                }
            }
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            if (this.keyProcessed)
            {
                this.keyProcessed = false;
                e.Handled = true;
            }
            else
            {
                base.OnKeyPress(e);
                if (!e.Handled && ((!this.NeedIncrementalSearch(e.KeyChar) && (this.keyState == 0)) && (e.KeyChar >= ' ')))
                {
                    this.lastKey = e.KeyChar;
                    this.ProcessKeyPress(e.KeyChar);
                    this.InternalProcessKey(e.KeyChar);
                    e.Handled = true;
                }
            }
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            this.CheckAutoCorrect();
            base.OnKeyUp(e);
            this.keyProcessed = false;
        }

        protected virtual void OnLineSeparatorChanged()
        {
        }

        protected virtual void OnLineStylesChanged()
        {
        }

        protected override void OnLostFocus(EventArgs e)
        {
            if (!this.IsCodeCompletionWindowFocused)
            {
                if (!this.hideCaret && !this.keepCaretOnLostFocus)
                {
                    this.DestroyCaret();
                }
                this.UpdateSeparator();
                this.selection.UpdateSelection();
                this.CancelDragging();
                base.OnLostFocus(e);
            }
        }

        protected virtual void OnMacroRecordingChanged()
        {
            if (this.macroRecording && !this.macroSuspended)
            {
                this.macroRecords.Clear();
            }
        }

        protected virtual void OnMacroRecordsChanged()
        {
        }

        protected virtual void OnMacroSuspendendChanged()
        {
        }

        protected void OnModifiedChanged()
        {
            if (this.ModifiedChanged != null)
            {
                this.ModifiedChanged(this, EventArgs.Empty);
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (this.GetClientRect(true).Contains(new Point(e.X, e.Y)))
            {
                this.needStartDrag = false;
                IHitTestInfo hitTestInfo = new QWhale.Editor.HitTestInfo();
                this.gutter.GetHitTest(e.X, e.Y, hitTestInfo);
                bool flag = ((hitTestInfo.HitTest & HitTest.OutlineArea) != HitTest.None) || ((hitTestInfo.HitTest & HitTest.LineNumber) != HitTest.None);
                bool flag2 = flag;
                if (base.CanFocus)
                {
                    base.Focus();
                    bool lineEnd = false;
                    Point position = this.ScreenToText(e.X, e.Y, ref lineEnd);
                    bool flag4 = (e.Button != MouseButtons.Right) || ((QWhale.Editor.TextSource.NavigateOptions.MoveOnRightButton & this.NavigateOptions) != QWhale.Editor.TextSource.NavigateOptions.None);
                    bool flag5 = (e.Button == MouseButtons.Left) && ((SelectionOptions.DisableSelection & this.selection.Options) == SelectionOptions.None);
                    bool flag6 = ((e.Button == MouseButtons.Left) && (e.Clicks == 2)) && (e.X >= this.gutter.DisplayWidth);
                    if ((this.gutter.Options & GutterOptions.SelectLineOnClick) != GutterOptions.None)
                    {
                        flag2 = flag2 || ((hitTestInfo.HitTest & HitTest.Gutter) != HitTest.None);
                    }
                    if (((((SelectionOptions.SelectLineOnDblClick & this.selection.Options) == SelectionOptions.None) && ((SelectionOptions.SelectLineOnTripleClick & this.selection.Options) != SelectionOptions.None)) && ((e.Button == MouseButtons.Left) && flag5)) && (e.X >= this.gutter.DisplayWidth))
                    {
                        this.StartTripleClickTimer();
                        this.lbuttonClicks += e.Clicks;
                    }
                    bool flag7 = this.selection.SelectionState == SelectionState.None;
                    bool flag8 = flag7 && (this.selection.IsPosInSelection(position) && ((SelectionOptions.DisableDragging & this.selection.Options) == SelectionOptions.None));
                    bool flag9 = (e.Button == MouseButtons.Left) && ((hitTestInfo.HitTest & HitTest.OutlineImage) != HitTest.None);
                    if (this.IsMouseOnPageWhiteSpace(e.X, e.Y))
                    {
                        this.pages.DisplayWhiteSpace = !this.pages.DisplayWhiteSpace;
                        return;
                    }
                    if (flag9)
                    {
                        int outlineIndex = hitTestInfo.OutlineIndex;
                        if (this.outlining.IsExpanded(outlineIndex))
                        {
                            this.outlining.Collapse(outlineIndex);
                            return;
                        }
                        if (this.outlining.IsCollapsed(outlineIndex))
                        {
                            this.outlining.Expand(outlineIndex);
                            return;
                        }
                    }
                    if ((this.outlining.AllowOutlining && ((this.outlining.OutlineOptions & OutlineOptions.DrawButtons) != OutlineOptions.None)) && (((hitTestInfo.HitTest & HitTest.OutlineButton) != HitTest.None) && (hitTestInfo.OutlineRange != null)))
                    {
                        if (flag6)
                        {
                            this.selection.Clear();
                            hitTestInfo.OutlineRange.Visible = true;
                            return;
                        }
                        if (e.Button == MouseButtons.Left)
                        {
                            this.Position = position;
                            this.selection.SelectionState = SelectionState.Select;
                            this.Selection.SetSelection(SelectionType.Stream, hitTestInfo.OutlineRange.StartPoint, hitTestInfo.OutlineRange.EndPoint);
                            return;
                        }
                    }
                    if (this.dragMargin && ((Control.ModifierKeys & Keys.Control) != Keys.None))
                    {
                        this.margin.IsDragging = true;
                        return;
                    }
                    if ((flag5 && !flag6) && ((Control.ModifierKeys & Keys.Shift) != Keys.None))
                    {
                        if (flag2)
                        {
                            this.scrolling.WindowOriginX = 0;
                            this.selection.StartSelection();
                            this.selection.SelectionState = SelectionState.SelectLine;
                        }
                        else
                        {
                            this.selection.SelectionState = SelectionState.Select;
                        }
                        this.selection.OnSelect(this, null);
                    }
                    else
                    {
                        if (flag4)
                        {
                            this.displayLines.LineEnd = lineEnd;
                            this.selection.BeginUpdate();
                            this.displayLines.DisableUpdate();
                            try
                            {
                                if ((((flag5 | ((e.Button == MouseButtons.Right) && !this.selection.IsPosInSelection(position))) && !flag6) && (flag7 && !flag8)) && !flag2)
                                {
                                    this.Selection.Clear();
                                }
                                this.Position = position;
                            }
                            finally
                            {
                                this.displayLines.EnableUpdate();
                                this.selection.EndUpdate();
                            }
                        }
                        if (flag5)
                        {
                            if (this.lbuttonClicks >= 4)
                            {
                                this.selection.SelectLine();
                                this.StopTripleClickTimer();
                                this.selection.SelectionState = SelectionState.SelectWord;
                            }
                            else if (flag6 && ((this.selection.Options & SelectionOptions.DeselectOnDblClick) == SelectionOptions.None))
                            {
                                if ((SelectionOptions.SelectLineOnDblClick & this.selection.Options) != SelectionOptions.None)
                                {
                                    this.selection.SelectLine();
                                }
                                else
                                {
                                    this.selection.UpdateSelStart(false);
                                    this.selection.SelectWord();
                                    this.selection.StartSelection();
                                }
                                this.selection.SelectionState = SelectionState.SelectWord;
                            }
                            else if (flag7)
                            {
                                if (flag2)
                                {
                                    this.selection.UpdateSelStart(false);
                                    this.scrolling.WindowOriginX = 0;
                                    this.selection.SetSelection(SelectionType.Stream, new Rectangle(0, this.Position.Y, 0, 1));
                                    this.selection.StartSelection();
                                    this.selection.SelectionState = SelectionState.SelectLine;
                                }
                                else
                                {
                                    if (flag8)
                                    {
                                        this.selection.EndSelection();
                                        this.selection.SelectionState = SelectionState.Drag;
                                        this.needStartDrag = true;
                                        this.startDragPos = new Point(e.X, e.Y);
                                    }
                                    else
                                    {
                                        this.selection.Clear();
                                        this.selection.SelectionState = SelectionState.Select;
                                        if (((this.NavigateOptions & QWhale.Editor.TextSource.NavigateOptions.BeyondEof) == QWhale.Editor.TextSource.NavigateOptions.None) || (position.Y < this.Lines.Count))
                                        {
                                            this.selection.StartSelection();
                                        }
                                    }
                                    this.selection.UpdateSelStart(false);
                                }
                            }
                        }
                        else if ((e.Button == MouseButtons.Right) && (this.selection.SelectionState != SelectionState.None))
                        {
                            this.selection.EndSelection();
                        }
                    }
                }
                this.urlAtCursor = this.hyperText.IsUrlAtPoint(e.X, e.Y);
                if (!flag && ((hitTestInfo.HitTest & HitTest.Gutter) != HitTest.None))
                {
                    if (e.Clicks == 1)
                    {
                        this.gutter.OnClick(new EventArgs());
                    }
                    else
                    {
                        this.gutter.OnDoubleClick(new EventArgs());
                    }
                }
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            this.DisableCodeCompletionTimer();
            this.mouseRange = null;
            this.snippetRange = null;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if ((this.needStartDrag && ((e.Button & MouseButtons.Left) != MouseButtons.None)) && this.IsSignificantMouseMove(new Point(e.X, e.Y), this.startDragPos))
            {
                this.StartDragging();
            }
            else
            {
                int num;
                ITextSource source = this.Source;
                bool lineEnd = false;
                Point point = this.ScreenToDisplay(e.X, e.Y);
                Point position = this.displayLines.DisplayPointToPoint(point.X, point.Y, ref lineEnd);
                ICodeSnippetRange range = null;
                if (((source.CodeSnippets.Count > 0) && source.CodeSnippets.FindSnippet(position, false, out num)) && source.CodeSnippets.IsFirstSnippet(num))
                {
                    range = source.CodeSnippets[num];
                }
                if (range != this.snippetRange)
                {
                    if (this.codeCompletionHint != null)
                    {
                        this.codeCompletionHint.Close(false);
                    }
                    this.DisableCodeCompletionTimer();
                    this.snippetRange = range;
                    if (((this.snippetRange != null) && (this.snippetRange.Tooltip != null)) && (this.snippetRange.Tooltip != string.Empty))
                    {
                        this.DoCodeToolTip(this.snippetRange.Tooltip, e.X, e.Y, false);
                        return;
                    }
                }
                if (range == null)
                {
                    IHitTestInfo hitTestInfo = new QWhale.Editor.HitTestInfo();
                    this.gutter.GetHitTest(e.X, e.Y, hitTestInfo);
                    if (this.outlining.AllowOutlining && ((OutlineOptions.ShowHints & this.Outlining.OutlineOptions) != OutlineOptions.None))
                    {
                        IOutlineRange outlineRange = null;
                        if ((hitTestInfo.HitTest & HitTest.OutlineButton) != HitTest.None)
                        {
                            outlineRange = hitTestInfo.OutlineRange;
                        }
                        if (this.mouseRange != outlineRange)
                        {
                            if (this.codeCompletionHint != null)
                            {
                                this.codeCompletionHint.Close(false);
                            }
                            this.DisableCodeCompletionTimer();
                            this.mouseRange = outlineRange;
                            if (this.mouseRange != null)
                            {
                                this.DoCodeToolTip(this.displayLines.GetOutlineHint(this.mouseRange), e.X, e.Y, false);
                                return;
                            }
                        }
                        if (outlineRange != null)
                        {
                            return;
                        }
                    }
                    if (this.margin.AllowDrag && this.margin.Visible)
                    {
                        if (this.margin.IsDragging)
                        {
                            this.margin.DragTo(e.X, e.Y);
                            return;
                        }
                        bool flag2 = this.margin.Contains(e.X, e.Y);
                        if (this.dragMargin != flag2)
                        {
                            if (this.margin.ShowHints)
                            {
                                if (this.codeCompletionHint != null)
                                {
                                    this.codeCompletionHint.Close(false);
                                }
                                this.DisableCodeCompletionTimer();
                            }
                            this.dragMargin = flag2;
                            if (this.dragMargin && this.margin.ShowHints)
                            {
                                this.DoCodeToolTip(StringConsts.DefaultDragMarginHint, e.X, e.Y, true);
                            }
                            return;
                        }
                        if (flag2 && this.margin.ShowHints)
                        {
                            return;
                        }
                    }
                    if (this.syntaxPaint.SyntaxErrorsHints)
                    {
                        ISyntaxError error;
                        if (!this.IsMouseOnSyntaxError(e.X, e.Y, out error))
                        {
                            error = null;
                        }
                        if (this.mouseError != error)
                        {
                            if (this.codeCompletionHint != null)
                            {
                                this.codeCompletionHint.Close(false);
                            }
                            this.DisableCodeCompletionTimer();
                            this.mouseError = error;
                            if (this.mouseError != null)
                            {
                                this.DoCodeToolTip(error.Description, e.X, e.Y, true);
                                return;
                            }
                        }
                        if (error != null)
                        {
                            return;
                        }
                    }
                    if ((this.gutter.Options & GutterOptions.PaintBookMarks) != GutterOptions.None)
                    {
                        IBookMark mark = null;
                        if ((hitTestInfo.HitTest & HitTest.BookMark) != HitTest.None)
                        {
                            int num2 = this.Source.BookMarks.FindBookMark((hitTestInfo.GutterImage == this.gutter.BookMarkImageIndex) ? 0x7fffffff : hitTestInfo.GutterImage, position.Y);
                            if ((num2 >= 0) && (this.Source.BookMarks[num2] is IBookMarkEx))
                            {
                                mark = this.Source.BookMarks[num2];
                            }
                        }
                        if (mark != this.mouseBookMark)
                        {
                            this.mouseBookMark = mark;
                            this.mouseBookMarkPt = new Point(e.X, e.Y);
                            if (this.gutter.ShowBookmarkHints)
                            {
                                if (this.codeCompletionHint != null)
                                {
                                    this.codeCompletionHint.Close(false);
                                }
                                this.DisableCodeCompletionTimer();
                                if (mark != null)
                                {
                                    string description = ((IBookMarkEx) mark).Description;
                                    string url = ((IBookMarkEx) mark).Url;
                                    if ((url != null) && (url != string.Empty))
                                    {
                                        description = description + (((description != null) && (description != string.Empty)) ? "\r\n" : string.Empty) + string.Format(StringConsts.DefaultHyperTextHint, url);
                                    }
                                    if (description != string.Empty)
                                    {
                                        this.DoCodeToolTip(description, e.X, e.Y, true);
                                    }
                                }
                            }
                        }
                        if ((mark != null) && this.gutter.ShowBookmarkHints)
                        {
                            return;
                        }
                    }
                    if (this.hyperText.HighlightHyperText && this.hyperText.ShowHints)
                    {
                        string str3 = string.Empty;
                        if (!this.hyperText.IsUrlAtPoint(e.X, e.Y, out str3))
                        {
                            str3 = string.Empty;
                        }
                        if (str3 != this.mouseUrl)
                        {
                            if (this.codeCompletionHint != null)
                            {
                                this.codeCompletionHint.Close(false);
                            }
                            this.DisableCodeCompletionTimer();
                            this.mouseUrl = str3;
                            if ((this.mouseUrl != string.Empty) && (this.mouseUrl != null))
                            {
                                this.mouseUrlPoint = point;
                                this.DoCodeToolTip(string.Format(StringConsts.DefaultHyperTextHint, this.mouseUrl), e.X, e.Y, true);
                            }
                        }
                        if ((this.mouseUrl != string.Empty) && (this.mouseUrl != null))
                        {
                            return;
                        }
                    }
                    if (this.Source.NeedQuickInfoTips())
                    {
                        int num3;
                        int num4;
                        Point point3 = new Point(-1, -1);
                        string s = this.Lines[position.Y];
                        if (this.Lines.GetWord(s, position.X, out num3, out num4) && (s.Substring(num3, (num4 - num3) + 1).Trim() != string.Empty))
                        {
                            point3 = new Point(num3 + 1, position.Y);
                        }
                        if (!this.infoTipPos.Equals(point3))
                        {
                            if (this.codeCompletionHint != null)
                            {
                                this.codeCompletionHint.Close(false);
                            }
                            this.DisableCodeCompletionTimer();
                            this.infoTipPos = point3;
                            if ((this.infoTipPos.Y >= 0) && (this.infoTipPos.X >= 0))
                            {
                                this.QuickInfo(this.codeCompletionArgs, this.infoTipPos, false);
                                this.OnNeedCompletion(this.codeCompletionArgs);
                            }
                        }
                    }
                }
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (e.Button == MouseButtons.Left)
            {
                string str;
                switch (this.selection.SelectionState)
                {
                    case SelectionState.None:
                        this.selection.Clear();
                        break;

                    case SelectionState.Drag:
                        if (this.needStartDrag)
                        {
                            this.selection.Clear();
                        }
                        this.selection.SelectionState = SelectionState.None;
                        break;

                    case SelectionState.Select:
                    case SelectionState.SelectLine:
                        this.selection.EndSelection();
                        if (!this.selection.IsValidSelectionPoint(this.Position))
                        {
                            this.selection.Clear();
                        }
                        break;

                    case SelectionState.SelectWord:
                        this.selection.SelectionState = SelectionState.None;
                        this.selection.EndSelection();
                        break;
                }
                this.needStartDrag = false;
                if ((this.urlAtCursor && (this.selection.SelectionState == SelectionState.None)) && (((Control.ModifierKeys & Keys.Control) != Keys.None) && this.hyperText.IsUrlAtPoint(e.X, e.Y, out str)))
                {
                    this.hyperText.UrlJump(str);
                    return;
                }
                if (((this.mouseBookMark != null) && (this.selection.SelectionState == SelectionState.None)) && ((Control.ModifierKeys & Keys.Control) != Keys.None))
                {
                    string url = ((IBookMarkEx) this.mouseBookMark).Url;
                    if ((url != null) && (url != string.Empty))
                    {
                        this.hyperText.UrlJump(url);
                        return;
                    }
                }
                if (this.margin.IsDragging)
                {
                    this.margin.Position = this.ScreenToDisplay(e.X, e.Y).X;
                    this.CancelDragging();
                }
            }
            if (((e.Button == MouseButtons.Right) && this.useDefaultMenu) && (this.ContextMenu == null))
            {
                this.PopupDefaultMenu(new Point(e.X, e.Y));
            }
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);
            this.scrolling.MouseScroll(e.Delta);
        }

        protected bool OnNeedCompletion(CodeCompletionArgs e)
        {
            this.DisableCodeCompletionTimer();
            if (this.NeedCodeCompletion != null)
            {
                this.NeedCodeCompletion(this, e);
            }
            if (((e.CompletionType == CodeCompletionType.CompleteWord) || (e.CompletionType == CodeCompletionType.CompleteComment)) && ((e.NeedShow && (e.SelIndex >= 0)) && ((e.Provider != null) && (e.SelIndex < e.Provider.Count))))
            {
                e.Handled = true;
                this.InsertTextFromProvider(e.Provider, e.Provider.GetText(e.SelIndex), e.StartPosition, e.EndPosition, e.Provider.UseIndent, e.UseFormat);
            }
            if ((!e.Handled && e.NeedShow) && (e.Provider != null))
            {
                this.DoCodeCompletion();
            }
            return e.Handled;
        }

        private void OnOutlineChanged(bool update)
        {
            if (this.outlining.AllowOutlining)
            {
                if (this.Source.NeedOutlineText())
                {
                    ISyntaxParser lexer = (ISyntaxParser) this.Source.Lexer;
                    IList<IRange> ranges = new List<IRange>();
                    lexer.Outline(ranges);
                    this.outlining.SetOutlineRanges(ranges, true);
                    if (this.Source.ActiveEdit == this)
                    {
                        Point position = this.Position;
                        if (position.X > 0)
                        {
                            position.X--;
                        }
                        this.outlining.EnsureExpanded(position);
                    }
                }
                else if (update)
                {
                    this.outlining.UnOutline();
                }
            }
        }

        protected virtual void OnOutliningChanged()
        {
        }

        protected virtual void OnPagesChanged()
        {
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            if (!pe.ClipRectangle.IsEmpty)
            {
                this.painter.BeginPaint(pe.Graphics);
                try
                {
                    this.PaintRulerRect(this.painter);
                    this.PaintScrollRect(this.painter);
                    Rectangle clientRect = this.GetClientRect(true);
                    Rectangle rect = clientRect;
                    rect.Intersect(pe.ClipRectangle);
                    switch (this.pages.PageType)
                    {
                        case PageType.PageBreaks:
                            this.syntaxPaint.PaintWindow(this.painter, this.scrolling.WindowOriginY, rect, clientRect.Location, 1f, 1f, this.pages.Rulers != EditRulers.None, false);
                            this.pages.Paint(this.painter, rect);
                            break;

                        case PageType.PageLayout:
                            this.pages.Paint(this.painter, rect);
                            break;

                        default:
                        {
                            int startLine = 0;
                            if (this.scrolling.ScrollByPixels)
                            {
                                if (this.painter.FontHeight != 0)
                                {
                                    startLine = this.scrolling.WindowOriginY / this.painter.FontHeight;
                                    int num2 = this.scrolling.WindowOriginY % this.painter.FontHeight;
                                    clientRect.Y -= num2;
                                    rect.Height += num2;
                                }
                            }
                            else
                            {
                                startLine = this.scrolling.WindowOriginY;
                            }
                            if (rect.Height <= 0x10)
                            {
                                startLine = (startLine + 1) - 1;
                            }
                            this.syntaxPaint.PaintWindow(this.painter, startLine, rect, clientRect.Location, 1f, 1f, (this.pages.Rulers != EditRulers.None) || this.scrolling.ScrollByPixels, false);
                            break;
                        }
                    }
                    if (this.Gutter.DrawLineBookmarks)
                    {
                        this.syntaxPaint.PaintLineBookMarks(this.painter, this.ClientRect);
                    }
                }
                finally
                {
                    this.painter.EndPaint();
                }
                base.OnPaint(pe);
            }
        }

        protected override void OnPaintBackground(PaintEventArgs pe)
        {
            base.OnPaintBackground(pe);
            if (this.IsTransparent && (this.PaintBackground != null))
            {
                this.PaintBackground(this, pe);
            }
        }

        protected virtual void OnPrintingChanged()
        {
        }

        protected override void OnResize(EventArgs pe)
        {
            bool flag = this.gutter.InvalidateLineNumberArea();
            base.OnResize(pe);
            if ((!flag && this.WordWrap) && ((this.GetWrapMargin() != this.WrapMargin) && (this.pages.PageType != PageType.PageLayout)))
            {
                this.UpdateWordWrap();
            }
            if (this.IsTransparent)
            {
                base.Invalidate();
            }
            this.pages.DisplayRulers();
            this.scrolling.UpdateScroll(true);
        }

        protected virtual void OnScrollingChanged()
        {
        }

        protected virtual void OnSearchDialogChanged()
        {
        }

        protected virtual void OnSearchOptionsChanged()
        {
        }

        protected virtual void OnSearchPosChanged()
        {
            this.FirstSearch = true;
        }

        protected virtual void OnSelectionChanged()
        {
        }

        private void OnSourceChanged()
        {
            if (this.displayLines != null)
            {
                this.displayLines.Lines = this.Source.Lines;
                this.OnOutlineChanged(true);
                this.MakeVisible(this.Position);
                this.UpdateCaret();
                this.scrolling.UpdateScroll();
                this.selection.Clear();
                base.Invalidate();
            }
        }

        protected void OnSourceStateChanged(NotifyState state, int first, int last)
        {
            if (this.SourceStateChanged != null)
            {
                this.notifyEventArgs.FirstChanged = first;
                this.notifyEventArgs.LastChanged = last;
                this.notifyEventArgs.State = state;
                this.notifyEventArgs.Update = false;
                this.SourceStateChanged(this, this.notifyEventArgs);
            }
            this.OnStateChanged(this.Source, state);
        }

        protected virtual void OnSpellingChanged()
        {
        }

        public virtual void OnStateChanged(object sender, NotifyState state)
        {
            if (((state & NotifyState.SelectedTextChanged) != NotifyState.None) || ((state & NotifyState.ScrollingOriginChanged) != NotifyState.None))
            {
                this.vertNavigate = false;
            }
            if (((state & NotifyState.ScrollingOptionsChanged) != NotifyState.None) || ((state & NotifyState.SelectionOptionsChanged) != NotifyState.None))
            {
                base.SetStyle(ControlStyles.ResizeRedraw, this.NeedResizeRedraw());
            }
            if ((state & NotifyState.PageOptionsChanged) != NotifyState.None)
            {
                this.OnTransparentChanged();
            }
            if ((state & NotifyState.SelectionChanged) != NotifyState.None)
            {
                this.UpdateStartSearchPos();
            }
        }

        protected void OnSyntaxChanged()
        {
            if (this.outlining.AllowOutlining && !this.Source.NeedOutlineText())
            {
                this.outlining.UnOutline();
            }
            if (this.codeCompletionBox != null)
            {
                this.codeCompletionBox.ShowTabs = this.Source.NeedCodeCompletionTabs();
            }
            this.DoFontChanged();
        }

        protected virtual void OnSyntaxPaintChanged()
        {
        }

        protected void OnTransparentChanged()
        {
            base.SetStyle(ControlStyles.Opaque, !this.IsTransparent);
            base.SetStyle(ControlStyles.DoubleBuffer, this.IsTransparent);
            base.Invalidate();
        }

        protected virtual void OnUseDefaultMenuChanged()
        {
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            if (base.Visible)
            {
                this.scrolling.UpdateScroll(true);
            }
        }

        protected virtual void OnWhiteSpaceChanged()
        {
        }

        protected void PaintDragCaret(Point pt, bool erase)
        {
            if (!pt.Equals(this.hideCaretPoint))
            {
                Rectangle rc = new Rectangle(pt.X - 1, pt.Y, 2, this.painter.FontHeight - 1);
                if (erase)
                {
                    rc.Width++;
                    rc.Height++;
                    base.Invalidate(rc);
                }
                else
                {
                    using (Graphics graphics = base.CreateGraphics())
                    {
                        Pen pen = new Pen(this.Selection.IsPosInSelection(this.Position) ? EditConsts.DefaultSelectionDragColor : Consts.DefaultControlForeColor, 1f);
                        graphics.DrawRectangle(pen, rc);
                        pen.Dispose();
                    }
                }
            }
        }

        private void PaintRulerRect(IPainter painter)
        {
            if (((this.pages.Rulers & EditRulers.Horizonal) != EditRulers.None) && ((this.pages.Rulers & EditRulers.Vertical) != EditRulers.None))
            {
                Rectangle rect = new Rectangle(0, 0, this.pages.HorzRuler.Left, this.pages.VertRuler.Top);
                Color backColor = this.Painter.BackColor;
                Color foreColor = this.Painter.ForeColor;
                try
                {
                    painter.BackColor = ((EditPages) this.Pages).RulerBackColor;
                    painter.FillRectangle(rect);
                    Rectangle rectangle2 = rect;
                    rectangle2.Inflate(-4, -4);
                    painter.BackColor = ((EditPages) this.Pages).RulerIndentBackColor;
                    rectangle2.Width -= 2;
                    rectangle2.Height -= 2;
                    painter.DrawRectangle(rectangle2);
                    painter.ExcludeClipRect(rect.Left, rect.Top, rect.Width, rect.Height);
                }
                finally
                {
                    painter.BackColor = backColor;
                    painter.ForeColor = foreColor;
                }
            }
        }

        private void PaintScrollRect(IPainter painter)
        {
            if (this.scrolling.HasVScrollBar && this.scrolling.HasHScrollBar)
            {
                Rectangle rect = new Rectangle(base.ClientRectangle.Right - this.scrolling.VScrollBar.Width, base.ClientRectangle.Bottom - this.scrolling.HScrollBar.Height, this.scrolling.VScrollBar.Width, this.scrolling.HScrollBar.Height);
                Color backColor = this.Painter.BackColor;
                try
                {
                    painter.BackColor = SystemColors.Control;
                    painter.FillRectangle(rect);
                    painter.ExcludeClipRect(rect.Left, rect.Top, rect.Width, rect.Height);
                }
                finally
                {
                    painter.BackColor = backColor;
                }
            }
        }

        public virtual void ParameterInfo()
        {
            this.ParameterInfo(this.codeCompletionArgs);
            this.OnNeedCompletion(this.codeCompletionArgs);
        }

        protected virtual void ParameterInfo(CodeCompletionArgs e)
        {
            e.Init(CodeCompletionType.ParameterInfo, this.Position);
            e.ToolTip = true;
            if (this.Source.NeedCodeCompletion())
            {
                this.DoCodeCompletion(this.Lexer as ISyntaxParser, this.Position, e);
            }
        }

        public virtual void PauseMacroRecording()
        {
            this.MacroSuspendend = true;
            this.MacroRecording = false;
        }

        protected bool PerformCycledSearch(string str, QWhale.Editor.TextSource.SearchOptions options, Regex expression, ref Point searchPos)
        {
            bool flag = this.PerformSearch(str, options, expression, ref searchPos);
            if (!flag || !this.searchCycled)
            {
                return flag;
            }
            if ((options & QWhale.Editor.TextSource.SearchOptions.BackwardSearch) != QWhale.Editor.TextSource.SearchOptions.None)
            {
                return ((searchPos.Y > this.searchStartPos.Y) || ((searchPos.Y == this.searchStartPos.Y) && ((searchPos.X + this.searchLen) > this.searchStartPos.X)));
            }
            return ((searchPos.Y < this.searchStartPos.Y) || ((searchPos.Y == this.searchStartPos.Y) && (searchPos.X < this.searchStartPos.X)));
        }

        protected bool PerformSearch(string str, QWhale.Editor.TextSource.SearchOptions options, Regex expression, ref Point searchPos)
        {
            bool flag;
            if (((options & QWhale.Editor.TextSource.SearchOptions.SelectionOnly) != QWhale.Editor.TextSource.SearchOptions.None) && !this.Selection.IsEmpty)
            {
                Point position = this.Selection.TextToSelectionPoint(searchPos);
                ITextStrings strings = new TextStrings(null) {
                    Text = this.Selection.SelectedText
                };
                flag = strings.Find(str, options, expression, ref position, out this.searchLen, out this.searchMatch);
                if (flag)
                {
                    searchPos = this.Selection.SelectionToTextPoint(position);
                }
                return flag;
            }
            if ((options & QWhale.Editor.TextSource.SearchOptions.SearchHiddenText) == QWhale.Editor.TextSource.SearchOptions.None)
            {
                Point point2 = this.displayLines.PointToDisplayPoint(searchPos.X, searchPos.Y, false);
                flag = this.displayLines.Find(str, options, expression, ref point2, out this.searchLen, out this.searchMatch);
                if (flag)
                {
                    searchPos = this.displayLines.DisplayPointToPoint(point2);
                    if ((expression != null) && ((expression.Options & RegexOptions.Multiline) != RegexOptions.None))
                    {
                        Point point3 = TextStrings.AbsolutePositionToTextPoint(this.displayLines, TextStrings.TextPointToAbsolutePosition(this.displayLines, point2, this.displayLines.LineTerminator) + this.searchLen, this.displayLines.LineTerminator);
                        point3 = this.displayLines.DisplayPointToPoint(point3);
                        this.searchLen = this.Lines.TextPointToAbsolutePosition(point3) - this.Lines.TextPointToAbsolutePosition(searchPos);
                        return flag;
                    }
                    this.searchLen = this.displayLines.DisplayPointToPoint(point2.X + this.searchLen, point2.Y, false, false, true).X - searchPos.X;
                }
                return flag;
            }
            return this.Find(str, options, expression, ref searchPos, out this.searchLen, out this.searchMatch);
        }

        public virtual void PlayBackMacro()
        {
            if (this.macroUpdateCount <= 0)
            {
                this.macroUpdateCount++;
                try
                {
                    this.macroRecording = false;
                    IMacroKeyList list = new MacroKeyList();
                    foreach (IMacroKeyData data in this.macroRecords)
                    {
                        list.Add(data);
                    }
                    foreach (IMacroKeyData data2 in list)
                    {
                        this.ProcessPlayBack(data2);
                    }
                }
                finally
                {
                    this.macroUpdateCount--;
                }
            }
        }

        private Point PointToBlockPoint(int x, Point pt, string str)
        {
            Point point = new Point(0, 0);
            string[] strArray = StringItem.Split(str.Substring(0, Math.Min(x, str.Length)));
            if (strArray.Length > 0)
            {
                point.X = strArray[strArray.Length - 1].Length;
                point.Y = strArray.Length - 1;
            }
            if (point.Y == 0)
            {
                return new Point(point.X + pt.X, pt.Y);
            }
            return new Point(point.X, point.Y + pt.Y);
        }

        protected virtual void PopupDefaultMenu(Point pos)
        {
            if (this.useDefaultMenu)
            {
                if (this.defaultMenu == null)
                {
                    this.InitDefaultMenu();
                }
                this.UpdateMenu();
                this.defaultMenu.Show(this, pos);
            }
        }

        protected void PositionChanged(UpdateReason reason, int deltaX, int deltaY)
        {
            Point position = this.Position;
            this.displayLines.PositionChanged(reason, position.X, position.Y, deltaX, deltaY);
            this.selection.PositionChanged(this.Position.X, this.Position.Y, deltaX, deltaY);
            if (this.codeCompletionBox != null)
            {
                this.codeCompletionBox.PositionChanged(this.Position.X, this.Position.Y, deltaX, deltaY);
            }
            if (this.codeCompletionHint != null)
            {
                this.codeCompletionHint.PositionChanged(this.Position.X, this.Position.Y, deltaX, deltaY);
            }
        }

        public virtual bool PositionIsReadonly(Point position)
        {
            return this.Source.PositionIsReadonly(position);
        }

        public override bool PreProcessMessage(ref Message msg)
        {
            if (msg.Msg == 260)
            {
                Keys keyData = (Keys)(((int)Control.ModifierKeys) | (msg.WParam.ToInt32() & 0xffff));
                if (this.ProcessKey(keyData))
                {
                    return true;
                }
            }
            return base.PreProcessMessage(ref msg);
        }

        public virtual bool ProcessEnter()
        {
            int num;
            if (this.InIncrementalSearch)
            {
                this.FinishIncrementalSearch();
                return true;
            }
            ITextSource source = this.Source;
            if (!source.CodeSnippets.FindSnippet(this.Position, false, out num))
            {
                return false;
            }
            ICodeSnippetRange range = null;
            foreach (ICodeSnippetRange range2 in source.CodeSnippets)
            {
                if (range2.ID == ("$" + EditConsts.DefaultSnippetEndPattern + "$"))
                {
                    range = range2;
                    break;
                }
            }
            source.CurrentSnippet = range;
            if (range != null)
            {
                this.MoveTo(range.StartPoint);
                this.selection.SmartIndent();
            }
            source.UnhighlightCodeSnippets();
            return true;
        }

        protected bool ProcessKey(Keys keyData)
        {
            if (this.keyList.ExecuteKey(keyData, ref this.keyState))
            {
                return true;
            }
            if ((Keys)((int)keyData & 0xffee) != Keys.None)
            {
                this.keyState = 0;
            }
            return false;
        }

        public virtual bool ProcessKeyMsg(ref Message msg)
        {
            bool flag = this.PreProcessMessage(ref msg);
            if (!flag)
            {
                return this.ProcessKeyMessage(ref msg);
            }
            return flag;
        }

        public virtual void ProcessKeyPress(char keyChar)
        {
            if (this.Source.Overwrite)
            {
                this.Source.DeleteRight(1);
            }
            this.selection.InsertString(keyChar.ToString());
            if (this.macroRecording)
            {
                this.RecordKeyData(new MacroKeyData(Keys.None, null, this.keyList.Handlers.MacroRecordEvent, keyChar, -1, 0));
            }
        }

        private void ProcessPlayBack(IMacroKeyData data)
        {
            if (data.Action != null)
            {
                data.Action();
            }
            else if (data.ActionEx != null)
            {
                data.ActionEx(data.Param);
            }
        }

        public virtual bool ProcessShiftTab(Point position)
        {
            return this.SelectSnippet(position, false, false);
        }

        public virtual bool ProcessTab(Point position)
        {
            return this.SelectSnippet(position, true, (this.selection.Options & SelectionOptions.DisableCodeSnippetOnTab) == SelectionOptions.None);
        }

        protected void QueryEndDrag(object sender, QueryContinueDragEventArgs e)
        {
            if (((e.Action == DragAction.Cancel) || (e.Action == DragAction.Drop)) || e.EscapePressed)
            {
                this.needStartDrag = false;
            }
            if ((e.Action == DragAction.Cancel) || e.EscapePressed)
            {
                this.Selection.BeginUpdate();
                try
                {
                    this.HideDragCaret();
                    this.Source.Position = this.saveDragPos;
                }
                finally
                {
                    this.Selection.EndUpdate();
                }
            }
        }

        public virtual void QuickInfo()
        {
            this.QuickInfo(this.codeCompletionArgs);
            this.OnNeedCompletion(this.codeCompletionArgs);
        }

        protected virtual void QuickInfo(CodeCompletionArgs e)
        {
            this.QuickInfo(e, this.Position, true);
        }

        protected virtual void QuickInfo(CodeCompletionArgs e, Point position, bool needReparse)
        {
            e.Init(CodeCompletionType.QuickInfo, position, needReparse);
            e.ToolTip = true;
            if (this.Source.NeedCodeCompletion())
            {
                this.DoCodeCompletion(this.Lexer as ISyntaxParser, position, e);
            }
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

        public virtual void RecordKeyData(IMacroKeyData keyData)
        {
            this.macroRecords.Add(keyData);
        }

        public virtual bool Replace(string text, string replaceWith)
        {
            return this.Replace(text, replaceWith, QWhale.Editor.TextSource.SearchOptions.None, null);
        }

        public virtual bool Replace(string text, string replaceWith, QWhale.Editor.TextSource.SearchOptions options)
        {
            return this.Replace(text, replaceWith, options, null);
        }

        public virtual bool Replace(string text, string replaceWith, QWhale.Editor.TextSource.SearchOptions options, Regex expression)
        {
            this.FirstSearch = true;
            return this.DoReplace(text, replaceWith, options, expression, false);
        }

        public virtual bool ReplaceAll(string text, string replaceWith, out int count)
        {
            return this.ReplaceAll(text, replaceWith, QWhale.Editor.TextSource.SearchOptions.None, null, out count);
        }

        public virtual bool ReplaceAll(string text, string replaceWith, QWhale.Editor.TextSource.SearchOptions options, out int count)
        {
            return this.ReplaceAll(text, replaceWith, options, null, out count);
        }

        public virtual bool ReplaceAll(string text, string replaceWith, QWhale.Editor.TextSource.SearchOptions options, Regex expression, out int count)
        {
            return this.DoReplaceAll(text, replaceWith, options, expression, out count);
        }

        public virtual bool ReplaceAll(string text, string replaceWith, QWhale.Editor.TextSource.SearchOptions options, Regex expression, out int count, out bool abort)
        {
            return this.DoReplaceAll(text, replaceWith, options, expression, out count, out abort);
        }

        public virtual bool ReplaceCurrent(string replaceWith, QWhale.Editor.TextSource.SearchOptions options, Match match)
        {
            bool flag = !this.Source.Readonly && this.NeedReplaceCurrent();
            if (flag)
            {
                this.DoReplace(this.searchText, replaceWith, options, match);
            }
            return flag;
        }

        private void RescanLines(int firstLine, int lastLine)
        {
            this.UpdateWordWrap(firstLine, lastLine);
            this.scrolling.UpdateScroll();
        }

        public virtual void ResetAcceptReturns()
        {
            this.AcceptReturns = true;
        }

        public virtual void ResetAcceptTabs()
        {
            this.AcceptTabs = true;
        }

        public virtual void ResetBorderColor()
        {
            this.BorderColor = Color.Empty;
        }

        public virtual void ResetBorderStyle()
        {
            this.BorderStyle = EditBorderStyle.Fixed3D;
        }

        public virtual void ResetHideCaret()
        {
            this.HideCaret = false;
        }

        public virtual void ResetIndentOptions()
        {
            this.IndentOptions = EditConsts.DefaultIndentOptions;
        }

        public virtual void ResetKeepCaretOnLostFocus()
        {
            this.keepCaretOnLostFocus = false;
        }

        public virtual void ResetMaxLength()
        {
            this.MaxLength = 0;
        }

        public virtual void ResetModified()
        {
            this.Modified = false;
        }

        public virtual void ResetNavigateOptions()
        {
            this.NavigateOptions = EditConsts.DefaultNavigateOptions;
        }

        public virtual void ResetOverWrite()
        {
            this.Overwrite = false;
        }

        public virtual void ResetReadonly()
        {
            this.Readonly = false;
        }

        public virtual void ResetSingleLineMode()
        {
            this.SingleLineMode = false;
        }

        public virtual void ResetWordWrap()
        {
            this.WordWrap = false;
        }

        public virtual void ResetWrapAtMargin()
        {
            this.WrapAtMargin = false;
        }

        public virtual Point RestorePosition(int index)
        {
            return this.Source.RestorePosition(index);
        }

        protected void RestorePositionWithUndo(int p)
        {
            this.Position = this.Source.RestorePosition(p);
            if ((this.Source.UndoOptions & UndoOptions.UndoNavigations) == UndoOptions.None)
            {
                this.Source.AddUndo(UndoOperation.NavigateEx, null);
            }
        }

        public virtual void ResumeMacroRecording()
        {
            this.MacroRecording = true;
            this.MacroSuspendend = false;
        }

        public virtual bool SaveFile(string fileName)
        {
            return this.displayLines.SaveFile(fileName);
        }

        public virtual bool SaveFile(string fileName, IStringExport exporter)
        {
            return this.displayLines.SaveFile(fileName, exporter, null);
        }

        public virtual bool SaveFile(string fileName, Encoding encoding)
        {
            return this.displayLines.SaveFile(fileName, null, encoding);
        }

        public virtual bool SaveFile(string fileName, IStringExport exporter, Encoding encoding)
        {
            return this.displayLines.SaveFile(fileName, exporter, encoding);
        }

        public virtual void SaveMacros(Stream stream)
        {
            StreamWriter writer = new StreamWriter(stream);
            try
            {
                this.SaveMacros(writer);
            }
            finally
            {
                writer.Close();
            }
        }

        public virtual void SaveMacros(TextWriter writer)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(XmlMacroKeysDataInfo));
            try
            {
                serializer.Serialize(writer, this.MacroRecords.SerializationInfo);
            }
            catch (Exception exception)
            {
                writer.Flush();
                ErrorHandler.Error(exception);
            }
        }

        public virtual void SaveMacros(string fileName)
        {
            Stream stream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
            try
            {
                this.SaveMacros(stream);
            }
            finally
            {
                stream.Close();
            }
        }

        public virtual bool SaveStream(Stream stream)
        {
            return this.displayLines.SaveStream(stream);
        }

        public virtual bool SaveStream(TextWriter writer)
        {
            return this.displayLines.SaveStream(writer);
        }

        public virtual bool SaveStream(Stream stream, IStringExport exporter)
        {
            return this.displayLines.SaveStream(stream, exporter);
        }

        public virtual bool SaveStream(Stream stream, Encoding encoding)
        {
            return this.displayLines.SaveStream(stream, encoding);
        }

        public virtual bool SaveStream(TextWriter writer, IStringExport exporter)
        {
            return this.displayLines.SaveStream(writer, exporter);
        }

        public virtual bool SaveStream(Stream stream, IStringExport exporter, Encoding encoding)
        {
            return this.displayLines.SaveStream(stream, exporter, encoding);
        }

        public virtual Point ScreenToDisplay(int x, int y)
        {
            int linesInHeight;
            int num2;
            if (this.pages.PageType == PageType.PageLayout)
            {
                IEditPage pageAtPoint = this.pages.GetPageAtPoint(x, y);
                Rectangle clientRect = pageAtPoint.ClientRect;
                x = Math.Min(x, clientRect.Right);
                x -= clientRect.Left;
                y -= clientRect.Top;
                linesInHeight = Math.Min(Math.Max(this.GetLinesInHeight(y), 0) + pageAtPoint.StartLine, pageAtPoint.EndLine);
                num2 = x - this.gutter.DisplayWidth;
            }
            else
            {
                Rectangle rectangle2 = this.ClientRect;
                if (this.scrolling.ScrollByPixels)
                {
                    linesInHeight = this.GetLinesInHeight((this.scrolling.WindowOriginY + y) - rectangle2.Top);
                    num2 = ((-rectangle2.Left + this.scrolling.WindowOriginX) + x) - this.gutter.DisplayWidth;
                }
                else
                {
                    linesInHeight = this.scrolling.WindowOriginY + this.GetLinesInHeight(y - rectangle2.Top);
                    num2 = ((-rectangle2.Left + (this.painter.FontWidth * this.scrolling.WindowOriginX)) + x) - this.gutter.DisplayWidth;
                }
            }
            int chars = 0;
            this.syntaxPaint.MeasureLine(linesInHeight, 0, -1, num2, out chars, false);
            return new Point(Math.Max(chars, 0), Math.Max(linesInHeight, 0));
        }

        protected int ScreenToDisplayX(int x, int line)
        {
            int num;
            if (this.pages.PageType == PageType.PageLayout)
            {
                Rectangle clientRect = this.pages.GetPageAt(0, line).ClientRect;
                x -= clientRect.Left;
                num = x - this.gutter.DisplayWidth;
            }
            else if (this.scrolling.ScrollByPixels)
            {
                num = ((-this.ClientRect.Left + this.scrolling.WindowOriginX) + x) - this.gutter.DisplayWidth;
            }
            else
            {
                num = ((-this.ClientRect.Left + (this.painter.FontWidth * this.scrolling.WindowOriginX)) + x) - this.gutter.DisplayWidth;
            }
            int chars = 0;
            this.syntaxPaint.MeasureLine(line, 0, -1, num, out chars, false);
            return Math.Max(chars, 0);
        }

        public virtual Point ScreenToText(Point position)
        {
            bool lineEnd = false;
            return this.ScreenToText(position.X, position.Y, ref lineEnd);
        }

        public virtual Point ScreenToText(int x, int y)
        {
            bool lineEnd = false;
            return this.ScreenToText(x, y, ref lineEnd);
        }

        public virtual Point ScreenToText(int x, int y, ref bool lineEnd)
        {
            Point point = this.ScreenToDisplay(x, y);
            return this.displayLines.DisplayPointToPoint(point.X, point.Y, ref lineEnd);
        }

        public virtual void ScrollLineDown()
        {
            if (this.scrolling.ScrollByPixels)
            {
                this.scrolling.WindowOriginY += this.painter.FontHeight;
            }
            else
            {
                this.scrolling.WindowOriginY++;
            }
        }

        public virtual void ScrollLineUp()
        {
            if (this.scrolling.ScrollByPixels)
            {
                this.scrolling.WindowOriginY -= this.painter.FontHeight;
            }
            else
            {
                this.scrolling.WindowOriginY--;
            }
        }

        private void ScrollTo(Point position, bool centerLine)
        {
            Point point = this.TextToScreen(position);
            if (this.scrolling.ScrollByPixels)
            {
                Rectangle clientRect = this.ClientRect;
                if (this.pages.PageType != PageType.PageLayout)
                {
                    clientRect.X += this.gutter.DisplayWidth;
                    clientRect.Width -= this.gutter.DisplayWidth;
                }
                if (point.X < clientRect.Left)
                {
                    this.scrolling.WindowOriginX += point.X - clientRect.Left;
                }
                else
                {
                    int width = this.GetCaretSize(position).Width;
                    if (point.X > (clientRect.Right - width))
                    {
                        this.scrolling.WindowOriginX += Math.Max((point.X - clientRect.Right) + width, 1);
                    }
                }
                if (point.Y < clientRect.Top)
                {
                    if (centerLine)
                    {
                        this.scrolling.WindowOriginY += point.Y - (clientRect.Height / 2);
                    }
                    else
                    {
                        this.scrolling.WindowOriginY += point.Y - clientRect.Top;
                    }
                }
                else if (point.Y > (clientRect.Bottom - this.painter.FontHeight))
                {
                    if (centerLine)
                    {
                        this.scrolling.WindowOriginY += point.Y - ((clientRect.Height - this.painter.FontHeight) / 2);
                    }
                    else
                    {
                        this.scrolling.WindowOriginY += (point.Y - clientRect.Height) + this.painter.FontHeight;
                    }
                }
            }
            else
            {
                position = this.displayLines.PointToDisplayPoint(position);
                int num2 = position.X - this.scrolling.WindowOriginX;
                int num3 = position.Y - this.scrolling.WindowOriginY;
                if (num2 < 0)
                {
                    this.scrolling.WindowOriginX = position.X - (((this.scrolling.Options & ScrollingOptions.UseScrollDelta) != ScrollingOptions.None) ? 1 : 0);
                }
                else
                {
                    int num4 = this.GetCaretSize(this.Position).Width;
                    int right = this.ClientRect.Right;
                    if (point.X > (right - num4))
                    {
                        if ((this.scrolling.Options & ScrollingOptions.UseScrollDelta) != ScrollingOptions.None)
                        {
                            right -= right / EditConsts.DefaultScrollDeltaRatio;
                        }
                        this.scrolling.WindowOriginX += Math.Max(this.GetCharsInWidth((point.X - right) + num4, false), 1);
                    }
                }
                if (num3 < 0)
                {
                    if (centerLine)
                    {
                        this.scrolling.WindowOriginY += this.GetLinesInHeight(point.Y - (this.ClientHeight / 2));
                    }
                    else
                    {
                        this.scrolling.WindowOriginY = position.Y;
                    }
                }
                else if (num3 > (this.LinesInHeight - 1))
                {
                    if (centerLine)
                    {
                        this.scrolling.WindowOriginY += this.GetLinesInHeight(point.Y - (this.ClientHeight / 2));
                    }
                    else
                    {
                        this.scrolling.WindowOriginY += (num3 - this.LinesInHeight) + 1;
                    }
                }
            }
        }

        protected bool SelectFirstSnippet()
        {
            ITextSource source = this.Source;
            int firstSnippet = source.CodeSnippets.GetFirstSnippet();
            if (firstSnippet >= 0)
            {
                IRange range = source.CodeSnippets[firstSnippet];
                this.MoveTo(range.StartPoint);
                this.Selection.SetSelection(SelectionType.Stream, range.StartPoint, range.EndPoint);
            }
            return (firstSnippet >= 0);
        }

        protected bool SelectionMatchesSearchText()
        {
            Match match;
            int num;
            ITextStrings strings = new TextStrings(null);
            string selectedText = this.selection.SelectedText;
            strings.Text = selectedText;
            Point position = ((this.searchOptions & QWhale.Editor.TextSource.SearchOptions.BackwardSearch) == QWhale.Editor.TextSource.SearchOptions.None) ? new Point(0, 0) : new Point(selectedText.Length, 0);
            return ((strings.Find(this.searchText, this.searchOptions, this.searchExpression, ref position, out num, out match) && (num == selectedText.Length)) && (position.X == 0));
        }

        protected bool SelectSnippet(Point position, bool selectNext, bool insertNew)
        {
            int num;
            ITextSource source = this.Source;
            if (source.CodeSnippets.FindSnippet(position, false, out num))
            {
                if (!source.CodeSnippets.IsFirstSnippet(num))
                {
                    if (!selectNext)
                    {
                        return true;
                    }
                    source.UnhighlightCodeSnippets();
                    return false;
                }
                int num2 = selectNext ? source.CodeSnippets.GetNextSnippet(num) : source.CodeSnippets.GetPrevSnippet(num);
                if (num2 != num)
                {
                    IRange range = source.CodeSnippets[num2];
                    this.MoveTo(range.StartPoint);
                    this.Selection.SetSelection(SelectionType.Stream, range.StartPoint, range.EndPoint);
                }
                return true;
            }
            if ((insertNew && !this.Readonly) && !this.Source.PositionIsReadonly(this.Position))
            {
                int num3;
                int num4;
                string s = this.Lines[this.Position.Y];
                if (((((s.Trim() != string.Empty) && this.Source.NeedCodeCompletion()) && ((this.Position.X <= s.Length) && !this.Lines.IsDelimiter(s, Math.Max(this.Position.X - 1, 0)))) && ((this.Position.X == s.Length) || this.Lines.IsDelimiter(this.Position.Y, Math.Max(this.Position.X, 0)))) && this.Lines.GetWord(s, Math.Max(this.Position.X - 1, 0), out num3, out num4))
                {
                    ISyntaxParser lexer = this.Lexer as ISyntaxParser;
                    ICodeSnippetsProvider codeSnippets = lexer.CodeSnippets;
                    if (codeSnippets != null)
                    {
                        ICodeSnippet snippet = codeSnippets.FindByShortcut(s.Substring(num3, (num4 - num3) + 1), lexer.CaseSensitive);
                        if (snippet != null)
                        {
                            this.InsertCodeSnippet(snippet, new Point(num3, this.Position.Y), new Point(num4 + 1, this.Position.Y), codeSnippets.UseIndent);
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public virtual void SetLineReadonly(int index, bool readOnly)
        {
            this.Source.SetLineReadonly(index, readOnly);
        }

        public virtual void SetNavigateOptions(QWhale.Editor.TextSource.NavigateOptions navigateOptions)
        {
            this.Source.SetNavigateOptions(navigateOptions);
        }

        public virtual bool ShouldSerializeBorderColor()
        {
            return (this.borderColor != Color.Empty);
        }

        public virtual bool ShouldSerializeBorderStyle()
        {
            return (this.borderStyle != EditBorderStyle.Fixed3D);
        }

        public bool ShouldSerializeCodeCompletionChars()
        {
            return (new string(this.codeCompletionChars) != EditConsts.DefaultCodeCompletionChars);
        }

        public bool ShouldSerializeIndentOptions()
        {
            return ((this.source == null) && (this.IndentOptions != EditConsts.DefaultIndentOptions));
        }

        public bool ShouldSerializeNavigateOptions()
        {
            return ((this.source == null) && (this.NavigateOptions != EditConsts.DefaultNavigateOptions));
        }

        public bool ShouldSerializeReadonly()
        {
            return ((this.source == null) && this.Readonly);
        }

        public bool ShouldSerializeSearchOptions()
        {
            return (this.searchOptions != EditConsts.DefaultSearchOptions);
        }

        public bool ShouldSerializeText()
        {
            return (this.source == null);
        }

        public virtual void ShowCaret(int x, int y)
        {
            if (this.dragCaret)
            {
                this.PaintDragCaret(this.oldDragPoint, true);
                this.oldDragPoint = new Point(x, y);
                this.PaintDragCaret(this.oldDragPoint, false);
            }
            else
            {
                OSUtils.SetCaretPos(x, y);
                if (this.NeedImeComposition())
                {
                    OSUtils.ImmSetCompositionWindow(base.Handle, this.TextToScreen(this.Position));
                }
            }
        }

        public virtual void ShowCodeCompletionBox(ICodeCompletionProvider provider)
        {
            Point position = base.PointToScreen(this.TextToScreen(((this.codeCompletionArgs.DisplayPosition.Y >= 0) && (this.codeCompletionArgs.StartPosition.X >= 0)) ? this.codeCompletionArgs.DisplayPosition : this.Position));
            position.Y += this.painter.FontHeight;
            this.ShowCodeCompletionBox(provider, position);
        }

        public virtual void ShowCodeCompletionBox(ICodeCompletionProvider provider, Point position)
        {
            this.ShowCodeCompletionBox(provider, position, this.codeCompletionArgs.StartPosition, this.codeCompletionArgs.EndPosition);
        }

        protected virtual void ShowCodeCompletionBox(ICodeCompletionProvider provider, Point pt, Point startPoint, Point endPoint)
        {
            this.CodeCompletionBox.Provider = provider;
            this.CodeCompletionBox.StartPos = startPoint;
            this.CodeCompletionBox.EndPos = endPoint;
            this.CodeCompletionBox.PopupAt(pt);
        }

        public virtual void ShowCodeCompletionHint(ICodeCompletionProvider provider)
        {
            Point position = base.PointToScreen(this.TextToScreen(((this.codeCompletionArgs.DisplayPosition.Y >= 0) && (this.codeCompletionArgs.DisplayPosition.X >= 0)) ? this.codeCompletionArgs.DisplayPosition : this.Position));
            position.Y += this.painter.FontHeight;
            this.ShowCodeCompletionHint(provider, position);
        }

        public virtual void ShowCodeCompletionHint(ICodeCompletionProvider provider, Point position)
        {
            this.ShowCodeCompletionHint(provider, position, this.codeCompletionArgs.StartPosition, this.codeCompletionArgs.EndPosition, this.codeCompletionArgs.DisplayPosition, false, this.codeCompletionArgs.CompletionType == CodeCompletionType.SpecialListMembers, null);
        }

        protected virtual void ShowCodeCompletionHint(ICodeCompletionProvider provider, Point pt, Point startPoint, Point endPoint, Point displayPoint, bool keepActive, bool acceptOnTab, ILexer lexer)
        {
            ICodeCompletionHint codeCompletionHint = this.CodeCompletionHint;
            codeCompletionHint.Lexer = lexer;
            codeCompletionHint.Provider = provider;
            codeCompletionHint.StartPos = startPoint;
            codeCompletionHint.DisplayPos = displayPoint;
            codeCompletionHint.EndPos = endPoint;
            if (keepActive)
            {
                codeCompletionHint.CompletionFlags |= CodeCompletionFlags.KeepActive;
            }
            else
            {
                codeCompletionHint.CompletionFlags &= ~CodeCompletionFlags.KeepActive;
            }
            if (acceptOnTab)
            {
                codeCompletionHint.CompletionFlags |= CodeCompletionFlags.AcceptOnTab;
            }
            else
            {
                codeCompletionHint.CompletionFlags &= ~CodeCompletionFlags.AcceptOnTab;
            }
            codeCompletionHint.PopupAt(pt);
        }

        public virtual void ShowNotFound(string caption)
        {
            if ((this.searchOptions & QWhale.Editor.TextSource.SearchOptions.SilentSearch) == QWhale.Editor.TextSource.SearchOptions.None)
            {
                MessageBox.Show(string.Format((((this.searchOptions & QWhale.Editor.TextSource.SearchOptions.CycledSearch) != QWhale.Editor.TextSource.SearchOptions.None) && !this.FirstSearch) ? StringConsts.SearchReachedStartPos : StringConsts.StringNotFound, this.searchText), caption);
            }
        }

        public virtual void ShowScrollHint(int pos)
        {
            this.CodeCompletionHint.Lexer = null;
            if (this.Pages.PageType == PageType.PageLayout)
            {
                this.CodeCompletionHint.Provider = this.GetQuickInfo(string.Format(StringConsts.PageNofMstr, this.pages.GetPageIndexAtPoint(0, pos - this.scrolling.WindowOriginY) + 1, this.Pages.Count), true);
            }
            else
            {
                this.CodeCompletionHint.Provider = this.GetQuickInfo(string.Format(StringConsts.LineNofMstr, this.displayLines.DisplayPointToPoint(0, pos).Y + 1, this.Lines.Count), true);
            }
            this.CodeCompletionHint.StartPos = new Point(-1, -1);
            this.CodeCompletionHint.EndPos = new Point(-1, -1);
            Point position = base.PointToScreen(new Point(base.Bounds.Width, 0));
            position.X -= this.CodeCompletionHint.Width;
            position.Y = Math.Min(Cursor.Position.Y + this.Painter.FontHeight, base.PointToScreen(new Point(base.Bounds.Width, base.Bounds.Height)).Y);
            this.CodeCompletionHint.PopupAt(position);
            base.Update();
        }

        protected void SplitterMoved(object sender, SplitterEventArgs e)
        {
            if ((sender == this.horzSplitter) && ((this.horzSplitEdit == null) || ((e.SplitY - this.horzSplitEdit.Top) <= (EditConsts.DefaltScrollSplitterSize + this.horzSplitter.Height))))
            {
                this.UnsplitView(false);
            }
            else if ((sender == this.vertSplitter) && ((this.vertSplitEdit == null) || ((e.SplitX - this.vertSplitEdit.Top) <= (EditConsts.DefaltScrollSplitterSize + this.vertSplitter.Width))))
            {
                this.UnsplitView(true);
            }
        }

        protected void SplitView(bool vert)
        {
            if (this.CanSplit(vert))
            {
                Splitter splitter = this.GetSplitter(vert);
                if (splitter == null)
                {
                    splitter = this.CreateSplitter(vert);
                }
                ISyntaxEdit splitEdit = this.GetSplitEdit(vert);
                if (splitEdit == null)
                {
                    splitEdit = this.CreateSplitEdit(vert);
                }
                splitEdit.Assign(this);
                if (vert)
                {
                    splitEdit.Width = 0;
                }
                else
                {
                    splitEdit.Height = 0;
                }
                splitEdit.Scrolling.WindowOriginY = this.Scrolling.WindowOriginY;
                splitEdit.Scrolling.WindowOriginX = this.Scrolling.WindowOriginX;
                splitter.Visible = true;
                splitEdit.Visible = true;
                base.BringToFront();
                if (vert)
                {
                    if (this.SplitVert != null)
                    {
                        this.SplitVert(this, EventArgs.Empty);
                    }
                }
                else if (this.SplitHorz != null)
                {
                    this.SplitHorz(this, EventArgs.Empty);
                }
            }
        }

        public virtual void SplitViewHorz()
        {
            this.SplitView(false);
        }

        public virtual void SplitViewVert()
        {
            this.SplitView(true);
        }

        protected void StartDragging()
        {
            this.saveDragPos = this.Position;
            if (!this.selection.IsEmpty)
            {
                if (base.DoDragDrop(this.selection.SelectedText, DragDropEffects.Move | DragDropEffects.Copy | DragDropEffects.Scroll) == DragDropEffects.Move)
                {
                    this.selection.Delete();
                }
                this.needStartDrag = false;
                this.selection.SelectionState = SelectionState.None;
            }
        }

        public virtual void StartIncrementalSearch()
        {
            this.StartIncrementalSearch(false);
        }

        public virtual void StartIncrementalSearch(bool backwardSearch)
        {
            this.inIncrementalSearch = true;
            this.incrSearchPosition = this.Position;
            this.incrStartSearchPosition = this.Position;
            this.searchText = string.Empty;
            this.searchOptions |= QWhale.Editor.TextSource.SearchOptions.FindTextAtCursor;
            this.searchStartPos = this.Position;
            this.searchCycled = false;
            if (backwardSearch)
            {
                this.searchOptions |= QWhale.Editor.TextSource.SearchOptions.BackwardSearch;
            }
            else
            {
                this.searchOptions &= ~QWhale.Editor.TextSource.SearchOptions.BackwardSearch;
            }
            this.Source.BeginUpdate(UpdateReason.Other);
            try
            {
                ITextSource source = this.Source;
                source.State |= NotifyState.IncrementalSearchChanged;
            }
            finally
            {
                this.Source.EndUpdate();
            }
            if (base.IsHandleCreated)
            {
                OSUtils.SendMessage(base.Handle, 0x20, IntPtr.Zero, IntPtr.Zero);
            }
        }

        public virtual void StartMacroRecording()
        {
            this.MacroRecording = true;
        }

        protected void StartTripleClickTimer()
        {
            if (this.tripleClickTimer == null)
            {
                this.tripleClickTimer = new Timer();
                this.tripleClickTimer.Interval = SystemInformation.DoubleClickTime;
                this.tripleClickTimer.Tick += new EventHandler(this.DoTripleClick);
            }
            this.tripleClickTimer.Enabled = true;
        }

        public virtual void StopMacroRecording()
        {
            this.MacroRecording = false;
        }

        protected void StopTripleClickTimer()
        {
            if (this.tripleClickTimer != null)
            {
                this.tripleClickTimer.Stop();
            }
            this.lbuttonClicks = 0;
        }

        public virtual int StorePosition(Point position)
        {
            return this.Source.StorePosition(position);
        }

        protected int StorePositionWithUndo(Point position)
        {
            if ((this.Source.UndoOptions & UndoOptions.UndoNavigations) == UndoOptions.None)
            {
                this.Source.AddUndo(UndoOperation.NavigateEx, null);
            }
            return this.Source.StorePosition(position);
        }

        protected void TextFound(QWhale.Editor.TextSource.SearchOptions options, Point position, int len, bool silent, bool multiline)
        {
            this.searchUpdateCount++;
            try
            {
                this.Source.BeginUpdate(UpdateReason.Navigate);
                try
                {
                    ITextSource source = this.Source;
                    source.State |= NotifyState.CenterLine;
                    if ((QWhale.Editor.TextSource.SearchOptions.BackwardSearch & options) != QWhale.Editor.TextSource.SearchOptions.None)
                    {
                        this.Source.Position = position;
                    }
                    else
                    {
                        this.Source.Position = new Point(position.X + len, position.Y);
                    }
                }
                finally
                {
                    this.Source.EndUpdate();
                }
                if (this.selection.UpdateCount == 0)
                {
                    this.selection.UpdateSelStart(position);
                    this.selection.SetSelection(SelectionType.Stream, new Rectangle(position.X, position.Y, len, 0));
                    if (multiline)
                    {
                        this.selection.SelectionLength = len;
                    }
                }
            }
            finally
            {
                this.searchUpdateCount--;
            }
            if ((!silent && (this.searchDialog != null)) && this.searchDialog.Visible)
            {
                Rectangle rect = this.selection.SelectionToScreen();
                rect.Inflate(this.Painter.FontWidth, 0);
                rect.Location = base.PointToScreen(rect.Location);
                this.searchDialog.EnsureVisible(rect);
            }
        }

        public virtual byte TextStyleAt(Point position)
        {
            if (this.Lexer != null)
            {
                IStringItem item = this.Lines.GetItem(position.Y);
                if (item != null)
                {
                    if (position.X == item.String.Length)
                    {
                        position.X--;
                    }
                    if ((position.X >= 0) && (position.X < item.TextData.Length))
                    {
                        return (byte) item.TextData[position.X];
                    }
                }
            }
            return 0;
        }

        public virtual Point TextToScreen(Point position)
        {
            return this.TextToScreen(position.X, position.Y);
        }

        public virtual Point TextToScreen(Point position, bool lineEnd)
        {
            return this.TextToScreen(position.X, position.Y, lineEnd);
        }

        public virtual Point TextToScreen(int x, int y)
        {
            return this.TextToScreen(x, y, this.displayLines.LineEnd);
        }

        protected Point TextToScreen(int x, int y, bool lineEnd)
        {
            Point point = this.displayLines.PointToDisplayPoint(x, y, lineEnd);
            return this.DisplayToScreen(point.X, point.Y);
        }

        public virtual void ToggleMacroRecording()
        {
            if (this.macroUpdateCount <= 0)
            {
                this.MacroRecording = !this.MacroRecording;
            }
        }

        public virtual bool UnBreakLine()
        {
            return this.Source.UnBreakLine();
        }

        protected void UnsplitView(bool vert)
        {
            if (this.CanUnsplit(vert))
            {
                Splitter splitter = this.GetSplitter(vert);
                if (splitter != null)
                {
                    splitter.Visible = false;
                }
                ISyntaxEdit splitEdit = this.GetSplitEdit(vert);
                if (splitEdit != null)
                {
                    splitEdit.Visible = false;
                }
                if (vert)
                {
                    if (this.UnsplitVert != null)
                    {
                        this.UnsplitVert(this, EventArgs.Empty);
                    }
                }
                else if (this.UnsplitHorz != null)
                {
                    this.UnsplitHorz(this, EventArgs.Empty);
                }
            }
        }

        public virtual void UnsplitViewHorz()
        {
            this.UnsplitView(false);
        }

        public virtual void UnsplitViewVert()
        {
            this.UnsplitView(true);
        }

        public virtual void UpdateCaret()
        {
            if (this.IsFocused || this.dragCaret)
            {
                Point point = this.TextToScreen(this.Position);
                if ((point.X < this.gutter.DisplayWidth) || (point.Y < 0))
                {
                    this.DisplayCaretNowhere();
                }
                else
                {
                    this.ShowCaret(point.X, point.Y);
                }
                if ((this.pages.Rulers != EditRulers.None) && !this.dragCaret)
                {
                    this.pages.DisplayRulers();
                }
            }
            this.UpdateSeparator();
        }

        protected void UpdateCaretMode()
        {
            if (this.IsFocused)
            {
                this.DestroyCaret();
                this.CreateCaret();
                this.UpdateCaret();
            }
        }

        protected virtual void UpdateMenu()
        {
            this.miUndo.Enabled = this.Source.CanUndo();
            this.miRedo.Enabled = this.Source.CanRedo();
            this.miCut.Enabled = this.Selection.CanCut();
            this.miCopy.Enabled = this.Selection.CanCopy();
            this.miPaste.Enabled = this.Selection.CanPaste();
            this.miDelete.Enabled = this.Text != string.Empty;
        }

        protected virtual void UpdateMonospaced()
        {
            if (this.Lexer != null)
            {
                foreach (ILexStyle style in this.Lexer.Scheme.Styles)
                {
                    this.painter.FontStyle = style.FontStyle;
                }
            }
            if (this.Braces.BracesOptions != BracesOptions.None)
            {
                this.painter.FontStyle = this.Braces.FontStyle;
            }
        }

        private void UpdatePages(int firstLine)
        {
            if (this.Pages.PageType != PageType.Normal)
            {
                this.Pages.GetPageAt(this.displayLines.PointToDisplayPoint(0, firstLine)).Update();
            }
        }

        private void UpdateSeparator()
        {
            if (this.lineSeparator.NeedHighlight())
            {
                this.InvalidateLine(this.oldLine);
                this.oldLine = this.displayLines.PointToDisplayPoint(this.Position).Y;
                this.InvalidateLine(this.oldLine);
            }
        }

        protected void UpdateStartSearchPos()
        {
            if ((this.searchUpdateCount == 0) && !this.FirstSearch)
            {
                this.searchStartPos = this.Position;
                this.searchCycled = false;
                if ((this.searchOptions & QWhale.Editor.TextSource.SearchOptions.SelectionOnly) != QWhale.Editor.TextSource.SearchOptions.None)
                {
                    this.searchSelType = this.Selection.SelectionType;
                    this.searchSelRect = this.Selection.SelectionRect;
                }
                this.InitSearchPos(this.searchOptions, true);
            }
        }

        public void UpdateView()
        {
            this.displayLines.UpdateNeeded();
            this.pages.DisplayRulers();
            this.UpdateCaretMode();
            base.Invalidate();
        }

        public virtual bool UpdateWordWrap()
        {
            return this.displayLines.UpdateWordWrap();
        }

        public virtual bool UpdateWordWrap(int first, int last)
        {
            return this.displayLines.UpdateWordWrap(first, last);
        }

        public virtual void ValidatePosition(ref Point position)
        {
            this.Source.ValidatePosition(ref position);
        }

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case 0x20:
                    if (!this.CheckCursor(base.PointToClient(Control.MousePosition)))
                    {
                        goto Label_0350;
                    }
                    m.Result = (IntPtr) 1;
                    return;

                case 0x85:
                    base.WndProc(ref m);
                    if (((this.BorderStyle == EditBorderStyle.System) && (XPThemes.CurrentTheme != XPThemeName.None)) && (this.BorderColor != Color.Empty))
                    {
                        IntPtr windowDC = OSUtils.GetWindowDC(base.Handle);
                        try
                        {
                            if (this.BorderColor != Color.Empty)
                            {
                                Graphics graphics = Graphics.FromHdc(windowDC);
                                Pen pen = new Pen(this.BorderColor);
                                try
                                {
                                    graphics.DrawRectangle(pen, 0, 0, base.Width - 1, base.Height - 1);
                                    break;
                                }
                                finally
                                {
                                    pen.Dispose();
                                    graphics.Dispose();
                                }
                            }
                            OSUtils.ExcludeClipRect(windowDC, 2, 2, base.Width - 2, base.Height - 2);
                            XPThemes.DrawEditBorder(windowDC, new Rectangle(0, 0, base.Width, base.Height));
                        }
                        finally
                        {
                            OSUtils.ReleaseDC(base.Handle, windowDC);
                        }
                    }
                    break;

                case 0xc6:
                    if (this.Source.CanUndo())
                    {
                        m.Result = (IntPtr) 1;
                    }
                    goto Label_0350;

                case 0x114:
                    if ((this.scrolling.Options & ScrollingOptions.SystemScrollbars) == ScrollingOptions.None)
                    {
                        goto Label_0350;
                    }
                    m.Result = (IntPtr) 1;
                    this.scrolling.SystemScroll(OSUtils.LoWord(m.WParam), false);
                    return;

                case 0x115:
                    if ((this.scrolling.Options & ScrollingOptions.SystemScrollbars) == ScrollingOptions.None)
                    {
                        goto Label_0350;
                    }
                    m.Result = (IntPtr) 1;
                    this.scrolling.SystemScroll(OSUtils.LoWord(m.WParam), true);
                    return;

                case 0x201:
                    base.Capture = true;
                    goto Label_0350;

                case 0x202:
                    base.Capture = false;
                    goto Label_0350;

                case 0x282:
                {
                    int num3 = m.WParam.ToInt32();
                    if (((num3 == 8) || (num3 == 11)) && this.NeedImeComposition())
                    {
                        OSUtils.ImmSetCompositionFont(base.Handle, this.Font);
                    }
                    goto Label_0350;
                }
                case 0x283:
                    if ((m.WParam.ToInt32() == 11) && this.NeedImeComposition())
                    {
                        OSUtils.UpdateCompositionWindow(base.Handle, this.TextToScreen(this.Position), m.LParam);
                    }
                    goto Label_0350;

                case 0x300:
                    if (this.selection.CanCut())
                    {
                        this.selection.Cut();
                    }
                    goto Label_0350;

                case 0x301:
                    if (this.selection.CanCopy())
                    {
                        this.selection.Copy();
                    }
                    goto Label_0350;

                case 770:
                    if (this.selection.CanPaste())
                    {
                        this.selection.Paste();
                    }
                    goto Label_0350;

                case 0x303:
                    if (!this.selection.IsEmpty)
                    {
                        this.selection.Delete();
                    }
                    goto Label_0350;

                case 0x304:
                    if (this.Source.CanUndo())
                    {
                        this.Source.Undo();
                        m.Result = (IntPtr) 1;
                    }
                    goto Label_0350;

                default:
                    goto Label_0350;
            }
            return;
        Label_0350:
            base.WndProc(ref m);
            if ((m.Msg == 0x10d) && this.NeedImeComposition())
            {
                OSUtils.ImmSetCompositionWindow(base.Handle, this.TextToScreen(this.Position));
            }
        }

        [Description("Gets or set a boolean value that indicates whether Enter key should be accepted by Edit control as input key."), DefaultValue(true), Category("Behavior")]
        public virtual bool AcceptReturns
        {
            get
            {
                return this.acceptReturns;
            }
            set
            {
                if (this.acceptReturns != value)
                {
                    this.acceptReturns = value;
                    this.OnAcceptReturnsChanged();
                }
            }
        }

        [Category("Behavior"), DefaultValue(true), Description("Gets or set a boolean value that indicates whether TAB key should be accepted by Edit control as input key.")]
        public virtual bool AcceptTabs
        {
            get
            {
                return this.acceptTabs;
            }
            set
            {
                if (this.acceptTabs != value)
                {
                    this.acceptTabs = value;
                    this.OnAcceptTabsChanged();
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public override bool AllowDrop
        {
            get
            {
                if (!base.AllowDrop)
                {
                    return ((this.selection.Options & SelectionOptions.DisableDragging) == SelectionOptions.None);
                }
                return true;
            }
            set
            {
                base.AllowDrop = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public virtual char[] AutoCorrectDelimiters
        {
            get
            {
                return this.autoCorrectDelimiters;
            }
            set
            {
                if (this.autoCorrectDelimiters != value)
                {
                    this.autoCorrectDelimiters = value;
                    this.OnAutoCorrectDelimitersChanged();
                }
            }
        }

        [DefaultValue(false), Category("SyntaxEdit"), Description("Gets or sets a boolean value indicating whether to auto correct words being typed.")]
        public virtual bool AutoCorrection
        {
            get
            {
                return this.autoCorrection;
            }
            set
            {
                if (this.autoCorrection != value)
                {
                    this.autoCorrection = value;
                    this.OnAutoCorrectionChanged();
                }
            }
        }

        [Category("Appearance"), Description("Gets or sets the border color for the Edit control.")]
        public virtual Color BorderColor
        {
            get
            {
                return this.borderColor;
            }
            set
            {
                if (this.borderColor != value)
                {
                    this.borderColor = value;
                    this.OnBorderColorChanged();
                }
            }
        }

        [DefaultValue(1), Description("Gets or sets the border style for the Edit control."), Category("Appearance")]
        public virtual EditBorderStyle BorderStyle
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

        [TypeConverter(typeof(ExpandableObjectConverter)), Category("SyntaxEdit"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Represents an object that implements \"IEditBraceMatching\" interface allowing to change appearance of matching braces within the control.")]
        public virtual IEditBraceMatching Braces
        {
            get
            {
                return this.braces;
            }
            set
            {
                if (this.braces != value)
                {
                    this.braces = value;
                    this.OnBracesChanged();
                }
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual int CharsInWidth
        {
            get
            {
                return this.GetCharsInWidth(this.GetClientWidth(true));
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public virtual Rectangle ClientArea
        {
            get
            {
                return this.GetClientRect(true);
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public virtual int ClientHeight
        {
            get
            {
                return this.GetClientRect(true).Height;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public virtual Rectangle ClientRect
        {
            get
            {
                return this.GetClientRect(false);
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual int ClientWidth
        {
            get
            {
                return this.GetClientWidth(false);
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual ICodeCompletionBox CodeCompletionBox
        {
            get
            {
                if (this.codeCompletionBox == null)
                {
                    this.codeCompletionBox = this.CreateCodeCompletionBox();
                    this.codeCompletionBox.ShowTabs = this.Source.NeedCodeCompletionTabs();
                    this.codeCompletionBox.ClosePopup += new ClosePopupEvent(this.CloseCodeCompletionBox);
                    this.codeCompletionBox.Disposed += new EventHandler(this.DisposeCodeCompletionBox);
                }
                return this.codeCompletionBox;
            }
        }

        [Category("SyntaxEdit"), Description("Represents a collection of characters that initializes a code completion procedure when typing in the editor.")]
        public virtual char[] CodeCompletionChars
        {
            get
            {
                return this.codeCompletionChars;
            }
            set
            {
                if (this.codeCompletionChars != value)
                {
                    this.codeCompletionChars = value;
                    this.OnCodeCompletionCharsChanged();
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public virtual ICodeCompletionHint CodeCompletionHint
        {
            get
            {
                if (this.codeCompletionHint == null)
                {
                    this.codeCompletionHint = this.CreateCodeCompletionHint();
                    this.codeCompletionHint.ClosePopup += new ClosePopupEvent(this.CloseCodeCompletionHint);
                    this.codeCompletionHint.Disposed += new EventHandler(this.DisposeCodeCompletionBox);
                }
                return this.codeCompletionHint;
            }
        }

        protected Timer CodeCompletionTimer
        {
            get
            {
                if (this.codeCompletionTimer == null)
                {
                    this.codeCompletionTimer = new Timer();
                    this.codeCompletionTimer.Enabled = false;
                    this.codeCompletionTimer.Interval = EditConsts.DefaultHintDelay;
                    this.codeCompletionTimer.Tick += new EventHandler(this.OnCodeCompletion);
                    this.components.Add(this.codeCompletionTimer);
                }
                return this.codeCompletionTimer;
            }
        }

        protected override System.Windows.Forms.CreateParams CreateParams
        {
            get
            {
                System.Windows.Forms.CreateParams createParams = base.CreateParams;
                switch (this.borderStyle)
                {
                    case EditBorderStyle.Fixed3D:
                    case EditBorderStyle.System:
                        createParams.ExStyle |= 0x200;
                        return createParams;

                    case EditBorderStyle.FixedSingle:
                        createParams.Style |= 0x800000;
                        return createParams;
                }
                return createParams;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public ContextMenuStrip DefaultMenu
        {
            get
            {
                if (this.defaultMenu == null)
                {
                    this.InitDefaultMenu();
                }
                return this.defaultMenu;
            }
            set
            {
                if (this.defaultMenu != value)
                {
                    this.defaultMenu = value;
                    this.OnDefaultMenuChanged();
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public virtual IDisplayStrings DisplayLines
        {
            get
            {
                return this.displayLines;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), TypeConverter(typeof(ExpandableObjectConverter)), Description("Represents an object that implements \"IMargin\" interface and specifies appearance of vertical line drawn over the text and used to mark some limit, for example, of the maximum string length allowed."), Category("SyntaxEdit")]
        public virtual IMargin EditMargin
        {
            get
            {
                return this.margin;
            }
            set
            {
                if (this.margin != value)
                {
                    this.margin = value;
                    this.OnEditMarginChanged();
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public virtual IEditorSettingsDialog EditorSettingsDialog
        {
            get
            {
                if (this.editorSettingsDialog == null)
                {
                    this.editorSettingsDialog = new DlgSyntaxSettings();
                    this.editorSettingsDialog.HelpRequested += new HelpEventHandler(this.syntaxSettings.OnHelpRequest);
                }
                return this.editorSettingsDialog;
            }
            set
            {
                if (this.editorSettingsDialog != value)
                {
                    this.editorSettingsDialog = value;
                    this.OnEditorSettingsDialogChanged();
                }
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IEventHandlers EventHandlers
        {
            get
            {
                return this.keyList.Handlers;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public virtual bool FirstSearch
        {
            get
            {
                return this.firstSearch;
            }
            set
            {
                if (this.firstSearch != value)
                {
                    this.Source.BeginUpdate(UpdateReason.Other);
                    try
                    {
                        this.firstSearch = value;
                        ITextSource source = this.Source;
                        source.State |= NotifyState.FirstSearchChanged;
                    }
                    finally
                    {
                        this.Source.EndUpdate();
                    }
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public virtual IGotoLineDialog GotoLineDialog
        {
            get
            {
                if (this.gotoLineDialog == null)
                {
                    this.gotoLineDialog = new DlgGoto();
                }
                return this.gotoLineDialog;
            }
            set
            {
                if (this.gotoLineDialog != value)
                {
                    this.gotoLineDialog = value;
                    this.OnGotoLineDialogChanged();
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), TypeConverter(typeof(ExpandableObjectConverter)), Category("SyntaxEdit"), Description("Represents object that implements \"IGutter\" interface containing methods and properties necessary to operate with gutter at the left size of the control.")]
        public virtual IGutter Gutter
        {
            get
            {
                return this.gutter;
            }
            set
            {
                if (this.gutter != value)
                {
                    this.gutter = value;
                    this.OnGutterChanged();
                }
            }
        }

        [DefaultValue(false), Description("Gets or sets a value indicating whether the control should display caret when it has input focus."), Category("Behavior")]
        public virtual bool HideCaret
        {
            get
            {
                return this.hideCaret;
            }
            set
            {
                if (this.hideCaret != value)
                {
                    this.hideCaret = value;
                    this.OnHideCaretChanged();
                }
            }
        }

        protected Cursor HideWhiteSpaceCursor
        {
            get
            {
                if (this.hideWhiteSpaceCursor == null)
                {
                    this.hideWhiteSpaceCursor = new Cursor(typeof(SyntaxEdit), "Images.HideWhiteSpace.cur");
                }
                return this.hideWhiteSpaceCursor;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public virtual ISyntaxEdit HorzSplitEdit
        {
            get
            {
                return this.horzSplitEdit;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public virtual Splitter HorzSplitter
        {
            get
            {
                return this.horzSplitter;
            }
        }

        [TypeConverter(typeof(ExpandableObjectConverter)), Category("SyntaxEdit"), Description("Represents object that implements \"IEditHyperText\" interface allowing to customize appearance and behaviour of hypertext sections within the control."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual IEditHyperText HyperText
        {
            get
            {
                return this.hyperText;
            }
            set
            {
                if (this.hyperText != value)
                {
                    this.hyperText = value;
                    this.OnHyperTextChanged();
                }
            }
        }

        protected Cursor IncrementalSearchCursor
        {
            get
            {
                if (this.incrementalSearchCursor == null)
                {
                    this.incrementalSearchCursor = new Cursor(typeof(SyntaxEdit), "Images.IncrementalSearch.cur");
                }
                return this.incrementalSearchCursor;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual string IncrementalSearchString
        {
            get
            {
                if (!this.inIncrementalSearch)
                {
                    return string.Empty;
                }
                return this.searchText;
            }
        }

        [Editor("QWhale.Design.FlagEnumerationEditor, QWhale.Editor", typeof(UITypeEditor)), Description("Gets or sets \"QWhale.Editor.IndentOptions\" for this class, allowing to customize behaior of Edit control when user presses Enter to insert new text line."), Category("Behavior")]
        public virtual QWhale.Editor.TextSource.IndentOptions IndentOptions
        {
            get
            {
                return this.Source.IndentOptions;
            }
            set
            {
                this.Source.IndentOptions = value;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual bool InIncrementalSearch
        {
            get
            {
                return this.inIncrementalSearch;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public virtual bool IsCodeCompletionWindowFocused
        {
            get
            {
                Control control;
                return this.CodeCompletionWindowFocused(out control);
            }
        }

        protected virtual bool IsFocused
        {
            get
            {
                if (!this.Focused)
                {
                    return this.IsCodeCompletionWindowFocused;
                }
                return true;
            }
        }

        protected bool IsTransparent
        {
            get
            {
                if (!this.transparent)
                {
                    return this.pages.Transparent;
                }
                return true;
            }
        }

        [Category("SyntaxEdit"), DefaultValue(false), Description("Keeps caret in visible state even Edit control lost focus.")]
        public virtual bool KeepCaretOnLostFocus
        {
            get
            {
                return this.keepCaretOnLostFocus;
            }
            set
            {
                if (this.keepCaretOnLostFocus != value)
                {
                    this.keepCaretOnLostFocus = value;
                    this.OnKeepCaretOnLostFocusChanged();
                }
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual IKeyList KeyList
        {
            get
            {
                return this.keyList;
            }
        }

        protected Cursor LeftArrowCursor
        {
            get
            {
                if (this.leftArrowCursor == null)
                {
                    this.leftArrowCursor = new Cursor(typeof(SyntaxEdit), "Images.LeftArrow.cur");
                }
                return this.leftArrowCursor;
            }
        }

        [Category("SyntaxEdit"), DefaultValue((string) null), Description("Gets or sets object that can make lexical analysis for the control's content.")]
        public virtual ILexer Lexer
        {
            get
            {
                return this.Source.Lexer;
            }
            set
            {
                this.Source.Lexer = value;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual ITextStrings Lines
        {
            get
            {
                return this.Source.Lines;
            }
            set
            {
                this.Source.Lines = value;
            }
        }

        [TypeConverter(typeof(ExpandableObjectConverter)), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Represents an object that implements \"ILineSeparator\" interface containing methods and properties necessary to separate lines and highlight current line within the control."), Category("SyntaxEdit")]
        public virtual ILineSeparator LineSeparator
        {
            get
            {
                return this.lineSeparator;
            }
            set
            {
                if (this.lineSeparator != value)
                {
                    this.lineSeparator = value;
                    this.OnLineSeparatorChanged();
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public virtual int LinesInHeight
        {
            get
            {
                return this.GetLinesInHeight(this.ClientHeight);
            }
        }

        [TypeConverter(typeof(LineStylesConverter)), Category("SyntaxEdit"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Editor("QWhale.Design.LineStylesEditor, QWhale.Editor", typeof(UITypeEditor))]
        public virtual IEditLineStyles LineStyles
        {
            get
            {
                return this.lineStyles;
            }
            set
            {
                if (this.lineStyles != value)
                {
                    this.lineStyles = value;
                    this.OnLineStylesChanged();
                }
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual string LineTerminator
        {
            get
            {
                return this.Lines.LineTerminator;
            }
            set
            {
                this.Lines.LineTerminator = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public virtual bool MacroRecording
        {
            get
            {
                return this.macroRecording;
            }
            set
            {
                if (this.macroRecording != value)
                {
                    this.macroRecording = value;
                    this.OnMacroRecordingChanged();
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public virtual IMacroKeyList MacroRecords
        {
            get
            {
                return this.macroRecords;
            }
            set
            {
                this.macroRecords.Clear();
                foreach (IMacroKeyData data in value)
                {
                    this.macroRecords.Add(data);
                }
                this.OnMacroRecordsChanged();
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual bool MacroSuspendend
        {
            get
            {
                return this.macroSuspended;
            }
            set
            {
                if (this.macroSuspended != value)
                {
                    this.macroSuspended = value;
                    this.OnMacroSuspendendChanged();
                }
            }
        }

        [DefaultValue(0), Description("Specifies the maximum number of characters that can be entered into the edit control."), Category("Behavior")]
        public virtual int MaxLength
        {
            get
            {
                return this.Source.MaxLength;
            }
            set
            {
                this.Source.MaxLength = value;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual bool Modified
        {
            get
            {
                return this.Source.Modified;
            }
            set
            {
                this.Source.Modified = value;
            }
        }

        [Description("Gets or sets navigating options."), Editor("QWhale.Design.FlagEnumerationEditor, QWhale.Editor", typeof(UITypeEditor)), Category("Behavior")]
        public virtual QWhale.Editor.TextSource.NavigateOptions NavigateOptions
        {
            get
            {
                return this.Source.NavigateOptions;
            }
            set
            {
                this.Source.NavigateOptions = value;
            }
        }

        [TypeConverter(typeof(ExpandableObjectConverter)), Description("Represents an object that implements \"IOutlining\" interface that specifies appearance and behaviour of outline sections within the control."), Category("SyntaxEdit"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual IOutlining Outlining
        {
            get
            {
                return this.outlining;
            }
            set
            {
                if (this.outlining != value)
                {
                    this.outlining = value;
                    this.OnOutliningChanged();
                }
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual bool Overwrite
        {
            get
            {
                return this.Source.Overwrite;
            }
            set
            {
                this.Source.Overwrite = value;
            }
        }

        [Category("SyntaxEdit"), TypeConverter(typeof(ExpandableObjectConverter)), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Represents an object that implements \"IEditPage\" interface containing properties and methods representing collection of particular pages.")]
        public virtual IEditPages Pages
        {
            get
            {
                return this.pages;
            }
            set
            {
                if (this.pages != value)
                {
                    this.pages.BeginUpdate();
                    try
                    {
                        this.pages.Clear();
                        foreach (IEditPage page in value)
                        {
                            this.pages.Add().Assign(page);
                        }
                    }
                    finally
                    {
                        this.pages.EndUpdate();
                    }
                    this.OnPagesChanged();
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public virtual IPainter Painter
        {
            get
            {
                return this.painter;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public virtual Point Position
        {
            get
            {
                return this.Source.Position;
            }
            set
            {
                this.Source.Position = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public virtual Point PrevPosition
        {
            get
            {
                return this.Source.PrevPosition;
            }
        }

        [Category("SyntaxEdit"), Description("Represents an object that implements \"IPrinting\" interface allowing to perform various printing actions such as print, preview document, and setup print options."), TypeConverter(typeof(ExpandableObjectConverter)), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual IPrinting Printing
        {
            get
            {
                return this.printing;
            }
            set
            {
                if (this.printing != value)
                {
                    this.printing = value;
                    this.OnPrintingChanged();
                }
            }
        }

        [Description("Gets or sets a value indicating whether the control's content is read-only."), DefaultValue(false), Category("Behavior")]
        public virtual bool Readonly
        {
            get
            {
                return this.Source.Readonly;
            }
            set
            {
                this.Source.Readonly = value;
            }
        }

        protected Cursor ReverseIncrementalSearchCursor
        {
            get
            {
                if (this.reverseIncrementalSearchCursor == null)
                {
                    this.reverseIncrementalSearchCursor = new Cursor(typeof(SyntaxEdit), "Images.ReverseIncrementalSearch.cur");
                }
                return this.reverseIncrementalSearchCursor;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("SyntaxEdit"), TypeConverter(typeof(ExpandableObjectConverter))]
        public virtual IScrolling Scrolling
        {
            get
            {
                return this.scrolling;
            }
            set
            {
                if (this.scrolling != value)
                {
                    this.scrolling = value;
                    this.OnScrollingChanged();
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public virtual ISearchDialog SearchDialog
        {
            get
            {
                if (this.searchDialog == null)
                {
                    this.searchDialog = new QWhale.Editor.Dialogs.SearchDialog();
                    this.searchDialog.SearchSettings.SearchOptions = this.searchOptions;
                }
                return this.searchDialog;
            }
            set
            {
                if (this.searchDialog != value)
                {
                    this.searchDialog = value;
                    this.OnSearchDialogChanged();
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public virtual int SearchLen
        {
            get
            {
                return this.searchLen;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual QWhale.Editor.TextSource.SearchOptions SearchOptions
        {
            get
            {
                return this.searchOptions;
            }
            set
            {
                if (this.searchOptions != value)
                {
                    this.searchOptions = value;
                    this.OnSearchOptionsChanged();
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public virtual Point SearchPos
        {
            get
            {
                return this.searchPos;
            }
            set
            {
                if (this.searchPos != value)
                {
                    this.searchPos = value;
                    this.OnSearchPosChanged();
                }
            }
        }

        [Category("SyntaxEdit"), TypeConverter(typeof(ExpandableObjectConverter)), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Represents an object that implements \"ISelection\" interface. This object represents various properties and methods to manipulate text selection, such as copy, paste and drag selected text.")]
        public virtual ISelection Selection
        {
            get
            {
                return this.selection;
            }
            set
            {
                if (this.selection != value)
                {
                    this.selection = value;
                    this.OnSelectionChanged();
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public virtual ISerializationInfo SerializationInfo
        {
            get
            {
                return new XmlSyntaxEditInfo(this);
            }
            set
            {
                value.FixupReferences(this);
            }
        }

        protected Cursor ShowWhiteSpaceCursor
        {
            get
            {
                if (this.showWhiteSpaceCursor == null)
                {
                    this.showWhiteSpaceCursor = new Cursor(typeof(SyntaxEdit), "Images.ShowWhiteSpace.cur");
                }
                return this.showWhiteSpaceCursor;
            }
        }

        [Description("Gets or sets a value indicating whether the control accepts only one line of the text."), DefaultValue(false), Category("Behavior")]
        public virtual bool SingleLineMode
        {
            get
            {
                return this.Source.SingleLineMode;
            }
            set
            {
                this.Source.SingleLineMode = value;
            }
        }

        [Description("Gets or sets an object that implements \"ITextSource\" interface containing an actual string data displayed by the control."), Category("SyntaxEdit")]
        public virtual ITextSource Source
        {
            get
            {
                if (this.source == null)
                {
                    return this.innerTextSource;
                }
                return this.source;
            }
            set
            {
                if (this.source != value)
                {
                    if (this.source != null)
                    {
                        this.source.RemoveNotifier(this);
                        this.source.ActiveEdit = null;
                        this.source.Edits.Remove(this);
                    }
                    this.source = value;
                    if (this.source != null)
                    {
                        this.source.AddNotifier(this);
                        if (this.source.ActiveEdit == null)
                        {
                            this.source.ActiveEdit = this;
                        }
                        this.source.Edits.Add(this);
                    }
                    this.OnSourceChanged();
                }
            }
        }

        [Category("SyntaxEdit"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), TypeConverter(typeof(ExpandableObjectConverter)), Description("Represents object that implements \"IEditSpelling\" interface containing properties and methods to check control's content spelling and highlight mispelled words.")]
        public virtual IEditSpelling Spelling
        {
            get
            {
                return this.spelling;
            }
            set
            {
                if (this.spelling != value)
                {
                    this.spelling = value;
                    this.OnSpellingChanged();
                }
            }
        }

        [TypeConverter(typeof(CollectionConverter)), Description("Represents \"Lines\" property in the form of array of strings."), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Category("SyntaxEdit")]
        public string[] Strings
        {
            get
            {
                ITextStrings lines = this.Source.Lines;
                string[] strArray = new string[lines.Count];
                for (int i = 0; i < lines.Count; i++)
                {
                    strArray[i] = lines[i];
                }
                return strArray;
            }
            set
            {
                ITextStrings lines = this.Source.Lines;
                this.Source.BeginUpdate(UpdateReason.Other);
                lines.BeginUpdate();
                try
                {
                    lines.Clear();
                    foreach (string str in value)
                    {
                        lines.Add(str);
                    }
                    ITextSource source = this.Source;
                    source.State |= NotifyState.CountChanged;
                    lines.Changed(0, 0x7fffffff);
                }
                finally
                {
                    lines.EndUpdate();
                    this.Source.EndUpdate();
                }
            }
        }

        [TypeConverter(typeof(ExpandableObjectConverter)), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("SyntaxEdit")]
        public virtual IEditSyntaxPaint SyntaxPaint
        {
            get
            {
                return this.syntaxPaint;
            }
            set
            {
                if (this.syntaxPaint != value)
                {
                    this.syntaxPaint = value;
                    this.OnSyntaxPaintChanged();
                }
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ISyntaxSettings SyntaxSettings
        {
            get
            {
                return this.syntaxSettings;
            }
        }

        [Description("Gets or sets the string collection as a single string with the individual lines delimited by carriage returns.")]
        public override string Text
        {
            get
            {
                return this.Lines.Text;
            }
            set
            {
                this.Lines.Text = value;
            }
        }

        [Category("Appearance"), Description("Gets or sets a boolean value that indicates whether Edit control should draw its background."), DefaultValue(false)]
        public virtual bool Transparent
        {
            get
            {
                return this.transparent;
            }
            set
            {
                if (this.transparent != value)
                {
                    this.transparent = value;
                    this.OnTransparentChanged();
                }
            }
        }

        [DefaultValue(true), Category("Behavior"), Description("Indicates whether edit control should use bultin popup menu.")]
        public virtual bool UseDefaultMenu
        {
            get
            {
                return this.useDefaultMenu;
            }
            set
            {
                if (this.useDefaultMenu != value)
                {
                    this.useDefaultMenu = value;
                    this.OnUseDefaultMenuChanged();
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public virtual ISyntaxEdit VertSplitEdit
        {
            get
            {
                return this.vertSplitEdit;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual Splitter VertSplitter
        {
            get
            {
                return this.vertSplitter;
            }
        }

        [Description("Represents an object that implements \"IWhiteSpace\" interface. This object specifies appearance of white space characters, as well as End-of-line and End-of-file marks."), TypeConverter(typeof(ExpandableObjectConverter)), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("SyntaxEdit")]
        public virtual IWhiteSpace WhiteSpace
        {
            get
            {
                return this.whiteSpace;
            }
            set
            {
                if (this.whiteSpace != value)
                {
                    this.whiteSpace = value;
                    this.OnWhiteSpaceChanged();
                }
            }
        }

        [Category("Appearance"), Description("Gets or sets a value indicating whether control automatically wraps words to the beginning of the next line when necessary."), DefaultValue(false)]
        public virtual bool WordWrap
        {
            get
            {
                return this.displayLines.WordWrap;
            }
            set
            {
                this.displayLines.WordWrap = value;
            }
        }

        [Description("Gets or sets a value indicating whether control automatically wraps words at margin position."), Category("Appearance"), DefaultValue(false)]
        public virtual bool WrapAtMargin
        {
            get
            {
                return this.displayLines.WrapAtMargin;
            }
            set
            {
                this.displayLines.WrapAtMargin = value;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual int WrapMargin
        {
            get
            {
                return this.displayLines.WrapMargin;
            }
        }
    }
}

