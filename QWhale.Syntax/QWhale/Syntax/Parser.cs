namespace QWhale.Syntax
{
    using QWhale.Common;
    using QWhale.Syntax.Lexer;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.InteropServices;

    [Serializable, ToolboxBitmap(typeof(Parser), "Images.Parser.bmp"), ToolboxItem(true)]
    public class Parser : QWhale.Syntax.Lexer.Lexer, IParser, ILexer, INotify, IUpdate
    {
        protected int currentPos;
        private IStringList internalStrings;
        protected int lineIndex;
        protected string source;
        private IList<ParserState> stack = new List<ParserState>();
        private int state;
        private IStringList strings;
        private int token;
        protected int tokenPos;

        protected virtual int DoNextValidToken()
        {
            int token = this.NextToken();
            while (!this.Eof && !this.IsValidToken(token))
            {
                token = this.NextToken();
            }
            return token;
        }

        protected bool IsStackEmpty()
        {
            return (this.stack.Count == 0);
        }

        protected virtual bool IsValidToken(int Token)
        {
            return true;
        }

        public virtual int NextToken()
        {
            this.tokenPos = this.currentPos;
            if (((this.source == null) || (this.currentPos >= this.source.Length)) && !this.UpdateLine())
            {
                this.token = -1;
            }
            else
            {
                int len = 0;
                this.State = this.ParseText(this.state, this.lineIndex, this.source, ref this.tokenPos, ref len, ref this.token);
                this.currentPos = this.tokenPos + len;
            }
            return this.token;
        }

        public virtual int NextToken(out string str)
        {
            int num = this.NextToken();
            str = this.TokenString;
            return num;
        }

        public virtual int NextValidToken()
        {
            return this.DoNextValidToken();
        }

        public virtual int NextValidToken(out string str)
        {
            int num = this.NextValidToken();
            str = this.TokenString;
            return num;
        }

        protected virtual void OnStringsChanged()
        {
        }

        protected virtual void OnTokenChanged()
        {
        }

        public virtual int PeekToken()
        {
            int num;
            this.SaveState();
            try
            {
                num = this.NextToken();
            }
            finally
            {
                this.RestoreState();
            }
            return num;
        }

        public virtual int PeekToken(out string str)
        {
            int num;
            this.SaveState();
            try
            {
                num = this.NextToken();
                str = this.TokenString;
            }
            finally
            {
                this.RestoreState();
            }
            return num;
        }

        public virtual int PeekValidToken()
        {
            int num;
            this.SaveState();
            try
            {
                num = this.NextValidToken();
            }
            finally
            {
                this.RestoreState();
            }
            return num;
        }

        public virtual int PeekValidToken(out string str)
        {
            int num2;
            this.SaveState();
            try
            {
                int num = this.NextValidToken();
                str = this.TokenString;
                num2 = num;
            }
            finally
            {
                this.RestoreState();
            }
            return num2;
        }

        public virtual void Reset()
        {
            this.Reset(0, 0, this.DefaultState);
        }

        public virtual void Reset(int line, int pos, int state)
        {
            this.lineIndex = line;
            this.token = 0;
            this.tokenPos = pos;
            this.currentPos = pos;
            this.State = state;
            this.ResetLine(line);
        }

        protected virtual void ResetLine(int line)
        {
            this.source = (((this.strings != null) && (line >= 0)) && (line < this.strings.Count)) ? this.strings[line] : null;
        }

        public virtual void RestoreState()
        {
            this.RestoreState(true);
        }

        public virtual void RestoreState(bool restore)
        {
            if (this.stack.Count > 0)
            {
                if (restore)
                {
                    ParserState state = this.stack[this.stack.Count - 1];
                    this.lineIndex = state.LineIndex;
                    this.token = state.Token;
                    this.tokenPos = state.TokenPos;
                    this.currentPos = state.CurrentPos;
                    this.State = state.State;
                    this.ResetLine(this.lineIndex);
                }
                this.stack.RemoveAt(this.stack.Count - 1);
            }
        }

        public virtual void SaveState()
        {
            ParserState item = new ParserState {
                LineIndex = this.lineIndex,
                Token = this.token,
                TokenPos = this.tokenPos,
                CurrentPos = this.currentPos,
                State = this.state
            };
            this.stack.Add(item);
        }

        protected void SetToken(int token)
        {
            this.token = token;
        }

        protected virtual void StateChanged()
        {
        }

        protected virtual bool UpdateLine()
        {
            this.currentPos = 0;
            this.tokenPos = 0;
            this.source = null;
            while (((this.strings != null) && ((this.source == null) || (this.source == string.Empty))) && (this.lineIndex < this.strings.Count))
            {
                this.lineIndex++;
                this.ResetLine(this.lineIndex);
            }
            return ((this.source != null) && (this.source != string.Empty));
        }

        protected virtual Point ValidatePosition(Point position)
        {
            if ((this.strings == null) || (position.Y < this.strings.Count))
            {
                return position;
            }
            if (this.strings.Count != 0)
            {
                return new Point(this.strings[this.strings.Count - 1].Length, this.strings.Count - 1);
            }
            return new Point(0, 0);
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public virtual Point CurrentPosition
        {
            get
            {
                return this.ValidatePosition(new Point(this.currentPos, this.lineIndex));
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual bool Eof
        {
            get
            {
                if ((this.token != -1) && (this.strings != null))
                {
                    return (this.lineIndex >= this.strings.Count);
                }
                return true;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public virtual string[] Lines
        {
            get
            {
                if (this.strings == null)
                {
                    return null;
                }
                string[] array = new string[this.strings.Count];
                this.strings.CopyTo(array, 0);
                return array;
            }
            set
            {
                if (this.internalStrings == null)
                {
                    this.internalStrings = new StringList();
                }
                this.strings = this.internalStrings;
                foreach (string str in value)
                {
                    this.strings.Add(str);
                }
                this.OnStringsChanged();
            }
        }

        protected internal IList<ParserState> Stack
        {
            get
            {
                return this.stack;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual int State
        {
            get
            {
                return this.state;
            }
            set
            {
                if (this.state != value)
                {
                    this.state = value;
                    this.StateChanged();
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public virtual IStringList Strings
        {
            get
            {
                return this.strings;
            }
            set
            {
                if (this.strings != value)
                {
                    this.strings = value;
                    this.OnStringsChanged();
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public virtual int Token
        {
            get
            {
                return this.token;
            }
            set
            {
                if (this.token != value)
                {
                    this.token = value;
                    this.OnTokenChanged();
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public virtual Point TokenPosition
        {
            get
            {
                return this.ValidatePosition(new Point(this.tokenPos, this.lineIndex));
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual string TokenString
        {
            get
            {
                if ((this.source != null) && (this.tokenPos < this.source.Length))
                {
                    return this.source.Substring(this.tokenPos, Math.Min(this.currentPos, this.source.Length) - this.tokenPos);
                }
                return string.Empty;
            }
        }

        internal protected class ParserState
        {
            public int CurrentPos;
            public int LineIndex;
            public int State;
            public int Token;
            public int TokenPos;
        }
    }
}

