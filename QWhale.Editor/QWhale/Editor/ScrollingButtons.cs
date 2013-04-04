namespace QWhale.Editor
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    [Serializable]
    public class ScrollingButtons : List<IScrollingButton>, IScrollingButtons, IList<IScrollingButton>, ICollection<IScrollingButton>, IEnumerable<IScrollingButton>, IEnumerable
    {
        private ISyntaxEdit owner;
        private IScrolling scrolling;

        public ScrollingButtons()
        {
        }

        public ScrollingButtons(IScrolling scrolling, ISyntaxEdit owner)
        {
            this.scrolling = scrolling;
            this.owner = owner;
        }

        public virtual void Add(IScrollingButton item)
        {
            base.Add(item);
            item.Scrolling = this.scrolling;
            if (this.owner != null)
            {
                this.owner.Update();
            }
        }

        public virtual int AddScrollingButton()
        {
            this.Add(new ScrollingButton());
            return (base.Count - 1);
        }

        public virtual int AddScrollingButton(string name, string description, int imageIndex)
        {
            IScrollingButton item = new ScrollingButton {
                Name = name,
                Description = description,
                ImageIndex = imageIndex
            };
            this.Add(item);
            return (base.Count - 1);
        }

        public virtual void Assign(IScrollingButtons source)
        {
            if (this.scrolling != null)
            {
                this.scrolling.BeginUpdate();
            }
            try
            {
                this.Clear();
                foreach (IScrollingButton button in source)
                {
                    IScrollingButton item = new ScrollingButton();
                    item.Assign(button);
                    this.Add(item);
                }
            }
            finally
            {
                if (this.scrolling != null)
                {
                    this.scrolling.EndUpdate();
                }
            }
        }

        public virtual void Clear()
        {
            base.Clear();
            if (this.owner != null)
            {
                this.owner.Update();
            }
        }
    }
}

