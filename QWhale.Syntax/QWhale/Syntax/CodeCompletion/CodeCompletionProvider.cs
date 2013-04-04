namespace QWhale.Syntax.CodeCompletion
{
    using QWhale.Common;
    using QWhale.Syntax;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Windows.Forms;
    using System.Xml.Serialization;

    public class CodeCompletionProvider : List<ICodeCompletionProviderItem>, ICodeCompletionProvider, IList<ICodeCompletionProviderItem>, ICollection<ICodeCompletionProviderItem>, IEnumerable<ICodeCompletionProviderItem>, IEnumerable, IExport, IImport
    {
        private string editField = string.Empty;
        private ImageList images;
        private int selIndex = -1;
        private bool showDescriptions;
        private bool useHtmlFormatting;
        private bool useIndent = true;

        public event ClosePopupEvent ClosePopup;

        public event ShowPopupEvent ShowPopup;

        public virtual bool ColumnVisible(int column)
        {
            return false;
        }

        public virtual string GetColumnText(int index, int column)
        {
            return string.Empty;
        }

        public virtual string GetDescription(int index)
        {
            return string.Empty;
        }

        public virtual int GetImageIndex(int index)
        {
            return -1;
        }

        public virtual string GetName(int index)
        {
            return string.Empty;
        }

        public virtual ICodeCompletionProvider GetParent()
        {
            return null;
        }

        public virtual int GetPriority(int index)
        {
            return 0;
        }

        public virtual string GetText(int index)
        {
            return string.Empty;
        }

        protected virtual System.Type GetXmlType()
        {
            return null;
        }

        public virtual int IndexOfName(string name, bool caseSensitive)
        {
            for (int i = 0; i < base.Count; i++)
            {
                if (string.Compare(name, this.GetName(i), !caseSensitive) == 0)
                {
                    return i;
                }
            }
            return -1;
        }

        public virtual bool LoadFile(string fileName)
        {
            return this.LoadFile(fileName, null);
        }

        public virtual bool LoadFile(string fileName, Encoding encoding)
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

        public virtual bool LoadStream(Stream stream)
        {
            return this.LoadStream(stream, null);
        }

        public virtual bool LoadStream(TextReader reader)
        {
            if (this.GetXmlType() == null)
            {
                return false;
            }
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

        public virtual void OnClosePopup(object sender, ClosingEventArgs e)
        {
            if (this.ClosePopup != null)
            {
                this.ClosePopup(sender, e);
            }
        }

        protected virtual void OnEditFieldChanged()
        {
        }

        protected virtual void OnImagesChanged()
        {
        }

        protected virtual void OnSelIndexChanged()
        {
        }

        protected virtual void OnShowDescriptionsChanged()
        {
        }

        public virtual void OnShowPopup(object sender, ShowingEventArgs e)
        {
            if (this.ShowPopup != null)
            {
                this.ShowPopup(sender, e);
            }
        }

        protected virtual void OnUseHtmlFormattingChanged()
        {
        }

        protected virtual void OnUseIndentChanged()
        {
        }

        void ICodeCompletionProvider.Sort()
        {
            base.Sort();
        }

        void ICodeCompletionProvider.Sort(IComparer<ICodeCompletionProviderItem> comparer1)
        {
            base.Sort(comparer1);
        }

        public virtual bool SaveFile(string fileName)
        {
            return this.SaveFile(fileName, null);
        }

        public virtual bool SaveFile(string fileName, Encoding encoding)
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

        public virtual bool SaveStream(Stream stream)
        {
            return this.SaveStream(stream, null);
        }

        public virtual bool SaveStream(TextWriter writer)
        {
            if (this.GetXmlType() == null)
            {
                return false;
            }
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

        public virtual int ColumnCount
        {
            get
            {
                return 0;
            }
        }

        public virtual string[] Descriptions
        {
            get
            {
                string[] strArray = new string[base.Count];
                for (int i = 0; i < base.Count; i++)
                {
                    strArray[i] = this.GetDescription(i);
                }
                return strArray;
            }
        }

        public virtual string EditField
        {
            get
            {
                return this.editField;
            }
            set
            {
                if (this.editField != value)
                {
                    this.editField = value;
                    this.OnEditFieldChanged();
                }
            }
        }

        public virtual string EditPath
        {
            get
            {
                return string.Empty;
            }
        }

        public virtual bool FormatDisplayText
        {
            get
            {
                return false;
            }
        }

        public virtual int[] ImageIndexes
        {
            get
            {
                int[] numArray = new int[base.Count];
                for (int i = 0; i < base.Count; i++)
                {
                    numArray[i] = this.GetImageIndex(i);
                }
                return numArray;
            }
        }

        public virtual ImageList Images
        {
            get
            {
                return this.images;
            }
            set
            {
                if (this.images != value)
                {
                    this.images = value;
                    this.OnImagesChanged();
                }
            }
        }

        public virtual int SelIndex
        {
            get
            {
                return this.selIndex;
            }
            set
            {
                if (this.selIndex != value)
                {
                    this.selIndex = value;
                    this.OnSelIndexChanged();
                }
            }
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

        public virtual bool ShowDescriptions
        {
            get
            {
                return this.showDescriptions;
            }
            set
            {
                if (this.showDescriptions != value)
                {
                    this.showDescriptions = value;
                    this.OnShowDescriptionsChanged();
                }
            }
        }

        public virtual string[] Strings
        {
            get
            {
                string[] strArray = new string[base.Count];
                for (int i = 0; i < base.Count; i++)
                {
                    strArray[i] = this.GetText(i);
                }
                return strArray;
            }
        }

        public virtual bool UseHtmlFormatting
        {
            get
            {
                return this.useHtmlFormatting;
            }
            set
            {
                if (this.useHtmlFormatting != value)
                {
                    this.useHtmlFormatting = value;
                    this.OnUseHtmlFormattingChanged();
                }
            }
        }

        public virtual bool UseIndent
        {
            get
            {
                return this.useIndent;
            }
            set
            {
                if (this.useIndent != value)
                {
                    this.useIndent = value;
                    this.OnUseIndentChanged();
                }
            }
        }
    }
}

