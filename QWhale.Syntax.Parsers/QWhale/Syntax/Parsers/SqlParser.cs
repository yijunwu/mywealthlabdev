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

    [ToolboxItem(true), ToolboxBitmap(typeof(SqlParser), "Images.SQLParser.bmp")]
    public class SqlParser : SyntaxParser
    {
        protected LexerProc lexCommentEndProc;
        protected LexerProc lexCommentProc;
        protected LexerProc lexIdentifierProc;
        protected LexerProc lexNumberProc;
        protected LexerProc lexStringProc;
        protected LexerProc lexSymbolProc;
        protected LexerProc lexWhitespaceProc;
        protected Point prevPosition;
        private Hashtable reswords;
        private const int stateComment = 1;
        private const int stateNormal = 0;
        private SqlLexerToken[] tokens;

        protected virtual void AddAttribute(ISyntaxAttribute attr)
        {
            this.SyntaxTree.Current.AddAttribute(attr);
        }

        protected virtual void AddNode(ISyntaxNode node)
        {
            this.SyntaxTree.Current.AddChild(node);
        }

        protected virtual bool ClearStack()
        {
            bool flag = true;
            ISyntaxNode current = this.SyntaxTree.Current;
            while (current != this.SyntaxTree.Root)
            {
                ISyntaxError err = new QWhale.Syntax.SyntaxError(current.Position, current.Name, StringConsts.ErrEndOfFileFound) {
                    Range = { EndPoint = new Point(current.Position.X + 1, current.Position.Y) }
                };
                current.AddError(err);
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

        protected bool Expected(SqlLexerToken token)
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
            return this.Expected((SqlLexerToken) token);
        }

        protected bool Expected(SqlLexerToken[] tokens)
        {
            bool flag = false;
            for (int i = 0; i < tokens.Length; i++)
            {
                if (this.Token == (int)(tokens[i]))
                {
                    flag = true;
                    break;
                }
            }
            if (!flag)
            {
                if (tokens.Length > 0)
                {
                    this.SyntaxError((int) tokens[0]);
                }
                else
                {
                    this.SyntaxError();
                }
            }
            if (flag)
            {
                this.MoveNext();
            }
            return flag;
        }

        protected bool Expected(SqlLexerToken token1, SqlLexerToken token2)
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

        protected override int GetLexerStyle(int token)
        {
            if (this.IsDatatypeToken(token))
            {
                return 8;
            }
            if (this.IsReswordToken(token))
            {
                return 2;
            }
            if ((token >= 0x108) && (token <= 290))
            {
                return 5;
            }
            switch (token)
            {
                case 0x123:
                case 0x124:
                case 0x125:
                    return 1;

                case 0x126:
                    return 7;

                case 0x127:
                    return 0;

                case 0x128:
                    return 3;
            }
            return 6;
        }

        protected bool IdentifierExpected()
        {
            return this.Expected(SqlLexerToken.Identifier_Literal);
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
            base.RegisterLexerProc(0, this.lexWhitespaceProc);
            base.RegisterLexerProc(0, '!', '\x00ff', this.lexSymbolProc);
            base.RegisterLexerProc(0, 'a', 'z', this.lexIdentifierProc);
            base.RegisterLexerProc(0, 'A', 'Z', this.lexIdentifierProc);
            base.RegisterLexerProc(0, new char[] { '"', '_', '$', '#' }, this.lexIdentifierProc);
            base.RegisterLexerProc(0, '0', '9', this.lexNumberProc);
            base.RegisterLexerProc(0, '+', this.lexNumberProc);
            base.RegisterLexerProc(0, new char[] { 'B', 'b', '\'' }, this.lexStringProc);
            base.RegisterLexerProc(0, new char[] { '/', '-' }, this.lexCommentProc);
            base.RegisterLexerProc(1, this.lexCommentEndProc);
        }

        protected virtual void InitReswords()
        {
            this.tokens = new SqlLexerToken[] { SqlLexerToken.Table, SqlLexerToken.View, SqlLexerToken.Sequence, SqlLexerToken.Schema, SqlLexerToken.User, SqlLexerToken.Into, SqlLexerToken.From };
            this.reswords = new Hashtable();
            this.reswords.Add("abort", SqlLexerToken.Abort);
            this.reswords.Add("accept", SqlLexerToken.Accept);
            this.reswords.Add("access", SqlLexerToken.Access);
            this.reswords.Add("account", SqlLexerToken.Account);
            this.reswords.Add("action", SqlLexerToken.Action);
            this.reswords.Add("add", SqlLexerToken.Add);
            this.reswords.Add("all", SqlLexerToken.All);
            this.reswords.Add("alter", SqlLexerToken.Alter);
            this.reswords.Add("and", SqlLexerToken.And);
            this.reswords.Add("any", SqlLexerToken.Any);
            this.reswords.Add("array", SqlLexerToken.Array);
            this.reswords.Add("as", SqlLexerToken.As);
            this.reswords.Add("asc", SqlLexerToken.Asc);
            this.reswords.Add("assert", SqlLexerToken.Assert);
            this.reswords.Add("assign", SqlLexerToken.Assign);
            this.reswords.Add("at", SqlLexerToken.At);
            this.reswords.Add("audit", SqlLexerToken.Audit);
            this.reswords.Add("authorization", SqlLexerToken.Authorization);
            this.reswords.Add("auto", SqlLexerToken.Auto);
            this.reswords.Add("avg", SqlLexerToken.Avg);
            this.reswords.Add("begin", SqlLexerToken.Begin);
            this.reswords.Add("between", SqlLexerToken.Between);
            this.reswords.Add("body", SqlLexerToken.Body);
            this.reswords.Add("by", SqlLexerToken.By);
            this.reswords.Add("case", SqlLexerToken.Case);
            this.reswords.Add("cascade", SqlLexerToken.Cascade);
            this.reswords.Add("casche", SqlLexerToken.Cashe);
            this.reswords.Add("char_base", SqlLexerToken.Char_base);
            this.reswords.Add("check", SqlLexerToken.Check);
            this.reswords.Add("close", SqlLexerToken.Close);
            this.reswords.Add("cluster", SqlLexerToken.Cluster);
            this.reswords.Add("clusters", SqlLexerToken.Clusters);
            this.reswords.Add("colauth", SqlLexerToken.Colauth);
            this.reswords.Add("column", SqlLexerToken.Column);
            this.reswords.Add("columns", SqlLexerToken.Columns);
            this.reswords.Add("comment", SqlLexerToken.Comment);
            this.reswords.Add("commit", SqlLexerToken.Commit);
            this.reswords.Add("compact", SqlLexerToken.Compact);
            this.reswords.Add("compress", SqlLexerToken.Compress);
            this.reswords.Add("connect", SqlLexerToken.Connect);
            this.reswords.Add("connections", SqlLexerToken.Connections);
            this.reswords.Add("constant", SqlLexerToken.Constant);
            this.reswords.Add("constraint", SqlLexerToken.Constraint);
            this.reswords.Add("count", SqlLexerToken.Count);
            this.reswords.Add("crash", SqlLexerToken.Crash);
            this.reswords.Add("create", SqlLexerToken.Create);
            this.reswords.Add("current", SqlLexerToken.Current);
            this.reswords.Add("cursor", SqlLexerToken.Cursor);
            this.reswords.Add("cycle", SqlLexerToken.Cycle);
            this.reswords.Add("data_base", SqlLexerToken.Data_base);
            this.reswords.Add("database", SqlLexerToken.Database);
            this.reswords.Add("dba", SqlLexerToken.Dba);
            this.reswords.Add("debugoff", SqlLexerToken.Debugoff);
            this.reswords.Add("debugon", SqlLexerToken.Debugon);
            this.reswords.Add("declare", SqlLexerToken.Declare);
            this.reswords.Add("default", SqlLexerToken.Default);
            this.reswords.Add("defferable", SqlLexerToken.Defferable);
            this.reswords.Add("deffered", SqlLexerToken.Deffered);
            this.reswords.Add("definition", SqlLexerToken.Definition);
            this.reswords.Add("delay", SqlLexerToken.Delay);
            this.reswords.Add("delete", SqlLexerToken.Delete);
            this.reswords.Add("delta", SqlLexerToken.Delta);
            this.reswords.Add("desc", SqlLexerToken.Desc);
            this.reswords.Add("describe", SqlLexerToken.Describe);
            this.reswords.Add("digits", SqlLexerToken.Digits);
            this.reswords.Add("dispose", SqlLexerToken.Dispose);
            this.reswords.Add("distinct", SqlLexerToken.Distinct);
            this.reswords.Add("do", SqlLexerToken.Do);
            this.reswords.Add("drop", SqlLexerToken.Drop);
            this.reswords.Add("else", SqlLexerToken.Else);
            this.reswords.Add("elsif", SqlLexerToken.Elsif);
            this.reswords.Add("end", SqlLexerToken.End);
            this.reswords.Add("entry", SqlLexerToken.Entry);
            this.reswords.Add("exception", SqlLexerToken.Exception);
            this.reswords.Add("exception_init", SqlLexerToken.Exception_init);
            this.reswords.Add("exclusive", SqlLexerToken.Exclusive);
            this.reswords.Add("exists", SqlLexerToken.Exists);
            this.reswords.Add("exit", SqlLexerToken.Exit);
            this.reswords.Add("false", SqlLexerToken.False);
            this.reswords.Add("fetch", SqlLexerToken.Fetch);
            this.reswords.Add("file", SqlLexerToken.File);
            this.reswords.Add("for", SqlLexerToken.For);
            this.reswords.Add("foreign", SqlLexerToken.Foreign);
            this.reswords.Add("form", SqlLexerToken.Form);
            this.reswords.Add("from", SqlLexerToken.From);
            this.reswords.Add("function", SqlLexerToken.Function);
            this.reswords.Add("generic", SqlLexerToken.Generic);
            this.reswords.Add("goto", SqlLexerToken.Goto);
            this.reswords.Add("grant", SqlLexerToken.Grant);
            this.reswords.Add("group", SqlLexerToken.Group);
            this.reswords.Add("groups", SqlLexerToken.Groups);
            this.reswords.Add("having", SqlLexerToken.Having);
            this.reswords.Add("identfied", SqlLexerToken.Identfied);
            this.reswords.Add("identified", SqlLexerToken.Identified);
            this.reswords.Add("if", SqlLexerToken.If);
            this.reswords.Add("immediate", SqlLexerToken.Immediate);
            this.reswords.Add("in", SqlLexerToken.In);
            this.reswords.Add("increment", SqlLexerToken.Increment);
            this.reswords.Add("index", SqlLexerToken.Index);
            this.reswords.Add("index_blist", SqlLexerToken.Index_Blist);
            this.reswords.Add("indexes", SqlLexerToken.Indexes);
            this.reswords.Add("index_none", SqlLexerToken.Index_None);
            this.reswords.Add("indicator", SqlLexerToken.Indicator);
            this.reswords.Add("initial", SqlLexerToken.Initial);
            this.reswords.Add("initially", SqlLexerToken.Initially);
            this.reswords.Add("inner", SqlLexerToken.Inner);
            this.reswords.Add("insert", SqlLexerToken.Insert);
            this.reswords.Add("intersect", SqlLexerToken.Intersect);
            this.reswords.Add("into", SqlLexerToken.Into);
            this.reswords.Add("is", SqlLexerToken.Is);
            this.reswords.Add("isolation", SqlLexerToken.Isolation);
            this.reswords.Add("join", SqlLexerToken.Join);
            this.reswords.Add("key", SqlLexerToken.Key);
            this.reswords.Add("left", SqlLexerToken.Left);
            this.reswords.Add("level", SqlLexerToken.Level);
            this.reswords.Add("like", SqlLexerToken.Like);
            this.reswords.Add("limit", SqlLexerToken.Limit);
            this.reswords.Add("limited", SqlLexerToken.Limited);
            this.reswords.Add("lock", SqlLexerToken.Lock);
            this.reswords.Add("long", SqlLexerToken.Long);
            this.reswords.Add("loop", SqlLexerToken.Loop);
            this.reswords.Add("max", SqlLexerToken.Max);
            this.reswords.Add("maxextents", SqlLexerToken.Maxextents);
            this.reswords.Add("min", SqlLexerToken.Min);
            this.reswords.Add("minvalue", SqlLexerToken.Minvalue);
            this.reswords.Add("minus", SqlLexerToken.Minus);
            this.reswords.Add("mod", SqlLexerToken.Mod);
            this.reswords.Add("mode", SqlLexerToken.Mode);
            this.reswords.Add("modify", SqlLexerToken.Modify);
            this.reswords.Add("new", SqlLexerToken.New);
            this.reswords.Add("no", SqlLexerToken.No);
            this.reswords.Add("noaudit", SqlLexerToken.Noaudit);
            this.reswords.Add("nocompress", SqlLexerToken.Nocompress);
            this.reswords.Add("not", SqlLexerToken.Not);
            this.reswords.Add("nowait", SqlLexerToken.Nowait);
            this.reswords.Add("null", SqlLexerToken.Null);
            this.reswords.Add("number", SqlLexerToken.Number);
            this.reswords.Add("number_base", SqlLexerToken.Number_base);
            this.reswords.Add("of", SqlLexerToken.Of);
            this.reswords.Add("off", SqlLexerToken.Off);
            this.reswords.Add("offline", SqlLexerToken.Offline);
            this.reswords.Add("on", SqlLexerToken.On);
            this.reswords.Add("online", SqlLexerToken.Online);
            this.reswords.Add("open", SqlLexerToken.Open);
            this.reswords.Add("option", SqlLexerToken.Option);
            this.reswords.Add("or", SqlLexerToken.Or);
            this.reswords.Add("order", SqlLexerToken.Order);
            this.reswords.Add("others", SqlLexerToken.Others);
            this.reswords.Add("out", SqlLexerToken.Out);
            this.reswords.Add("outer", SqlLexerToken.Outer);
            this.reswords.Add("package", SqlLexerToken.Package);
            this.reswords.Add("partition", SqlLexerToken.Partition);
            this.reswords.Add("password", SqlLexerToken.Password);
            this.reswords.Add("pctfree", SqlLexerToken.Pctfree);
            this.reswords.Add("pragma", SqlLexerToken.Pragma);
            this.reswords.Add("primary", SqlLexerToken.Primary);
            this.reswords.Add("prior", SqlLexerToken.Prior);
            this.reswords.Add("private", SqlLexerToken.Private);
            this.reswords.Add("privileges", SqlLexerToken.Privileges);
            this.reswords.Add("procedure", SqlLexerToken.Procedure);
            this.reswords.Add("public", SqlLexerToken.Public);
            this.reswords.Add("raise", SqlLexerToken.Raise);
            this.reswords.Add("range", SqlLexerToken.Range);
            this.reswords.Add("raw", SqlLexerToken.Raw);
            this.reswords.Add("record", SqlLexerToken.Record);
            this.reswords.Add("references", SqlLexerToken.References);
            this.reswords.Add("release", SqlLexerToken.Release);
            this.reswords.Add("rem", SqlLexerToken.Rem);
            this.reswords.Add("rename", SqlLexerToken.Rename);
            this.reswords.Add("replace", SqlLexerToken.Replace);
            this.reswords.Add("resource", SqlLexerToken.Resource);
            this.reswords.Add("return", SqlLexerToken.Return);
            this.reswords.Add("reverse", SqlLexerToken.Reverse);
            this.reswords.Add("revoke", SqlLexerToken.Revoke);
            this.reswords.Add("right", SqlLexerToken.Right);
            this.reswords.Add("rollback", SqlLexerToken.Rollback);
            this.reswords.Add("row", SqlLexerToken.Row);
            this.reswords.Add("rowid", SqlLexerToken.Rowid);
            this.reswords.Add("rowlabel", SqlLexerToken.Rowlabel);
            this.reswords.Add("rownum", SqlLexerToken.Rownum);
            this.reswords.Add("rows", SqlLexerToken.Rows);
            this.reswords.Add("rowtype", SqlLexerToken.Rowtype);
            this.reswords.Add("run", SqlLexerToken.Run);
            this.reswords.Add("savepoint", SqlLexerToken.Savepoint);
            this.reswords.Add("schema", SqlLexerToken.Schema);
            this.reswords.Add("select", SqlLexerToken.Select);
            this.reswords.Add("separate", SqlLexerToken.Separate);
            this.reswords.Add("sequence", SqlLexerToken.Sequence);
            this.reswords.Add("serializable", SqlLexerToken.Serializable);
            this.reswords.Add("session", SqlLexerToken.Session);
            this.reswords.Add("set", SqlLexerToken.Set);
            this.reswords.Add("share", SqlLexerToken.Share);
            this.reswords.Add("show", SqlLexerToken.Show);
            this.reswords.Add("shutdown", SqlLexerToken.Shutdown);
            this.reswords.Add("size", SqlLexerToken.Size);
            this.reswords.Add("space", SqlLexerToken.Space);
            this.reswords.Add("sql", SqlLexerToken.Sql);
            this.reswords.Add("sqlcode", SqlLexerToken.Sqlcode);
            this.reswords.Add("sqlerrm", SqlLexerToken.Sqlerrm);
            this.reswords.Add("start", SqlLexerToken.Start);
            this.reswords.Add("statement", SqlLexerToken.Statement);
            this.reswords.Add("status", SqlLexerToken.Status);
            this.reswords.Add("stddev", SqlLexerToken.Stddev);
            this.reswords.Add("subtype", SqlLexerToken.Subtype);
            this.reswords.Add("successful", SqlLexerToken.Successful);
            this.reswords.Add("sum", SqlLexerToken.Sum);
            this.reswords.Add("synonym", SqlLexerToken.Synonym);
            this.reswords.Add("sysdate", SqlLexerToken.Sysdate);
            this.reswords.Add("tabauth", SqlLexerToken.Tabauth);
            this.reswords.Add("table", SqlLexerToken.Table);
            this.reswords.Add("tables", SqlLexerToken.Tables);
            this.reswords.Add("task", SqlLexerToken.Task);
            this.reswords.Add("terminate", SqlLexerToken.Terminate);
            this.reswords.Add("then", SqlLexerToken.Then);
            this.reswords.Add("to", SqlLexerToken.To);
            this.reswords.Add("transaction", SqlLexerToken.Transaction);
            this.reswords.Add("trigger", SqlLexerToken.Trigger);
            this.reswords.Add("true", SqlLexerToken.True);
            this.reswords.Add("type", SqlLexerToken.Type);
            this.reswords.Add("uid", SqlLexerToken.Uid);
            this.reswords.Add("union", SqlLexerToken.Union);
            this.reswords.Add("unique", SqlLexerToken.Unique);
            this.reswords.Add("unlock", SqlLexerToken.Unlock);
            this.reswords.Add("update", SqlLexerToken.Update);
            this.reswords.Add("usage", SqlLexerToken.Usage);
            this.reswords.Add("use", SqlLexerToken.Use);
            this.reswords.Add("user", SqlLexerToken.User);
            this.reswords.Add("validate", SqlLexerToken.Validate);
            this.reswords.Add("values", SqlLexerToken.Values);
            this.reswords.Add("varchar2", SqlLexerToken.Varchar2);
            this.reswords.Add("variance", SqlLexerToken.Variance);
            this.reswords.Add("view", SqlLexerToken.View);
            this.reswords.Add("views", SqlLexerToken.Views);
            this.reswords.Add("when", SqlLexerToken.When);
            this.reswords.Add("whenever", SqlLexerToken.Whenever);
            this.reswords.Add("where", SqlLexerToken.Where);
            this.reswords.Add("while", SqlLexerToken.While);
            this.reswords.Add("with", SqlLexerToken.With);
            this.reswords.Add("work", SqlLexerToken.Work);
            this.reswords.Add("xor", SqlLexerToken.Xor);
            this.reswords.Add("bit", SqlLexerToken.Bit);
            this.reswords.Add("real", SqlLexerToken.Real);
            this.reswords.Add("char", SqlLexerToken.Char);
            this.reswords.Add("text", SqlLexerToken.Text);
            this.reswords.Add("date", SqlLexerToken.Date);
            this.reswords.Add("time", SqlLexerToken.Time);
            this.reswords.Add("interval", SqlLexerToken.Interval);
            this.reswords.Add("float", SqlLexerToken.Float);
            this.reswords.Add("bigint", SqlLexerToken.Bigint);
            this.reswords.Add("double", SqlLexerToken.Double);
            this.reswords.Add("string", SqlLexerToken.String);
            this.reswords.Add("binary", SqlLexerToken.Binary);
            this.reswords.Add("numeric", SqlLexerToken.Numeric);
            this.reswords.Add("decimal", SqlLexerToken.Decimal);
            this.reswords.Add("boolean", SqlLexerToken.Boolean);
            this.reswords.Add("tinyint", SqlLexerToken.Tinyint);
            this.reswords.Add("integer", SqlLexerToken.Integer);
            this.reswords.Add("varchar", SqlLexerToken.Varchar);
            this.reswords.Add("smallint", SqlLexerToken.Smallint);
            this.reswords.Add("varbinary", SqlLexerToken.Varbinary);
            this.reswords.Add("timestamp", SqlLexerToken.Timestamp);
            this.reswords.Add("longvarchar", SqlLexerToken.Longvarchar);
            this.reswords.Add("java_object", SqlLexerToken.Java_object);
            this.reswords.Add("longvarbinary", SqlLexerToken.Longvarbinary);
        }

        protected override void InitStyles()
        {
            this.InitDefaultStyles();
        }

        protected virtual bool IsDatatypeToken(int token)
        {
            return ((token >= 240) && (token <= 0x107));
        }

        protected virtual bool IsDateOrIntervalToken(int token)
        {
            switch (((SqlLexerToken) token))
            {
                case SqlLexerToken.Date:
                case SqlLexerToken.Time:
                case SqlLexerToken.Interval:
                case SqlLexerToken.Timestamp:
                    return true;
            }
            return false;
        }

        protected virtual bool IsNextQueryToken()
        {
            bool flag = false;
            Point prevPosition = this.prevPosition;
            this.SaveState();
            try
            {
                this.MoveNext();
                SqlLexerToken token = (SqlLexerToken) this.Token;
                if (token <= SqlLexerToken.Sequence)
                {
                    switch (token)
                    {
                        case SqlLexerToken.Create:
                        case SqlLexerToken.Schema:
                        case SqlLexerToken.Sequence:
                            goto Label_0054;
                    }
                    return flag;
                }
                if (((token != SqlLexerToken.Table) && (token != SqlLexerToken.User)) && (token != SqlLexerToken.View))
                {
                    return flag;
                }
            Label_0054:
                flag = true;
            }
            finally
            {
                this.RestoreState();
                this.prevPosition = prevPosition;
            }
            return flag;
        }

        protected virtual bool IsReswordToken(int token)
        {
            return ((token >= 0) && (token <= 0x107));
        }

        protected override bool IsValidToken(int tok)
        {
            return ((tok != 0x129) && (tok != 0x128));
        }

        protected virtual int LexComment()
        {
            char ch = base.source[base.currentPos];
            base.currentPos++;
            int length = base.source.Length;
            if (base.currentPos < length)
            {
                char ch2 = base.source[base.currentPos];
                switch (ch)
                {
                    case '-':
                        if (ch2 == '-')
                        {
                            base.currentPos = length + 1;
                            return 0x128;
                        }
                        base.currentPos--;
                        return this.LexNumber();

                    case '/':
                        switch (ch2)
                        {
                            case '/':
                                base.currentPos = length;
                                return 0x128;

                            case '*':
                                base.currentPos++;
                                return this.LexCommentEnd();
                        }
                        break;
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
                        return 0x128;
                    }
                }
                else
                {
                    base.currentPos++;
                }
            }
            this.State = 1;
            return 0x128;
        }

        protected virtual int LexIdentifier()
        {
            char ch = base.source[base.currentPos];
            switch (ch)
            {
                case '"':
                {
                    int length = base.source.Length;
                    base.currentPos++;
                    while (base.currentPos < length)
                    {
                        ch = base.source[base.currentPos];
                        if (ch == '"')
                        {
                            base.currentPos++;
                            break;
                        }
                        base.currentPos++;
                    }
                    break;
                }
                case 'N':
                case 'x':
                case 'X':
                case 'n':
                    if ((base.currentPos >= (base.source.Length - 1)) || (base.source[base.currentPos + 1] != '\''))
                    {
                        goto Label_0155;
                    }
                    base.currentPos++;
                    return this.LexString();

                case 'U':
                case 'u':
                    if (((base.currentPos < (base.source.Length - 2)) && (base.source[base.currentPos + 1] == '&')) && (base.source[base.currentPos + 2] == '\''))
                    {
                        base.currentPos += 2;
                        return this.LexString();
                    }
                    goto Label_0155;

                default:
                    goto Label_0155;
            }
            return 0x127;
        Label_0155:
            this.LexIdent();
            object obj2 = this.reswords[this.TokenString.ToLower()];
            if (obj2 == null)
            {
                return 0x127;
            }
            return (int) obj2;
        }

        protected virtual int LexNumber()
        {
            int num2;
            char ch = base.source[base.currentPos];
            int length = base.source.Length;
            int num3 = 0x123;
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
            base.LexNum();
            if (base.currentPos < length)
            {
                ch = base.source[base.currentPos];
                if ((ch == '.') && (base.currentPos < (length - 1)))
                {
                    ch = base.source[base.currentPos + 1];
                    if ((ch >= '0') && (ch <= '9'))
                    {
                        base.currentPos++;
                        this.LexNum();
                        num3 = 0x124;
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
                        num3 = 0x125;
                    }
                }
            }
            return num3;
        }

        protected virtual int LexString()
        {
            int length = base.source.Length;
            char ch = base.source[base.currentPos];
            char ch2 = ch;
            bool flag = false;
            switch (ch)
            {
                case 'b':
                case 'B':
                    base.currentPos++;
                    if ((base.currentPos == length) || (base.source[base.currentPos] != '\''))
                    {
                        base.currentPos--;
                        return this.LexIdentifier();
                    }
                    ch2 = base.source[base.currentPos];
                    break;
            }
            base.currentPos++;
            while (base.currentPos < length)
            {
                ch = base.source[base.currentPos];
                if ((ch == ch2) && !flag)
                {
                    base.currentPos++;
                    if ((base.currentPos >= length) || (base.currentPos != ch2))
                    {
                        break;
                    }
                    base.currentPos++;
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
            return 0x126;
        }

        protected virtual int LexSymbol()
        {
            SqlLexerToken bang = SqlLexerToken.Whitespace_Literal;
            char ch = base.source[base.currentPos];
            base.currentPos++;
            switch (ch)
            {
                case '!':
                    bang = SqlLexerToken.Bang;
                    break;

                case '#':
                    bang = SqlLexerToken.Turma;
                    break;

                case '$':
                    bang = SqlLexerToken.Dollar;
                    break;

                case '%':
                    bang = SqlLexerToken.Percent;
                    break;

                case '&':
                    bang = SqlLexerToken.Ampersant;
                    break;

                case '(':
                    bang = SqlLexerToken.Open_parens;
                    break;

                case ')':
                    bang = SqlLexerToken.Close_parens;
                    break;

                case '*':
                    bang = SqlLexerToken.Op_Star;
                    break;

                case '+':
                    bang = SqlLexerToken.Op_Plus;
                    break;

                case ',':
                    bang = SqlLexerToken.Comma;
                    break;

                case '-':
                    bang = SqlLexerToken.Op_Minus;
                    break;

                case '.':
                    bang = SqlLexerToken.Dot;
                    break;

                case '/':
                    bang = SqlLexerToken.Op_Div;
                    break;

                case ';':
                    bang = SqlLexerToken.Semicolon;
                    break;

                case '<':
                    switch (base.CurChar())
                    {
                        case '=':
                            base.currentPos++;
                            bang = SqlLexerToken.Op_le;
                            goto Label_0227;

                        case '>':
                            base.currentPos++;
                            bang = SqlLexerToken.Op_ne;
                            goto Label_0227;
                    }
                    bang = SqlLexerToken.Op_lt;
                    break;

                case '=':
                    bang = SqlLexerToken.Op_Assign;
                    break;

                case '>':
                    if (base.CurChar() != '=')
                    {
                        bang = SqlLexerToken.Op_gt;
                        break;
                    }
                    base.currentPos++;
                    bang = SqlLexerToken.Op_ge;
                    break;

                case '?':
                    bang = SqlLexerToken.Interr;
                    break;

                case '@':
                    bang = SqlLexerToken.Uxo;
                    break;

                case '^':
                    bang = SqlLexerToken.Carret;
                    break;

                case '`':
                    bang = SqlLexerToken.Cav;
                    break;

                case '|':
                    bang = SqlLexerToken.Bitwise_or;
                    break;

                case '~':
                    bang = SqlLexerToken.Tilde;
                    break;
            }
        Label_0227:
            return (int) bang;
        }

        protected virtual int LexWhitespace()
        {
            this.LexSpace();
            return 0x129;
        }

        protected virtual int MoveNext()
        {
            this.prevPosition = this.CurrentPosition;
            int token = this.NextToken();
            while (!this.Eof && !this.IsValidToken(token))
            {
                token = this.NextToken();
            }
            return token;
        }

        protected virtual bool ParseAdditiveExpression(ref ISyntaxNode node)
        {
            bool flag = this.ParseMultiplicativeExpression(ref node);
            switch (this.Token)
            {
                case 0x121:
                case 290:
                {
                    node = this.CreateExpressionNode(this.TokenPosition, this.TokenString, 0x36, node, true);
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

        protected virtual bool ParseAlterCreateTable()
        {
            this.MoveNext();
            if (this.Token == 0xd1)
            {
                return this.ParseCreateTable();
            }
            return this.Expected(0xd1);
        }

        protected virtual bool ParseAlterQuery()
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, this.TokenString.ToUpper(), 3, SyntaxNodeOptions.Indentation);
            this.AddNode(node);
            this.SyntaxTree.Push(node);
            try
            {
                this.MoveNext();
                SqlLexerToken token = (SqlLexerToken) this.Token;
                if (token != SqlLexerToken.Create)
                {
                    if (token != SqlLexerToken.Table)
                    {
                        if (token != SqlLexerToken.User)
                        {
                            goto Label_0075;
                        }
                        if (!this.ParseAlterUser())
                        {
                            flag = false;
                        }
                    }
                    else if (!this.ParseAlterTable())
                    {
                        flag = false;
                    }
                }
                else if (!this.ParseAlterCreateTable())
                {
                    flag = false;
                }
                goto Label_008C;
            Label_0075:
                flag = false;
                this.SyntaxError();
            }
            finally
            {
                this.SyntaxTree.Pop();
            }
        Label_008C:
            node.Range.EndPoint = this.prevPosition;
            return flag;
        }

        protected virtual bool ParseAlterTable()
        {
            string str;
            bool flag = true;
            this.MoveNext();
            ISyntaxNode current = this.SyntaxTree.Current;
            current.AddAttribute(new SyntaxAttribute(this.prevPosition, SyntaxConsts.DeclarationScope, null));
            if (!this.ParseQualifiedIdentifier(out str))
            {
                return false;
            }
            current.Name = str;
            current.NodeType = 0x18;
            current.Options = SyntaxNodeOptions.Outlining | SyntaxNodeOptions.Indentation;
            bool flag2 = false;
            do
            {
                switch (((SqlLexerToken) this.Token))
                {
                    case SqlLexerToken.Add:
                        if (!this.ParseAlterTableAdd())
                        {
                            flag = false;
                        }
                        break;

                    case SqlLexerToken.Alter:
                        if (!this.IsNextQueryToken())
                        {
                            if (!this.ParseAlterTableAlter())
                            {
                                flag = false;
                            }
                        }
                        else
                        {
                            flag2 = true;
                        }
                        break;

                    case SqlLexerToken.Drop:
                        if (this.IsNextQueryToken())
                        {
                            flag2 = true;
                        }
                        else if (!this.ParseAlterTableDrop())
                        {
                            flag = false;
                        }
                        break;

                    default:
                        flag2 = true;
                        break;
                }
            }
            while (!this.Eof && !flag2);
            current.Range.EndPoint = this.prevPosition;
            return flag;
        }

        protected virtual bool ParseAlterTableAdd()
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, this.TokenString, 30);
            this.AddNode(node);
            this.SyntaxTree.Push(node);
            try
            {
                this.MoveNext();
                if (this.Token == 0x21)
                {
                    this.MoveNext();
                }
                if (this.Token == 0x2a)
                {
                    if (!this.ParseConstraintDeclaration())
                    {
                        flag = false;
                    }
                }
                else if (!this.ParseColumnDeclaration())
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

        protected virtual bool ParseAlterTableAlter()
        {
            string str;
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, this.TokenString, 3);
            this.AddNode(node);
            this.MoveNext();
            if (this.Token == 0x21)
            {
                this.MoveNext();
            }
            Point tokenPosition = this.TokenPosition;
            if (!this.ParseQualifiedIdentifier(out str))
            {
                flag = false;
            }
            else
            {
                node.AddAttribute(new SyntaxAttribute(tokenPosition, SqlNodeType.ColumnName.ToString(), str));
                SqlLexerToken token = (SqlLexerToken) this.Token;
                if (token != SqlLexerToken.Drop)
                {
                    if (token != SqlLexerToken.Set)
                    {
                        this.SyntaxError();
                    }
                    else
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
                else
                {
                    this.MoveNext();
                    if (!this.Expected(SqlLexerToken.Default))
                    {
                        flag = false;
                    }
                }
            }
            node.Range.EndPoint = this.prevPosition;
            return flag;
        }

        protected virtual bool ParseAlterTableDrop()
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, this.TokenString, 4);
            this.AddNode(node);
            this.MoveNext();
            if (this.Token == 0x21)
            {
                this.MoveNext();
            }
            Point tokenPosition = this.TokenPosition;
            string identifier = string.Empty;
            SqlLexerToken token = (SqlLexerToken) this.Token;
            if (token == SqlLexerToken.Constraint)
            {
                this.MoveNext();
                tokenPosition = this.TokenPosition;
                if (!this.ParseQualifiedIdentifier(out identifier))
                {
                    flag = false;
                }
            }
            else if (token == SqlLexerToken.Primary)
            {
                identifier = this.TokenString.ToUpper();
                tokenPosition = this.TokenPosition;
                this.MoveNext();
                if (this.Token == 0x70)
                {
                    identifier = identifier + " " + this.TokenString.ToUpper();
                    this.MoveNext();
                }
                else
                {
                    flag = this.Expected(0x70);
                }
            }
            else if (!this.ParseQualifiedIdentifier(out identifier))
            {
                flag = false;
            }
            if (flag)
            {
                node.AddAttribute(new SyntaxAttribute(tokenPosition, SqlNodeType.ColumnName.ToString(), identifier));
            }
            node.Range.EndPoint = this.prevPosition;
            return flag;
        }

        protected virtual bool ParseAlterUser()
        {
            string str;
            ISyntaxAttribute attribute3;
            bool flag = true;
            this.MoveNext();
            Point tokenPosition = this.TokenPosition;
            ISyntaxNode current = this.SyntaxTree.Current;
            current.NodeType = 0x17;
            if (!this.ParseQualifiedIdentifier(out str))
            {
                flag = false;
                goto Label_0177;
            }
            current.Name = str;
        Label_0038:
            if (!this.Expected(SqlLexerToken.Set))
            {
                flag = false;
                goto Label_0177;
            }
            SqlLexerToken token = (SqlLexerToken) this.Token;
            switch (token)
            {
                case SqlLexerToken.Account:
                    attribute3 = new SyntaxAttribute(this.TokenPosition, SqlNodeType.AccountAttribute.ToString(), null);
                    this.MoveNext();
                    if ((this.Token != 0x76) && (this.Token != 0xde))
                    {
                        flag = false;
                        break;
                    }
                    attribute3.Value = this.TokenString;
                    this.MoveNext();
                    break;

                case SqlLexerToken.Groups:
                {
                    string str2;
                    ISyntaxAttribute attr = new SyntaxAttribute(this.TokenPosition, SqlNodeType.IdentifierList.ToString(), null);
                    this.MoveNext();
                    if (this.ParseIdentifierList(out str2))
                    {
                        attr.Value = str2;
                    }
                    else
                    {
                        flag = false;
                    }
                    this.AddAttribute(attr);
                    goto Label_0163;
                }
                default:
                    if (token == SqlLexerToken.Password)
                    {
                        ISyntaxAttribute attribute = new SyntaxAttribute(this.TokenPosition, SqlNodeType.PasswordAttribute.ToString(), null);
                        this.MoveNext();
                        ISyntaxNode node = null;
                        if (this.ParseExpression(ref node))
                        {
                            if ((node != null) && (node.NodeType == 0x3d))
                            {
                                attribute.Value = node.Name;
                            }
                        }
                        else
                        {
                            flag = false;
                        }
                        this.AddAttribute(attribute);
                    }
                    goto Label_0163;
            }
            this.AddAttribute(attribute3);
        Label_0163:
            if (this.Token == 190)
            {
                goto Label_0038;
            }
        Label_0177:
            current.Range.EndPoint = this.prevPosition;
            return flag;
        }

        protected virtual bool ParseAndExpression(ref ISyntaxNode node)
        {
            bool flag = this.ParseEqualityExpression(ref node);
            if (this.Token == 8)
            {
                node = this.CreateExpressionNode(this.TokenPosition, this.TokenString, 0x38, node, true);
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

        protected virtual bool ParseArgumentList(ref ISyntaxNode node)
        {
            node = new SyntaxNode(this.TokenPosition, this.TokenString, 0x34);
            ISyntaxNode node2 = null;
            bool flag = (this.Expected(SqlLexerToken.Open_parens) && this.ParseExpressionList(ref node2)) && this.Expected(SqlLexerToken.Close_parens);
            if (node2 != null)
            {
                node.AddChild(node2);
            }
            return flag;
        }

        protected virtual bool ParseCheckStatement()
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, this.TokenString, 0x23);
            this.AddNode(node);
            this.MoveNext();
            ISyntaxNode node2 = null;
            flag = (this.Expected(SqlLexerToken.Open_parens) && this.ParseExpression(ref node2)) && this.Expected(SqlLexerToken.Close_parens);
            if (node2 != null)
            {
                node.AddChild(node2);
            }
            node.Range.EndPoint = this.prevPosition;
            return flag;
        }

        protected virtual bool ParseColumnDeclaration()
        {
            string str;
            bool flag = true;
            Point tokenPosition = this.TokenPosition;
            if (this.ParseQualifiedIdentifier(out str))
            {
                ISyntaxNode node = new SyntaxNode(tokenPosition, str, 0x47);
                this.AddNode(node);
                this.SyntaxTree.Push(node);
                try
                {
                    tokenPosition = this.TokenPosition;
                    if (this.ParseType(out str))
                    {
                        node.AddAttribute(new SyntaxAttribute(tokenPosition, SqlNodeType.TypeAttribute.ToString(), str));
                    }
                    else
                    {
                        flag = false;
                    }
                    tokenPosition = this.TokenPosition;
                    bool flag2 = false;
                Label_006C:
                    tokenPosition = this.TokenPosition;
                    switch (((SqlLexerToken) this.Token))
                    {
                        case SqlLexerToken.Not:
                        {
                            string str2 = this.TokenString.ToUpper();
                            this.MoveNext();
                            if (!this.Expected(0x87))
                            {
                                break;
                            }
                            node.AddAttribute(new SyntaxAttribute(tokenPosition, SqlNodeType.Attribute.ToString(), str2 + " " + this.TokenString.ToUpper()));
                            goto Label_01C9;
                        }
                        case SqlLexerToken.Null:
                            this.MoveNext();
                            node.AddAttribute(new SyntaxAttribute(tokenPosition, SqlNodeType.Attribute.ToString(), this.TokenString.ToUpper()));
                            goto Label_01C9;

                        case SqlLexerToken.Unique:
                            this.MoveNext();
                            node.AddAttribute(new SyntaxAttribute(tokenPosition, SqlNodeType.Attribute.ToString(), this.TokenString.ToUpper()));
                            goto Label_01C9;

                        case SqlLexerToken.Index_Blist:
                        case SqlLexerToken.Index_None:
                            tokenPosition = this.TokenPosition;
                            this.MoveNext();
                            node.AddAttribute(new SyntaxAttribute(tokenPosition, SqlNodeType.Attribute.ToString(), this.TokenString.ToUpper()));
                            goto Label_01C9;

                        case SqlLexerToken.Default:
                            if (!this.ParseDefaultStatement())
                            {
                                flag = false;
                            }
                            goto Label_01C9;

                        default:
                            flag2 = true;
                            goto Label_01C9;
                    }
                    flag = false;
                Label_01C9:
                    if (!this.Eof && !flag2)
                    {
                        goto Label_006C;
                    }
                }
                finally
                {
                    this.SyntaxTree.Pop();
                }
                node.Range.EndPoint = this.prevPosition;
                return flag;
            }
            return false;
        }

        protected virtual bool ParseColumnList()
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, string.Empty, 0x31);
            this.AddNode(node);
            if (this.Expected(SqlLexerToken.Open_parens))
            {
                string str;
                Point tokenPosition = this.TokenPosition;
                if (this.ParseIdentifierList(out str))
                {
                    node.AddAttribute(new SyntaxAttribute(tokenPosition, SqlNodeType.IdentifierList.ToString(), str));
                    flag = this.Expected(SqlLexerToken.Close_parens);
                }
                else
                {
                    flag = false;
                }
            }
            node.Range.EndPoint = this.prevPosition;
            return flag;
        }

        protected virtual bool ParseCommitQuery()
        {
            this.AddNode(new SyntaxNode(this.TokenPosition, this.TokenString.ToUpper(), 14, SyntaxNodeOptions.Indentation));
            this.MoveNext();
            return true;
        }

        protected virtual bool ParseCompactQuery()
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, this.TokenString.ToUpper(), 15, SyntaxNodeOptions.Indentation);
            this.AddNode(node);
            this.SyntaxTree.Push(node);
            try
            {
                string str;
                this.MoveNext();
                if (!this.Expected(SqlLexerToken.Table))
                {
                    flag = false;
                }
                Point tokenPosition = this.TokenPosition;
                if (this.ParseQualifiedIdentifier(out str))
                {
                    node.AddAttribute(new SyntaxAttribute(tokenPosition, SqlNodeType.TableName.ToString(), str));
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

        protected virtual bool ParseConstraintDeclaration()
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, string.Empty, 0x49);
            this.AddNode(node);
            this.SyntaxTree.Push(node);
            try
            {
                string str;
                string str2;
                string str3;
                SqlLexerToken token;
                this.MoveNext();
                if (!this.ParseQualifiedIdentifier(out str))
                {
                    goto Label_0275;
                }
                node.Name = str;
                bool flag2 = false;
            Label_0045:
                token = (SqlLexerToken) this.Token;
                if (token <= SqlLexerToken.Foreign)
                {
                    switch (token)
                    {
                        case SqlLexerToken.Check:
                            goto Label_00DD;

                        case SqlLexerToken.Defferable:
                            goto Label_0166;

                        case SqlLexerToken.Foreign:
                            goto Label_00B9;
                    }
                    goto Label_01E7;
                }
                if (token <= SqlLexerToken.Not)
                {
                    switch (token)
                    {
                        case SqlLexerToken.Initially:
                            goto Label_00EF;

                        case SqlLexerToken.Not:
                            goto Label_0191;
                    }
                    goto Label_01E7;
                }
                if (token != SqlLexerToken.Primary)
                {
                    if (token == SqlLexerToken.Unique)
                    {
                        goto Label_00CB;
                    }
                    goto Label_01E7;
                }
                if (!this.ParsePrimaryKeyStatement())
                {
                    flag = false;
                }
                goto Label_01E9;
            Label_00B9:
                if (!this.ParseForeignKeyStatement())
                {
                    flag = false;
                }
                goto Label_01E9;
            Label_00CB:
                if (!this.ParseUniqueStatement())
                {
                    flag = false;
                }
                goto Label_01E9;
            Label_00DD:
                if (!this.ParseCheckStatement())
                {
                    flag = false;
                }
                goto Label_01E9;
            Label_00EF:
                str2 = this.TokenString.ToUpper();
                Point tokenPosition = this.TokenPosition;
                this.MoveNext();
                if ((this.Token == 0x39) || (this.Token == 0x5f))
                {
                    node.AddAttribute(new SyntaxAttribute(tokenPosition, SqlNodeType.Attribute.ToString(), str2 = str2 + " " + this.TokenString.ToUpper()));
                    this.MoveNext();
                }
                else
                {
                    flag = false;
                }
                goto Label_01E9;
            Label_0166:
                node.AddAttribute(new SyntaxAttribute(this.TokenPosition, SqlNodeType.Attribute.ToString(), this.TokenString.ToUpper()));
                goto Label_01E9;
            Label_0191:
                str3 = this.TokenString.ToUpper();
                Point position = this.TokenPosition;
                if (this.Expected(0x38))
                {
                    node.AddAttribute(new SyntaxAttribute(position, SqlNodeType.Attribute.ToString(), str3 + " " + this.TokenString.ToUpper()));
                }
                else
                {
                    flag = false;
                }
                goto Label_01E9;
            Label_01E7:
                flag2 = true;
            Label_01E9:
                if (this.Token == 0x11b)
                {
                    bool flag3 = false;
                    this.SaveState();
                    try
                    {
                        this.MoveNext();
                        SqlLexerToken token2 = (SqlLexerToken) this.Token;
                        if (token2 <= SqlLexerToken.Foreign)
                        {
                            switch (token2)
                            {
                                case SqlLexerToken.Check:
                                case SqlLexerToken.Defferable:
                                case SqlLexerToken.Foreign:
                                    goto Label_0254;
                            }
                            goto Label_0265;
                        }
                        if (token2 <= SqlLexerToken.Not)
                        {
                            switch (token2)
                            {
                                case SqlLexerToken.Initially:
                                case SqlLexerToken.Not:
                                    goto Label_0254;
                            }
                            goto Label_0265;
                        }
                        if ((token2 != SqlLexerToken.Primary) && (token2 != SqlLexerToken.Unique))
                        {
                            goto Label_0265;
                        }
                    Label_0254:
                        flag3 = true;
                    }
                    finally
                    {
                        this.RestoreState(!flag3);
                    }
                }
            Label_0265:
                if (!this.Eof && !flag2)
                {
                    goto Label_0045;
                }
                goto Label_0286;
            Label_0275:
                flag = false;
            }
            finally
            {
                this.SyntaxTree.Pop();
            }
        Label_0286:
            node.Range.EndPoint = this.prevPosition;
            return flag;
        }

        protected virtual bool ParseCreateQuery()
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, this.TokenString.ToUpper(), 2, SyntaxNodeOptions.Indentation);
            this.AddNode(node);
            this.SyntaxTree.Push(node);
            try
            {
                this.MoveNext();
                SqlLexerToken token = (SqlLexerToken) this.Token;
                if (token <= SqlLexerToken.Sequence)
                {
                    switch (token)
                    {
                        case SqlLexerToken.Schema:
                            goto Label_0086;

                        case SqlLexerToken.Sequence:
                            goto Label_007A;
                    }
                    goto Label_00AA;
                }
                switch (token)
                {
                    case SqlLexerToken.Table:
                        if (!this.ParseCreateTable())
                        {
                            flag = false;
                        }
                        goto Label_00C1;

                    case SqlLexerToken.User:
                        if (!this.ParseCreateUser())
                        {
                            flag = false;
                        }
                        goto Label_00C1;

                    default:
                        if (token != SqlLexerToken.View)
                        {
                            goto Label_00AA;
                        }
                        if (!this.ParseCreateView())
                        {
                            flag = false;
                        }
                        goto Label_00C1;
                }
            Label_007A:
                if (!this.ParseCreateSequence())
                {
                    flag = false;
                }
                goto Label_00C1;
            Label_0086:
                if (!this.ParseCreateSchema())
                {
                    flag = false;
                }
                goto Label_00C1;
            Label_00AA:
                flag = false;
                this.SyntaxError();
            }
            finally
            {
                this.SyntaxTree.Pop();
            }
        Label_00C1:
            node.Range.EndPoint = this.prevPosition;
            return flag;
        }

        protected virtual bool ParseCreateSchema()
        {
            string str;
            bool flag = true;
            this.MoveNext();
            ISyntaxNode current = this.SyntaxTree.Current;
            current.NodeType = 20;
            current.AddAttribute(new SyntaxAttribute(this.prevPosition, SyntaxConsts.DeclarationScope, null));
            if (this.ParseQualifiedIdentifier(out str))
            {
                current.Name = str;
            }
            else
            {
                flag = false;
            }
            current.Range.EndPoint = this.prevPosition;
            return flag;
        }

        protected virtual bool ParseCreateSequence()
        {
            string str;
            SqlLexerToken token;
            bool flag = true;
            this.MoveNext();
            ISyntaxNode current = this.SyntaxTree.Current;
            current.NodeType = 0x16;
            current.AddAttribute(new SyntaxAttribute(this.prevPosition, SyntaxConsts.DeclarationScope, null));
            if (!this.ParseQualifiedIdentifier(out str))
            {
                flag = false;
                goto Label_0116;
            }
            current.Name = str;
            bool flag2 = false;
        Label_004B:
            token = (SqlLexerToken) this.Token;
            if (token <= SqlLexerToken.Cycle)
            {
                switch (token)
                {
                    case SqlLexerToken.Cashe:
                        goto Label_0082;

                    case SqlLexerToken.Cycle:
                        current.AddAttribute(new SyntaxAttribute(this.TokenPosition, SqlNodeType.Attribute.ToString(), this.TokenString.ToUpper()));
                        this.MoveNext();
                        goto Label_0104;
                }
                goto Label_0102;
            }
            if (((token != SqlLexerToken.Increment) && (token != SqlLexerToken.Minvalue)) && (token != SqlLexerToken.Start))
            {
                goto Label_0102;
            }
        Label_0082:
            current.AddAttribute(new SyntaxAttribute(this.TokenPosition, SqlNodeType.Attribute.ToString(), this.TokenString.ToUpper()));
            this.MoveNext();
            ISyntaxNode node = null;
            if (!this.ParseExpression(ref node))
            {
                flag = false;
            }
            if (node != null)
            {
                current.AddChild(node);
            }
            goto Label_0104;
        Label_0102:
            flag2 = true;
        Label_0104:
            if (!this.Eof && !flag2)
            {
                goto Label_004B;
            }
        Label_0116:
            current.Range.EndPoint = this.prevPosition;
            return flag;
        }

        protected virtual bool ParseCreateTable()
        {
            string str;
            bool flag = true;
            this.MoveNext();
            ISyntaxNode current = this.SyntaxTree.Current;
            current.NodeType = 0x12;
            current.Options = SyntaxNodeOptions.Outlining | SyntaxNodeOptions.Indentation;
            current.AddAttribute(new SyntaxAttribute(this.prevPosition, SyntaxConsts.DeclarationScope, null));
            Point tokenPosition = this.TokenPosition;
            if (this.Token == 0x5e)
            {
                string str2 = this.TokenString.ToUpper();
                this.MoveNext();
                if (this.Expected(SqlLexerToken.Not))
                {
                    str2 = str2 + " " + this.TokenString.ToUpper();
                }
                else
                {
                    flag = false;
                }
                if (this.Expected(SqlLexerToken.Exists))
                {
                    str2 = str2 + " " + this.TokenString.ToUpper();
                }
                else
                {
                    flag = false;
                }
                current.AddAttribute(new SyntaxAttribute(tokenPosition, SqlNodeType.Attribute.ToString(), str2));
            }
            if (!this.ParseQualifiedIdentifier(out str))
            {
                flag = false;
                goto Label_0145;
            }
            current.Name = str;
            if (this.Token != 0x108)
            {
                goto Label_0145;
            }
            this.MoveNext();
        Label_00F6:
            if (this.Token == 0x2a)
            {
                if (!this.ParseConstraintDeclaration())
                {
                    flag = false;
                }
            }
            else if (!this.ParseColumnDeclaration())
            {
                flag = false;
            }
            if (this.Token == 0x11b)
            {
                this.MoveNext();
                if (!this.Eof)
                {
                    goto Label_00F6;
                }
            }
            if (!this.Expected(SqlLexerToken.Close_parens))
            {
                flag = false;
            }
        Label_0145:
            current.Range.EndPoint = this.prevPosition;
            return flag;
        }

        protected virtual bool ParseCreateUser()
        {
            bool flag = this.ParseAlterUser();
            this.SyntaxTree.Current.NodeType = 0x15;
            return flag;
        }

        protected virtual bool ParseCreateView()
        {
            string str;
            bool flag = true;
            this.MoveNext();
            ISyntaxNode current = this.SyntaxTree.Current;
            current.NodeType = 0x13;
            current.AddAttribute(new SyntaxAttribute(this.prevPosition, SyntaxConsts.DeclarationScope, null));
            if (this.ParseQualifiedIdentifier(out str))
            {
                current.Name = str;
                if ((this.Token == 0x108) && !this.ParseColumnList())
                {
                    flag = false;
                }
                if (this.Expected(SqlLexerToken.As))
                {
                    if (this.Token == 0xb9)
                    {
                        if (!this.ParseSelectQuery())
                        {
                            flag = false;
                        }
                    }
                    else
                    {
                        flag = this.Expected(SqlLexerToken.Select);
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
            current.Range.EndPoint = this.prevPosition;
            return flag;
        }

        protected virtual bool ParseDatabaseObject(out string databaseObject)
        {
            string str;
            bool flag = true;
            databaseObject = string.Empty;
            switch (((SqlLexerToken) this.Token))
            {
                case SqlLexerToken.Schema:
                case SqlLexerToken.Table:
                    databaseObject = this.TokenString.ToUpper();
                    this.MoveNext();
                    break;
            }
            flag = this.ParseQualifiedIdentifier(out str);
            if (flag)
            {
                databaseObject = (databaseObject == string.Empty) ? str : (databaseObject + " " + str);
            }
            return flag;
        }

        protected virtual bool ParseDefaultStatement()
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, this.TokenString, 0x2b);
            this.AddNode(node);
            this.MoveNext();
            ISyntaxNode node2 = null;
            flag = this.ParseExpression(ref node2);
            if (node2 != null)
            {
                node.AddChild(node2);
            }
            node.Range.EndPoint = this.prevPosition;
            return flag;
        }

        protected virtual bool ParseDeleteQuery()
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, this.TokenString.ToUpper(), 8, SyntaxNodeOptions.Outlining | SyntaxNodeOptions.Indentation);
            this.AddNode(node);
            this.SyntaxTree.Push(node);
            try
            {
                string str;
                this.MoveNext();
                if (!this.Expected(SqlLexerToken.From))
                {
                    flag = false;
                }
                node.AddAttribute(new SyntaxAttribute(this.prevPosition, SyntaxConsts.DeclarationScope, null));
                Point tokenPosition = this.TokenPosition;
                if (this.ParseQualifiedIdentifier(out str))
                {
                    node.AddAttribute(new SyntaxAttribute(tokenPosition, SqlNodeType.TableName.ToString(), str));
                    if ((this.Token == 0xeb) && !this.ParseWhereStatement())
                    {
                        flag = false;
                    }
                    if ((this.Token == 0x74) && !this.ParseLimitStatement())
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

        protected virtual bool ParseDescribeQuery()
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, this.TokenString.ToUpper(), 11, SyntaxNodeOptions.Indentation);
            this.AddNode(node);
            this.SyntaxTree.Push(node);
            try
            {
                string str;
                this.MoveNext();
                Point tokenPosition = this.TokenPosition;
                if (this.ParseQualifiedIdentifier(out str))
                {
                    node.AddAttribute(new SyntaxAttribute(tokenPosition, SqlNodeType.TableName.ToString(), str));
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

        protected virtual bool ParseDropQuery()
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, this.TokenString.ToUpper(), 4, SyntaxNodeOptions.Indentation);
            this.AddNode(node);
            this.SyntaxTree.Push(node);
            try
            {
                this.MoveNext();
                node.AddAttribute(new SyntaxAttribute(this.prevPosition, SyntaxConsts.DeclarationScope, null));
                SqlLexerToken token = (SqlLexerToken) this.Token;
                if (token <= SqlLexerToken.Sequence)
                {
                    switch (token)
                    {
                        case SqlLexerToken.Schema:
                            goto Label_009E;

                        case SqlLexerToken.Sequence:
                            goto Label_0092;
                    }
                    goto Label_00C2;
                }
                switch (token)
                {
                    case SqlLexerToken.Table:
                        if (!this.ParseDropTable())
                        {
                            flag = false;
                        }
                        goto Label_00D9;

                    case SqlLexerToken.User:
                        if (!this.ParseDropUser())
                        {
                            flag = false;
                        }
                        goto Label_00D9;

                    default:
                        if (token != SqlLexerToken.View)
                        {
                            goto Label_00C2;
                        }
                        if (!this.ParseDropView())
                        {
                            flag = false;
                        }
                        goto Label_00D9;
                }
            Label_0092:
                if (!this.ParseDropSequence())
                {
                    flag = false;
                }
                goto Label_00D9;
            Label_009E:
                if (!this.ParseDropSchema())
                {
                    flag = false;
                }
                goto Label_00D9;
            Label_00C2:
                flag = false;
                this.SyntaxError();
            }
            finally
            {
                this.SyntaxTree.Pop();
            }
        Label_00D9:
            node.Range.EndPoint = this.prevPosition;
            return flag;
        }

        protected virtual bool ParseDropSchema()
        {
            bool flag = this.ParseDropUser();
            if (flag)
            {
                this.SyntaxTree.Current.NodeType = 0x1a;
            }
            return flag;
        }

        protected virtual bool ParseDropSequence()
        {
            bool flag = this.ParseDropUser();
            if (flag)
            {
                this.SyntaxTree.Current.NodeType = 0x1c;
            }
            return flag;
        }

        protected virtual bool ParseDropTable()
        {
            string str2;
            bool flag = true;
            this.MoveNext();
            ISyntaxNode current = this.SyntaxTree.Current;
            if (this.Token == 0x5e)
            {
                Point tokenPosition = this.TokenPosition;
                string str = this.TokenString.ToUpper();
                this.MoveNext();
                if (this.Token == 0x4c)
                {
                    current.AddAttribute(new SyntaxAttribute(tokenPosition, SqlNodeType.Attribute.ToString(), str + " " + this.TokenString.ToUpper()));
                    this.MoveNext();
                }
                else
                {
                    flag = false;
                }
            }
            if (this.ParseIdentifierList(out str2))
            {
                current.Name = str2;
                current.NodeType = 0x1d;
            }
            else
            {
                flag = false;
            }
            current.Range.EndPoint = this.prevPosition;
            return flag;
        }

        protected virtual bool ParseDropUser()
        {
            string str;
            bool flag = true;
            this.MoveNext();
            if (this.ParseQualifiedIdentifier(out str))
            {
                ISyntaxNode current = this.SyntaxTree.Current;
                current.Name = str;
                current.NodeType = 0x1b;
                current.Range.EndPoint = this.prevPosition;
                return flag;
            }
            return false;
        }

        protected virtual bool ParseDropView()
        {
            bool flag = this.ParseDropUser();
            if (flag)
            {
                this.SyntaxTree.Current.NodeType = 0x19;
            }
            return flag;
        }

        protected virtual bool ParseEqualityExpression(ref ISyntaxNode node)
        {
            bool flag = this.ParseRelationalExpression(ref node);
            switch (((SqlLexerToken) this.Token))
            {
                case SqlLexerToken.Like:
                case SqlLexerToken.Op_ne:
                case SqlLexerToken.Op_Assign:
                {
                    node = this.CreateExpressionNode(this.TokenPosition, this.TokenString, (this.Token == 0x73) ? 0x3a : 60, node, true);
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
                    break;
                }
            }
            return flag;
        }

        protected virtual bool ParseExpression(ref ISyntaxNode node)
        {
            return this.ParseInclusiveOrExpression(ref node);
        }

        protected virtual bool ParseExpressionList(ref ISyntaxNode node)
        {
            ISyntaxNode node2;
            bool flag = true;
            node = new SyntaxNode(this.TokenPosition, string.Empty, 0x83);
        Label_0019:
            node2 = null;
            if (!this.ParseExpression(ref node2))
            {
                flag = false;
            }
            if (node2 != null)
            {
                node.AddChild(node2);
            }
            if (this.Token == 0x11b)
            {
                this.MoveNext();
                if (!this.Eof)
                {
                    goto Label_0019;
                }
            }
            return flag;
        }

        protected virtual bool ParseForeignKeyStatement()
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, this.TokenString, 0x22);
            this.AddNode(node);
            this.SyntaxTree.Push(node);
            try
            {
                this.MoveNext();
                if (this.Expected(0x70))
                {
                    flag = this.ParseColumnList();
                    if (this.Expected(SqlLexerToken.References))
                    {
                        if (!this.ParseReferenceStatement())
                        {
                            flag = false;
                        }
                        while (this.Token == 0x8d)
                        {
                            this.MoveNext();
                            if (!this.ParseUpdateDeleteStatement())
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

        protected virtual bool ParseFromClause()
        {
            bool flag2;
            bool flag = true;
            do
            {
                if (!this.ParseSelectTable())
                {
                    flag = false;
                }
                flag2 = true;
                switch (((SqlLexerToken) this.Token))
                {
                    case SqlLexerToken.Right:
                    case SqlLexerToken.Left:
                    {
                        Point tokenPosition = this.TokenPosition;
                        string str = this.TokenString.ToUpper();
                        this.MoveNext();
                        if (this.Token == 0x95)
                        {
                            str = str + " " + this.TokenString.ToUpper();
                        }
                        if (this.Expected(SqlLexerToken.Join))
                        {
                            str = str + " " + this.TokenString.ToUpper();
                        }
                        else
                        {
                            flag = false;
                        }
                        this.AddAttribute(new SyntaxAttribute(tokenPosition, SqlNodeType.Attribute.ToString(), str));
                        if (!this.ParseSelectTable())
                        {
                            flag = false;
                        }
                        ISyntaxNode node = null;
                        if (!this.Expected(SqlLexerToken.On) || !this.ParseExpression(ref node))
                        {
                            flag = false;
                        }
                        if (node != null)
                        {
                            this.AddNode(node);
                        }
                        break;
                    }
                    case SqlLexerToken.Comma:
                        flag2 = false;
                        this.MoveNext();
                        break;

                    case SqlLexerToken.Inner:
                    {
                        Point position = this.TokenPosition;
                        string str2 = this.TokenString.ToUpper();
                        this.MoveNext();
                        if (this.Expected(SqlLexerToken.Join))
                        {
                            str2 = str2 + " " + this.TokenString.ToUpper();
                        }
                        else
                        {
                            flag = false;
                        }
                        this.AddAttribute(new SyntaxAttribute(position, SqlNodeType.Attribute.ToString(), str2));
                        if (!this.ParseSelectTable())
                        {
                            flag = false;
                        }
                        break;
                    }
                }
            }
            while (!this.Eof && !flag2);
            return flag;
        }

        protected virtual bool ParseFromStatement()
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, this.TokenString, 0x27);
            this.AddNode(node);
            this.SyntaxTree.Push(node);
            try
            {
                this.MoveNext();
                flag = this.ParseFromClause();
            }
            finally
            {
                this.SyntaxTree.Pop();
            }
            node.Range.EndPoint = this.prevPosition;
            return flag;
        }

        protected virtual bool ParseGrantQuery()
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, this.TokenString.ToUpper(), 12, SyntaxNodeOptions.Indentation);
            this.AddNode(node);
            this.SyntaxTree.Push(node);
            try
            {
                string str;
                string str2;
                this.MoveNext();
                Point tokenPosition = this.TokenPosition;
                if (this.ParseIdentifierList(out str))
                {
                    node.AddAttribute(new SyntaxAttribute(tokenPosition, SqlNodeType.PrivilegesList.ToString(), str));
                }
                else
                {
                    flag = false;
                }
                if (!this.Expected(SqlLexerToken.On))
                {
                    flag = false;
                }
                tokenPosition = this.TokenPosition;
                if (this.ParseDatabaseObject(out str2))
                {
                    node.AddAttribute(new SyntaxAttribute(tokenPosition, SqlNodeType.DatabaseObjectAttribute.ToString(), str2));
                }
                else
                {
                    flag = false;
                }
                if (!this.Expected(SqlLexerToken.To))
                {
                    flag = false;
                }
                tokenPosition = this.TokenPosition;
                if (this.ParseUserList(out str))
                {
                    node.AddAttribute(new SyntaxAttribute(tokenPosition, SqlNodeType.UserList.ToString(), str));
                }
                else
                {
                    flag = false;
                }
                if (this.Token == 0xed)
                {
                    this.MoveNext();
                    if (!this.Expected(SqlLexerToken.Grant) || !this.Expected(SqlLexerToken.Option))
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

        protected virtual bool ParseGroupbyStatement()
        {
            bool flag = true;
            this.MoveNext();
            if (this.Expected(SqlLexerToken.By))
            {
                ISyntaxNode node = new SyntaxNode(this.TokenPosition, this.TokenString, 0x2a);
                this.AddNode(node);
                this.MoveNext();
                ISyntaxNode node2 = null;
                flag = this.ParseExpressionList(ref node2);
                if (node2 != null)
                {
                    node.AddChild(node2);
                }
                node.Range.EndPoint = this.prevPosition;
                return flag;
            }
            return false;
        }

        protected virtual bool ParseHavingStatement()
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, this.TokenString, 40);
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
            node.Range.EndPoint = this.prevPosition;
            return flag;
        }

        protected virtual bool ParseIdentifier(out string identifier)
        {
            identifier = this.TokenString;
            bool flag = this.IsReswordToken(this.Token) || (this.Token == 0x127);
            if (flag)
            {
                this.MoveNext();
                return flag;
            }
            return this.IdentifierExpected();
        }

        protected virtual bool ParseIdentifierList(out string list)
        {
            bool flag = this.ParseQualifiedIdentifier(out list);
            if (flag)
            {
                while (this.Token == 0x11b)
                {
                    string str;
                    list = list + this.TokenString;
                    this.MoveNext();
                    if (this.ParseQualifiedIdentifier(out str))
                    {
                        list = list + str;
                    }
                    else
                    {
                        flag = false;
                    }
                }
            }
            return flag;
        }

        protected virtual bool ParseInclusiveOrExpression(ref ISyntaxNode node)
        {
            bool flag = this.ParseAndExpression(ref node);
            if (this.Token == 0x91)
            {
                node = this.CreateExpressionNode(this.TokenPosition, this.TokenString, 0x37, node, true);
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

        protected virtual bool ParseInsertQuery()
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, this.TokenString.ToUpper(), 6, SyntaxNodeOptions.Outlining | SyntaxNodeOptions.Indentation);
            this.AddNode(node);
            this.SyntaxTree.Push(node);
            try
            {
                string str;
                ISyntaxNode node2;
                this.MoveNext();
                if (!this.Expected(SqlLexerToken.Into))
                {
                    flag = false;
                }
                node.AddAttribute(new SyntaxAttribute(this.prevPosition, SyntaxConsts.DeclarationScope, null));
                Point tokenPosition = this.TokenPosition;
                if (!this.ParseQualifiedIdentifier(out str))
                {
                    goto Label_0139;
                }
                node.AddAttribute(new SyntaxAttribute(tokenPosition, SqlNodeType.TableName.ToString(), str));
                if (!this.ParseOptionalColumnList())
                {
                    flag = false;
                }
                SqlLexerToken token = (SqlLexerToken) this.Token;
                switch (token)
                {
                    case SqlLexerToken.Select:
                        if (!this.ParseSelectQuery())
                        {
                            flag = false;
                        }
                        goto Label_014A;

                    case SqlLexerToken.Set:
                        this.MoveNext();
                        if (!this.ParseSetStatement())
                        {
                            flag = false;
                        }
                        goto Label_014A;

                    default:
                        if (token != SqlLexerToken.Values)
                        {
                            goto Label_012F;
                        }
                        this.MoveNext();
                        break;
                }
            Label_00BC:
                node2 = null;
                if ((!this.Expected(SqlLexerToken.Open_parens) || !this.ParseExpressionList(ref node2)) || !this.Expected(SqlLexerToken.Close_parens))
                {
                    flag = false;
                }
                if (node2 != null)
                {
                    node.AddChild(node2);
                }
                if (this.Token == 0x11b)
                {
                    this.MoveNext();
                    if (!this.Eof)
                    {
                        goto Label_00BC;
                    }
                }
                goto Label_014A;
            Label_012F:
                flag = false;
                this.SyntaxError();
                goto Label_014A;
            Label_0139:
                flag = false;
            }
            finally
            {
                this.SyntaxTree.Pop();
            }
        Label_014A:
            node.Range.EndPoint = this.prevPosition;
            return flag;
        }

        protected virtual bool ParseInvocationExpression(ref ISyntaxNode node)
        {
            bool flag = true;
            node = this.CreateExpressionNode(this.TokenPosition, this.TokenString, 0x40, node, false);
            ISyntaxNode node2 = null;
            flag = this.ParseArgumentList(ref node2);
            if (node2 != null)
            {
                node.AddChild(node2);
            }
            node.Range.EndPoint = this.prevPosition;
            return flag;
        }

        protected virtual bool ParseLimitStatement()
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, this.TokenString, 0x26);
            this.AddNode(node);
            this.MoveNext();
            ISyntaxNode node2 = null;
            flag = this.ParseExpression(ref node2);
            if (node2 != null)
            {
                node.AddChild(node2);
            }
            node.Range.EndPoint = this.prevPosition;
            return flag;
        }

        protected virtual bool ParseMemberAccess(ref ISyntaxNode node)
        {
            bool flag = true;
            node = this.CreateExpressionNode(this.TokenPosition, this.TokenString, 0x41, node, true);
            this.MoveNext();
            if (this.IsReswordToken(this.Token))
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
            node.Range.EndPoint = this.prevPosition;
            return flag;
        }

        protected virtual bool ParseMultiplicativeExpression(ref ISyntaxNode node)
        {
            bool flag = this.ParsePrefixedUnaryExpression(ref node);
            switch (this.Token)
            {
                case 0x11f:
                case 0x120:
                {
                    node = this.CreateExpressionNode(this.TokenPosition, this.TokenString, 0x39, node, true);
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

        protected virtual bool ParseOptionalColumnList()
        {
            bool flag = true;
            if (this.Token == 0x108)
            {
                string str;
                this.MoveNext();
                Point tokenPosition = this.TokenPosition;
                flag = this.ParseIdentifierList(out str);
                if (flag)
                {
                    this.AddAttribute(new SyntaxAttribute(tokenPosition, SqlNodeType.ColumnList.ToString(), str));
                }
                if (!this.Expected(SqlLexerToken.Close_parens))
                {
                    flag = false;
                }
            }
            return flag;
        }

        protected virtual bool ParseOrderbyStatement()
        {
            bool flag = true;
            this.MoveNext();
            if (this.Expected(SqlLexerToken.By))
            {
                ISyntaxNode node = new SyntaxNode(this.TokenPosition, this.TokenString, 0x29);
                this.AddNode(node);
                this.SyntaxTree.Push(node);
                try
                {
                    this.MoveNext();
                Label_0041:
                    if (!this.ParseOrderColumn())
                    {
                        flag = false;
                    }
                    if (this.Token == 0x11b)
                    {
                        this.MoveNext();
                        if (!this.Eof)
                        {
                            goto Label_0041;
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
            return false;
        }

        protected virtual bool ParseOrderColumn()
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, string.Empty, 0x47);
            this.AddNode(node);
            ISyntaxNode node2 = null;
            flag = this.ParseExpression(ref node2);
            if (node2 != null)
            {
                node.AddChild(node2);
            }
            switch (((SqlLexerToken) this.Token))
            {
                case SqlLexerToken.Asc:
                case SqlLexerToken.Desc:
                    node.AddAttribute(new SyntaxAttribute(this.TokenPosition, SqlNodeType.Attribute.ToString(), this.TokenString));
                    this.MoveNext();
                    break;
            }
            node.Range.EndPoint = this.prevPosition;
            return flag;
        }

        protected virtual bool ParseParenthesizedExpression(ref ISyntaxNode node)
        {
            bool flag = true;
            if (this.Token == 0x108)
            {
                node = new SyntaxNode(this.TokenPosition, this.TokenString, 0x42);
                this.MoveNext();
                ISyntaxNode node2 = null;
                flag = this.ParseExpression(ref node2);
                if (node2 != null)
                {
                    node.AddChild(node2);
                }
                if (!this.Expected(SqlLexerToken.Close_parens))
                {
                    flag = false;
                }
                node.Range.EndPoint = this.prevPosition;
                return flag;
            }
            this.SyntaxError(0x108);
            return false;
        }

        protected virtual bool ParsePrefixedUnaryExpression(ref ISyntaxNode node)
        {
            bool flag = true;
            switch (((SqlLexerToken) this.Token))
            {
                case SqlLexerToken.Op_Minus:
                case SqlLexerToken.Op_Plus:
                case SqlLexerToken.Not:
                {
                    node = this.CreateExpressionNode(this.TokenPosition, this.TokenString, 0x3e, node, false);
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
            bool flag = this.ParseSimpleExpression(ref node);
            if ((node != null) && !this.TryParsePostPrimaryExpression(ref node))
            {
                flag = false;
            }
            return flag;
        }

        protected virtual bool ParsePrimaryKeyStatement()
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, this.TokenString, 0x21);
            this.AddNode(node);
            this.SyntaxTree.Push(node);
            try
            {
                this.MoveNext();
                if (this.Expected(0x70))
                {
                    flag = this.ParseColumnList();
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

        protected virtual bool ParseQualifiedIdentifier(out string identifier)
        {
            bool flag = this.ParseIdentifier(out identifier);
            if (flag)
            {
                while (this.Token == 0x11c)
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

        protected virtual bool ParseReferenceStatement()
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, this.TokenString, 0x2e);
            this.AddNode(node);
            this.SyntaxTree.Push(node);
            try
            {
                string str;
                this.MoveNext();
                if (this.ParseQualifiedIdentifier(out str))
                {
                    node.Name = str;
                }
                else
                {
                    flag = false;
                }
                if (!this.ParseOptionalColumnList())
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

        protected virtual bool ParseRelationalExpression(ref ISyntaxNode node)
        {
            bool flag = this.ParseAdditiveExpression(ref node);
            switch (this.Token)
            {
                case 0x10b:
                case 0x10c:
                case 0x10d:
                case 270:
                {
                    node = this.CreateExpressionNode(this.TokenPosition, this.TokenString, 0x3b, node, true);
                    this.MoveNext();
                    ISyntaxNode node2 = null;
                    if (!this.ParseRelationalExpression(ref node))
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

        protected virtual bool ParseRevokeQuery()
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, this.TokenString.ToUpper(), 13, SyntaxNodeOptions.Indentation);
            this.AddNode(node);
            this.SyntaxTree.Push(node);
            try
            {
                string str;
                string str2;
                this.MoveNext();
                if (this.Token == 0x58)
                {
                    this.MoveNext();
                    if (!this.Expected(SqlLexerToken.Option) || !this.Expected(SqlLexerToken.For))
                    {
                        flag = false;
                    }
                }
                Point tokenPosition = this.TokenPosition;
                if (this.ParseIdentifierList(out str))
                {
                    node.AddAttribute(new SyntaxAttribute(tokenPosition, SqlNodeType.PrivilegesList.ToString(), str));
                }
                else
                {
                    flag = false;
                }
                if (!this.Expected(SqlLexerToken.On))
                {
                    flag = false;
                }
                tokenPosition = this.TokenPosition;
                if (this.ParseDatabaseObject(out str2))
                {
                    node.AddAttribute(new SyntaxAttribute(tokenPosition, SqlNodeType.DatabaseObjectAttribute.ToString(), str2));
                }
                else
                {
                    flag = false;
                }
                if (!this.Expected(SqlLexerToken.From))
                {
                    flag = false;
                }
                tokenPosition = this.TokenPosition;
                if (this.ParseUserList(out str))
                {
                    node.AddAttribute(new SyntaxAttribute(tokenPosition, SqlNodeType.UserList.ToString(), str));
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

        protected virtual bool ParseRollbackQuery()
        {
            this.AddNode(new SyntaxNode(this.TokenPosition, this.TokenString.ToUpper(), 0x10, SyntaxNodeOptions.Indentation));
            this.MoveNext();
            return true;
        }

        protected virtual bool ParseSelectColumnList()
        {
            ISyntaxNode node;
            bool flag = true;
        Label_0002:
            node = new SyntaxNode(this.TokenPosition, this.TokenString, 0x47);
            this.AddNode(node);
            ISyntaxNode node2 = null;
            if (!this.ParseExpression(ref node2))
            {
                flag = false;
            }
            if (node2 != null)
            {
                node.AddChild(node2);
            }
            if (this.Token == 11)
            {
                string str;
                this.MoveNext();
                Point tokenPosition = this.TokenPosition;
                if (this.ParseQualifiedIdentifier(out str))
                {
                    node.AddAttribute(new SyntaxAttribute(tokenPosition, SqlNodeType.ColumnAlias.ToString(), str));
                }
                else
                {
                    flag = false;
                }
            }
            node.Range.EndPoint = this.prevPosition;
            if (this.Token == 0x11b)
            {
                this.MoveNext();
                if (!this.Eof)
                {
                    goto Label_0002;
                }
            }
            return flag;
        }

        protected virtual bool ParseSelectQuery()
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, this.TokenString.ToUpper(), 5, SyntaxNodeOptions.Outlining | SyntaxNodeOptions.Indentation);
            this.AddNode(node);
            this.SyntaxTree.Push(node);
            try
            {
                this.MoveNext();
                node.AddAttribute(new SyntaxAttribute(this.prevPosition, SyntaxConsts.DeclarationScope, null));
                switch (((SqlLexerToken) this.Token))
                {
                    case SqlLexerToken.All:
                    case SqlLexerToken.Distinct:
                        node.AddAttribute(new SyntaxAttribute(this.TokenPosition, SqlNodeType.Attribute.ToString(), this.TokenString.ToUpper()));
                        this.MoveNext();
                        break;
                }
                if (!this.ParseSelectColumnList())
                {
                    flag = false;
                }
                bool flag2 = false;
                do
                {
                    switch (((SqlLexerToken) this.Token))
                    {
                        case SqlLexerToken.Order:
                            if (!this.ParseOrderbyStatement())
                            {
                                flag = false;
                            }
                            break;

                        case SqlLexerToken.Where:
                            if (!this.ParseWhereStatement())
                            {
                                flag = false;
                            }
                            break;

                        case SqlLexerToken.Group:
                            if (!this.ParseGroupbyStatement())
                            {
                                flag = false;
                            }
                            break;

                        case SqlLexerToken.Having:
                            if (!this.ParseHavingStatement())
                            {
                                flag = false;
                            }
                            break;

                        case SqlLexerToken.From:
                            if (!this.ParseFromStatement())
                            {
                                flag = false;
                            }
                            break;

                        default:
                            flag2 = true;
                            break;
                    }
                }
                while (!this.Eof && !flag2);
                if (this.Token == 0x11d)
                {
                    this.MoveNext();
                }
            }
            finally
            {
                this.SyntaxTree.Pop();
            }
            node.Range.EndPoint = this.prevPosition;
            return flag;
        }

        protected virtual bool ParseSelectTable()
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, string.Empty, 0x43);
            this.AddNode(node);
            this.SyntaxTree.Push(node);
            try
            {
                string str;
                if (this.Token == 0xb9)
                {
                    if (!this.ParseSelectQuery())
                    {
                        flag = false;
                    }
                }
                else if (this.ParseQualifiedIdentifier(out str))
                {
                    node.Name = str;
                }
                else
                {
                    flag = false;
                }
                if (this.Token == 11)
                {
                    this.MoveNext();
                    Point tokenPosition = this.TokenPosition;
                    if (this.ParseQualifiedIdentifier(out str))
                    {
                        this.AddAttribute(new SyntaxAttribute(tokenPosition, SqlNodeType.TableAlias.ToString(), str));
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
            node.Range.EndPoint = this.prevPosition;
            return flag;
        }

        protected virtual bool ParseSetQuery()
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, this.TokenString.ToUpper(), 9, SyntaxNodeOptions.Indentation);
            this.AddNode(node);
            this.SyntaxTree.Push(node);
            try
            {
                Point point2;
                Point point3;
                string str;
                string str2;
                Point point4;
                this.MoveNext();
                SqlLexerToken token = (SqlLexerToken) this.Token;
                if (token != SqlLexerToken.Auto)
                {
                    if (token == SqlLexerToken.Schema)
                    {
                        goto Label_0153;
                    }
                    if (token == SqlLexerToken.Transaction)
                    {
                        goto Label_00D6;
                    }
                    goto Label_018E;
                }
                Point tokenPosition = this.TokenPosition;
                this.MoveNext();
                flag = this.Expected(SqlLexerToken.Commit);
                if (flag)
                {
                    ISyntaxAttribute attr = new SyntaxAttribute(tokenPosition, SqlNodeType.AutoCommitAttribute.ToString(), null);
                    if ((this.Token == 0x8d) || (this.Token == 0x8b))
                    {
                        attr.Value = this.TokenString;
                        node.AddAttribute(attr);
                        this.MoveNext();
                    }
                    else
                    {
                        this.SyntaxError();
                        flag = false;
                    }
                }
                goto Label_01F8;
            Label_00D6:
                point2 = this.TokenPosition;
                this.MoveNext();
                flag = this.Expected(SqlLexerToken.Isolation) && this.Expected(SqlLexerToken.Level);
                if (flag)
                {
                    ISyntaxAttribute attribute2 = new SyntaxAttribute(point2, SqlNodeType.IsolationLevelAttribute.ToString(), null);
                    if (this.Token == 0xbc)
                    {
                        attribute2.Value = this.TokenString;
                        node.AddAttribute(attribute2);
                        this.MoveNext();
                    }
                    else
                    {
                        this.SyntaxError();
                        flag = false;
                    }
                }
                goto Label_01F8;
            Label_0153:
                point3 = this.TokenPosition;
                this.MoveNext();
                if (this.ParseQualifiedIdentifier(out str))
                {
                    node.AddAttribute(new SyntaxAttribute(point3, SqlNodeType.SchemaName.ToString(), str));
                }
                else
                {
                    flag = false;
                }
                goto Label_01F8;
            Label_018E:
                point4 = this.TokenPosition;
                if (this.ParseQualifiedIdentifier(out str2))
                {
                    node.AddAttribute(new SyntaxAttribute(point4, SqlNodeType.VariableName.ToString(), str2));
                    ISyntaxNode node2 = null;
                    if (!this.Expected(SqlLexerToken.Op_Assign) || !this.ParseExpression(ref node2))
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
        Label_01F8:
            node.Range.EndPoint = this.prevPosition;
            return flag;
        }

        protected virtual bool ParseSetStatement()
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, this.TokenString, 0x2f);
            this.AddNode(node);
            this.SyntaxTree.Push(node);
            try
            {
                do
                {
                    string str;
                    Point tokenPosition = this.TokenPosition;
                    if (this.ParseQualifiedIdentifier(out str))
                    {
                        SyntaxNode node2 = new SyntaxNode(tokenPosition, str, 0x47);
                        this.AddNode(node2);
                        ISyntaxNode node3 = null;
                        if (!this.Expected(SqlLexerToken.Op_Assign) || !this.ParseExpression(ref node3))
                        {
                            flag = false;
                        }
                        if (node3 != null)
                        {
                            node2.AddChild(node3);
                        }
                        node2.Range.EndPoint = this.prevPosition;
                        if (this.Token != 0x11b)
                        {
                            goto Label_00B4;
                        }
                        this.MoveNext();
                    }
                }
                while (!this.Eof);
            }
            finally
            {
                this.SyntaxTree.Pop();
            }
        Label_00B4:
            node.Range.EndPoint = this.prevPosition;
            return flag;
        }

        protected virtual bool ParseShowQuery()
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, this.TokenString.ToUpper(), 10, SyntaxNodeOptions.Indentation);
            this.AddNode(node);
            this.SyntaxTree.Push(node);
            try
            {
                this.MoveNext();
                SqlLexerToken token = (SqlLexerToken) this.Token;
                if (token <= SqlLexerToken.Schema)
                {
                    switch (token)
                    {
                        case SqlLexerToken.Connections:
                        case SqlLexerToken.Schema:
                            goto Label_0064;
                    }
                    goto Label_0091;
                }
                if ((token != SqlLexerToken.Status) && (token != SqlLexerToken.Tables))
                {
                    goto Label_0091;
                }
            Label_0064:
                node.AddAttribute(new SyntaxAttribute(this.TokenPosition, SqlNodeType.Attribute.ToString(), this.TokenString));
                this.MoveNext();
                goto Label_00A8;
            Label_0091:
                flag = false;
                this.SyntaxError();
            }
            finally
            {
                this.SyntaxTree.Pop();
            }
        Label_00A8:
            node.Range.EndPoint = this.prevPosition;
            return flag;
        }

        protected virtual bool ParseShutdownQuery()
        {
            this.AddNode(new SyntaxNode(this.TokenPosition, this.TokenString.ToUpper(), 0x11, SyntaxNodeOptions.Indentation));
            this.MoveNext();
            return true;
        }

        protected virtual bool ParseSimpleExpression(ref ISyntaxNode node)
        {
            bool flag = true;
            switch (((SqlLexerToken) this.Token))
            {
                case SqlLexerToken.Op_Star:
                case SqlLexerToken.Integer_Literal:
                case SqlLexerToken.Float_Literal:
                case SqlLexerToken.Double_Literal:
                case SqlLexerToken.String_Literal:
                case SqlLexerToken.Identifier_Literal:
                case SqlLexerToken.False:
                case SqlLexerToken.True:
                    node = new SyntaxNode(this.TokenPosition, this.TokenString, 0x3d);
                    this.MoveNext();
                    return flag;

                case SqlLexerToken.Open_parens:
                    return this.ParseParenthesizedExpression(ref node);
            }
            if (this.IsReswordToken(this.Token))
            {
                node = new SyntaxNode(this.TokenPosition, this.TokenString, 0x3d);
                this.MoveNext();
                return flag;
            }
            flag = false;
            this.SyntaxError();
            this.MoveNext();
            return flag;
        }

        protected virtual bool ParseTriggeredAction()
        {
            bool flag = true;
            string str = this.TokenString.ToUpper();
            Point tokenPosition = this.TokenPosition;
            SqlLexerToken token = (SqlLexerToken) this.Token;
            if (token == SqlLexerToken.Cascade)
            {
                this.AddAttribute(new SyntaxAttribute(tokenPosition, SqlNodeType.TriggerAttribute.ToString(), str));
                return flag;
            }
            if (token != SqlLexerToken.No)
            {
                if (token == SqlLexerToken.Set)
                {
                    this.MoveNext();
                    switch (((SqlLexerToken) this.Token))
                    {
                        case SqlLexerToken.Default:
                        case SqlLexerToken.Null:
                            this.AddAttribute(new SyntaxAttribute(tokenPosition, SqlNodeType.TriggerAttribute.ToString(), str + " " + this.TokenString.ToUpper()));
                            this.MoveNext();
                            return flag;
                    }
                    flag = false;
                    this.SyntaxError();
                    return flag;
                }
                flag = false;
                this.SyntaxError();
                return flag;
            }
            this.MoveNext();
            if (this.Token == 4)
            {
                this.AddAttribute(new SyntaxAttribute(tokenPosition, SqlNodeType.TriggerAttribute.ToString(), str + " " + this.TokenString.ToUpper()));
                this.MoveNext();
                return flag;
            }
            return this.Expected(SqlLexerToken.Action);
        }

        protected virtual bool ParseType(out string type)
        {
            bool flag = false;
            type = string.Empty;
            flag = this.IsDatatypeToken(this.Token);
            if (flag)
            {
                bool flag2 = this.IsDateOrIntervalToken(this.Token);
                type = this.TokenString;
                this.MoveNext();
                if (flag2 && (this.Token == 0x126))
                {
                    this.MoveNext();
                }
                if (this.Token == 0x108)
                {
                    this.MoveNext();
                    ISyntaxNode node = null;
                    flag = this.ParseExpressionList(ref node) && this.Expected(SqlLexerToken.Close_parens);
                }
            }
            return flag;
        }

        protected virtual bool ParseUniqueStatement()
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, this.TokenString, 0x24);
            this.AddNode(node);
            this.SyntaxTree.Push(node);
            try
            {
                this.MoveNext();
                flag = this.ParseColumnList();
            }
            finally
            {
                this.SyntaxTree.Pop();
            }
            node.Range.EndPoint = this.prevPosition;
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
            return flag;
        }

        protected virtual bool ParseUnitBody()
        {
            bool flag = true;
            ISyntaxNode current = this.SyntaxTree.Current;
            ISyntaxNode root = this.SyntaxTree.Root;
            while (!this.Eof)
            {
                switch (((SqlLexerToken) this.Token))
                {
                    case SqlLexerToken.Delete:
                        flag = this.ParseDeleteQuery();
                        break;

                    case SqlLexerToken.Describe:
                        flag = this.ParseDescribeQuery();
                        break;

                    case SqlLexerToken.Drop:
                        flag = this.ParseDropQuery();
                        break;

                    case SqlLexerToken.Commit:
                        flag = this.ParseCommitQuery();
                        break;

                    case SqlLexerToken.Compact:
                        flag = this.ParseCompactQuery();
                        break;

                    case SqlLexerToken.Create:
                        flag = this.ParseCreateQuery();
                        break;

                    case SqlLexerToken.Alter:
                        flag = this.ParseAlterQuery();
                        break;

                    case SqlLexerToken.Revoke:
                        flag = this.ParseRevokeQuery();
                        break;

                    case SqlLexerToken.Rollback:
                        flag = this.ParseRollbackQuery();
                        break;

                    case SqlLexerToken.Insert:
                        flag = this.ParseInsertQuery();
                        break;

                    case SqlLexerToken.Grant:
                        flag = this.ParseGrantQuery();
                        break;

                    case SqlLexerToken.Set:
                        flag = this.ParseSetQuery();
                        break;

                    case SqlLexerToken.Show:
                        flag = this.ParseShowQuery();
                        break;

                    case SqlLexerToken.Shutdown:
                        flag = this.ParseShutdownQuery();
                        break;

                    case SqlLexerToken.Update:
                        flag = this.ParseUpdateQuery();
                        break;

                    case SqlLexerToken.Select:
                        flag = this.ParseSelectQuery();
                        break;

                    default:
                        flag = false;
                        this.SyntaxError();
                        this.MoveNext();
                        break;
                }
                if (this.Token == 0x11d)
                {
                    this.MoveNext();
                }
            }
            return flag;
        }

        protected virtual bool ParseUpdateDeleteStatement()
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, this.TokenString);
            this.MoveNext();
            SqlLexerToken token = (SqlLexerToken) this.Token;
            if (token != SqlLexerToken.Delete)
            {
                if (token != SqlLexerToken.Update)
                {
                    flag = false;
                }
                else
                {
                    node.NodeType = 0x2d;
                    this.AddNode(node);
                    this.MoveNext();
                    flag = this.ParseTriggeredAction();
                }
            }
            else
            {
                node.NodeType = 0x2c;
                this.AddNode(node);
                this.MoveNext();
                flag = this.ParseTriggeredAction();
            }
            node.Range.EndPoint = this.prevPosition;
            return flag;
        }

        protected virtual bool ParseUpdateQuery()
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, this.TokenString.ToUpper(), 7, SyntaxNodeOptions.Outlining | SyntaxNodeOptions.Indentation);
            this.AddNode(node);
            this.SyntaxTree.Push(node);
            try
            {
                string str;
                this.MoveNext();
                node.AddAttribute(new SyntaxAttribute(this.prevPosition, SyntaxConsts.DeclarationScope, null));
                Point tokenPosition = this.TokenPosition;
                if (this.ParseQualifiedIdentifier(out str))
                {
                    node.AddAttribute(new SyntaxAttribute(tokenPosition, SqlNodeType.TableName.ToString(), str));
                    if (this.Expected(190))
                    {
                        if (!this.ParseSetStatement())
                        {
                            flag = false;
                        }
                    }
                    else
                    {
                        flag = false;
                    }
                    if ((this.Token == 0xeb) && !this.ParseWhereStatement())
                    {
                        flag = false;
                    }
                    if ((this.Token == 0x74) && !this.ParseLimitStatement())
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

        protected virtual bool ParseUser(out string identifier)
        {
            if (this.Token == 160)
            {
                identifier = this.TokenString;
                this.MoveNext();
                return true;
            }
            return this.ParseQualifiedIdentifier(out identifier);
        }

        protected virtual bool ParseUserList(out string list)
        {
            bool flag = this.ParseUser(out list);
            if (flag)
            {
                while (this.Token == 0x11b)
                {
                    string str;
                    list = list + this.TokenString;
                    this.MoveNext();
                    if (this.ParseUser(out str))
                    {
                        list = list + str;
                    }
                    else
                    {
                        flag = false;
                    }
                }
            }
            return flag;
        }

        protected virtual bool ParseWhereStatement()
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, this.TokenString, 0x25);
            this.AddNode(node);
            this.MoveNext();
            ISyntaxNode node2 = null;
            flag = this.ParseExpression(ref node2);
            if (node2 != null)
            {
                node.AddChild(node2);
            }
            node.Range.EndPoint = this.prevPosition;
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
            this.Options = SyntaxParserConsts.DefaultNetSyntaxOptions;
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
                ISyntaxError err = new QWhale.Syntax.SyntaxError(this.TokenPosition, this.TokenString, ((SqlLexerToken) token).ToString() + " " + StringConsts.ErrExpected);
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

        protected virtual bool TryParsePostPrimaryExpression(ref ISyntaxNode node)
        {
            bool flag = true;
            while (!this.Eof)
            {
                SqlLexerToken token = (SqlLexerToken) this.Token;
                if (token != SqlLexerToken.Open_parens)
                {
                    if ((token != SqlLexerToken.Bang) && (token != SqlLexerToken.Dot))
                    {
                        return flag;
                    }
                    if (!this.ParseMemberAccess(ref node))
                    {
                        flag = false;
                    }
                }
                else
                {
                    if (!this.ParseInvocationExpression(ref node))
                    {
                        flag = false;
                    }
                    continue;
                }
            }
            return flag;
        }

        public override bool CaseSensitive
        {
            get
            {
                return false;
            }
        }
    }
}

