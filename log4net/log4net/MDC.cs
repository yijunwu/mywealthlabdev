namespace log4net
{
    using System;

    public sealed class MDC
    {
        private MDC()
        {
        }

        public static void Clear()
        {
            ThreadContext.Properties.Clear();
        }

        public static string Get(string key)
        {
            object obj2 = ThreadContext.Properties[key];
            if (obj2 == null)
            {
                return null;
            }
            return obj2.ToString();
        }

        public static void Remove(string key)
        {
            ThreadContext.Properties.Remove(key);
        }

        public static void Set(string key, string value)
        {
            ThreadContext.Properties[key] = value;
        }
    }
}

