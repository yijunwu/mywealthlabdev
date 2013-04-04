namespace QWhale.Syntax.Parsers
{
    using QWhale.Common;
    using QWhale.Syntax;
    using QWhale.Syntax.CodeCompletion;
    using System;
    using System.Drawing;
    using System.Runtime.InteropServices;

    [ToolboxBitmap(typeof(JScriptNETParser), "Images.JScriptNETParser.bmp")]
    public class JScriptNETParser : JavaScriptParser
    {
        protected override bool AfterDeclaration(ISyntaxNode node)
        {
            bool flag = true;
            NetNodeType nodeType = (NetNodeType) this.SyntaxTree.Current.NodeType;
            switch (((NetNodeType) node.NodeType))
            {
                case NetNodeType.Method:
                case NetNodeType.PropertyAccessor:
                {
                    ISyntaxAttribute attribute = node.FindAttribute(SyntaxConsts.DefinitionScope);
                    if (attribute != null)
                    {
                        node.Options |= SyntaxNodeOptions.Outlining;
                        if (nodeType == NetNodeType.Interface)
                        {
                            node.AddError(new SyntaxError(attribute.Position, attribute.Name, StringConsts.ErrInterfaceMemberDeclaration));
                            flag = false;
                        }
                    }
                    break;
                }
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

        protected override bool BeforeDeclaration(ISyntaxNode node)
        {
            bool flag = true;
            NetNodeType nodeType = (NetNodeType) this.SyntaxTree.Current.NodeType;
            switch (((NetNodeType) node.NodeType))
            {
                case NetNodeType.UsingList:
                case NetNodeType.Using:
                    if (((nodeType != NetNodeType.Unit) && (nodeType != NetNodeType.Namespace)) && (this.SyntaxTree.Current != this.SyntaxTree.Root))
                    {
                        flag = false;
                        this.SyntaxError();
                    }
                    return flag;

                case NetNodeType.UsingAlias:
                case NetNodeType.Alias:
                case NetNodeType.AliasList:
                case NetNodeType.Struct:
                case NetNodeType.Module:
                    return flag;

                case NetNodeType.Namespace:
                    switch (nodeType)
                    {
                        case NetNodeType.Unit:
                        case NetNodeType.Namespace:
                            if (node.HasAttributes)
                            {
                                foreach (ISyntaxAttribute attribute in node.AttributeList)
                                {
                                    if ((attribute.Name == NetNodeType.Modifier.ToString()) && (attribute.Value is string))
                                    {
                                        this.SyntaxError(StringConsts.ErrNamespaceModifiers);
                                        return false;
                                    }
                                }
                            }
                            return flag;
                    }
                    this.SyntaxError();
                    return flag;

                case NetNodeType.Class:
                case NetNodeType.Interface:
                case NetNodeType.Enum:
                    if (nodeType == NetNodeType.Interface)
                    {
                        this.SyntaxError(node.Position, this.TokenPosition, node.Name, StringConsts.ErrInterfaceDeclaration);
                        flag = false;
                    }
                    return flag;

                case NetNodeType.Field:
                    if ((nodeType != NetNodeType.Namespace) && (nodeType != NetNodeType.Unit))
                    {
                        if (nodeType == NetNodeType.Interface)
                        {
                            this.SyntaxError(node.Position, base.prevPosition, node.Name, StringConsts.ErrInterfaceDeclaration);
                            flag = false;
                        }
                        return flag;
                    }
                    this.SyntaxError(node.Position, base.prevPosition, node.Name, StringConsts.ErrNamespaceDeclaration);
                    return false;

                case NetNodeType.Method:
                case NetNodeType.Property:
                    if ((nodeType != NetNodeType.Namespace) && (nodeType != NetNodeType.Unit))
                    {
                        return flag;
                    }
                    this.SyntaxError(node.Position, base.prevPosition, node.Name, StringConsts.ErrNamespaceDeclaration);
                    return false;
            }
            return flag;
        }

        public override ICodeCompletionRepository CreateRepository()
        {
            ReflectionRepository repository = new ReflectionRepository(this.CaseSensitive, this.SyntaxTree);
            repository.RegisterDefaultAssemblies();
            return repository;
        }

        public override CodeCompletionType GetCompletionType(char ch)
        {
            if (ch == ' ')
            {
                return CodeCompletionType.SpecialListMembers;
            }
            return base.GetCompletionType(ch);
        }

        protected override void InitLanguage()
        {
            this.Scheme.FileType = "jscript.NET";
        }

        protected override void InitReswords()
        {
            base.InitReswords();
            base.reswords.Add("internal", JavaScriptLexerToken.Internal);
            base.reswords.Add("infinity", JavaScriptLexerToken.Infinity);
            base.reswords.Add("override", JavaScriptLexerToken.Override);
            base.reswords.Add("hide", JavaScriptLexerToken.Hide);
            base.reswords.Add("expando", JavaScriptLexerToken.Expando);
            base.reswords.Add("get", JavaScriptLexerToken.Get);
            base.reswords.Add("set", JavaScriptLexerToken.Set);
            base.reswords.Add("undefined", JavaScriptLexerToken.Undefined);
            base.reswords.Add("decimal", JavaScriptLexerToken.Decimal);
            base.reswords.Add("uint", JavaScriptLexerToken.Uint);
            base.reswords.Add("ulong", JavaScriptLexerToken.Ulong);
            base.reswords.Add("ushort", JavaScriptLexerToken.Ushort);
        }

        protected override bool IsBuiltInType(int token)
        {
            switch (this.Token)
            {
                case 0x4c:
                case 0x4d:
                case 0x4e:
                case 0x4f:
                case 80:
                    return true;
            }
            return base.IsBuiltInType(token);
        }

        protected virtual bool IsModifier(int token)
        {
            switch (((JavaScriptLexerToken) token))
            {
                case JavaScriptLexerToken.Abstract:
                case JavaScriptLexerToken.Final:
                case JavaScriptLexerToken.Private:
                case JavaScriptLexerToken.Protected:
                case JavaScriptLexerToken.Public:
                case JavaScriptLexerToken.Static:
                case JavaScriptLexerToken.Internal:
                case JavaScriptLexerToken.Override:
                case JavaScriptLexerToken.Hide:
                case JavaScriptLexerToken.Expando:
                    return true;
            }
            return false;
        }

        protected override int LexSymbol()
        {
            char ch = base.source[base.currentPos];
            base.currentPos++;
            switch (ch)
            {
                case '<':
                    if (base.CurChar() != '<')
                    {
                        break;
                    }
                    base.currentPos++;
                    if (base.CurChar() != '=')
                    {
                        return 0x7a;
                    }
                    base.currentPos++;
                    return 0x7c;

                case '>':
                    if (base.CurChar() != '>')
                    {
                        break;
                    }
                    base.currentPos++;
                    switch (base.CurChar())
                    {
                        case '=':
                            base.currentPos++;
                            return 0x7d;

                        case '>':
                            base.currentPos++;
                            if (base.CurChar() == '=')
                            {
                                base.currentPos++;
                                return 0x7f;
                            }
                            return 0x7e;
                    }
                    return 0x7b;

                case '?':
                    return 0x5b;

                case '^':
                    if (base.CurChar() == '=')
                    {
                        base.currentPos++;
                        return 0x79;
                    }
                    return 0x75;

                case '|':
                    switch (base.CurChar())
                    {
                        case '=':
                            base.currentPos++;
                            return 0x77;

                        case '|':
                            base.currentPos++;
                            return 0x6d;
                    }
                    return 0x74;

                case '!':
                    if (base.CurChar() == '=')
                    {
                        base.currentPos++;
                        if (base.CurChar() == '=')
                        {
                            base.currentPos++;
                            return 0x67;
                        }
                        return 0x6a;
                    }
                    break;

                case '&':
                    switch (base.CurChar())
                    {
                        case '&':
                            base.currentPos++;
                            return 0x6c;

                        case '=':
                            base.currentPos++;
                            return 120;
                    }
                    return 0x76;
            }
            base.currentPos--;
            return base.LexSymbol();
        }

        protected virtual bool ParseAndExpression(ref ISyntaxNode node)
        {
            bool flag = this.ParseEqualityExpression(ref node);
            if (this.Token == 0x76)
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

        protected virtual bool ParseBaseClass()
        {
            string str;
            this.MoveNext();
            Point tokenPosition = this.TokenPosition;
            bool flag = this.ParseQualifiedIdentifier(out str);
            if (flag)
            {
                this.AddAttribute(new SyntaxAttribute(tokenPosition, NetNodeType.TypeList.ToString(), str));
            }
            return flag;
        }

        protected virtual bool ParseBaseList()
        {
            string str;
            this.MoveNext();
            Point tokenPosition = this.TokenPosition;
            bool flag = this.ParseQualifiedIdentifier(out str);
            if (flag)
            {
                ISyntaxNode current = this.SyntaxTree.Current;
                ISyntaxAttribute attribute = current.FindAttribute(NetNodeType.TypeList.ToString());
                string str2 = (attribute != null) ? ((string) attribute.Value) : string.Empty;
                while (this.Token == 0x58)
                {
                    this.MoveNext();
                    if (this.ParseQualifiedIdentifier(out str))
                    {
                        str2 = (str2 == string.Empty) ? str : (str2 + "," + str);
                    }
                    else
                    {
                        flag = false;
                    }
                }
                if (!(str2 != string.Empty))
                {
                    return flag;
                }
                if (attribute != null)
                {
                    attribute.Value = str2;
                    return flag;
                }
                current.AddAttribute(new SyntaxAttribute(tokenPosition, NetNodeType.TypeList.ToString(), str2));
            }
            return flag;
        }

        protected virtual bool ParseClassBody(bool block)
        {
            bool flag = true;
            ISyntaxAttributes attrs = null;
            while (!this.Eof)
            {
                if (block && (this.Token == 0x52))
                {
                    return flag;
                }
                this.ParseModifiers(ref attrs);
                JavaScriptLexerToken token = (JavaScriptLexerToken) this.Token;
                if (token <= JavaScriptLexerToken.Import)
                {
                    switch (token)
                    {
                        case JavaScriptLexerToken.Class:
                        case JavaScriptLexerToken.Const:
                        case JavaScriptLexerToken.Enum:
                        case JavaScriptLexerToken.Function:
                        case JavaScriptLexerToken.Import:
                            goto Label_0081;
                    }
                    goto Label_0090;
                }
                if (token <= JavaScriptLexerToken.Package)
                {
                    switch (token)
                    {
                        case JavaScriptLexerToken.Interface:
                        case JavaScriptLexerToken.Package:
                            goto Label_0081;
                    }
                    goto Label_0090;
                }
                if (token != JavaScriptLexerToken.Var)
                {
                    if (token != JavaScriptLexerToken.Directive_Literal)
                    {
                        goto Label_0090;
                    }
                    if (!this.ParseDirective())
                    {
                        flag = false;
                    }
                    continue;
                }
            Label_0081:
                if (!this.ParseKnownMemberDeclaration(attrs))
                {
                    flag = false;
                }
                attrs = null;
                continue;
            Label_0090:
                if (block)
                {
                    this.SyntaxError();
                    this.MoveNext();
                }
                else if (!this.ParseStatementList(block))
                {
                    flag = false;
                }
                attrs = null;
            }
            return flag;
        }

        protected override bool ParseConditionalAndExpression(ref ISyntaxNode node)
        {
            bool flag = this.ParseInclusiveOrExpression(ref node);
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
                node.Range.EndPoint = base.prevPosition;
            }
            return flag;
        }

        protected override bool ParseConditionalExpression(ref ISyntaxNode node)
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
                if (base.Expected(JavaScriptLexerToken.Colon))
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
                node.Range.EndPoint = base.prevPosition;
            }
            return flag;
        }

        protected virtual bool ParseDeclaration(ISyntaxNode node)
        {
            ISyntaxAttributes attrs = null;
            this.ParseModifiers(ref attrs);
            switch (((JavaScriptLexerToken) this.Token))
            {
                case JavaScriptLexerToken.Class:
                    return this.ParseDeclaration(attrs, node, NetNodeType.Class);

                case JavaScriptLexerToken.Enum:
                    return this.ParseDeclaration(attrs, node, NetNodeType.Enum);

                case JavaScriptLexerToken.Interface:
                    return this.ParseDeclaration(attrs, node, NetNodeType.Interface);

                case JavaScriptLexerToken.Package:
                    return this.ParseDeclaration(attrs, node, NetNodeType.Namespace);
            }
            return false;
        }

        protected virtual bool ParseDeclaration(ISyntaxAttributes attrs, NetNodeType nodeType)
        {
            ISyntaxNode node = new SyntaxNode(((attrs != null) && (attrs.Count > 0)) ? attrs[0].Position : this.TokenPosition, this.TokenString, (int) nodeType);
            return this.ParseDeclaration(attrs, node, nodeType);
        }

        protected virtual bool ParseDeclaration(ISyntaxAttributes attrs, ISyntaxNode node, NetNodeType nodeType)
        {
            string str;
            bool flag = true;
            if (attrs != null)
            {
                node.AddAttributes(attrs);
            }
            this.MoveNext();
            if ((nodeType == NetNodeType.Namespace) ? this.ParseQualifiedIdentifier(out str) : this.ParseIdentifier(out str))
            {
                node.Name = str;
                node.Options |= SyntaxNodeOptions.CodeCompletion;
                this.AddNode(node);
                if (!this.BeforeDeclaration(node))
                {
                    flag = false;
                }
                if (nodeType != NetNodeType.Namespace)
                {
                    this.SyntaxTree.Push(node);
                    try
                    {
                        if (nodeType == NetNodeType.Enum)
                        {
                            if ((this.Token == 0x59) && !this.ParseEnumType())
                            {
                                flag = false;
                            }
                        }
                        else
                        {
                            if ((this.Token == 0x15) && !this.ParseBaseClass())
                            {
                                flag = false;
                            }
                            if ((this.Token == 30) && !this.ParseBaseList())
                            {
                                flag = false;
                            }
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

        protected virtual bool ParseDeclarationBody(ISyntaxNode node, NetNodeType nodeType)
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
                        case 10:
                            if (!this.ParseClassBody(true))
                            {
                                flag = false;
                            }
                            goto Label_009A;

                        case 11:
                            if (!this.ParseEnumBody())
                            {
                                flag = false;
                            }
                            goto Label_009A;
                    }
                    flag = false;
                }
            }
            finally
            {
                this.SyntaxTree.Pop();
            }
        Label_009A:
            if (flag2 && base.Expected(JavaScriptLexerToken.Close_brace))
            {
                node.AddAttribute(new SyntaxAttribute(base.prevPosition, SyntaxConsts.DefinitionScopeEnd, null));
                if (this.Token == 90)
                {
                    this.MoveNext();
                }
                return flag;
            }
            return false;
        }

        protected override bool ParseDefaultBlock()
        {
            return this.ParseClassBody(false);
        }

        protected virtual bool ParseDeleteExpression(ref ISyntaxNode node)
        {
            bool flag = true;
            node = new SyntaxNode(this.TokenPosition, this.TokenString, 200);
            ISyntaxNode node2 = null;
            flag = this.ParseExpression(ref node2);
            if (node2 != null)
            {
                node.AddChild(node2);
            }
            node.Range.EndPoint = base.prevPosition;
            return flag;
        }

        protected virtual bool ParseDirective()
        {
            if (this.State == 1)
            {
                this.MoveNext();
            }
            this.MoveNext();
            return true;
        }

        protected override bool ParseEmbeddedStatement()
        {
            if (this.Token == 0x43)
            {
                return this.ParseWithStatement();
            }
            return base.ParseEmbeddedStatement();
        }

        protected virtual bool ParseEnumBody()
        {
            bool flag = true;
            while (!this.Eof && (this.Token != 0x52))
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
                this.SyntaxTree.Push(node);
                try
                {
                    if (this.Token == 0x69)
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
                    if (this.Token == 0x58)
                    {
                        this.MoveNext();
                    }
                }
                finally
                {
                    this.SyntaxTree.Pop();
                }
                node.Range.EndPoint = base.prevPosition;
                return flag;
            }
            this.MoveNext();
            return flag;
        }

        protected virtual bool ParseEnumType()
        {
            string str;
            this.MoveNext();
            Point tokenPosition = this.TokenPosition;
            bool flag = this.ParseTypeName(out str);
            if (flag)
            {
                this.AddAttribute(new SyntaxAttribute(tokenPosition, NetNodeType.Type.ToString(), str));
            }
            return flag;
        }

        protected virtual bool ParseExclusiveOrExpression(ref ISyntaxNode node)
        {
            bool flag = this.ParseAndExpression(ref node);
            if (this.Token == 0x75)
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

        protected virtual bool ParseGetSetFunction(ISyntaxNode node)
        {
            string str;
            this.MoveNext();
            if (this.ParseIdentifier(out str))
            {
                node.Name = str;
                return this.ParseMethodDeclaration(node);
            }
            return false;
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
                if (!this.SemicolonNeeded())
                {
                    flag = false;
                }
                node.Range.EndPoint = base.prevPosition;
                return flag;
            }
            return false;
        }

        protected virtual bool ParseImportList()
        {
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, string.Empty, 2, SyntaxNodeOptions.Outlining);
            return this.ParseImportList(node);
        }

        protected virtual bool ParseImportList(ISyntaxNode node)
        {
            bool flag = true;
            node.AddAttribute(new SyntaxAttribute(this.TokenPosition, SyntaxConsts.OutlineText, SyntaxParserConsts.OutlineImportText));
            this.AddNode(node);
            if (!this.BeforeDeclaration(node))
            {
                flag = false;
            }
            this.SyntaxTree.Push(node);
            try
            {
                while (this.Token == 0x1f)
                {
                    this.MoveNext();
                    this.ParseImportDeclaration();
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
            if (this.Token == 0x74)
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

        protected virtual bool ParseKnownMemberDeclaration(ISyntaxAttributes attrs)
        {
            bool flag = true;
            JavaScriptLexerToken token = (JavaScriptLexerToken) this.Token;
            if (token <= JavaScriptLexerToken.Function)
            {
                switch (token)
                {
                    case JavaScriptLexerToken.Class:
                        if (!this.ParseDeclaration(attrs, NetNodeType.Class))
                        {
                            flag = false;
                        }
                        return flag;

                    case JavaScriptLexerToken.Const:
                    {
                        ISyntaxNode node2 = null;
                        if (!this.ParseVariableDeclaration(attrs, 14, ref node2))
                        {
                            flag = false;
                        }
                        if (!this.SemicolonNeeded())
                        {
                            flag = false;
                        }
                        if (node2 != null)
                        {
                            node2.Range.EndPoint = base.prevPosition;
                        }
                        return flag;
                    }
                    case JavaScriptLexerToken.Enum:
                        if (!this.ParseDeclaration(attrs, NetNodeType.Enum))
                        {
                            flag = false;
                        }
                        return flag;

                    case JavaScriptLexerToken.Function:
                        if (!this.ParseMethodDeclaration(attrs))
                        {
                            flag = false;
                        }
                        return flag;
                }
                return flag;
            }
            if (token <= JavaScriptLexerToken.Interface)
            {
                switch (token)
                {
                    case JavaScriptLexerToken.Import:
                        if (!this.ParseImportList())
                        {
                            flag = false;
                        }
                        return flag;

                    case JavaScriptLexerToken.Interface:
                        if (!this.ParseDeclaration(attrs, NetNodeType.Interface))
                        {
                            flag = false;
                        }
                        return flag;
                }
                return flag;
            }
            switch (token)
            {
                case JavaScriptLexerToken.Package:
                    if (!this.ParseDeclaration(attrs, NetNodeType.Namespace))
                    {
                        flag = false;
                    }
                    return flag;

                case JavaScriptLexerToken.Var:
                {
                    ISyntaxNode node = null;
                    if (!this.ParseVariableDeclaration(attrs, 13, ref node))
                    {
                        flag = false;
                    }
                    if (!this.SemicolonNeeded())
                    {
                        flag = false;
                    }
                    if (node != null)
                    {
                        node.Range.EndPoint = base.prevPosition;
                    }
                    return flag;
                }
            }
            return flag;
        }

        protected virtual bool ParseMethodDeclaration(ISyntaxAttributes attrs)
        {
            string str;
            Point tokenPosition = this.TokenPosition;
            this.MoveNext();
            ISyntaxNode node = new SyntaxNode(((attrs != null) && (attrs.Count > 0)) ? attrs[0].Position : tokenPosition, string.Empty, 0x11, SyntaxNodeOptions.CodeCompletion);
            if (attrs != null)
            {
                node.AddAttributes(attrs);
            }
            switch (this.Token)
            {
                case 0x49:
                case 0x4a:
                    this.AddNode(node);
                    return this.ParseGetSetFunction(node);
            }
            if (this.ParseIdentifier(out str))
            {
                node.Name = str;
                this.AddNode(node);
                return this.ParseMethodDeclaration(node);
            }
            return false;
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

        protected override bool ParseParameterDeclaration()
        {
            string str;
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, string.Empty, 0x1a, SyntaxNodeOptions.CodeCompletion);
            if (this.ParseIdentifier(out str))
            {
                node.Name = str;
                this.AddNode(node);
                if (!this.ParseReturnType(node))
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

        protected override bool ParseRelationalExpression(ref ISyntaxNode node)
        {
            bool flag = this.ParseShiftExpression(ref node);
            switch (((JavaScriptLexerToken) this.Token))
            {
                case JavaScriptLexerToken.Op_le:
                case JavaScriptLexerToken.Op_lt:
                case JavaScriptLexerToken.Op_ge:
                case JavaScriptLexerToken.Op_gt:
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
                case JavaScriptLexerToken.Instanceof:
                {
                    node = this.CreateExpressionNode(this.TokenPosition, this.TokenString, 0xc9, node, true);
                    this.MoveNext();
                    ISyntaxNode node3 = null;
                    if (!this.ParseExpression(ref node3))
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

        protected override bool ParseReturnType()
        {
            return this.ParseReturnType(this.SyntaxTree.Current);
        }

        protected virtual bool ParseReturnType(ISyntaxNode node)
        {
            string str;
            bool flag = true;
            if (this.Token != 0x59)
            {
                return flag;
            }
            this.MoveNext();
            Point tokenPosition = this.TokenPosition;
            if (this.ParseType(out str))
            {
                node.AddAttribute(new SyntaxAttribute(tokenPosition, NetNodeType.Type.ToString(), str));
                return flag;
            }
            return false;
        }

        protected virtual bool ParseShiftExpression(ref ISyntaxNode node)
        {
            bool flag = this.ParseAdditiveExpression(ref node);
            switch (this.Token)
            {
                case 0x7a:
                case 0x7b:
                case 0x7e:
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
                case 0x7c:
                case 0x7d:
                    return flag;
            }
            return flag;
        }

        protected override bool ParseSimpleExpression(ref ISyntaxNode node)
        {
            switch (((JavaScriptLexerToken) this.Token))
            {
                case JavaScriptLexerToken.Delete:
                    return this.ParseDeleteExpression(ref node);

                case JavaScriptLexerToken.Double:
                case JavaScriptLexerToken.Float:
                case JavaScriptLexerToken.Boolean:
                case JavaScriptLexerToken.Byte:
                case JavaScriptLexerToken.Char:
                case JavaScriptLexerToken.Int:
                case JavaScriptLexerToken.Long:
                case JavaScriptLexerToken.Object:
                case JavaScriptLexerToken.Short:
                case JavaScriptLexerToken.String:
                case JavaScriptLexerToken.Void:
                case JavaScriptLexerToken.Undefined:
                case JavaScriptLexerToken.Decimal:
                case JavaScriptLexerToken.Sbyte:
                case JavaScriptLexerToken.Uint:
                case JavaScriptLexerToken.Ulong:
                case JavaScriptLexerToken.Ushort:
                    node = new SyntaxNode(this.TokenPosition, this.TokenString, 0x99);
                    this.MoveNext();
                    return true;

                case JavaScriptLexerToken.Super:
                    return this.ParseSuperAccess(ref node);
            }
            return base.ParseSimpleExpression(ref node);
        }

        protected override bool ParseStatement(bool block)
        {
            bool flag = true;
            bool parsed = false;
            if (this.Token == 0x85)
            {
                flag = this.TryParseLabeledStatement(out parsed);
                if (parsed)
                {
                    return flag;
                }
            }
            return base.ParseStatement(block);
        }

        protected virtual bool ParseSuperAccess(ref ISyntaxNode node)
        {
            bool flag = true;
            node = new SyntaxNode(this.TokenPosition, this.TokenString, 0x9d);
            this.MoveNext();
            JavaScriptLexerToken token = (JavaScriptLexerToken) this.Token;
            if ((token != JavaScriptLexerToken.Open_parens) && (token != JavaScriptLexerToken.Dot))
            {
                this.SyntaxError();
                flag = false;
            }
            node.Range.EndPoint = base.prevPosition;
            return flag;
        }

        protected override bool ParseUnitBody()
        {
            return this.ParseDefaultBlock();
        }

        protected virtual bool ParseWithStatement()
        {
            bool flag = true;
            ISyntaxNode current = this.SyntaxTree.Current;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, this.TokenString, 0x80, ((current.NodeType == 0x67) && (current.FindAttribute(SyntaxConsts.DefinitionScope) == null)) ? SyntaxNodeOptions.BackIndentation : SyntaxNodeOptions.Indentation);
            this.AddNode(node);
            this.SyntaxTree.Push(node);
            try
            {
                this.MoveNext();
                if (base.Expected(JavaScriptLexerToken.Open_parens))
                {
                    ISyntaxNode node3 = null;
                    flag = this.ParseExpression(ref node3);
                    if (node3 != null)
                    {
                        node.AddChild(node3);
                    }
                    if (!base.Expected(JavaScriptLexerToken.Close_parens))
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

        public override bool ReparseBlock(Point position)
        {
            bool flag = false;
            ISyntaxNode blockNode = this.SyntaxTree.FindNode(new SyntaxNode(position, string.Empty), base.pointNodeComparer);
            if (blockNode == null)
            {
                return flag;
            }
            if (blockNode.NodeType == 3)
            {
                return this.ReparseImport(blockNode);
            }
            if (blockNode.NodeType == 2)
            {
                return this.ReparseImportList(blockNode);
            }
            if (NETRepository.IsDeclarationNode(blockNode))
            {
                return this.ReparseDeclaration(blockNode);
            }
            blockNode = NETRepository.GetBlockNode(blockNode);
            return ((blockNode != null) ? this.ReparseBlock(blockNode, position) : false);
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

        protected virtual bool ReparseImport(ISyntaxNode node)
        {
            this.Reset(node.Position.Y, node.Position.X, 0);
            node.Clear();
            this.MoveNext();
            this.ParseImportDeclaration(node);
            return true;
        }

        protected virtual bool ReparseImportList(ISyntaxNode node)
        {
            this.Reset(node.Position.Y, node.Position.X, 0);
            node.Clear();
            this.MoveNext();
            this.ParseImportList(node);
            return true;
        }

        protected virtual bool SkipToDeclarationStart(NetNodeType nodeType)
        {
            return this.SkipTo(0x51);
        }

        protected override bool TryParseAssignmentExpression(ref ISyntaxNode node)
        {
            bool flag = true;
            switch (this.Token)
            {
                case 0x77:
                case 120:
                case 0x79:
                case 0x7c:
                case 0x7d:
                case 0x7f:
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
            return base.TryParseAssignmentExpression(ref node);
        }

        protected virtual bool TryParseLabeledStatement(out bool parsed)
        {
            string str;
            Point tokenPosition = this.TokenPosition;
            parsed = false;
            this.SaveState();
            try
            {
                parsed = this.ParseIdentifier(out str) && base.Expected(JavaScriptLexerToken.Colon);
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
                    flag = this.ParseStatement(true);
                }
                finally
                {
                    this.SyntaxTree.Pop();
                }
                node.Range.EndPoint = base.prevPosition;
            }
            return flag;
        }
    }
}

