namespace QWhale.Syntax.Parsers
{
    using QWhale.Common;
    using QWhale.Syntax;
    using QWhale.Syntax.CodeCompletion;
    using QWhale.Syntax.Lexer;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.InteropServices;

    [ToolboxItem(true), ToolboxBitmap(typeof(CParser), "Images.CParser.bmp")]
    public class CParser : SyntaxParser
    {
        protected ISyntaxNodes comments = new SyntaxNodes();
        protected string defaultTypeName = CLexerToken.Int.ToString().ToLower();
        protected Hashtable defines;
        protected LexerProc lexCommentEndProc;
        protected LexerProc lexCommentProc;
        protected LexerProc lexDefineCommentEndProc;
        protected LexerProc lexDefineCommentProc;
        protected LexerProc lexDefineProc;
        protected LexerProc lexIdentifierProc;
        protected LexerProc lexIncludeProc;
        protected LexerProc lexNumberProc;
        protected LexerProc lexStringEndProc;
        protected LexerProc lexStringProc;
        protected LexerProc lexSymbolProc;
        protected LexerProc lexWhitespaceProc;
        protected LexerProc lexXmlCommentProc;
        protected LexerProc lexXmlCommentTagProc;
        protected Point prevPosition;
        protected Hashtable reswords;
        protected Point savePrevPosition;
        private const int stateComment = 1;
        private const int stateDefine = 2;
        private const int stateDefineComment = 3;
        private const int stateNormal = 0;

        public CParser()
        {
            this.Options = SyntaxParserConsts.DefaultNetSyntaxOptions;
        }

        protected virtual void AddAttribute(ISyntaxAttribute attr)
        {
            this.SyntaxTree.Current.AddAttribute(attr);
        }

        protected virtual void AddNode(ISyntaxNode node)
        {
            this.SyntaxTree.Current.AddChild(node);
        }

        protected virtual bool AfterDeclaration(ISyntaxNode node)
        {
            bool flag = true;
            ISyntaxAttribute attribute = node.FindAttribute(NetNodeType.Modifier.ToString());
            if ((attribute != null) && (((string) attribute.Value) == CLexerToken.Typedef.ToString().ToLower()))
            {
                this.ProcessTypeDefList(node);
            }
            if (flag && node.HasChildren)
            {
                foreach (ISyntaxNode node2 in node.ChildList)
                {
                    if (!this.AfterDeclaration(node2))
                    {
                        flag = false;
                    }
                }
            }
            return flag;
        }

        protected virtual bool BeforeDeclaration(ISyntaxNode node)
        {
            return true;
        }

        protected virtual bool ClearStack()
        {
            bool flag = true;
            ISyntaxNode current = this.SyntaxTree.Current;
            while (current != this.SyntaxTree.Root)
            {
                this.SyntaxError(current.Position, new Point(current.Position.X + 1, current.Position.Y), current.Name, StringConsts.ErrEndOfFileFound);
                current.Range.EndPoint = this.prevPosition;
                this.SyntaxTree.Pop();
                current = this.SyntaxTree.Current;
                flag = false;
            }
            return flag;
        }

        protected virtual ISyntaxNode CreateExpressionNode(Point position, string name, int nodeType, ISyntaxNode refNode, bool addAttrbute)
        {
            ISyntaxNode node = new SyntaxNode(position, name, nodeType);
            if (refNode != null)
            {
                if (addAttrbute)
                {
                    node.AddAttribute(new SyntaxAttribute(position, NetNodeType.Expression.ToString(), name));
                }
                node.Range.StartPoint = refNode.Position;
                node.AddChild(refNode);
            }
            return node;
        }

        protected override IListMembers CreateListMembers()
        {
            return new CsListMembers();
        }

        protected override IParameterInfo CreateParameterInfo()
        {
            return new CsParameterInfo();
        }

        public override ICodeCompletionRepository CreateRepository()
        {
            IReflectionRepository repository = new CRepository(this.CaseSensitive, this.SyntaxTree);
            repository.RegisterType("void", typeof(void));
            repository.RegisterType("bool", typeof(bool));
            repository.RegisterType("char", typeof(char));
            repository.RegisterType("double", typeof(double));
            repository.RegisterType("float", typeof(float));
            repository.RegisterType("int", typeof(int));
            repository.RegisterType("long", typeof(long));
            repository.RegisterType("short", typeof(short));
            repository.RegisterType("unsigned", typeof(int));
            repository.RegisterType("signed", typeof(long));
            return repository;
        }

        protected bool Expected(CLexerToken token)
        {
            bool flag = this.Token == (int)token;
            if (!flag)
            {
                this.SyntaxError((int) token);
            }
            if (flag)
            {
                this.MoveNext();
            }
            return flag;
        }

        protected bool Expected(int token)
        {
            return this.Expected((CLexerToken) token);
        }

        protected bool Expected(CLexerToken token1, CLexerToken token2)
        {
            bool flag = (this.Token == (int)token1) || (this.Token == (int)token2);
            if (!flag)
            {
                this.SyntaxError((int) token1);
            }
            if (flag)
            {
                this.MoveNext();
            }
            return flag;
        }

        protected virtual void FixupComments()
        {
            foreach (ISyntaxNode node in this.comments)
            {
                ISyntaxNode node2 = this.SyntaxTree.FindNode(node, base.pointNodeComparer);
                if (node2 != null)
                {
                    if (this.ShouldOutlineCommentNode(node))
                    {
                        node.Options |= SyntaxNodeOptions.Outlining;
                    }
                    node2.InsertChild(node, base.nodeComparer);
                }
            }
            this.comments.Clear();
        }

        protected virtual bool FixupRegions(ISyntaxNode node)
        {
            bool flag = true;
            if (node.HasChildren)
            {
                for (int i = node.ChildList.Count - 1; i >= 0; i--)
                {
                    if (!this.FixupRegions(node.ChildList[i]))
                    {
                        flag = false;
                    }
                }
            }
            if (((node.NodeType == 40) && ((node.Options & SyntaxNodeOptions.Outlining) == SyntaxNodeOptions.None)) && (node.Parent != null))
            {
                if (node.HasChildren)
                {
                    foreach (ISyntaxNode node2 in node.ChildList)
                    {
                        node.Parent.AddChild(node2);
                    }
                    node.ChildList.Clear();
                }
                node.Parent.AddError(new QWhale.Syntax.SyntaxError(node.Position, node.Name, StringConsts.ErrEndOfFileFound));
            }
            return flag;
        }

        public override ISyntaxNode GetAutoFormatNode(Point position, bool extended, out Point startPt)
        {
            return NETRepository.GetAutoFormatNode(this.SyntaxTree.FindNode(new SyntaxNode(position, string.Empty), base.pointNodeComparer), extended, out startPt);
        }

        protected virtual ISyntaxNode GetBlockNode(ISyntaxNode node, Point position)
        {
            ISyntaxNode blockNode = NETRepository.GetBlockNode(node);
            while (blockNode != null)
            {
                if (blockNode.FindAttribute(SyntaxConsts.SuppressReparsingScope) == null)
                {
                    return blockNode;
                }
                node = NETRepository.GetBlockNode(blockNode.Parent);
                if (node == null)
                {
                    return blockNode;
                }
                blockNode = node;
            }
            return blockNode;
        }

        public override CodeCompletionType GetCompletionType(char ch)
        {
            switch (ch)
            {
                case ' ':
                    return CodeCompletionType.SpecialListMembers;

                case '(':
                    return CodeCompletionType.ParameterInfo;

                case '.':
                    return CodeCompletionType.ListMembers;

                case '/':
                    return CodeCompletionType.CompleteComment;

                case '[':
                    return CodeCompletionType.ParameterInfo;
            }
            return base.GetCompletionType(ch);
        }

        private string GetHeaderName()
        {
            string tokenString = this.TokenString;
            if (tokenString.StartsWith("<") || tokenString.StartsWith("\""))
            {
                tokenString = tokenString.Substring(1);
            }
            if (!tokenString.EndsWith(">") && !tokenString.EndsWith("\""))
            {
                return tokenString;
            }
            return tokenString.Substring(0, tokenString.Length - 1);
        }

        protected virtual ISyntaxNode GetLastChild(ISyntaxNode node)
        {
            if (node.HasChildren)
            {
                return this.GetLastChild(node.ChildList[node.ChildList.Count - 1]);
            }
            return node;
        }

        protected override int GetLexerStyle(int token)
        {
            if (this.IsKeywordToken(token))
            {
                return 2;
            }
            if ((token >= 0x27) && (token <= 0x60))
            {
                return 5;
            }
            switch (token)
            {
                case 0x61:
                case 0x62:
                case 0x63:
                    return 1;

                case 100:
                case 0x65:
                    return 7;

                case 0x66:
                    return 0;

                case 0x67:
                    return 3;

                case 0x68:
                    return 8;

                case 0x6a:
                case 0x6b:
                    return 9;
            }
            return 6;
        }

        protected virtual ISyntaxNode GetValidNode(ISyntaxNode node)
        {
            while ((node.Parent != null) && (node.NodeType == 40))
            {
                node = node.Parent;
            }
            return node;
        }

        protected bool IdentifierExpected()
        {
            return this.Expected(CLexerToken.Identifier_Literal);
        }

        protected virtual void InitDefines()
        {
            this.defines = new Hashtable();
            this.defines.Add("if", 1);
            this.defines.Add("ifdef", 1);
            this.defines.Add("ifndef", 1);
            this.defines.Add("elif", 1);
            this.defines.Add("else", 1);
            this.defines.Add("endif", 1);
            this.defines.Add("include", 1);
            this.defines.Add("define", 1);
            this.defines.Add("undef", 1);
            this.defines.Add("line", 1);
            this.defines.Add("error", 1);
            this.defines.Add("pragma", 1);
            this.defines.Add("region", 1);
            this.defines.Add("endregion", 1);
        }

        protected override void InitLexer()
        {
            base.InitLexer();
            this.InitReswords();
            this.InitDefines();
            this.lexWhitespaceProc = new LexerProc(this.LexWhitespace);
            this.lexSymbolProc = new LexerProc(this.LexSymbol);
            this.lexIdentifierProc = new LexerProc(this.LexIdentifier);
            this.lexNumberProc = new LexerProc(this.LexNumber);
            this.lexStringProc = new LexerProc(this.LexString);
            this.lexCommentProc = new LexerProc(this.LexComment);
            this.lexCommentEndProc = new LexerProc(this.LexCommentEnd);
            this.lexDefineProc = new LexerProc(this.LexDefine);
            this.lexIncludeProc = new LexerProc(this.LexInclude);
            this.lexDefineCommentProc = new LexerProc(this.LexDefineComment);
            this.lexDefineCommentEndProc = new LexerProc(this.LexDefineCommentEnd);
            base.RegisterLexerProc(0, this.lexWhitespaceProc);
            base.RegisterLexerProc(0, '!', '\x00ff', this.lexSymbolProc);
            base.RegisterLexerProc(0, '€', this.lexSymbolProc);
            base.RegisterLexerProc(0, 'a', 'z', this.lexIdentifierProc);
            base.RegisterLexerProc(0, 'A', 'Z', this.lexIdentifierProc);
            base.RegisterLexerProc(0, '_', this.lexIdentifierProc);
            base.RegisterLexerProc(0, '0', '9', this.lexNumberProc);
            base.RegisterLexerProc(0, new char[] { '+', '-' }, this.lexSymbolProc);
            base.RegisterLexerProc(0, new char[] { '"', '\'' }, this.lexStringProc);
            base.RegisterLexerProc(0, '/', this.lexCommentProc);
            base.RegisterLexerProc(1, this.lexCommentEndProc);
            base.RegisterLexerProc(2, this.lexWhitespaceProc);
            base.RegisterLexerProc(2, '!', '\x00ff', this.lexSymbolProc);
            base.RegisterLexerProc(2, 'a', 'z', this.lexIdentifierProc);
            base.RegisterLexerProc(2, 'A', 'Z', this.lexIdentifierProc);
            base.RegisterLexerProc(2, '_', this.lexIdentifierProc);
            base.RegisterLexerProc(2, '0', '9', this.lexNumberProc);
            base.RegisterLexerProc(2, new char[] { '+', '-' }, this.lexSymbolProc);
            base.RegisterLexerProc(2, new char[] { '"', '\'' }, this.lexStringProc);
            base.RegisterLexerProc(2, '/', this.lexDefineCommentProc);
            base.RegisterLexerProc(3, this.lexDefineCommentEndProc);
            base.RegisterLexerProc(2, new char[] { '<', '"' }, this.lexIncludeProc);
        }

        protected virtual void InitReswords()
        {
            this.reswords = new Hashtable();
            this.reswords.Add("auto", CLexerToken.Auto);
            this.reswords.Add("break", CLexerToken.Break);
            this.reswords.Add("case", CLexerToken.Case);
            this.reswords.Add("char", CLexerToken.Char);
            this.reswords.Add("const", CLexerToken.Const);
            this.reswords.Add("continue", CLexerToken.Continue);
            this.reswords.Add("default", CLexerToken.Default);
            this.reswords.Add("do", CLexerToken.Do);
            this.reswords.Add("double", CLexerToken.Double);
            this.reswords.Add("else", CLexerToken.Else);
            this.reswords.Add("enum", CLexerToken.Enum);
            this.reswords.Add("extern", CLexerToken.Extern);
            this.reswords.Add("float", CLexerToken.Float);
            this.reswords.Add("for", CLexerToken.For);
            this.reswords.Add("goto", CLexerToken.Goto);
            this.reswords.Add("if", CLexerToken.If);
            this.reswords.Add("inline", CLexerToken.Inline);
            this.reswords.Add("int", CLexerToken.Int);
            this.reswords.Add("long", CLexerToken.Long);
            this.reswords.Add("register", CLexerToken.Register);
            this.reswords.Add("restrict", CLexerToken.Restrict);
            this.reswords.Add("return", CLexerToken.Return);
            this.reswords.Add("short", CLexerToken.Short);
            this.reswords.Add("signed", CLexerToken.Signed);
            this.reswords.Add("sizeof", CLexerToken.Sizeof);
            this.reswords.Add("static", CLexerToken.Static);
            this.reswords.Add("struct", CLexerToken.Struct);
            this.reswords.Add("switch", CLexerToken.Switch);
            this.reswords.Add("typedef", CLexerToken.Typedef);
            this.reswords.Add("union", CLexerToken.Union);
            this.reswords.Add("unsigned", CLexerToken.Unsigned);
            this.reswords.Add("void", CLexerToken.Void);
            this.reswords.Add("volatile", CLexerToken.Volatile);
            this.reswords.Add("while", CLexerToken.While);
        }

        protected override void InitStyles()
        {
            this.InitDefaultStyles();
        }

        private void InvalidPreviousMethodDeclaration(string methodName)
        {
            this.ProcessMethodNode(this.SyntaxTree.Root, methodName);
        }

        protected virtual bool IsBaseList(int token)
        {
            return (token == 50);
        }

        protected virtual bool IsBuiltInType(int Token)
        {
            switch (((CLexerToken) Token))
            {
                case CLexerToken.Char:
                case CLexerToken.Double:
                case CLexerToken.Int:
                case CLexerToken.Long:
                case CLexerToken.Short:
                case CLexerToken.Signed:
                case CLexerToken.Float:
                case CLexerToken.Unsigned:
                case CLexerToken.Void:
                case CLexerToken.Bool:
                case CLexerToken.Complex:
                case CLexerToken.Imaginary:
                    return true;
            }
            return false;
        }

        protected virtual bool IsComment(int tok)
        {
            return (tok == 0x67);
        }

