namespace WealthLabPro
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class StrategyCenterExecutionItem
    {
        private DateTime nextRun;
        private static int int_0;
        private int updateCount;
        [CompilerGenerated]
        private int counter;
        private List<string> symbolsProcessing = new List<string>();
        private List<WealthLab.Bars> barsList = new List<WealthLab.Bars>();
        private StrategyCenterItem strategyCenterItem_0;

        public StrategyCenterExecutionItem(StrategyCenterItem item, DateTime nextRun)
        {
            this.Counter = int_0++;
            this.strategyCenterItem_0 = item;
            this.nextRun = nextRun;
            if ((item.Symbol != "") && (item.Symbol != null))
            {
                this.symbolsProcessing.Add(item.Symbol);
            }
            else
            {
                foreach (string str in item.DataSet.Symbols)
                {
                    this.symbolsProcessing.Add(str);
                }
            }
        }

        public override string ToString()
        {
            return string.Concat(new object[] { "SCEI(SP=", this.SymbolsProcessing.Count, ",ID=", this.Counter });
        }

        public List<WealthLab.Bars> BarsList
        {
            get
            {
                return this.barsList;
            }
        }

        private int Counter
        {
            [CompilerGenerated]
            get
            {
                return this.counter;
            }
            [CompilerGenerated]
            set
            {
                this.counter = value;
            }
        }

        public StrategyCenterItem Item
        {
            get
            {
                return this.strategyCenterItem_0;
            }
        }

        public DateTime NextRun
        {
            get
            {
                return this.nextRun;
            }
        }

        public string SymbolsNeedingProcessing
        {
            get
            {
                string str = "";
                foreach (string str2 in this.SymbolsProcessingCopied)
                {
                    str = str + str2 + ",";
                }
                return str;
            }
        }

        public List<string> SymbolsProcessing
        {
            get
            {
                return this.symbolsProcessing;
            }
        }

        public List<string> SymbolsProcessingCopied
        {
            get
            {
                lock (this.symbolsProcessing)
                {
                    List<string> list2 = new List<string>();
                    foreach (string str in this.symbolsProcessing)
                    {
                        list2.Add(str);
                    }
                    return list2;
                }
            }
        }

        public int UpdateCount
        {
            get
            {
                return this.updateCount;
            }
            set
            {
                this.updateCount = value;
            }
        }
    }
}

