namespace log4net.Config
{
    using log4net;
    using log4net.Repository;
    using log4net.Util;
    using System;
    using System.Configuration;
    using System.IO;
    using System.Net;
    using System.Reflection;
    using System.Threading;
    using System.Xml;

    public sealed class XmlConfigurator
    {
        private XmlConfigurator()
        {
        }

        public static void Configure()
        {
            Configure(LogManager.GetRepository(Assembly.GetCallingAssembly()));
        }

        public static void Configure(ILoggerRepository repository)
        {
            LogLog.Debug("XmlConfigurator: configuring repository [" + repository.Name + "] using .config file section");
            try
            {
                LogLog.Debug("XmlConfigurator: Application config file is [" + SystemInfo.ConfigurationFileLocation + "]");
            }
            catch
            {
                LogLog.Debug("XmlConfigurator: Application config file location unknown");
            }
            try
            {
                XmlElement section = null;
                section = ConfigurationManager.GetSection("log4net") as XmlElement;
                if (section == null)
                {
                    LogLog.Error("XmlConfigurator: Failed to find configuration section 'log4net' in the application's .config file. Check your .config file for the <log4net> and <configSections> elements. The configuration section should look like: <section name=\"log4net\" type=\"log4net.Config.Log4NetConfigurationSectionHandler,log4net\" />");
                }
                else
                {
                    ConfigureFromXml(repository, section);
                }
            }
            catch (ConfigurationException exception)
            {
                if (exception.BareMessage.IndexOf("Unrecognized element") >= 0)
                {
                    LogLog.Error("XmlConfigurator: Failed to parse config file. Check your .config file is well formed XML.", exception);
                }
                else
                {
                    LogLog.Error("XmlConfigurator: Failed to parse config file. Is the <configSections> specified as: " + ("<section name=\"log4net\" type=\"log4net.Config.Log4NetConfigurationSectionHandler," + Assembly.GetExecutingAssembly().FullName + "\" />"), exception);
                }
            }
        }

        public static void Configure(FileInfo configFile)
        {
            Configure(LogManager.GetRepository(Assembly.GetCallingAssembly()), configFile);
        }

        public static void Configure(Stream configStream)
        {
            Configure(LogManager.GetRepository(Assembly.GetCallingAssembly()), configStream);
        }

        public static void Configure(Uri configUri)
        {
            Configure(LogManager.GetRepository(Assembly.GetCallingAssembly()), configUri);
        }

        public static void Configure(XmlElement element)
        {
            ConfigureFromXml(LogManager.GetRepository(Assembly.GetCallingAssembly()), element);
        }

        public static void Configure(ILoggerRepository repository, FileInfo configFile)
        {
            LogLog.Debug(string.Concat(new object[] { "XmlConfigurator: configuring repository [", repository.Name, "] using file [", configFile, "]" }));
            if (configFile == null)
            {
                LogLog.Error("XmlConfigurator: Configure called with null 'configFile' parameter");
            }
            else if (!System.IO.File.Exists(configFile.FullName))
            {
                LogLog.Debug("XmlConfigurator: config file [" + configFile.FullName + "] not found. Configuration unchanged.");
            }
            else
            {
                FileStream configStream = null;
                int num = 5;
                while (--num >= 0)
                {
                    try
                    {
                        configStream = configFile.Open(FileMode.Open, FileAccess.Read, FileShare.Read);
                        break;
                    }
                    catch (IOException exception)
                    {
                        if (num == 0)
                        {
                            LogLog.Error("XmlConfigurator: Failed to open XML config file [" + configFile.Name + "]", exception);
                            configStream = null;
                        }
                        Thread.Sleep(250);
                    }
                }
                if (configStream != null)
                {
                    try
                    {
                        Configure(repository, configStream);
                    }
                    finally
                    {
                        configStream.Close();
                    }
                }
            }
        }

        public static void Configure(ILoggerRepository repository, Stream configStream)
        {
            LogLog.Debug("XmlConfigurator: configuring repository [" + repository.Name + "] using stream");
            if (configStream == null)
            {
                LogLog.Error("XmlConfigurator: Configure called with null 'configStream' parameter");
            }
            else
            {
                XmlDocument document = new XmlDocument();
                try
                {
                    XmlReaderSettings settings = new XmlReaderSettings {
                        ProhibitDtd = false
                    };
                    XmlReader reader = XmlReader.Create(configStream, settings);
                    document.Load(reader);
                }
                catch (Exception exception)
                {
                    LogLog.Error("XmlConfigurator: Error while loading XML configuration", exception);
                    document = null;
                }
                if (document != null)
                {
                    LogLog.Debug("XmlConfigurator: loading XML configuration");
                    XmlNodeList elementsByTagName = document.GetElementsByTagName("log4net");
                    if (elementsByTagName.Count == 0)
                    {
                        LogLog.Debug("XmlConfigurator: XML configuration does not contain a <log4net> element. Configuration Aborted.");
                    }
                    else if (elementsByTagName.Count > 1)
                    {
                        LogLog.Error("XmlConfigurator: XML configuration contains [" + elementsByTagName.Count + "] <log4net> elements. Only one is allowed. Configuration Aborted.");
                    }
                    else
                    {
                        ConfigureFromXml(repository, elementsByTagName[0] as XmlElement);
                    }
                }
            }
        }

