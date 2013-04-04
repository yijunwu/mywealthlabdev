namespace log4net.Util
{
    using System;
    using System.Collections;

    public sealed class NullDictionaryEnumerator : IDictionaryEnumerator, IEnumerator
    {
        private static readonly NullDictionaryEnumerator s_instance = new NullDictionaryEnumerator();

        private NullDictionaryEnumerator()
        {
        }

        public bool MoveNext()
        {
            return false;
        }

        public void Reset()
        {
        }

        public object Current
        {
            get
            {
                throw new InvalidOperationException();
            }
        }

        public DictionaryEntry Entry
        {
            get
            {
                throw new InvalidOperationException();
            }
        }

        public static NullDictionaryEnumerator Instance
        {
            get
            {
                return s_instance;
            }
        }

        public object Key
        {
            get
            {
                throw new InvalidOperationException();
            }
        }

        public object Value
        {
            get
            {
                throw new InvalidOperationException();
            }
        }
    }
}

