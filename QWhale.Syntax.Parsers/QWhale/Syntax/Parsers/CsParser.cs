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

    [ToolboxBitmap(typeof(CsParser), "Images.CsParser.bmp"), ToolboxItem(true)]
    public class CsParser : NetSyntaxParser
    {
        protected LexerProc lexCommentEndProc;
        protected LexerProc lexCommentProc;
        protected LexerProc lexDefineEndProc;
        protected LexerProc lexDefineProc;
        protected LexerProc lexIdentifierProc;
        protected LexerProc lexNumberProc;
        protected LexerProc lexStringEndProc;
        protected LexerProc lexStringProc;
        protected LexerProc lexSymbolProc;
        protected LexerProc lexWhitespaceProc;
        protected LexerProc lexXmlCommentProc;
        protected LexerProc lexXmlCommentTagProc;
        protected Hashtable reswords;
        private const int stateComment = 1;
        private const int stateDefine = 3;
        private const int stateNormal = 0;
        private const int stateString = 4;
        private const int stateXmlComment = 2;

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
            IReflectionRepository repository = this.InternalCreateRepository();
            repository.RegisterDefaultAssemblies();
            repository.RegisterType("void", typeof(void));
            repository.RegisterType("bool", typeof(bool));
            repository.RegisterType("byte", typeof(byte));
            repository.RegisterType("char", typeof(char));
            repository.RegisterType("decimal", typeof(decimal));
            repository.RegisterType("double", typeof(double));
            repository.RegisterType("float", typeof(float));
            repository.RegisterType("int", typeof(int));
            repository.RegisterType("long", typeof(long));
            repository.RegisterType("object", typeof(object));
            repository.RegisterType("sbyte", typeof(sbyte));
            repository.RegisterType("short", typeof(short));
            repository.RegisterType("string", typeof(string));
            repository.RegisterType("uint", typeof(uint));
            repository.RegisterType("ulong", typeof(ulong));
            repository.RegisterType("ushort", typeof(ushort));
            repository.RegisterSnippet("as", false);
            repository.RegisterSnippet("base", false);
            repository.RegisterSnippet("break", false);
            repository.RegisterSnippet("const", false);
            repository.RegisterSnippet("case", false);
            repository.RegisterSnippet("catch", false);
            repository.RegisterSnippet("continue", false);
            repository.RegisterSnippet("default", false);
            repository.RegisterSnippet("delegate", false);
            repository.RegisterSnippet("false", false);
            repository.RegisterSnippet("finally", false);
            repository.RegisterSnippet("fixed", false);
            repository.RegisterSnippet("goto", false);
            repository.RegisterSnippet("in", false);
            repository.RegisterSnippet("new", false);
            repository.RegisterSnippet("is", false);
            repository.RegisterSnippet("null", false);
            repository.RegisterSnippet("out", false);
            repository.RegisterSnippet("ref", false);
            repository.RegisterSnippet("return", false);
            repository.RegisterSnippet("sizeof", false);
            repository.RegisterSnippet("stackalloc", false);
            repository.RegisterSnippet("this", false);
            repository.RegisterSnippet("throw", false);
            repository.RegisterSnippet("true", false);
            repository.RegisterSnippet("typeof", false);
            repository.RegisterSnippet("virtual", false);
            repository.RegisterSnippet("void", false);
            repository.RegisterSnippet("yield", false);
            repository.RegisterSnippet("checked", true);
            repository.RegisterSnippet("class", true);
            repository.RegisterSnippet("do", true);
            repository.RegisterSnippet("else", true);
            repository.RegisterSnippet("enum", true);
            repository.RegisterSnippet("for", true);
            repository.RegisterSnippet("foreach", true);
            repository.RegisterSnippet("if", true);
            repository.RegisterSnippet("interface", true);
            repository.RegisterSnippet("lock", true);
            repository.RegisterSnippet("namespace", true);
            repository.RegisterSnippet("struct", true);
            repository.RegisterSnippet("switch", true);
            repository.RegisterSnippet("unchecked", true);
            repository.RegisterSnippet("unsafe", true);
            repository.RegisterSnippet("using", true);
            repository.RegisterSnippet("while", true);
            return repository;
        }

        protected bool Expected(CsLexerToken token)
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
            return this.Expected((CsLexerToken) token);
        }

        protected bool Expected(CsLexerToken token1, CsLexerToken token2)
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

        public override CodeCompletionType GetCompletionType(char ch)
        {
            switch (ch)
            {
                case ' ':
                    return CodeCompletionType.SpecialListMembers;

                case '/':
                    return CodeCompletionType.CompleteComment;

                case '<':
                    return CodeCompletionType.ListMembers;

                case '>':
                    return CodeCompletionType.CompleteWord;

                case '[':
                    return CodeCompletionType.ParameterInfo;
            }
            return base.GetCompletionType(ch);
        }

        protected virtual string GetExpressionType(ISyntaxNode node, int token, out Point typePos)
        {
            typePos = node.Position;
            switch (node.NodeType)
            {
                case 0x99:
                    switch (((CsLexerToken) token))
                    {
                        case CsLexerToken.False:
                        case CsLexerToken.True:
                            return CsLexerToken.Bool.ToString().ToLower();

                        case CsLexerToken.Integer_Literal:
                            return CsLexerToken.Int.ToString().ToLower();

                        case CsLexerToken.Float_Literal:
                            return CsLexerToken.Float.ToString().ToLower();

                        case CsLexerToken.Double_Literal:
                            return CsLexerToken.Double.ToString().ToLower();

                        case CsLexerToken.Decimal_Literal:
                            return CsLexerToken.Decimal.ToString().ToLower();

                        case CsLexerToken.Character_Literal:
                            return CsLexerToken.Char.ToString().ToLower();

                        case CsLexerToken.String_Literal:
                            return CsLexerToken.String.ToString().ToLower();
                    }
                    break;

                case 0x9a:
                case 0x9b:
                {
                    ISyntaxAttribute attribute = node.FindAttribute(NetNodeType.Type.ToString());
                    if (attribute == null)
                    {
                        break;
                    }
                    typePos = attribute.Position;
                    return (attribute.Value.ToString() + ((node.NodeType == 0x9a) ? "[]" : string.Empty));
                }
            }
            return string.Empty;
        }

        protected override int GetLexerStyle(int token)
        {
            if (this.IsKeywordToken(token))
            {
                return 2;
            }
            if ((token >= 0x67) && (token <= 150))
            {
                return 5;
            }
            switch (token)
            {
                case 0x98:
                case 0x99:
                case 0x9a:
                case 0x9b:
                    return 1;

                case 0x9c:
                case 0x9d:
                    return 7;

                case 0x9e:
                    return 0;

                case 0x9f:
                case 0xa1:
                    return 3;

                case 160:
                    return 4;

                case 0xa2:
                    return 8;
            }
            return 6;
        }

        public override string GetSingleLineComment()
        {
            return "//";
        }

        public override string GetXmlComment()
        {
            return "///";
        }

        protected bool IdentifierExpected()
        {
            return this.Expected(CsLexerToken.Identifier_Literal, CsLexerToken.Value);
        }

        protected override void InitLanguage()
        {
            this.Scheme.FileType = "c#";
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
            this.lexStringEndProc = new LexerProc(this.LexStringEnd);
            this.lexCommentProc = new LexerProc(this.LexComment);
            this.lexCommentEndProc = new LexerProc(this.LexCommentEnd);
            this.lexDefineProc = new LexerProc(this.LexDefine);
            this.lexDefineEndProc = new LexerProc(this.LexDefineEnd);
            this.lexXmlCommentProc = new LexerProc(this.LexXmlComment);
            this.lexXmlCommentTagProc = new LexerProc(this.LexXmlCommentTag);
            base.RegisterLexerProc(0, this.lexWhitespaceProc);
            base.RegisterLexerProc(0, '!', '\x00ff', this.lexSymbolProc);
            base.RegisterLexerProc(0, 'a', 'z', this.lexIdentifierProc);
            base.RegisterLexerProc(0, 'A', 'Z', this.lexIdentifierProc);
            base.RegisterLexerProc(0, '_', this.lexIdentifierProc);
            base.RegisterLexerProc(0, '0', '9', this.lexNumberProc);
            base.RegisterLexerProc(0, new char[] { '+', '-' }, this.lexSymbolProc);
            base.RegisterLexerProc(0, new char[] { '@', '"', '\'' }, this.lexStringProc);
            base.RegisterLexerProc(0, '/', this.lexCommentProc);
            base.RegisterLexerProc(0, '#', this.lexDefineProc);
            base.RegisterLexerProc(4, this.lexStringEndProc);
            base.RegisterLexerProc(1, this.lexCommentEndProc);
            base.RegisterLexerProc(2, this.lexXmlCommentProc);
            base.RegisterLexerProc(2, '<', this.lexXmlCommentTagProc);
            base.RegisterLexerProc(3, this.lexDefineEndProc);
        }

        protected virtual void InitReswords()
        {
            this.reswords = new Hashtable();
            this.reswords.Add("abstract", CsLexerToken.Abstract);
            this.reswords.Add("as", CsLexerToken.As);
            this.reswords.Add("add", CsLexerToken.Add);
            this.reswords.Add("base", CsLexerToken.Base);
            this.reswords.Add("bool", CsLexerToken.Bool);
            this.reswords.Add("break", CsLexerToken.Break);
            this.reswords.Add("byte", CsLexerToken.Byte);
            this.reswords.Add("case", CsLexerToken.Case);
            this.reswords.Add("catch", CsLexerToken.Catch);
            this.reswords.Add("char", CsLexerToken.Char);
            this.reswords.Add("checked", CsLexerToken.Checked);
            this.reswords.Add("class", CsLexerToken.Class);
            this.reswords.Add("const", CsLexerToken.Consts);
            this.reswords.Add("continue", CsLexerToken.Continue);
            this.reswords.Add("decimal", CsLexerToken.Decimal);
            this.reswords.Add("default", CsLexerToken.Default);
            this.reswords.Add("delegate", CsLexerToken.Delegate);
            this.reswords.Add("do", CsLexerToken.Do);
            this.reswords.Add("double", CsLexerToken.Double);
            this.reswords.Add("else", CsLexerToken.Else);
            this.reswords.Add("enum", CsLexerToken.Enum);
            this.reswords.Add("event", CsLexerToken.Event);
            this.reswords.Add("explicit", CsLexerToken.Explicit);
            this.reswords.Add("extern", CsLexerToken.Extern);
            this.reswords.Add("false", CsLexerToken.False);
            this.reswords.Add("finally", CsLexerToken.Finally);
            this.reswords.Add("fixed", CsLexerToken.Fixed);
            this.reswords.Add("float", CsLexerToken.Float);
            this.reswords.Add("for", CsLexerToken.For);
            this.reswords.Add("foreach", CsLexerToken.Foreach);
            this.reswords.Add("goto", CsLexerToken.Goto);
            this.reswords.Add("if", CsLexerToken.If);
            this.reswords.Add("implicit", CsLexerToken.Implicit);
            this.reswords.Add("in", CsLexerToken.In);
            this.reswords.Add("int", CsLexerToken.Int);
            this.reswords.Add("interface", CsLexerToken.Interface);
            this.reswords.Add("internal", CsLexerToken.Internal);
            this.reswords.Add("is", CsLexerToken.Is);
            this.reswords.Add("lock", CsLexerToken.Lock);
            this.reswords.Add("long", CsLexerToken.Long);
            this.reswords.Add("namespace", CsLexerToken.Namespace);
            this.reswords.Add("new", CsLexerToken.New);
            this.reswords.Add("null", CsLexerToken.Null);
            this.reswords.Add("object", CsLexerToken.Object);
            this.reswords.Add("operator", CsLexerToken.Operator);
            this.reswords.Add("out", CsLexerToken.Out);
            this.reswords.Add("override", CsLexerToken.Override);
            this.reswords.Add("params", CsLexerToken.Params);
            this.reswords.Add("private", CsLexerToken.Private);
            this.reswords.Add("protected", CsLexerToken.Protected);
            this.reswords.Add("public", CsLexerToken.Public);
            this.reswords.Add("readonly", CsLexerToken.Readonly);
            this.reswords.Add("ref", CsLexerToken.Ref);
            this.reswords.Add("return", CsLexerToken.Return);
            this.reswords.Add("remove", CsLexerToken.Remove);
            this.reswords.Add("sbyte", CsLexerToken.Sbyte);
            this.reswords.Add("sealed", CsLexerToken.Sealed);
            this.reswords.Add("short", CsLexerToken.Short);
            this.reswords.Add("sizeof", CsLexerToken.Sizeof);
            this.reswords.Add("stackalloc", CsLexerToken.Stackalloc);
            this.reswords.Add("static", CsLexerToken.Static);
            this.reswords.Add("string", CsLexerToken.String);
            this.reswords.Add("struct", CsLexerToken.Struct);
            this.reswords.Add("switch", CsLexerToken.Switch);
            this.reswords.Add("this", CsLexerToken.This);
            this.reswords.Add("throw", CsLexerToken.Throw);
            this.reswords.Add("true", CsLexerToken.True);
            this.reswords.Add("try", CsLexerToken.Try);
            this.reswords.Add("typeof", CsLexerToken.Typeof);
            this.reswords.Add("uint", CsLexerToken.Uint);
            this.reswords.Add("ulong", CsLexerToken.Ulong);
            this.reswords.Add("unchecked", CsLexerToken.Unchecked);
            this.reswords.Add("unsafe", CsLexerToken.Unsafe);
            this.reswords.Add("ushort", CsLexerToken.Ushort);
            this.reswords.Add("using", CsLexerToken.Using);
            this.reswords.Add("virtual", CsLexerToken.Virtual);
            this.reswords.Add("void", CsLexerToken.Void);
            this.reswords.Add("volatile", CsLexerToken.Volatile);
            this.reswords.Add("while", CsLexerToken.While);
            this.reswords.Add("alias", CsLexerToken.Alias);
            this.reswords.Add("where", CsLexerToken.Where);
            this.reswords.Add("argList", CsLexerToken.ArgList);
            this.reswords.Add("partial", CsLexerToken.Partial);
            this.reswords.Add("yield", CsLexerToken.Yield);
            this.reswords.Add("value", CsLexerToken.Value);
            this.reswords.Add("global", CsLexerToken.Global);
            this.reswords.Add("get", CsLexerToken.Get);
            this.reswords.Add("set", CsLexerToken.Set);
            this.reswords.Add("var", CsLexerToken.Var);
            this.reswords.Add("from", CsLexerToken.From);
            this.reswords.Add("join", CsLexerToken.Join);
            this.reswords.Add("on", CsLexerToken.On);
            this.reswords.Add("equals", CsLexerToken.Equals);
            this.reswords.Add("into", CsLexerToken.Into);
            this.reswords.Add("let", CsLexerToken.Let);
            this.reswords.Add("orderby", CsLexerToken.OrderBy);
            this.reswords.Add("ascending", CsLexerToken.Ascending);
            this.reswords.Add("descending", CsLexerToken.Descending);
            this.reswords.Add("select", CsLexerToken.Select);
            this.reswords.Add("group", CsLexerToken.Group);
            this.reswords.Add("by", CsLexerToken.By);
        }

        protected virtual IReflectionRepository InternalCreateRepository()
        {
            return new CsRepository(this.CaseSensitive, this.SyntaxTree);
        }

        protected virtual bool IsAccessor(NetNodeType accessorType)
        {
            return (((this.Token == 0x65) || ((this.Token == 0x66) && (accessorType == NetNodeType.PropertyAccessor))) || ((this.Token == 3) || ((this.Token == 0x38) && (accessorType == NetNodeType.EventAccessor))));
        }

        protected virtual bool IsAccessorModifier(int token)
        {
            switch (((CsLexerToken) token))
            {
                case CsLexerToken.Private:
                case CsLexerToken.Protected:
                case CsLexerToken.Internal:
                    return true;
            }
            return false;
        }

        protected virtual bool IsBaseList(int token)
        {
            return (token == 0x6f);
        }

        protected virtual bool IsBuiltInType(int token)
        {
            switch (((CsLexerToken) token))
            {
                case CsLexerToken.Double:
                case CsLexerToken.Float:
                case CsLexerToken.Int:
                case CsLexerToken.Bool:
                case CsLexerToken.Byte:
                case CsLexerToken.Char:
                case CsLexerToken.Decimal:
                case CsLexerToken.Sbyte:
                case CsLexerToken.Short:
                case CsLexerToken.Object:
                case CsLexerToken.Long:
                case CsLexerToken.Uint:
                case CsLexerToken.Ulong:
                case CsLexerToken.Ushort:
                case CsLexerToken.Void:
                case CsLexerToken.String:
                    return true;
            }
            return false;
        }

        public override bool IsCodeCompletionChar(char ch, byte style, ref int interval)
        {
            if ((style != 5) || (((ch != '/') && (ch != '<')) && (ch != '>')))
            {
                return base.IsCodeCompletionChar(ch, style, ref interval);
            }
            return true;
        }

        protected override bool IsComment(int tok)
        {
            if (tok != 0x9f)
            {
                return this.IsXmlComment(tok);
            }
            return true;
        }

        protected bool IsFromLetWhereClause()
        {
            if ((this.Token != 90) && (this.Token != 0x62))
            {
                return (this.Token == 0x53);
            }
            return true;
        }

        protected bool IsIdentifierToken(int token)
        {
            if ((token != 0x9e) && (token != 0x55))
            {
                return (token == 0x57);
            }
            return true;
        }

        protected virtual bool IsImplicitVariableDeclaration(int token)
        {
            return (token == 0x58);
        }

        protected virtual bool IsKeywordToken(int token)
        {
            return ((token >= 1) && (token <= 0x66));
        }

        protected virtual bool IsModifier(int token)
        {
            switch (((CsLexerToken) token))
            {
                case CsLexerToken.Extern:
                case CsLexerToken.Fixed:
                case CsLexerToken.Internal:
                case CsLexerToken.Abstract:
                case CsLexerToken.Consts:
                case CsLexerToken.New:
                case CsLexerToken.Override:
                case CsLexerToken.Private:
                case CsLexerToken.Protected:
                case CsLexerToken.Public:
                case CsLexerToken.Readonly:
                case CsLexerToken.Sealed:
                case CsLexerToken.Static:
                case CsLexerToken.Virtual:
                case CsLexerToken.Volatile:
                case CsLexerToken.Partial:
                case CsLexerToken.Unsafe:
                    return true;
            }
            return false;
        }

        protected virtual bool IsNullableType(int token)
        {
            return (token == 0x7f);
        }

        protected bool IsOrderingDirection()
        {
            if (this.Token != 0x63)
            {
                return (this.Token == 100);
            }
            return true;
        }

        protected virtual bool IsPointerType(int token)
        {
            return (token == 0x7b);
        }

        protected virtual bool IsTypeArgumentList(int token)
        {
            return (token == 0x77);
        }

        protected virtual bool IsTypeParameterConstraintsClause(int token)
        {
            return (token == 0x53);
        }

        protected virtual bool IsTypeParameterList(int token)
        {
            return (token == 0x77);
        }

        protected override bool IsValidToken(int tok)
        {
            return ((tok != 0xa3) && !this.IsComment(tok));
        }

        protected virtual bool IsXmlComment(int tok)
        {
            if (tok != 160)
            {
                return (tok == 0xa1);
            }
            return true;
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
                        int num2 = base.currentPos + 1;
                        if ((num2 < length) && (base.source[num2] == '/'))
                        {
                            base.currentPos = num2 + 1;
                            if (base.currentPos < length)
                            {
                                this.State = 2;
                            }
                            return 160;
                        }
                        base.currentPos = length;
                        return 0x9f;
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
                        return 0x9f;
                    }
                }
                else
                {
                    base.currentPos++;
                }
            }
            this.State = 1;
            return 0x9f;
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
            this.State = (base.currentPos == length) ? 0 : 3;
            return 0xa2;
        }

        protected virtual int LexDefineEnd()
        {
            base.currentPos = base.source.Length;
            this.State = 0;
            return 0x9e;
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
            return 0x98;
        }

        protected virtual int LexIdentifier()
        {
            this.LexIdent();
            object obj2 = this.reswords[this.TokenString];
            if (obj2 == null)
            {
                return 0x9e;
            }
            return (int) obj2;
        }

        protected virtual int LexNumber()
        {
            int num2;
            char ch = base.source[base.currentPos];
            int length = base.source.Length;
            int num3 = 0x98;
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
                switch (ch)
                {
                    case 'd':
                    case 'D':
                        base.currentPos++;
                        return 0x9a;

                    case 'f':
                    case 'F':
                        base.currentPos++;
                        return 0x99;

                    case 'm':
                    case 'M':
                        base.currentPos++;
                        return 0x9b;

                    case '.':
                        if (base.currentPos < (length - 1))
                        {
                            ch = base.source[base.currentPos + 1];
                            if ((ch >= '0') && (ch <= '9'))
                            {
                                base.currentPos++;
                                this.LexNum();
                                num3 = 0x99;
                                if (base.currentPos < length)
                                {
                                    ch = base.source[base.currentPos];
                                    switch (ch)
                                    {
                                        case 'F':
                                        case 'f':
                                            base.currentPos++;
                                            return 0x99;
                                    }
                                }
                            }
                        }
                        break;
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
                        num3 = 0x9a;
                    }
                }
            }
            return num3;
        }

        protected virtual int LexString()
        {
            char startChar = base.source[base.currentPos];
            base.currentPos++;
            if (startChar != '@')
            {
                return this.LexString(startChar);
            }
            int length = base.source.Length;
            if (base.currentPos < length)
            {
                startChar = base.source[base.currentPos];
                if (startChar == '"')
                {
                    base.currentPos++;
                    return this.LexStringEnd();
                }
            }
            return 5;
        }

        protected virtual int LexString(char startChar)
        {
            int length = base.source.Length;
            bool flag = false;
            while (base.currentPos < length)
            {
                char ch = base.source[base.currentPos];
                if ((ch == startChar) && !flag)
                {
                    base.currentPos++;
                    break;
                }
                if (ch == '\\')
                {
                    flag = !flag;
                }
                else
                {
                    flag = false;
                }
                base.currentPos++;
            }
            if (startChar != '\'')
            {
                return 0x9d;
            }
            return 0x9c;
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
                        this.State = 0;
                        return 0x9d;
                    }
                    base.currentPos++;
                }
                else
                {
                    base.currentPos++;
                }
            }
            this.State = 4;
            return 0x9d;
        }

        protected virtual int LexSymbol()
        {
            CsLexerToken bang = CsLexerToken.Whitespace_Literal;
            char ch = base.source[base.currentPos];
            base.currentPos++;
            switch (ch)
            {
                case '!':
                    if (base.CurChar() != '=')
                    {
                        bang = CsLexerToken.Bang;
                        break;
                    }
                    base.currentPos++;
                    bang = CsLexerToken.Op_ne;
                    break;

                case '%':
                    if (base.CurChar() != '=')
                    {
                        bang = CsLexerToken.Percent;
                        break;
                    }
                    base.currentPos++;
                    bang = CsLexerToken.Op_mod_assign;
                    break;

                case '&':
                    switch (base.CurChar())
                    {
                        case '&':
                            base.currentPos++;
                            bang = CsLexerToken.Op_and;
                            goto Label_04EC;

                        case '=':
                            base.currentPos++;
                            bang = CsLexerToken.Op_and_assign;
                            goto Label_04EC;
                    }
                    bang = CsLexerToken.Bitwise_and;
                    break;

                case '(':
                    bang = CsLexerToken.Open_parens;
                    break;

                case ')':
                    bang = CsLexerToken.Close_parens;
                    break;

                case '*':
                    if (base.CurChar() != '=')
                    {
                        bang = CsLexerToken.Star;
                        break;
                    }
                    base.currentPos++;
                    bang = CsLexerToken.Op_mult_assign;
                    break;

                case '+':
                    switch (base.CurChar())
                    {
                        case '+':
                            base.currentPos++;
                            bang = CsLexerToken.Op_inc;
                            goto Label_04EC;

                        case '=':
                            base.currentPos++;
                            bang = CsLexerToken.Op_add_assign;
                            goto Label_04EC;
                    }
                    bang = CsLexerToken.Plus;
                    break;

                case ',':
                    bang = CsLexerToken.Comma;
                    break;

                case '-':
                    switch (base.CurChar())
                    {
                        case '=':
                            base.currentPos++;
                            bang = CsLexerToken.Op_sub_assign;
                            goto Label_04EC;

                        case '>':
                            base.currentPos++;
                            bang = CsLexerToken.Op_ptr;
                            goto Label_04EC;

                        case '-':
                            base.currentPos++;
                            bang = CsLexerToken.Op_dec;
                            goto Label_04EC;
                    }
                    bang = CsLexerToken.Minus;
                    break;

                case '.':
                    bang = CsLexerToken.Dot;
                    break;

                case '/':
                    if (base.CurChar() != '=')
                    {
                        bang = CsLexerToken.Div;
                        break;
                    }
                    base.currentPos++;
                    bang = CsLexerToken.Op_div_assign;
                    break;

                case ':':
                    if (base.CurChar() != ':')
                    {
                        bang = CsLexerToken.Colon;
                        break;
                    }
                    base.currentPos++;
                    bang = CsLexerToken.DblColon;
                    break;

                case ';':
                    bang = CsLexerToken.Semicolon;
                    break;

                case '<':
                    switch (base.CurChar())
                    {
                        case '<':
                            base.currentPos++;
                            if (base.CurChar() == '=')
                            {
                                base.currentPos++;
                                bang = CsLexerToken.Op_shift_left_assign;
                            }
                            else
                            {
                                bang = CsLexerToken.Op_shift_left;
                            }
                            goto Label_04EC;

                        case '=':
                            base.currentPos++;
                            bang = CsLexerToken.Op_le;
                            goto Label_04EC;
                    }
                    bang = CsLexerToken.Op_lt;
                    break;

                case '=':
                    switch (base.CurChar())
                    {
                        case '=':
                            base.currentPos++;
                            bang = CsLexerToken.Op_eq;
                            goto Label_04EC;

                        case '>':
                            base.currentPos++;
                            bang = CsLexerToken.Lambda;
                            goto Label_04EC;
                    }
                    bang = CsLexerToken.Assign;
                    break;

                case '>':
                    switch (base.CurChar())
                    {
                        case '=':
                            base.currentPos++;
                            bang = CsLexerToken.Op_ge;
                            goto Label_04EC;

                        case '>':
                            base.currentPos++;
                            if (base.CurChar() == '=')
                            {
                                base.currentPos++;
                                bang = CsLexerToken.Op_shift_right_assign;
                            }
                            else
                            {
                                bang = CsLexerToken.Op_shift_right;
                            }
                            goto Label_04EC;
                    }
                    bang = CsLexerToken.Op_gt;
                    break;

                case '?':
                    if (base.CurChar() != '?')
                    {
                        bang = CsLexerToken.Interr;
                        break;
                    }
                    base.currentPos++;
                    bang = CsLexerToken.DblInterr;
                    break;

                case '[':
                    bang = CsLexerToken.Open_bracket;
                    break;

                case ']':
                    bang = CsLexerToken.Close_bracket;
                    break;

                case '^':
                    if (base.CurChar() != '=')
                    {
                        bang = CsLexerToken.Carret;
                        break;
                    }
                    base.currentPos++;
                    bang = CsLexerToken.Op_xor_assign;
                    break;

                case '{':
                    bang = CsLexerToken.Open_brace;
                    break;

                case '|':
                {
                    char ch9 = base.CurChar();
                    if (ch9 == '=')
                    {
                        base.currentPos++;
                        bang = CsLexerToken.Op_or_assign;
                        break;
                    }
                    if (ch9 != '|')
                    {
                        bang = CsLexerToken.Bitwise_or;
                        break;
                    }
                    base.currentPos++;
                    bang = CsLexerToken.Op_or;
                    break;
                }
                case '}':
                    bang = CsLexerToken.Close_brace;
                    break;

                case '~':
                    bang = CsLexerToken.Tilde;
                    break;
            }
        Label_04EC:
            return (int) bang;
        }

        protected virtual int LexWhitespace()
        {
            this.LexSpace();
            return 0xa3;
        }

        protected virtual int LexXmlComment()
        {
            int length = base.source.Length;
            base.currentPos++;
            while (base.currentPos < length)
            {
                if (base.source[base.currentPos] == '<')
                {
                    break;
                }
                base.currentPos++;
            }
            if (base.currentPos == length)
            {
                this.State = 0;
            }
            return 0xa1;
        }

        protected virtual int LexXmlCommentTag()
        {
            base.currentPos++;
            int length = base.source.Length;
            while (base.currentPos < length)
            {
                if (base.source[base.currentPos] == '>')
                {
                    base.currentPos++;
                    break;
                }
                base.currentPos++;
            }
            if (base.currentPos == length)
            {
                this.State = 0;
            }
            return 160;
        }

        protected override int MoveNext()
        {
            int num = base.MoveNext();
            while (!this.Eof && (this.State == 4))
            {
                num = this.NextToken();
            }
            return num;
        }

        protected virtual bool ParseAccessorBody()
        {
            return this.ParseMethodBody();
        }

        protected virtual bool ParseAccessorModifiers(ref ISyntaxAttributes attrs)
        {
            bool flag = false;
            while (this.IsAccessorModifier(this.Token))
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

        protected virtual bool ParseAccessors(ISyntaxAttributes attrs, NetNodeType accessorType)
        {
            bool flag = true;
            if (this.IsAccessor(accessorType))
            {
                ISyntaxNode node = new SyntaxNode(((attrs != null) && (attrs.Count > 0)) ? attrs[0].Position : this.TokenPosition, this.TokenString, (int) accessorType);
                this.AddNode(node);
                if (attrs != null)
                {
                    node.AddAttributes(attrs);
                }
                node.AddAttribute(new SyntaxAttribute(this.CurrentPosition, SyntaxConsts.DeclarationScope, null));
                this.SyntaxTree.Push(node);
                try
                {
                    this.MoveNext();
                    if (!this.ParseAccessorBody())
                    {
                        flag = false;
                    }
                }
                finally
                {
                    this.SyntaxTree.Pop();
                }
                node.Range.EndPoint = base.prevPosition;
                return flag;
            }
            this.SyntaxError();
            return false;
        }

        protected virtual bool ParseAdditiveExpression(ref ISyntaxNode node)
        {
            bool flag = this.ParseMultiplicativeExpression(ref node);
            switch (this.Token)
            {
                case 0x73:
                case 0x74:
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
                    node.Range.EndPoint = base.prevPosition;
                    return flag;
                }
            }
            return flag;
        }

        protected virtual bool ParseAliasDeclaration(ISyntaxAttributes attrs)
        {
            string str;
            bool flag = true;
            Point tokenPosition = this.TokenPosition;
            if (this.ParseIdentifier(out str))
            {
                ISyntaxNode node = new SyntaxNode(tokenPosition, str, 5);
                this.AddNode(node);
                if (this.Expected(CsLexerToken.Semicolon))
                {
                    flag = false;
                }
                node.Range.EndPoint = base.prevPosition;
                return flag;
            }
            return false;
        }

        protected virtual bool ParseAliasList(ISyntaxAttributes attrs)
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(((attrs != null) && (attrs.Count > 0)) ? attrs[0].Position : this.TokenPosition, string.Empty, 6, SyntaxNodeOptions.Outlining);
            this.AddNode(node);
            if (!this.BeforeDeclaration(node))
            {
                flag = false;
            }
            this.SyntaxTree.Push(node);
            try
            {
                if (attrs == null)
                {
                    attrs = new SyntaxAttributes();
                }
                while (this.Token == 0x56)
                {
                    this.MoveNext();
                    if (!this.ParseAliasDeclaration(attrs))
                    {
                        flag = false;
                    }
                    attrs.Clear();
                    if (this.Token != 0x19)
                    {
                        goto Label_00BE;
                    }
                    attrs.Add(new SyntaxAttribute(this.TokenPosition, NetNodeType.Modifier.ToString(), this.TokenString));
                    this.MoveNext();
                }
            }
            finally
            {
                this.SyntaxTree.Pop();
            }
        Label_00BE:
            if (!this.AfterDeclaration(node))
            {
                flag = false;
            }
            node.Range.EndPoint = base.prevPosition;
            return flag;
        }

        protected virtual bool ParseAndExpression(ref ISyntaxNode node)
        {
            bool flag = this.ParseEqualityExpression(ref node);
            if (this.Token == 0x79)
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
                node.Range.EndPoint = base.prevPosition;
            }
            return flag;
        }

        protected virtual bool ParseAnonymousMethodExpression(ref ISyntaxNode node)
        {
            bool flag = true;
            node = new SyntaxNode(this.TokenPosition, this.TokenString, 0xad);
            this.SyntaxTree.Push(node);
            try
            {
                this.MoveNext();
                if ((this.Token == 0x6b) && !this.ParseParameterListDeclaration())
                {
                    flag = false;
                }
                node.AddAttribute(new SyntaxAttribute(base.prevPosition, SyntaxConsts.DeclarationScope, null));
                if (!this.ParseMethodBody())
                {
                    flag = false;
                }
            }
            finally
            {
                this.SyntaxTree.Pop();
            }
            node.Range.EndPoint = base.prevPosition;
            return flag;
        }

        protected virtual bool ParseArgument(ref ISyntaxNode node)
        {
            bool flag = true;
            node = new SyntaxNode(this.TokenPosition, string.Empty, 0x1c);
            ISyntaxNode node2 = null;
            CsLexerToken token = (CsLexerToken) this.Token;
            if ((token == CsLexerToken.Out) || (token == CsLexerToken.Ref))
            {
                this.MoveNext();
                flag = this.ParseVariableReference(ref node2);
            }
            else if (token == CsLexerToken.ArgList)
            {
                this.MoveNext();
                if (this.Token == 0x6b)
                {
                    flag = this.ParseArgumentList(ref node2);
                }
            }
            else
            {
                bool flag2 = false;
                this.SaveState();
                try
                {
                    flag = this.ParseExpression(ref node2);
                    flag2 = flag || (this.Token == 0x6c);
                    if (!flag2)
                    {
                        node2 = null;
                    }
                    base.savePrevPosition = base.prevPosition;
                }
                finally
                {
                    this.RestoreState(!flag2);
                }
            }
            if (node2 != null)
            {
                node.AddChild(node2);
            }
            node.Range.EndPoint = base.prevPosition;
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
            node.Range.EndPoint = base.prevPosition;
            return flag;
        }

        protected virtual bool ParseArgumentExpressionList(ref ISyntaxNode node)
        {
            bool flag = true;
            node = new SyntaxNode(this.TokenPosition, string.Empty, 0x1d);
            this.MoveNext();
            if (this.Token != 0x6a)
            {
                ISyntaxNode node2 = null;
                flag = (this.Token == 110) || this.ParseArgumentExpression(ref node2);
                if (node2 != null)
                {
                    node.AddChild(node2);
                }
                while (this.Token == 110)
                {
                    this.MoveNext();
                    node2 = null;
                    if ((this.Token != 110) && !this.ParseArgumentExpression(ref node2))
                    {
                        flag = false;
                    }
                    if (node2 != null)
                    {
                        node.AddChild(node2);
                    }
                }
            }
            if (!this.Expected(CsLexerToken.Close_bracket))
            {
                flag = false;
            }
            node.Range.EndPoint = base.prevPosition;
            return flag;
        }

        protected virtual bool ParseArgumentList(ref ISyntaxNode node)
        {
            bool flag = true;
            node = new SyntaxNode(this.TokenPosition, this.TokenString, 0x1d);
            if (this.Token != 0x6b)
            {
                this.SyntaxError(0x6b);
                return false;
            }
            this.MoveNext();
            while (!this.Eof && (this.Token != 0x6c))
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
                if (this.Token != 110)
                {
                    break;
                }
                this.MoveNext();
            }
            if (!this.Expected(CsLexerToken.Close_parens))
            {
                flag = false;
            }
            node.Range.EndPoint = base.prevPosition;
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
                while (!this.Eof && (this.Token != 0x68))
                {
                    ISyntaxNode child = null;
                    flag = this.ParseVariableInitializer(ref child);
                    if (this.Token != 110)
                    {
                        goto Label_006D;
                    }
                    this.MoveNext();
                }
            }
            finally
            {
                this.SyntaxTree.Pop();
            }
        Label_006D:
            if (!this.Expected(CsLexerToken.Close_brace))
            {
                flag = false;
            }
            node.Range.EndPoint = base.prevPosition;
            return flag;
        }

        protected virtual bool ParseAsIsType(ISyntaxNode node)
        {
            string str;
            bool flag = true;
            Point tokenPosition = this.TokenPosition;
            flag = this.ParseType(out str);
            if (str != string.Empty)
            {
                node.AddAttribute(new SyntaxAttribute(tokenPosition, NetNodeType.Type.ToString(), str));
            }
            return flag;
        }

        protected virtual bool ParseAttributeDeclaration()
        {
            string str;
            Point tokenPosition = this.TokenPosition;
            bool flag = true;
            if (!this.ParseQualifiedIdentifier(out str))
            {
                return false;
            }
            ISyntaxNode node = new SyntaxNode(tokenPosition, str, 30);
            this.AddNode(node);
            if (this.Token != 0x6b)
            {
                goto Label_00BE;
            }
            ISyntaxNode node2 = new SyntaxNode(this.TokenPosition, string.Empty, 0x1b);
            node.AddChild(node2);
            this.SyntaxTree.Push(node2);
            try
            {
                this.MoveNext();
                while (!this.Eof && (this.Token != 0x6c))
                {
                    flag = this.ParsePositionalOrNamedParam();
                    node2.Range.EndPoint = base.prevPosition;
                    if (this.Token != 110)
                    {
                        goto Label_00B2;
                    }
                    this.MoveNext();
                }
            }
            finally
            {
                this.SyntaxTree.Pop();
            }
        Label_00B2:
            if (!this.Expected(CsLexerToken.Close_parens))
            {
                flag = false;
            }
        Label_00BE:
            node.Range.EndPoint = base.prevPosition;
            return flag;
        }

        protected virtual bool ParseAttributeListDeclaration()
        {
            Point tokenPosition = this.TokenPosition;
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(tokenPosition, string.Empty, 0x1f, SyntaxNodeOptions.Indentation);
            this.AddNode(node);
            this.SyntaxTree.Push(node);
            try
            {
                this.MoveNext();
                if (!this.TryParseAttributeSpecifier())
                {
                    flag = false;
                }
                while (!this.Eof && (this.Token != 0x6a))
                {
                    if (!this.ParseAttributeDeclaration())
                    {
                        flag = false;
                    }
                    if (this.Token != 110)
                    {
                        goto Label_007A;
                    }
                    this.MoveNext();
                }
            }
            finally
            {
                this.SyntaxTree.Pop();
            }
        Label_007A:
            if (!this.Expected(CsLexerToken.Close_bracket))
            {
                flag = false;
            }
            node.Range.EndPoint = base.prevPosition;
            return flag;
        }

        protected virtual bool ParseBaseAccess(ref ISyntaxNode node)
        {
            bool flag = true;
            node = new SyntaxNode(this.TokenPosition, this.TokenString, 0x9d);
            this.MoveNext();
            switch (((CsLexerToken) this.Token))
            {
                case CsLexerToken.Open_bracket:
                {
                    ISyntaxNode node2 = null;
                    flag = this.ParseArgumentExpressionList(ref node2);
                    if (node2 != null)
                    {
                        node.AddChild(node2);
                    }
                    break;
                }
                case CsLexerToken.Dot:
                    break;

                default:
                    this.SyntaxError();
                    flag = false;
                    break;
            }
            node.Range.EndPoint = base.prevPosition;
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
                while (this.Token == 110)
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

        protected override bool ParseBlock()
        {
            bool flag = true;
            Point tokenPosition = this.TokenPosition;
            if (this.Expected(CsLexerToken.Open_brace))
            {
                this.AddAttribute(new SyntaxAttribute(tokenPosition, SyntaxConsts.BlockScope, null));
                this.AddAttribute(new SyntaxAttribute(tokenPosition, SyntaxConsts.DefinitionScope, null));
                if ((this.Token != 0x68) && !this.ParseStatementList())
                {
                    flag = false;
                }
                if (this.Expected(CsLexerToken.Close_brace))
                {
                    this.AddAttribute(new SyntaxAttribute(base.prevPosition, SyntaxConsts.DefinitionScopeEnd, null));
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
            node.Range.EndPoint = base.prevPosition;
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
            if (!this.Expected(CsLexerToken.Semicolon))
            {
                flag = false;
            }
            node.Range.EndPoint = base.prevPosition;
            return flag;
        }

        protected virtual bool ParseCastTargetExpression(ref ISyntaxNode node)
        {
            bool flag = true;
            node = this.CreateExpressionNode(this.TokenPosition, string.Empty, 0x98, node, false);
            ISyntaxNode node2 = null;
            if (!this.ParseUnaryExpression(ref node2))
            {
                flag = false;
            }
            if (node2 != null)
            {
                node.AddChild(node2);
            }
            node.Range.EndPoint = base.prevPosition;
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
                if (this.Token == 0x6b)
                {
                    string str;
                    this.MoveNext();
                    if (!this.ParseType(out str))
                    {
                        flag = false;
                    }
                    if (this.IsIdentifierToken(this.Token))
                    {
                        this.MoveNext();
                    }
                    if (!this.Expected(CsLexerToken.Close_parens))
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
            node.Range.EndPoint = base.prevPosition;
            return flag;
        }

        protected virtual bool ParseCatchStatements()
        {
            bool flag = true;
            while (this.Token == 10)
            {
                flag = this.ParseCatchStatement();
            }
            if ((this.Token == 0x1b) && !this.ParseFinallyStatement())
            {
                flag = false;
            }
            return flag;
        }

        protected virtual bool ParseCheckedExpression(ref ISyntaxNode node)
        {
            return this.ParseCheckedExpression(NetNodeType.CheckedExpression, ref node);
        }

        protected virtual bool ParseCheckedExpression(NetNodeType nodeType, ref ISyntaxNode node)
        {
            bool flag = true;
            node = new SyntaxNode(this.TokenPosition, this.TokenString, (int) nodeType);
            this.MoveNext();
            if (this.Token == 0x6b)
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
                if (!this.Expected(CsLexerToken.Close_parens))
                {
                    flag = false;
                }
            }
            else
            {
                this.SyntaxError(0x6b);
                flag = false;
            }
            node.Range.EndPoint = base.prevPosition;
            return flag;
        }

        protected virtual bool ParseCheckedStatement()
        {
            return this.ParseCheckedStatement(NetNodeType.CheckedStatement);
        }

        protected virtual bool ParseCheckedStatement(NetNodeType nodeType)
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, this.TokenString, (int) nodeType, SyntaxNodeOptions.Indentation);
            this.AddNode(node);
            this.SyntaxTree.Push(node);
            try
            {
                this.MoveNext();
                if (!this.ParseBlock())
                {
                    flag = false;
                }
            }
            finally
            {
                this.SyntaxTree.Pop();
            }
            node.Range.EndPoint = base.prevPosition;
            return flag;
        }

        protected virtual bool ParseClassBody()
        {
            bool flag = true;
            bool flag2 = this.SyntaxTree.Current != this.SyntaxTree.Root;
            ISyntaxAttributes attrs = null;
            while (!this.Eof)
            {
                if (flag2 && (this.Token == 0x68))
                {
                    return flag;
                }
                this.ParseModifiers(ref attrs);
                CsLexerToken token = (CsLexerToken) this.Token;
                if (token <= CsLexerToken.Namespace)
                {
                    switch (token)
                    {
                        case CsLexerToken.Delegate:
                        case CsLexerToken.Enum:
                        case CsLexerToken.Event:
                        case CsLexerToken.Explicit:
                        case CsLexerToken.Class:
                        case CsLexerToken.Implicit:
                        case CsLexerToken.Interface:
                        case CsLexerToken.Namespace:
                            goto Label_00CC;
                    }
                    goto Label_00DB;
                }
                if (token <= CsLexerToken.Alias)
                {
                    switch (token)
                    {
                        case CsLexerToken.Struct:
                        case CsLexerToken.Using:
                        case CsLexerToken.Alias:
                            goto Label_00CC;
                    }
                    goto Label_00DB;
                }
                if (token != CsLexerToken.Open_bracket)
                {
                    if (token == CsLexerToken.Tilde)
                    {
                        goto Label_00CC;
                    }
                    if (token != CsLexerToken.Directive_Literal)
                    {
                        goto Label_00DB;
                    }
                    if (!this.ParseDirective())
                    {
                        flag = false;
                    }
                }
                else if (!this.ParseAttributeListDeclaration())
                {
                    flag = false;
                }
                continue;
            Label_00CC:
                if (!this.ParseKnownMemberDeclaration(attrs))
                {
                    flag = false;
                }
                attrs = null;
                continue;
            Label_00DB:
                if (!this.ParseUnknownMemberDeclaration(attrs))
                {
                    flag = false;
                }
                attrs = null;
            }
            return flag;
        }

        protected override bool ParseComment()
        {
            bool flag = this.Token == 160;
            ISyntaxNode item = new SyntaxNode(this.TokenPosition, SyntaxParserConsts.OutlineCommentText, flag ? 0x39 : 0x33, ((this.SyntaxTree.Current == this.SyntaxTree.Root) || (this.State == 1)) ? SyntaxNodeOptions.KeepIndentation : SyntaxNodeOptions.None);
            item.AddAttribute(new SyntaxAttribute(item.Position, SyntaxConsts.OutlineText, item.Name));
            base.comments.Add(item);
            Point currentPosition = this.CurrentPosition;
            while (((flag && this.IsXmlComment(this.Token)) || (!flag && (this.Token == 0x9f))) || (this.Token == 0xa3))
            {
                if (this.Token != 0xa3)
                {
                    currentPosition = this.CurrentPosition;
                }
                this.NextToken();
            }
            item.Range.EndPoint = currentPosition;
            if (flag)
            {
                this.ParseXmlComment(item);
            }
            return true;
        }

        protected virtual bool ParseConditionalAndExpression(ref ISyntaxNode node)
        {
            bool flag = this.ParseInclusiveOrExpression(ref node);
            if (this.Token == 0x89)
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
                node.Range.EndPoint = base.prevPosition;
            }
            return flag;
        }

        protected virtual bool ParseConditionalExpression(ref ISyntaxNode node)
        {
            bool flag = this.ParseNullCoalescingExpression(ref node);
            if (this.Token == 0x7f)
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
                ISyntaxAttribute attr = new SyntaxAttribute(this.TokenPosition, NetNodeType.Expression.ToString(), this.TokenString);
                if (this.Expected(CsLexerToken.Colon))
                {
                    node2 = null;
                    if (!this.ParseExpression(ref node2))
                    {
                        flag = false;
                    }
                    if (node2 != null)
                    {
                        node.AddChild(node2);
                        node.AddAttribute(attr);
                    }
                }
                else
                {
                    flag = false;
                }
                node.Range.EndPoint = base.prevPosition;
            }
            return flag;
        }

        protected virtual bool ParseConditionalOrExpression(ref ISyntaxNode node)
        {
            bool flag = this.ParseConditionalAndExpression(ref node);
            if (this.Token == 0x8a)
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
                node.Range.EndPoint = base.prevPosition;
            }
            return flag;
        }

        protected virtual bool ParseConstantExpression(ref ISyntaxNode node)
        {
            return this.ParseExpression(ref node);
        }

        protected virtual bool ParseConstructorDeclaration(ISyntaxAttributes attrs, string type, Point typePos)
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(((attrs != null) && (attrs.Count > 0)) ? attrs[0].Position : typePos, type, 0x12);
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
                if (!this.ParseParameterListDeclaration())
                {
                    flag = false;
                }
                if (this.Token == 0x6f)
                {
                    this.MoveNext();
                    if (!this.ParseConstructorInitializer())
                    {
                        flag = false;
                    }
                }
                node.AddAttribute(new SyntaxAttribute(base.prevPosition, SyntaxConsts.DeclarationScope, null));
                if (!this.ParseMethodBody())
                {
                    flag = false;
                }
            }
            finally
            {
                this.SyntaxTree.Pop();
            }
            if (!this.AfterDeclaration(node))
            {
                flag = false;
            }
            node.Range.EndPoint = base.prevPosition;
            return flag;
        }

        protected virtual bool ParseConstructorInitializer()
        {
            bool flag = true;
            switch (((CsLexerToken) this.Token))
            {
                case CsLexerToken.Base:
                case CsLexerToken.This:
                {
                    this.MoveNext();
                    ISyntaxNode node = null;
                    if (!this.ParseArgumentList(ref node))
                    {
                        flag = false;
                    }
                    if (node != null)
                    {
                        this.AddNode(node);
                    }
                    return flag;
                }
            }
            this.SyntaxError();
            return false;
        }

        protected virtual bool ParseContinueStatement()
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, this.TokenString, 0x5c);
            this.AddNode(node);
            this.MoveNext();
            if (!this.Expected(CsLexerToken.Semicolon))
            {
                flag = false;
            }
            node.Range.EndPoint = base.prevPosition;
            return flag;
        }

        protected override bool ParseDeclaration(ISyntaxNode node)
        {
            ISyntaxAttributes attrs = null;
            this.ParseModifiers(ref attrs);
            CsLexerToken token = (CsLexerToken) this.Token;
            if (token <= CsLexerToken.Enum)
            {
                switch (token)
                {
                    case CsLexerToken.Class:
                    case CsLexerToken.Enum:
                        goto Label_0032;
                }
                goto Label_0041;
            }
            if (((token != CsLexerToken.Interface) && (token != CsLexerToken.Namespace)) && (token != CsLexerToken.Struct))
            {
                goto Label_0041;
            }
        Label_0032:
            return this.ParseDeclaration(attrs, node, node.NodeType);
        Label_0041:
            return false;
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
            if (flag2)
            {
                node = new SyntaxNode(((attrs != null) && (attrs.Count > 0)) ? attrs[0].Position : this.TokenPosition, this.TokenString, nodeType);
            }
            if (attrs != null)
            {
                node.AddAttributes(attrs);
            }
            this.MoveNext();
            Point tokenPosition = this.TokenPosition;
            if ((nodeType == 7) ? this.ParseQualifiedIdentifier(out str) : this.ParseIdentifier(out str))
            {
                node.Name = str;
                node.AddAttribute(new SyntaxAttribute(tokenPosition, NetNodeType.Name.ToString(), str));
                node.Options |= SyntaxNodeOptions.CodeCompletion;
                if (flag2)
                {
                    this.AddNode(node);
                }
                if (!this.BeforeDeclaration(node))
                {
                    flag = false;
                }
                if (nodeType != 7)
                {
                    this.SyntaxTree.Push(node);
                    try
                    {
                        if (((nodeType != 11) && this.IsTypeParameterList(this.Token)) && !this.ParseTypeParameterList())
                        {
                            flag = false;
                        }
                        if (this.IsBaseList(this.Token) && !this.ParseBaseList())
                        {
                            flag = false;
                        }
                        if (this.IsTypeParameterConstraintsClause(this.Token) && !this.ParseTypeParameterConstraintsClauses())
                        {
                            flag = false;
                        }
                    }
                    finally
                    {
                        this.SyntaxTree.Pop();
                    }
                }
                node.AddAttribute(new SyntaxAttribute(base.prevPosition, SyntaxConsts.DeclarationScope, null));
                if (!this.ParseDeclarationBody(node, nodeType))
                {
                    flag = false;
                }
            }
            if (!this.AfterDeclaration(node))
            {
                flag = false;
            }
            node.Range.EndPoint = base.prevPosition;
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
                        case 7:
                        case 8:
                        case 9:
                        case 10:
                            if (!this.ParseClassBody())
                            {
                                flag = false;
                            }
                            goto Label_0099;

                        case 11:
                            if (!this.ParseEnumBody())
                            {
                                flag = false;
                            }
                            goto Label_0099;
                    }
                    flag = false;
                }
            }
            finally
            {
                this.SyntaxTree.Pop();
            }
        Label_0099:
            if (flag2 && this.ParseDeclarationBodyEnd(node))
            {
                node.AddAttribute(new SyntaxAttribute(base.prevPosition, SyntaxConsts.DefinitionScopeEnd, null));
                if (this.Token == 0x71)
                {
                    this.MoveNext();
                }
                return flag;
            }
            return false;
        }

        protected virtual bool ParseDeclarationBodyEnd(ISyntaxNode node)
        {
            return this.Expected(CsLexerToken.Close_brace);
        }

        protected virtual bool ParseDefaultExpression(ref ISyntaxNode node)
        {
            return this.ParseTypeofExpression(NetNodeType.DefaultExpression, ref node);
        }

        protected virtual bool ParseDelegateDeclaration(ISyntaxAttributes attrs)
        {
            string str;
            string str2;
            bool flag = true;
            this.MoveNext();
            Point tokenPosition = this.TokenPosition;
            if (this.ParseType(out str) && this.ParseQualifiedIdentifier(out str2))
            {
                ISyntaxNode node = new SyntaxNode(((attrs != null) && (attrs.Count > 0)) ? attrs[0].Position : tokenPosition, str2, 20, SyntaxNodeOptions.CodeCompletion);
                node.AddAttribute(new SyntaxAttribute(tokenPosition, NetNodeType.Type.ToString(), str));
                if (attrs != null)
                {
                    node.AddAttributes(attrs);
                }
                if (!this.BeforeDeclaration(node))
                {
                    flag = false;
                }
                if (this.Token == 0x6b)
                {
                    this.SyntaxTree.Push(node);
                    try
                    {
                        if (!this.ParseParameterListDeclaration())
                        {
                            flag = false;
                        }
                    }
                    finally
                    {
                        this.SyntaxTree.Pop();
                    }
                    if (!this.Expected(CsLexerToken.Semicolon))
                    {
                        flag = false;
                    }
                }
                if (!this.AfterDeclaration(node))
                {
                    flag = false;
                }
                node.Range.EndPoint = base.prevPosition;
                return flag;
            }
            return false;
        }

        protected virtual bool ParseDestructorDeclaration(ISyntaxAttributes attrs)
        {
            string str;
            bool flag = true;
            Point tokenPosition = this.TokenPosition;
            this.MoveNext();
            if (!this.ParseQualifiedIdentifier(out str))
            {
                return flag;
            }
            ISyntaxNode node = new SyntaxNode(((attrs != null) && (attrs.Count > 0)) ? attrs[0].Position : tokenPosition, "~" + str, 0x13);
            this.AddNode(node);
            if (attrs != null)
            {
                node.AddAttributes(attrs);
            }
            return this.ParseMethodDeclaration(node);
        }

        protected virtual bool ParseDirective()
        {
            bool flag = true;
            string tokenString = this.TokenString;
            if (tokenString.StartsWith("#"))
            {
                tokenString = tokenString.Remove(0, 1).Trim();
            }
            switch (tokenString)
            {
                case "region":
                {
                    ISyntaxNode node = new SyntaxNode(this.TokenPosition, this.TokenString, 40);
                    if (this.State == 3)
                    {
                        this.MoveNext();
                    }
                    node.Name = this.TokenString.Trim();
                    this.AddNode(node);
                    this.SyntaxTree.Push(node);
                    this.MoveNext();
                    return flag;
                }
                case "endregion":
                {
                    Point point2 = new Point(base.source.Length, base.lineIndex);
                    if (this.State == 3)
                    {
                        this.MoveNext();
                    }
                    this.MoveNext();
                    ISyntaxNode current = this.SyntaxTree.Current;
                    if (current.NodeType == 40)
                    {
                        this.SyntaxTree.Pop();
                        current.Range.EndPoint = point2;
                        current.AddAttribute(new SyntaxAttribute(current.Position, SyntaxConsts.OutlineText, current.Name));
                        current.Options = SyntaxNodeOptions.Outlining;
                    }
                    return flag;
                }
            }
            if (this.State == 3)
            {
                this.MoveNext();
            }
            this.MoveNext();
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
            node.Range.EndPoint = base.prevPosition;
            return flag;
        }

        protected virtual bool ParseDoWhileStatement()
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, this.TokenString, 0x49, SyntaxNodeOptions.BackIndentation);
            this.AddNode(node);
            this.MoveNext();
            if (this.Token == 0x6b)
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
                if (!this.Expected(CsLexerToken.Close_parens) || !this.Expected(CsLexerToken.Semicolon))
                {
                    flag = false;
                }
            }
            else
            {
                this.SyntaxError(0x6b);
                flag = false;
            }
            node.Range.EndPoint = base.prevPosition;
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
            node.Range.EndPoint = base.prevPosition;
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
            node.Range.EndPoint = base.prevPosition;
            return flag;
        }

        protected virtual bool ParseEmbeddedStatement()
        {
            switch (((CsLexerToken) this.Token))
            {
                case CsLexerToken.Fixed:
                    return this.ParseFixedStatement();

                case CsLexerToken.For:
                    return this.ParseForStatement();

                case CsLexerToken.Foreach:
                    return this.ParseForeachStatement();

                case CsLexerToken.Goto:
                    return this.ParseGotoStatement();

                case CsLexerToken.If:
                    return this.ParseIfStatement();

                case CsLexerToken.Lock:
                    return this.ParseLockStatement();

                case CsLexerToken.Do:
                    return this.ParseDoStatement();

                case CsLexerToken.Break:
                    return this.ParseBreakStatement();

                case CsLexerToken.Checked:
                    return this.ParseCheckedStatement();

                case CsLexerToken.Continue:
                    return this.ParseContinueStatement();

                case CsLexerToken.Switch:
                    return this.ParseSwitchStatement();

                case CsLexerToken.Throw:
                    return this.ParseThrowStatement();

                case CsLexerToken.Try:
                    return this.ParseTryStatement();

                case CsLexerToken.Return:
                    return this.ParseReturnStatement();

                case CsLexerToken.Unchecked:
                    return this.ParseUncheckedStatement();

                case CsLexerToken.Unsafe:
                    return this.ParseUnsafeStatement();

                case CsLexerToken.Using:
                    return this.ParseUsingStatement();

                case CsLexerToken.While:
                    return this.ParseWhileStatement();

                case CsLexerToken.Yield:
                    return this.ParseYieldStatement();

                case CsLexerToken.Open_brace:
                    return this.ParseBlockStatement();
            }
            return this.ParseExpressionStatement();
        }

        protected virtual bool ParseEnumBody()
        {
            bool flag = true;
            while (!this.Eof && (this.Token != 0x68))
            {
                CsLexerToken token = (CsLexerToken) this.Token;
                if (token != CsLexerToken.Open_bracket)
                {
                    if (token != CsLexerToken.Directive_Literal)
                    {
                        goto Label_0030;
                    }
                    if (!this.ParseDirective())
                    {
                        flag = false;
                    }
                }
                else if (!this.ParseAttributeListDeclaration())
                {
                    flag = false;
                }
                continue;
            Label_0030:
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
                if (this.Token == 0x76)
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
                if (this.Token == 110)
                {
                    this.MoveNext();
                }
                node.Range.EndPoint = base.prevPosition;
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
                case 0x87:
                case 0x88:
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
                    node.Range.EndPoint = base.prevPosition;
                    return flag;
                }
            }
            return flag;
        }

        protected virtual bool ParseEventDeclaration(ISyntaxAttributes attrs)
        {
            string str;
            string str2;
            bool flag = true;
            this.MoveNext();
            Point tokenPosition = this.TokenPosition;
            if (!this.ParseType(out str))
            {
                return false;
            }
            Point point1 = this.TokenPosition;
            if (!this.ParseQualifiedIdentifier(out str2))
            {
                return false;
            }
            ISyntaxNode node = new SyntaxNode(((attrs != null) && (attrs.Count > 0)) ? attrs[0].Position : tokenPosition, str2, 0x16, SyntaxNodeOptions.CodeCompletion);
            if (attrs != null)
            {
                node.AddAttributes(attrs);
            }
            this.AddNode(node);
            node.AddAttribute(new SyntaxAttribute(tokenPosition, NetNodeType.Type.ToString(), str));
            node.AddAttribute(new SyntaxAttribute(base.prevPosition, SyntaxConsts.DeclarationScope, null));
            if (!this.BeforeDeclaration(node))
            {
                flag = false;
            }
            CsLexerToken token = (CsLexerToken) this.Token;
            if (token != CsLexerToken.Open_brace)
            {
                if (token == CsLexerToken.Semicolon)
                {
                    this.MoveNext();
                }
                else
                {
                    this.SyntaxError();
                    flag = false;
                }
                goto Label_0181;
            }
            node.AddAttribute(new SyntaxAttribute(this.TokenPosition, SyntaxConsts.DefinitionScope, null));
            node.Options |= SyntaxNodeOptions.Indentation;
            this.SyntaxTree.Push(node);
            try
            {
                this.MoveNext();
                while (!this.Eof)
                {
                    if ((this.Token == 0x69) && !this.ParseAttributeListDeclaration())
                    {
                        flag = false;
                    }
                    if (!this.IsAccessor(NetNodeType.EventAccessor))
                    {
                        goto Label_0147;
                    }
                    if (!this.ParseAccessors(null, NetNodeType.EventAccessor))
                    {
                        flag = false;
                    }
                }
            }
            finally
            {
                this.SyntaxTree.Pop();
            }
        Label_0147:
            if (this.Expected(CsLexerToken.Close_brace))
            {
                node.AddAttribute(new SyntaxAttribute(base.prevPosition, SyntaxConsts.DefinitionScopeEnd, null));
            }
            else
            {
                flag = false;
            }
        Label_0181:
            node.Range.EndPoint = base.prevPosition;
            if (!this.AfterDeclaration(node))
            {
                flag = false;
            }
            return flag;
        }

        protected virtual bool ParseExclusiveOrExpression(ref ISyntaxNode node)
        {
            bool flag = this.ParseAndExpression(ref node);
            if (this.Token == 0x7e)
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
                node.Range.EndPoint = base.prevPosition;
            }
            return flag;
        }

        protected virtual bool ParseExpression(ref ISyntaxNode node)
        {
            bool flag = this.ParseConditionalExpression(ref node);
            if (this.Token == 150)
            {
                if (!this.ParseLabmdaExpression(ref node))
                {
                    flag = false;
                }
                return flag;
            }
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
            if (this.Token == 0x71)
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
                    if (!this.Expected(CsLexerToken.Semicolon))
                    {
                        flag = false;
                    }
                }
                finally
                {
                    this.SyntaxTree.Pop();
                }
            }
            node.Range.EndPoint = base.prevPosition;
            return flag;
        }

        protected virtual bool ParseFieldDeclaration(ISyntaxAttributes attrs, string type, Point typePos, string name)
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(((attrs != null) && (attrs.Count > 0)) ? attrs[0].Position : typePos, name, 13, SyntaxNodeOptions.CodeCompletion | SyntaxNodeOptions.Indentation);
            node.AddAttribute(new SyntaxAttribute(typePos, NetNodeType.Type.ToString(), type));
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
                if (!this.ParseOptionalExressionInBracket())
                {
                    flag = false;
                }
                if (!this.ParseVariableDeclarators(NetNodeType.Field, false))
                {
                    flag = false;
                }
            }
            finally
            {
                this.SyntaxTree.Pop();
            }
            if (!this.Expected(CsLexerToken.Semicolon))
            {
                flag = false;
            }
            if (!this.AfterDeclaration(node))
            {
                flag = false;
            }
            node.Range.EndPoint = base.prevPosition;
            return flag;
        }

        protected virtual bool ParseFinallyStatement()
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, this.TokenString, 0x62, (this.SyntaxTree.Current.NodeType != 0x60) ? SyntaxNodeOptions.Indentation : SyntaxNodeOptions.BackIndentation);
            this.AddNode(node);
            this.SyntaxTree.Push(node);
            try
            {
                this.MoveNext();
                flag = this.ParseBlock();
            }
            finally
            {
                this.SyntaxTree.Pop();
            }
            node.Range.EndPoint = base.prevPosition;
            return flag;
        }

        protected virtual bool ParseFixedStatement()
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, this.TokenString, 0x68, SyntaxNodeOptions.Indentation);
            this.AddNode(node);
            this.SyntaxTree.Push(node);
            try
            {
                this.MoveNext();
                if (this.Expected(CsLexerToken.Open_parens))
                {
                    if (!this.ParseVariableDeclaration(this.TokenPosition, NetNodeType.FixedVariable))
                    {
                        flag = false;
                    }
                    if (!this.Expected(CsLexerToken.Close_parens))
                    {
                        flag = false;
                    }
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
            node.Range.EndPoint = base.prevPosition;
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
            node.Range.EndPoint = base.prevPosition;
            return flag;
        }

        protected virtual bool ParseForeachInitializer()
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, string.Empty, 0x4f);
            this.AddNode(node);
            this.SyntaxTree.Push(node);
            try
            {
                bool isImplicit = this.Token == 0x58;
                ISyntaxNode node2 = null;
                if (!this.ParseVariableDeclaration(this.TokenPosition, isImplicit ? NetNodeType.ImplicitVariable : NetNodeType.LocalVariable, isImplicit, false, ref node2))
                {
                    flag = false;
                }
                if (this.Expected(CsLexerToken.In))
                {
                    ISyntaxNode node3 = null;
                    Point tokenPosition = this.TokenPosition;
                    string str = isImplicit ? this.TokenString : string.Empty;
                    flag = this.ParseExpression(ref node3);
                    if (node3 != null)
                    {
                        node.AddChild(node3);
                        if (((node2 != null) && isImplicit) && ((str != string.Empty) && (node3.NodeType == 0x99)))
                        {
                            node2.AddAttribute(new SyntaxAttribute(tokenPosition, NetNodeType.VariableReference.ToString(), str));
                        }
                    }
                }
                else
                {
                    flag = false;
                }
            }
            finally
            {
                this.SyntaxTree.Pop();
            }
            node.Range.EndPoint = base.prevPosition;
            return flag;
        }

        protected virtual bool ParseForeachStatement()
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, this.TokenString, 0x4e, SyntaxNodeOptions.Indentation);
            this.AddNode(node);
            this.SyntaxTree.Push(node);
            try
            {
                this.MoveNext();
                if (this.Expected(CsLexerToken.Open_parens))
                {
                    if (!this.ParseForeachInitializer())
                    {
                        flag = false;
                    }
                    if (!this.Expected(CsLexerToken.Close_parens))
                    {
                        flag = false;
                    }
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
            node.Range.EndPoint = base.prevPosition;
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
                    flag = this.ParseVariableDeclaration(this.TokenPosition, (this.Token == 0x58) ? NetNodeType.ImplicitVariable : NetNodeType.LocalVariable);
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
            node.Range.EndPoint = base.prevPosition;
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
            node.Range.EndPoint = base.prevPosition;
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
                if (this.Expected(CsLexerToken.Open_parens))
                {
                    if ((this.Token != 0x71) && !this.ParseForInitializer())
                    {
                        flag = false;
                    }
                    if (this.Expected(CsLexerToken.Semicolon))
                    {
                        if ((this.Token != 0x71) && !this.ParseForCondition())
                        {
                            flag = false;
                        }
                    }
                    else
                    {
                        flag = false;
                    }
                    if (this.Expected(CsLexerToken.Semicolon))
                    {
                        if ((this.Token != 0x6c) && !this.ParseForIterator())
                        {
                            flag = false;
                        }
                    }
                    else
                    {
                        flag = false;
                    }
                    if (!this.Expected(CsLexerToken.Close_parens))
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
            node.Range.EndPoint = base.prevPosition;
            return flag;
        }

        protected virtual bool ParseFromClause()
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, this.TokenString, 0xb7, SyntaxNodeOptions.Indentation);
            this.AddNode(node);
            this.SyntaxTree.Push(node);
            try
            {
                this.MoveNext();
                if (this.ParseOptionalTypeAndIdentifier(node))
                {
                    if (this.Expected(CsLexerToken.In))
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
                    else
                    {
                        flag = false;
                    }
                    if ((this.Token == 0x5b) && !this.ParseJoinClauses())
                    {
                        flag = false;
                    }
                }
                else
                {
                    flag = false;
                }
            }
            finally
            {
                this.SyntaxTree.Pop();
            }
            node.Range.EndPoint = base.prevPosition;
            return flag;
        }

        protected virtual bool ParseFromLetWhereClauses()
        {
            bool flag = true;
            while (this.IsFromLetWhereClause())
            {
                CsLexerToken token = (CsLexerToken) this.Token;
                if (token == CsLexerToken.Where)
                {
                    goto Label_0034;
                }
                if (token != CsLexerToken.From)
                {
                    if (token == CsLexerToken.Let)
                    {
                        goto Label_0028;
                    }
                    goto Label_0040;
                }
                if (!this.ParseFromClause())
                {
                    flag = false;
                }
                continue;
            Label_0028:
                if (!this.ParseLetClause())
                {
                    flag = false;
                }
                continue;
            Label_0034:
                if (!this.ParseWhereClause())
                {
                    flag = false;
                }
                continue;
            Label_0040:
                flag = false;
            }
            return flag;
        }

        protected virtual bool ParseGotoStatement()
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, this.TokenString, 0x5d);
            this.AddNode(node);
            this.MoveNext();
            CsLexerToken token = (CsLexerToken) this.Token;
            if (token <= CsLexerToken.Default)
            {
                switch (token)
                {
                    case CsLexerToken.Case:
                    {
                        this.SyntaxTree.Push(node);
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
                        goto Label_0082;
                    }
                    case CsLexerToken.Default:
                        goto Label_0049;
                }
                goto Label_0080;
            }
            if ((token != CsLexerToken.Value) && (token != CsLexerToken.Identifier_Literal))
            {
                goto Label_0080;
            }
        Label_0049:
            this.MoveNext();
            goto Label_0082;
        Label_0080:
            flag = false;
        Label_0082:
            if (!this.Expected(CsLexerToken.Semicolon))
            {
                flag = false;
            }
            node.Range.EndPoint = base.prevPosition;
            return flag;
        }

        protected virtual bool ParseGroupClause()
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, this.TokenString, 0xbc, SyntaxNodeOptions.Indentation);
            this.AddNode(node);
            this.SyntaxTree.Push(node);
            try
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
                if (this.Token == 0x5d)
                {
                    this.MoveNext();
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
            }
            finally
            {
                this.SyntaxTree.Pop();
            }
            node.Range.EndPoint = base.prevPosition;
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
                if ((this.Token == 0x15) && !this.ParseElseStatement())
                {
                    flag = false;
                }
            }
            finally
            {
                this.SyntaxTree.Pop();
            }
            node.Range.EndPoint = base.prevPosition;
            return flag;
        }

        protected virtual bool ParseImplicitVariableDeclaration()
        {
            ISyntaxNode node = null;
            bool flag = this.ParseVariableDeclaration(this.TokenPosition, NetNodeType.ImplicitVariable, ref node);
            if (!this.Expected(CsLexerToken.Semicolon))
            {
                flag = false;
            }
            if (node != null)
            {
                node.Range.EndPoint = base.prevPosition;
            }
            return flag;
        }

        protected virtual bool ParseInclusiveOrExpression(ref ISyntaxNode node)
        {
            bool flag = this.ParseExclusiveOrExpression(ref node);
            if (this.Token == 0x7a)
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
                node.Range.EndPoint = base.prevPosition;
            }
            return flag;
        }

        protected virtual bool ParseIndexerDeclaration(ISyntaxAttributes attrs, string type, Point typePos)
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(((attrs != null) && (attrs.Count > 0)) ? attrs[0].Position : typePos, this.TokenString, 0x19, SyntaxNodeOptions.CodeCompletion);
            if (attrs != null)
            {
                node.AddAttributes(attrs);
            }
            node.AddAttribute(new SyntaxAttribute(typePos, NetNodeType.Type.ToString(), type));
            this.AddNode(node);
            this.MoveNext();
            if (this.Token == 0x69)
            {
                this.SyntaxTree.Push(node);
                try
                {
                    if (!this.ParseParameterListDeclaration(0x69, 0x6a))
                    {
                        flag = false;
                    }
                }
                finally
                {
                    this.SyntaxTree.Pop();
                }
            }
            node.AddAttribute(new SyntaxAttribute(base.prevPosition, SyntaxConsts.DeclarationScope, null));
            if ((this.Token == 0x67) && !this.ParsePropertyDeclaration(node))
            {
                flag = false;
            }
            node.Range.EndPoint = base.prevPosition;
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
            node.Range.EndPoint = base.prevPosition;
            return flag;
        }

        protected virtual bool ParseJoinClause()
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, string.Empty, 0xb9);
            this.AddNode(node);
            this.SyntaxTree.Push(node);
            try
            {
                this.MoveNext();
                if (this.ParseOptionalTypeAndIdentifier(node))
                {
                    if (this.Expected(CsLexerToken.In))
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
                    else
                    {
                        flag = false;
                    }
                    if (this.Expected(CsLexerToken.On))
                    {
                        ISyntaxNode node3 = null;
                        if (!this.ParseExpression(ref node3))
                        {
                            flag = false;
                        }
                        if (node3 != null)
                        {
                            node.AddChild(node3);
                        }
                    }
                    else
                    {
                        flag = false;
                    }
                    if (this.Expected(CsLexerToken.Equals))
                    {
                        ISyntaxNode node4 = null;
                        if (!this.ParseExpression(ref node4))
                        {
                            flag = false;
                        }
                        if (node4 != null)
                        {
                            node.AddChild(node4);
                        }
                    }
                    else
                    {
                        flag = false;
                    }
                    if (this.Token == 0x61)
                    {
                        string str;
                        this.MoveNext();
                        Point tokenPosition = this.TokenPosition;
                        if (this.ParseIdentifier(out str))
                        {
                            node.AddChild(new SyntaxNode(tokenPosition, str, 0x99));
                        }
                        else
                        {
                            flag = false;
                        }
                    }
                }
                else
                {
                    flag = false;
                }
            }
            finally
            {
                this.SyntaxTree.Pop();
            }
            node.Range.EndPoint = base.prevPosition;
            return flag;
        }

        protected virtual bool ParseJoinClauses()
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, string.Empty, 0xb8);
            this.AddNode(node);
            this.SyntaxTree.Push(node);
            try
            {
                while (this.Token == 0x5b)
                {
                    if (!this.ParseJoinClause())
                    {
                        flag = false;
                    }
                }
            }
            finally
            {
                this.SyntaxTree.Pop();
            }
            node.Range.EndPoint = base.prevPosition;
            return flag;
        }

        protected virtual bool ParseKnownMemberDeclaration(ISyntaxAttributes attrs)
        {
            bool flag = true;
            CsLexerToken token = (CsLexerToken) this.Token;
            if (token <= CsLexerToken.Interface)
            {
                if (token <= CsLexerToken.Explicit)
                {
                    switch (token)
                    {
                        case CsLexerToken.Delegate:
                            if (!this.ParseDelegateDeclaration(attrs))
                            {
                                flag = false;
                            }
                            return flag;

                        case CsLexerToken.Do:
                        case CsLexerToken.Double:
                        case CsLexerToken.Else:
                            return flag;

                        case CsLexerToken.Enum:
                            if (!this.ParseDeclaration(attrs, 11))
                            {
                                flag = false;
                            }
                            return flag;

                        case CsLexerToken.Event:
                            if (!this.ParseEventDeclaration(attrs))
                            {
                                flag = false;
                            }
                            return flag;

                        case CsLexerToken.Explicit:
                            goto Label_0104;

                        case CsLexerToken.Class:
                            if (!this.ParseDeclaration(attrs, 8))
                            {
                                flag = false;
                            }
                            return flag;
                    }
                    return flag;
                }
                switch (token)
                {
                    case CsLexerToken.Implicit:
                        goto Label_0104;

                    case CsLexerToken.Interface:
                        if (!this.ParseDeclaration(attrs, 10))
                        {
                            flag = false;
                        }
                        return flag;
                }
                return flag;
            }
            if (token <= CsLexerToken.Struct)
            {
                switch (token)
                {
                    case CsLexerToken.Namespace:
                        if (!this.ParseDeclaration(attrs, 7))
                        {
                            flag = false;
                        }
                        return flag;

                    case CsLexerToken.Struct:
                        if (!this.ParseDeclaration(attrs, 9))
                        {
                            flag = false;
                        }
                        return flag;
                }
                return flag;
            }
            if (token != CsLexerToken.Using)
            {
                if (token != CsLexerToken.Alias)
                {
                    if ((token == CsLexerToken.Tilde) && !this.ParseDestructorDeclaration(attrs))
                    {
                        flag = false;
                    }
                    return flag;
                }
                if (!this.ParseAliasList(attrs))
                {
                    flag = false;
                }
                return flag;
            }
            if (!this.ParseUsingList())
            {
                flag = false;
            }
            return flag;
        Label_0104:
            if (!this.ParseOperatorDeclaration(attrs))
            {
                flag = false;
            }
            return flag;
        }

        protected virtual bool ParseLabmdaExpression(ref ISyntaxNode node)
        {
            bool flag = true;
            node = this.CreateExpressionNode(this.TokenPosition, this.TokenString, 0xb1, node, true);
            this.SyntaxTree.Push(node);
            try
            {
                this.MoveNext();
                if (this.Token == 0x67)
                {
                    flag = this.ParseBlockStatement();
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
            }
            finally
            {
                this.SyntaxTree.Pop();
            }
            node.Range.EndPoint = base.prevPosition;
            return flag;
        }

        protected virtual bool ParseLambdaParameterDeclaration()
        {
            string str2;
            bool flag = false;
            string identifier = string.Empty;
            ISyntaxAttribute attr = null;
            switch (((CsLexerToken) this.Token))
            {
                case CsLexerToken.Out:
                case CsLexerToken.Ref:
                case CsLexerToken.ArgList:
                    attr = new SyntaxAttribute(this.TokenPosition, NetNodeType.ParameterModifier.ToString(), this.TokenString);
                    this.MoveNext();
                    break;

                default:
                    if (this.IsIdentifierToken(this.Token))
                    {
                        identifier = this.TokenString;
                    }
                    break;
            }
            Point tokenPosition = this.TokenPosition;
            if (this.ParseType(out str2))
            {
                bool flag2 = (this.Token == 0x9e) && this.ParseIdentifier(out identifier);
                flag = flag2 || (identifier != string.Empty);
                if (!flag)
                {
                    return flag;
                }
                ISyntaxNode node = new SyntaxNode(tokenPosition, identifier, 0xb2);
                this.AddNode(node);
                if (attr != null)
                {
                    node.AddAttribute(attr);
                }
                if (flag2)
                {
                    node.AddAttribute(new SyntaxAttribute(tokenPosition, NetNodeType.Type.ToString(), str2));
                }
                node.Range.EndPoint = base.prevPosition;
            }
            return flag;
        }

        protected virtual bool ParseLetClause()
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, this.TokenString, 0xba, SyntaxNodeOptions.Indentation);
            this.AddNode(node);
            this.SyntaxTree.Push(node);
            try
            {
                this.MoveNext();
                string identifier = string.Empty;
                if (this.ParseIdentifier(out identifier))
                {
                    if (this.Token == 0x76)
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
                    else
                    {
                        flag = false;
                    }
                }
                else
                {
                    flag = false;
                }
            }
            finally
            {
                this.SyntaxTree.Pop();
            }
            node.Range.EndPoint = base.prevPosition;
            return flag;
        }

        protected virtual bool ParseLocalConstantDeclarationStatement()
        {
            Point tokenPosition = this.TokenPosition;
            this.MoveNext();
            ISyntaxNode node = null;
            bool flag = this.ParseVariableDeclaration(tokenPosition, NetNodeType.Constant, ref node);
            if (!this.Expected(CsLexerToken.Semicolon))
            {
                flag = false;
            }
            if (node != null)
            {
                node.Range.EndPoint = base.prevPosition;
            }
            return flag;
        }

        protected virtual bool ParseLockStatement()
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, this.TokenString, 0x66, SyntaxNodeOptions.Indentation);
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
            node.Range.EndPoint = base.prevPosition;
            return flag;
        }

        protected virtual bool ParseMemberAccess(ref ISyntaxNode node)
        {
            return this.ParseMemberAccess(ref node, 0x9f);
        }

        protected virtual bool ParseMemberAccess(ref ISyntaxNode node, int nodeType)
        {
            string str;
            bool flag = true;
            node = this.CreateExpressionNode(this.TokenPosition, this.TokenString, nodeType, node, true);
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
            node.Range.EndPoint = base.prevPosition;
            return flag;
        }

        protected virtual bool ParseMemberInitializerList(ref ISyntaxNode node, int nodeType)
        {
            bool flag = true;
            node = new SyntaxNode(this.TokenPosition, this.TokenString, nodeType, SyntaxNodeOptions.Indentation);
            if (this.Token != 0x67)
            {
                this.SyntaxError(0x67);
                return false;
            }
            this.MoveNext();
            while (!this.Eof && (this.Token != 0x6c))
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
                if (this.Token != 110)
                {
                    break;
                }
                this.MoveNext();
            }
            if (!this.Expected(CsLexerToken.Close_brace))
            {
                flag = false;
            }
            node.Range.EndPoint = base.prevPosition;
            return flag;
        }

        protected virtual bool ParseMethodBody()
        {
            if (!this.SkipTo(0x67, 0x71))
            {
                return false;
            }
            bool flag = true;
            ISyntaxNode current = this.SyntaxTree.Current;
            CsLexerToken token = (CsLexerToken) this.Token;
            if (token != CsLexerToken.Open_brace)
            {
                if (token == CsLexerToken.Semicolon)
                {
                    current.Range.EndPoint = this.CurrentPosition;
                    this.MoveNext();
                    return flag;
                }
                this.SyntaxError();
                return false;
            }
            current.Options |= SyntaxNodeOptions.Outlining | SyntaxNodeOptions.Indentation;
            if ((this.Token == 0x67) && !this.ParseBlockStatement())
            {
                flag = false;
            }
            current.Range.EndPoint = base.prevPosition;
            return flag;
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
                if (this.IsTypeParameterConstraintsClause(this.Token) && !this.ParseTypeParameterConstraintsClauses())
                {
                    flag = false;
                }
                node.AddAttribute(new SyntaxAttribute(base.prevPosition, SyntaxConsts.DeclarationScope, null));
                if (!this.ParseMethodBody())
                {
                    flag = false;
                }
            }
            finally
            {
                this.SyntaxTree.Pop();
            }
            node.Range.EndPoint = base.prevPosition;
            if (!this.AfterDeclaration(node))
            {
                flag = false;
            }
            return flag;
        }

        protected virtual bool ParseMethodDeclaration(ISyntaxAttributes attrs, string type, Point typePos, string name)
        {
            ISyntaxNode node = new SyntaxNode(((attrs != null) && (attrs.Count > 0)) ? attrs[0].Position : typePos, name, 0x11, SyntaxNodeOptions.CodeCompletion);
            if (attrs != null)
            {
                node.AddAttributes(attrs);
            }
            node.AddAttribute(new SyntaxAttribute(typePos, NetNodeType.Type.ToString(), type));
            this.AddNode(node);
            return this.ParseMethodDeclaration(node);
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
                case 0x7b:
                case 0x7c:
                case 0x7d:
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
                    node.Range.EndPoint = base.prevPosition;
                    return flag;
                }
            }
            return flag;
        }

        protected virtual bool ParseNamespaceAlias(ref ISyntaxNode node)
        {
            return this.ParseMemberAccess(ref node, 160);
        }

        protected virtual bool ParseNewExpression(ref ISyntaxNode node)
        {
            string str;
            bool flag = true;
            node = new SyntaxNode(this.TokenPosition, this.TokenString, 0x9b);
            this.MoveNext();
            if (this.Token == 0x69)
            {
                this.MoveNext();
                this.Expected(CsLexerToken.Close_bracket);
                node.NodeType = 0x9a;
            }
            if (this.Token == 0x67)
            {
                ISyntaxNode node2 = null;
                flag = this.ParseMemberInitializerList(ref node2, 0xb5);
                if (node2 != null)
                {
                    node.AddChild(node2);
                }
                node.Range.EndPoint = base.prevPosition;
                return flag;
            }
            Point tokenPosition = this.TokenPosition;
            flag = this.ParseTypeName(out str);
            if (!(str != string.Empty))
            {
                base.prevPosition = this.TokenPosition;
                flag = false;
            }
            else
            {
                node.AddAttribute(new SyntaxAttribute(tokenPosition, NetNodeType.Type.ToString(), str));
                switch (this.Token)
                {
                    case 0x67:
                    {
                        ISyntaxNode node3 = null;
                        flag = this.ParseMemberInitializerList(ref node3, 180);
                        if (node3 != null)
                        {
                            node.AddChild(node3);
                        }
                        goto Label_01A5;
                    }
                    case 0x69:
                    {
                        string str2;
                        node.NodeType = 0x9a;
                        ISyntaxNode node5 = null;
                        flag = this.ParseArgumentExpressionList(ref node5);
                        if (node5 != null)
                        {
                            node.AddChild(node5);
                        }
                        if (!this.TryParseRankSpecifiers(out str2))
                        {
                            flag = false;
                        }
                        if (this.Token == 0x67)
                        {
                            node5 = null;
                            if (!this.ParseArrayInitializerExpression(ref node5))
                            {
                                flag = false;
                            }
                            if (node5 != null)
                            {
                                node.AddChild(node5);
                            }
                        }
                        goto Label_01A5;
                    }
                    case 0x6b:
                    {
                        ISyntaxNode node4 = null;
                        flag = this.ParseArgumentList(ref node4);
                        if (node4 != null)
                        {
                            node.AddChild(node4);
                        }
                        goto Label_01A5;
                    }
                }
                flag = false;
                this.SyntaxError(StringConsts.ErrNewExpression);
            }
        Label_01A5:
            node.Range.EndPoint = base.prevPosition;
            return flag;
        }

        protected virtual bool ParseNullableType(out string type)
        {
            type = this.TokenString;
            this.MoveNext();
            return true;
        }

        protected virtual bool ParseNullCoalescingExpression(ref ISyntaxNode node)
        {
            bool flag = this.ParseConditionalOrExpression(ref node);
            if (this.Token == 0x80)
            {
                node = this.CreateExpressionNode(this.TokenPosition, this.TokenString, 0xae, node, true);
                this.SyntaxTree.Push(node);
                try
                {
                    this.MoveNext();
                    ISyntaxNode node2 = null;
                    if (!this.ParseNullCoalescingExpression(ref node2))
                    {
                        flag = false;
                    }
                    if (node2 != null)
                    {
                        node.AddChild(node2);
                    }
                }
                finally
                {
                    this.SyntaxTree.Pop();
                }
                node.Range.EndPoint = base.prevPosition;
            }
            return flag;
        }

        protected virtual bool ParseOperatorDeclaration(ISyntaxAttributes attrs)
        {
            string tokenString = this.TokenString;
            Point tokenPosition = this.TokenPosition;
            this.MoveNext();
            return this.ParseOperatorDeclaration(null, tokenString, tokenPosition, true);
        }

        protected virtual bool ParseOperatorDeclaration(ISyntaxAttributes attrs, string type, Point typePos, bool isExplicit)
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(((attrs != null) && (attrs.Count > 0)) ? attrs[0].Position : typePos, this.TokenString, 0x17);
            this.AddNode(node);
            if (attrs != null)
            {
                node.AddAttributes(attrs);
            }
            node.AddAttribute(new SyntaxAttribute(typePos, NetNodeType.Type.ToString(), type));
            if (!this.BeforeDeclaration(node))
            {
                flag = false;
            }
            this.SyntaxTree.Push(node);
            try
            {
                this.MoveNext();
                if (isExplicit)
                {
                    typePos = this.TokenPosition;
                    if (this.ParseType(out type))
                    {
                        node.AddAttribute(new SyntaxAttribute(typePos, NetNodeType.Operator.ToString(), type));
                    }
                    else
                    {
                        flag = false;
                    }
                }
                else
                {
                    switch (((CsLexerToken) this.Token))
                    {
                        case CsLexerToken.Tilde:
                        case CsLexerToken.Plus:
                        case CsLexerToken.Minus:
                        case CsLexerToken.Bang:
                        case CsLexerToken.Op_lt:
                        case CsLexerToken.Op_gt:
                        case CsLexerToken.Bitwise_and:
                        case CsLexerToken.Bitwise_or:
                        case CsLexerToken.Star:
                        case CsLexerToken.Percent:
                        case CsLexerToken.Div:
                        case CsLexerToken.Carret:
                        case CsLexerToken.Op_inc:
                        case CsLexerToken.Op_dec:
                        case CsLexerToken.Op_shift_left:
                        case CsLexerToken.Op_shift_right:
                        case CsLexerToken.Op_le:
                        case CsLexerToken.Op_ge:
                        case CsLexerToken.Op_eq:
                        case CsLexerToken.Op_ne:
                        case CsLexerToken.False:
                        case CsLexerToken.True:
                            node.AddAttribute(new SyntaxAttribute(this.TokenPosition, NetNodeType.Operator.ToString(), this.TokenString));
                            this.MoveNext();
                            goto Label_015D;
                    }
                    flag = false;
                    this.SyntaxError();
                }
            Label_015D:
                if (flag)
                {
                    if (this.Expected(CsLexerToken.Open_parens))
                    {
                        if (!this.ParseOperatorType())
                        {
                            flag = false;
                        }
                        if (!this.Expected(CsLexerToken.Close_parens))
                        {
                            flag = false;
                        }
                    }
                    node.AddAttribute(new SyntaxAttribute(base.prevPosition, SyntaxConsts.DeclarationScope, null));
                    if (!this.ParseMethodBody())
                    {
                        flag = false;
                    }
                }
            }
            finally
            {
                this.SyntaxTree.Pop();
            }
            if (!this.AfterDeclaration(node))
            {
                flag = false;
            }
            node.Range.EndPoint = base.prevPosition;
            return flag;
        }

        protected virtual bool ParseOperatorType()
        {
            Point tokenPosition = this.TokenPosition;
            bool flag = true;
            while (!this.Eof)
            {
                string str;
                if (this.ParseType(out str))
                {
                    string str2;
                    Point position = this.TokenPosition;
                    if (this.ParseIdentifier(out str2))
                    {
                        ISyntaxNode node = new SyntaxNode(position, str2, 0x18);
                        this.AddNode(node);
                        node.AddAttribute(new SyntaxAttribute(tokenPosition, NetNodeType.Type.ToString(), str));
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
                if (this.Token != 110)
                {
                    return flag;
                }
                this.MoveNext();
            }
            return flag;
        }

        protected virtual bool ParseOptionalExressionInBracket()
        {
            bool flag = true;
            if (this.Token == 0x69)
            {
                this.MoveNext();
                if (this.Token != 0x6a)
                {
                    ISyntaxNode node = null;
                    if (!this.ParseExpression(ref node))
                    {
                        flag = false;
                    }
                    if (node != null)
                    {
                        this.AddNode(node);
                    }
                }
                if (!this.Expected(CsLexerToken.Close_bracket))
                {
                    flag = false;
                }
            }
            return flag;
        }

        protected bool ParseOptionalTypeAndIdentifier(ISyntaxNode node)
        {
            bool flag = true;
            Point tokenPosition = this.TokenPosition;
            string identifier = string.Empty;
            this.SaveState();
            try
            {
                string str;
                flag = this.ParseType(out str) && this.ParseIdentifier(out identifier);
                if (flag)
                {
                    node.AddAttribute(new SyntaxAttribute(tokenPosition, NetNodeType.Type.ToString(), str));
                }
            }
            finally
            {
                this.RestoreState(!flag);
            }
            if (!flag)
            {
                flag = this.ParseIdentifier(out identifier);
            }
            if (flag)
            {
                node.Name = identifier;
            }
            return flag;
        }

        protected virtual bool ParseOrderByClause()
        {
            bool flag = true;
            if (this.Token != 0x5e)
            {
                return flag;
            }
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, this.TokenString, 0xbd, SyntaxNodeOptions.Indentation);
            this.AddNode(node);
            this.SyntaxTree.Push(node);
            try
            {
                this.MoveNext();
                while (!this.Eof)
                {
                    ISyntaxNode node2 = null;
                    if (!this.ParseExpression(ref node2))
                    {
                        flag = false;
                    }
                    if (node2 != null)
                    {
                        node.AddChild(node2);
                        if (this.IsOrderingDirection())
                        {
                            node2.AddAttribute(new SyntaxAttribute(this.TokenPosition, NetNodeType.OrderingDirection.ToString(), this.TokenString));
                            this.MoveNext();
                        }
                    }
                    if (this.Token != 110)
                    {
                        goto Label_00BA;
                    }
                    this.MoveNext();
                }
            }
            finally
            {
                this.SyntaxTree.Pop();
            }
        Label_00BA:
            node.Range.EndPoint = base.prevPosition;
            return flag;
        }

        protected virtual bool ParseParameterDeclaration()
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, string.Empty, 0x1a, SyntaxNodeOptions.CodeCompletion);
            CsLexerToken token = (CsLexerToken) this.Token;
            if (token <= CsLexerToken.Ref)
            {
                switch (token)
                {
                    case CsLexerToken.Out:
                    case CsLexerToken.Params:
                    case CsLexerToken.Ref:
                        goto Label_0074;
                }
                goto Label_009F;
            }
            if (token != CsLexerToken.This)
            {
                if (token != CsLexerToken.Open_bracket)
                {
                    goto Label_009F;
                }
                this.SyntaxTree.Push(node);
                try
                {
                    if (!this.ParseAttributeListDeclaration())
                    {
                        flag = false;
                    }
                    goto Label_009F;
                }
                finally
                {
                    this.SyntaxTree.Pop();
                }
            }
        Label_0074:
            node.AddAttribute(new SyntaxAttribute(this.TokenPosition, NetNodeType.ParameterModifier.ToString(), this.TokenString));
            this.MoveNext();
        Label_009F:
            if (this.Token == 0x51)
            {
                this.MoveNext();
                node.Name = this.TokenString;
                if (this.Token != 0x6c)
                {
                    this.SyntaxError(0x6c);
                    flag = false;
                }
                this.AddNode(node);
            }
            else
            {
                string str;
                string str2;
                Point tokenPosition = this.TokenPosition;
                if (this.ParseType(out str) && this.ParseIdentifier(out str2))
                {
                    node.Name = str2;
                    node.AddAttribute(new SyntaxAttribute(tokenPosition, NetNodeType.Type.ToString(), str));
                    if (this.Token == 0x69)
                    {
                        string str3;
                        this.ParseRankSpecifier(out str3);
                        this.SyntaxError(StringConsts.ErrArrayParamSpecifier);
                        flag = false;
                    }
                    if (this.Token == 0x76)
                    {
                        this.MoveNext();
                        ISyntaxNode node2 = null;
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
            node.Range.EndPoint = base.prevPosition;
            return flag;
        }

        protected virtual bool ParseParameterListDeclaration()
        {
            return this.ParseParameterListDeclaration(0x6b, 0x6c);
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
                    if (this.Token != 110)
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
            node.Range.EndPoint = base.prevPosition;
            return flag;
        }

        protected virtual bool ParseParenthesizedExpression(ref ISyntaxNode node, bool extended)
        {
            bool flag = true;
            if (this.Token == 0x6b)
            {
                node = new SyntaxNode(this.TokenPosition, this.TokenString, 0xa8);
                this.MoveNext();
                ISyntaxNode node2 = null;
                if (extended && this.TryParseLambdaParameterOrExpression(ref node2, node.Position))
                {
                    if (node2 != null)
                    {
                        node.AddChild(node2);
                    }
                }
                else
                {
                    node2 = null;
                    flag = this.ParseExpression(ref node2);
                    if (node2 != null)
                    {
                        node.AddChild(node2);
                    }
                    if (!this.Expected(CsLexerToken.Close_parens))
                    {
                        flag = false;
                    }
                }
                node.Range.EndPoint = base.prevPosition;
                return flag;
            }
            this.SyntaxError(0x6b);
            return false;
        }

        protected virtual bool ParseParenthesizedStatementExpression(ref ISyntaxNode node)
        {
            if (this.Token == 0x6b)
            {
                return this.ParseParenthesizedExpression(ref node, false);
            }
            this.Expected(CsLexerToken.Open_parens);
            return this.ParseExpression(ref node);
        }

        protected virtual bool ParsePointerMemberAccess(ref ISyntaxNode node)
        {
            return this.ParseMemberAccess(ref node, 0xa1);
        }

        protected virtual bool ParsePointerType(out string type)
        {
            type = this.TokenString;
            this.MoveNext();
            return true;
        }

        protected virtual bool ParsePositionalOrNamedParam()
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, string.Empty, 0x1a);
            this.AddNode(node);
            if (this.IsIdentifierToken(this.Token))
            {
                bool flag2 = false;
                this.SaveState();
                try
                {
                    string tokenString = this.TokenString;
                    this.MoveNext();
                    if (this.Token == 0x76)
                    {
                        flag2 = true;
                        this.MoveNext();
                        node.Name = tokenString;
                    }
                }
                finally
                {
                    this.RestoreState(!flag2);
                }
            }
            ISyntaxNode node2 = null;
            flag = this.ParseExpression(ref node2);
            if (node2 != null)
            {
                node.AddChild(node2);
            }
            node.Range.EndPoint = base.prevPosition;
            return flag;
        }

        protected virtual bool ParsePostDecrementExpression(ref ISyntaxNode node)
        {
            node = this.CreateExpressionNode(this.TokenPosition, this.TokenString, 170, node, false);
            this.MoveNext();
            node.Range.EndPoint = base.prevPosition;
            return true;
        }

        protected virtual bool ParsePostIncrementExpression(ref ISyntaxNode node)
        {
            node = this.CreateExpressionNode(this.TokenPosition, this.TokenString, 0xa9, node, false);
            this.MoveNext();
            node.Range.EndPoint = base.prevPosition;
            return true;
        }

        protected virtual bool ParsePrefixedUnaryExpression(ref ISyntaxNode node)
        {
            bool flag = true;
            switch (((CsLexerToken) this.Token))
            {
                case CsLexerToken.Tilde:
                case CsLexerToken.Plus:
                case CsLexerToken.Minus:
                case CsLexerToken.Bang:
                case CsLexerToken.Star:
                case CsLexerToken.Op_inc:
                case CsLexerToken.Op_dec:
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
                    node.Range.EndPoint = base.prevPosition;
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

        protected virtual bool ParsePropertyDeclaration(ISyntaxNode node)
        {
            bool flag = true;
            if (!this.BeforeDeclaration(node))
            {
                flag = false;
            }
            if (this.Token != 0x67)
            {
                this.SyntaxError(0x67);
                flag = false;
                goto Label_00E3;
            }
            node.Options |= SyntaxNodeOptions.Outlining | SyntaxNodeOptions.Indentation;
            node.AddAttribute(new SyntaxAttribute(this.TokenPosition, SyntaxConsts.DefinitionScope, null));
            this.SyntaxTree.Push(node);
            try
            {
                this.MoveNext();
                while (!this.Eof)
                {
                    if ((this.Token == 0x69) && !this.ParseAttributeListDeclaration())
                    {
                        flag = false;
                    }
                    ISyntaxAttributes attrs = null;
                    if (this.IsAccessorModifier(this.Token))
                    {
                        this.ParseAccessorModifiers(ref attrs);
                    }
                    if (!this.IsAccessor(NetNodeType.PropertyAccessor))
                    {
                        goto Label_00B1;
                    }
                    if (!this.ParseAccessors(attrs, NetNodeType.PropertyAccessor))
                    {
                        flag = false;
                    }
                }
            }
            finally
            {
                this.SyntaxTree.Pop();
            }
        Label_00B1:
            if (this.Expected(CsLexerToken.Close_brace))
            {
                node.AddAttribute(new SyntaxAttribute(base.prevPosition, SyntaxConsts.DefinitionScopeEnd, null));
            }
            else
            {
                flag = false;
            }
        Label_00E3:
            node.Range.EndPoint = base.prevPosition;
            if (!this.AfterDeclaration(node))
            {
                flag = false;
            }
            return flag;
        }

        protected virtual bool ParsePropertyDeclaration(ISyntaxAttributes attrs, string type, Point typePos, string name)
        {
            ISyntaxNode node = new SyntaxNode(((attrs != null) && (attrs.Count > 0)) ? attrs[0].Position : typePos, name, 0x15, SyntaxNodeOptions.CodeCompletion);
            if (attrs != null)
            {
                node.AddAttributes(attrs);
            }
            node.AddAttribute(new SyntaxAttribute(typePos, NetNodeType.Type.ToString(), type));
            this.AddNode(node);
            return this.ParsePropertyDeclaration(node);
        }

        protected virtual bool ParseQualifiedIdentifier(out string identifier)
        {
            bool flag = this.ParseTypeIdentifier(out identifier, false);
            if (flag)
            {
                while ((this.Token == 0x6d) || (this.Token == 0x70))
                {
                    string str;
                    identifier = identifier + this.TokenString;
                    this.MoveNext();
                    if (this.Token == 0x42)
                    {
                        return flag;
                    }
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

        protected virtual bool ParseQueryBody()
        {
            bool flag = true;
            if (!this.ParseFromLetWhereClauses())
            {
                flag = false;
            }
            if (!this.ParseOrderByClause())
            {
                flag = false;
            }
            if (!this.ParseSelectOrGroupClause())
            {
                flag = false;
            }
            if (!this.ParseQueryContinuation())
            {
                flag = false;
            }
            return flag;
        }

        protected virtual bool ParseQueryContinuation()
        {
            bool flag = true;
            if (this.Token == 0x61)
            {
                ISyntaxNode node = new SyntaxNode(this.TokenPosition, string.Empty, 0xbf, SyntaxNodeOptions.Indentation);
                Point tokenPosition = this.TokenPosition;
                this.AddNode(node);
                this.SyntaxTree.Push(node);
                try
                {
                    this.MoveNext();
                    string identifier = string.Empty;
                    if (this.ParseIdentifier(out identifier))
                    {
                        node.Name = identifier;
                        if ((this.Token == 0x5b) && !this.ParseJoinClause())
                        {
                            flag = false;
                        }
                        if (!this.ParseQueryBody())
                        {
                            flag = false;
                        }
                    }
                    else
                    {
                        flag = false;
                    }
                }
                finally
                {
                    this.SyntaxTree.Pop();
                }
                node.Range.EndPoint = base.prevPosition;
            }
            return flag;
        }

        protected virtual bool ParseQueryExpression(ref ISyntaxNode node)
        {
            bool flag = true;
            node = new SyntaxNode(this.TokenPosition, this.TokenString, 0xb6, SyntaxNodeOptions.Indentation);
            this.SyntaxTree.Push(node);
            try
            {
                if (!this.ParseFromClause())
                {
                    flag = false;
                }
                if (!this.ParseQueryBody())
                {
                    flag = false;
                }
            }
            finally
            {
                this.SyntaxTree.Pop();
            }
            node.Range.EndPoint = base.prevPosition;
            return flag;
        }

        protected virtual bool ParseRankSpecifier(out string rank)
        {
            bool flag = true;
            rank = this.TokenString;
            if (!this.Expected(CsLexerToken.Open_bracket))
            {
                return flag;
            }
            while (this.Token == 110)
            {
                rank = rank + this.TokenString;
                this.MoveNext();
            }
            if (this.Token == 0x6a)
            {
                rank = rank + this.TokenString;
            }
            return this.Expected(CsLexerToken.Close_bracket);
        }

        protected virtual bool ParseRelationalExpression(ref ISyntaxNode node)
        {
            bool flag = this.ParseShiftExpression(ref node);
            CsLexerToken token = (CsLexerToken) this.Token;
            if (token <= CsLexerToken.Is)
            {
                switch (token)
                {
                    case CsLexerToken.As:
                    case CsLexerToken.Is:
                        node = this.CreateExpressionNode(this.TokenPosition, this.TokenString, 0x8f, node, true);
                        this.MoveNext();
                        if (!this.ParseAsIsType(node))
                        {
                            base.prevPosition = this.TokenPosition;
                            flag = false;
                        }
                        node.Range.EndPoint = base.prevPosition;
                        return flag;
                }
                return flag;
            }
            switch (token)
            {
                case CsLexerToken.Op_lt:
                case CsLexerToken.Op_gt:
                case CsLexerToken.Op_le:
                case CsLexerToken.Op_ge:
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
                    node.Range.EndPoint = base.prevPosition;
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
            if (this.Token != 0x71)
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
            if (!this.Expected(CsLexerToken.Semicolon))
            {
                flag = false;
            }
            node.Range.EndPoint = base.prevPosition;
            return flag;
        }

        protected virtual bool ParseSelectClause()
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, this.TokenString, 190, SyntaxNodeOptions.Indentation);
            this.AddNode(node);
            this.SyntaxTree.Push(node);
            try
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
            finally
            {
                this.SyntaxTree.Pop();
            }
            node.Range.EndPoint = base.prevPosition;
            return flag;
        }

        protected virtual bool ParseSelectOrGroupClause()
        {
            switch (((CsLexerToken) this.Token))
            {
                case CsLexerToken.Select:
                    return this.ParseSelectClause();

                case CsLexerToken.Group:
                    return this.ParseGroupClause();
            }
            return false;
        }

        protected virtual bool ParseShiftExpression(ref ISyntaxNode node)
        {
            bool flag = this.ParseAdditiveExpression(ref node);
            switch (this.Token)
            {
                case 0x83:
                case 0x84:
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
                    node.Range.EndPoint = base.prevPosition;
                    return flag;
                }
            }
            return flag;
        }

        protected virtual bool ParseSimpleExpression(ref ISyntaxNode node)
        {
            bool flag = true;
            switch (((CsLexerToken) this.Token))
            {
                case CsLexerToken.Long:
                case CsLexerToken.Object:
                case CsLexerToken.Int:
                case CsLexerToken.Float:
                case CsLexerToken.Bool:
                case CsLexerToken.Byte:
                case CsLexerToken.Char:
                case CsLexerToken.Decimal:
                case CsLexerToken.Double:
                case CsLexerToken.Sbyte:
                case CsLexerToken.Short:
                case CsLexerToken.String:
                case CsLexerToken.Uint:
                case CsLexerToken.Ulong:
                case CsLexerToken.Ushort:
                case CsLexerToken.Void:
                    node = new SyntaxNode(this.TokenPosition, this.TokenString, 0x99);
                    this.MoveNext();
                    if (this.Token != 0x6d)
                    {
                        flag = false;
                    }
                    return flag;

                case CsLexerToken.New:
                    return this.ParseNewExpression(ref node);

                case CsLexerToken.Null:
                case CsLexerToken.False:
                case CsLexerToken.True:
                case CsLexerToken.Value:
                case CsLexerToken.Global:
                case CsLexerToken.Integer_Literal:
                case CsLexerToken.Float_Literal:
                case CsLexerToken.Double_Literal:
                case CsLexerToken.Decimal_Literal:
                case CsLexerToken.Character_Literal:
                case CsLexerToken.String_Literal:
                case CsLexerToken.Identifier_Literal:
                    node = new SyntaxNode(this.TokenPosition, this.TokenString, 0x99);
                    this.MoveNext();
                    return flag;

                case CsLexerToken.Base:
                    return this.ParseBaseAccess(ref node);

                case CsLexerToken.Checked:
                    return this.ParseCheckedExpression(ref node);

                case CsLexerToken.Default:
                    return this.ParseDefaultExpression(ref node);

                case CsLexerToken.Delegate:
                    return this.ParseAnonymousMethodExpression(ref node);

                case CsLexerToken.Sizeof:
                    return this.ParseSizeofExpression(ref node);

                case CsLexerToken.This:
                    return this.ParseThisAccess(ref node);

                case CsLexerToken.Typeof:
                    return this.ParseTypeofExpression(ref node);

                case CsLexerToken.Unchecked:
                    return this.ParseUncheckedExpression(ref node);

                case CsLexerToken.From:
                    return this.ParseQueryExpression(ref node);

                case CsLexerToken.Open_parens:
                    return this.ParseParenthesizedExpression(ref node, true);

                case CsLexerToken.Open_brace:
                    return this.ParseMemberInitializerList(ref node, 180);
            }
            flag = false;
            this.SyntaxError();
            this.MoveNext();
            return flag;
        }

        protected virtual bool ParseSizeofExpression(ref ISyntaxNode node)
        {
            return this.ParseTypeofExpression(NetNodeType.SizeofExpression, ref node);
        }

        protected virtual bool ParseStatement()
        {
            bool flag = true;
            if (this.Token == 0xa2)
            {
                if (!this.ParseDirective())
                {
                    flag = false;
                }
                return flag;
            }
            if (this.Token == 14)
            {
                return this.ParseLocalConstantDeclarationStatement();
            }
            if (this.IsBuiltInType(this.Token))
            {
                ISyntaxNode node = null;
                flag = this.ParseVariableDeclaration(this.TokenPosition, NetNodeType.LocalVariable, ref node);
                if (!this.Expected(CsLexerToken.Semicolon))
                {
                    flag = false;
                }
                if (node != null)
                {
                    node.Range.EndPoint = base.prevPosition;
                }
                return flag;
            }
            if (this.IsImplicitVariableDeclaration(this.Token))
            {
                return this.ParseImplicitVariableDeclaration();
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
                while (this.Token == 110)
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
            while (!this.Eof && (this.Token != 0x68))
            {
                if (!this.ParseStatement())
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
            if (this.Expected(CsLexerToken.Open_brace))
            {
                this.AddAttribute(new SyntaxAttribute(tokenPosition, SyntaxConsts.DefinitionScope, null));
                while (!this.Eof && ((this.Token == 9) || (this.Token == 0x11)))
                {
                    if (!this.ParseSwitchSection())
                    {
                        flag = false;
                    }
                }
                if (this.Expected(CsLexerToken.Close_brace))
                {
                    this.AddAttribute(new SyntaxAttribute(base.prevPosition, SyntaxConsts.DefinitionScopeEnd, null));
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
            switch (((CsLexerToken) this.Token))
            {
                case CsLexerToken.Case:
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
                case CsLexerToken.Default:
                    this.MoveNext();
                    break;
            }
            if (!this.Expected(CsLexerToken.Colon))
            {
                flag = false;
            }
            node.Range.EndPoint = base.prevPosition;
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
            while ((this.Token == 9) || (this.Token == 0x11));
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
                if (this.Token != 0x67)
                {
                    goto Label_0051;
                }
                if (!this.ParseBlock())
                {
                    flag = false;
                }
                goto Label_0086;
            Label_0047:
                if (!this.ParseStatement())
                {
                    flag = false;
                }
            Label_0051:
                if ((!this.Eof && (this.Token != 9)) && ((this.Token != 0x11) && (this.Token != 0x68)))
                {
                    goto Label_0047;
                }
            }
            finally
            {
                this.SyntaxTree.Pop();
            }
        Label_0086:
            node.Range.EndPoint = base.prevPosition;
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
            node.Range.EndPoint = base.prevPosition;
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
                CsLexerToken token = (CsLexerToken) this.Token;
                if (token != CsLexerToken.Catch)
                {
                    if (token == CsLexerToken.Finally)
                    {
                        goto Label_005A;
                    }
                    goto Label_0066;
                }
                if (!this.ParseCatchStatements())
                {
                    flag = false;
                }
                goto Label_0082;
            Label_005A:
                if (!this.ParseFinallyStatement())
                {
                    flag = false;
                }
                goto Label_0082;
            Label_0066:
                flag = false;
                this.SyntaxError(StringConsts.ErrCatchOrFinallyExpected);
            }
            finally
            {
                this.SyntaxTree.Pop();
            }
        Label_0082:
            node.Range.EndPoint = base.prevPosition;
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

        protected virtual bool ParseTypeArgumentList(out string typeList)
        {
            string str;
            typeList = string.Empty;
            bool flag = this.Expected(CsLexerToken.Op_lt);
            if (!flag)
            {
                return flag;
            }
            typeList = "<";
            flag = this.ParseType(out str);
            if (flag)
            {
                typeList = typeList + str;
            }
            while (this.Token == 110)
            {
                this.MoveNext();
                if (this.ParseType(out str))
                {
                    typeList = typeList + "," + str;
                }
            }
            if (this.Token == 0x84)
            {
                base.tokenPos++;
                this.Token = 120;
                typeList = typeList + ">";
                return flag;
            }
            if (this.Expected(CsLexerToken.Op_gt))
            {
                typeList = typeList + ">";
                return flag;
            }
            return false;
        }

        protected virtual bool ParseTypeIdentifier(out string identifier, bool checkBuiltIn)
        {
            identifier = this.TokenString;
            bool flag = true;
            if (checkBuiltIn && this.IsBuiltInType(this.Token))
            {
                this.MoveNext();
            }
            else
            {
                flag = this.IdentifierExpected();
            }
            if (flag)
            {
                string str;
                string str2;
                string str3;
                if (this.IsTypeArgumentList(this.Token) && !this.ParseTypeArgumentList(out str))
                {
                    flag = false;
                }
                if (this.IsNullableType(this.Token) && !this.ParseNullableType(out str2))
                {
                    flag = false;
                }
                if (this.IsPointerType(this.Token) && !this.ParsePointerType(out str3))
                {
                    flag = false;
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

        protected virtual bool ParseTypeofExpression(ref ISyntaxNode node)
        {
            return this.ParseTypeofExpression(NetNodeType.TypeofExpression, ref node);
        }

        protected virtual bool ParseTypeofExpression(NetNodeType nodeType, ref ISyntaxNode node)
        {
            bool flag = true;
            node = new SyntaxNode(this.TokenPosition, this.TokenString, (int) nodeType);
            this.MoveNext();
            if (this.Expected(CsLexerToken.Open_parens))
            {
                string str;
                Point tokenPosition = this.TokenPosition;
                if (this.ParseType(out str))
                {
                    node.AddAttribute(new SyntaxAttribute(tokenPosition, NetNodeType.Type.ToString(), str));
                    if (!this.Expected(CsLexerToken.Close_parens))
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
            node.Range.EndPoint = base.prevPosition;
            return flag;
        }

        protected virtual bool ParseTypeParameter()
        {
            string str;
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, string.Empty, 0x34);
            this.AddNode(node);
            if (this.Token == 0x69)
            {
                this.SyntaxTree.Push(node);
                try
                {
                    if (!this.ParseAttributeListDeclaration())
                    {
                        flag = false;
                    }
                }
                finally
                {
                    this.SyntaxTree.Pop();
                }
            }
            if (this.ParseIdentifier(out str))
            {
                node.Name = str;
                return flag;
            }
            return false;
        }

        protected virtual bool ParseTypeParameterConstraint()
        {
            bool flag = true;
            Point tokenPosition = this.TokenPosition;
            string type = string.Empty;
            CsLexerToken token = (CsLexerToken) this.Token;
            if (token != CsLexerToken.Class)
            {
                if (token == CsLexerToken.New)
                {
                    type = this.TokenString;
                    this.MoveNext();
                    if (!this.Expected(CsLexerToken.Open_parens) || !this.Expected(CsLexerToken.Close_parens))
                    {
                        flag = false;
                    }
                    goto Label_006B;
                }
                if (token != CsLexerToken.Struct)
                {
                    if (!this.ParseTypeName(out type))
                    {
                        flag = false;
                    }
                    goto Label_006B;
                }
            }
            type = this.TokenString;
            this.MoveNext();
        Label_006B:
            if (flag)
            {
                ISyntaxNode node = new SyntaxNode(tokenPosition, type, 0x36);
                this.AddNode(node);
                node.Range.EndPoint = base.prevPosition;
            }
            return flag;
        }

        protected virtual bool ParseTypeParameterConstraints()
        {
            bool flag = this.ParseTypeParameterConstraint();
            while (this.Token == 110)
            {
                this.MoveNext();
                if (this.ParseTypeParameterConstraint())
                {
                    flag = false;
                }
            }
            return flag;
        }

        protected virtual bool ParseTypeParameterConstraintsClause()
        {
            string str;
            bool flag = this.Expected(CsLexerToken.Where);
            Point tokenPosition = this.TokenPosition;
            if (!this.ParseIdentifier(out str))
            {
                return false;
            }
            ISyntaxNode node = new SyntaxNode(tokenPosition, str, 0x38);
            this.AddNode(node);
            if (this.Expected(CsLexerToken.Colon))
            {
                this.SyntaxTree.Push(node);
                try
                {
                    if (!this.ParseTypeParameterConstraints())
                    {
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
                flag = false;
            }
            node.Range.EndPoint = base.prevPosition;
            return flag;
        }

        protected virtual bool ParseTypeParameterConstraintsClauses()
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, string.Empty, 0x37);
            this.AddNode(node);
            this.SyntaxTree.Push(node);
            try
            {
                while (this.IsTypeParameterConstraintsClause(this.Token))
                {
                    if (!this.ParseTypeParameterConstraintsClause())
                    {
                        flag = false;
                    }
                }
            }
            finally
            {
                this.SyntaxTree.Pop();
            }
            node.Range.EndPoint = base.prevPosition;
            return flag;
        }

        protected virtual bool ParseTypeParameterList()
        {
            bool flag = true;
            if (this.Expected(CsLexerToken.Op_lt))
            {
                ISyntaxNode node = new SyntaxNode(this.TokenPosition, string.Empty, 0x35);
                this.AddNode(node);
                this.SyntaxTree.Push(node);
                try
                {
                    flag = this.ParseTypeParameter();
                    while (this.Token == 110)
                    {
                        this.MoveNext();
                        if (!this.ParseTypeParameter())
                        {
                            flag = false;
                        }
                    }
                }
                finally
                {
                    this.SyntaxTree.Pop();
                }
                if (this.Token == 0x84)
                {
                    base.tokenPos++;
                    this.Token = 120;
                }
                else if (!this.Expected(CsLexerToken.Op_gt))
                {
                    flag = false;
                }
                node.Range.EndPoint = base.prevPosition;
            }
            return flag;
        }

        protected virtual bool ParseUnaryExpression(ref ISyntaxNode node)
        {
            bool flag = false;
            if ((this.Token == 0x6b) && this.TryParseCastExpression(ref node))
            {
                flag = true;
            }
            if (!flag)
            {
                flag = this.ParsePrimaryExpression(ref node);
            }
            return flag;
        }

        protected virtual bool ParseUncheckedExpression(ref ISyntaxNode node)
        {
            return this.ParseCheckedExpression(NetNodeType.UncheckedExpression, ref node);
        }

        protected virtual bool ParseUncheckedStatement()
        {
            return this.ParseCheckedStatement(NetNodeType.UncheckedStatement);
        }

        protected override bool ParseUnitBody()
        {
            return this.ParseClassBody();
        }

        protected virtual bool ParseUnknownMemberDeclaration(ISyntaxAttributes attrs)
        {
            string str;
            Point tokenPosition = this.TokenPosition;
            bool flag = this.ParseType(out str);
            if (!flag)
            {
                if (this.TokenPosition.Equals(tokenPosition))
                {
                    this.MoveNext();
                }
                return flag;
            }
            CsLexerToken token = (CsLexerToken) this.Token;
            if (token <= CsLexerToken.This)
            {
                switch (token)
                {
                    case CsLexerToken.Operator:
                        if (!this.ParseOperatorDeclaration(attrs, str, tokenPosition, false))
                        {
                            flag = false;
                        }
                        return flag;

                    case CsLexerToken.This:
                        if (!this.ParseIndexerDeclaration(attrs, str, tokenPosition))
                        {
                            flag = false;
                        }
                        return flag;
                }
            }
            else
            {
                string str2;
                if (token != CsLexerToken.Value)
                {
                    if (token == CsLexerToken.Open_parens)
                    {
                        if (!this.ParseConstructorDeclaration(attrs, str, tokenPosition))
                        {
                            flag = false;
                        }
                        return flag;
                    }
                    if (token != CsLexerToken.Identifier_Literal)
                    {
                        goto Label_014E;
                    }
                }
                Point point1 = this.TokenPosition;
                if (!this.ParseQualifiedIdentifier(out str2) && (!(str2 != string.Empty) || (this.Token != 0x42)))
                {
                    return flag;
                }
                switch (((CsLexerToken) this.Token))
                {
                    case CsLexerToken.Comma:
                    case CsLexerToken.Semicolon:
                    case CsLexerToken.Assign:
                    case CsLexerToken.Open_bracket:
                        if (!this.ParseFieldDeclaration(attrs, str, tokenPosition, str2))
                        {
                            flag = false;
                        }
                        return flag;

                    case CsLexerToken.Open_brace:
                        if (!this.ParsePropertyDeclaration(attrs, str, tokenPosition, str2))
                        {
                            flag = false;
                        }
                        return flag;

                    case CsLexerToken.Open_parens:
                        if (!this.ParseMethodDeclaration(attrs, str, tokenPosition, str2))
                        {
                            flag = false;
                        }
                        return flag;

                    case CsLexerToken.This:
                        if (!this.ParseIndexerDeclaration(attrs, str, tokenPosition))
                        {
                            flag = false;
                        }
                        return flag;
                }
                this.SyntaxError();
                return false;
            }
        Label_014E:
            this.SyntaxError();
            return false;
        }

        protected virtual bool ParseUnsafeStatement()
        {
            return this.ParseCheckedStatement(NetNodeType.UnsafeStatement);
        }

        protected virtual bool ParseUsingDeclaration()
        {
            return this.ParseUsingDeclaration(new SyntaxNode());
        }

        protected override bool ParseUsingDeclaration(ISyntaxNode node)
        {
            string str;
            bool flag = true;
            Point tokenPosition = this.TokenPosition;
            if (this.ParseQualifiedIdentifier(out str) || (str != string.Empty))
            {
                node.NodeType = 3;
                node.Position = tokenPosition;
                node.Name = str;
                this.AddNode(node);
                if (this.Token == 0x76)
                {
                    this.MoveNext();
                    Point position = this.TokenPosition;
                    if (this.ParseQualifiedIdentifier(out str))
                    {
                        node.AddAttribute(new SyntaxAttribute(position, NetNodeType.UsingAlias.ToString(), str));
                    }
                    else
                    {
                        flag = false;
                    }
                }
                if (this.Expected(CsLexerToken.Semicolon))
                {
                    flag = false;
                }
                node.Range.EndPoint = base.prevPosition;
                return flag;
            }
            return false;
        }

        protected virtual bool ParseUsingList()
        {
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, string.Empty, 2, SyntaxNodeOptions.Outlining);
            return this.ParseUsingList(node);
        }

        protected override bool ParseUsingList(ISyntaxNode node)
        {
            bool flag = true;
            node.AddAttribute(new SyntaxAttribute(this.TokenPosition, SyntaxConsts.OutlineText, SyntaxParserConsts.OutlineUsingText));
            this.AddNode(node);
            if (!this.BeforeDeclaration(node))
            {
                flag = false;
            }
            this.SyntaxTree.Push(node);
            try
            {
                while (this.Token == 0x4c)
                {
                    this.MoveNext();
                    this.ParseUsingDeclaration();
                }
            }
            finally
            {
                this.SyntaxTree.Pop();
            }
            if (!this.AfterDeclaration(node))
            {
                flag = false;
            }
            node.Range.EndPoint = base.prevPosition;
            return flag;
        }

        protected virtual bool ParseUsingStatement()
        {
            bool flag = true;
            ISyntaxNode current = this.SyntaxTree.Current;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, this.TokenString, 0x67, ((current.NodeType == 0x67) && (current.FindAttribute(SyntaxConsts.DefinitionScope) == null)) ? SyntaxNodeOptions.BackIndentation : SyntaxNodeOptions.Indentation);
            this.AddNode(node);
            this.SyntaxTree.Push(node);
            try
            {
                this.MoveNext();
                if (this.Expected(CsLexerToken.Open_parens))
                {
                    this.SaveState();
                    try
                    {
                        flag = this.ParseVariableDeclaration(this.TokenPosition, (this.Token == 0x58) ? NetNodeType.ImplicitVariable : NetNodeType.LocalVariable);
                    }
                    finally
                    {
                        this.RestoreState(!flag);
                    }
                    if (!flag)
                    {
                        ISyntaxNode node3 = null;
                        flag = this.ParseExpression(ref node3);
                        if (node3 != null)
                        {
                            node.AddChild(node3);
                        }
                    }
                    if (!this.Expected(CsLexerToken.Close_parens))
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
            node.Range.EndPoint = base.prevPosition;
            return flag;
        }

        protected virtual bool ParseVariableDeclaration(Point position, NetNodeType nodeType)
        {
            ISyntaxNode node = null;
            return this.ParseVariableDeclaration(position, nodeType, ref node);
        }

        protected virtual bool ParseVariableDeclaration(Point position, NetNodeType nodeType, ref ISyntaxNode node)
        {
            bool isImplicit = this.IsImplicitVariableDeclaration(this.Token);
            return this.ParseVariableDeclaration(position, nodeType, isImplicit, isImplicit, ref node);
        }

        protected virtual bool ParseVariableDeclaration(Point position, NetNodeType nodeType, bool isImplicit, bool required, ref ISyntaxNode node)
        {
            string str;
            Point tokenPosition = this.TokenPosition;
            string type = string.Empty;
            bool flag = isImplicit || this.ParseType(out type);
            if (isImplicit)
            {
                this.MoveNext();
            }
            Point point2 = this.TokenPosition;
            if (flag && this.ParseIdentifier(out str))
            {
                node = new SyntaxNode(position, str, (int) nodeType, SyntaxNodeOptions.CodeCompletion | SyntaxNodeOptions.Indentation);
                node.AddAttribute(new SyntaxAttribute(point2, NetNodeType.Name.ToString(), str));
                if (type != string.Empty)
                {
                    node.AddAttribute(new SyntaxAttribute(tokenPosition, NetNodeType.Type.ToString(), type));
                }
                this.AddNode(node);
                this.SyntaxTree.Push(node);
                try
                {
                    if (!this.ParseOptionalExressionInBracket())
                    {
                        flag = false;
                    }
                    if (required && (this.Token != 0x76))
                    {
                        flag = this.Expected(CsLexerToken.Assign);
                    }
                    else if (!this.ParseVariableDeclarators(nodeType, isImplicit))
                    {
                        flag = false;
                    }
                }
                finally
                {
                    this.SyntaxTree.Pop();
                }
                node.Range.EndPoint = base.prevPosition;
                return flag;
            }
            return false;
        }

        protected virtual bool ParseVariableDeclarators(NetNodeType nodeType, bool isImplicit)
        {
            bool flag = true;
            if (this.Token == 0x76)
            {
                ISyntaxNode child = null;
                ISyntaxNode node = new SyntaxNode(this.TokenPosition, this.TokenString, 0x21);
                int token = 0;
                this.AddNode(node);
                this.SyntaxTree.Push(node);
                try
                {
                    this.MoveNext();
                    token = this.Token;
                    if (!this.ParseVariableInitializer(ref child))
                    {
                        flag = false;
                    }
                }
                finally
                {
                    this.SyntaxTree.Pop();
                }
                node.Range.EndPoint = base.prevPosition;
                if (isImplicit && (child != null))
                {
                    Point point;
                    string str = this.GetExpressionType(child, token, out point);
                    if (str != string.Empty)
                    {
                        this.AddAttribute(new SyntaxAttribute(point, NetNodeType.Type.ToString(), str));
                    }
                }
            }
            while (this.Token == 110)
            {
                string str2;
                this.MoveNext();
                Point tokenPosition = this.TokenPosition;
                if (this.ParseIdentifier(out str2))
                {
                    ISyntaxNode node3 = new SyntaxNode(tokenPosition, str2, (int) nodeType, SyntaxNodeOptions.CodeCompletion);
                    this.AddNode(node3);
                    this.SyntaxTree.Push(node3);
                    try
                    {
                        if (!this.ParseOptionalExressionInBracket())
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
            return flag;
        }

        protected virtual bool ParseVariableInitializer(ref ISyntaxNode child)
        {
            bool flag = true;
            switch (((CsLexerToken) this.Token))
            {
                case CsLexerToken.Stackalloc:
                    string str;
                    this.MoveNext();
                    if (!this.ParseType(out str))
                    {
                        return flag;
                    }
                    if (this.Expected(CsLexerToken.Open_bracket))
                    {
                        if (!this.ParseOptionalExressionInBracket())
                        {
                            flag = false;
                        }
                        return flag;
                    }
                    return false;

                case CsLexerToken.Open_brace:
                    flag = this.ParseArrayInitializerExpression(ref child);
                    if (child != null)
                    {
                        this.AddNode(child);
                    }
                    return flag;
            }
            flag = this.ParseExpression(ref child);
            if (child != null)
            {
                this.AddNode(child);
            }
            return flag;
        }

        protected virtual bool ParseVariableReference(ref ISyntaxNode node)
        {
            return this.ParseExpression(ref node);
        }

        protected virtual bool ParseWhereClause()
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, this.TokenString, 0xbb, SyntaxNodeOptions.Indentation);
            this.AddNode(node);
            this.SyntaxTree.Push(node);
            try
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
            finally
            {
                this.SyntaxTree.Pop();
            }
            node.Range.EndPoint = base.prevPosition;
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
            node.Range.EndPoint = base.prevPosition;
            return flag;
        }

        protected virtual bool ParseYieldStatement()
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, this.TokenString, 0x6c);
            this.AddNode(node);
            this.SyntaxTree.Push(node);
            try
            {
                this.MoveNext();
                CsLexerToken token = (CsLexerToken) this.Token;
                if (token != CsLexerToken.Break)
                {
                    if (token == CsLexerToken.Return)
                    {
                        goto Label_004B;
                    }
                    goto Label_0054;
                }
                flag = this.ParseBreakStatement();
                goto Label_0065;
            Label_004B:
                flag = this.ParseReturnStatement();
                goto Label_0065;
            Label_0054:
                flag = false;
            }
            finally
            {
                this.SyntaxTree.Pop();
            }
        Label_0065:
            node.Range.EndPoint = base.prevPosition;
            return flag;
        }

        public override void ResetAutoIndentChars()
        {
            this.AutoIndentChars = SyntaxParserConsts.DefaultCsAutoIndentChars.ToCharArray();
        }

        public override void ResetCodeCompletionChars()
        {
            this.CodeCompletionChars = SyntaxParserConsts.DefaultCsCodeCompletionChars.ToCharArray();
        }

        public override void ResetCodeCompletionStopChars()
        {
            this.CodeCompletionStopChars = SyntaxParserConsts.DefaultCsCodeCompletionStopChars.ToCharArray();
        }

        public override void ResetSmartFormatChars()
        {
            this.SmartFormatChars = SyntaxParserConsts.DefaultCsSmartFormatChars.ToCharArray();
        }

        public override bool ShouldSerializeAutoIndentChars()
        {
            return (new string(this.AutoIndentChars) != SyntaxParserConsts.DefaultCsAutoIndentChars);
        }

        public override bool ShouldSerializeCodeCompletionChars()
        {
            return (new string(this.CodeCompletionChars) != SyntaxParserConsts.DefaultCsCodeCompletionChars);
        }

        public override bool ShouldSerializeCodeCompletionStopChars()
        {
            return (new string(this.CodeCompletionStopChars) != SyntaxParserConsts.DefaultCsCodeCompletionStopChars);
        }

        public override bool ShouldSerializeSmartFormatChars()
        {
            return (new string(this.SmartFormatChars) != SyntaxParserConsts.DefaultCsSmartFormatChars);
        }

        protected virtual void SkipComment()
        {
            while (this.IsComment(this.Token) || (this.Token == 0xa3))
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
            return this.SkipTo(0x67);
        }

        protected virtual void SyntaxError(int token)
        {
            if (base.Stack.Count == 0)
            {
                ISyntaxError err = new QWhale.Syntax.SyntaxError(this.TokenPosition, this.TokenString, ((CsLexerToken) token).ToString() + " " + StringConsts.ErrExpected);
                if (this.prevPosition.Y != this.TokenPosition.Y)
                {
                    err.Position = base.prevPosition;
                    err.Range.EndPoint = new Point(this.prevPosition.X + 1, this.prevPosition.Y);
                }
                this.SyntaxTree.Current.AddError(err);
            }
        }

        protected virtual bool TryParseAssignmentExpression(ref ISyntaxNode node)
        {
            bool flag = true;
            switch (((CsLexerToken) this.Token))
            {
                case CsLexerToken.Op_mult_assign:
                case CsLexerToken.Op_div_assign:
                case CsLexerToken.Op_mod_assign:
                case CsLexerToken.Op_add_assign:
                case CsLexerToken.Op_sub_assign:
                case CsLexerToken.Op_shift_left_assign:
                case CsLexerToken.Op_shift_right_assign:
                case CsLexerToken.Op_and_assign:
                case CsLexerToken.Op_xor_assign:
                case CsLexerToken.Op_or_assign:
                case CsLexerToken.Assign:
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
                    node.Range.EndPoint = base.prevPosition;
                    return flag;
                }
            }
            return true;
        }

        protected virtual bool TryParseAttributeSpecifier()
        {
            bool flag = true;
            CsLexerToken token = (CsLexerToken) this.Token;
            if (token <= CsLexerToken.Event)
            {
                switch (token)
                {
                    case CsLexerToken.Assembly:
                    case CsLexerToken.Event:
                        goto Label_003A;
                }
                return flag;
            }
            switch (token)
            {
                case CsLexerToken.Return:
                    break;

                case CsLexerToken.Value:
                case CsLexerToken.Identifier_Literal:
                {
                    bool flag2 = false;
                    this.SaveState();
                    try
                    {
                        string str;
                        Point tokenPosition = this.TokenPosition;
                        if (this.ParseIdentifier(out str))
                        {
                            flag2 = this.Token == 0x6f;
                            if (flag2)
                            {
                                this.MoveNext();
                                this.AddAttribute(new SyntaxAttribute(tokenPosition, NetNodeType.AttributeTarget.ToString(), str));
                            }
                        }
                    }
                    finally
                    {
                        this.RestoreState(!flag2);
                    }
                    return flag;
                }
                default:
                    return flag;
            }
        Label_003A:
            this.AddAttribute(new SyntaxAttribute(this.TokenPosition, NetNodeType.AttributeTarget.ToString(), this.TokenString));
            this.MoveNext();
            if (!this.Expected(CsLexerToken.Colon))
            {
                flag = false;
            }
            return flag;
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
                this.MoveNext();
                position = this.TokenPosition;
                if (this.ParseType(out type))
                {
                    flag = this.Token == 0x6c;
                    if (flag)
                    {
                        ISyntaxNode item = node;
                        node = this.CreateExpressionNode(tokenPosition, type, 0x95, node, false);
                        node.AddAttribute(new SyntaxAttribute(position, NetNodeType.Type.ToString(), type));
                        this.MoveNext();
                        ISyntaxNode node3 = null;
                        flag = this.ParseCastTargetExpression(ref node3);
                        if (node3 != null)
                        {
                            node.AddChild(node3);
                        }
                        node.Range.EndPoint = base.prevPosition;
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

        protected virtual bool TryParseGenericExpression(ref ISyntaxNode node, out bool generic)
        {
            bool flag = true;
            generic = false;
            if (node != null)
            {
                NetNodeType nodeType = (NetNodeType) node.NodeType;
                if ((nodeType != NetNodeType.PrimaryExpression) && (nodeType != NetNodeType.MemberAccessExpression))
                {
                    return flag;
                }
            }
            else
            {
                return flag;
            }
            string typeList = string.Empty;
            Point tokenPosition = this.TokenPosition;
            this.SaveState();
            try
            {
                if (this.ParseTypeArgumentList(out typeList))
                {
                    switch (((CsLexerToken) this.Token))
                    {
                        case CsLexerToken.Close_brace:
                        case CsLexerToken.Open_parens:
                        case CsLexerToken.Close_parens:
                        case CsLexerToken.Dot:
                        case CsLexerToken.Colon:
                        case CsLexerToken.Semicolon:
                        case CsLexerToken.Interr:
                        case CsLexerToken.Op_eq:
                        case CsLexerToken.Op_ne:
                            goto Label_0099;
                    }
                }
                goto Label_00AA;
            Label_0099:
                generic = true;
            }
            finally
            {
                this.RestoreState(!generic);
            }
        Label_00AA:
            if (generic)
            {
                node = this.CreateExpressionNode(tokenPosition, typeList, 0xac, node, false);
                node.Range.EndPoint = base.prevPosition;
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
                parsed = this.ParseIdentifier(out str) && this.Expected(CsLexerToken.Colon);
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
                node.Range.EndPoint = base.prevPosition;
            }
            return flag;
        }

        protected virtual bool TryParseLambdaParameterOrExpression(ref ISyntaxNode node, Point position)
        {
            bool flag = true;
            this.SaveState();
            try
            {
                node = new SyntaxNode(this.TokenPosition, string.Empty, 0xb3);
                this.SyntaxTree.Push(node);
                try
                {
                    flag = true;
                    while (!this.Eof && (this.Token != 0x6c))
                    {
                        if (!this.ParseLambdaParameterDeclaration())
                        {
                            flag = false;
                            break;
                        }
                        if (this.Token != 110)
                        {
                            break;
                        }
                        this.MoveNext();
                    }
                    flag = this.Expected(CsLexerToken.Close_parens) && (this.Token == 150);
                }
                finally
                {
                    this.SyntaxTree.Pop();
                }
                base.savePrevPosition = base.prevPosition;
            }
            finally
            {
                this.RestoreState(!flag);
            }
            return flag;
        }

        protected virtual bool TryParsePostPrimaryExpression(ref ISyntaxNode node)
        {
            bool flag = true;
            while (!this.Eof)
            {
                CsLexerToken token = (CsLexerToken) this.Token;
                if (token <= CsLexerToken.DblColon)
                {
                    switch (token)
                    {
                        case CsLexerToken.Open_bracket:
                        {
                            if (!this.ParseElementAccess(ref node))
                            {
                                flag = false;
                            }
                            continue;
                        }
                        case CsLexerToken.Close_bracket:
                        case CsLexerToken.Close_parens:
                            return flag;

                        case CsLexerToken.Open_parens:
                        {
                            if (!this.ParseInvocationExpression(ref node))
                            {
                                flag = false;
                            }
                            continue;
                        }
                        case CsLexerToken.Dot:
                        {
                            if (!this.ParseMemberAccess(ref node))
                            {
                                flag = false;
                            }
                            continue;
                        }
                        case CsLexerToken.DblColon:
                            goto Label_007C;
                    }
                    return flag;
                }
                switch (token)
                {
                    case CsLexerToken.Op_inc:
                    {
                        if (!this.ParsePostIncrementExpression(ref node))
                        {
                            flag = false;
                        }
                        continue;
                    }
                    case CsLexerToken.Op_dec:
                    {
                        if (!this.ParsePostDecrementExpression(ref node))
                        {
                            flag = false;
                        }
                        continue;
                    }
                    case CsLexerToken.Op_ptr:
                    {
                        if (!this.ParsePointerMemberAccess(ref node))
                        {
                            flag = false;
                        }
                        continue;
                    }
                    case CsLexerToken.Op_lt:
                        bool flag2;
                        if (!this.TryParseGenericExpression(ref node, out flag2))
                        {
                            flag = false;
                        }
                        if (flag2)
                        {
                            continue;
                        }
                        return flag;

                    default:
                        return flag;
                }
            Label_007C:
                if (!this.ParseNamespaceAlias(ref node))
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
            while (this.Token == 0x69)
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
            ISyntaxNode node = null;
            this.SaveState();
            try
            {
                parsed = this.ParseVariableDeclaration(this.TokenPosition, NetNodeType.LocalVariable, ref node);
            }
            finally
            {
                this.RestoreState(!parsed);
            }
            flag = parsed;
            if (flag)
            {
                if (!this.Expected(CsLexerToken.Semicolon))
                {
                    flag = false;
                }
                if (node != null)
                {
                    node.Range.EndPoint = base.prevPosition;
                }
            }
            return flag;
        }
    }
}

