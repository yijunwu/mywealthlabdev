namespace log4net.Util
{
    using System;
    using System.Collections;
    using System.Reflection;
    using System.Runtime.Serialization;

    [Serializable]
    public sealed class PropertiesDictionary : ReadOnlyPropertiesDictionary, ISerializable, IDictionary, ICollection, IEnumerable
    {
        public PropertiesDictionary()
        {
        }

        public PropertiesDictionary(ReadOnlyPropertiesDictionary propertiesDictionary) : base(propertiesDictionary)
        {
        }

        private PropertiesDictionary(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public override void Clear()
        {
            base.InnerHashtable.Clear();
        }

        public void Remove(string key)
        {
            base.InnerHashtable.Remove(key);
        }

        void ICollection.CopyTo(Array array, int index)
        {
            base.InnerHashtable.CopyTo(array, index);
        }

        void IDictionary.Add(object key, object value)
        {
            if (!(key is string))
            {
                throw new ArgumentException("key must be a string", "key");
            }
            base.InnerHashtable.Add(key, value);
        }

        bool IDictionary.Contains(object key)
        {
            return base.InnerHashtable.Contains(key);
        }

        IDictionaryEnumerator IDictionary.GetEnumerator()
        {
            return base.InnerHashtable.GetEnumerator();
        }

        void IDictionary.Remove(object key)
        {
            base.InnerHashtable.Remove(key);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable) base.InnerHashtable).GetEnumerator();
        }

        public override object this[string key]
        {
            get
            {
                return base.InnerHashtable[key];
            }
            set
            {
                base.InnerHashtable[key] = value;
            }
        }

        bool ICollection.IsSynchronized
        {
            get
            {
                return base.InnerHashtable.IsSynchronized;
            }
        }

        object ICollection.SyncRoot
        {
            get
            {
                return base.InnerHashtable.SyncRoot;
            }
        }

        bool IDictionary.IsFixedSize
        {
            get
            {
                return false;
            }
        }

        bool IDictionary.IsReadOnly
        {
            get
            {
                return false;
            }
        }

        object IDictionary.this[object key]
        {
            get
            {
                if (!(key is string))
                {
                    throw new ArgumentException("key must be a string", "key");
                }
                return base.InnerHashtable[key];
            }
            set
            {
                if (!(key is string))
                {
                    throw new ArgumentException("key must be a string", "key");
                }
                base.InnerHashtable[key] = value;
            }
        }

        ICollection IDictionary.Keys
        {
            get
            {
                return base.InnerHashtable.Keys;
            }
        }

        ICollection IDictionary.Values
        {
            get
            {
                return base.InnerHashtable.Values;
            }
        }
    }
}

