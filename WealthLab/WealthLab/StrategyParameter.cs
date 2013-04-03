namespace WealthLab
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Xml.Serialization;

    public class StrategyParameter
    {
        private double double_0;
        private double double_1;
        private double double_2;
        private double double_3;
        private double double_4;
        private double double_5;
        [CompilerGenerated]
        private object object_0;
        private string string_0;
        private string string_1;
        [CompilerGenerated]
        private string string_2;
        [CompilerGenerated]
        private string string_3;

        public StrategyParameter()
        {
            this.double_5 = double.NaN;
        }

        public StrategyParameter(string name, double value, double start, double stop, double step) : this(name, value, start, stop, step, "")
        {
        }

        public StrategyParameter(string name, double value, double start, double stop, double step, string description)
        {
            this.double_5 = double.NaN;
            this.string_0 = name;
            this.NameEdited = name;
            this.double_0 = value;
            this.DefaultValue = value;
            this.double_1 = start;
            this.double_2 = stop;
            this.double_3 = step;
            this.double_4 = value;
            this.string_1 = description;
        }

        public void Reset()
        {
            this.double_0 = this.double_4;
        }

        public override string ToString()
        {
            return this.Name;
        }

        public double DefaultValue
        {
            get
            {
                if (this.double_5 == double.NaN)
                {
                    return this.double_0;
                }
                return this.double_5;
            }
            set
            {
                this.double_5 = value;
            }
        }

        public string Description
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
        public bool IsInteger
        {
            get
            {
                if (this.Start != ((int) this.Start))
                {
                    return false;
                }
                if (this.Stop != ((int) this.Stop))
                {
                    return false;
                }
                if (this.Step != ((int) this.Step))
                {
                    return false;
                }
                if (this.Value != this.ValueInt)
                {
                    return false;
                }
                return true;
            }
        }

        public string Name
        {
            get
            {
                return this.string_0;
            }
            set
            {
                this.string_0 = value;
                this.NameEdited = this.string_0;
            }
        }

        public string NameEdited
        {
            [CompilerGenerated]
            get
            {
                return this.string_2;
            }
            [CompilerGenerated]
            set
            {
                this.string_2 = value;
            }
        }

        public int NumberOfRuns
        {
            get
            {
                if (this.Step > 0.0)
                {
                    return (int) (((this.Stop - this.Start) / this.Step) + 1.0);
                }
                return 1;
            }
        }

        [XmlIgnore]
        public object OptimizerTag
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

        public double Start
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

        public double Step
        {
            get
            {
                return this.double_3;
            }
            set
            {
                this.double_3 = value;
            }
        }

        public double Stop
        {
            get
            {
                return this.double_2;
            }
            set
            {
                this.double_2 = value;
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

        [XmlIgnore]
        public int ValueInt
        {
            get
            {
                return (int) this.double_0;
            }
            set
            {
                this.double_0 = value;
            }
        }

        public string VariableName
        {
            [CompilerGenerated]
            get
            {
                return this.string_3;
            }
            [CompilerGenerated]
            set
            {
                this.string_3 = value;
            }
        }
    }
}

