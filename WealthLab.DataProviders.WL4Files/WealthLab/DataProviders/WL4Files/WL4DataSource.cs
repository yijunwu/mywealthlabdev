namespace WealthLab.DataProviders.WL4Files
{
    using System;

    [Serializable]
    internal class WL4DataSource : IComparable
    {
        private string _details;
        private int _id;
        private string _location;
        private string _name;
        private string _type;

        int IComparable.CompareTo(object object_0)
        {
            return this.Name.CompareTo((object_0 as WL4DataSource).Name);
        }

        string object.ToString()
        {
            return this._name;
        }

        public string Details
        {
            get
            {
                return this._details;
            }
            set
            {
                this._details = value;
            }
        }

        public int ID
        {
            get
            {
                return this._id;
            }
            set
            {
                this._id = value;
            }
        }

        public string Location
        {
            get
            {
                return this._location;
            }
            set
            {
                this._location = value;
            }
        }

        public string Name
        {
            get
            {
                return this._name;
            }
            set
            {
                this._name = value;
            }
        }

        public string Type
        {
            get
            {
                return this._type;
            }
            set
            {
                this._type = value;
            }
        }
    }
}

