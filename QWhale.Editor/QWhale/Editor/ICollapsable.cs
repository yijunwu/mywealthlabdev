namespace QWhale.Editor
{
    using QWhale.Syntax;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using QWhale.Common;

    public interface ICollapsable
    {
        void Collapse(int index);
        void CollapseToDefinitions();
        void EnsureExpanded(Point position);
        void EnsureExpanded(int index);
        void Expand(int index);
        void FullCollapse();
        void FullCollapse(IList<IRange> ranges);
        void FullExpand();
        void FullExpand(IList<IRange> ranges);
        string GetOutlineHint(IOutlineRange range);
        IOutlineRange GetOutlineRange(Point position);
        IOutlineRange GetOutlineRange(int index);
        int GetOutlineRanges(IList<IRange> ranges);
        int GetOutlineRanges(IList<IRange> ranges, Point position);
        int GetOutlineRanges(IList<IRange> ranges, int index);
        int GetOutlineRanges(IList<IRange> ranges, Point startPoint, Point endPoint);
        bool IsCollapsed(int index);
        bool IsExpanded(int index);
        bool IsVisible(Point position);
        bool IsVisible(int index);
        IOutlineRange Outline(Point startPoint, Point endPoint);
        IOutlineRange Outline(int first, int last);
        IOutlineRange Outline(Point startPoint, Point endPoint, int level);
        IOutlineRange Outline(Point startPoint, Point endPoint, string outlineText);
        IOutlineRange Outline(int first, int last, int level);
        IOutlineRange Outline(int first, int last, string outlineText);
        IOutlineRange Outline(Point startPoint, Point endPoint, int level, string outlineText);
        IOutlineRange Outline(int first, int last, int level, string outlineText);
        void ResetAllowOutlining();
        void ResetOutlineOptions();
        void SetOutlineRanges(IList<IRange> ranges);
        void SetOutlineRanges(IList<IRange> ranges, bool preserveVisible);
        void ToggleOutlining();
        void ToggleOutlining(IList<IRange> ranges, IOutlineRange range);
        void UnOutline();
        void UnOutline(Point position);
        void UnOutline(int index);

        bool AllowOutlining { get; set; }

        int CollapsedCount { get; }

        QWhale.Editor.OutlineOptions OutlineOptions { get; set; }
    }
}

