namespace QWhale.Editor
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public interface IScrollingButtons : IList<IScrollingButton>, ICollection<IScrollingButton>, IEnumerable<IScrollingButton>, IEnumerable
    {
        void Add(IScrollingButton button);
        int AddScrollingButton();
        int AddScrollingButton(string name, string description, int imageIndex);
        void Assign(IScrollingButtons source);
    }
}

