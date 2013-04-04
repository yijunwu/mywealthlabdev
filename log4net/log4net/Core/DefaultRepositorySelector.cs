namespace log4net.Core
{
    using log4net.Config;
    using log4net.Plugin;
    using log4net.Repository;
    using log4net.Util;
    using System;
    using System.Collections;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    public class DefaultRepositorySelector : IRepositorySelector
    {
        private const string DefaultRepositoryName = "log4net-default-repository";
        private readonly Hashtable m_alias2repositoryMap = new Hashtable();
        private readonly Hashtable m_assembly2repositoryMap = new Hashtable();
        private readonly Type m_defaultRepositoryType;
        private readonly Hashtable m_name2repositoryMap = new Hashtable();

        public event LoggerRepositoryCreationEventHandler LoggerRepositoryCreatedEvent;

        private event LoggerRepositoryCreationEventHandler m_loggerRepositoryCreatedEvent;

        public DefaultRepositorySelector(Type defaultRepositoryType)
        {
            if (defaultRepositoryType == null)
            {
                throw new ArgumentNullException("defaultRepositoryType");
            }
            if (!typeof(ILoggerRepository).IsAssignableFrom(defaultRepositoryType))
            {
                throw SystemInfo.CreateArgumentOutOfRangeException("defaultRepositoryType", defaultRepositoryType, "Parameter: defaultRepositoryType, Value: [" + defaultRepositoryType + "] out of range. Argument must implement the ILoggerRepository interface");
            }
            this.m_defaultRepositoryType = defaultRepositoryType;
            LogLog.Debug("DefaultRepositorySelector: defaultRepositoryType [" + this.m_defaultRepositoryType + "]");
        }

        public void AliasRepository(string repositoryAlias, ILoggerRepository repositoryTarget)
        {
            if (repositoryAlias == null)
            {
                throw new ArgumentNullException("repositoryAlias");
            }
            if (repositoryTarget == null)
            {
                throw new ArgumentNullException("repositoryTarget");
            }
            lock (this)
            {
                if (this.m_alias2repositoryMap.Contains(repositoryAlias))
                {
                    if (repositoryTarget != ((ILoggerRepository) this.m_alias2repositoryMap[repositoryAlias]))
                    {
                        throw new InvalidOperationException("Repository [" + repositoryAlias + "] is already aliased to repository [" + ((ILoggerRepository) this.m_alias2repositoryMap[repositoryAlias]).Name + "]. Aliases cannot be redefined.");
                    }
                }
                else if (this.m_name2repositoryMap.Contains(repositoryAlias))
                {
                    if (repositoryTarget != ((ILoggerRepository) this.m_name2repositoryMap[repositoryAlias]))
                    {
                        throw new InvalidOperationException("Repository [" + repositoryAlias + "] already exists and cannot be aliased to repository [" + repositoryTarget.Name + "].");
                    }
                }
                else
                {
                    this.m_alias2repositoryMap[repositoryAlias] = repositoryTarget;
                }
            }
        }

        private void ConfigureRepository(Assembly assembly, ILoggerRepository repository)
        {
            Exception exception;
            if (assembly == null)
            {
                throw new ArgumentNullException("assembly");
            }
            if (repository == null)
            {
                throw new ArgumentNullException("repository");
            }
            object[] array = Attribute.GetCustomAttributes(assembly, typeof(ConfiguratorAttribute), false);
            if ((array != null) && (array.Length > 0))
            {
                Array.Sort<object>(array);
                foreach (ConfiguratorAttribute attribute in array)
                {
                    if (attribute != null)
                    {
                        try
                        {
                            attribute.Configure(assembly, repository);
                        }
                        catch (Exception exception1)
                        {
                            exception = exception1;
                            LogLog.Error("DefaultRepositorySelector: Exception calling [" + attribute.GetType().FullName + "] .Configure method.", exception);
                        }
                    }
                }
            }
            if (repository.Name == "log4net-default-repository")
            {
                string appSetting = SystemInfo.GetAppSetting("log4net.Config");
                if ((appSetting != null) && (appSetting.Length > 0))
                {
                    string uriString = null;
                    try
                    {
                        uriString = SystemInfo.ApplicationBaseDirectory;
                    }
                    catch (Exception exception2)
                    {
                        exception = exception2;
                        LogLog.Warn("DefaultRepositorySelector: Exception getting ApplicationBaseDirectory. appSettings log4net.Config path [" + appSetting + "] will be treated as an absolute URI", exception);
                    }
                    Uri configUri = null;
                    try
                    {
                        if (uriString != null)
                        {
                            configUri = new Uri(new Uri(uriString), appSetting);
                        }
                        else
                        {
                            configUri = new Uri(appSetting);
                        }
                    }
                    catch (Exception exception3)
                    {
                        exception = exception3;
                        LogLog.Error("DefaultRepositorySelector: Exception while parsing log4net.Config file path [" + appSetting + "]", exception);
                    }
                    if (configUri != null)
                    {
                        LogLog.Debug("DefaultRepositorySelector: Loading configuration for default repository from AppSettings specified Config URI [" + configUri.ToString() + "]");
                        try
                        {
                            XmlConfigurator.Configure(repository, configUri);
                        }
                        catch (Exception exception4)
                        {
                            exception = exception4;
                            LogLog.Error("DefaultRepositorySelector: Exception calling XmlConfigurator.Configure method with ConfigUri [" + configUri + "]", exception);
                        }
                    }
                }
            }
        }

        public ILoggerRepository CreateRepository(Assembly repositoryAssembly, Type repositoryType)
        {
            return this.CreateRepository(repositoryAssembly, repositoryType, "log4net-default-repository", true);
        }

        public ILoggerRepository CreateRepository(string repositoryName, Type repositoryType)
        {
            if (repositoryName == null)
            {
                throw new ArgumentNullException("repositoryName");
            }
            if (repositoryType == null)
            {
                repositoryType = this.m_defaultRepositoryType;
            }
            lock (this)
            {
                ILoggerRepository repository = null;
                repository = this.m_name2repositoryMap[repositoryName] as ILoggerRepository;
                if (repository != null)
                {
                    throw new LogException("Repository [" + repositoryName + "] is already defined. Repositories cannot be redefined.");
                }
                ILoggerRepository repository2 = this.m_alias2repositoryMap[repositoryName] as ILoggerRepository;
                if (repository2 != null)
                {
                    if (repository2.GetType() == repositoryType)
                    {
                        LogLog.Debug("DefaultRepositorySelector: Aliasing repository [" + repositoryName + "] to existing repository [" + repository2.Name + "]");
                        repository = repository2;
                        this.m_name2repositoryMap[repositoryName] = repository;
                    }
                    else
                    {
                        LogLog.Error("DefaultRepositorySelector: Failed to alias repository [" + repositoryName + "] to existing repository [" + repository2.Name + "]. Requested repository type [" + repositoryType.FullName + "] is not compatible with existing type [" + repository2.GetType().FullName + "]");
                    }
                }
                if (repository == null)
                {
                    LogLog.Debug(string.Concat(new object[] { "DefaultRepositorySelector: Creating repository [", repositoryName, "] using type [", repositoryType, "]" }));
                    repository = (ILoggerRepository) Activator.CreateInstance(repositoryType);
                    repository.Name = repositoryName;
                    this.m_name2repositoryMap[repositoryName] = repository;
                    this.OnLoggerRepositoryCreatedEvent(repository);
                }
                return repository;
            }
        }

        public ILoggerRepository CreateRepository(Assembly repositoryAssembly, Type repositoryType, string repositoryName, bool readAssemblyAttributes)
        {
            if (repositoryAssembly == null)
            {
                throw new ArgumentNullException("repositoryAssembly");
            }
            if (repositoryType == null)
            {
                repositoryType = this.m_defaultRepositoryType;
            }
            lock (this)
            {
                ILoggerRepository repository = this.m_assembly2repositoryMap[repositoryAssembly] as ILoggerRepository;
                if (repository == null)
                {
                    Exception exception;
                    LogLog.Debug("DefaultRepositorySelector: Creating repository for assembly [" + repositoryAssembly + "]");
                    string str = repositoryName;
                    Type type = repositoryType;
                    if (readAssemblyAttributes)
                    {
                        this.GetInfoForAssembly(repositoryAssembly, ref str, ref type);
                    }
                    LogLog.Debug(string.Concat(new object[] { "DefaultRepositorySelector: Assembly [", repositoryAssembly, "] using repository [", str, "] and repository type [", type, "]" }));
                    repository = this.m_name2repositoryMap[str] as ILoggerRepository;
                    if (repository == null)
                    {
                        repository = this.CreateRepository(str, type);
                        if (readAssemblyAttributes)
                        {
                            try
                            {
                                this.LoadAliases(repositoryAssembly, repository);
                                this.LoadPlugins(repositoryAssembly, repository);
                                this.ConfigureRepository(repositoryAssembly, repository);
                            }
                            catch (Exception exception1)
                            {
                                exception = exception1;
                                LogLog.Error("DefaultRepositorySelector: Failed to configure repository [" + str + "] from assembly attributes.", exception);
                            }
                        }
                    }
                    else
                    {
                        LogLog.Debug("DefaultRepositorySelector: repository [" + str + "] already exists, using repository type [" + repository.GetType().FullName + "]");
                        if (readAssemblyAttributes)
                        {
                            try
                            {
                                this.LoadPlugins(repositoryAssembly, repository);
                            }
                            catch (Exception exception2)
                            {
                                exception = exception2;
                                LogLog.Error("DefaultRepositorySelector: Failed to configure repository [" + str + "] from assembly attributes.", exception);
                            }
                        }
                    }
                    this.m_assembly2repositoryMap[repositoryAssembly] = repository;
                }
                return repository;
            }
        }

        public bool ExistsRepository(string repositoryName)
        {
            lock (this)
            {
                return this.m_name2repositoryMap.ContainsKey(repositoryName);
            }
        }

        public ILoggerRepository[] GetAllRepositories()
        {
            lock (this)
            {
                ICollection values = this.m_name2repositoryMap.Values;
                ILoggerRepository[] array = new ILoggerRepository[values.Count];
                values.CopyTo(array, 0);
                return array;
            }
        }

        private void GetInfoForAssembly(Assembly assembly, ref string repositoryName, ref Type repositoryType)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException("assembly");
            }
            try
            {
                LogLog.Debug("DefaultRepositorySelector: Assembly [" + assembly.FullName + "] Loaded From [" + SystemInfo.AssemblyLocationInfo(assembly) + "]");
            }
            catch
            {
            }
            try
            {
                object[] objArray = Attribute.GetCustomAttributes(assembly, typeof(RepositoryAttribute), false);
                if ((objArray == null) || (objArray.Length == 0))
                {
                    LogLog.Debug("DefaultRepositorySelector: Assembly [" + assembly + "] does not have a RepositoryAttribute specified.");
                }
                else
                {
                    if (objArray.Length > 1)
                    {
                        LogLog.Error("DefaultRepositorySelector: Assembly [" + assembly + "] has multiple log4net.Config.RepositoryAttribute assembly attributes. Only using first occurrence.");
                    }
                    RepositoryAttribute attribute = objArray[0] as RepositoryAttribute;
                    if (attribute == null)
                    {
                        LogLog.Error("DefaultRepositorySelector: Assembly [" + assembly + "] has a RepositoryAttribute but it does not!.");
                    }
                    else
                    {
                        if (attribute.Name != null)
                        {
                            repositoryName = attribute.Name;
                        }
                        if (attribute.RepositoryType != null)
                        {
                            if (typeof(ILoggerRepository).IsAssignableFrom(attribute.RepositoryType))
                            {
                                repositoryType = attribute.RepositoryType;
                            }
                            else
                            {
                                LogLog.Error("DefaultRepositorySelector: Repository Type [" + attribute.RepositoryType + "] must implement the ILoggerRepository interface.");
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                LogLog.Error("DefaultRepositorySelector: Unhandled exception in GetInfoForAssembly", exception);
            }
        }

        public ILoggerRepository GetRepository(Assembly repositoryAssembly)
        {
            if (repositoryAssembly == null)
            {
                throw new ArgumentNullException("repositoryAssembly");
            }
            return this.CreateRepository(repositoryAssembly, this.m_defaultRepositoryType);
        }

        public ILoggerRepository GetRepository(string repositoryName)
        {
            if (repositoryName == null)
            {
                throw new ArgumentNullException("repositoryName");
            }
            lock (this)
            {
                ILoggerRepository repository = this.m_name2repositoryMap[repositoryName] as ILoggerRepository;
                if (repository == null)
                {
                    throw new LogException("Repository [" + repositoryName + "] is NOT defined.");
                }
                return repository;
            }
        }

        private void LoadAliases(Assembly assembly, ILoggerRepository repository)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException("assembly");
            }
            if (repository == null)
            {
                throw new ArgumentNullException("repository");
            }
            object[] objArray = Attribute.GetCustomAttributes(assembly, typeof(AliasRepositoryAttribute), false);
            if ((objArray != null) && (objArray.Length > 0))
            {
                foreach (AliasRepositoryAttribute attribute in objArray)
                {
                    try
                    {
                        this.AliasRepository(attribute.Name, repository);
                    }
                    catch (Exception exception)
                    {
                        LogLog.Error("DefaultRepositorySelector: Failed to alias repository [" + attribute.Name + "]", exception);
                    }
                }
            }
        }

        private void LoadPlugins(Assembly assembly, ILoggerRepository repository)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException("assembly");
            }
            if (repository == null)
            {
                throw new ArgumentNullException("repository");
            }
            object[] objArray = Attribute.GetCustomAttributes(assembly, typeof(PluginAttribute), false);
            if ((objArray != null) && (objArray.Length > 0))
            {
                foreach (IPluginFactory factory in objArray)
                {
                    try
                    {
                        repository.PluginMap.Add(factory.CreatePlugin());
                    }
                    catch (Exception exception)
                    {
                        LogLog.Error("DefaultRepositorySelector: Failed to create plugin. Attribute [" + factory.ToString() + "]", exception);
                    }
                }
            }
        }

        protected virtual void OnLoggerRepositoryCreatedEvent(ILoggerRepository repository)
        {
            LoggerRepositoryCreationEventHandler loggerRepositoryCreatedEvent = this.m_loggerRepositoryCreatedEvent;
            if (loggerRepositoryCreatedEvent != null)
            {
                loggerRepositoryCreatedEvent(this, new LoggerRepositoryCreationEventArgs(repository));
            }
        }
    }
}

