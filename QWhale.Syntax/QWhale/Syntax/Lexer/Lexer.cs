namespace QWhale.Syntax.Lexer
{
    using QWhale.Common;
    using QWhale.Syntax.Design;
    using QWhale.Syntax.Serialization;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing.Design;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Text.RegularExpressions;

    [ToolboxItem(false)]
    public class Lexer : Component, ILexer, INotify, IUpdate
    {
        private Container components;
        private int defaultState;
        private EventHandler notifyHandler;
        private ParseTextEventArgs parseTextEventArgs;
        private ILexScheme scheme;
        private int updateCount;

        [Description("Occurs when text line is parsed, allowing to modify colors/styles information about the parsed line.")]
        public event ParseTextEvent Parse;

        public Lexer()
        {
            this.InitializeComponent();
            this.parseTextEventArgs = new ParseTextEventArgs();
            this.scheme = this.CreateLexScheme();
            TrialVersion.CheckTrialVersion();
        }

        public Lexer(IContainer container) : this()
        {
            container.Add(this);
        }

        public virtual void AddNotifier(INotifier sender)
        {
            this.notifyHandler = (EventHandler) Delegate.Combine(this.notifyHandler, new EventHandler(sender.Notification));
        }

        public virtual int BeginUpdate()
        {
            this.updateCount++;
            return this.updateCount;
        }

        protected virtual LexScheme CreateLexScheme()
        {
            return new LexScheme(this);
        }

        public virtual int DisableUpdate()
        {
            this.updateCount++;
            return this.updateCount;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        public virtual int EnableUpdate()
        {
            this.updateCount--;
            return this.updateCount;
        }

        public virtual int EndUpdate()
        {
            this.updateCount--;
            if (this.updateCount == 0)
            {
                this.Update();
            }
            return this.updateCount;
        }

        private void InitializeComponent()
        {
            this.components = new Container();
        }

        public virtual void Notify()
        {
            if (this.notifyHandler != null)
            {
                this.notifyHandler(this, EventArgs.Empty);
            }
        }

        protected virtual void OnDefaultStateChanged()
        {
            this.Update();
        }

        protected virtual void OnSchemeChanged()
        {
        }

        protected void OnTextParsed(string str, ref short[] colorData)
        {
            if (this.Parse != null)
            {
                this.parseTextEventArgs.String = str;
                this.parseTextEventArgs.ColorData = colorData;
                this.Parse(this, this.parseTextEventArgs);
            }
        }

        protected virtual void OnXmlSchemeChanged()
        {
        }

        public virtual int ParseText(int state, int line, string str, ref short[] colorData)
        {
            if ((str != null) && (str != string.Empty))
            {
                int num2;
                int length = str.Length;
                if (this.scheme.States.Count == 0)
                {
                    num2 = 0;
                    while (num2 < length)
                    {
                        colorData[num2] = 0;
                        num2++;
                    }
                    this.OnTextParsed(str, ref colorData);
                    return state;
                }
                ILexState state2 = this.scheme.States[state];
                Match match = state2.Regex.Match(str);
                int startIndex = 0;
                int num4 = 0;
                int style = 0;
                while (match.Success)
                {
                    if (match.Index > startIndex)
                    {
                        num2 = startIndex;
                        while (num2 < match.Index)
                        {
                            colorData[num2] = 0;
                            num2++;
                        }
                    }
                    startIndex = match.Index;
                    this.StateFromMatch(match, str.Substring(startIndex, match.Length), state2.Blocks, ref num4, ref style);
                    if (match.Length > 0)
                    {
                        num2 = startIndex;
                        while (num2 < (startIndex + match.Length))
                        {
                            colorData[num2] = (byte) (style + 1);
                            num2++;
                        }
                    }
                    startIndex += match.Length;
                    if (num4 != state)
                    {
                        state = num4;
                        state2 = this.scheme.States[num4];
                        match = state2.Regex.Match(str, startIndex);
                    }
                    else
                    {
                        match = match.NextMatch();
                    }
                }
                for (num2 = startIndex; num2 < length; num2++)
                {
                    colorData[num2] = 0;
                }
                this.OnTextParsed(str, ref colorData);
            }
            return state;
        }

        public virtual int ParseText(int state, int line, string str, ref int pos, ref int len, ref int style)
        {
            len = 0;
            style = 0;
            if (str != null)
            {
                ILexState state2 = this.scheme.States[state];
                Match match = state2.Regex.Match(str, pos);
                if (match.Success)
                {
                    pos = match.Index;
                    len = match.Length;
                    int num = state;
                    int num2 = 0;
                    this.StateFromMatch(match, str.Substring(pos, len), state2.Blocks, ref num, ref style);
                    bool flag = (pos + match.Length) >= str.Length;
                    int startat = flag ? (pos + match.Length) : pos;
                    while ((match.Length == 0) || flag)
                    {
                        if (state != num)
                        {
                            state = num;
                            state2 = this.scheme.States[state];
                            match = state2.Regex.Match(str, startat);
                        }
                        else
                        {
                            match = match.NextMatch();
                        }
                        if (!match.Success)
                        {
                            break;
                        }
                        if (!flag)
                        {
                            pos = match.Index;
                            len = match.Length;
                        }
                        this.StateFromMatch(match, string.Empty, state2.Blocks, ref num, ref num2);
                        if (!flag)
                        {
                            style = num2;
                        }
                    }
                    state = num;
                }
                if (len == 0)
                {
                    len = 1;
                }
            }
            return state;
        }

        public virtual void RemoveNotifier(INotifier sender)
        {
            this.notifyHandler = (EventHandler) Delegate.Remove(this.notifyHandler, new EventHandler(sender.Notification));
        }

        public virtual string RemovePlainText(string s, short[] textData)
        {
            string str = string.Empty;
            for (int i = 0; i < s.Length; i++)
            {
                str = str + (this.scheme.IsPlainText(((byte) textData[i]) - 1) ? 0x20 : s[i]);
            }
            return str;
        }

        public virtual void ResetDefaultState()
        {
            this.DefaultState = 0;
        }

        private void StateFromMatch(Match match, string s, Dictionary<int, ILexSyntaxBlock> blocks, ref int state, ref int style)
        {
            style = -1;
            ILexSyntaxBlock block = null;
            for (int i = 0; i < match.Groups.Count; i++)
            {
                if (match.Groups[i].Success && blocks.ContainsKey(i))
                {
                    block = blocks[i];
                    break;
                }
            }
            if (block != null)
            {
                if (block.LeaveState != null)
                {
                    state = block.LeaveState.Index;
                }
                if (block.Style != null)
                {
                    style = block.Style.Index;
                }
                int num2 = block.FindResword(s);
                if (num2 >= 0)
                {
                    ILexStyle reswordStyle = block.ReswordSets[num2].ReswordStyle;
                    if (reswordStyle != null)
                    {
                        style = reswordStyle.Index;
                    }
                }
            }
        }

        public virtual void Update()
        {
            if (this.updateCount == 0)
            {
                this.Notify();
            }
        }

        [Category("Parser"), Description("Gets or sets default state of the \"Lexer\".")]
        public virtual int DefaultState
        {
            get
            {
                return this.defaultState;
            }
            set
            {
                if (this.defaultState != value)
                {
                    this.defaultState = value;
                    this.OnDefaultStateChanged();
                }
            }
        }

        [Editor("QWhale.Syntax.Design.SyntaxBuilderEditor, QWhale.Syntax", typeof(UITypeEditor)), TypeConverter(typeof(LexSchemeConverter)), Category("Parser"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual ILexScheme Scheme
        {
            get
            {
                return this.scheme;
            }
            set
            {
                if (this.scheme != value)
                {
                    this.scheme = value;
                    this.OnSchemeChanged();
                }
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual ISerializationInfo SerializationInfo
        {
            get
            {
                return new XmlLexerInfo(this);
            }
            set
            {
                value.FixupReferences(this);
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public virtual int UpdateCount
        {
            get
            {
                return this.updateCount;
            }
        }

        [Browsable(false)]
        public virtual string XmlScheme
        {
            get
            {
                string str;
                try
                {
                    StringWriter writer = new StringWriter();
                    try
                    {
                        this.Scheme.SaveStream(writer);
                        str = writer.ToString();
                    }
                    finally
                    {
                        writer.Close();
                    }
                }
                catch
                {
                    str = string.Empty;
                }
                return str;
            }
            set
            {
                try
                {
                    StringReader reader = new StringReader(value);
                    try
                    {
                        this.scheme.LoadStream(reader);
                        this.OnXmlSchemeChanged();
                    }
                    finally
                    {
                        reader.Close();
                    }
                }
                catch
                {
                }
            }
        }
    }
}

