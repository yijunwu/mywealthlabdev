namespace QWhale.Editor
{
    using QWhale.Common;
    using QWhale.Editor.Serialization;
    using QWhale.Editor.TextSource;
    using QWhale.Syntax;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Design;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Windows.Forms;

    public class Selection : ISelection
    {
        private QWhale.Editor.AllowedSelectionMode allowedSelectionMode;
        private bool atBottomRightEnd;
        private bool atTopLeftEnd;
        private Color backColor;
        private Color borderColor;
        private TextUndoEvent capitalizeLineEvent;
        private TextUndoEvent commentLineEvent;
        private TextUndoEvent deleteWhiteSpaceEvent;
        private Color foreColor;
        private Color inActiveBackColor;
        private Color inActiveForeColor;
        private TextUndoEvent indentLineEvent;
        private TextUndoEvent lowerCaseLineEvent;
        private KeyEvent moveSelection;
        private Point oldPos;
        private Rectangle oldSelectionRect;
        private QWhale.Editor.SelectionType oldSelectionType;
        private SelectionOptions options;
        private ISyntaxEdit owner;
        private Rectangle selectionRect;
        private QWhale.Editor.SelectionState selectionState;
        private QWhale.Editor.SelectionType selectionType;
        private Point selEnd;
        private bool selForward;
        private Point selStart;
        private Timer selTimer;
        private TextUndoEvent tabifyLineEvent;
        private TextUndoEvent uncommentLineEvent;
        private IComparer<ITextUndo> undoComparer;
        private TextUndoEvent unIndentLineEvent;
        private TextUndoEvent unTabifyLineEvent;
        private int updateCount;
        private TextUndoEvent upperCaseLineEvent;

        [Browsable(false)]
        public event EventHandler SelectionChanged;

        public Selection()
        {
            this.options = EditConsts.DefaultSelectionOptions;
            this.allowedSelectionMode = EditConsts.DefaultSelectionMode;
            this.foreColor = EditConsts.DefaultHighlightForeColor;
            this.backColor = EditConsts.DefaultHighlightBackColor;
            this.inActiveForeColor = EditConsts.DefaultInactiveHighlightForeColor;
            this.inActiveBackColor = EditConsts.DefaultInactiveHighlightBackColor;
            this.borderColor = EditConsts.DefaultSelectionBorderColor;
            this.oldSelectionRect = Rectangle.Empty;
            this.oldPos = Point.Empty;
            this.selStart = Point.Empty;
            this.selEnd = Point.Empty;
            this.moveSelection = new KeyEvent(this.MoveSelection);
            this.tabifyLineEvent = new TextUndoEvent(this.TabifyLine);
            this.unTabifyLineEvent = new TextUndoEvent(this.UnTabifyLine);
            this.indentLineEvent = new TextUndoEvent(this.IndentLine);
            this.unIndentLineEvent = new TextUndoEvent(this.UnIndentLine);
            this.lowerCaseLineEvent = new TextUndoEvent(this.LowerCaseLine);
            this.upperCaseLineEvent = new TextUndoEvent(this.UpperCaseLine);
            this.capitalizeLineEvent = new TextUndoEvent(this.CapitalizeLine);
            this.deleteWhiteSpaceEvent = new TextUndoEvent(this.DeleteWhiteSpace);
            this.commentLineEvent = new TextUndoEvent(this.CommentLine);
            this.uncommentLineEvent = new TextUndoEvent(this.UncommentLine);
            this.selTimer = new Timer();
            this.selTimer.Enabled = false;
            this.selTimer.Interval = EditConsts.DefaultSelDelay;
            this.selTimer.Tick += new EventHandler(this.OnSelect);
            this.undoComparer = new UndoComparer();
        }

        public Selection(ISyntaxEdit owner) : this()
        {
            this.owner = owner;
        }

        public virtual void Assign(ISelection source)
        {
            this.Options = source.Options;
            this.BackColor = source.BackColor;
            this.ForeColor = source.ForeColor;
            this.InActiveBackColor = source.InActiveBackColor;
            this.InActiveForeColor = source.InActiveForeColor;
            this.BorderColor = source.BorderColor;
            this.SetSelection(source.SelectionType, source.SelectionRect);
        }

        public virtual int BeginUpdate()
        {
            if (this.updateCount == 0)
            {
                this.oldSelectionRect = this.selectionRect;
                this.oldSelectionType = this.selectionType;
                this.oldPos = this.owner.Position;
            }
            this.updateCount++;
            return this.updateCount;
        }

        public virtual bool CanCopy()
        {
            if (this.IsEmpty)
            {
                return ((this.options & SelectionOptions.CopyLineWhenEmpty) != SelectionOptions.None);
            }
            return true;
        }

        public virtual bool CanCut()
        {
            if (this.owner.Source.Readonly || (this.IsEmpty && ((this.options & SelectionOptions.CopyLineWhenEmpty) == SelectionOptions.None)))
            {
                return false;
            }
            return !this.SelectionContainsReadonlyLines();
        }

        public virtual bool CanDrag(Point position)
        {
            return ((!this.owner.Source.Readonly && ((SelectionOptions.DisableDragging & this.options) == SelectionOptions.None)) && !this.IsPosInSelection(position));
        }

        public bool CanPaste()
        {
            if (this.owner.Readonly || this.owner.Source.LineIsReadonly(this.owner.Position.Y))
            {
                return false;
            }
            return (Clipboard.ContainsText(TextDataFormat.UnicodeText) || (Clipboard.ContainsText(TextDataFormat.Text) && !this.SelectionContainsReadonlyLines()));
        }

        protected bool CanSelectBlock()
        {
            return ((Control.ModifierKeys & Keys.Alt) != Keys.None);
        }

        public virtual void Capitalize()
        {
            this.ChangeBlock(this.capitalizeLineEvent, true, false);
        }

        private void CapitalizeLine(string str, ITextUndoList operations)
        {
            char[] chArray = str.ToLower().ToCharArray();
            ITextStrings lines = this.owner.Lines;
            for (int i = 0; i < chArray.Length; i++)
            {
                bool flag = !lines.IsDelimiter(chArray[i]);
                if (i != 0)
                {
                    flag = flag && lines.IsDelimiter(chArray[i - 1]);
                }
                if (flag)
                {
                    operations.Add(new TextUndo(i, 1, char.ToUpper(chArray[i]).ToString()));
                }
            }
        }

        public virtual void ChangeBlock(StringEvent action)
        {
            this.ChangeBlock(action, false, false);
        }

        public virtual void ChangeBlock(TextUndoEvent action)
        {
            this.ChangeBlock(action, false, false);
        }

        public virtual void ChangeBlock(StringEvent action, bool changeIfEmpty, bool extendFirstLine)
        {
            if (changeIfEmpty || !this.IsEmpty)
            {
                string str;
                int x = 0;
                ITextSource source = this.owner.Source;
                if (this.IsEmpty)
                {
                    str = source.Lines[source.Position.Y];
                    if (extendFirstLine || (source.Position.X < str.Length))
                    {
                        source.BeginUpdate(UpdateReason.Other);
                        try
                        {
                            if (extendFirstLine)
                            {
                                x = source.Position.X - str.Length;
                                source.MoveToChar(0);
                                source.DeleteRight(0x7fffffff);
                                str = action(str);
                                x += str.Length;
                                source.Insert(str);
                                source.MoveToChar(x);
                            }
                            else
                            {
                                char ch = str[source.Position.X];
                                source.DeleteRight(1);
                                source.Insert(action(ch.ToString()));
                            }
                        }
                        finally
                        {
                            source.EndUpdate();
                        }
                    }
                }
                else
                {
                    Rectangle selectionRect;
                    x = 0;
                    if (extendFirstLine)
                    {
                        selectionRect = this.SelectionRect;
                        x = selectionRect.Left;
                        this.SelectionRect = new Rectangle(0, selectionRect.Top, selectionRect.Right, selectionRect.Height);
                    }
                    StringBuilder builder = new StringBuilder();
                    int num2 = this.SelectedCount();
                    for (int i = 0; i < num2; i++)
                    {
                        string str2;
                        str = this.SelectedString(i);
                        if (((i == (num2 - 1)) && (this.selectionType == QWhale.Editor.SelectionType.Stream)) && (this.selectionRect.Right == 0))
                        {
                            str2 = str;
                        }
                        else
                        {
                            str2 = action(str);
                        }
                        if (extendFirstLine && (i == 0))
                        {
                            x += str2.Length - str.Length;
                        }
                        builder.Append(str2);
                        if (i < (num2 - 1))
                        {
                            builder.Append(source.LineTerminator);
                        }
                    }
                    this.SetSelectedText(builder.ToString(), (this.SelectionType != QWhale.Editor.SelectionType.None) ? this.SelectionType : QWhale.Editor.SelectionType.Stream);
                    if (extendFirstLine)
                    {
                        selectionRect = this.SelectionRect;
                        int introduced13 = Math.Max(selectionRect.Left + x, 0);
                        this.SetSelection(this.SelectionType, new Point(introduced13, selectionRect.Top), new Point(selectionRect.Right, selectionRect.Bottom));
                    }
                }
            }
        }

        public virtual void ChangeBlock(TextUndoEvent action, bool changeIfEmpty, bool extendFirstLine)
        {
            if (changeIfEmpty || !this.IsEmpty)
            {
                string str;
                ITextSource source = this.owner.Source;
                ITextUndoList operations = new TextUndoList();
                int x = 0;
                if (this.IsEmpty)
                {
                    str = source.Lines[source.Position.Y];
                    if (extendFirstLine || (source.Position.X < str.Length))
                    {
                        source.BeginUpdate(UpdateReason.Other);
                        try
                        {
                            if (extendFirstLine)
                            {
                                x = source.Position.X;
                                action(str, operations);
                                source.MoveToChar(this.UndoOperations(source.Position.Y, operations) + x);
                            }
                            else
                            {
                                char ch = str[source.Position.X];
                                action(ch.ToString(), operations);
                                foreach (ITextUndo undo in operations)
                                {
                                    undo.Start += source.Position.X;
                                }
                                this.UndoOperations(source.Position.Y, operations);
                            }
                        }
                        finally
                        {
                            source.EndUpdate();
                        }
                    }
                }
                else
                {
                    this.BeginUpdate();
                    source.BeginUpdate(UpdateReason.Other);
                    try
                    {
                        Rectangle selectionRect;
                        x = 0;
                        if (extendFirstLine)
                        {
                            selectionRect = this.SelectionRect;
                            x = selectionRect.Left;
                            this.SelectionRect = new Rectangle(0, selectionRect.Top, selectionRect.Right, selectionRect.Height);
                        }
                        int top = this.SelectionRect.Top;
                        int num3 = this.SelectedCount();
                        for (int i = 0; i < num3; i++)
                        {
                            str = this.SelectedString(i);
                            if (((i != (num3 - 1)) || (this.selectionType != QWhale.Editor.SelectionType.Stream)) || (this.selectionRect.Right != 0))
                            {
                                operations.Clear();
                                action(str, operations);
                                int num5 = this.UndoOperations(top + i, operations);
                                if (extendFirstLine && (i == 0))
                                {
                                    x += num5;
                                }
                            }
                        }
                        if (extendFirstLine)
                        {
                            selectionRect = this.SelectionRect;
                            int introduced20 = Math.Max(selectionRect.Left + x, 0);
                            this.SetSelection(this.SelectionType, new Point(introduced20, selectionRect.Top), new Point(selectionRect.Right, selectionRect.Bottom));
                        }
                    }
                    finally
                    {
                        source.EndUpdate();
                        this.EndUpdate();
                    }
                }
            }
        }

        public virtual void CharTransponse()
        {
            Point position = this.owner.Position;
            string str = this.owner.Lines[position.Y];
            if (str.Length >= 2)
            {
                position.X = Math.Max(Math.Min(position.X, str.Length - 1), 1);
                ITextSource source = this.owner.Source;
                source.BeginUpdate(UpdateReason.Insert);
                try
                {
                    char ch = str[position.X];
                    source.MoveToChar(position.X);
                    source.DeleteRight(1);
                    this.owner.MoveToChar(position.X - 1);
                    source.Insert(ch.ToString());
                    source.MoveToChar(position.X + 1);
                }
                finally
                {
                    source.EndUpdate();
                }
            }
        }

        private void CheckSelectionMode(ref QWhale.Editor.SelectionType selType)
        {
            if (this.allowedSelectionMode == QWhale.Editor.AllowedSelectionMode.None)
            {
                selType = QWhale.Editor.SelectionType.None;
            }
            else
            {
                switch (selType)
                {
                    case QWhale.Editor.SelectionType.Stream:
                        if ((this.allowedSelectionMode & QWhale.Editor.AllowedSelectionMode.Stream) != QWhale.Editor.AllowedSelectionMode.None)
                        {
                            break;
                        }
                        selType = QWhale.Editor.SelectionType.Block;
                        return;

                    case QWhale.Editor.SelectionType.Block:
                        if ((this.allowedSelectionMode & QWhale.Editor.AllowedSelectionMode.Block) == QWhale.Editor.AllowedSelectionMode.None)
                        {
                            selType = QWhale.Editor.SelectionType.Stream;
                        }
                        break;

                    default:
                        return;
                }
            }
        }

        public virtual void Clear()
        {
            this.SelectionType = QWhale.Editor.SelectionType.None;
        }

        public virtual void CollapseToDefinitions()
        {
            this.owner.DisplayLines.CollapseToDefinitions();
        }

        private void CommentLine(string str, ITextUndoList operations)
        {
            if (this.owner.Lexer is ISyntaxParser)
            {
                string singleLineComment = ((ISyntaxParser) this.owner.Lexer).GetSingleLineComment();
                if (singleLineComment != string.Empty)
                {
                    operations.Add(new TextUndo(0, 0, singleLineComment));
                }
            }
        }

        public virtual void CommentSelection()
        {
            if (this.owner.Lexer is ISyntaxParser)
            {
                if (this.IsEmpty)
                {
                    this.SelectLine();
                }
                this.SelectionType = QWhale.Editor.SelectionType.Stream;
                this.ChangeBlock(this.commentLineEvent, false, true);
            }
        }

        public virtual void Copy()
        {
            if (this.CanCopy())
            {
                bool isEmpty = this.IsEmpty;
                if (isEmpty)
                {
                    this.SelectLine();
                }
                this.WriteToClipboard();
                if (((this.options & SelectionOptions.DeselectOnCopy) != SelectionOptions.None) || isEmpty)
                {
                    this.Clear();
                }
            }
        }

        public virtual void Cut()
        {
            if (this.CanCut())
            {
                if (this.IsEmpty)
                {
                    this.SelectLine();
                }
                this.WriteToClipboard();
                this.Delete();
            }
        }

        public virtual void CutLine()
        {
            if (this.IsEmpty)
            {
                this.SelectLine();
            }
            this.Cut();
        }

        public virtual bool Delete()
        {
            if (this.owner.Readonly)
            {
                return false;
            }
            bool flag = true;
            if (!this.IsEmpty)
            {
                this.BeginUpdate();
                try
                {
                    ITextSource source = this.owner.Source;
                    source.BeginUpdate(UpdateReason.Delete);
                    try
                    {
                        if (this.selectionType == QWhale.Editor.SelectionType.Stream)
                        {
                            flag = source.DeleteBlock(this.SelectionRect);
                        }
                        else
                        {
                            IList<IRange> list = new List<IRange>();
                            for (int i = this.selectionRect.Top; i <= this.selectionRect.Bottom; i++)
                            {
                                this.GetSelectedBounds(i, list);
                            }
                            for (int j = list.Count - 1; j >= 0; j--)
                            {
                                IRange range = list[j];
                                source.MoveTo(range.StartPoint.X, range.StartPoint.Y);
                                source.DeleteRight((range.EndPoint.X == 0x7fffffff) ? range.EndPoint.X : (range.EndPoint.X - range.StartPoint.X));
                                if (range.EndPoint.X == 0x7fffffff)
                                {
                                    source.UnBreakLine();
                                }
                            }
                        }
                    }
                    finally
                    {
                        source.EndUpdate();
                    }
                    if (flag)
                    {
                        this.Clear();
                    }
                }
                finally
                {
                    this.EndUpdate();
                }
            }
            return flag;
        }

        public virtual void DeleteLeft()
        {
            this.DeleteLeft(false);
        }

        protected void DeleteLeft(bool deleteWord)
        {
            if (this.ShouldDeleteBlock())
            {
                this.Delete();
            }
            else if (this.owner.Position.X == 0)
            {
                if (this.owner.Position.Y > 0)
                {
                    this.owner.Source.BeginUpdate(UpdateReason.UnBreak);
                    try
                    {
                        this.owner.MoveTo(this.owner.Lines.GetLength(this.owner.Position.Y - 1), this.owner.Position.Y - 1);
                        this.owner.Source.UnBreakLine();
                    }
                    finally
                    {
                        this.owner.Source.EndUpdate();
                    }
                }
            }
            else if (deleteWord)
            {
                bool flag = true;
                this.BeginUpdate();
                this.owner.Source.BeginUpdate(UpdateReason.Delete);
                try
                {
                    this.SelectWordLeft();
                    flag = this.Delete();
                }
                finally
                {
                    this.owner.Source.EndUpdate();
                    this.EndUpdate();
                }
                if (!flag && !this.IsEmpty)
                {
                    this.Clear();
                }
            }
            else
            {
                this.owner.Source.DeleteLeft(1);
            }
        }

        public virtual void DeleteLine()
        {
            if (this.IsEmpty)
            {
                this.SelectLine();
            }
            this.Delete();
        }

        public virtual void DeleteRight()
        {
            this.DeleteRight(false);
        }

        protected void DeleteRight(bool deleteWord)
        {
            if (this.ShouldDeleteBlock())
            {
                this.Delete();
            }
            else if (this.owner.Position.X >= this.owner.Lines.GetLength(this.owner.Position.Y))
            {
                this.owner.Source.UnBreakLine();
            }
            else if (deleteWord)
            {
                bool flag = true;
                this.owner.Source.BeginUpdate(UpdateReason.Delete);
                this.BeginUpdate();
                try
                {
                    this.SelectWordRight();
                    flag = this.Delete();
                }
                finally
                {
                    this.EndUpdate();
                    this.owner.Source.EndUpdate();
                }
                if (!flag && !this.IsEmpty)
                {
                    this.Clear();
                }
            }
            else
            {
                this.owner.Source.DeleteRight(1);
            }
        }

        public virtual void DeleteWhiteSpace()
        {
            if (this.IsEmpty)
            {
                string str = this.owner.Lines[this.owner.Position.Y];
                if ((this.owner.Position.X < str.Length) && (this.owner.Position.X <= (str.Length - str.TrimStart(new char[0]).Length)))
                {
                    this.SelectLineBegin();
                    this.ChangeBlock(this.deleteWhiteSpaceEvent);
                    this.Clear();
                }
            }
            else
            {
                this.ChangeBlock(this.deleteWhiteSpaceEvent);
            }
        }

        private void DeleteWhiteSpace(string str, ITextUndoList operations)
        {
            int start = 0;
            int length = str.Length;
            while (start < length)
            {
                if ((str[start] == ' ') || (str[start] == '\t'))
                {
                    int num3 = start;
                    while ((num3 < (length - 1)) && ((str[num3 + 1] == ' ') || (str[num3 + 1] == '\t')))
                    {
                        num3++;
                    }
                    operations.Add(new TextUndo(start, (num3 - start) + 1, string.Empty));
                    start = num3;
                }
                start++;
            }
        }

        public virtual void DeleteWordLeft()
        {
            this.DeleteLeft(true);
        }

        public virtual void DeleteWordRight()
        {
            this.DeleteRight(true);
        }

        private int DoInsertString(QWhale.Editor.SelectionType selType, string s, bool insertLine)
        {
            if (insertLine)
            {
                if (selType == QWhale.Editor.SelectionType.Block)
                {
                    if (this.owner.Position.Y < (this.owner.DisplayLines.DisplayCount - 1))
                    {
                        this.owner.MoveLineDown();
                    }
                    else
                    {
                        this.owner.Source.NewLineBelow();
                    }
                }
                else
                {
                    this.owner.Source.BreakLine();
                    this.owner.MoveTo(0, this.owner.Position.Y + 1);
                }
            }
            this.owner.Source.Insert(s);
            return s.Length;
        }

        protected virtual bool DragScroll(Point pt)
        {
            Rectangle clientRect = this.owner.ClientRect;
            int windowOriginX = this.owner.Scrolling.WindowOriginX;
            int windowOriginY = this.owner.Scrolling.WindowOriginY;
            bool scrollByPixels = this.owner.Scrolling.ScrollByPixels;
            int fontHeight = this.owner.Painter.FontHeight;
            int fontWidth = this.owner.Painter.FontWidth;
            if ((pt.Y - fontHeight) < clientRect.Top)
            {
                IScrolling scrolling = this.owner.Scrolling;
                scrolling.WindowOriginY -= scrollByPixels ? fontHeight : 1;
            }
            else if ((pt.Y + fontHeight) > clientRect.Bottom)
            {
                IScrolling scrolling2 = this.owner.Scrolling;
                scrolling2.WindowOriginY += scrollByPixels ? fontHeight : 1;
            }
            else if ((pt.X - fontWidth) < clientRect.Left)
            {
                IScrolling scrolling3 = this.owner.Scrolling;
                scrolling3.WindowOriginX -= scrollByPixels ? fontWidth : 1;
            }
            else if ((pt.X + fontWidth) > clientRect.Right)
            {
                IScrolling scrolling4 = this.owner.Scrolling;
                scrolling4.WindowOriginX += scrollByPixels ? fontWidth : 1;
            }
            if (windowOriginX == this.owner.Scrolling.WindowOriginX)
            {
                return (windowOriginY != this.owner.Scrolling.WindowOriginY);
            }
            return true;
        }

        public virtual void DragTo(Point position, bool deleteOrigin)
        {
            this.Move(position, deleteOrigin);
        }

        public void EndSelection()
        {
            if (this.selectionState != QWhale.Editor.SelectionState.None)
            {
                this.selTimer.Stop();
                this.selectionState = QWhale.Editor.SelectionState.None;
                this.selForward = false;
            }
        }

        public virtual int EndUpdate()
        {
            this.updateCount--;
            if ((this.updateCount == 0) && ((!this.oldSelectionRect.Equals(this.selectionRect) || (this.oldSelectionType != this.selectionType)) || !this.oldPos.Equals(this.owner.Position)))
            {
                this.OnSelectionChanged();
            }
            return this.updateCount;
        }

        ~Selection()
        {
            this.selTimer.Dispose();
        }

        protected IDataObject GetDataObject()
        {
            try
            {
                return Clipboard.GetDataObject();
            }
            catch
            {
                return null;
            }
        }

        private int GetSelectedBounds(int index, IList<IRange> list)
        {
            int left;
            int right;
            switch (this.selectionType)
            {
                case QWhale.Editor.SelectionType.Stream:
                    if ((index > this.selectionRect.Bottom) || (index < this.selectionRect.Top))
                    {
                        goto Label_01BB;
                    }
                    if (index != this.selectionRect.Top)
                    {
                        if (index == this.selectionRect.Bottom)
                        {
                            left = 0;
                            right = this.selectionRect.Right;
                        }
                        else
                        {
                            left = 0;
                            right = 0x7fffffff;
                        }
                        break;
                    }
                    if (index != this.selectionRect.Bottom)
                    {
                        left = this.selectionRect.Left;
                        right = 0x7fffffff;
                        break;
                    }
                    left = this.selectionRect.Left;
                    right = this.SelectionRect.Right;
                    break;

                case QWhale.Editor.SelectionType.Block:
                {
                    IDisplayStrings displayLines = this.owner.DisplayLines;
                    for (int i = displayLines.PointToDisplayPoint(0, index).Y; i <= displayLines.PointToDisplayPoint(this.owner.Lines.GetLength(index), index).Y; i++)
                    {
                        if (this.GetSelectionForLine(i, out left, out right, false))
                        {
                            Point point = displayLines.DisplayPointToPoint(left, i);
                            Point point2 = displayLines.DisplayPointToPoint(right, i, false, false, true);
                            if ((index >= point.Y) && (index <= point2.Y))
                            {
                                if (index == point.Y)
                                {
                                    if (index == point2.Y)
                                    {
                                        left = point.X;
                                        right = point2.X;
                                    }
                                    else
                                    {
                                        left = point.X;
                                        right = 0x7fffffff;
                                    }
                                }
                                else if (index == point2.Y)
                                {
                                    left = 0;
                                    right = point2.X;
                                }
                                else
                                {
                                    left = 0;
                                    right = 0x7fffffff;
                                }
                                list.Add(new Range(left, index, right, index));
                            }
                        }
                    }
                    goto Label_01BB;
                }
                default:
                    goto Label_01BB;
            }
            list.Add(new Range(left, index, right, index));
        Label_01BB:
            return list.Count;
        }

        public virtual bool GetSelectionForLine(int index, out int left, out int right)
        {
            return this.GetSelectionForLine(index, out left, out right, true);
        }

        protected bool GetSelectionForLine(int index, out int left, out int right, bool checkBounds)
        {
            bool flag = false;
            left = 0;
            right = 0;
            if (!this.IsEmpty)
            {
                IDisplayStrings displayLines = this.owner.DisplayLines;
                Point point = displayLines.PointToDisplayPoint(this.selectionRect.Left, this.selectionRect.Top, this.atTopLeftEnd);
                Point point2 = displayLines.PointToDisplayPoint(this.selectionRect.Right, this.selectionRect.Bottom, this.atBottomRightEnd);
                flag = (index >= point.Y) && (index <= point2.Y);
                if (flag)
                {
                    if ((this.selectionType == QWhale.Editor.SelectionType.Block) || (point.Y == point2.Y))
                    {
                        left = point.X;
                        right = point2.X;
                        if (((this.selectionType == QWhale.Editor.SelectionType.Block) && (left == right)) && (point.Y != point2.Y))
                        {
                            if ((left != 0) || (index != point2.Y))
                            {
                                right = 0x7fffffff;
                            }
                            left = 0;
                        }
                    }
                    else if (index == point.Y)
                    {
                        left = point.X;
                        right = 0x7fffffff;
                    }
                    else if (index == point2.Y)
                    {
                        left = 0;
                        right = point2.X;
                    }
                    else
                    {
                        left = 0;
                        right = 0x7fffffff;
                    }
                    this.SwapMaxInt(ref left, ref right);
                }
            }
            if (!flag)
            {
                return false;
            }
            if (checkBounds)
            {
                return (right > left);
            }
            return true;
        }

        private Point GetSelectionPoint()
        {
            if (!this.IsEmpty)
            {
                return this.selectionRect.Location;
            }
            return this.owner.Position;
        }

        private Region GetSelectionRegion(QWhale.Editor.SelectionType selectionType, Rectangle rect)
        {
            return this.owner.SyntaxPaint.GetRectRegion(selectionType, rect, this.atTopLeftEnd, this.atBottomRightEnd);
        }

        private int GetTabIndent(int indent)
        {
            return this.owner.Lines.TabPosToPos(new string('\t', indent), indent);
        }

        public void Indent()
        {
            this.ChangeBlock(this.indentLineEvent, true, this.SelectionType != QWhale.Editor.SelectionType.Block);
        }

        private void IndentLine(string str, ITextUndoList operations)
        {
            ITextStrings lines = this.owner.Lines;
            operations.Add(new TextUndo(0, 0, lines.UseSpaces ? new string(' ', lines.GetTabStop(0)) : "\t"));
        }

        public virtual void InsertString(string s)
        {
            bool flag = true;
            ITextSource source = this.owner.Source;
            bool flag2 = (this.selectionType == QWhale.Editor.SelectionType.Block) && ((this.options & SelectionOptions.ExtendedBlockMode) != SelectionOptions.None);
            if (flag2)
            {
                this.BeginUpdate();
            }
            source.BeginUpdate(UpdateReason.Insert);
            try
            {
                if (!flag2 && this.ShouldDelete())
                {
                    flag = this.Delete();
                }
                if (flag2)
                {
                    IList<IRange> list = new List<IRange>();
                    for (int i = this.selectionRect.Top; i <= this.selectionRect.Bottom; i++)
                    {
                        this.GetSelectedBounds(i, list);
                    }
                    int num2 = 0;
                    if (list.Count >= 0)
                    {
                        IRange range = list[0];
                        num2 = source.Position.X - range.StartPoint.X;
                    }
                    for (int j = list.Count - 1; j >= 0; j--)
                    {
                        IRange range2 = list[j];
                        source.MoveTo(range2.StartPoint.X + num2, range2.StartPoint.Y);
                        source.Insert(s);
                    }
                    this.SetSelection(QWhale.Editor.SelectionType.Block, this.SelectionRect.Location, new Point(this.SelectionRect.Right + s.Length, this.SelectionRect.Bottom));
                }
                else
                {
                    source.Insert(s);
                }
            }
            finally
            {
                source.EndUpdate();
                if (flag2)
                {
                    this.EndUpdate();
                }
            }
            if (!flag && !this.IsEmpty)
            {
                this.Clear();
            }
        }

        public virtual void Invalidate()
        {
            if (this.owner.Source.UpdateCount == 0)
            {
                Region selectionRegion = this.GetSelectionRegion(this.selectionType, this.selectionRect);
                if (selectionRegion != null)
                {
                    this.owner.Invalidate(selectionRegion, false);
                    selectionRegion.Dispose();
                }
            }
        }

        public virtual bool IsPosInSelection(Point position)
        {
            return this.IsPosInSelection(position.X, position.Y, false);
        }

        public virtual bool IsPosInSelection(int x, int y)
        {
            return this.IsPosInSelection(x, y, false);
        }

        protected bool IsPosInSelection(int x, int y, bool checkEnd)
        {
            if (!this.IsEmpty)
            {
                int num;
                int num2;
                Point point = this.owner.DisplayLines.PointToDisplayPoint(x, y);
                if (this.GetSelectionForLine(point.Y, out num, out num2))
                {
                    if (point.X >= num)
                    {
                        if (point.X < num2)
                        {
                            return true;
                        }
                        if (checkEnd)
                        {
                            return (point.X == num2);
                        }
                    }
                    return false;
                }
            }
            return false;
        }

        protected bool IsSelectionRectEmpty()
        {
            return this.IsSelectionRectEmpty(this.selectionRect);
        }

        protected bool IsSelectionRectEmpty(Rectangle rect)
        {
            return this.IsSelectionRectEmpty(this.selectionType, rect);
        }

        protected bool IsSelectionRectEmpty(QWhale.Editor.SelectionType selectionType, Rectangle rect)
        {
            switch (selectionType)
            {
                case QWhale.Editor.SelectionType.Stream:
                    if (rect.Height < 0)
                    {
                        return true;
                    }
                    if (rect.Height != 0)
                    {
                        return false;
                    }
                    return (rect.Width <= 0);

                case QWhale.Editor.SelectionType.Block:
                {
                    IDisplayStrings displayLines = this.owner.DisplayLines;
                    return ((rect.Height < 0) || (displayLines.PointToDisplayPoint(rect.Left, rect.Top, this.atTopLeftEnd).X > displayLines.PointToDisplayPoint(rect.Right, rect.Bottom, this.atBottomRightEnd).X));
                }
            }
            return true;
        }

        public virtual bool IsValidSelectionPoint(Point position)
        {
            switch (this.selectionType)
            {
                case QWhale.Editor.SelectionType.Stream:
                    return (((this.selectionRect.Top == position.Y) && (this.selectionRect.Left == position.X)) || ((this.selectionRect.Bottom == position.Y) && (this.selectionRect.Right == position.X)));

                case QWhale.Editor.SelectionType.Block:
                {
                    IDisplayStrings displayLines = this.owner.DisplayLines;
                    Point point = displayLines.PointToDisplayPoint(this.selectionRect.Left, this.selectionRect.Top, this.atTopLeftEnd);
                    Point point2 = displayLines.PointToDisplayPoint(this.selectionRect.Right, this.selectionRect.Bottom, this.atBottomRightEnd);
                    Point point3 = displayLines.PointToDisplayPoint(position);
                    if ((point.Y != point3.Y) && (point2.Y != point3.Y))
                    {
                        return false;
                    }
                    if (point.X != point3.X)
                    {
                        return (point2.X == point3.X);
                    }
                    return true;
                }
            }
            return true;
        }

        public virtual void LineTransponse()
        {
            if (this.owner.Position.Y < (this.owner.Lines.Count - 1))
            {
                ITextSource source = this.owner.Source;
                source.BeginUpdate(UpdateReason.Insert);
                try
                {
                    string text = this.owner.Lines[this.owner.Position.Y];
                    this.owner.MoveLineBegin();
                    source.DeleteRight(0x7fffffff);
                    source.UnBreakLine();
                    this.owner.MoveLineEnd();
                    source.BreakLine();
                    source.MoveTo(0, source.Position.Y + 1);
                    source.Insert(text);
                }
                finally
                {
                    source.EndUpdate();
                }
            }
        }

        public virtual void LowerCase()
        {
            this.ChangeBlock(this.lowerCaseLineEvent, true, false);
        }

        private void LowerCaseLine(string str, ITextUndoList operations)
        {
            operations.Add(new TextUndo(0, str.Length, str.ToLower()));
        }

        public virtual bool Move(Point position, bool deleteOrigin)
        {
            bool flag = (!this.IsPosInSelection(position) && !this.owner.Source.Readonly) && !this.IsEmpty;
            if (flag)
            {
                bool flag2 = true;
                QWhale.Editor.SelectionType selectionType = this.selectionType;
                ITextSource source = this.owner.Source;
                this.BeginUpdate();
                source.BeginUpdate(UpdateReason.Other);
                try
                {
                    int index = source.StorePosition(position);
                    string selectedText = this.SelectedText;
                    if (deleteOrigin)
                    {
                        flag2 = this.Delete();
                    }
                    source.Position = source.RestorePosition(index);
                    if (flag2)
                    {
                        this.Clear();
                        this.SetSelectedText(selectedText, selectionType);
                    }
                }
                finally
                {
                    source.EndUpdate();
                    this.EndUpdate();
                }
                if (!flag2 && !this.IsEmpty)
                {
                    this.Clear();
                }
            }
            return flag;
        }

        private void MoveSelection()
        {
            Point pt = this.UpdateWordSelectionEnd(this.selEnd);
            this.UpdateWordSelection(ref pt, this.selForward);
            if (this.selectionType == QWhale.Editor.SelectionType.Block)
            {
                ITextSource source = this.owner.Source;
                NavigateOptions navigateOptions = source.NavigateOptions;
                try
                {
                    source.SetNavigateOptions(navigateOptions | NavigateOptions.BeyondEol);
                    this.owner.Position = pt;
                }
                finally
                {
                    source.SetNavigateOptions(navigateOptions);
                }
            }
            else
            {
                this.owner.Position = pt;
            }
        }

        protected virtual bool NeedDragScroll(Point pt)
        {
            Rectangle clientRect = this.owner.ClientRect;
            Rectangle rectangle2 = clientRect;
            rectangle2.Inflate(-this.owner.Painter.FontWidth, -this.owner.Painter.FontHeight);
            if (this.owner.WordWrap)
            {
                if (!this.owner.WrapAtMargin)
                {
                    if ((pt.Y < clientRect.Top) || (pt.Y > clientRect.Bottom))
                    {
                        return false;
                    }
                    if (pt.Y >= rectangle2.Top)
                    {
                        return (pt.Y > rectangle2.Bottom);
                    }
                    return true;
                }
                if (pt.X >= this.owner.DisplayToScreen(this.owner.EditMargin.Position, 0).X)
                {
                    if ((pt.Y < clientRect.Top) || (pt.Y > clientRect.Bottom))
                    {
                        return false;
                    }
                    if (pt.Y >= rectangle2.Top)
                    {
                        return (pt.Y > rectangle2.Bottom);
                    }
                    return true;
                }
            }
            return (clientRect.Contains(pt) && !rectangle2.Contains(pt));
        }

        public virtual void NewLine()
        {
            if (!this.owner.SingleLineMode)
            {
                bool flag = true;
                ITextSource source = this.owner.Source;
                this.BeginUpdate();
                source.BeginUpdate(UpdateReason.Break);
                try
                {
                    if (!this.owner.ProcessEnter())
                    {
                        if (this.ShouldDelete())
                        {
                            flag = this.Delete();
                        }
                        Point position = source.Position;
                        if (source.NewLine() && ((source.IndentOptions & IndentOptions.AutoIndent) != IndentOptions.None))
                        {
                            Point point2 = source.Position;
                            string code = string.Empty;
                            if ((source.NeedAutoComplete() && (source.Lines[point2.Y].Trim() == string.Empty)) && source.ProcessAutoComplete(out code))
                            {
                                string[] sourceArray = StringItem.Split(code);
                                if (sourceArray.Length > 0)
                                {
                                    string text = sourceArray[0];
                                    if (text != string.Empty)
                                    {
                                        source.MoveTo(source.Lines[position.Y].TrimEnd(new char[0]).Length, position.Y);
                                        source.Insert(text);
                                        source.MoveTo(point2);
                                    }
                                    string[] destinationArray = new string[sourceArray.Length - 1];
                                    Array.Copy(sourceArray, 1, destinationArray, 0, destinationArray.Length);
                                    source.InsertBlock(destinationArray);
                                    if (destinationArray.Length > 1)
                                    {
                                        this.SetSelection(QWhale.Editor.SelectionType.Stream, point2, this.owner.Position);
                                        this.SmartFormat();
                                        source.Position = point2;
                                        this.Clear();
                                        if (source.Lines[source.Position.Y].Trim() == string.Empty)
                                        {
                                            source.DeleteLeft(source.Position.X);
                                            source.IndentLine();
                                        }
                                    }
                                }
                            }
                            else
                            {
                                this.SmartIndent('\r');
                            }
                        }
                    }
                }
                finally
                {
                    source.EndUpdate();
                    if (!flag && !this.IsEmpty)
                    {
                        this.Clear();
                    }
                    this.EndUpdate();
                }
            }
        }

        public virtual void NewLineAbove()
        {
            if (!this.owner.ProcessEnter())
            {
                this.owner.Source.NewLineAbove();
            }
        }

        public virtual void NewLineBelow()
        {
            if (!this.owner.ProcessEnter())
            {
                this.owner.Source.NewLineBelow();
            }
        }

        protected virtual void OnAllowedSelectionModeChanged()
        {
            this.SetSelection(this.selectionType, this.selectionRect);
        }

        protected virtual void OnBackColorChanged()
        {
            this.Invalidate();
        }

        protected virtual void OnBorderColorChanged()
        {
            this.Invalidate();
        }

        protected virtual void OnForeColorChanged()
        {
            this.Invalidate();
        }

        protected virtual void OnInActiveBackColorChanged()
        {
            this.Invalidate();
        }

        protected virtual void OnInActiveForeColorChanged()
        {
            this.Invalidate();
        }

        protected virtual void OnOptionsChanged()
        {
            if ((SelectionOptions.DisableSelection & this.options) != SelectionOptions.None)
            {
                this.Clear();
            }
            if (this.owner != null)
            {
                this.owner.OnStateChanged(this, NotifyState.SelectionOptionsChanged);
            }
            this.Invalidate();
        }

        public virtual void OnSelect(object source, EventArgs e)
        {
            bool flag;
            Point empty;
            IDisplayStrings displayLines;
            if ((this.selectionState != QWhale.Editor.SelectionState.None) && (this.owner.Source.UpdateCount <= 0))
            {
                Point pt = this.owner.PointToClient(Cursor.Position);
                flag = false;
                empty = Point.Empty;
                ITextSource source2 = this.owner.Source;
                NavigateOptions navigateOptions = source2.NavigateOptions;
                try
                {
                    source2.SetNavigateOptions(navigateOptions & ~NavigateOptions.BeyondEof);
                    empty = this.owner.ScreenToText(pt.X, pt.Y, ref flag);
                }
                finally
                {
                    source2.SetNavigateOptions(navigateOptions);
                }
                if (empty.Equals(this.selEnd) && (this.selectionState != QWhale.Editor.SelectionState.SelectLine))
                {
                    return;
                }
                displayLines = this.owner.DisplayLines;
                switch (this.selectionState)
                {
                    case QWhale.Editor.SelectionState.Drag:
                        if (!this.owner.ClientRect.Contains(pt) || this.NeedDragScroll(pt))
                        {
                            return;
                        }
                        this.BeginUpdate();
                        displayLines.DisableUpdate();
                        try
                        {
                            displayLines.LineEnd = flag;
                            this.owner.Position = empty;
                            return;
                        }
                        finally
                        {
                            displayLines.EnableUpdate();
                            this.EndUpdate();
                        }
                        goto Label_012E;

                    case QWhale.Editor.SelectionState.Select:
                        goto Label_012E;

                    case QWhale.Editor.SelectionState.SelectWord:
                        goto Label_02A3;

                    case QWhale.Editor.SelectionState.SelectLine:
                        goto Label_018B;
                }
            }
            return;
        Label_012E:
            this.BeginUpdate();
            displayLines.DisableUpdate();
            try
            {
                this.selEnd = empty;
                displayLines.LineEnd = flag;
                this.SelectBlock((e != null) ? QWhale.Editor.SelectionType.None : this.selectionType, this.CanSelectBlock() ? QWhale.Editor.SelectionType.Block : QWhale.Editor.SelectionType.Stream, this.selStart, this.moveSelection);
                return;
            }
            finally
            {
                displayLines.EnableUpdate();
                this.EndUpdate();
            }
        Label_018B:
            this.BeginUpdate();
            displayLines.DisableUpdate();
            try
            {
                displayLines.LineEnd = flag;
                Point selStart = this.selStart;
                if (empty.Y < this.selStart.Y)
                {
                    selStart = new Point(0, this.selStart.Y + 1);
                }
                else if (empty.Y >= this.selStart.Y)
                {
                    selStart.X = 0;
                }
                if (empty.X != 0)
                {
                    selStart.X = 0;
                    this.selectionState = QWhale.Editor.SelectionState.Select;
                }
                else if (empty.Y >= this.selStart.Y)
                {
                    empty.Y++;
                }
                if (!this.selEnd.Equals(empty) || !this.selStart.Equals(selStart))
                {
                    this.selEnd = empty;
                    this.SelectBlock((e != null) ? QWhale.Editor.SelectionType.None : this.selectionType, this.CanSelectBlock() ? QWhale.Editor.SelectionType.Block : QWhale.Editor.SelectionType.Stream, selStart, this.moveSelection);
                }
                return;
            }
            finally
            {
                displayLines.EnableUpdate();
                this.EndUpdate();
            }
        Label_02A3:
            this.BeginUpdate();
            displayLines.DisableUpdate();
            try
            {
                displayLines.LineEnd = flag;
                this.selForward = (this.selEnd.Y > this.selStart.Y) || ((this.selEnd.Y == this.selStart.Y) && (this.selEnd.X >= this.selStart.X));
                Point point4 = this.selStart;
                this.UpdateWordSelection(ref point4, !this.selForward);
                this.selEnd = empty;
                this.SelectBlock((e != null) ? QWhale.Editor.SelectionType.None : this.selectionType, this.CanSelectBlock() ? QWhale.Editor.SelectionType.Block : QWhale.Editor.SelectionType.Stream, point4, this.moveSelection);
            }
            finally
            {
                displayLines.EnableUpdate();
                this.EndUpdate();
            }
        }

        public virtual void OnSelectionChanged()
        {
            if (this.updateCount == 0)
            {
                this.owner.OnStateChanged(this, NotifyState.SelectionChanged);
                if (this.SelectionChanged != null)
                {
                    this.SelectionChanged(this, EventArgs.Empty);
                }
            }
        }

        protected virtual void OnSelectionStateChanged()
        {
        }

        public virtual void Paste()
        {
            object data = null;
            if (Clipboard.ContainsText(TextDataFormat.UnicodeText))
            {
                data = Clipboard.GetData(DataFormats.UnicodeText);
            }
            else if (Clipboard.ContainsText(TextDataFormat.Text))
            {
                data = Clipboard.GetData(DataFormats.Text);
            }
            if (data != null)
            {
                ITextSource source = this.owner.Source;
                source.BeginUpdate(UpdateReason.Delete);
                this.BeginUpdate();
                try
                {
                    object obj3 = Clipboard.GetData(DataFormats.Serializable);
                    this.SetSelectedText((string) data, ((obj3 is QWhale.Editor.SelectionType) && (((QWhale.Editor.SelectionType) obj3) != QWhale.Editor.SelectionType.None)) ? ((QWhale.Editor.SelectionType) obj3) : QWhale.Editor.SelectionType.Stream, true);
                    if (((this.options & SelectionOptions.ConvertToSpacesOnPaste) != SelectionOptions.None) && this.owner.Lines.UseSpaces)
                    {
                        this.UnTabify();
                    }
                    if ((((this.options & SelectionOptions.SmartFormat) != SelectionOptions.None) && !this.IsEmpty) && (this.selectionRect.Height > 0))
                    {
                        this.SmartFormat();
                    }
                    if ((this.options & SelectionOptions.PersistentBlocks) == SelectionOptions.None)
                    {
                        this.Clear();
                    }
                    this.UpdateSelStart(false);
                }
                finally
                {
                    this.EndUpdate();
                    source.EndUpdate();
                }
            }
        }

        public virtual void PositionChanged(int x, int y, int deltaX, int deltaY)
        {
            if ((this.updateCount == 0) && !this.IsEmpty)
            {
                Point location = this.selectionRect.Location;
                Point pt = new Point(this.selectionRect.Right, this.selectionRect.Bottom);
                bool flag = Range.UpdatePos(x, y, deltaX, deltaY, ref location, false);
                bool flag2 = Range.UpdatePos(x, y, deltaX, deltaY, ref pt, true);
                if (flag || flag2)
                {
                    this.SetSelection(this.selectionType, new Rectangle(location.X, location.Y, pt.X - location.X, pt.Y - location.Y));
                }
            }
        }

        public virtual void ProcessEscape()
        {
            this.SelectionType = QWhale.Editor.SelectionType.None;
        }

        public virtual void ProcessShiftTab()
        {
            if (!this.owner.ProcessShiftTab(this.owner.Position))
            {
                if (this.IsEmpty)
                {
                    this.owner.MoveToChar(this.owner.Lines.GetPrevTabStop(this.owner.Position.X));
                }
                else
                {
                    this.UnIndent();
                }
            }
        }

        public virtual void ProcessTab()
        {
            if (!this.owner.ProcessTab(this.owner.Position))
            {
                if (this.IsEmpty || (this.SelectedCount() == 1))
                {
                    if (!this.owner.Lines.UseSpaces)
                    {
                        this.InsertString('\t'.ToString());
                    }
                    else
                    {
                        ITextStrings lines = this.owner.Lines;
                        Point position = this.owner.Position;
                        position.X = lines.TabPosToPos(lines[position.Y], position.X);
                        int tabStop = lines.GetTabStop(position.X);
                        int count = tabStop - position.X;
                        if (count > 0)
                        {
                            this.InsertString(new string(' ', count));
                        }
                    }
                }
                else if (!this.owner.Source.Readonly && !this.SelectionContainsReadonlyLines())
                {
                    this.Indent();
                }
            }
        }

        public virtual void ResetAllowedSelectionMode()
        {
            this.AllowedSelectionMode = EditConsts.DefaultSelectionMode;
        }

        public virtual void ResetBackColor()
        {
            this.BackColor = EditConsts.DefaultHighlightBackColor;
        }

        public virtual void ResetBorderColor()
        {
            this.BorderColor = EditConsts.DefaultSelectionBorderColor;
        }

        public virtual void ResetForeColor()
        {
            this.ForeColor = EditConsts.DefaultHighlightForeColor;
        }

        public virtual void ResetInActiveBackColor()
        {
            this.InActiveBackColor = EditConsts.DefaultInactiveHighlightBackColor;
        }

        public virtual void ResetInActiveForeColor()
        {
            this.InActiveForeColor = EditConsts.DefaultInactiveHighlightForeColor;
        }

        public virtual void ResetOptions()
        {
            this.Options = EditConsts.DefaultSelectionOptions;
        }

        protected void RestoreSelection(ITextSource source, QWhale.Editor.SelectionType selType, int index1, int index2, int index3)
        {
            source.Position = source.RestorePosition(index3);
            if ((index1 >= 0) && (index2 >= 0))
            {
                Point selectionEnd = source.RestorePosition(index2);
                Point selectionStart = source.RestorePosition(index1);
                this.SetSelection(selType, selectionStart, selectionEnd);
            }
            else
            {
                this.Clear();
            }
        }

        private string SafeSubString(string s, int left, int right)
        {
            if (left >= s.Length)
            {
                return string.Empty;
            }
            if (right < s.Length)
            {
                return s.Substring(left, right - left);
            }
            return s.Substring(left);
        }

        private string SafeSubString(short[] data, int left, int right)
        {
            if (left >= data.Length)
            {
                return string.Empty;
            }
            char[] chArray = new char[Math.Min(right, data.Length) - left];
            for (int i = 0; i < chArray.Length; i++)
            {
                chArray[i] = (char) ((ushort) data[i + left]);
            }
            return new string(chArray);
        }

        public virtual bool ScrollIfNeeded(Point pt)
        {
            if (this.NeedDragScroll(pt))
            {
                this.DragScroll(pt);
                return true;
            }
            return false;
        }

        public virtual void SelectAll()
        {
            if (this.owner.Lines.Count > 0)
            {
                this.selStart = this.owner.Position;
                this.SetSelection(QWhale.Editor.SelectionType.Stream, new Rectangle(0, 0, this.owner.Lines.GetLength(this.owner.Lines.Count - 1), this.owner.Lines.Count - 1));
            }
        }

        private void SelectBlock(QWhale.Editor.SelectionType newSelType, KeyEvent action)
        {
            this.UpdateSelStart(true);
            this.SelectBlock(this.selectionType, newSelType, this.selStart, action);
        }

        private void SelectBlock(QWhale.Editor.SelectionType selType, QWhale.Editor.SelectionType newSelType, Point position, KeyEvent action)
        {
            Rectangle selectionRect = this.SelectionRect;
            int x = position.X;
            int y = position.Y;
            int num3 = y;
            if ((newSelType == QWhale.Editor.SelectionType.Block) && !this.owner.Painter.IsMonoSpaced)
            {
                newSelType = QWhale.Editor.SelectionType.Stream;
            }
            this.CheckSelectionMode(ref newSelType);
            this.BeginUpdate();
            try
            {
                Point point2;
                IDisplayStrings displayLines = this.owner.DisplayLines;
                if ((selType == QWhale.Editor.SelectionType.None) || !this.IsValidSelectionPoint(position))
                {
                    selectionRect.X = position.X;
                    selectionRect.Y = position.Y;
                    selectionRect.Width = 0;
                    selectionRect.Height = 0;
                }
                Point point = displayLines.PointToDisplayPoint(x, y);
                if (action != null)
                {
                    if ((newSelType == QWhale.Editor.SelectionType.Block) && !this.owner.WordWrap)
                    {
                        ITextSource source = this.owner.Source;
                        NavigateOptions navigateOptions = source.NavigateOptions;
                        try
                        {
                            source.SetNavigateOptions(navigateOptions | NavigateOptions.BeyondEol);
                            action();
                            goto Label_00E1;
                        }
                        finally
                        {
                            source.SetNavigateOptions(navigateOptions);
                        }
                    }
                    action();
                }
            Label_00E1:
                point2 = this.owner.Position;
                if (num3 >= this.owner.Lines.Count)
                {
                    ITextSource source2 = this.owner.Source;
                    NavigateOptions options2 = source2.NavigateOptions;
                    try
                    {
                        source2.SetNavigateOptions(options2 & ~NavigateOptions.BeyondEof);
                        source2.ValidatePosition(ref point2);
                    }
                    finally
                    {
                        source2.SetNavigateOptions(options2);
                    }
                }
                if (newSelType == QWhale.Editor.SelectionType.Block)
                {
                    point2 = displayLines.PointToDisplayPoint(point2);
                    if (point2.X < point.X)
                    {
                        int num4 = point.X;
                        point.X = point2.X;
                        point2.X = num4;
                    }
                    if (point2.Y < point.Y)
                    {
                        int num5 = point.Y;
                        point.Y = point2.Y;
                        point2.Y = num5;
                    }
                    point = displayLines.DisplayPointToPoint(point.X, point.Y, true, false, false);
                    point2 = displayLines.DisplayPointToPoint(point2.X, point2.Y, true, false, false);
                    selectionRect = new Rectangle(point.X, point.Y, point2.X - point.X, point2.Y - point.Y);
                }
                else
                {
                    selectionRect = new Rectangle(x, y, point2.X - x, point2.Y - y);
                    if ((selectionRect.Height < 0) || ((selectionRect.Width < 0) && (selectionRect.Height == 0)))
                    {
                        this.SwapRect(ref selectionRect, false);
                    }
                }
                if ((this.owner.Source.UpdateCount == 0) && !this.IsEmpty)
                {
                    Region selectionRegion = this.GetSelectionRegion(this.SelectionType, this.SelectionRect);
                    if (selectionRegion != null)
                    {
                        this.owner.Invalidate(selectionRegion);
                        selectionRegion.Dispose();
                    }
                }
                if (newSelType != QWhale.Editor.SelectionType.None)
                {
                    if (selectionRect.Location == this.owner.Position)
                    {
                        this.atTopLeftEnd = displayLines.LineEnd;
                    }
                    if (this.owner.Position == new Point(selectionRect.Right, selectionRect.Bottom))
                    {
                        this.atBottomRightEnd = displayLines.LineEnd;
                    }
                }
                this.SetSelection(newSelType, selectionRect);
            }
            finally
            {
                this.EndUpdate();
            }
        }

        public virtual void SelectCharLeft()
        {
            this.SelectCharLeft(QWhale.Editor.SelectionType.Stream);
        }

        public virtual void SelectCharLeft(QWhale.Editor.SelectionType selectionType)
        {
            this.SelectBlock(selectionType, this.KeyHandlers.moveCharLeftEvent);
        }

        public virtual void SelectCharRight()
        {
            this.SelectCharRight(QWhale.Editor.SelectionType.Stream);
        }

        public virtual void SelectCharRight(QWhale.Editor.SelectionType selectionType)
        {
            this.SelectBlock(selectionType, this.KeyHandlers.moveCharRightEvent);
        }

        public bool SelectCurrentWord()
        {
            ITextStrings lines = this.owner.Lines;
            Point position = this.owner.Position;
            while (position.Y < lines.Count)
            {
                string s = lines[position.Y];
                int length = s.Length;
                while ((position.X < length) && lines.IsDelimiter(s, position.X))
                {
                    position.X++;
                }
                if (position.X < length)
                {
                    break;
                }
                position.Y++;
                position.X = 0;
            }
            if (position.Y < lines.Count)
            {
                this.owner.Position = position;
                this.SelectWord();
            }
            return (position.Y < lines.Count);
        }

        public virtual int SelectedCount()
        {
            if (this.selectionType == QWhale.Editor.SelectionType.None)
            {
                return 0;
            }
            return ((this.SelectionRect.Bottom - this.SelectionRect.Top) + 1);
        }

        public string SelectedData(int index)
        {
            if (!this.IsEmpty)
            {
                int top = this.selectionRect.Top;
                IList<IRange> list = new List<IRange>();
                this.GetSelectedBounds(index + top, list);
                if (list.Count == 0)
                {
                    return null;
                }
                switch (this.SelectionType)
                {
                    case QWhale.Editor.SelectionType.Stream:
                    {
                        IRange range2 = list[0];
                        IStringItem item2 = this.owner.Lines.GetItem(range2.StartPoint.Y);
                        if (item2 != null)
                        {
                            return this.SafeSubString(item2.TextData, range2.StartPoint.X, range2.EndPoint.X);
                        }
                        return string.Empty;
                    }
                    case QWhale.Editor.SelectionType.Block:
                    {
                        StringBuilder builder = new StringBuilder();
                        for (int i = 0; i < list.Count; i++)
                        {
                            IRange range = list[i];
                            IStringItem item = this.owner.Lines.GetItem(range.StartPoint.Y);
                            if (item != null)
                            {
                                builder.Append(this.SafeSubString(item.TextData, range.StartPoint.X, range.EndPoint.X));
                            }
                            if (i != (list.Count - 1))
                            {
                                builder.Append(new string('\0', this.owner.LineTerminator.Length));
                            }
                        }
                        return builder.ToString();
                    }
                }
            }
            return null;
        }

        public virtual string SelectedString(int index)
        {
            if (!this.IsEmpty)
            {
                int top = this.selectionRect.Top;
                IList<IRange> list = new List<IRange>();
                this.GetSelectedBounds(index + top, list);
                if (list.Count == 0)
                {
                    return null;
                }
                switch (this.SelectionType)
                {
                    case QWhale.Editor.SelectionType.Stream:
                    {
                        IRange range2 = list[0];
                        return this.SafeSubString(this.owner.Lines[range2.StartPoint.Y], range2.StartPoint.X, range2.EndPoint.X);
                    }
                    case QWhale.Editor.SelectionType.Block:
                    {
                        StringBuilder builder = new StringBuilder();
                        for (int i = 0; i < list.Count; i++)
                        {
                            IRange range = list[i];
                            builder.Append(this.SafeSubString(this.owner.Lines[range.StartPoint.Y], range.StartPoint.X, range.EndPoint.X));
                            if (i != (list.Count - 1))
                            {
                                builder.Append(this.owner.LineTerminator);
                            }
                        }
                        return builder.ToString();
                    }
                }
            }
            return null;
        }

        public virtual void SelectFileBegin()
        {
            this.SelectFileBegin(QWhale.Editor.SelectionType.Stream);
        }

        public virtual void SelectFileBegin(QWhale.Editor.SelectionType selectionType)
        {
            this.SelectBlock(selectionType, this.KeyHandlers.moveFileBeginEvent);
        }

        public virtual void SelectFileEnd()
        {
            this.SelectFileEnd(QWhale.Editor.SelectionType.Stream);
        }

        public virtual void SelectFileEnd(QWhale.Editor.SelectionType selectionType)
        {
            this.SelectBlock(selectionType, this.KeyHandlers.moveFileEndEvent);
        }

        private bool SelectionContainsReadonlyLines()
        {
            if (!this.IsEmpty)
            {
                for (int i = this.SelectionRect.Top; i < this.SelectionRect.Bottom; i++)
                {
                    if (this.owner.Source.LineIsReadonly(i))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public virtual Rectangle SelectionToScreen()
        {
            if (!this.IsEmpty)
            {
                Point point = this.owner.TextToScreen(this.selectionRect.Location, this.atTopLeftEnd);
                Point point2 = this.owner.TextToScreen(this.selectionRect.Location + this.selectionRect.Size, this.atBottomRightEnd);
                return new Rectangle(point.X, point.Y, point2.X - point.X, (point2.Y - point.Y) + this.owner.Painter.FontHeight);
            }
            return new Rectangle(0, 0, 0, 0);
        }

        public virtual Point SelectionToTextPoint(Point position)
        {
            switch (this.selectionType)
            {
                case QWhale.Editor.SelectionType.Stream:
                    if (position.Y != 0)
                    {
                        return new Point(position.X, this.selectionRect.Top + position.Y);
                    }
                    return new Point(position.X + this.selectionRect.Left, this.selectionRect.Top);

                case QWhale.Editor.SelectionType.Block:
                    return new Point(position.X + this.selectionRect.Left, position.Y + this.selectionRect.Top);
            }
            return position;
        }

        public virtual void SelectLine()
        {
            ITextSource source = this.owner.Source;
            NavigateOptions navigateOptions = source.NavigateOptions;
            try
            {
                source.SetNavigateOptions(navigateOptions | NavigateOptions.BeyondEof);
                Point position = this.owner.DisplayLines.PointToDisplayPoint(this.owner.Position);
                position.X = 0;
                Point selectionStart = this.owner.DisplayLines.DisplayPointToPoint(position);
                position.Y++;
                Point selectionEnd = this.owner.DisplayLines.DisplayPointToPoint(position);
                this.SetSelection(QWhale.Editor.SelectionType.Stream, selectionStart, selectionEnd);
            }
            finally
            {
                source.SetNavigateOptions(navigateOptions);
            }
        }

        public virtual void SelectLineBegin()
        {
            this.SelectLineBegin(QWhale.Editor.SelectionType.Stream);
        }

        public virtual void SelectLineBegin(QWhale.Editor.SelectionType selectionType)
        {
            this.SelectBlock(selectionType, this.KeyHandlers.moveLineBeginEvent);
        }

        public virtual void SelectLineDown()
        {
            this.SelectLineDown(QWhale.Editor.SelectionType.Stream);
        }

        public virtual void SelectLineDown(QWhale.Editor.SelectionType selectionType)
        {
            this.SelectBlock(selectionType, this.KeyHandlers.moveLineDownEvent);
        }

        public virtual void SelectLineEnd()
        {
            this.SelectLineEnd(QWhale.Editor.SelectionType.Stream);
        }

        public virtual void SelectLineEnd(QWhale.Editor.SelectionType selectionType)
        {
            this.SelectBlock(selectionType, this.KeyHandlers.moveLineEndEvent);
        }

        public virtual void SelectLineUp()
        {
            this.SelectLineUp(QWhale.Editor.SelectionType.Stream);
        }

        public virtual void SelectLineUp(QWhale.Editor.SelectionType selectionType)
        {
            this.SelectBlock(selectionType, this.KeyHandlers.moveLineUpEvent);
        }

        public bool SelectNextWord()
        {
            ITextStrings lines = this.owner.Lines;
            Point position = this.owner.Position;
            string s = lines[position.Y];
            while ((position.X < s.Length) && !lines.IsDelimiter(s, position.X))
            {
                position.X++;
            }
            this.owner.Position = position;
            return this.SelectCurrentWord();
        }

        public virtual void SelectPageDown()
        {
            this.SelectPageDown(QWhale.Editor.SelectionType.Stream);
        }

        public virtual void SelectPageDown(QWhale.Editor.SelectionType selectionType)
        {
            this.SelectBlock(selectionType, this.KeyHandlers.movePageDownEvent);
        }

        public virtual void SelectPageUp()
        {
            this.SelectPageUp(QWhale.Editor.SelectionType.Stream);
        }

        public virtual void SelectPageUp(QWhale.Editor.SelectionType selectionType)
        {
            this.SelectBlock(selectionType, this.KeyHandlers.movePageUpEvent);
        }

        public virtual void SelectScreenBottom()
        {
            this.SelectScreenBottom(QWhale.Editor.SelectionType.Stream);
        }

        public virtual void SelectScreenBottom(QWhale.Editor.SelectionType selectionType)
        {
            this.SelectBlock(selectionType, this.KeyHandlers.moveScreenBottomEvent);
        }

        public virtual void SelectScreenTop()
        {
            this.SelectScreenTop(QWhale.Editor.SelectionType.Stream);
        }

        public virtual void SelectScreenTop(QWhale.Editor.SelectionType selectionType)
        {
            this.SelectBlock(selectionType, this.KeyHandlers.moveScreenTopEvent);
        }

        public virtual void SelectToBrace()
        {
            Point position = this.owner.Position;
            bool flag = false;
            bool flag2 = false;
            if (Array.IndexOf<char>(this.owner.Braces.OpenBraces, this.owner.Lines.GetCharAt(position)) >= 0)
            {
                flag = false;
                flag2 = true;
            }
            else if (Array.IndexOf<char>(this.owner.Braces.ClosingBraces, this.owner.Lines.GetCharAt(position)) >= 0)
            {
                flag = false;
                flag2 = true;
            }
            else if ((position.X > 0) && (Array.IndexOf<char>(this.owner.Braces.OpenBraces, this.owner.Lines.GetCharAt(new Point(position.X - 1, position.Y))) >= 0))
            {
                flag = true;
                flag2 = true;
            }
            else if ((position.X > 0) && (Array.IndexOf<char>(this.owner.Braces.ClosingBraces, this.owner.Lines.GetCharAt(new Point(position.X - 1, position.Y))) >= 0))
            {
                flag = false;
                flag2 = false;
            }
            this.SelectBlock(this.selectionType, (this.SelectionType != QWhale.Editor.SelectionType.None) ? this.SelectionType : QWhale.Editor.SelectionType.Stream, position, this.KeyHandlers.moveToBraceEvent);
            if ((flag || flag2) && (!this.owner.Position.Equals(position) && !this.IsEmpty))
            {
                Rectangle selectionRect = this.SelectionRect;
                if (flag && (selectionRect.X > 0))
                {
                    selectionRect.X--;
                    selectionRect.Width++;
                }
                if (flag2)
                {
                    selectionRect.Width++;
                }
                this.SetSelection(this.selectionType, selectionRect);
            }
        }

        public virtual void SelectToCloseBrace()
        {
            this.SelectBlock((this.SelectionType != QWhale.Editor.SelectionType.None) ? this.SelectionType : QWhale.Editor.SelectionType.Stream, this.KeyHandlers.moveToCloseBraceEvent);
        }

        public virtual void SelectToOpenBrace()
        {
            this.SelectBlock((this.SelectionType != QWhale.Editor.SelectionType.None) ? this.SelectionType : QWhale.Editor.SelectionType.Stream, this.KeyHandlers.moveToOpenBraceEvent);
        }

        public virtual void SelectWord()
        {
            int num;
            int num2;
            ITextStrings lines = this.owner.Lines;
            Point position = this.owner.Position;
            string s = this.owner.Lines[position.Y];
            if (((position.X > 0) && (position.X < s.Length)) && (this.owner.Lines.IsDelimiter(s, position.X) && !this.owner.Lines.IsDelimiter(s, position.X - 1)))
            {
                position.X--;
            }
            if (lines.GetWord(position.Y, position.X, out num, out num2))
            {
                this.owner.MoveTo(num2 + 1, position.Y);
                this.selStart = new Point(num, position.Y);
                this.SetSelection(QWhale.Editor.SelectionType.Stream, new Rectangle(num, position.Y, (num2 - num) + 1, 0));
            }
        }

        public virtual void SelectWordLeft()
        {
            this.SelectWordLeft(QWhale.Editor.SelectionType.Stream);
        }

        public virtual void SelectWordLeft(QWhale.Editor.SelectionType selectionType)
        {
            this.SelectBlock(selectionType, this.KeyHandlers.moveWordLeftEvent);
        }

        public virtual void SelectWordRight()
        {
            this.SelectWordRight(QWhale.Editor.SelectionType.Stream);
        }

        public virtual void SelectWordRight(QWhale.Editor.SelectionType selectionType)
        {
            this.SelectBlock(selectionType, this.KeyHandlers.moveWordRightEvent);
        }

        private void SetRectLeft(ref Rectangle rect, int left)
        {
            int num = rect.Right - left;
            rect.X = left;
            rect.Width = num;
        }

        private void SetRectTop(ref Rectangle rect, int top)
        {
            int num = rect.Bottom - top;
            rect.Y = top;
            rect.Height = num;
        }

        public void SetSelectedText(string text, QWhale.Editor.SelectionType selType)
        {
            this.SetSelectedText(text, selType, false);
        }

        public void SetSelectedText(string text, QWhale.Editor.SelectionType selType, bool moveToEnd)
        {
            this.BeginUpdate();
            ITextSource source = this.owner.Source;
            source.BeginUpdate(UpdateReason.Other);
            NavigateOptions navigateOptions = source.NavigateOptions;
            try
            {
                source.SetNavigateOptions(navigateOptions | NavigateOptions.BeyondEol);
                this.owner.OnStateChanged(this, NotifyState.SelectedTextChanged);
                Point point = this.IsEmpty ? source.Position : this.SelectionRect.Location;
                bool flag = !this.IsEmpty && source.Position.Equals(point);
                string s = source.Lines[point.Y];
                int length = s.Length;
                if ((point.X > length) && !source.Lines.UseSpaces)
                {
                    point.X = length + source.Lines.GetIndentString(point.X - length, source.Lines.TabPosToPos(s, length)).Length;
                }
                if (this.Delete())
                {
                    if ((text != string.Empty) && (text != null))
                    {
                        if (selType == QWhale.Editor.SelectionType.Stream)
                        {
                            source.InsertBlock(text);
                            this.SetSelection(QWhale.Editor.SelectionType.Stream, point, this.owner.Position);
                            if (flag && !moveToEnd)
                            {
                                this.owner.Position = point;
                            }
                        }
                        else
                        {
                            source.Position = point;
                            source.BeginUpdate(UpdateReason.Insert);
                            try
                            {
                                string[] strArray = StringItem.Split(text);
                                length = 0;
                                for (int i = 0; i < strArray.Length; i++)
                                {
                                    if ((i > 0) && (selType == QWhale.Editor.SelectionType.Block))
                                    {
                                        source.Navigate(-length, 0);
                                    }
                                    length = this.DoInsertString(selType, strArray[i], i != 0);
                                }
                                this.SetSelection(selType, point, this.owner.Position);
                                if (flag)
                                {
                                    this.owner.Position = point;
                                }
                            }
                            finally
                            {
                                this.owner.Source.EndUpdate();
                            }
                        }
                    }
                    this.selStart = new Point(this.selectionRect.Left, this.selectionRect.Top);
                    this.selEnd = new Point(this.selectionRect.Right, this.selectionRect.Bottom);
                }
            }
            finally
            {
                source.SetNavigateOptions(navigateOptions);
                source.EndUpdate();
                this.EndUpdate();
            }
        }

        public virtual void SetSelection(QWhale.Editor.SelectionType selectionType, Rectangle selectionRect)
        {
            this.CheckSelectionMode(ref this.selectionType);
            bool changed = false;
            this.UpdateSelRect(ref this.selectionType, selectionType, ref this.selectionRect, selectionRect, ref changed);
            if (changed)
            {
                this.OnSelectionChanged();
            }
            if (this.IsEmpty)
            {
                this.selectionType = QWhale.Editor.SelectionType.None;
            }
        }

        public virtual void SetSelection(QWhale.Editor.SelectionType selectionType, Point selectionStart, Point selectionEnd)
        {
            this.SetSelection(selectionType, new Rectangle(selectionStart.X, selectionStart.Y, selectionEnd.X - selectionStart.X, selectionEnd.Y - selectionStart.Y));
        }

        protected bool ShouldDelete()
        {
            return ((((this.options & SelectionOptions.OverwriteBlocks) != SelectionOptions.None) && ((this.options & SelectionOptions.PersistentBlocks) == SelectionOptions.None)) && !this.IsEmpty);
        }

        protected bool ShouldDeleteBlock()
        {
            return (((this.options & SelectionOptions.PersistentBlocks) == SelectionOptions.None) && !this.IsEmpty);
        }

        public bool ShouldSerializeAllowedSelectionMode()
        {
            return (this.AllowedSelectionMode != EditConsts.DefaultSelectionMode);
        }

        public bool ShouldSerializeBackColor()
        {
            return (this.backColor != EditConsts.DefaultHighlightBackColor);
        }

        public bool ShouldSerializeBorderColor()
        {
            return (this.borderColor != EditConsts.DefaultSelectionBorderColor);
        }

        public bool ShouldSerializeForeColor()
        {
            return (this.foreColor != EditConsts.DefaultHighlightForeColor);
        }

        public bool ShouldSerializeInActiveBackColor()
        {
            return (this.inActiveBackColor != EditConsts.DefaultInactiveHighlightBackColor);
        }

        public bool ShouldSerializeInActiveForeColor()
        {
            return (this.inActiveForeColor != EditConsts.DefaultInactiveHighlightForeColor);
        }

        public bool ShouldSerializeOptions()
        {
            return (this.options != EditConsts.DefaultSelectionOptions);
        }

        public virtual void SmartFormat()
        {
            this.SmartFormat(null, true, false, this.owner.Position.Y);
        }

        public virtual bool SmartFormat(char ch)
        {
            if ((this.Options & SelectionOptions.SmartFormat) != SelectionOptions.None)
            {
                ITextSource source = this.owner.Source;
                if (source.NeedFormatText())
                {
                    ISyntaxParser lexer = source.Lexer as ISyntaxParser;
                    int index = Array.IndexOf<char>(lexer.SmartFormatChars, ch);
                    if (index >= 0)
                    {
                        this.SmartFormatBlock(index == 0);
                        return true;
                    }
                }
            }
            return false;
        }

        public virtual void SmartFormat(int line)
        {
            this.SmartFormat(null, false, false, line);
        }

        protected void SmartFormat(ISyntaxNode node, bool needFormat, bool needIndent, int line)
        {
            if (this.owner.Source.NeedFormatText())
            {
                this.BeginUpdate();
                try
                {
                    bool isEmpty = this.IsEmpty;
                    if (isEmpty)
                    {
                        this.SetSelection(QWhale.Editor.SelectionType.Stream, new Rectangle(0, line, this.owner.Lines[line].Length, 0));
                    }
                    if (!this.IsEmpty)
                    {
                        int num = 0;
                        int num2 = 0;
                        int num3 = 0;
                        ITextSource source = this.owner.Source;
                        source.BeginUpdate(UpdateReason.InsertBlock);
                        source.State |= NotifyState.SmartFormat;
                        QWhale.Editor.SelectionType none = QWhale.Editor.SelectionType.None;
                        if (!isEmpty)
                        {
                            this.StoreSelection(source, out none, out num, out num2, out num3);
                        }
                        else
                        {
                            num = source.StorePosition(source.Position);
                        }
                        try
                        {
                            if (needFormat)
                            {
                                source.FormatText();
                            }
                            ISyntaxParser lexer = (ISyntaxParser) this.owner.Source.Lexer;
                            ITextStrings lines = this.owner.Lines;
                            ITextUndoList operations = new TextUndoList();
                            for (int i = this.selectionRect.Top; i <= this.selectionRect.Bottom; i++)
                            {
                                IStringItem item = lines.GetItem(i);
                                if (item != null)
                                {
                                    string text = item.String;
                                    if ((text.TrimStart(new char[0]) != string.Empty) || needIndent)
                                    {
                                        operations.Clear();
                                        int indent = lexer.SmartFormatLine(i, text, item.TextData, operations);
                                        if ((indent >= 0) || (operations.Count > 0))
                                        {
                                            this.UndoOperations(i, operations);
                                            text = item.String;
                                            int length = text.Length - text.TrimStart(new char[0]).Length;
                                            string str2 = (indent >= 0) ? lines.GetIndentString(this.GetTabIndent(indent), 0) : string.Empty;
                                            if (str2 != text.Substring(0, length))
                                            {
                                                if (length >= 0)
                                                {
                                                    source.MoveTo(0, i);
                                                    source.DeleteRight(length);
                                                }
                                                source.Insert(str2);
                                            }
                                        }
                                    }
                                }
                            }
                            source.SetLastParsed(Math.Min(source.FirstChanged, this.selectionRect.Top));
                        }
                        finally
                        {
                            if (!isEmpty)
                            {
                                this.RestoreSelection(source, none, num, num2, num3);
                            }
                            else
                            {
                                this.Clear();
                                source.Position = source.RestorePosition(num);
                            }
                            source.EndUpdate();
                        }
                    }
                }
                finally
                {
                    this.EndUpdate();
                }
            }
        }

        public virtual void SmartFormatBlock(bool extended)
        {
            if (((this.Options & SelectionOptions.SmartFormat) != SelectionOptions.None) && this.owner.Source.NeedFormatText())
            {
                Point point2;
                ITextSource source = this.owner.Source;
                ISyntaxParser lexer = (ISyntaxParser) source.Lexer;
                Point position = source.Position;
                source.FormatText();
                ISyntaxNode node = lexer.GetAutoFormatNode(position, extended, out point2);
                if ((node != null) && (extended || !node.ContainsErrors()))
                {
                    IRange range = node.Range;
                    Point startPoint = range.StartPoint;
                    this.BeginUpdate();
                    try
                    {
                        this.SetSelection(QWhale.Editor.SelectionType.Stream, range.StartPoint, range.EndPoint);
                        int index = source.StorePosition(source.Position);
                        this.SmartFormat(node, false, false, this.owner.Position.Y);
                        if ((this.owner.Braces.BracesOptions != BracesOptions.None) && !this.owner.Braces.UseRoundRect)
                        {
                            Rectangle[] rects = new Rectangle[] { new Rectangle(this.selectionRect.Left, this.selectionRect.Top, point2.X - this.selectionRect.Left, point2.Y - this.selectionRect.Top) };
                            source.TempHighlightBraces(rects);
                        }
                        source.Position = source.RestorePosition(index);
                        source.HighlightBraces();
                        source.HighlightSyntaxErrors();
                        this.Clear();
                    }
                    finally
                    {
                        this.EndUpdate();
                    }
                }
            }
        }

        public virtual void SmartFormatDocument()
        {
            ITextSource source = this.owner.Source;
            if (source.NeedFormatText())
            {
                this.BeginUpdate();
                source.BeginUpdate(UpdateReason.Other);
                try
                {
                    int num;
                    int num2;
                    int num3;
                    QWhale.Editor.SelectionType type;
                    Point selStart = this.selStart;
                    this.StoreSelection(source, out type, out num, out num2, out num3);
                    try
                    {
                        this.SelectAll();
                        this.SmartFormat();
                    }
                    finally
                    {
                        this.RestoreSelection(source, type, num, num2, num3);
                        this.selStart = selStart;
                    }
                }
                finally
                {
                    source.EndUpdate();
                    this.EndUpdate();
                }
            }
        }

        public virtual void SmartIndent()
        {
            if (((this.Options & SelectionOptions.SmartFormat) != SelectionOptions.None) && this.owner.Source.NeedFormatText())
            {
                Point point;
                ITextSource source = this.owner.Source;
                ISyntaxParser lexer = (ISyntaxParser) source.Lexer;
                ISyntaxNode node = lexer.GetAutoFormatNode(source.Position, true, out point);
                if (node != null)
                {
                    IRange range = node.Range;
                    lexer.ReparseBlock(point);
                    this.BeginUpdate();
                    source.BeginUpdate(UpdateReason.Insert);
                    try
                    {
                        int index = source.StorePosition(source.Position);
                        this.Clear();
                        this.SmartFormat(node, false, true, this.owner.Position.Y);
                        this.Clear();
                        source.State |= NotifyState.Edit;
                        source.Position = source.RestorePosition(index);
                    }
                    finally
                    {
                        source.EndUpdate();
                        this.EndUpdate();
                    }
                }
            }
        }

        public virtual bool SmartIndent(char ch)
        {
            ITextSource source = this.owner.Source;
            if (source.NeedFormatText())
            {
                ISyntaxParser lexer = source.Lexer as ISyntaxParser;
                if (Array.IndexOf<char>(lexer.AutoIndentChars, ch) >= 0)
                {
                    this.SmartIndent();
                    return true;
                }
            }
            return false;
        }

        public virtual void StartSelection()
        {
            this.selTimer.Start();
        }

        protected void StoreSelection(ITextSource source, out QWhale.Editor.SelectionType selType, out int index1, out int index2, out int index3)
        {
            selType = this.SelectionType;
            if (!this.IsEmpty)
            {
                index1 = source.StorePosition(this.selectionRect.Location);
                index2 = source.StorePosition(this.selectionRect.Location + this.selectionRect.Size);
            }
            else
            {
                index1 = -1;
                index2 = -1;
            }
            index3 = source.StorePosition(source.Position);
        }

        public virtual void SwapAnchor()
        {
            if (!this.IsEmpty)
            {
                this.BeginUpdate();
                try
                {
                    if (this.owner.Position == (this.selectionRect.Location + this.selectionRect.Size))
                    {
                        this.owner.Position = this.selectionRect.Location;
                    }
                    else
                    {
                        this.owner.Position = this.selectionRect.Location + this.selectionRect.Size;
                    }
                }
                finally
                {
                    this.EndUpdate();
                }
            }
        }

        private void SwapMaxInt(ref int left, ref int right)
        {
            if (right < left)
            {
                int num = left;
                left = right;
                right = num;
            }
        }

        private void SwapRect(ref Rectangle rect, bool swapMax)
        {
            Point location = rect.Location;
            Point point2 = rect.Location + rect.Size;
            rect.Location = point2;
            rect.Width = location.X - rect.X;
            rect.Height = location.Y - rect.Y;
        }

        public virtual void Tabify()
        {
            this.ChangeBlock(this.tabifyLineEvent);
        }

        private void TabifyLine(string str, ITextUndoList operations)
        {
            int start = 0;
            ITextStrings lines = this.owner.Lines;
            int length = str.Length;
            while (start < length)
            {
                if (str[start] == ' ')
                {
                    int num3 = start;
                    while (((num3 + 1) < str.Length) && (str[num3 + 1] == ' '))
                    {
                        num3++;
                    }
                    operations.Add(new TextUndo(start, (num3 - start) + 1, lines.GetIndentString((num3 - start) + 1, this.owner.Lines.TabPosToPos(str, start), false)));
                    start = num3;
                }
                start++;
            }
        }

        public virtual Point TextToSelectionPoint(Point position)
        {
            switch (this.selectionType)
            {
                case QWhale.Editor.SelectionType.Stream:
                    if (position.Y != this.selectionRect.Top)
                    {
                        return new Point(position.X, position.Y - this.selectionRect.Top);
                    }
                    return new Point(position.X - this.selectionRect.Left, 0);

                case QWhale.Editor.SelectionType.Block:
                    return new Point(position.X - this.selectionRect.Left, position.Y - this.selectionRect.Top);
            }
            return position;
        }

        public virtual void ToggleOutlining()
        {
            IDisplayStrings displayLines = this.owner.DisplayLines;
            IList<IRange> ranges = new List<IRange>();
            if (!this.IsEmpty)
            {
                displayLines.GetOutlineRanges(ranges, this.selectionRect.Location, this.selectionRect.Location + this.selectionRect.Size);
            }
            if (ranges.Count == 0)
            {
                IOutlineRange outlineRange = displayLines.GetOutlineRange(this.owner.Position);
                if (outlineRange != null)
                {
                    ranges.Add(outlineRange);
                }
            }
            if (ranges.Count != 0)
            {
                displayLines.ToggleOutlining(ranges, null);
            }
        }

        public virtual void ToggleOverWrite()
        {
            this.owner.Source.Overwrite = !this.owner.Source.Overwrite;
        }

        private void UncommentLine(string str, ITextUndoList operations)
        {
            if (this.owner.Lexer is ISyntaxParser)
            {
                string singleLineComment = ((ISyntaxParser) this.owner.Lexer).GetSingleLineComment();
                if (singleLineComment != string.Empty)
                {
                    int index = str.IndexOf(singleLineComment);
                    if ((index >= 0) && (index <= (str.Length - str.TrimStart(new char[0]).Length)))
                    {
                        operations.Add(new TextUndo(index, singleLineComment.Length, string.Empty));
                    }
                }
            }
        }

        public virtual void UncommentSelection()
        {
            if (this.owner.Lexer is ISyntaxParser)
            {
                if (this.IsEmpty)
                {
                    this.SelectLine();
                }
                this.SelectionType = QWhale.Editor.SelectionType.Stream;
                this.ChangeBlock(this.uncommentLineEvent, false, true);
            }
        }

        protected int UndoOperations(int line, ITextUndoList operations)
        {
            int num = 0;
            operations.Sort(this.undoComparer);
            ITextSource source = this.owner.Source;
            for (int i = operations.Count - 1; i >= 0; i--)
            {
                ITextUndo undo = operations[i];
                bool flag = undo.Text.Length == undo.Len;
                if (flag)
                {
                    source.DisablePositionUpdate();
                }
                int index = source.StorePosition(source.Position);
                try
                {
                    if (undo.Len > 0)
                    {
                        source.MoveTo(undo.Start, line);
                        source.DeleteRight(undo.Len);
                    }
                    if (undo.Text != string.Empty)
                    {
                        source.MoveTo(undo.Start, line);
                        source.Insert(undo.Text);
                    }
                    num += undo.Text.Length - undo.Len;
                }
                finally
                {
                    source.Position = source.RestorePosition(index);
                    if (flag)
                    {
                        source.EnablePositionUpdate();
                    }
                }
            }
            return num;
        }

        public virtual void UnIndent()
        {
            this.ChangeBlock(this.unIndentLineEvent, true, this.SelectionType != QWhale.Editor.SelectionType.Block);
        }

        private void UnIndentLine(string str, ITextUndoList operations)
        {
            if (str != string.Empty)
            {
                ITextStrings lines = this.owner.Lines;
                int num = lines.UseSpaces ? lines.GetTabStop(0) : 1;
                string str2 = str.Substring(0, Math.Min(num, str.Length));
                num = str2.Length - str2.TrimStart(null).Length;
                if (num > 0)
                {
                    operations.Add(new TextUndo(0, num, string.Empty));
                }
            }
        }

        public virtual void UnTabify()
        {
            this.ChangeBlock(this.unTabifyLineEvent);
        }

        private void UnTabifyLine(string str, ITextUndoList operations)
        {
            this.owner.Lines.GetTabString(str, operations);
        }

        public virtual void UpdateSelection()
        {
            if (!this.IsEmpty && ((((this.options & SelectionOptions.HideSelection) != SelectionOptions.None) || (this.ForeColor != this.InActiveForeColor)) || (this.BackColor != this.InActiveBackColor)))
            {
                this.Invalidate();
            }
        }

        protected void UpdateSelRect(ref QWhale.Editor.SelectionType oldSelectionType, QWhale.Editor.SelectionType newSelectionType, ref Rectangle oldRect, Rectangle newRect, ref bool changed)
        {
            changed = false;
            if ((oldSelectionType != newSelectionType) || (oldRect != newRect))
            {
                bool flag = this.owner.Source.UpdateCount == 0;
                Region region = flag ? this.GetSelectionRegion(oldSelectionType, oldRect) : null;
                if ((SelectionOptions.DisableSelection & this.options) != SelectionOptions.None)
                {
                    newSelectionType = QWhale.Editor.SelectionType.None;
                }
                else if (this.IsSelectionRectEmpty(newSelectionType, newRect))
                {
                    newSelectionType = QWhale.Editor.SelectionType.None;
                }
                if (newSelectionType == QWhale.Editor.SelectionType.None)
                {
                    newRect = Rectangle.Empty;
                    this.atTopLeftEnd = false;
                    this.atBottomRightEnd = false;
                }
                changed = true;
                oldRect = newRect;
                oldSelectionType = newSelectionType;
                if (flag)
                {
                    Region selectionRegion = this.GetSelectionRegion(newSelectionType, newRect);
                    if (region == null)
                    {
                        region = selectionRegion;
                    }
                    else if (selectionRegion != null)
                    {
                        region.Union(selectionRegion);
                        selectionRegion.Dispose();
                    }
                    if (region != null)
                    {
                        this.owner.Invalidate(region, false);
                        region.Dispose();
                    }
                }
            }
        }

        public virtual void UpdateSelStart(bool checkIfEmpty)
        {
            this.UpdateSelStart(this.owner.Position, checkIfEmpty);
        }

        public virtual void UpdateSelStart(Point position)
        {
            this.UpdateSelStart(position, false);
        }

        protected void UpdateSelStart(Point position, bool checkIfEmpty)
        {
            if ((!checkIfEmpty || (this.selectionType == QWhale.Editor.SelectionType.None)) || !this.IsValidSelectionPoint(position))
            {
                this.selStart = position;
            }
        }

        private void UpdateWordSelection(ref Point pt, bool direction)
        {
            if (this.selectionState == QWhale.Editor.SelectionState.SelectWord)
            {
                int num;
                int num2;
                if (this.selectionType == QWhale.Editor.SelectionType.Block)
                {
                    ITextSource source = this.owner.Source;
                    NavigateOptions navigateOptions = source.NavigateOptions;
                    try
                    {
                        source.SetNavigateOptions(navigateOptions | NavigateOptions.BeyondEol);
                        source.ValidatePosition(ref pt);
                    }
                    finally
                    {
                        source.SetNavigateOptions(navigateOptions);
                    }
                }
                else
                {
                    this.owner.Source.ValidatePosition(ref pt);
                }
                if (this.owner.Lines.GetWord(pt.Y, pt.X, out num, out num2))
                {
                    pt.X = direction ? (num2 + 1) : num;
                }
            }
        }

        private Point UpdateWordSelectionEnd(Point pt)
        {
            Point point = pt;
            int left = 0;
            int right = 0;
            if ((((SelectionOptions.WordSelect & this.Options) != SelectionOptions.None) && this.owner.Lines.GetWord(pt.Y, pt.X, out left, out right)) && (((pt.Y != this.selStart.Y) || (this.selStart.X < left)) || (this.selStart.X > right)))
            {
                bool flag = (pt.Y > this.selStart.Y) || ((pt.Y == this.selStart.Y) && (pt.X >= this.selStart.X));
                point.X = flag ? (right + 1) : left;
            }
            return point;
        }

        public virtual void UpperCase()
        {
            this.ChangeBlock(this.upperCaseLineEvent, true, false);
        }

        private void UpperCaseLine(string str, ITextUndoList operations)
        {
            operations.Add(new TextUndo(0, str.Length, str.ToUpper()));
        }

        public virtual void WordTransponse()
        {
            this.owner.Source.BeginUpdate(UpdateReason.Insert);
            try
            {
                Point position = this.owner.Position;
                if (this.SelectCurrentWord() && !this.IsEmpty)
                {
                    Rectangle selectionRect = this.SelectionRect;
                    QWhale.Editor.SelectionType selectionType = this.SelectionType;
                    string selectedText = this.SelectedText;
                    this.SelectNextWord();
                    if (!this.IsEmpty)
                    {
                        Point point1 = this.owner.Position;
                        string s = this.SelectedText;
                        this.Delete();
                        this.InsertString(selectedText);
                        this.owner.MoveTo(selectionRect.Location);
                        this.owner.Selection.SetSelection(selectionType, selectionRect);
                        this.Delete();
                        this.InsertString(s);
                        return;
                    }
                }
                this.owner.Position = position;
            }
            finally
            {
                this.owner.Source.EndUpdate();
            }
        }

        protected void WriteToClipboard()
        {
            DataObject data = new DataObject();
            string selectedText = this.SelectedText;
            if (selectedText != string.Empty)
            {
                data.SetText(selectedText);
                if ((this.options & SelectionOptions.RtfClipboard) != SelectionOptions.None)
                {
                    ITextStrings strings = new TextStrings(null) {
                        Owner = this.owner
                    };
                    strings.SetTextAndData(selectedText, this.owner.Source.NeedParse() ? this.SelectedColorData : null);
                    StringWriter writer = new StringWriter();
                    try
                    {
                        strings.SaveStream(writer, new RtfExport());
                        selectedText = writer.ToString();
                        if (selectedText != string.Empty)
                        {
                            data.SetText(selectedText, TextDataFormat.Rtf);
                        }
                    }
                    finally
                    {
                        writer.Close();
                    }
                }
            }
            else
            {
                data.SetData(selectedText);
            }
            if (this.SelectionType != QWhale.Editor.SelectionType.None)
            {
                data.SetData(DataFormats.Serializable, this.SelectionType);
            }
            try
            {
                Clipboard.SetDataObject(data, true);
            }
            catch
            {
                Clipboard.SetDataObject(data);
            }
        }

        [Editor("QWhale.Design.FlagEnumerationEditor, QWhale.Editor", typeof(UITypeEditor)), Description("Gets or sets type of selection allowed to Edit control content.")]
        public virtual QWhale.Editor.AllowedSelectionMode AllowedSelectionMode
        {
            get
            {
                return this.allowedSelectionMode;
            }
            set
            {
                if (this.allowedSelectionMode != value)
                {
                    this.allowedSelectionMode = value;
                    this.OnAllowedSelectionModeChanged();
                }
            }
        }

        [Description("Gets or sets a background color of the \"SelectedText\" when owner control has input focus.")]
        public virtual Color BackColor
        {
            get
            {
                return this.backColor;
            }
            set
            {
                if (this.backColor != value)
                {
                    this.backColor = value;
                    this.OnBackColorChanged();
                }
            }
        }

        [Description("Gets or sets a color of the selection border.")]
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

        [Description("Gets or sets a foreground color of the \"SelectedText\" when owner control has input focus.")]
        public virtual Color ForeColor
        {
            get
            {
                return this.foreColor;
            }
            set
            {
                if (this.foreColor != value)
                {
                    this.foreColor = value;
                    this.OnForeColorChanged();
                }
            }
        }

        [Description("Gets or sets a background color of the \"SelectedText\" when owner control does not have input focus.")]
        public virtual Color InActiveBackColor
        {
            get
            {
                return this.inActiveBackColor;
            }
            set
            {
                if (this.inActiveBackColor != value)
                {
                    this.inActiveBackColor = value;
                    this.OnInActiveBackColorChanged();
                }
            }
        }

        [Description("Gets or sets a foreground color of the \"SelectedText\" when owner control does not have input focus.")]
        public virtual Color InActiveForeColor
        {
            get
            {
                return this.inActiveForeColor;
            }
            set
            {
                if (this.inActiveForeColor != value)
                {
                    this.inActiveForeColor = value;
                    this.OnInActiveForeColorChanged();
                }
            }
        }

        [Browsable(false)]
        public virtual bool IsEmpty
        {
            get
            {
                return (this.selectionType == QWhale.Editor.SelectionType.None);
            }
        }

        protected EventHandlers KeyHandlers
        {
            get
            {
                return (this.owner.KeyList.Handlers as EventHandlers);
            }
        }

        [Description("Gets or sets options determining behaviour or the \"Selection\"."), Editor("QWhale.Design.FlagEnumerationEditor, QWhale.Editor", typeof(UITypeEditor))]
        public virtual SelectionOptions Options
        {
            get
            {
                return this.options;
            }
            set
            {
                if (this.options != value)
                {
                    this.options = value;
                    this.OnOptionsChanged();
                }
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual string SelectedColorData
        {
            get
            {
                if (this.IsEmpty)
                {
                    return string.Empty;
                }
                StringBuilder builder = new StringBuilder();
                bool flag = true;
                for (int i = 0; i < this.SelectedCount(); i++)
                {
                    string str = this.SelectedData(i);
                    if (str != null)
                    {
                        if (!flag)
                        {
                            builder.Append(new string('\0', this.owner.LineTerminator.Length));
                        }
                        builder.Append(str);
                        flag = false;
                    }
                }
                return builder.ToString();
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual string SelectedText
        {
            get
            {
                if (this.IsEmpty)
                {
                    return string.Empty;
                }
                StringBuilder builder = new StringBuilder();
                bool flag = true;
                for (int i = 0; i < this.SelectedCount(); i++)
                {
                    string str = this.SelectedString(i);
                    if (str != null)
                    {
                        if (!flag)
                        {
                            builder.Append(this.owner.LineTerminator);
                        }
                        builder.Append(str);
                        flag = false;
                    }
                }
                return builder.ToString();
            }
            set
            {
                this.SetSelectedText(value, QWhale.Editor.SelectionType.Stream);
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public virtual int SelectionLength
        {
            get
            {
                return this.SelectedText.Length;
            }
            set
            {
                this.SetSelection(QWhale.Editor.SelectionType.Stream, this.GetSelectionPoint(), this.owner.Source.AbsolutePositionToTextPoint(this.SelectionStart + value));
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual Rectangle SelectionRect
        {
            get
            {
                return this.selectionRect;
            }
            set
            {
                this.SetSelection(this.selectionType, value);
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual int SelectionStart
        {
            get
            {
                return this.owner.Source.TextPointToAbsolutePosition(this.GetSelectionPoint());
            }
            set
            {
                this.selectionType = QWhale.Editor.SelectionType.None;
                this.owner.Source.MoveTo(this.owner.Source.AbsolutePositionToTextPoint(value));
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public virtual QWhale.Editor.SelectionState SelectionState
        {
            get
            {
                return this.selectionState;
            }
            set
            {
                if (this.selectionState != value)
                {
                    this.selectionState = value;
                    this.OnSelectionStateChanged();
                }
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual QWhale.Editor.SelectionType SelectionType
        {
            get
            {
                return this.selectionType;
            }
            set
            {
                this.SetSelection(value, this.selectionRect);
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual ISerializationInfo SerializationInfo
        {
            get
            {
                return new XmlSelectionInfo(this);
            }
            set
            {
                value.FixupReferences(this);
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

        internal class UndoComparer : IComparer<ITextUndo>
        {
            public int Compare(ITextUndo x, ITextUndo y)
            {
                int num = x.Start - y.Start;
                if (num == 0)
                {
                    num = ((x.Text != null) ? 0 : 1) - ((y.Text != null) ? 0 : 1);
                }
                return num;
            }
        }
    }
}

