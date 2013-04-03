namespace WealthLab
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;

    public class FundamentalItem
    {
        public const string attrCurrentQtr = "current quarter";
        public const string attrFiscalYear = "fiscal year";
        public const string attrMultiplier = "multiplier";
        public const string attrObserveDate = "observation date";
        public const string attrPeriod = "period";
        public const string attrPeriodAnnual = "annual";
        public const string attrPeriodMonthly = "monthly";
        public const string attrPeriodQuarterly = "quarterly";
        public const string attrPeriodSemiAnnual = "semi-annual";
        public const string attrPeriodWeekly = "weekly";
        public const string attrPeriodYearly = "yearly";
        public const string attrUnits = "units";
        private static Bitmap bitmap_0 = Properties.Resources.FundamentalItem;
        private DateTime dateTime_0;
        private Dictionary<string, string> dictionary_0;
        private double double_0;
        private int int_0 = -1;
        private string string_0;

        public FundamentalItem(string name)
        {
            this.string_0 = name;
        }

        public virtual string FormatValue()
        {
            if (this.Value < 0.01)
            {
                return (this.string_0 + ": " + this.Value.ToString());
            }
            return (this.string_0 + ": " + this.Value.ToString("N2"));
        }

        public string GetDetail(string detailName)
        {
            if ((this.dictionary_0 != null) && this.dictionary_0.ContainsKey(detailName))
            {
                return this.dictionary_0[detailName];
            }
            return "";
        }

        internal void method_0(BinaryWriter binaryWriter_0)
        {
            binaryWriter_0.Write(this.dateTime_0.Ticks);
            binaryWriter_0.Write(this.double_0);
            if (this.dictionary_0 == null)
            {
                binaryWriter_0.Write(0);
            }
            else
            {
                binaryWriter_0.Write(this.dictionary_0.Count);
                foreach (KeyValuePair<string, string> pair in this.dictionary_0)
                {
                    binaryWriter_0.Write(pair.Key);
                    binaryWriter_0.Write(pair.Value);
                }
            }
        }

        internal void method_1(BinaryReader binaryReader_0)
        {
            this.dateTime_0 = new DateTime(binaryReader_0.ReadInt64());
            this.double_0 = binaryReader_0.ReadDouble();
            int num = binaryReader_0.ReadInt32();
            if (num == 0)
            {
                this.dictionary_0 = null;
            }
            else
            {
                this.dictionary_0 = new Dictionary<string, string>();
                for (int i = 0; i < num; i++)
                {
                    string key = binaryReader_0.ReadString();
                    string str2 = binaryReader_0.ReadString();
                    this.dictionary_0.Add(key, str2);
                }
            }
        }

        public void SetDetail(string detailName, string value)
        {
            if (this.dictionary_0 == null)
            {
                this.dictionary_0 = new Dictionary<string, string>();
            }
            this.dictionary_0[detailName] = value;
        }

        public int Bar
        {
            get
            {
                return this.int_0;
            }
            internal set
            {
                this.int_0 = value;
            }
        }

        public DateTime Date
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

        public virtual Bitmap Glyph
        {
            get
            {
                return bitmap_0;
            }
        }

        public string Name
        {
            get
            {
                return this.string_0;
            }
        }

        public double Value
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
    }
}

