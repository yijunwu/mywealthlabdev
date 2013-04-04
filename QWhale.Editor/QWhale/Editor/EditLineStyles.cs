namespace QWhale.Editor
{
    using QWhale.Common;
    using QWhale.Editor.Serialization;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;

    [Serializable]
    public class EditLineStyles : List<IEditLineStyle>, IEditLineStyles, IList<IEditLineStyle>, ICollection<IEditLineStyle>, IEnumerable<IEditLineStyle>, IEnumerable
    {
        private ISyntaxEdit owner;

        public EditLineStyles()
        {
        }

        public EditLineStyles(ISyntaxEdit owner)
        {
            this.owner = owner;
        }

        public virtual int AddLineStyle()
        {
            base.Add(new EditLineStyle(this.owner));
            return (base.Count - 1);
        }

        public virtual int AddLineStyle(string name, Color foreColor, Color backColor, Color penColor, int imageIndex, LineStyleOptions options)
        {
            base.Add(new EditLineStyle(this.owner, name, foreColor, backColor, penColor, imageIndex, options));
            return (base.Count - 1);
        }

        public virtual void Assign(IEditLineStyles source)
        {
            base.Clear();
            foreach (IEditLineStyle style in source)
            {
                this.AddLineStyle(style.Name, style.ForeColor, style.BackColor, style.PenColor, style.ImageIndex, style.Options);
            }
        }

        public virtual int IndexOfName(string name)
        {
            for (int i = 0; i < base.Count; i++)
            {
                if (base[i].Name == name)
                {
                    return i;
                }
            }
            return -1;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public virtual ISerializationInfo SerializationInfo
        {
            get
            {
                return new XmlEditLineStylesInfo(this);
            }
            set
            {
                value.FixupReferences(this);
            }
        }
    }
}

