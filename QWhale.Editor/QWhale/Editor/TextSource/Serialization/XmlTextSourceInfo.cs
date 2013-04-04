namespace QWhale.Editor.TextSource.Serialization
{
    using QWhale.Common;
    using QWhale.Editor;
    using QWhale.Editor.TextSource;
    using QWhale.Syntax.Serialization;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Xml.Serialization;

    public class XmlTextSourceInfo : ISerializationInfo
    {
        private XmlBookmarkInfo[] bookMarks;
        private QWhale.Editor.TextSource.BracesOptions bracesOptions;
        private bool checkSpelling;
        private char[] closingBraces;
        private string fileName;
        private bool highlightHyperText;
        private QWhale.Editor.TextSource.IndentOptions indentOptions;
        private XmlLexerInfo lexer;
        private XmlLineStyleInfo[] lineStyles;
        private int maxLength;
        private QWhale.Editor.TextSource.NavigateOptions navigateOptions;
        private char[] openBraces;
        private bool overWrite;
        private ITextSource owner;
        private Point position;
        private bool readOnly;
        private bool singleLineMode;
        private XmlTextStringsInfo textStrings;
        private int undoLimit;
        private QWhale.Editor.TextSource.UndoOptions undoOptions;

        public XmlTextSourceInfo()
        {
            this.openBraces = new char[0];
            this.closingBraces = new char[0];
            this.navigateOptions = EditConsts.DefaultNavigateOptions;
            this.indentOptions = EditConsts.DefaultIndentOptions;
            this.undoOptions = EditConsts.DefaultUndoOptions;
            this.bookMarks = new XmlBookmarkInfo[0];
            this.lineStyles = new XmlLineStyleInfo[0];
        }

        public XmlTextSourceInfo(ITextSource owner) : this()
        {
            this.owner = owner;
        }

        public virtual void FixupReferences(object owner)
        {
            this.owner = (ITextSource) owner;
            this.owner.BeginUpdate(UpdateReason.Other);
            try
            {
                this.FileName = this.fileName;
                this.Position = this.position;
                this.NavigateOptions = this.navigateOptions;
                this.ReadOnly = this.readOnly;
                this.OverWrite = this.overWrite;
                this.IndentOptions = this.indentOptions;
                this.UndoOptions = this.undoOptions;
                this.UndoLimit = this.undoLimit;
                this.HighlightHyperText = this.highlightHyperText;
                this.CheckSpelling = this.checkSpelling;
                this.Lexer = this.lexer;
                this.TextStrings = this.textStrings;
                this.BracesOptions = this.bracesOptions;
                this.MaxLength = this.maxLength;
                this.SingleLineMode = this.singleLineMode;
                this.BookMarks = this.bookMarks;
                this.LineStyles = this.lineStyles;
                this.OpenBraces = this.openBraces;
                this.ClosingBraces = this.closingBraces;
            }
            finally
            {
                this.owner.EndUpdate();
            }
        }

        public virtual void Load()
        {
            if (this.owner != null)
            {
                this.fileName = this.FileName;
                this.position = this.Position;
                this.bookMarks = this.BookMarks;
                this.lineStyles = this.LineStyles;
                this.navigateOptions = this.NavigateOptions;
                this.readOnly = this.ReadOnly;
                this.overWrite = this.OverWrite;
                this.indentOptions = this.IndentOptions;
                this.undoOptions = this.UndoOptions;
                this.undoLimit = this.UndoLimit;
                this.highlightHyperText = this.HighlightHyperText;
                this.checkSpelling = this.CheckSpelling;
                this.bracesOptions = this.BracesOptions;
                this.maxLength = this.MaxLength;
                this.singleLineMode = this.SingleLineMode;
                this.lexer = this.Lexer;
                this.textStrings = this.TextStrings;
                this.openBraces = this.OpenBraces;
                this.closingBraces = this.ClosingBraces;
                foreach (XmlBookmarkInfo info in this.bookMarks)
                {
                    info.Load();
                }
                foreach (XmlLineStyleInfo info2 in this.lineStyles)
                {
                    info2.Load();
                }
                if (this.lexer != null)
                {
                    this.lexer.Load();
                }
                if (this.textStrings != null)
                {
                    this.textStrings.Load();
                }
            }
        }

        public bool ShouldSerializeBookMarks()
        {
            return ((this.bookMarks.Length > 0) || ((this.owner != null) && (this.owner.BookMarks.Count > 0)));
        }

        public bool ShouldSerializeClosingBraces()
        {
            return (new string(this.ClosingBraces) != new string(EditConsts.DefaultClosingBraces));
        }

        public bool ShouldSerializeIndentOptions()
        {
            return (this.IndentOptions != EditConsts.DefaultIndentOptions);
        }

        public bool ShouldSerializeLineStyles()
        {
            return ((this.lineStyles.Length > 0) || ((this.owner != null) && (this.owner.LineStyles.Count > 0)));
        }

        public bool ShouldSerializeNavigateOptions()
        {
            return (this.NavigateOptions != EditConsts.DefaultNavigateOptions);
        }

        public bool ShouldSerializeOpenBraces()
        {
            return (new string(this.OpenBraces) != new string(EditConsts.DefaultOpenBraces));
        }

        public bool ShouldSerializePosition()
        {
            return !this.Position.Equals(Point.Empty);
        }

        public bool ShouldSerializeUndoOptions()
        {
            return (this.UndoOptions != EditConsts.DefaultUndoOptions);
        }

        [XmlArrayItem("BookMark"), XmlArray]
        public XmlBookmarkInfo[] BookMarks
        {
            get
            {
                if (this.owner == null)
                {
                    return this.bookMarks;
                }
                XmlBookmarkInfo[] infoArray = new XmlBookmarkInfo[this.owner.BookMarks.Count];
                for (int i = 0; i < this.owner.BookMarks.Count; i++)
                {
                    infoArray[i] = (XmlBookmarkInfo) this.owner.BookMarks[i].SerializationInfo;
                }
                return infoArray;
            }
            set
            {
                this.bookMarks = value;
                if ((this.owner != null) && (value != null))
                {
                    this.owner.BookMarks.Clear();
                    foreach (XmlBookmarkInfo info in value)
                    {
                        if (((info.Name != string.Empty) || (info.Description != string.Empty)) || (info.Url != string.Empty))
                        {
                            this.owner.BookMarks.SetBookMark(new Point(info.Pos, info.Line), info.Index, info.Name, info.Description, info.Url);
                        }
                        else
                        {
                            this.owner.BookMarks.SetBookMark(new Point(info.Pos, info.Line), info.Index);
                        }
                    }
                }
            }
        }

        [DefaultValue(0)]
        public QWhale.Editor.TextSource.BracesOptions BracesOptions
        {
            get
            {
                if (this.owner == null)
                {
                    return this.bracesOptions;
                }
                return this.owner.BracesOptions;
            }
            set
            {
                this.bracesOptions = value;
                if (this.owner != null)
                {
                    this.owner.BracesOptions = value;
                }
            }
        }

        [DefaultValue(false)]
        public bool CheckSpelling
        {
            get
            {
                if (this.owner == null)
                {
                    return this.checkSpelling;
                }
                return this.owner.CheckSpelling;
            }
            set
            {
                this.checkSpelling = value;
                if (this.owner != null)
                {
                    this.owner.CheckSpelling = value;
                }
            }
        }

        public char[] ClosingBraces
        {
            get
            {
                if (this.owner == null)
                {
                    return this.closingBraces;
                }
                return this.owner.ClosingBraces;
            }
            set
            {
                this.closingBraces = value;
                if (this.owner != null)
                {
                    this.owner.ClosingBraces = value;
                }
            }
        }

        [DefaultValue("")]
        public string FileName
        {
            get
            {
                if (this.owner == null)
                {
                    return this.fileName;
                }
                return this.owner.FileName;
            }
            set
            {
                this.fileName = value;
                if (this.owner != null)
                {
                    this.owner.FileName = value;
                }
            }
        }

        [DefaultValue(false)]
        public bool HighlightHyperText
        {
            get
            {
                if (this.owner == null)
                {
                    return this.highlightHyperText;
                }
                return this.owner.HighlightHyperText;
            }
            set
            {
                this.highlightHyperText = value;
                if (this.owner != null)
                {
                    this.owner.HighlightHyperText = value;
                }
            }
        }

        public QWhale.Editor.TextSource.IndentOptions IndentOptions
        {
            get
            {
                if (this.owner == null)
                {
                    return this.indentOptions;
                }
                return this.owner.IndentOptions;
            }
            set
            {
                this.indentOptions = value;
                if (this.owner != null)
                {
                    this.owner.IndentOptions = value;
                }
            }
        }

        public XmlLexerInfo Lexer
        {
            get
            {
                if ((this.owner != null) && (this.owner.Lexer != null))
                {
                    return (XmlLexerInfo) this.owner.Lexer.SerializationInfo;
                }
                return this.lexer;
            }
            set
            {
                this.lexer = value;
                if (((this.owner != null) && (value != null)) && (this.owner.Lexer != null))
                {
                    this.owner.Lexer.SerializationInfo = value;
                }
            }
        }

        [XmlArray, XmlArrayItem("LineStyle")]
        public XmlLineStyleInfo[] LineStyles
        {
            get
            {
                if (this.owner == null)
                {
                    return this.lineStyles;
                }
                XmlLineStyleInfo[] infoArray = new XmlLineStyleInfo[this.owner.LineStyles.Count];
                for (int i = 0; i < this.owner.LineStyles.Count; i++)
                {
                    infoArray[i] = (XmlLineStyleInfo) this.owner.LineStyles[i].SerializationInfo;
                }
                return infoArray;
            }
            set
            {
                this.lineStyles = value;
                if ((this.owner != null) && (value != null))
                {
                    this.owner.LineStyles.Clear();
                    foreach (XmlLineStyleInfo info in value)
                    {
                        this.owner.LineStyles.SetLineStyle(new Point(info.Pos, info.Line), info.Range, info.Priority, info.Index);
                    }
                }
            }
        }

        [DefaultValue(0)]
        public int MaxLength
        {
            get
            {
                if (this.owner == null)
                {
                    return this.maxLength;
                }
                return this.owner.MaxLength;
            }
            set
            {
                this.maxLength = value;
                if (this.owner != null)
                {
                    this.owner.MaxLength = value;
                }
            }
        }

        public QWhale.Editor.TextSource.NavigateOptions NavigateOptions
        {
            get
            {
                if (this.owner == null)
                {
                    return this.navigateOptions;
                }
                return this.owner.NavigateOptions;
            }
            set
            {
                this.navigateOptions = value;
                if (this.owner != null)
                {
                    this.owner.NavigateOptions = value;
                }
            }
        }

        public char[] OpenBraces
        {
            get
            {
                if (this.owner == null)
                {
                    return this.openBraces;
                }
                return this.owner.OpenBraces;
            }
            set
            {
                this.openBraces = value;
                if (this.owner != null)
                {
                    this.owner.OpenBraces = value;
                }
            }
        }

        [DefaultValue(false)]
        public bool OverWrite
        {
            get
            {
                if (this.owner == null)
                {
                    return this.overWrite;
                }
                return this.owner.Overwrite;
            }
            set
            {
                this.overWrite = value;
                if (this.owner != null)
                {
                    this.owner.Overwrite = value;
                }
            }
        }

        public Point Position
        {
            get
            {
                if (this.owner == null)
                {
                    return this.position;
                }
                return this.owner.Position;
            }
            set
            {
                this.position = value;
                if (this.owner != null)
                {
                    this.owner.Position = value;
                }
            }
        }

        [DefaultValue(false)]
        public bool ReadOnly
        {
            get
            {
                if (this.owner == null)
                {
                    return this.readOnly;
                }
                return this.owner.Readonly;
            }
            set
            {
                this.readOnly = value;
                if (this.owner != null)
                {
                    this.owner.Readonly = value;
                }
            }
        }

        [DefaultValue(false)]
        public bool SingleLineMode
        {
            get
            {
                if (this.owner == null)
                {
                    return this.singleLineMode;
                }
                return this.owner.SingleLineMode;
            }
            set
            {
                this.singleLineMode = value;
                if (this.owner != null)
                {
                    this.owner.SingleLineMode = value;
                }
            }
        }

        public XmlTextStringsInfo TextStrings
        {
            get
            {
                if (this.owner == null)
                {
                    return this.textStrings;
                }
                return (XmlTextStringsInfo) this.owner.Lines.SerializationInfo;
            }
            set
            {
                this.textStrings = value;
                if ((this.owner != null) && (value != null))
                {
                    this.owner.Lines.SerializationInfo = value;
                }
            }
        }

        [DefaultValue(0)]
        public int UndoLimit
        {
            get
            {
                if (this.owner == null)
                {
                    return this.undoLimit;
                }
                return this.owner.UndoLimit;
            }
            set
            {
                this.undoLimit = value;
                if (this.owner != null)
                {
                    this.owner.UndoLimit = value;
                }
            }
        }

        public QWhale.Editor.TextSource.UndoOptions UndoOptions
        {
            get
            {
                if (this.owner == null)
                {
                    return this.undoOptions;
                }
                return this.owner.UndoOptions;
            }
            set
            {
                this.undoOptions = value;
                if (this.owner != null)
                {
                    this.owner.UndoOptions = value;
                }
            }
        }
    }
}

