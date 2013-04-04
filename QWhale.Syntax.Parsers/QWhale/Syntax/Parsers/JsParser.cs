namespace QWhale.Syntax.Parsers
{
    using QWhale.Common;
    using QWhale.Syntax;
    using QWhale.Syntax.CodeCompletion;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.InteropServices;

    [ToolboxBitmap(typeof(JsParser), "Images.JsParser.bmp"), ToolboxItem(true)]
    public class JsParser : CsParser
    {
        protected override bool AfterDeclaration(ISyntaxNode node)
        {
            NetNodeType nodeType = (NetNodeType) this.GetValidNode(this.SyntaxTree.Current).NodeType;
            return (((node.NodeType == 0xc6) && (nodeType == NetNodeType.Interface)) || base.AfterDeclaration(node));
        }

        protected override bool BeforeDeclaration(ISyntaxNode node)
        {
            if (this.GetValidNode(this.SyntaxTree.Current).NodeType == 10)
            {
                switch (node.NodeType)
                {
                    case 8:
                    case 10:
                    case 11:
                        return true;
                }
            }
            return base.BeforeDeclaration(node);
        }

        protected override int GetLexerStyle(int token)
        {
            if ((token >= 0xa4) && (token <= 0xb1))
            {
                return 2;
            }
            if (token == 0xb5)
            {
                return 1;
            }
            return base.GetLexerStyle(token);
        }

        protected override void InitLanguage()
        {
            this.Scheme.FileType = "j#";
        }

        protected override void InitReswords()
        {
            base.reswords = new Hashtable();
            base.reswords.Add("abstract", CsLexerToken.Abstract);
            base.reswords.Add("assert", JsLexerToken.Assert);
            base.reswords.Add("boolean", JsLexerToken.Boolean);
            base.reswords.Add("break", CsLexerToken.Break);
            base.reswords.Add("byte", CsLexerToken.Byte);
            base.reswords.Add("case", CsLexerToken.Case);
            base.reswords.Add("catch", CsLexerToken.Catch);
            base.reswords.Add("char", CsLexerToken.Char);
            base.reswords.Add("class", CsLexerToken.Class);
            base.reswords.Add("const", CsLexerToken.Consts);
            base.reswords.Add("continue", CsLexerToken.Continue);
            base.reswords.Add("default", CsLexerToken.Default);
            base.reswords.Add("do", CsLexerToken.Do);
            base.reswords.Add("double", CsLexerToken.Double);
            base.reswords.Add("else", CsLexerToken.Else);
            base.reswords.Add("enum", CsLexerToken.Enum);
            base.reswords.Add("extends", JsLexerToken.Extends);
            base.reswords.Add("false", CsLexerToken.False);
            base.reswords.Add("final", JsLexerToken.Final);
            base.reswords.Add("finally", CsLexerToken.Finally);
            base.reswords.Add("float", CsLexerToken.Float);
            base.reswords.Add("for", CsLexerToken.For);
            base.reswords.Add("goto", CsLexerToken.Goto);
            base.reswords.Add("if", CsLexerToken.If);
            base.reswords.Add("implements", JsLexerToken.Implements);
            base.reswords.Add("import", JsLexerToken.Import);
            base.reswords.Add("instanceof", JsLexerToken.Instanceof);
            base.reswords.Add("int", CsLexerToken.Int);
            base.reswords.Add("interface", CsLexerToken.Interface);
            base.reswords.Add("long", CsLexerToken.Long);
            base.reswords.Add("native", JsLexerToken.Native);
            base.reswords.Add("new", CsLexerToken.New);
            base.reswords.Add("null", CsLexerToken.Null);
            base.reswords.Add("package", JsLexerToken.Package);
            base.reswords.Add("private", CsLexerToken.Private);
            base.reswords.Add("protected", CsLexerToken.Protected);
            base.reswords.Add("public", CsLexerToken.Public);
            base.reswords.Add("return", CsLexerToken.Return);
            base.reswords.Add("short", CsLexerToken.Short);
            base.reswords.Add("static", CsLexerToken.Static);
            base.reswords.Add("strictfp", JsLexerToken.Strictfp);
            base.reswords.Add("super", JsLexerToken.Super);
            base.reswords.Add("switch", CsLexerToken.Switch);
            base.reswords.Add("synchronized", JsLexerToken.Synchronized);
            base.reswords.Add("this", CsLexerToken.This);
            base.reswords.Add("throw", CsLexerToken.Throw);
            base.reswords.Add("throws", JsLexerToken.Throws);
            base.reswords.Add("transient", JsLexerToken.Transient);
            base.reswords.Add("true", CsLexerToken.True);
            base.reswords.Add("try", CsLexerToken.Try);
            base.reswords.Add("void", CsLexerToken.Void);
            base.reswords.Add("volatile", CsLexerToken.Volatile);
            base.reswords.Add("while", CsLexerToken.While);
            base.reswords.Add("delegate", CsLexerToken.Delegate);
        }

        protected override IReflectionRepository InternalCreateRepository()
        {
            return new JsRepository(this.CaseSensitive, this.SyntaxTree);
        }

        protected virtual bool IsAnnotationStart()
        {
            return (this.Token == 180);
        }

        protected override bool IsBaseList(int token)
        {
            if (token != 0xa6)
            {
                return (token == 0xa8);
            }
            return true;
        }

        protected override bool IsBuiltInType(int token)
        {
            if (token == 0xa4)
            {
                return true;
            }
            CsLexerToken token2 = (CsLexerToken) token;
            if (token2 <= CsLexerToken.Float)
            {
                switch (token2)
                {
                    case CsLexerToken.Double:
                    case CsLexerToken.Float:
                    case CsLexerToken.Byte:
                    case CsLexerToken.Char:
                        goto Label_0048;
                }
                goto Label_004A;
            }
            if (token2 <= CsLexerToken.Long)
            {
                switch (token2)
                {
                    case CsLexerToken.Int:
                    case CsLexerToken.Long:
                        goto Label_0048;
                }
                goto Label_004A;
            }
            if ((token2 != CsLexerToken.Short) && (token2 != CsLexerToken.Void))
            {
                goto Label_004A;
            }
        Label_0048:
            return true;
        Label_004A:
            return false;
        }

        protected override bool IsModifier(int token)
        {
            if ((token == 0xae) || (token == 0xa7))
            {
                return true;
            }
            switch (token)
            {
                case 50:
                case 0x33:
                case 0x34:
                case 14:
                case 1:
                case 0xab:
                case 0xae:
                case 0xb0:
                case 0xb1:
                case 0xa7:
                case 0x3e:
                case 0x4f:
                    return true;
            }
            return false;
        }

        protected override int LexComment()
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
                        return 0x9f;
                }
            }
            base.currentPos--;
            return this.LexSymbol();
        }

        protected override int LexNumber()
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

                    case 'l':
                    case 'L':
                        base.currentPos++;
                        return 0xb5;

                    case '.':
                        if (base.currentPos < (length - 1))
                        {
                            ch = base.source[base.currentPos + 1];
                            if ((ch >= '0') && (ch <= '9'))
                            {
                                base.currentPos++;
                                this.LexNum();
                                num3 = 0x99;
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

        protected override int LexString()
        {
            char ch = base.source[base.currentPos];
            if ((ch != '@') || (((base.currentPos + 1) < base.source.Length) && (base.source[base.currentPos + 1] == '"')))
            {
                return base.LexString();
            }
            base.currentPos++;
            return 180;
        }

        protected override int LexSymbol()
        {
            char ch = base.source[base.currentPos];
            base.currentPos++;
            char ch2 = ch;
            if ((ch2 == '>') && (base.CurChar() == '>'))
            {
                base.currentPos++;
                switch (base.CurChar())
                {
                    case '=':
                        base.currentPos++;
                        return 0x91;

                    case '>':
                        base.currentPos++;
                        if (base.CurChar() != '=')
                        {
                            return 0xb3;
                        }
                        base.currentPos++;
                        return 0xb2;
                }
                return 0x84;
            }
            base.currentPos--;
            return base.LexSymbol();
        }

        protected virtual bool ParseAnnotationInterface(ISyntaxAttributes attrs)
        {
            string str;
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(((attrs != null) && (attrs.Count > 0)) ? attrs[0].Position : this.TokenPosition, this.TokenString, 10);
            if (attrs != null)
            {
                node.AddAttributes(attrs);
            }
            if (!base.Expected(180) || !base.Expected(0x25))
            {
                flag = false;
            }
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
                node.AddAttribute(new SyntaxAttribute(base.prevPosition, SyntaxConsts.DeclarationScope, null));
                if (!this.ParseDeclarationBody(node, 10))
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

        protected virtual bool ParseAssertStatement()
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, this.TokenString, 0x55);
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
            if (flag && (this.Token == 0x6f))
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
            if (!base.Expected(CsLexerToken.Semicolon))
            {
                flag = false;
            }
            node.Range.EndPoint = base.prevPosition;
            return flag;
        }

        protected override bool ParseBaseList(out string types)
        {
            bool flag = true;
            if (this.ParseType(out types))
            {
                string str;
                if (this.IsBaseList(this.Token))
                {
                    this.MoveNext();
                    if (this.ParseType(out str))
                    {
                        types = types + "," + str;
                    }
                    else
                    {
                        flag = false;
                    }
                }
                while (this.Token == 110)
                {
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

        protected virtual bool ParseBound(out string types)
        {
            bool flag = true;
            if (this.ParseType(out types))
            {
                while (this.Token == 0x79)
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

        protected override bool ParseBreakStatement()
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, this.TokenString, 0x5b);
            this.AddNode(node);
            this.MoveNext();
            this.TryParseIdentifier(node);
            if (!base.Expected(CsLexerToken.Semicolon))
            {
                flag = false;
            }
            node.Range.EndPoint = base.prevPosition;
            return flag;
        }

        protected override bool ParseCatchStatement()
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, this.TokenString, 0x61, (this.SyntaxTree.Current.NodeType != 0x60) ? SyntaxNodeOptions.Indentation : SyntaxNodeOptions.BackIndentation);
            this.AddNode(node);
            this.SyntaxTree.Push(node);
            try
            {
                this.MoveNext();
                if (base.Expected(CsLexerToken.Open_parens))
                {
                    if (!this.ParseFormalParameter())
                    {
                        flag = false;
                    }
                    if (!base.Expected(CsLexerToken.Close_parens))
                    {
                        flag = false;
                    }
                }
                else
                {
                    flag = false;
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

        protected override bool ParseClassBody()
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
                if ((this.Token == 0xa9) || (this.Token == 0xac))
                {
                    if (!this.ParseKnownMemberDeclaration(attrs))
                    {
                        flag = false;
                    }
                    attrs = null;
                    continue;
                }
                int token = this.Token;
                if (token <= 0x16)
                {
                    switch (token)
                    {
                        case 13:
                        case 0x12:
                        case 0x16:
                            goto Label_00B3;
                    }
                    goto Label_00D2;
                }
                if (token <= 0x67)
                {
                    switch (token)
                    {
                        case 0x25:
                            goto Label_00B3;

                        case 0x67:
                            goto Label_00C2;
                    }
                    goto Label_00D2;
                }
                if (token == 0xa2)
                {
                    if (!this.ParseDirective())
                    {
                        flag = false;
                    }
                    continue;
                }
                if (token != 180)
                {
                    goto Label_00D2;
                }
            Label_00B3:
                if (!this.ParseKnownMemberDeclaration(attrs))
                {
                    flag = false;
                }
                attrs = null;
                continue;
            Label_00C2:
                if (!this.ParseBlockStatement(attrs, SyntaxNodeOptions.Indentation))
                {
                    flag = false;
                }
                attrs = null;
                continue;
            Label_00D2:
                if (!this.ParseUnknownMemberDeclaration(attrs))
                {
                    flag = false;
                }
                attrs = null;
            }
            return flag;
        }

        protected override bool ParseContinueStatement()
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, this.TokenString, 0x5c);
            this.AddNode(node);
            this.MoveNext();
            this.TryParseIdentifier(node);
            if (!base.Expected(CsLexerToken.Semicolon))
            {
                flag = false;
            }
            node.Range.EndPoint = base.prevPosition;
            return flag;
        }

        protected virtual bool ParseElementValue(ref ISyntaxNode node)
        {
            if (this.IsAnnotationStart())
            {
                bool flag = true;
                bool isAnnotation = false;
                if (!this.TryParseAnnotation(ref node, ref isAnnotation))
                {
                    flag = false;
                }
                if (isAnnotation)
                {
                    return flag;
                }
            }
            if (this.Token != 0x67)
            {
                return this.ParseExpression(ref node);
            }
            this.MoveNext();
            return (this.ParseElementValueList(ref node) && base.Expected(0x68));
        }

        protected virtual bool ParseElementValueList(ref ISyntaxNode node)
        {
            node = new SyntaxNode(this.TokenPosition, string.Empty, 0x83);
            ISyntaxNode node2 = null;
            bool flag = (this.Token == 110) || this.ParseElementValue(ref node2);
            if (node2 != null)
            {
                node.AddChild(node2);
            }
            while (this.Token == 110)
            {
                this.MoveNext();
                node2 = null;
                if ((this.Token != 110) && !this.ParseElementValue(ref node2))
                {
                    flag = false;
                }
                if (node2 != null)
                {
                    node.AddChild(node2);
                }
            }
            return flag;
        }

        protected virtual bool ParseElementValuePair(ref ISyntaxNode node)
        {
            bool flag = true;
            Point tokenPosition = this.TokenPosition;
            bool flag2 = false;
            node = null;
            this.SaveState();
            try
            {
                string str;
                if (this.ParseIdentifier(out str))
                {
                    node = new SyntaxNode(tokenPosition, str, 0x54);
                    if (this.Token == 0x76)
                    {
                        this.MoveNext();
                        flag2 = true;
                    }
                }
            }
            finally
            {
                this.RestoreState(!flag2);
            }
            ISyntaxNode node2 = null;
            if (!this.ParseElementValue(ref node2))
            {
                flag = false;
            }
            if (node2 != null)
            {
                if (node != null)
                {
                    node.AddChild(node2);
                    return flag;
                }
                node = node2;
            }
            return flag;
        }

        protected override bool ParseEmbeddedStatement()
        {
            switch (this.Token)
            {
                case 13:
                    return this.ParseKnownMemberDeclaration(null);

                case 0xa5:
                    return this.ParseAssertStatement();

                case 0xae:
                    return this.ParseSynchronizedStatement();
            }
            return base.ParseEmbeddedStatement();
        }

        protected override bool ParseEnumBody()
        {
            bool flag = true;
            while (!this.Eof && (this.Token != 0x68))
            {
                if (!this.ParseEnumMember())
                {
                    flag = false;
                }
                if (this.Token == 0x71)
                {
                    this.MoveNext();
                    ISyntaxNode node = new SyntaxNode(this.TokenPosition, string.Empty, 0xc6, SyntaxNodeOptions.None);
                    this.AddNode(node);
                    try
                    {
                        this.SyntaxTree.Push(node);
                        if (!this.ParseClassBody())
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

        protected override bool ParseEnumMember()
        {
            ISyntaxAttributes attrs = null;
            string str;
            this.ParseModifiers(ref attrs);
            bool flag = true;
            Point tokenPosition = this.TokenPosition;
            flag = this.ParseIdentifier(out str);
            if (flag)
            {
                ISyntaxNode node = new SyntaxNode(tokenPosition, str, 13, SyntaxNodeOptions.CodeCompletion);
                this.AddNode(node);
                if (attrs != null)
                {
                    node.AddAttributes(attrs);
                }
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
                if (this.Token == 0x6b)
                {
                    this.MoveNext();
                    if (this.Token != 0x6c)
                    {
                        ISyntaxNode node3 = null;
                        if (!this.ParseExpressionList(ref node3))
                        {
                            flag = false;
                        }
                        if (node3 != null)
                        {
                            node.AddChild(node3);
                        }
                    }
                    if (!base.Expected(0x6c))
                    {
                        flag = false;
                    }
                }
                if (this.Token == 0x67)
                {
                    this.MoveNext();
                    if (!this.ParseClassBody())
                    {
                        flag = false;
                    }
                    if (!base.Expected(0x68))
                    {
                        flag = false;
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

        protected virtual bool ParseExpressionList(ref ISyntaxNode node)
        {
            node = new SyntaxNode(this.TokenPosition, string.Empty, 0x83);
            ISyntaxNode node2 = null;
            bool flag = (this.Token == 110) || this.ParseExpression(ref node2);
            if (node2 != null)
            {
                node.AddChild(node2);
            }
            while (this.Token == 110)
            {
                this.MoveNext();
                node2 = null;
                if ((this.Token != 110) && !this.ParseExpression(ref node2))
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

        protected virtual bool ParseFormalParameter()
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, string.Empty, 0x1a, SyntaxNodeOptions.CodeCompletion);
            ISyntaxAttributes attrs = null;
            this.SyntaxTree.Push(node);
            try
            {
                this.ParseModifiers(ref attrs);
            }
            finally
            {
                this.SyntaxTree.Pop();
            }
            if (attrs != null)
            {
                node.AddAttributes(attrs);
            }
            if (!this.ParseVariableDeclaration(this.TokenPosition, NetNodeType.FixedVariable))
            {
                flag = false;
            }
            return flag;
        }

        protected override bool ParseForStatement()
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, this.TokenString, 0x4a, SyntaxNodeOptions.Indentation);
            this.AddNode(node);
            this.SyntaxTree.Push(node);
            try
            {
                this.MoveNext();
                if (base.Expected(CsLexerToken.Open_parens))
                {
                    if (!this.TryParseForVarControl())
                    {
                        if ((this.Token != 0x71) && !this.ParseForInitializer())
                        {
                            flag = false;
                        }
                        if (base.Expected(CsLexerToken.Semicolon))
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
                        if (base.Expected(CsLexerToken.Semicolon))
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
                    }
                    if (!base.Expected(CsLexerToken.Close_parens))
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

        protected virtual bool ParseForVarControlRest()
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, this.TokenString, 0x51);
            this.AddNode(node);
            this.SyntaxTree.Push(node);
            try
            {
                if (this.Token == 0x6f)
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
                    if ((this.Token != 0x71) && !this.ParseVariableDeclaratorsRest())
                    {
                        flag = false;
                    }
                    if (base.Expected(CsLexerToken.Semicolon))
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
                    if (base.Expected(CsLexerToken.Semicolon))
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
                }
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
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, string.Empty, 2, SyntaxNodeOptions.Outlining);
            this.AddNode(node);
            return this.ParseImportDeclaration(node);
        }

        protected virtual bool ParseImportDeclaration(ISyntaxNode node)
        {
            bool flag = true;
            node.AddAttribute(new SyntaxAttribute(this.TokenPosition, SyntaxConsts.BlockScope, null));
            node.AddAttribute(new SyntaxAttribute(this.TokenPosition, SyntaxConsts.OutlineText, SyntaxParserConsts.OutlineImportText));
            if (!this.BeforeDeclaration(node))
            {
                flag = false;
            }
            this.SyntaxTree.Push(node);
            try
            {
                while (this.Token == 0xa9)
                {
                    string str;
                    this.MoveNext();
                    Point tokenPosition = this.TokenPosition;
                    if (this.ParseImportIdentifier(out str) || (str != string.Empty))
                    {
                        ISyntaxNode node2 = new SyntaxNode(tokenPosition, str, 3);
                        this.AddNode(node2);
                        if (this.Token == 0x76)
                        {
                            this.MoveNext();
                            Point position = this.TokenPosition;
                            if (this.ParseQualifiedIdentifier(out str))
                            {
                                node2.AddAttribute(new SyntaxAttribute(position, NetNodeType.UsingAlias.ToString(), str));
                            }
                            else
                            {
                                flag = false;
                            }
                        }
                        if (base.Expected(CsLexerToken.Semicolon))
                        {
                            flag = false;
                        }
                        node2.Range.EndPoint = base.prevPosition;
                    }
                    else
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

        protected virtual bool ParseImportIdentifier(out string identifier)
        {
            identifier = this.TokenString;
            if (!base.Expected(CsLexerToken.Identifier_Literal))
            {
                return false;
            }
            while (this.Token == 0x6d)
            {
                this.MoveNext();
                if (this.Token == 0x7b)
                {
                    this.MoveNext();
                    return true;
                }
                identifier = identifier + ".";
                string tokenString = this.TokenString;
                if (base.Expected(CsLexerToken.Identifier_Literal))
                {
                    identifier = identifier + tokenString;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        protected override bool ParseKnownMemberDeclaration(ISyntaxAttributes attrs)
        {
            bool flag = true;
            switch (this.Token)
            {
                case 0xa9:
                    if (!this.ParseImportDeclaration())
                    {
                        flag = false;
                    }
                    return flag;

                case 0xac:
                    if (!this.ParseDeclaration(attrs, 7))
                    {
                        flag = false;
                    }
                    return flag;

                case 180:
                    return this.ParseAnnotationInterface(attrs);
            }
            return base.ParseKnownMemberDeclaration(attrs);
        }

        protected override bool ParseMemberAccess(ref ISyntaxNode node, int nodeType)
        {
            bool flag = true;
            node = this.CreateExpressionNode(this.TokenPosition, this.TokenString, nodeType, node, true);
            Point currentPosition = this.CurrentPosition;
            this.MoveNext();
            if (this.Token == 13)
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

        protected override bool ParseMethodDeclaration(ISyntaxNode node)
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
                node.AddAttribute(new SyntaxAttribute(base.prevPosition, SyntaxConsts.DeclarationScope, null));
                if (this.Token == 0xaf)
                {
                    string str;
                    this.MoveNext();
                    Point tokenPosition = this.TokenPosition;
                    if (this.ParseThrowsList(out str))
                    {
                        node.AddAttribute(new SyntaxAttribute(tokenPosition, NetNodeType.ThrowsList.ToString(), str));
                    }
                    else
                    {
                        flag = false;
                    }
                }
                bool flag2 = this.Token == 0x71;
                if (flag2)
                {
                    this.MoveNext();
                }
                if (this.Token == 0x11)
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
                if (flag2)
                {
                    if (this.Token == 0x71)
                    {
                        this.MoveNext();
                    }
                }
                else if (!this.ParseMethodBody())
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

        protected override bool ParseModifiers(ref ISyntaxAttributes attrs)
        {
            bool flag = false;
            while (this.IsModifier(this.Token) || this.IsAnnotationStart())
            {
                flag = true;
                if (attrs == null)
                {
                    attrs = new SyntaxAttributes();
                }
                if (this.IsAnnotationStart())
                {
                    bool isAnnotation = false;
                    ISyntaxNode node = null;
                    if (!this.TryParseAnnotation(ref node, ref isAnnotation))
                    {
                        flag = false;
                    }
                    if (!isAnnotation)
                    {
                        return flag;
                    }
                    if (node != null)
                    {
                        this.AddNode(node);
                    }
                }
                else
                {
                    attrs.Add(new SyntaxAttribute(this.TokenPosition, NetNodeType.Modifier.ToString(), this.TokenString));
                    this.MoveNext();
                }
            }
            return flag;
        }

        protected override bool ParseParameterDeclaration()
        {
            string str;
            string str2;
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, string.Empty, 0x1a, SyntaxNodeOptions.CodeCompletion);
            ISyntaxAttributes attrs = null;
            this.SyntaxTree.Push(node);
            try
            {
                this.ParseModifiers(ref attrs);
            }
            finally
            {
                this.SyntaxTree.Pop();
            }
            if (attrs != null)
            {
                node.AddAttributes(attrs);
            }
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
                this.AddNode(node);
            }
            else
            {
                flag = false;
            }
            node.Range.EndPoint = base.prevPosition;
            return flag;
        }

        protected override bool ParseRelationalExpression(ref ISyntaxNode node)
        {
            bool flag = this.ParseShiftExpression(ref node);
            int token = this.Token;
            if (token <= 0x27)
            {
                switch (token)
                {
                    case 2:
                    case 0x27:
                        goto Label_00AB;
                }
                return flag;
            }
            switch (token)
            {
                case 0x77:
                case 120:
                case 0x85:
                case 0x86:
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
                case 170:
                    break;

                default:
                    return flag;
            }
        Label_00AB:
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

        protected override bool ParseShiftExpression(ref ISyntaxNode node)
        {
            bool flag = base.ParseShiftExpression(ref node);
            if (this.Token == 0xb3)
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
            }
            return flag;
        }

        protected override bool ParseSimpleExpression(ref ISyntaxNode node)
        {
            if (this.Token == 0xad)
            {
                return this.ParseSuperAccess(ref node);
            }
            if (this.Token == 0xb5)
            {
                node = new SyntaxNode(this.TokenPosition, this.TokenString, 0x99);
                this.MoveNext();
                return true;
            }
            return base.ParseSimpleExpression(ref node);
        }

        protected override bool ParseStatement()
        {
            if (this.Token == 0xa7)
            {
                return this.ParseLocalConstantDeclarationStatement();
            }
            return base.ParseStatement();
        }

        protected virtual bool ParseSuperAccess(ref ISyntaxNode node)
        {
            bool flag = true;
            node = new SyntaxNode(this.TokenPosition, this.TokenString, 0x9d);
            this.MoveNext();
            switch (this.Token)
            {
                case 0x6b:
                case 0x6d:
                    break;

                default:
                    this.SyntaxError();
                    flag = false;
                    break;
            }
            node.Range.EndPoint = base.prevPosition;
            return flag;
        }

        protected virtual bool ParseSynchronizedStatement()
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, this.TokenString, 0x56, SyntaxNodeOptions.Indentation);
            this.AddNode(node);
            this.SyntaxTree.Push(node);
            try
            {
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
                    if (!base.Expected(CsLexerToken.Close_parens))
                    {
                        flag = false;
                    }
                }
                if ((this.Token == 0x67) && !this.ParseBlock())
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

        protected virtual bool ParseThrowsList(out string throws)
        {
            bool flag = true;
            if (this.ParseType(out throws))
            {
                while (this.Token == 110)
                {
                    string str;
                    throws = throws + this.TokenString;
                    this.MoveNext();
                    if (this.ParseType(out str))
                    {
                        throws = throws + str;
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

        protected override bool ParseTypeArgumentList(out string typeList)
        {
            string str;
            typeList = string.Empty;
            bool flag = base.Expected(CsLexerToken.Op_lt);
            if (!flag)
            {
                return flag;
            }
            typeList = "<";
            if (this.Token == 0x7f)
            {
                this.MoveNext();
                typeList = typeList + this.TokenString;
                if ((this.Token == 0xa6) || (this.Token == 0xad))
                {
                    typeList = typeList + this.TokenString;
                    this.MoveNext();
                }
                else
                {
                    flag = false;
                }
            }
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
            if (base.Expected(CsLexerToken.Op_gt))
            {
                typeList = typeList + ">";
                return flag;
            }
            return false;
        }

        protected override bool ParseTypeParameter()
        {
            string str;
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, string.Empty, 0x34);
            this.AddNode(node);
            if (this.ParseIdentifier(out str))
            {
                node.Name = str;
            }
            else
            {
                flag = false;
            }
            if ((this.Token == 0xa6) || (this.Token == 0xa8))
            {
                this.MoveNext();
                Point tokenPosition = this.TokenPosition;
                string types = string.Empty;
                this.ParseBound(out types);
                if (types != string.Empty)
                {
                    node.AddAttribute(new SyntaxAttribute(tokenPosition, NetNodeType.Type.ToString(), types));
                }
            }
            return flag;
        }

        protected override bool ParseUnknownMemberDeclaration(ISyntaxAttributes attrs)
        {
            string str;
            string str2;
            bool flag = true;
            Point tokenPosition = this.TokenPosition;
            if ((this.Token == 0x77) && !this.ParseTypeParameterList())
            {
                flag = false;
            }
            if (!this.ParseType(out str))
            {
                flag = false;
                if (this.TokenPosition.Equals(tokenPosition))
                {
                    this.MoveNext();
                }
                return flag;
            }
            CsLexerToken token = (CsLexerToken) this.Token;
            if (token != CsLexerToken.Open_parens)
            {
                if (token != CsLexerToken.Identifier_Literal)
                {
                    this.SyntaxError();
                    return false;
                }
                Point point1 = this.TokenPosition;
                if (!this.ParseQualifiedIdentifier(out str2))
                {
                    return flag;
                }
                CsLexerToken token2 = (CsLexerToken) this.Token;
                if (token2 > CsLexerToken.Comma)
                {
                    if ((token2 != CsLexerToken.Semicolon) && (token2 != CsLexerToken.Assign))
                    {
                        goto Label_00C9;
                    }
                    goto Label_00B9;
                }
                switch (token2)
                {
                    case CsLexerToken.Open_bracket:
                    case CsLexerToken.Comma:
                        goto Label_00B9;

                    case CsLexerToken.Open_parens:
                        if (!this.ParseMethodDeclaration(attrs, str, tokenPosition, str2))
                        {
                            flag = false;
                        }
                        return flag;
                }
                goto Label_00C9;
            }
            if (!this.ParseConstructorDeclaration(attrs, str, tokenPosition))
            {
                flag = false;
            }
            return flag;
        Label_00B9:
            if (!this.ParseFieldDeclaration(attrs, str, tokenPosition, str2))
            {
                flag = false;
            }
            return flag;
        Label_00C9:
            this.SyntaxError();
            return false;
        }

        protected virtual bool ParseVariableDeclaratorsRest()
        {
            bool flag = true;
            if (!this.ParseOptionalExressionInBracket())
            {
                flag = false;
            }
            if (!this.ParseVariableDeclarators(NetNodeType.Parameter, false))
            {
                flag = false;
            }
            return flag;
        }

        protected override bool SkipTo(int token1, int token2)
        {
            if ((token1 != 0xac) && (token2 != 0xac))
            {
                while ((!this.Eof && (this.Token != token1)) && (this.Token != token2))
                {
                    this.SyntaxError();
                    this.MoveNext();
                    if (((this.Token >= 1) && (this.Token <= 0x66)) || ((this.Token >= 0xa4) && (this.Token <= 0xb1)))
                    {
                        break;
                    }
                }
            }
            else
            {
                this.MoveNext();
                return !this.Eof;
            }
            if (this.Token != token1)
            {
                return (this.Token == token2);
            }
            return true;
        }

        protected override bool SkipToDeclarationStart(int nodeType)
        {
            if ((nodeType == 7) && (this.Token == 0x71))
            {
                this.MoveNext();
                return false;
            }
            return base.SkipToDeclarationStart(nodeType);
        }

        protected virtual bool TryParseAnnotation(ref ISyntaxNode node, ref bool isAnnotation)
        {
            bool flag = true;
            isAnnotation = true;
            this.SaveState();
            try
            {
                node = new SyntaxNode(this.TokenPosition, string.Empty, 0x53);
                if (this.Token == 180)
                {
                    Point tokenPosition = this.TokenPosition;
                    this.MoveNext();
                    if (this.Token == 0x9e)
                    {
                        Point position = this.TokenPosition;
                        string type = string.Empty;
                        if (this.ParseType(out type))
                        {
                            node.AddAttribute(new SyntaxAttribute(position, NetNodeType.Type.ToString(), type));
                        }
                        else
                        {
                            flag = false;
                        }
                        if (base.Expected(0x6b))
                        {
                            while (!this.Eof)
                            {
                                ISyntaxNode node2 = null;
                                if (!this.ParseElementValuePair(ref node2))
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
                            if (!base.Expected(CsLexerToken.Close_parens))
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
                        isAnnotation = false;
                    }
                }
                else
                {
                    flag = false;
                }
                node.Range.EndPoint = base.prevPosition;
            }
            finally
            {
                this.RestoreState(!isAnnotation);
            }
            return flag;
        }

        protected override bool TryParseAssignmentExpression(ref ISyntaxNode node)
        {
            bool flag = true;
            if (this.Token != 0xb2)
            {
                return base.TryParseAssignmentExpression(ref node);
            }
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

        protected virtual bool TryParseForVarControl()
        {
            bool flag = false;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, this.TokenString, 80);
            this.SyntaxTree.Push(node);
            try
            {
                string str;
                string str2;
                ISyntaxAttributes attrs = null;
                this.ParseModifiers(ref attrs);
                if (attrs != null)
                {
                    node.AddAttributes(attrs);
                }
                Point tokenPosition = this.TokenPosition;
                if (this.ParseType(out str) && this.ParseIdentifier(out str2))
                {
                    flag = true;
                    node.Name = str2;
                    node.AddAttribute(new SyntaxAttribute(tokenPosition, NetNodeType.Type.ToString(), str));
                    if (!this.ParseForVarControlRest())
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
            if (flag)
            {
                this.AddNode(node);
            }
            node.Range.EndPoint = base.prevPosition;
            return flag;
        }

        protected void TryParseIdentifier(ISyntaxNode node)
        {
            string identifier = string.Empty;
            bool flag = false;
            this.SaveState();
            try
            {
                flag = this.ParseIdentifier(out identifier);
            }
            finally
            {
                this.RestoreState(!flag);
            }
            if (flag)
            {
                node.AddAttribute(new SyntaxAttribute(base.prevPosition, NetNodeType.Name.ToString(), identifier));
            }
        }
    }
}

