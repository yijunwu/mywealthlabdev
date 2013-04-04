namespace QWhale.Design
{
    using QWhale.Editor;
    using System;
    using System.ComponentModel.Design;

    public class ScrollingButtonsEditor : CollectionEditor
    {
        public ScrollingButtonsEditor(Type type) : base(typeof(ScrollingButtons))
        {
        }

        protected override bool CanSelectMultipleInstances()
        {
            return false;
        }

        protected override Type CreateCollectionItemType()
        {
            return typeof(ScrollingButton);
        }
    }
}

