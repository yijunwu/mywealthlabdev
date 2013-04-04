namespace QWhale.Syntax.Lexer
{
    using QWhale.Common;
    using QWhale.Syntax;
    using QWhale.Syntax.Design;
    using QWhale.Syntax.Serialization;
    using System;
    using System.ComponentModel;
    using System.IO;
    using System.Text;
    using System.Xml.Serialization;

    [Serializable, TypeConverter(typeof(LexSchemeConverter))]
    public class LexScheme : ILexScheme, IImport, IExport
    {
        private string author = string.Empty;
        private string copyright = string.Empty;
        private string desc = string.Empty;
        private string fileExtension = string.Empty;
        private string fileType = string.Empty;
        private string name = string.Empty;
        private ILexer owner;
        private ILexStates states;
        private ILexStyles styles;
        private string version = SyntaxConsts.DefaultLexSchemeVersion;

        public LexScheme(ILexer owner)
        {
            this.owner = owner;
            this.states = new LexStates(this);
            this.styles = new LexStyles(this);
        }

        public virtual void Clear()
        {
            this.styles.Clear();
            this.states.Clear();
            this.author = string.Empty;
            this.name = string.Empty;
            this.desc = string.Empty;
            this.copyright = string.Empty;
            this.version = SyntaxConsts.DefaultLexSchemeVersion;
            this.fileExtension = string.Empty;
            this.fileType = string.Empty;
        }

        protected virtual ILexState GetLexState(int index)
        {
            if ((index >= 0) && (index < this.states.Count))
            {
                return this.states[index];
            }
            return null;
        }

        protected virtual ILexStyle GetLexStyle(int index)
        {
            if ((index >= 0) && (index < this.styles.Count))
            {
                return this.styles[index];
            }
            return null;
        }

        protected void Init()
        {
            this.OnStatesChanged();
        }

        public virtual bool IsEmpty()
        {
            return (((((this.styles.Count == 0) && (this.states.Count == 0)) && ((this.author == string.Empty) && (this.name == string.Empty))) && (((this.desc == string.Empty) && (this.copyright == string.Empty)) && (this.fileType == string.Empty))) && (this.fileExtension == string.Empty));
        }

        public virtual bool IsPlainText(int style)
        {
            return (((style >= 0) && (style < this.styles.Count)) && this.styles[style].PlainText);
        }

        public virtual bool LoadFile(string fileName)
        {
            return this.LoadFile(fileName, null);
        }

        public virtual bool LoadFile(string fileName, Encoding encoding)
        {
            bool flag;
            try
            {
                Stream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                try
                {
                    TextReader reader = (encoding != null) ? new StreamReader(stream, encoding) : new StreamReader(stream);
                    try
                    {
                        flag = this.LoadStream(reader);
                    }
                    finally
                    {
                        reader.Close();
                    }
                }
                finally
                {
                    stream.Close();
                }
            }
            catch (Exception exception)
            {
                ErrorHandler.Error(exception);
                flag = false;
            }
            return flag;
        }

        public virtual bool LoadStream(Stream stream)
        {
            return this.LoadStream(stream, null);
        }

        public virtual bool LoadStream(TextReader reader)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(XmlLexSchemeInfo));
            try
            {
                this.SerializationInfo = (XmlLexSchemeInfo) serializer.Deserialize(reader);
                if (this.owner != null)
                {
                    this.owner.Update();
                }
                return true;
            }
            catch (Exception exception)
            {
                ErrorHandler.Error(exception);
                return false;
            }
        }

        public virtual bool LoadStream(Stream stream, Encoding encoding)
        {
            bool flag;
            TextReader reader = (encoding != null) ? new StreamReader(stream, encoding) : new StreamReader(stream);
            try
            {
                flag = this.LoadStream(reader);
            }
            finally
            {
                reader.Close();
            }
            return flag;
        }

        protected virtual void OnAuthorChanged()
        {
        }

        protected virtual void OnCopyrightChanged()
        {
        }

        protected virtual void OnDescChanged()
        {
        }

        protected virtual void OnFileExtensionChanged()
        {
        }

        protected virtual void OnFileTypeChanged()
        {
        }

        protected virtual void OnNameChanged()
        {
        }

        protected virtual void OnStatesChanged()
        {
        }

        protected virtual void OnStylesChanged()
        {
        }

        protected virtual void OnVersionChanged()
        {
        }

        public virtual bool SaveFile(string fileName)
        {
            return this.SaveFile(fileName, null);
        }

        public virtual bool SaveFile(string fileName, Encoding encoding)
        {
            bool flag;
            try
            {
                Stream stream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
                try
                {
                    StreamWriter writer = new StreamWriter(stream);
                    try
                    {
                        flag = this.SaveStream(writer);
                    }
                    finally
                    {
                        writer.Close();
                    }
                }
                finally
                {
                    stream.Close();
                }
            }
            catch (Exception exception)
            {
                ErrorHandler.Error(exception);
                flag = false;
            }
            return flag;
        }

        public virtual bool SaveStream(Stream stream)
        {
            return this.SaveStream(stream, null);
        }

        public virtual bool SaveStream(TextWriter writer)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(XmlLexSchemeInfo));
            try
            {
                serializer.Serialize(writer, this.SerializationInfo);
                return true;
            }
            catch (Exception exception)
            {
                writer.Flush();
                ErrorHandler.Error(exception);
                return false;
            }
        }

        public virtual bool SaveStream(Stream stream, Encoding encoding)
        {
            bool flag;
            TextWriter writer = (encoding != null) ? new StreamWriter(stream, encoding) : new StreamWriter(stream);
            try
            {
                flag = this.SaveStream(writer);
            }
            finally
            {
                writer.Close();
            }
            return flag;
        }

        [Description("Gets or sets author of the \"LexScheme\".")]
        public virtual string Author
        {
            get
            {
                return this.author;
            }
            set
            {
                if (this.author != value)
                {
                    this.author = value;
                    this.OnAuthorChanged();
                }
            }
        }

        [Description("Gets or sets copyright of the \"LexScheme\".")]
        public virtual string Copyright
        {
            get
            {
                return this.copyright;
            }
            set
            {
                if (this.copyright != value)
                {
                    this.copyright = value;
                    this.OnCopyrightChanged();
                }
            }
        }

        [Description("Gets or sets description of the \"LexScheme\".")]
        public virtual string Desc
        {
            get
            {
                return this.desc;
            }
            set
            {
                if (this.desc != value)
                {
                    this.desc = value;
                    this.OnDescChanged();
                }
            }
        }

        [Description("Gets or sets string value indicating associated file extension of the \"ILexScheme\".")]
        public virtual string FileExtension
        {
            get
            {
                return this.fileExtension;
            }
            set
            {
                if (this.fileExtension != value)
                {
                    this.fileExtension = value;
                    this.OnFileExtensionChanged();
                }
            }
        }

        [Description("Gets or sets string value indicating associated file type of the \"ILexScheme\".")]
        public virtual string FileType
        {
            get
            {
                return this.fileType;
            }
            set
            {
                if (this.fileType != value)
                {
                    this.fileType = value;
                    this.OnFileTypeChanged();
                }
            }
        }

        [Description("Gets or sets \"LexScheme\" name.")]
        public virtual string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                if (this.name != value)
                {
                    this.name = value;
                    this.OnNameChanged();
                }
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual ISerializationInfo SerializationInfo
        {
            get
            {
                return new XmlLexSchemeInfo(this);
            }
            set
            {
                value.FixupReferences(this);
            }
        }

        [Description("Gets or sets collection of lexical states.")]
        public virtual ILexStates States
        {
            get
            {
                return this.states;
            }
            set
            {
                this.states.Clear();
                foreach (ILexState state in value)
                {
                    this.states.Add(state);
                }
                this.OnStatesChanged();
            }
        }

        [Description("Gets or sets collection of lexical styles.")]
        public virtual ILexStyles Styles
        {
            get
            {
                return this.styles;
            }
            set
            {
                this.styles.Clear();
                foreach (ILexStyle style in value)
                {
                    this.styles.Add(style);
                }
                this.OnStylesChanged();
            }
        }

        [Description("Gets or sets version of the \"LexScheme\".")]
        public virtual string Version
        {
            get
            {
                return this.version;
            }
            set
            {
                if (this.version != value)
                {
                    this.version = value;
                    this.OnVersionChanged();
                }
            }
        }
    }
}

