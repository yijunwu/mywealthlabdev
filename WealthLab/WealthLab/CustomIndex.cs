namespace WealthLab
{
    using Fidelity.Components;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Xml.Serialization;

    [XmlRoot(ElementName="CustomIndex", IsNullable=false)]
    public class CustomIndex
    {
        private BarScale barScale_0;
        private WealthLab.DataSource dataSource_0;
        private Guid guid_0;
        private Guid guid_1;
        private IDataHost idataHost_0;
        private WealthLab.IndexDefinition indexDefinition_0;
        private IndexInformationControl indexInformationControl_0;
        private int int_0;
        private string string_0;
        private string string_1;
        private string string_2;
        private string string_3;

        public CustomIndex()
        {
            this.guid_0 = Guid.Empty;
            this.string_0 = "";
            this.string_1 = "";
            this.guid_1 = Guid.Empty;
            this.string_2 = "";
            this.string_3 = "";
            this.indexInformationControl_0 = new IndexInformationControl();
        }

        internal CustomIndex(string string_4, string string_5, Guid guid_2, string string_6, BarScale barScale_1, int int_1, string string_7, WealthLab.DataSource dataSource_1)
        {
            this.guid_0 = Guid.Empty;
            this.string_0 = "";
            this.string_1 = "";
            this.guid_1 = Guid.Empty;
            this.string_2 = "";
            this.string_3 = "";
            this.indexInformationControl_0 = new IndexInformationControl();
            this.string_0 = string_4;
            this.string_1 = string_5;
            this.guid_1 = guid_2;
            this.string_2 = string_6;
            this.barScale_0 = barScale_1;
            this.int_0 = int_1;
            this.string_3 = string_7;
            this.dataSource_0 = dataSource_1;
        }

        public static CustomIndex FromFile(string fileName)
        {
            FileStream stream = null;
            CustomIndex index;
            try
            {
                stream = File.Open(fileName, FileMode.Open, FileAccess.Read);
                XmlSerializer serializer = new XmlSerializer(typeof(CustomIndex));
                index = (CustomIndex) serializer.Deserialize(stream);
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }
            return index;
        }

        public bool SaveSettings()
        {
            if (this.IndexInformation != null)
            {
                return this.IndexInformation.method_0(this);
            }
            return true;
        }

        public void SaveToFile(string fileName)
        {
            FileNameValidator.ValidateFileName(fileName);
            FileStream stream = null;
            try
            {
                stream = File.Create(fileName);
                new XmlSerializer(typeof(CustomIndex)).Serialize((Stream) stream, this);
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }
        }

        public override string ToString()
        {
            return this.FriendlyName;
        }

        public int BarInterval
        {
            get
            {
                return this.int_0;
            }
            set
            {
                this.int_0 = value;
            }
        }

        [XmlIgnore]
        internal IDataHost DataHost
        {
            set
            {
                this.idataHost_0 = value;
            }
        }

        public WealthLab.DataSource DataSource
        {
            get
            {
                WealthLab.DataSource source = null;
                if (this.idataHost_0 != null)
                {
                    using (IEnumerator<WealthLab.DataSource> enumerator = this.idataHost_0.DataSources.GetEnumerator())
                    {
                        WealthLab.DataSource current;
                        while (enumerator.MoveNext())
                        {
                            current = enumerator.Current;
                            if (current.Name == this.DataSourceName)
                            {
                                ///goto  Label_0042;  ///WYJ fix, simplify the flow
                                source = current;
                                break;
                            }
                        }
                    }
                }
                if (source == null)
                {
                    throw new DataSetNotFoundException();
                }
                if ((this.Scale != source.Scale) || (this.BarInterval != source.BarInterval))
                {
                    throw new DataSetScaleMismatchException();
                }
                return source;
            }
        }

        public string DataSourceName
        {
            get
            {
                return this.string_2;
            }
            set
            {
                this.string_2 = value;
            }
        }

        [XmlIgnore]
        public WealthLab.DataSource DataSourceParent
        {
            get
            {
                return this.dataSource_0;
            }
            set
            {
                this.dataSource_0 = value;
            }
        }

        public string DataSourceParentName
        {
            get
            {
                return this.string_3;
            }
            set
            {
                this.string_3 = value;
            }
        }

        public static string FolderName
        {
            get
            {
                return "CustomIndices";
            }
        }

        [XmlIgnore]
        public string FriendlyName
        {
            get
            {
                return this.Symbol;
            }
        }

        public Guid ID
        {
            get
            {
                if (this.guid_0 == Guid.Empty)
                {
                    this.guid_0 = Guid.NewGuid();
                }
                return this.guid_0;
            }
            set
            {
                this.guid_0 = value;
            }
        }

        [XmlIgnore]
        public WealthLab.IndexDefinition IndexDefinition
        {
            get
            {
                return this.indexDefinition_0;
            }
            internal set
            {
                this.indexDefinition_0 = value;
            }
        }

        public Guid IndexDefinitionTypeID
        {
            get
            {
                return this.guid_1;
            }
            set
            {
                this.guid_1 = value;
            }
        }

        [XmlIgnore]
        public IndexInformationControl IndexInformation
        {
            get
            {
                return this.indexInformationControl_0;
            }
            set
            {
                this.indexInformationControl_0 = value;
            }
        }

        public string Parameters
        {
            get
            {
                return this.string_1;
            }
            set
            {
                this.string_1 = value;
            }
        }

        public BarScale Scale
        {
            get
            {
                return this.barScale_0;
            }
            set
            {
                this.barScale_0 = value;
            }
        }

        public string Symbol
        {
            get
            {
                return this.string_0;
            }
            set
            {
                this.string_0 = value;
            }
        }
    }
}

