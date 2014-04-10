namespace WealthLab
{
    using Fidelity.Components;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Xml.Serialization;

    [XmlRoot(ElementName="Strategy", IsNullable=false)]
    public class Strategy
    {
        private BarDataRange barDataRange = new BarDataRange();
        private BarDataScale barDataScale = new BarDataScale(BarScale.Daily, 0);
        private bool singlePosition = true;
        [CompilerGenerated]
        private bool usePreferredValues;
        private DateTime creationDate = DateTime.Now;
        private DateTime lastModified = DateTime.Now;
        private Dictionary<string, double> dictionary_0 = new Dictionary<string, double>();
        private double startingEquity = 100000.0;
        private double marginFactor = 1.0;
        private Guid guid = Guid.NewGuid();
        private List<double> parameterValues = new List<double>();
        private List<StrategyRule> rules = new List<StrategyRule>();
        private List<CombinedStrategyInfo> combinedStrategyChildren = new List<CombinedStrategyInfo>();
        [CompilerGenerated]
        private object tag;
        private WealthLab.PositionSize positionSize = new WealthLab.PositionSize();
        private WealthLab.StrategyType strategyType;
        private string networkDrivePath = "";
        private string name = "";
        private string indicators = "";
        private string references = "";
        private string accountNumber = "";
        private string origin = "";
        [CompilerGenerated]
        private string panelSize;
        private string code = "";
        private string description = "";
        private string author = "Local";
        private string fileName = "";
        private string url;
        private string folder = "";
        private string dataSetName = "";
        private string symbol = "";
        private Type wealthScriptType;

        public static Strategy FromFile(string fileName)
        {
            FileStream stream = null;
            Strategy strategy;
            try
            {
                stream = File.Open(fileName, FileMode.Open, FileAccess.Read);
                XmlSerializer serializer = new XmlSerializer(typeof(Strategy));
                strategy = (Strategy) serializer.Deserialize(stream);
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }
            return strategy;
        }

        public void LoadPreferredValues(string symbol, WealthScript wealthScript_0)
        {
            if (!this.RestoreSavedParameterValues(wealthScript_0))
            {
                wealthScript_0.RestoreParameterDefaults();
            }
            foreach (StrategyParameter parameter in wealthScript_0.Parameters)
            {
                string key = symbol + "|" + parameter.Name;
                if (this.dictionary_0.ContainsKey(key))
                {
                    parameter.Value = this.dictionary_0[key];
                }
            }
        }

        public bool RestoreSavedParameterValues(WealthScript wealthScript_0)
        {
            if (this.parameterValues.Count != wealthScript_0.Parameters.Count)
            {
                return false;
            }
            for (int i = 0; i < this.parameterValues.Count; i++)
            {
                wealthScript_0.Parameters[i].Value = this.parameterValues[i];
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
                new XmlSerializer(typeof(Strategy)).Serialize((Stream) stream, this);
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }
        }

        public void StorePreferredValues(string symbol, WealthScript wealthScript_0)
        {
            foreach (StrategyParameter parameter in wealthScript_0.Parameters)
            {
                this.dictionary_0[symbol + "|" + parameter.Name] = parameter.Value;
            }
        }

        public void StorePreferredValues(string symbol, WealthScript wealthScript_0, List<double> values)
        {
            for (int i = 0; i < values.Count; i++)
            {
                StrategyParameter parameter = wealthScript_0.Parameters[i];
                this.dictionary_0[symbol + "|" + parameter.Name] = values[i];
            }
        }

        public override string ToString()
        {
            return this.Name;
        }

        public string AccountNumber
        {
            get
            {
                return this.accountNumber;
            }
            set
            {
                this.accountNumber = value;
            }
        }

        public string Author
        {
            get
            {
                return this.author;
            }
            set
            {
                this.author = value;
            }
        }

        public string Code
        {
            get
            {
                return this.code;
            }
            set
            {
                this.code = value;
            }
        }

        public List<CombinedStrategyInfo> CombinedStrategyChildren
        {
            get
            {
                return this.combinedStrategyChildren;
            }
            set
            {
                this.combinedStrategyChildren = value;
            }
        }

        public DateTime CreationDate
        {
            get
            {
                return this.creationDate;
            }
            set
            {
                this.creationDate = value;
            }
        }

        public BarDataRange DataRange
        {
            get
            {
                return this.barDataRange;
            }
            set
            {
                this.barDataRange = value;
            }
        }

        public BarDataScale DataScale
        {
            get
            {
                return this.barDataScale;
            }
            set
            {
                this.barDataScale = value;
            }
        }

        public string DataSetName
        {
            get
            {
                return this.dataSetName;
            }
            set
            {
                this.dataSetName = value;
            }
        }

        public string Description
        {
            get
            {
                return this.description;
            }
            set
            {
                this.description = value;
            }
        }

        [XmlIgnore]
        public string FileName
        {
            get
            {
                return this.fileName;
            }
            set
            {
                this.fileName = value;
            }
        }

        [XmlIgnore]
        public string Folder
        {
            get
            {
                return this.folder;
            }
            set
            {
                this.folder = value;
            }
        }

        public Guid ID
        {
            get
            {
                return this.guid;
            }
            set
            {
                this.guid = value;
            }
        }

        public string Indicators
        {
            get
            {
                return this.indicators;
            }
            set
            {
                this.indicators = value;
            }
        }

        public DateTime LastModified
        {
            get
            {
                return this.lastModified;
            }
            set
            {
                this.lastModified = value;
            }
        }

        public double MarginFactor
        {
            get
            {
                return this.marginFactor;
            }
            set
            {
                this.marginFactor = value;
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

        public string NetworkDrivePath
        {
            get
            {
                return this.networkDrivePath;
            }
            set
            {
                this.networkDrivePath = value;
                if (this.networkDrivePath == null)
                {
                    this.networkDrivePath = "";
                }
            }
        }

        public string Origin
        {
            get
            {
                return this.origin;
            }
            set
            {
                this.origin = value;
            }
        }

        public string PanelSize
        {
            [CompilerGenerated]
            get
            {
                return this.panelSize;
            }
            [CompilerGenerated]
            set
            {
                this.panelSize = value;
            }
        }

        public List<double> ParameterValues
        {
            get
            {
                return this.parameterValues;
            }
            set
            {
                this.parameterValues = value;
            }
        }

        public WealthLab.PositionSize PositionSize
        {
            get
            {
                return this.positionSize;
            }
            set
            {
                this.positionSize = value;
            }
        }

        public string PreferredValues
        {
            get
            {
                string str = "";
                foreach (KeyValuePair<string, double> pair in this.dictionary_0)
                {
                    object obj2 = str;
                    str = string.Concat(new object[] { obj2, pair.Key, "|", pair.Value, "|" });
                }
                return str;
            }
            set
            {
                this.dictionary_0.Clear();
                if (value != null)
                {
                    string[] strArray = value.Split(new char[] { '|' });
                    int num = 0;
                    while (num < strArray.Length)
                    {
                        string str = strArray[num++];
                        if (str.Trim() == "")
                        {
                            return;
                        }
                        str = str + "|" + strArray[num++];
                        double num2 = double.Parse(strArray[num++]);
                        this.dictionary_0[str] = num2;
                    }
                }
            }
        }

        [XmlIgnore]
        public List<string> PreferredValueSymbols
        {
            get
            {
                List<string> list = new List<string>();
                foreach (string str in this.dictionary_0.Keys)
                {
                    string[] strArray = str.Split(new char[] { '|' });
                    if (!list.Contains(strArray[0]))
                    {
                        list.Add(strArray[0]);
                    }
                }
                return list;
            }
        }

        public string References
        {
            get
            {
                return this.references;
            }
            set
            {
                this.references = value;
            }
        }

        public List<StrategyRule> Rules
        {
            get
            {
                return this.rules;
            }
            set
            {
                this.rules = value;
            }
        }

        public bool SinglePosition
        {
            get
            {
                return this.singlePosition;
            }
            set
            {
                this.singlePosition = value;
            }
        }

        public double StartingEquity
        {
            get
            {
                return this.startingEquity;
            }
            set
            {
                this.startingEquity = value;
            }
        }

        public WealthLab.StrategyType StrategyType
        {
            get
            {
                return this.strategyType;
            }
            set
            {
                this.strategyType = value;
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

        [XmlIgnore]
        public object Tag
        {
            [CompilerGenerated]
            get
            {
                return this.tag;
            }
            [CompilerGenerated]
            set
            {
                this.tag = value;
            }
        }

        [XmlIgnore]
        public string URL
        {
            get
            {
                return this.url;
            }
            set
            {
                this.url = value;
            }
        }

        public bool UsePreferredValues
        {
            [CompilerGenerated]
            get
            {
                return this.usePreferredValues;
            }
            [CompilerGenerated]
            set
            {
                this.usePreferredValues = value;
            }
        }

        [XmlIgnore]
        public Type WealthScriptType
        {
            get
            {
                return this.wealthScriptType;
            }
            set
            {
                this.wealthScriptType = value;
            }
        }
    }
}

