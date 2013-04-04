namespace QWhale.Editor.TextSource
{
    using QWhale.Common;
    using QWhale.Editor.TextSource.Serialization;
    using System;
    using System.ComponentModel;
    using System.Drawing;

    public class BookMark : IBookMark
    {
        private int index;
        private int line;
        private int pos;

        public BookMark() : this(0, 0, 0)
        {
        }

        public BookMark(int line)
        {
            this.line = line;
        }

        public BookMark(int line, int pos, int index)
        {
            this.line = line;
            this.pos = pos;
            this.index = index;
        }

        public virtual void Assign(IBookMark source)
        {
            this.line = source.Line;
            this.pos = source.Pos;
            this.index = source.Index;
        }

        [Description("Gets an integer value that specifies ordinal number of the bookmark.")]
        public virtual int Index
        {
            get
            {
                return this.index;
            }
        }

        [Description("Gets position of the bookmark within the text. 0 corresponds to the first line, 1 to second the line, and so on.")]
        public virtual int Line
        {
            get
            {
                return this.line;
            }
        }

        [Description("Gets position of the bookmark within the text line. 0 corresponds to the first character in a line, 1 to the second character, and so on.")]
        public virtual int Pos
        {
            get
            {
                return this.pos;
            }
        }

        [Description("Gets position of the bookmark within the text (index of character and line). Corresponds to \"Pos\" and \"Line\" properties.")]
        public virtual Point Position
        {
            get
            {
                return new Point(this.pos, this.line);
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual ISerializationInfo SerializationInfo
        {
            get
            {
                return new XmlBookmarkInfo(this);
            }
            set
            {
                value.FixupReferences(this);
            }
        }
    }
}

