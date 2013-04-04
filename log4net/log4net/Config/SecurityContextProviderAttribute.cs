namespace log4net.Config
{
    using log4net.Core;
    using log4net.Repository;
    using log4net.Util;
    using System;
    using System.Reflection;

    [Serializable, AttributeUsage(AttributeTargets.Assembly)]
    public sealed class SecurityContextProviderAttribute : ConfiguratorAttribute
    {
        private Type m_providerType;

        public SecurityContextProviderAttribute(Type providerType) : base(100)
        {
            this.m_providerType = null;
            this.m_providerType = providerType;
        }

        public override void Configure(Assembly sourceAssembly, ILoggerRepository targetRepository)
        {
            if (this.m_providerType == null)
            {
                LogLog.Error("SecurityContextProviderAttribute: Attribute specified on assembly [" + sourceAssembly.FullName + "] with null ProviderType.");
            }
            else
            {
                LogLog.Debug("SecurityContextProviderAttribute: Creating provider of type [" + this.m_providerType.FullName + "]");
                SecurityContextProvider provider = Activator.CreateInstance(this.m_providerType) as SecurityContextProvider;
                if (provider == null)
                {
                    LogLog.Error("SecurityContextProviderAttribute: Failed to create SecurityContextProvider instance of type [" + this.m_providerType.Name + "].");
                }
                else
                {
                    SecurityContextProvider.DefaultProvider = provider;
                }
            }
        }

        public Type ProviderType
        {
            get
            {
                return this.m_providerType;
            }
            set
            {
                this.m_providerType = value;
            }
        }
    }
}

