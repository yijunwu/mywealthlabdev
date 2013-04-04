namespace log4net.Repository.Hierarchy
{
    using log4net.Core;
    using System;

    internal class DefaultLoggerFactory : ILoggerFactory
    {
        internal DefaultLoggerFactory()
        {
        }

        public Logger CreateLogger(string name)
        {
            if (name == null)
            {
                return new RootLogger(Level.Debug);
            }
            return new LoggerImpl(name);
        }

        internal sealed class LoggerImpl : Logger
        {
            internal LoggerImpl(string name) : base(name)
            {
            }
        }
    }
}

