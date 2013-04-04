namespace QWhale.Editor
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public interface ISplitView
    {
        event EventHandler SplitHorz;

        event EventHandler SplitVert;

        event EventHandler UnsplitHorz;

        event EventHandler UnsplitVert;

        bool CanSplitHorz();
        bool CanSplitVert();
        bool CanUnsplitHorz();
        bool CanUnsplitVert();
        void SplitViewHorz();
        void SplitViewVert();
        void UnsplitViewHorz();
        void UnsplitViewVert();

        ISyntaxEdit HorzSplitEdit { get; }

        Splitter HorzSplitter { get; }

        ISyntaxEdit VertSplitEdit { get; }

        Splitter VertSplitter { get; }
    }
}

