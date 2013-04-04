namespace QWhale.Editor.TextSource
{
    using QWhale.Common;
    using QWhale.Editor;
    using QWhale.Editor.TextSource.Serialization;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Design;
    using System.Reflection;
    using System.Runtime.InteropServices;

    public class Braces : IEditBraceMatching, IBraceMatching
    {
        private Color backColor = EditConsts.DefaultBracesBackColor;
        private System.Drawing.FontStyle fontStyle = EditConsts.DefaultBracesFontStyle;
        private Color foreColor = EditConsts.DefaultBracesForeColor;
        private ISyntaxEdit owner;
        private bool useRoundRect;

        public Braces(ISyntaxEdit owner)
        {
            this.owner = owner;
        }

        public virtual void Assign(IEditBraceMatching source)
        {
            this.ForeColor = source.ForeColor;
            this.BackColor = source.BackColor;
            this.FontStyle = source.FontStyle;
            this.OpenBraces = source.OpenBraces;
            this.ClosingBraces = source.ClosingBraces;
            this.BracesOptions = source.BracesOptions;
            this.UseRoundRect = source.UseRoundRect;
        }

        public virtual bool FindClosingBrace(ref Point position)
        {
            if (this.owner == null)
            {
                return false;
            }
            return this.owner.Source.FindClosingBrace(ref position);
        }

        public virtual bool FindClosingBrace(ref int x, ref int y)
        {
            if (this.owner == null)
            {
                return false;
            }
            return this.owner.Source.FindClosingBrace(ref x, ref y);
        }

        public virtual bool FindOpenBrace(ref Point position)
        {
            if (this.owner == null)
            {
                return false;
            }
            return this.owner.Source.FindOpenBrace(ref position);
        }

        public virtual bool FindOpenBrace(ref int x, ref int y)
        {
            if (this.owner == null)
            {
                return false;
            }
            return this.owner.Source.FindOpenBrace(ref x, ref y);
        }

        public virtual void HighlightBraces()
        {
            if (this.owner != null)
            {
                this.owner.Source.HighlightBraces();
            }
        }

        protected virtual void OnBackColorChagned()
        {
            this.Update();
        }

        protected virtual void OnFontStyleChagned()
        {
            this.Update();
        }

        protected virtual void OnForeColorChagned()
        {
            this.Update();
        }

        protected virtual void OnUseRoundRectChanged()
        {
            this.Update();
        }

        public virtual void ResetBackColor()
        {
            this.BackColor = EditConsts.DefaultBracesBackColor;
        }

        public virtual void ResetBracesOptions()
        {
            this.BracesOptions = QWhale.Editor.TextSource.BracesOptions.None;
        }

        public virtual void ResetClosingBraces()
        {
            this.ClosingBraces = EditConsts.DefaultClosingBraces;
        }

        public virtual void ResetFontStyle()
        {
            this.FontStyle = EditConsts.DefaultBracesFontStyle;
        }

        public virtual void ResetForeColor()
        {
            this.ForeColor = EditConsts.DefaultBracesForeColor;
        }

        public virtual void ResetOpenBraces()
        {
            this.OpenBraces = EditConsts.DefaultOpenBraces;
        }

        public virtual void ResetUseRoundRect()
        {
            this.UseRoundRect = false;
        }

        public bool ShouldSerializeBackColor()
        {
            return (this.backColor != EditConsts.DefaultBracesBackColor);
        }

        public bool ShouldSerializeClosingBraces()
        {
            return ((this.owner.Source is IInnerTextSource) && (new string(this.ClosingBraces) != new string(EditConsts.DefaultClosingBraces)));
        }

        public bool ShouldSerializeFontStyle()
        {
            return (this.fontStyle != EditConsts.DefaultBracesFontStyle);
        }

        public bool ShouldSerializeForeColor()
        {
            return (this.foreColor != EditConsts.DefaultBracesForeColor);
        }

        public bool ShouldSerializeOpenBraces()
        {
            return ((this.owner.Source is IInnerTextSource) && (new string(this.OpenBraces) != new string(EditConsts.DefaultOpenBraces)));
        }

        public virtual void TempHighlightBraces(Rectangle[] rects)
        {
            if (this.owner != null)
            {
                this.owner.Source.TempHighlightBraces(rects);
            }
        }

        public virtual void TempUnhighlightBraces()
        {
            if (this.owner != null)
            {
                this.owner.Source.TempUnhighlightBraces();
            }
        }

        public virtual void UnhighlightBraces()
        {
            if (this.owner != null)
            {
                this.owner.Source.UnhighlightBraces();
            }
        }

        protected void Update()
        {
            if ((this.BracesOptions != QWhale.Editor.TextSource.BracesOptions.None) && (this.owner != null))
            {
                this.owner.Invalidate();
            }
        }

        [Description("Gets or sets a value that represents background color to draw matching braces.")]
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
                    this.OnBackColorChagned();
                }
            }
        }

        [DefaultValue(0), Editor("QWhale.Design.FlagEnumerationEditor, QWhale.Editor", typeof(UITypeEditor)), Description("Gets or sets options specifying appearance and behaviour of matching braces within Edit control.")]
        public virtual QWhale.Editor.TextSource.BracesOptions BracesOptions
        {
            get
            {
                if (this.owner == null)
                {
                    return QWhale.Editor.TextSource.BracesOptions.None;
                }
                return this.owner.Source.BracesOptions;
            }
            set
            {
                if (this.owner != null)
                {
                    this.owner.Source.BracesOptions = value;
                }
            }
        }

        [Description("Gets or sets an array of characters each one representing a closing brace.")]
        public virtual char[] ClosingBraces
        {
            get
            {
                if (this.owner == null)
                {
                    return null;
                }
                return this.owner.Source.ClosingBraces;
            }
            set
            {
                if (this.owner != null)
                {
                    this.owner.Source.ClosingBraces = value;
                }
            }
        }

        [Description("Gets or sets a FontStyle value that is used to draw matching braces.")]
        public virtual System.Drawing.FontStyle FontStyle
        {
            get
            {
                return this.fontStyle;
            }
            set
            {
                if (this.fontStyle != value)
                {
                    this.fontStyle = value;
                    this.OnFontStyleChagned();
                }
            }
        }

        [Description("Gets or sets a value that represents foreground color to draw matching braces.")]
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
                    this.OnForeColorChagned();
                }
            }
        }

        [Description("Gets or sets an array of characters each one representing an open brace.")]
        public virtual char[] OpenBraces
        {
            get
            {
                if (this.owner == null)
                {
                    return null;
                }
                return this.owner.Source.OpenBraces;
            }
            set
            {
                if (this.owner != null)
                {
                    this.owner.Source.OpenBraces = value;
                }
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ISerializationInfo SerializationInfo
        {
            get
            {
                return new XmlBracesInfo(this);
            }
            set
            {
                value.FixupReferences(this);
            }
        }

        [DefaultValue(false), Description("ets or sets a boolean value that indicates whether Edit control should draw rectangle around matching braces.")]
        public virtual bool UseRoundRect
        {
            get
            {
                return this.useRoundRect;
            }
            set
            {
                if (this.useRoundRect != value)
                {
                    this.useRoundRect = value;
                    this.OnUseRoundRectChanged();
                }
            }
        }

        private class BraceComparer : IComparer<Braces.BraceItem>
        {
            public int Compare(Braces.BraceItem x, Braces.BraceItem y)
            {
                if (y.X <= x.X)
                {
                    return 1;
                }
                return 0;
            }
        }

        internal class BraceItem
        {
            public int BraceIndex;
            public bool Open;
            public int X;

            public BraceItem(int x)
            {
                this.X = x;
            }

            public BraceItem(int x, int braceIndex, bool open)
            {
                this.X = x;
                this.BraceIndex = braceIndex;
                this.Open = open;
            }
        }

        internal class BraceItemList : SortList<Braces.BraceItem>
        {
            private IComparer<Braces.BraceItem> braceComparer = new Braces.BraceComparer();
            private IComparer<Braces.BraceItem> exactBraceComparer = new Braces.ExactBraceComparer();

            public void AddBrace(Braces.BraceItem brace)
            {
                int num;
                base.FindLast(brace, out num, this.exactBraceComparer);
                base.Insert(num, brace);
            }

            public bool FindBrace(int x, out int index)
            {
                return base.FindExact(new Braces.BraceItem(x), out index, this.exactBraceComparer);
            }

            public bool FindLast(int x, out int index)
            {
                return base.FindLast(new Braces.BraceItem(x), out index, this.braceComparer);
            }
        }

        internal class BraceLine
        {
            private QWhale.Editor.TextSource.Braces.BraceItemList braces = new QWhale.Editor.TextSource.Braces.BraceItemList();
            private int line;

            public BraceLine(int line)
            {
                this.line = line;
            }

            public void AddBrace(QWhale.Editor.TextSource.Braces.BraceItem brace)
            {
                this.braces.AddBrace(brace);
            }

            ~BraceLine()
            {
                this.braces.Clear();
            }

            public bool FindBrace(int x, out int index)
            {
                return this.braces.FindBrace(x, out index);
            }

            public bool FindLast(int x, out int index)
            {
                return this.braces.FindLast(x, out index);
            }

            public QWhale.Editor.TextSource.Braces.BraceItemList Braces
            {
                get
                {
                    return this.braces;
                }
            }

            public int Line
            {
                get
                {
                    return this.line;
                }
                set
                {
                    this.line = value;
                }
            }
        }

        internal class BracesList : SortList<Braces.BraceLine>
        {
            private IComparer<Braces.BraceLine> exactComparer = new Braces.ExactComparer();
            private IComparer<Braces.BraceLine> lineComparer = new Braces.LineComparer();

            public void AddBrace(int y, int x, int braceIndex, bool open)
            {
                int num;
                Braces.BraceLine line = new Braces.BraceLine(y);
                if (base.FindLast(line, out num, this.exactComparer))
                {
                    line = this[num];
                }
                else
                {
                    line = new Braces.BraceLine(y);
                    base.Insert(num, line);
                }
                line.AddBrace(new Braces.BraceItem(x, braceIndex, open));
            }

            public bool BlockDeleting(Rectangle rect)
            {
                int num;
                bool flag = false;
                base.FindFirst(new Braces.BraceLine(rect.Bottom), out num, this.exactComparer);
                if (num >= 0)
                {
                    for (int i = Math.Min(num, base.Count - 1); i >= 0; i--)
                    {
                        Braces.BraceLine line = this[i];
                        if ((line.Line >= rect.Top) && (line.Line <= rect.Bottom))
                        {
                            flag = true;
                            base.RemoveAt(i);
                        }
                        else if (line.Line < rect.Top)
                        {
                            return flag;
                        }
                    }
                }
                return flag;
            }

            public bool FindBrace(Point position, out bool open)
            {
                int num;
                open = false;
                if (base.FindLast(new Braces.BraceLine(position.Y), out num, this.exactComparer))
                {
                    Braces.BraceLine line = this[num];
                    if (line.FindBrace(position.X, out num))
                    {
                        open = line.Braces[num].Open;
                        return true;
                    }
                }
                return false;
            }

            public bool FindClosingBrace(ref Point position, ref int braceIndex, int bracesLen)
            {
                int num;
                if (base.FindLast(new Braces.BraceLine(position.Y), out num, this.lineComparer))
                {
                    int[] numArray = new int[bracesLen];
                    for (int i = 0; i < numArray.Length; i++)
                    {
                        numArray[i] = 1;
                    }
                    for (int j = num; j < base.Count; j++)
                    {
                        Braces.BraceLine line = this[j];
                        int num4 = 0;
                        if (line.Line == position.Y)
                        {
                            num4 = line.FindLast(position.X, out num) ? (num + 1) : line.Braces.Count;
                        }
                        for (int k = num4; k < line.Braces.Count; k++)
                        {
                            Braces.BraceItem item = line.Braces[k];
                            if (item.BraceIndex < bracesLen)
                            {
                                if (item.Open)
                                {
                                    numArray[item.BraceIndex]++;
                                }
                                else
                                {
                                    numArray[item.BraceIndex]--;
                                    if ((numArray[item.BraceIndex] == 0) && ((braceIndex < 0) || (braceIndex == item.BraceIndex)))
                                    {
                                        position = new Point(item.X, line.Line);
                                        braceIndex = item.BraceIndex;
                                        return true;
                                    }
                                }
                            }
                        }
                    }
                }
                return false;
            }

            public bool FindOpenBrace(ref Point position, ref int braceIndex, int bracesLen)
            {
                int num;
                if (base.FindLast(new Braces.BraceLine(position.Y), out num, this.lineComparer))
                {
                    int[] numArray = new int[bracesLen];
                    for (int i = 0; i < numArray.Length; i++)
                    {
                        numArray[i] = 1;
                    }
                    for (int j = num; j >= 0; j--)
                    {
                        Braces.BraceLine line = this[j];
                        int num4 = line.Braces.Count - 1;
                        if (line.Line == position.Y)
                        {
                            num4 = line.FindLast(position.X, out num) ? num : -1;
                        }
                        for (int k = num4; k >= 0; k--)
                        {
                            Braces.BraceItem item = line.Braces[k];
                            if (item.BraceIndex < bracesLen)
                            {
                                if (!item.Open)
                                {
                                    numArray[item.BraceIndex]++;
                                }
                                else
                                {
                                    numArray[item.BraceIndex]--;
                                    if ((numArray[item.BraceIndex] == 0) && ((braceIndex < 0) || (braceIndex == item.BraceIndex)))
                                    {
                                        position = new Point(item.X, line.Line);
                                        braceIndex = item.BraceIndex;
                                        return true;
                                    }
                                }
                            }
                        }
                    }
                }
                return false;
            }

            public bool PositionChanged(int x, int y, int deltaX, int deltaY)
            {
                if (deltaY != 0)
                {
                    int num;
                    base.FindFirst(new Braces.BraceLine(y), out num, this.exactComparer);
                    if (num >= 0)
                    {
                        bool flag = false;
                        for (int i = num; i < base.Count; i++)
                        {
                            Braces.BraceLine line = this[i];
                            Point pt = new Point(0, line.Line);
                            if (SortList<Braces.BraceLine>.UpdatePos(x, y, deltaX, deltaY, ref pt, true))
                            {
                                if ((deltaX != 0) && (deltaY != 0))
                                {
                                    foreach (Braces.BraceItem item in line.Braces)
                                    {
                                        Point point2 = new Point(item.X, line.Line);
                                        if (SortList<Braces.BraceLine>.UpdatePos(x, y, deltaX, deltaY, ref point2, true))
                                        {
                                            item.X = point2.X;
                                        }
                                    }
                                }
                                line.Line = pt.Y;
                                flag = true;
                            }
                        }
                        if (flag)
                        {
                            base.Sort(this.exactComparer);
                            return true;
                        }
                    }
                }
                return false;
            }

            public void RemoveBraces(int line)
            {
                int num;
                if (base.FindLast(new Braces.BraceLine(line), out num, this.exactComparer))
                {
                    base.RemoveAt(num);
                }
            }

            public Braces.BraceLine this[int index]
            {
                get
                {
                    return base[index];
                }
            }
        }

        private class ExactBraceComparer : IComparer<Braces.BraceItem>
        {
            public int Compare(Braces.BraceItem x, Braces.BraceItem y)
            {
                return (x.X - y.X);
            }
        }

        private class ExactComparer : IComparer<Braces.BraceLine>
        {
            public int Compare(Braces.BraceLine x, Braces.BraceLine y)
            {
                return (x.Line - y.Line);
            }
        }

        private class LineComparer : IComparer<Braces.BraceLine>
        {
            public int Compare(Braces.BraceLine x, Braces.BraceLine y)
            {
                if (y.Line < x.Line)
                {
                    return 1;
                }
                return 0;
            }
        }
    }
}

