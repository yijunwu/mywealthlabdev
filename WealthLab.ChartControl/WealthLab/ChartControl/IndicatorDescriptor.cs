namespace WealthLab.ChartControl
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using WealthLab;

    public class IndicatorDescriptor
    {
        private bool bool_0;
        private bool bool_1;
        private bool bool_2;
        private System.Drawing.Color color_0;
        private System.Drawing.Color color_1 = System.Drawing.Color.Empty;
        private System.Drawing.Color color_2 = System.Drawing.Color.Empty;
        private System.Drawing.Color color_3 = System.Drawing.Color.Empty;
        private double double_0;
        private double double_1;
        private int int_0;
        private LineStyle lineStyle_0;
        private object[] object_0;
        private WealthLab.PlottedIndicator plottedIndicator_0;
        private WealthLab.PlottedIndicator plottedIndicator_1;
        private string string_0;
        private string string_1 = "";
        private string string_2 = "";
        private Type type_0;
        private Type type_1;

        public IndicatorDescriptor(int paramCount)
        {
            this.object_0 = new object[paramCount];
        }

        internal void method_0(BinaryReader binaryReader_0, List<Type> list_0)
        {
            binaryReader_0.ReadDouble();
            this.IndicatorType = null;
            string str = binaryReader_0.ReadString();
            using (List<Type>.Enumerator enumerator = list_0.GetEnumerator())
            {
                Type current;
                while (enumerator.MoveNext())
                {
                    current = enumerator.Current;
                    if (current.Name == str)
                    {
                        ///goto  Label_0042;  ///WYJ fix, simplify the flow
                        this.IndicatorType = current;
                        break;
                    }
                }
            }
            if ((this.IndicatorType == null) && (str != "*Fundamental*"))
            {
                throw new InvalidDataException("Type not found in IndicatorDescriptor Read: " + str);
            }
            this.Color = System.Drawing.Color.FromArgb(binaryReader_0.ReadInt32());
            this.Style = (LineStyle) binaryReader_0.ReadInt32();
            this.Width = binaryReader_0.ReadInt32();
            int num2 = binaryReader_0.ReadInt32();
            this.PaneDescription = binaryReader_0.ReadString();
            this.FundamentalItemName = binaryReader_0.ReadString();
            this.PlotBandPairIndicator = binaryReader_0.ReadBoolean();
            this.FillBand = binaryReader_0.ReadBoolean();
            this.BandFillColor = System.Drawing.Color.FromArgb(binaryReader_0.ReadInt32());
            this.PlotOscillator = binaryReader_0.ReadBoolean();
            this.OverboughtLevel = binaryReader_0.ReadDouble();
            this.OverboughtColor = System.Drawing.Color.FromArgb(binaryReader_0.ReadInt32());
            this.OversoldLevel = binaryReader_0.ReadDouble();
            this.OversoldColor = System.Drawing.Color.FromArgb(binaryReader_0.ReadInt32());
            this.Parameters = new object[num2];
            for (int i = 0; i < num2; i++)
            {
                string str2 = binaryReader_0.ReadString();
                if (str2 == "CDS")
                {
                    this.Parameters[i] = (CoreDataSeries) binaryReader_0.ReadInt32();
                }
                else if (str2 == "ENUM")
                {
                    this.Parameters[i] = binaryReader_0.ReadString();
                }
                else if (str2 == "IDS")
                {
                    this.Parameters[i] = new IndicatorDescriptionString(binaryReader_0.ReadString());
                }
                else if (str2 == "BDS")
                {
                    this.Parameters[i] = new BarsDescriptorString(binaryReader_0.ReadString());
                }
                else if (str2 == "Int32")
                {
                    this.Parameters[i] = binaryReader_0.ReadInt32();
                }
                else if (str2 == "Double")
                {
                    this.Parameters[i] = binaryReader_0.ReadDouble();
                }
                else if (str2 == "String")
                {
                    this.Parameters[i] = binaryReader_0.ReadString();
                }
                else if (str2 == "Boolean")
                {
                    this.Parameters[i] = binaryReader_0.ReadBoolean();
                }
                else
                {
                    if (!(str2 == "DateTime"))
                    {
                        throw new InvalidOperationException("Invalid Parameter type in IndicatorDescriptor Read: " + str2);
                    }
                    this.Parameters[i] = new DateTime(binaryReader_0.ReadInt64());
                }
            }
        }

        internal void method_1(BinaryWriter binaryWriter_0)
        {
            binaryWriter_0.Write((double) 1.0);
            if (this.IndicatorType == null)
            {
                binaryWriter_0.Write("*Fundamental*");
            }
            else
            {
                binaryWriter_0.Write(this.IndicatorType.Name);
            }
            binaryWriter_0.Write(this.Color.ToArgb());
            binaryWriter_0.Write((int) this.Style);
            binaryWriter_0.Write(this.Width);
            binaryWriter_0.Write(this.Parameters.Length);
            binaryWriter_0.Write(this.PaneDescription);
            binaryWriter_0.Write(this.FundamentalItemName);
            binaryWriter_0.Write(this.PlotBandPairIndicator);
            binaryWriter_0.Write(this.FillBand);
            binaryWriter_0.Write(this.BandFillColor.ToArgb());
            binaryWriter_0.Write(this.PlotOscillator);
            binaryWriter_0.Write(this.OverboughtLevel);
            binaryWriter_0.Write(this.OverboughtColor.ToArgb());
            binaryWriter_0.Write(this.OversoldLevel);
            binaryWriter_0.Write(this.OversoldColor.ToArgb());
            foreach (object obj2 in this.Parameters)
            {
                if (obj2 is CoreDataSeries)
                {
                    binaryWriter_0.Write("CDS");
                    CoreDataSeries series = (CoreDataSeries) obj2;
                    binaryWriter_0.Write((int) series);
                }
                else if (obj2 is Enum)
                {
                    binaryWriter_0.Write("ENUM");
                    binaryWriter_0.Write((obj2 as Enum).ToString());
                }
                else if (obj2 is IndicatorDescriptionString)
                {
                    binaryWriter_0.Write("IDS");
                    IndicatorDescriptionString str = (IndicatorDescriptionString) obj2;
                    binaryWriter_0.Write(str.Description);
                }
                else if (obj2 is BarsDescriptorString)
                {
                    binaryWriter_0.Write("BDS");
                    BarsDescriptorString str3 = (BarsDescriptorString) obj2;
                    binaryWriter_0.Write(str3.Description);
                }
                else if (obj2 is int)
                {
                    binaryWriter_0.Write("Int32");
                    int num3 = (int) obj2;
                    binaryWriter_0.Write(num3);
                }
                else if (obj2 is double)
                {
                    binaryWriter_0.Write("Double");
                    double num = (double) obj2;
                    binaryWriter_0.Write(num);
                }
                else if (obj2 is string)
                {
                    binaryWriter_0.Write("String");
                    string str2 = (string) obj2;
                    binaryWriter_0.Write(str2);
                }
                else if (obj2 is bool)
                {
                    binaryWriter_0.Write("Boolean");
                    bool flag = (bool) obj2;
                    binaryWriter_0.Write(flag);
                }
                else
                {
                    if (!(obj2 is DateTime))
                    {
                        throw new InvalidDataException("Invalid argument type to IndicatorDescriptor Write: " + obj2.GetType().Name);
                    }
                    binaryWriter_0.Write("DateTime");
                    DateTime time = (DateTime) obj2;
                    binaryWriter_0.Write(time.Ticks);
                }
            }
        }

        public System.Drawing.Color BandFillColor
        {
            get
            {
                return this.color_1;
            }
            set
            {
                this.color_1 = value;
            }
        }

        public Type BandPairIndicatorType
        {
            get
            {
                return this.type_1;
            }
            set
            {
                this.type_1 = value;
            }
        }

        public System.Drawing.Color Color
        {
            get
            {
                return this.color_0;
            }
            set
            {
                this.color_0 = value;
            }
        }

        public bool FillBand
        {
            get
            {
                return this.bool_1;
            }
            set
            {
                this.bool_1 = value;
            }
        }

        public string FundamentalItemName
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

        public Type IndicatorType
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

        public bool IsFundamental
        {
            get
            {
                return (this.string_2 != "");
            }
        }

        internal string LinkDescription
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

        public System.Drawing.Color OverboughtColor
        {
            get
            {
                return this.color_2;
            }
            set
            {
                this.color_2 = value;
            }
        }

        public double OverboughtLevel
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

        public System.Drawing.Color OversoldColor
        {
            get
            {
                return this.color_3;
            }
            set
            {
                this.color_3 = value;
            }
        }

        public double OversoldLevel
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

        public string PaneDescription
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

        public object[] Parameters
        {
            get
            {
                return this.object_0;
            }
            set
            {
                this.object_0 = value;
            }
        }

        public bool PlotBandPairIndicator
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

        public bool PlotOscillator
        {
            get
            {
                return this.bool_2;
            }
            set
            {
                this.bool_2 = value;
            }
        }

        public WealthLab.PlottedIndicator PlottedIndicator
        {
            get
            {
                return this.plottedIndicator_0;
            }
            set
            {
                this.plottedIndicator_0 = value;
            }
        }

        public WealthLab.PlottedIndicator PlottedPartner
        {
            get
            {
                return this.plottedIndicator_1;
            }
            set
            {
                this.plottedIndicator_1 = value;
            }
        }

        public LineStyle Style
        {
            get
            {
                return this.lineStyle_0;
            }
            set
            {
                this.lineStyle_0 = value;
            }
        }

        public int Width
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
    }
}

