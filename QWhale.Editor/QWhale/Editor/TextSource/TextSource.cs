namespace QWhale.Editor.TextSource
{
    using QWhale.Common;
    using QWhale.Editor;
    using QWhale.Editor.TextSource.Serialization;
    using QWhale.Syntax;
    using QWhale.Syntax.Lexer;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Design;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Windows.Forms;

    [ToolboxBitmap(typeof(SyntaxEdit), "Images.TextSource.bmp")]
    public class TextSource : Component, ITextSource, IEdit, INavigate, IUndo, ITextNotify, INotify, IUpdate, INotifier, ITextImport, IImport, ITextExport, IExport, IHyperText, ISpelling, IBraceMatching, ITextParsing, ITextSnippets, ITextErrors
    {
        private object activeEdit;
        private IBookMarks bookMarks;
        private Braces.BracesList bracesList;
        private QWhale.Editor.TextSource.BracesOptions bracesOptions;
        private Timer bracesTimer;
        private bool checkSpelling;
        private Point closingBrace;
        private char[] closingBraces;
        private ICodeSnippetRanges codeSnippets;
        private int count;
        private ICodeSnippetRange currentSnippet;
        private IList<ISyntaxEdit> edits;
        private IComparer<ISyntaxError> errComparer;
        private string fileName;
        private int firstChanged;
        private Timer fmtTimer;
        private bool highlightHyperText;
        private HyperTextEventArgs hyperTextArgs;
        private QWhale.Editor.TextSource.IndentOptions indentOptions;
        private string insertStr;
        private int lastChanged;
        private int lastParsed;
        private ILexer lexer;
        private ITextStrings lines;
        private ILineStyles lineStyles;
        private int lockUndoCount;
        private int maxLength;
        private bool modified;
        private QWhale.Editor.TextSource.NavigateOptions navigateOptions;
        private bool needFmtTimer;
        private bool needFormatting;
        private Point openBrace;
        private char[] openBraces;
        private static bool overWrite;
        private int parserLine;
        private Point position;
        private List<Point> positionList;
        private Point prevPosition;
        private bool readOnly;
        private UpdateReason reason;
        private bool redo;
        private IUndoList redoList;
        private int saveModifiedIdx;
        private Rectangle selectBlockRect;
        private bool singleLineMode;
        private int snippetUpdateCount;
        private Point spellSkipPt;
        private Hashtable spellTable;
        private NotifyState state;
        private int stringsUpdateCount;
        private ISyntaxErrors syntaxErrors;
        private Rectangle[] tempBraceRects;
        private IComparer trackChangeComparer;
        private Hashtable trackChanges;
        private UndoFlags undoFlag;
        private int undoLimit;
        private int undoLimitDelta;
        private IUndoList undoList;
        private QWhale.Editor.TextSource.UndoOptions undoOptions;
        private int undoUpdateCount;
        private int updateCount;
        private int updatePositionCount;
        private Hashtable urlTable;
        private WordSpellEventArgs wordSpellArgs;

        [Description("Occurs when a control needs checking whether some string represents hypertext."), Category("TextSource")]
        public event HyperTextEvent HyperText;

        private event EventHandler notifyHandler;

        [Description("Occurs when undo/redo operation performed."), Category("TextSource")]
        public event QWhale.Editor.TextSource.UndoEvent UndoEvent;

        [Description("Occurs when spelling of some word within the text needs checking."), Category("TextSource")]
        public event WordSpellEvent WordSpell;

        public TextSource()
        {
            this.indentOptions = EditConsts.DefaultIndentOptions;
            this.navigateOptions = EditConsts.DefaultNavigateOptions;
            this.undoOptions = EditConsts.DefaultUndoOptions;
            this.saveModifiedIdx = -1;
            this.undoLimitDelta = 0x10;
            this.firstChanged = -1;
            this.lastChanged = -1;
            this.lastParsed = -1;
            this.spellSkipPt = new Point(-1, -1);
            this.openBrace = new Point(-1, -1);
            this.closingBrace = new Point(-1, -1);
            this.openBraces = EditConsts.DefaultOpenBraces;
            this.closingBraces = EditConsts.DefaultClosingBraces;
            this.insertStr = string.Empty;
            this.fileName = string.Empty;
            this.positionList = new List<Point>();
            this.lines = new TextStrings(this);
            this.lines.AddNotifier(this);
            this.undoList = new QWhale.Editor.TextSource.UndoList();
            this.redoList = new QWhale.Editor.TextSource.UndoList();
            this.edits = new List<ISyntaxEdit>();
            this.positionList = new List<Point>();
            this.lineStyles = new QWhale.Editor.TextSource.LineStyles(this);
            this.bookMarks = new QWhale.Editor.TextSource.BookMarks(this);
            this.syntaxErrors = new QWhale.Syntax.SyntaxErrors();
            this.codeSnippets = new CodeSnippetRanges();
            this.errComparer = new ErrComparer();
            this.bracesList = new Braces.BracesList();
            this.trackChangeComparer = new TrackChangeComparer();
            this.hyperTextArgs = new HyperTextEventArgs("", false);
            this.wordSpellArgs = new WordSpellEventArgs("", true, -1);
            TrialVersion.CheckTrialVersion();
        }

        public TextSource(IContainer container) : this()
        {
            container.Add(this);
        }

        public virtual Point AbsolutePositionToTextPoint(int position)
        {
            return this.lines.AbsolutePositionToTextPoint(position);
        }

        public virtual void AddNotifier(INotifier sender)
        {
            this.notifyHandler = (EventHandler) Delegate.Combine(this.notifyHandler, new EventHandler(sender.Notification));
        }

        private void AddTrackChange(ISortList<TrackChange> list, int line, bool saved)
        {
            int num;
            if (this.FindTrackChange(list, line, out num))
            {
                TrackChange local1 = list[num];
                local1.Saved &= saved;
            }
            else
            {
                list.Insert(num, new TrackChange(line, saved));
            }
        }

        public virtual void AddUndo(UndoOperation operation, object data)
        {
            if (this.AllowUndo && (this.undoUpdateCount == 0))
            {
                IUndoData item = new UndoData(operation, data) {
                    Position = this.position,
                    Reason = this.reason,
                    UpdateCount = this.lockUndoCount,
                    UndoFlag = this.undoFlag
                };
                this.GetUndoList().Add(item);
                if ((!this.redo && (this.undoLimit != 0)) && (this.undoList.Count >= this.undoLimit))
                {
                    this.ApplyUndoLimit();
                }
                this.undoFlag = UndoFlags.None;
            }
        }

        private void ApplyUndoLimit()
        {
            if ((this.undoLimit != 0) && (this.undoList.Count >= this.undoLimit))
            {
                int num = Math.Min(this.undoLimitDelta + (this.undoList.Count - this.undoLimit), this.undoList.Count);
                for (int i = 0; i < num; i++)
                {
                    this.undoList.RemoveAt(0);
                }
                this.saveModifiedIdx -= num;
                if (this.saveModifiedIdx < 0)
                {
                    this.saveModifiedIdx = -1;
                    this.modified = this.GetModified(this.undoList, 0, true);
                }
            }
        }

        public virtual int BeginUndoUpdate()
        {
            this.lockUndoCount++;
            return this.lockUndoCount;
        }

        public virtual int BeginUpdate()
        {
            return this.BeginUpdate(UpdateReason.Other);
        }

        public virtual int BeginUpdate(UpdateReason reason)
        {
            if (this.updateCount == 0)
            {
                if ((reason != UpdateReason.Other) && (reason != UpdateReason.Navigate))
                {
                    if (this.bracesOptions != QWhale.Editor.TextSource.BracesOptions.None)
                    {
                        this.UnhighlightBraces(false);
                    }
                    this.UnhighlightSyntaxErrors(this.syntaxErrors, this.position.Y, false);
                    this.needFmtTimer = (this.fmtTimer != null) && this.fmtTimer.Enabled;
                    this.EndFmtTimer();
                }
                if (reason != UpdateReason.Other)
                {
                    this.TempUnhighlightBraces(false);
                }
                this.insertStr = string.Empty;
                this.state = NotifyState.None;
                this.firstChanged = -1;
                this.lastChanged = -1;
                this.prevPosition = this.position;
                this.selectBlockRect = Rectangle.Empty;
                this.reason = reason;
                this.undoFlag = UndoFlags.FirstTime;
                this.count = this.lines.Count;
            }
            this.updateCount++;
            return this.updateCount;
        }

        public virtual int BeginUpdateSnippet()
        {
            this.snippetUpdateCount++;
            return this.SnippetUpdateCount;
        }

        protected void BlockDeleting(Rectangle rect)
        {
            if (((rect.X == 0) || (rect.Height > 0)) && (this.lines.GetLength(rect.Bottom) <= rect.Width))
            {
                rect.Width = 0x7fffffff;
            }
            this.bracesList.BlockDeleting(rect);
            this.syntaxErrors.BlockDeleting(rect);
            this.codeSnippets.BlockDeleting(rect);
            if ((this.snippetUpdateCount == 0) && this.codeSnippets.NeedClear(rect))
            {
                this.UnhighlightCodeSnippets(false);
            }
            if (this.bookMarks.BlockDeleting(rect))
            {
                this.state |= NotifyState.BookMarkChanged;
            }
            if (this.lineStyles.BlockDeleting(rect))
            {
                this.state |= NotifyState.BookMarkChanged;
            }
            if ((this.lexer is ISyntaxParser) && (((ISyntaxParser) this.lexer).Options != SyntaxOptions.None))
            {
                ((ISyntaxParser) this.lexer).SyntaxTree.BlockDeleting(rect);
            }
        }

        protected void BlockDeleting(Point pt, int Len)
        {
            if (this.updatePositionCount <= 0)
            {
                if ((pt.X == 0) && (this.lines[pt.Y] == string.Empty))
                {
                    Len = 0x7fffffff;
                }
                Rectangle rect = new Rectangle(pt.X, pt.Y, Len, 0);
                if (this.bookMarks.BlockDeleting(rect))
                {
                    this.state |= NotifyState.BookMarkChanged;
                }
                if (this.lineStyles.BlockDeleting(rect))
                {
                    this.state |= NotifyState.BookMarkChanged;
                }
                if (this.codeSnippets.BlockDeleting(rect))
                {
                    this.state |= NotifyState.BlockChanged;
                }
            }
        }

        public virtual bool BreakLine()
        {
            if (this.readOnly || this.PositionIsReadonly(this.position))
            {
                return false;
            }
            if (this.maxLength > 0)
            {
                string lineTerminator = this.Lines.LineTerminator;
                this.StripToMaxLength(ref lineTerminator);
                if (lineTerminator == string.Empty)
                {
                    return false;
                }
            }
            if ((this.position.Y == 0) && (this.lines.Count == 0))
            {
                this.lines.Add(string.Empty);
            }
            if (this.singleLineMode)
            {
                return false;
            }
            if (this.position.Y >= this.lines.Count)
            {
                if ((QWhale.Editor.TextSource.NavigateOptions.BeyondEof & this.navigateOptions) == QWhale.Editor.TextSource.NavigateOptions.None)
                {
                    return false;
                }
                this.EnsurePosInsideText();
            }
            this.BeginUpdate(UpdateReason.Break);
            this.stringsUpdateCount++;
            try
            {
                string str2 = this.lines[this.position.Y];
                int length = str2.Length;
                if (this.position.X < length)
                {
                    this.lines.Insert(this.position.Y + 1, str2.Substring(this.position.X, length - this.position.X));
                    this.lines[this.position.Y] = str2.Substring(0, this.position.X);
                }
                else
                {
                    this.lines.Insert(this.position.Y + 1, string.Empty);
                }
                this.AddUndo(UndoOperation.Break, null);
                this.LinesChanged(this.position.Y, 0x7fffffff, true);
                this.PositionChanged(UpdateReason.Break, -this.position.X, 1);
                if ((this.state & NotifyState.Undo) == NotifyState.None)
                {
                    this.RemoveTrailingSpaces(true, true);
                }
            }
            finally
            {
                this.stringsUpdateCount--;
                this.EndUpdate();
            }
            return true;
        }

        public virtual bool CanRedo()
        {
            return (this.AllowUndo && !this.IsEmptyUndoList(this.redoList));
        }

        public virtual bool CanUndo()
        {
            return (this.AllowUndo && !this.IsEmptyUndoList(this.undoList));
        }

        public virtual void Clear()
        {
            this.BeginUpdate();
            try
            {
                this.lines.Clear();
                this.bookMarks.Clear();
                this.lineStyles.Clear();
                this.undoList.Clear();
                this.redoList.Clear();
                this.syntaxErrors.Clear();
                this.codeSnippets.Clear();
            }
            finally
            {
                this.EndUpdate();
            }
        }

        public void ClearRedo()
        {
            this.redoList.Clear();
            if (this.saveModifiedIdx >= this.undoList.Count)
            {
                this.saveModifiedIdx = -1;
            }
        }

        protected void ClearTrackChanges()
        {
            if (this.trackChanges != null)
            {
                this.trackChanges.Clear();
            }
            this.trackChanges = null;
        }

        public virtual void ClearUndo()
        {
            this.undoList.Clear();
        }

        public virtual IStringItem CreateStringItem(string s)
        {
            return new StringItem(s);
        }

        public virtual bool DeleteBlock(Rectangle rect)
        {
            if (this.readOnly)
            {
                return false;
            }
            rect.Height = Math.Min(rect.Bottom, this.lines.Count) - rect.Top;
            if (rect.Height < 0)
            {
                return false;
            }
            for (int i = rect.Top; i <= rect.Bottom; i++)
            {
                if (this.LineIsReadonly(i))
                {
                    return false;
                }
            }
            this.BeginUpdate(UpdateReason.DeleteBlock);
            this.stringsUpdateCount++;
            try
            {
                this.BlockDeleting(rect);
                if (this.notifyHandler != null)
                {
                    this.notifyHandler(this, new BlockDeletingEventArgs(rect));
                }
                this.MoveTo(rect.Left, rect.Top);
                string str = this.lines[rect.Top];
                int length = str.Length;
                int count = rect.Left - length;
                if ((rect.Top == rect.Bottom) && (count > 0))
                {
                    return false;
                }
                int bottom = rect.Bottom;
                if (bottom == this.lines.Count)
                {
                    bottom--;
                }
                string[] data = new string[(bottom - rect.Top) + 1];
                this.AddUndo(UndoOperation.DeleteBlock, data);
                for (int j = rect.Bottom - 1; j > rect.Top; j--)
                {
                    data[j - rect.Top] = this.lines[j];
                    this.lines.RemoveAt(j);
                }
                if (rect.Left < length)
                {
                    if (rect.Top == rect.Bottom)
                    {
                        length = Math.Min(rect.Right, length) - rect.Left;
                    }
                    else
                    {
                        length = str.Length - rect.Left;
                    }
                    if (data.Length > 0)
                    {
                        data[0] = str.Substring(rect.Left, length);
                    }
                    if (rect.Top < this.lines.Count)
                    {
                        this.lines[rect.Top] = str.Remove(rect.Left, length);
                    }
                }
                else if (data.Length > 0)
                {
                    data[0] = string.Empty;
                }
                if (rect.Top != rect.Bottom)
                {
                    str = this.lines[rect.Top + 1];
                    length = Math.Min(str.Length, rect.Right);
                    if ((rect.Bottom - rect.Top) < data.Length)
                    {
                        data[rect.Bottom - rect.Top] = str.Substring(0, length);
                    }
                    if (rect.Top < this.lines.Count)
                    {
                        this.lines[rect.Top] = this.lines[rect.Top] + ((count > 0) ? new string(' ', count) : string.Empty) + str.Remove(0, length);
                        if ((rect.Top + 1) < this.lines.Count)
                        {
                            this.lines.RemoveAt(rect.Top + 1);
                        }
                    }
                }
                this.PositionChanged(UpdateReason.DeleteBlock, -(rect.Right - rect.Left), -(rect.Bottom - rect.Top));
                this.LinesChanged(rect.Top, 0x7fffffff, true);
            }
            finally
            {
                this.stringsUpdateCount--;
                this.EndUpdate();
            }
            return true;
        }

        public virtual bool DeleteBlock(int len)
        {
            Point point = this.IncPosition(this.position, len);
            return this.DeleteBlock(new Rectangle(this.position.X, this.position.Y, point.X - this.position.X, point.Y - this.position.Y));
        }

        public virtual bool DeleteLeft(int len)
        {
            bool flag;
            if (this.readOnly)
            {
                return false;
            }
            this.BeginUpdate(UpdateReason.Delete);
            try
            {
                len = Math.Min(len, this.position.X);
                this.Navigate(-len, 0);
                flag = this.DeleteRight(len);
            }
            finally
            {
                this.EndUpdate();
            }
            return flag;
        }

        public virtual bool DeleteRight(int len)
        {
            if ((this.readOnly || this.PositionIsReadonly(this.position)) || (((len <= 0) || (this.position.Y >= this.lines.Count)) || (this.position.X >= this.lines.GetLength(this.position.Y))))
            {
                return false;
            }
            this.BeginUpdate(UpdateReason.Delete);
            this.stringsUpdateCount++;
            try
            {
                string str = this.lines[this.position.Y];
                len = Math.Min(len, str.Length - this.position.X);
                this.AddUndo(UndoOperation.Delete, str.Substring(this.position.X, len));
                str = str.Remove(this.position.X, len);
                this.lines[this.position.Y] = str;
                this.BlockDeleting(this.position, len);
                if (this.LexStateChanged(this.position.Y))
                {
                    this.LinesChanged(this.position.Y, 0x7fffffff, true);
                }
                else
                {
                    this.LinesChanged(this.position.Y, this.position.Y, true);
                }
                this.PositionChanged(UpdateReason.Delete, -len, 0);
            }
            finally
            {
                this.stringsUpdateCount--;
                this.EndUpdate();
            }
            return true;
        }

        public virtual void DisablePositionUpdate()
        {
            this.updatePositionCount++;
        }

        public virtual int DisableUndo()
        {
            this.undoUpdateCount++;
            return this.undoUpdateCount;
        }

        public virtual int DisableUpdate()
        {
            this.updateCount++;
            return this.updateCount;
        }

        protected void DoCheckSpelling(IStringItem item, int line, bool withUpdate)
        {
            string s = item.String;
            if (s != string.Empty)
            {
                this.InitSpellTable();
                bool flag = ((this.state & NotifyState.Edit) != NotifyState.None) && (this.position.Y == line);
                this.spellSkipPt = flag ? this.position : new Point(-1, -1);
                int start = 0;
                bool flag2 = false;
                bool flag3 = false;
                int length = s.Length;
                if (withUpdate)
                {
                    this.BeginUpdate(UpdateReason.Other);
                }
                try
                {
                    for (int i = 0; i < length; i++)
                    {
                        flag3 = this.spellTable.ContainsKey(s[i]) || this.lines.IsDelimiter(s, i);
                        if (i == 0)
                        {
                            flag2 = flag3;
                        }
                        if (flag2 != flag3)
                        {
                            if (flag3 && ((!flag || (this.spellSkipPt.X <= start)) || (this.spellSkipPt.X > i)))
                            {
                                this.DoCheckSpelling(item, line, start, i, withUpdate);
                            }
                            flag2 = flag3;
                            start = i;
                        }
                    }
                    if (!flag2 && ((!flag || (this.spellSkipPt.X <= start)) || (this.spellSkipPt.X > length)))
                    {
                        this.DoCheckSpelling(item, line, start, length, withUpdate);
                    }
                }
                finally
                {
                    if (withUpdate)
                    {
                        this.EndUpdate();
                    }
                }
            }
        }

        private void DoCheckSpelling(IStringItem item, int line, int start, int end, bool withUpdate)
        {
            if ((start != end) && !this.IsWordCorrect(item.String.Substring(start, end - start), item.TextData[start]))
            {
                item.SetTextStyle(start, end - start, TextStyle.MisSpelledWord);
                if (withUpdate)
                {
                    this.LinesChanged(line, line);
                    this.state |= NotifyState.BlockChanged;
                }
            }
        }

        private void DoFormatText()
        {
            this.BeginUpdate(UpdateReason.Other);
            try
            {
                ISyntaxParser lexer = (ISyntaxParser) this.Lexer;
                lexer.Strings = this.Lines;
                lexer.ReparseText();
                if (this.NeedSyntaxErrors())
                {
                    this.HighlightSyntaxErrors();
                }
                this.state |= NotifyState.TextParsed | NotifyState.Outline | NotifyState.BlockChanged;
            }
            finally
            {
                this.EndUpdate();
            }
        }

        protected void DoHighlightUrls(IStringItem item, int line)
        {
            string str = item.String;
            int length = str.Length;
            int startIndex = 0;
            int num3 = 0;
            int num4 = 0;
            this.InitUrlTable();
            while (startIndex < length)
            {
                num4 = startIndex;
                while (startIndex < length)
                {
                    object obj2 = this.urlTable[str[startIndex]];
                    if ((obj2 != null) && ((bool) obj2))
                    {
                        break;
                    }
                    startIndex++;
                }
                num3 = startIndex;
                while ((num3 < length) && this.urlTable.ContainsKey(str[num3]))
                {
                    num3++;
                }
                if ((num3 != startIndex) && this.IsHyperText(str.Substring(startIndex, num3 - startIndex)))
                {
                    item.SetTextStyle(startIndex, num3 - startIndex, TextStyle.HyperText);
                }
                startIndex = num3;
                if (startIndex == num4)
                {
                    startIndex++;
                }
            }
        }

        private void DoParseBraces(IStringItem item, int line)
        {
            this.bracesList.RemoveBraces(line);
            string str = item.String;
            short[] textData = item.TextData;
            for (int i = 0; i < str.Length; i++)
            {
                if ((this.lexer == null) || !this.lexer.Scheme.IsPlainText(((byte) textData[i]) - 1))
                {
                    char ch = str[i];
                    int index = Array.IndexOf<char>(this.openBraces, ch);
                    if (index >= 0)
                    {
                        this.bracesList.AddBrace(line, i, index, true);
                    }
                    else
                    {
                        index = Array.IndexOf<char>(this.closingBraces, ch);
                        if (index >= 0)
                        {
                            this.bracesList.AddBrace(line, i, index, false);
                        }
                    }
                }
            }
        }

        public virtual void EnablePositionUpdate()
        {
            this.updatePositionCount--;
        }

        public virtual int EnableUndo()
        {
            this.undoUpdateCount--;
            return this.undoUpdateCount;
        }

        public virtual int EnableUpdate()
        {
            this.updateCount--;
            return this.updateCount;
        }

        private void EndBracesTimer()
        {
            if (this.bracesTimer != null)
            {
                this.bracesTimer.Enabled = false;
            }
        }

        private void EndFmtTimer()
        {
            if (this.fmtTimer != null)
            {
                this.fmtTimer.Enabled = false;
            }
        }

        public virtual int EndUndoUpdate()
        {
            this.lockUndoCount--;
            if (this.lockUndoCount == 0)
            {
                this.AddUndo(UndoOperation.UndoBlock, null);
            }
            return this.lockUndoCount;
        }

        public virtual int EndUpdate()
        {
            this.updateCount--;
            if (this.updateCount == 0)
            {
                if (this.position != this.prevPosition)
                {
                    this.state |= NotifyState.PositionChanged;
                }
                if ((this.state & NotifyState.Edit) != NotifyState.None)
                {
                    if (!this.modified)
                    {
                        this.state |= NotifyState.ModifiedChanged;
                    }
                    this.modified = true;
                    if (this.firstChanged >= 0)
                    {
                        this.lastParsed = Math.Min(this.lastParsed, this.firstChanged);
                    }
                }
                if (((this.state & NotifyState.Edit) != NotifyState.None) || ((this.state & NotifyState.ModifiedChanged) != NotifyState.None))
                {
                    this.ClearTrackChanges();
                }
                if (this.lines.Count != this.count)
                {
                    this.state |= NotifyState.CountChanged;
                }
                if (((this.state & NotifyState.Undo) == NotifyState.None) && (((this.state & NotifyState.Edit) != NotifyState.None) || (((this.state & NotifyState.PositionChanged) != NotifyState.None) && this.UndoNavigations)))
                {
                    this.ClearRedo();
                }
                if (this.state != NotifyState.None)
                {
                    bool modified = this.modified;
                    if (this.UndoAfterSave)
                    {
                        if (this.undoList.Count > this.saveModifiedIdx)
                        {
                            this.modified = this.GetModified(this.undoList, this.saveModifiedIdx + 1, true);
                        }
                        else
                        {
                            this.modified = this.GetModified(this.redoList, (this.redoList.Count + this.undoList.Count) - (this.saveModifiedIdx + 1), false);
                        }
                    }
                    else
                    {
                        this.modified = this.GetModified(this.undoList, 0, true);
                    }
                    if (modified != this.modified)
                    {
                        this.state |= NotifyState.ModifiedChanged;
                    }
                    bool flag2 = ((this.needFmtTimer || ((this.state & NotifyState.Edit) != NotifyState.None)) || ((this.state & NotifyState.SyntaxChanged) != NotifyState.None)) || ((this.state & NotifyState.StringsChanged) != NotifyState.None);
                    bool lineChanged = (this.position.Y != this.prevPosition.Y) || ((this.state & NotifyState.CountChanged) != NotifyState.None);
                    if (flag2 || lineChanged)
                    {
                        this.updateCount++;
                        try
                        {
                            this.StartFmtTimer(flag2, lineChanged);
                        }
                        finally
                        {
                            this.updateCount--;
                        }
                    }
                    this.needFmtTimer = false;
                    this.Update();
                }
                if (((this.bracesOptions != QWhale.Editor.TextSource.BracesOptions.None) && ((this.state & NotifyState.PositionChanged) != NotifyState.None)) && ((this.state & NotifyState.Edit) == NotifyState.None))
                {
                    this.HighlightBraces();
                }
                if (((this.checkSpelling && ((this.state & NotifyState.PositionChanged) != NotifyState.None)) && ((this.spellSkipPt.Y >= 0) && (this.spellSkipPt.X >= 0))) && ((this.spellSkipPt.Y == this.prevPosition.Y) && (this.prevPosition.Y != this.position.Y)))
                {
                    IStringItem item = this.lines.GetItem(this.prevPosition.Y);
                    if (item != null)
                    {
                        this.DoCheckSpelling(item, this.prevPosition.Y, true);
                    }
                }
            }
            return this.updateCount;
        }

        public virtual int EndUpdateSnippet()
        {
            this.snippetUpdateCount--;
            return this.snippetUpdateCount;
        }

        private void EnsurePosInsideText()
        {
            if (this.position.Y >= this.lines.Count)
            {
                while (this.position.Y >= this.lines.Count)
                {
                    this.lines.Add(string.Empty);
                }
                this.LinesChanged(this.position.Y, 0x7fffffff, false);
            }
        }

        ~TextSource()
        {
            if (this.fmtTimer != null)
            {
                this.fmtTimer.Enabled = false;
                this.fmtTimer.Dispose();
            }
            if (this.bracesTimer != null)
            {
                this.bracesTimer.Enabled = false;
                this.bracesTimer.Dispose();
            }
            this.ClearUndo();
            this.ClearRedo();
            this.lines.RemoveNotifier(this);
        }

        public virtual bool FindClosingBrace(ref Point position)
        {
            int braceIndex = -1;
            return this.FindClosingBrace(ref position, ref braceIndex);
        }

        private bool FindClosingBrace(ref Point position, ref int braceIndex)
        {
            if (this.closingBraces == null)
            {
                return false;
            }
            return this.bracesList.FindClosingBrace(ref position, ref braceIndex, this.closingBraces.Length);
        }

        public virtual bool FindClosingBrace(ref int x, ref int y)
        {
            Point position = new Point(x, y);
            if (this.FindClosingBrace(ref position))
            {
                x = position.X;
                y = position.Y;
                return true;
            }
            return false;
        }

        public virtual bool FindOpenBrace(ref Point position)
        {
            int braceIndex = -1;
            return this.FindOpenBrace(ref position, ref braceIndex);
        }

        private bool FindOpenBrace(ref Point position, ref int braceIndex)
        {
            if (this.openBraces == null)
            {
                return false;
            }
            return this.bracesList.FindOpenBrace(ref position, ref braceIndex, this.openBraces.Length);
        }

        public virtual bool FindOpenBrace(ref int x, ref int y)
        {
            Point position = new Point(x, y);
            if (this.FindOpenBrace(ref position))
            {
                x = position.X;
                y = position.Y;
                return true;
            }
            return false;
        }

        private bool FindTrackChange(ISortList<TrackChange> list, int line, out int idx)
        {
            return list.FindLast(line, out idx, this.trackChangeComparer);
        }

        public virtual void FormatText()
        {
            this.needFormatting = false;
            this.EndFmtTimer();
            if (this.NeedReparseText())
            {
                this.DoFormatText();
            }
        }

        private string GetAutoIndent()
        {
            if ((this.indentOptions & QWhale.Editor.TextSource.IndentOptions.AutoIndent) == QWhale.Editor.TextSource.IndentOptions.None)
            {
                return string.Empty;
            }
            if (((this.indentOptions & QWhale.Editor.TextSource.IndentOptions.SmartIndent) != QWhale.Editor.TextSource.IndentOptions.None) && this.NeedFormatText())
            {
                int num = Math.Max(((ISyntaxParser) this.lexer).GetSmartIndent(this.Position.Y, true), 0);
                return this.lines.GetIndentString(this.GetTabIndent(num), 0);
            }
            int indent = 0;
            string prevIndentStr = string.Empty;
            prevIndentStr = this.GetPrevIndentStr(out indent);
            if ((this.indentOptions & QWhale.Editor.TextSource.IndentOptions.UsePrevIndent) != QWhale.Editor.TextSource.IndentOptions.None)
            {
                return prevIndentStr;
            }
            return this.lines.GetIndentString(indent, 0);
        }

        public virtual int GetCharIndexFromPosition(Point position)
        {
            return this.lines.TextPointToAbsolutePosition(position);
        }

        public virtual ICodeSnippetRange GetCodeSnippetRangeAt(Point position)
        {
            int num;
            if (!this.codeSnippets.FindSnippet(position, false, out num))
            {
                return null;
            }
            return this.codeSnippets[num];
        }

        private bool GetModified(IList<IUndoData> list, int index, bool saved)
        {
            index = Math.Max(0, index);
            for (int i = list.Count - 1; i >= index; i--)
            {
                IUndoData data = list[i];
                if ((data.Operation != UndoOperation.Navigate) && (data.Operation != UndoOperation.UndoBlock))
                {
                    return true;
                }
            }
            return false;
        }

        public virtual Point GetPositionFromCharIndex(int charIndex)
        {
            return this.lines.AbsolutePositionToTextPoint(charIndex);
        }

        private bool GetPrevIndent(out string indent)
        {
            indent = string.Empty;
            string s = this.Lines[this.position.Y];
            if (s.Trim() != string.Empty)
            {
                int num = s.Length - s.TrimStart(null).Length;
                if (this.position.X <= num)
                {
                    if ((this.indentOptions & QWhale.Editor.TextSource.IndentOptions.UsePrevIndent) != QWhale.Editor.TextSource.IndentOptions.None)
                    {
                        indent = s.Substring(0, this.position.X);
                    }
                    else
                    {
                        indent = this.lines.GetIndentString(this.Lines.TabPosToPos(s, this.position.X), 0);
                    }
                    return true;
                }
            }
            return false;
        }

        private string GetPrevIndentStr(out int indent)
        {
            indent = 0;
            for (int i = this.position.Y - 1; i >= 0; i--)
            {
                string s = this.Lines[i];
                if (s.Trim() != string.Empty)
                {
                    int pos = s.Length - s.TrimStart(null).Length;
                    indent = this.Lines.TabPosToPos(s, pos);
                    return s.Substring(0, pos);
                }
            }
            return string.Empty;
        }

        private string GetSnippetValue(ICodeSnippetRange range)
        {
            string str = this.Lines[range.StartPoint.Y];
            if ((range.StartPoint.X < 0) || (range.StartPoint.X >= str.Length))
            {
                return string.Empty;
            }
            if (range.EndPoint.Y != range.StartPoint.Y)
            {
                return str.Substring(range.StartPoint.X);
            }
            if (range.StartPoint.X > range.EndPoint.X)
            {
                return string.Empty;
            }
            return str.Substring(range.StartPoint.X, Math.Min(range.EndPoint.X, str.Length) - range.StartPoint.X);
        }

        public virtual ISyntaxError GetSyntaxErrorAt(int x, int y)
        {
            int num;
            if (!this.syntaxErrors.FindErrorAt(new Point(x, y), false, out num, this.errComparer))
            {
                return null;
            }
            return this.syntaxErrors[num];
        }

        private int GetTabIndent(int indent)
        {
            return this.lines.TabPosToPos(new string('\t', indent), indent);
        }

        protected IList<IUndoData> GetUndoList()
        {
            if (this.redo)
            {
                return this.redoList;
            }
            return this.undoList;
        }

        public virtual void HighlightBraces()
        {
            this.HighlightBraces(true);
        }

        protected virtual void HighlightBraces(bool needUpdate)
        {
            this.HighlightBraces(this.position, needUpdate);
        }

        private bool HighlightBraces(Point position, bool needUpdate)
        {
            Point openBrace = this.openBrace;
            Point closingBrace = this.closingBrace;
            Point point3 = position;
            this.openBrace = new Point(-1, -1);
            this.closingBrace = new Point(-1, -1);
            bool flag = false;
            bool open = false;
            if (((position.X > 0) && this.bracesList.FindBrace(new Point(position.X - 1, position.Y), out open)) && !open)
            {
                point3.X--;
            }
            else if (this.bracesList.FindBrace(position, out open) && open)
            {
                point3.X++;
            }
            int braceIndex = -1;
            if (this.FindOpenBrace(ref point3, ref braceIndex))
            {
                Point point4 = point3;
                point4.X++;
                if (this.FindClosingBrace(ref point4, ref braceIndex) && ((((this.bracesOptions & QWhale.Editor.TextSource.BracesOptions.HighlightBounds) == QWhale.Editor.TextSource.BracesOptions.None) || position.Equals(point3)) || position.Equals(new Point(point4.X + 1, point4.Y))))
                {
                    this.openBrace = point3;
                    this.closingBrace = point4;
                    flag = true;
                }
            }
            bool flag3 = (this.openBrace != openBrace) || (this.closingBrace != closingBrace);
            if (flag3)
            {
                this.UpdateBrace(openBrace, false, needUpdate);
                this.UpdateBrace(closingBrace, false, needUpdate);
                this.UpdateBraces(needUpdate);
            }
            if (flag && ((this.bracesOptions & QWhale.Editor.TextSource.BracesOptions.TempHighlight) != QWhale.Editor.TextSource.BracesOptions.None))
            {
                this.StartBracesTimer();
            }
            return flag3;
        }

        public virtual void HighlightCodeSnippets()
        {
            this.HighlightCodeSnippets(true);
        }

        protected virtual void HighlightCodeSnippets(bool needUpdate)
        {
            for (int i = 0; i < this.codeSnippets.Count; i++)
            {
                if (this.codeSnippets.IsFirstSnippet(i))
                {
                    IRange range = this.codeSnippets[i];
                    this.SetTextStyle(new Rectangle(range.StartPoint, new Size(range.EndPoint.X - range.StartPoint.X, range.EndPoint.Y - range.StartPoint.Y)), TextStyle.CodeSnippet, true, needUpdate);
                }
            }
        }

        public void HighlightSyntaxErrors()
        {
            ISyntaxParser lexer = this.Lexer as ISyntaxParser;
            ISyntaxErrors errors = new QWhale.Syntax.SyntaxErrors();
            lexer.GetSyntaxErrors(errors);
            errors.Sort(this.errComparer);
            foreach (ISyntaxError error in errors)
            {
                int num;
                if (this.syntaxErrors.FindErrorAt(error.Position, true, out num, this.errComparer) && this.syntaxErrors[num].Range.EndPoint.Equals(error.Range.EndPoint))
                {
                    this.syntaxErrors.RemoveAt(num);
                }
            }
            this.BeginUpdate(UpdateReason.Other);
            try
            {
                if (errors.Count > 0)
                {
                    this.ParseToString(Math.Min(errors[errors.Count - 1].Position.Y, this.lines.Count - 1));
                }
                this.UnhighlightSyntaxErrors(this.syntaxErrors, false);
                this.syntaxErrors.Clear();
                foreach (ISyntaxError error2 in errors)
                {
                    this.syntaxErrors.Add((ISyntaxError) error2.Clone());
                }
                foreach (ISyntaxError error3 in this.syntaxErrors)
                {
                    this.SetTextStyle(new Rectangle(error3.Position, error3.Size), TextStyle.WaveLine, true, true);
                }
            }
            finally
            {
                this.EndUpdate();
            }
        }

        protected Point IncPosition(Point position, int len)
        {
            int count = this.lines.Count;
            int num2 = 0;
            Point point = position;
            while (len > 0)
            {
                if (position.Y < count)
                {
                    num2 = (point.Y < count) ? this.lines.GetLength(point.Y) : 0;
                    if (point.Y == position.Y)
                    {
                        num2 -= position.X;
                    }
                    num2 = Math.Max(num2, 0);
                }
                if (len > num2)
                {
                    point.Y++;
                }
                else
                {
                    point.X = len;
                    if (position.Y == point.Y)
                    {
                        point.X += position.X;
                    }
                    return point;
                }
                len -= num2 + 2;
                if ((len <= 0) && (position.Y != point.Y))
                {
                    point.X = Math.Max(len, 0);
                }
            }
            return point;
        }

        public virtual void IndentLine()
        {
            this.IndentLine(this.GetAutoIndent());
        }

        protected void IndentLine(string indent)
        {
            if (indent != string.Empty)
            {
                if ((this.indentOptions & QWhale.Editor.TextSource.IndentOptions.JumpToIndent) != QWhale.Editor.TextSource.IndentOptions.None)
                {
                    QWhale.Editor.TextSource.NavigateOptions navigateOptions = this.navigateOptions;
                    try
                    {
                        this.navigateOptions |= QWhale.Editor.TextSource.NavigateOptions.BeyondEol;
                        this.Navigate(this.lines.TabPosToPos(indent, indent.Length), 0);
                        return;
                    }
                    finally
                    {
                        this.navigateOptions = navigateOptions;
                    }
                }
                this.Insert(indent);
            }
        }

        private void InitSpellTable()
        {
            if (this.spellTable == null)
            {
                this.spellTable = new Hashtable();
                foreach (char ch in EditConsts.DefaultSpellDelimiters.ToCharArray())
                {
                    this.spellTable.Add(ch, ch);
                }
            }
        }

        private void InitUrlTable()
        {
            if (this.urlTable == null)
            {
                this.urlTable = new Hashtable();
                for (char ch = 'A'; ch <= 'Z'; ch = (char) (ch + '\x0001'))
                {
                    this.urlTable.Add(ch, true);
                }
                for (char ch2 = 'a'; ch2 <= 'z'; ch2 = (char) (ch2 + '\x0001'))
                {
                    this.urlTable.Add(ch2, true);
                }
                for (char ch3 = '0'; ch3 <= '9'; ch3 = (char) (ch3 + '\x0001'))
                {
                    this.urlTable.Add(ch3, false);
                }
                foreach (char ch4 in EditConsts.DefaultUrlChars.ToCharArray())
                {
                    this.urlTable.Add(ch4, false);
                }
            }
        }

        public virtual bool Insert(string text)
        {
            if (this.readOnly || this.PositionIsReadonly(this.position))
            {
                return false;
            }
            this.StripToMaxLength(ref text);
            this.insertStr = text;
            this.BeginUpdate(UpdateReason.Insert);
            this.stringsUpdateCount++;
            try
            {
                this.EnsurePosInsideText();
                string s = this.lines[this.position.Y];
                int length = s.Length;
                if (this.position.X > length)
                {
                    text = this.lines.GetIndentString(this.position.X - length, this.Lines.TabPosToPos(s, length)) + text;
                    this.MoveTo(length, this.position.Y);
                }
                this.lines[this.position.Y] = s.Insert(this.position.X, text);
                length = text.Length;
                this.AddUndo(UndoOperation.Insert, length);
                if (this.LexStateChanged(this.position.Y))
                {
                    this.LinesChanged(this.position.Y, 0x7fffffff, true);
                }
                else
                {
                    this.LinesChanged(this.position.Y, this.position.Y, true);
                }
                this.PositionChanged(UpdateReason.Insert, length, 0);
                if (!this.UndoNavigations || ((this.state & NotifyState.Undo) == NotifyState.None))
                {
                    if ((this.state & NotifyState.Undo) != NotifyState.None)
                    {
                        this.DisableUndo();
                        try
                        {
                            this.Navigate(length, 0);
                            goto Label_0199;
                        }
                        finally
                        {
                            this.EnableUndo();
                        }
                    }
                    this.Navigate(length, 0);
                }
            }
            finally
            {
                this.stringsUpdateCount--;
                this.EndUpdate();
            }
        Label_0199:
            return true;
        }

        public virtual bool InsertBlock(string text)
        {
            return this.InsertBlock(StringItem.Split(text));
        }

        public virtual bool InsertBlock(string[] strings)
        {
            return this.InsertBlock(strings, false);
        }

        public virtual bool InsertBlock(ITextStrings strings)
        {
            if (!this.readOnly)
            {
                string[] array = new string[strings.Count];
                strings.CopyTo(array, 0);
                return this.InsertBlock(array);
            }
            return false;
        }

        public virtual bool InsertBlock(string[] strings, bool select)
        {
            if ((this.readOnly || this.PositionIsReadonly(this.position)) || (strings.Length == 0))
            {
                return false;
            }
            if (this.singleLineMode)
            {
                strings = new string[] { strings[0] };
            }
            this.StripToMaxLength(ref strings);
            this.BeginUpdate(UpdateReason.InsertBlock);
            this.stringsUpdateCount++;
            try
            {
                Point position = this.Position;
                this.EnsurePosInsideText();
                string s = this.lines[this.position.Y];
                int length = s.Length;
                string str2 = string.Empty;
                if (this.position.X > length)
                {
                    strings[0] = this.lines.GetIndentString(this.position.X - length, this.Lines.TabPosToPos(s, length)) + strings[0];
                    this.MoveTo(length, this.position.Y);
                }
                length = 0;
                foreach (string str3 in strings)
                {
                    length += str3.Length + 2;
                }
                length -= 2;
                this.AddUndo(UndoOperation.InsertBlock, new Point(length, strings.Length - 1));
                for (int i = 0; i < strings.Length; i++)
                {
                    if (i == 0)
                    {
                        string str4 = this.lines[this.position.Y];
                        length = str4.Length;
                        if (this.position.X < length)
                        {
                            str2 = str4.Substring(this.position.X, length - this.Position.X);
                            str4 = str4.Substring(0, this.position.X) + strings[0];
                        }
                        else
                        {
                            str4 = str4 + strings[0];
                        }
                        if (i == (strings.Length - 1))
                        {
                            str4 = str4 + str2;
                        }
                        this.lines[this.position.Y] = str4;
                    }
                    else if (i == (strings.Length - 1))
                    {
                        this.lines.Insert(this.position.Y + i, strings[i] + str2);
                    }
                    else
                    {
                        this.lines.Insert(this.position.Y + i, strings[i]);
                    }
                }
                this.LinesChanged(this.position.Y, 0x7fffffff, true);
                int deltaY = strings.Length - 1;
                int deltaX = strings[strings.Length - 1].Length;
                if (deltaY > 0)
                {
                    deltaX -= this.Position.X;
                }
                this.PositionChanged(UpdateReason.InsertBlock, deltaX, deltaY);
                if (!this.UndoNavigations || ((this.state & NotifyState.Undo) == NotifyState.None))
                {
                    this.Navigate(deltaX, deltaY);
                }
                if (select)
                {
                    this.state |= NotifyState.SelectBlock;
                    this.selectBlockRect = new Rectangle(position, new Size(this.position.X - position.X, this.position.Y - position.Y));
                }
            }
            finally
            {
                this.stringsUpdateCount--;
                this.EndUpdate();
            }
            return true;
        }

        public virtual bool InsertFromFile(string fileName)
        {
            if (this.readOnly)
            {
                return false;
            }
            ITextStrings strings = new TextStrings(null);
            strings.LoadFile(fileName);
            return this.InsertBlock(strings);
        }

        private void InvalidateSnippets()
        {
            if (this.codeSnippets.Count > 0)
            {
                this.BeginUpdate(UpdateReason.Other);
                try
                {
                    this.LinesChanged(this.codeSnippets[0].StartPoint.Y, this.codeSnippets[this.codeSnippets.Count - 1].EndPoint.Y, false);
                    this.state |= NotifyState.BlockChanged;
                }
                finally
                {
                    this.EndUpdate();
                }
            }
        }

        protected bool IsEmptyUndoList(IList<IUndoData> list)
        {
            if (this.UndoNavigations)
            {
                return (list.Count == 0);
            }
            for (int i = list.Count - 1; i >= 0; i--)
            {
                IUndoData data = list[i];
                if (data.Operation != UndoOperation.Navigate)
                {
                    return false;
                }
            }
            return true;
        }

        protected bool IsFirstSnippet(Point position)
        {
            int num;
            return (this.codeSnippets.FindSnippet(position, false, out num) && this.codeSnippets.IsFirstSnippet(num));
        }

        public virtual bool IsHyperText(string text)
        {
            bool isHyperText = QWhale.Editor.TextSource.HyperText.IsHyperText(text);
            if (this.HyperText != null)
            {
                this.hyperTextArgs.Text = text;
                this.hyperTextArgs.IsHyperText = isHyperText;
                this.HyperText(this, this.hyperTextArgs);
                isHyperText = this.hyperTextArgs.IsHyperText;
            }
            return isHyperText;
        }

        public virtual bool IsWordCorrect(string text)
        {
            return this.IsWordCorrect(text, 0);
        }

        protected bool IsWordCorrect(string text, int colorStyle)
        {
            bool correct = true;
            if (this.WordSpell != null)
            {
                this.wordSpellArgs.Text = text;
                this.wordSpellArgs.Correct = correct;
                this.wordSpellArgs.ColorStyle = colorStyle;
                this.WordSpell(this, this.wordSpellArgs);
                correct = this.wordSpellArgs.Correct;
            }
            return correct;
        }

        private void LexerChanged()
        {
            this.BeginUpdate(UpdateReason.Other);
            try
            {
                this.lastParsed = -1;
                this.state |= NotifyState.SyntaxChanged;
                this.UpdateParsed(0, this.lines.Count - 1);
                this.bracesList.Clear();
                this.UnhighlightSyntaxErrors(this.syntaxErrors, false);
                this.UnhighlightCodeSnippets(false);
            }
            finally
            {
                this.EndUpdate();
            }
        }

        protected bool LexStateChanged(int line)
        {
            if (((this.lexer == null) || (line < 0)) || (line >= this.lines.Count))
            {
                return false;
            }
            int lexState = this.lines.GetItem(line).LexState;
            int state = ((line > 0) && ((line - 1) < this.lines.Count)) ? this.lines.GetItem(line - 1).LexState : this.lexer.DefaultState;
            IStringItem item = this.lines.GetItem(line);
            return (this.ParseText(state, item, line, true) != lexState);
        }

        public virtual bool LineIsModified(int index)
        {
            bool saved = false;
            return this.LineIsModified(index, out saved);
        }

        public virtual bool LineIsModified(int index, out bool saved)
        {
            if (this.trackChanges == null)
            {
                this.TrackChanges();
            }
            object obj2 = this.trackChanges[index];
            saved = (obj2 != null) ? ((bool) obj2) : false;
            return (obj2 != null);
        }

        public virtual bool LineIsReadonly(int index)
        {
            IStringItem item = this.lines.GetItem(index);
            return ((item != null) && (((byte) (item.State & (ItemState.None | ItemState.Readonly))) != 0));
        }

        public virtual void LinesChanged(int first, int last)
        {
            this.LinesChanged(first, last, false);
        }

        public virtual void LinesChanged(int first, int last, bool modified)
        {
            if (this.firstChanged == -1)
            {
                this.firstChanged = first;
            }
            else
            {
                this.firstChanged = Math.Min(this.firstChanged, first);
            }
            this.lastChanged = Math.Max(this.lastChanged, last);
            if (modified)
            {
                this.state |= NotifyState.Modified | NotifyState.Edit;
            }
        }

        public virtual bool LoadFile(string fileName)
        {
            return this.lines.LoadFile(fileName);
        }

        public virtual bool LoadFile(string fileName, IStringImport importer)
        {
            return this.lines.LoadFile(fileName, importer);
        }

        public virtual bool LoadFile(string fileName, Encoding encoding)
        {
            return this.lines.LoadFile(fileName, encoding);
        }

        public virtual bool LoadFile(string fileName, IStringImport importer, Encoding encoding)
        {
            return this.lines.LoadFile(fileName, importer, encoding);
        }

        public virtual bool LoadStream(Stream stream)
        {
            return this.lines.LoadStream(stream);
        }

        public virtual bool LoadStream(TextReader reader)
        {
            return this.lines.LoadStream(reader);
        }

        public virtual bool LoadStream(Stream stream, IStringImport importer)
        {
            return this.lines.LoadStream(stream, importer);
        }

        public virtual bool LoadStream(Stream stream, Encoding encoding)
        {
            return this.lines.LoadStream(stream, encoding);
        }

        public virtual bool LoadStream(TextReader reader, IStringImport importer)
        {
            return this.lines.LoadStream(reader, importer);
        }

        public virtual bool LoadStream(Stream stream, IStringImport importer, Encoding encoding)
        {
            return this.lines.LoadStream(stream, importer, encoding);
        }

        protected void ModifiedChanged()
        {
            if (!this.modified)
            {
                if (this.UndoAfterSave)
                {
                    for (int i = 0; i < this.undoList.Count; i++)
                    {
                        IUndoData local1 = this.undoList[i];
                        local1.UndoFlag = (UndoFlags) ((byte) (local1.UndoFlag | (UndoFlags.None | UndoFlags.Saved)));
                    }
                    for (int j = 0; j < this.redoList.Count; j++)
                    {
                        IUndoData local2 = this.redoList[j];
                        local2.UndoFlag = (UndoFlags) ((byte) (((int) local2.UndoFlag) & 0xfd));
                    }
                    this.saveModifiedIdx = this.GetUndoList().Count - 1;
                }
                else
                {
                    this.ClearUndo();
                    this.ClearRedo();
                }
            }
            this.state |= NotifyState.ModifiedChanged;
        }

        public virtual void MoveTo(Point position)
        {
            this.MoveTo(position.X, position.Y);
        }

        public virtual void MoveTo(int x, int y)
        {
            this.Navigate(x - this.position.X, y - this.position.Y);
        }

        public virtual void MoveToChar(int x)
        {
            this.Navigate(x - this.position.X, 0);
        }

        public virtual void MoveToLine(int y)
        {
            this.Navigate(0, y - this.position.Y);
        }

        public virtual void MoveToLine(int y, int linesAbove)
        {
            this.BeginUpdate();
            try
            {
                this.MoveToLine(y);
                if ((this.activeEdit != null) && (this.activeEdit is ISyntaxEdit))
                {
                    ((ISyntaxEdit) this.activeEdit).Scrolling.WindowOriginY = Math.Max(y - linesAbove, 0);
                }
            }
            finally
            {
                this.EndUpdate();
            }
        }

        public virtual void Navigate(int deltaX, int deltaY)
        {
            if ((deltaX != 0) || (deltaY != 0))
            {
                this.BeginUpdate(UpdateReason.Navigate);
                try
                {
                    if (deltaY != 0)
                    {
                        this.RemoveTrailingSpaces((QWhale.Editor.TextSource.NavigateOptions.BeyondEof & this.navigateOptions) == QWhale.Editor.TextSource.NavigateOptions.None, false);
                    }
                    Point position = this.position;
                    this.position.Offset(deltaX, deltaY);
                    this.ValidatePosition(ref this.position);
                    if (this.UndoNavigations)
                    {
                        this.AddUndo(UndoOperation.Navigate, new Point(this.position.X - position.X, this.position.Y - position.Y));
                    }
                    else
                    {
                        this.AddUndo(UndoOperation.Navigate, null);
                    }
                }
                finally
                {
                    this.EndUpdate();
                }
            }
        }

        public virtual bool NeedAutoComplete()
        {
            return ((this.lexer is ISyntaxParser) && ((((ISyntaxParser) this.lexer).Options & SyntaxOptions.AutoComplete) != SyntaxOptions.None));
        }

        public virtual bool NeedCodeCompletion()
        {
            return ((this.lexer is ISyntaxParser) && ((((ISyntaxParser) this.lexer).Options & SyntaxOptions.CodeCompletion) != SyntaxOptions.None));
        }

        public virtual bool NeedCodeCompletionTabs()
        {
            return ((this.lexer is ISyntaxParser) && ((((ISyntaxParser) this.lexer).Options & SyntaxOptions.CodeCompletionTabs) != SyntaxOptions.None));
        }

        public virtual bool NeedFormatText()
        {
            return ((this.lexer is ISyntaxParser) && ((((ISyntaxParser) this.lexer).Options & SyntaxOptions.SmartIndent) != SyntaxOptions.None));
        }

        public virtual bool NeedOutlineText()
        {
            return ((this.lexer is ISyntaxParser) && ((((ISyntaxParser) this.lexer).Options & SyntaxOptions.Outline) != SyntaxOptions.None));
        }

        public virtual bool NeedParse()
        {
            if (((this.lexer == null) && !this.checkSpelling) && !this.highlightHyperText)
            {
                return (this.bracesOptions != QWhale.Editor.TextSource.BracesOptions.None);
            }
            return true;
        }

        public virtual bool NeedQuickInfoTips()
        {
            return ((this.lexer is ISyntaxParser) && ((((ISyntaxParser) this.lexer).Options & SyntaxOptions.QuickInfoTips) != SyntaxOptions.None));
        }

        public virtual bool NeedReparseText()
        {
            return ((this.lexer is ISyntaxParser) && (((ISyntaxParser) this.lexer).Options != SyntaxOptions.None));
        }

        public virtual bool NeedReparseTextOnLineChange()
        {
            return ((this.lexer is ISyntaxParser) && ((((ISyntaxParser) this.lexer).Options & SyntaxOptions.ReparseOnLineChange) != SyntaxOptions.None));
        }

        public virtual bool NeedSyntaxErrors()
        {
            return ((this.lexer is ISyntaxParser) && ((((ISyntaxParser) this.lexer).Options & SyntaxOptions.SyntaxErrors) != SyntaxOptions.None));
        }

        public virtual bool NewLine()
        {
            bool flag3;
            if (this.singleLineMode)
            {
                return false;
            }
            this.BeginUpdate(UpdateReason.Break);
            try
            {
                string str;
                bool prevIndent = this.GetPrevIndent(out str);
                bool flag2 = this.BreakLine();
                this.MoveTo(0, this.Position.Y + 1);
                if (prevIndent)
                {
                    this.IndentLine(str);
                }
                else
                {
                    this.IndentLine();
                }
                flag3 = flag2;
            }
            finally
            {
                this.EndUpdate();
            }
            return flag3;
        }

        public virtual bool NewLineAbove()
        {
            bool flag2;
            if (this.singleLineMode)
            {
                return false;
            }
            this.BeginUpdate(UpdateReason.Break);
            try
            {
                this.MoveToChar(0);
                bool flag = this.BreakLine();
                string autoIndent = this.GetAutoIndent();
                if (autoIndent != string.Empty)
                {
                    this.Insert(autoIndent);
                }
                flag2 = flag;
            }
            finally
            {
                this.EndUpdate();
            }
            return flag2;
        }

        public virtual bool NewLineBelow()
        {
            bool flag2;
            if (this.singleLineMode)
            {
                return false;
            }
            this.BeginUpdate(UpdateReason.Break);
            try
            {
                this.MoveToChar(this.Lines[this.Position.Y].Length);
                bool flag = this.BreakLine();
                this.MoveTo(0, this.Position.Y + 1);
                string autoIndent = this.GetAutoIndent();
                if (autoIndent != string.Empty)
                {
                    this.Insert(autoIndent);
                }
                flag2 = flag;
            }
            finally
            {
                this.EndUpdate();
            }
            return flag2;
        }

        public virtual void Notification(object sender, EventArgs e)
        {
            if (sender is ITextStrings)
            {
                this.StringsChanged(sender, e);
            }
            else if (sender is ILexer)
            {
                this.LexerChanged();
            }
        }

        public virtual void Notify()
        {
            if (this.notifyHandler != null)
            {
                this.notifyHandler(this, EventArgs.Empty);
            }
        }

        protected virtual void OnActiveEditChanged()
        {
        }

        protected virtual void OnBookmarksChanged()
        {
        }

        protected virtual void OnBracesOptionsChanged()
        {
            this.LexerChanged();
            if ((this.bracesOptions & QWhale.Editor.TextSource.BracesOptions.TempHighlight) == QWhale.Editor.TextSource.BracesOptions.None)
            {
                this.UpdateTempBraces(false, true);
            }
            if ((this.bracesOptions & QWhale.Editor.TextSource.BracesOptions.Highlight) != QWhale.Editor.TextSource.BracesOptions.None)
            {
                this.HighlightBraces();
            }
        }

        protected virtual void OnCheckSpellingChanged()
        {
            this.LexerChanged();
        }

        protected virtual void OnClosingBracesChanged()
        {
            if (this.bracesOptions != QWhale.Editor.TextSource.BracesOptions.None)
            {
                this.LexerChanged();
            }
        }

        protected virtual void OnFileNameChanged()
        {
        }

        protected void OnFormatting(object source, EventArgs e)
        {
            this.EndFmtTimer();
            if (this.NeedReparseText())
            {
                this.FormatText();
            }
        }

        protected virtual void OnHighlightHyperTextChanged()
        {
            this.LexerChanged();
        }

        protected virtual void OnIndentOptionsChanged()
        {
        }

        protected virtual void OnLinesChanged()
        {
        }

        protected virtual void OnLineStylesChanged()
        {
        }

        protected virtual void OnMaxLengthChanged()
        {
        }

        protected virtual void OnModifiedChanged()
        {
            this.BeginUpdate(UpdateReason.Other);
            try
            {
                this.ModifiedChanged();
            }
            finally
            {
                this.EndUpdate();
            }
        }

        protected virtual void OnNavigateOptionsChanged()
        {
            this.ValidatePosition(ref this.position);
        }

        protected virtual void OnOpenBracesChanged()
        {
            if (this.bracesOptions != QWhale.Editor.TextSource.BracesOptions.None)
            {
                this.LexerChanged();
            }
        }

        protected virtual void OnOverWriteChanged()
        {
            this.BeginUpdate(UpdateReason.Other);
            try
            {
                this.State |= this.State | NotifyState.OverWriteChanged;
            }
            finally
            {
                this.EndUpdate();
            }
        }

        protected virtual void OnReadonlyChanged()
        {
            this.BeginUpdate(UpdateReason.Other);
            try
            {
                this.State |= NotifyState.ReadonlyChanged;
            }
            finally
            {
                this.EndUpdate();
            }
        }

        protected virtual void OnSingleLineModeChanged()
        {
            if (this.singleLineMode && (this.lines.Count > 1))
            {
                this.DeleteBlock(new Rectangle(0, 1, 0, this.lines.Count - 1));
                this.MoveToLine(0);
            }
        }

        protected virtual void OnStateChanged()
        {
        }

        protected virtual void OnTextChanged()
        {
        }

        protected virtual void OnUndoLimitChanged()
        {
            this.ApplyUndoLimit();
        }

        protected virtual void OnUndoOptionsChanged()
        {
        }

        protected void OnUnhighlightBraces(object source, EventArgs e)
        {
            if (this.bracesOptions != QWhale.Editor.TextSource.BracesOptions.None)
            {
                this.UnhighlightBraces();
            }
            this.TempUnhighlightBraces();
        }

        public virtual void ParseString(int index)
        {
            this.ParseStrings(index, index);
        }

        public virtual void ParseStrings(int first, int last)
        {
            if (this.NeedParse())
            {
                int state = ((first > 0) && ((first - 1) < this.lines.Count)) ? this.lines.GetItem(first - 1).LexState : ((this.lexer != null) ? this.lexer.DefaultState : 0);
                first = Math.Max(first, 0);
                last = Math.Min(last, this.lines.Count - 1);
                for (int i = first; i <= last; i++)
                {
                    IStringItem item = this.lines.GetItem(i);
                    if ((((byte) (item.State & (ItemState.None | ItemState.Parsed))) == 0) || (item.PrevLexState != state))
                    {
                        item.PrevLexState = state;
                        state = this.ParseText(state, item, i, false);
                        item.LexState = state;
                        item.State = (ItemState) ((byte) (item.State | (ItemState.None | ItemState.Parsed)));
                    }
                    else
                    {
                        state = item.LexState;
                    }
                }
            }
        }

        protected int ParseText(int state, IStringItem item, int line, bool parseTextOnly)
        {
            this.parserLine = line;
            int num = state;
            if (this.lexer != null)
            {
                short[] textData = item.TextData;
                num = this.lexer.ParseText(state, line, item.String, ref textData);
                item.TextData = textData;
            }
            if (!parseTextOnly)
            {
                if (this.highlightHyperText)
                {
                    this.DoHighlightUrls(item, line);
                }
                if (this.checkSpelling && (this.WordSpell != null))
                {
                    this.DoCheckSpelling(item, line, false);
                }
                if (this.bracesOptions != QWhale.Editor.TextSource.BracesOptions.None)
                {
                    this.DoParseBraces(item, line);
                }
            }
            return num;
        }

        public virtual void ParseToString(int index)
        {
            if (this.lastParsed < index)
            {
                this.ParseStrings(this.lastParsed, index);
                this.lastParsed = index + 1;
                if ((this.bracesOptions != QWhale.Editor.TextSource.BracesOptions.None) && !this.HighlightBraces(this.Position, false))
                {
                    this.UpdateBraces(false);
                }
                if (this.tempBraceRects != null)
                {
                    this.UpdateTempBraces(true, false);
                }
                this.HighlightCodeSnippets(false);
            }
        }

        protected void PositionChanged(UpdateReason reason, int deltaX, int deltaY)
        {
            if (this.updatePositionCount <= 0)
            {
                if (this.notifyHandler != null)
                {
                    this.notifyHandler(this, new PositionChangedEventArgs(reason, deltaX, deltaY));
                }
                this.bracesList.PositionChanged(this.Position.X, this.Position.Y, deltaX, deltaY);
                this.syntaxErrors.PositionChanged(this.Position.X, this.Position.Y, deltaX, deltaY);
                this.codeSnippets.PositionChanged(this.Position.X, this.Position.Y, deltaX, deltaY, (this.state & NotifyState.SmartFormat) != NotifyState.None);
                if ((this.snippetUpdateCount == 0) && this.codeSnippets.NeedClear(this.Position.Y))
                {
                    this.UnhighlightCodeSnippets(false);
                }
                if (this.bookMarks.PositionChanged(this.Position.X, this.Position.Y, deltaX, deltaY))
                {
                    this.state |= NotifyState.BookMarkChanged;
                }
                if (this.lineStyles.PositionChanged(this.Position.X, this.Position.Y, deltaX, deltaY))
                {
                    this.state |= NotifyState.BookMarkChanged;
                }
                if ((this.lexer is ISyntaxParser) && (((ISyntaxParser) this.lexer).Options != SyntaxOptions.None))
                {
                    ((ISyntaxParser) this.lexer).SyntaxTree.PositionChanged(this.Position.X, this.Position.Y, deltaX, deltaY);
                }
                for (int i = 0; i < this.positionList.Count; i++)
                {
                    Point pt = this.positionList[i];
                    QWhale.Common.Range.UpdatePos(this.Position.X, this.Position.Y, deltaX, deltaY, ref pt, false);
                    this.positionList[i] = pt;
                }
            }
        }

        public virtual bool PositionIsReadonly(Point position)
        {
            return this.LineIsReadonly(position.Y);
        }

        public virtual bool ProcessAutoComplete(out string code)
        {
            code = string.Empty;
            return (this.NeedAutoComplete() && ((ISyntaxParser) this.lexer).ProcessAutoComplete(this.Lines[this.prevPosition.Y], this.prevPosition, out code));
        }

        public virtual void Redo()
        {
            this.redo = !this.redo;
            try
            {
                this.Undo();
            }
            finally
            {
                this.redo = !this.redo;
            }
        }

        public virtual void RemoveNotifier(INotifier sender)
        {
            this.notifyHandler = (EventHandler) Delegate.Remove(this.notifyHandler, new EventHandler(sender.Notification));
        }

        protected void RemoveTrailingSpaces(bool addUndo, bool lineEnd)
        {
            if (this.lines.RemoveTrailingSpaces)
            {
                string str = this.lines[this.position.Y];
                string str2 = str.TrimEnd(new char[0]);
                int len = str.Length - str2.Length;
                if (len > 0)
                {
                    this.BeginUpdate(UpdateReason.Delete);
                    try
                    {
                        if (addUndo)
                        {
                            QWhale.Editor.TextSource.NavigateOptions navigateOptions = this.navigateOptions;
                            try
                            {
                                this.navigateOptions |= QWhale.Editor.TextSource.NavigateOptions.BeyondEol;
                                int x = this.position.X;
                                this.MoveToChar(str2.Length);
                                this.DeleteRight(len);
                                if (lineEnd)
                                {
                                    this.position.X = Math.Min(x, str2.Length);
                                }
                                else
                                {
                                    this.position.X = x;
                                }
                            }
                            finally
                            {
                                this.navigateOptions = navigateOptions;
                            }
                        }
                        else
                        {
                            this.stringsUpdateCount++;
                            try
                            {
                                this.lines[this.position.Y] = str2;
                            }
                            finally
                            {
                                this.stringsUpdateCount--;
                            }
                        }
                        if (this.LexStateChanged(this.position.Y))
                        {
                            this.LinesChanged(this.position.Y, 0x7fffffff, true);
                        }
                        else
                        {
                            this.LinesChanged(this.position.Y, this.position.Y, true);
                        }
                    }
                    finally
                    {
                        this.EndUpdate();
                    }
                }
            }
        }

        public virtual void ResetBracesOptions()
        {
            this.BracesOptions = QWhale.Editor.TextSource.BracesOptions.None;
        }

        public virtual void ResetCheckSpelling()
        {
            this.CheckSpelling = false;
        }

        public virtual void ResetClosingBraces()
        {
            this.ClosingBraces = EditConsts.DefaultClosingBraces;
        }

        public virtual void ResetHighlightHyperText()
        {
            this.HighlightHyperText = false;
        }

        public virtual void ResetIndentOptions()
        {
            this.IndentOptions = EditConsts.DefaultIndentOptions;
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

        public virtual void ResetOpenBraces()
        {
            this.OpenBraces = EditConsts.DefaultOpenBraces;
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

        public virtual void ResetUndoLimit()
        {
            this.UndoLimit = 0;
        }

        public virtual void ResetUndoOptions()
        {
            this.UndoOptions = EditConsts.DefaultUndoOptions;
        }

        public virtual Point RestorePosition(int index)
        {
            if (index < this.positionList.Count)
            {
                Point point = this.positionList[index];
                this.positionList.RemoveAt(index);
                return point;
            }
            return new Point(0, 0);
        }

        public virtual bool SaveFile(string fileName)
        {
            return this.SaveFile(fileName, null, null);
        }

        public virtual bool SaveFile(string fileName, IStringExport exporter)
        {
            return this.lines.SaveFile(fileName, exporter);
        }

        public virtual bool SaveFile(string fileName, Encoding encoding)
        {
            return this.lines.SaveFile(fileName, encoding);
        }

        public virtual bool SaveFile(string fileName, IStringExport exporter, Encoding encoding)
        {
            return this.lines.SaveFile(fileName, exporter, encoding);
        }

        public virtual bool SaveStream(Stream stream)
        {
            return this.lines.SaveStream(stream);
        }

        public virtual bool SaveStream(TextWriter writer)
        {
            return this.lines.SaveStream(writer);
        }

        public virtual bool SaveStream(Stream stream, IStringExport exporter)
        {
            return this.lines.SaveStream(stream, exporter);
        }

        public virtual bool SaveStream(Stream stream, Encoding encoding)
        {
            return this.lines.SaveStream(stream, encoding);
        }

        public virtual bool SaveStream(TextWriter writer, IStringExport exporter)
        {
            return this.lines.SaveStream(writer, exporter);
        }

        public virtual bool SaveStream(Stream stream, IStringExport exporter, Encoding encoding)
        {
            return this.lines.SaveStream(stream, exporter, encoding);
        }

        public virtual void SetLastParsed(int index)
        {
            this.lastParsed = index;
            this.firstChanged = -1;
        }

        public virtual void SetLineReadonly(int index, bool readOnly)
        {
            IStringItem item = this.lines.GetItem(index);
            if (item != null)
            {
                if (readOnly)
                {
                    item.State = (ItemState) ((byte) (item.State | (ItemState.None | ItemState.Readonly)));
                }
                else
                {
                    item.State = (ItemState) ((byte) (((int) item.State) & 0xfd));
                }
            }
        }

        public virtual void SetNavigateOptions(QWhale.Editor.TextSource.NavigateOptions navigateOptions)
        {
            this.navigateOptions = navigateOptions;
        }

        private void SetSnippetValue(ICodeSnippetRange range, string value)
        {
            this.BeginUpdate(UpdateReason.Insert);
            try
            {
                this.MoveTo(range.StartPoint);
                this.DeleteRight((range.EndPoint.Y == range.StartPoint.Y) ? (range.EndPoint.X - range.StartPoint.X) : 0x7fffffff);
                this.Insert(value);
            }
            finally
            {
                this.EndUpdate();
            }
        }

        protected void SetTextStyle(Rectangle rect, TextStyle style, bool setFlag, bool needUpdate)
        {
            for (int i = rect.Top; i <= rect.Bottom; i++)
            {
                int x = 0;
                int right = 0;
                if (i == rect.Top)
                {
                    if (i == rect.Bottom)
                    {
                        x = rect.Left;
                        right = rect.Right;
                    }
                    else
                    {
                        x = rect.Left;
                        right = 0x7fffffff;
                    }
                }
                else if (i == rect.Bottom)
                {
                    x = 0;
                    right = rect.Right;
                }
                else
                {
                    x = 0;
                    right = 0x7fffffff;
                }
                this.SetTextStyle(new Point(x, i), (right == 0x7fffffff) ? right : (right - x), style, setFlag, needUpdate);
            }
        }

        protected void SetTextStyle(Point pos, int len, TextStyle style, bool setFlag, bool needUpdate)
        {
            if (pos.Y >= 0)
            {
                IStringItem item = this.Lines.GetItem(pos.Y);
                if (item != null)
                {
                    if (needUpdate)
                    {
                        this.BeginUpdate(UpdateReason.Other);
                    }
                    try
                    {
                        int length = item.TextData.Length;
                        if ((pos.X >= 0) && (pos.X < length))
                        {
                            len = Math.Min(len, length - pos.X);
                            if (len > 0)
                            {
                                if (setFlag)
                                {
                                    item.SetTextStyle(pos.X, len, style);
                                }
                                else
                                {
                                    item.ClearTextStyle(pos.X, len, style);
                                }
                            }
                        }
                        this.LinesChanged(pos.Y, pos.Y);
                        this.state |= NotifyState.BlockChanged;
                    }
                    finally
                    {
                        if (needUpdate)
                        {
                            this.EndUpdate();
                        }
                    }
                }
            }
        }

        public bool ShouldSerializeClosingBraces()
        {
            return (new string(this.closingBraces) != new string(EditConsts.DefaultClosingBraces));
        }

        public bool ShouldSerializeIndentOptions()
        {
            return (this.indentOptions != EditConsts.DefaultIndentOptions);
        }

        public bool ShouldSerializeLexer()
        {
            return (this.Lexer != null);
        }

        public bool ShouldSerializeNavigateOptions()
        {
            return (this.navigateOptions != EditConsts.DefaultNavigateOptions);
        }

        public bool ShouldSerializeOpenBraces()
        {
            return (new string(this.openBraces) != new string(EditConsts.DefaultOpenBraces));
        }

        public bool ShouldSerializeText()
        {
            return (this.lines.Count > 0);
        }

        public bool ShouldSerializeUndoOptions()
        {
            return (this.undoOptions != EditConsts.DefaultUndoOptions);
        }

        private bool SkipNavigations(IList<IUndoData> list)
        {
            if (list.Count != 0)
            {
                IUndoData undoData = list[list.Count - 1];
                if ((undoData.Operation == UndoOperation.Navigate) && (undoData.Data == null))
                {
                    this.BeginUpdate(UpdateReason.Navigate);
                    try
                    {
                        while (list.Count > 0)
                        {
                            undoData = list[list.Count - 1];
                            if ((undoData.Operation != UndoOperation.Navigate) || (undoData.Data != null))
                            {
                                goto Label_007A;
                            }
                            this.Undo(undoData);
                            list.RemoveAt(list.Count - 1);
                        }
                    }
                    finally
                    {
                        this.EndUpdate();
                    }
                }
            }
        Label_007A:
            return (list.Count == 0);
        }

        private void StartBracesTimer()
        {
            this.EndBracesTimer();
            if (this.bracesTimer == null)
            {
                this.bracesTimer = new Timer();
                this.bracesTimer.Enabled = false;
                this.bracesTimer.Interval = EditConsts.DefaultBracesDelay;
                this.bracesTimer.Tick += new EventHandler(this.OnUnhighlightBraces);
            }
            this.bracesTimer.Enabled = true;
        }

        private void StartFmtTimer(bool modified, bool lineChanged)
        {
            this.needFormatting |= modified;
            if (modified)
            {
                this.EndFmtTimer();
                if (this.NeedReparseText() && ((((this.state & NotifyState.StringsChanged) != NotifyState.None) || ((this.state & NotifyState.SyntaxChanged) != NotifyState.None)) || !this.NeedReparseTextOnLineChange()))
                {
                    if (this.fmtTimer == null)
                    {
                        this.fmtTimer = new Timer();
                        this.fmtTimer.Enabled = false;
                        this.fmtTimer.Interval = EditConsts.DefaultOutlineDelay;
                        this.fmtTimer.Tick += new EventHandler(this.OnFormatting);
                    }
                    this.fmtTimer.Enabled = true;
                }
            }
            if ((lineChanged && this.needFormatting) && this.NeedReparseTextOnLineChange())
            {
                this.EndFmtTimer();
                this.FormatText();
            }
        }

        public virtual int StorePosition(Point position)
        {
            this.positionList.Add(position);
            return (this.positionList.Count - 1);
        }

        protected void StringsChanged(object sender, EventArgs ea)
        {
            if (this.stringsUpdateCount == 0)
            {
                this.BeginUpdate(UpdateReason.Other);
                try
                {
                    this.ClearUndo();
                    this.ClearRedo();
                    int firstChanged = ((ITextStrings) sender).FirstChanged;
                    int lastChanged = ((ITextStrings) sender).LastChanged;
                    this.BlockDeleting(new Rectangle(0, firstChanged, 0, lastChanged));
                    this.modified = false;
                    this.LinesChanged(firstChanged, lastChanged);
                    this.State |= ((((this.State | NotifyState.Edit) | NotifyState.PositionChanged) | NotifyState.ModifiedChanged) | NotifyState.CountChanged) | NotifyState.StringsChanged;
                    this.PositionChanged(UpdateReason.Other, 0, 0);
                    this.UpdateParsed(firstChanged, lastChanged);
                    if ((this.navigateOptions & QWhale.Editor.TextSource.NavigateOptions.KeepCaret) == QWhale.Editor.TextSource.NavigateOptions.None)
                    {
                        this.MoveToLine(Math.Max(Math.Min(firstChanged, this.lines.Count - 1), 0));
                    }
                    else if (this.Position.Y >= (this.lines.Count - 1))
                    {
                        this.MoveToLine(Math.Max(this.lines.Count - 1, 0));
                    }
                }
                finally
                {
                    this.EndUpdate();
                }
            }
        }

        private void StripToMaxLength(ref string str)
        {
            if (this.maxLength > 0)
            {
                int num = this.maxLength - this.Text.Length;
                if (num <= str.Length)
                {
                    str = str.Substring(0, Math.Max(0, num));
                }
            }
        }

        private void StripToMaxLength(ref string[] strings)
        {
            if (this.maxLength > 0)
            {
                int num = this.maxLength - this.Text.Length;
                int num2 = 0;
                int length = this.Lines.LineTerminator.Length;
                int num4 = strings.Length;
                for (int i = 0; i < strings.Length; i++)
                {
                    num2 += strings[i].Length;
                    if (num2 > num)
                    {
                        int num6 = strings[i].Length - (num2 - num);
                        strings[i] = strings[i].Substring(0, Math.Max(0, num6));
                        num4 = i + 1;
                        break;
                    }
                    num2 += length;
                }
                if (num4 < strings.Length)
                {
                    string[] destinationArray = new string[num4];
                    Array.Copy(strings, 0, destinationArray, 0, num4);
                    strings = destinationArray;
                }
            }
        }

        public virtual void TempHighlightBraces(Rectangle[] rects)
        {
            this.TempBraceRects = rects;
        }

        public virtual void TempUnhighlightBraces()
        {
            this.TempBraceRects = null;
            this.UpdateBraces(true);
        }

        public virtual void TempUnhighlightBraces(bool needUpdate)
        {
            this.TempBraceRects = null;
            this.UpdateBraces(needUpdate);
        }

        public virtual int TextPointToAbsolutePosition(Point position)
        {
            return this.lines.TextPointToAbsolutePosition(position);
        }

        protected void TrackChanges()
        {
            this.trackChanges = new Hashtable();
            ISortList<TrackChange> list = new SortList<TrackChange>();
            this.TrackChanges(this.undoList, list);
            foreach (TrackChange change in list)
            {
                this.trackChanges[change.Line] = change.Saved;
            }
            list.Clear();
        }

        protected void TrackChanges(IList<IUndoData> uList, ISortList<TrackChange> list)
        {
            for (int i = 0; i < uList.Count; i++)
            {
                IUndoData data = uList[i];
                int y = data.Position.Y;
                int num3 = 0;
                bool flag = true;
                switch (data.Operation)
                {
                    case UndoOperation.Insert:
                    case UndoOperation.Delete:
                        num3 = 0;
                        break;

                    case UndoOperation.Break:
                        num3 = 1;
                        break;

                    case UndoOperation.UnBreak:
                        num3 = -1;
                        break;

                    case UndoOperation.InsertBlock:
                    {
                        Point point2 = (Point) data.Data;
                        num3 = point2.Y;
                        break;
                    }
                    case UndoOperation.DeleteBlock:
                        num3 = -((string[]) data.Data).Length + 1;
                        break;

                    default:
                        flag = false;
                        break;
                }
                if (flag)
                {
                    bool saved = ((byte) (data.UndoFlag & (UndoFlags.None | UndoFlags.Saved))) != 0;
                    if (num3 != 0)
                    {
                        for (int k = list.Count - 1; k >= 0; k--)
                        {
                            TrackChange change = list[k];
                            if (change.Line < y)
                            {
                                break;
                            }
                            if ((change.Line + num3) < y)
                            {
                                saved &= change.Saved;
                                list.RemoveAt(k);
                            }
                            else
                            {
                                change.Line += num3;
                            }
                        }
                    }
                    for (int j = 0; j <= Math.Max(num3, 0); j++)
                    {
                        this.AddTrackChange(list, y + j, saved);
                    }
                }
            }
        }

        public virtual bool UnBreakLine()
        {
            if ((this.readOnly || (this.position.Y >= (this.lines.Count - 1))) || (((this.lines.Count <= 1) || this.PositionIsReadonly(this.position)) || this.LineIsReadonly(this.position.Y + 1)))
            {
                return false;
            }
            string str = this.lines[this.position.Y];
            if ((((this.state & NotifyState.Undo) != NotifyState.None) && ((this.position.X + 1) < str.Length)) && ((this.position.X + 1) >= str.TrimEnd(new char[0]).Length))
            {
                str = str.Substring(0, this.position.X);
            }
            int length = str.Length;
            if ((this.position.X + 1) < length)
            {
                return false;
            }
            this.BeginUpdate(UpdateReason.UnBreak);
            this.stringsUpdateCount++;
            try
            {
                length = this.position.X - length;
                if ((length >= 0) && (this.lines[this.position.Y + 1] != string.Empty))
                {
                    this.lines[this.position.Y] = str + new string(' ', length) + this.lines[this.position.Y + 1];
                }
                else
                {
                    this.lines[this.position.Y] = str + this.lines[this.position.Y + 1];
                }
                this.lines.RemoveAt(this.position.Y + 1);
                this.AddUndo(UndoOperation.UnBreak, null);
                this.LinesChanged(this.position.Y, 0x7fffffff, true);
                this.PositionChanged(UpdateReason.UnBreak, this.position.X, -1);
            }
            finally
            {
                this.stringsUpdateCount--;
                this.EndUpdate();
            }
            return true;
        }

        public virtual void Undo()
        {
            IList<IUndoData> undoList = this.GetUndoList();
            if ((this.AllowUndo && (undoList.Count > 0)) && !this.readOnly)
            {
                this.redo = !this.redo;
                try
                {
                    if (!this.SkipNavigations(undoList))
                    {
                        IUndoData undoData = undoList[undoList.Count - 1];
                        UpdateReason reason = undoData.Reason;
                        bool flag = undoData.Operation == UndoOperation.UndoBlock;
                        this.BeginUpdate(reason);
                        try
                        {
                            this.state |= NotifyState.Undo;
                            while (undoList.Count > 0)
                            {
                                undoData = undoList[undoList.Count - 1];
                                int updateCount = undoData.UpdateCount;
                                if ((reason != undoData.Reason) && (updateCount == 0))
                                {
                                    return;
                                }
                                bool flag2 = ((byte) (undoData.UndoFlag & UndoFlags.FirstTime)) != 0;
                                if ((undoData.Operation == UndoOperation.UndoBlock) && !flag)
                                {
                                    return;
                                }
                                this.Undo(undoData);
                                undoList.RemoveAt(undoList.Count - 1);
                                if (flag2 && ((!this.GroupUndo && (updateCount <= 0)) || (((this.undoOptions & QWhale.Editor.TextSource.UndoOptions.UngroupBreaks) != QWhale.Editor.TextSource.UndoOptions.None) && ((reason == UpdateReason.Break) || (reason == UpdateReason.UnBreak)))))
                                {
                                    return;
                                }
                            }
                        }
                        finally
                        {
                            this.EndUpdate();
                        }
                    }
                }
                finally
                {
                    this.redo = !this.redo;
                }
            }
        }

        public virtual void Undo(IUndoData undoData)
        {
            if (!this.UndoNavigations && ((undoData.Operation <= UndoOperation.DeleteBlock) || (undoData.Operation == UndoOperation.NavigateEx)))
            {
                this.position = undoData.Position;
            }
            this.undoFlag = (((byte) (undoData.UndoFlag & (UndoFlags.None | UndoFlags.Saved))) != 0) ? ((UndoFlags) ((byte) (this.undoFlag | UndoFlags.None | UndoFlags.Saved))) : ((UndoFlags) ((byte) (((int) this.undoFlag) & 0xfd)));
            switch (undoData.Operation)
            {
                case UndoOperation.Insert:
                    this.DeleteRight((int) undoData.Data);
                    break;

                case UndoOperation.Delete:
                    this.Insert((string) undoData.Data);
                    break;

                case UndoOperation.Break:
                    this.UnBreakLine();
                    break;

                case UndoOperation.UnBreak:
                    this.BreakLine();
                    break;

                case UndoOperation.InsertBlock:
                {
                    Point data = (Point) undoData.Data;
                    this.DeleteBlock(data.X);
                    break;
                }
                case UndoOperation.DeleteBlock:
                    this.InsertBlock((string[]) undoData.Data, (this.reason == UpdateReason.Delete) || (this.reason == UpdateReason.DeleteBlock));
                    break;

                case UndoOperation.Navigate:
                {
                    if (undoData.Data == null)
                    {
                        this.AddUndo(UndoOperation.Navigate, null);
                        break;
                    }
                    Point point2 = (Point) undoData.Data;
                    Point point3 = (Point) undoData.Data;
                    this.Navigate(-point2.X, -point3.Y);
                    break;
                }
                case UndoOperation.NavigateEx:
                    this.AddUndo(UndoOperation.NavigateEx, undoData.Data);
                    break;
            }
            if (this.UndoEvent != null)
            {
                this.UndoEvent(this, new UndoEventArgs(undoData));
            }
        }

        public virtual void UnhighlightBraces()
        {
            this.UnhighlightBraces(true);
        }

        public virtual void UnhighlightBraces(bool needUpdate)
        {
            this.UpdateBrace(this.openBrace, false, needUpdate);
            this.UpdateBrace(this.closingBrace, false, needUpdate);
            this.openBrace = new Point(-1, -1);
            this.closingBrace = new Point(-1, -1);
            this.EndBracesTimer();
        }

        public virtual void UnhighlightCodeSnippets()
        {
            this.UnhighlightCodeSnippets(true);
        }

        protected virtual void UnhighlightCodeSnippets(bool needUpdate)
        {
            foreach (IRange range in this.codeSnippets)
            {
                this.SetTextStyle(new Rectangle(range.StartPoint, new Size(range.EndPoint.X - range.StartPoint.X, range.EndPoint.Y - range.StartPoint.Y)), TextStyle.CodeSnippet, false, needUpdate);
            }
            this.codeSnippets.Clear();
        }

        public void UnhighlightSyntaxErrors()
        {
            this.UnhighlightSyntaxErrors(this.syntaxErrors, true);
        }

        protected virtual void UnhighlightSyntaxErrors(ISyntaxErrors errors, bool needUpdate)
        {
            foreach (ISyntaxError error in errors)
            {
                this.SetTextStyle(new Rectangle(error.Position, error.Size), TextStyle.WaveLine, false, needUpdate);
            }
        }

        protected virtual void UnhighlightSyntaxErrors(ISyntaxErrors errors, int line, bool needUpdate)
        {
            for (int i = errors.Count - 1; i >= 0; i--)
            {
                ISyntaxError error = errors[i];
                if (error.Position.Y == line)
                {
                    this.SetTextStyle(new Rectangle(error.Position, error.Size), TextStyle.WaveLine, false, needUpdate);
                    errors.RemoveAt(i);
                }
            }
        }

        public virtual void Update()
        {
            if (this.updateCount == 0)
            {
                this.Notify();
            }
        }

        private void UpdateBrace(Point point, bool value, bool needUpdate)
        {
            this.SetTextStyle(point, 1, TextStyle.Brace, value, needUpdate);
        }

        private void UpdateBraces(bool needUpdate)
        {
            this.UpdateBrace(this.openBrace, true, needUpdate);
            this.UpdateBrace(this.closingBrace, true, needUpdate);
        }

        protected void UpdateParsed(int fromIndex, int toIndex)
        {
            for (int i = fromIndex; i <= Math.Min(toIndex, this.lines.Count - 1); i++)
            {
                IStringItem item = this.lines.GetItem(i);
                item.State = (ItemState) ((byte) (((int) item.State) & 0xfe));
            }
        }

        private void UpdateSnippetValues(ICodeSnippetRange snippet)
        {
            int index = this.StorePosition(this.Position);
            this.BeginUpdate(UpdateReason.Insert);
            try
            {
                string snippetValue = this.GetSnippetValue(snippet);
                for (int i = this.codeSnippets.Count - 1; i >= 0; i--)
                {
                    ICodeSnippetRange range = this.codeSnippets[i];
                    if (((range != snippet) && (range.ID == snippet.ID)) && (this.GetSnippetValue(range) != snippetValue))
                    {
                        this.SetSnippetValue(range, snippetValue);
                    }
                }
            }
            finally
            {
                this.Position = this.RestorePosition(index);
                this.EndUpdate();
            }
        }

        private void UpdateTempBraces(bool value, bool needUpdate)
        {
            if (this.tempBraceRects != null)
            {
                foreach (Rectangle rectangle in this.tempBraceRects)
                {
                    this.SetTextStyle(rectangle, TextStyle.Brace, value, needUpdate);
                }
            }
        }

        public virtual void ValidatePosition(ref Point position)
        {
            if (position.Y < 0)
            {
                position.Y = 0;
            }
            if (position.X < 0)
            {
                position.X = 0;
            }
            if ((((QWhale.Editor.TextSource.NavigateOptions.BeyondEof & this.navigateOptions) == QWhale.Editor.TextSource.NavigateOptions.None) || this.singleLineMode) && (position.Y >= this.lines.Count))
            {
                position.Y = Math.Max(this.lines.Count - 1, 0);
            }
            if ((QWhale.Editor.TextSource.NavigateOptions.BeyondEol & this.navigateOptions) == QWhale.Editor.TextSource.NavigateOptions.None)
            {
                int num = (position.Y < this.lines.Count) ? this.lines.GetLength(position.Y) : 0;
                if (position.X >= num)
                {
                    position.X = num;
                }
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual object ActiveEdit
        {
            get
            {
                return this.activeEdit;
            }
            set
            {
                if (this.activeEdit != value)
                {
                    this.activeEdit = value;
                    this.OnActiveEditChanged();
                }
            }
        }

        protected bool AllowUndo
        {
            get
            {
                return ((this.undoOptions & QWhale.Editor.TextSource.UndoOptions.AllowUndo) != QWhale.Editor.TextSource.UndoOptions.None);
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public virtual IBookMarks BookMarks
        {
            get
            {
                return this.bookMarks;
            }
            set
            {
                if (this.bookMarks != value)
                {
                    this.bookMarks = value;
                    this.OnBookmarksChanged();
                }
            }
        }

        [Category("Braces"), DefaultValue(0), Editor("QWhale.Design.FlagEnumerationEditor, QWhale.Editor", typeof(UITypeEditor)), Description("Gets or sets options specifying appearance and behaviour of matching braces within Edit control.")]
        public virtual QWhale.Editor.TextSource.BracesOptions BracesOptions
        {
            get
            {
                return this.bracesOptions;
            }
            set
            {
                if (this.bracesOptions != value)
                {
                    this.bracesOptions = value;
                    this.OnBracesOptionsChanged();
                }
            }
        }

        [Category("Behavior"), DefaultValue(false), Description("Gets or sets a value indicating whether the document can check spelling for its content.")]
        public virtual bool CheckSpelling
        {
            get
            {
                return this.checkSpelling;
            }
            set
            {
                if (this.checkSpelling != value)
                {
                    this.checkSpelling = value;
                    this.OnCheckSpellingChanged();
                }
            }
        }

        [Description("Gets or sets an array of characters each one representing a closing brace."), Category("Braces")]
        public virtual char[] ClosingBraces
        {
            get
            {
                return this.closingBraces;
            }
            set
            {
                this.closingBraces = value;
                this.OnClosingBracesChanged();
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual ICodeSnippetRanges CodeSnippets
        {
            get
            {
                return this.codeSnippets;
            }
            set
            {
                this.BeginUpdate(UpdateReason.Other);
                try
                {
                    this.UnhighlightCodeSnippets(false);
                    foreach (ICodeSnippetRange range in value)
                    {
                        this.codeSnippets.Add(range);
                    }
                    this.HighlightCodeSnippets(false);
                }
                finally
                {
                    this.EndUpdate();
                }
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual ICodeSnippetRange CurrentSnippet
        {
            get
            {
                return this.currentSnippet;
            }
            set
            {
                if (this.currentSnippet != value)
                {
                    if (this.currentSnippet != null)
                    {
                        int index = this.codeSnippets.IndexOf(this.currentSnippet);
                        if ((index >= 0) && this.CodeSnippets.IsFirstSnippet(index))
                        {
                            this.UpdateSnippetValues(this.currentSnippet);
                        }
                    }
                    this.currentSnippet = value;
                    this.InvalidateSnippets();
                }
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual IList<ISyntaxEdit> Edits
        {
            get
            {
                return this.edits;
            }
        }

        [Category("TextSource"), DefaultValue(""), Description("Gets or sets name of file that holds text source content.")]
        public virtual string FileName
        {
            get
            {
                return this.fileName;
            }
            set
            {
                if (this.fileName != value)
                {
                    this.fileName = value;
                    this.OnFileNameChanged();
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public virtual int FirstChanged
        {
            get
            {
                return this.firstChanged;
            }
        }

        protected bool GroupUndo
        {
            get
            {
                return ((this.undoOptions & QWhale.Editor.TextSource.UndoOptions.GroupUndo) != QWhale.Editor.TextSource.UndoOptions.None);
            }
        }

        [DefaultValue(false), Category("Behavior"), Description("Gets or sets a value indicating whether hypertext urls in the text should be highlighted.")]
        public virtual bool HighlightHyperText
        {
            get
            {
                return this.highlightHyperText;
            }
            set
            {
                if (this.highlightHyperText != value)
                {
                    this.highlightHyperText = value;
                    this.OnHighlightHyperTextChanged();
                }
            }
        }

        [Editor("QWhale.Design.FlagEnumerationEditor, QWhale.Editor", typeof(UITypeEditor)), Description("Gets or sets \"QWhale.Editor.TextSource.IndentOptions\" for this class, allowing to customize behaior of Edit control when user presses Enter to insert new text line."), Category("Behavior")]
        public virtual QWhale.Editor.TextSource.IndentOptions IndentOptions
        {
            get
            {
                return this.indentOptions;
            }
            set
            {
                if (this.indentOptions != value)
                {
                    this.indentOptions = value;
                    this.OnIndentOptionsChanged();
                }
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual int LastChanged
        {
            get
            {
                return this.lastChanged;
            }
        }

        [Category("TextSource"), Description("Gets or sets object that can perform lexical analysis of the text source content.")]
        public virtual ILexer Lexer
        {
            get
            {
                return this.lexer;
            }
            set
            {
                if (this.lexer != value)
                {
                    if (this.lexer != null)
                    {
                        this.lexer.RemoveNotifier(this);
                    }
                    this.lexer = value;
                    if (this.lexer != null)
                    {
                        this.lexer.AddNotifier(this);
                    }
                    this.LexerChanged();
                }
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual ITextStrings Lines
        {
            get
            {
                return this.lines;
            }
            set
            {
                if (this.lines != value)
                {
                    this.lines = value;
                    this.OnLinesChanged();
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public virtual ILineStyles LineStyles
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

        [DefaultValue("\r\n"), Description("Gets or sets a string value that terminates line."), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual string LineTerminator
        {
            get
            {
                return this.lines.LineTerminator;
            }
            set
            {
                if (this.lines != null)
                {
                    this.lines.LineTerminator = value;
                }
            }
        }

        [Description("Specifies the maximum number of characters that can be entered into the edit control."), Category("Behavior"), DefaultValue(0)]
        public virtual int MaxLength
        {
            get
            {
                return this.maxLength;
            }
            set
            {
                if (this.maxLength != value)
                {
                    this.maxLength = value;
                    this.OnMaxLengthChanged();
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public virtual bool Modified
        {
            get
            {
                return this.modified;
            }
            set
            {
                if (this.modified != value)
                {
                    this.modified = value;
                    this.OnModifiedChanged();
                }
            }
        }

        [Editor("QWhale.Design.FlagEnumerationEditor, QWhale.Editor", typeof(UITypeEditor)), Description("Gets or sets navigating options."), Category("Behavior")]
        public virtual QWhale.Editor.TextSource.NavigateOptions NavigateOptions
        {
            get
            {
                return this.navigateOptions;
            }
            set
            {
                if (this.navigateOptions != value)
                {
                    this.navigateOptions = value;
                    this.OnNavigateOptionsChanged();
                }
            }
        }

        [Category("Braces"), Description("Gets or sets an array of characters each one representing an open brace.")]
        public virtual char[] OpenBraces
        {
            get
            {
                return this.openBraces;
            }
            set
            {
                this.openBraces = value;
                this.OnOpenBracesChanged();
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual bool Overwrite
        {
            get
            {
                return overWrite;
            }
            set
            {
                if (overWrite != value)
                {
                    overWrite = value;
                    this.OnOverWriteChanged();
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public virtual int ParserLine
        {
            get
            {
                return this.parserLine;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual Point Position
        {
            get
            {
                return this.position;
            }
            set
            {
                this.MoveTo(value.X, value.Y);
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public virtual Point PrevPosition
        {
            get
            {
                return this.prevPosition;
            }
        }

        [DefaultValue(false), Category("Behavior"), Description("Gets or sets a value indicating whether the control's content is read-only.")]
        public virtual bool Readonly
        {
            get
            {
                return this.readOnly;
            }
            set
            {
                if (this.readOnly != value)
                {
                    this.readOnly = value;
                    this.OnReadonlyChanged();
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public virtual IUndoList RedoList
        {
            get
            {
                return this.redoList;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public virtual Rectangle SelectBlockRect
        {
            get
            {
                return this.selectBlockRect;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual ISerializationInfo SerializationInfo
        {
            get
            {
                return new XmlTextSourceInfo(this);
            }
            set
            {
                value.FixupReferences(this);
            }
        }

        [Description("Gets or sets a value indicating whether the control accepts only one line of the text."), Category("Behavior"), DefaultValue(false)]
        public virtual bool SingleLineMode
        {
            get
            {
                return this.singleLineMode;
            }
            set
            {
                if (this.singleLineMode != value)
                {
                    this.singleLineMode = value;
                    this.OnSingleLineModeChanged();
                }
            }
        }

        protected int SnippetUpdateCount
        {
            get
            {
                return this.snippetUpdateCount;
            }
            set
            {
                this.snippetUpdateCount = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public virtual Hashtable SpellTable
        {
            get
            {
                return this.spellTable;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual NotifyState State
        {
            get
            {
                return this.state;
            }
            set
            {
                if (this.state != value)
                {
                    this.state = value;
                    this.OnStateChanged();
                }
            }
        }

        [TypeConverter(typeof(CollectionConverter)), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Category("TextSource"), Description("Gets or sets text source content in the form of string array.")]
        public virtual string[] Strings
        {
            get
            {
                string[] strArray = new string[this.lines.Count];
                for (int i = 0; i < this.lines.Count; i++)
                {
                    strArray[i] = this.lines[i];
                }
                return strArray;
            }
            set
            {
                this.lines.BeginUpdate();
                try
                {
                    this.lines.Clear();
                    foreach (string str in value)
                    {
                        this.lines.Add(str);
                    }
                }
                finally
                {
                    this.lines.EndUpdate();
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public ISyntaxErrors SyntaxErrors
        {
            get
            {
                return this.syntaxErrors;
            }
        }

        protected Rectangle[] TempBraceRects
        {
            get
            {
                return this.tempBraceRects;
            }
            set
            {
                if (this.tempBraceRects != value)
                {
                    this.UpdateTempBraces(false, true);
                    this.tempBraceRects = value;
                    this.UpdateTempBraces(true, true);
                    if (value != null)
                    {
                        this.StartBracesTimer();
                    }
                }
            }
        }

        [Description("Gets or sets text source content as a single string with the individual strings delimited by carriage returns.")]
        public virtual string Text
        {
            get
            {
                return this.Lines.Text;
            }
            set
            {
                if ((this.Lines != null) && (this.Lines.Text != value))
                {
                    this.Lines.Text = value;
                    this.OnTextChanged();
                }
            }
        }

        protected bool UndoAfterSave
        {
            get
            {
                return ((this.undoOptions & QWhale.Editor.TextSource.UndoOptions.UndoAfterSave) != QWhale.Editor.TextSource.UndoOptions.None);
            }
        }

        [Category("Behavior"), DefaultValue(0), Description("Gets or sets a value that limits number of undo operations.")]
        public virtual int UndoLimit
        {
            get
            {
                return this.undoLimit;
            }
            set
            {
                if (this.undoLimit != value)
                {
                    this.undoLimit = value;
                    this.OnUndoLimitChanged();
                }
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual IUndoList UndoList
        {
            get
            {
                return this.undoList;
            }
        }

        protected bool UndoNavigations
        {
            get
            {
                return ((this.undoOptions & QWhale.Editor.TextSource.UndoOptions.UndoNavigations) != QWhale.Editor.TextSource.UndoOptions.None);
            }
        }

        [Category("Behavior"), Editor("QWhale.Design.FlagEnumerationEditor, QWhale.Editor", typeof(UITypeEditor)), Description("Gets or sets options for undo and redo operations.")]
        public virtual QWhale.Editor.TextSource.UndoOptions UndoOptions
        {
            get
            {
                return this.undoOptions;
            }
            set
            {
                if (this.undoOptions != value)
                {
                    if (((this.undoOptions ^ value) & QWhale.Editor.TextSource.UndoOptions.AllowUndo) != QWhale.Editor.TextSource.UndoOptions.None)
                    {
                        this.ClearUndo();
                        this.ClearRedo();
                    }
                    this.undoOptions = value;
                    this.OnUndoOptionsChanged();
                }
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual int UndoUpdateCount
        {
            get
            {
                return this.undoUpdateCount;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public virtual int UpdateCount
        {
            get
            {
                return this.updateCount;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Hashtable UrlTable
        {
            get
            {
                return this.urlTable;
            }
        }

        private class ErrComparer : IComparer<ISyntaxError>
        {
            public int Compare(ISyntaxError x, ISyntaxError y)
            {
                Point position = x.Position;
                Point point2 = y.Position;
                int num = position.Y - point2.Y;
                if (num == 0)
                {
                    num = position.X - point2.X;
                }
                return num;
            }
        }

        protected class TrackChange
        {
            public int Line;
            public bool Saved;

            public TrackChange(int line, bool saved)
            {
                this.Line = line;
                this.Saved = saved;
            }
        }

        protected class TrackChangeComparer : IComparer
        {
            public int Compare(object x, object y)
            {
                return (((QWhale.Editor.TextSource.TextSource.TrackChange) x).Line - ((int) y));
            }
        }
    }
}