        private bool IsDeclaration()
        {
            bool flag = true;
            this.SaveState();
            try
            {
                string str;
                this.MoveNext();
                this.ParseType(out str);
                if (this.Token == 0x66)
                {
                    flag = false;
                }
            }
            finally
            {
                this.RestoreState(true);
            }
            return flag;
        }

        public override bool IsDeclaration(ISyntaxNode node)
        {
            return ((node != null) && NETRepository.IsDeclarationNode(node));
        }

        protected virtual bool IsFunctionPointer(out string identifier, bool saveState, ref ISyntaxNode node)
        {
            bool flag = false;
            identifier = string.Empty;
            if (this.Token == 0x2b)
            {
                if (saveState)
                {
                    this.SaveState();
                }
                try
                {
                    this.MoveNext();
                    flag = this.Expected(CLexerToken.Star) && this.ParseIdentifier(out identifier);
                    if ((flag && (this.Token == 0x2b)) && !this.ParseParameterListDeclaration(true, ref node))
                    {
                        return false;
                    }
                    if (flag)
                    {
                        string str;
                        this.ParseRankSpecifier(out str);
                        if (str != string.Empty)
                        {
                            identifier = identifier + str;
                        }
                    }
                    if (!this.Expected(CLexerToken.Close_parens))
                    {
                        flag = false;
                    }
                }
                finally
                {
                    if (saveState)
                    {
                        this.RestoreState(!flag);
                    }
                }
            }
            return flag;
        }

        protected bool IsIdentifierToken(int token)
        {
            return (token == 0x66);
        }

        protected virtual bool IsKeywordToken(int token)
        {
            return ((token >= 0) && (token <= 0x26));
        }

        protected virtual bool IsModifier(int token)
        {
            CLexerToken token2 = (CLexerToken) token;
            if (token2 <= CLexerToken.Inline)
            {
                switch (token2)
                {
                    case CLexerToken.Extern:
                    case CLexerToken.Inline:
                    case CLexerToken.Auto:
                    case CLexerToken.Const:
                        goto Label_003C;
                }
                goto Label_003E;
            }
            if (token2 <= CLexerToken.Static)
            {
                switch (token2)
                {
                    case CLexerToken.Register:
                    case CLexerToken.Static:
                        goto Label_003C;
                }
                goto Label_003E;
            }
            if ((token2 != CLexerToken.Typedef) && (token2 != CLexerToken.Volatile))
            {
                goto Label_003E;
            }
        Label_003C:
            return true;
        Label_003E:
            return false;
        }

        protected virtual bool IsParameterModifier(int token)
        {
            CLexerToken token2 = (CLexerToken) token;
            if (token2 <= CLexerToken.Enum)
            {
                switch (token2)
                {
                    case CLexerToken.Class:
                    case CLexerToken.Const:
                    case CLexerToken.Enum:
                        goto Label_002C;
                }
                goto Label_002E;
            }
            if ((token2 != CLexerToken.Struct) && (token2 != CLexerToken.Union))
            {
                goto Label_002E;
            }
        Label_002C:
            return true;
        Label_002E:
            return false;
        }

        protected virtual bool IsPointerType(int token)
        {
            return (token == 0x41);
        }

        protected override bool IsValidToken(int tok)
        {
            return ((tok != 0x69) && !this.IsComment(tok));
        }

        protected virtual int LexComment()
        {
            base.currentPos++;
            int length = base.source.Length;
            if (base.currentPos < length)
            {
                switch (base.source[base.currentPos])
                {
                    case '*':
                        base.currentPos++;
                        return this.LexCommentEnd();

                    case '/':
                    {
                        int currentPos = base.currentPos;
                        base.currentPos = length + 1;
                        return 0x67;
                    }
                }
            }
            base.currentPos--;
            return this.LexSymbol();
        }

        protected virtual int LexCommentEnd()
        {
            int length = base.source.Length;
            while (base.currentPos < length)
            {
                char ch = base.source[base.currentPos];
                if (ch == '*')
                {
                    base.currentPos++;
                    if ((base.currentPos < length) && (base.source[base.currentPos] == '/'))
                    {
                        this.State = 0;
                        base.currentPos++;
                        return 0x67;
                    }
                }
                else
                {
                    base.currentPos++;
                }
            }
            this.State = 1;
            return 0x67;
        }

        protected virtual int LexDefine()
        {
            int length = base.source.Length;
            int currentPos = base.currentPos;
            if (base.currentPos < base.source.Length)
            {
                char ch = base.source[base.currentPos];
                if ((ch >= '\0') && (ch <= ' '))
                {
                    this.LexSpace();
                }
                if (base.currentPos < base.source.Length)
                {
                    ch = base.source[base.currentPos];
                    if ((((ch >= 'a') && (ch <= 'z')) || ((ch >= 'A') && (ch <= 'Z'))) || (ch == '_'))
                    {
                        int startIndex = base.currentPos;
                        this.LexIdent();
                        string str = base.source.Substring(startIndex, base.currentPos - startIndex);
                        if (this.defines[str] != null)
                        {
                            this.State = ((base.currentPos == length) || (str == "endif")) ? 0 : 2;
                            return 0x68;
                        }
                    }
                }
            }
            base.currentPos = currentPos;
            return 0x3f;
        }

        protected virtual int LexDefineComment()
        {
            base.currentPos++;
            int length = base.source.Length;
            if (base.currentPos < length)
            {
                switch (base.source[base.currentPos])
                {
                    case '*':
                        base.currentPos++;
                        return this.LexDefineCommentEnd();

                    case '/':
                    {
                        int currentPos = base.currentPos;
                        base.currentPos = length + 1;
                        return 0x67;
                    }
                }
            }
            base.currentPos--;
            return this.LexSymbol();
        }

        protected virtual int LexDefineCommentEnd()
        {
            int length = base.source.Length;
            while (base.currentPos < length)
            {
                char ch = base.source[base.currentPos];
                if (ch == '*')
                {
                    base.currentPos++;
                    if ((base.currentPos < length) && (base.source[base.currentPos] == '/'))
                    {
                        this.State = 2;
                        base.currentPos++;
                        return 0x67;
                    }
                }
                else
                {
                    base.currentPos++;
                }
            }
            this.State = 3;
            return 0x67;
        }

        protected virtual int LexHexNumber()
        {
            int length = base.source.Length;
            while (base.currentPos < length)
            {
                char ch = base.source[base.currentPos];
                if ((((ch < '0') || (ch > '9')) && ((ch < 'a') || (ch > 'f'))) && ((ch < 'A') || (ch > 'F')))
                {
                    break;
                }
                base.currentPos++;
            }
            this.LexSuffixes();
            return 0x61;
        }

        protected virtual int LexIdentifier()
        {
            char ch = base.source[base.currentPos];
            if (((ch == 'l') || (ch == 'L')) && ((base.currentPos < (base.source.Length - 1)) && ((base.source[base.currentPos + 1] == '\'') || (base.source[base.currentPos + 1] == '"'))))
            {
                base.currentPos++;
                return this.LexString();
            }
            this.LexIdent();
            if ((this.State == 2) && (this.TokenString == "defined"))
            {
                return 0x68;
            }
            object obj2 = this.reswords[this.TokenString];
            if (obj2 == null)
            {
                return 0x66;
            }
            return (int) obj2;
        }

        protected virtual int LexInclude()
        {
            char ch = base.source[base.currentPos];
            if (!base.source.Substring(0, base.currentPos).Trim().EndsWith("include"))
            {
                if (ch == '"')
                {
                    return this.LexString();
                }
                return this.LexWhitespace();
            }
            if (ch == '<')
            {
                ch = '>';
            }
            base.currentPos++;
            int length = base.source.Length;
            while (base.currentPos < length)
            {
                char ch2 = base.source[base.currentPos];
                if (ch2 == ch)
                {
                    base.currentPos++;
                    break;
                }
                base.currentPos++;
            }
            if (ch != '\'')
            {
                return 0x6b;
            }
            return 0x6a;
        }

        protected virtual int LexNumber()
        {
            int num2;
            char ch = base.source[base.currentPos];
            int length = base.source.Length;
            int num3 = 0x61;
            switch (ch)
            {
                case '+':
                case '-':
                    num2 = base.currentPos + 1;
                    if (((num2 >= length) || (base.source[num2] < '0')) || (base.source[num2] > '9'))
                    {
                        return this.LexSymbol();
                    }
                    break;
            }
            if (ch == '0')
            {
                num2 = base.currentPos + 1;
                if ((num2 < length) && ((base.source[num2] == 'x') || (base.source[num2] == 'X')))
                {
                    base.currentPos = num2 + 1;
                    return this.LexHexNumber();
                }
                if ((num2 < length) && ((base.source[num2] == 'b') || (base.source[num2] == 'B')))
                {
                    base.currentPos = num2 + 1;
                    return this.LexOctNumber();
                }
            }
            base.LexNum();
            if (!this.LexSuffixes())
            {
                if (base.currentPos < length)
                {
                    ch = base.source[base.currentPos];
                    char ch2 = ch;
                    if ((ch2 == '.') && (base.currentPos < (length - 1)))
                    {
                        ch = base.source[base.currentPos + 1];
                        if ((ch >= '0') && (ch <= '9'))
                        {
                            base.currentPos++;
                            this.LexNum();
                            num3 = 0x62;
                            if (base.currentPos < length)
                            {
                                ch = base.source[base.currentPos];
                                switch (ch)
                                {
                                    case 'f':
                                    case 'l':
                                    case 'F':
                                    case 'L':
                                        base.currentPos++;
                                        return 0x62;
                                }
                            }
                        }
                    }
                }
                if (base.currentPos < length)
                {
                    ch = base.source[base.currentPos];
                    if ((ch != 'E') && (ch != 'e'))
                    {
                        return num3;
                    }
                    num2 = base.currentPos + 1;
                    if (num2 < length)
                    {
                        ch = base.source[num2];
                        switch (ch)
                        {
                            case '+':
                            case '-':
                                num2++;
                                break;
                        }
                    }
                    if (num2 < length)
                    {
                        ch = base.source[num2];
                        if ((ch >= '0') && (ch <= '9'))
                        {
                            base.currentPos = num2;
                            this.LexNum();
                            num3 = 0x63;
                        }
                    }
                }
            }
            return num3;
        }

        protected virtual int LexOctNumber()
        {
            int length = base.source.Length;
            while (base.currentPos < length)
            {
                char ch = base.source[base.currentPos];
                if ((ch < '0') || (ch > '7'))
                {
                    break;
                }
                base.currentPos++;
            }
            return 0x61;
        }

        protected virtual int LexString()
        {
            char ch = base.source[base.currentPos];
            base.currentPos++;
            int length = base.source.Length;
            bool flag = false;
            while (base.currentPos < length)
            {
                char ch2 = base.source[base.currentPos];
                if ((ch2 == ch) && !flag)
                {
                    base.currentPos++;
                    break;
                }
                if (ch2 == '\\')
                {
                    flag = !flag;
                }
                else
                {
                    flag = false;
                }
                base.currentPos++;
            }
            if (ch != '\'')
            {
                return 0x65;
            }
            return 100;
        }

        protected virtual bool LexSuffixes()
        {
            string str = base.source.Substring(base.currentPos).ToLower();
            if (str.StartsWith("ull") || str.StartsWith("llu"))
            {
                base.currentPos += 3;
                return true;
            }
            if (str.StartsWith("ul") || str.StartsWith("lu"))
            {
                base.currentPos += 2;
                return true;
            }
            if (!str.StartsWith("u") && !str.StartsWith("l"))
            {
                return false;
            }
            base.currentPos++;
            return true;
        }

        protected virtual int LexSymbol()
        {
            CLexerToken percent = CLexerToken.Whitespace_Literal;
            char ch = base.source[base.currentPos];
            base.currentPos++;
            switch (ch)
            {
                case '{':
                    percent = CLexerToken.Open_brace;
                    goto Label_05EF;

                case '|':
                {
                    char ch8 = base.CurChar();
                    if (ch8 == '=')
                    {
                        base.currentPos++;
                        percent = CLexerToken.Op_or_assign;
                    }
                    else if (ch8 != '|')
                    {
                        percent = CLexerToken.Bitwise_or;
                    }
                    else
                    {
                        base.currentPos++;
                        percent = CLexerToken.Op_or;
                    }
                    goto Label_05EF;
                }
                case '}':
                    percent = CLexerToken.Close_brace;
                    goto Label_05EF;

                case '~':
                    percent = CLexerToken.Tilda;
                    goto Label_05EF;

                case '€':
                    if (base.CurChar() == '=')
                    {
                        base.currentPos++;
                        percent = CLexerToken.Op_evro_assign;
                    }
                    else
                    {
                        percent = CLexerToken.Evro;
                    }
                    goto Label_05EF;

                case '!':
                    if (base.CurChar() != '=')
                    {
                        percent = CLexerToken.Bang;
                    }
                    else
                    {
                        base.currentPos++;
                        percent = CLexerToken.Op_ne;
                    }
                    goto Label_05EF;

                case '#':
                    if (base.CurChar() != '#')
                    {
                        return this.LexDefine();
                    }
                    base.currentPos++;
                    percent = CLexerToken.DblSharp;
                    goto Label_05EF;

                case '%':
                    switch (base.CurChar())
                    {
                        case ':':
                            if (((base.currentPos < (base.source.Length - 2)) && (base.source[base.currentPos + 1] == '%')) && (base.source[base.currentPos + 1] == ':'))
                            {
                                base.currentPos += 3;
                                percent = CLexerToken.Op_Dbl_persent_colon;
                            }
                            base.currentPos++;
                            percent = CLexerToken.Op_persent_colon;
                            goto Label_05EF;

                        case '=':
                            base.currentPos++;
                            percent = CLexerToken.Op_mod_assign;
                            goto Label_05EF;

                        case '>':
                            base.currentPos++;
                            percent = CLexerToken.Op_percent_shift_right;
                            goto Label_05EF;
                    }
                    break;

                case '&':
                    switch (base.CurChar())
                    {
                        case '&':
                            base.currentPos++;
                            percent = CLexerToken.Op_and;
                            goto Label_05EF;

                        case '=':
                            base.currentPos++;
                            percent = CLexerToken.Op_and_assign;
                            goto Label_05EF;
                    }
                    percent = CLexerToken.Bitwise_and;
                    goto Label_05EF;

                case '(':
                    percent = CLexerToken.Open_parens;
                    goto Label_05EF;

                case ')':
                    percent = CLexerToken.Close_parens;
                    goto Label_05EF;

                case '*':
                    if (base.CurChar() != '=')
                    {
                        percent = CLexerToken.Star;
                    }
                    else
                    {
                        base.currentPos++;
                        percent = CLexerToken.Op_mult_assign;
                    }
                    goto Label_05EF;

                case '+':
                    switch (base.CurChar())
                    {
                        case '+':
                            base.currentPos++;
                            percent = CLexerToken.Op_inc;
                            goto Label_05EF;

                        case '=':
                            base.currentPos++;
                            percent = CLexerToken.Op_add_assign;
                            goto Label_05EF;
                    }
                    percent = CLexerToken.Plus;
                    goto Label_05EF;

                case ',':
                    percent = CLexerToken.Comma;
                    goto Label_05EF;

                case '-':
                    switch (base.CurChar())
                    {
                        case '=':
                            base.currentPos++;
                            percent = CLexerToken.Op_sub_assign;
                            goto Label_05EF;

                        case '>':
                            base.currentPos++;
                            percent = CLexerToken.Op_ptr;
                            goto Label_05EF;

                        case '-':
                            base.currentPos++;
                            percent = CLexerToken.Op_dec;
                            goto Label_05EF;
                    }
                    percent = CLexerToken.Minus;
                    goto Label_05EF;

                case '.':
                    if (((base.currentPos >= (base.source.Length - 1)) || (base.source[base.currentPos] != '.')) || (base.source[base.currentPos + 1] != '.'))
                    {
                        percent = CLexerToken.Dot;
                    }
                    else
                    {
                        base.currentPos += 2;
                        percent = CLexerToken.Ellipsis;
                    }
                    goto Label_05EF;

                case '/':
                    if (base.CurChar() != '=')
                    {
                        percent = CLexerToken.Div;
                    }
                    else
                    {
                        base.currentPos++;
                        percent = CLexerToken.Op_div_assign;
                    }
                    goto Label_05EF;

                case ':':
                    if (base.CurChar() != ':')
                    {
                        if (base.CurChar() == '>')
                        {
                            base.currentPos++;
                            percent = CLexerToken.Colon_shift_right;
                        }
                        else
                        {
                            percent = CLexerToken.Colon;
                        }
                    }
                    else
                    {
                        base.currentPos++;
                        percent = CLexerToken.DblColon;
                    }
                    goto Label_05EF;

                case ';':
                    percent = CLexerToken.Semicolon;
                    goto Label_05EF;

                case '<':
                    switch (base.CurChar())
                    {
                        case ':':
                            base.currentPos++;
                            percent = CLexerToken.Op_lt_colon;
                            goto Label_05EF;

                        case '<':
                            base.currentPos++;
                            if (base.CurChar() == '=')
                            {
                                base.currentPos++;
                                percent = CLexerToken.Op_shift_left_assign;
                            }
                            else
                            {
                                percent = CLexerToken.Op_shift_left;
                            }
                            goto Label_05EF;

                        case '=':
                            base.currentPos++;
                            percent = CLexerToken.Op_le;
                            goto Label_05EF;

                        case '%':
                            base.currentPos++;
                            percent = CLexerToken.Op_lt_persent;
                            goto Label_05EF;
                    }
                    percent = CLexerToken.Op_lt;
                    goto Label_05EF;

                case '=':
                    if (base.CurChar() != '=')
                    {
                        percent = CLexerToken.Assign;
                    }
                    else
                    {
                        base.currentPos++;
                        percent = CLexerToken.Op_eq;
                    }
                    goto Label_05EF;

                case '>':
                    switch (base.CurChar())
                    {
                        case '=':
                            base.currentPos++;
                            percent = CLexerToken.Op_ge;
                            goto Label_05EF;

                        case '>':
                            base.currentPos++;
                            if (base.CurChar() == '=')
                            {
                                base.currentPos++;
                                percent = CLexerToken.Op_shift_right_assign;
                            }
                            else
                            {
                                percent = CLexerToken.Op_shift_right;
                            }
                            goto Label_05EF;
                    }
                    percent = CLexerToken.Op_gt;
                    goto Label_05EF;

                case '?':
                    percent = CLexerToken.Interr;
                    goto Label_05EF;

                case '[':
                    percent = CLexerToken.Open_bracket;
                    goto Label_05EF;

                case ']':
                    percent = CLexerToken.Close_bracket;
                    goto Label_05EF;

                case '^':
                    percent = CLexerToken.Exponent;
                    goto Label_05EF;

                default:
                    goto Label_05EF;
            }
            percent = CLexerToken.Percent;
        Label_05EF:
            return (int) percent;
        }

