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

        private BarScale barScale_0;
        private int int_0;
        private List<string> list_0;
        private StaticDataProvider staticDataProvider_0;
        private string string_0;
        private string string_1;
        private string string_2;

        public DataSource()
        {
            this.string_0 = "";
            this.string_1 = "";
            this.string_2 = "";
        }

        public DataSource(StaticDataProvider staticDataProvider_1)
        {
            this.string_0 = "";
            this.string_1 = "";
            this.string_2 = "";
            this.staticDataProvider_0 = staticDataProvider_1;
            if (staticDataProvider_1 != null)
            {
                this.string_2 = staticDataProvider_1.GetType().Name;
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
            this.list_0 = null;
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
                return this.int_0;
            }
            set
            {
                this.int_0 = value;
            }
        }

        public string DSString
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
                return this.string_1;
            }
            set
            {
                this.string_1 = value;
            }
        }

        [XmlIgnore]
        public StaticDataProvider Provider
        {
            get
            {
                return this.staticDataProvider_0;
            }
            set
            {
                this.staticDataProvider_0 = value;
            }
        }

        public string ProviderName
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

        [XmlIgnore]
        public List<string> Symbols
        {
            get
            {
                if (this.list_0 == null)
                {
                    this.list_0 = new List<string>();
                    this.Provider.PopulateSymbols(this, this.list_0);
                }
                return this.list_0;
            }
        }
    }
}

