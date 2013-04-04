namespace log4net.Config
{
    using log4net;
    using log4net.Appender;
    using log4net.Layout;
    using log4net.Repository;
    using log4net.Util;
    using System;
    using System.Reflection;

    public sealed class BasicConfigurator
    {
        private BasicConfigurator()
        {
        }

        public static void Configure()
        {
            Configure(LogManager.GetRepository(Assembly.GetCallingAssembly()));
        }

        public static void Configure(IAppender appender)
        {
            Configure(LogManager.GetRepository(Assembly.GetCallingAssembly()), appender);
        }

        public static void Configure(ILoggerRepository repository)
        {
            PatternLayout layout = new PatternLayout {
                ConversionPattern = "%timestamp [%thread] %level %logger %ndc - %message%newline"
            };
            layout.ActivateOptions();
            ConsoleAppender appender = new ConsoleAppender {
                Layout = layout
            };
            appender.ActivateOptions();
            Configure(repository, appender);
        }

        public static void Configure(ILoggerRepository repository, IAppender appender)
        {
            IBasicRepositoryConfigurator configurator = repository as IBasicRepositoryConfigurator;
            if (configurator != null)
            {
                configurator.Configure(appender);
            }
            else
            {
                LogLog.Warn("BasicConfigurator: Repository [" + repository + "] does not support the BasicConfigurator");
            }
        }
    }
}

