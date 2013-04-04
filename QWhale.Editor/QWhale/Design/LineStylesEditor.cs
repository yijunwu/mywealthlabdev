namespace QWhale.Design
{
    using QWhale.Editor;
    using System;
    using System.ComponentModel.Design;

    public class LineStylesEditor : CollectionEditor
    {
        public LineStylesEditor(Type type) : base(typeof(EditLineStyles))
        {
        }

        protected override bool CanSelectMultipleInstances()
        {
            return false;
        }

        protected override Type CreateCollectionItemType()
        {
            return typeof(EditLineStyle);
        }
    }
}

