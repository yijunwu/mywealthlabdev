namespace QWhale.Syntax.Parsers
{
    using Microsoft.VisualBasic;
    using QWhale.Common;
    using QWhale.Syntax;
    using QWhale.Syntax.CodeCompletion;
    using QWhale.Syntax.Lexer;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Reflection;
    using System.Runtime.InteropServices;

    [ToolboxItem(true), ToolboxBitmap(typeof(VbScriptParser), "Images.VbScriptParser.bmp")]
    public class VbScriptParser : SyntaxParser
    {
        private int expressionCount;
        private IList<int> expressions = new List<int>();
        protected LexerProc lexCommentProc;
        protected LexerProc lexIdentifierProc;
        protected LexerProc lexNumberProc;
        protected LexerProc lexStringProc;
        protected LexerProc lexSymbolProc;
        protected LexerProc lexWhitespaceProc;
        protected Point prevPosition;
        private int prevToken;
        protected Hashtable reswords;
        protected const int reswordStyle = 2;
        protected Point savePrevPosition;
        protected const int stateNormal = 0;

        public VbScriptParser()
        {
            this.Options = SyntaxParserConsts.DefaultVbSyntaxOptions;
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
            return true;
        }

        protected virtual bool BeforeDeclaration(ISyntaxNode node)
        {
            return true;
        }

        protected virtual bool CheckParentNodeWithSameType(ref ISyntaxNode node)
        {
            int nodeType = node.NodeType;
            ISyntaxNode lastParentWithSameType = this.GetLastParentWithSameType(node);
            if (lastParentWithSameType != null)
            {
                this.ReparseBlock(new Point(0, lastParentWithSameType.Position.Y));
                node = this.GetNodeAt(node.Position);
            }
            for (lastParentWithSameType = node.Parent; lastParentWithSameType != null; lastParentWithSameType = lastParentWithSameType.Parent)
            {
                if ((lastParentWithSameType.NodeType == nodeType) && (lastParentWithSameType.FindAttribute(SyntaxConsts.DefinitionScopeEndExpected) != null))
                {
                    node = lastParentWithSameType;
                    return true;
                }
            }
            return false;
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
            return new VbListMembers();
        }

        protected override IParameterInfo CreateParameterInfo()
        {
            return new VbParameterInfo();
        }

        public override ICodeCompletionRepository CreateRepository()
        {
            IReflectionRepository repository = new VbScriptRepository(this.CaseSensitive, this.SyntaxTree);
            this.InitGlobalModules(repository);
            return repository;
        }

        protected virtual bool EndExpression()
        {
            bool flag = true;
            this.expressionCount--;
            if (((this.expressionCount == 0) && (this.expressions.Count > 0)) && (base.Stack.Count == 0))
            {
                string name = VbScriptLexerToken.LineContinuation.ToString();
                foreach (int num in this.expressions)
                {
                    if (num != this.prevPosition.Y)
                    {
                        this.SyntaxError(new Point(this.Strings[num].Length, num), new Point(this.Strings[num].Length, num), name, "_" + ' ' + StringConsts.ErrExpected);
                        flag = false;
                    }
                }
            }
            return flag;
        }

        protected bool Expected(VbScriptLexerToken token)
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
            return this.Expected((VbScriptLexerToken) token);
        }

        protected bool Expected(VbScriptLexerToken token1, VbScriptLexerToken token2)
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

                case ',':
                case '(':
                    return CodeCompletionType.ParameterInfo;

                case '.':
                    return CodeCompletionType.ListMembers;
            }
            return base.GetCompletionType(ch);
        }

        protected virtual ISyntaxNode GetIncompleteNode(ISyntaxNode node)
        {
            while (node != null)
            {
                NetNodeType nodeType = (NetNodeType) node.NodeType;
                if (nodeType <= NetNodeType.ForStatement)
                {
                    switch (nodeType)
                    {
                        case NetNodeType.WhileStatement:
                        case NetNodeType.DoStatement:
                        case NetNodeType.ForStatement:
                        case NetNodeType.IfStatement:
                            goto Label_005A;

                        case NetNodeType.ParameterList:
                            goto Label_0068;
                    }
                    goto Label_00CF;
                }
                if (nodeType <= NetNodeType.BlockStatement)
                {
                    switch (nodeType)
                    {
                        case NetNodeType.ForEachStatement:
                        case NetNodeType.BlockStatement:
                            goto Label_005A;
                    }
                    goto Label_00CF;
                }
                if ((nodeType != NetNodeType.SelectStatement) && (nodeType != NetNodeType.WithStatement))
                {
                    goto Label_00CF;
                }
            Label_005A:
                if (this.IsIncompleteNode(ref node))
                {
                    return node;
                }
                return null;
            Label_0068:
                if (node.Parent != null)
                {
                    foreach (ISyntaxNode node2 in node.Parent.Childs)
                    {
                        if ((node2.NodeType == 0x6a) && (node.Range.EndPoint == node2.Range.StartPoint))
                        {
                            node = node2;
                            if (this.IsIncompleteNode(ref node))
                            {
                                return node;
                            }
                            return null;
                        }
                    }
                }
            Label_00CF:
                node = node.Parent;
            }
            return null;
        }

        private ISyntaxNode GetLastParentWithSameType(ISyntaxNode node)
        {
            ISyntaxNode node2 = node;
            for (ISyntaxNode node3 = node.Parent; node3 != null; node3 = node3.Parent)
            {
                if (node3.NodeType == node.NodeType)
                {
                    node2 = node3;
                }
            }
            return node2;
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
                case 0x6a:
                case 0x6b:
                case 0x6c:
                case 0x6d:
                    return 1;

                case 110:
                case 0x6f:
                    return 7;

                case 0x70:
                    return 0;

                case 0x71:
                    return 3;
            }
            return 6;
        }

        public override string GetSingleLineComment()
        {
            return "'";
        }

        protected virtual bool IdentifierExpected()
        {
            return this.Expected(VbScriptLexerToken.Identifier_Literal);
        }

        protected virtual void InitGlobalModules(IReflectionRepository repository)
        {
            repository.RegisterType("", typeof(VbScriptBuiltInFunctions), true);
            repository.RegisterType("Err", typeof(ErrObject), true);
            repository.RegisterType("Byte", typeof(byte), true);
            repository.RegisterType("Boolean", typeof(bool), true);
            repository.RegisterType("Char", typeof(char), true);
            repository.RegisterType("DateTime", typeof(DateTime), true);
            repository.RegisterType("Date", typeof(DateTime), true);
            repository.RegisterType("Decimal", typeof(decimal), true);
            repository.RegisterType("Double", typeof(double), true);
            repository.RegisterType("Float", typeof(float), true);
            repository.RegisterType("Integer", typeof(int), true);
            repository.RegisterType("Long", typeof(int), true);
            repository.RegisterType("Object", typeof(object), true);
            repository.RegisterType("Short", typeof(short), true);
            repository.RegisterType("Single", typeof(float), true);
            repository.RegisterType("String", typeof(string), true);
        }

        protected override void InitLanguage()
        {
            this.Scheme.FileType = "vbscript";
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
            base.RegisterLexerProc(0, this.lexWhitespaceProc);
            base.RegisterLexerProc(0, '!', '\x00ff', this.lexSymbolProc);
            base.RegisterLexerProc(0, 'a', 'z', this.lexIdentifierProc);
            base.RegisterLexerProc(0, 'A', 'Z', this.lexIdentifierProc);
            base.RegisterLexerProc(0, '_', this.lexIdentifierProc);
            base.RegisterLexerProc(0, '0', '9', this.lexNumberProc);
            base.RegisterLexerProc(0, new char[] { '#', '&' }, this.lexNumberProc);
            base.RegisterLexerProc(0, new char[] { '+', '-' }, this.lexSymbolProc);
            base.RegisterLexerProc(0, '"', this.lexStringProc);
            base.RegisterLexerProc(0, new char[] { '\'', 'r', 'R' }, this.lexCommentProc);
        }

        protected virtual void InitReswords()
        {
            this.reswords = new Hashtable();
            this.reswords.Add("empty", VbScriptLexerToken.Empty);
            this.reswords.Add("false", VbScriptLexerToken.False);
            this.reswords.Add("nothing", VbScriptLexerToken.Nothing);
            this.reswords.Add("null", VbScriptLexerToken.Null);
            this.reswords.Add("true", VbScriptLexerToken.True);
            this.reswords.Add("mod", VbScriptLexerToken.Mod);
            this.reswords.Add("is", VbScriptLexerToken.Is);
            this.reswords.Add("not", VbScriptLexerToken.Not);
            this.reswords.Add("and", VbScriptLexerToken.And);
            this.reswords.Add("or", VbScriptLexerToken.Or);
            this.reswords.Add("xor", VbScriptLexerToken.Xor);
            this.reswords.Add("eqv", VbScriptLexerToken.Eqv);
            this.reswords.Add("imp", VbScriptLexerToken.Imp);
            this.reswords.Add("cbool", VbScriptLexerToken.CBool);
            this.reswords.Add("cbyte", VbScriptLexerToken.CByte);
            this.reswords.Add("cchar", VbScriptLexerToken.CChar);
            this.reswords.Add("cdate", VbScriptLexerToken.CDate);
            this.reswords.Add("cdbl", VbScriptLexerToken.CDbl);
            this.reswords.Add("cdec", VbScriptLexerToken.CDec);
            this.reswords.Add("cint", VbScriptLexerToken.CInt);
            this.reswords.Add("clng", VbScriptLexerToken.CLng);
            this.reswords.Add("cobj", VbScriptLexerToken.CObj);
            this.reswords.Add("cshort", VbScriptLexerToken.CShort);
            this.reswords.Add("csng", VbScriptLexerToken.CSng);
            this.reswords.Add("cstr", VbScriptLexerToken.CStr);
            this.reswords.Add("function", VbScriptLexerToken.Function);
            this.reswords.Add("sub", VbScriptLexerToken.Sub);
            this.reswords.Add("dim", VbScriptLexerToken.Dim);
            this.reswords.Add("const", VbScriptLexerToken.Const);
            this.reswords.Add("redim", VbScriptLexerToken.ReDim);
            this.reswords.Add("call", VbScriptLexerToken.Call);
            this.reswords.Add("preserve", VbScriptLexerToken.Preserve);
            this.reswords.Add("public", VbScriptLexerToken.Public);
            this.reswords.Add("private", VbScriptLexerToken.Private);
            this.reswords.Add("default", VbScriptLexerToken.Default);
            this.reswords.Add("class", VbScriptLexerToken.Class);
            this.reswords.Add("property", VbScriptLexerToken.Property);
            this.reswords.Add("new", VbScriptLexerToken.New);
            this.reswords.Add("do", VbScriptLexerToken.Do);
            this.reswords.Add("erase", VbScriptLexerToken.Erase);
            this.reswords.Add("execute", VbScriptLexerToken.Execute);
            this.reswords.Add("executeglobal", VbScriptLexerToken.ExecuteGlobal);
            this.reswords.Add("exit", VbScriptLexerToken.Exit);
            this.reswords.Add("for", VbScriptLexerToken.For);
            this.reswords.Add("if", VbScriptLexerToken.If);
            this.reswords.Add("on", VbScriptLexerToken.On);
            this.reswords.Add("option", VbScriptLexerToken.Option);
            this.reswords.Add("randomize", VbScriptLexerToken.Randomize);
            this.reswords.Add("select", VbScriptLexerToken.Select);
            this.reswords.Add("set", VbScriptLexerToken.Set);
            this.reswords.Add("stop", VbScriptLexerToken.Stop);
            this.reswords.Add("while", VbScriptLexerToken.While);
            this.reswords.Add("with", VbScriptLexerToken.With);
            this.reswords.Add("then", VbScriptLexerToken.Then);
            this.reswords.Add("else", VbScriptLexerToken.Else);
            this.reswords.Add("elseif", VbScriptLexerToken.ElseIf);
            this.reswords.Add("error", VbScriptLexerToken.Error);
            this.reswords.Add("resume", VbScriptLexerToken.Resume);
            this.reswords.Add("goto", VbScriptLexerToken.GoTo);
            this.reswords.Add("get", VbScriptLexerToken.Get);
            this.reswords.Add("let", VbScriptLexerToken.Let);
            this.reswords.Add("next", VbScriptLexerToken.Next);
            this.reswords.Add("case", VbScriptLexerToken.Case);
            this.reswords.Add("wend", VbScriptLexerToken.Wend);
            this.reswords.Add("loop", VbScriptLexerToken.Loop);
            this.reswords.Add("until", VbScriptLexerToken.Until);
            this.reswords.Add("each", VbScriptLexerToken.Each);
            this.reswords.Add("in", VbScriptLexerToken.In);
            this.reswords.Add("to", VbScriptLexerToken.To);
            this.reswords.Add("step", VbScriptLexerToken.Step);
            this.reswords.Add("byval", VbScriptLexerToken.ByVal);
            this.reswords.Add("byref", VbScriptLexerToken.ByRef);
            this.reswords.Add("end", VbScriptLexerToken.End);
        }

        protected override void InitStyles()
        {
            this.InitDefaultStyles();
        }

        protected virtual bool IsComment(int tok)
        {
            return (tok == 0x71);
        }

        public override bool IsDeclaration(ISyntaxNode node)
        {
            return ((node != null) && NETRepository.IsDeclarationNode(node));
        }

        protected virtual bool IsDeclarationToken(int token)
        {
            switch (((VbScriptLexerToken) token))
            {
                case VbScriptLexerToken.Function:
                case VbScriptLexerToken.Sub:
                case VbScriptLexerToken.Class:
                case VbScriptLexerToken.Property:
                case VbScriptLexerToken.Option:
                    return true;
            }
            return false;
        }

        protected virtual bool IsIdentifierToken(int token)
        {
            return (token == 0x70);
        }

        protected virtual bool IsIncompleteNode(ref ISyntaxNode node)
        {
            if ((node.FindAttribute(SyntaxConsts.DefinitionScopeEndExpected) == null) && !this.CheckParentNodeWithSameType(ref node))
            {
                return false;
            }
            return true;
        }

        protected virtual bool IsInvalidBlockToken(int token)
        {
            if (token != 60)
            {
                return this.IsDeclarationToken(token);
            }
            return true;
        }

        protected virtual bool IsInvalidExpressionToken(int token)
        {
            switch (((VbScriptLexerToken) token))
            {
                case VbScriptLexerToken.Dim:
                case VbScriptLexerToken.Const:
                case VbScriptLexerToken.ReDim:
                case VbScriptLexerToken.Call:
                case VbScriptLexerToken.Do:
                case VbScriptLexerToken.Erase:
                case VbScriptLexerToken.Exit:
                case VbScriptLexerToken.For:
                case VbScriptLexerToken.If:
                case VbScriptLexerToken.On:
                case VbScriptLexerToken.Select:
                case VbScriptLexerToken.Stop:
                case VbScriptLexerToken.While:
                case VbScriptLexerToken.With:
                case VbScriptLexerToken.Then:
                case VbScriptLexerToken.Else:
                case VbScriptLexerToken.ElseIf:
                case VbScriptLexerToken.Error:
                case VbScriptLexerToken.Resume:
                case VbScriptLexerToken.GoTo:
                case VbScriptLexerToken.Next:
                case VbScriptLexerToken.Case:
                case VbScriptLexerToken.Wend:
                case VbScriptLexerToken.Loop:
                case VbScriptLexerToken.End:
                    return true;
            }
            return this.IsInvalidBlockToken(token);
        }

        protected virtual bool IsModifier(int token)
        {
            switch (((VbScriptLexerToken) token))
            {
                case VbScriptLexerToken.Public:
                case VbScriptLexerToken.Private:
                case VbScriptLexerToken.Default:
                    return true;
            }
            return false;
        }

        protected virtual bool IsParameterModifier(int token)
        {
            switch (((VbScriptLexerToken) token))
            {
                case VbScriptLexerToken.ByVal:
                case VbScriptLexerToken.ByRef:
                    return true;
            }
            return false;
        }

        protected virtual bool IsReswordToken(int token)
        {
            return ((token >= 0) && (token <= 60));
        }

        protected virtual bool IsSymbol(int token)
        {
            return ((token >= 0x57) && (token <= 0x68));
        }

        protected override bool IsValidToken(int tok)
        {
            return (((tok != 0x73) && !this.IsComment(tok)) && (tok != 0x69));
        }

        protected virtual int LexComment()
        {
            char ch = base.source[base.currentPos];
            int length = base.source.Length;
            if (ch == '\'')
            {
                base.currentPos = length;
                return 0x71;
            }
            int currentPos = base.currentPos;
            this.LexIdent();
            if (this.TokenString.ToLower() == "rem")
            {
                base.currentPos = length;
                return 0x71;
            }
            base.currentPos = currentPos;
            return this.LexIdentifier();
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
            return 0x6a;
        }

        protected virtual int LexIdentifier()
        {
            if ((base.source[base.currentPos] == '_') && (base.currentPos == (base.source.TrimEnd(new char[0]).Length - 1)))
            {
                base.currentPos++;
                return 0x69;
            }
            this.LexIdent();
            object obj2 = this.reswords[this.TokenString.ToLower()];
            if (obj2 == null)
            {
                return 0x70;
            }
            return (int) obj2;
        }

        protected virtual int LexNumber()
        {
            int currentPos;
            char ch = base.source[base.currentPos];
            int length = base.source.Length;
            int num3 = 0x6a;
            switch (ch)
            {
                case '#':
                    currentPos = base.currentPos;
                    base.currentPos++;
                    while (base.currentPos < length)
                    {
                        if (base.source[base.currentPos] == '#')
                        {
                            base.currentPos++;
                            return 0x6d;
                        }
                        base.currentPos++;
                    }
                    base.currentPos = currentPos;
                    return this.LexSymbol();

                case '+':
                case '-':
                    currentPos = base.currentPos + 1;
                    if (((currentPos >= length) || (base.source[currentPos] < '0')) || (base.source[currentPos] > '9'))
                    {
                        return this.LexSymbol();
                    }
                    break;

                case '0':
                    currentPos = base.currentPos + 1;
                    if ((currentPos < length) && ((base.source[currentPos] == 'x') || (base.source[currentPos] == 'X')))
                    {
                        base.currentPos = currentPos + 1;
                        return this.LexHexNumber();
                    }
                    break;

                case '&':
                    currentPos = base.currentPos + 1;
                    if ((currentPos >= length) || ((base.source[currentPos] != 'h') && (base.source[currentPos] != 'H')))
                    {
                        return this.LexSymbol();
                    }
                    base.currentPos = currentPos + 1;
                    return this.LexHexNumber();
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
                    num3 = 0x6b;
                }
            }
            if (base.currentPos < length)
            {
                ch = base.source[base.currentPos];
                if ((ch != 'E') && (ch != 'e'))
                {
                    return num3;
                }
                currentPos = base.currentPos + 1;
                if (currentPos < length)
                {
                    ch = base.source[currentPos];
                    switch (ch)
                    {
                        case '+':
                        case '-':
                            currentPos++;
                            break;
                    }
                }
                if (currentPos < length)
                {
                    ch = base.source[currentPos];
                    if ((ch >= '0') && (ch <= '9'))
                    {
                        base.currentPos = currentPos;
                        this.LexNum();
                        num3 = 0x6c;
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
                    if ((base.currentPos >= length) || (base.source[base.currentPos] != ch))
                    {
                        break;
                    }
                    base.currentPos++;
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
                return 0x6f;
            }
            return 110;
        }

        protected virtual int LexSymbol()
        {
            VbScriptLexerToken star = VbScriptLexerToken.Whitespace_Literal;
            char ch = base.source[base.currentPos];
            base.currentPos++;
            switch (ch)
            {
                case '&':
                    star = VbScriptLexerToken.Op_and;
                    break;

                case '(':
                    star = VbScriptLexerToken.Open_parens;
                    break;

                case ')':
                    star = VbScriptLexerToken.Close_parens;
                    break;

                case '*':
                    star = VbScriptLexerToken.Star;
                    break;

                case '+':
                    star = VbScriptLexerToken.Plus;
                    break;

                case ',':
                    star = VbScriptLexerToken.Comma;
                    break;

                case '-':
                    star = VbScriptLexerToken.Minus;
                    break;

                case '.':
                    star = VbScriptLexerToken.Dot;
                    break;

                case '/':
                    star = VbScriptLexerToken.Div;
                    break;

                case ':':
                    star = VbScriptLexerToken.Colon;
                    break;

                case '<':
                    switch (base.CurChar())
                    {
                        case '=':
                            base.currentPos++;
                            star = VbScriptLexerToken.Op_le;
                            goto Label_016B;

                        case '>':
                            base.currentPos++;
                            star = VbScriptLexerToken.Op_ne;
                            goto Label_016B;
                    }
                    star = VbScriptLexerToken.Op_lt;
                    break;

                case '=':
                    star = VbScriptLexerToken.Assign;
                    break;

                case '>':
                    if (base.CurChar() != '=')
                    {
                        star = VbScriptLexerToken.Op_gt;
                        break;
                    }
                    base.currentPos++;
                    star = VbScriptLexerToken.Op_ge;
                    break;

                case '\\':
                    star = VbScriptLexerToken.IntDiv;
                    break;

                case '^':
                    star = VbScriptLexerToken.Exponent;
                    break;
            }
        Label_016B:
            return (int) star;
        }

        protected virtual int LexWhitespace()
        {
            this.LexSpace();
            return 0x73;
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

        public override int NextToken()
        {
            int token = this.Token;
            if (this.IsValidToken(token) || (token == 0x69))
            {
                this.prevToken = token;
            }
            return base.NextToken();
        }

        protected virtual bool ParseAdditiveExpression(ref ISyntaxNode node)
        {
            bool flag = this.ParseMultiplicativeExpression(ref node);
            switch (((VbScriptLexerToken) this.Token))
            {
                case VbScriptLexerToken.Plus:
                case VbScriptLexerToken.Minus:
                case VbScriptLexerToken.Op_and:
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
            bool flag = this.ParseImpExpression(ref node);
            if (this.Token == 8)
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
            bool flag2 = false;
            ISyntaxNode node2 = null;
            if (this.IsIdentifierToken(this.Token))
            {
                this.SaveState();
                try
                {
                    string str;
                    if (this.ParseIdentifier(out str) && (this.Token == 0x63))
                    {
                        flag2 = true;
                        node.Name = str;
                        this.MoveNext();
                        flag = this.ParseExpression(ref node2);
                    }
                }
                finally
                {
                    this.RestoreState(!flag2);
                }
            }
            if (!flag2)
            {
                node2 = null;
                flag = this.ParseExpression(ref node2);
                if (!flag)
                {
                    this.prevPosition = this.TokenPosition;
                }
            }
            if (node2 != null)
            {
                node.AddChild(node2);
            }
            node.Range.EndPoint = this.prevPosition;
            return flag;
        }

        protected virtual bool ParseArgumentList(ref ISyntaxNode node, bool useBraces)
        {
            bool flag = true;
            node = new SyntaxNode(this.TokenPosition, this.TokenString, 0x1d);
            if ((this.Token != 0x57) && useBraces)
            {
                this.SyntaxError(0x57);
                return false;
            }
            if (useBraces)
            {
                this.MoveNext();
            }
            while (!this.Eof && (this.Token != 0x58))
            {
                ISyntaxNode node2 = null;
                if (this.Token == 90)
                {
                    node.AddChild(new SyntaxNode(this.TokenPosition, string.Empty, 0x1c));
                    this.MoveNext();
                }
                else
                {
                    if (!this.ParseArgument(ref node2))
                    {
                        flag = false;
                    }
                    if (node2 != null)
                    {
                        node.AddChild(node2);
                    }
                    if (this.Token != 90)
                    {
                        break;
                    }
                    this.MoveNext();
                }
            }
            if (useBraces && !this.Expected(VbScriptLexerToken.Close_parens))
            {
                flag = false;
            }
            node.Range.EndPoint = this.prevPosition;
            return flag;
        }

        protected virtual bool ParseArraySizeInitializationModifier(out string rank)
        {
            bool flag = true;
            rank = this.TokenString;
            if (!this.Expected(VbScriptLexerToken.Open_parens))
            {
                return flag;
            }
        Label_0017:
            rank = rank + this.TokenString;
            if (this.Token != 0x58)
            {
                if (this.Token == 90)
                {
                    this.MoveNext();
                    goto Label_0074;
                }
                ISyntaxNode node = null;
                if ((this.ParseExpression(ref node) && (node != null)) && (node.NodeType == 0x99))
                {
                    rank = rank + node.Name;
                    goto Label_0074;
                }
                flag = false;
            }
            goto Label_0086;
        Label_0074:
            if (!this.Eof && (this.Token != 0x58))
            {
                goto Label_0017;
            }
        Label_0086:
            if (this.Token == 0x58)
            {
                rank = rank + this.TokenString;
            }
            return this.Expected(VbScriptLexerToken.Close_parens);
        }

        protected virtual bool ParseBlock()
        {
            bool blockEnd = false;
            bool flag2 = this.ParseBlock(new int[] { 60, 0x2a, 0x2b, 50, 0x31, 0x34 }, ref blockEnd);
            this.MoveNext();
            return flag2;
        }

        protected virtual bool ParseBlock(int endToken)
        {
            bool blockEnd = false;
            return this.ParseBlock(new int[] { endToken }, ref blockEnd);
        }

        protected virtual bool ParseBlock(int[] endTokens, ref bool blockEnd)
        {
            bool flag = true;
            blockEnd = false;
            this.AddAttribute(new SyntaxAttribute(this.prevPosition, SyntaxConsts.BlockScope, null));
            while (!this.Eof && (Array.IndexOf<int>(endTokens, this.Token) < 0))
            {
                if (this.IsInvalidBlockToken(this.Token))
                {
                    flag = false;
                    this.SyntaxError();
                    break;
                }
                if (!this.ParseStatement(true))
                {
                    flag = false;
                }
                if (!this.ParseStatementTerminator())
                {
                    flag = false;
                }
            }
            if (Array.IndexOf<int>(endTokens, this.Token) < 0)
            {
                blockEnd = false;
                flag = false;
            }
            this.AddAttribute(new SyntaxAttribute(this.CurrentPosition, SyntaxConsts.DefinitionScopeEnd, null));
            return flag;
        }

        protected virtual bool ParseBlockStatement(int token, int endToken)
        {
            bool flag = false;
            ISyntaxNode node = new SyntaxNode(this.prevPosition, string.Empty, 0x6a);
            this.AddNode(node);
            this.SyntaxTree.Push(node);
            try
            {
                flag = this.ParseBlock(token);
                if (this.IsDeclarationToken(this.Token) || !this.ParseBlockStatementEnd(endToken))
                {
                    if (endToken != 0x74)
                    {
                        node.AddAttribute(new SyntaxAttribute(this.prevPosition, SyntaxConsts.DefinitionScopeEndExpected, "\r\n\r\n" + VbScriptLexerToken.End.ToString() + " " + ((VbScriptLexerToken) endToken).ToString()));
                    }
                    flag = false;
                }
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
            attr = node.FindAttribute(SyntaxConsts.DefinitionScopeEndExpected);
            if (attr != null)
            {
                this.AddAttribute(attr);
            }
            return flag;
        }

        protected virtual bool ParseBlockStatementEnd(int endToken)
        {
            if (endToken != 0x74)
            {
                this.MoveNext();
                return this.Expected(endToken);
            }
            return true;
        }

        protected virtual bool ParseCallStatement()
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, this.TokenString, 0x6f);
            this.AddNode(node);
            this.MoveNext();
            ISyntaxNode node2 = null;
            flag = this.ParseExpression(ref node2);
            if (node2 != null)
            {
                node.AddChild(node2);
            }
            if (!this.ParseStatementTerminator())
            {
                flag = false;
            }
            node.Range.EndPoint = this.prevPosition;
            return flag;
        }

        protected virtual bool ParseCaseClause()
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

        protected virtual bool ParseCaseClauses()
        {
            bool flag = true;
        Label_0002:
            if (!this.ParseCaseClause())
            {
                flag = false;
            }
            if (this.Token == 90)
            {
                this.MoveNext();
                if (!this.Eof)
                {
                    goto Label_0002;
                }
            }
            return flag;
        }

        protected virtual bool ParseCaseStatement()
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, this.TokenString, 0x70, SyntaxNodeOptions.Indentation);
            this.AddNode(node);
            node.AddAttribute(new SyntaxAttribute(this.TokenPosition, SyntaxConsts.SuppressReparsingScope, null));
            this.SyntaxTree.Push(node);
            try
            {
                this.MoveNext();
                if (this.Token == 0x2a)
                {
                    this.MoveNext();
                    node.NodeType = 0x71;
                }
                else if (!this.ParseCaseClauses())
                {
                    flag = false;
                }
                if (!this.ParseStatementTerminator())
                {
                    flag = false;
                }
                bool blockEnd = false;
                if (!this.ParseBlock(new int[] { 50, 60 }, ref blockEnd))
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

        protected virtual bool ParseCastTargetExpression(ref ISyntaxNode node, VbScriptLexerToken token)
        {
            bool flag = true;
            node = new SyntaxNode(this.TokenPosition, this.TokenString, 0x97);
            node.AddAttribute(new SyntaxAttribute(this.TokenPosition, NetNodeType.Type.ToString(), token.ToString()));
            this.MoveNext();
            if (this.Expected(VbScriptLexerToken.Open_parens))
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
                if (!this.Expected(VbScriptLexerToken.Close_parens))
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

        protected virtual bool ParseClassBody()
        {
            bool flag = true;
            ISyntaxNode current = this.SyntaxTree.Current;
            current.Options |= SyntaxNodeOptions.Outlining | SyntaxNodeOptions.Indentation;
            if (!this.ParseBlockStatement(60, 0x17))
            {
                flag = false;
            }
            current.AddAttribute(new SyntaxAttribute(this.prevPosition, SyntaxConsts.DefinitionScopeEnd, null));
            current.Range.EndPoint = this.prevPosition;
            return flag;
        }

        protected virtual bool ParseClassDeclaration(ISyntaxAttributes attrs)
        {
            string str;
            bool flag = true;
            Point position = ((attrs != null) && (attrs.Count > 0)) ? attrs[0].Position : this.TokenPosition;
            ISyntaxNode node = new SyntaxNode(position, this.TokenString, 8);
            if (attrs != null)
            {
                node.AddAttributes(attrs);
            }
            this.MoveNext();
            Point tokenPosition = this.TokenPosition;
            if (this.ParseIdentifier(out str))
            {
                node.Name = str;
                node.AddAttribute(new SyntaxAttribute(tokenPosition, NetNodeType.Name.ToString(), str));
                node.Options |= SyntaxNodeOptions.CodeCompletion;
                this.AddNode(node);
                if (!this.BeforeDeclaration(node))
                {
                    flag = false;
                }
                position = this.prevPosition;
                position = (this.prevPosition.Y == position.Y) ? this.prevPosition : position;
                node.AddAttribute(new SyntaxAttribute(position, SyntaxConsts.DeclarationScope, null));
                this.SyntaxTree.Push(node);
                try
                {
                    node.Options |= SyntaxNodeOptions.Outlining | SyntaxNodeOptions.Indentation;
                    if (!this.ParseClassBody())
                    {
                        flag = false;
                    }
                }
                finally
                {
                    this.SyntaxTree.Pop();
                }
            }
            if (!this.AfterDeclaration(node))
            {
                flag = false;
            }
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

        protected virtual bool ParseDeclarationEnd(int token)
        {
            return this.Expected(token);
        }

        protected virtual bool ParseDefaultBlock()
        {
            bool flag = true;
            while (!this.Eof)
            {
                bool flag2 = false;
                switch (this.Token)
                {
                    case 20:
                    case 0x15:
                        this.SaveState();
                        try
                        {
                            flag2 = this.ParseVariableDeclaration(null, this.TokenPosition, 13);
                        }
                        finally
                        {
                            this.RestoreState(!flag2);
                        }
                        break;
                }
                if (!flag2)
                {
                    ISyntaxAttributes attrs = null;
                    this.ParseModifiers(ref attrs);
                    switch (this.Token)
                    {
                        case 13:
                        case 14:
                        {
                            if (!this.ParseMethodDeclaration(attrs))
                            {
                                flag = false;
                            }
                            continue;
                        }
                        case 15:
                        case 0x11:
                        {
                            if (!this.ParseVariableDeclaration(attrs, this.TokenPosition, 13))
                            {
                                flag = false;
                            }
                            continue;
                        }
                        case 0x10:
                        {
                            if (!this.ParseVariableDeclaration(attrs, this.TokenPosition, 14))
                            {
                                flag = false;
                            }
                            continue;
                        }
                        case 0x17:
                        {
                            if (!this.ParseClassDeclaration(attrs))
                            {
                                flag = false;
                            }
                            continue;
                        }
                    }
                    if (!this.ParseStatementList())
                    {
                        flag = false;
                    }
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
                bool parsed = false;
                if (!this.TryParseWhileOrUntil(out parsed))
                {
                    flag = false;
                }
                bool blockEnd = flag;
                if (!this.ParseStatementTerminator())
                {
                    blockEnd = false;
                    flag = false;
                }
                if (!this.ParseBlock(new int[] { 0x34 }, ref blockEnd))
                {
                    if (!blockEnd)
                    {
                        node.AddAttribute(new SyntaxAttribute(this.prevPosition, SyntaxConsts.DefinitionScopeEndExpected, "\r\n\r\n" + VbScriptLexerToken.Loop.ToString()));
                    }
                    flag = false;
                }
                this.MoveNext();
                if (!this.TryParseWhileOrUntil(out parsed))
                {
                    flag = false;
                }
                if (!this.ParseStatementTerminator())
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

        protected virtual bool ParseElseIfStatement()
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, this.TokenString, 0x72, SyntaxNodeOptions.BackIndentation);
            node.AddAttribute(new SyntaxAttribute(this.TokenPosition, SyntaxConsts.SuppressReparsingScope, null));
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
                if (this.Token == 0x29)
                {
                    this.MoveNext();
                }
                if (!this.ParseStatementTerminator())
                {
                    flag = false;
                }
                bool blockEnd = false;
                if (!this.ParseBlock(new int[] { 0x2a, 0x2b, 60 }, ref blockEnd))
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

        protected virtual bool ParseElseStatement(bool block)
        {
            bool flag = true;
            this.SaveState();
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, this.TokenString, 0x43, SyntaxNodeOptions.BackIndentation);
            this.AddNode(node);
            this.SyntaxTree.Push(node);
            try
            {
                this.MoveNext();
                if (block)
                {
                    if (!this.ParseStatementTerminator())
                    {
                        flag = false;
                    }
                    if (!this.ParseBlock(60))
                    {
                        flag = false;
                    }
                }
                else if (!this.ParseStatement(true))
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
            return this.ParseEmbeddedStatement(null);
        }

        protected virtual bool ParseEmbeddedStatement(ISyntaxAttributes attrs)
        {
            switch (this.Token)
            {
                case 13:
                case 14:
                    return this.ParseMethodDeclaration(attrs);

                case 15:
                case 0x10:
                case 0x11:
                    return this.ParseVariableDeclaration(attrs, this.TokenPosition, 13);

                case 0x12:
                    return this.ParseCallStatement();

                case 0x17:
                    return this.ParseClassDeclaration(attrs);

                case 0x18:
                    return this.ParsePropertyStatement();

                case 0x1a:
                    return this.ParseDoStatement();

                case 0x1b:
                    return this.ParseEraseStatement();

                case 0x1c:
                case 0x1d:
                    return this.ParseExecuteStatement(this.Token == 0x1d);

                case 30:
                    return this.ParseExitStatement();

                case 0x1f:
                    return this.ParseForStatement();

                case 0x20:
                    return this.ParseIfStatement();

                case 0x21:
                    return this.ParseOnErrorStatement();

                case 0x22:
                    return this.ParseOptionStatement();

                case 0x23:
                    return this.ParseRandomizeStatement();

                case 0x24:
                    return this.ParseSelectStatement();

                case 0x25:
                    return this.ParseSetStatement();

                case 0x26:
                    return this.ParseStopStatement();

                case 0x27:
                    return this.ParseWhileStatement();

                case 40:
                    return this.ParseWithStatement();
            }
            return this.ParseExpressionStatement();
        }

        protected virtual bool ParseEqualityExpression(ref ISyntaxNode node)
        {
            bool flag = this.ParseRelationalExpression(ref node);
            VbScriptLexerToken token = (VbScriptLexerToken) this.Token;
            if (token <= VbScriptLexerToken.Eqv)
            {
                switch (token)
                {
                    case VbScriptLexerToken.Is:
                    {
                        node = this.CreateExpressionNode(this.TokenPosition, this.TokenString, 0x8f, node, true);
                        this.MoveNext();
                        ISyntaxNode node3 = null;
                        if (!this.ParseEqualityExpression(ref node3))
                        {
                            flag = false;
                        }
                        if (node3 != null)
                        {
                            node.AddChild(node3);
                        }
                        node.Range.EndPoint = this.prevPosition;
                        return flag;
                    }
                    case VbScriptLexerToken.Eqv:
                        goto Label_002F;
                }
                return flag;
            }
            if ((token != VbScriptLexerToken.Op_ne) && (token != VbScriptLexerToken.Assign))
            {
                return flag;
            }
        Label_002F:
            node = this.CreateExpressionNode(this.TokenPosition, this.TokenString, 0xc4, node, true);
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

        protected virtual bool ParseEraseStatement()
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, this.TokenString, 0x74, SyntaxNodeOptions.Indentation);
            this.AddNode(node);
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
            if (!this.ParseStatementTerminator())
            {
                flag = false;
            }
            node.Range.EndPoint = this.prevPosition;
            return flag;
        }

        protected virtual bool ParseExclusiveOrExpression(ref ISyntaxNode node)
        {
            bool flag = this.ParseAndExpression(ref node);
            if (this.Token == 10)
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

        protected virtual bool ParseExecuteStatement(bool isGlobal)
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, this.TokenString, isGlobal ? 0xcf : 0xce, SyntaxNodeOptions.Indentation);
            this.AddNode(node);
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
            if (!this.ParseStatementTerminator())
            {
                flag = false;
            }
            node.Range.EndPoint = this.prevPosition;
            return flag;
        }

        protected virtual bool ParseExitStatement()
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, this.TokenString, 0x76, SyntaxNodeOptions.Indentation);
            this.AddNode(node);
            this.MoveNext();
            switch (((VbScriptLexerToken) this.Token))
            {
                case VbScriptLexerToken.Function:
                case VbScriptLexerToken.Sub:
                case VbScriptLexerToken.Property:
                case VbScriptLexerToken.Do:
                case VbScriptLexerToken.For:
                    this.MoveNext();
                    break;

                default:
                    flag = false;
                    this.SyntaxError();
                    break;
            }
            if (!this.ParseStatementTerminator())
            {
                flag = false;
            }
            node.Range.EndPoint = this.prevPosition;
            return flag;
        }

        protected virtual bool ParseExpression(ref ISyntaxNode node)
        {
            bool flag = true;
            this.StartExpression();
            try
            {
                int lineIndex = base.lineIndex;
                flag = this.ParseInclusiveOrExpression(ref node);
                if (!this.TryParseAssignmentExpression(ref node, lineIndex))
                {
                    flag = false;
                }
            }
            finally
            {
                if (!this.EndExpression())
                {
                    flag = false;
                }
            }
            return flag;
        }

        protected virtual bool ParseExpressionBlockStatement(int endToken, int nodeType)
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, this.TokenString, nodeType, SyntaxNodeOptions.Outlining | SyntaxNodeOptions.Indentation);
            node.AddAttribute(new SyntaxAttribute(this.TokenPosition, SyntaxConsts.SuppressReparsingScope, null));
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
                bool flag2 = flag;
                if (!this.ParseStatementTerminator())
                {
                    flag2 = false;
                    flag = false;
                }
                flag = this.ParseBlock(60);
                this.MoveNext();
                if (!this.Expected(endToken) || !this.ParseStatementTerminator())
                {
                    if (flag2 && (endToken != 0x74))
                    {
                        node.AddAttribute(new SyntaxAttribute(this.prevPosition, SyntaxConsts.DefinitionScopeEndExpected, "\r\n\r\n" + VbScriptLexerToken.End.ToString() + " " + ((VbScriptLexerToken) endToken).ToString()));
                    }
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

        protected virtual bool ParseExpressionStatement()
        {
            if (this.IsInvalidExpressionToken(this.Token))
            {
                this.SyntaxError();
                this.MoveNext();
                return false;
            }
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, string.Empty, 0x6b, SyntaxNodeOptions.Indentation);
            this.AddNode(node);
            this.SyntaxTree.Push(node);
            try
            {
                flag = this.ParseStatementExpression();
                if (!this.ParseStatementTerminator())
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

        protected virtual bool ParseFieldDeclaration(ISyntaxAttributes attrs, Point pos, string name, ISyntaxNodes names, int nodeType)
        {
            bool flag = true;
            pos = ((attrs != null) && (attrs.Count > 0)) ? attrs[0].Position : pos;
            ISyntaxNode node = new SyntaxNode(pos, name, nodeType, SyntaxNodeOptions.CodeCompletion | SyntaxNodeOptions.Indentation);
            if (names != null)
            {
                node.AddChildren(names);
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
            if (this.Token == 0x63)
            {
                ISyntaxNode node2 = new SyntaxNode(this.TokenPosition, this.TokenString, 0x21);
                node.AddChild(node2);
                this.MoveNext();
                ISyntaxNode node3 = null;
                if (!this.ParseVariableInitializer(ref node3))
                {
                    flag = false;
                }
                if (node3 != null)
                {
                    Point point;
                    node2.AddChild(node3);
                    string expressionType = VbScriptRepository.GetExpressionType(node3, out point);
                    if (expressionType != string.Empty)
                    {
                        node.AddAttribute(new SyntaxAttribute(point, NetNodeType.Type.ToString(), expressionType));
                    }
                }
                node2.Range.EndPoint = this.prevPosition;
            }
            if (!this.AfterDeclaration(node))
            {
                flag = false;
            }
            node.Range.EndPoint = this.prevPosition;
            return flag;
        }

        protected virtual bool ParseForEachStatement(ISyntaxNode node)
        {
            bool flag = true;
            node.NodeType = 0x4e;
            this.MoveNext();
            if (!this.ParseLoopControlVariable())
            {
                flag = false;
            }
            if (this.Token == 0x37)
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
            bool flag2 = flag;
            if (!this.ParseStatementTerminator())
            {
                flag2 = false;
                flag = false;
            }
            bool blockEnd = false;
            if (!this.ParseBlock(new int[] { 0x31, 0x74 }, ref blockEnd))
            {
                flag = false;
                if (flag2)
                {
                    node.AddAttribute(new SyntaxAttribute(this.prevPosition, SyntaxConsts.DefinitionScopeEndExpected, "\r\n\r\n" + VbScriptLexerToken.Next.ToString()));
                    this.prevPosition = this.TokenPosition;
                }
            }
            else
            {
                int lineIndex = base.lineIndex;
                this.MoveNext();
                if (!this.ParseStatementTerminator(lineIndex))
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
            }
            if (!this.ParseStatementTerminator())
            {
                flag = false;
            }
            node.Range.EndPoint = this.prevPosition;
            return flag;
        }

        protected virtual bool ParseForIterator()
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, this.TokenString, 0x4d);
            this.AddNode(node);
            ISyntaxNode node2 = null;
            flag = this.Expected(VbScriptLexerToken.To) && this.ParseExpression(ref node2);
            if (node2 != null)
            {
                node.AddChild(node2);
            }
            if (this.Token == 0x39)
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
                if (this.Token == 0x36)
                {
                    flag = this.ParseForEachStatement(node);
                }
                else
                {
                    if (!this.ParseLoopControlVariable())
                    {
                        flag = false;
                    }
                    ISyntaxNode node2 = null;
                    if (!this.TryParseAssignmentExpression(ref node2, base.lineIndex))
                    {
                        flag = false;
                    }
                    if (node2 != null)
                    {
                        node.AddChild(node2);
                    }
                    if (!this.ParseForIterator())
                    {
                        flag = false;
                    }
                    bool flag2 = flag;
                    if (!this.ParseStatementTerminator())
                    {
                        flag2 = false;
                        flag = false;
                    }
                    bool blockEnd = false;
                    if (!this.ParseBlock(new int[] { 0x31, 0x74 }, ref blockEnd))
                    {
                        flag = false;
                        if (flag2)
                        {
                            node.AddAttribute(new SyntaxAttribute(this.prevPosition, SyntaxConsts.DefinitionScopeEndExpected, "\r\n\r\n" + VbScriptLexerToken.Next.ToString()));
                            this.prevPosition = this.TokenPosition;
                        }
                    }
                    else
                    {
                        this.MoveNext();
                    }
                    if (!this.ParseStatementTerminator())
                    {
                        flag = false;
                    }
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
            string str;
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, this.TokenString, 0x5d, SyntaxNodeOptions.Indentation);
            this.AddNode(node);
            this.MoveNext();
            if (this.ParseLabelName(out str))
            {
                node.Name = str;
            }
            else
            {
                flag = false;
            }
            if (!this.ParseStatementTerminator())
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
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, this.TokenString, 0x42, SyntaxNodeOptions.Indentation);
            this.AddNode(node);
            node.AddAttribute(new SyntaxAttribute(this.TokenPosition, SyntaxConsts.SuppressReparsingScope, null));
            bool block = false;
            int lineIndex = base.lineIndex;
            this.SyntaxTree.Push(node);
            try
            {
                this.MoveNext();
                ISyntaxNode node2 = null;
                ISyntaxAttribute attr = null;
                if (!this.ParseExpression(ref node2))
                {
                    flag = false;
                    block = true;
                }
                if (node2 != null)
                {
                    node.AddChild(node2);
                }
                bool flag3 = flag;
                if (this.Token == 0x29)
                {
                    lineIndex = base.lineIndex;
                    this.MoveNext();
                    block = this.ParseStatementTerminator(lineIndex);
                }
                else
                {
                    block = this.ParseStatementTerminator(lineIndex) | block;
                    flag3 = flag && (base.lineIndex != lineIndex);
                    if (flag3)
                    {
                        attr = new SyntaxAttribute(this.prevPosition, SyntaxConsts.DefinitionScopeEndExpected, ' ' + VbScriptLexerToken.Then.ToString());
                        node.AddAttribute(attr);
                    }
                }
                if (block)
                {
                    bool blockEnd = false;
                    if (!this.ParseBlock(new int[] { 0x2a, 0x2b, 60 }, ref blockEnd))
                    {
                        flag = false;
                    }
                }
                else if (!this.ParseStatement(true))
                {
                    flag = false;
                }
                if (block)
                {
                    while (!this.Eof && ((this.Token == 0x2b) || (this.Token == 0x2a)))
                    {
                        bool flag5 = this.Token == 0x2b;
                        if (!flag5)
                        {
                            this.SaveState();
                            try
                            {
                                lineIndex = base.lineIndex;
                                flag5 = (this.MoveNext() == 0x20) && (base.lineIndex == lineIndex);
                            }
                            finally
                            {
                                this.RestoreState(!flag5);
                            }
                        }
                        if (flag5)
                        {
                            if (!this.ParseElseIfStatement())
                            {
                                flag = false;
                            }
                        }
                        else
                        {
                            if (!this.ParseElseStatement(block))
                            {
                                flag = false;
                            }
                            break;
                        }
                    }
                }
                if (block)
                {
                    if (!this.Expected(VbScriptLexerToken.End) || !this.Expected(VbScriptLexerToken.If))
                    {
                        if (flag3)
                        {
                            string str = "\r\n\r\n" + VbScriptLexerToken.End.ToString() + " " + VbScriptLexerToken.If.ToString();
                            if (attr != null)
                            {
                                attr.Value = attr.Value.ToString() + str;
                            }
                            else
                            {
                                node.AddAttribute(new SyntaxAttribute(this.prevPosition, SyntaxConsts.DefinitionScopeEndExpected, str));
                            }
                        }
                        flag = false;
                    }
                    ISyntaxAttribute attribute2 = node.FindAttribute(SyntaxConsts.DefinitionScopeEnd);
                    if (attribute2 != null)
                    {
                        attribute2.Position = this.prevPosition;
                    }
                }
                else if ((this.Token == 0x2a) && !this.ParseElseStatement(block))
                {
                    flag = false;
                }
                if (!this.ParseStatementTerminator())
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

        protected virtual bool ParseImpExpression(ref ISyntaxNode node)
        {
            bool flag = this.ParseEqualityExpression(ref node);
            if (this.Token == 12)
            {
                node = this.CreateExpressionNode(this.TokenPosition, this.TokenString, 210, node, true);
                this.MoveNext();
                ISyntaxNode node2 = null;
                if (!this.ParseImpExpression(ref node2))
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

        protected virtual bool ParseInclusiveOrExpression(ref ISyntaxNode node)
        {
            bool flag = this.ParseExclusiveOrExpression(ref node);
            if (this.Token == 9)
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

        protected virtual bool ParseInvocationExpression(ref ISyntaxNode node, bool useBraces)
        {
            bool flag = true;
            node = this.CreateExpressionNode(this.TokenPosition, (this.Token == 0x57) ? this.TokenString : NetNodeType.InvocationExpression.ToString(), 0xa3, node, false);
            ISyntaxNode node2 = null;
            flag = this.ParseArgumentList(ref node2, useBraces);
            if (node2 != null)
            {
                node.AddChild(node2);
            }
            node.Range.EndPoint = this.prevPosition;
            return flag;
        }

        protected virtual bool ParseLabelName(out string name)
        {
            name = string.Empty;
            switch (((VbScriptLexerToken) this.Token))
            {
                case VbScriptLexerToken.Integer_Literal:
                case VbScriptLexerToken.Identifier_Literal:
                    name = this.TokenString;
                    this.MoveNext();
                    return true;
            }
            this.SyntaxError(0x70);
            return false;
        }

        protected virtual bool ParseLoopControlVariable()
        {
            string str;
            bool flag = true;
            Point tokenPosition = this.TokenPosition;
            if (this.ParseIdentifier(out str))
            {
                ISyntaxNode node = new SyntaxNode(tokenPosition, str, 15, SyntaxNodeOptions.CodeCompletion);
                this.AddNode(node);
                return flag;
            }
            return false;
        }

        protected virtual bool ParseMemberAccess(ref ISyntaxNode node)
        {
            return this.ParseMemberAccess(ref node, 0x9f);
        }

        protected virtual bool ParseMemberAccess(ref ISyntaxNode node, int nodeType)
        {
            bool flag = true;
            node = this.CreateExpressionNode(this.TokenPosition, this.TokenString, nodeType, node, true);
            Point currentPosition = this.CurrentPosition;
            this.MoveNext();
            if (this.IsReswordToken(this.Token) && (currentPosition.Y == this.TokenPosition.Y))
            {
                node.Name = this.TokenString;
                this.MoveNext();
            }
            else
            {
                string str;
                if (this.ParseIdentifier(out str))
                {
                    node.Name = str;
                }
                else
                {
                    flag = false;
                }
            }
            node.AddAttribute(new SyntaxAttribute(currentPosition, NetNodeType.Name.ToString(), node.Name));
            node.Range.EndPoint = this.prevPosition;
            return flag;
        }

        protected virtual bool ParseMethodBody(int endToken)
        {
            bool flag = true;
            ISyntaxNode current = this.SyntaxTree.Current;
            current.Options |= SyntaxNodeOptions.Outlining | SyntaxNodeOptions.Indentation;
            if (!this.ParseBlockStatement(60, endToken))
            {
                flag = false;
            }
            current.Range.EndPoint = this.prevPosition;
            return flag;
        }

        protected virtual bool ParseMethodDeclaration(ISyntaxAttributes attrs)
        {
            string str;
            Point tokenPosition = this.TokenPosition;
            bool func = this.Token == 13;
            this.MoveNext();
            ISyntaxNode node = new SyntaxNode(((attrs != null) && (attrs.Count > 0)) ? attrs[0].Position : tokenPosition, string.Empty, 0x11, SyntaxNodeOptions.CodeCompletion);
            if (this.ParseIdentifier(out str))
            {
                node.Name = str;
                this.AddNode(node);
                return this.ParseMethodDeclaration(node, func);
            }
            return false;
        }

        protected virtual bool ParseMethodDeclaration(ISyntaxNode node, bool func)
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
                node.AddAttribute(new SyntaxAttribute(this.prevPosition, SyntaxConsts.DeclarationScope, null));
                if (!this.ParseMethodBody(func ? 13 : 14))
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
            switch (((VbScriptLexerToken) this.Token))
            {
                case VbScriptLexerToken.Star:
                case VbScriptLexerToken.Div:
                case VbScriptLexerToken.IntDiv:
                case VbScriptLexerToken.Exponent:
                case VbScriptLexerToken.Mod:
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

        protected virtual bool ParseNewExpression(ref ISyntaxNode node)
        {
            string str;
            node = new SyntaxNode(this.TokenPosition, this.TokenString, 0x9b);
            this.MoveNext();
            Point tokenPosition = this.TokenPosition;
            bool flag = this.ParseTypeName(out str);
            if (str != string.Empty)
            {
                node.AddAttribute(new SyntaxAttribute(tokenPosition, NetNodeType.Type.ToString(), str));
            }
            else
            {
                flag = false;
            }
            node.Range.EndPoint = this.prevPosition;
            return flag;
        }

        protected virtual bool ParseOnErrorStatement()
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, this.TokenString, 120, SyntaxNodeOptions.Indentation);
            this.AddNode(node);
            this.SyntaxTree.Push(node);
            try
            {
                this.MoveNext();
                if (this.Expected(VbScriptLexerToken.Error))
                {
                    switch (this.Token)
                    {
                        case 0x2d:
                            this.MoveNext();
                            if (!this.Expected(VbScriptLexerToken.Next))
                            {
                                flag = false;
                            }
                            break;

                        case 0x2e:
                            if (!this.ParseGotoStatement())
                            {
                                flag = false;
                            }
                            break;

                        default:
                            flag = false;
                            this.SyntaxError(0x2d);
                            break;
                    }
                    if (!this.ParseStatementTerminator())
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
            node.Range.EndPoint = this.prevPosition;
            return flag;
        }

        protected virtual bool ParseOptionStatement()
        {
            string str;
            bool flag = true;
            Point tokenPosition = this.TokenPosition;
            this.MoveNext();
            if (((str = this.TokenString.ToLower()) != null) && (str == "explicit"))
            {
                ISyntaxNode node = new SyntaxNode(tokenPosition, this.TokenString, 0xc3);
                this.AddNode(node);
                node.AddAttribute(new SyntaxAttribute(this.TokenPosition, NetNodeType.Attribute.ToString(), this.TokenString));
                this.MoveNext();
                node.Range.EndPoint = this.prevPosition;
                return flag;
            }
            flag = false;
            this.SyntaxError();
            return flag;
        }

        protected virtual bool ParseParameterDeclaration()
        {
            string str;
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, string.Empty, 0x1a, SyntaxNodeOptions.CodeCompletion);
            while (this.IsParameterModifier(this.Token))
            {
                node.AddAttribute(new SyntaxAttribute(this.TokenPosition, NetNodeType.ParameterModifier.ToString(), this.TokenString));
                this.MoveNext();
            }
            if (this.ParseVariableIdentifier(out str))
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
            return this.ParseParameterListDeclaration(0x57, 0x58);
        }

        protected virtual bool ParseParameterListDeclaration(int startToken, int endToken)
        {
            bool flag = true;
            if (this.Token != startToken)
            {
                return flag;
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
                    if (this.Token != 90)
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
            if (this.Token == 0x57)
            {
                this.MoveNext();
                ISyntaxNode node2 = null;
                flag = this.ParseExpression(ref node2);
                if (node2 != null)
                {
                    node.AddChild(node2);
                }
                if (!this.Expected(VbScriptLexerToken.Close_parens))
                {
                    flag = false;
                }
            }
            else
            {
                this.SyntaxError(0x57);
                flag = false;
            }
            node.Range.EndPoint = this.prevPosition;
            return flag;
        }

        protected virtual bool ParsePrefixedUnaryExpression(ref ISyntaxNode node)
        {
            bool flag = true;
            switch (((VbScriptLexerToken) this.Token))
            {
                case VbScriptLexerToken.Plus:
                case VbScriptLexerToken.Minus:
                case VbScriptLexerToken.Not:
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
            return this.ParsePrimaryExpression(ref node);
        }

        protected virtual bool ParsePrimaryExpression(ref ISyntaxNode node)
        {
            bool flag = true;
            bool post = false;
            flag = this.ParseSimpleExpression(ref node, ref post);
            if (((node != null) && post) && !this.TryParsePostPrimaryExpression(ref node))
            {
                flag = false;
            }
            return flag;
        }

        protected virtual bool ParsePropertyBody()
        {
            bool flag = true;
            ISyntaxNode current = this.SyntaxTree.Current;
            current.Options |= SyntaxNodeOptions.Outlining | SyntaxNodeOptions.Indentation;
            if (!this.ParseBlockStatement(60, 0x18))
            {
                flag = false;
            }
            current.Range.EndPoint = this.prevPosition;
            return flag;
        }

        protected virtual bool ParsePropertyDeclaration(ISyntaxNode node)
        {
            bool flag = true;
            if (!this.BeforeDeclaration(node))
            {
                flag = false;
            }
            this.SyntaxTree.Push(node);
            try
            {
                if ((this.Token == 0x57) && !this.ParseParameterListDeclaration())
                {
                    flag = false;
                }
                node.AddAttribute(new SyntaxAttribute(this.prevPosition, SyntaxConsts.DeclarationScope, null));
                if (!this.ParsePropertyBody())
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

        protected virtual bool ParsePropertyStatement()
        {
            bool flag = true;
            Point tokenPosition = this.TokenPosition;
            this.MoveNext();
            ISyntaxNode node = new SyntaxNode(tokenPosition, string.Empty, 0x15, SyntaxNodeOptions.CodeCompletion);
            this.AddNode(node);
            switch (((VbScriptLexerToken) this.Token))
            {
                case VbScriptLexerToken.Get:
                case VbScriptLexerToken.Let:
                case VbScriptLexerToken.Set:
                    string str;
                    node.AddAttribute(new SyntaxAttribute(this.TokenPosition, NetNodeType.Attribute.ToString(), this.TokenString));
                    this.MoveNext();
                    if (this.ParseIdentifier(out str))
                    {
                        node.Name = str;
                        return this.ParsePropertyDeclaration(node);
                    }
                    return false;
            }
            flag = false;
            this.SyntaxError();
            return flag;
        }

        protected virtual bool ParseQualifiedIdentifier(out string identifier)
        {
            identifier = this.TokenString;
            if (!this.ParseIdentifier(out identifier))
            {
                identifier = string.Empty;
                return false;
            }
            while (this.Token == 0x59)
            {
                identifier = identifier + this.TokenString;
                this.MoveNext();
                string tokenString = this.TokenString;
                if (this.IsReswordToken(this.Token))
                {
                    this.MoveNext();
                    identifier = identifier + tokenString;
                }
                else if (this.ParseIdentifier(out tokenString))
                {
                    identifier = identifier + tokenString;
                    continue;
                }
            }
            return true;
        }

        protected virtual bool ParseRandomizeStatement()
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, this.TokenString, 0xd0, SyntaxNodeOptions.Indentation);
            this.AddNode(node);
            int lineIndex = base.lineIndex;
            this.MoveNext();
            if (!this.ParseStatementTerminator(lineIndex))
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
            if (!this.ParseStatementTerminator())
            {
                flag = false;
            }
            node.Range.EndPoint = this.prevPosition;
            return flag;
        }

        protected virtual bool ParseRelationalExpression(ref ISyntaxNode node)
        {
            bool flag = this.ParseAdditiveExpression(ref node);
            switch (this.Token)
            {
                case 0x5f:
                case 0x60:
                case 0x61:
                case 0x62:
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

        protected virtual bool ParseSelectStatement()
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, this.TokenString, 0x7d, SyntaxNodeOptions.Indentation);
            this.AddNode(node);
            this.SyntaxTree.Push(node);
            try
            {
                this.MoveNext();
                if (this.Token == 50)
                {
                    this.MoveNext();
                }
                ISyntaxNode node2 = null;
                if (!this.ParseExpression(ref node2))
                {
                    flag = false;
                }
                if (node2 != null)
                {
                    node.AddChild(node2);
                }
                bool flag2 = flag;
                if (!this.ParseStatementTerminator())
                {
                    flag2 = false;
                    flag = false;
                }
                node.AddAttribute(new SyntaxAttribute(this.prevPosition, SyntaxConsts.DefinitionScope, null));
                while (!this.Eof && (this.Token == 50))
                {
                    if (!this.ParseCaseStatement())
                    {
                        flag = false;
                    }
                }
                if (!this.Expected(VbScriptLexerToken.End) || !this.Expected(VbScriptLexerToken.Select))
                {
                    if (flag2)
                    {
                        node.AddAttribute(new SyntaxAttribute(this.prevPosition, SyntaxConsts.DefinitionScopeEndExpected, "\r\n\r\n" + VbScriptLexerToken.End.ToString() + " " + VbScriptLexerToken.Select.ToString()));
                    }
                    flag = false;
                }
                node.AddAttribute(new SyntaxAttribute(this.prevPosition, SyntaxConsts.DefinitionScopeEnd, null));
                if (!this.ParseStatementTerminator())
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

        protected virtual bool ParseSetStatement()
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, this.TokenString, 0xd1, SyntaxNodeOptions.Indentation);
            this.AddNode(node);
            this.SyntaxTree.Push(node);
            try
            {
                this.MoveNext();
                string identifier = string.Empty;
                Point tokenPosition = this.TokenPosition;
                if (this.ParseQualifiedIdentifier(out identifier))
                {
                    node.AddAttribute(new SyntaxAttribute(tokenPosition, NetNodeType.Attribute.ToString(), identifier));
                    if (this.Token == 0x63)
                    {
                        this.MoveNext();
                        ISyntaxNode node2 = null;
                        if (!this.ParseVariableInitializer(ref node2))
                        {
                            flag = false;
                        }
                        if (node2 != null)
                        {
                            node.AddChild(node2);
                        }
                    }
                }
                else
                {
                    flag = false;
                    this.SyntaxError();
                }
            }
            finally
            {
                this.SyntaxTree.Pop();
            }
            node.Range.EndPoint = this.prevPosition;
            return flag;
        }

        protected virtual bool ParseSimpleExpression(ref ISyntaxNode node, ref bool post)
        {
            bool flag = true;
            int lineIndex = base.lineIndex;
            switch (((VbScriptLexerToken) this.Token))
            {
                case VbScriptLexerToken.False:
                case VbScriptLexerToken.Nothing:
                case VbScriptLexerToken.Null:
                case VbScriptLexerToken.True:
                case VbScriptLexerToken.Integer_Literal:
                case VbScriptLexerToken.Float_Literal:
                case VbScriptLexerToken.Double_Literal:
                case VbScriptLexerToken.Date_Literal:
                case VbScriptLexerToken.Character_Literal:
                case VbScriptLexerToken.String_Literal:
                case VbScriptLexerToken.Identifier_Literal:
                    node = new PrimaryExpressionNode(this.TokenPosition, this.TokenString, 0x99, this.Token);
                    this.MoveNext();
                    break;

                case VbScriptLexerToken.New:
                    flag = this.ParseNewExpression(ref node);
                    break;

                case VbScriptLexerToken.CBool:
                    flag = this.ParseCastTargetExpression(ref node, VbScriptLexerToken.Boolean);
                    break;

                case VbScriptLexerToken.CByte:
                    flag = this.ParseCastTargetExpression(ref node, VbScriptLexerToken.Byte);
                    break;

                case VbScriptLexerToken.CChar:
                    flag = this.ParseCastTargetExpression(ref node, VbScriptLexerToken.Char);
                    break;

                case VbScriptLexerToken.CDate:
                    flag = this.ParseCastTargetExpression(ref node, VbScriptLexerToken.Date);
                    break;

                case VbScriptLexerToken.CDbl:
                    flag = this.ParseCastTargetExpression(ref node, VbScriptLexerToken.Double);
                    break;

                case VbScriptLexerToken.CDec:
                    flag = this.ParseCastTargetExpression(ref node, VbScriptLexerToken.Decimal);
                    break;

                case VbScriptLexerToken.CInt:
                    flag = this.ParseCastTargetExpression(ref node, VbScriptLexerToken.Integer);
                    break;

                case VbScriptLexerToken.CLng:
                    flag = this.ParseCastTargetExpression(ref node, VbScriptLexerToken.Long);
                    break;

                case VbScriptLexerToken.CObj:
                    flag = this.ParseCastTargetExpression(ref node, VbScriptLexerToken.Object);
                    break;

                case VbScriptLexerToken.CShort:
                    flag = this.ParseCastTargetExpression(ref node, VbScriptLexerToken.Short);
                    break;

                case VbScriptLexerToken.CSng:
                    flag = this.ParseCastTargetExpression(ref node, VbScriptLexerToken.Single);
                    break;

                case VbScriptLexerToken.CStr:
                    flag = this.ParseCastTargetExpression(ref node, VbScriptLexerToken.String);
                    break;

                case VbScriptLexerToken.Open_parens:
                    flag = this.ParseParenthesizedExpression(ref node);
                    break;

                case VbScriptLexerToken.Dot:
                    node = new SyntaxNode(this.TokenPosition, this.TokenString, 0x99);
                    break;

                default:
                    flag = false;
                    this.SyntaxError();
                    if (this.IsSymbol(this.Token))
                    {
                        this.SkipSymbols();
                    }
                    else
                    {
                        this.MoveNext();
                    }
                    break;
            }
            post = !this.ParseStatementTerminator(lineIndex);
            return flag;
        }

        protected virtual bool ParseStatement(bool block)
        {
            switch (this.Token)
            {
                case 15:
                    return this.ParseVariableDeclaration(null, this.TokenPosition, block ? 15 : 13);

                case 0x10:
                    return this.ParseVariableDeclaration(null, this.TokenPosition, 14);

                case 20:
                case 0x15:
                {
                    bool flag = false;
                    this.SaveState();
                    try
                    {
                        flag = this.ParseVariableDeclaration(null, this.TokenPosition, block ? 15 : 13);
                    }
                    finally
                    {
                        this.RestoreState(!flag);
                    }
                    if (flag)
                    {
                        return true;
                    }
                    ISyntaxAttributes attrs = null;
                    this.ParseModifiers(ref attrs);
                    return this.ParseEmbeddedStatement(attrs);
                }
            }
            return this.ParseEmbeddedStatement();
        }

        protected virtual bool ParseStatementExpression()
        {
            ISyntaxNode node = null;
            int lineIndex = base.lineIndex;
            bool flag = true;
            this.StartExpression();
            try
            {
                flag = this.ParsePrimaryExpression(ref node);
                if (this.Token == 0x63)
                {
                    if (!this.TryParseAssignmentExpression(ref node, lineIndex))
                    {
                        flag = false;
                    }
                }
                else if ((!this.ParseStatementTerminator(lineIndex) && !this.IsInvalidExpressionToken(this.Token)) && !this.ParseInvocationExpression(ref node, false))
                {
                    flag = false;
                }
                if (node != null)
                {
                    this.AddNode(node);
                }
            }
            finally
            {
                if (!this.EndExpression())
                {
                    flag = false;
                }
            }
            return flag;
        }

        protected virtual bool ParseStatementExpressionList()
        {
            bool flag = this.ParseStatementExpression();
            while (this.Token == 90)
            {
                this.MoveNext();
                if (!this.ParseStatementExpression())
                {
                    flag = false;
                }
            }
            return flag;
        }

        protected virtual bool ParseStatementList()
        {
            bool flag = true;
            while (!this.Eof)
            {
                if (!this.ParseStatement(false))
                {
                    flag = false;
                }
            }
            return flag;
        }

        protected virtual bool ParseStatementTerminator()
        {
            if (this.Token == 0x5b)
            {
                this.MoveNext();
            }
            return true;
        }

        protected virtual bool ParseStatementTerminator(int line)
        {
            if (this.Token == 0x5b)
            {
                this.MoveNext();
                return true;
            }
            return (base.lineIndex != line);
        }

        protected virtual bool ParseStopStatement()
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, this.TokenString, 0x7e);
            this.AddNode(node);
            this.MoveNext();
            if (!this.ParseStatementTerminator())
            {
                flag = false;
            }
            node.Range.EndPoint = this.prevPosition;
            return flag;
        }

        protected virtual bool ParseTypeName(out string type)
        {
            return this.ParseQualifiedIdentifier(out type);
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

        protected virtual bool ParseVariableDeclaration(ISyntaxAttributes attrs, Point pos, int nodeType)
        {
            bool flag = true;
            this.MoveNext();
            if (this.Token == 0x13)
            {
                this.MoveNext();
            }
            while (!this.Eof)
            {
                if (this.IsIdentifierToken(this.Token))
                {
                    string identifier = string.Empty;
                    if (this.ParseVariableIdentifier(out identifier))
                    {
                        ISyntaxNodes names = null;
                        while (this.Token == 90)
                        {
                            string str2;
                            this.MoveNext();
                            Point tokenPosition = this.TokenPosition;
                            if (this.ParseVariableIdentifier(out str2))
                            {
                                if (names == null)
                                {
                                    names = new SyntaxNodes();
                                }
                                names.Add(new SyntaxNode(tokenPosition, str2, nodeType, SyntaxNodeOptions.CodeCompletion | SyntaxNodeOptions.Indentation));
                            }
                        }
                        if (!this.ParseFieldDeclaration(attrs, pos, identifier, names, nodeType))
                        {
                            this.SyntaxError();
                            flag = false;
                        }
                    }
                    else
                    {
                        this.SyntaxError();
                        flag = false;
                    }
                }
                else
                {
                    this.SyntaxError();
                    this.MoveNext();
                    return false;
                }
                if (this.Token != 90)
                {
                    return flag;
                }
                this.MoveNext();
            }
            return flag;
        }

        protected virtual bool ParseVariableIdentifier(out string identifier)
        {
            string str;
            bool flag = true;
            identifier = string.Empty;
            if (!this.ParseQualifiedIdentifier(out identifier))
            {
                flag = false;
            }
            if (!this.TryParseArraySizeInitializationModifier(out str))
            {
                identifier = identifier + str;
            }
            return flag;
        }

        protected virtual bool ParseVariableInitializer(ref ISyntaxNode node)
        {
            return this.ParseExpression(ref node);
        }

        protected virtual bool ParseWhileStatement()
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, this.TokenString, 0x47, SyntaxNodeOptions.Indentation);
            node.AddAttribute(new SyntaxAttribute(this.TokenPosition, SyntaxConsts.SuppressReparsingScope, null));
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
                bool flag2 = flag;
                if (!this.ParseStatementTerminator())
                {
                    flag2 = false;
                    flag = false;
                }
                flag = this.ParseBlock(0x33);
                flag2 = flag;
                this.MoveNext();
                if (!flag2)
                {
                    node.AddAttribute(new SyntaxAttribute(this.prevPosition, SyntaxConsts.DefinitionScopeEndExpected, "\r\n\r\n" + VbScriptLexerToken.Wend.ToString()));
                }
                if (!this.ParseStatementTerminator())
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

        protected virtual bool ParseWithStatement()
        {
            return this.ParseExpressionBlockStatement(40, 0x80);
        }

        public override bool ProcessAutoComplete(string text, Point position, out string code)
        {
            code = string.Empty;
            ISyntaxNode nodeAt = this.GetNodeAt(position);
            bool flag = true;
            if ((nodeAt.NodeType == 1) || (nodeAt == this.SyntaxTree.Root))
            {
                this.ReparseText();
            }
            else
            {
                flag = this.ReparseBlock(new Point(0, position.Y));
            }
            if (flag)
            {
                nodeAt = this.GetNodeAt(position);
                if (nodeAt != null)
                {
                    nodeAt = this.GetIncompleteNode(nodeAt);
                    if (nodeAt != null)
                    {
                        ISyntaxAttribute attribute = nodeAt.FindAttribute(SyntaxConsts.DefinitionScopeEndExpected);
                        if (attribute != null)
                        {
                            code = (string) attribute.Value;
                            return true;
                        }
                    }
                }
            }
            return base.ProcessAutoComplete(text, position, out code);
        }

        public virtual void RegisterAssembly(Assembly assembly)
        {
            ((IReflectionRepository) this.CompletionRepository).RegisterAssembly(assembly);
        }

        public virtual bool RegisterAssembly(string name)
        {
            return ((IReflectionRepository) this.CompletionRepository).RegisterAssembly(name);
        }

        public override bool ReparseBlock(Point position)
        {
            bool flag = false;
            ISyntaxNode blockNode = this.SyntaxTree.FindNode(new SyntaxNode(position, string.Empty), base.pointNodeComparer);
            if (blockNode == null)
            {
                return flag;
            }
            blockNode = this.GetBlockNode(blockNode, position);
            if (blockNode == null)
            {
                return flag;
            }
            if (blockNode.Parent == null)
            {
                this.ReparseText();
                return true;
            }
            return this.ReparseBlock(blockNode, position);
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

        public override bool ReparseBlock(Point position, string text, out ISyntaxNode node, CodeCompletionType completionType)
        {
            bool flag = this.ReparseBlock(position);
            if ((completionType == CodeCompletionType.SpecialListMembers) && (position.X <= text.Length))
            {
                while ((position.X > 0) && (text[position.X - 1] == ' '))
                {
                    position.X--;
                }
            }
            node = this.GetNodeAt(position);
            return flag;
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
            this.CodeCompletionChars = SyntaxParserConsts.DefaultVbScriptCodeCompletionChars.ToCharArray();
        }

        public override void ResetCodeCompletionStopChars()
        {
            this.CodeCompletionStopChars = SyntaxParserConsts.DefaultNetCodeCompletionStopChars.ToCharArray();
        }

        public override void ResetOptions()
        {
            this.Options = SyntaxParserConsts.DefaultNetSyntaxOptions | SyntaxOptions.ReparseOnLineChange;
        }

        public override void ResetSmartFormatChars()
        {
            this.SmartFormatChars = SyntaxParserConsts.DefaultVbSmartFormatChars.ToCharArray();
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

        public override bool ShouldSerializeAutoIndentChars()
        {
            return (new string(this.AutoIndentChars) != SyntaxParserConsts.DefaultCsAutoIndentChars);
        }

        public override bool ShouldSerializeCodeCompletionChars()
        {
            return (new string(this.CodeCompletionChars) != SyntaxParserConsts.DefaultVbScriptCodeCompletionChars);
        }

        public override bool ShouldSerializeCodeCompletionStopChars()
        {
            return (new string(this.CodeCompletionStopChars) != SyntaxParserConsts.DefaultNetCodeCompletionStopChars);
        }

        public override bool ShouldSerializeSmartFormatChars()
        {
            return (new string(this.SmartFormatChars) != SyntaxParserConsts.DefaultVbSmartFormatChars);
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

        protected virtual void SmartCapitalize(string s, short[] colorData, ITextUndoList operations)
        {
            int index = 0;
            int length = s.Length;
            while (index < length)
            {
                if (((byte) colorData[index]) == 3)
                {
                    int num3 = index;
                    while ((num3 < (length - 1)) && (((byte) colorData[num3 + 1]) == 3))
                    {
                        num3++;
                    }
                    string str = s.Substring(index, (num3 - index) + 1);
                    object obj2 = this.reswords[str.ToLower()];
                    if (obj2 != null)
                    {
                        string text = obj2.ToString();
                        if (text != str)
                        {
                            operations.Add(new TextUndo(index, (num3 - index) + 1, text));
                        }
                    }
                    index = num3;
                }
                index++;
            }
        }

        public override int SmartFormatLine(int index, string text, short[] textData, ITextUndoList operations)
        {
            if ((this.Options & SyntaxOptions.FormatCase) != SyntaxOptions.None)
            {
                this.SmartCapitalize(text, textData, operations);
            }
            ISyntaxNodes nodes = new SyntaxNodes();
            this.SyntaxTree.FindNodes(new SyntaxNode(new Point(0, index), string.Empty), base.lineNodeComparer, nodes);
            int num = this.GetSmartIndent(nodes, index, false);
            if ((this.Options & SyntaxOptions.FormatSpaces) != SyntaxOptions.None)
            {
                NETRepository.SmartFormatLine(index, text, textData, nodes, operations);
            }
            return num;
        }

        protected virtual void StartExpression()
        {
            if (this.expressionCount == 0)
            {
                this.expressions.Clear();
            }
            this.expressionCount++;
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
                ISyntaxError err = new QWhale.Syntax.SyntaxError(this.TokenPosition, this.TokenString, ((VbScriptLexerToken) token).ToString() + " " + StringConsts.ErrExpected);
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

        protected virtual bool TryParseArraySizeInitializationModifier(out string rank)
        {
            bool flag = true;
            rank = string.Empty;
            while (this.Token == 0x57)
            {
                string str;
                flag = this.ParseArraySizeInitializationModifier(out str);
                if (flag)
                {
                    rank = rank + str;
                }
            }
            return flag;
        }

        protected virtual bool TryParseAssignmentExpression(ref ISyntaxNode node, int line)
        {
            bool flag = true;
            if (this.Token == 0x63)
            {
                node = this.CreateExpressionNode(this.TokenPosition, this.TokenString, 0x86, node, true);
                node.AddAttribute(new SyntaxAttribute(this.TokenPosition, NetNodeType.Assignment.ToString(), null));
                ISyntaxNode node2 = null;
                this.MoveNext();
                if (!this.ParseInclusiveOrExpression(ref node2))
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
            return true;
        }

        protected virtual bool TryParsePostPrimaryExpression(ref ISyntaxNode node)
        {
            bool flag = true;
            while (!this.Eof)
            {
                switch (this.Token)
                {
                    case 0x57:
                    {
                        if (!this.ParseInvocationExpression(ref node, true))
                        {
                            flag = false;
                        }
                        continue;
                    }
                    case 0x58:
                        return flag;

                    case 0x59:
                    {
                        if (!this.ParseMemberAccess(ref node))
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

        protected virtual bool TryParseWhileOrUntil(out bool parsed)
        {
            bool flag = true;
            parsed = false;
            switch (((VbScriptLexerToken) this.Token))
            {
                case VbScriptLexerToken.While:
                case VbScriptLexerToken.Until:
                {
                    int y = this.TokenPosition.Y;
                    this.MoveNext();
                    parsed = y == this.TokenPosition.Y;
                    ISyntaxNode node = null;
                    flag = this.ParseExpression(ref node);
                    if (node != null)
                    {
                        this.AddNode(node);
                    }
                    break;
                }
            }
            return flag;
        }

        public virtual bool UnregisterAssembly(Assembly assembly, bool removeReferences)
        {
            return ((IReflectionRepository) this.CompletionRepository).UnregisterAssembly(assembly, removeReferences);
        }

        public virtual bool UnregisterAssembly(string name, bool removeReferences)
        {
            return ((IReflectionRepository) this.CompletionRepository).UnregisterAssembly(name, removeReferences);
        }

        protected override bool UpdateLine()
        {
            if ((this.expressionCount > 0) && (this.prevToken != 0x69))
            {
                this.expressions.Add(this.prevPosition.Y);
            }
            return base.UpdateLine();
        }

        public override bool CaseSensitive
        {
            get
            {
                return false;
            }
        }

        internal class VbScriptBuiltInFunctions
        {
            public static decimal Abs(object Number)
            {
                return 0M;
            }

            public static IList<object> Array(object[] values)
            {
                return null;
            }

            public static int Asc(string Expression)
            {
                return 0;
            }

            public static int AscB(string Expression)
            {
                return 0;
            }

            public static int AscW(string Expression)
            {
                return 0;
            }

            public static double Atn(object Number)
            {
                return 0.0;
            }

            public static char Chr(int Expression)
            {
                return '\0';
            }

            public static char ChrB(int Expression)
            {
                return '\0';
            }

            public static char ChrW(int Expression)
            {
                return '\0';
            }

            public static double Cos(object Number)
            {
                return 0.0;
            }

            public static object CreateObject(object Expression)
            {
                return null;
            }

            public static DateTime Date()
            {
                return DateTime.Now;
            }

            public static DateTime DateAdd(object Interval, int Number, object Date)
            {
                return DateTime.Now;
            }

            public static object DateDiff(object Interval, object Date1, object Date2, int FirstDayOfWeek, int FirstWeekOfYear)
            {
                return null;
            }

            public static object DatePart(object Interval, object Date, int FirstDayOfWeek, int FirstWeekOfYear)
            {
                return null;
            }

            public static DateTime DateSerial(int Year, int Month, int Day)
            {
                return DateTime.Now;
            }

            public static DateTime DateValue(object Expression)
            {
                return DateTime.Now;
            }

            public static int Day(object Date)
            {
                return 0;
            }

            public static string Escape(string CharString)
            {
                return string.Empty;
            }

            public static object Eval(object Expression)
            {
                return null;
            }

            public static object Execute(string String)
            {
                return null;
            }

            public static double Exp(object Number)
            {
                return 0.0;
            }

            public static IList<string> Filter(IList<string> InputStrings, string Value, bool Include, int Compare)
            {
                return null;
            }

            public static int Fix(object Number)
            {
                return 0;
            }

            public static object FormatCurrency(object Expression, int NumDigitsAfterDecimal, int IncludeLeadingDigit, int UseParensForNegativeNumbers, int GroupDigits)
            {
                return null;
            }

            public static object FormatDateTime(object Expression, int NamedFormat)
            {
                return null;
            }

            public static object FormatNumber(object Expression, int NumDigitsAfterDecimal, int IncludeLeadingDigit, int UseParensForNegativeNumbers, int GroupDigits)
            {
                return null;
            }

            public static object FormatPercent(object Expression, int NumDigitsAfterDecimal, int IncludeLeadingDigit, int UseParensForNegativeNumbers, int GroupDigits)
            {
                return null;
            }

            public static object GetObject(string PathName, string Class)
            {
                return null;
            }

            public static object GetRef(string ProcName)
            {
                return null;
            }

            public static object Hex(object Number)
            {
                return null;
            }

            public static int Hour(object Time)
            {
                return 0;
            }

            public static string InputBox(string Prompt, string Title, string Default, int XPos, int YPos, string HelpFile, int Context)
            {
                return string.Empty;
            }

            public static int InStr(int Start, string String1, string String2, int Compare)
            {
                return 0;
            }

            public static int InStrB(int Start, string String1, string String2, int Compare)
            {
                return 0;
            }

            public static int InStrRev(string String1, string String2, int Start, int Compare)
            {
                return 0;
            }

            public static int Int(object Number)
            {
                return 0;
            }

            public static bool IsArray(object VarName)
            {
                return false;
            }

            public static bool IsDate(object Expression)
            {
                return false;
            }

            public static bool IsEmpty(object Expression)
            {
                return false;
            }

            public static bool IsNull(object Expression)
            {
                return false;
            }

            public static bool IsNumeric(object Expression)
            {
                return false;
            }

            public static bool IsObject(object Expression)
            {
                return false;
            }

            public static string Join(IList<string> List, string Delimiter)
            {
                return string.Empty;
            }

            public static int LBound(string ArrayName, int Dimension)
            {
                return 0;
            }

            public static string LCase(string String)
            {
                return string.Empty;
            }

            public static string Left(string String, int Length)
            {
                return string.Empty;
            }

            public static string LeftB(string String, int Length)
            {
                return string.Empty;
            }

            public static int Len(object VarName)
            {
                return 0;
            }

            public static int Len(string String)
            {
                return 0;
            }

            public static int LenB(object VarName)
            {
                return 0;
            }

            public static int LenB(string String)
            {
                return 0;
            }

            public static object LoadPicture(string PictureName)
            {
                return null;
            }

            public static double Log(object Number)
            {
                return 0.0;
            }

            public static string LTrim(string String)
            {
                return string.Empty;
            }

            public static string Mid(string String, int Start, int Length)
            {
                return string.Empty;
            }

            public static string MidB(string String, int Start, int Length)
            {
                return string.Empty;
            }

            public static int Minute(object Time)
            {
                return 0;
            }

            public static int Month(object Date)
            {
                return 0;
            }

            public static string MonthName(int Month, bool Abbreviate)
            {
                return string.Empty;
            }

            public static string MsgBox(string Prompt, int Buttons, string Title, string HelpFile, int Context)
            {
                return string.Empty;
            }

            public static DateTime Now()
            {
                return DateTime.Now;
            }

            public static string Oct(object Number)
            {
                return string.Empty;
            }

            public static string Replace(string Expression, string Find, string ReplaceWith, int Start, int Count, int Compare)
            {
                return string.Empty;
            }

            public static int RGB(int Red, int Green, int Blue)
            {
                return 0;
            }

            public static string Right(string String, int Length)
            {
                return string.Empty;
            }

            public static string RightB(string String, int Length)
            {
                return string.Empty;
            }

            public static int Rnd(object Number)
            {
                return 0;
            }

            public static double Round(object Expression, int NumDecimalPlaces)
            {
                return 0.0;
            }

            public static string RTrim(string String)
            {
                return string.Empty;
            }

            public static string ScriptEngine()
            {
                return string.Empty;
            }

            public static string ScriptEngineBuildVersion()
            {
                return string.Empty;
            }

            public static string ScriptEngineMajorVersion()
            {
                return string.Empty;
            }

            public static string ScriptEngineMinorVersion()
            {
                return string.Empty;
            }

            public static int Second(object Time)
            {
                return 0;
            }

            public static object SetLocale(object lcid)
            {
                return null;
            }

            public static int Sgn(object Number)
            {
                return 0;
            }

            public static double Sin(object Number)
            {
                return 0.0;
            }

            public static string Space(int Number)
            {
                return string.Empty;
            }

            public static IList<string> Split(string Expression, string Delimiter, int Count, int Compare)
            {
                return null;
            }

            public static double Sqr(object Number)
            {
                return 0.0;
            }

            public static int StrComp(string String1, string String2, int Compare)
            {
                return 0;
            }

            public static string String(int Number, object Character)
            {
                return string.Empty;
            }

            public static string StrReverse(string String1)
            {
                return string.Empty;
            }

            public static double Tan(object Number)
            {
                return 0.0;
            }

            public static DateTime Time()
            {
                return DateTime.Now;
            }

            public static int Timer()
            {
                return 0;
            }

            public static DateTime TimeSerial(int Hour, int Minute, int Second)
            {
                return DateTime.Now;
            }

            public static DateTime TimeValue(object Time)
            {
                return DateTime.Now;
            }

            public static string Trim(string String)
            {
                return string.Empty;
            }

            public static string TypeName(object VarName)
            {
                return string.Empty;
            }

            public static int UBound(string ArrayName, int Dimension)
            {
                return 0;
            }

            public static string UCase(string String)
            {
                return string.Empty;
            }

            public static string Unescape(string CharString)
            {
                return string.Empty;
            }

            public static int VarType(object VarName)
            {
                return 0;
            }

            public static int Weekday(object Date, int FirstDayOfWeek)
            {
                return 0;
            }

            public static string WeekdayName(int Weekday, bool Abbreviate, int FirstDayOfWeek)
            {
                return string.Empty;
            }

            public static int Year(object Date)
            {
                return 0;
            }
        }
    }
}

