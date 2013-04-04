namespace QWhale.Syntax
{
    using QWhale.Common;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Reflection;
    using System.Text;

    public class StringList : List<string>, IStringList, IList<string>, ICollection<string>, IEnumerable<string>, IEnumerable, IImport, IExport
    {
        private string lineTerminator;

        public StringList()
        {
            this.lineTerminator = "\r\n";
        }

        public StringList(TextReader reader)
        {
            string str;
            this.lineTerminator = "\r\n";
            while ((str = reader.ReadLine()) != null)
            {
                base.Add(str);
            }
        }

        public virtual bool LoadFile(string fileName)
        {
            return this.LoadFile(fileName, null);
        }

        public virtual bool LoadFile(string fileName, Encoding encoding)
        {
            bool flag = true;
            try
            {
                Stream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                try
                {
                    StreamReader reader = (encoding != null) ? new StreamReader(stream, encoding) : new StreamReader(stream);
                    try
                    {
                        this.LoadStream(reader);
                    }
                    finally
                    {
                        reader.Close();
                    }
                    return flag;
                }
                finally
                {
                    stream.Close();
                }
            }
            catch (Exception exception)
            {
                ErrorHandler.Error(exception);
                flag = false;
            }
            return flag;
        }

        public virtual bool LoadStream(Stream stream)
        {
            return this.LoadStream(stream, null);
        }

        public virtual bool LoadStream(TextReader reader)
        {
            string str;
            base.Clear();
            while ((str = reader.ReadLine()) != null)
            {
                base.Add(str);
            }
            return true;
        }

        public virtual bool LoadStream(Stream stream, Encoding encoding)
        {
            bool flag;
            TextReader reader = (encoding != null) ? new StreamReader(stream, encoding) : new StreamReader(stream);
            try
            {
                flag = this.LoadStream(reader);
            }
            finally
            {
                reader.Close();
            }
            return flag;
        }

        protected virtual void OnLineTerminatorChanged()
        {
        }

        protected virtual void OnTextChanged()
        {
        }

        public virtual bool SaveFile(string fileName)
        {
            return this.SaveFile(fileName, null);
        }

        public virtual bool SaveFile(string fileName, Encoding encoding)
        {
            bool flag = true;
            try
            {
                Stream stream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
                try
                {
                    StreamWriter writer = (encoding != null) ? new StreamWriter(stream, encoding) : new StreamWriter(stream);
                    try
                    {
                        this.SaveStream(writer);
                    }
                    finally
                    {
                        writer.Close();
                    }
                    return flag;
                }
                finally
                {
                    stream.Close();
                }
            }
            catch (Exception exception)
            {
                ErrorHandler.Error(exception);
                flag = false;
            }
            return flag;
        }

        public virtual bool SaveStream(Stream stream)
        {
            return this.SaveStream(stream, null);
        }

        public virtual bool SaveStream(TextWriter writer)
        {
            foreach (string str in this)
            {
                writer.WriteLine(str);
            }
            return true;
        }

        public virtual bool SaveStream(Stream stream, Encoding encoding)
        {
            bool flag;
            TextWriter writer = (encoding != null) ? new StreamWriter(stream, encoding) : new StreamWriter(stream);
            try
            {
                flag = this.SaveStream(writer);
            }
            finally
            {
                writer.Close();
            }
            return flag;
        }

        public virtual string this[int index]
        {
            get
            {
                return base[index];
            }
            set
            {
                base[index] = value;
            }
        }

        [Description("Gets or sets a string value that terminates line."), DefaultValue("\r\n")]
        public string LineTerminator
        {
            get
            {
                return this.lineTerminator;
            }
            set
            {
                if (this.lineTerminator != value)
                {
                    this.lineTerminator = value;
                    this.OnLineTerminatorChanged();
                }
            }
        }

        [Description("Gets or sets the strings in the \"SyntaxStrings\" as a single string with the individual strings delimited by carriage returns.")]
        public virtual string Text
        {
            get
            {
                StringBuilder builder = new StringBuilder();
                foreach (string str in this)
                {
                    builder.Append(str + this.lineTerminator);
                }
                if (builder.Length >= 2)
                {
                    builder.Remove(builder.Length - 2, 2);
                }
                return builder.ToString();
            }
            set
            {
                base.Clear();
                if ((value != null) && (value != string.Empty))
                {
                    string str;
                    StringReader reader = new StringReader(value);
                    while ((str = reader.ReadLine()) != null)
                    {
                        base.Add(str);
                    }
                }
                this.OnTextChanged();
            }
        }
    }
}

