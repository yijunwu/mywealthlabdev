namespace log4net.Util
{
    using System;
    using System.Reflection;
    using System.Runtime.Remoting.Messaging;

    public sealed class LogicalThreadContextProperties : ContextPropertiesBase
    {
        internal LogicalThreadContextProperties()
        {
        }

        public void Clear()
        {
            PropertiesDictionary properties = this.GetProperties(false);
            if (properties != null)
            {
                properties.Clear();
            }
        }

        internal PropertiesDictionary GetProperties(bool create)
        {
            PropertiesDictionary data = (PropertiesDictionary) CallContext.GetData("log4net.Util.LogicalThreadContextProperties");
            if ((data == null) && create)
            {
                data = new PropertiesDictionary();
                CallContext.SetData("log4net.Util.LogicalThreadContextProperties", data);
            }
            return data;
        }

        public void Remove(string key)
        {
            PropertiesDictionary properties = this.GetProperties(false);
            if (properties != null)
            {
                properties.Remove(key);
            }
        }

        public override object this[string key]
        {
            get
            {
                PropertiesDictionary properties = this.GetProperties(false);
                if (properties != null)
                {
                    return properties[key];
                }
                return null;
            }
            set
            {
                this.GetProperties(true)[key] = value;
            }
        }
    }
}

