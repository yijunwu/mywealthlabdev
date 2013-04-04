namespace QWhale.Syntax
{
    using QWhale.Common;
    using QWhale.Syntax.CodeCompletion;
    using QWhale.Syntax.Lexer;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Design;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text;

    [ToolboxItem(false)]
    public class SyntaxParser : Parser, ISyntaxParser, IParser, ILexer, INotify, IUpdate, IImport
    {
        private char[] autoIndentChars;
        private char[] codeCompletionChars;
        private char[] codeCompletionStopChars;
        private LexerState lexerState;
        private Hashtable lexerStates = new Hashtable();
        protected IComparer<ISyntaxNode> lineNodeComparer = new LineNodeComparer();
        protected IComparer<ICodeCompletionProviderItem> listMemberComparer;
        protected IComparer<ISyntaxNode> nodeComparer;
        private SyntaxOptions options;
        protected IComparer<ISyntaxNode> pointNodeComparer = new PointNodeComparer();
        private ICodeCompletionRepository repository;
        private char[] smartFormatChars;
        private ISyntaxTree syntaxTree = new QWhale.Syntax.SyntaxTree();
        private bool useScheme;

        [Description("Occurs when \"SyntaxParser\" text content is fully parsed.")]
        public event EventHandler TextReparsed;

        public SyntaxParser()
        {
            this.listMemberComparer = new ListMemberComparer(this);
            this.nodeComparer = new NodeComparer();
            this.ResetOptions();
            this.ResetCodeCompletionChars();
            this.ResetCodeCompletionStopChars();
            this.ResetAutoIndentChars();
            this.ResetSmartFormatChars();
            this.InitLexer();
        }

        protected void AddStyle(string name, Color color)
        {
            this.AddStyle(name, color, Color.Empty, FontStyle.Regular, false);
        }

        protected void AddStyle(string name, Color color, FontStyle fontStyle)
        {
            this.AddStyle(name, color, Color.Empty, fontStyle, false);
        }

        protected void AddStyle(string name, Color color, FontStyle fontStyle, bool plainText)
        {
            this.AddStyle(name, color, Color.Empty, fontStyle, plainText);
        }

        protected void AddStyle(string name, Color color, Color backColor, FontStyle fontStyle, bool plainText)
        {
            ILexStyle style = this.Scheme.Styles.AddLexStyle();
            style.Name = name;
            style.FontStyle = fontStyle;
            if (color != Color.Empty)
            {
                style.ForeColor = color;
            }
            if (backColor != Color.Empty)
            {
                style.BackColor = backColor;
            }
            style.PlainText = plainText;
        }

        protected virtual void AfterLoad()
        {
            this.ReparseText();
        }

        protected virtual void BeforeLoad()
        {
            if (this.Strings == null)
            {
                this.Strings = new StringList();
            }
        }

        public virtual void CodeCompletion(string text, short[] textData, Point position, CodeCompletionArgs e)
        {
            Point point;
            ISyntaxNode nodeForCodeCompletion;
            CodeCompletionType completionType = this.GetCompletionType(e);
            switch (completionType)
            {
                case CodeCompletionType.CompleteWord:
                case CodeCompletionType.ListMembers:
                case CodeCompletionType.ParameterInfo:
                case CodeCompletionType.SpecialListMembers:
                    point = position;
                    if (!e.NeedReparse)
                    {
                        nodeForCodeCompletion = this.GetNodeForCodeCompletion(position);
                        break;
                    }
                    this.ReparseBlock(position, text, out nodeForCodeCompletion, completionType);
                    break;

                case CodeCompletionType.QuickInfo:
                    ISyntaxNode node2;
                    if (!e.NeedReparse)
                    {
                        node2 = this.GetNodeForCodeCompletion(position);
                    }
                    else
                    {
                        this.ReparseBlock(position, text, out node2, completionType);
                    }
                    if (node2 != null)
                    {
                        text = this.RemovePlainText(text, textData);
                        object member = this.CompletionRepository.FindDeclaration(text, node2, position);
                        if (member != null)
                        {
                            string str2 = this.CompletionRepository.GetDescription(this.CreateListMembers(), (member is ISyntaxNode) ? node2 : null, member, SyntaxConsts.SummaryDescription, true);
                            if ((str2 != null) && (str2 != string.Empty))
                            {
                                e.Provider = new QuickInfo();
                                e.ToolTip = true;
                                ((IQuickInfo) e.Provider).Text = str2;
                            }
                        }
                    }
                    goto Label_02C5;

                case CodeCompletionType.CodeSnippets:
                    e.Provider = this.CodeSnippets;
                    goto Label_02C5;

                case CodeCompletionType.None:
                    goto Label_02CD;

                default:
                    goto Label_02C5;
            }
            if (nodeForCodeCompletion != null)
            {
                text = this.RemovePlainText(text, textData);
                IListMembers members = null;
                CodeCompletionScope none = CodeCompletionScope.None;
                Point endPos = new Point(-1, -1);
                string name = string.Empty;
                if (completionType == CodeCompletionType.ParameterInfo)
                {
                    int num;
                    int num2;
                    object obj2 = this.CompletionRepository.GetMethodType(text, nodeForCodeCompletion, ref name, ref position, ref endPos, out num, out num2, out none);
                    if (obj2 != null)
                    {
                        members = this.CreateParameterInfo();
                        members.ShowParams = true;
                        members.ShowQualifiers = false;
                        this.CompletionRepository.FillMember(members, obj2, name, num, none);
                    }
                    e.ToolTip = true;
                }
                else
                {
                    object obj3 = (completionType == CodeCompletionType.SpecialListMembers) ? this.CompletionRepository.GetSpecialMemberType(text, nodeForCodeCompletion, ref name, ref position, ref endPos, out none) : this.CompletionRepository.GetMemberType(text, nodeForCodeCompletion, ref name, ref position, ref endPos, out none);
                    bool flag = false;
                    if (obj3 != null)
                    {
                        flag = (none & CodeCompletionScope.Delegate) != CodeCompletionScope.None;
                        if (flag)
                        {
                            members = this.CreateParameterInfo();
                            members.ShowParams = false;
                            members.ShowQualifiers = false;
                        }
                        else
                        {
                            members = this.CreateListMembers();
                            members.ShowDescriptions = true;
                            members.ShowResults = false;
                            members.ShowQualifiers = false;
                        }
                        int selIndex = -1;
                        this.CompletionRepository.FillMembers(nodeForCodeCompletion, position, members, obj3, name, none, ref selIndex);
                        this.SortMembers(members);
                        if (!flag && (members.Count > 0))
                        {
                            int idx = 0;
                            if ((((selIndex >= 0) || this.FindMember(name, members, out idx)) && (completionType == CodeCompletionType.CompleteWord)) && ((selIndex >= 0) || !position.Equals(point)))
                            {
                                e.SelIndex = (selIndex >= 0) ? selIndex : idx;
                            }
                            members.SelIndex = (selIndex >= 0) ? selIndex : idx;
                        }
                    }
                    e.ToolTip = flag;
                }
                e.StartPosition = position;
                e.DisplayPosition = position;
                e.EndPosition = endPos;
                e.Provider = members;
            }
        Label_02C5:
            e.CompletionType = completionType;
        Label_02CD:
            e.Interval = SyntaxConsts.DefaultCompletionDelay;
            e.NeedShow = (e.Provider != null) && (e.Provider.Count > 0);
        }

        protected virtual IListMembers CreateListMembers()
        {
            return new ListMembers();
        }

        protected virtual IParameterInfo CreateParameterInfo()
        {
            return new ParameterInfo();
        }

        public virtual ICodeCompletionRepository CreateRepository()
        {
            return new CodeCompletionRepository(this.CaseSensitive, this.SyntaxTree);
        }

        protected char CurChar()
        {
            if (base.currentPos >= base.source.Length)
            {
                return '\0';
            }
            return base.source[base.currentPos];
        }

        public virtual object FindDeclaration(string text, Point position)
        {
            ISyntaxNode nodeAt = this.GetNodeAt(position);
            if (nodeAt == null)
            {
                return null;
            }
            return this.CompletionRepository.FindDeclaration(text, nodeAt, position);
        }

        protected virtual bool FindMember(string name, IListMembers members, out int idx)
        {
            idx = -1;
            if (name == string.Empty)
            {
                return false;
            }
            ISortList<MemberItem> list = new SortList<MemberItem>();
            for (int i = 0; i < members.Count; i++)
            {
                list.Add(new MemberItem(members[i].Name, i));
            }
            MemberItemComparer comparer = new MemberItemComparer();
            list.Sort(comparer);
            if (!comparer.FindPartial(list, name, out idx))
            {
                while (name != string.Empty)
                {
                    name = name.Substring(0, name.Length - 1);
                    if ((name != string.Empty) && comparer.FindPartial(list, name, out idx))
                    {
                        idx = list[idx].Index;
                        return false;
                    }
                }
                idx = -1;
                return false;
            }
            bool flag = (idx == (list.Count - 1)) || (comparer.PartialCompare(name, list[idx + 1]) != 0);
            idx = list[idx].Index;
            return flag;
        }

        public virtual int FindReferences(ISyntaxNode node, ISyntaxNodes references)
        {
            return this.CompletionRepository.FindReferences(node, references);
        }

        public virtual ISyntaxNode GetAutoFormatNode(Point position, bool extended, out Point startPt)
        {
            startPt = Point.Empty;
            return null;
        }

        public virtual CodeCompletionType GetCompletionType(CodeCompletionArgs e)
        {
            if (e.CompletionType != CodeCompletionType.None)
            {
                return e.CompletionType;
            }
            return this.GetCompletionType(e.KeyChar);
        }

        public virtual CodeCompletionType GetCompletionType(char ch)
        {
            return CodeCompletionType.None;
        }

        private LexerState GetLexerState(int state)
        {
            LexerState state2 = (LexerState) this.lexerStates[state];
            if (state2 == null)
            {
                state2 = new LexerState();
                this.lexerStates[state] = state2;
            }
            return state2;
        }

        protected virtual int GetLexerStyle(int token)
        {
            return token;
        }

        public virtual ISyntaxNode GetNodeAt(Point position)
        {
            return this.SyntaxTree.FindNode(new SyntaxNode(position, string.Empty), this.pointNodeComparer);
        }

        protected virtual ISyntaxNode GetNodeForCodeCompletion(Point position)
        {
            return this.GetNodeAt(position);
        }

        public virtual string GetSingleLineComment()
        {
            return string.Empty;
        }

        public virtual int GetSmartIndent(int index, bool autoIndent)
        {
            int indent = -1;
            ISyntaxNode root = this.syntaxTree.Root;
            ISyntaxNode node2 = null;
            ISyntaxNode node3 = new SyntaxNode(new Point(0, index), string.Empty);
            while (root != null)
            {
                if ((((root.Options & SyntaxNodeOptions.Indentation) != SyntaxNodeOptions.None) || ((root.Options & SyntaxNodeOptions.BackIndentation) != SyntaxNodeOptions.None)) || ((root.Options & SyntaxNodeOptions.KeepIndentation) != SyntaxNodeOptions.None))
                {
                    if (((root.Options & SyntaxNodeOptions.BackIndentation) == SyntaxNodeOptions.None) && ((root.Options & SyntaxNodeOptions.KeepIndentation) == SyntaxNodeOptions.None))
                    {
                        indent = (indent < 0) ? 1 : (indent + 1);
                    }
                    node2 = root;
                }
                if ((root.Position.Y == index) && (root != this.syntaxTree.Root))
                {
                    break;
                }
                root = root.FindNode(node3, this.lineNodeComparer);
            }
            if ((!autoIndent && (indent >= 0)) && (node2 != null))
            {
                indent = node2.GetIndent(index, indent);
            }
            return indent;
        }

        protected virtual bool GetSmartIndent(ISyntaxNode node, int index, ref int indent)
        {
            bool flag = (((node.Options & SyntaxNodeOptions.Indentation) != SyntaxNodeOptions.None) || ((node.Options & SyntaxNodeOptions.BackIndentation) != SyntaxNodeOptions.None)) || ((node.Options & SyntaxNodeOptions.KeepIndentation) != SyntaxNodeOptions.None);
            if ((flag && ((node.Options & SyntaxNodeOptions.BackIndentation) == SyntaxNodeOptions.None)) && ((node.Options & SyntaxNodeOptions.KeepIndentation) == SyntaxNodeOptions.None))
            {
                indent = (indent < 0) ? 1 : (indent + 1);
            }
            return flag;
        }

        protected virtual int GetSmartIndent(ISyntaxNodes nodes, int index, bool autoIndent)
        {
            int indent = -1;
            ISyntaxNode node = null;
            if (nodes != null)
            {
                foreach (ISyntaxNode node2 in nodes)
                {
                    if (this.GetSmartIndent(node2, index, ref indent))
                    {
                        node = node2;
                    }
                    if ((node2.Position.Y == index) && (node2 != this.syntaxTree.Root))
                    {
                        break;
                    }
                }
            }
            else
            {
                ISyntaxNode root = this.syntaxTree.Root;
                ISyntaxNode node4 = new SyntaxNode(new Point(0, index), string.Empty);
                while (root != null)
                {
                    if (this.GetSmartIndent(root, index, ref indent))
                    {
                        node = root;
                    }
                    if ((root.Position.Y == index) && (root != this.syntaxTree.Root))
                    {
                        break;
                    }
                    root = root.FindNode(node4, this.lineNodeComparer);
                }
            }
            if ((!autoIndent && (indent >= 0)) && (node != null))
            {
                indent = node.GetIndent(index, indent);
            }
            return indent;
        }

        public virtual int GetSyntaxErrors(IList<ISyntaxError> errors)
        {
            this.GetSyntaxErrors(errors, this.syntaxTree.Root);
            return errors.Count;
        }

        protected virtual void GetSyntaxErrors(IList<ISyntaxError> errors, ISyntaxNode node)
        {
            if (node.HasErrors)
            {
                foreach (ISyntaxError error in node.ErrorList)
                {
                    errors.Add(error);
                }
            }
            if (node.HasChildren)
            {
                foreach (ISyntaxNode node2 in node.ChildList)
                {
                    this.GetSyntaxErrors(errors, node2);
                }
            }
        }

        protected virtual void InitCopyright()
        {
            this.Scheme.Author = "Quantum Whale, LLC.";
            this.Scheme.Copyright = "Copyright (c) 2004 - 2008 Quantum Whale LLC.";
        }

        protected virtual void InitDefaultStyles()
        {
            this.AddStyle(StringConsts.IdentsInternalName, SyntaxConsts.DefaultIndentsForeColor);
            this.AddStyle(StringConsts.NumbersInternalName, SyntaxConsts.DefaultNumbersForeColor);
            this.AddStyle(StringConsts.ReswordsInternalName, SyntaxConsts.DefaultReswordForeColor);
            this.AddStyle(StringConsts.CommentsInternalName, SyntaxConsts.DefaultCommentsForeColor, FontStyle.Regular, true);
            this.AddStyle(StringConsts.XMLCommentsInternalName, SyntaxConsts.DefaultXmlCommentsForeColor, FontStyle.Regular, false);
            this.AddStyle(StringConsts.SymbolsInternalName, SyntaxConsts.DefaultSymbolForeColor);
            this.AddStyle(StringConsts.WhiteSpaceInternalName, Color.Empty);
            this.AddStyle(StringConsts.StringsInternalName, SyntaxConsts.DefaultStringsForeColor, FontStyle.Regular, true);
            this.AddStyle(StringConsts.DirectivesInternalName, SyntaxConsts.DefaultDirectivesForeColor);
            this.AddStyle(StringConsts.HTMLParamsInternalName, SyntaxConsts.DefaultHtmlParamsForeColor);
            this.AddStyle(StringConsts.SyntaxErrorsInternalName, SyntaxConsts.DefaultSyntaxErrorsForeColor);
            this.AddStyle(StringConsts.CodeSnippetsInternalName, Color.Black, SyntaxConsts.DefaultCodeSnippetBackColor, FontStyle.Regular, false);
        }

        protected virtual void InitLanguage()
        {
        }

        protected virtual void InitLexer()
        {
            this.lexerStates.Clear();
            this.InitCopyright();
            this.InitLanguage();
            this.InitStyles();
            this.StateChanged();
        }

        protected virtual void InitStyles()
        {
        }

        public virtual bool IsCodeCompletionChar(char ch, byte style, ref int interval)
        {
            if ((style > 0) && this.Scheme.IsPlainText((byte) (style - 1)))
            {
                return false;
            }
            bool flag = Array.IndexOf<char>(this.codeCompletionChars, ch) >= 0;
            if (flag && (this.GetCompletionType(ch) == CodeCompletionType.CompleteComment))
            {
                interval = 0;
            }
            return flag;
        }

        public virtual bool IsContentDivider(int index)
        {
            return false;
        }

        public virtual bool IsDeclaration(ISyntaxNode node)
        {
            return false;
        }

        protected virtual void LexIdent()
        {
            base.currentPos++;
            int length = base.source.Length;
            while (base.currentPos < length)
            {
                char ch = base.source[base.currentPos];
                if ((((ch < 'a') || (ch > 'z')) && ((ch < 'A') || (ch > 'Z'))) && ((((ch < '0') || (ch > '9')) && ((ch != '_') && ((ch < '\x00c0') || (ch > 'Ɏ')))) && ((ch < '\x00df') || (ch > 'ʯ'))))
                {
                    break;
                }
                base.currentPos++;
            }
        }

        protected virtual void LexNum()
        {
            base.currentPos++;
            int length = base.source.Length;
            while (base.currentPos < length)
            {
                char ch = base.source[base.currentPos];
                if ((ch < '0') || (ch > '9'))
                {
                    break;
                }
                base.currentPos++;
            }
        }

        protected virtual void LexSpace()
        {
            base.currentPos++;
            int length = base.source.Length;
            while (base.currentPos < length)
            {
                char ch = base.source[base.currentPos];
                if ((ch < '\0') || (ch > ' '))
                {
                    break;
                }
                base.currentPos++;
            }
        }

        public virtual bool LoadFile(string fileName)
        {
            return this.LoadFile(fileName, null);
        }

        public virtual bool LoadFile(string fileName, Encoding encoding)
        {
            bool flag;
            this.BeforeLoad();
            try
            {
                flag = this.Strings.LoadFile(fileName, encoding);
            }
            finally
            {
                this.AfterLoad();
            }
            return flag;
        }

        public virtual bool LoadStream(Stream stream)
        {
            return this.LoadStream(stream, null);
        }

        public virtual bool LoadStream(TextReader reader)
        {
            bool flag;
            this.BeforeLoad();
            try
            {
                flag = this.Strings.LoadStream(reader);
            }
            finally
            {
                this.AfterLoad();
            }
            return flag;
        }

        public virtual bool LoadStream(Stream stream, Encoding encoding)
        {
            bool flag;
            this.BeforeLoad();
            try
            {
                flag = this.Strings.LoadStream(stream, encoding);
            }
            finally
            {
                this.AfterLoad();
            }
            return flag;
        }

        protected virtual void OnAutoIndentCharsChanged()
        {
        }

        protected virtual void OnCodeCompletionCharsChanged()
        {
        }

        protected virtual void OnCodeCompletionStopCharsChanged()
        {
        }

        protected virtual void OnOptionsChanged()
        {
        }

        protected virtual void OnRepositoryChanged()
        {
        }

        protected virtual void OnSmartFormatCharsChanged()
        {
        }

        protected virtual void OnSyntaxTreeChanged()
        {
        }

        protected override void OnXmlSchemeChanged()
        {
            this.InitLanguage();
        }

        public virtual int Outline(IList<IRange> ranges)
        {
            this.Outline(ranges, this.syntaxTree.Root, 0);
            return ranges.Count;
        }

        protected virtual void Outline(IList<IRange> ranges, ISyntaxNode node, int level)
        {
            if ((node.Options & SyntaxNodeOptions.Outlining) != SyntaxNodeOptions.None)
            {
                Point startPoint = node.Range.StartPoint;
                ISyntaxAttribute attribute = node.FindAttribute(SyntaxConsts.DeclarationScope);
                if (attribute != null)
                {
                    startPoint = attribute.Position;
                }
                attribute = node.FindAttribute(SyntaxConsts.OutlineText);
                ranges.Add(new OutlineRange(startPoint, node.Range.EndPoint, level, (attribute != null) ? ((string) attribute.Value) : "..."));
                level++;
            }
            if (node.HasChildren)
            {
                foreach (ISyntaxNode node2 in node.ChildList)
                {
                    this.Outline(ranges, node2, level);
                }
            }
        }

        public override int ParseText(int state, int line, string str, ref short[] colorData)
        {
            if (this.useScheme)
            {
                return base.ParseText(state, line, str, ref colorData);
            }
            this.State = state;
            base.lineIndex = line;
            base.source = str;
            int length = str.Length;
            int index = 0;
            while (index < length)
            {
                int num2 = index;
                LexerProc proc = this.lexerState.GetProc(str[index]);
                if (proc == null)
                {
                    colorData[index] = 0;
                    index++;
                }
                else
                {
                    if (num2 != index)
                    {
                        for (int j = index; j < num2; j++)
                        {
                            colorData[j] = 0;
                        }
                    }
                    base.currentPos = index;
                    base.tokenPos = index;
                    int lexerStyle = this.GetLexerStyle(proc());
                    for (int i = index; i < Math.Min(base.currentPos, length); i++)
                    {
                        colorData[i] = (byte) (lexerStyle + 1);
                    }
                    index = base.currentPos;
                }
            }
            base.OnTextParsed(str, ref colorData);
            return this.State;
        }

        public override int ParseText(int state, int line, string s, ref int pos, ref int len, ref int token)
        {
            if (this.useScheme)
            {
                return base.ParseText(state, line, s, ref pos, ref len, ref token);
            }
            this.State = state;
            base.lineIndex = line;
            base.source = s;
            int length = s.Length;
            LexerProc proc = null;
            while (pos < length)
            {
                proc = this.lexerState.GetProc(s[pos]);
                if (proc != null)
                {
                    break;
                }
                pos++;
            }
            if (proc != null)
            {
                base.currentPos = pos;
                base.tokenPos = pos;
                token = proc();
                len = base.currentPos - pos;
            }
            return this.State;
        }

        public virtual bool ProcessAutoComplete(string text, Point position, out string code)
        {
            code = string.Empty;
            return false;
        }

        protected void RegisterLexerProc(int state, LexerProc proc)
        {
            this.GetLexerState(state).StateProc = proc;
        }

        protected void RegisterLexerProc(int state, char[] chars, LexerProc proc)
        {
            this.GetLexerState(state).Add(chars, proc);
        }

        protected void RegisterLexerProc(int state, char ch, LexerProc proc)
        {
            this.RegisterLexerProc(state, new char[] { ch }, proc);
        }

        protected void RegisterLexerProc(int state, char startChar, char endChar, LexerProc proc)
        {
            char[] chars = new char[(endChar - startChar) + 1];
            for (char ch = startChar; ch <= endChar; ch = (char) (ch + '\x0001'))
            {
                chars[ch - startChar] = ch;
            }
            this.RegisterLexerProc(state, chars, proc);
        }

        public virtual bool ReparseBlock(Point position)
        {
            return false;
        }

        public virtual bool ReparseBlock(Point position, string text, out ISyntaxNode node, CodeCompletionType completionType)
        {
            bool flag = this.ReparseBlock(position);
            node = this.GetNodeAt(position);
            return flag;
        }

        public virtual void ReparseText()
        {
            if (this.TextReparsed != null)
            {
                this.TextReparsed(this, EventArgs.Empty);
            }
        }

        public override void Reset()
        {
            base.Reset();
            this.syntaxTree.Clear();
        }

        public virtual void ResetAutoIndentChars()
        {
            this.AutoIndentChars = new char[0];
        }

        public virtual void ResetCodeCompletionChars()
        {
            this.CodeCompletionChars = new char[0];
        }

        public virtual void ResetCodeCompletionStopChars()
        {
            this.CodeCompletionStopChars = new char[0];
        }

        public virtual void ResetOptions()
        {
            this.Options = SyntaxOptions.None;
        }

        public virtual void ResetSmartFormatChars()
        {
            this.SmartFormatChars = new char[0];
        }

        public virtual bool ShouldSerializeAutoIndentChars()
        {
            return (this.autoIndentChars.Length != 0);
        }

        public virtual bool ShouldSerializeCodeCompletionChars()
        {
            return (this.codeCompletionChars.Length != 0);
        }

        public virtual bool ShouldSerializeCodeCompletionStopChars()
        {
            return (this.codeCompletionStopChars.Length != 0);
        }

        public virtual bool ShouldSerializeSmartFormatChars()
        {
            return (this.smartFormatChars.Length != 0);
        }

        public virtual int SmartFormatLine(int index, string text, short[] textData, ITextUndoList operations)
        {
            return this.GetSmartIndent(index, false);
        }

        protected virtual void SortMembers(IListMembers members)
        {
            members.Sort(this.listMemberComparer);
        }

        protected override void StateChanged()
        {
            this.lexerState = this.GetLexerState(this.State);
        }

        [Description("Gets or sets a collection of characters that initializes an indentation procedure when typing.")]
        public virtual char[] AutoIndentChars
        {
            get
            {
                return this.autoIndentChars;
            }
            set
            {
                if (this.autoIndentChars != value)
                {
                    this.autoIndentChars = value;
                    this.OnAutoIndentCharsChanged();
                }
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual bool CaseSensitive
        {
            get
            {
                return false;
            }
        }

        [Description("Gets or sets a collection of characters that initializes a code completion procedure when typing.")]
        public virtual char[] CodeCompletionChars
        {
            get
            {
                return this.codeCompletionChars;
            }
            set
            {
                if (this.codeCompletionChars != value)
                {
                    this.codeCompletionChars = value;
                    this.OnCodeCompletionCharsChanged();
                }
            }
        }

        [Description("Gets or sets a collection of characters that finalizes a code completion procedure when typing.")]
        public virtual char[] CodeCompletionStopChars
        {
            get
            {
                return this.codeCompletionStopChars;
            }
            set
            {
                if (this.codeCompletionStopChars != value)
                {
                    this.codeCompletionStopChars = value;
                    this.OnCodeCompletionStopCharsChanged();
                }
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual ICodeSnippetsProvider CodeSnippets
        {
            get
            {
                return this.CompletionRepository.GetCodeSnippets(this.Scheme.FileType);
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public virtual ICodeCompletionRepository CompletionRepository
        {
            get
            {
                if (this.repository == null)
                {
                    this.repository = this.CreateRepository();
                }
                return this.repository;
            }
            set
            {
                if (this.repository != value)
                {
                    this.repository = value;
                    this.OnRepositoryChanged();
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), Browsable(true), Editor("QWhale.Design.FlagEnumerationEditor, QWhale.Editor", typeof(UITypeEditor)), Description("Gets or sets a flags determining syntax parsing and formatting behavior.")]
        public virtual SyntaxOptions Options
        {
            get
            {
                return this.options;
            }
            set
            {
                if (this.options != value)
                {
                    this.options = value;
                    this.OnOptionsChanged();
                }
            }
        }

        [Description("Gets or sets a collection of characters that initializes a smart formatting procedure when typing.")]
        public virtual char[] SmartFormatChars
        {
            get
            {
                return this.smartFormatChars;
            }
            set
            {
                if (this.smartFormatChars != value)
                {
                    this.smartFormatChars = value;
                    this.OnSmartFormatCharsChanged();
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public virtual ISyntaxTree SyntaxTree
        {
            get
            {
                return this.syntaxTree;
            }
            set
            {
                if (this.syntaxTree != value)
                {
                    this.syntaxTree = value;
                    this.OnSyntaxTreeChanged();
                }
            }
        }

        [Description("Gets or sets a boolean value that indicates whether \"ISyntaxParser\" should perform lexical analysis based on it's rules rather than using internal method."), DefaultValue(false)]
        public virtual bool UseScheme
        {
            get
            {
                return this.useScheme;
            }
            set
            {
                if (this.useScheme != value)
                {
                    this.useScheme = value;
                    this.Update();
                }
            }
        }

        [Browsable(false)]
        public override string XmlScheme
        {
            get
            {
                return base.XmlScheme;
            }
            set
            {
                base.XmlScheme = value;
            }
        }

        private class LexerState
        {
            private Hashtable hash = new Hashtable();
            private LexerProc stateProc;

            public void Add(char[] chars, LexerProc proc)
            {
                foreach (char ch in chars)
                {
                    this.hash[ch] = proc;
                }
            }

            public void Clear()
            {
                this.hash.Clear();
            }

            public LexerProc GetProc(char ch)
            {
                LexerProc stateProc = (LexerProc) this.hash[ch];
                if (stateProc == null)
                {
                    stateProc = this.stateProc;
                }
                return stateProc;
            }

            public Hashtable Hash
            {
                get
                {
                    return this.hash;
                }
            }

            public LexerProc StateProc
            {
                get
                {
                    return this.stateProc;
                }
                set
                {
                    this.stateProc = value;
                }
            }
        }

        private class LineNodeComparer : IComparer<ISyntaxNode>
        {
            public int Compare(ISyntaxNode x, ISyntaxNode y)
            {
                IRange range = x.Range;
                int num = y.Range.StartPoint.Y;
                if ((num > range.StartPoint.Y) && (num <= range.EndPoint.Y))
                {
                    return 0;
                }
                return (range.StartPoint.Y - num);
            }
        }

        private class ListMemberComparer : IComparer<ICodeCompletionProviderItem>
        {
            private ISyntaxParser owner;

            public ListMemberComparer(ISyntaxParser owner)
            {
                this.owner = owner;
            }

            public int Compare(ICodeCompletionProviderItem x, ICodeCompletionProviderItem y)
            {
                return string.Compare(((IListMember) x).Name, ((IListMember) y).Name, !this.owner.CaseSensitive);
            }
        }

        internal class MemberItemComparer : IComparer<MemberItem>
        {
            public int Compare(MemberItem x, MemberItem y)
            {
                return string.Compare(x.String, y.String, true);
            }

            public bool FindPartial(ISortList<MemberItem> list, string name, out int idx)
            {
                list.FindFirst(new MemberItem(name, -1), out idx, this);
                return (((idx >= 0) && (idx < list.Count)) && (this.PartialCompare(name, list[idx].ToString()) == 0));
            }

            public int PartialCompare(object x, object y)
            {
                string strA = x.ToString();
                string strB = y.ToString();
                return string.Compare(strA, 0, strB, 0, Math.Min(strA.Length, strB.Length), true);
            }
        }

        private class NodeComparer : IComparer<ISyntaxNode>
        {
            public int Compare(ISyntaxNode x, ISyntaxNode y)
            {
                IRange range = x.Range;
                IRange range2 = y.Range;
                int num = range.StartPoint.Y - range2.StartPoint.Y;
                if (num == 0)
                {
                    num = range.StartPoint.X - range2.StartPoint.X;
                }
                if (num == 0)
                {
                    num = range.EndPoint.Y - range2.EndPoint.Y;
                }
                if (num == 0)
                {
                    num = range.EndPoint.X - range2.EndPoint.X;
                }
                return num;
            }
        }

        private class PointNodeComparer : IComparer<ISyntaxNode>
        {
            public int Compare(ISyntaxNode x, ISyntaxNode y)
            {
                IRange range = x.Range;
                Point startPoint = y.Range.StartPoint;
                if (((startPoint.Y > range.StartPoint.Y) || ((startPoint.Y == range.StartPoint.Y) && (startPoint.X >= range.StartPoint.X))) && ((startPoint.Y < range.EndPoint.Y) || ((startPoint.Y == range.EndPoint.Y) && (startPoint.X <= range.EndPoint.X))))
                {
                    return 0;
                }
                if (startPoint.Equals(range.StartPoint))
                {
                    return 1;
                }
                if (range.StartPoint.Y != startPoint.Y)
                {
                    return (range.StartPoint.Y - startPoint.Y);
                }
                return (range.StartPoint.X - startPoint.X);
            }
        }
    }
}

