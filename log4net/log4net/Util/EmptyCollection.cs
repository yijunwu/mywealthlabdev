namespace log4net.Util
{
    using System;
    using System.Collections;

    [Serializable]
    public sealed class EmptyCollection : ICollection, IEnumerable
    {
        private static readonly EmptyCollection s_instance = new EmptyCollection();

        private EmptyCollection()
        {
        }

        public void CopyTo(Array array, int index)
        {
        }

        public IEnumerator GetEnumerator()
        {
            return NullEnumerator.Instance;
        }

        public int Count
        {
            get
            {
                return 0;
            }
        }

        public static EmptyCollection Instance
        {
            get
            {
                return s_instance;
            }
        }

        public bool IsSynchronized
        {
            get
            {
                return true;
            }
        }

        public object SyncRoot
        {
            get
            {
                return this;
            }
        }
    }
}

