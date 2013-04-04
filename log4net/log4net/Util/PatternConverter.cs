namespace log4net.Util
{
    using log4net.Repository;
    using System;
    using System.Collections;
    using System.IO;
    using System.Text;
    using System.Globalization;

    public abstract class PatternConverter
    {
        private const int c_renderBufferMaxCapacity = 0x400;
        private const int c_renderBufferSize = 0x100;
        private ReusableStringWriter m_formatWriter = new ReusableStringWriter(CultureInfo.InvariantCulture);
        private bool m_leftAlign = false;
        private int m_max = 0x7fffffff;
        private int m_min = -1;
        private PatternConverter m_next;
        private string m_option = null;
        private static readonly string[] SPACES = new string[] { " ", "  ", "    ", "        ", "                ", "                                " };

        protected PatternConverter()
        {
        }

        protected abstract void Convert(TextWriter writer, object state);
        public virtual void Format(TextWriter writer, object state)
        {
            if ((this.m_min < 0) && (this.m_max == 0x7fffffff))
            {
                this.Convert(writer, state);
            }
            else
            {
                this.m_formatWriter.Reset(0x400, 0x100);
                this.Convert(this.m_formatWriter, state);
                StringBuilder stringBuilder = this.m_formatWriter.GetStringBuilder();
                int length = stringBuilder.Length;
                if (length > this.m_max)
                {
                    writer.Write(stringBuilder.ToString(length - this.m_max, this.m_max));
                }
                else if (length < this.m_min)
                {
                    if (this.m_leftAlign)
                    {
                        writer.Write(stringBuilder.ToString());
                        SpacePad(writer, this.m_min - length);
                    }
                    else
                    {
                        SpacePad(writer, this.m_min - length);
                        writer.Write(stringBuilder.ToString());
                    }
                }
                else
                {
                    writer.Write(stringBuilder.ToString());
                }
            }
        }

        public virtual PatternConverter SetNext(PatternConverter patternConverter)
        {
            this.m_next = patternConverter;
            return this.m_next;
        }

        protected static void SpacePad(TextWriter writer, int length)
        {
            while (length >= 0x20)
            {
                writer.Write(SPACES[5]);
                length -= 0x20;
            }
            for (int i = 4; i >= 0; i--)
            {
                if ((length & (((int) 1) << i)) != 0)
                {
                    writer.Write(SPACES[i]);
                }
            }
        }

        protected static void WriteDictionary(TextWriter writer, ILoggerRepository repository, IDictionary value)
        {
            writer.Write("{");
            bool flag = true;
            foreach (DictionaryEntry entry in value)
            {
                if (flag)
                {
                    flag = false;
                }
                else
                {
                    writer.Write(", ");
                }
                WriteObject(writer, repository, entry.Key);
                writer.Write("=");
                WriteObject(writer, repository, entry.Value);
            }
            writer.Write("}");
        }

        protected static void WriteObject(TextWriter writer, ILoggerRepository repository, object value)
        {
            if (repository != null)
            {
                repository.RendererMap.FindAndRender(value, writer);
            }
            else if (value == null)
            {
                writer.Write(SystemInfo.NullText);
            }
            else
            {
                writer.Write(value.ToString());
            }
        }

        public virtual log4net.Util.FormattingInfo FormattingInfo
        {
            get
            {
                return new log4net.Util.FormattingInfo(this.m_min, this.m_max, this.m_leftAlign);
            }
            set
            {
                this.m_min = value.Min;
                this.m_max = value.Max;
                this.m_leftAlign = value.LeftAlign;
            }
        }

        public virtual PatternConverter Next
        {
            get
            {
                return this.m_next;
            }
        }

        public virtual string Option
        {
            get
            {
                return this.m_option;
            }
            set
            {
                this.m_option = value;
            }
        }
    }
}

