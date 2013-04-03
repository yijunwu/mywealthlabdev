namespace WealthLab
{
    using System;
    using System.Collections.Generic;
    using System.Threading;

    public class SymbolLock
    {
        private List<string> list_0 = new List<string>();

        public void LockSymbol(string symbol)
        {
            bool flag = true;
            while (flag)
            {
                lock (this.list_0)
                {
                    if (!(flag = this.list_0.Contains(symbol)))
                    {
                        break;
                    }
                }
                Thread.Sleep(10);
            }
            bool flag2 = false;
            lock (this.list_0)
            {
                if (this.list_0.Contains(symbol))
                {
                    flag2 = true;
                }
                else
                {
                    this.list_0.Add(symbol);
                }
            }
            if (flag2)
            {
                this.LockSymbol(symbol);
            }
        }

        public void UnlockSymbol(string symbol)
        {
            lock (this.list_0)
            {
                while (this.list_0.Contains(symbol))
                {
                    this.list_0.Remove(symbol);
                }
            }
        }
    }
}

