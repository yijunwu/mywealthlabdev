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

    [ToolboxBitmap(typeof(JavaScriptParser), "Images.JavaScriptParser.bmp"), ToolboxItem(true)]
    public class JavaScriptParser : SyntaxParser
    {
        protected LexerProc lexCommentEndProc;
        protected LexerProc lexCommentProc;
        protected LexerProc lexDefineEndProc;
        protected LexerProc lexDefineProc;
        protected LexerProc lexIdentifierProc;
        protected LexerProc lexNumberProc;
        protected LexerProc lexStringProc;
        protected LexerProc lexSymbolProc;
        protected LexerProc lexWhitespaceProc;
        protected Point prevPosition;
        protected Hashtable reswords;
        protected const int stateComment = 2;
        protected const int stateDefine = 1;
        protected const int stateNormal = 0;

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
            return true;
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
            return new JsListMembers();
        }

        protected override IParameterInfo CreateParameterInfo()
        {
            return new JsParameterInfo();
        }

        public override ICodeCompletionRepository CreateRepository()
        {
            IReflectionRepository repository = new JavaScriptRepository(this.CaseSensitive, this.SyntaxTree);
            this.InitGlobalModules(repository);
            return repository;
        }

        protected bool Expected(JavaScriptLexerToken token)
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
            return this.Expected((JavaScriptLexerToken) token);
        }

        protected bool Expected(JavaScriptLexerToken token1, JavaScriptLexerToken token2)
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

        public override ISyntaxNode GetAutoFormatNode(Point position, bool extended, out Point startPt)
        {
            return NETRepository.GetAutoFormatNode(this.SyntaxTree.FindNode(new SyntaxNode(position, string.Empty), base.pointNodeComparer), extended, out startPt);
        }

        public override CodeCompletionType GetCompletionType(char ch)
        {
            switch (ch)
            {
                case '(':
                    return CodeCompletionType.ParameterInfo;

                case '.':
                    return CodeCompletionType.ListMembers;
            }
            return base.GetCompletionType(ch);
        }

        protected override int GetLexerStyle(int token)
        {
            if (this.IsReswordToken(token))
            {
                return 2;
            }
            if (this.IsSymbol(token))
            {
                return 5;
            }
            switch (token)
            {
                case 0x80:
                case 0x81:
                case 130:
                    return 1;

                case 0x83:
                case 0x84:
                    return 7;

                case 0x85:
                    return 0;

                case 0x86:
                    return 3;

                case 0x87:
                    return 8;
            }
            return 6;
        }

        protected bool IdentifierExpected()
        {
            return this.Expected(JavaScriptLexerToken.Identifier_Literal);
        }

        protected virtual void InitGlobalModules(IReflectionRepository repository)
        {
            repository.RegisterType("", typeof(JavaScriptBuiltInFunctions), true);
        }

        protected override void InitLanguage()
        {
            this.Scheme.FileType = "jscript";
        }

        protected override void InitLexer()
        {
            base.InitLexer();
            this.InitReswords();
            this.lexWhitespaceProc = new LexerProc(this.LexWhitespace);
            this.lexSymbolProc = new LexerProc(this.LexSymbol);
            this.lexIdentifierProc = new LexerProc(this.LexIdentifier);
            this.lexNumberProc = new LexerProc(this.LexNumber);
            this.lexStringProc = new LexerProc(this.LexString);
            this.lexCommentProc = new LexerProc(this.LexComment);
            this.lexCommentEndProc = new LexerProc(this.LexCommentEnd);
            this.lexDefineProc = new LexerProc(this.LexDefine);
            this.lexDefineEndProc = new LexerProc(this.LexDefineEnd);
            base.RegisterLexerProc(0, this.lexWhitespaceProc);
            base.RegisterLexerProc(0, '!', '\x00ff', this.lexSymbolProc);
            base.RegisterLexerProc(0, 'a', 'z', this.lexIdentifierProc);
            base.RegisterLexerProc(0, 'A', 'Z', this.lexIdentifierProc);
            base.RegisterLexerProc(0, '_', this.lexIdentifierProc);
            base.RegisterLexerProc(0, '0', '9', this.lexNumberProc);
            base.RegisterLexerProc(0, new char[] { '+', '-' }, this.lexSymbolProc);
            base.RegisterLexerProc(0, new char[] { '"', '\'' }, this.lexStringProc);
            base.RegisterLexerProc(0, '/', this.lexCommentProc);
            base.RegisterLexerProc(0, '@', this.lexDefineProc);
            base.RegisterLexerProc(2, this.lexCommentEndProc);
            base.RegisterLexerProc(1, this.lexDefineEndProc);
        }

        protected virtual void InitReswords()
        {
            this.reswords = new Hashtable();
            this.reswords.Add("abstract", JavaScriptLexerToken.Abstract);
            this.reswords.Add("assert", JavaScriptLexerToken.Assert);
            this.reswords.Add("boolean", JavaScriptLexerToken.Boolean);
            this.reswords.Add("break", JavaScriptLexerToken.Break);
            this.reswords.Add("byte", JavaScriptLexerToken.Byte);
            this.reswords.Add("case", JavaScriptLexerToken.Case);
            this.reswords.Add("catch", JavaScriptLexerToken.Catch);
            this.reswords.Add("char", JavaScriptLexerToken.Char);
            this.reswords.Add("class", JavaScriptLexerToken.Class);
            this.reswords.Add("const", JavaScriptLexerToken.Const);
            this.reswords.Add("continue", JavaScriptLexerToken.Continue);
            this.reswords.Add("debugger", JavaScriptLexerToken.Debugger);
            this.reswords.Add("default", JavaScriptLexerToken.Default);
            this.reswords.Add("delegate", JavaScriptLexerToken.Delegate);
            this.reswords.Add("delete", JavaScriptLexerToken.Delete);
            this.reswords.Add("do", JavaScriptLexerToken.Do);
            this.reswords.Add("double", JavaScriptLexerToken.Double);
            this.reswords.Add("else", JavaScriptLexerToken.Else);
            this.reswords.Add("enum", JavaScriptLexerToken.Enum);
            this.reswords.Add("export", JavaScriptLexerToken.Export);
            this.reswords.Add("extends", JavaScriptLexerToken.Extends);
            this.reswords.Add("false", JavaScriptLexerToken.False);
            this.reswords.Add("final", JavaScriptLexerToken.Final);
            this.reswords.Add("finally", JavaScriptLexerToken.Finally);
            this.reswords.Add("float", JavaScriptLexerToken.Float);
            this.reswords.Add("for", JavaScriptLexerToken.For);
            this.reswords.Add("function", JavaScriptLexerToken.Function);
            this.reswords.Add("goto", JavaScriptLexerToken.Goto);
            this.reswords.Add("if", JavaScriptLexerToken.If);
            this.reswords.Add("implements", JavaScriptLexerToken.Implements);
            this.reswords.Add("import", JavaScriptLexerToken.Import);
            this.reswords.Add("in", JavaScriptLexerToken.In);
            this.reswords.Add("instanceof", JavaScriptLexerToken.Instanceof);
            this.reswords.Add("int", JavaScriptLexerToken.Int);
            this.reswords.Add("interface", JavaScriptLexerToken.Interface);
            this.reswords.Add("long", JavaScriptLexerToken.Long);
            this.reswords.Add("multicast", JavaScriptLexerToken.Multicast);
            this.reswords.Add("native", JavaScriptLexerToken.Native);
            this.reswords.Add("new", JavaScriptLexerToken.New);
            this.reswords.Add("null", JavaScriptLexerToken.Null);
            this.reswords.Add("package", JavaScriptLexerToken.Package);
            this.reswords.Add("private", JavaScriptLexerToken.Private);
            this.reswords.Add("protected", JavaScriptLexerToken.Protected);
            this.reswords.Add("public", JavaScriptLexerToken.Public);
            this.reswords.Add("return", JavaScriptLexerToken.Return);
            this.reswords.Add("short", JavaScriptLexerToken.Short);
            this.reswords.Add("static", JavaScriptLexerToken.Static);
            this.reswords.Add("strictfp", JavaScriptLexerToken.Strictfp);
            this.reswords.Add("super", JavaScriptLexerToken.Super);
            this.reswords.Add("switch", JavaScriptLexerToken.Switch);
            this.reswords.Add("synchronized", JavaScriptLexerToken.Synchronized);
            this.reswords.Add("this", JavaScriptLexerToken.This);
            this.reswords.Add("throw", JavaScriptLexerToken.Throw);
            this.reswords.Add("throws", JavaScriptLexerToken.Throws);
            this.reswords.Add("transient", JavaScriptLexerToken.Transient);
            this.reswords.Add("true", JavaScriptLexerToken.True);
            this.reswords.Add("try", JavaScriptLexerToken.Try);
            this.reswords.Add("typeof", JavaScriptLexerToken.Typeof);
            this.reswords.Add("using", JavaScriptLexerToken.Using);
            this.reswords.Add("var", JavaScriptLexerToken.Var);
            this.reswords.Add("void", JavaScriptLexerToken.Void);
            this.reswords.Add("volatile", JavaScriptLexerToken.Volatile);
            this.reswords.Add("while", JavaScriptLexerToken.While);
            this.reswords.Add("with", JavaScriptLexerToken.With);
        }

        protected override void InitStyles()
        {
            this.InitDefaultStyles();
        }

        protected virtual bool IsBuiltInType(int token)
        {
            JavaScriptLexerToken token2 = (JavaScriptLexerToken) this.Token;
            if (token2 <= JavaScriptLexerToken.Float)
            {
                switch (token2)
                {
                    case JavaScriptLexerToken.Double:
                    case JavaScriptLexerToken.Float:
                    case JavaScriptLexerToken.Array:
                    case JavaScriptLexerToken.Boolean:
                    case JavaScriptLexerToken.Byte:
                    case JavaScriptLexerToken.Char:
                        goto Label_0075;
                }
                goto Label_0077;
            }
            if (token2 <= JavaScriptLexerToken.Object)
            {
                switch (token2)
                {
                    case JavaScriptLexerToken.Int:
                    case JavaScriptLexerToken.Long:
                    case JavaScriptLexerToken.Number:
                    case JavaScriptLexerToken.Object:
                        goto Label_0075;
                }
                goto Label_0077;
            }
            if ((token2 != JavaScriptLexerToken.Short) && (token2 != JavaScriptLexerToken.String))
            {
                goto Label_0077;
            }
        Label_0075:
            return true;
        Label_0077:
            return false;
        }

        protected virtual bool IsComment(int tok)
        {
            return (tok == 0x86);
        }

        public override bool IsDeclaration(ISyntaxNode node)
        {
            return ((node != null) && NETRepository.IsDeclarationNode(node));
        }

        protected virtual bool IsReswordToken(int token)
        {
            return ((token >= 0) && (token <= 80));
        }

        protected virtual bool IsSymbol(int token)
        {
            return ((token >= 0x51) && (token <= 0x7f));
        }

        protected override bool IsValidToken(int tok)
        {
            return ((tok != 0x88) && !this.IsComment(tok));
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
                        base.currentPos = length;
                        return 0x86;
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
                        return 0x86;
                    }
                }
                else
                {
                    base.currentPos++;
                }
            }
            this.State = 2;
            return 0x86;
        }

        protected virtual int LexDefine()
        {
            int length = base.source.Length;
            base.currentPos++;
            if (base.currentPos < base.source.Length)
            {
                char ch = base.source[base.currentPos];
                int currentPos = base.currentPos;
                switch (ch)
                {
                    case ' ':
                    case '\t':
                        currentPos++;
                        while ((currentPos < length) && ((base.source[currentPos] == ' ') || (base.source[currentPos] == '\t')))
                        {
                            currentPos++;
                        }
                        if (currentPos < length)
                        {
                            ch = base.source[currentPos];
                        }
                        break;
                }
                if ((((ch >= 'a') && (ch <= 'z')) || ((ch >= 'A') && (ch <= 'Z'))) || (ch == '_'))
                {
                    base.currentPos = currentPos;
                    this.LexIdent();
                }
            }
            this.State = (base.currentPos == length) ? 0 : 1;
            return 0x87;
        }

        protected virtual int LexDefineEnd()
        {
            base.currentPos = base.source.Length;
            this.State = 0;
            return 0x85;
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
            return 0x80;
        }

        protected virtual int LexIdentifier()
        {
            this.LexIdent();
            object obj2 = this.reswords[this.TokenString];
            if (obj2 == null)
            {
                return 0x85;
            }
            return (int) obj2;
        }

        protected virtual int LexNumber()
        {
            int num2;
            char ch = base.source[base.currentPos];
            int length = base.source.Length;
            int num3 = 0x80;
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
            }
            base.LexNum();
            if (base.currentPos < length)
            {
                ch = base.source[base.currentPos];
                if (ch == '.')
                {
                    if (base.currentPos < (length - 1))
                    {
                        ch = base.source[base.currentPos + 1];
                        if ((ch >= '0') && (ch <= '9'))
                        {
                            base.currentPos++;
                            this.LexNum();
                        }
                        else
                        {
                            base.currentPos++;
                        }
                    }
                    num3 = 0x81;
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
                        num3 = 130;
                    }
                }
            }
            return num3;
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
                return 0x84;
            }
            return 0x83;
        }

        protected virtual int LexSymbol()
        {
            JavaScriptLexerToken bang = JavaScriptLexerToken.Whitespace_Literal;
            char ch = base.source[base.currentPos];
            base.currentPos++;
            switch (ch)
            {
                case '!':
                    if (base.CurChar() != '=')
                    {
                        bang = JavaScriptLexerToken.Bang;
                        break;
                    }
                    base.currentPos++;
                    bang = JavaScriptLexerToken.Op_ne;
                    break;

                case '%':
                    if (base.CurChar() != '=')
                    {
                        bang = JavaScriptLexerToken.Percent;
                        break;
                    }
                    base.currentPos++;
                    bang = JavaScriptLexerToken.Op_mod_assign;
                    break;

                case '&':
                    if (base.CurChar() != '&')
                    {
                        bang = JavaScriptLexerToken.Whitespace_Literal;
                        break;
                    }
                    base.currentPos++;
                    bang = JavaScriptLexerToken.Op_and;
                    break;

                case '(':
                    bang = JavaScriptLexerToken.Open_parens;
                    break;

                case ')':
                    bang = JavaScriptLexerToken.Close_parens;
                    break;

                case '*':
                    if (base.CurChar() != '=')
                    {
                        bang = JavaScriptLexerToken.Star;
                        break;
                    }
                    base.currentPos++;
                    bang = JavaScriptLexerToken.Op_mult_assign;
                    break;

                case '+':
                    switch (base.CurChar())
                    {
                        case '+':
                            base.currentPos++;
                            bang = JavaScriptLexerToken.Op_inc;
                            goto Label_0341;

                        case '=':
                            base.currentPos++;
                            bang = JavaScriptLexerToken.Op_add_assign;
                            goto Label_0341;
                    }
                    bang = JavaScriptLexerToken.Plus;
                    break;

                case ',':
                    bang = JavaScriptLexerToken.Comma;
                    break;

                case '-':
                    switch (base.CurChar())
                    {
                        case '-':
                            base.currentPos++;
                            bang = JavaScriptLexerToken.Op_dec;
                            goto Label_0341;

                        case '=':
                            base.currentPos++;
                            bang = JavaScriptLexerToken.Op_sub_assign;
                            goto Label_0341;
                    }
                    bang = JavaScriptLexerToken.Minus;
                    break;

                case '.':
                    bang = JavaScriptLexerToken.Dot;
                    break;

                case '/':
                    if (base.CurChar() != '=')
                    {
                        bang = JavaScriptLexerToken.Div;
                        break;
                    }
                    base.currentPos++;
                    bang = JavaScriptLexerToken.Op_div_assign;
                    break;

                case ':':
                    bang = JavaScriptLexerToken.Colon;
                    break;

                case ';':
                    bang = JavaScriptLexerToken.Semicolon;
                    break;

                case '<':
                    if (base.CurChar() != '=')
                    {
                        bang = JavaScriptLexerToken.Op_lt;
                        break;
                    }
                    base.currentPos++;
                    bang = JavaScriptLexerToken.Op_le;
                    break;

                case '=':
                    if (base.CurChar() != '=')
                    {
                        bang = JavaScriptLexerToken.Assign;
                        break;
                    }
                    base.currentPos++;
                    if (base.CurChar() != '=')
                    {
                        bang = JavaScriptLexerToken.Op_eq;
                        break;
                    }
                    base.currentPos++;
                    bang = JavaScriptLexerToken.Op_op_eq;
                    break;

                case '>':
                    if (base.CurChar() != '=')
                    {
                        bang = JavaScriptLexerToken.Op_gt;
                        break;
                    }
                    base.currentPos++;
                    bang = JavaScriptLexerToken.Op_ge;
                    break;

                case '?':
                    bang = JavaScriptLexerToken.Interr;
                    break;

                case '[':
                    bang = JavaScriptLexerToken.Open_bracket;
                    break;

                case ']':
                    bang = JavaScriptLexerToken.Close_bracket;
                    break;

                case '{':
                    bang = JavaScriptLexerToken.Open_brace;
                    break;

                case '|':
                    if (base.CurChar() != '|')
                    {
                        bang = JavaScriptLexerToken.Whitespace_Literal;
                        break;
                    }
                    base.currentPos++;
                    bang = JavaScriptLexerToken.Op_or;
                    break;

                case '}':
                    bang = JavaScriptLexerToken.Close_brace;
                    break;
            }
        Label_0341:
            return (int) bang;
        }

        protected virtual int LexWhitespace()
        {
            this.LexSpace();
            return 0x88;
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
            switch (((JavaScriptLexerToken) this.Token))
            {
                case JavaScriptLexerToken.Plus:
                case JavaScriptLexerToken.Minus:
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
                    break;
                }
            }
            return flag;
        }

        protected virtual bool ParseArgument(ref ISyntaxNode node)
        {
            node = new SyntaxNode(this.TokenPosition, string.Empty, 0x1c);
            ISyntaxNode node2 = null;
            bool flag = this.ParseExpression(ref node2);
            if (!flag)
            {
                this.prevPosition = this.TokenPosition;
            }
            if (node2 != null)
            {
                node.AddChild(node2);
            }
            node.Range.EndPoint = this.prevPosition;
            return flag;
        }

        protected virtual bool ParseArgumentList(ref ISyntaxNode node)
        {
            bool flag = true;
            node = new SyntaxNode(this.TokenPosition, this.TokenString, 0x1d);
            if (this.Token != 0x53)
            {
                this.SyntaxError(0x53);
                return false;
            }
            this.MoveNext();
            while (!this.Eof && (this.Token != 0x54))
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
                if (this.Token != 0x58)
                {
                    break;
                }
                this.MoveNext();
            }
            if (!this.Expected(JavaScriptLexerToken.Close_parens))
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
            if (this.Token == 0x55)
            {
                this.MoveNext();
                if (this.Token != 0x56)
                {
                    ISyntaxNode node2 = null;
                    flag = this.ParseExpressionList(ref node2);
                    if (node2 != null)
                    {
                        node.AddChild(node2);
                    }
                }
                if (!this.Expected(JavaScriptLexerToken.Close_bracket))
                {
                    flag = false;
                }
            }
            else
            {
                this.SyntaxError(0x55);
                flag = false;
            }
            node.Range.EndPoint = this.prevPosition;
            return flag;
        }

        protected virtual bool ParseBlock()
        {
            bool flag = true;
            Point tokenPosition = this.TokenPosition;
            if (this.Expected(JavaScriptLexerToken.Open_brace))
            {
                this.AddAttribute(new SyntaxAttribute(tokenPosition, SyntaxConsts.BlockScope, null));
                this.AddAttribute(new SyntaxAttribute(tokenPosition, SyntaxConsts.DefinitionScope, null));
                if ((this.Token != 0x52) && !this.ParseStatementList(true))
                {
                    flag = false;
                }
                if (this.Expected(JavaScriptLexerToken.Close_brace))
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
            bool flag = false;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, string.Empty, 0x6a);
            this.AddNode(node);
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
            if (!this.SemicolonNeeded())
            {
                flag = false;
            }
            node.Range.EndPoint = this.prevPosition;
            return flag;
        }

        protected virtual bool ParseCatchStatement()
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, this.TokenString, 0x61, (this.SyntaxTree.Current.NodeType != 0x60) ? SyntaxNodeOptions.Indentation : SyntaxNodeOptions.BackIndentation);
            this.AddNode(node);
            this.SyntaxTree.Push(node);
            try
            {
                this.MoveNext();
                if (this.Token == 0x53)
                {
                    this.MoveNext();
                    if (this.Token == 0x85)
                    {
                        this.MoveNext();
                    }
                    if (!this.Expected(JavaScriptLexerToken.Close_parens))
                    {
                        flag = false;
                    }
                }
                if (!this.ParseBlock())
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

        protected virtual bool ParseComment()
        {
            while (this.IsComment(this.Token))
            {
                this.NextToken();
            }
            return true;
        }

        protected virtual bool ParseConditionalAndExpression(ref ISyntaxNode node)
        {
            bool flag = this.ParseEqualityExpression(ref node);
            if (this.Token == 0x6c)
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
            if (this.Token == 0x5b)
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
                if (this.Expected(JavaScriptLexerToken.Colon))
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
            if (this.Token == 0x6d)
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

        protected virtual bool ParseContinueStatement()
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, this.TokenString, 0x5c);
            this.AddNode(node);
            this.MoveNext();
            if (this.Token == 0x85)
            {
                this.MoveNext();
            }
            if (!this.SemicolonNeeded())
            {
                flag = false;
            }
            node.Range.EndPoint = this.prevPosition;
            return flag;
        }

        protected virtual bool ParseDefaultBlock()
        {
            bool flag = true;
            while (!this.Eof)
            {
                ISyntaxNode node;
                JavaScriptLexerToken token = (JavaScriptLexerToken) this.Token;
                if (token != JavaScriptLexerToken.Function)
                {
                    if (token == JavaScriptLexerToken.Var)
                    {
                        goto Label_0023;
                    }
                    goto Label_0058;
                }
                if (!this.ParseMethodDeclaration())
                {
                    flag = false;
                }
                continue;
            Label_0023:
                node = new SyntaxNode();
                if (!this.ParseVariableDeclaration(null, 13, ref node))
                {
                    flag = false;
                }
                if (!this.SemicolonNeeded())
                {
                    flag = false;
                }
                if (node != null)
                {
                    node.Range.EndPoint = this.prevPosition;
                }
                continue;
            Label_0058:
                if (!this.ParseStatementList(false))
                {
                    flag = false;
                }
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
                if (this.ParseDoWhileStatement())
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
            if (this.Token == 0x53)
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
                if (!this.Expected(JavaScriptLexerToken.Close_parens))
                {
                    flag = false;
                }
                if (!this.SemicolonNeeded())
                {
                    flag = false;
                }
            }
            else
            {
                this.SyntaxError(0x53);
                flag = false;
            }
            node.Range.EndPoint = this.prevPosition;
            return flag;
        }

        protected virtual bool ParseElementAccess(ref ISyntaxNode node)
        {
            bool flag = true;
            node = this.CreateExpressionNode(this.TokenPosition, this.TokenString, 0xa2, node, false);
            this.MoveNext();
            ISyntaxNode node2 = null;
            flag = (this.Token == 0x56) || this.ParseExpressionList(ref node2);
            if (node2 != null)
            {
                node.AddChild(node2);
            }
            if (!this.Expected(JavaScriptLexerToken.Close_bracket))
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
            switch (((JavaScriptLexerToken) this.Token))
            {
                case JavaScriptLexerToken.For:
                    return this.ParseForStatement();

                case JavaScriptLexerToken.Function:
                    return this.ParseMethodDeclaration();

                case JavaScriptLexerToken.If:
                    return this.ParseIfStatement();

                case JavaScriptLexerToken.Return:
                    return this.ParseReturnStatement();

                case JavaScriptLexerToken.Do:
                    return this.ParseDoStatement();

                case JavaScriptLexerToken.Break:
                    return this.ParseBreakStatement();

                case JavaScriptLexerToken.Continue:
                    return this.ParseContinueStatement();

                case JavaScriptLexerToken.Switch:
                    return this.ParseSwitchStatement();

                case JavaScriptLexerToken.Throw:
                    return this.ParseThrowStatement();

                case JavaScriptLexerToken.Try:
                    return this.ParseTryStatement();

                case JavaScriptLexerToken.While:
                    return this.ParseWhileStatement();

                case JavaScriptLexerToken.Open_brace:
                    return this.ParseBlockStatement();
            }
            return this.ParseExpressionStatement();
        }

        protected virtual bool ParseEqualityExpression(ref ISyntaxNode node)
        {
            bool flag = this.ParseRelationalExpression(ref node);
            switch (this.Token)
            {
                case 0x66:
                case 0x67:
                case 0x68:
                case 0x6a:
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
                case 0x69:
                    return flag;
            }
            return flag;
        }

        protected virtual bool ParseExpression(ref ISyntaxNode node)
        {
            bool flag = true;
            flag = this.ParseConditionalExpression(ref node);
            if (!this.TryParseAssignmentExpression(ref node))
            {
                flag = false;
            }
            return flag;
        }

        protected virtual bool ParseExpressionList(ref ISyntaxNode node)
        {
            node = new SyntaxNode(this.TokenPosition, string.Empty, 0x83);
            ISyntaxNode node2 = null;
            bool flag = (this.Token == 0x58) || this.ParseExpression(ref node2);
            if (node2 != null)
            {
                node.AddChild(node2);
            }
            while (this.Token == 0x58)
            {
                this.MoveNext();
                node2 = null;
                if ((this.Token != 0x58) && !this.ParseExpression(ref node2))
                {
                    flag = false;
                }
                if (node2 != null)
                {
                    node.AddChild(node2);
                }
            }
            node.Range.EndPoint = this.prevPosition;
            return flag;
        }

        protected virtual bool ParseExpressionStatement()
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, string.Empty, 0x6b, SyntaxNodeOptions.Indentation);
            this.AddNode(node);
            this.SyntaxTree.Push(node);
            try
            {
                flag = this.ParseStatementExpression();
                if (!this.SemicolonNeeded())
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

        protected virtual bool ParseForeachInitializer()
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, string.Empty, 0x4f);
            this.AddNode(node);
            if (this.Expected(JavaScriptLexerToken.In))
            {
                ISyntaxNode node2 = null;
                flag = this.ParseExpression(ref node2);
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
                    flag = this.ParseVariableDeclaration(null, 15);
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
                if (this.Expected(JavaScriptLexerToken.Open_parens))
                {
                    if ((this.Token != 90) && !this.ParseForInitializer())
                    {
                        flag = false;
                    }
                    if (this.Token == 0x20)
                    {
                        if (!this.ParseForeachInitializer())
                        {
                            flag = false;
                        }
                    }
                    else
                    {
                        if (this.Expected(JavaScriptLexerToken.Semicolon))
                        {
                            if ((this.Token != 90) && !this.ParseForCondition())
                            {
                                flag = false;
                            }
                        }
                        else
                        {
                            flag = false;
                        }
                        if (this.Expected(JavaScriptLexerToken.Semicolon))
                        {
                            if ((this.Token != 0x52) && !this.ParseForIterator())
                            {
                                flag = false;
                            }
                        }
                        else
                        {
                            flag = false;
                        }
                    }
                    if (!this.Expected(JavaScriptLexerToken.Close_parens))
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

        protected virtual bool ParseFunctionPrototypeExpression(ref ISyntaxNode node)
        {
            node = new SyntaxNode(this.TokenPosition, string.Empty, 0xca);
            this.MoveNext();
            bool flag = this.ParseMethodDeclaration(node);
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
                if ((this.Token == 0x12) && !this.ParseElseStatement())
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

        protected virtual bool ParseMemberAccess(ref ISyntaxNode node)
        {
            string str;
            bool flag = true;
            node = this.CreateExpressionNode(this.TokenPosition, this.TokenString, 0x9f, node, true);
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
            if (!this.SkipTo(0x51, 90))
            {
                return false;
            }
            bool flag = true;
            ISyntaxNode current = this.SyntaxTree.Current;
            JavaScriptLexerToken token = (JavaScriptLexerToken) this.Token;
            if (token != JavaScriptLexerToken.Open_brace)
            {
                if (token == JavaScriptLexerToken.Semicolon)
                {
                    current.Range.EndPoint = this.CurrentPosition;
                    this.MoveNext();
                    return flag;
                }
                this.SyntaxError();
                return false;
            }
            current.Options |= SyntaxNodeOptions.Outlining | SyntaxNodeOptions.Indentation;
            if ((this.Token == 0x51) && !this.ParseBlockStatement())
            {
                flag = false;
            }
            current.Range.EndPoint = this.prevPosition;
            return flag;
        }

        protected virtual bool ParseMethodDeclaration()
        {
            string str;
            Point tokenPosition = this.TokenPosition;
            this.MoveNext();
            ISyntaxNode node = new SyntaxNode(tokenPosition, string.Empty, 0x11, SyntaxNodeOptions.CodeCompletion);
            if (this.ParseIdentifier(out str))
            {
                node.Name = str;
                this.AddNode(node);
                return this.ParseMethodDeclaration(node);
            }
            return false;
        }

        protected virtual bool ParseMethodDeclaration(ISyntaxNode node)
        {
            bool flag = true;
            if (!this.BeforeDeclaration(node))
            {
                flag = false;
            }
            this.SyntaxTree.Push(node);
            try
            {
                if (!this.ParseParameterListDeclaration())
                {
                    flag = false;
                }
                if (!this.ParseReturnType())
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
            if (!this.SemicolonNeeded())
            {
                flag = false;
            }
            node.Range.EndPoint = this.prevPosition;
            if (!this.AfterDeclaration(node))
            {
                flag = false;
            }
            return flag;
        }

        protected virtual bool ParseMultiplicativeExpression(ref ISyntaxNode node)
        {
            bool flag = this.ParsePrefixedUnaryExpression(ref node);
            switch (this.Token)
            {
                case 0x6f:
                case 0x71:
                case 0x73:
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
                case 0x70:
                case 0x72:
                    return flag;
            }
            return flag;
        }

        protected virtual bool ParseNewExpression(ref ISyntaxNode node)
        {
            node = new SyntaxNode(this.TokenPosition, this.TokenString, 0x9b);
            this.MoveNext();
            string type = string.Empty;
            Point tokenPosition = this.TokenPosition;
            bool flag = (this.Token == 0x53) || (this.ParseTypeName(out type) && (type != string.Empty));
            if (flag)
            {
                if (type != string.Empty)
                {
                    node.AddAttribute(new SyntaxAttribute(tokenPosition, NetNodeType.Type.ToString(), type));
                }
                if (this.Token == 0x53)
                {
                    ISyntaxNode node2 = null;
                    flag = this.ParseArgumentList(ref node2);
                    if (node2 != null)
                    {
                        node.AddChild(node2);
                    }
                }
            }
            else
            {
                flag = false;
            }
            node.Range.EndPoint = this.prevPosition;
            return flag;
        }

        protected virtual bool ParseParameterDeclaration()
        {
            string str;
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, string.Empty, 0x1a, SyntaxNodeOptions.CodeCompletion);
            if (this.ParseIdentifier(out str))
            {
                node.Name = str;
                this.AddNode(node);
            }
            else
            {
                flag = false;
            }
            node.Range.EndPoint = this.prevPosition;
            return flag;
        }

        protected virtual bool ParseParameterListDeclaration()
        {
            return this.ParseParameterListDeclaration(0x53, 0x54);
        }

        protected virtual bool ParseParameterListDeclaration(int startToken, int endToken)
        {
            bool flag = true;
            if (this.Token != startToken)
            {
                this.SyntaxError(startToken);
                return false;
            }
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, string.Empty, 0x1b);
            this.AddNode(node);
            this.SyntaxTree.Push(node);
            try
            {
                this.MoveNext();
                while (!this.Eof && (this.Token != endToken))
                {
                    if (!this.ParseParameterDeclaration())
                    {
                        flag = false;
                    }
                    if (this.Token != 0x58)
                    {
                        goto Label_0078;
                    }
                    this.MoveNext();
                }
            }
            finally
            {
                this.SyntaxTree.Pop();
            }
        Label_0078:
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
            node = new SyntaxNode(this.TokenPosition, this.TokenString, 0xa8);
            if (this.Token == 0x53)
            {
                this.MoveNext();
                ISyntaxNode node2 = null;
                flag = this.ParseExpression(ref node2);
                if (node2 != null)
                {
                    node.AddChild(node2);
                }
                if (!this.Expected(JavaScriptLexerToken.Close_parens))
                {
                    flag = false;
                }
            }
            else
            {
                this.SyntaxError(0x53);
                flag = false;
            }
            node.Range.EndPoint = this.prevPosition;
            return flag;
        }

        protected virtual bool ParseParenthesizedStatementExpression(ref ISyntaxNode node)
        {
            if (this.Token == 0x53)
            {
                return this.ParseParenthesizedExpression(ref node);
            }
            this.Expected(JavaScriptLexerToken.Open_parens);
            return this.ParseExpression(ref node);
        }

        protected virtual bool ParsePostDecrementExpression(ref ISyntaxNode node)
        {
            node = new SyntaxNode(this.TokenPosition, this.TokenString, 170);
            this.MoveNext();
            node.Range.EndPoint = this.prevPosition;
            return true;
        }

        protected virtual bool ParsePostIncrementExpression(ref ISyntaxNode node)
        {
            node = new SyntaxNode(this.TokenPosition, this.TokenString, 0xa9);
            this.MoveNext();
            node.Range.EndPoint = this.prevPosition;
            return true;
        }

        protected virtual bool ParsePrefixedUnaryExpression(ref ISyntaxNode node)
        {
            bool flag = true;
            switch (((JavaScriptLexerToken) this.Token))
            {
                case JavaScriptLexerToken.Op_inc:
                case JavaScriptLexerToken.Plus:
                case JavaScriptLexerToken.Op_dec:
                case JavaScriptLexerToken.Minus:
                case JavaScriptLexerToken.Bang:
                {
                    node = this.CreateExpressionNode(this.TokenPosition, this.TokenString, 0x93, node, false);
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
                    node.Range.EndPoint = this.prevPosition;
                    return flag;
                }
            }
            return this.ParseUnaryExpression(ref node);
        }

        protected virtual bool ParsePrimaryExpression(ref ISyntaxNode node)
        {
            bool flag = this.ParseSimpleExpression(ref node);
            if ((node != null) && !this.TryParsePostPrimaryExpression(ref node))
            {
                flag = false;
            }
            return flag;
        }

        protected virtual bool ParseQualifiedIdentifier(out string identifier)
        {
            bool flag = this.ParseIdentifier(out identifier);
            if (flag)
            {
                while (this.Token == 0x57)
                {
                    string str;
                    identifier = identifier + this.TokenString;
                    this.MoveNext();
                    if (this.ParseIdentifier(out str))
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
            rank = this.TokenString;
            if (!this.Expected(JavaScriptLexerToken.Open_bracket))
            {
                return flag;
            }
            while (this.Token == 0x58)
            {
                rank = rank + this.TokenString;
                this.MoveNext();
            }
            if (this.Token == 0x56)
            {
                rank = rank + this.TokenString;
            }
            return this.Expected(JavaScriptLexerToken.Close_bracket);
        }

        protected virtual bool ParseRegularExpression(ref ISyntaxNode node)
        {
            bool flag = true;
            int lineIndex = base.lineIndex;
            node = new SyntaxNode(this.TokenPosition, string.Empty, 0xcc);
            while (!this.Eof)
            {
                char ch;
                this.MoveNext();
                if (lineIndex != base.lineIndex)
                {
                    this.SyntaxError();
                    flag = false;
                    break;
                }
                if (this.Token != 0x71)
                {
                    continue;
                }
                node.Name = base.source.Substring(node.Position.X, this.CurrentPosition.X - node.Position.X);
                if ((lineIndex == base.lineIndex) && (this.CurrentPosition.X < base.source.Length))
                {
                    switch (base.source[this.CurrentPosition.X])
                    {
                        case 'g':
                        case 'i':
                        case 'm':
                            goto Label_00F3;
                    }
                }
                goto Label_014C;
            Label_00F3:
                ch = base.source[this.CurrentPosition.X];
                this.MoveNext();
                if (this.TokenString == ch.ToString())
                {
                    node.Name = node.Name + ch.ToString();
                }
                else
                {
                    this.SyntaxError();
                    flag = false;
                }
            Label_014C:
                this.MoveNext();
                break;
            }
            node.Range.EndPoint = this.prevPosition;
            return flag;
        }

        protected virtual bool ParseRelationalExpression(ref ISyntaxNode node)
        {
            bool flag = this.ParseAdditiveExpression(ref node);
            switch (this.Token)
            {
                case 0x62:
                case 0x63:
                case 100:
                case 0x65:
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
            return this.ParseReturnStatement(NetNodeType.ReturnStatement);
        }

        protected virtual bool ParseReturnStatement(NetNodeType nodeType)
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, this.TokenString, (int) nodeType, SyntaxNodeOptions.Indentation);
            this.AddNode(node);
            this.MoveNext();
            if (this.Token != 90)
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
            if (!this.SemicolonNeeded())
            {
                flag = false;
            }
            node.Range.EndPoint = this.prevPosition;
            return flag;
        }

        protected virtual bool ParseReturnType()
        {
            return true;
        }

        protected virtual bool ParseSimpleExpression(ref ISyntaxNode node)
        {
            bool flag = true;
            switch (((JavaScriptLexerToken) this.Token))
            {
                case JavaScriptLexerToken.New:
                    return this.ParseNewExpression(ref node);

                case JavaScriptLexerToken.Null:
                case JavaScriptLexerToken.False:
                case JavaScriptLexerToken.True:
                case JavaScriptLexerToken.Integer_Literal:
                case JavaScriptLexerToken.Float_Literal:
                case JavaScriptLexerToken.Double_Literal:
                case JavaScriptLexerToken.Character_Literal:
                case JavaScriptLexerToken.String_Literal:
                case JavaScriptLexerToken.Identifier_Literal:
                    node = new SyntaxNode(this.TokenPosition, this.TokenString, 0x99);
                    this.MoveNext();
                    return flag;

                case JavaScriptLexerToken.This:
                    return this.ParseThisAccess(ref node);

                case JavaScriptLexerToken.Function:
                    return this.ParseFunctionPrototypeExpression(ref node);

                case JavaScriptLexerToken.Typeof:
                    return this.ParseTypeofExpression(ref node);

                case JavaScriptLexerToken.Open_brace:
                    return this.ParseTypeExpression(ref node);

                case JavaScriptLexerToken.Open_parens:
                    return this.ParseParenthesizedExpression(ref node);

                case JavaScriptLexerToken.Open_bracket:
                    return this.ParseArrayInitializerExpression(ref node);

                case JavaScriptLexerToken.Div:
                    return this.ParseRegularExpression(ref node);
            }
            flag = false;
            this.SyntaxError();
            if (this.IsSymbol(this.Token))
            {
                this.SkipSymbols();
                return flag;
            }
            this.MoveNext();
            return flag;
        }

        protected virtual bool ParseStatement(bool block)
        {
            JavaScriptLexerToken token = (JavaScriptLexerToken) this.Token;
            if (token != JavaScriptLexerToken.Const)
            {
                if (token != JavaScriptLexerToken.Var)
                {
                    return this.ParseEmbeddedStatement();
                }
                ISyntaxNode node = null;
                bool flag = this.ParseVariableDeclaration(null, block ? 15 : 13, ref node);
                if (!this.SemicolonNeeded())
                {
                    flag = false;
                }
                if (node != null)
                {
                    node.Range.EndPoint = this.prevPosition;
                }
                return flag;
            }
            ISyntaxNode node2 = null;
            bool flag2 = this.ParseVariableDeclaration(null, 14, ref node2);
            if (!this.SemicolonNeeded())
            {
                flag2 = false;
            }
            if (node2 != null)
            {
                node2.Range.EndPoint = this.prevPosition;
            }
            return flag2;
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
            bool flag = this.ParseStatementExpression();
            while (this.Token == 0x58)
            {
                this.MoveNext();
                if (!this.ParseStatementExpression())
                {
                    flag = false;
                }
            }
            return flag;
        }

        protected virtual bool ParseStatementList(bool block)
        {
            bool flag = true;
            while (!this.Eof && (!block || (this.Token != 0x52)))
            {
                if (!block && (this.Token == 0x1b))
                {
                    if (!this.ParseMethodDeclaration())
                    {
                        flag = false;
                    }
                }
                else if (!this.ParseStatement(block))
                {
                    flag = false;
                }
            }
            return flag;
        }

        protected virtual bool ParseSwitchBlock()
        {
            bool flag = true;
            Point tokenPosition = this.TokenPosition;
            if (this.Expected(JavaScriptLexerToken.Open_brace))
            {
                this.AddAttribute(new SyntaxAttribute(tokenPosition, SyntaxConsts.DefinitionScope, null));
                while (!this.Eof && ((this.Token == 6) || (this.Token == 13)))
                {
                    if (!this.ParseSwitchSection())
                    {
                        flag = false;
                    }
                }
                if (this.Expected(JavaScriptLexerToken.Close_brace))
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
            switch (((JavaScriptLexerToken) this.Token))
            {
                case JavaScriptLexerToken.Case:
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
                case JavaScriptLexerToken.Default:
                    this.MoveNext();
                    break;
            }
            if (!this.Expected(0x59))
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
            while ((this.Token == 6) || (this.Token == 13));
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
                if (this.Token != 0x51)
                {
                    goto Label_0052;
                }
                if (!this.ParseBlock())
                {
                    flag = false;
                }
                goto Label_0086;
            Label_0047:
                if (!this.ParseStatement(true))
                {
                    flag = false;
                }
            Label_0052:
                if ((!this.Eof && (this.Token != 6)) && ((this.Token != 13) && (this.Token != 0x52)))
                {
                    goto Label_0047;
                }
            }
            finally
            {
                this.SyntaxTree.Pop();
            }
        Label_0086:
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

        protected virtual bool ParseThisAccess(ref ISyntaxNode node)
        {
            node = new SyntaxNode(this.TokenPosition, this.TokenString, 0x9e);
            this.MoveNext();
            return true;
        }

        protected virtual bool ParseThrowStatement()
        {
            return this.ParseReturnStatement(NetNodeType.ThrowStatement);
        }

        protected virtual bool ParseTryStatement()
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, this.TokenString, 0x60, SyntaxNodeOptions.Indentation);
            this.AddNode(node);
            this.SyntaxTree.Push(node);
            try
            {
                this.MoveNext();
                if (!this.ParseBlock())
                {
                    flag = false;
                }
                if (this.Token == 7)
                {
                    if (!this.ParseCatchStatement())
                    {
                        flag = false;
                    }
                }
                else
                {
                    this.SyntaxError(7);
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

        protected virtual bool ParseType(out string type)
        {
            bool flag = this.ParseTypeName(out type);
            if (flag)
            {
                string str;
                flag = this.TryParseRankSpecifiers(out str);
                if (flag)
                {
                    type = type + str;
                }
            }
            return flag;
        }

        protected virtual bool ParseTypeExpression(ref ISyntaxNode node)
        {
            bool flag = true;
            node = new SyntaxNode(this.TokenPosition, string.Empty, 0xcb);
            this.MoveNext();
            while (!this.Eof)
            {
                ISyntaxNode node2 = new SyntaxNode(this.TokenPosition, string.Empty, 13);
                node.AddChild(node2);
                ISyntaxNode node3 = null;
                if (!this.ParseExpression(ref node3))
                {
                    flag = false;
                }
                if (node3 != null)
                {
                    node2.AddChild(node3);
                }
                if (!this.Expected(JavaScriptLexerToken.Colon))
                {
                    flag = false;
                }
                if (!this.ParseExpression(ref node3))
                {
                    flag = false;
                }
                if (node3 != null)
                {
                    node2.AddChild(node3);
                }
                node2.Range.EndPoint = this.prevPosition;
                if (this.Token != 0x58)
                {
                    break;
                }
                this.MoveNext();
            }
            if (!this.Expected(JavaScriptLexerToken.Close_brace))
            {
                flag = false;
            }
            node.Range.EndPoint = this.prevPosition;
            return flag;
        }

        protected virtual bool ParseTypeName(out string type)
        {
            if (this.IsBuiltInType(this.Token))
            {
                type = this.TokenString;
                this.MoveNext();
                return true;
            }
            return this.ParseIdentifier(out type);
        }

        protected virtual bool ParseTypeofExpression(ref ISyntaxNode node)
        {
            bool flag = true;
            node = new SyntaxNode(this.TokenPosition, this.TokenString, 0xa6);
            this.MoveNext();
            ISyntaxNode node2 = null;
            if (this.Token == 0x53)
            {
                this.MoveNext();
                flag = this.ParseExpression(ref node2) && this.Expected(JavaScriptLexerToken.Close_parens);
            }
            else
            {
                flag = this.ParseExpression(ref node2);
            }
            if (node2 != null)
            {
                node.AddChild(node2);
            }
            node.Range.EndPoint = this.prevPosition;
            return flag;
        }

        protected virtual bool ParseUnaryExpression(ref ISyntaxNode node)
        {
            return this.ParsePrimaryExpression(ref node);
        }

        protected virtual bool ParseUnit()
        {
            bool flag = true;
            ISyntaxNode root = this.SyntaxTree.Root;
            root.NodeType = 1;
            root.Position = Point.Empty;
            flag = this.ParseUnitBody();
            this.SyntaxTree.Root.Range.EndPoint = this.CurrentPosition;
            if (!this.ClearStack())
            {
                flag = false;
            }
            return flag;
        }

        protected virtual bool ParseUnitBody()
        {
            ISyntaxNode current = this.SyntaxTree.Current;
            current.NodeType = 8;
            current.AddAttribute(new SyntaxAttribute(this.TokenPosition, SyntaxConsts.BlockScope, null));
            return this.ParseDefaultBlock();
        }

        protected virtual bool ParseVariableDeclaration(ISyntaxAttributes attrs, int nodeType)
        {
            ISyntaxNode node = null;
            return this.ParseVariableDeclaration(attrs, nodeType, ref node);
        }

        protected virtual bool ParseVariableDeclaration(ISyntaxAttributes attrs, int nodeType, ref ISyntaxNode node)
        {
            string str;
            bool flag = true;
            Point tokenPosition = this.TokenPosition;
            if ((this.Token == 0x3f) || (this.Token == 10))
            {
                this.MoveNext();
            }
        Label_0024:
            if (this.ParseIdentifier(out str))
            {
                node = new SyntaxNode(tokenPosition, str, nodeType, SyntaxNodeOptions.CodeCompletion | SyntaxNodeOptions.Indentation);
                this.AddNode(node);
                if (attrs != null)
                {
                    node.AddAttributes(attrs);
                }
                this.SyntaxTree.Push(node);
                try
                {
                    if (!this.ParseReturnType())
                    {
                        flag = false;
                    }
                    if (!this.ParseVariableDeclarators())
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
            else
            {
                flag = false;
            }
            if (this.Token == 0x58)
            {
                this.MoveNext();
                if (!this.Eof)
                {
                    goto Label_0024;
                }
            }
            return flag;
        }

        protected virtual bool ParseVariableDeclarators()
        {
            bool flag = true;
            if (this.Token == 0x69)
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
            return flag;
        }

        protected virtual bool ParseVariableInitializer()
        {
            bool flag = true;
            ISyntaxNode node = null;
            flag = this.ParseExpression(ref node);
            if (node != null)
            {
                this.AddNode(node);
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

        public override bool ReparseBlock(Point position)
        {
            this.ReparseText();
            return true;
        }

        public override void ReparseText()
        {
            this.Reset();
            this.MoveNext();
            this.ParseUnit();
            base.ReparseText();
        }

        public override void ResetAutoIndentChars()
        {
            this.AutoIndentChars = SyntaxParserConsts.DefaultCsAutoIndentChars.ToCharArray();
        }

        public override void ResetCodeCompletionChars()
        {
            this.CodeCompletionChars = SyntaxParserConsts.DefaultNetCodeCompletionChars.ToCharArray();
        }

        public override void ResetCodeCompletionStopChars()
        {
            this.CodeCompletionStopChars = SyntaxParserConsts.DefaultNetCodeCompletionStopChars.ToCharArray();
        }

        public override void ResetOptions()
        {
            this.Options = SyntaxParserConsts.DefaultNetSyntaxOptions;
        }

        public override void ResetSmartFormatChars()
        {
            this.SmartFormatChars = SyntaxParserConsts.DefaultCsSmartFormatChars.ToCharArray();
        }

        protected virtual bool SemicolonNeeded()
        {
            if (this.Token == 90)
            {
                this.MoveNext();
            }
            return true;
        }

        public override bool ShouldSerializeAutoIndentChars()
        {
            return (new string(this.AutoIndentChars) != SyntaxParserConsts.DefaultCsAutoIndentChars);
        }

        public override bool ShouldSerializeCodeCompletionChars()
        {
            return (new string(this.CodeCompletionChars) != SyntaxParserConsts.DefaultNetCodeCompletionChars);
        }

        public override bool ShouldSerializeCodeCompletionStopChars()
        {
            return (new string(this.CodeCompletionStopChars) != SyntaxParserConsts.DefaultNetCodeCompletionStopChars);
        }

        public override bool ShouldSerializeSmartFormatChars()
        {
            return (new string(this.SmartFormatChars) != SyntaxParserConsts.DefaultCsSmartFormatChars);
        }

        protected virtual void SkipSymbols()
        {
            this.MoveNext();
            while (this.IsSymbol(this.Token))
            {
                this.MoveNext();
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
                if (this.IsReswordToken(this.Token))
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
                ISyntaxError err = new QWhale.Syntax.SyntaxError(this.TokenPosition, this.TokenString, ((JavaScriptLexerToken) token).ToString() + " " + StringConsts.ErrExpected);
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

        protected virtual bool TryParseAssignmentExpression(ref ISyntaxNode node)
        {
            bool flag = true;
            switch (((JavaScriptLexerToken) this.Token))
            {
                case JavaScriptLexerToken.Op_mult_assign:
                case JavaScriptLexerToken.Op_div_assign:
                case JavaScriptLexerToken.Op_mod_assign:
                case JavaScriptLexerToken.Assign:
                case JavaScriptLexerToken.Op_add_assign:
                case JavaScriptLexerToken.Op_sub_assign:
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

        protected virtual bool TryParsePostPrimaryExpression(ref ISyntaxNode node)
        {
            bool flag = true;
            while (!this.Eof)
            {
                switch (((JavaScriptLexerToken) this.Token))
                {
                    case JavaScriptLexerToken.Open_parens:
                    {
                        if (!this.ParseInvocationExpression(ref node))
                        {
                            flag = false;
                        }
                        continue;
                    }
                    case JavaScriptLexerToken.Close_parens:
                    case JavaScriptLexerToken.Close_bracket:
                        return flag;

                    case JavaScriptLexerToken.Open_bracket:
                    {
                        if (!this.ParseElementAccess(ref node))
                        {
                            flag = false;
                        }
                        continue;
                    }
                    case JavaScriptLexerToken.Dot:
                    {
                        if (!this.ParseMemberAccess(ref node))
                        {
                            flag = false;
                        }
                        continue;
                    }
                    case JavaScriptLexerToken.Op_inc:
                    {
                        if (!this.ParsePostIncrementExpression(ref node))
                        {
                            flag = false;
                        }
                        continue;
                    }
                    case JavaScriptLexerToken.Op_dec:
                    {
                        if (!this.ParsePostDecrementExpression(ref node))
                        {
                            flag = false;
                        }
                        continue;
                    }
                }
                return flag;
            }
            return flag;
        }

        protected virtual bool TryParseRankSpecifiers(out string rank)
        {
            bool flag = true;
            rank = string.Empty;
            while (this.Token == 0x55)
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

        public override bool CaseSensitive
        {
            get
            {
                return true;
            }
        }

        internal class JavaScriptBuiltInFunctions
        {
            public static string Escape(string CharString)
            {
                return string.Empty;
            }

            public static object Eval(string CodeString)
            {
                return null;
            }

            public static bool IsFinite(object TestNumber)
            {
                return false;
            }

            public static bool IsNaN(object TestValue)
            {
                return false;
            }

            public static double Number(object Value)
            {
                return 0.0;
            }

            public static float ParseFloat(string CharString)
            {
                return 0f;
            }

            public static int ParseInt(string CharString, int radIx)
            {
                return 0;
            }

            public static string String(object Value)
            {
                return string.Empty;
            }

            public static string Unescape(string EncodeString)
            {
                return string.Empty;
            }
        }
    }
}