        protected virtual int LexWhitespace()
        {
            this.LexSpace();
            return 0x69;
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

        protected virtual bool ParseAdditiveExpression(ref ISyntaxNode node)
        {
            bool flag = this.ParseMultiplicativeExpression(ref node);
            switch (this.Token)
            {
                case 0x35:
                case 0x36:
                {
                    node = this.CreateExpressionNode(this.TokenPosition, this.TokenString, 0x91, node, true);
                    this.MoveNext();
                    ISyntaxNode node2 = null;
                    if (!this.ParseAdditiveExpression(ref node2))
                    {
                        flag = false;
                    }
                    if (node2 != null)
                    {
                        node.AddChild(node2);
                    }
                    node.Range.EndPoint = this.prevPosition;
                    return flag;
                }
            }
            return flag;
        }

        protected virtual bool ParseAndExpression(ref ISyntaxNode node)
        {
            bool flag = this.ParseEqualityExpression(ref node);
            if (this.Token == 0x3d)
            {
                node = this.CreateExpressionNode(this.TokenPosition, this.TokenString, 0x8b, node, true);
                this.MoveNext();
                ISyntaxNode node2 = null;
                if (!this.ParseAndExpression(ref node2))
                {
                    flag = false;
                }
                if (node2 != null)
                {
                    node.AddChild(node2);
                }
                node.Range.EndPoint = this.prevPosition;
            }
            return flag;
        }

        protected virtual bool ParseArgument(ref ISyntaxNode node)
        {
            bool flag = true;
            node = new SyntaxNode(this.TokenPosition, string.Empty, 0x1c);
            ISyntaxNode node2 = null;
            this.SaveState();
            try
            {
                flag = this.ParseExpression(ref node2);
                if (!flag)
                {
                    node2 = null;
                }
                this.savePrevPosition = this.prevPosition;
            }
            finally
            {
                this.RestoreState(!flag);
            }
            if (node2 != null)
            {
                node.AddChild(node2);
            }
            node.Range.EndPoint = this.prevPosition;
            return flag;
        }

        protected virtual bool ParseArgumentExpression(ref ISyntaxNode node)
        {
            bool flag = true;
            node = new SyntaxNode(this.TokenPosition, string.Empty, 0x1c);
            ISyntaxNode node2 = null;
            flag = this.ParseExpression(ref node2);
            if (node2 != null)
            {
                node.AddChild(node2);
            }
            node.Range.EndPoint = this.prevPosition;
            return flag;
        }

        protected virtual bool ParseArgumentExpressionList(ref ISyntaxNode node)
        {
            bool flag = true;
            node = new SyntaxNode(this.TokenPosition, string.Empty, 0x1d);
            this.MoveNext();
            if (this.Token != 0x2a)
            {
                ISyntaxNode node2 = null;
                flag = (this.Token == 0x30) || this.ParseArgumentExpression(ref node2);
                if (node2 != null)
                {
                    node.AddChild(node2);
                }
                while (this.Token == 0x30)
                {
                    this.MoveNext();
                    node2 = null;
                    if ((this.Token != 0x30) && !this.ParseArgumentExpression(ref node2))
                    {
                        flag = false;
                    }
                    if (node2 != null)
                    {
                        node.AddChild(node2);
                    }
                }
            }
            node.Range.EndPoint = this.prevPosition;
            return flag;
        }

        protected virtual bool ParseArgumentList(ref ISyntaxNode node)
        {
            bool flag = true;
            node = new SyntaxNode(this.TokenPosition, this.TokenString, 0x1d);
            if (this.Token != 0x2b)
            {
                this.SyntaxError(0x2b);
                return false;
            }
            this.MoveNext();
            while (!this.Eof && (this.Token != 0x2c))
            {
                ISyntaxNode node2 = null;
                if (!this.ParseArgument(ref node2))
                {
                    flag = false;
                }
                if (node2 != null)
                {
                    node.AddChild(node2);
                }
                if (this.Token != 0x30)
                {
                    break;
                }
                this.MoveNext();
            }
            if (!this.Expected(CLexerToken.Close_parens))
            {
                flag = false;
            }
            node.Range.EndPoint = this.prevPosition;
            return flag;
        }

        protected virtual bool ParseArrayInitializerExpression(ref ISyntaxNode node)
        {
            bool flag = true;
            node = new SyntaxNode(this.TokenPosition, this.TokenString, 0x9c);
            this.SyntaxTree.Push(node);
            try
            {
                this.MoveNext();
                while (!this.Eof && (this.Token != 40))
                {
                    flag = this.ParseVariableInitializer();
                    if (this.Token != 0x30)
                    {
                        goto Label_0069;
                    }
                    this.MoveNext();
                }
            }
            finally
            {
                this.SyntaxTree.Pop();
            }
        Label_0069:
            if (!this.Expected(CLexerToken.Close_brace))
            {
                flag = false;
            }
            node.Range.EndPoint = this.prevPosition;
            return flag;
        }

        protected virtual bool ParseAttributeDeclaration()
        {
            string str;
            if (this.Token == 0x30)
            {
                return true;
            }
            Point tokenPosition = this.TokenPosition;
            bool flag = true;
            if (!this.ParseQualifiedIdentifier(out str))
            {
                return false;
            }
            ISyntaxNode node = new SyntaxNode(tokenPosition, str, 30);
            this.AddNode(node);
            if (this.Token != 0x2b)
            {
                goto Label_00CA;
            }
            ISyntaxNode node2 = new SyntaxNode(this.TokenPosition, string.Empty, 0x1b);
            node.AddChild(node2);
            this.SyntaxTree.Push(node2);
            try
            {
                this.MoveNext();
                while (!this.Eof && (this.Token != 0x2c))
                {
                    flag = this.ParseNamedOrExpressionParam();
                    node2.Range.EndPoint = this.prevPosition;
                    if (this.Token != 0x30)
                    {
                        goto Label_00BE;
                    }
                    this.MoveNext();
                }
            }
            finally
            {
                this.SyntaxTree.Pop();
            }
        Label_00BE:
            if (!this.Expected(CLexerToken.Close_parens))
            {
                flag = false;
            }
        Label_00CA:
            node.Range.EndPoint = this.prevPosition;
            return flag;
        }

        protected virtual bool ParseAttributeListDeclaration(Point position)
        {
            return this.ParseAttributeListDeclaration(position, 0x2b, 0x2c);
        }

        protected virtual bool ParseAttributeListDeclaration(Point position, int startToken, int endToken)
        {
            bool flag = true;
            this.MoveNext();
            if (!this.Expected(startToken) || !this.Expected(startToken))
            {
                this.SyntaxError(startToken);
                return false;
            }
            ISyntaxNode node = new SyntaxNode(position, string.Empty, 0x1f, SyntaxNodeOptions.Indentation);
            this.AddNode(node);
            this.SyntaxTree.Push(node);
            try
            {
                while (!this.Eof && (this.Token != endToken))
                {
                    if (!this.ParseAttributeDeclaration())
                    {
                        flag = false;
                    }
                    if (this.Token != 0x30)
                    {
                        goto Label_0080;
                    }
                    this.MoveNext();
                }
            }
            finally
            {
                this.SyntaxTree.Pop();
            }
        Label_0080:
            if (!this.Expected(endToken) || !this.Expected(endToken))
            {
                flag = false;
            }
            node.Range.EndPoint = this.prevPosition;
            return flag;
        }

        protected virtual bool ParseBaseList()
        {
            string str;
            this.MoveNext();
            bool flag = true;
            Point tokenPosition = this.TokenPosition;
            if (this.ParseBaseList(out str))
            {
                this.AddAttribute(new SyntaxAttribute(tokenPosition, NetNodeType.TypeList.ToString(), str));
                return flag;
            }
            return false;
        }

        protected virtual bool ParseBaseList(out string types)
        {
            bool flag = true;
            if (this.ParseType(out types))
            {
                while (this.Token == 0x30)
                {
                    string str;
                    types = types + this.TokenString;
                    this.MoveNext();
                    if (this.ParseType(out str))
                    {
                        types = types + str;
                    }
                    else
                    {
                        return false;
                    }
                }
                return flag;
            }
            return false;
        }

        protected virtual bool ParseBlock()
        {
            bool flag = true;
            Point tokenPosition = this.TokenPosition;
            if (this.Expected(CLexerToken.Open_brace))
            {
                this.AddAttribute(new SyntaxAttribute(tokenPosition, SyntaxConsts.BlockScope, null));
                this.AddAttribute(new SyntaxAttribute(tokenPosition, SyntaxConsts.DefinitionScope, null));
                if ((this.Token != 40) && !this.ParseStatementList())
                {
                    flag = false;
                }
                if (this.Expected(CLexerToken.Close_brace))
                {
                    this.AddAttribute(new SyntaxAttribute(this.prevPosition, SyntaxConsts.DefinitionScopeEnd, null));
                    return flag;
                }
                return false;
            }
            return false;
        }

        protected virtual bool ParseBlockStatement()
        {
            return this.ParseBlockStatement(null, SyntaxNodeOptions.None);
        }

        protected virtual bool ParseBlockStatement(ISyntaxAttributes attrs, SyntaxNodeOptions options)
        {
            bool flag = false;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, string.Empty, 0x6a, options);
            this.AddNode(node);
            if (attrs != null)
            {
                node.AddAttributes(attrs);
            }
            this.SyntaxTree.Push(node);
            try
            {
                flag = this.ParseBlock();
            }
            finally
            {
                this.SyntaxTree.Pop();
            }
            node.Range.EndPoint = this.prevPosition;
            ISyntaxAttribute attr = node.FindAttribute(SyntaxConsts.DefinitionScope);
            if (attr != null)
            {
                this.AddAttribute(attr);
            }
            attr = node.FindAttribute(SyntaxConsts.DefinitionScopeEnd);
            if (attr != null)
            {
                this.AddAttribute(attr);
            }
            return flag;
        }

