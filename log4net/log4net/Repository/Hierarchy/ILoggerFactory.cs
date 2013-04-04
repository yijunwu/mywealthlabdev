namespace log4net.Repository.Hierarchy
{
    using System;

    public interface ILoggerFactory
    {
        Logger CreateLogger(string name);
    }
}

