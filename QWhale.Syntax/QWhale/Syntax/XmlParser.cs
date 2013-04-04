namespace QWhale.Syntax
{
    using QWhale.Common;
    using QWhale.Syntax.Lexer;
    using System;
    using System.ComponentModel;
    using System.Drawing;

    [ToolboxItem(true), ToolboxBitmap(typeof(XmlParser), "Images.XmlParser.bmp")]
    public class XmlParser : SyntaxParser
    {
        internal const string CDATA = "![CDATA[";
        protected LexerProc lexBodyProc;
        protected LexerProc lexCDATAEndProc;
        protected LexerProc lexCDATAProc;
        protected LexerProc lexCommentEndProc;
        protected LexerProc lexCommentProc;
        protected LexerProc lexEqualProc;
        protected LexerProc lexNameProc;
        protected LexerProc lexParamNumberProc;
        protected LexerProc lexParamProc;
        protected LexerProc lexStringEndProc;
        protected LexerProc lexStringParamProc;
        protected LexerProc lexStringProc;
        protected LexerProc lexSymbolProc;
        protected LexerProc lexTagProc;
        protected LexerProc lexWhitespaceProc;
        protected Point prevPosition;
        protected const int stateCDATA = 4;
        protected const int stateComment = 3;
        protected const int stateNormal = 0;
        protected const int stateParam = 2;
        protected const int stateString = 5;
        protected const int stateTag = 1;

        protected virtual void AddNode(ISyntaxNode node)
        {
            this.SyntaxTree.Current.AddChild(node);
        }

        public override CodeCompletionType GetCompletionType(char ch)
        {
            if (ch == '<')
            {
                return CodeCompletionType.ListMembers;
            }
            return base.GetCompletionType(ch);
        }

        protected override int GetLexerStyle(int token)
        {
            switch (token)
            {
                case 2:
                case 14:
                    return 5;

                case 3:
                case 4:
                case 5:
                case 6:
                case 7:
                case 13:
                    return 7;

                case 8:
                    return 7;

                case 9:
                    return 9;

                case 10:
                    return 2;

                case 11:
                    return 3;
            }
            return 6;
        }

        protected override void InitLanguage()
        {
            this.Scheme.FileType = "xml";
        }

        protected override void InitLexer()
        {
            base.InitLexer();
            this.lexWhitespaceProc = new LexerProc(this.LexWhitespace);
            this.lexSymbolProc = new LexerProc(this.LexSymbol);
            this.lexTagProc = new LexerProc(this.LexTag);
            this.lexEqualProc = new LexerProc(this.LexEqual);
            this.lexStringProc = new LexerProc(this.LexString);
            this.lexNameProc = new LexerProc(this.LexName);
            this.lexBodyProc = new LexerProc(this.LexBody);
            this.lexStringParamProc = new LexerProc(this.LexStringParam);
            this.lexParamProc = new LexerProc(this.LexParam);
            this.lexParamNumberProc = new LexerProc(this.LexParamNumber);
            this.lexCommentProc = new LexerProc(this.LexComment);
            this.lexCommentEndProc = new LexerProc(this.LexCommentEnd);
            this.lexCDATAProc = new LexerProc(this.LexCDATA);
            this.lexCDATAEndProc = new LexerProc(this.LexCDATAEnd);
            this.lexStringEndProc = new LexerProc(this.LexStringEnd);
            base.RegisterLexerProc(0, this.lexBodyProc);
            base.RegisterLexerProc(0, '<', this.lexTagProc);
            base.RegisterLexerProc(1, this.lexWhitespaceProc);
            base.RegisterLexerProc(1, '!', '\x00ff', this.lexSymbolProc);
            base.RegisterLexerProc(1, new char[] { '/', '>', '<' }, this.lexTagProc);
            base.RegisterLexerProc(1, '=', this.lexEqualProc);
            base.RegisterLexerProc(1, new char[] { '"', '\'' }, this.lexStringProc);
            base.RegisterLexerProc(1, 'a', 'z', this.lexNameProc);
            base.RegisterLexerProc(1, 'A', 'Z', this.lexNameProc);
            base.RegisterLexerProc(5, this.lexStringEndProc);
            base.RegisterLexerProc(2, this.lexWhitespaceProc);
            base.RegisterLexerProc(2, new char[] { '/', '>' }, this.lexTagProc);
            base.RegisterLexerProc(2, new char[] { '"', '\'' }, this.lexStringParamProc);
            base.RegisterLexerProc(2, 'A', 'Z', this.lexParamProc);
            base.RegisterLexerProc(2, 'a', 'z', this.lexParamProc);
            base.RegisterLexerProc(2, '0', '9', this.lexParamNumberProc);
            base.RegisterLexerProc(3, this.lexCommentEndProc);
            base.RegisterLexerProc(4, ']', this.lexCDATAEndProc);
            base.RegisterLexerProc(4, this.lexCDATAProc);
        }

        protected override void InitStyles()
        {
            this.InitDefaultStyles();
        }

        protected virtual bool IsComment(int tok)
        {
            return (tok == 11);
        }

        protected override bool IsValidToken(int tok)
        {
            return ((((tok != 0) && (tok != 1)) && (!this.IsComment(tok) && (tok != 13))) && (tok != 14));
        }

        protected virtual int LexBody()
        {
            base.currentPos++;
            int length = base.source.Length;
            while ((base.currentPos < length) && (base.source[base.currentPos] != '<'))
            {
                base.currentPos++;
            }
            return 2;
        }

        protected virtual int LexCDATA()
        {
            int length = base.source.Length;
            if (base.currentPos < length)
            {
                while (base.currentPos < length)
                {
                    if (((base.currentPos < (length - 2)) && (base.source[base.currentPos] == ']')) && ((base.source[base.currentPos + 1] == ']') && (base.source[base.currentPos + 2] == '>')))
                    {
                        return 14;
                    }
                    base.currentPos++;
                }
            }
            else
            {
                base.currentPos++;
            }
            return 14;
        }

        protected virtual int LexCDATAEnd()
        {
            int length = base.source.Length;
            if (((base.currentPos < (length - 2)) && (base.source[base.currentPos] == ']')) && ((base.source[base.currentPos + 1] == ']') && (base.source[base.currentPos + 2] == '>')))
            {
                base.currentPos += 3;
                this.State = 0;
                return 13;
            }
            return this.LexCDATA();
        }

        protected virtual int LexComment()
        {
            bool flag = (base.currentPos > 0) && (base.source[base.currentPos - 1] == '<');
            base.currentPos++;
            if ((flag && (base.currentPos < (base.source.Length - 1))) && ((base.source[base.currentPos] == '-') && (base.source[base.currentPos + 1] == '-')))
            {
                base.currentPos += 2;
                this.State = 3;
                return 11;
            }
            return 1;
        }

        protected virtual int LexCommentEnd()
        {
            int length = base.source.Length;
            if (base.currentPos < length)
            {
                while (base.currentPos < length)
                {
                    if (((base.currentPos < (length - 2)) && (base.source[base.currentPos] == '-')) && ((base.source[base.currentPos + 1] == '-') && (base.source[base.currentPos + 2] == '>')))
                    {
                        base.currentPos += 3;
                        this.State = 0;
                        break;
                    }
                    base.currentPos++;
                }
            }
            else
            {
                base.currentPos++;
            }
            return 11;
        }

        protected virtual int LexEqual()
        {
            base.currentPos++;
            this.State = 2;
            return 7;
        }

        protected override void LexIdent()
        {
            base.currentPos++;
            int length = base.source.Length;
            while (base.currentPos < length)
            {
                char ch = base.source[base.currentPos];
                if ((((ch < 'a') || (ch > 'z')) && ((ch < 'A') || (ch > 'Z'))) && (((ch < '0') || (ch > '9')) && (((ch != '_') && (ch != ':')) && (ch != '-'))))
                {
                    break;
                }
                base.currentPos++;
            }
        }

        protected virtual int LexName()
        {
            int num = (((base.currentPos > 0) && (base.source[base.currentPos - 1] == '<')) || (((base.currentPos > 1) && (base.source[base.currentPos - 1] == '/')) && (base.source[base.currentPos - 2] == '<'))) ? 8 : 9;
            this.LexIdent();
            return num;
        }

        protected virtual int LexParam()
        {
            this.LexIdent();
            this.State = 1;
            return 10;
        }

        protected virtual int LexParamNumber()
        {
            this.LexNum();
            this.State = 1;
            return 10;
        }

        protected virtual int LexString()
        {
            int length = base.source.Length;
            char ch1 = base.source[base.currentPos];
            base.currentPos++;
            return this.LexStringEnd();
        }

        protected virtual int LexStringEnd()
        {
            int length = base.source.Length;
            while (base.currentPos < length)
            {
                char ch = base.source[base.currentPos];
                if (ch == '"')
                {
                    base.currentPos++;
                    if ((base.currentPos >= length) || (base.source[base.currentPos] != '"'))
                    {
                        this.State = 1;
                        return 10;
                    }
                    base.currentPos++;
                }
                else
                {
                    base.currentPos++;
                }
            }
            this.State = 5;
            return 10;
        }

        protected virtual int LexStringParam()
        {
            this.LexString();
            return 10;
        }

        protected virtual int LexSymbol()
        {
            XmlLexerToken whiteSpace = XmlLexerToken.WhiteSpace;
            char ch = base.source[base.currentPos];
            base.currentPos++;
            switch (ch)
            {
                case '?':
                {
                    whiteSpace = XmlLexerToken.Interr;
                    break;
                }
            }
            return (int) whiteSpace;
        }

        protected virtual int LexTag()
        {
            char ch = base.source[base.currentPos];
            int length = base.source.Length;
            base.currentPos++;
            switch (ch)
            {
                case '<':
                    this.State = 1;
                    if (base.currentPos < length)
                    {
                        ch = base.source[base.currentPos];
                        if (ch == '/')
                        {
                            base.currentPos++;
                            return 5;
                        }
                        if ((ch == '!') && base.source.Substring(base.currentPos).StartsWith("![CDATA["))
                        {
                            this.State = 4;
                            base.currentPos += "![CDATA[".Length;
                            return 13;
                        }
                        if (((ch == '!') && (base.currentPos < (length - 2))) && ((base.source[base.currentPos + 1] == '-') && (base.source[base.currentPos + 2] == '-')))
                        {
                            return this.LexComment();
                        }
                    }
                    return 3;

                case '>':
                    this.State = 0;
                    return 4;
            }
            if ((ch == '/') && (base.currentPos < length))
            {
                ch = base.source[base.currentPos];
                if (ch == '>')
                {
                    base.currentPos++;
                    this.State = 0;
                    return 6;
                }
            }
            return 0;
        }

        protected virtual int LexWhitespace()
        {
            this.LexSpace();
            return 1;
        }

        protected virtual int MoveNext()
        {
            this.prevPosition = this.CurrentPosition;
            int token = this.NextToken();
            while (!this.Eof && !this.IsValidToken(token))
            {
                if (this.IsComment(this.Token))
                {
                    this.ParseComment();
                    token = this.Token;
                }
                else
                {
                    token = this.NextToken();
                }
            }
            return token;
        }

        protected virtual bool ParseBody()
        {
            bool flag = true;
            ISyntaxNode current = this.SyntaxTree.Current;
            while (!this.Eof)
            {
                switch (this.Token)
                {
                    case 3:
                    {
                        if (!this.ParseOpenTag())
                        {
                            flag = false;
                        }
                        continue;
                    }
                    case 5:
                    {
                        ISyntaxNode node = new SyntaxNode(this.TokenPosition, string.Empty, 3, SyntaxNodeOptions.Outlining);
                        Point tokenPosition = this.TokenPosition;
                        this.SaveState();
                        try
                        {
                            this.MoveNext();
                            if ((this.Token == 8) && (this.TokenString == current.Name))
                            {
                                current.AddChild(node);
                            }
                        }
                        finally
                        {
                            this.RestoreState(this.TokenString != current.Name);
                        }
                        if (this.TokenString != current.Name)
                        {
                            current.AddError(new SyntaxError(this.TokenPosition, current.Name, string.Format("{0} end tag {1}", current.Name, StringConsts.ErrExpected)));
                            return flag;
                        }
                        this.SkipTagBody();
                        node.Range.EndPoint = this.prevPosition;
                        return flag;
                    }
                }
                this.AddNode(new SyntaxNode(this.TokenPosition, this.TokenString, 6));
                this.MoveNext();
            }
            return flag;
        }

        protected virtual bool ParseComment()
        {
            int lineIndex = base.lineIndex;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, string.Empty, 7, SyntaxNodeOptions.None);
            this.AddNode(node);
            Point currentPosition = this.CurrentPosition;
            while (this.IsComment(this.Token))
            {
                currentPosition = this.CurrentPosition;
                this.NextToken();
            }
            if (base.lineIndex != lineIndex)
            {
                node.Options |= SyntaxNodeOptions.Outlining;
            }
            node.Range.EndPoint = currentPosition;
            return true;
        }

        protected virtual bool ParseOpenTag()
        {
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, string.Empty, 2);
            this.AddNode(node);
            return this.ParseTag(node);
        }

        protected virtual bool ParseParams()
        {
            string name = string.Empty;
            Point empty = Point.Empty;
            ISyntaxNode node = null;
            ISyntaxNode node2 = new SyntaxNode(this.TokenPosition, string.Empty, 5);
            this.SyntaxTree.Push(node2);
            try
            {
                bool flag = false;
                while (!this.Eof && !flag)
                {
                    switch (this.Token)
                    {
                        case 4:
                        case 6:
                        {
                            flag = true;
                            continue;
                        }
                        case 7:
                        {
                            this.MoveNext();
                            continue;
                        }
                        case 9:
                        {
                            empty = this.TokenPosition;
                            name = this.TokenString;
                            this.MoveNext();
                            continue;
                        }
                        case 10:
                        {
                            if (name != string.Empty)
                            {
                                node = new SyntaxNode(empty, name, 4);
                                node.AddAttribute(new SyntaxAttribute(this.TokenPosition, XmlNodeType.XmlParameter.ToString(), this.TokenString));
                                node.Range.EndPoint = this.CurrentPosition;
                                node2.AddChild(node);
                            }
                            name = string.Empty;
                            this.MoveNext();
                            continue;
                        }
                    }
                    this.MoveNext();
                }
            }
            finally
            {
                this.SyntaxTree.Pop();
            }
            node2.Range.EndPoint = this.prevPosition;
            if (node2.HasChildren)
            {
                this.SyntaxTree.Current.AddChild(node2);
            }
            return (name == string.Empty);
        }

        protected virtual bool ParseTag(ISyntaxNode node)
        {
            bool flag = true;
            this.MoveNext();
            bool flag2 = this.TokenString == "?";
            this.SyntaxTree.Push(node);
            try
            {
                if (!this.ParseTagBody())
                {
                    flag = false;
                }
            }
            finally
            {
                this.SyntaxTree.Pop();
            }
            node.Range.EndPoint = this.prevPosition;
            if (!flag2 && (node.Range.EndPoint.Y != node.Range.StartPoint.Y))
            {
                node.Options = SyntaxNodeOptions.Outlining | SyntaxNodeOptions.Indentation;
                node.AddAttribute(new SyntaxAttribute(this.prevPosition, SyntaxConsts.DefinitionScopeEnd, null));
            }
            return flag;
        }

        protected virtual bool ParseTagBody()
        {
            bool flag = true;
            ISyntaxNode current = this.SyntaxTree.Current;
            bool flag2 = this.TokenString == "!";
            while (!this.Eof)
            {
                switch (this.Token)
                {
                    case 4:
                        current.AddAttribute(new SyntaxAttribute(this.TokenPosition, XmlLexerToken.CloseTag.ToString(), null));
                        this.MoveNext();
                        if (!flag2 && !this.ParseBody())
                        {
                            flag = false;
                        }
                        return flag;

                    case 6:
                        current.AddAttribute(new SyntaxAttribute(this.TokenPosition, XmlLexerToken.CloseEndTag.ToString(), null));
                        this.MoveNext();
                        return flag;

                    case 8:
                    {
                        if (current.Name == string.Empty)
                        {
                            current.Name = this.TokenString;
                        }
                        this.MoveNext();
                        continue;
                    }
                    case 9:
                    {
                        if (!this.ParseParams())
                        {
                            flag = false;
                        }
                        continue;
                    }
                }
                this.MoveNext();
            }
            return flag;
        }

        protected virtual bool ParseUnit()
        {
            bool flag = true;
            ISyntaxNode root = this.SyntaxTree.Root;
            root.NodeType = 1;
            root.Position = Point.Empty;
            flag = this.ParseBody();
            this.SyntaxTree.Root.Range.EndPoint = this.prevPosition;
            root = this.SyntaxTree.Current;
            while (root != this.SyntaxTree.Root)
            {
                root.AddError(new SyntaxError(root.Position, root.Name, StringConsts.ErrEndOfFileFound));
                root.Range.EndPoint = this.prevPosition;
                this.SyntaxTree.Pop();
                root = this.SyntaxTree.Current;
                flag = false;
            }
            return flag;
        }

        public override void ReparseText()
        {
            this.Reset();
            this.MoveNext();
            this.ParseUnit();
            base.ReparseText();
        }

        public override void ResetOptions()
        {
            this.Options = SyntaxOptions.SmartIndent | SyntaxOptions.Outline;
        }

        protected virtual void SkipTagBody()
        {
            while (!this.Eof)
            {
                if (this.Token == 4)
                {
                    break;
                }
                this.MoveNext();
            }
            this.MoveNext();
        }
    }
}