        protected virtual bool ParseBreakStatement()
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, this.TokenString, 0x5b);
            this.AddNode(node);
            this.MoveNext();
            if (!this.Expected(CLexerToken.Semicolon))
            {
                flag = false;
            }
            node.Range.EndPoint = this.prevPosition;
            return flag;
        }

        protected virtual bool ParseCastTargetExpression(ref ISyntaxNode node)
        {
            bool flag = true;
            node = this.CreateExpressionNode(this.TokenPosition, string.Empty, 0x98, node, false);
            ISyntaxNode node2 = null;
            if (this.Token == 0x27)
            {
                this.ParseStructInitializer(ref node2);
            }
            else if (!this.ParseUnaryExpression(ref node2))
            {
                flag = false;
            }
            if (node2 != null)
            {
                node.AddChild(node2);
            }
            node.Range.EndPoint = this.prevPosition;
            return flag;
        }

        protected virtual bool ParseClassBody()
        {
            bool flag = true;
            bool flag2 = this.SyntaxTree.Current != this.SyntaxTree.Root;
            ISyntaxAttributes attrs = null;
            while (!this.Eof)
            {
                if (flag2 && (this.Token == 40))
                {
                    return flag;
                }
                this.ParseModifiers(ref attrs);
                CLexerToken token = (CLexerToken) this.Token;
                if (token <= CLexerToken.Enum)
                {
                    switch (token)
                    {
                        case CLexerToken.Class:
                        case CLexerToken.Enum:
                            goto Label_006D;
                    }
                    goto Label_0098;
                }
                switch (token)
                {
                    case CLexerToken.Struct:
                    case CLexerToken.Union:
                        break;

                    default:
                    {
                        if (token != CLexerToken.Directive_Literal)
                        {
                            goto Label_0098;
                        }
                        if (!this.ParseDirective())
                        {
                            flag = false;
                        }
                        continue;
                    }
                }
            Label_006D:
                if (this.IsDeclaration())
                {
                    if (!this.ParseKnownMemberDeclaration(attrs))
                    {
                        flag = false;
                    }
                }
                else
                {
                    this.MoveNext();
                    if (!this.ParseUnknownMemberDeclaration(attrs))
                    {
                        flag = false;
                    }
                }
                attrs = null;
                continue;
            Label_0098:
                if (!this.ParseUnknownMemberDeclaration(attrs))
                {
                    flag = false;
                }
                attrs = null;
            }
            return flag;
        }

        protected virtual bool ParseComment()
        {
            bool flag = this.TokenString.StartsWith("/*");
            ISyntaxNode item = new SyntaxNode(this.TokenPosition, flag ? SyntaxParserConsts.OutlineCommentText : string.Empty, 0x33, ((this.SyntaxTree.Current == this.SyntaxTree.Root) || (this.State == 1)) ? SyntaxNodeOptions.KeepIndentation : SyntaxNodeOptions.None);
            this.comments.Add(item);
            if (flag)
            {
                item.AddAttribute(new SyntaxAttribute(item.Position, SyntaxConsts.OutlineText, item.Name));
            }
            Point currentPosition = this.CurrentPosition;
            while (this.IsComment(this.Token) || (this.Token == 0x69))
            {
                if (this.Token != 0x69)
                {
                    currentPosition = this.CurrentPosition;
                    if (currentPosition.X > base.source.Length)
                    {
                        currentPosition.X = base.source.Length;
                    }
                }
                this.NextToken();
            }
            item.Range.EndPoint = currentPosition;
            return true;
        }

        protected virtual bool ParseConditionalAndExpression(ref ISyntaxNode node)
        {
            bool flag = this.ParseInclusiveOrExpression(ref node);
            if (this.Token == 0x54)
            {
                node = this.CreateExpressionNode(this.TokenPosition, this.TokenString, 0x88, node, true);
                this.MoveNext();
                ISyntaxNode node2 = null;
                if (!this.ParseConditionalAndExpression(ref node2))
                {
                    flag = false;
                }
                if (node2 != null)
                {
                    node.AddChild(node2);
                }
                node.Range.EndPoint = this.prevPosition;
            }
            return flag;
        }

        protected virtual bool ParseConditionalExpression(ref ISyntaxNode node)
        {
            bool flag = this.ParseConditionalOrExpression(ref node);
            if (this.Token == 0x45)
            {
                node = this.CreateExpressionNode(this.TokenPosition, this.TokenString, 0x85, node, true);
                this.MoveNext();
                ISyntaxNode node2 = null;
                if (!this.ParseExpression(ref node2))
                {
                    flag = false;
                }
                if (node2 != null)
                {
                    node.AddChild(node2);
                }
                if (this.Expected(CLexerToken.Colon))
                {
                    node2 = null;
                    if (!this.ParseExpression(ref node2))
                    {
                        flag = false;
                    }
                    if (node2 != null)
                    {
                        node.AddChild(node2);
                    }
                }
                else
                {
                    flag = false;
                }
                node.Range.EndPoint = this.prevPosition;
            }
            return flag;
        }

        protected virtual bool ParseConditionalOrExpression(ref ISyntaxNode node)
        {
            bool flag = this.ParseConditionalAndExpression(ref node);
            if (this.Token == 0x55)
            {
                node = this.CreateExpressionNode(this.TokenPosition, this.TokenString, 0x85, node, true);
                this.MoveNext();
                ISyntaxNode node2 = null;
                if (!this.ParseConditionalOrExpression(ref node2))
                {
                    flag = false;
                }
                if (node2 != null)
                {
                    node.AddChild(node2);
                }
                node.Range.EndPoint = this.prevPosition;
            }
            return flag;
        }

        protected virtual bool ParseConstantExpression(ref ISyntaxNode node)
        {
            return this.ParseExpression(ref node);
        }

        protected virtual bool ParseContinueStatement()
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, this.TokenString, 0x5c);
            this.AddNode(node);
            this.MoveNext();
            if (!this.Expected(CLexerToken.Semicolon))
            {
                flag = false;
            }
            node.Range.EndPoint = this.prevPosition;
            return flag;
        }

        protected virtual bool ParseDeclaration(ISyntaxNode node)
        {
            ISyntaxAttributes attrs = null;
            this.ParseModifiers(ref attrs);
            CLexerToken token = (CLexerToken) this.Token;
            if (((token != CLexerToken.Class) && (token != CLexerToken.Enum)) && (token != CLexerToken.Struct))
            {
                return false;
            }
            return this.ParseDeclaration(attrs, node, node.NodeType);
        }

        protected virtual bool ParseDeclaration(ISyntaxAttributes attrs, int nodeType)
        {
            return this.ParseDeclaration(attrs, null, nodeType);
        }

        protected virtual bool ParseDeclaration(ISyntaxAttributes attrs, ISyntaxNode node, int nodeType)
        {
            string str;
            bool flag = true;
            bool flag2 = node == null;
            bool flag3 = ((this.Token == 0x1b) || (this.Token == 30)) || (this.Token == 11);
            if (flag2)
            {
                node = new SyntaxNode(((attrs != null) && (attrs.Count > 0)) ? attrs[0].Position : this.TokenPosition, string.Empty, nodeType);
            }
            if (attrs != null)
            {
                node.AddAttributes(attrs);
            }
            this.MoveNext();
            Point tokenPosition = this.TokenPosition;
            if (((this.Token == 0x66) || !flag3) && this.ParseIdentifier(out str))
            {
                node.Name = str;
                node.AddAttribute(new SyntaxAttribute(tokenPosition, NetNodeType.Name.ToString(), str));
            }
            node.Options |= SyntaxNodeOptions.CodeCompletion;
            if (flag2)
            {
                this.AddNode(node);
            }
            if (!this.BeforeDeclaration(node))
            {
                flag = false;
            }
            this.SyntaxTree.Push(node);
            try
            {
                if (this.IsBaseList(this.Token) && !this.ParseBaseList())
                {
                    flag = false;
                }
            }
            finally
            {
                this.SyntaxTree.Pop();
            }
            node.AddAttribute(new SyntaxAttribute(this.prevPosition, SyntaxConsts.DeclarationScope, null));
            if (!this.ParseDeclarationBody(node, nodeType))
            {
                flag = false;
            }
            if ((this.Token == 0x23) && !this.ParseAttributeListDeclaration(this.TokenPosition))
            {
                flag = false;
            }
            if ((flag3 && (node.Name == string.Empty)) && this.ParseIdentifier(out str))
            {
                node.Name = str;
            }
            if (!this.AfterDeclaration(node))
            {
                flag = false;
            }
            node.Range.EndPoint = this.prevPosition;
            return flag;
        }

        protected virtual bool ParseDeclarationBody(ISyntaxNode node, int nodeType)
        {
            bool flag = true;
            Point tokenPosition = this.TokenPosition;
            this.SyntaxTree.Push(node);
            bool flag2 = false;
            try
            {
                flag2 = this.SkipToDeclarationStart(nodeType);
                if (flag2)
                {
                    node.AddAttribute(new SyntaxAttribute(tokenPosition, SyntaxConsts.DefinitionScope, null));
                    node.Options |= SyntaxNodeOptions.Outlining | SyntaxNodeOptions.Indentation;
                    this.MoveNext();
                    switch (node.NodeType)
                    {
                        case 8:
                        case 9:
                            if (!this.ParseClassBody())
                            {
                                flag = false;
                            }
                            goto Label_0095;

                        case 11:
                            if (!this.ParseEnumBody())
                            {
                                flag = false;
                            }
                            goto Label_0095;
                    }
                    flag = false;
                }
            }
            finally
            {
                this.SyntaxTree.Pop();
            }
        Label_0095:
            if (flag2 && this.ParseDeclarationBodyEnd(node))
            {
                node.AddAttribute(new SyntaxAttribute(this.prevPosition, SyntaxConsts.DefinitionScopeEnd, null));
                if (this.Token == 0x34)
                {
                    this.MoveNext();
                }
                return flag;
            }
            return false;
        }

        protected virtual bool ParseDeclarationBodyEnd(ISyntaxNode node)
        {
            return this.Expected(CLexerToken.Close_brace);
        }

        protected virtual bool ParseDirective()
        {
            bool flag = true;
            string name = string.Empty;
            while (this.Token == 0x68)
            {
                name = this.TokenString.Trim();
                if (name.StartsWith("#"))
                {
                    name = name.Substring(1);
                }
                switch (name)
                {
                    case "define":
                    case "defined":
                    {
                        int lineIndex = base.lineIndex;
                        string source = base.source;
                        this.MoveNext();
                        ISyntaxNode node = null;
                        if (!this.ParseExpression(ref node))
                        {
                            flag = false;
                        }
                        if (node != null)
                        {
                            this.AddNode(node);
                        }
                        if (this.Token != 0x68)
                        {
                            while (!this.Eof)
                            {
                                if (base.lineIndex != lineIndex)
                                {
                                    if (!(source.TrimEnd(new char[0]) != string.Empty) || (source[source.TrimEnd(new char[0]).Length - 1] != '\\'))
                                    {
                                        break;
                                    }
                                    lineIndex = base.lineIndex;
                                }
                                if ((node != null) && this.IsValidToken(this.Token))
                                {
                                    node.AddAttribute(new SyntaxAttribute(this.TokenPosition, name, this.TokenString));
                                }
                                source = base.source;
                                this.NextToken();
                            }
                        }
                        continue;
                    }
                    case "region":
                    case "if":
                    case "ifdef":
                    case "ifndef":
                    {
                        ISyntaxNode node2 = new SyntaxNode(this.TokenPosition, this.TokenString, (name == "region") ? 40 : 0x29);
                        if (this.State == 2)
                        {
                            this.MoveNext();
                        }
                        node2.Name = this.TokenString.Trim();
                        this.AddNode(node2);
                        this.SyntaxTree.Push(node2);
                        if (this.Token != 0x68)
                        {
                            ISyntaxNode node3 = null;
                            if (!this.ParseExpression(ref node3))
                            {
                                flag = false;
                            }
                            if (node3 != null)
                            {
                                this.AddNode(node3);
                            }
                        }
                        continue;
                    }
                    case "endregion":
                    case "endif":
                    case "elif":
                    {
                        Point point2 = new Point(base.source.Length, base.lineIndex);
                        if (this.State == 2)
                        {
                            this.MoveNext();
                        }
                        this.MoveNext();
                        ISyntaxNode current = this.SyntaxTree.Current;
                        if (((name == "endregion") && (current.NodeType == 40)) || ((name == "endif") && (current.NodeType == 0x29)))
                        {
                            this.SyntaxTree.Pop();
                            current.Range.EndPoint = point2;
                            current.AddAttribute(new SyntaxAttribute(current.Position, SyntaxConsts.OutlineText, current.Name));
                            current.Options = SyntaxNodeOptions.Outlining;
                        }
                        continue;
                    }
                }
                this.MoveNext();
            }
            return flag;
        }

        protected virtual bool ParseDoStatement()
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, this.TokenString, 0x48, SyntaxNodeOptions.Indentation);
            this.AddNode(node);
            this.SyntaxTree.Push(node);
            try
            {
                this.MoveNext();
                if (!this.ParseEmbeddedStatement())
                {
                    flag = false;
                }
                if (!this.ParseDoWhileStatement())
                {
                    flag = false;
                }
            }
            finally
            {
                this.SyntaxTree.Pop();
            }
            node.Range.EndPoint = this.prevPosition;
            return flag;
        }

        protected virtual bool ParseDoWhileStatement()
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, this.TokenString, 0x49, SyntaxNodeOptions.BackIndentation);
            this.AddNode(node);
            this.MoveNext();
            if (this.Token == 0x2b)
            {
                this.MoveNext();
                ISyntaxNode node2 = null;
                if (!this.ParseExpression(ref node2))
                {
                    flag = false;
                }
                if (node2 != null)
                {
                    node.AddChild(node2);
                }
                if (!this.Expected(CLexerToken.Close_parens) || !this.Expected(CLexerToken.Semicolon))
                {
                    flag = false;
                }
            }
            else
            {
                this.SyntaxError(0x2b);
                flag = false;
            }
            node.Range.EndPoint = this.prevPosition;
            return flag;
        }

        protected virtual bool ParseElementAccess(ref ISyntaxNode node)
        {
            bool flag = true;
            node = this.CreateExpressionNode(this.TokenPosition, this.TokenString, 0xa2, node, false);
            ISyntaxNode node2 = null;
            flag = this.ParseArgumentExpressionList(ref node2);
            if (node2 != null)
            {
                node.AddChild(node2);
            }
            if (!this.Expected(CLexerToken.Close_bracket))
            {
                flag = false;
            }
            node.Range.EndPoint = this.prevPosition;
            return flag;
        }

        protected virtual bool ParseElseStatement()
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, this.TokenString, 0x43, (this.SyntaxTree.Current.NodeType == 0x42) ? SyntaxNodeOptions.BackIndentation : SyntaxNodeOptions.None);
            this.AddNode(node);
            this.SyntaxTree.Push(node);
            try
            {
                this.MoveNext();
                if (!this.ParseEmbeddedStatement())
                {
                    flag = false;
                }
            }
            finally
            {
                this.SyntaxTree.Pop();
            }
            node.Range.EndPoint = this.prevPosition;
            return flag;
        }

        protected virtual bool ParseEmbeddedStatement()
        {
            switch (((CLexerToken) this.Token))
            {
                case CLexerToken.For:
                    return this.ParseForStatement();

                case CLexerToken.Goto:
                    return this.ParseGotoStatement();

                case CLexerToken.If:
                    return this.ParseIfStatement();

                case CLexerToken.Return:
                    return this.ParseReturnStatement();

                case CLexerToken.Continue:
                    return this.ParseContinueStatement();

                case CLexerToken.Do:
                    return this.ParseDoStatement();

                case CLexerToken.Break:
                    return this.ParseBreakStatement();

                case CLexerToken.Switch:
                    return this.ParseSwitchStatement();

                case CLexerToken.While:
                    return this.ParseWhileStatement();

                case CLexerToken.Open_brace:
                    return this.ParseBlockStatement();

                case CLexerToken.Directive_Literal:
                    return this.ParseDirective();
            }
            return this.ParseExpressionStatement();
        }

        protected virtual bool ParseEnumBody()
        {
            bool flag = true;
            while (!this.Eof && (this.Token != 40))
            {
                if (!this.ParseEnumMember())
                {
                    flag = false;
                }
            }
            return flag;
        }

        protected virtual bool ParseEnumMember()
        {
            string str;
            bool flag = true;
            Point tokenPosition = this.TokenPosition;
            flag = this.ParseIdentifier(out str);
            if (flag)
            {
                ISyntaxNode node = new SyntaxNode(tokenPosition, str, 13, SyntaxNodeOptions.CodeCompletion);
                this.AddNode(node);
                if (this.Token == 0x38)
                {
                    this.MoveNext();
                    ISyntaxNode node2 = null;
                    if (!this.ParseExpression(ref node2))
                    {
                        flag = false;
                    }
                    if (node2 != null)
                    {
                        node.AddChild(node2);
                    }
                }
                if (this.Token == 0x30)
                {
                    this.MoveNext();
                }
                node.Range.EndPoint = this.prevPosition;
                return flag;
            }
            this.MoveNext();
            return flag;
        }

        protected virtual bool ParseEqualityExpression(ref ISyntaxNode node)
        {
            bool flag = this.ParseRelationalExpression(ref node);
            switch (this.Token)
            {
                case 0x52:
                case 0x53:
                {
                    node = this.CreateExpressionNode(this.TokenPosition, this.TokenString, 0x8d, node, true);
                    this.MoveNext();
                    ISyntaxNode node2 = null;
                    if (!this.ParseEqualityExpression(ref node2))
                    {
                        flag = false;
                    }
                    if (node2 != null)
                    {
                        node.AddChild(node2);
                    }
                    node.Range.EndPoint = this.prevPosition;
                    return flag;
                }
            }
            return flag;
        }

        protected virtual bool ParseExclusiveOrExpression(ref ISyntaxNode node)
        {
            bool flag = this.ParseAndExpression(ref node);
            if (this.Token == 0x3a)
            {
                node = this.CreateExpressionNode(this.TokenPosition, this.TokenString, 0x8a, node, true);
                this.MoveNext();
                ISyntaxNode node2 = null;
                if (!this.ParseExclusiveOrExpression(ref node2))
                {
                    flag = false;
                }
                if (node2 != null)
                {
                    node.AddChild(node2);
                }
                node.Range.EndPoint = this.prevPosition;
            }
            return flag;
        }

        protected virtual bool ParseExpression(ref ISyntaxNode node)
        {
            bool flag = this.ParseConditionalExpression(ref node);
            if (!this.TryParseAssignmentExpression(ref node))
            {
                flag = false;
            }
            return flag;
        }

        protected virtual bool ParseExpressionStatement()
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, string.Empty, 0x6b, SyntaxNodeOptions.Indentation);
            this.AddNode(node);
            if (this.Token == 0x34)
            {
                node.Name = this.TokenString;
                node.NodeType = 130;
                this.MoveNext();
            }
            else
            {
                this.SyntaxTree.Push(node);
                try
                {
                    flag = this.ParseStatementExpression();
                    if ((this.Token == 0x34) || (this.Token == 0x30))
                    {
                        this.MoveNext();
                    }
                    else
                    {
                        this.SyntaxError(this.Token);
                    }
                }
                finally
                {
                    this.SyntaxTree.Pop();
                }
            }
            node.Range.EndPoint = this.prevPosition;
            return flag;
        }

        protected virtual bool ParseFieldDeclaration(ISyntaxAttributes attrs, string type, Point typePos, string name)
        {
            bool flag = true;
            while (!this.Eof && (this.Token == 0x29))
            {
                string str;
                this.ParseRankSpecifier(out str);
                if (str != string.Empty)
                {
                    type = type + str;
                }
            }
            ISyntaxNode node = new SyntaxNode(((attrs != null) && (attrs.Count > 0)) ? attrs[0].Position : typePos, name, 13, SyntaxNodeOptions.CodeCompletion | SyntaxNodeOptions.Indentation);
            if (type == string.Empty)
            {
                this.TryFindNodeType(ref type);
            }
            if (type != string.Empty)
            {
                node.AddAttribute(new SyntaxAttribute(typePos, NetNodeType.Type.ToString(), type));
            }
            if (attrs != null)
            {
                node.AddAttributes(attrs);
            }
            this.AddNode(node);
            if (!this.BeforeDeclaration(node))
            {
                flag = false;
            }
            this.SyntaxTree.Push(node);
            try
            {
                if (!this.ParseVariableDeclarators(NetNodeType.Field))
                {
                    flag = false;
                }
            }
            finally
            {
                this.SyntaxTree.Pop();
            }
            if (!this.Expected(CLexerToken.Semicolon))
            {
                flag = false;
            }
            if (!this.AfterDeclaration(node))
            {
                flag = false;
            }
            node.Range.EndPoint = this.prevPosition;
            return flag;
        }

        protected virtual bool ParseFieldInitializerExpression(ref ISyntaxNode node)
        {
            bool flag = true;
            node = new SyntaxNode(this.TokenPosition, this.TokenString, 0x6a);
            this.SyntaxTree.Push(node);
            try
            {
                this.MoveNext();
                while (!this.Eof && (this.Token != 40))
                {
                    ISyntaxNode node2 = null;
                    flag = this.ParseStructInitializer(ref node2);
                    if (node2 != null)
                    {
                        node.AddChild(node2);
                    }
                    if (this.Token != 0x30)
                    {
                        goto Label_0076;
                    }
                    this.MoveNext();
                }
            }
            finally
            {
                this.SyntaxTree.Pop();
            }
        Label_0076:
            if (!this.Expected(CLexerToken.Close_brace))
            {
                flag = false;
            }
            node.Range.EndPoint = this.prevPosition;
            return flag;
        }

        protected virtual bool ParseForCondition()
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, this.TokenString, 0x4b);
            this.AddNode(node);
            ISyntaxNode node2 = null;
            flag = this.ParseExpression(ref node2);
            if (node2 != null)
            {
                node.AddChild(node2);
            }
            node.Range.EndPoint = this.prevPosition;
            return flag;
        }

        protected virtual bool ParseForInitializer()
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, this.TokenString, 0x4c);
            this.AddNode(node);
            this.SyntaxTree.Push(node);
            try
            {
                this.SaveState();
                try
                {
                    flag = this.ParseVariableDeclaration(this.TokenPosition, NetNodeType.LocalVariable);
                }
                finally
                {
                    this.RestoreState(!flag);
                }
                if (!flag)
                {
                    flag = this.ParseStatementExpressionList();
                }
            }
            finally
            {
                this.SyntaxTree.Pop();
            }
            node.Range.EndPoint = this.prevPosition;
            return flag;
        }

        protected virtual bool ParseForIterator()
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, this.TokenString, 0x4d);
            this.AddNode(node);
            this.SyntaxTree.Push(node);
            try
            {
                flag = this.ParseStatementExpressionList();
            }
            finally
            {
                this.SyntaxTree.Pop();
            }
            node.Range.EndPoint = this.prevPosition;
            return flag;
        }

        protected virtual bool ParseForStatement()
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, this.TokenString, 0x4a, SyntaxNodeOptions.Indentation);
            this.AddNode(node);
            this.SyntaxTree.Push(node);
            try
            {
                this.MoveNext();
                if (this.Expected(CLexerToken.Open_parens))
                {
                    if ((this.Token != 0x34) && !this.ParseForInitializer())
                    {
                        flag = false;
                    }
                    if (this.Expected(CLexerToken.Semicolon))
                    {
                        if ((this.Token != 0x34) && !this.ParseForCondition())
                        {
                            flag = false;
                        }
                    }
                    else
                    {
                        flag = false;
                    }
                    if (this.Expected(CLexerToken.Semicolon))
                    {
                        if ((this.Token != 0x2c) && !this.ParseForIterator())
                        {
                            flag = false;
                        }
                    }
                    else
                    {
                        flag = false;
                    }
                    if (!this.Expected(CLexerToken.Close_parens))
                    {
                        flag = false;
                    }
                }
                else
                {
                    flag = false;
                }
                if (!this.ParseEmbeddedStatement())
                {
                    flag = false;
                }
            }
            finally
            {
                this.SyntaxTree.Pop();
            }
            node.Range.EndPoint = this.prevPosition;
            return flag;
        }

        protected virtual bool ParseGotoStatement()
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, this.TokenString, 0x5d);
            this.AddNode(node);
            this.MoveNext();
            if (this.Token == 0x66)
            {
                this.MoveNext();
            }
            else
            {
                flag = false;
            }
            if (!this.Expected(CLexerToken.Semicolon))
            {
                flag = false;
            }
            node.Range.EndPoint = this.prevPosition;
            return flag;
        }

        protected virtual bool ParseIdentifier(out string identifier)
        {
            identifier = this.TokenString;
            return this.IdentifierExpected();
        }

        protected virtual bool ParseIfStatement()
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, this.TokenString, 0x42, (this.SyntaxTree.Current.NodeType != 0x43) ? SyntaxNodeOptions.Indentation : (SyntaxNodeOptions.BackIndentation | SyntaxNodeOptions.Indentation));
            this.AddNode(node);
            this.SyntaxTree.Push(node);
            try
            {
                this.MoveNext();
                ISyntaxNode node2 = null;
                if (!this.ParseParenthesizedStatementExpression(ref node2))
                {
                    flag = false;
                }
                if (node2 != null)
                {
                    node.AddChild(node2);
                }
                if (!this.ParseEmbeddedStatement())
                {
                    flag = false;
                }
                if ((this.Token == 10) && !this.ParseElseStatement())
                {
                    flag = false;
                }
            }
            finally
            {
                this.SyntaxTree.Pop();
            }
            node.Range.EndPoint = this.prevPosition;
            return flag;
        }

        protected virtual bool ParseInclusiveOrExpression(ref ISyntaxNode node)
        {
            bool flag = this.ParseExclusiveOrExpression(ref node);
            if (this.Token == 0x3e)
            {
                node = this.CreateExpressionNode(this.TokenPosition, this.TokenString, 0x89, node, true);
                this.MoveNext();
                ISyntaxNode node2 = null;
                if (!this.ParseInclusiveOrExpression(ref node2))
                {
                    flag = false;
                }
                if (node2 != null)
                {
                    node.AddChild(node2);
                }
                node.Range.EndPoint = this.prevPosition;
            }
            return flag;
        }

        protected virtual bool ParseInvocationExpression(ref ISyntaxNode node)
        {
            bool flag = true;
            node = this.CreateExpressionNode(this.TokenPosition, this.TokenString, 0xa3, node, false);
            ISyntaxNode node2 = null;
            flag = this.ParseArgumentList(ref node2);
            if (node2 != null)
            {
                node.AddChild(node2);
            }
            node.Range.EndPoint = this.prevPosition;
            return flag;
        }

        protected virtual bool ParseKnownMemberDeclaration(ISyntaxAttributes attrs)
        {
            bool flag = true;
            CLexerToken token = (CLexerToken) this.Token;
            if (token <= CLexerToken.Enum)
            {
                switch (token)
                {
                    case CLexerToken.Class:
                        if (!this.ParseDeclaration(attrs, 8))
                        {
                            flag = false;
                        }
                        return flag;

                    case CLexerToken.Enum:
                        if (!this.ParseDeclaration(attrs, 11))
                        {
                            flag = false;
                        }
                        return flag;
                }
                return flag;
            }
            switch (token)
            {
                case CLexerToken.Struct:
                case CLexerToken.Union:
                    if (!this.ParseDeclaration(attrs, 9))
                    {
                        flag = false;
                    }
                    return flag;
            }
            return flag;
        }

        protected virtual bool ParseLocalConstantDeclarationStatement()
        {
            Point tokenPosition = this.TokenPosition;
            this.MoveNext();
            return this.ParseVariableDeclaration(tokenPosition, NetNodeType.Constant);
        }

        protected virtual bool ParseMemberAccess(ref ISyntaxNode node)
        {
            return this.ParseMemberAccess(ref node, 0x9f);
        }

        protected virtual bool ParseMemberAccess(ref ISyntaxNode node, int nodeType)
        {
            string str;
            bool flag = true;
            node = this.CreateExpressionNode(this.TokenPosition, string.Empty, nodeType, node, true);
            Point currentPosition = this.CurrentPosition;
            this.MoveNext();
            if (this.ParseIdentifier(out str))
            {
                node.Name = str;
            }
            else
            {
                flag = false;
            }
            node.AddAttribute(new SyntaxAttribute(currentPosition, NetNodeType.Name.ToString(), node.Name));
            node.Range.EndPoint = this.prevPosition;
            return flag;
        }

        protected virtual bool ParseMethodBody()
        {
            if (!this.SkipTo(0x27, 0x34))
            {
                return false;
            }
            bool flag = true;
            ISyntaxNode current = this.SyntaxTree.Current;
            CLexerToken token = (CLexerToken) this.Token;
            if (token != CLexerToken.Open_brace)
            {
                if (token == CLexerToken.Semicolon)
                {
                    current.Range.EndPoint = this.CurrentPosition;
                    current.Options = (current.Options | SyntaxNodeOptions.CodeCompletion) & ~SyntaxNodeOptions.Outlining;
                    this.MoveNext();
                    return flag;
                }
                this.SyntaxError();
                return false;
            }
            current.AddAttribute(new SyntaxAttribute(this.TokenPosition, SyntaxConsts.DefinitionScope, null));
            current.Options |= SyntaxNodeOptions.CodeCompletion | SyntaxNodeOptions.Outlining | SyntaxNodeOptions.Indentation;
            if ((this.Token == 0x27) && !this.ParseBlockStatement())
            {
                flag = false;
            }
            current.Range.EndPoint = this.prevPosition;
            return flag;
        }

        protected virtual bool ParseMethodDeclaration(bool funcPtr, ISyntaxNode node)
        {
            bool flag = true;
            if (!this.BeforeDeclaration(node))
            {
                flag = false;
            }
            this.SyntaxTree.Push(node);
            try
            {
                ISyntaxNode node2 = null;
                if (!this.ParseParameterListDeclaration(funcPtr, ref node2))
                {
                    flag = false;
                }
                if (node2 != null)
                {
                    node.AddChild(node2);
                }
                if ((this.Token == 0x23) && !this.ParseAttributeListDeclaration(this.TokenPosition))
                {
                    flag = false;
                }
                node.AddAttribute(new SyntaxAttribute(this.prevPosition, SyntaxConsts.DeclarationScope, null));
                if (!this.ParseMethodBody())
                {
                    flag = false;
                }
            }
            finally
            {
                this.SyntaxTree.Pop();
            }
            node.Range.EndPoint = this.prevPosition;
            if (!this.AfterDeclaration(node))
            {
                flag = false;
            }
            return flag;
        }

        protected virtual bool ParseMethodDeclaration(ISyntaxAttributes attrs, string type, Point typePos, string name, bool funcPtr, ref ISyntaxNode node)
        {
            node = new SyntaxNode(((attrs != null) && (attrs.Count > 0)) ? attrs[0].Position : typePos, name, 0x11, SyntaxNodeOptions.Outlining);
            if (attrs != null)
            {
                node.AddAttributes(attrs);
            }
            node.AddAttribute(new SyntaxAttribute(typePos, NetNodeType.Type.ToString(), type));
            this.InvalidPreviousMethodDeclaration(node.Name);
            this.AddNode(node);
            return this.ParseMethodDeclaration(funcPtr, node);
        }

        protected virtual bool ParseModifiers(ref ISyntaxAttributes attrs)
        {
            bool flag = false;
            while (this.IsModifier(this.Token))
            {
                flag = true;
                if (attrs == null)
                {
                    attrs = new SyntaxAttributes();
                }
                attrs.Add(new SyntaxAttribute(this.TokenPosition, NetNodeType.Modifier.ToString(), this.TokenString));
                this.MoveNext();
            }
            return flag;
        }

        protected virtual bool ParseMultiplicativeExpression(ref ISyntaxNode node)
        {
            bool flag = this.ParsePrefixedUnaryExpression(ref node);
            switch (this.Token)
            {
                case 0x41:
                case 0x42:
                case 0x43:
                case 0x44:
                {
                    node = this.CreateExpressionNode(this.TokenPosition, this.TokenString, 0x92, node, true);
                    this.MoveNext();
                    ISyntaxNode node2 = null;
                    if (!this.ParseMultiplicativeExpression(ref node2))
                    {
                        flag = false;
                    }
                    if (node2 != null)
                    {
                        node.AddChild(node2);
                    }
                    node.Range.EndPoint = this.prevPosition;
                    return flag;
                }
            }
            return flag;
        }

        protected virtual bool ParseNamedOrExpressionParam()
        {
            if (this.Token == 0x30)
            {
                return true;
            }
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, string.Empty, 0x1a);
            this.AddNode(node);
            if (this.IsIdentifierToken(this.Token))
            {
                node.Name = this.TokenString;
                this.MoveNext();
            }
            else
            {
                ISyntaxNode node2 = null;
                flag = this.ParseExpression(ref node2);
                if (node2 != null)
                {
                    node.AddChild(node2);
                }
            }
            node.Range.EndPoint = this.prevPosition;
            return flag;
        }

        protected virtual bool ParseParameterDeclaration(bool funcPtr)
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, string.Empty, 0x1a, SyntaxNodeOptions.CodeCompletion);
            ISyntaxNode node2 = null;
            Point tokenPosition = this.TokenPosition;
            while (this.IsParameterModifier(this.Token))
            {
                node.AddAttribute(new SyntaxAttribute(this.TokenPosition, NetNodeType.ParameterModifier.ToString(), this.TokenString));
                this.MoveNext();
            }
            if (this.Token == 0x2d)
            {
                node.Name = this.TokenString;
                this.MoveNext();
                this.AddNode(node);
            }
            else
            {
                string str;
                if (this.ParseType(out str))
                {
                    string str2;
                    if (this.IsFunctionPointer(out str2, true, ref node2))
                    {
                        node.Name = str2;
                        this.SyntaxTree.Push(node);
                        try
                        {
                            if (this.Token == 0x2b)
                            {
                                if (!this.ParseParameterListDeclaration(true, ref node2))
                                {
                                    flag = false;
                                }
                            }
                            else
                            {
                                this.Expected(CLexerToken.Open_parens);
                                flag = false;
                            }
                        }
                        finally
                        {
                            this.SyntaxTree.Pop();
                        }
                    }
                    else
                    {
                        if (str != "void")
                        {
                            if (this.Token != 0x66)
                            {
                                str2 = str;
                                str = funcPtr ? string.Empty : this.defaultTypeName;
                            }
                            else
                            {
                                this.ParseIdentifier(out str2);
                            }
                            node.Name = str2;
                        }
                        if (this.Token == 0x2d)
                        {
                            node.Name = node.Name + this.TokenString;
                            this.MoveNext();
                        }
                        if (this.Token == 0x29)
                        {
                            string str3;
                            this.ParseRankSpecifier(out str3);
                            if (str3 != string.Empty)
                            {
                                str = str + str3;
                            }
                        }
                    }
                    node.AddAttribute(new SyntaxAttribute(tokenPosition, NetNodeType.Type.ToString(), str));
                    if (node2 != null)
                    {
                        node.AddChild(node2);
                    }
                    if (this.Token == 0x38)
                    {
                        this.MoveNext();
                        node2 = null;
                        this.ParseConstantExpression(ref node2);
                        if (node2 != null)
                        {
                            node.AddChild(node2);
                        }
                        this.SyntaxError(StringConsts.ErrDefaultParamNotPermitted);
                        flag = false;
                    }
                    this.AddNode(node);
                }
                else
                {
                    flag = false;
                }
            }
            node.Range.EndPoint = this.prevPosition;
            return flag;
        }

        protected virtual bool ParseParameterListDeclaration(bool funcPtr, ref ISyntaxNode node)
        {
            return this.ParseParameterListDeclaration(0x2b, 0x2c, funcPtr, ref node);
        }

        protected virtual bool ParseParameterListDeclaration(int startToken, int endToken, bool funcPtr, ref ISyntaxNode node)
        {
            bool flag = true;
            if (this.Token != startToken)
            {
                this.SyntaxError(startToken);
                return false;
            }
            node = new SyntaxNode(this.TokenPosition, string.Empty, 0x1b);
            this.SyntaxTree.Push(node);
            try
            {
                this.MoveNext();
                while (!this.Eof && (this.Token != endToken))
                {
                    if (!this.ParseParameterDeclaration(funcPtr))
                    {
                        flag = false;
                    }
                    if (this.Token != 0x30)
                    {
                        goto Label_0076;
                    }
                    this.MoveNext();
                }
            }
            finally
            {
                this.SyntaxTree.Pop();
            }
        Label_0076:
            if (!this.Expected(endToken))
            {
                flag = false;
            }
            node.Range.EndPoint = this.prevPosition;
            return flag;
        }

        protected virtual bool ParseParenthesizedExpression(ref ISyntaxNode node)
        {
            bool flag = true;
            if (this.Token == 0x2b)
            {
                node = new SyntaxNode(this.TokenPosition, this.TokenString, 0xa8);
                this.MoveNext();
                ISyntaxNode node2 = null;
                flag = this.ParseExpression(ref node2);
                if (node2 != null)
                {
                    node.AddChild(node2);
                }
                if (!this.Expected(CLexerToken.Close_parens))
                {
                    flag = false;
                }
                node.Range.EndPoint = this.prevPosition;
                return flag;
            }
            this.SyntaxError(0x2b);
            return false;
        }

        protected virtual bool ParseParenthesizedStatementExpression(ref ISyntaxNode node)
        {
            if (this.Token == 0x2b)
            {
                return this.ParseParenthesizedExpression(ref node);
            }
            this.Expected(CLexerToken.Open_parens);
            return this.ParseExpression(ref node);
        }

        protected virtual bool ParsePointerMemberAccess(ref ISyntaxNode node)
        {
            return this.ParseMemberAccess(ref node, 0xa1);
        }

        protected virtual bool ParsePointerType(ref string type)
        {
            type = type + this.TokenString;
            this.MoveNext();
            return true;
        }

        protected virtual bool ParsePostDecrementExpression(ref ISyntaxNode node)
        {
            node = this.CreateExpressionNode(this.TokenPosition, this.TokenString, 170, node, false);
            this.MoveNext();
            return true;
        }

        protected virtual bool ParsePostIncrementExpression(ref ISyntaxNode node)
        {
            node = this.CreateExpressionNode(this.TokenPosition, this.TokenString, 0xa9, node, false);
            this.MoveNext();
            return true;
        }

        protected virtual bool ParsePrefixedUnaryExpression(ref ISyntaxNode node)
        {
            bool flag = true;
            switch (((CLexerToken) this.Token))
            {
                case CLexerToken.Op_inc:
                case CLexerToken.Op_dec:
                case CLexerToken.Star:
                case CLexerToken.Plus:
                case CLexerToken.Minus:
                case CLexerToken.Bang:
                case CLexerToken.Tilda:
                {
                    node = this.CreateExpressionNode(this.TokenPosition, this.TokenString, 0x93, node, true);
                    this.MoveNext();
                    ISyntaxNode node2 = null;
                    if (!this.ParsePrefixedUnaryExpression(ref node2))
                    {
                        flag = false;
                    }
                    if (node2 != null)
                    {
                        node.AddChild(node2);
                    }
                    return flag;
                }
            }
            return this.ParseUnaryExpression(ref node);
        }

        protected virtual bool ParsePrimaryExpression(ref ISyntaxNode node)
        {
            bool flag = true;
            flag = this.ParseSimpleExpression(ref node);
            if ((node != null) && !this.TryParsePostPrimaryExpression(ref node))
            {
                flag = false;
            }
            return flag;
        }

        protected virtual bool ParseQualifiedIdentifier(out string identifier)
        {
            bool flag = this.ParseTypeIdentifier(out identifier, false);
            if (flag)
            {
                while ((this.Token == 0x2f) || (this.Token == 0x33))
                {
                    string str;
                    identifier = identifier + this.TokenString;
                    this.MoveNext();
                    if (this.ParseTypeIdentifier(out str, false))
                    {
                        identifier = identifier + str;
                    }
                    else
                    {
                        flag = false;
                    }
                }
            }
            return flag;
        }

        protected virtual bool ParseRankSpecifier(out string rank)
        {
            bool flag = true;
            rank = string.Empty;
            if (!this.Expected(CLexerToken.Open_bracket))
            {
                return flag;
            }
            rank = "[";
            if (this.Token != 0x2a)
            {
                while (!this.Eof)
                {
                    ISyntaxNode node = null;
                    if (((this.Token == 5) || (this.Token == 0x15)) || (this.Token == 0x1a))
                    {
                        rank = rank + this.TokenString + " ";
                        this.MoveNext();
                        if (this.Token != 0x2a)
                        {
                            continue;
                        }
                        break;
                    }
                    if (!this.ParseExpression(ref node))
                    {
                        break;
                    }
                    if (node != null)
                    {
                        rank = rank + node.Name;
                    }
                    if (this.Token != 0x30)
                    {
                        break;
                    }
                    this.MoveNext();
                }
            }
            if (this.Token == 0x2a)
            {
                rank = rank + this.TokenString;
            }
            return this.Expected(CLexerToken.Close_bracket);
        }

        protected virtual bool ParseRelationalExpression(ref ISyntaxNode node)
        {
            bool flag = this.ParseShiftExpression(ref node);
            switch (((CLexerToken) this.Token))
            {
                case CLexerToken.Op_lt:
                case CLexerToken.Op_gt:
                case CLexerToken.Op_le:
                case CLexerToken.Op_ge:
                {
                    node = this.CreateExpressionNode(this.TokenPosition, this.TokenString, 0x8e, node, true);
                    this.MoveNext();
                    ISyntaxNode node2 = null;
                    if (!this.ParseRelationalExpression(ref node2))
                    {
                        flag = false;
                    }
                    if (node2 != null)
                    {
                        node.AddChild(node2);
                    }
                    node.Range.EndPoint = this.prevPosition;
                    return flag;
                }
            }
            return flag;
        }

        protected virtual bool ParseReturnStatement()
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, this.TokenString, 0x5e, SyntaxNodeOptions.Indentation);
            this.AddNode(node);
            this.MoveNext();
            if (this.Token != 0x34)
            {
                ISyntaxNode node2 = null;
                if (!this.ParseExpression(ref node2))
                {
                    flag = false;
                }
                if (node2 != null)
                {
                    node.AddChild(node2);
                }
            }
            if (!this.Expected(CLexerToken.Semicolon))
            {
                flag = false;
            }
            node.Range.EndPoint = this.prevPosition;
            return flag;
        }

        protected virtual bool ParseShiftExpression(ref ISyntaxNode node)
        {
            bool flag = this.ParseAdditiveExpression(ref node);
            switch (this.Token)
            {
                case 0x4e:
                case 0x4f:
                {
                    node = this.CreateExpressionNode(this.TokenPosition, this.TokenString, 0x90, node, true);
                    this.MoveNext();
                    ISyntaxNode node2 = null;
                    if (!this.ParseShiftExpression(ref node2))
                    {
                        flag = false;
                    }
                    if (node2 != null)
                    {
                        node.AddChild(node2);
                    }
                    node.Range.EndPoint = this.prevPosition;
                    return flag;
                }
            }
            return flag;
        }

        protected virtual bool ParseSimpleExpression(ref ISyntaxNode node)
        {
            bool flag = true;
            switch (((CLexerToken) this.Token))
            {
                case CLexerToken.Int:
                case CLexerToken.Long:
                case CLexerToken.Short:
                case CLexerToken.Signed:
                case CLexerToken.Float:
                case CLexerToken.Char:
                case CLexerToken.Double:
                case CLexerToken.Unsigned:
                case CLexerToken.Void:
                case CLexerToken.Bool:
                case CLexerToken.Complex:
                case CLexerToken.Imaginary:
                    node = new SyntaxNode(this.TokenPosition, this.TokenString, 0x99);
                    this.MoveNext();
                    if (this.Token != 0x2f)
                    {
                        flag = false;
                    }
                    return flag;

                case CLexerToken.Sizeof:
                    return this.ParseSizeOfExpression(ref node);

                case CLexerToken.Open_parens:
                {
                    bool flag2 = false;
                    ISyntaxNode node3 = null;
                    Point tokenPosition = this.TokenPosition;
                    string identifier = string.Empty;
                    this.SaveState();
                    try
                    {
                        flag2 = this.IsFunctionPointer(out identifier, false, ref node3);
                    }
                    finally
                    {
                        this.RestoreState(!flag2);
                    }
                    if (flag2)
                    {
                        node = new SyntaxNode(tokenPosition, identifier, 0x99);
                        if (node3 != null)
                        {
                            node.AddChild(node3);
                        }
                        return flag;
                    }
                    return this.ParseParenthesizedExpression(ref node);
                }
                case CLexerToken.Integer_Literal:
                case CLexerToken.Float_Literal:
                case CLexerToken.Double_Literal:
                case CLexerToken.Character_Literal:
                case CLexerToken.String_Literal:
                case CLexerToken.Identifier_Literal:
                case CLexerToken.Directive_Literal:
                    node = new SyntaxNode(this.TokenPosition, this.TokenString, 0x99);
                    this.MoveNext();
                    return flag;

                case CLexerToken.Bitwise_and:
                {
                    node = new SyntaxNode(this.TokenPosition, this.TokenString, 0xc1);
                    this.MoveNext();
                    ISyntaxNode node2 = null;
                    flag = this.ParseExpression(ref node2);
                    if (node2 != null)
                    {
                        node.AddChild(node2);
                    }
                    return flag;
                }
            }
            flag = false;
            this.SyntaxError();
            this.MoveNext();
            return flag;
        }

        protected virtual bool ParseSizeOfExpression(ref ISyntaxNode node)
        {
            bool flag = true;
            node = new SyntaxNode(this.TokenPosition, this.TokenString, 0xa7);
            this.MoveNext();
            if (this.Expected(CLexerToken.Open_parens))
            {
                string str;
                Point tokenPosition = this.TokenPosition;
                while ((this.Token == 0x1b) || (this.Token == 0x1d))
                {
                    node.AddAttribute(new SyntaxAttribute(this.TokenPosition, NetNodeType.Modifier.ToString(), this.TokenString));
                    this.MoveNext();
                }
                if (this.ParseType(out str))
                {
                    node.AddAttribute(new SyntaxAttribute(tokenPosition, NetNodeType.Type.ToString(), str));
                    if (!this.Expected(CLexerToken.Close_parens))
                    {
                        flag = false;
                    }
                }
                else
                {
                    flag = false;
                }
            }
            else
            {
                flag = false;
            }
            node.Range.EndPoint = this.prevPosition;
            return flag;
        }

        protected virtual bool ParseStatement()
        {
            bool flag = true;
            switch (((CLexerToken) this.Token))
            {
                case CLexerToken.Static:
                case CLexerToken.Struct:
                case CLexerToken.Typedef:
                case CLexerToken.Union:
                case CLexerToken.Volatile:
                case CLexerToken.Inline:
                case CLexerToken.Register:
                case CLexerToken.Auto:
                case CLexerToken.Const:
                case CLexerToken.Extern:
                    return this.ParseVariableDeclaration(this.TokenPosition, NetNodeType.LocalVariable);
            }
            if (this.IsBuiltInType(this.Token))
            {
                flag = this.ParseVariableDeclaration(this.TokenPosition, NetNodeType.LocalVariable);
                if (!this.Expected(CLexerToken.Semicolon))
                {
                    flag = false;
                }
                return flag;
            }
            bool parsed = false;
            if (this.IsIdentifierToken(this.Token))
            {
                flag = this.TryParseLabeledStatement(out parsed);
                if (!parsed)
                {
                    flag = this.TryParseVariableDeclarationStatement(out parsed);
                }
            }
            if (!parsed)
            {
                flag = this.ParseEmbeddedStatement();
            }
            return flag;
        }

        protected virtual bool ParseStatementExpression()
        {
            ISyntaxNode node = null;
            bool flag = this.ParseExpression(ref node);
            if (node != null)
            {
                this.AddNode(node);
            }
            return flag;
        }

        protected virtual bool ParseStatementExpressionList()
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, string.Empty, 0x83);
            this.AddNode(node);
            this.SyntaxTree.Push(node);
            try
            {
                flag = this.ParseStatementExpression();
                while (this.Token == 0x30)
                {
                    this.MoveNext();
                    if (!this.ParseStatementExpression())
                    {
                        flag = false;
                    }
                }
            }
            finally
            {
                this.SyntaxTree.Pop();
            }
            return flag;
        }

        protected virtual bool ParseStatementList()
        {
            bool flag = true;
            while (!this.Eof && (this.Token != 40))
            {
                if (!this.ParseStatement())
                {
                    flag = false;
                }
            }
            return flag;
        }

        protected virtual bool ParseStructInitializer(ref ISyntaxNode node)
        {
            bool flag = true;
            if (this.Token == 0x27)
            {
                return this.ParseFieldInitializerExpression(ref node);
            }
            bool parsed = false;
            flag = this.TryParseFieldInitializerStatement(ref node, out parsed);
            if (!parsed)
            {
                flag = this.ParseExpression(ref node);
            }
            return flag;
        }

        protected virtual bool ParseSwitchBlock()
        {
            bool flag = true;
            Point tokenPosition = this.TokenPosition;
            if (this.Expected(CLexerToken.Open_brace))
            {
                this.AddAttribute(new SyntaxAttribute(tokenPosition, SyntaxConsts.DefinitionScope, null));
                while (!this.Eof && ((this.Token == 2) || (this.Token == 7)))
                {
                    if (!this.ParseSwitchSection())
                    {
                        flag = false;
                    }
                }
                if (this.Expected(CLexerToken.Close_brace))
                {
                    this.AddAttribute(new SyntaxAttribute(this.prevPosition, SyntaxConsts.DefinitionScopeEnd, null));
                    return flag;
                }
                return false;
            }
            return false;
        }

        protected virtual bool ParseSwitchLabel()
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, this.TokenString, 70, SyntaxNodeOptions.BackIndentation);
            this.AddNode(node);
            switch (((CLexerToken) this.Token))
            {
                case CLexerToken.Case:
                {
                    this.MoveNext();
                    ISyntaxNode node2 = null;
                    if (!this.ParseExpression(ref node2))
                    {
                        flag = false;
                    }
                    if (node2 != null)
                    {
                        node.AddChild(node2);
                    }
                    break;
                }
                case CLexerToken.Default:
                    this.MoveNext();
                    break;
            }
            if (!this.Expected(CLexerToken.Colon))
            {
                flag = false;
            }
            node.Range.EndPoint = this.prevPosition;
            return flag;
        }

        protected virtual bool ParseSwitchLabels()
        {
            bool flag = true;
            do
            {
                if (!this.ParseSwitchLabel())
                {
                    flag = false;
                }
            }
            while ((this.Token == 2) || (this.Token == 7));
            return flag;
        }

        protected virtual bool ParseSwitchSection()
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, this.TokenString, 0x45, SyntaxNodeOptions.Indentation);
            this.AddNode(node);
            this.SyntaxTree.Push(node);
            try
            {
                flag = this.ParseSwitchLabels();
                if (this.Token != 0x27)
                {
                    goto Label_0051;
                }
                if (!this.ParseBlock())
                {
                    flag = false;
                }
                goto Label_0084;
            Label_0047:
                if (!this.ParseStatement())
                {
                    flag = false;
                }
            Label_0051:
                if ((!this.Eof && (this.Token != 2)) && ((this.Token != 7) && (this.Token != 40)))
                {
                    goto Label_0047;
                }
            }
            finally
            {
                this.SyntaxTree.Pop();
            }
        Label_0084:
            node.Range.EndPoint = this.prevPosition;
            return flag;
        }

        protected virtual bool ParseSwitchStatement()
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, this.TokenString, 0x44, SyntaxNodeOptions.Indentation);
            this.AddNode(node);
            this.SyntaxTree.Push(node);
            try
            {
                this.MoveNext();
                ISyntaxNode node2 = null;
                if (!this.ParseParenthesizedStatementExpression(ref node2))
                {
                    flag = false;
                }
                if (node2 != null)
                {
                    node.AddChild(node2);
                }
                if (!this.ParseSwitchBlock())
                {
                    flag = false;
                }
            }
            finally
            {
                this.SyntaxTree.Pop();
            }
            node.Range.EndPoint = this.prevPosition;
            return flag;
        }

        public override int ParseText(int state, int line, string str, ref short[] colorData)
        {
            int num = base.ParseText(state, line, str, ref colorData);
            if ((num != 2) || (((str != null) && (str.TrimEnd(new char[0]).Length != 0)) && (str[str.TrimEnd(new char[0]).Length - 1] == '\\')))
            {
                return num;
            }
            return 0;
        }

        public override int ParseText(int state, int line, string s, ref int pos, ref int len, ref int token)
        {
            int num = base.ParseText(state, line, s, ref pos, ref len, ref token);
            if ((((pos + len) < s.Length) || (num != 2)) || (((s != null) && (s.TrimEnd(new char[0]).Length != 0)) && (s[s.TrimEnd(new char[0]).Length - 1] == '\\')))
            {
                return num;
            }
            return 0;
        }

        protected virtual bool ParseType(out string type)
        {
            ISyntaxAttributes attrs = null;
            this.ParseModifiers(ref attrs);
            string str = string.Empty;
            if (this.IsPointerType(this.Token))
            {
                this.ParsePointerType(ref str);
            }
            bool flag = this.ParseTypeName(out type);
            if (str != string.Empty)
            {
                type = str + type;
            }
            return flag;
        }

        protected virtual bool ParseTypeIdentifier(out string identifier, bool checkBuiltIn)
        {
            identifier = this.TokenString;
            bool flag = true;
            if (checkBuiltIn && this.IsBuiltInType(this.Token))
            {
                bool flag2 = (((this.Token == 0x18) || (this.Token == 0x1f)) || (this.Token == 0x17)) || (this.Token == 0x13);
                this.MoveNext();
                while (flag2 && this.IsBuiltInType(this.Token))
                {
                    identifier = identifier + " " + this.TokenString;
                    this.MoveNext();
                }
            }
            else
            {
                flag = this.IdentifierExpected();
            }
            if (flag)
            {
                while ((this.IsPointerType(this.Token) || (this.Token == 5)) || (this.Token == 0x15))
                {
                    if ((this.Token == 5) || (this.Token == 0x15))
                    {
                        identifier = identifier + " " + this.TokenString;
                        this.MoveNext();
                    }
                    else
                    {
                        string type = string.Empty;
                        if (this.ParsePointerType(ref type))
                        {
                            identifier = identifier + type;
                        }
                        else
                        {
                            flag = false;
                        }
                    }
                }
                return flag;
            }
            identifier = string.Empty;
            return flag;
        }

        protected virtual bool ParseTypeName(out string type)
        {
            if (!this.IsBuiltInType(this.Token))
            {
                return this.ParseQualifiedIdentifier(out type);
            }
            return this.ParseTypeIdentifier(out type, true);
        }

        protected virtual bool ParseUnaryExpression(ref ISyntaxNode node)
        {
            bool flag = false;
            if ((this.Token == 0x2b) && this.TryParseCastExpression(ref node))
            {
                flag = true;
            }
            if (!flag)
            {
                flag = this.ParsePrimaryExpression(ref node);
            }
            return flag;
        }

        protected virtual bool ParseUnit()
        {
            bool flag = true;
            ISyntaxNode root = this.SyntaxTree.Root;
            root.NodeType = 1;
            root.Position = Point.Empty;
            flag = this.ParseUnitBody();
            this.SyntaxTree.Root.Range.EndPoint = this.prevPosition;
            if (!this.ClearStack())
            {
                flag = false;
            }
            if (!this.FixupRegions(this.SyntaxTree.Current))
            {
                flag = false;
            }
            return flag;
        }

        protected virtual bool ParseUnitBody()
        {
            return this.ParseClassBody();
        }

        protected virtual bool ParseUnknownMemberDeclaration(ISyntaxAttributes attrs)
        {
            string str2;
            Point tokenPosition = this.TokenPosition;
            string identifier = string.Empty;
            bool flag = true;
            bool flag2 = false;
            ISyntaxNode node = null;
            this.SaveState();
            try
            {
                flag = this.ParseType(out str2);
                if ((this.Token == 0x23) && !this.ParseAttributeListDeclaration(this.TokenPosition))
                {
                    flag = false;
                }
            }
            finally
            {
                bool restore = this.Token != 0x66;
                if (restore)
                {
                    flag2 = this.IsFunctionPointer(out identifier, false, ref node);
                    restore = !flag2;
                }
                this.RestoreState(restore);
                if (restore)
                {
                    str2 = string.Empty;
                }
            }
            this.ParseModifiers(ref attrs);
            if (!flag)
            {
                if (this.TokenPosition.Equals(tokenPosition))
                {
                    this.MoveNext();
                }
                return flag;
            }
            Point position = this.TokenPosition;
            if (flag2)
            {
                if (this.Token == 0x2b)
                {
                    ISyntaxNode node2 = null;
                    if (this.ParseMethodDeclaration(attrs, str2, tokenPosition, identifier, true, ref node2))
                    {
                        node2.Options |= SyntaxNodeOptions.CodeCompletion;
                        node2.AddAttribute(new SyntaxAttribute(position, NetNodeType.FunctionPointer.ToString(), null));
                        if (node != null)
                        {
                            node2.AddChild(node);
                        }
                        return flag;
                    }
                    return false;
                }
                this.Expected(CLexerToken.Open_parens);
                return false;
            }
            if (this.Token != 0x66)
            {
                this.SyntaxError();
                this.MoveNext();
                return false;
            }
            if (!this.ParseIdentifier(out identifier))
            {
                return flag;
            }
            if ((this.Token == 0x23) && !this.ParseAttributeListDeclaration(this.TokenPosition))
            {
                flag = false;
            }
            CLexerToken token = (CLexerToken) this.Token;
            if (token <= CLexerToken.Comma)
            {
                switch (token)
                {
                    case CLexerToken.Open_bracket:
                    case CLexerToken.Comma:
                        goto Label_0195;

                    case CLexerToken.Open_parens:
                    {
                        ISyntaxNode node3 = null;
                        if (!this.ParseMethodDeclaration(attrs, str2, tokenPosition, identifier, false, ref node3))
                        {
                            flag = false;
                        }
                        return flag;
                    }
                }
                goto Label_01A5;
            }
            if ((token != CLexerToken.Semicolon) && (token != CLexerToken.Assign))
            {
                goto Label_01A5;
            }
        Label_0195:
            if (!this.ParseFieldDeclaration(attrs, str2, tokenPosition, identifier))
            {
                flag = false;
            }
            return flag;
        Label_01A5:
            if (!this.ParseFieldDeclaration(attrs, str2, tokenPosition, identifier))
            {
                this.SyntaxError();
                flag = false;
            }
            return flag;
        }

        protected virtual bool ParseVariableDeclaration(Point position, NetNodeType nodeType)
        {
            string str;
            string str2;
            bool flag = true;
            ISyntaxAttributes attrs = null;
            while ((this.IsModifier(this.Token) || (this.Token == 0x1b)) || (this.Token == 30))
            {
                if (attrs == null)
                {
                    attrs = new SyntaxAttributes();
                }
                attrs.Add(new SyntaxAttribute(this.TokenPosition, NetNodeType.Modifier.ToString(), this.TokenString));
                this.MoveNext();
            }
            Point tokenPosition = this.TokenPosition;
            flag = this.ParseType(out str);
            ISyntaxNode node = null;
            this.ParseModifiers(ref attrs);
            bool flag2 = this.IsFunctionPointer(out str2, true, ref node);
            if (flag && (flag2 || this.ParseIdentifier(out str2)))
            {
                string str3;
                this.TryParseRankSpecifiers(out str3);
                if (str3 != string.Empty)
                {
                    str = str + str3;
                }
                ISyntaxNode node2 = new SyntaxNode(position, str2, (int) nodeType, SyntaxNodeOptions.CodeCompletion | SyntaxNodeOptions.Indentation);
                if (attrs != null)
                {
                    node2.AddAttributes(attrs);
                }
                node2.AddAttribute(new SyntaxAttribute(tokenPosition, NetNodeType.Type.ToString(), str));
                this.AddNode(node2);
                if (node != null)
                {
                    node2.AddChild(node);
                }
                this.SyntaxTree.Push(node2);
                try
                {
                    if (flag2)
                    {
                        if (this.Token == 0x2b)
                        {
                            node = null;
                            if (!this.ParseParameterListDeclaration(true, ref node))
                            {
                                flag = false;
                            }
                            if (node != null)
                            {
                                node2.AddChild(node);
                            }
                        }
                        else
                        {
                            this.Expected(CLexerToken.Open_parens);
                            flag = false;
                        }
                    }
                    if ((this.Token == 0x23) && !this.ParseAttributeListDeclaration(this.TokenPosition))
                    {
                        flag = false;
                    }
                    if (!this.ParseVariableDeclarators(nodeType))
                    {
                        flag = false;
                    }
                }
                finally
                {
                    this.SyntaxTree.Pop();
                }
                node2.Range.EndPoint = this.prevPosition;
                return flag;
            }
            this.IdentifierExpected();
            this.MoveNext();
            return false;
        }

        protected virtual bool ParseVariableDeclarators(NetNodeType nodeType)
        {
            bool flag = true;
            if (this.Token == 0x38)
            {
                ISyntaxNode node = new SyntaxNode(this.TokenPosition, this.TokenString, 0x21);
                this.AddNode(node);
                this.SyntaxTree.Push(node);
                try
                {
                    this.MoveNext();
                    if (!this.ParseVariableInitializer())
                    {
                        flag = false;
                    }
                }
                finally
                {
                    this.SyntaxTree.Pop();
                }
                node.Range.EndPoint = this.prevPosition;
            }
            while (this.Token == 0x30)
            {
                string str2;
                this.MoveNext();
                Point tokenPosition = this.TokenPosition;
                string type = string.Empty;
                while (this.IsPointerType(this.Token))
                {
                    if (!this.ParsePointerType(ref type))
                    {
                        flag = false;
                    }
                }
                if (this.ParseIdentifier(out str2))
                {
                    string str3;
                    this.TryParseRankSpecifiers(out str3);
                    ISyntaxNode node2 = new SyntaxNode(tokenPosition, str2, (int) nodeType, SyntaxNodeOptions.CodeCompletion);
                    if (this.SyntaxTree.Current != null)
                    {
                        ISyntaxAttribute attr = this.SyntaxTree.Current.FindAttribute(NetNodeType.Type.ToString());
                        if (attr != null)
                        {
                            if (type != string.Empty)
                            {
                                node2.AddAttribute(new SyntaxAttribute(attr.Position, attr.Name, attr.Value.ToString().Replace("*", "") + type));
                            }
                            else
                            {
                                node2.AddAttribute(attr);
                            }
                        }
                    }
                    this.AddNode(node2);
                    if (this.Token == 0x38)
                    {
                        this.SyntaxTree.Push(node2);
                        try
                        {
                            this.MoveNext();
                            if (!this.ParseVariableInitializer())
                            {
                                flag = false;
                            }
                            continue;
                        }
                        finally
                        {
                            this.SyntaxTree.Pop();
                        }
                    }
                }
            }
            return flag;
        }

        protected virtual bool ParseVariableInitializer()
        {
            bool flag = true;
            if (this.Token == 0x27)
            {
                ISyntaxNode node = null;
                flag = this.ParseArrayInitializerExpression(ref node);
                if (node != null)
                {
                    this.AddNode(node);
                }
                return flag;
            }
            ISyntaxNode node2 = null;
            flag = this.ParseExpression(ref node2);
            if (node2 != null)
            {
                this.AddNode(node2);
            }
            return flag;
        }

        protected virtual bool ParseWhileStatement()
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, this.TokenString, 0x47, SyntaxNodeOptions.Indentation);
            this.AddNode(node);
            this.SyntaxTree.Push(node);
            try
            {
                this.MoveNext();
                ISyntaxNode node2 = null;
                if (!this.ParseParenthesizedStatementExpression(ref node2))
                {
                    flag = false;
                }
                if (node2 != null)
                {
                    node.AddChild(node2);
                }
                if (!this.ParseEmbeddedStatement())
                {
                    flag = false;
                }
            }
            finally
            {
                this.SyntaxTree.Pop();
            }
            node.Range.EndPoint = this.prevPosition;
            return flag;
        }

        private void ProcessMethodNode(ISyntaxNode node, string methodName)
        {
            if (node != null)
            {
                if (((node.Name == methodName) && (node.NodeType == 0x11)) && ((node.Options & SyntaxNodeOptions.CodeCompletion) != SyntaxNodeOptions.None))
                {
                    node.Options ^= SyntaxNodeOptions.CodeCompletion;
                }
                if (node.HasChildren)
                {
                    foreach (ISyntaxNode node2 in node.ChildList)
                    {
                        this.ProcessMethodNode(node2, methodName);
                    }
                }
            }
        }

        private void ProcessTypeDef(ISyntaxNode node)
        {
            string identifier = string.Empty;
            if ((this.Token == 0x66) && this.ParseQualifiedIdentifier(out identifier))
            {
                node.AddAttribute(new SyntaxAttribute(this.prevPosition, NetNodeType.TypeDeclaration.ToString(), identifier));
            }
            if (node.Name == string.Empty)
            {
                node.Name = identifier;
            }
        }

        private void ProcessTypeDefList(ISyntaxNode node)
        {
            this.ProcessTypeDef(node);
            while (this.Token == 0x30)
            {
                this.MoveNext();
                this.ProcessTypeDef(node);
            }
        }

        public override bool ReparseBlock(Point position)
        {
            bool flag = false;
            this.comments.Clear();
            ISyntaxNode blockNode = this.SyntaxTree.FindNode(new SyntaxNode(position, string.Empty), base.pointNodeComparer);
            if (blockNode != null)
            {
                if (NETRepository.IsDeclarationNode(blockNode))
                {
                    flag = this.ReparseDeclaration(blockNode);
                }
                else
                {
                    blockNode = this.GetBlockNode(blockNode, position);
                    flag = (blockNode != null) ? this.ReparseBlock(blockNode, position) : false;
                }
            }
            this.FixupComments();
            return flag;
        }

        protected virtual bool ReparseBlock(ISyntaxNode node, Point position)
        {
            ISyntaxAttribute attribute = node.FindAttribute(SyntaxConsts.BlockScope);
            if (attribute == null)
            {
                return false;
            }
            this.Reset(attribute.Position.Y, attribute.Position.X, 0);
            node.Clear();
            this.SyntaxTree.Push(node);
            try
            {
                this.MoveNext();
                this.ParseBlock();
                if (node.Parent != null)
                {
                    ISyntaxAttribute attribute2;
                    attribute = node.FindAttribute(SyntaxConsts.DefinitionScope);
                    if (attribute != null)
                    {
                        attribute2 = node.Parent.FindAttribute(SyntaxConsts.DefinitionScope);
                        if (attribute2 != null)
                        {
                            attribute2.Position = attribute.Position;
                        }
                    }
                    attribute = node.FindAttribute(SyntaxConsts.DefinitionScopeEnd);
                    if (attribute != null)
                    {
                        attribute2 = node.Parent.FindAttribute(SyntaxConsts.DefinitionScopeEnd);
                        if (attribute2 != null)
                        {
                            attribute2.Position = attribute.Position;
                        }
                    }
                }
            }
            finally
            {
                this.SyntaxTree.Pop();
            }
            return true;
        }

        protected virtual bool ReparseDeclaration(ISyntaxNode node)
        {
            this.Reset(node.Position.Y, node.Position.X, 0);
            node.Clear();
            this.MoveNext();
            this.ParseDeclaration(node);
            return true;
        }

        public override void ReparseText()
        {
            this.Reset();
            this.MoveNext();
            this.ParseUnit();
            this.FixupComments();
            base.ReparseText();
        }

        public override void Reset()
        {
            base.Reset();
            this.comments.Clear();
        }

        public override void ResetAutoIndentChars()
        {
            this.AutoIndentChars = SyntaxParserConsts.DefaultCAutoIndentChars.ToCharArray();
        }

        public override void ResetCodeCompletionChars()
        {
            this.CodeCompletionChars = SyntaxParserConsts.DefaultCCodeCompletionChars.ToCharArray();
        }

        public override void ResetCodeCompletionStopChars()
        {
            this.CodeCompletionStopChars = SyntaxParserConsts.DefaultCCodeCompletionStopChars.ToCharArray();
        }

        public override void ResetOptions()
        {
            this.Options = SyntaxParserConsts.DefaultNetSyntaxOptions;
        }

        public override void ResetSmartFormatChars()
        {
            this.SmartFormatChars = SyntaxParserConsts.DefaultCSmartFormatChars.ToCharArray();
        }

        public override void RestoreState(bool restore)
        {
            base.RestoreState(restore);
            if (restore)
            {
                this.prevPosition = this.savePrevPosition;
            }
        }

        public override void SaveState()
        {
            base.SaveState();
            this.savePrevPosition = this.prevPosition;
        }

        protected virtual bool ShouldOutlineCommentNode(ISyntaxNode node)
        {
            return (node.Name == SyntaxParserConsts.OutlineCommentText);
        }

        public override bool ShouldSerializeAutoIndentChars()
        {
            return (new string(this.AutoIndentChars) != SyntaxParserConsts.DefaultCAutoIndentChars);
        }

        public override bool ShouldSerializeCodeCompletionChars()
        {
            return (new string(this.CodeCompletionChars) != SyntaxParserConsts.DefaultCCodeCompletionChars);
        }

        public override bool ShouldSerializeCodeCompletionStopChars()
        {
            return (new string(this.CodeCompletionStopChars) != SyntaxParserConsts.DefaultCCodeCompletionStopChars);
        }

        public override bool ShouldSerializeSmartFormatChars()
        {
            return (new string(this.SmartFormatChars) != SyntaxParserConsts.DefaultCSmartFormatChars);
        }

        protected virtual void SkipComment()
        {
            while (this.IsComment(this.Token) || (this.Token == 0x69))
            {
                this.NextToken();
            }
        }

        protected virtual bool SkipTo(int token)
        {
            return this.SkipTo(token, token);
        }

        protected virtual bool SkipTo(int token1, int token2)
        {
            while ((!this.Eof && (this.Token != token1)) && (this.Token != token2))
            {
                this.SyntaxError();
                this.MoveNext();
                if (this.IsKeywordToken(this.Token))
                {
                    break;
                }
            }
            if (this.Token != token1)
            {
                return (this.Token == token2);
            }
            return true;
        }

        protected virtual bool SkipToDeclarationStart(int nodeType)
        {
            return this.SkipTo(0x27);
        }

        public override int SmartFormatLine(int index, string text, short[] textData, ITextUndoList operations)
        {
            ISyntaxNodes nodes = new SyntaxNodes();
            this.SyntaxTree.FindNodes(new SyntaxNode(new Point(0, index), string.Empty), base.lineNodeComparer, nodes);
            int num = this.GetSmartIndent(nodes, index, false);
            if ((this.Options & SyntaxOptions.FormatSpaces) != SyntaxOptions.None)
            {
                NETRepository.SmartFormatLine(index, text, textData, nodes, operations);
            }
            return num;
        }

        protected virtual void SyntaxError()
        {
            if (base.Stack.Count == 0)
            {
                this.SyntaxError(StringConsts.ErrSyntax);
            }
        }

        protected virtual void SyntaxError(int token)
        {
            if (base.Stack.Count == 0)
            {
                ISyntaxError err = new QWhale.Syntax.SyntaxError(this.TokenPosition, this.TokenString, ((CLexerToken) token).ToString() + " " + StringConsts.ErrExpected);
                if (this.prevPosition.Y != this.TokenPosition.Y)
                {
                    err.Position = this.prevPosition;
                    err.Range.EndPoint = new Point(this.prevPosition.X + 1, this.prevPosition.Y);
                }
                this.SyntaxTree.Current.AddError(err);
            }
        }

        protected virtual void SyntaxError(string error)
        {
            if (base.Stack.Count == 0)
            {
                this.SyntaxTree.Current.AddError(new QWhale.Syntax.SyntaxError(this.TokenPosition, this.TokenString, error));
            }
        }

        protected virtual void SyntaxError(Point position, Point endPos, string name, string error)
        {
            if (base.Stack.Count == 0)
            {
                ISyntaxError err = new QWhale.Syntax.SyntaxError(position, name, error) {
                    Range = { EndPoint = endPos }
                };
                this.SyntaxTree.Current.AddError(err);
            }
        }

        private void TryFindNodeType(ref string type)
        {
            if ((this.SyntaxTree.Current != null) && this.SyntaxTree.Current.HasChildren)
            {
                ISyntaxNode node = this.SyntaxTree.Current.Childs[this.SyntaxTree.Current.Childs.Length - 1];
                switch (node.NodeType)
                {
                    case 7:
                    case 8:
                    case 9:
                    case 11:
                    case 12:
                        type = node.Name;
                        return;

                    case 10:
                        return;
                }
            }
        }

        protected virtual bool TryParseAssignmentExpression(ref ISyntaxNode node)
        {
            bool flag = true;
            switch (((CLexerToken) this.Token))
            {
                case CLexerToken.Op_mult_assign:
                case CLexerToken.Op_div_assign:
                case CLexerToken.Op_mod_assign:
                case CLexerToken.Op_add_assign:
                case CLexerToken.Op_sub_assign:
                case CLexerToken.Op_shift_left_assign:
                case CLexerToken.Op_shift_right_assign:
                case CLexerToken.Op_and_assign:
                case CLexerToken.Op_xor_assign:
                case CLexerToken.Op_or_assign:
                case CLexerToken.Assign:
                {
                    node = this.CreateExpressionNode(this.TokenPosition, this.TokenString, 0x86, node, true);
                    this.MoveNext();
                    ISyntaxNode node2 = null;
                    if (!this.ParseExpression(ref node2))
                    {
                        flag = false;
                    }
                    if (node2 != null)
                    {
                        node.AddChild(node2);
                    }
                    node.Range.EndPoint = this.prevPosition;
                    return flag;
                }
            }
            return true;
        }

        protected virtual bool TryParseCastExpression(ref ISyntaxNode node)
        {
            bool flag = true;
            Point tokenPosition = this.TokenPosition;
            string type = string.Empty;
            Point position = tokenPosition;
            this.SaveState();
            try
            {
                ISyntaxAttributes attrs = null;
                this.MoveNext();
                while ((this.Token == 0x1b) || (this.Token == 0x1d))
                {
                    if (attrs == null)
                    {
                        attrs = new SyntaxAttributes();
                    }
                    attrs.Add(new SyntaxAttribute(this.TokenPosition, NetNodeType.Modifier.ToString(), this.TokenString));
                    this.MoveNext();
                }
                position = this.TokenPosition;
                if (this.ParseType(out type))
                {
                    flag = this.Token == 0x2c;
                    if (flag)
                    {
                        ISyntaxNode item = node;
                        node = this.CreateExpressionNode(tokenPosition, type, 0x95, node, false);
                        if (attrs != null)
                        {
                            node.AddAttributes(attrs);
                        }
                        node.AddAttribute(new SyntaxAttribute(position, NetNodeType.Type.ToString(), type));
                        this.MoveNext();
                        ISyntaxNode node3 = null;
                        flag = this.ParseCastTargetExpression(ref node3);
                        if (node3 != null)
                        {
                            node.AddChild(node3);
                        }
                        node.Range.EndPoint = this.prevPosition;
                        if (flag)
                        {
                            return flag;
                        }
                        if ((item != null) && node.HasChildren)
                        {
                            node.ChildList.Remove(item);
                        }
                        node = item;
                    }
                    return flag;
                }
                flag = false;
            }
            finally
            {
                this.RestoreState(!flag);
            }
            return flag;
        }

        protected virtual bool TryParseFieldInitializerStatement(ref ISyntaxNode node, out bool parsed)
        {
            string str;
            Point tokenPosition = this.TokenPosition;
            parsed = false;
            this.SaveState();
            try
            {
                parsed = this.ParseIdentifier(out str) && this.Expected(CLexerToken.Colon);
            }
            finally
            {
                this.RestoreState(!parsed);
            }
            bool flag = parsed;
            if (parsed)
            {
                node = new SyntaxNode(tokenPosition, str, 13, SyntaxNodeOptions.Indentation);
                ISyntaxNode node2 = null;
                if (!this.ParseExpression(ref node2))
                {
                    flag = false;
                }
                if (node2 != null)
                {
                    node.AddChild(node2);
                }
                node.Range.EndPoint = this.prevPosition;
            }
            return flag;
        }

        protected virtual bool TryParseLabeledStatement(out bool parsed)
        {
            string str;
            Point tokenPosition = this.TokenPosition;
            parsed = false;
            this.SaveState();
            try
            {
                parsed = this.ParseIdentifier(out str) && this.Expected(CLexerToken.Colon);
            }
            finally
            {
                this.RestoreState(!parsed);
            }
            bool flag = parsed;
            if (parsed)
            {
                ISyntaxNode node = new SyntaxNode(tokenPosition, str, 0x69, SyntaxNodeOptions.Indentation);
                this.AddNode(node);
                this.SyntaxTree.Push(node);
                try
                {
                    flag = this.ParseStatement();
                }
                finally
                {
                    this.SyntaxTree.Pop();
                }
                node.Range.EndPoint = this.prevPosition;
            }
            return flag;
        }

        protected virtual bool TryParsePostPrimaryExpression(ref ISyntaxNode node)
        {
            bool flag = true;
            bool flag2 = false;
            while (!this.Eof)
            {
                if (this.Token == 0x2b)
                {
                    if (flag2)
                    {
                        return flag;
                    }
                    flag2 = true;
                }
                CLexerToken token = (CLexerToken) this.Token;
                if (token <= CLexerToken.Dot)
                {
                    switch (token)
                    {
                        case CLexerToken.Open_bracket:
                        {
                            if (!this.ParseElementAccess(ref node))
                            {
                                flag = false;
                            }
                            continue;
                        }
                        case CLexerToken.Close_bracket:
                            return flag;

                        case CLexerToken.Open_parens:
                        {
                            if (!this.ParseInvocationExpression(ref node))
                            {
                                flag = false;
                            }
                            continue;
                        }
                        case CLexerToken.Dot:
                            goto Label_005B;
                    }
                    return flag;
                }
                switch (token)
                {
                    case CLexerToken.Op_inc:
                    {
                        if (!this.ParsePostIncrementExpression(ref node))
                        {
                            flag = false;
                        }
                        continue;
                    }
                    case CLexerToken.Op_dec:
                    {
                        if (!this.ParsePostDecrementExpression(ref node))
                        {
                            flag = false;
                        }
                        continue;
                    }
                    case CLexerToken.Op_ptr:
                    {
                        if (!this.ParsePointerMemberAccess(ref node))
                        {
                            flag = false;
                        }
                        continue;
                    }
                    default:
                        return flag;
                }
            Label_005B:
                if (!this.ParseMemberAccess(ref node))
                {
                    flag = false;
                }
            }
            return flag;
        }

        protected virtual bool TryParseRankSpecifiers(out string rank)
        {
            bool flag = true;
            rank = string.Empty;
            while (this.Token == 0x29)
            {
                string str;
                flag = this.ParseRankSpecifier(out str);
                if (flag)
                {
                    rank = rank + str;
                }
            }
            return flag;
        }

        protected virtual bool TryParseVariableDeclarationStatement(out bool parsed)
        {
            bool flag = true;
            parsed = false;
            this.SaveState();
            try
            {
                parsed = this.ParseVariableDeclaration(this.TokenPosition, NetNodeType.LocalVariable);
            }
            finally
            {
                this.RestoreState(!parsed);
            }
            flag = parsed;
            if (flag && !this.Expected(CLexerToken.Semicolon))
            {
                flag = false;
            }
            return flag;
        }
    }
}

