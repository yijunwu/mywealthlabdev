namespace QWhale.Syntax.Serialization
{
    using QWhale.Common;
    using QWhale.Syntax.Lexer;
    using System;
    using System.ComponentModel;

    public class XmlLexerInfo : ISerializationInfo
    {
        private int defaultState;
        private ILexer owner;
        private XmlLexSchemeInfo scheme;

        public XmlLexerInfo()
        {
        }

        public XmlLexerInfo(ILexer owner) : this()
        {
            this.owner = owner;
        }

        public virtual void FixupReferences(object owner)
        {
            this.owner = (ILexer) owner;
            this.owner.BeginUpdate();
            try
            {
                this.Scheme = this.scheme;
                this.DefaultState = this.defaultState;
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
                this.scheme = this.Scheme;
                this.defaultState = this.DefaultState;
                if (this.scheme != null)
                {
                    this.scheme.Load();
                }
            }
        }

        [DefaultValue(0)]
        public int DefaultState
        {
            get
            {
                if (this.owner == null)
                {
                    return this.defaultState;
                }
                return this.owner.DefaultState;
            }
            set
            {
                this.defaultState = value;
                if (this.owner != null)
                {
                    this.owner.DefaultState = value;
                }
            }
        }

        public XmlLexSchemeInfo Scheme
        {
            get
            {
                if (this.owner == null)
                {
                    return this.scheme;
                }
                return (XmlLexSchemeInfo) this.owner.Scheme.SerializationInfo;
            }
            set
            {
                this.scheme = value;
                if (this.owner != null)
                {
                    this.owner.Scheme.SerializationInfo = value;
                }
            }
        }
    }
}

