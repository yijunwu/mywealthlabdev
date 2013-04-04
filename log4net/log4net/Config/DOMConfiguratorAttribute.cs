namespace log4net.Config
{
    using System;

    [Serializable, Obsolete("Use XmlConfiguratorAttribute instead of DOMConfiguratorAttribute"), AttributeUsage(AttributeTargets.Assembly)]
    public sealed class DOMConfiguratorAttribute : XmlConfiguratorAttribute
    {
    }
}

