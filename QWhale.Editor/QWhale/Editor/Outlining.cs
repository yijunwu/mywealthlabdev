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

    public class Outlining : IOutlining, ICollapsable
    {
        private IDisplayStrings displayLines;
        private Color outlineColor = EditConsts.DefaultOutlineForeColor;
        private ISyntaxEdit owner;
        private bool useRoundRect = true;

        public Outlining(ISyntaxEdit owner)
        {
            this.owner = owner;
            if (owner != null)
            {
                this.displayLines = owner.DisplayLines;
            }
        }

        public virtual void Assign(IOutlining source)
        {
            this.AllowOutlining = source.AllowOutlining;
            this.OutlineOptions = source.OutlineOptions;
            this.OutlineColor = source.OutlineColor;
        }

        public virtual int BeginUpdate()
        {
            if (this.displayLines == null)
            {
                return 0;
            }
            return this.displayLines.BeginUpdate();
        }

        public virtual void Collapse(int index)
        {
            if (this.displayLines != null)
            {
                this.displayLines.Collapse(index);
            }
        }

        public virtual void CollapseToDefinitions()
        {
            if (this.displayLines != null)
            {
                this.displayLines.CollapseToDefinitions();
            }
        }

        public virtual int EndUpdate()
        {
            if (this.displayLines == null)
            {
                return 0;
            }
            return this.displayLines.EndUpdate();
        }

        public virtual void EnsureExpanded(Point position)
        {
            if (this.displayLines != null)
            {
                this.displayLines.EnsureExpanded(position);
            }
        }

        public virtual void EnsureExpanded(int index)
        {
            if (this.displayLines != null)
            {
                this.displayLines.EnsureExpanded(index);
            }
        }

        public virtual void Expand(int index)
        {
            if (this.displayLines != null)
            {
                this.displayLines.Expand(index);
            }
        }

        public virtual void FullCollapse()
        {
            if (this.displayLines != null)
            {
                this.displayLines.FullCollapse();
            }
        }

        public virtual void FullCollapse(IList<IRange> ranges)
        {
            if (this.displayLines != null)
            {
                this.displayLines.FullCollapse(ranges);
            }
        }

        public virtual void FullExpand()
        {
            if (this.displayLines != null)
            {
                this.displayLines.FullExpand();
            }
        }

        public virtual void FullExpand(IList<IRange> ranges)
        {
            if (this.displayLines != null)
            {
                this.displayLines.FullExpand(ranges);
            }
        }

        public virtual string GetOutlineHint(IOutlineRange range)
        {
            if (this.displayLines == null)
            {
                return string.Empty;
            }
            return this.displayLines.GetOutlineHint(range);
        }

        public virtual IOutlineRange GetOutlineRange(Point position)
        {
            if (this.displayLines == null)
            {
                return null;
            }
            return this.displayLines.GetOutlineRange(position);
        }

        public virtual IOutlineRange GetOutlineRange(int index)
        {
            if (this.displayLines == null)
            {
                return null;
            }
            return this.displayLines.GetOutlineRange(index);
        }

        public virtual int GetOutlineRanges(IList<IRange> ranges)
        {
            if (this.displayLines == null)
            {
                return 0;
            }
            return this.displayLines.GetOutlineRanges(ranges);
        }

        public virtual int GetOutlineRanges(IList<IRange> ranges, Point position)
        {
            if (this.displayLines == null)
            {
                return 0;
            }
            return this.displayLines.GetOutlineRanges(ranges, position);
        }

        public virtual int GetOutlineRanges(IList<IRange> ranges, int index)
        {
            if (this.displayLines == null)
            {
                return 0;
            }
            return this.displayLines.GetOutlineRanges(ranges, index);
        }

        public virtual int GetOutlineRanges(IList<IRange> ranges, Point startPoint, Point endPoint)
        {
            if (this.displayLines == null)
            {
                return 0;
            }
            return this.displayLines.GetOutlineRanges(ranges, startPoint, endPoint);
        }

        public virtual bool IsCollapsed(int index)
        {
            if (this.displayLines == null)
            {
                return false;
            }
            return this.displayLines.IsCollapsed(index);
        }

        public virtual bool IsExpanded(int index)
        {
            if (this.displayLines == null)
            {
                return false;
            }
            return this.displayLines.IsExpanded(index);
        }

        public virtual bool IsVisible(Point position)
        {
            return ((this.displayLines == null) || this.displayLines.IsVisible(position));
        }

        public virtual bool IsVisible(int index)
        {
            return ((this.displayLines == null) || this.displayLines.IsVisible(index));
        }

        protected virtual void OnOutlineColorChanged()
        {
            if ((this.owner != null) && this.AllowOutlining)
            {
                this.owner.Invalidate();
            }
        }

        protected virtual void OnUseRoundRectChanged()
        {
            if ((this.owner != null) && this.AllowOutlining)
            {
                this.owner.Invalidate();
            }
        }

        public virtual IOutlineRange Outline(Point startPoint, Point endPoint)
        {
            if (this.displayLines == null)
            {
                return null;
            }
            return this.displayLines.Outline(startPoint, endPoint);
        }

        public virtual IOutlineRange Outline(int first, int last)
        {
            if (this.displayLines == null)
            {
                return null;
            }
            return this.displayLines.Outline(first, last);
        }

        public virtual IOutlineRange Outline(Point startPoint, Point endPoint, int level)
        {
            if (this.displayLines == null)
            {
                return null;
            }
            return this.displayLines.Outline(startPoint, endPoint, level);
        }

        public virtual IOutlineRange Outline(Point startPoint, Point endPoint, string outlineText)
        {
            if (this.displayLines == null)
            {
                return null;
            }
            return this.displayLines.Outline(startPoint, endPoint, outlineText);
        }

        public virtual IOutlineRange Outline(int first, int last, int level)
        {
            if (this.displayLines == null)
            {
                return null;
            }
            return this.displayLines.Outline(first, last, level);
        }

        public virtual IOutlineRange Outline(int first, int last, string outlineText)
        {
            if (this.displayLines == null)
            {
                return null;
            }
            return this.displayLines.Outline(first, last, outlineText);
        }

        public virtual IOutlineRange Outline(Point startPoint, Point endPoint, int level, string outlineText)
        {
            if (this.displayLines == null)
            {
                return null;
            }
            return this.displayLines.Outline(startPoint, endPoint, level, outlineText);
        }

        public virtual IOutlineRange Outline(int first, int last, int level, string outlineText)
        {
            if (this.displayLines == null)
            {
                return null;
            }
            return this.displayLines.Outline(first, last, level, outlineText);
        }

        public virtual void OutlineText()
        {
            if ((this.AllowOutlining && (this.owner != null)) && this.owner.Source.NeedOutlineText())
            {
                ISyntaxParser lexer = (ISyntaxParser) this.owner.Source.Lexer;
                lexer.Strings = this.owner.Lines;
                lexer.ReparseText();
                IList<IRange> ranges = new List<IRange>();
                lexer.Outline(ranges);
                this.SetOutlineRanges(ranges, true);
            }
        }

        public virtual void ResetAllowOutlining()
        {
            this.AllowOutlining = false;
        }

        public virtual void ResetOutlineColor()
        {
            this.OutlineColor = EditConsts.DefaultOutlineForeColor;
        }

        public virtual void ResetOutlineOptions()
        {
            this.OutlineOptions = EditConsts.DefaultOutlineOptions;
        }

        public virtual void SetOutlineRanges(IList<IRange> ranges)
        {
            if (this.displayLines != null)
            {
                this.displayLines.SetOutlineRanges(ranges);
            }
        }

        public virtual void SetOutlineRanges(IList<IRange> ranges, bool preserveVisible)
        {
            if (this.displayLines != null)
            {
                this.displayLines.SetOutlineRanges(ranges, preserveVisible);
            }
        }

        public bool ShouldSerializeOutlineColor()
        {
            return (this.outlineColor != EditConsts.DefaultOutlineForeColor);
        }

        public bool ShouldSerializeOutlineOptions()
        {
            return (this.OutlineOptions != EditConsts.DefaultOutlineOptions);
        }

        public virtual void ToggleOutlining()
        {
            if (this.displayLines != null)
            {
                this.displayLines.ToggleOutlining();
            }
        }

        public virtual void ToggleOutlining(IList<IRange> ranges, IOutlineRange range)
        {
            if (this.displayLines != null)
            {
                this.displayLines.ToggleOutlining(ranges, range);
            }
        }

        public virtual void UnOutline()
        {
            if (this.displayLines != null)
            {
                this.displayLines.UnOutline();
            }
        }

        public virtual void UnOutline(Point position)
        {
            if (this.displayLines != null)
            {
                this.displayLines.UnOutline(position);
            }
        }

        public virtual void UnOutline(int index)
        {
            if (this.displayLines != null)
            {
                this.displayLines.UnOutline(index);
            }
        }

        public virtual void UnOutlineText()
        {
            if (this.displayLines != null)
            {
                this.displayLines.UnOutline();
            }
        }

        [Description("Gets or sets a value indicating whether outlining enabled."), DefaultValue(false)]
        public virtual bool AllowOutlining
        {
            get
            {
                if (this.displayLines == null)
                {
                    return false;
                }
                return this.displayLines.AllowOutlining;
            }
            set
            {
                if (this.displayLines != null)
                {
                    this.displayLines.AllowOutlining = value;
                }
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual int CollapsedCount
        {
            get
            {
                if (this.displayLines == null)
                {
                    return 0;
                }
                return this.displayLines.CollapsedCount;
            }
        }

        [Description("Gets or sets color that is used to draw outline button.")]
        public virtual Color OutlineColor
        {
            get
            {
                return this.outlineColor;
            }
            set
            {
                if (this.outlineColor != value)
                {
                    this.outlineColor = value;
                    this.OnOutlineColorChanged();
                }
            }
        }

        [Editor("QWhale.Design.FlagEnumerationEditor, QWhale.Editor", typeof(UITypeEditor)), Category("SyntaxEdit"), Description("Gets or sets outlining options.")]
        public virtual QWhale.Editor.OutlineOptions OutlineOptions
        {
            get
            {
                if (this.displayLines == null)
                {
                    return QWhale.Editor.OutlineOptions.None;
                }
                return this.displayLines.OutlineOptions;
            }
            set
            {
                if (this.displayLines != null)
                {
                    this.displayLines.OutlineOptions = value;
                }
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual ISerializationInfo SerializationInfo
        {
            get
            {
                return new XmlOutliningInfo(this);
            }
            set
            {
                value.FixupReferences(this);
            }
        }

        protected virtual int UpdateCount
        {
            get
            {
                if (this.displayLines == null)
                {
                    return 0;
                }
                return this.displayLines.UpdateCount;
            }
        }

        [DefaultValue(true)]
        public bool UseRoundRect
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

        internal class CollapsedList : RangeList
        {
            private int collapsedCount;
            private IComparer<IRange> displayComparer = new DisplayComparer();
            private IComparer<IRange> realComparer = new RealComparer();

            public int Add(IRange value)
            {
                int index = base.Add(value);
                base.TopRange.Add(new SortRange(value, index));
                return index;
            }

            public void GetDisplayPoint(ref Point point)
            {
                Outlining.ICollapsedRange range = base.FindRange(point, this.realComparer) as Outlining.ICollapsedRange;
                if (range != null)
                {
                    if ((point.Y == range.EndPoint.Y) && (point.X != 0x7fffffff))
                    {
                        point.X += range.DeltaX;
                    }
                    if (point.Y != 0x7fffffff)
                    {
                        point.Y += range.DeltaY;
                    }
                }
            }

            public void GetRealPoint(ref Point point, bool rangeStart)
            {
                Outlining.ICollapsedRange range = base.FindRange(point, this.displayComparer) as Outlining.ICollapsedRange;
                if (range != null)
                {
                    int num = range.EndPoint.Y + range.DeltaY;
                    int num2 = range.EndPoint.X + range.DeltaX;
                    if ((point.Y == num) && (point.X != 0x7fffffff))
                    {
                        int num3 = num2;
                        if ((point.X < num3) && rangeStart)
                        {
                            point.X = num3 - range.DisplayText.Length;
                            point.Y = range.StartPoint.Y;
                            return;
                        }
                        if (point.X < num3)
                        {
                            point.X = num3;
                        }
                        point.X += range.EndPoint.X - num3;
                    }
                    if (point.Y != 0x7fffffff)
                    {
                        point.Y += range.EndPoint.Y - num;
                    }
                }
            }

            public override bool UpdatePosition(IRange range, int x, int y, int deltaX, int deltaY)
            {
                int num = range.EndPoint.X - range.StartPoint.X;
                int num2 = range.EndPoint.Y - range.StartPoint.Y;
                if (!base.UpdatePosition(range, x, y, deltaX, deltaY))
                {
                    return false;
                }
                if ((range.EndPoint.X - range.StartPoint.X) == num)
                {
                    return ((range.EndPoint.Y - range.StartPoint.Y) != num2);
                }
                return true;
            }

            public int CollapsedCount
            {
                get
                {
                    return this.collapsedCount;
                }
                set
                {
                    this.collapsedCount = value;
                }
            }

            private class DisplayComparer : IComparer<IRange>
            {
                public int Compare(IRange x, IRange y)
                {
                    Outlining.ICollapsedRange range = ((ISortRange) x).Range as Outlining.ICollapsedRange;
                    int num = range.EndPoint.Y + range.DeltaY;
                    int num2 = (range.EndPoint.X + range.DeltaX) - (range.DisplayText.Length - 1);
                    Point startPoint = y.StartPoint;
                    if ((startPoint.Y >= num) && ((startPoint.Y != num) || (startPoint.X >= num2)))
                    {
                        return 0;
                    }
                    return 1;
                }
            }

            private class RealComparer : IComparer<IRange>
            {
                public int Compare(IRange x, IRange y)
                {
                    Point endPoint = x.EndPoint;
                    Point startPoint = y.StartPoint;
                    if ((startPoint.Y >= endPoint.Y) && ((startPoint.Y != endPoint.Y) || (startPoint.X >= endPoint.X)))
                    {
                        return 0;
                    }
                    return 1;
                }
            }
        }

        internal class CollapsedRange : Outlining.DisplayRange, Outlining.ICollapsedRange, IOutlineRange, IRange, ICloneable
        {
            private int deltaX;
            private int deltaY;

            public CollapsedRange(Outlining.OutlineList owner, Point startPt, Point endPt, int deltaX, int deltaY, int level, string text) : base(owner, startPt, endPt, level, text)
            {
                this.deltaX = deltaX;
                this.deltaY = deltaY;
            }

            public int DeltaX
            {
                get
                {
                    return this.deltaX;
                }
            }

            public int DeltaY
            {
                get
                {
                    return this.deltaY;
                }
            }
        }

        internal class DisplayRange : OutlineRange
        {
            private Outlining.OutlineList owner;

            public DisplayRange()
            {
            }

            public DisplayRange(Outlining.OutlineList owner, Point startPt, Point endPt, int level, string text) : base(startPt, endPt, level, text)
            {
                this.owner = owner;
            }

            public DisplayRange(Outlining.OutlineList owner, Point startPt, Point endPt, int level, string text, bool visible) : base(startPt, endPt, level, text, visible)
            {
                this.owner = owner;
            }

            public override string DisplayText
            {
                get
                {
                    if ((this.owner != null) && ((this.owner.Owner.OutlineOptions & OutlineOptions.DrawButtons) == OutlineOptions.None))
                    {
                        return string.Empty;
                    }
                    return this.Text;
                }
            }

            public override bool Visible
            {
                get
                {
                    return base.Visible;
                }
                set
                {
                    if (base.Visible != value)
                    {
                        base.Visible = value;
                        if (this.owner != null)
                        {
                            this.owner.UpdateRange(this, true);
                        }
                    }
                }
            }
        }

        internal interface ICollapsedRange : IOutlineRange, IRange, ICloneable
        {
            int DeltaX { get; }

            int DeltaY { get; }
        }

        internal class OutlineList : RangeList
        {
            private QWhale.Editor.Outlining.CollapsedList collapsedList;
            private IDisplayStrings owner;

            public OutlineList(IDisplayStrings owner)
            {
                this.owner = owner;
                this.collapsedList = new QWhale.Editor.Outlining.CollapsedList();
            }

            public override bool BlockDeleting(Rectangle rect)
            {
                bool flag = base.BlockDeleting(rect);
                if (flag)
                {
                    this.UpdateRange(null, false);
                }
                return flag;
            }

            public void CheckVisible(ref int index)
            {
                IRange range = this.collapsedList.FindInclusiveRange(index);
                if (range != null)
                {
                    index = range.StartPoint.Y;
                }
            }

            public void Clear()
            {
                base.Clear();
                this.collapsedList.Clear();
                this.UpdateRange(null, true);
            }

            public void CollapseChanged(IOutlineRange range)
            {
                int first = 0;
                int last = 0x7fffffff;
                if (this.owner.WordWrap)
                {
                    if (range != null)
                    {
                        first = range.StartPoint.Y;
                        if (range.StartPoint.Y == range.EndPoint.Y)
                        {
                            last = range.EndPoint.Y;
                        }
                        this.owner.UpdateWordWrap(range.StartPoint.Y, 0x7fffffff);
                    }
                    else
                    {
                        this.owner.UpdateWordWrap(0, 0x7fffffff);
                    }
                }
                this.owner.UpdateNeeded();
                this.owner.Notify(NotifyState.Outline, first, last);
            }

            public int DisplayLineToLine(int index)
            {
                Point point = new Point(0, index);
                this.GetRealPoint(ref point, false);
                return point.Y;
            }

            public IRange FindCollapsedRange(Point point)
            {
                return this.collapsedList.FindExactRange(point);
            }

            public IRange FindCollapsedRange(int index)
            {
                return this.collapsedList.FindExactRange(index);
            }

            public int GetCollapsedRanges(int index, IList<IRange> ranges)
            {
                return this.collapsedList.GetExactRanges(ranges, index);
            }

            public void GetDisplayPoint(ref Point point)
            {
                IRange range = this.collapsedList.FindRange(point);
                if (range != null)
                {
                    point = range.StartPoint;
                }
                this.collapsedList.GetDisplayPoint(ref point);
            }

            public void GetRealPoint(ref Point point, bool rangeStart)
            {
                this.collapsedList.GetRealPoint(ref point, rangeStart);
            }

            public bool IsVisible(Point point)
            {
                return (this.collapsedList.FindRange(point) == null);
            }

            public bool IsVisible(int index)
            {
                return (this.collapsedList.FindInclusiveRange(index) == null);
            }

            public int LineToDisplayLine(int index)
            {
                Point point = new Point(0, index);
                this.GetDisplayPoint(ref point);
                return point.Y;
            }

            public override bool PositionChanged(int x, int y, int deltaX, int deltaY)
            {
                bool flag = base.PositionChanged(x, y, deltaX, deltaY);
                if (flag && this.collapsedList.PositionChanged(x, y, deltaX, deltaY))
                {
                    this.UpdateRange(null, false);
                }
                return flag;
            }

            public override void Update()
            {
                base.Update();
                this.UpdateRange(null, true);
            }

            public void UpdateRange(IOutlineRange rng, bool update)
            {
                if (base.UpdateCount <= 0)
                {
                    Point empty = Point.Empty;
                    int num = 0;
                    int num2 = 0;
                    this.collapsedList.Clear();
                    int count = this.owner.Lines.Count;
                    this.collapsedList.BeginUpdate();
                    try
                    {
                        foreach (IOutlineRange range in this)
                        {
                            if ((!range.Visible && !range.IsEmpty) && this.IsVisible(range.StartPoint))
                            {
                                num2 += Math.Min(range.EndPoint.Y, count - 1) - Math.Min(range.StartPoint.Y, count - 1);
                                if (range.StartPoint.Y != empty.Y)
                                {
                                    num = 0;
                                }
                                num += (range.EndPoint.X - range.StartPoint.X) - range.DisplayText.Length;
                                this.collapsedList.Add(new Outlining.CollapsedRange(null, range.StartPoint, range.EndPoint, -num, -num2, 0, range.DisplayText));
                                empty = range.EndPoint;
                            }
                        }
                    }
                    finally
                    {
                        this.collapsedList.EndUpdate();
                    }
                    this.collapsedList.CollapsedCount = num2;
                    if (update)
                    {
                        this.CollapseChanged(rng);
                    }
                }
            }

            public int CollapsedCount
            {
                get
                {
                    return this.collapsedList.CollapsedCount;
                }
            }

            public IList<IRange> CollapsedList
            {
                get
                {
                    return this.collapsedList;
                }
            }

            public IDisplayStrings Owner
            {
                get
                {
                    return this.owner;
                }
            }
        }
    }
}

