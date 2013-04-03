namespace WealthLab
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    public class FundamentalDataStore : DataStore
    {
        private Dictionary<string, List<FundamentalItem>> dictionary_0;
        private Dictionary<string, List<FundamentalItem>> dictionary_1;
        private FundamentalDataProvider fundamentalDataProvider_0;
        private string string_2;
        private static SymbolLock symbolLock_0 = new SymbolLock();

        public FundamentalDataStore(IDataHost dataHost, FundamentalDataProvider provider) : base(dataHost.BaseDataFolder, provider.GetType().Name, "WLF")
        {
            this.string_2 = "";
            this.dictionary_0 = new Dictionary<string, List<FundamentalItem>>();
            this.dictionary_1 = new Dictionary<string, List<FundamentalItem>>();
            this.fundamentalDataProvider_0 = provider;
        }

        private void method_1(string string_3)
        {
            this.string_2 = string_3;
            string str = base.FileNameForSymbol(string_3);
            this.method_3(str, this.dictionary_1);
        }

        private void method_2()
        {
            string str = this.method_5();
            this.method_3(str, this.dictionary_0);
        }

        private void method_3(string string_3, Dictionary<string, List<FundamentalItem>> dictionary_2)
        {
            FileStream input = null;
            dictionary_2.Clear();
            if (File.Exists(string_3))
            {
                string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(string_3);
                try
                {
                    symbolLock_0.LockSymbol(fileNameWithoutExtension);
                    input = File.OpenRead(string_3);
                    try
                    {
                        BinaryReader reader = new BinaryReader(input);
                        reader.ReadDouble();
                        int num = reader.ReadInt32();
                        for (int i = 0; i < num; i++)
                        {
                            string itemName = reader.ReadString();
                            List<FundamentalItem> list = new List<FundamentalItem>();
                            int num4 = reader.ReadInt32();
                            for (int j = 0; j < num4; j++)
                            {
                                FundamentalItem item = this.fundamentalDataProvider_0.CreateItem(itemName);
                                item.method_1(reader);
                                list.Add(item);
                            }
                            if (!dictionary_2.ContainsKey(itemName))
                            {
                                dictionary_2.Add(itemName, list);
                            }
                        }
                    }
                    finally
                    {
                        if (input != null)
                        {
                            input.Close();
                        }
                        symbolLock_0.UnlockSymbol(fileNameWithoutExtension);
                    }
                }
                catch
                {
                    symbolLock_0.UnlockSymbol(fileNameWithoutExtension);
                    File.Delete(string_3);
                }
            }
        }

        private void method_4(string string_3, Dictionary<string, List<FundamentalItem>> dictionary_2)
        {
            FileStream output = null;
            double num = 1.0;
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(string_3);
            try
            {
                symbolLock_0.LockSymbol(fileNameWithoutExtension);
                output = File.Create(string_3);
                BinaryWriter writer = new BinaryWriter(output);
                writer.Write(num);
                writer.Write(dictionary_2.Count);
                foreach (KeyValuePair<string, List<FundamentalItem>> pair in dictionary_2)
                {
                    writer.Write(pair.Key);
                    List<FundamentalItem> list = pair.Value;
                    writer.Write(list.Count);
                    foreach (FundamentalItem item in list)
                    {
                        item.method_0(writer);
                    }
                }
            }
            finally
            {
                if (output != null)
                {
                    output.Close();
                }
                symbolLock_0.UnlockSymbol(fileNameWithoutExtension);
            }
        }

        private string method_5()
        {
            return (base.RootPath + "NonSymbol.WLF");
        }

        public List<FundamentalItem> RequestNonSymbolItems(string itemName)
        {
            if (this.dictionary_0.Count == 0)
            {
                this.method_2();
            }
            if (this.dictionary_0.ContainsKey(itemName))
            {
                return this.dictionary_0[itemName];
            }
            return new List<FundamentalItem>();
        }

        public List<FundamentalItem> RequestSymbolItems(string symbol, string itemName)
        {
            if (symbol != this.string_2)
            {
                this.method_1(symbol);
            }
            if (this.dictionary_1.ContainsKey(itemName))
            {
                return this.dictionary_1[itemName];
            }
            return new List<FundamentalItem>();
        }

        public void SaveNonSymbolItems()
        {
            string str = this.method_5();
            this.method_4(str, this.dictionary_0);
        }

        public void SaveSymbolItems()
        {
            string str = base.FileNameForSymbol(this.string_2);
            this.method_4(str, this.dictionary_1);
        }

        public void UpdateNonSymbolItems(string itemName, List<FundamentalItem> items)
        {
            if (this.dictionary_0.Count == 0)
            {
                this.method_2();
            }
            this.dictionary_0.Remove(itemName);
            this.dictionary_0.Add(itemName, items);
        }

        public void UpdateSymbolItems(string symbol, string itemName, List<FundamentalItem> items)
        {
            if (symbol != this.string_2)
            {
                this.method_1(symbol);
            }
            this.dictionary_1.Remove(itemName);
            this.dictionary_1.Add(itemName, items);
        }
    }
}

