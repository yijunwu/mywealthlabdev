namespace WealthLab
{
    using Fidelity.Components;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Xml.Serialization;

    [XmlRoot(ElementName="DataSet", IsNullable=false)]
    public class DataSource
    {
        bool filtered = false; ///WYJ fix, to support filtering datasource in datasource panel

        private BarScale barScale;
        private int barInterval;
        private List<string> symbols;
        private StaticDataProvider staticDataProvider;
        private string dsString;
        private string name;
        private string providerName;

        public DataSource()
        {
            this.dsString = "";
            this.name = "";
            this.providerName = "";
        }

        public DataSource(StaticDataProvider staticDataProvider_1)
        {
            this.dsString = "";
            this.name = "";
            this.providerName = "";
            this.staticDataProvider = staticDataProvider_1;
            if (staticDataProvider_1 != null)
            {
                this.providerName = staticDataProvider_1.GetType().Name;
            }
        }

        public bool Filtered
        {
            get { return filtered; }
            set { filtered = value; }
        }

        public static DataSource FromFile(string fileName)
        {
            FileStream stream = null;
            DataSource source;
            try
            {
                stream = File.Open(fileName, FileMode.Open);
                XmlSerializer serializer = new XmlSerializer(typeof(DataSource));
                source = (DataSource) serializer.Deserialize(stream);
            }
            catch
            {
                source = null;
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }
            return source;
        }

        internal void method_0()
        {
            this.symbols = null;
        }

        public void SaveToFile(string fileName)
        {
            FileNameValidator.ValidateFileName(fileName);
            FileStream stream = null;
            try
            {
                stream = File.Create(fileName);
                new XmlSerializer(typeof(DataSource)).Serialize((Stream) stream, this);
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }
        }

        [XmlIgnore]
        public WealthLab.BarDataScale BarDataScale
        {
            get
            {
                return new WealthLab.BarDataScale(this.Scale, this.BarInterval);
            }
            set
            {
                this.Scale = value.Scale;
                this.BarInterval = value.BarInterval;
            }
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

        public string DSString
        {
            get
            {
                return this.dsString;
            }
            set
            {
                this.dsString = value;
            }
        }

        public bool IsIndexLabDataset
        {
            get
            {
                return ((this.Provider != null) && (this.Provider is IndexStaticProvider));
            }
        }

        public bool IsIntraday
        {
            get
            {
                if ((this.Scale != BarScale.Minute) && (this.Scale != BarScale.Second))
                {
                    return (this.Scale == BarScale.Tick);
                }
                return true;
            }
        }

        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
            }
        }

        [XmlIgnore]
        public StaticDataProvider Provider
        {
            get
            {
                return this.staticDataProvider;
            }
            set
            {
                this.staticDataProvider = value;
            }
        }

        public string ProviderName
        {
            get
            {
                return this.providerName;
            }
            set
            {
                this.providerName = value;
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

        [XmlIgnore]
        public List<string> Symbols
        {
            get
            {
                if (this.symbols == null)
                {
                    this.symbols = new List<string>();
                    this.Provider.PopulateSymbols(this, this.symbols);
                }
                return this.symbols;
            }
        }
    }
}

