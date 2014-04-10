namespace WealthLab
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    public class DataStore
    {
        private bool bool_0 = true;
        private string rootPath;
        private string string_1;

        public DataStore(string rootPath, string dataStoreName, string extension)
        {
            this.string_1 = extension;
            this.rootPath = rootPath;
            if (!Directory.Exists(this.rootPath))
            {
                Directory.CreateDirectory(this.rootPath);
            }
            this.rootPath = this.rootPath + @"\" + dataStoreName + @"\";
            if (!Directory.Exists(this.rootPath))
            {
                Directory.CreateDirectory(this.rootPath);
            }
        }

        protected string BasePathPerScale(BarScale scale, int barInterval)
        {
            string str;
            switch (scale)
            {
                case BarScale.Weekly:
                    str = this.rootPath + "Weekly";
                    break;

                case BarScale.Monthly:
                    str = this.rootPath + "Monthly";
                    break;

                case BarScale.Minute:
                    str = this.rootPath + barInterval + " minute";
                    break;

                case BarScale.Second:
                    str = this.rootPath + barInterval + " second";
                    break;

                case BarScale.Tick:
                    str = this.rootPath + barInterval + " tick";
                    break;

                case BarScale.Quarterly:
                    str = this.rootPath + "Quarterly";
                    break;

                case BarScale.Yearly:
                    str = this.rootPath + "Yearly";
                    break;

                default:
                    str = this.rootPath + "Daily";
                    break;
            }
            if (this.bool_0 && !Directory.Exists(str))
            {
                Directory.CreateDirectory(str);
            }
            return str;
        }

        public bool ContainsSymbol(string symbol)
        {
            this.bool_0 = false;
            string path = this.FileNameForSymbol(symbol);
            this.bool_0 = true;
            return File.Exists(path);
        }

        public bool ContainsSymbol(string symbol, BarScale scale, int barInterval)
        {
            this.bool_0 = false;
            string path = this.FileNameForSymbol(symbol, scale, barInterval);
            this.bool_0 = true;
            return File.Exists(path);
        }

        protected string DataPathForSymbol(string symbol)
        {
            string path = this.rootPath;
            if ((symbol == null) || (symbol == ""))
            {
                throw new ArgumentException("Symbol must not be blank");
            }
            char ch = symbol.ToUpper()[0];
            if ((ch >= 'A') && (ch <= 'Z'))
            {
                path = path + ch + @"\";
                if (this.bool_0 && !Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
            }
            return path;
        }

        protected string DataPathForSymbol(string symbol, BarScale scale, int barInterval)
        {
            if ((symbol == null) || (symbol == ""))
            {
                throw new ArgumentException("Symbol must not be blank");
            }
            string path = this.BasePathPerScale(scale, barInterval);
            char ch = symbol.ToUpper()[0];
            if ((ch >= 'A') && (ch <= 'Z'))
            {
                path = path + @"\" + ch;
                if (this.bool_0 && !Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
            }
            return path;
        }

        public string FileNameForBars(Bars bars)
        {
            return this.FileNameForSymbol(bars.Symbol, bars.Scale, bars.BarInterval);
        }

        public string FileNameForSymbol(string symbol)
        {
            return (this.DataPathForSymbol(symbol) + SymbolFileNameConverter.SymbolToFileName(symbol) + "." + this.string_1);
        }

        public string FileNameForSymbol(string symbol, BarScale scale, int barInterval)
        {
            return (this.DataPathForSymbol(symbol, scale, barInterval) + @"\" + SymbolFileNameConverter.SymbolToFileName(symbol) + "." + this.string_1);
        }

        public IList<BarDataScale> GetExistingBarScales()
        {
            List<BarDataScale> list = new List<BarDataScale>();
            foreach (string str2 in Directory.GetDirectories(this.rootPath))
            {
                string[] strArray3 = str2.Split(new char[] { '\\' });
                string str = strArray3[strArray3.Length - 1];
                if (str.Contains(" "))
                {
                    string[] strArray4 = str.Split(new char[] { ' ' });
                    int barInterval = int.Parse(strArray4[0]);
                    try
                    {
                        BarScale scale2 = (BarScale) Enum.Parse(typeof(BarScale), strArray4[1], true);
                        list.Add(new BarDataScale(scale2, barInterval));
                    }
                    catch
                    {
                    }
                }
                else
                {
                    try
                    {
                        BarScale scale = (BarScale) Enum.Parse(typeof(BarScale), str, true);
                        list.Add(new BarDataScale(scale, 0));
                    }
                    catch
                    {
                    }
                }
            }
            return list;
        }

        public List<string> GetExistingSymbols()
        {
            List<string> list = new List<string>();
            this.method_0(this.rootPath, list);
            return list;
        }

        public List<string> GetExistingSymbols(BarScale scale, int barInterval)
        {
            List<string> list = new List<string>();
            string str = this.BasePathPerScale(scale, barInterval);
            this.method_0(str, list);
            return list;
        }

        private void method_0(string string_2, List<string> list_0)
        {
            foreach (string str2 in Directory.GetFiles(string_2))
            {
                string item = SymbolFileNameConverter.FileNameToSymbol(Path.GetFileNameWithoutExtension(str2));
                list_0.Add(item);
            }
            foreach (string str in Directory.GetDirectories(string_2))
            {
                this.method_0(str, list_0);
            }
        }

        public void RemoveFile(string symbol)
        {
            string path = this.FileNameForSymbol(symbol);
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        public void RemoveFile(Bars bars)
        {
            this.RemoveFile(bars.Symbol, bars.Scale, bars.BarInterval);
        }

        public void RemoveFile(string symbol, BarScale scale, int barInterval)
        {
            string path = this.FileNameForSymbol(symbol, scale, barInterval);
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        public virtual DateTime SymbolLastUpdated(string symbol)
        {
            string path = this.FileNameForSymbol(symbol);
            if (File.Exists(path))
            {
                return File.GetLastWriteTime(path);
            }
            return DateTime.MinValue;
        }

        public virtual DateTime SymbolLastUpdated(string symbol, BarScale scale, int barInterval)
        {
            string path = this.FileNameForSymbol(symbol, scale, barInterval);
            if (File.Exists(path))
            {
                return File.GetLastWriteTime(path);
            }
            return DateTime.MinValue;
        }

        public string RootPath
        {
            get
            {
                return this.rootPath;
            }
            set
            {
                if (value.Substring(value.Length - 1, 1) != @"\")
                {
                    this.rootPath = value + @"\";
                }
                else
                {
                    this.rootPath = value;
                }
                if (!Directory.Exists(this.rootPath))
                {
                    Directory.CreateDirectory(this.rootPath);
                }
            }
        }
    }
}

