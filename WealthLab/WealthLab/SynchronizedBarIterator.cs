namespace WealthLab
{
    using System;
    using System.Collections.Generic;

    public class SynchronizedBarIterator
    {
        private DateTime dateTime_0;
        private Dictionary<string, int> dictionary_0 = new Dictionary<string, int>();
        private ICollection<Bars> icollection_0;

        public SynchronizedBarIterator(ICollection<Bars> barCollection)
        {
            this.icollection_0 = barCollection;
            foreach (Bars bars in barCollection)
            {
                this.dictionary_0[bars.UniqueDescription] = -1;
            }
            this.dateTime_0 = DateTime.MaxValue;
            foreach (Bars bars2 in barCollection)
            {
                if ((bars2.Count > 0) && (bars2.Date[0] < this.dateTime_0))
                {
                    this.dateTime_0 = bars2.Date[0];
                }
            }
            foreach (Bars bars3 in barCollection)
            {
                if ((bars3.Count > 0) && (bars3.Date[0] == this.dateTime_0))
                {
                    this.dictionary_0[bars3.UniqueDescription] = 0;
                }
            }
        }

        public int Bar(Bars bars)
        {
            return this.dictionary_0[bars.UniqueDescription];
        }

        public bool Next()
        {
            foreach (Bars bars4 in this.icollection_0)
            {
                int num3 = this.dictionary_0[bars4.UniqueDescription];
                if (num3 >= 0)
                {
                    while (num3 < (bars4.Count - 1))
                    {
                        if (bars4.Date[num3] != bars4.Date[num3 + 1])
                        {
                            break;
                        }
                        num3++;
                        this.dictionary_0[bars4.UniqueDescription] = num3;
                    }
                }
            }
            bool flag = true;
            using (IEnumerator<Bars> enumerator2 = this.icollection_0.GetEnumerator())
            {
                while (enumerator2.MoveNext())
                {
                    Bars current = enumerator2.Current;
                    if (this.dictionary_0[current.UniqueDescription] < (current.Count - 1))
                    {
                        ///goto  Label_00D2;  ///WYJ fix, simplify the flow
                        flag = false;
                        break;
                    }
                }
            }
            if (flag)
            {
                return false;
            }
            this.dateTime_0 = DateTime.MaxValue;
            foreach (Bars bars2 in this.icollection_0)
            {
                int num2 = this.dictionary_0[bars2.UniqueDescription];
                if ((num2 < (bars2.Count - 1)) && (bars2.Date[num2 + 1] < this.dateTime_0))
                {
                    this.dateTime_0 = bars2.Date[num2 + 1];
                }
            }
            foreach (Bars bars in this.icollection_0)
            {
                int num = this.dictionary_0[bars.UniqueDescription];
                if (num < (bars.Count - 1))
                {
                    num++;
                    if (bars.Date[num] == this.dateTime_0)
                    {
                        this.dictionary_0[bars.UniqueDescription] = num;
                    }
                }
            }
            return true;
        }

        public DateTime Date
        {
            get
            {
                return this.dateTime_0;
            }
        }
    }
}

