namespace WealthLab
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;

    public abstract class IndexDefinition
    {
        private IDataHost idataHost_0;

        protected IndexDefinition()
        {
        }

        public virtual void ClearUserInterface()
        {
        }

        public abstract void SetIndexValues(List<Bars> bars, List<int> barNumbers, DateTime dateTime_0, Bars indexToDate);
        public override string ToString()
        {
            return this.FriendlyName;
        }

        public virtual bool ValidateUserInput(ref string errMsg)
        {
            errMsg = "";
            return true;
        }

        public IDataHost DataHost
        {
            get
            {
                return this.idataHost_0;
            }
            set
            {
                this.idataHost_0 = value;
            }
        }

        public abstract string Description { get; }

        public abstract string FriendlyName { get; }

        public virtual bool NeedsSeparateUIForParameters
        {
            get
            {
                return true;
            }
        }

        public virtual string ParameterString
        {
            get
            {
                return "";
            }
            set
            {
            }
        }

        public virtual UserControl ParameterUserInterface
        {
            get
            {
                return null;
            }
        }

        public abstract string Prefix { get; }

        public virtual bool SupportsParameters
        {
            get
            {
                return false;
            }
        }
    }
}

