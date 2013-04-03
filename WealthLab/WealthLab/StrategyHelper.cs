namespace WealthLab
{
    using System;
    using System.Collections.Generic;

    public abstract class StrategyHelper
    {
        private List<StrategyParameter> list_0;

        protected StrategyHelper()
        {
        }

        public abstract string Author { get; }

        public abstract DateTime CreationDate { get; }

        public abstract string Description { get; }

        public abstract Guid ID { get; }

        public abstract DateTime LastModifiedDate { get; }

        public abstract string Name { get; }

        public List<StrategyParameter> Parameters
        {
            get
            {
                if (this.list_0 == null)
                {
                    this.list_0 = new List<StrategyParameter>();
                    if (this.WealthScriptType != null)
                    {
                        WealthScript script = Activator.CreateInstance(this.WealthScriptType) as WealthScript;
                        foreach (StrategyParameter parameter in script.Parameters)
                        {
                            this.list_0.Add(parameter);
                        }
                    }
                }
                return this.list_0;
            }
        }

        public virtual string URL
        {
            get
            {
                return "";
            }
        }

        public abstract Type WealthScriptType { get; }
    }
}

