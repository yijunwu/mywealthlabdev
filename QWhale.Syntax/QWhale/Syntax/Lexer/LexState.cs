namespace QWhale.Syntax.Lexer
{
    using QWhale.Common;
    using QWhale.Syntax.Serialization;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Text;
    using System.Text.RegularExpressions;

    public class LexState : ILexState
    {
        private Dictionary<int, ILexSyntaxBlock> blocks;
        private bool caseSensitive;
        private string desc;
        private string expression;
        private string name;
        private System.Text.RegularExpressions.Regex regex;
        private ILexScheme scheme;
        private ILexSyntaxBlocks syntaxBlocks;

        public LexState()
        {
            this.name = string.Empty;
            this.desc = string.Empty;
            this.expression = string.Empty;
            this.syntaxBlocks = new LexSyntaxBlocks(this);
            this.regex = new System.Text.RegularExpressions.Regex(string.Empty);
            this.blocks = new Dictionary<int, ILexSyntaxBlock>();
        }

        public LexState(ILexScheme scheme) : this()
        {
            this.scheme = scheme;
        }

        private void BuildExpression()
        {
            StringBuilder builder = new StringBuilder();
            foreach (LexSyntaxBlock block in this.syntaxBlocks)
            {
                block.CaseSensitive = this.caseSensitive;
                string expression = block.Expression;
                if ((expression != null) && (expression != string.Empty))
                {
                    builder.Append(string.Format("(?<{0}>{1})", "_" + block.Index.ToString(), expression) + "|");
                }
            }
            if (builder.Length > 0)
            {
                builder.Remove(builder.Length - 1, 1);
            }
            this.UpdateExpression(builder.ToString());
        }

        private void InitBlockHash()
        {
            this.blocks.Clear();
            foreach (int num2 in this.regex.GetGroupNumbers())
            {
                string str = this.regex.GroupNameFromNumber(num2);
                if (((str != null) && (str.Length > 0)) && (str[0] == '_'))
                {
                    int num = int.Parse(str.Substring(1));
                    this.blocks.Add(num2, this.syntaxBlocks[num]);
                }
            }
        }

        protected virtual void OnBlocksChanged()
        {
            this.BuildExpression();
        }

        protected virtual void OnCaseSensitiveChanged()
        {
            this.BuildExpression();
        }

        protected virtual void OnDescChanged()
        {
        }

        protected virtual void OnNameChanged()
        {
        }

        protected virtual void OnSchemeChanged()
        {
        }

        public virtual void ResetCaseSensitive()
        {
            this.CaseSensitive = false;
        }

        protected void UpdateExpression(string expr)
        {
            if (this.expression != expr)
            {
                this.expression = expr;
                RegexOptions options = RegexOptions.IgnorePatternWhitespace | RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.ExplicitCapture;
                if (!this.caseSensitive)
                {
                    options |= RegexOptions.IgnoreCase;
                }
                this.regex = new System.Text.RegularExpressions.Regex(expr, options);
                this.InitBlockHash();
            }
        }

        public virtual Dictionary<int, ILexSyntaxBlock> Blocks
        {
            get
            {
                return this.blocks;
            }
        }

        [Description("Gets or sets a value indicating whether the analizer should perform case sensitive parsing for this \"LexState\".")]
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

        [Description("Gets or sets description of the \"ILexState\".")]
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

        [Description("Represents a collective regular expression pattern for the \"LexState\".")]
        public virtual string Expression
        {
            get
            {
                return this.expression;
            }
        }

        [Description("Represents index of the \"LexState\" within the lexical state collection.")]
        public virtual int Index
        {
            get
            {
                if (this.scheme == null)
                {
                    return -1;
                }
                return this.scheme.States.IndexOf(this);
            }
        }

        [Description("Gets or sets name of the \"LexState\".")]
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

        [Description("Represents a regular expression that contains \"Expression\" as a pattern.")]
        public virtual System.Text.RegularExpressions.Regex Regex
        {
            get
            {
                return this.regex;
            }
        }

        [Description("Gets or sets \"ILexScheme\" that owns this \"LexState\".")]
        public virtual ILexScheme Scheme
        {
            get
            {
                return this.scheme;
            }
            set
            {
                if (this.scheme != value)
                {
                    this.scheme = value;
                    this.OnSchemeChanged();
                }
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual ISerializationInfo SerializationInfo
        {
            get
            {
                return new XmlLexStateInfo(this);
            }
            set
            {
                value.FixupReferences(this);
            }
        }

        [Description("Gets or sets a collection of lexical syntax blocks.")]
        public virtual ILexSyntaxBlocks SyntaxBlocks
        {
            get
            {
                return this.syntaxBlocks;
            }
            set
            {
                this.syntaxBlocks.Clear();
                foreach (LexSyntaxBlock block in value)
                {
                    this.syntaxBlocks.Add(block);
                }
                this.OnBlocksChanged();
            }
        }
    }
}

