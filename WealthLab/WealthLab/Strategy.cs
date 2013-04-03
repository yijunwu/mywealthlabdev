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
        private BarDataRange barDataRange_0 = new BarDataRange();
        private BarDataScale barDataScale_0 = new BarDataScale(BarScale.Daily, 0);
        private bool bool_0 = true;
        [CompilerGenerated]
        private bool bool_1;
        private DateTime dateTime_0 = DateTime.Now;
        private DateTime dateTime_1 = DateTime.Now;
        private Dictionary<string, double> dictionary_0 = new Dictionary<string, double>();
        private double double_0 = 100000.0;
        private double double_1 = 1.0;
        private Guid guid_0 = Guid.NewGuid();
        private List<double> list_0 = new List<double>();
        private List<StrategyRule> list_1 = new List<StrategyRule>();
        private List<CombinedStrategyInfo> list_2 = new List<CombinedStrategyInfo>();
        [CompilerGenerated]
        private object object_0;
        private WealthLab.PositionSize positionSize_0 = new WealthLab.PositionSize();
        private WealthLab.StrategyType strategyType_0;
        private string string_0 = "";
        private string string_1 = "";
        private string string_10 = "";
        private string string_11 = "";
        private string string_12 = "";
        private string string_13 = "";
        [CompilerGenerated]
        private string string_14;
        private string string_2 = "";
        private string string_3 = "";
        private string string_4 = "Local";
        private string string_5 = "";
        private string string_6;
        private string string_7 = "";
        private string string_8 = "";
        private string string_9 = "";
        private Type type_0;

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
            if (this.list_0.Count != wealthScript_0.Parameters.Count)
            {
                return false;
            }
            for (int i = 0; i < this.list_0.Count; i++)
            {
                wealthScript_0.Parameters[i].Value = this.list_0[i];
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
                return this.string_12;
            }
            set
            {
                this.string_12 = value;
            }
        }

        public string Author
        {
            get
            {
                return this.string_4;
            }
            set
            {
                this.string_4 = value;
            }
        }

        public string Code
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

        public List<CombinedStrategyInfo> CombinedStrategyChildren
        {
            get
            {
                return this.list_2;
            }
            set
            {
                this.list_2 = value;
            }
        }

        public DateTime CreationDate
        {
            get
            {
                return this.dateTime_0;
            }
            set
            {
                this.dateTime_0 = value;
            }
        }

        public BarDataRange DataRange
        {
            get
            {
                return this.barDataRange_0;
            }
            set
            {
                this.barDataRange_0 = value;
            }
        }

        public BarDataScale DataScale
        {
            get
            {
                return this.barDataScale_0;
            }
            set
            {
                this.barDataScale_0 = value;
            }
        }

        public string DataSetName
        {
            get
            {
                return this.string_8;
            }
            set
            {
                this.string_8 = value;
            }
        }

        public string Description
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

        [XmlIgnore]
        public string FileName
        {
            get
            {
                return this.string_5;
            }
            set
            {
                this.string_5 = value;
            }
        }

        [XmlIgnore]
        public string Folder
        {
            get
            {
                return this.string_7;
            }
            set
            {
                this.string_7 = value;
            }
        }

        public Guid ID
        {
            get
            {
                return this.guid_0;
            }
            set
            {
                this.guid_0 = value;
            }
        }

        public string Indicators
        {
            get
            {
                return this.string_10;
            }
            set
            {
                this.string_10 = value;
            }
        }

        public DateTime LastModified
        {
            get
            {
                return this.dateTime_1;
            }
            set
            {
                this.dateTime_1 = value;
            }
        }

        public double MarginFactor
        {
            get
            {
                return this.double_1;
            }
            set
            {
                this.double_1 = value;
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

        public string NetworkDrivePath
        {
            get
            {
                return this.string_0;
            }
            set
            {
                this.string_0 = value;
                if (this.string_0 == null)
                {
                    this.string_0 = "";
                }
            }
        }

        public string Origin
        {
            get
            {
                return this.string_13;
            }
            set
            {
                this.string_13 = value;
            }
        }

        public string PanelSize
        {
            [CompilerGenerated]
            get
            {
                return this.string_14;
            }
            [CompilerGenerated]
            set
            {
                this.string_14 = value;
            }
        }

        public List<double> ParameterValues
        {
            get
            {
                return this.list_0;
            }
            set
            {
                this.list_0 = value;
            }
        }

        public WealthLab.PositionSize PositionSize
        {
            get
            {
                return this.positionSize_0;
            }
            set
            {
                this.positionSize_0 = value;
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
                return this.string_11;
            }
            set
            {
                this.string_11 = value;
            }
        }

        public List<StrategyRule> Rules
        {
            get
            {
                return this.list_1;
            }
            set
            {
                this.list_1 = value;
            }
        }

        public bool SinglePosition
        {
            get
            {
                return this.bool_0;
            }
            set
            {
                this.bool_0 = value;
            }
        }

        public double StartingEquity
        {
            get
            {
                return this.double_0;
            }
            set
            {
                this.double_0 = value;
            }
        }

        public WealthLab.StrategyType StrategyType
        {
            get
            {
                return this.strategyType_0;
            }
            set
            {
                this.strategyType_0 = value;
            }
        }

        public string Symbol
        {
            get
            {
                return this.string_9;
            }
            set
            {
                this.string_9 = value;
            }
        }

        [XmlIgnore]
        public object Tag
        {
            [CompilerGenerated]
            get
            {
                return this.object_0;
            }
            [CompilerGenerated]
            set
            {
                this.object_0 = value;
            }
        }

        [XmlIgnore]
        public string URL
        {
            get
            {
                return this.string_6;
            }
            set
            {
                this.string_6 = value;
            }
        }

        public bool UsePreferredValues
        {
            [CompilerGenerated]
            get
            {
                return this.bool_1;
            }
            [CompilerGenerated]
            set
            {
                this.bool_1 = value;
            }
        }

        [XmlIgnore]
        public Type WealthScriptType
        {
            get
            {
                return this.type_0;
            }
            set
            {
                this.type_0 = value;
            }
        }
    }
}

