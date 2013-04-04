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
    using System.Runtime.InteropServices;

    [ToolboxBitmap(typeof(VbParser), "Images.VbParser.bmp"), ToolboxItem(true)]
    public class VbParser : NetSyntaxParser
    {
        private int expressionCount;
        private IList<int> expressions = new List<int>();
        protected LexerProc lexCommentProc;
        protected LexerProc lexDefineProc;
        protected LexerProc lexIdentifierProc;
        protected LexerProc lexNumberProc;
        protected LexerProc lexStringProc;
        protected LexerProc lexSymbolProc;
        protected LexerProc lexWhitespaceProc;
        protected LexerProc lexXmlCommentProc;
        protected LexerProc lexXmlCommentTagProc;
        private int prevToken;
        protected Hashtable reswords;
        private const int stateNormal = 0;
        private const int stateXmlComment = 2;

        public VbParser()
        {
            this.Options |= SyntaxOptions.AutoComplete | SyntaxOptions.ReparseOnLineChange;
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
            IReflectionRepository repository = new VbRepository(this.CaseSensitive, this.SyntaxTree);
            repository.RegisterDefaultAssemblies();
            repository.RegisterType("Boolean", typeof(bool));
            repository.RegisterType("Byte", typeof(byte));
            repository.RegisterType("Char", typeof(char));
            repository.RegisterType("Date", typeof(DateTime));
            repository.RegisterType("Decimal", typeof(decimal));
            repository.RegisterType("Double", typeof(double));
            repository.RegisterType("Single", typeof(float));
            repository.RegisterType("Integer", typeof(int));
            repository.RegisterType("Long", typeof(long));
            repository.RegisterType("Object", typeof(object));
            repository.RegisterType("Short", typeof(short));
            repository.RegisterType("String", typeof(string));
            repository.RegisterType("__enum", typeof(Enum));
            this.InitGlobalModules(repository);
            repository.RegisterSnippet("Boolean", false);
            repository.RegisterSnippet("Byte", false);
            repository.RegisterSnippet("Char", false);
            repository.RegisterSnippet("Date", false);
            repository.RegisterSnippet("DateTime", false);
            repository.RegisterSnippet("Decimal", false);
            repository.RegisterSnippet("Double", false);
            repository.RegisterSnippet("Int16", false);
            repository.RegisterSnippet("Int32", false);
            repository.RegisterSnippet("Int64", false);
            repository.RegisterSnippet("Integer", false);
            repository.RegisterSnippet("Long", false);
            repository.RegisterSnippet("New", false);
            repository.RegisterSnippet("Object", false);
            repository.RegisterSnippet("SByte", false);
            repository.RegisterSnippet("Short", false);
            repository.RegisterSnippet("Single", false);
            repository.RegisterSnippet("String", false);
            repository.RegisterSnippet("UInt16", false);
            repository.RegisterSnippet("UInt32", false);
            repository.RegisterSnippet("UInt64", false);
            repository.RegisterSnippet("UInteger", false);
            repository.RegisterSnippet("ULong", false);
            repository.RegisterSnippet("UShort", false);
            return repository;
        }

        protected virtual bool EndExpression()
        {
            bool flag = true;
            this.expressionCount--;
            if (((this.expressionCount == 0) && (this.expressions.Count > 0)) && (base.Stack.Count == 0))
            {
                string name = VbLexerToken.LineContinuation.ToString();
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

        protected bool Expected(VbLexerToken token)
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
            return this.Expected((VbLexerToken) token);
        }

        protected bool Expected(VbLexerToken token1, VbLexerToken token2)
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

        protected virtual ISyntaxNode FindWithStatement(ISyntaxNode node)
        {
            while (node != null)
            {
                if (node.NodeType == 0x80)
                {
                    return node;
                }
                node = node.Parent;
            }
            return null;
        }

        protected virtual int GetBlockToken(int nodeType)
        {
            switch (nodeType)
            {
                case 7:
                    return 0x56;

                case 8:
                    return 0x18;

                case 9:
                    return 0x81;

                case 10:
                    return 0x44;

                case 11:
                    return 0x2f;

                case 12:
                    return 0x51;

                case 0x15:
                    return 0x6c;
            }
            return 0x56;
        }

        public override CodeCompletionType GetCompletionType(char ch)
        {
            switch (ch)
            {
                case '<':
                    return CodeCompletionType.ListMembers;

                case '=':
                case ' ':
                    return CodeCompletionType.SpecialListMembers;

                case '>':
                    return CodeCompletionType.CompleteWord;

                case ',':
                    return CodeCompletionType.ParameterInfo;

                case '\'':
                    return CodeCompletionType.CompleteComment;
            }
            return base.GetCompletionType(ch);
        }

        protected virtual ISyntaxNode GetIncompleteNode(ISyntaxNode node)
        {
            while (node != null)
            {
                NetNodeType nodeType = (NetNodeType) node.NodeType;
                if (nodeType <= NetNodeType.ForEachStatement)
                {
                    switch (nodeType)
                    {
                        case NetNodeType.WhileStatement:
                        case NetNodeType.DoStatement:
                        case NetNodeType.ForStatement:
                        case NetNodeType.ForEachStatement:
                        case NetNodeType.IfStatement:
                        case NetNodeType.Class:
                        case NetNodeType.Struct:
                        case NetNodeType.XmlComment:
                            goto Label_009B;

                        case NetNodeType.ParameterList:
                            goto Label_00AE;
                    }
                    goto Label_011D;
                }
                if (nodeType <= NetNodeType.BlockStatement)
                {
                    switch (nodeType)
                    {
                        case NetNodeType.TryStatement:
                        case NetNodeType.UsingStatement:
                        case NetNodeType.BlockStatement:
                            goto Label_009B;
                    }
                    goto Label_011D;
                }
                if (((nodeType != NetNodeType.ElseIfStatement) && (nodeType != NetNodeType.SelectStatement)) && (nodeType != NetNodeType.WithStatement))
                {
                    goto Label_011D;
                }
            Label_009B:
                if (node.FindAttribute(SyntaxConsts.DefinitionScopeEndExpected) != null)
                {
                    return node;
                }
                return null;
            Label_00AE:
                if (node.Parent != null)
                {
                    foreach (ISyntaxNode node2 in node.Parent.Childs)
                    {
                        if ((node2.NodeType == 0x6a) && (node.Range.EndPoint == node2.Range.StartPoint))
                        {
                            node = node2;
                            if (node.FindAttribute(SyntaxConsts.DefinitionScopeEndExpected) != null)
                            {
                                return node;
                            }
                            return null;
                        }
                    }
                }
            Label_011D:
                node = node.Parent;
            }
            return null;
        }

        protected override int GetLexerStyle(int token)
        {
            if (this.IsKeywordToken(token))
            {
                return 2;
            }
            switch (token)
            {
                case 190:
                    return 0;

                case 0xbf:
                case 0xc0:
                case 0xc1:
                case 0xc2:
                case 0xc3:
                case 0xc4:
                case 0xc5:
                    return 1;

                case 0xc6:
                case 0xc7:
                    return 7;

                case 200:
                case 0xca:
                    return 3;

                case 0xc9:
                    return 4;

                case 0xcb:
                    return 8;
            }
            return 6;
        }

        public override string GetSingleLineComment()
        {
            return "'";
        }

        public override string GetXmlComment()
        {
            return "'''";
        }

        protected bool IdentifierExpected()
        {
            return this.Expected(VbLexerToken.Identifier_Literal, VbLexerToken.MyClass);
        }

        protected virtual void InitGlobalModules(IReflectionRepository repository)
        {
            repository.RegisterType("Interaction", typeof(Interaction), true);
            repository.RegisterType("Information", typeof(Information), true);
            repository.RegisterType("Strings", typeof(Strings), true);
            repository.RegisterType("Err", typeof(ErrObject), true);
            repository.RegisterNamespace("Microsoft.VisualBasic");
        }

        protected override void InitLanguage()
        {
            this.Scheme.FileType = "vb";
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
            this.lexDefineProc = new LexerProc(this.LexDefine);
            this.lexXmlCommentProc = new LexerProc(this.LexXmlComment);
            this.lexXmlCommentTagProc = new LexerProc(this.LexXmlCommentTag);
            base.RegisterLexerProc(0, this.lexWhitespaceProc);
            base.RegisterLexerProc(0, '!', '\x00ff', this.lexSymbolProc);
            base.RegisterLexerProc(0, 'a', 'z', this.lexIdentifierProc);
            base.RegisterLexerProc(0, 'A', 'Z', this.lexIdentifierProc);
            base.RegisterLexerProc(0, '_', this.lexIdentifierProc);
            base.RegisterLexerProc(0, '0', '9', this.lexNumberProc);
            base.RegisterLexerProc(0, new char[] { '.', '&' }, this.lexNumberProc);
            base.RegisterLexerProc(0, new char[] { '+', '-' }, this.lexSymbolProc);
            base.RegisterLexerProc(0, '"', this.lexStringProc);
            base.RegisterLexerProc(0, new char[] { '\'', 'r', 'R' }, this.lexCommentProc);
            base.RegisterLexerProc(0, '#', this.lexDefineProc);
            base.RegisterLexerProc(2, this.lexXmlCommentProc);
            base.RegisterLexerProc(2, '<', this.lexXmlCommentTagProc);
        }

        protected virtual void InitReswords()
        {
            this.reswords = new Hashtable();
            this.reswords.Add("addhandler", VbLexerToken.AddHandler);
            this.reswords.Add("addressof", VbLexerToken.AddressOf);
            this.reswords.Add("alias", VbLexerToken.Alias);
            this.reswords.Add("and", VbLexerToken.And);
            this.reswords.Add("andalso", VbLexerToken.AndAlso);
            this.reswords.Add("ansi", VbLexerToken.Ansi);
            this.reswords.Add("as", VbLexerToken.As);
            this.reswords.Add("assembly", VbLexerToken.Assembly);
            this.reswords.Add("auto", VbLexerToken.Auto);
            this.reswords.Add("boolean", VbLexerToken.Boolean);
            this.reswords.Add("byref", VbLexerToken.ByRef);
            this.reswords.Add("byte", VbLexerToken.Byte);
            this.reswords.Add("byval", VbLexerToken.ByVal);
            this.reswords.Add("call", VbLexerToken.Call);
            this.reswords.Add("case", VbLexerToken.Case);
            this.reswords.Add("catch", VbLexerToken.Catch);
            this.reswords.Add("cbool", VbLexerToken.CBool);
            this.reswords.Add("cbyte", VbLexerToken.CByte);
            this.reswords.Add("cchar", VbLexerToken.CChar);
            this.reswords.Add("cdate", VbLexerToken.CDate);
            this.reswords.Add("cdbl", VbLexerToken.CDbl);
            this.reswords.Add("cdec", VbLexerToken.CDec);
            this.reswords.Add("char", VbLexerToken.Char);
            this.reswords.Add("cint", VbLexerToken.CInt);
            this.reswords.Add("class", VbLexerToken.Class);
            this.reswords.Add("clng", VbLexerToken.CLng);
            this.reswords.Add("cobj", VbLexerToken.CObj);
            this.reswords.Add("const", VbLexerToken.Const);
            this.reswords.Add("continue", VbLexerToken.Continue);
            this.reswords.Add("cshort", VbLexerToken.CShort);
            this.reswords.Add("csng", VbLexerToken.CSng);
            this.reswords.Add("cstr", VbLexerToken.CStr);
            this.reswords.Add("ctype", VbLexerToken.CType);
            this.reswords.Add("date", VbLexerToken.Date);
            this.reswords.Add("decimal", VbLexerToken.Decimal);
            this.reswords.Add("declare", VbLexerToken.Declare);
            this.reswords.Add("default", VbLexerToken.Default);
            this.reswords.Add("delegate", VbLexerToken.Delegate);
            this.reswords.Add("dim", VbLexerToken.Dim);
            this.reswords.Add("directcast", VbLexerToken.DirectCast);
            this.reswords.Add("do", VbLexerToken.Do);
            this.reswords.Add("double", VbLexerToken.Double);
            this.reswords.Add("each", VbLexerToken.Each);
            this.reswords.Add("else", VbLexerToken.Else);
            this.reswords.Add("elseif", VbLexerToken.ElseIf);
            this.reswords.Add("end", VbLexerToken.End);
            this.reswords.Add("endif", VbLexerToken.EndIf);
            this.reswords.Add("enum", VbLexerToken.Enum);
            this.reswords.Add("erase", VbLexerToken.Erase);
            this.reswords.Add("error", VbLexerToken.Error);
            this.reswords.Add("event", VbLexerToken.Event);
            this.reswords.Add("exit", VbLexerToken.Exit);
            this.reswords.Add("false", VbLexerToken.False);
            this.reswords.Add("finally", VbLexerToken.Finally);
            this.reswords.Add("for", VbLexerToken.For);
            this.reswords.Add("friend", VbLexerToken.Friend);
            this.reswords.Add("function", VbLexerToken.Function);
            this.reswords.Add("get", VbLexerToken.Get);
            this.reswords.Add("gettype", VbLexerToken.GetType);
            this.reswords.Add("gosub", VbLexerToken.GoSub);
            this.reswords.Add("goto", VbLexerToken.GoTo);
            this.reswords.Add("handles", VbLexerToken.Handles);
            this.reswords.Add("if", VbLexerToken.If);
            this.reswords.Add("implements", VbLexerToken.Implements);
            this.reswords.Add("imports", VbLexerToken.Imports);
            this.reswords.Add("in", VbLexerToken.In);
            this.reswords.Add("inherits", VbLexerToken.Inherits);
            this.reswords.Add("integer", VbLexerToken.Integer);
            this.reswords.Add("interface", VbLexerToken.Interface);
            this.reswords.Add("is", VbLexerToken.Is);
            this.reswords.Add("isnot", VbLexerToken.IsNot);
            this.reswords.Add("isfalse", VbLexerToken.IsFalse);
            this.reswords.Add("istrue", VbLexerToken.IsTrue);
            this.reswords.Add("let", VbLexerToken.Let);
            this.reswords.Add("lib", VbLexerToken.Lib);
            this.reswords.Add("like", VbLexerToken.Like);
            this.reswords.Add("long", VbLexerToken.Long);
            this.reswords.Add("loop", VbLexerToken.Loop);
            this.reswords.Add("me", VbLexerToken.Me);
            this.reswords.Add("mid", VbLexerToken.Mid);
            this.reswords.Add("mod", VbLexerToken.Mod);
            this.reswords.Add("module", VbLexerToken.Module);
            this.reswords.Add("mustinherit", VbLexerToken.MustInherit);
            this.reswords.Add("mustoverride", VbLexerToken.MustOverride);
            this.reswords.Add("mybase", VbLexerToken.MyBase);
            this.reswords.Add("myclass", VbLexerToken.MyClass);
            this.reswords.Add("namespace", VbLexerToken.Namespace);
            this.reswords.Add("new", VbLexerToken.New);
            this.reswords.Add("next", VbLexerToken.Next);
            this.reswords.Add("not", VbLexerToken.Not);
            this.reswords.Add("nothing", VbLexerToken.Nothing);
            this.reswords.Add("notinheritable", VbLexerToken.NotInheritable);
            this.reswords.Add("notoverridable", VbLexerToken.NotOverridable);
            this.reswords.Add("object", VbLexerToken.Object);
            this.reswords.Add("of", VbLexerToken.Of);
            this.reswords.Add("on", VbLexerToken.On);
            this.reswords.Add("operator", VbLexerToken.Operator);
            this.reswords.Add("option", VbLexerToken.Option);
            this.reswords.Add("optional", VbLexerToken.Optional);
            this.reswords.Add("or", VbLexerToken.Or);
            this.reswords.Add("orelse", VbLexerToken.OrElse);
            this.reswords.Add("overloads", VbLexerToken.Overloads);
            this.reswords.Add("overridable", VbLexerToken.Overridable);
            this.reswords.Add("overrides", VbLexerToken.Overrides);
            this.reswords.Add("paramarray", VbLexerToken.ParamArray);
            this.reswords.Add("partial", VbLexerToken.Partial);
            this.reswords.Add("preserve", VbLexerToken.Preserve);
            this.reswords.Add("private", VbLexerToken.Private);
            this.reswords.Add("property", VbLexerToken.Property);
            this.reswords.Add("protected", VbLexerToken.Protected);
            this.reswords.Add("public", VbLexerToken.Public);
            this.reswords.Add("raiseevent", VbLexerToken.RaiseEvent);
            this.reswords.Add("readonly", VbLexerToken.ReadOnly);
            this.reswords.Add("redim", VbLexerToken.ReDim);
            this.reswords.Add("rem", VbLexerToken.REM);
            this.reswords.Add("removehandler", VbLexerToken.RemoveHandler);
            this.reswords.Add("resume", VbLexerToken.Resume);
            this.reswords.Add("return", VbLexerToken.Return);
            this.reswords.Add("select", VbLexerToken.Select);
            this.reswords.Add("set", VbLexerToken.Set);
            this.reswords.Add("shadows", VbLexerToken.Shadows);
            this.reswords.Add("shared", VbLexerToken.Shared);
            this.reswords.Add("short", VbLexerToken.Short);
            this.reswords.Add("single", VbLexerToken.Single);
            this.reswords.Add("static", VbLexerToken.Static);
            this.reswords.Add("step", VbLexerToken.Step);
            this.reswords.Add("stop", VbLexerToken.Stop);
            this.reswords.Add("string", VbLexerToken.String);
            this.reswords.Add("structure", VbLexerToken.Structure);
            this.reswords.Add("sub", VbLexerToken.Sub);
            this.reswords.Add("synclock", VbLexerToken.SyncLock);
            this.reswords.Add("then", VbLexerToken.Then);
            this.reswords.Add("throw", VbLexerToken.Throw);
            this.reswords.Add("to", VbLexerToken.To);
            this.reswords.Add("true", VbLexerToken.True);
            this.reswords.Add("try", VbLexerToken.Try);
            this.reswords.Add("typeof", VbLexerToken.TypeOf);
            this.reswords.Add("unicode", VbLexerToken.Unicode);
            this.reswords.Add("until", VbLexerToken.Until);
            this.reswords.Add("using", VbLexerToken.Using);
            this.reswords.Add("variant", VbLexerToken.Variant);
            this.reswords.Add("wend", VbLexerToken.Wend);
            this.reswords.Add("when", VbLexerToken.When);
            this.reswords.Add("while", VbLexerToken.While);
            this.reswords.Add("with", VbLexerToken.With);
            this.reswords.Add("withevents", VbLexerToken.WithEvents);
            this.reswords.Add("writeonly", VbLexerToken.WriteOnly);
            this.reswords.Add("xor", VbLexerToken.Xor);
            this.reswords.Add("region", VbLexerToken.Region);
        }

        protected virtual bool IsAccessor()
        {
            if (this.Token != 0x39)
            {
                return (this.Token == 120);
            }
            return true;
        }

        protected virtual bool IsBaseList(int token)
        {
            if (token != 0x42)
            {
                return (token == 0x3f);
            }
            return true;
        }

        protected bool IsBlockEnd()
        {
            return (this.Token == 0x2d);
        }

        protected virtual bool IsBuiltInType(int Token)
        {
            switch (((VbLexerToken) Token))
            {
                case VbLexerToken.Date:
                case VbLexerToken.Decimal:
                case VbLexerToken.Double:
                case VbLexerToken.Boolean:
                case VbLexerToken.Byte:
                case VbLexerToken.Char:
                case VbLexerToken.Integer:
                case VbLexerToken.Long:
                case VbLexerToken.Short:
                case VbLexerToken.Single:
                case VbLexerToken.String:
                case VbLexerToken.Object:
                    return true;
            }
            return false;
        }

        public override bool IsCodeCompletionChar(char ch, byte style, ref int interval)
        {
            if ((style != 5) || (((ch != '\'') && (ch != '<')) && (ch != '>')))
            {
                return base.IsCodeCompletionChar(ch, style, ref interval);
            }
            return true;
        }

        protected override bool IsComment(int tok)
        {
            if (tok != 200)
            {
                return this.IsXmlComment(tok);
            }
            return true;
        }

        protected virtual bool IsComparisionOperator(int token)
        {
            switch (((VbLexerToken) token))
            {
                case VbLexerToken.Assign:
                case VbLexerToken.Op_lt:
                case VbLexerToken.Op_gt:
                case VbLexerToken.Op_le:
                case VbLexerToken.Op_ge:
                case VbLexerToken.Op_ne:
                    return true;
            }
            return false;
        }

        protected virtual bool IsDeclarationToken(int token)
        {
            switch (((VbLexerToken) token))
            {
                case VbLexerToken.Event:
                case VbLexerToken.Function:
                case VbLexerToken.Imports:
                case VbLexerToken.Class:
                case VbLexerToken.Delegate:
                case VbLexerToken.Enum:
                case VbLexerToken.Interface:
                case VbLexerToken.Module:
                case VbLexerToken.Namespace:
                case VbLexerToken.Operator:
                case VbLexerToken.Option:
                case VbLexerToken.Property:
                case VbLexerToken.Structure:
                case VbLexerToken.Sub:
                    return true;
            }
            return false;
        }

        private bool IsFloatChar(char ch, ref int token)
        {
            bool flag = false;
            char ch2 = ch;
            if (ch2 <= 'R')
            {
                switch (ch2)
                {
                    case 'D':
                        goto Label_0071;

                    case 'E':
                        return flag;

                    case 'F':
                        goto Label_0058;

                    case 'R':
                        goto Label_003F;
                }
                return flag;
            }
            switch (ch2)
            {
                case 'd':
                    goto Label_0071;

                case 'e':
                    return flag;

                case 'f':
                    goto Label_0058;

                case 'r':
                    break;

                default:
                    return flag;
            }
        Label_003F:
            base.currentPos++;
            token = 0xc3;
            return true;
        Label_0058:
            base.currentPos++;
            token = 0xc2;
            return true;
        Label_0071:
            base.currentPos++;
            token = 0xc4;
            return true;
        }

        protected virtual bool IsFunctionToken(int token)
        {
            return (token == 0x38);
        }

        protected bool IsIdentifierToken(int token)
        {
            if (token != 190)
            {
                return (token == 0x55);
            }
            return true;
        }

        protected virtual bool IsInvalidBlockToken(int token)
        {
            if (token != 0x2d)
            {
                return this.IsDeclarationToken(token);
            }
            return true;
        }

        protected virtual bool IsInvalidExpressionToken(int token)
        {
            switch (((VbLexerToken) token))
            {
                case VbLexerToken.Const:
                case VbLexerToken.Continue:
                case VbLexerToken.Call:
                case VbLexerToken.AddHandler:
                case VbLexerToken.Dim:
                case VbLexerToken.Do:
                case VbLexerToken.ElseIf:
                case VbLexerToken.End:
                case VbLexerToken.EndIf:
                case VbLexerToken.Erase:
                case VbLexerToken.Error:
                case VbLexerToken.Exit:
                case VbLexerToken.For:
                case VbLexerToken.GoTo:
                case VbLexerToken.If:
                case VbLexerToken.Loop:
                case VbLexerToken.Mid:
                case VbLexerToken.RaiseEvent:
                case VbLexerToken.ReDim:
                case VbLexerToken.RemoveHandler:
                case VbLexerToken.Resume:
                case VbLexerToken.Return:
                case VbLexerToken.Select:
                case VbLexerToken.On:
                case VbLexerToken.Next:
                case VbLexerToken.While:
                case VbLexerToken.With:
                case VbLexerToken.Try:
                case VbLexerToken.Static:
                case VbLexerToken.Stop:
                case VbLexerToken.SyncLock:
                case VbLexerToken.Throw:
                    return true;
            }
            return this.IsInvalidBlockToken(token);
        }

        protected virtual bool IsKeywordToken(int token)
        {
            return ((token >= 0) && (token <= 0x94));
        }

        protected virtual bool IsKnownMemberDeclaration(int token)
        {
            VbLexerToken token2 = (VbLexerToken) token;
            if (token2 <= VbLexerToken.Dim)
            {
                switch (token2)
                {
                    case VbLexerToken.Const:
                    case VbLexerToken.Dim:
                        goto Label_001D;
                }
                goto Label_001F;
            }
            if ((token2 != VbLexerToken.New) && (token2 != VbLexerToken.Static))
            {
                goto Label_001F;
            }
        Label_001D:
            return true;
        Label_001F:
            return this.IsDeclarationToken(token);
        }

        protected virtual bool IsModifier(int token)
        {
            switch (((VbLexerToken) token))
            {
                case VbLexerToken.MustInherit:
                case VbLexerToken.MustOverride:
                case VbLexerToken.Friend:
                case VbLexerToken.Assembly:
                case VbLexerToken.Default:
                case VbLexerToken.NotInheritable:
                case VbLexerToken.NotOverridable:
                case VbLexerToken.Overloads:
                case VbLexerToken.Overridable:
                case VbLexerToken.Overrides:
                case VbLexerToken.Partial:
                case VbLexerToken.Private:
                case VbLexerToken.Protected:
                case VbLexerToken.Public:
                case VbLexerToken.ReadOnly:
                case VbLexerToken.Shadows:
                case VbLexerToken.Shared:
                case VbLexerToken.WithEvents:
                case VbLexerToken.WriteOnly:
                    return true;
            }
            return false;
        }

        protected virtual bool IsParameterModifier(int token)
        {
            switch (((VbLexerToken) token))
            {
                case VbLexerToken.ByRef:
                case VbLexerToken.ByVal:
                case VbLexerToken.Optional:
                case VbLexerToken.ParamArray:
                    return true;
            }
            return false;
        }

        protected virtual bool IsType(int token)
        {
            return (token == 6);
        }

        protected override bool IsValidToken(int tok)
        {
            return (((tok != 0xbd) && !this.IsComment(tok)) && (tok != 0x9e));
        }

        protected virtual bool IsXmlComment(int tok)
        {
            if (tok != 0xc9)
            {
                return (tok == 0xca);
            }
            return true;
        }

        protected virtual int LexComment()
        {
            char ch = base.source[base.currentPos];
            int length = base.source.Length;
            if (ch == '\'')
            {
                int num2 = base.currentPos + 1;
                if (((num2 < (length - 1)) && (base.source[num2] == '\'')) && (base.source[num2 + 1] == '\''))
                {
                    base.currentPos = num2 + 2;
                    if (base.currentPos < length)
                    {
                        this.State = 2;
                    }
                    return 0xc9;
                }
                base.currentPos = length;
                return 200;
            }
            int currentPos = base.currentPos;
            this.LexIdent();
            if (this.TokenString.ToLower() == "rem")
            {
                base.currentPos = length;
                return 200;
            }
            base.currentPos = currentPos;
            return this.LexIdentifier();
        }

        protected virtual int LexDefine()
        {
            base.currentPos++;
            int length = base.source.Length;
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
                if ((((ch < 'a') || (ch > 'z')) && ((ch < 'A') || (ch > 'Z'))) && (ch != '_'))
                {
                    while (base.currentPos < length)
                    {
                        ch = base.source[base.currentPos];
                        if (ch == '#')
                        {
                            base.currentPos++;
                            break;
                        }
                        base.currentPos++;
                    }
                    return 0xc5;
                }
                if (base.source.Substring(0, base.currentPos - 1).Trim() != string.Empty)
                {
                    return this.LexSymbol();
                }
                base.currentPos = currentPos;
                this.LexIdent();
            }
            return 0xcb;
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
            return 0xbf;
        }

        protected virtual int LexIdentifier()
        {
            if ((base.source[base.currentPos] == '_') && (base.currentPos == (base.source.TrimEnd(new char[0]).Length - 1)))
            {
                base.currentPos++;
                return 0x9e;
            }
            this.LexIdent();
            object obj2 = this.reswords[this.TokenString.ToLower()];
            if (obj2 == null)
            {
                return 190;
            }
            return (int) obj2;
        }

        protected virtual int LexNumber()
        {
            int num2;
            char ch = base.source[base.currentPos];
            int length = base.source.Length;
            int token = 0xbf;
            if (((ch == '+') || (ch == '-')) || (ch == '.'))
            {
                num2 = base.currentPos + 1;
                if ((num2 >= length) || (((base.source[num2] < '0') || (base.source[num2] > '9')) && (base.source[num2] != '&')))
                {
                    return this.LexSymbol();
                }
            }
            if (ch == '&')
            {
                num2 = base.currentPos + 1;
                if ((num2 < length) && ((base.source[num2] == 'h') || (base.source[num2] == 'H')))
                {
                    base.currentPos = num2 + 1;
                    return this.LexHexNumber();
                }
                if ((num2 >= length) || ((base.source[num2] != 'o') && (base.source[num2] != 'O')))
                {
                    return this.LexSymbol();
                }
                base.currentPos = num2 + 1;
                return this.LexOctNumber();
            }
            base.LexNum();
            if (base.currentPos < length)
            {
                ch = base.source[base.currentPos];
                switch (ch)
                {
                    case 'i':
                    case 'I':
                        base.currentPos++;
                        return 0xbf;

                    case 'l':
                    case 'L':
                        base.currentPos++;
                        return 0xc1;

                    case 's':
                    case 'S':
                        base.currentPos++;
                        return 0xc0;
                }
            }
            if (base.currentPos < length)
            {
                ch = base.source[base.currentPos];
                if (this.IsFloatChar(ch, ref token))
                {
                    return token;
                }
                if ((ch == '.') && (base.currentPos < (length - 1)))
                {
                    ch = base.source[base.currentPos + 1];
                    if ((ch >= '0') && (ch <= '9'))
                    {
                        base.currentPos++;
                        this.LexNum();
                        token = 0xc2;
                    }
                }
            }
            if (base.currentPos < length)
            {
                ch = base.source[base.currentPos];
                if (this.IsFloatChar(ch, ref token))
                {
                    return token;
                }
                if ((ch == 'E') || (ch == 'e'))
                {
                    num2 = base.currentPos + 1;
                    if (num2 < length)
                    {
                        ch = base.source[num2];
                        switch (ch)
                        {
                            case '+':
                            case '-':
                                num2++;
                                base.currentPos = num2;
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
                            token = 0xc3;
                        }
                    }
                }
            }
            if (base.currentPos < length)
            {
                ch = base.source[base.currentPos];
                if (this.IsFloatChar(ch, ref token))
                {
                    return token;
                }
            }
            return token;
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
            return 0xbf;
        }

        protected virtual int LexString()
        {
            char ch = base.source[base.currentPos];
            base.currentPos++;
            int length = base.source.Length;
            while (base.currentPos < length)
            {
                char ch2 = base.source[base.currentPos];
                if (ch2 == ch)
                {
                    base.currentPos++;
                    if ((base.currentPos >= length) || (base.source[base.currentPos] != ch))
                    {
                        break;
                    }
                    base.currentPos++;
                }
                else
                {
                    base.currentPos++;
                }
            }
            if ((base.currentPos < length) && (base.source[base.currentPos] == 'c'))
            {
                return 0xc6;
            }
            return 0xc7;
        }

        protected virtual int LexSymbol()
        {
            VbLexerToken star = VbLexerToken.Whitespace_Literal;
            char ch = base.source[base.currentPos];
            base.currentPos++;
            switch (ch)
            {
                case '!':
                    star = VbLexerToken.Op_single;
                    break;

                case '#':
                    star = VbLexerToken.Op_double;
                    break;

                case '$':
                    star = VbLexerToken.Op_string;
                    break;

                case '%':
                    star = VbLexerToken.Op_integer;
                    break;

                case '&':
                    if (base.CurChar() != '=')
                    {
                        star = VbLexerToken.Op_long;
                        break;
                    }
                    base.currentPos++;
                    star = VbLexerToken.String_and_assign;
                    break;

                case '(':
                    star = VbLexerToken.Open_parens;
                    break;

                case ')':
                    star = VbLexerToken.Close_parens;
                    break;

                case '*':
                    if (base.CurChar() != '=')
                    {
                        star = VbLexerToken.Star;
                        break;
                    }
                    base.currentPos++;
                    star = VbLexerToken.Op_mult_assign;
                    break;

                case '+':
                    if (base.CurChar() != '=')
                    {
                        star = VbLexerToken.Plus;
                        break;
                    }
                    base.currentPos++;
                    star = VbLexerToken.Op_add_assign;
                    break;

                case ',':
                    star = VbLexerToken.Comma;
                    break;

                case '-':
                    if (base.CurChar() != '=')
                    {
                        star = VbLexerToken.Minus;
                        break;
                    }
                    base.currentPos++;
                    star = VbLexerToken.Op_sub_assign;
                    break;

                case '.':
                    star = VbLexerToken.Dot;
                    break;

                case '/':
                    if (base.CurChar() != '=')
                    {
                        star = VbLexerToken.Div;
                        break;
                    }
                    base.currentPos++;
                    star = VbLexerToken.Op_div_assign;
                    break;

                case ':':
                    star = VbLexerToken.Colon;
                    break;

                case ';':
                    star = VbLexerToken.Semicolon;
                    break;

                case '<':
                    switch (base.CurChar())
                    {
                        case '<':
                            base.currentPos++;
                            if (base.CurChar() == '=')
                            {
                                base.currentPos++;
                                star = VbLexerToken.Op_shift_left_assign;
                            }
                            else
                            {
                                star = VbLexerToken.Op_shift_left;
                            }
                            goto Label_03C1;

                        case '=':
                            base.currentPos++;
                            star = VbLexerToken.Op_le;
                            goto Label_03C1;

                        case '>':
                            base.currentPos++;
                            star = VbLexerToken.Op_ne;
                            goto Label_03C1;
                    }
                    star = VbLexerToken.Op_lt;
                    break;

                case '=':
                    star = VbLexerToken.Assign;
                    break;

                case '>':
                    switch (base.CurChar())
                    {
                        case '=':
                            base.currentPos++;
                            star = VbLexerToken.Op_ge;
                            goto Label_03C1;

                        case '>':
                            base.currentPos++;
                            if (base.CurChar() == '=')
                            {
                                base.currentPos++;
                                star = VbLexerToken.Op_shift_right_assign;
                            }
                            else
                            {
                                star = VbLexerToken.Op_shift_right;
                            }
                            goto Label_03C1;
                    }
                    star = VbLexerToken.Op_gt;
                    break;

                case '@':
                    star = VbLexerToken.Op_decimal;
                    break;

                case '\\':
                    if (base.CurChar() != '=')
                    {
                        star = VbLexerToken.IntDiv;
                        break;
                    }
                    base.currentPos++;
                    star = VbLexerToken.Op_intdiv_assign;
                    break;

                case '^':
                    if (base.CurChar() != '=')
                    {
                        star = VbLexerToken.Carret;
                        break;
                    }
                    base.currentPos++;
                    star = VbLexerToken.Op_exp_assign;
                    break;

                case '{':
                    star = VbLexerToken.Open_brace;
                    break;

                case '}':
                    star = VbLexerToken.Close_brace;
                    break;
            }
        Label_03C1:
            return (int) star;
        }

        protected virtual int LexWhitespace()
        {
            this.LexSpace();
            return 0xbd;
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
            return 0xca;
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
            return 0xc9;
        }

        public override int NextToken()
        {
            int token = this.Token;
            if (this.IsValidToken(token) || (token == 0x9e))
            {
                this.prevToken = token;
            }
            return base.NextToken();
        }

        protected virtual bool ParseAccessor(NetNodeType accessorType)
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, string.Empty, (int) accessorType, SyntaxNodeOptions.Indentation);
            this.AddNode(node);
            this.SyntaxTree.Push(node);
            try
            {
                if ((this.Token == 0xa3) && !this.ParseAttributeListDeclaration())
                {
                    flag = false;
                }
                if (this.IsAccessor())
                {
                    VbLexerToken token = (VbLexerToken) this.Token;
                    node.Position = this.TokenPosition;
                    node.Name = this.TokenString;
                    this.MoveNext();
                    if (((token == VbLexerToken.Set) && (this.Token == 0x97)) && !this.ParseParameterListDeclaration())
                    {
                        flag = false;
                    }
                    node.AddAttribute(new SyntaxAttribute(base.prevPosition, SyntaxConsts.DeclarationScope, null));
                    if ((this.SyntaxTree.Current.NodeType != 10) && !this.ParseAccessorBody(node, (int) token))
                    {
                        flag = false;
                    }
                }
                else
                {
                    this.SyntaxError();
                    this.MoveNext();
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

        protected virtual bool ParseAccessorBody(ISyntaxNode parent, int endToken)
        {
            return this.ParseMethodBody(parent, endToken);
        }

        protected virtual bool ParseAddHandlerStatement()
        {
            return this.ParseEventHandlerStatement(110);
        }

        protected virtual bool ParseAdditiveExpression(ref ISyntaxNode node)
        {
            bool flag = this.ParseMultiplicativeExpression(ref node);
            switch (((VbLexerToken) this.Token))
            {
                case VbLexerToken.Plus:
                case VbLexerToken.Minus:
                case VbLexerToken.Op_long:
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

        protected virtual bool ParseAddressofExpression(ref ISyntaxNode node)
        {
            bool flag = true;
            node = new SyntaxNode(this.TokenPosition, this.TokenString, 0xc1);
            this.MoveNext();
            ISyntaxNode node2 = null;
            flag = this.ParseExpression(ref node2);
            if (node2 != null)
            {
                node.AddChild(node2);
            }
            node.Range.EndPoint = base.prevPosition;
            return flag;
        }

        protected virtual bool ParseAndExpression(ref ISyntaxNode node)
        {
            bool flag = this.ParseEqualityExpression(ref node);
            switch (this.Token)
            {
                case 3:
                case 4:
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
                    return flag;
                }
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
                    if (this.ParseIdentifier(out str) && (this.Token == 0xa2))
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
                    base.prevPosition = this.TokenPosition;
                }
            }
            if (node2 != null)
            {
                node.AddChild(node2);
            }
            node.Range.EndPoint = base.prevPosition;
            return flag;
        }

        protected virtual bool ParseArgumentList(ref ISyntaxNode node, bool generic)
        {
            bool flag = true;
            node = new SyntaxNode(this.TokenPosition, this.TokenString, 0x1d);
            if (this.Token != 0x97)
            {
                this.SyntaxError(0x97);
                return false;
            }
            this.MoveNext();
            if (!generic || (this.Token != 0x5e))
            {
                while (!this.Eof && (this.Token != 0x98))
                {
                    ISyntaxNode node2 = null;
                    if (this.Token == 0x9a)
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
                        if (this.Token != 0x9a)
                        {
                            break;
                        }
                        this.MoveNext();
                    }
                }
            }
            else
            {
                string str;
                this.MoveNext();
                flag = this.ParseTypeName(out str);
                node.Name = str;
                node.NodeType = 0x34;
            }
            if (!this.Expected(VbLexerToken.Close_parens))
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
            this.MoveNext();
            while (!this.Eof && (this.Token != 150))
            {
                ISyntaxNode node2 = null;
                flag = this.ParseVariableInitializer(ref node2);
                if (node2 != null)
                {
                    node.AddChild(node2);
                }
                if (this.Token != 0x9a)
                {
                    break;
                }
                this.MoveNext();
            }
            if (!this.Expected(VbLexerToken.Close_brace))
            {
                flag = false;
            }
            return flag;
        }

        protected virtual bool ParseArraySizeInitializationModifier(out string rank)
        {
            bool flag = true;
            rank = this.TokenString;
            if (!this.Expected(VbLexerToken.Open_parens))
            {
                return flag;
            }
        Label_001A:
            rank = rank + this.TokenString;
            if (this.Token != 0x98)
            {
                if (this.Token == 0x9a)
                {
                    this.MoveNext();
                    goto Label_007D;
                }
                ISyntaxNode node = null;
                if ((this.ParseExpression(ref node) && (node != null)) && (node.NodeType == 0x99))
                {
                    rank = rank + node.Name;
                    goto Label_007D;
                }
                flag = false;
            }
            goto Label_0092;
        Label_007D:
            if (!this.Eof && (this.Token != 0x98))
            {
                goto Label_001A;
            }
        Label_0092:
            if (this.Token == 0x98)
            {
                rank = rank + this.TokenString;
            }
            return this.Expected(VbLexerToken.Close_parens);
        }

        protected virtual bool ParseAssignmentExpression(ref ISyntaxNode node)
        {
            bool flag = this.Expected(VbLexerToken.Assign);
            if (flag)
            {
                node = this.CreateExpressionNode(this.TokenPosition, "=", 0x86, node, true);
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
            if (this.Token != 0x97)
            {
                goto Label_00CA;
            }
            ISyntaxNode node2 = new SyntaxNode(this.TokenPosition, string.Empty, 0x1b);
            node.AddChild(node2);
            this.SyntaxTree.Push(node2);
            try
            {
                this.MoveNext();
                while (!this.Eof && (this.Token != 0x98))
                {
                    flag = this.ParsePositionalOrNamedParam();
                    node2.Range.EndPoint = base.prevPosition;
                    if (this.Token != 0x9a)
                    {
                        goto Label_00BB;
                    }
                    this.MoveNext();
                }
            }
            finally
            {
                this.SyntaxTree.Pop();
            }
        Label_00BB:
            if (!this.Expected(VbLexerToken.Close_parens))
            {
                flag = false;
            }
        Label_00CA:
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
                if (!this.TryParseAttributeModifier())
                {
                    flag = false;
                }
                while (!this.Eof && (this.Token != 0xa4))
                {
                    if (!this.ParseAttributeDeclaration())
                    {
                        flag = false;
                    }
                    if (this.Token != 0x9a)
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
            if (!this.Expected(VbLexerToken.Op_gt))
            {
                flag = false;
            }
            node.Range.EndPoint = base.prevPosition;
            return flag;
        }

        protected virtual bool ParseBaseAccess(ref ISyntaxNode node)
        {
            return this.ParseMeAccess(ref node, 0x9d);
        }

        private bool ParseBaseList(out string types, bool isList)
        {
            bool flag = true;
            if (this.ParseType(out types))
            {
                if (isList)
                {
                    while (this.Token == 0x9a)
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
                }
                return flag;
            }
            return false;
        }

        protected override bool ParseBlock()
        {
            bool flag = this.ParseBlock(new int[] { 0x2d, 0x2b, 0x2c, 0x2e, 14, 0x58, 0x4d, 15, 0x35 });
            this.MoveNext();
            return flag;
        }

        protected virtual bool ParseBlock(int[] endTokens)
        {
            bool blockEnd = false;
            return this.ParseBlock(endTokens, true, out blockEnd);
        }

        protected virtual bool ParseBlock(int endToken)
        {
            return this.ParseBlock(new int[] { endToken });
        }

        protected virtual bool ParseBlock(int[] endTokens, bool scope, out bool blockEnd)
        {
            bool flag = true;
            blockEnd = false;
            if (scope)
            {
                this.AddAttribute(new SyntaxAttribute(base.prevPosition, SyntaxConsts.BlockScope, null));
            }
            while (!this.Eof && (Array.IndexOf<int>(endTokens, this.Token) < 0))
            {
                if (this.IsInvalidBlockToken(this.Token))
                {
                    flag = false;
                    break;
                }
                if (!this.ParseStatement())
                {
                    flag = false;
                }
                if (!this.ParseStatementTerminator())
                {
                    flag = false;
                }
            }
            blockEnd = Array.IndexOf<int>(endTokens, this.Token) >= 0;
            this.AddAttribute(new SyntaxAttribute(this.CurrentPosition, SyntaxConsts.DefinitionScopeEnd, null));
            return flag;
        }

        protected virtual bool ParseBlockStatement(int token, int endToken)
        {
            return this.ParseBlockStatement(new int[] { token }, endToken);
        }

        protected virtual bool ParseBlockStatement(int[] tokens, int endToken)
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(base.prevPosition, string.Empty, 0x6a);
            this.AddNode(node);
            this.SyntaxTree.Push(node);
            try
            {
                if (!this.ParseBlock(tokens))
                {
                    flag = false;
                }
                if (this.IsDeclarationToken(this.Token) || !this.ParseBlockStatementEnd(endToken))
                {
                    node.AddAttribute(new SyntaxAttribute(base.prevPosition, SyntaxConsts.DefinitionScopeEndExpected, "\r\n\r\n" + VbLexerToken.End.ToString() + " " + ((VbLexerToken) endToken).ToString()));
                    flag = false;
                }
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
            attr = node.FindAttribute(SyntaxConsts.DefinitionScopeEndExpected);
            if (attr != null)
            {
                this.AddAttribute(attr);
            }
            return flag;
        }

        protected virtual bool ParseBlockStatementEnd(int endToken)
        {
            if (endToken != 0xcc)
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
            node.Range.EndPoint = base.prevPosition;
            return flag;
        }

        protected virtual bool ParseCaseClause()
        {
            bool flag = true;
            if (this.Token == 0x45)
            {
                this.MoveNext();
            }
            if (this.IsComparisionOperator(this.Token))
            {
                ISyntaxNode node = null;
                flag = this.ParseComparisionExpression(ref node);
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
            if (this.Token == 0x86)
            {
                this.MoveNext();
                node2 = null;
                if (!this.ParseExpression(ref node2))
                {
                    flag = false;
                }
                if (node2 != null)
                {
                    this.AddNode(node2);
                }
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
            if (this.Token == 0x9a)
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
            this.SyntaxTree.Push(node);
            try
            {
                this.MoveNext();
                if (this.Token == 0x2b)
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
                if (!this.ParseBlock(new int[] { 14, 0x2d }))
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

        protected virtual bool ParseCastExpression(ref ISyntaxNode node)
        {
            bool flag = true;
            node = new SyntaxNode(this.TokenPosition, this.TokenString, 150);
            this.MoveNext();
            ISyntaxNode node2 = new SyntaxNode(this.TokenPosition, this.TokenString, 0x1d);
            node.AddChild(node2);
            if (this.Expected(VbLexerToken.Open_parens))
            {
                string str;
                Point prevPosition;
                ISyntaxNode node3 = new SyntaxNode(this.TokenPosition, string.Empty, 0x1c);
                node2.AddChild(node3);
                ISyntaxNode node4 = null;
                if (!this.ParseExpression(ref node4))
                {
                    flag = false;
                }
                if (node4 != null)
                {
                    node3.AddChild(node4);
                }
                node3.Range.EndPoint = base.prevPosition;
                if (!this.Expected(VbLexerToken.Comma))
                {
                    flag = false;
                }
                node3 = new SyntaxNode(this.TokenPosition, string.Empty, 0x1c);
                node2.AddChild(node3);
                if (this.ParseType(out str))
                {
                    node3.Name = str;
                    node.AddAttribute(new SyntaxAttribute(node3.Position, NetNodeType.Type.ToString(), str));
                    prevPosition = base.prevPosition;
                }
                else
                {
                    flag = false;
                    prevPosition = this.TokenPosition;
                }
                node3.Range.EndPoint = prevPosition;
                if (this.Expected(VbLexerToken.Close_parens))
                {
                    prevPosition = base.prevPosition;
                }
                else
                {
                    flag = false;
                }
                node2.Range.EndPoint = prevPosition;
            }
            else
            {
                flag = false;
                node2.Range.EndPoint = base.prevPosition;
            }
            node.Range.EndPoint = node2.Range.EndPoint;
            return flag;
        }

        protected virtual bool ParseCastTargetExpression(ref ISyntaxNode node, VbLexerToken token)
        {
            bool flag = true;
            node = new SyntaxNode(this.TokenPosition, this.TokenString, 0x97);
            node.AddAttribute(new SyntaxAttribute(this.TokenPosition, NetNodeType.Type.ToString(), token.ToString()));
            this.MoveNext();
            if (this.Expected(VbLexerToken.Open_parens))
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
                if (!this.Expected(VbLexerToken.Close_parens))
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

        protected virtual bool ParseCatchStatement()
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, this.TokenString, 0x61, (this.SyntaxTree.Current.NodeType != 0x60) ? SyntaxNodeOptions.Indentation : SyntaxNodeOptions.BackIndentation);
            this.AddNode(node);
            this.SyntaxTree.Push(node);
            try
            {
                this.MoveNext();
                if (this.IsIdentifierToken(this.Token))
                {
                    bool asType = false;
                    if (!this.ParseSimpleVariable(ref asType))
                    {
                        flag = false;
                    }
                }
                if (this.Token == 0x8f)
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
                if (!this.ParseStatementTerminator())
                {
                    flag = false;
                }
                if (!this.ParseBlock(new int[] { 15, 0x35, 0x2d }))
                {
                    flag = false;
                }
            }
            finally
            {
                this.SyntaxTree.Pop();
            }
            node.Range.EndPoint = this.TokenPosition;
            return flag;
        }

        protected virtual bool ParseClassBody()
        {
            bool flag = true;
            bool flag2 = this.SyntaxTree.Current != this.SyntaxTree.Root;
            ISyntaxAttributes attrs = null;
            while (!this.Eof)
            {
                if (flag2 && this.ParseEndBlock(this.Token))
                {
                    return flag;
                }
                this.ParseModifiers(ref attrs);
                if (this.IsKnownMemberDeclaration(this.Token))
                {
                    if (!this.ParseKnownMemberDeclaration(attrs))
                    {
                        flag = false;
                    }
                    attrs = null;
                }
                else
                {
                    VbLexerToken token = (VbLexerToken) this.Token;
                    if (token != VbLexerToken.Op_lt)
                    {
                        if (token != VbLexerToken.Directive_Literal)
                        {
                            goto Label_0088;
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
                }
                continue;
            Label_0088:
                if (!this.ParseVariableMemberDeclaration(attrs, this.TokenPosition, 13))
                {
                    flag = false;
                }
                attrs = null;
            }
            return flag;
        }

        protected override bool ParseComment()
        {
            bool flag = this.Token == 0xc9;
            ISyntaxNode item = new SyntaxNode(this.TokenPosition, SyntaxParserConsts.OutlineCommentText, flag ? 0x39 : 0x33, SyntaxNodeOptions.None);
            item.AddAttribute(new SyntaxAttribute(item.Position, SyntaxConsts.OutlineText, item.Name));
            base.comments.Add(item);
            Point currentPosition = this.CurrentPosition;
            while (((flag && this.IsXmlComment(this.Token)) || (!flag && (this.Token == 200))) || (this.Token == 0xbd))
            {
                if (this.Token != 0xbd)
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

        protected virtual bool ParseComparisionExpression(ref ISyntaxNode node)
        {
            bool flag = this.IsComparisionOperator(this.Token);
            if (flag)
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
            }
            return flag;
        }

        protected virtual bool ParseConstantExpression(ref ISyntaxNode node)
        {
            return this.ParseExpression(ref node);
        }

        protected virtual bool ParseConstructorDeclaration(ISyntaxAttributes attrs)
        {
            return this.ParseConstructorDeclaration(attrs, this.TokenPosition);
        }

        protected virtual bool ParseConstructorDeclaration(ISyntaxAttributes attrs, Point pos)
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(((attrs != null) && (attrs.Count > 0)) ? attrs[0].Position : pos, VbLexerToken.New.ToString(), 0x12, SyntaxNodeOptions.CodeCompletion);
            if (attrs != null)
            {
                node.AddAttributes(attrs);
            }
            this.MoveNext();
            this.AddNode(node);
            if (!this.BeforeDeclaration(node))
            {
                flag = false;
            }
            this.SyntaxTree.Push(node);
            try
            {
                if ((this.Token == 0x97) && !this.ParseParameterListDeclaration())
                {
                    flag = false;
                }
                node.AddAttribute(new SyntaxAttribute(base.prevPosition, SyntaxConsts.DeclarationScope, null));
                if (!this.ParseMethodBody(node, 130))
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

        protected virtual bool ParseContinueStatement()
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, this.TokenString, 0x5c);
            this.AddNode(node);
            this.MoveNext();
            if (((this.Token == 40) || (this.Token == 0x36)) || (this.Token == 0x90))
            {
                this.MoveNext();
            }
            else
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
            VbLexerToken token = (VbLexerToken) this.Token;
            if (token <= VbLexerToken.Interface)
            {
                switch (token)
                {
                    case VbLexerToken.Class:
                    case VbLexerToken.Enum:
                    case VbLexerToken.Interface:
                        goto Label_003A;
                }
                goto Label_0049;
            }
            if (((token != VbLexerToken.Module) && (token != VbLexerToken.Namespace)) && (token != VbLexerToken.Structure))
            {
                goto Label_0049;
            }
        Label_003A:
            return this.ParseDeclaration(attrs, node, node.NodeType);
        Label_0049:
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
                Point prevPosition = base.prevPosition;
                if (nodeType == 11)
                {
                    if (!this.ParseEnumDeclaration(node))
                    {
                        flag = false;
                    }
                    prevPosition = base.prevPosition;
                }
                else if ((nodeType != 7) && (!this.ParseStatementTerminator() || !this.ParseInheritsOrImplementsClause(node)))
                {
                    flag = false;
                }
                prevPosition = (this.prevPosition.Y == prevPosition.Y) ? base.prevPosition : prevPosition;
                node.AddAttribute(new SyntaxAttribute(prevPosition, SyntaxConsts.DeclarationScope, null));
                this.SyntaxTree.Push(node);
                try
                {
                    if (this.Token == 0x97)
                    {
                        this.MoveNext();
                        if (!this.ParseTypeParameterList() || !this.Expected(VbLexerToken.Close_parens))
                        {
                            flag = false;
                        }
                    }
                    node.Options |= SyntaxNodeOptions.Outlining | SyntaxNodeOptions.Indentation;
                    if (nodeType == 11)
                    {
                        if (!this.ParseEnumBody())
                        {
                            flag = false;
                        }
                    }
                    else if (!this.ParseClassBody())
                    {
                        flag = false;
                    }
                }
                finally
                {
                    this.SyntaxTree.Pop();
                }
                if (this.ParseDeclarationEnd(nodeType))
                {
                    node.AddAttribute(new SyntaxAttribute(base.prevPosition, SyntaxConsts.DefinitionScopeEnd, null));
                }
                else
                {
                    string str2 = "\r\n\r\n" + VbLexerToken.End.ToString() + " ";
                    if (nodeType == 9)
                    {
                        str2 = str2 + SyntaxParserConsts.StructureResWord;
                    }
                    else
                    {
                        str2 = str2 + ((NetNodeType) nodeType).ToString();
                    }
                    node.AddAttribute(new SyntaxAttribute(base.prevPosition, SyntaxConsts.DefinitionScopeEndExpected, str2));
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

        protected virtual bool ParseDeclarationEnd(int nodeType)
        {
            return this.Expected(this.GetBlockToken(nodeType));
        }

        protected virtual bool ParseDelegateDeclaration(ISyntaxAttributes attrs)
        {
            string str;
            bool flag = true;
            int endToken = 130;
            ISyntaxNode node = new SyntaxNode(((attrs != null) && (attrs.Count > 0)) ? attrs[0].Position : this.TokenPosition, string.Empty, 20, SyntaxNodeOptions.CodeCompletion);
            if (attrs != null)
            {
                node.AddAttributes(attrs);
            }
            this.MoveNext();
            if ((this.Token == 0x38) || (this.Token == 130))
            {
                endToken = this.Token;
                this.MoveNext();
            }
            else
            {
                flag = false;
            }
            if (this.ParseIdentifier(out str))
            {
                node.Name = str;
            }
            this.AddNode(node);
            return this.ParseMethodDeclaration(node, endToken, false);
        }

        protected virtual bool ParseDictionaryAccess(ref ISyntaxNode node)
        {
            return this.ParseMemberAccess(ref node, 0xa2);
        }

        protected virtual bool ParseDimDeclaration(ISyntaxAttributes attrs, int nodeType)
        {
            Point tokenPosition = this.TokenPosition;
            bool flag = this.Token == 0x7d;
            this.MoveNext();
            if (flag && (this.Token == 0x26))
            {
                this.MoveNext();
            }
            return this.ParseVariableMemberDeclaration(attrs, tokenPosition, nodeType);
        }

        protected virtual bool ParseDirective()
        {
            bool flag = true;
            string tokenString = this.TokenString;
            if (tokenString.StartsWith("#"))
            {
                tokenString = tokenString.Remove(0, 1).Trim();
            }
            int lineIndex = base.lineIndex;
            switch (tokenString.ToLower())
            {
                case "region":
                {
                    ISyntaxNode node = new SyntaxNode(this.TokenPosition, this.TokenString, 40);
                    this.MoveNext();
                    if ((lineIndex == base.lineIndex) && (this.Token == 0xc7))
                    {
                        node.Name = this.TokenString.TrimEnd(new char[] { '"' }).TrimStart(new char[] { '"' }).Trim();
                    }
                    this.AddNode(node);
                    this.SyntaxTree.Push(node);
                    while (!this.Eof && (lineIndex == base.lineIndex))
                    {
                        this.MoveNext();
                    }
                    return flag;
                }
                case "end":
                {
                    Point point2 = new Point(base.source.Length, base.lineIndex);
                    this.MoveNext();
                    if (this.Token == 0x72)
                    {
                        this.MoveNext();
                    }
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
            while (!this.Eof && (lineIndex == base.lineIndex))
            {
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
                bool parsed = false;
                if (!this.TryParseWhileOrUntil(out parsed))
                {
                    flag = false;
                }
                if (!this.ParseStatementTerminator())
                {
                    flag = false;
                }
                bool blockEnd = false;
                if (!this.ParseBlock(new int[] { 0x4d }, true, out blockEnd))
                {
                    if (parsed && !blockEnd)
                    {
                        node.AddAttribute(new SyntaxAttribute(base.prevPosition, SyntaxConsts.DefinitionScopeEndExpected, "\r\n\r\n" + VbLexerToken.Loop.ToString()));
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
            node.Range.EndPoint = base.prevPosition;
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
                if (this.Token == 0x84)
                {
                    this.MoveNext();
                }
                else
                {
                    node.AddAttribute(new SyntaxAttribute(base.prevPosition, SyntaxConsts.DefinitionScopeEndExpected, ' ' + VbLexerToken.Then.ToString()));
                }
                if (!this.ParseStatementTerminator())
                {
                    flag = false;
                }
                if (!this.ParseBlock(new int[] { 0x2b, 0x2c, 0x2d, 0x2e }))
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

        protected virtual bool ParseElseStatement(bool block)
        {
            bool flag = true;
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
                    if (!this.ParseBlock(new int[] { 0x2d, 0x2e }))
                    {
                        flag = false;
                    }
                }
                else if (!this.ParseStatement())
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

        protected virtual bool ParseEndBlock(int token)
        {
            if (this.Token == 0x2d)
            {
                this.MoveNext();
                return true;
            }
            return false;
        }

        protected virtual bool ParseEndStatement()
        {
            return this.ParseStopOrEndStatement(0x73);
        }

        protected virtual bool ParseEnumBody()
        {
            bool flag = true;
            while (!this.Eof)
            {
                if (this.Token == 0x2d)
                {
                    this.MoveNext();
                    return flag;
                }
                if (!this.ParseEnumMember())
                {
                    flag = false;
                }
            }
            return flag;
        }

        protected virtual bool ParseEnumDeclaration(ISyntaxNode node)
        {
            string str;
            bool flag = true;
            if (!this.IsType(this.Token))
            {
                return flag;
            }
            Point tokenPosition = this.TokenPosition;
            this.MoveNext();
            if (this.ParseType(out str))
            {
                Point prevPosition = base.prevPosition;
                node.AddAttribute(new SyntaxAttribute(tokenPosition, NetNodeType.Type.ToString(), str));
                return flag;
            }
            return false;
        }

        protected virtual bool ParseEnumMember()
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, string.Empty, 13, SyntaxNodeOptions.CodeCompletion);
            this.AddNode(node);
            this.SyntaxTree.Push(node);
            try
            {
                string str;
                if ((this.Token == 0xa3) && !this.ParseAttributeListDeclaration())
                {
                    flag = false;
                }
                if (this.ParseIdentifier(out str))
                {
                    node.Name = str;
                }
                else
                {
                    this.MoveNext();
                    flag = false;
                }
                if (this.Token == 0xa2)
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
            }
            finally
            {
                this.SyntaxTree.Pop();
            }
            node.Range.EndPoint = base.prevPosition;
            return flag;
        }

        protected virtual bool ParseEqualityExpression(ref ISyntaxNode node)
        {
            bool flag = this.ParseRelationalExpression(ref node);
            VbLexerToken token = (VbLexerToken) this.Token;
            if (token <= VbLexerToken.Like)
            {
                switch (token)
                {
                    case VbLexerToken.Is:
                    case VbLexerToken.IsNot:
                    case VbLexerToken.Like:
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
                        node.Range.EndPoint = base.prevPosition;
                        return flag;
                    }
                }
                return flag;
            }
            switch (token)
            {
                case VbLexerToken.Assign:
                case VbLexerToken.Op_ne:
                {
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
                    node.Range.EndPoint = base.prevPosition;
                    break;
                }
            }
            return flag;
        }

        protected virtual bool ParseEraseStatement()
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, this.TokenString, 0x74, SyntaxNodeOptions.Indentation);
            this.AddNode(node);
            this.MoveNext();
            ISyntaxNode node2 = null;
            if (!this.ParseExpressionList(ref node2))
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
            node.Range.EndPoint = base.prevPosition;
            return flag;
        }

        protected virtual bool ParseErrorStatement()
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, this.TokenString, 0x75, SyntaxNodeOptions.Indentation);
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
            node.Range.EndPoint = base.prevPosition;
            return flag;
        }

        protected virtual bool ParseEventDeclaration(ISyntaxAttributes attrs)
        {
            string str;
            bool flag = true;
            this.MoveNext();
            Point tokenPosition = this.TokenPosition;
            if (this.ParseIdentifier(out str))
            {
                ISyntaxNode node = new SyntaxNode(((attrs != null) && (attrs.Count > 0)) ? attrs[0].Position : tokenPosition, str, 0x16, SyntaxNodeOptions.CodeCompletion);
                if (attrs != null)
                {
                    node.AddAttributes(attrs);
                }
                this.AddNode(node);
                if (this.IsType(this.Token))
                {
                    string str2;
                    this.MoveNext();
                    Point position = this.TokenPosition;
                    flag = this.ParseType(out str2);
                    node.AddAttribute(new SyntaxAttribute(position, NetNodeType.Type.ToString(), str2));
                }
                else if (this.Token == 0x97)
                {
                    flag = this.ParseParameterListDeclaration();
                }
                else
                {
                    flag = false;
                }
                if (this.Token == 0x3f)
                {
                    string str4;
                    string tokenString = this.TokenString;
                    tokenPosition = this.TokenPosition;
                    if (this.ParseImplements(out str4))
                    {
                        node.AddAttribute(new SyntaxAttribute(tokenPosition, tokenString, str4));
                    }
                }
                node.Range.EndPoint = base.prevPosition;
                return flag;
            }
            return false;
        }

        protected virtual bool ParseEventHandlerStatement(int nodeType)
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, this.TokenString, nodeType);
            this.AddNode(node);
            this.MoveNext();
            ISyntaxNode node2 = null;
            ISyntaxNode node3 = null;
            if ((!this.ParseExpression(ref node2) || !this.Expected(VbLexerToken.Comma)) || !this.ParseExpression(ref node3))
            {
                flag = false;
            }
            if (node2 != null)
            {
                node.AddChild(node2);
            }
            if (node3 != null)
            {
                node.AddChild(node3);
            }
            if (!this.ParseStatementTerminator())
            {
                flag = false;
            }
            node.Range.EndPoint = base.prevPosition;
            return flag;
        }

        protected virtual bool ParseExclusiveOrExpression(ref ISyntaxNode node)
        {
            bool flag = this.ParseAndExpression(ref node);
            if (this.Token == 0x94)
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

        protected virtual bool ParseExitStatement()
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, this.TokenString, 0x76, SyntaxNodeOptions.Indentation);
            this.AddNode(node);
            this.MoveNext();
            VbLexerToken token = (VbLexerToken) this.Token;
            if (token <= VbLexerToken.Property)
            {
                switch (token)
                {
                    case VbLexerToken.For:
                    case VbLexerToken.Function:
                    case VbLexerToken.Property:
                    case VbLexerToken.Do:
                        goto Label_0079;
                }
                goto Label_0082;
            }
            if (token <= VbLexerToken.Sub)
            {
                switch (token)
                {
                    case VbLexerToken.Select:
                    case VbLexerToken.Sub:
                        goto Label_0079;
                }
                goto Label_0082;
            }
            if ((token != VbLexerToken.Try) && (token != VbLexerToken.While))
            {
                goto Label_0082;
            }
        Label_0079:
            this.MoveNext();
            goto Label_008A;
        Label_0082:
            flag = false;
            this.SyntaxError();
        Label_008A:
            if (!this.ParseStatementTerminator())
            {
                flag = false;
            }
            node.Range.EndPoint = base.prevPosition;
            return flag;
        }

        protected virtual bool ParseExpression(ref ISyntaxNode node)
        {
            bool flag = true;
            this.StartExpression();
            try
            {
                flag = this.ParseInclusiveOrExpression(ref node);
                if (!this.TryParseAssignmentExpression(ref node))
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
            return this.ParseExpressionBlockStatement(new int[] { 0x2d }, endToken, nodeType, true);
        }

        protected virtual bool ParseExpressionBlockStatement(int[] tokens, int endToken, int nodeType, bool scope)
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
                bool flag2 = true;
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
                    flag2 = false;
                    flag = false;
                }
                bool blockEnd = false;
                flag = this.ParseBlock(tokens, scope, out blockEnd);
                if (!this.ParseExpressionBlockStatementEnd(endToken))
                {
                    if (flag2 && (endToken != 0xcc))
                    {
                        node.AddAttribute(new SyntaxAttribute(base.prevPosition, SyntaxConsts.DefinitionScopeEndExpected, "\r\n\r\n" + VbLexerToken.End.ToString() + " " + ((VbLexerToken) endToken).ToString()));
                    }
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

        protected virtual bool ParseExpressionBlockStatementEnd(int endToken)
        {
            this.MoveNext();
            return (this.Expected(endToken) && this.ParseStatementTerminator());
        }

        protected virtual bool ParseExpressionList(ref ISyntaxNode node)
        {
            node = new SyntaxNode(this.TokenPosition, string.Empty, 0x83);
            ISyntaxNode node2 = null;
            bool flag = this.ParseExpression(ref node2);
            if (node2 != null)
            {
                node.AddChild(node2);
            }
            while (!this.Eof && (this.Token == 0x9a))
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
            node.Range.EndPoint = base.prevPosition;
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
            node.Range.EndPoint = base.prevPosition;
            return flag;
        }

        protected virtual bool ParseFieldDeclaration(ISyntaxAttributes attrs, Point pos, string name, ISyntaxNodes names, int nodeType, bool isArray)
        {
            bool flag = true;
            pos = ((attrs != null) && (attrs.Count > 0)) ? attrs[0].Position : pos;
            ISyntaxNode node = new SyntaxNode(pos, name, nodeType, SyntaxNodeOptions.CodeCompletion | SyntaxNodeOptions.Indentation);
            if (isArray)
            {
                node.AddAttribute(new SyntaxAttribute(this.TokenPosition, NetNodeType.ArraySpecifier.ToString(), null));
            }
            if (names != null)
            {
                node.AddChildren(names);
            }
            if (attrs != null)
            {
                node.AddAttributes(attrs);
            }
            if (this.IsType(this.Token))
            {
                node.AddAttribute(new SyntaxAttribute(this.TokenPosition, NetNodeType.TypeModifier.ToString(), this.TokenString));
                this.MoveNext();
                Point empty = Point.Empty;
                string type = string.Empty;
                if (this.Token == 0x57)
                {
                    ISyntaxNode node2 = null;
                    if (!this.ParseVariableInitializer(ref node2))
                    {
                        flag = false;
                    }
                    if (node2 != null)
                    {
                        node.AddChild(node2);
                    }
                    empty = base.prevPosition;
                    ISyntaxAttribute attribute = node2.FindAttribute(NetNodeType.Type.ToString());
                    if (attribute != null)
                    {
                        type = attribute.Value.ToString();
                    }
                }
                else
                {
                    empty = this.TokenPosition;
                    if (!this.ParseTypeName(out type))
                    {
                        flag = false;
                    }
                }
                if (type != string.Empty)
                {
                    node.AddAttribute(new SyntaxAttribute(empty, NetNodeType.Type.ToString(), type));
                    if (this.Token == 0x97)
                    {
                        ISyntaxNode node3 = null;
                        if (!this.ParseArgumentList(ref node3, true))
                        {
                            flag = false;
                        }
                        if (node3 != null)
                        {
                            node.AddChild(node3);
                        }
                    }
                }
                else
                {
                    flag = false;
                    base.prevPosition = this.TokenPosition;
                }
            }
            this.AddNode(node);
            if (!this.BeforeDeclaration(node))
            {
                flag = false;
            }
            if (this.Token == 0xa2)
            {
                ISyntaxNode node4 = new SyntaxNode(this.TokenPosition, this.TokenString, 0x21);
                node.AddChild(node4);
                this.MoveNext();
                ISyntaxNode node5 = null;
                if (!this.ParseVariableInitializer(ref node5))
                {
                    flag = false;
                }
                if (node5 != null)
                {
                    node4.AddChild(node5);
                }
                node4.Range.EndPoint = base.prevPosition;
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
                if (!this.ParseStatementTerminator())
                {
                    flag = false;
                }
                if (!this.ParseBlock(0x2d))
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

        protected virtual bool ParseForEachStatement(ISyntaxNode node)
        {
            bool flag = true;
            node.NodeType = 0x4e;
            this.MoveNext();
            int y = this.TokenPosition.Y;
            int num2 = node.Position.Y;
            if (!this.ParseLoopControlVariable())
            {
                flag = false;
            }
            if (this.Token == 0x41)
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
            if (!this.ParseBlock(new int[] { 0x58, 0xcc }))
            {
                flag = false;
                if (flag2)
                {
                    node.AddAttribute(new SyntaxAttribute(base.prevPosition, SyntaxConsts.DefinitionScopeEndExpected, "\r\n\r\n" + VbLexerToken.Next.ToString()));
                    base.prevPosition = this.TokenPosition;
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
            node.Range.EndPoint = base.prevPosition;
            return flag;
        }

        protected virtual bool ParseForIterator()
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, this.TokenString, 0x4d);
            this.AddNode(node);
            ISyntaxNode node2 = null;
            flag = this.Expected(VbLexerToken.To) && this.ParseExpression(ref node2);
            if (node2 != null)
            {
                node.AddChild(node2);
            }
            if (this.Token == 0x7e)
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
                if (this.Token == 0x2a)
                {
                    flag = this.ParseForEachStatement(node);
                }
                else
                {
                    int y = this.TokenPosition.Y;
                    int num2 = node.Position.Y;
                    if (!this.ParseLoopControlVariable())
                    {
                        flag = false;
                    }
                    ISyntaxNode node2 = null;
                    if ((this.Token == 0xa2) && !this.ParseAssignmentExpression(ref node2))
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
                    if (!this.ParseBlock(new int[] { 0x58, 0xcc }))
                    {
                        flag = false;
                        if (flag2)
                        {
                            node.AddAttribute(new SyntaxAttribute(base.prevPosition, SyntaxConsts.DefinitionScopeEndExpected, "\r\n\r\n" + VbLexerToken.Next.ToString()));
                            base.prevPosition = this.TokenPosition;
                        }
                    }
                    else
                    {
                        int lineIndex = base.lineIndex;
                        this.MoveNext();
                        if (!this.ParseStatementTerminator(lineIndex))
                        {
                            node2 = null;
                            if (!this.ParseExpressionList(ref node2))
                            {
                                flag = false;
                            }
                            if (node2 != null)
                            {
                                node.AddChild(node2);
                            }
                        }
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
            node.Range.EndPoint = base.prevPosition;
            return flag;
        }

        protected virtual bool ParseGetTypeExpression(ref ISyntaxNode node)
        {
            bool flag = true;
            node = new SyntaxNode(this.TokenPosition, this.TokenString, 0xc2);
            this.MoveNext();
            if (this.Expected(VbLexerToken.Open_parens))
            {
                string str;
                Point tokenPosition = this.TokenPosition;
                if (this.ParseType(out str))
                {
                    node.AddAttribute(new SyntaxAttribute(tokenPosition, NetNodeType.Type.ToString(), str));
                }
                else
                {
                    flag = false;
                }
                if (!this.Expected(VbLexerToken.Close_parens))
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
            node.Range.EndPoint = base.prevPosition;
            return flag;
        }

        protected virtual bool ParseHandlesClause()
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, this.TokenString, 0x30);
            this.AddNode(node);
            this.SyntaxTree.Push(node);
            try
            {
                this.MoveNext();
                do
                {
                    string str2;
                    if (this.Token == 0x9a)
                    {
                        this.MoveNext();
                    }
                    Point tokenPosition = this.TokenPosition;
                    string tokenString = string.Empty;
                    if (this.Token == 0x54)
                    {
                        tokenString = this.TokenString;
                        this.MoveNext();
                        if (this.Expected(VbLexerToken.Dot))
                        {
                            tokenString = tokenString + '.';
                        }
                        else
                        {
                            flag = false;
                        }
                    }
                    if (this.ParseQualifiedIdentifier(out str2))
                    {
                        tokenString = tokenString + str2;
                    }
                    else
                    {
                        flag = false;
                    }
                    node.AddAttribute(new SyntaxAttribute(tokenPosition, NetNodeType.Name.ToString(), tokenString));
                }
                while (this.Token == 0x9a);
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
            if (!this.IsIdentifierToken(this.Token))
            {
                return this.IdentifierExpected();
            }
            char ch = base.CurChar();
            switch (ch)
            {
                case '!':
                case '#':
                case '$':
                case '%':
                case '&':
                case '@':
                {
                    bool flag = false;
                    this.MoveNext();
                    this.SaveState();
                    try
                    {
                        flag = this.TokenString == ch.ToString();
                        if (flag)
                        {
                            this.MoveNext();
                        }
                        goto Label_0085;
                    }
                    finally
                    {
                        this.RestoreState(!flag);
                    }
                    break;
                }
            }
            this.MoveNext();
        Label_0085:
            return true;
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
                if (this.Token == 0x84)
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
                        attr = new SyntaxAttribute(base.prevPosition, SyntaxConsts.DefinitionScopeEndExpected, ' ' + VbLexerToken.Then.ToString());
                        node.AddAttribute(attr);
                    }
                }
                if (block)
                {
                    if (!this.ParseBlock(new int[] { 0x2b, 0x2c, 0x2d, 0x2e }))
                    {
                        flag = false;
                    }
                }
                else if (!this.ParseStatement())
                {
                    flag = false;
                }
                if (block)
                {
                    while (!this.Eof && ((this.Token == 0x2c) || (this.Token == 0x2b)))
                    {
                        bool flag4 = this.Token == 0x2c;
                        if (!flag4)
                        {
                            this.SaveState();
                            try
                            {
                                lineIndex = base.lineIndex;
                                flag4 = (this.MoveNext() == 0x3e) && (base.lineIndex == lineIndex);
                            }
                            finally
                            {
                                this.RestoreState(!flag4);
                            }
                        }
                        if (flag4)
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
                    if (this.Token == 0x2e)
                    {
                        this.MoveNext();
                    }
                    else if (!this.Expected(VbLexerToken.End) || !this.Expected(VbLexerToken.If))
                    {
                        if (flag3)
                        {
                            string str = "\r\n\r\n" + VbLexerToken.End.ToString() + " " + VbLexerToken.If.ToString();
                            if (attr != null)
                            {
                                attr.Value = attr.Value.ToString() + str;
                            }
                            else
                            {
                                node.AddAttribute(new SyntaxAttribute(base.prevPosition, SyntaxConsts.DefinitionScopeEndExpected, str));
                            }
                        }
                        flag = false;
                    }
                    ISyntaxAttribute attribute2 = node.FindAttribute(SyntaxConsts.DefinitionScopeEnd);
                    if (attribute2 != null)
                    {
                        attribute2.Position = base.prevPosition;
                    }
                }
                else if ((this.Token == 0x2b) && !this.ParseElseStatement(block))
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
            node.Range.EndPoint = base.prevPosition;
            return flag;
        }

        protected virtual bool ParseImplements(out string types)
        {
            return this.ParseBaseList(out types, true);
        }

        protected virtual bool ParseImplementsClause()
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, this.TokenString, 0x31);
            this.AddNode(node);
            this.SyntaxTree.Push(node);
            try
            {
                do
                {
                    string str;
                    this.MoveNext();
                    Point tokenPosition = this.TokenPosition;
                    if (this.ParseType(out str))
                    {
                        node.AddAttribute(new SyntaxAttribute(tokenPosition, NetNodeType.Type.ToString(), str));
                    }
                    else
                    {
                        flag = false;
                    }
                }
                while (this.Token == 0x9a);
            }
            finally
            {
                this.SyntaxTree.Pop();
            }
            node.Range.EndPoint = base.prevPosition;
            return flag;
        }

        protected virtual bool ParseImportDeclaration()
        {
            return this.ParseImportDeclaration(new SyntaxNode());
        }

        protected virtual bool ParseImportDeclaration(ISyntaxNode node)
        {
            string str;
            bool flag = true;
            Point tokenPosition = this.TokenPosition;
            if (this.ParseQualifiedIdentifier(out str) || (str != string.Empty))
            {
                node.Position = tokenPosition;
                node.Name = str;
                node.NodeType = 3;
                this.AddNode(node);
                if (this.Token == 0xa2)
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
                node.Range.EndPoint = base.prevPosition;
            }
            return flag;
        }

        protected virtual bool ParseImportsList()
        {
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, string.Empty, 2, SyntaxNodeOptions.Outlining);
            this.AddNode(node);
            return this.ParseImportsList(node);
        }

        protected virtual bool ParseImportsList(ISyntaxNode node)
        {
            bool flag = true;
            node.AddAttribute(new SyntaxAttribute(this.TokenPosition, SyntaxConsts.OutlineText, SyntaxParserConsts.OutlineImportsText));
            if (!this.BeforeDeclaration(node))
            {
                flag = false;
            }
            this.SyntaxTree.Push(node);
            try
            {
                int lineIndex = base.lineIndex;
                for (bool flag2 = true; (this.Token == 0x40) || flag2; flag2 = (this.Token == 0x9a) && (lineIndex == base.lineIndex))
                {
                    this.MoveNext();
                    if (!this.ParseImportDeclaration())
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

        protected virtual bool ParseInclusiveOrExpression(ref ISyntaxNode node)
        {
            bool flag = this.ParseExclusiveOrExpression(ref node);
            switch (this.Token)
            {
                case 0x63:
                case 100:
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
                    return flag;
                }
            }
            return flag;
        }

        protected virtual bool ParseInherits(out string types, bool isList)
        {
            return this.ParseBaseList(out types, isList);
        }

        protected bool ParseInheritsOrImplementsClause(ISyntaxNode node)
        {
            bool flag = true;
            if (this.IsBaseList(this.Token))
            {
                Point tokenPosition;
                string str;
                ISyntaxAttribute attr = null;
                if (this.Token == 0x42)
                {
                    this.MoveNext();
                    tokenPosition = this.TokenPosition;
                    if (this.ParseInherits(out str, node.NodeType == 10))
                    {
                        attr = new SyntaxAttribute(tokenPosition, NetNodeType.TypeList.ToString(), str);
                    }
                    else
                    {
                        flag = false;
                    }
                }
                if (this.Token == 0x3f)
                {
                    this.MoveNext();
                    tokenPosition = this.TokenPosition;
                    if (this.ParseImplements(out str))
                    {
                        if (attr == null)
                        {
                            attr = new SyntaxAttribute(tokenPosition, NetNodeType.TypeList.ToString(), str);
                        }
                        else
                        {
                            attr.Value = ((string) attr.Value) + "," + str;
                        }
                    }
                    else
                    {
                        flag = false;
                    }
                }
                if (attr != null)
                {
                    node.AddAttribute(attr);
                }
            }
            return flag;
        }

        protected virtual bool ParseInvocationExpression(ref ISyntaxNode node)
        {
            bool flag = true;
            node = this.CreateExpressionNode(this.TokenPosition, this.TokenString, 0xa3, node, false);
            ISyntaxNode node2 = null;
            flag = this.ParseArgumentList(ref node2, false);
            if (node2 != null)
            {
                node.AddChild(node2);
            }
            node.Range.EndPoint = base.prevPosition;
            return flag;
        }

        protected virtual bool ParseKnownMemberDeclaration(ISyntaxAttributes attrs)
        {
            bool flag = true;
            VbLexerToken token = (VbLexerToken) this.Token;
            if (token <= VbLexerToken.Imports)
            {
                if (token <= VbLexerToken.Dim)
                {
                    switch (token)
                    {
                        case VbLexerToken.Delegate:
                            if (!this.ParseDelegateDeclaration(attrs))
                            {
                                flag = false;
                            }
                            return flag;

                        case VbLexerToken.Dim:
                        case VbLexerToken.Const:
                            goto Label_01A6;

                        case VbLexerToken.Class:
                            if (!this.ParseDeclaration(attrs, 8))
                            {
                                flag = false;
                            }
                            return flag;
                    }
                    return flag;
                }
                if (token <= VbLexerToken.Event)
                {
                    switch (token)
                    {
                        case VbLexerToken.Enum:
                            if (!this.ParseDeclaration(attrs, 11))
                            {
                                flag = false;
                            }
                            return flag;

                        case VbLexerToken.Event:
                            if (!this.ParseEventDeclaration(attrs))
                            {
                                flag = false;
                            }
                            return flag;
                    }
                    return flag;
                }
                switch (token)
                {
                    case VbLexerToken.Function:
                        goto Label_0165;

                    case VbLexerToken.Imports:
                        if (!this.ParseImportsList())
                        {
                            flag = false;
                        }
                        return flag;
                }
                return flag;
            }
            if (token <= VbLexerToken.New)
            {
                switch (token)
                {
                    case VbLexerToken.Namespace:
                        if (!this.ParseDeclaration(attrs, 7))
                        {
                            flag = false;
                        }
                        return flag;

                    case VbLexerToken.New:
                        if (!this.ParseConstructorDeclaration(attrs))
                        {
                            flag = false;
                        }
                        return flag;

                    case VbLexerToken.Module:
                        if (!this.ParseDeclaration(attrs, 12))
                        {
                            flag = false;
                        }
                        return flag;

                    case VbLexerToken.Interface:
                        if (!this.ParseDeclaration(attrs, 10))
                        {
                            flag = false;
                        }
                        return flag;
                }
                return flag;
            }
            if (token <= VbLexerToken.Property)
            {
                switch (token)
                {
                    case VbLexerToken.Operator:
                        if (!this.ParseOperatorDeclaration(attrs))
                        {
                            flag = false;
                        }
                        return flag;

                    case VbLexerToken.Option:
                        if (!this.ParseOptionStatement())
                        {
                            flag = false;
                        }
                        return flag;

                    case VbLexerToken.Property:
                        if (!this.ParsePropertyDeclaration(attrs))
                        {
                            flag = false;
                        }
                        return flag;
                }
                return flag;
            }
            switch (token)
            {
                case VbLexerToken.Structure:
                    if (!this.ParseDeclaration(attrs, 9))
                    {
                        flag = false;
                    }
                    return flag;

                case VbLexerToken.Sub:
                    break;

                case VbLexerToken.Static:
                    goto Label_01A6;

                default:
                    return flag;
            }
        Label_0165:
            if (!this.ParseMethodDeclaration(attrs))
            {
                flag = false;
            }
            return flag;
        Label_01A6:
            if (!this.ParseDimDeclaration(attrs, 13))
            {
                flag = false;
            }
            return flag;
        }

        protected virtual bool ParseLabelName(out string name)
        {
            name = string.Empty;
            switch (this.Token)
            {
                case 190:
                case 0xbf:
                case 0xc0:
                case 0xc1:
                    name = this.TokenString;
                    this.MoveNext();
                    return true;
            }
            this.SyntaxError(190);
            return false;
        }

        protected virtual bool ParseLocalDeclarationStatement()
        {
            bool flag = this.ParseDimDeclaration(null, 15);
            if (!this.ParseStatementTerminator())
            {
                flag = false;
            }
            return flag;
        }

        protected virtual bool ParseLoopControlVariable()
        {
            bool flag = true;
            bool asType = false;
            if (this.IsIdentifierToken(this.Token))
            {
                this.SaveState();
                try
                {
                    flag = this.ParseSimpleVariable(ref asType);
                }
                finally
                {
                    this.RestoreState(!asType);
                }
            }
            if (!asType)
            {
                ISyntaxNode node = null;
                flag = this.ParseExpression(ref node);
                if (node != null)
                {
                    this.AddNode(node);
                }
            }
            return flag;
        }

        protected virtual bool ParseMeAccess(ref ISyntaxNode node)
        {
            return this.ParseMeAccess(ref node, 0x9e);
        }

        protected virtual bool ParseMeAccess(ref ISyntaxNode node, int nodeType)
        {
            node = new SyntaxNode(this.TokenPosition, this.TokenString, nodeType);
            this.MoveNext();
            return true;
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
            if (this.IsKeywordToken(this.Token) && (currentPosition.Y == this.TokenPosition.Y))
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
            node.Range.EndPoint = base.prevPosition;
            return flag;
        }

        protected virtual bool ParseMethodBlock(int endToken)
        {
            return this.ParseBlockStatement(0x2d, endToken);
        }

        protected virtual bool ParseMethodBody(ISyntaxNode node, int endToken)
        {
            bool flag = true;
            node.Options |= SyntaxNodeOptions.Outlining | SyntaxNodeOptions.Indentation;
            if (!this.ParseMethodBlock(endToken))
            {
                flag = false;
            }
            node.AddAttribute(new SyntaxAttribute(base.prevPosition, SyntaxConsts.DefinitionScopeEnd, null));
            node.Range.EndPoint = base.prevPosition;
            return flag;
        }

        protected virtual bool ParseMethodDeclaration(ISyntaxAttributes attrs)
        {
            string str;
            Point tokenPosition = this.TokenPosition;
            int token = this.Token;
            this.MoveNext();
            if (this.Token == 0x57)
            {
                return this.ParseConstructorDeclaration(attrs, tokenPosition);
            }
            ISyntaxNode node = new SyntaxNode(((attrs != null) && (attrs.Count > 0)) ? attrs[0].Position : tokenPosition, string.Empty, 0x11, SyntaxNodeOptions.CodeCompletion);
            if (attrs != null)
            {
                node.AddAttributes(attrs);
            }
            if (this.ParseIdentifier(out str))
            {
                node.Name = str;
                this.AddNode(node);
                return this.ParseMethodDeclaration(node, token, true);
            }
            return false;
        }

        protected virtual bool ParseMethodDeclaration(ISyntaxNode node, int endToken, bool block)
        {
            bool flag = true;
            if (!this.BeforeDeclaration(node))
            {
                flag = false;
            }
            bool flag2 = this.SyntaxTree.Current.NodeType != 10;
            this.SyntaxTree.Push(node);
            try
            {
                if ((this.Token == 0x97) && !this.ParseParameterListDeclaration())
                {
                    flag = false;
                }
                if (this.IsFunctionToken(endToken))
                {
                    if (this.IsType(this.Token))
                    {
                        string str;
                        this.MoveNext();
                        Point tokenPosition = this.TokenPosition;
                        if (this.ParseType(out str))
                        {
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
                }
                switch (this.Token)
                {
                    case 0x3d:
                        flag2 = true;
                        if (!this.ParseHandlesClause())
                        {
                            flag = false;
                        }
                        break;

                    case 0x3f:
                        flag2 = true;
                        if (!this.ParseImplementsClause())
                        {
                            flag = false;
                        }
                        break;
                }
                node.AddAttribute(new SyntaxAttribute(base.prevPosition, SyntaxConsts.DeclarationScope, null));
                if ((block && flag2) && !this.ParseMethodBody(node, endToken))
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

        protected virtual bool ParseMidAssignmentStatement()
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, this.TokenString, 0x77);
            this.AddNode(node);
            this.MoveNext();
            if (this.Token == 170)
            {
                this.MoveNext();
            }
            ISyntaxNode node2 = null;
            if (this.Expected(VbLexerToken.Open_parens))
            {
                flag = this.ParseExpressionList(ref node2);
                if (node2 != null)
                {
                    node.AddChild(node2);
                }
                if (!this.Expected(VbLexerToken.Close_parens))
                {
                    flag = false;
                }
            }
            else
            {
                flag = false;
            }
            node2 = null;
            if (!this.ParseAssignmentExpression(ref node2))
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
            node.Range.EndPoint = base.prevPosition;
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
            switch (((VbLexerToken) this.Token))
            {
                case VbLexerToken.Star:
                case VbLexerToken.Div:
                case VbLexerToken.IntDiv:
                case VbLexerToken.Carret:
                case VbLexerToken.Mod:
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
                if (this.Token == 0x97)
                {
                    ISyntaxNode node2 = null;
                    flag = this.ParseArgumentList(ref node2, true);
                    if (node2 != null)
                    {
                        node.AddChild(node2);
                    }
                }
                if (this.Token == 0x95)
                {
                    node.NodeType = 0x9a;
                    ISyntaxNode node3 = null;
                    if (!this.ParseArrayInitializerExpression(ref node3))
                    {
                        flag = false;
                    }
                    if (node3 != null)
                    {
                        node.AddChild(node3);
                    }
                }
            }
            else
            {
                flag = false;
            }
            node.Range.EndPoint = base.prevPosition;
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
                if (!this.Expected(VbLexerToken.Error))
                {
                    goto Label_0083;
                }
                VbLexerToken token = (VbLexerToken) this.Token;
                if (token != VbLexerToken.GoTo)
                {
                    if (token != VbLexerToken.Resume)
                    {
                        goto Label_006D;
                    }
                    this.MoveNext();
                    if (!this.Expected(VbLexerToken.Next))
                    {
                        flag = false;
                    }
                }
                else if (!this.ParseGotoStatement())
                {
                    flag = false;
                }
                goto Label_0077;
            Label_006D:
                flag = false;
                this.SyntaxError(0x75);
            Label_0077:
                if (!this.ParseStatementTerminator())
                {
                    flag = false;
                }
                goto Label_0094;
            Label_0083:
                flag = false;
            }
            finally
            {
                this.SyntaxTree.Pop();
            }
        Label_0094:
            node.Range.EndPoint = base.prevPosition;
            return flag;
        }

        protected virtual bool ParseOperatorDeclaration(ISyntaxAttributes attrs)
        {
            bool flag = true;
            Point tokenPosition = this.TokenPosition;
            this.MoveNext();
            ISyntaxNode node = new SyntaxNode(((attrs != null) && (attrs.Count > 0)) ? attrs[0].Position : tokenPosition, this.TokenString, 0x17);
            this.AddNode(node);
            if (attrs != null)
            {
                node.AddAttributes(attrs);
            }
            if (!this.BeforeDeclaration(node))
            {
                flag = false;
            }
            this.SyntaxTree.Push(node);
            try
            {
                switch (((VbLexerToken) this.Token))
                {
                    case VbLexerToken.IsFalse:
                    case VbLexerToken.IsTrue:
                    case VbLexerToken.Like:
                    case VbLexerToken.Mod:
                    case VbLexerToken.And:
                    case VbLexerToken.Plus:
                    case VbLexerToken.Minus:
                    case VbLexerToken.Assign:
                    case VbLexerToken.Op_lt:
                    case VbLexerToken.Op_gt:
                    case VbLexerToken.Op_long:
                    case VbLexerToken.Star:
                    case VbLexerToken.Div:
                    case VbLexerToken.IntDiv:
                    case VbLexerToken.Carret:
                    case VbLexerToken.Op_shift_left:
                    case VbLexerToken.Op_shift_right:
                    case VbLexerToken.Op_le:
                    case VbLexerToken.Op_ge:
                    case VbLexerToken.Op_ne:
                    case VbLexerToken.Xor:
                    case VbLexerToken.Not:
                    case VbLexerToken.Or:
                        this.MoveNext();
                        break;

                    default:
                        flag = false;
                        this.SyntaxError();
                        break;
                }
                if (flag)
                {
                    if (!this.ParseParameterListDeclaration())
                    {
                        flag = false;
                    }
                    if (this.IsType(this.Token))
                    {
                        string str;
                        this.MoveNext();
                        tokenPosition = this.TokenPosition;
                        if (this.ParseType(out str))
                        {
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
                    node.AddAttribute(new SyntaxAttribute(base.prevPosition, SyntaxConsts.DeclarationScope, null));
                    if (!this.ParseMethodBody(node, 0x60))
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

        protected virtual bool ParseOptionStatement()
        {
            bool flag = true;
            Point tokenPosition = this.TokenPosition;
            this.MoveNext();
            switch (this.TokenString.ToLower())
            {
                case "explicit":
                case "strict":
                {
                    string str2;
                    ISyntaxNode node = new SyntaxNode(tokenPosition, this.TokenString, 0xc3);
                    this.AddNode(node);
                    this.MoveNext();
                    if (((str2 = this.TokenString.ToLower()) != null) && ((str2 == "on") || (str2 == "off")))
                    {
                        node.AddAttribute(new SyntaxAttribute(this.TokenPosition, NetNodeType.Attribute.ToString(), this.TokenString));
                        this.MoveNext();
                    }
                    node.Range.EndPoint = base.prevPosition;
                    return flag;
                }
                case "compare":
                {
                    string str3;
                    ISyntaxNode node2 = new SyntaxNode(tokenPosition, this.TokenString, 0xc3);
                    this.AddNode(node2);
                    this.MoveNext();
                    if (((str3 = this.TokenString.ToLower()) != null) && ((str3 == "text") || (str3 == "binary")))
                    {
                        node2.AddAttribute(new SyntaxAttribute(this.TokenPosition, NetNodeType.Attribute.ToString(), this.TokenString));
                        this.MoveNext();
                    }
                    else
                    {
                        this.SyntaxError();
                        flag = false;
                    }
                    node2.Range.EndPoint = base.prevPosition;
                    return flag;
                }
            }
            flag = false;
            this.SyntaxError();
            return flag;
        }

        protected virtual bool ParseParameterDeclaration()
        {
            string str;
            bool flag2;
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, string.Empty, 0x1a, SyntaxNodeOptions.CodeCompletion);
            if (this.Token != 0xa3)
            {
                goto Label_0074;
            }
            this.SyntaxTree.Push(node);
            try
            {
                if (!this.ParseAttributeListDeclaration())
                {
                    flag = false;
                }
                goto Label_0074;
            }
            finally
            {
                this.SyntaxTree.Pop();
            }
        Label_0049:
            node.AddAttribute(new SyntaxAttribute(this.TokenPosition, NetNodeType.ParameterModifier.ToString(), this.TokenString));
            this.MoveNext();
        Label_0074:
            if (this.IsParameterModifier(this.Token))
            {
                goto Label_0049;
            }
            if (this.ParseVariableIdentifier(out str, out flag2))
            {
                node.Name = str;
                if (flag2)
                {
                    node.AddAttribute(new SyntaxAttribute(this.TokenPosition, NetNodeType.ArraySpecifier.ToString(), null));
                }
                if (this.IsType(this.Token))
                {
                    string str2;
                    this.MoveNext();
                    Point tokenPosition = this.TokenPosition;
                    if (this.ParseType(out str2))
                    {
                        node.AddAttribute(new SyntaxAttribute(tokenPosition, NetNodeType.Type.ToString(), str2));
                    }
                    else
                    {
                        flag = false;
                    }
                }
                if (this.Token == 0xa2)
                {
                    this.MoveNext();
                    node.AddAttribute(new SyntaxAttribute(this.TokenPosition, NetNodeType.OptionalParameter.ToString(), null));
                    ISyntaxNode node2 = null;
                    if (!this.ParseConstantExpression(ref node2))
                    {
                        flag = false;
                    }
                    if (node2 != null)
                    {
                        node.AddChild(node2);
                    }
                }
                this.AddNode(node);
            }
            else
            {
                flag = false;
            }
            node.Range.EndPoint = base.prevPosition;
            return flag;
        }

        protected virtual bool ParseParameterListDeclaration()
        {
            return this.ParseParameterListDeclaration(VbLexerToken.Open_parens, VbLexerToken.Close_parens);
        }

        protected virtual bool ParseParameterListDeclaration(VbLexerToken startToken, VbLexerToken endToken)
        {
            bool flag = true;
            if (this.Token != (int)startToken)
            {
                this.SyntaxError((int) startToken);
                return false;
            }
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, string.Empty, 0x1b);
            this.AddNode(node);
            this.SyntaxTree.Push(node);
            try
            {
                this.MoveNext();
                while (!this.Eof && (this.Token != (int)endToken))
                {
                    if (!this.ParseParameterDeclaration())
                    {
                        flag = false;
                    }
                    if (this.Token != 0x9a)
                    {
                        goto Label_007B;
                    }
                    this.MoveNext();
                }
            }
            finally
            {
                this.SyntaxTree.Pop();
            }
        Label_007B:
            if (!this.Expected(endToken))
            {
                flag = false;
            }
            node.Range.EndPoint = base.prevPosition;
            return flag;
        }

        protected virtual bool ParseParenthesizedExpression(ref ISyntaxNode node)
        {
            bool flag = true;
            node = new SyntaxNode(this.TokenPosition, this.TokenString, 0xa8);
            if (this.Token == 0x97)
            {
                this.MoveNext();
                ISyntaxNode node2 = null;
                flag = this.ParseExpression(ref node2);
                if (node2 != null)
                {
                    node.AddChild(node2);
                }
                if (!this.Expected(VbLexerToken.Close_parens))
                {
                    flag = false;
                }
            }
            else
            {
                this.SyntaxError(0x97);
                flag = false;
            }
            node.Range.EndPoint = base.prevPosition;
            return flag;
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
                    if (this.Token == 0x9c)
                    {
                        this.MoveNext();
                    }
                    if (this.Token == 0xa2)
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

        protected virtual bool ParsePrefixedUnaryExpression(ref ISyntaxNode node)
        {
            bool flag = true;
            switch (((VbLexerToken) this.Token))
            {
                case VbLexerToken.Plus:
                case VbLexerToken.Minus:
                case VbLexerToken.Not:
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
            return this.ParsePrimaryExpression(ref node);
        }

        protected virtual bool ParsePrimaryExpression(ref ISyntaxNode node)
        {
            bool flag = true;
            int lineIndex = base.lineIndex;
            flag = this.ParseSimpleExpression(ref node);
            if (((node != null) && (node != null)) && ((lineIndex == base.lineIndex) && !this.TryParsePostPrimaryExpression(ref node)))
            {
                flag = false;
            }
            return flag;
        }

        protected virtual bool ParsePropertyDeclaration(ISyntaxAttributes attrs)
        {
            string str;
            Point tokenPosition = this.TokenPosition;
            this.MoveNext();
            ISyntaxNode node = new SyntaxNode(((attrs != null) && (attrs.Count > 0)) ? attrs[0].Position : tokenPosition, string.Empty, 0x15, SyntaxNodeOptions.CodeCompletion | SyntaxNodeOptions.Indentation);
            if (attrs != null)
            {
                node.AddAttributes(attrs);
            }
            if (this.ParseIdentifier(out str))
            {
                node.Name = str;
                this.AddNode(node);
                return this.ParsePropertyDeclaration(node);
            }
            return false;
        }

        protected virtual bool ParsePropertyDeclaration(ISyntaxNode node)
        {
            bool flag = true;
            if (!this.BeforeDeclaration(node))
            {
                flag = false;
            }
            bool flag2 = this.SyntaxTree.Current.NodeType != 10;
            this.SyntaxTree.Push(node);
            try
            {
                Point tokenPosition;
                if ((this.Token == 0x97) && !this.ParseParameterListDeclaration())
                {
                    flag = false;
                }
                if (this.IsType(this.Token))
                {
                    string str;
                    this.MoveNext();
                    tokenPosition = this.TokenPosition;
                    if (this.ParseType(out str))
                    {
                        node.AddAttribute(new SyntaxAttribute(tokenPosition, NetNodeType.Type.ToString(), str));
                    }
                    else
                    {
                        flag = false;
                    }
                }
                if (this.Token == 0x3f)
                {
                    string str3;
                    string tokenString = this.TokenString;
                    this.MoveNext();
                    tokenPosition = this.TokenPosition;
                    if (this.ParseImplements(out str3))
                    {
                        node.AddAttribute(new SyntaxAttribute(tokenPosition, tokenString, str3));
                    }
                    else
                    {
                        flag = false;
                    }
                }
                node.AddAttribute(new SyntaxAttribute(base.prevPosition, SyntaxConsts.DeclarationScope, null));
                if (flag2)
                {
                    while (!this.Eof)
                    {
                        if (this.Token == 0x2d)
                        {
                            this.MoveNext();
                            break;
                        }
                        if (!this.ParseAccessor(NetNodeType.PropertyAccessor))
                        {
                            flag = false;
                        }
                    }
                    if (!this.Expected(VbLexerToken.Property))
                    {
                        flag = false;
                    }
                    node.AddAttribute(new SyntaxAttribute(base.prevPosition, SyntaxConsts.DefinitionScopeEnd, null));
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

        protected virtual bool ParseQualifiedIdentifier(out string identifier)
        {
            identifier = this.TokenString;
            if (!this.ParseIdentifier(out identifier))
            {
                identifier = string.Empty;
                return false;
            }
            while (this.Token == 0x99)
            {
                identifier = identifier + this.TokenString;
                this.MoveNext();
                string tokenString = this.TokenString;
                if (this.IsKeywordToken(this.Token))
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

        protected virtual bool ParseRaiseEventStatement()
        {
            string str;
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, this.TokenString, 0x79);
            this.AddNode(node);
            this.MoveNext();
            if (this.ParseIdentifier(out str))
            {
                node.Name = str;
            }
            else
            {
                flag = false;
            }
            if (this.Token == 0x97)
            {
                ISyntaxNode node2 = null;
                if (!this.ParseArgumentList(ref node2, false))
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
            node.Range.EndPoint = base.prevPosition;
            return flag;
        }

        protected virtual bool ParseRankSpecifier(out string rank)
        {
            bool flag = true;
            rank = this.TokenString;
            if (!this.Expected(VbLexerToken.Open_parens))
            {
                return flag;
            }
            while (this.Token == 0x9a)
            {
                rank = rank + this.TokenString;
                this.MoveNext();
            }
            if (this.Token == 0x98)
            {
                rank = rank + this.TokenString;
            }
            return this.Expected(VbLexerToken.Close_parens);
        }

        protected virtual bool ParseRedimClause()
        {
            ISyntaxNode node = null;
            bool flag = this.ParseExpression(ref node);
            if (node != null)
            {
                this.AddNode(node);
            }
            return flag;
        }

        protected virtual bool ParseRedimStatement()
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, this.TokenString, 0x7a, SyntaxNodeOptions.Indentation);
            this.AddNode(node);
            this.SyntaxTree.Push(node);
            try
            {
                this.MoveNext();
                if (this.Token == 0x6a)
                {
                    this.MoveNext();
                }
                if (!this.ParseRedimClause())
                {
                    flag = false;
                }
                while (!this.Eof && (this.Token == 0x9a))
                {
                    if (!this.ParseRedimClause())
                    {
                        flag = false;
                    }
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
            node.Range.EndPoint = base.prevPosition;
            return flag;
        }

        protected virtual bool ParseRelationalExpression(ref ISyntaxNode node)
        {
            bool flag = this.ParseShiftExpression(ref node);
            switch (((VbLexerToken) this.Token))
            {
                case VbLexerToken.Op_lt:
                case VbLexerToken.Op_gt:
                case VbLexerToken.Op_le:
                case VbLexerToken.Op_ge:
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

        protected virtual bool ParseRemoveHandlerStatement()
        {
            return this.ParseEventHandlerStatement(0x7b);
        }

        protected virtual bool ParseResumeStatement()
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, this.TokenString, 0x7c, SyntaxNodeOptions.Indentation);
            this.AddNode(node);
            int lineIndex = base.lineIndex;
            this.MoveNext();
            if (!this.ParseStatementTerminator(lineIndex))
            {
                if (this.Token == 0x58)
                {
                    this.MoveNext();
                }
                else
                {
                    string str;
                    if (!this.ParseLabelName(out str))
                    {
                        flag = false;
                    }
                }
            }
            if (!this.ParseStatementTerminator())
            {
                flag = false;
            }
            node.Range.EndPoint = base.prevPosition;
            return flag;
        }

        protected virtual bool ParseReturnStatement()
        {
            return this.ParseThrowOrReturnStatement(0x5e);
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
                if (this.Token == 14)
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
                    flag = false;
                }
                node.AddAttribute(new SyntaxAttribute(base.prevPosition, SyntaxConsts.DefinitionScope, null));
                while (!this.Eof && (this.Token == 14))
                {
                    if (!this.ParseCaseStatement())
                    {
                        flag = false;
                    }
                }
                if (!this.Expected(VbLexerToken.End) || !this.Expected(VbLexerToken.Select))
                {
                    if (flag2)
                    {
                        node.AddAttribute(new SyntaxAttribute(base.prevPosition, SyntaxConsts.DefinitionScopeEndExpected, "\r\n\r\n" + VbLexerToken.End.ToString() + " " + VbLexerToken.Select.ToString()));
                    }
                    flag = false;
                }
                node.AddAttribute(new SyntaxAttribute(base.prevPosition, SyntaxConsts.DefinitionScopeEnd, null));
                if (!this.ParseStatementTerminator())
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

        protected virtual bool ParseShiftExpression(ref ISyntaxNode node)
        {
            bool flag = this.ParseAdditiveExpression(ref node);
            switch (this.Token)
            {
                case 0xaf:
                case 0xb0:
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
            switch (((VbLexerToken) this.Token))
            {
                case VbLexerToken.AddressOf:
                    return this.ParseAddressofExpression(ref node);

                case VbLexerToken.Boolean:
                case VbLexerToken.Byte:
                case VbLexerToken.Char:
                case VbLexerToken.Date:
                case VbLexerToken.Decimal:
                case VbLexerToken.Double:
                case VbLexerToken.Long:
                case VbLexerToken.Integer:
                case VbLexerToken.Short:
                case VbLexerToken.Single:
                case VbLexerToken.String:
                case VbLexerToken.Object:
                    node = new SyntaxNode(this.TokenPosition, this.TokenString, 0x99);
                    this.MoveNext();
                    if (this.Token != 0x99)
                    {
                        flag = false;
                    }
                    return flag;

                case VbLexerToken.CBool:
                    return this.ParseCastTargetExpression(ref node, VbLexerToken.Boolean);

                case VbLexerToken.CByte:
                    return this.ParseCastTargetExpression(ref node, VbLexerToken.Byte);

                case VbLexerToken.CChar:
                    return this.ParseCastTargetExpression(ref node, VbLexerToken.Char);

                case VbLexerToken.CDate:
                    return this.ParseCastTargetExpression(ref node, VbLexerToken.Date);

                case VbLexerToken.CDbl:
                    return this.ParseCastTargetExpression(ref node, VbLexerToken.Double);

                case VbLexerToken.CDec:
                    return this.ParseCastTargetExpression(ref node, VbLexerToken.Decimal);

                case VbLexerToken.CInt:
                    return this.ParseCastTargetExpression(ref node, VbLexerToken.Integer);

                case VbLexerToken.CLng:
                    return this.ParseCastTargetExpression(ref node, VbLexerToken.Long);

                case VbLexerToken.CObj:
                    return this.ParseCastTargetExpression(ref node, VbLexerToken.Object);

                case VbLexerToken.CShort:
                    return this.ParseCastTargetExpression(ref node, VbLexerToken.Short);

                case VbLexerToken.CSng:
                    return this.ParseCastTargetExpression(ref node, VbLexerToken.Single);

                case VbLexerToken.CStr:
                    return this.ParseCastTargetExpression(ref node, VbLexerToken.String);

                case VbLexerToken.CType:
                case VbLexerToken.DirectCast:
                    return this.ParseCastExpression(ref node);

                case VbLexerToken.False:
                case VbLexerToken.Nothing:
                case VbLexerToken.True:
                case VbLexerToken.Identifier_Literal:
                case VbLexerToken.Integer_Literal:
                case VbLexerToken.Short_Literal:
                case VbLexerToken.Long_Literal:
                case VbLexerToken.Float_Literal:
                case VbLexerToken.Double_Literal:
                case VbLexerToken.Decimal_Literal:
                case VbLexerToken.Date_Literal:
                case VbLexerToken.Character_Literal:
                case VbLexerToken.String_Literal:
                    node = new SyntaxNode(this.TokenPosition, this.TokenString, 0x99);
                    this.MoveNext();
                    return flag;

                case VbLexerToken.GetType:
                    return this.ParseGetTypeExpression(ref node);

                case VbLexerToken.Me:
                    return this.ParseMeAccess(ref node);

                case VbLexerToken.MyBase:
                    return this.ParseBaseAccess(ref node);

                case VbLexerToken.MyClass:
                    return this.ParseMeAccess(ref node);

                case VbLexerToken.New:
                    return this.ParseNewExpression(ref node);

                case VbLexerToken.TypeOf:
                    return this.ParseTypeofIsExpression(ref node);

                case VbLexerToken.Open_parens:
                    return this.ParseParenthesizedExpression(ref node);

                case VbLexerToken.Dot:
                {
                    ISyntaxNode node2 = this.FindWithStatement(this.SyntaxTree.Current);
                    if (node2 == null)
                    {
                        flag = false;
                        this.SyntaxError();
                        this.MoveNext();
                        return flag;
                    }
                    ISyntaxNode node3 = new SyntaxNode(this.TokenPosition, this.TokenString, 0x81);
                    this.AddNode(node3);
                    node3.AddAttribute(new SyntaxAttribute(this.TokenPosition, NetNodeType.WithStatement.ToString(), node2));
                    this.MoveNext();
                    node3.Range.EndPoint = base.prevPosition;
                    return this.ParseSimpleExpression(ref node);
                }
            }
            flag = false;
            this.SyntaxError();
            if (!this.IsInvalidExpressionToken(this.Token))
            {
                this.MoveNext();
            }
            return flag;
        }

        protected virtual bool ParseSimpleVariable(ref bool asType)
        {
            string str;
            string str2;
            bool flag = true;
            Point tokenPosition = this.TokenPosition;
            if (!this.ParseIdentifier(out str))
            {
                return flag;
            }
            asType = this.Token == 6;
            if (!asType)
            {
                return flag;
            }
            this.MoveNext();
            ISyntaxNode node = new SyntaxNode(tokenPosition, str, 15, SyntaxNodeOptions.CodeCompletion);
            this.AddNode(node);
            if (this.ParseType(out str2))
            {
                node.AddAttribute(new SyntaxAttribute(tokenPosition, NetNodeType.Type.ToString(), str2));
                return flag;
            }
            return false;
        }

        protected virtual bool ParseStatement()
        {
            bool flag;
            switch (((VbLexerToken) this.Token))
            {
                case VbLexerToken.AddHandler:
                    return this.ParseAddHandlerStatement();

                case VbLexerToken.Call:
                    return this.ParseCallStatement();

                case VbLexerToken.Const:
                case VbLexerToken.Dim:
                case VbLexerToken.Static:
                    return this.ParseLocalDeclarationStatement();

                case VbLexerToken.Continue:
                    return this.ParseContinueStatement();

                case VbLexerToken.Do:
                    return this.ParseDoStatement();

                case VbLexerToken.End:
                    return this.ParseEndStatement();

                case VbLexerToken.Erase:
                    return this.ParseEraseStatement();

                case VbLexerToken.Error:
                    return this.ParseErrorStatement();

                case VbLexerToken.Exit:
                    return this.ParseExitStatement();

                case VbLexerToken.For:
                    return this.ParseForStatement();

                case VbLexerToken.GoTo:
                    return this.ParseGotoStatement();

                case VbLexerToken.If:
                    return this.ParseIfStatement();

                case VbLexerToken.Mid:
                    return this.ParseMidAssignmentStatement();

                case VbLexerToken.RaiseEvent:
                    return this.ParseRaiseEventStatement();

                case VbLexerToken.ReDim:
                    return this.ParseRedimStatement();

                case VbLexerToken.RemoveHandler:
                    return this.ParseRemoveHandlerStatement();

                case VbLexerToken.Resume:
                    return this.ParseResumeStatement();

                case VbLexerToken.Return:
                    return this.ParseReturnStatement();

                case VbLexerToken.Select:
                    return this.ParseSelectStatement();

                case VbLexerToken.On:
                    return this.ParseOnErrorStatement();

                case VbLexerToken.Stop:
                    return this.ParseStopStatement();

                case VbLexerToken.SyncLock:
                    return this.ParseSyncLockStatement();

                case VbLexerToken.Throw:
                    return this.ParseThrowStatement();

                case VbLexerToken.Try:
                    return this.ParseTryStatement();

                case VbLexerToken.Using:
                    return this.ParseUsingStatement();

                case VbLexerToken.While:
                    return this.ParseWhileStatement();

                case VbLexerToken.With:
                    return this.ParseWithStatement();

                case VbLexerToken.Directive_Literal:
                    return this.ParseDirective();
            }
            this.TryParseLabeledStatement(out flag);
            if (!flag)
            {
                return this.ParseExpressionStatement();
            }
            return true;
        }

        protected virtual bool ParseStatementExpression()
        {
            bool flag = true;
            ISyntaxNode node = null;
            this.StartExpression();
            try
            {
                flag = this.ParsePrimaryExpression(ref node);
                if (!this.TryParseAssignmentExpression(ref node))
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

        protected virtual bool ParseStatementTerminator()
        {
            if (this.Token == 0x9c)
            {
                this.MoveNext();
            }
            return true;
        }

        protected virtual bool ParseStatementTerminator(int line)
        {
            if (this.Token == 0x9c)
            {
                this.MoveNext();
                return true;
            }
            return (base.lineIndex != line);
        }

        protected virtual bool ParseStopOrEndStatement(int nodeType)
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, this.TokenString, nodeType);
            this.AddNode(node);
            this.MoveNext();
            if (!this.ParseStatementTerminator())
            {
                flag = false;
            }
            node.Range.EndPoint = base.prevPosition;
            return flag;
        }

        protected virtual bool ParseStopStatement()
        {
            return this.ParseStopOrEndStatement(0x7e);
        }

        protected virtual bool ParseSyncLockStatement()
        {
            return this.ParseExpressionBlockStatement(0x83, 0x7f);
        }

        protected virtual bool ParseThrowOrReturnStatement(int nodeType)
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, this.TokenString, nodeType, SyntaxNodeOptions.Indentation);
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
            node.Range.EndPoint = base.prevPosition;
            return flag;
        }

        protected virtual bool ParseThrowStatement()
        {
            return this.ParseThrowOrReturnStatement(0x5f);
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
                flag = this.ParseBlock(new int[] { 15, 0x35, 0x2d });
                while (!this.Eof && (this.Token == 15))
                {
                    if (!this.ParseCatchStatement())
                    {
                        flag = false;
                    }
                }
                if ((this.Token == 0x35) && !this.ParseFinallyStatement())
                {
                    flag = false;
                }
                if (!this.Expected(VbLexerToken.End) || !this.Expected(VbLexerToken.Try))
                {
                    flag = false;
                    node.AddAttribute(new SyntaxAttribute(base.prevPosition, SyntaxConsts.DefinitionScopeEndExpected, "\r\n\r\n" + VbLexerToken.End.ToString() + " " + VbLexerToken.Try.ToString()));
                }
                ISyntaxAttribute attribute = node.FindAttribute(SyntaxConsts.DefinitionScopeEnd);
                if (attribute != null)
                {
                    attribute.Position = base.prevPosition;
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

        protected virtual bool ParseTypeName(out string type)
        {
            bool flag = true;
            if (this.IsBuiltInType(this.Token) || (this.Token == 7))
            {
                type = this.TokenString;
                flag = true;
                this.MoveNext();
                return flag;
            }
            return this.ParseQualifiedIdentifier(out type);
        }

        protected virtual bool ParseTypeofIsExpression(ref ISyntaxNode node)
        {
            bool flag = true;
            node = new SyntaxNode(this.TokenPosition, this.TokenString, 0xa6);
            this.MoveNext();
            ISyntaxNode node2 = null;
            flag = this.ParseExpression(ref node2);
            if (node2 != null)
            {
                node.AddChild(node2);
            }
            node.Range.EndPoint = base.prevPosition;
            return flag;
        }

        protected virtual bool ParseTypeParameterConstraintsClause()
        {
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, string.Empty, 0x38);
            bool flag = this.IsKeywordToken(this.Token);
            if (flag)
            {
                node.Name = this.TokenString;
                this.MoveNext();
            }
            else
            {
                string str;
                flag = this.ParseIdentifier(out str);
                if (flag)
                {
                    node.Name = str;
                }
            }
            node.Range.EndPoint = base.prevPosition;
            if (flag)
            {
                this.AddNode(node);
            }
            return flag;
        }

        protected virtual bool ParseTypeParameterConstraintsClauses()
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, string.Empty, 0x37);
            this.AddNode(node);
            bool flag2 = this.Token == 0x95;
            if (flag2)
            {
                this.MoveNext();
            }
            this.SyntaxTree.Push(node);
            try
            {
            Label_0040:
                if (!this.ParseTypeParameterConstraintsClause())
                {
                    flag = false;
                }
                if (this.Token == 0x9a)
                {
                    this.MoveNext();
                    if (!this.Eof)
                    {
                        goto Label_0040;
                    }
                }
            }
            finally
            {
                this.SyntaxTree.Pop();
            }
            if (flag2 && !this.Expected(VbLexerToken.Close_brace))
            {
                flag = false;
            }
            node.Range.EndPoint = base.prevPosition;
            return flag;
        }

        protected virtual bool ParseTypeParameterList()
        {
            string str;
            bool flag = this.Expected(VbLexerToken.Of);
            if (!flag)
            {
                return flag;
            }
            Point tokenPosition = this.TokenPosition;
            if (this.ParseTypeName(out str))
            {
                ISyntaxNode node = new SyntaxNode(tokenPosition, str, 0x35);
                this.AddNode(node);
                if (this.Token == 6)
                {
                    this.SyntaxTree.Push(node);
                    try
                    {
                        this.MoveNext();
                        if (!this.ParseTypeParameterConstraintsClauses())
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
            return false;
        }

        protected override bool ParseUnitBody()
        {
            return this.ParseClassBody();
        }

        protected override bool ParseUsingDeclaration(ISyntaxNode node)
        {
            return this.ParseImportDeclaration(node);
        }

        protected override bool ParseUsingList(ISyntaxNode node)
        {
            return this.ParseImportsList(node);
        }

        protected virtual bool ParseUsingStatement()
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, this.TokenString, 0x67, SyntaxNodeOptions.Outlining | SyntaxNodeOptions.Indentation);
            node.AddAttribute(new SyntaxAttribute(this.TokenPosition, SyntaxConsts.SuppressReparsingScope, null));
            this.AddNode(node);
            this.SyntaxTree.Push(node);
            try
            {
                this.MoveNext();
                this.ParseVariableMemberDeclaration(null, this.TokenPosition, 15);
                bool flag2 = true;
                if (!this.ParseStatementTerminator())
                {
                    flag2 = false;
                    flag = false;
                }
                bool blockEnd = false;
                flag = this.ParseBlock(new int[] { 0x2d }, false, out blockEnd);
                if (!this.ParseExpressionBlockStatementEnd(140))
                {
                    if (flag2)
                    {
                        node.AddAttribute(new SyntaxAttribute(base.prevPosition, SyntaxConsts.DefinitionScopeEndExpected, "\r\n\r\n" + VbLexerToken.End.ToString() + " " + VbLexerToken.Using.ToString()));
                    }
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

        protected virtual bool ParseVariableIdentifier(out string identifier, out bool isArray)
        {
            string str;
            bool flag = true;
            identifier = string.Empty;
            if (!this.ParseQualifiedIdentifier(out identifier))
            {
                flag = false;
            }
            this.TryParseArraySizeInitializationModifier(out str);
            isArray = str != string.Empty;
            return flag;
        }

        protected virtual bool ParseVariableInitializer(ref ISyntaxNode node)
        {
            if (this.Token == 0x95)
            {
                return this.ParseArrayInitializerExpression(ref node);
            }
            return this.ParseExpression(ref node);
        }

        protected virtual bool ParseVariableMemberDeclaration(ISyntaxAttributes attrs, Point pos, int nodeType)
        {
            bool flag = true;
            while (!this.Eof)
            {
                if (this.IsIdentifierToken(this.Token))
                {
                    bool flag2;
                    string identifier = string.Empty;
                    if (this.ParseVariableIdentifier(out identifier, out flag2))
                    {
                        ISyntaxNodes names = null;
                        while (this.Token == 0x9a)
                        {
                            string str2;
                            bool flag3;
                            this.MoveNext();
                            Point tokenPosition = this.TokenPosition;
                            if (this.ParseVariableIdentifier(out str2, out flag3))
                            {
                                if (names == null)
                                {
                                    names = new SyntaxNodes();
                                }
                                ISyntaxNode item = new SyntaxNode(tokenPosition, str2, 13, SyntaxNodeOptions.CodeCompletion | SyntaxNodeOptions.Indentation);
                                names.Add(item);
                                if (flag3)
                                {
                                    item.AddAttribute(new SyntaxAttribute(this.TokenPosition, NetNodeType.ArraySpecifier.ToString(), null));
                                }
                            }
                        }
                        switch (((VbLexerToken) this.Token))
                        {
                            case VbLexerToken.As:
                            case VbLexerToken.Assign:
                                this.ParseFieldDeclaration(attrs, pos, identifier, names, nodeType, flag2);
                                goto Label_00E8;
                        }
                        this.SyntaxError(6);
                        flag = false;
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
            Label_00E8:
                if (this.Token != 0x9a)
                {
                    return flag;
                }
                this.MoveNext();
            }
            return flag;
        }

        protected virtual bool ParseWhileStatement()
        {
            return this.ParseExpressionBlockStatement(0x90, 0x47);
        }

        protected virtual bool ParseWithStatement()
        {
            return this.ParseExpressionBlockStatement(new int[] { 0x2d }, 0x91, 0x80, false);
        }

        public override bool ProcessAutoComplete(string text, Point position, out string code)
        {
            code = string.Empty;
            ISyntaxNode nodeAt = this.GetNodeAt(position);
            bool flag = true;
            if (nodeAt.NodeType == 1)
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

        public override void ResetCodeCompletionChars()
        {
            this.CodeCompletionChars = SyntaxParserConsts.DefaultVbCodeCompletionChars.ToCharArray();
        }

        public override void ResetOptions()
        {
            this.Options = SyntaxParserConsts.DefaultVbSyntaxOptions;
        }

        public override void ResetSmartFormatChars()
        {
            this.SmartFormatChars = SyntaxParserConsts.DefaultVbSmartFormatChars.ToCharArray();
        }

        protected override bool ShouldOutlineCommentNode(ISyntaxNode node)
        {
            if (!base.ShouldOutlineCommentNode(node))
            {
                return (this.GetValidNode(node) == this.SyntaxTree.Root);
            }
            return true;
        }

        public override bool ShouldSerializeCodeCompletionChars()
        {
            return (new string(this.CodeCompletionChars) != SyntaxParserConsts.DefaultVbCodeCompletionChars);
        }

        public override bool ShouldSerializeSmartFormatChars()
        {
            return (new string(this.SmartFormatChars) != SyntaxParserConsts.DefaultVbSmartFormatChars);
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
                        if (text == VbLexerToken.EndIf.ToString())
                        {
                            operations.Add(new TextUndo(index, (num3 - index) + 1, VbLexerToken.End.ToString() + ' ' + VbLexerToken.If.ToString()));
                        }
                        else if (text != str)
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
            return base.SmartFormatLine(index, text, textData, operations);
        }

        protected virtual void StartExpression()
        {
            if (this.expressionCount == 0)
            {
                this.expressions.Clear();
            }
            this.expressionCount++;
        }

        protected virtual void SyntaxError(int token)
        {
            if (base.Stack.Count == 0)
            {
                ISyntaxError err = new QWhale.Syntax.SyntaxError(this.TokenPosition, this.TokenString, ((VbLexerToken) token).ToString() + ' ' + StringConsts.ErrExpected);
                if (this.prevPosition.Y != this.TokenPosition.Y)
                {
                    err.Position = base.prevPosition;
                    err.Range.EndPoint = new Point(this.prevPosition.X + 1, this.prevPosition.Y);
                }
                this.SyntaxTree.Current.AddError(err);
            }
        }

        protected virtual bool TryParseArraySizeInitializationModifier(out string rank)
        {
            bool flag = true;
            rank = string.Empty;
            while (this.Token == 0x97)
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

        protected virtual bool TryParseAssignmentExpression(ref ISyntaxNode node)
        {
            bool flag = true;
            switch (((VbLexerToken) this.Token))
            {
                case VbLexerToken.Op_mult_assign:
                case VbLexerToken.Op_div_assign:
                case VbLexerToken.Op_intdiv_assign:
                case VbLexerToken.Op_add_assign:
                case VbLexerToken.Op_sub_assign:
                case VbLexerToken.Op_shift_left_assign:
                case VbLexerToken.Op_shift_right_assign:
                case VbLexerToken.Op_exp_assign:
                case VbLexerToken.String_and_assign:
                case VbLexerToken.Assign:
                {
                    node = this.CreateExpressionNode(this.TokenPosition, this.TokenString, 0x86, node, true);
                    node.AddAttribute(new SyntaxAttribute(this.TokenPosition, NetNodeType.Assignment.ToString(), null));
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

        protected virtual bool TryParseAttributeModifier()
        {
            bool flag = true;
            switch (((VbLexerToken) this.Token))
            {
                case VbLexerToken.Assembly:
                case VbLexerToken.Module:
                    this.AddAttribute(new SyntaxAttribute(this.TokenPosition, NetNodeType.AttributeTarget.ToString(), this.TokenString));
                    this.MoveNext();
                    if (!this.Expected(VbLexerToken.Colon))
                    {
                        flag = false;
                    }
                    break;
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
                parsed = this.ParseLabelName(out str) && this.Expected(VbLexerToken.Colon);
            }
            finally
            {
                this.RestoreState(!parsed);
            }
            if (parsed)
            {
                ISyntaxNode node = new SyntaxNode(tokenPosition, str, 0x69, SyntaxNodeOptions.Indentation);
                this.AddNode(node);
                node.Range.EndPoint = base.prevPosition;
            }
            return parsed;
        }

        protected virtual bool TryParsePostPrimaryExpression(ref ISyntaxNode node)
        {
            bool flag = true;
            while (!this.Eof)
            {
                switch (((VbLexerToken) this.Token))
                {
                    case VbLexerToken.Open_parens:
                    {
                        if (!this.ParseInvocationExpression(ref node))
                        {
                            flag = false;
                        }
                        continue;
                    }
                    case VbLexerToken.Close_parens:
                        return flag;

                    case VbLexerToken.Dot:
                    {
                        if (!this.ParseMemberAccess(ref node))
                        {
                            flag = false;
                        }
                        continue;
                    }
                    case VbLexerToken.Op_single:
                    {
                        if (!this.ParseDictionaryAccess(ref node))
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
            while (this.Token == 0x97)
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

        protected virtual bool TryParseWhileOrUntil(out bool parsed)
        {
            bool flag = true;
            parsed = false;
            switch (((VbLexerToken) this.Token))
            {
                case VbLexerToken.Until:
                case VbLexerToken.While:
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

        protected override bool UpdateLine()
        {
            if ((this.expressionCount > 0) && (this.prevToken != 0x9e))
            {
                this.expressions.Add(this.prevPosition.Y);
            }
            return base.UpdateLine();
        }

        [Description("Gets or sets a boolean value that indicates whether \"VbSyntaxParser\" should perform case-sensitive analysis if its content.")]
        public override bool CaseSensitive
        {
            get
            {
                return false;
            }
        }
    }
}

