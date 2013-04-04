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

    [ToolboxBitmap(typeof(CsParser), "Images.SQLParser.bmp"), ToolboxItem(true)]
    public class MSSQLParser : SyntaxParser
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
        protected Point prevPosition;
        private Hashtable reswords;
        private const int stateComment = 1;
        private const int stateDefine = 2;
        private const int stateNormal = 0;
        private const int stateString = 3;
        private MsSqlLexerToken[] tokens;

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

        public override ICodeCompletionRepository CreateRepository()
        {
            return new SqlRepository(this.CaseSensitive, this.SyntaxTree);
        }

        protected bool Expected(MsSqlLexerToken token)
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
            return this.Expected((MsSqlLexerToken) token);
        }

        protected bool Expected(MsSqlLexerToken[] tokens)
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

        protected bool Expected(MsSqlLexerToken token1, MsSqlLexerToken token2)
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
            if (ch == '.')
            {
                return CodeCompletionType.ListMembers;
            }
            return base.GetCompletionType(ch);
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
            if ((token >= 0x133) && (token <= 0x14c))
            {
                return 5;
            }
            switch (token)
            {
                case 0x14d:
                case 0x14e:
                case 0x14f:
                    return 1;

                case 0x150:
                    return 7;

                case 0x151:
                    return 0;

                case 0x152:
                    return 3;

                case 340:
                    return 8;
            }
            return 6;
        }

        public override string GetSingleLineComment()
        {
            return "--";
        }

        protected bool IdentifierExpected()
        {
            return this.Expected(MsSqlLexerToken.Identifier_Literal);
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
            this.lexStringEndProc = new LexerProc(this.LexStringEnd);
            base.RegisterLexerProc(0, this.lexWhitespaceProc);
            base.RegisterLexerProc(0, '!', '\x00ff', this.lexSymbolProc);
            base.RegisterLexerProc(0, 'a', 'z', this.lexIdentifierProc);
            base.RegisterLexerProc(0, 'A', 'Z', this.lexIdentifierProc);
            base.RegisterLexerProc(0, new char[] { '"', '_', '$', '#', '@' }, this.lexIdentifierProc);
            base.RegisterLexerProc(0, '0', '9', this.lexNumberProc);
            base.RegisterLexerProc(0, '+', this.lexNumberProc);
            base.RegisterLexerProc(0, new char[] { 'B', 'b', '\'' }, this.lexStringProc);
            base.RegisterLexerProc(0, new char[] { '/', '-' }, this.lexCommentProc);
            base.RegisterLexerProc(1, this.lexCommentEndProc);
            base.RegisterLexerProc(0, '#', this.lexDefineProc);
            base.RegisterLexerProc(2, this.lexDefineEndProc);
            base.RegisterLexerProc(3, this.lexStringEndProc);
        }

        protected virtual void InitReswords()
        {
            this.tokens = new MsSqlLexerToken[] { MsSqlLexerToken.Table, MsSqlLexerToken.View, MsSqlLexerToken.Sequence, MsSqlLexerToken.Schema, MsSqlLexerToken.User, MsSqlLexerToken.Into, MsSqlLexerToken.From };
            this.reswords = new Hashtable();
            this.reswords.Add("abort", MsSqlLexerToken.Abort);
            this.reswords.Add("accept", MsSqlLexerToken.Accept);
            this.reswords.Add("access", MsSqlLexerToken.Access);
            this.reswords.Add("account", MsSqlLexerToken.Account);
            this.reswords.Add("action", MsSqlLexerToken.Action);
            this.reswords.Add("add", MsSqlLexerToken.Add);
            this.reswords.Add("all", MsSqlLexerToken.All);
            this.reswords.Add("alter", MsSqlLexerToken.Alter);
            this.reswords.Add("and", MsSqlLexerToken.And);
            this.reswords.Add("any", MsSqlLexerToken.Any);
            this.reswords.Add("array", MsSqlLexerToken.Array);
            this.reswords.Add("as", MsSqlLexerToken.As);
            this.reswords.Add("asc", MsSqlLexerToken.Asc);
            this.reswords.Add("assert", MsSqlLexerToken.Assert);
            this.reswords.Add("assign", MsSqlLexerToken.Assign);
            this.reswords.Add("at", MsSqlLexerToken.At);
            this.reswords.Add("audit", MsSqlLexerToken.Audit);
            this.reswords.Add("authorization", MsSqlLexerToken.Authorization);
            this.reswords.Add("auto", MsSqlLexerToken.Auto);
            this.reswords.Add("avg", MsSqlLexerToken.Avg);
            this.reswords.Add("body", MsSqlLexerToken.Body);
            this.reswords.Add("base64", MsSqlLexerToken.Base64);
            this.reswords.Add("between", MsSqlLexerToken.Between);
            this.reswords.Add("browse", MsSqlLexerToken.Browse);
            this.reswords.Add("by", MsSqlLexerToken.By);
            this.reswords.Add("case", MsSqlLexerToken.Case);
            this.reswords.Add("cascade", MsSqlLexerToken.Cascade);
            this.reswords.Add("casche", MsSqlLexerToken.Cashe);
            this.reswords.Add("char_base", MsSqlLexerToken.Char_base);
            this.reswords.Add("check", MsSqlLexerToken.Check);
            this.reswords.Add("close", MsSqlLexerToken.Close);
            this.reswords.Add("cluster", MsSqlLexerToken.Cluster);
            this.reswords.Add("clusters", MsSqlLexerToken.Clusters);
            this.reswords.Add("colauth", MsSqlLexerToken.Colauth);
            this.reswords.Add("column", MsSqlLexerToken.Column);
            this.reswords.Add("columns", MsSqlLexerToken.Columns);
            this.reswords.Add("comment", MsSqlLexerToken.Comment);
            this.reswords.Add("commit", MsSqlLexerToken.Commit);
            this.reswords.Add("compact", MsSqlLexerToken.Compact);
            this.reswords.Add("compress", MsSqlLexerToken.Compress);
            this.reswords.Add("compute", MsSqlLexerToken.Compute);
            this.reswords.Add("concat", MsSqlLexerToken.Concat);
            this.reswords.Add("connect", MsSqlLexerToken.Connect);
            this.reswords.Add("connections", MsSqlLexerToken.Connections);
            this.reswords.Add("constant", MsSqlLexerToken.Constant);
            this.reswords.Add("constraint", MsSqlLexerToken.Constraint);
            this.reswords.Add("count", MsSqlLexerToken.Count);
            this.reswords.Add("crash", MsSqlLexerToken.Crash);
            this.reswords.Add("create", MsSqlLexerToken.Create);
            this.reswords.Add("cross", MsSqlLexerToken.Cross);
            this.reswords.Add("cube", MsSqlLexerToken.Cube);
            this.reswords.Add("current", MsSqlLexerToken.Current);
            this.reswords.Add("cursor", MsSqlLexerToken.Cursor);
            this.reswords.Add("cycle", MsSqlLexerToken.Cycle);
            this.reswords.Add("data_base", MsSqlLexerToken.Data_base);
            this.reswords.Add("database", MsSqlLexerToken.Database);
            this.reswords.Add("dba", MsSqlLexerToken.Dba);
            this.reswords.Add("debugoff", MsSqlLexerToken.Debugoff);
            this.reswords.Add("debugon", MsSqlLexerToken.Debugon);
            this.reswords.Add("declare", MsSqlLexerToken.Declare);
            this.reswords.Add("default", MsSqlLexerToken.Default);
            this.reswords.Add("defferable", MsSqlLexerToken.Defferable);
            this.reswords.Add("deffered", MsSqlLexerToken.Deffered);
            this.reswords.Add("definition", MsSqlLexerToken.Definition);
            this.reswords.Add("delay", MsSqlLexerToken.Delay);
            this.reswords.Add("delete", MsSqlLexerToken.Delete);
            this.reswords.Add("delta", MsSqlLexerToken.Delta);
            this.reswords.Add("desc", MsSqlLexerToken.Desc);
            this.reswords.Add("describe", MsSqlLexerToken.Describe);
            this.reswords.Add("digits", MsSqlLexerToken.Digits);
            this.reswords.Add("dispose", MsSqlLexerToken.Dispose);
            this.reswords.Add("distinct", MsSqlLexerToken.Distinct);
            this.reswords.Add("do", MsSqlLexerToken.Do);
            this.reswords.Add("drop", MsSqlLexerToken.Drop);
            this.reswords.Add("elements", MsSqlLexerToken.Elements);
            this.reswords.Add("else", MsSqlLexerToken.Else);
            this.reswords.Add("elsif", MsSqlLexerToken.Elsif);
            this.reswords.Add("end", MsSqlLexerToken.End);
            this.reswords.Add("entry", MsSqlLexerToken.Entry);
            this.reswords.Add("exception", MsSqlLexerToken.Exception);
            this.reswords.Add("exception_init", MsSqlLexerToken.Exception_init);
            this.reswords.Add("exclusive", MsSqlLexerToken.Exclusive);
            this.reswords.Add("exists", MsSqlLexerToken.Exists);
            this.reswords.Add("exit", MsSqlLexerToken.Exit);
            this.reswords.Add("expand", MsSqlLexerToken.Expand);
            this.reswords.Add("explicit", MsSqlLexerToken.Explicit);
            this.reswords.Add("false", MsSqlLexerToken.False);
            this.reswords.Add("fast", MsSqlLexerToken.Fast);
            this.reswords.Add("fastfirstrow", MsSqlLexerToken.Fastfirstrow);
            this.reswords.Add("fetch", MsSqlLexerToken.Fetch);
            this.reswords.Add("file", MsSqlLexerToken.File);
            this.reswords.Add("for", MsSqlLexerToken.For);
            this.reswords.Add("force", MsSqlLexerToken.Force);
            this.reswords.Add("foreign", MsSqlLexerToken.Foreign);
            this.reswords.Add("form", MsSqlLexerToken.Form);
            this.reswords.Add("from", MsSqlLexerToken.From);
            this.reswords.Add("full", MsSqlLexerToken.Full);
            this.reswords.Add("function", MsSqlLexerToken.Function);
            this.reswords.Add("generic", MsSqlLexerToken.Generic);
            this.reswords.Add("goto", MsSqlLexerToken.Goto);
            this.reswords.Add("grant", MsSqlLexerToken.Grant);
            this.reswords.Add("group", MsSqlLexerToken.Group);
            this.reswords.Add("groups", MsSqlLexerToken.Groups);
            this.reswords.Add("hash", MsSqlLexerToken.Hash);
            this.reswords.Add("having", MsSqlLexerToken.Having);
            this.reswords.Add("holdlock", MsSqlLexerToken.Holdlock);
            this.reswords.Add("identfied", MsSqlLexerToken.Identfied);
            this.reswords.Add("identified", MsSqlLexerToken.Identified);
            this.reswords.Add("identitycol", MsSqlLexerToken.IdentityCol);
            this.reswords.Add("if", MsSqlLexerToken.If);
            this.reswords.Add("immediate", MsSqlLexerToken.Immediate);
            this.reswords.Add("in", MsSqlLexerToken.In);
            this.reswords.Add("increment", MsSqlLexerToken.Increment);
            this.reswords.Add("index", MsSqlLexerToken.Index);
            this.reswords.Add("index_blist", MsSqlLexerToken.Index_Blist);
            this.reswords.Add("indexes", MsSqlLexerToken.Indexes);
            this.reswords.Add("index_none", MsSqlLexerToken.Index_None);
            this.reswords.Add("indicator", MsSqlLexerToken.Indicator);
            this.reswords.Add("initial", MsSqlLexerToken.Initial);
            this.reswords.Add("initially", MsSqlLexerToken.Initially);
            this.reswords.Add("inner", MsSqlLexerToken.Inner);
            this.reswords.Add("insert", MsSqlLexerToken.Insert);
            this.reswords.Add("intersect", MsSqlLexerToken.Intersect);
            this.reswords.Add("into", MsSqlLexerToken.Into);
            this.reswords.Add("is", MsSqlLexerToken.Is);
            this.reswords.Add("isolation", MsSqlLexerToken.Isolation);
            this.reswords.Add("join", MsSqlLexerToken.Join);
            this.reswords.Add("keep", MsSqlLexerToken.Keep);
            this.reswords.Add("keepfixed", MsSqlLexerToken.Keepfixed);
            this.reswords.Add("key", MsSqlLexerToken.Key);
            this.reswords.Add("left", MsSqlLexerToken.Left);
            this.reswords.Add("level", MsSqlLexerToken.Level);
            this.reswords.Add("like", MsSqlLexerToken.Like);
            this.reswords.Add("limit", MsSqlLexerToken.Limit);
            this.reswords.Add("limited", MsSqlLexerToken.Limited);
            this.reswords.Add("lock", MsSqlLexerToken.Lock);
            this.reswords.Add("long", MsSqlLexerToken.Long);
            this.reswords.Add("loop", MsSqlLexerToken.Loop);
            this.reswords.Add("max", MsSqlLexerToken.Max);
            this.reswords.Add("maxdop", MsSqlLexerToken.Maxdop);
            this.reswords.Add("maxextents", MsSqlLexerToken.Maxextents);
            this.reswords.Add("merge", MsSqlLexerToken.Merge);
            this.reswords.Add("min", MsSqlLexerToken.Min);
            this.reswords.Add("minvalue", MsSqlLexerToken.Minvalue);
            this.reswords.Add("minus", MsSqlLexerToken.Minus);
            this.reswords.Add("mod", MsSqlLexerToken.Mod);
            this.reswords.Add("mode", MsSqlLexerToken.Mode);
            this.reswords.Add("modify", MsSqlLexerToken.Modify);
            this.reswords.Add("new", MsSqlLexerToken.New);
            this.reswords.Add("no", MsSqlLexerToken.No);
            this.reswords.Add("noaudit", MsSqlLexerToken.Noaudit);
            this.reswords.Add("nocompress", MsSqlLexerToken.Nocompress);
            this.reswords.Add("nolock", MsSqlLexerToken.Nolock);
            this.reswords.Add("not", MsSqlLexerToken.Not);
            this.reswords.Add("nowait", MsSqlLexerToken.Nowait);
            this.reswords.Add("null", MsSqlLexerToken.Null);
            this.reswords.Add("number", MsSqlLexerToken.Number);
            this.reswords.Add("number_base", MsSqlLexerToken.Number_base);
            this.reswords.Add("of", MsSqlLexerToken.Of);
            this.reswords.Add("off", MsSqlLexerToken.Off);
            this.reswords.Add("offline", MsSqlLexerToken.Offline);
            this.reswords.Add("on", MsSqlLexerToken.On);
            this.reswords.Add("online", MsSqlLexerToken.Online);
            this.reswords.Add("open", MsSqlLexerToken.Open);
            this.reswords.Add("option", MsSqlLexerToken.Option);
            this.reswords.Add("or", MsSqlLexerToken.Or);
            this.reswords.Add("order", MsSqlLexerToken.Order);
            this.reswords.Add("others", MsSqlLexerToken.Others);
            this.reswords.Add("out", MsSqlLexerToken.Out);
            this.reswords.Add("outer", MsSqlLexerToken.Outer);
            this.reswords.Add("package", MsSqlLexerToken.Package);
            this.reswords.Add("paglock", MsSqlLexerToken.Paglock);
            this.reswords.Add("partition", MsSqlLexerToken.Partition);
            this.reswords.Add("password", MsSqlLexerToken.Password);
            this.reswords.Add("pctfree", MsSqlLexerToken.Pctfree);
            this.reswords.Add("percent", MsSqlLexerToken.Percent);
            this.reswords.Add("plan", MsSqlLexerToken.Plan);
            this.reswords.Add("pragma", MsSqlLexerToken.Pragma);
            this.reswords.Add("primary", MsSqlLexerToken.Primary);
            this.reswords.Add("prior", MsSqlLexerToken.Prior);
            this.reswords.Add("private", MsSqlLexerToken.Private);
            this.reswords.Add("privileges", MsSqlLexerToken.Privileges);
            this.reswords.Add("procedure", MsSqlLexerToken.Procedure);
            this.reswords.Add("public", MsSqlLexerToken.Public);
            this.reswords.Add("raise", MsSqlLexerToken.Raise);
            this.reswords.Add("range", MsSqlLexerToken.Range);
            this.reswords.Add("raw", MsSqlLexerToken.Raw);
            this.reswords.Add("readcommited", MsSqlLexerToken.Readcommited);
            this.reswords.Add("readpast", MsSqlLexerToken.Readpast);
            this.reswords.Add("readuncommited", MsSqlLexerToken.Readuncommited);
            this.reswords.Add("record", MsSqlLexerToken.Record);
            this.reswords.Add("references", MsSqlLexerToken.References);
            this.reswords.Add("release", MsSqlLexerToken.Release);
            this.reswords.Add("rem", MsSqlLexerToken.Rem);
            this.reswords.Add("rename", MsSqlLexerToken.Rename);
            this.reswords.Add("repeatableread", MsSqlLexerToken.Repeatableread);
            this.reswords.Add("replace", MsSqlLexerToken.Replace);
            this.reswords.Add("resource", MsSqlLexerToken.Resource);
            this.reswords.Add("return", MsSqlLexerToken.Return);
            this.reswords.Add("reverse", MsSqlLexerToken.Reverse);
            this.reswords.Add("revoke", MsSqlLexerToken.Revoke);
            this.reswords.Add("right", MsSqlLexerToken.Right);
            this.reswords.Add("robust", MsSqlLexerToken.Robust);
            this.reswords.Add("rollback", MsSqlLexerToken.Rollback);
            this.reswords.Add("rollup", MsSqlLexerToken.Rollup);
            this.reswords.Add("row", MsSqlLexerToken.Row);
            this.reswords.Add("rowguidcol", MsSqlLexerToken.RowGuidCol);
            this.reswords.Add("rowid", MsSqlLexerToken.Rowid);
            this.reswords.Add("rowlabel", MsSqlLexerToken.Rowlabel);
            this.reswords.Add("rowlock", MsSqlLexerToken.Rowlock);
            this.reswords.Add("rownum", MsSqlLexerToken.Rownum);
            this.reswords.Add("rows", MsSqlLexerToken.Rows);
            this.reswords.Add("rowtype", MsSqlLexerToken.Rowtype);
            this.reswords.Add("run", MsSqlLexerToken.Run);
            this.reswords.Add("savepoint", MsSqlLexerToken.Savepoint);
            this.reswords.Add("schema", MsSqlLexerToken.Schema);
            this.reswords.Add("select", MsSqlLexerToken.Select);
            this.reswords.Add("separate", MsSqlLexerToken.Separate);
            this.reswords.Add("sequence", MsSqlLexerToken.Sequence);
            this.reswords.Add("serializable", MsSqlLexerToken.Serializable);
            this.reswords.Add("session", MsSqlLexerToken.Session);
            this.reswords.Add("set", MsSqlLexerToken.Set);
            this.reswords.Add("share", MsSqlLexerToken.Share);
            this.reswords.Add("show", MsSqlLexerToken.Show);
            this.reswords.Add("shutdown", MsSqlLexerToken.Shutdown);
            this.reswords.Add("size", MsSqlLexerToken.Size);
            this.reswords.Add("space", MsSqlLexerToken.Space);
            this.reswords.Add("sql", MsSqlLexerToken.Sql);
            this.reswords.Add("sqlcode", MsSqlLexerToken.Sqlcode);
            this.reswords.Add("sqlerrm", MsSqlLexerToken.Sqlerrm);
            this.reswords.Add("start", MsSqlLexerToken.Start);
            this.reswords.Add("statement", MsSqlLexerToken.Statement);
            this.reswords.Add("status", MsSqlLexerToken.Status);
            this.reswords.Add("stddev", MsSqlLexerToken.Stddev);
            this.reswords.Add("stdevp", MsSqlLexerToken.Stdevp);
            this.reswords.Add("subtype", MsSqlLexerToken.Subtype);
            this.reswords.Add("successful", MsSqlLexerToken.Successful);
            this.reswords.Add("sum", MsSqlLexerToken.Sum);
            this.reswords.Add("synonym", MsSqlLexerToken.Synonym);
            this.reswords.Add("sysdate", MsSqlLexerToken.Sysdate);
            this.reswords.Add("tabauth", MsSqlLexerToken.Tabauth);
            this.reswords.Add("table", MsSqlLexerToken.Table);
            this.reswords.Add("tables", MsSqlLexerToken.Tables);
            this.reswords.Add("tablock", MsSqlLexerToken.Tablock);
            this.reswords.Add("tablockx", MsSqlLexerToken.Tablockx);
            this.reswords.Add("task", MsSqlLexerToken.Task);
            this.reswords.Add("terminate", MsSqlLexerToken.Terminate);
            this.reswords.Add("then", MsSqlLexerToken.Then);
            this.reswords.Add("ties", MsSqlLexerToken.Ties);
            this.reswords.Add("top", MsSqlLexerToken.To);
            this.reswords.Add("to", MsSqlLexerToken.Top);
            this.reswords.Add("transaction", MsSqlLexerToken.Transaction);
            this.reswords.Add("trigger", MsSqlLexerToken.Trigger);
            this.reswords.Add("true", MsSqlLexerToken.True);
            this.reswords.Add("type", MsSqlLexerToken.Type);
            this.reswords.Add("uid", MsSqlLexerToken.Uid);
            this.reswords.Add("union", MsSqlLexerToken.Union);
            this.reswords.Add("unique", MsSqlLexerToken.Unique);
            this.reswords.Add("unlock", MsSqlLexerToken.Unlock);
            this.reswords.Add("update", MsSqlLexerToken.Update);
            this.reswords.Add("uplock", MsSqlLexerToken.Uplock);
            this.reswords.Add("usage", MsSqlLexerToken.Usage);
            this.reswords.Add("use", MsSqlLexerToken.Use);
            this.reswords.Add("user", MsSqlLexerToken.User);
            this.reswords.Add("validate", MsSqlLexerToken.Validate);
            this.reswords.Add("values", MsSqlLexerToken.Values);
            this.reswords.Add("varchar2", MsSqlLexerToken.Varchar2);
            this.reswords.Add("variance", MsSqlLexerToken.Variance);
            this.reswords.Add("view", MsSqlLexerToken.View);
            this.reswords.Add("views", MsSqlLexerToken.Views);
            this.reswords.Add("when", MsSqlLexerToken.When);
            this.reswords.Add("whenever", MsSqlLexerToken.Whenever);
            this.reswords.Add("where", MsSqlLexerToken.Where);
            this.reswords.Add("while", MsSqlLexerToken.While);
            this.reswords.Add("with", MsSqlLexerToken.With);
            this.reswords.Add("work", MsSqlLexerToken.Work);
            this.reswords.Add("xlock", MsSqlLexerToken.Xlock);
            this.reswords.Add("xml", MsSqlLexerToken.Xml);
            this.reswords.Add("xmldata", MsSqlLexerToken.Xmldata);
            this.reswords.Add("xor", MsSqlLexerToken.Xor);
            this.reswords.Add("exec", MsSqlLexerToken.Exec);
            this.reswords.Add("execute", MsSqlLexerToken.Execute);
            this.reswords.Add("bit", MsSqlLexerToken.Bit);
            this.reswords.Add("real", MsSqlLexerToken.Real);
            this.reswords.Add("char", MsSqlLexerToken.Char);
            this.reswords.Add("text", MsSqlLexerToken.Text);
            this.reswords.Add("date", MsSqlLexerToken.Date);
            this.reswords.Add("time", MsSqlLexerToken.Time);
            this.reswords.Add("interval", MsSqlLexerToken.Interval);
            this.reswords.Add("float", MsSqlLexerToken.Float);
            this.reswords.Add("bigint", MsSqlLexerToken.Bigint);
            this.reswords.Add("double", MsSqlLexerToken.Double);
            this.reswords.Add("string", MsSqlLexerToken.String);
            this.reswords.Add("binary", MsSqlLexerToken.Binary);
            this.reswords.Add("numeric", MsSqlLexerToken.Numeric);
            this.reswords.Add("decimal", MsSqlLexerToken.Decimal);
            this.reswords.Add("boolean", MsSqlLexerToken.Boolean);
            this.reswords.Add("tinyint", MsSqlLexerToken.Tinyint);
            this.reswords.Add("integer", MsSqlLexerToken.Integer);
            this.reswords.Add("varchar", MsSqlLexerToken.Varchar);
            this.reswords.Add("smallint", MsSqlLexerToken.Smallint);
            this.reswords.Add("varbinary", MsSqlLexerToken.Varbinary);
            this.reswords.Add("timestamp", MsSqlLexerToken.Timestamp);
            this.reswords.Add("longvarchar", MsSqlLexerToken.Longvarchar);
            this.reswords.Add("java_object", MsSqlLexerToken.Java_object);
            this.reswords.Add("longvarbinary", MsSqlLexerToken.Longvarbinary);
        }

        protected override void InitStyles()
        {
            this.InitDefaultStyles();
        }

        protected virtual bool IsDatatypeToken(int token)
        {
            return ((token >= 0x11b) && (token <= 0x132));
        }

        protected virtual bool IsDateOrIntervalToken(int token)
        {
            switch (((MsSqlLexerToken) token))
            {
                case MsSqlLexerToken.Date:
                case MsSqlLexerToken.Time:
                case MsSqlLexerToken.Interval:
                case MsSqlLexerToken.Timestamp:
                    return true;
            }
            return false;
        }

        protected virtual bool IsQueryToken(int token)
        {
            MsSqlLexerToken token2 = (MsSqlLexerToken) token;
            if (token2 <= MsSqlLexerToken.Into)
            {
                switch (token2)
                {
                    case MsSqlLexerToken.From:
                    case MsSqlLexerToken.Full:
                    case MsSqlLexerToken.For:
                    case MsSqlLexerToken.Compute:
                    case MsSqlLexerToken.Inner:
                    case MsSqlLexerToken.Into:
                    case MsSqlLexerToken.Group:
                    case MsSqlLexerToken.Having:
                        goto Label_00C0;
                }
                goto Label_00C2;
            }
            if (token2 <= MsSqlLexerToken.Order)
            {
                switch (token2)
                {
                    case MsSqlLexerToken.Option:
                    case MsSqlLexerToken.Order:
                    case MsSqlLexerToken.On:
                    case MsSqlLexerToken.Join:
                    case MsSqlLexerToken.Left:
                        goto Label_00C0;
                }
                goto Label_00C2;
            }
            if (token2 <= MsSqlLexerToken.Right)
            {
                switch (token2)
                {
                    case MsSqlLexerToken.Outer:
                    case MsSqlLexerToken.Right:
                        goto Label_00C0;
                }
                goto Label_00C2;
            }
            if ((token2 != MsSqlLexerToken.Select) && (token2 != MsSqlLexerToken.Where))
            {
                goto Label_00C2;
            }
        Label_00C0:
            return true;
        Label_00C2:
            return false;
        }

        protected virtual bool IsReswordToken(int token)
        {
            return ((token >= 0) && (token <= 0x132));
        }

        protected override bool IsValidToken(int tok)
        {
            return ((tok != 0x153) && (tok != 0x152));
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
                            base.currentPos = length;
                            return 0x152;
                        }
                        base.currentPos--;
                        return this.LexNumber();

                    case '/':
                        switch (ch2)
                        {
                            case '/':
                                base.currentPos = length;
                                return 0x152;

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
                        return 0x152;
                    }
                }
                else
                {
                    base.currentPos++;
                }
            }
            this.State = 1;
            return 0x152;
        }

        protected virtual int LexDefine()
        {
            int length = base.source.Length;
            base.currentPos++;
            if (base.currentPos < base.source.Length)
            {
                char ch = base.source[base.currentPos];
                if ((((ch >= 'a') && (ch <= 'z')) || ((ch >= 'A') && (ch <= 'Z'))) || (ch == '_'))
                {
                    this.LexIdent();
                }
            }
            this.State = (base.currentPos == length) ? 0 : 2;
            return 340;
        }

        protected virtual int LexDefineEnd()
        {
            base.currentPos = base.source.Length;
            this.State = 0;
            return 0x151;
        }

        protected virtual int LexIdentifier()
        {
            char ch = base.source[base.currentPos];
            switch (ch)
            {
                case 'U':
                case 'u':
                    if (((base.currentPos < (base.source.Length - 2)) && (base.source[base.currentPos + 1] == '&')) && (base.source[base.currentPos + 2] == '\''))
                    {
                        base.currentPos += 2;
                        return this.LexString();
                    }
                    goto Label_01DC;

                case 'X':
                case 'N':
                case 'n':
                case 'x':
                    if ((base.currentPos >= (base.source.Length - 1)) || (base.source[base.currentPos + 1] != '\''))
                    {
                        goto Label_01DC;
                    }
                    base.currentPos++;
                    return this.LexString();

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
                case '[':
                {
                    int num2 = base.source.Length;
                    base.currentPos++;
                    while (base.currentPos < num2)
                    {
                        ch = base.source[base.currentPos];
                        if (ch == ']')
                        {
                            base.currentPos++;
                            break;
                        }
                        base.currentPos++;
                    }
                    return 0x151;
                }
                default:
                    goto Label_01DC;
            }
            return 0x151;
        Label_01DC:
            this.LexIdent();
            object obj2 = this.reswords[this.TokenString.ToLower()];
            if (obj2 == null)
            {
                return 0x151;
            }
            return (int) obj2;
        }

        protected virtual int LexNumber()
        {
            int num2;
            char ch = base.source[base.currentPos];
            int length = base.source.Length;
            int num3 = 0x14d;
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
                        num3 = 0x14e;
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
                        num3 = 0x14f;
                    }
                }
            }
            return num3;
        }

        protected virtual int LexString()
        {
            int length = base.source.Length;
            char ch = base.source[base.currentPos];
            char startChar = ch;
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
                    startChar = base.source[base.currentPos];
                    break;
            }
            base.currentPos++;
            return this.LexStringEnd(startChar);
        }

        protected virtual int LexStringEnd()
        {
            return this.LexStringEnd('\'');
        }

        protected virtual int LexStringEnd(char startChar)
        {
            bool flag = false;
            int length = base.source.Length;
            while (base.currentPos < length)
            {
                char ch = base.source[base.currentPos];
                if ((ch == startChar) && !flag)
                {
                    base.currentPos++;
                    if ((base.currentPos >= length) || (base.source[base.currentPos] != startChar))
                    {
                        this.State = 0;
                        return 0x150;
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
            this.State = 3;
            return 0x150;
        }

        protected virtual int LexSymbol()
        {
            MsSqlLexerToken bang = MsSqlLexerToken.Whitespace_Literal;
            char ch = base.source[base.currentPos];
            base.currentPos++;
            switch (ch)
            {
                case '!':
                    bang = MsSqlLexerToken.Bang;
                    break;

                case '#':
                    bang = MsSqlLexerToken.Turma;
                    break;

                case '$':
                    bang = MsSqlLexerToken.Dollar;
                    break;

                case '%':
                    bang = MsSqlLexerToken.Percent;
                    break;

                case '&':
                    bang = MsSqlLexerToken.Ampersant;
                    break;

                case '(':
                    bang = MsSqlLexerToken.Open_parens;
                    break;

                case ')':
                    bang = MsSqlLexerToken.Close_parens;
                    break;

                case '*':
                    bang = MsSqlLexerToken.Op_Star;
                    break;

                case '+':
                    bang = MsSqlLexerToken.Op_Plus;
                    break;

                case ',':
                    bang = MsSqlLexerToken.Comma;
                    break;

                case '-':
                    bang = MsSqlLexerToken.Op_Minus;
                    break;

                case '.':
                    bang = MsSqlLexerToken.Dot;
                    break;

                case '/':
                    bang = MsSqlLexerToken.Op_Div;
                    break;

                case ';':
                    bang = MsSqlLexerToken.Semicolon;
                    break;

                case '<':
                    switch (base.CurChar())
                    {
                        case '=':
                            base.currentPos++;
                            bang = MsSqlLexerToken.Op_le;
                            goto Label_0253;

                        case '>':
                            base.currentPos++;
                            bang = MsSqlLexerToken.Op_ne;
                            goto Label_0253;
                    }
                    bang = MsSqlLexerToken.Op_lt;
                    break;

                case '=':
                    bang = MsSqlLexerToken.Op_Assign;
                    break;

                case '>':
                    if (base.CurChar() != '=')
                    {
                        bang = MsSqlLexerToken.Op_gt;
                        break;
                    }
                    base.currentPos++;
                    bang = MsSqlLexerToken.Op_ge;
                    break;

                case '?':
                    bang = MsSqlLexerToken.Interr;
                    break;

                case '@':
                    bang = MsSqlLexerToken.Uxo;
                    break;

                case '[':
                    base.currentPos--;
                    return this.LexIdentifier();

                case '^':
                    bang = MsSqlLexerToken.Carret;
                    break;

                case '`':
                    bang = MsSqlLexerToken.Cav;
                    break;

                case '|':
                    bang = MsSqlLexerToken.Bitwise_or;
                    break;

                case '~':
                    bang = MsSqlLexerToken.Tilde;
                    break;
            }
        Label_0253:
            return (int) bang;
        }

        protected virtual int LexWhitespace()
        {
            this.LexSpace();
            return 0x153;
        }

        protected virtual int MoveNext()
        {
            this.prevPosition = this.CurrentPosition;
            int token = this.NextToken();
            while (!this.Eof && (!this.IsValidToken(token) || (this.State == 3)))
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
                case 0x14b:
                case 0x14c:
                {
                    node = this.CreateExpressionNode(this.TokenPosition, this.TokenString, 0x49, node, true);
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

        protected virtual bool ParseAlias(ref ISyntaxNode node)
        {
            bool flag = true;
            node = null;
            switch (((MsSqlLexerToken) this.Token))
            {
                case MsSqlLexerToken.As:
                case MsSqlLexerToken.Assign:
                    this.MoveNext();
                    if (!this.ParseExpression(ref node))
                    {
                        flag = false;
                    }
                    return flag;
            }
            bool flag2 = false;
            this.SaveState();
            try
            {
                if (this.ParseExpression(ref node))
                {
                    flag2 = true;
                }
            }
            finally
            {
                this.RestoreState(!flag2);
            }
            return flag;
        }

        protected virtual bool ParseAndExpression(ref ISyntaxNode node)
        {
            bool flag = this.ParseEqualityExpression(ref node);
            if (this.Token == 8)
            {
                node = this.CreateExpressionNode(this.TokenPosition, this.TokenString, 0x4b, node, true);
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
            node = new SyntaxNode(this.TokenPosition, this.TokenString, 70);
            ISyntaxNode node2 = null;
            bool flag = (this.Expected(MsSqlLexerToken.Open_parens) && this.ParseExpressionList(ref node2)) && this.Expected(MsSqlLexerToken.Close_parens);
            if (node2 != null)
            {
                node.AddChild(node2);
            }
            return flag;
        }

        protected virtual bool ParseColumnAliasList()
        {
            ISyntaxNode node = null;
            bool flag = this.ParseExpressionList(ref node);
            if (node != null)
            {
                this.AddNode(node);
            }
            return flag;
        }

        protected virtual bool ParseColumnName(out string name)
        {
            return this.ParseQualifiedIdentifier(out name);
        }

        protected virtual bool ParseComputeClause()
        {
            bool flag = true;
            switch (((MsSqlLexerToken) this.Token))
            {
                case MsSqlLexerToken.Stddev:
                case MsSqlLexerToken.Stdevp:
                case MsSqlLexerToken.Max:
                case MsSqlLexerToken.Min:
                case MsSqlLexerToken.Avg:
                case MsSqlLexerToken.Count:
                    this.AddAttribute(new SyntaxAttribute(this.TokenPosition, MsSqlNodeType.Attribute.ToString(), this.TokenString.ToUpper()));
                    this.MoveNext();
                    break;
            }
            ISyntaxNode node = null;
            if (!this.ParseExpression(ref node))
            {
                flag = false;
            }
            if (node != null)
            {
                this.AddNode(node);
            }
            return flag;
        }

        protected virtual bool ParseComputeStatement()
        {
            bool flag = true;
            this.MoveNext();
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, string.Empty, 0x30);
            this.AddNode(node);
            this.SyntaxTree.Push(node);
            try
            {
                while (!this.Eof)
                {
                    if (!this.ParseComputeClause())
                    {
                        flag = false;
                    }
                    if (this.Token != 0x145)
                    {
                        break;
                    }
                    this.MoveNext();
                }
                if (this.Token == 0x19)
                {
                    this.MoveNext();
                    ISyntaxNode node2 = null;
                    if (this.ParseExpressionList(ref node2))
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
            node.Range.EndPoint = this.prevPosition;
            return flag;
        }

        protected virtual bool ParseDirective()
        {
            bool flag = true;
            switch (this.TokenString)
            {
                case "#region":
                {
                    ISyntaxNode node = new SyntaxNode(this.TokenPosition, this.TokenString, 40);
                    if (this.State == 2)
                    {
                        this.MoveNext();
                    }
                    node.Name = this.TokenString.Trim();
                    this.AddNode(node);
                    this.SyntaxTree.Push(node);
                    this.MoveNext();
                    return flag;
                }
                case "#endregion":
                {
                    Point point2 = new Point(base.source.Length, base.lineIndex);
                    if (this.State == 2)
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
            if (this.State == 2)
            {
                this.MoveNext();
            }
            this.MoveNext();
            return flag;
        }

        protected virtual bool ParseEqualityExpression(ref ISyntaxNode node)
        {
            bool flag = this.ParseRelationalExpression(ref node);
            if (this.Token == 0x9c)
            {
                this.MoveNext();
            }
            MsSqlLexerToken token = (MsSqlLexerToken) this.Token;
            if (token <= MsSqlLexerToken.Is)
            {
                switch (token)
                {
                    case MsSqlLexerToken.Between:
                    case MsSqlLexerToken.Is:
                        goto Label_00AB;

                    case MsSqlLexerToken.In:
                        node = this.CreateExpressionNode(this.TokenPosition, this.TokenString, 0x4e, node, true);
                        this.SyntaxTree.Push(node);
                        try
                        {
                            this.MoveNext();
                            if (!this.ParseSubQuery())
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
                return flag;
            }
            if (token != MsSqlLexerToken.Like)
            {
                if ((token == MsSqlLexerToken.Op_ne) || (token == MsSqlLexerToken.Op_Assign))
                {
                    node = this.CreateExpressionNode(this.TokenPosition, this.TokenString, 80, node, true);
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
                }
                return flag;
            }
        Label_00AB:
            node = this.CreateExpressionNode(this.TokenPosition, this.TokenString, 0x4d, node, true);
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

        protected virtual bool ParseExecProc()
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, this.TokenString.ToUpper(), 8);
            this.AddNode(node);
            this.SyntaxTree.Push(node);
            try
            {
                this.MoveNext();
                ISyntaxNode node2 = null;
                if (this.ParseExpressionList(ref node2))
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
            node.Range.EndPoint = this.prevPosition;
            return flag;
        }

        protected virtual bool ParseExpression(ref ISyntaxNode node)
        {
            return this.ParseInclusiveOrExpression(ref node);
        }

        protected virtual bool ParseExpressionList(ref ISyntaxNode node)
        {
            bool flag = true;
            node = new SyntaxNode(this.TokenPosition, string.Empty, 0x48);
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
                }
                if (this.Token != 0x145)
                {
                    break;
                }
                this.MoveNext();
            }
            node.Range.EndPoint = this.prevPosition;
            return flag;
        }

        protected virtual bool ParseForStatement()
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, this.TokenString, 0x31);
            this.AddNode(node);
            this.MoveNext();
            MsSqlLexerToken token = (MsSqlLexerToken) this.Token;
            if (token == MsSqlLexerToken.Browse)
            {
                this.MoveNext();
            }
            else if (token == MsSqlLexerToken.Xml)
            {
                this.MoveNext();
                switch (((MsSqlLexerToken) this.Token))
                {
                    case MsSqlLexerToken.Auto:
                    case MsSqlLexerToken.Explicit:
                    case MsSqlLexerToken.Raw:
                        node.AddAttribute(new SyntaxAttribute(this.TokenPosition, MsSqlNodeType.Attribute.ToString(), this.TokenString));
                        this.MoveNext();
                        break;
                }
                while (this.Token == 0x145)
                {
                    switch (((MsSqlLexerToken) this.Token))
                    {
                        case MsSqlLexerToken.Elements:
                        case MsSqlLexerToken.Xmldata:
                            node.AddAttribute(new SyntaxAttribute(this.TokenPosition, MsSqlNodeType.Attribute.ToString(), this.TokenString));
                            this.MoveNext();
                            break;

                        case MsSqlLexerToken.Binary:
                            goto Label_00EB;
                    }
                    continue;
                Label_00EB:
                    if (this.Expected(20))
                    {
                        node.AddAttribute(new SyntaxAttribute(this.TokenPosition, MsSqlNodeType.Attribute.ToString(), this.TokenString));
                    }
                    else
                    {
                        this.SyntaxError();
                        flag = false;
                    }
                }
            }
            else
            {
                this.SyntaxError();
                flag = false;
            }
            node.Range.EndPoint = this.prevPosition;
            return flag;
        }

        protected virtual bool ParseFromStatement()
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, this.TokenString, 50, SyntaxNodeOptions.Indentation);
            this.AddNode(node);
            this.SyntaxTree.Push(node);
            try
            {
                this.MoveNext();
                while (!this.Eof)
                {
                    if (!this.ParseTableSource())
                    {
                        flag = false;
                    }
                    if (this.Token != 0x145)
                    {
                        goto Label_0068;
                    }
                    this.MoveNext();
                }
            }
            finally
            {
                this.SyntaxTree.Pop();
            }
        Label_0068:
            node.Range.EndPoint = this.prevPosition;
            return flag;
        }

        protected virtual bool ParseGroupbyStatement()
        {
            bool flag = true;
            this.MoveNext();
            if (this.Expected(MsSqlLexerToken.By))
            {
                ISyntaxNode node = new SyntaxNode(this.TokenPosition, this.TokenString, 0x36, SyntaxNodeOptions.Indentation);
                this.AddNode(node);
                if (this.Token == 6)
                {
                    node.AddAttribute(new SyntaxAttribute(this.TokenPosition, MsSqlNodeType.Attribute.ToString(), this.TokenString.ToUpper()));
                    this.MoveNext();
                }
                ISyntaxNode node2 = null;
                flag = this.ParseExpressionList(ref node2);
                if (node2 != null)
                {
                    node.AddChild(node2);
                }
                if (this.Token == 0x115)
                {
                    node.AddAttribute(new SyntaxAttribute(this.TokenPosition, MsSqlNodeType.Attribute.ToString(), this.TokenString.ToUpper()));
                    this.MoveNext();
                    switch (((MsSqlLexerToken) this.Token))
                    {
                        case MsSqlLexerToken.Cube:
                        case MsSqlLexerToken.Rollup:
                            node.AddAttribute(new SyntaxAttribute(this.TokenPosition, MsSqlNodeType.Attribute.ToString(), this.TokenString.ToUpper()));
                            this.MoveNext();
                            break;
                    }
                }
                node.Range.EndPoint = this.prevPosition;
                return flag;
            }
            return false;
        }

        protected virtual bool ParseHavingStatement()
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, this.TokenString, 0x33, SyntaxNodeOptions.Indentation);
            this.AddNode(node);
            this.SyntaxTree.Push(node);
            try
            {
                this.MoveNext();
                flag = this.ParseSearchConditionList();
            }
            finally
            {
                this.SyntaxTree.Pop();
            }
            node.Range.EndPoint = this.prevPosition;
            return flag;
        }

        protected virtual bool ParseIdentifier(out string identifier)
        {
            identifier = this.TokenString;
            bool flag = this.IsReswordToken(this.Token) || (this.Token == 0x151);
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
                while (this.Token == 0x145)
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
            if (this.Token == 0xa8)
            {
                node = this.CreateExpressionNode(this.TokenPosition, this.TokenString, 0x4a, node, true);
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

        protected virtual bool ParseIntoStatement()
        {
            this.MoveNext();
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, string.Empty, 0x37, SyntaxNodeOptions.Indentation);
            string name = string.Empty;
            bool flag = this.ParseTableName(out name);
            node.Name = name;
            node.Range.EndPoint = this.prevPosition;
            return flag;
        }

        protected virtual bool ParseInvocationExpression(ref ISyntaxNode node)
        {
            bool flag = true;
            node = this.CreateExpressionNode(this.TokenPosition, this.TokenString, 0x54, node, false);
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
            bool flag = true;
            node = this.CreateExpressionNode(this.TokenPosition, this.TokenString, 0x55, node, true);
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
                case 0x149:
                case 330:
                {
                    node = this.CreateExpressionNode(this.TokenPosition, this.TokenString, 0x4c, node, true);
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

        protected virtual bool ParseOptionStatement()
        {
            bool flag = true;
            this.MoveNext();
            if (!this.Expected(MsSqlLexerToken.Open_parens))
            {
                return false;
            }
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, this.TokenString, 0x34);
            this.AddNode(node);
            this.SyntaxTree.Push(node);
            try
            {
                this.MoveNext();
                while (!this.Eof)
                {
                    if (!this.ParseQueryHint())
                    {
                        flag = false;
                    }
                    if (this.Token != 0x145)
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
            node.Range.EndPoint = this.prevPosition;
            return flag;
        }

        protected virtual bool ParseOrderbyStatement()
        {
            bool flag = true;
            this.MoveNext();
            if (!this.Expected(MsSqlLexerToken.By))
            {
                return false;
            }
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, this.TokenString, 0x35, SyntaxNodeOptions.Indentation);
            this.AddNode(node);
            this.SyntaxTree.Push(node);
            try
            {
                while (!this.Eof)
                {
                    if (!this.ParseOrderColumn())
                    {
                        flag = false;
                    }
                    if (this.Token != 0x145)
                    {
                        goto Label_0072;
                    }
                    this.MoveNext();
                }
            }
            finally
            {
                this.SyntaxTree.Pop();
            }
        Label_0072:
            node.Range.EndPoint = this.prevPosition;
            return flag;
        }

        protected virtual bool ParseOrderColumn()
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, string.Empty, 0x5b);
            this.AddNode(node);
            ISyntaxNode node2 = null;
            flag = this.ParseExpression(ref node2);
            if (node2 != null)
            {
                node.AddChild(node2);
            }
            switch (((MsSqlLexerToken) this.Token))
            {
                case MsSqlLexerToken.Asc:
                case MsSqlLexerToken.Desc:
                    node.AddAttribute(new SyntaxAttribute(this.TokenPosition, MsSqlNodeType.Attribute.ToString(), this.TokenString));
                    this.MoveNext();
                    break;
            }
            node.Range.EndPoint = this.prevPosition;
            return flag;
        }

        protected virtual bool ParseParenthesizedExpression(ref ISyntaxNode node)
        {
            bool flag = true;
            if (this.Token == 0x133)
            {
                node = new SyntaxNode(this.TokenPosition, this.TokenString, 0x56);
                this.MoveNext();
                ISyntaxNode node2 = null;
                flag = this.ParseExpression(ref node2);
                if (node2 != null)
                {
                    node.AddChild(node2);
                }
                if (!this.Expected(MsSqlLexerToken.Close_parens))
                {
                    flag = false;
                }
                node.Range.EndPoint = this.prevPosition;
                return flag;
            }
            this.SyntaxError(0x133);
            return false;
        }

        protected virtual bool ParsePrefixedUnaryExpression(ref ISyntaxNode node)
        {
            bool flag = true;
            switch (((MsSqlLexerToken) this.Token))
            {
                case MsSqlLexerToken.Op_Minus:
                case MsSqlLexerToken.Op_Plus:
                case MsSqlLexerToken.Not:
                {
                    node = this.CreateExpressionNode(this.TokenPosition, this.TokenString, 0x52, node, false);
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

        protected virtual bool ParseQualifiedIdentifier(out string identifier)
        {
            bool flag = this.ParseIdentifier(out identifier);
            if (flag)
            {
                while (this.Token == 0x146)
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

        protected virtual bool ParseQueryExpression()
        {
            bool flag = true;
            if (this.Token == 0x103)
            {
                this.AddAttribute(new SyntaxAttribute(this.TokenPosition, MsSqlNodeType.Attribute.ToString(), this.TokenString));
                this.MoveNext();
                if (this.Token == 6)
                {
                    this.AddAttribute(new SyntaxAttribute(this.TokenPosition, MsSqlNodeType.Attribute.ToString(), this.TokenString));
                    this.MoveNext();
                }
            }
            if (this.Token == 0x133)
            {
                this.MoveNext();
                flag = this.ParseQueryExpression();
                if (!this.Expected(0x134))
                {
                    flag = false;
                }
                return flag;
            }
            return this.ParseQuerySpecification();
        }

        protected virtual bool ParseQueryHint()
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, this.TokenString, 0x16);
            this.AddNode(node);
            this.SyntaxTree.Push(node);
            try
            {
                switch (((MsSqlLexerToken) this.Token))
                {
                    case MsSqlLexerToken.Fast:
                    case MsSqlLexerToken.Maxdop:
                    {
                        node.AddAttribute(new SyntaxAttribute(this.TokenPosition, MsSqlNodeType.Attribute.ToString(), this.TokenString.ToUpper()));
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
                        goto Label_0522;
                    }
                    case MsSqlLexerToken.Force:
                    {
                        node.AddAttribute(new SyntaxAttribute(this.TokenPosition, MsSqlNodeType.Attribute.ToString(), this.TokenString.ToUpper()));
                        this.MoveNext();
                        Point tokenPosition = this.TokenPosition;
                        if (this.Expected(MsSqlLexerToken.Order))
                        {
                            node.AddAttribute(new SyntaxAttribute(tokenPosition, MsSqlNodeType.Attribute.ToString(), this.TokenString.ToUpper()));
                        }
                        else
                        {
                            flag = false;
                        }
                        goto Label_0522;
                    }
                    case MsSqlLexerToken.Concat:
                    {
                        node.AddAttribute(new SyntaxAttribute(this.TokenPosition, MsSqlNodeType.Attribute.ToString(), this.TokenString.ToUpper()));
                        this.MoveNext();
                        Point position = this.TokenPosition;
                        if (this.Expected(MsSqlLexerToken.Union))
                        {
                            node.AddAttribute(new SyntaxAttribute(position, MsSqlNodeType.Attribute.ToString(), this.TokenString.ToUpper()));
                        }
                        else
                        {
                            flag = false;
                        }
                        goto Label_0522;
                    }
                    case MsSqlLexerToken.Expand:
                    {
                        node.AddAttribute(new SyntaxAttribute(this.TokenPosition, MsSqlNodeType.Attribute.ToString(), this.TokenString.ToUpper()));
                        this.MoveNext();
                        Point point5 = this.TokenPosition;
                        if (this.Expected(MsSqlLexerToken.Views))
                        {
                            node.AddAttribute(new SyntaxAttribute(point5, MsSqlNodeType.Attribute.ToString(), this.TokenString.ToUpper()));
                        }
                        else
                        {
                            flag = false;
                        }
                        goto Label_0522;
                    }
                    case MsSqlLexerToken.Keep:
                    case MsSqlLexerToken.Keepfixed:
                    case MsSqlLexerToken.Robust:
                    {
                        node.AddAttribute(new SyntaxAttribute(this.TokenPosition, MsSqlNodeType.Attribute.ToString(), this.TokenString.ToUpper()));
                        this.MoveNext();
                        Point point4 = this.TokenPosition;
                        if (this.Expected(MsSqlLexerToken.Plan))
                        {
                            node.AddAttribute(new SyntaxAttribute(point4, MsSqlNodeType.Attribute.ToString(), this.TokenString.ToUpper()));
                        }
                        else
                        {
                            flag = false;
                        }
                        goto Label_0522;
                    }
                    case MsSqlLexerToken.Hash:
                        node.AddAttribute(new SyntaxAttribute(this.TokenPosition, MsSqlNodeType.Attribute.ToString(), this.TokenString.ToUpper()));
                        this.MoveNext();
                        if (((this.Token == 0x68) || (this.Token == 0x103)) || (this.Token == 0x81))
                        {
                            node.AddAttribute(new SyntaxAttribute(this.TokenPosition, MsSqlNodeType.Attribute.ToString(), this.TokenString.ToUpper()));
                            this.MoveNext();
                        }
                        else
                        {
                            this.SyntaxError(0x68);
                            flag = false;
                        }
                        goto Label_0522;

                    case MsSqlLexerToken.Loop:
                    {
                        node.AddAttribute(new SyntaxAttribute(this.TokenPosition, MsSqlNodeType.Attribute.ToString(), this.TokenString.ToUpper()));
                        this.MoveNext();
                        Point point2 = this.TokenPosition;
                        if (!this.Expected(MsSqlLexerToken.Join))
                        {
                            goto Label_035A;
                        }
                        node.AddAttribute(new SyntaxAttribute(point2, MsSqlNodeType.Attribute.ToString(), this.TokenString.ToUpper()));
                        goto Label_0522;
                    }
                    case MsSqlLexerToken.Merge:
                        node.AddAttribute(new SyntaxAttribute(this.TokenPosition, MsSqlNodeType.Attribute.ToString(), this.TokenString.ToUpper()));
                        this.MoveNext();
                        if ((this.Token != 0x103) && (this.Token != 0x81))
                        {
                            break;
                        }
                        node.AddAttribute(new SyntaxAttribute(this.TokenPosition, MsSqlNodeType.Attribute.ToString(), this.TokenString.ToUpper()));
                        this.MoveNext();
                        goto Label_0522;

                    case MsSqlLexerToken.Order:
                        node.AddAttribute(new SyntaxAttribute(this.TokenPosition, MsSqlNodeType.Attribute.ToString(), this.TokenString.ToUpper()));
                        this.MoveNext();
                        if (this.Token == 0x68)
                        {
                            node.AddAttribute(new SyntaxAttribute(this.TokenPosition, MsSqlNodeType.Attribute.ToString(), this.TokenString.ToUpper()));
                            this.MoveNext();
                        }
                        else
                        {
                            this.SyntaxError(0x68);
                            flag = false;
                        }
                        goto Label_0522;

                    default:
                        goto Label_050B;
                }
                flag = false;
                this.SyntaxError(0x103);
                goto Label_0522;
            Label_035A:
                flag = false;
                goto Label_0522;
            Label_050B:
                this.SyntaxError();
                flag = false;
            }
            finally
            {
                this.SyntaxTree.Pop();
            }
        Label_0522:
            node.Range.EndPoint = this.prevPosition;
            return flag;
        }

        protected virtual bool ParseQuerySpecification()
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, this.TokenString.ToUpper(), 7);
            this.AddNode(node);
            this.SyntaxTree.Push(node);
            try
            {
                this.MoveNext();
                node.AddAttribute(new SyntaxAttribute(this.prevPosition, SyntaxConsts.DeclarationScope, null));
                switch (((MsSqlLexerToken) this.Token))
                {
                    case MsSqlLexerToken.All:
                    case MsSqlLexerToken.Distinct:
                        node.AddAttribute(new SyntaxAttribute(this.TokenPosition, MsSqlNodeType.Attribute.ToString(), this.TokenString.ToUpper()));
                        this.MoveNext();
                        break;
                }
                if (this.Token == 0xfd)
                {
                    this.ParseTop();
                }
                if (!this.ParseSelectList())
                {
                    flag = false;
                }
                bool flag2 = false;
                while (!this.Eof && !flag2)
                {
                    MsSqlLexerToken token = (MsSqlLexerToken) this.Token;
                    if (token <= MsSqlLexerToken.Group)
                    {
                        switch (token)
                        {
                            case MsSqlLexerToken.From:
                                goto Label_00ED;

                            case MsSqlLexerToken.Group:
                                goto Label_0105;
                        }
                        goto Label_011D;
                    }
                    if (token == MsSqlLexerToken.Having)
                    {
                        goto Label_0111;
                    }
                    if (token != MsSqlLexerToken.Into)
                    {
                        if (token == MsSqlLexerToken.Where)
                        {
                            goto Label_00F9;
                        }
                        goto Label_011D;
                    }
                    if (!this.ParseIntoStatement())
                    {
                        flag = false;
                    }
                    continue;
                Label_00ED:
                    if (!this.ParseFromStatement())
                    {
                        flag = false;
                    }
                    continue;
                Label_00F9:
                    if (!this.ParseWhereStatement())
                    {
                        flag = false;
                    }
                    continue;
                Label_0105:
                    if (!this.ParseGroupbyStatement())
                    {
                        flag = false;
                    }
                    continue;
                Label_0111:
                    if (!this.ParseHavingStatement())
                    {
                        flag = false;
                    }
                    continue;
                Label_011D:
                    flag2 = true;
                }
                while (!this.Eof && !flag2)
                {
                }
                if (this.Token == 0x147)
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

        protected virtual bool ParseRelationalExpression(ref ISyntaxNode node)
        {
            bool flag = this.ParseAdditiveExpression(ref node);
            switch (this.Token)
            {
                case 310:
                case 0x137:
                case 0x138:
                case 0x139:
                {
                    node = this.CreateExpressionNode(this.TokenPosition, this.TokenString, 0x4f, node, true);
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

        protected virtual bool ParseSearchCondition()
        {
            ISyntaxNode node = null;
            bool flag = this.ParseExpression(ref node);
            if (node != null)
            {
                this.AddNode(node);
            }
            return flag;
        }

        protected virtual bool ParseSearchConditionList()
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, string.Empty, 0x45);
            this.AddNode(node);
            this.SyntaxTree.Push(node);
            try
            {
                while (!this.Eof)
                {
                    if (!this.ParseSearchCondition())
                    {
                        flag = false;
                    }
                    if (this.Token != 0x145)
                    {
                        goto Label_005F;
                    }
                    this.MoveNext();
                }
            }
            finally
            {
                this.SyntaxTree.Pop();
            }
        Label_005F:
            node.Range.EndPoint = this.prevPosition;
            return flag;
        }

        protected virtual bool ParseSelectItem()
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, string.Empty, 0x19);
            this.AddNode(node);
            this.SyntaxTree.Push(node);
            try
            {
                node.AddAttribute(new SyntaxAttribute(this.TokenPosition, SyntaxConsts.DeclarationScope, null));
                string columnName = string.Empty;
                switch (((MsSqlLexerToken) this.Token))
                {
                    case MsSqlLexerToken.IdentityCol:
                    case MsSqlLexerToken.RowGuidCol:
                        columnName = this.TokenString.ToUpper();
                        this.MoveNext();
                        break;

                    default:
                        this.SaveState();
                        try
                        {
                            if (!this.TryParseColumnName(out columnName))
                            {
                                flag = false;
                            }
                        }
                        finally
                        {
                            this.RestoreState(columnName == string.Empty);
                        }
                        break;
                }
                ISyntaxNode node2 = null;
                if (columnName != string.Empty)
                {
                    node.Name = columnName;
                }
                else
                {
                    if (!this.ParseExpression(ref node2))
                    {
                        flag = false;
                    }
                    if (node2 != null)
                    {
                        node.AddChild(node2);
                    }
                }
                node2 = null;
                if (!this.ParseAlias(ref node2))
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
            node.Range.EndPoint = this.prevPosition;
            return flag;
        }

        protected virtual bool ParseSelectList()
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, this.TokenString.ToUpper(), 0x42);
            this.AddNode(node);
            this.SyntaxTree.Push(node);
            try
            {
                node.AddAttribute(new SyntaxAttribute(this.TokenPosition, SyntaxConsts.DeclarationScope, null));
                while (!this.Eof)
                {
                    if (!this.ParseSelectItem())
                    {
                        flag = false;
                    }
                    if (this.Token != 0x145)
                    {
                        goto Label_007D;
                    }
                    this.MoveNext();
                }
            }
            finally
            {
                this.SyntaxTree.Pop();
            }
        Label_007D:
            node.Range.EndPoint = this.prevPosition;
            return flag;
        }

        protected virtual bool ParseSelectQuery()
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, this.TokenString.ToUpper(), 5, SyntaxNodeOptions.Outlining | SyntaxNodeOptions.Indentation);
            node.AddAttribute(new SyntaxAttribute(node.Position, SyntaxConsts.OutlineText, node.Name));
            this.AddNode(node);
            this.SyntaxTree.Push(node);
            try
            {
                MsSqlLexerToken token;
                node.AddAttribute(new SyntaxAttribute(this.TokenPosition, SyntaxConsts.DeclarationScope, null));
                if (!this.ParseQueryExpression())
                {
                    flag = false;
                }
                bool flag2 = false;
            Label_0070:
                token = (MsSqlLexerToken) this.Token;
                if (token != MsSqlLexerToken.Compute)
                {
                    switch (token)
                    {
                        case MsSqlLexerToken.Option:
                            if (!this.ParseOptionStatement())
                            {
                                flag = false;
                            }
                            goto Label_00CD;

                        case MsSqlLexerToken.Order:
                            if (!this.ParseOrderbyStatement())
                            {
                                flag = false;
                            }
                            goto Label_00CD;

                        case MsSqlLexerToken.For:
                            goto Label_00B3;
                    }
                    goto Label_00CB;
                }
                if (!this.ParseComputeStatement())
                {
                    flag = false;
                }
                goto Label_00CD;
            Label_00B3:
                if (!this.ParseForStatement())
                {
                    flag = false;
                }
                goto Label_00CD;
            Label_00CB:
                flag2 = true;
            Label_00CD:
                if (!this.Eof && !flag2)
                {
                    goto Label_0070;
                }
            }
            finally
            {
                this.SyntaxTree.Pop();
            }
            node.Range.EndPoint = this.prevPosition;
            return flag;
        }

        protected virtual bool ParseSimpleExpression(ref ISyntaxNode node)
        {
            bool flag = true;
            switch (((MsSqlLexerToken) this.Token))
            {
                case MsSqlLexerToken.All:
                case MsSqlLexerToken.Distinct:
                {
                    ISyntaxAttribute attr = new SyntaxAttribute(this.TokenPosition, MsSqlNodeType.Attribute.ToString(), this.TokenString.ToUpper());
                    this.MoveNext();
                    flag = this.ParseSimpleExpression(ref node);
                    if (node != null)
                    {
                        node.AddAttribute(attr);
                    }
                    return flag;
                }
                case MsSqlLexerToken.False:
                case MsSqlLexerToken.Op_Star:
                case MsSqlLexerToken.Integer_Literal:
                case MsSqlLexerToken.Float_Literal:
                case MsSqlLexerToken.Double_Literal:
                case MsSqlLexerToken.String_Literal:
                case MsSqlLexerToken.Identifier_Literal:
                case MsSqlLexerToken.True:
                    node = new SyntaxNode(this.TokenPosition, this.TokenString, 0x51);
                    this.MoveNext();
                    return flag;

                case MsSqlLexerToken.Open_parens:
                    return this.ParseParenthesizedExpression(ref node);
            }
            if (this.IsReswordToken(this.Token) && !this.IsQueryToken(this.Token))
            {
                node = new SyntaxNode(this.TokenPosition, this.TokenString, 0x51);
                this.MoveNext();
                return flag;
            }
            flag = false;
            this.SyntaxError();
            this.MoveNext();
            return flag;
        }

        protected virtual bool ParseSubQuery()
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, this.TokenString, 6, SyntaxNodeOptions.Indentation);
            this.AddNode(node);
            this.SyntaxTree.Push(node);
            try
            {
                if (this.Expected(MsSqlLexerToken.Open_parens))
                {
                    if ((this.Token == 340) && !this.ParseDirective())
                    {
                        flag = false;
                    }
                    if (this.Token == 0xdb)
                    {
                        if (!this.ParseSelectQuery())
                        {
                            flag = false;
                        }
                    }
                    else
                    {
                        ISyntaxNode node2 = null;
                        if (!this.ParseExpressionList(ref node2))
                        {
                            flag = false;
                        }
                        if (node2 != null)
                        {
                            node.AddChild(node2);
                        }
                    }
                    if ((this.Token == 340) && !this.ParseDirective())
                    {
                        flag = false;
                    }
                    if (!this.Expected(MsSqlLexerToken.Close_parens))
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

        protected virtual bool ParseTableAlias(out string name)
        {
            return this.ParseQualifiedIdentifier(out name);
        }

        protected virtual bool ParseTableHint()
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, string.Empty, 0x17);
            this.AddNode(node);
            if (!this.Expected(MsSqlLexerToken.Open_parens))
            {
                flag = false;
            }
            else
            {
                while (!this.Eof)
                {
                    switch (((MsSqlLexerToken) this.Token))
                    {
                        case MsSqlLexerToken.Fastfirstrow:
                        case MsSqlLexerToken.Holdlock:
                        case MsSqlLexerToken.Readcommited:
                        case MsSqlLexerToken.Readpast:
                        case MsSqlLexerToken.Readuncommited:
                        case MsSqlLexerToken.Paglock:
                        case MsSqlLexerToken.Nolock:
                        case MsSqlLexerToken.Repeatableread:
                        case MsSqlLexerToken.Rowlock:
                        case MsSqlLexerToken.Serializable:
                        case MsSqlLexerToken.Tablock:
                        case MsSqlLexerToken.Tablockx:
                        case MsSqlLexerToken.Uplock:
                        case MsSqlLexerToken.Xlock:
                            break;

                        case MsSqlLexerToken.Index:
                            this.MoveNext();
                            this.SyntaxTree.Push(node);
                            try
                            {
                                if (!this.ParseTableHintIndex())
                                {
                                    flag = false;
                                }
                                goto Label_0134;
                            }
                            finally
                            {
                                this.SyntaxTree.Pop();
                            }
                            break;

                        default:
                            goto Label_0134;
                    }
                    node.AddAttribute(new SyntaxAttribute(this.TokenPosition, MsSqlNodeType.Attribute.ToString(), this.TokenString));
                    this.MoveNext();
                Label_0134:
                    if (this.Token != 0x145)
                    {
                        break;
                    }
                    this.MoveNext();
                }
                if (!this.Expected(MsSqlLexerToken.Close_parens))
                {
                    flag = false;
                }
            }
            node.Range.EndPoint = this.prevPosition;
            return flag;
        }

        protected virtual bool ParseTableHintIndex()
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, string.Empty, 0x18);
            this.AddNode(node);
            if (!this.Expected(MsSqlLexerToken.Open_parens))
            {
                flag = false;
            }
            else
            {
                string identifier = string.Empty;
                while (!this.Eof)
                {
                    if (this.ParseIdentifier(out identifier))
                    {
                        node.AddAttribute(new SyntaxAttribute(this.prevPosition, MsSqlNodeType.Attribute.ToString(), identifier));
                    }
                    else
                    {
                        flag = false;
                        break;
                    }
                    if (this.Token != 0x145)
                    {
                        break;
                    }
                    this.MoveNext();
                }
                if (!this.Expected(MsSqlLexerToken.Close_parens))
                {
                    flag = false;
                }
            }
            node.Range.EndPoint = this.prevPosition;
            return flag;
        }

        protected virtual bool ParseTableName(out string name)
        {
            return this.ParseQualifiedIdentifier(out name);
        }

        protected virtual bool ParseTableSource()
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, string.Empty, 0x3f);
            this.AddNode(node);
            this.SyntaxTree.Push(node);
            try
            {
                while (!this.Eof)
                {
                    if (!this.ParseTableSourceAlias())
                    {
                        flag = false;
                    }
                    if (this.Token != 0x145)
                    {
                        goto Label_005F;
                    }
                    this.MoveNext();
                }
            }
            finally
            {
                this.SyntaxTree.Pop();
            }
        Label_005F:
            node.Range.EndPoint = this.prevPosition;
            return flag;
        }

        protected virtual bool ParseTableSourceAlias()
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, string.Empty, 0x57);
            this.AddNode(node);
            this.SyntaxTree.Push(node);
            try
            {
                if (this.Token == 0x133)
                {
                    node.Name = this.TokenString;
                    if (!this.ParseSubQuery())
                    {
                        flag = false;
                    }
                }
                else
                {
                    string str;
                    if (this.ParseTableName(out str))
                    {
                        node.Name = str;
                    }
                    else
                    {
                        flag = false;
                    }
                }
                ISyntaxNode node2 = null;
                if (this.Token == 0x133)
                {
                    if (!this.ParseInvocationExpression(ref node2))
                    {
                        flag = false;
                    }
                    if (node2 != null)
                    {
                        node.AddChild(node2);
                    }
                }
                node2 = null;
                Point tokenPosition = this.TokenPosition;
                if (!this.ParseAlias(ref node2))
                {
                    flag = false;
                }
                if (node2 != null)
                {
                    ISyntaxNode node3 = new SyntaxNode(tokenPosition, string.Empty, 0x58);
                    node.AddChild(node3);
                    node3.AddChild(node2);
                }
                if (this.Token == 0x115)
                {
                    this.MoveNext();
                    if (!this.ParseTableHint())
                    {
                        flag = false;
                    }
                }
                if (this.Token == 0x133)
                {
                    this.MoveNext();
                    if (!this.ParseColumnAliasList())
                    {
                        flag = false;
                    }
                    if (!this.Expected(MsSqlLexerToken.Close_parens))
                    {
                        flag = false;
                    }
                }
                bool flag2 = false;
                while (!this.Eof && !flag2)
                {
                    switch (((MsSqlLexerToken) this.Token))
                    {
                        case MsSqlLexerToken.Left:
                        case MsSqlLexerToken.Right:
                        case MsSqlLexerToken.Full:
                        {
                            node.AddAttribute(new SyntaxAttribute(this.TokenPosition, MsSqlNodeType.Attribute.ToString(), this.TokenString));
                            this.MoveNext();
                            if (this.Token == 0xac)
                            {
                                node.AddAttribute(new SyntaxAttribute(this.TokenPosition, MsSqlNodeType.Attribute.ToString(), this.TokenString));
                                this.MoveNext();
                            }
                            if (this.Expected(MsSqlLexerToken.Join))
                            {
                                if (!this.ParseTableSource())
                                {
                                    flag = false;
                                }
                                if (!this.Expected(MsSqlLexerToken.On) || !this.ParseSearchCondition())
                                {
                                    flag = false;
                                }
                            }
                            else
                            {
                                flag = false;
                            }
                            continue;
                        }
                        case MsSqlLexerToken.Outer:
                        case MsSqlLexerToken.Inner:
                        {
                            node.AddAttribute(new SyntaxAttribute(this.TokenPosition, MsSqlNodeType.Attribute.ToString(), this.TokenString));
                            this.MoveNext();
                            if (this.Expected(MsSqlLexerToken.Join))
                            {
                                if (!this.ParseTableSource())
                                {
                                    flag = false;
                                }
                                if (!this.Expected(MsSqlLexerToken.On) || !this.ParseSearchCondition())
                                {
                                    flag = false;
                                }
                            }
                            else
                            {
                                flag = false;
                            }
                            continue;
                        }
                        case MsSqlLexerToken.Cross:
                        {
                            node.AddAttribute(new SyntaxAttribute(this.TokenPosition, MsSqlNodeType.Attribute.ToString(), this.TokenString));
                            this.MoveNext();
                            if (this.Expected(MsSqlLexerToken.Join))
                            {
                                if (!this.ParseTableSource())
                                {
                                    flag = false;
                                }
                            }
                            else
                            {
                                flag = false;
                            }
                            continue;
                        }
                    }
                    flag2 = true;
                }
            }
            finally
            {
                this.SyntaxTree.Pop();
            }
            node.Range.EndPoint = this.prevPosition;
            return flag;
        }

        protected virtual bool ParseTop()
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, string.Empty, 14);
            this.AddNode(node);
            ISyntaxNode node2 = null;
            flag = this.ParseExpression(ref node2);
            if (node2 != null)
            {
                node.AddChild(node2);
            }
            if (this.Token == 0xb2)
            {
                node.AddAttribute(new SyntaxAttribute(this.TokenPosition, MsSqlNodeType.Attribute.ToString(), this.TokenString));
                this.MoveNext();
            }
            if (this.Token == 0x115)
            {
                node.AddAttribute(new SyntaxAttribute(this.TokenPosition, MsSqlNodeType.Attribute.ToString(), this.TokenString));
                this.MoveNext();
                if (this.Expected(0xfb))
                {
                    node.AddAttribute(new SyntaxAttribute(this.prevPosition, MsSqlNodeType.Attribute.ToString(), MsSqlLexerToken.Ties.ToString()));
                }
                else
                {
                    flag = false;
                }
            }
            node.Range.EndPoint = this.prevPosition;
            return flag;
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
                if (flag2 && (this.Token == 0x150))
                {
                    this.MoveNext();
                }
                if (this.Token == 0x133)
                {
                    this.MoveNext();
                    ISyntaxNode node = null;
                    flag = this.ParseExpressionList(ref node) && this.Expected(MsSqlLexerToken.Close_parens);
                }
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
            return flag;
        }

        protected virtual bool ParseUnitBody()
        {
            bool flag = true;
            ISyntaxNode current = this.SyntaxTree.Current;
            ISyntaxNode root = this.SyntaxTree.Root;
            while (!this.Eof)
            {
                switch (((MsSqlLexerToken) this.Token))
                {
                    case MsSqlLexerToken.Open_parens:
                    case MsSqlLexerToken.Select:
                        flag = this.ParseSelectQuery();
                        break;

                    case MsSqlLexerToken.Directive_Literal:
                        if (!this.ParseDirective())
                        {
                            flag = false;
                        }
                        break;

                    case MsSqlLexerToken.Exec:
                    case MsSqlLexerToken.Execute:
                        flag = this.ParseExecProc();
                        break;

                    default:
                        flag = false;
                        this.SyntaxError();
                        this.MoveNext();
                        break;
                }
                if (this.Token == 0x147)
                {
                    this.MoveNext();
                }
            }
            return flag;
        }

        protected virtual bool ParseWhereStatement()
        {
            bool flag = true;
            ISyntaxNode node = new SyntaxNode(this.TokenPosition, this.TokenString, 0x2e, SyntaxNodeOptions.Indentation);
            this.AddNode(node);
            this.SyntaxTree.Push(node);
            try
            {
                this.MoveNext();
                string name = string.Empty;
                MsSqlLexerToken token = (MsSqlLexerToken) this.Token;
                if (token != MsSqlLexerToken.Assign)
                {
                    if (token == MsSqlLexerToken.Op_Star)
                    {
                        goto Label_0098;
                    }
                    goto Label_00DC;
                }
                this.MoveNext();
                if (this.Expected(MsSqlLexerToken.Start))
                {
                    Point tokenPosition = this.TokenPosition;
                    if (this.ParseColumnName(out name))
                    {
                        node.AddAttribute(new SyntaxAttribute(tokenPosition, MsSqlNodeType.OldOuterJoinAttribute.ToString(), name));
                    }
                    else
                    {
                        flag = false;
                    }
                }
                goto Label_00F5;
            Label_0098:
                this.MoveNext();
                if (this.Expected(MsSqlLexerToken.Assign))
                {
                    Point position = this.TokenPosition;
                    if (this.ParseColumnName(out name))
                    {
                        node.AddAttribute(new SyntaxAttribute(position, MsSqlNodeType.OldOuterJoinAttribute.ToString(), name));
                    }
                    else
                    {
                        flag = false;
                    }
                }
                goto Label_00F5;
            Label_00DC:
                if (!this.ParseSearchConditionList())
                {
                    flag = false;
                }
            }
            finally
            {
                this.SyntaxTree.Pop();
            }
        Label_00F5:
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

        public override void ResetCodeCompletionChars()
        {
            this.CodeCompletionChars = new char[] { '.' };
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
                ISyntaxError err = new QWhale.Syntax.SyntaxError(this.TokenPosition, this.TokenString, ((MsSqlLexerToken) token).ToString() + " " + StringConsts.ErrExpected);
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

        protected bool TryParseColumnName(out string columnName)
        {
            if ((this.ParseTableName(out columnName) && this.Expected(MsSqlLexerToken.Dot)) && this.Expected(MsSqlLexerToken.Op_Star))
            {
                columnName = columnName + ".*";
            }
            else
            {
                columnName = string.Empty;
            }
            return true;
        }

        protected virtual bool TryParsePostPrimaryExpression(ref ISyntaxNode node)
        {
            bool flag = true;
            while (!this.Eof)
            {
                MsSqlLexerToken token = (MsSqlLexerToken) this.Token;
                if (token != MsSqlLexerToken.Open_parens)
                {
                    if ((token != MsSqlLexerToken.Bang) && (token != MsSqlLexerToken.Dot))
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

