namespace WealthLabPro
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class StrategyCenterExecutionItem
    {
        private DateTime dateTime_0;
        private static int int_0;
        private int int_1;
        [CompilerGenerated]
        private int int_2;
        private List<string> list_0 = new List<string>();
        private List<Bars> list_1 = new List<Bars>();
        private StrategyCenterItem strategyCenterItem_0;

        public StrategyCenterExecutionItem(StrategyCenterItem item, DateTime nextRun)
        {
            this.Counter = int_0++;
            this.strategyCenterItem_0 = item;
            this.dateTime_0 = nextRun;
            if ((item.Symbol != "") && (item.Symbol != null))
            {
                this.list_0.Add(item.Symbol);
            }
            else
            {
                foreach (string str in item.DataSet.Symbols)
                {
                    this.list_0.Add(str);
                }
            }
        }

        public override string ToString()
        {
            return string.Concat(new object[] { "SCEI(SP=", this.SymbolsProcessing.Count, ",ID=", this.Counter });
        }

        public List<Bars> BarsList
        {
            get
            {
                return this.list_1;
            }
        }

        private int Counter
        {
            [CompilerGenerated]
            get
            {
                return this.int_2;
            }
            [CompilerGenerated]
            set
            {
                this.int_2 = value;
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
                return this.dateTime_0;
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
                return this.list_0;
            }
        }

        public List<string> SymbolsProcessingCopied
        {
            get
            {
                lock (this.list_0)
                {
                    List<string> list2 = new List<string>();
                    foreach (string str in this.list_0)
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
                return this.int_1;
            }
            set
            {
                this.int_1 = value;
            }
        }
    }
}

