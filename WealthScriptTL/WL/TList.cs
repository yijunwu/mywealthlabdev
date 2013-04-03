namespace WL
{
    using System;
    using System.Collections;

    public class TList
    {
        private Hashtable dataTable = new Hashtable();
        private ArrayList list = new ArrayList();
        private NumericComparer numericComparer = new NumericComparer();
        private WL.StringComparer stringComparer = new WL.StringComparer();

        public int Add(object obj)
        {
            return this.list.Add(obj);
        }

        public int AddData(object valueKey, object data)
        {
            this.dataTable[valueKey] = data;
            return this.list.Add(valueKey);
        }

        public int AddObject(object valueKey, object data)
        {
            this.dataTable[valueKey] = data;
            return this.list.Add(valueKey);
        }

        public void ChangeItem(int index, object value)
        {
            if ((index >= 0) && (index <= (this.list.Count - 1)))
            {
                this.list[index] = value;
            }
        }

        public void Clear()
        {
            this.list.Clear();
            this.dataTable.Clear();
        }

        public object Data(int index)
        {
            object obj2 = null;
            object obj3 = this.list[index];
            if (obj3 != null)
            {
                obj2 = this.dataTable[obj3];
            }
            return obj2;
        }

        public void Delete(int index)
        {
            if ((index >= 0) && (index <= (this.list.Count - 1)))
            {
                object key = this.list[index];
                if (key != null)
                {
                    this.dataTable.Remove(key);
                }
                this.list.RemoveAt(index);
            }
        }

        public void Free()
        {
        }

        public int IndexOf(object valueKey)
        {
            return this.list.IndexOf(valueKey);
        }

        public int IndexOfData(object data)
        {
            int index = -1;
            object key = null;
            if ((data != null) && this.dataTable.ContainsValue(data))
            {
                IDictionaryEnumerator enumerator = this.dataTable.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    if (data.Equals(enumerator.Value))
                    {
                        key = enumerator.Key;
                        break;
                    }
                }
                if (key != null)
                {
                    index = this.list.IndexOf(key);
                }
            }
            return index;
        }

        public int IndexOfObject(object data)
        {
            return this.IndexOfData(data);
        }

        public object Item(int index)
        {
            if ((index < 0) || (index > (this.list.Count - 1)))
            {
                return null;
            }
            return this.list[index];
        }

        public object Object(int index)
        {
            if ((index < 0) || (index > (this.list.Count - 1)))
            {
                return null;
            }
            object obj2 = null;
            object obj3 = this.list[index];
            if (obj3 != null)
            {
                obj2 = this.dataTable[obj3];
            }
            return obj2;
        }

        public void SortNumeric()
        {
            this.list.Sort(this.numericComparer);
        }

        public void SortString()
        {
            this.list.Sort(this.stringComparer);
        }

        public int Count
        {
            get
            {
                return this.list.Count;
            }
        }
    }
}

