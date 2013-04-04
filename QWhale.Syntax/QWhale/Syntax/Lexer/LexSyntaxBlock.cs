namespace QWhale.Syntax.Lexer
{
    using QWhale.Common;
    using QWhale.Syntax.Serialization;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Text;

    public class LexSyntaxBlock : ILexSyntaxBlock
    {
        private bool caseSensitive;
        private string desc;
        private IList<string> expressions;
        private ILexState leaveState;
        private string name;
        private ILexReswordSets reswordSets;
        private ILexState state;
        private ILexStyle style;

        public LexSyntaxBlock()
        {
            this.name = string.Empty;
            this.desc = string.Empty;
            this.expressions = new List<string>();
            this.reswordSets = new LexReswordSets(this);
        }

        public LexSyntaxBlock(ILexState state) : this()
        {
            this.state = state;
        }

        public virtual int AddExpression(string expression)
        {
            this.expressions.Add(expression);
            return (this.expressions.Count - 1);
        }

        public virtual int FindResword(string resword)
        {
            for (int i = 0; i < this.reswordSets.Count; i++)
            {
                if (this.reswordSets[i].FindResword(resword))
                {
                    return i;
                }
            }
            return -1;
        }

        protected virtual void OnCaseSensitiveChanged()
        {
            foreach (ILexReswordSet set in this.reswordSets)
            {
                set.CaseSensitive = this.caseSensitive;
            }
        }

        protected virtual void OnDescChanged()
        {
        }

        protected virtual void OnExpressionsChanged()
        {
        }

        protected virtual void OnLeaveStateChanged()
        {
        }

        protected virtual void OnNameChanged()
        {
        }

        protected virtual void OnReswordSetsChanged()
        {
        }

        protected virtual void OnStateChanged()
        {
        }

        protected virtual void OnStyleChanged()
        {
        }

        [Description("Gets or sets a value indicating whether \"LexSyntaxBlock\" is case-sensitive or not.")]
        public virtual bool CaseSensitive
        {
            get
            {
                return this.caseSensitive;
            }
            set
            {
                if (this.caseSensitive != value)
                {
                    this.caseSensitive = value;
                    this.OnCaseSensitiveChanged();
                }
            }
        }

        [Description("Gets or sets description of the \"LexSyntaxBlock\".")]
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

        [Description("Represents a collective regular expression pattern for the \"LexSyntaxBlock.Expressions\".")]
        public virtual string Expression
        {
            get
            {
                switch (this.expressions.Count)
                {
                    case 0:
                        return string.Empty;

                    case 1:
                        return this.expressions[0];
                }
                StringBuilder builder = new StringBuilder();
                foreach (string str in this.expressions)
                {
                    builder.Append(string.Format("({0})", str) + "|");
                }
                if (builder.Length > 0)
                {
                    builder.Remove(builder.Length - 1, 1);
                }
                return builder.ToString();
            }
        }

        [Description("Represents a list of regular expression patterns for the \"LexSyntaxBlock.Expressions\".")]
        public IList<string> Expressions
        {
            get
            {
                return this.expressions;
            }
            set
            {
                this.expressions.Clear();
                foreach (string str in value)
                {
                    this.expressions.Add(str);
                }
                this.OnExpressionsChanged();
            }
        }

        [Description("Represents index of the \"LexSyntaxBlock\" within the lexical syntax block collection.")]
        public virtual int Index
        {
            get
            {
                if (this.state == null)
                {
                    return -1;
                }
                return this.state.SyntaxBlocks.IndexOf(this);
            }
        }

        [Description("Gets or sets \"ILexState\" object that specifies lexical resulting state after lexical analyzer locates text, that matches to the \"LexSyntaxBlock.Expression\".")]
        public virtual ILexState LeaveState
        {
            get
            {
                return this.leaveState;
            }
            set
            {
                if (this.leaveState != value)
                {
                    this.leaveState = value;
                    this.OnLeaveStateChanged();
                }
            }
        }

        [Description("Gets or sets name of the \"LexSyntaxBlock\".")]
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

        [Description("Gets or sets collection of the \"ILexReswordSet\" object containing reserwed words.")]
        public virtual ILexReswordSets ReswordSets
        {
            get
            {
                return this.reswordSets;
            }
            set
            {
                this.reswordSets.Clear();
                foreach (ILexReswordSet set in value)
                {
                    this.reswordSets.Add(set);
                }
                this.OnReswordSetsChanged();
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public virtual ISerializationInfo SerializationInfo
        {
            get
            {
                return new XmlLexSyntaxBlockInfo(this);
            }
            set
            {
                value.FixupReferences(this);
            }
        }

        public ILexState State
        {
            get
            {
                return this.state;
            }
            set
            {
                if (this.state != value)
                {
                    this.state = value;
                    this.OnStateChanged();
                }
            }
        }

        [Description("Gets or sets an \"ILexStyle\" object, applicable for text that matches \"LexSyntaxBlock\" expression.")]
        public virtual ILexStyle Style
        {
            get
            {
                return this.style;
            }
            set
            {
                if (this.style != value)
                {
                    this.style = value;
                    this.OnStyleChanged();
                }
            }
        }
    }
}

