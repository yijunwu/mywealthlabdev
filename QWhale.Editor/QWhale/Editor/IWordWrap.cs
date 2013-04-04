namespace QWhale.Editor
{
    using System;

    public interface IWordWrap
    {
        int GetWrapMargin();
        void ResetWordWrap();
        void ResetWrapAtMargin();
        bool UpdateWordWrap();
        bool UpdateWordWrap(int first, int last);

        bool WordWrap { get; set; }

        bool WrapAtMargin { get; set; }

        int WrapMargin { get; }
    }
}