        public static void Configure(ILoggerRepository repository, Uri configUri)
        {
            LogLog.Debug(string.Concat(new object[] { "XmlConfigurator: configuring repository [", repository.Name, "] using URI [", configUri, "]" }));
            if (configUri == null)
            {
                LogLog.Error("XmlConfigurator: Configure called with null 'configUri' parameter");
            }
            else if (configUri.IsFile)
            {
                Configure(repository, new FileInfo(configUri.LocalPath));
            }
            else
            {
                Exception exception;
                WebRequest request = null;
                try
                {
                    request = WebRequest.Create(configUri);
                }
                catch (Exception exception1)
                {
                    exception = exception1;
                    LogLog.Error("XmlConfigurator: Failed to create WebRequest for URI [" + configUri + "]", exception);
                }
                if (request != null)
                {
                    try
                    {
                        request.Credentials = CredentialCache.DefaultCredentials;
                    }
                    catch
                    {
                    }
                    try
                    {
                        WebResponse response = request.GetResponse();
                        if (response != null)
                        {
                            try
                            {
                                using (Stream stream = response.GetResponseStream())
                                {
                                    Configure(repository, stream);
                                }
                            }
                            finally
                            {
                                response.Close();
                            }
                        }
                    }
                    catch (Exception exception2)
                    {
                        exception = exception2;
                        LogLog.Error("XmlConfigurator: Failed to request config from URI [" + configUri + "]", exception);
                    }
                }
            }
        }

        public static void Configure(ILoggerRepository repository, XmlElement element)
        {
            LogLog.Debug("XmlConfigurator: configuring repository [" + repository.Name + "] using XML element");
            ConfigureFromXml(repository, element);
        }

        public static void ConfigureAndWatch(FileInfo configFile)
        {
            ConfigureAndWatch(LogManager.GetRepository(Assembly.GetCallingAssembly()), configFile);
        }

        public static void ConfigureAndWatch(ILoggerRepository repository, FileInfo configFile)
        {
            LogLog.Debug(string.Concat(new object[] { "XmlConfigurator: configuring repository [", repository.Name, "] using file [", configFile, "] watching for file updates" }));
            if (configFile == null)
            {
                LogLog.Error("XmlConfigurator: ConfigureAndWatch called with null 'configFile' parameter");
            }
            else
            {
                Configure(repository, configFile);
                try
                {
                    ConfigureAndWatchHandler.StartWatching(repository, configFile);
                }
                catch (Exception exception)
                {
                    LogLog.Error("XmlConfigurator: Failed to initialize configuration file watcher for file [" + configFile.FullName + "]", exception);
                }
            }
        }

        private static void ConfigureFromXml(ILoggerRepository repository, XmlElement element)
        {
            if (element == null)
            {
                LogLog.Error("XmlConfigurator: ConfigureFromXml called with null 'element' parameter");
            }
            else if (repository == null)
            {
                LogLog.Error("XmlConfigurator: ConfigureFromXml called with null 'repository' parameter");
            }
            else
            {
                LogLog.Debug("XmlConfigurator: Configuring Repository [" + repository.Name + "]");
                IXmlRepositoryConfigurator configurator = repository as IXmlRepositoryConfigurator;
                if (configurator == null)
                {
                    LogLog.Warn("XmlConfigurator: Repository [" + repository + "] does not support the XmlConfigurator");
                }
                else
                {
                    XmlDocument document = new XmlDocument();
                    XmlElement element2 = (XmlElement) document.AppendChild(document.ImportNode(element, true));
                    configurator.Configure(element2);
                }
            }
        }

        private sealed class ConfigureAndWatchHandler
        {
            private FileInfo m_configFile;
            private ILoggerRepository m_repository;
            private System.Threading.Timer m_timer;
            private const int TimeoutMillis = 500;

            private ConfigureAndWatchHandler(ILoggerRepository repository, FileInfo configFile)
            {
                this.m_repository = repository;
                this.m_configFile = configFile;
                FileSystemWatcher watcher = new FileSystemWatcher {
                    Path = this.m_configFile.DirectoryName,
                    Filter = this.m_configFile.Name,
                    NotifyFilter = NotifyFilters.CreationTime | NotifyFilters.LastWrite | NotifyFilters.FileName
                };
                watcher.Changed += new FileSystemEventHandler(this.ConfigureAndWatchHandler_OnChanged);
                watcher.Created += new FileSystemEventHandler(this.ConfigureAndWatchHandler_OnChanged);
                watcher.Deleted += new FileSystemEventHandler(this.ConfigureAndWatchHandler_OnChanged);
                watcher.Renamed += new RenamedEventHandler(this.ConfigureAndWatchHandler_OnRenamed);
                watcher.EnableRaisingEvents = true;
                this.m_timer = new System.Threading.Timer(new TimerCallback(this.OnWatchedFileChange), null, -1, -1);
            }

            private void ConfigureAndWatchHandler_OnChanged(object source, FileSystemEventArgs e)
            {
                LogLog.Debug(string.Concat(new object[] { "ConfigureAndWatchHandler: ", e.ChangeType, " [", this.m_configFile.FullName, "]" }));
                this.m_timer.Change(500, -1);
            }

            private void ConfigureAndWatchHandler_OnRenamed(object source, RenamedEventArgs e)
            {
                LogLog.Debug(string.Concat(new object[] { "ConfigureAndWatchHandler: ", e.ChangeType, " [", this.m_configFile.FullName, "]" }));
                this.m_timer.Change(500, -1);
            }

            private void OnWatchedFileChange(object state)
            {
                XmlConfigurator.Configure(this.m_repository, this.m_configFile);
            }

            internal static void StartWatching(ILoggerRepository repository, FileInfo configFile)
            {
                new XmlConfigurator.ConfigureAndWatchHandler(repository, configFile);
            }
        }
    }
}

