namespace QWhale.Editor.Dialogs
{
    using QWhale.Common;
    using QWhale.Syntax;
    using System;
    using System.ComponentModel;
    using System.IO;
    using System.Text;
    using System.Xml.Serialization;

    public class PersistentSettings : IPersistentSettings, IImport, IExport
    {
        public virtual void Assign(IPersistentSettings source)
        {
        }

        public virtual Type GetXmlType()
        {
            return null;
        }

        public virtual bool LoadFile(string fileName)
        {
            return this.LoadFile(fileName, null);
        }

        public bool LoadFile(string fileName, Encoding encoding)
        {
            bool flag;
            try
            {
                Stream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                try
                {
                    flag = this.LoadStream(stream, encoding);
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

        public bool LoadStream(Stream stream)
        {
            return this.LoadStream(stream, null);
        }

        public virtual bool LoadStream(TextReader reader)
        {
            XmlSerializer serializer = new XmlSerializer(this.GetXmlType());
            try
            {
                this.SerializationInfo = (ISerializationInfo) serializer.Deserialize(reader);
                return true;
            }
            catch (Exception exception)
            {
                ErrorHandler.Error(exception);
                return false;
            }
        }

        public bool LoadStream(Stream stream, Encoding encoding)
        {
            bool flag;
            StreamReader reader = (encoding != null) ? new StreamReader(stream, encoding) : new StreamReader(stream);
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

        public virtual bool SaveFile(string fileName)
        {
            return this.SaveFile(fileName, null);
        }

        public bool SaveFile(string fileName, Encoding encoding)
        {
            bool flag;
            try
            {
                Stream stream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
                try
                {
                    flag = this.SaveStream(stream, encoding);
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

        public bool SaveStream(Stream stream)
        {
            return this.SaveStream(stream, null);
        }

        public virtual bool SaveStream(TextWriter writer)
        {
            XmlSerializer serializer = new XmlSerializer(this.GetXmlType());
            try
            {
                serializer.Serialize(writer, this.SerializationInfo);
                return true;
            }
            catch (Exception exception)
            {
                writer.Flush();
                ErrorHandler.Error(exception);
                return false;
            }
        }

        public bool SaveStream(Stream stream, Encoding encoding)
        {
            bool flag;
            StreamWriter writer = (encoding != null) ? new StreamWriter(stream, encoding) : new StreamWriter(stream);
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

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual ISerializationInfo SerializationInfo
        {
            get
            {
                return null;
            }
            set
            {
            }
        }
    }
}

