namespace WealthLab
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Xml.Serialization;

    public class StrategyParameter
    {
        private double doubleValue;
        private double start;
        private double stop;
        private double step;
        private double double_4;
        private double defaultValue;
        [CompilerGenerated]
        private object optimizerTag;
        private string name;
        private string description;
        [CompilerGenerated]
        private string nameEdited;
        [CompilerGenerated]
        private string variableName;

        public StrategyParameter()
        {
            this.defaultValue = double.NaN;
        }

        public StrategyParameter(string name, double value, double start, double stop, double step) : this(name, value, start, stop, step, "")
        {
        }

        public StrategyParameter(string name, double value, double start, double stop, double step, string description)
        {
            this.defaultValue = double.NaN;
            this.name = name;
            this.NameEdited = name;
            this.doubleValue = value;
            this.DefaultValue = value;
            this.start = start;
            this.stop = stop;
            this.step = step;
            this.double_4 = value;
            this.description = description;
        }

        public void Reset()
        {
            this.doubleValue = this.double_4;
        }

        public override string ToString()
        {
            return this.Name;
        }

        public double DefaultValue
        {
            get
            {
                if (this.defaultValue == double.NaN)
                {
                    return this.doubleValue;
                }
                return this.defaultValue;
            }
            set
            {
                this.defaultValue = value;
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
                return this.name;
            }
            set
            {
                this.name = value;
                this.NameEdited = this.name;
            }
        }

        public string NameEdited
        {
            [CompilerGenerated]
            get
            {
                return this.nameEdited;
            }
            [CompilerGenerated]
            set
            {
                this.nameEdited = value;
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
                return this.optimizerTag;
            }
            [CompilerGenerated]
            set
            {
                this.optimizerTag = value;
            }
        }

        public double Start
        {
            get
            {
                return this.start;
            }
            set
            {
                this.start = value;
            }
        }

        public double Step
        {
            get
            {
                return this.step;
            }
            set
            {
                this.step = value;
            }
        }

        public double Stop
        {
            get
            {
                return this.stop;
            }
            set
            {
                this.stop = value;
            }
        }

        public double Value
        {
            get
            {
                return this.doubleValue;
            }
            set
            {
                this.doubleValue = value;
            }
        }

        [XmlIgnore]
        public int ValueInt
        {
            get
            {
                return (int) this.doubleValue;
            }
            set
            {
                this.doubleValue = value;
            }
        }

        public string VariableName
        {
            [CompilerGenerated]
            get
            {
                return this.variableName;
            }
            [CompilerGenerated]
            set
            {
                this.variableName = value;
            }
        }
    }
}

