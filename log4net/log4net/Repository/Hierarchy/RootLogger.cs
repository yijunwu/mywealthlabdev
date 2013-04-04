namespace log4net.Repository.Hierarchy
{
    using log4net.Core;
    using log4net.Util;
    using System;

    public class RootLogger : Logger
    {
        public RootLogger(log4net.Core.Level level) : base("root")
        {
            this.Level = level;
        }

        public override log4net.Core.Level EffectiveLevel
        {
            get
            {
                return base.Level;
            }
        }

        public override log4net.Core.Level Level
        {
            get
            {
                return base.Level;
            }
            set
            {
                if (value == null)
                {
                    LogLog.Error("RootLogger: You have tried to set a null level to root.", new LogException());
                }
                else
                {
                    base.Level = value;
                }
            }
        }
    }
}

