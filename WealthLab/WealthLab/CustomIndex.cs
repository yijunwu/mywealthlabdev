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
        private BarScale barScale;
        private WealthLab.DataSource dataSourceParent;
        private Guid guid;
        private Guid indexDefinitionTypeID;
        private IDataHost idataHost_0;
        private WealthLab.IndexDefinition indexDefinition;
        private IndexInformationControl indexInformationControl;
        private int barInterval;
        private string symbol;
        private string parameters;
        private string dataSourceName;
        private string dataSourceParentName;

        public CustomIndex()
        {
            this.guid = Guid.Empty;
            this.symbol = "";
            this.parameters = "";
            this.indexDefinitionTypeID = Guid.Empty;
            this.dataSourceName = "";
            this.dataSourceParentName = "";
            this.indexInformationControl = new IndexInformationControl();
        }

        internal CustomIndex(string string_4, string string_5, Guid guid_2, string string_6, BarScale barScale_1, int int_1, string string_7, WealthLab.DataSource dataSource_1)
        {
            this.guid = Guid.Empty;
            this.symbol = "";
            this.parameters = "";
            this.indexDefinitionTypeID = Guid.Empty;
            this.dataSourceName = "";
            this.dataSourceParentName = "";
            this.indexInformationControl = new IndexInformationControl();
            this.symbol = string_4;
            this.parameters = string_5;
            this.indexDefinitionTypeID = guid_2;
            this.dataSourceName = string_6;
            this.barScale = barScale_1;
            this.barInterval = int_1;
            this.dataSourceParentName = string_7;
            this.dataSourceParent = dataSource_1;
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
                return this.barInterval;
            }
            set
            {
                this.barInterval = value;
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
                return this.dataSourceName;
            }
            set
            {
                this.dataSourceName = value;
            }
        }

        [XmlIgnore]
        public WealthLab.DataSource DataSourceParent
        {
            get
            {
                return this.dataSourceParent;
            }
            set
            {
                this.dataSourceParent = value;
            }
        }

        public string DataSourceParentName
        {
            get
            {
                return this.dataSourceParentName;
            }
            set
            {
                this.dataSourceParentName = value;
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
                if (this.guid == Guid.Empty)
                {
                    this.guid = Guid.NewGuid();
                }
                return this.guid;
            }
            set
            {
                this.guid = value;
            }
        }

        [XmlIgnore]
        public WealthLab.IndexDefinition IndexDefinition
        {
            get
            {
                return this.indexDefinition;
            }
            internal set
            {
                this.indexDefinition = value;
            }
        }

        public Guid IndexDefinitionTypeID
        {
            get
            {
                return this.indexDefinitionTypeID;
            }
            set
            {
                this.indexDefinitionTypeID = value;
            }
        }

        [XmlIgnore]
        public IndexInformationControl IndexInformation
        {
            get
            {
                return this.indexInformationControl;
            }
            set
            {
                this.indexInformationControl = value;
            }
        }

        public string Parameters
        {
            get
            {
                return this.parameters;
            }
            set
            {
                this.parameters = value;
            }
        }

        public BarScale Scale
        {
            get
            {
                return this.barScale;
            }
            set
            {
                this.barScale = value;
            }
        }

        public string Symbol
        {
            get
            {
                return this.symbol;
            }
            set
            {
                this.symbol = value;
            }
        }
    }
}

