namespace QWhale.Editor.TextSource
{
    using QWhale.Common;
    using QWhale.Syntax;
    using System;
    using System.ComponentModel;

    public class StringItem : IStringItem
    {
        private int lexState;
        private int prevLexState;
        private ItemState state;
        private string str;
        private short[] textData;

        public StringItem(string s)
        {
            this.String = s;
        }

        public virtual void ClearTextStyle(int start, int len, TextStyle style)
        {
            ClearTextStyle(ref this.textData, start, len, style);
        }

        public static void ClearTextStyle(ref short[] textData, int start, int len, TextStyle style)
        {
            for (int i = start; i < (start + len); i++)
            {
                short num = textData[i];
                byte num2 = (byte) ((num >> 8) & ~((byte) style));
                textData[i] = (short) (((byte) num) | (num2 << 8));
            }
        }

        protected virtual void OnLexStateChanged()
        {
        }

        protected virtual void OnPrevLexStateChanged()
        {
        }

        protected virtual void OnStateCahnged()
        {
        }

        protected virtual void OnStringChanged()
        {
            this.textData = new short[this.str.Length];
        }

        protected virtual void OnTextDataChanged()
        {
        }

        public virtual void SetTextStyle(int start, int len, TextStyle style)
        {
            SetTextStyle(ref this.textData, start, len, style);
        }

        public static void SetTextStyle(ref short[] textData, int start, int len, TextStyle style)
        {
            for (int i = start; i < (start + len); i++)
            {
                short num = textData[i];
                byte num4 = (byte) style;
                byte num2 = (byte) ((num >> 8) | num4);
                textData[i] = (short) (((byte) num) | (num2 << 8));
            }
        }

        public static string[] Split(string text)
        {
            if ((text == null) || (text == string.Empty))
            {
                return new string[0];
            }
            char ch = '\r';
            return text.Replace("\r\n", ch.ToString()).Split(Consts.crlfArray, 0x7fffffff);
        }

        public virtual TextStyle TextStyleAt(int pos)
        {
            int num = this.textData[pos];
            return (TextStyle) (num >> 8);
        }

        [Description("Gets or sets index of lexical state at item end.")]
        public virtual int LexState
        {
            get
            {
                return this.lexState;
            }
            set
            {
                if (this.lexState != value)
                {
                    this.lexState = value;
                    this.OnLexStateChanged();
                }
            }
        }

        [Description("Gets or sets index of lexical state at item start.")]
        public virtual int PrevLexState
        {
            get
            {
                return this.prevLexState;
            }
            set
            {
                if (this.prevLexState != value)
                {
                    this.prevLexState = value;
                    this.OnPrevLexStateChanged();
                }
            }
        }

        [Description("Gets or sets current state of the \"IStringItem\".")]
        public virtual ItemState State
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
                    this.OnStateCahnged();
                }
            }
        }

        [Description("Gets or sets string content of the \"IStringItem\".")]
        public virtual string String
        {
            get
            {
                return this.str;
            }
            set
            {
                if (this.str != value)
                {
                    this.str = value;
                    this.OnStringChanged();
                }
            }
        }

        [Description("Gets or sets information of the \"IStringItem\".")]
        public virtual short[] TextData
        {
            get
            {
                return this.textData;
            }
            set
            {
                if (this.textData != value)
                {
                    this.textData = value;
                    this.OnTextDataChanged();
                }
            }
        }
    }
}

