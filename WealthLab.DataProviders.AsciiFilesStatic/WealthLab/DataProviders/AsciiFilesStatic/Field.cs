namespace WealthLab.DataProviders.AsciiFilesStatic
{
    using System;

    [Serializable]
    public class Field
    {
        private string _name;
        public FieldType Type;

        public Field()
        {
            this.Type = FieldType.Unknow;
            this._name = "";
        }

        public Field(FieldType type) : this()
        {
            this.Type = type;
        }

        public Field(FieldType type, string name) : this(type)
        {
            this._name = name;
        }

        public override string ToString()
        {
            if (this.Type == FieldType.OpenInterest)
            {
                return "Open Interest";
            }
            if (this.Type == FieldType.SecurityName)
            {
                return "Security Name";
            }
            if (this.Type == FieldType.Custom)
            {
                return this._name;
            }
            if (this.Type == FieldType.Date)
            {
                return "Date (or Date with Time)";
            }
            return Enum.GetName(typeof(FieldType), this.Type);
        }

        public string Name
        {
            get
            {
                return this.ToString();
            }
            set
            {
                this._name = value;
            }
        }
    }
}

