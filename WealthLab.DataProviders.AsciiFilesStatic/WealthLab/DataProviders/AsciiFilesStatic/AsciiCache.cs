namespace WealthLab.DataProviders.AsciiFilesStatic
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using WealthLab;

    public class AsciiCache
    {
        private BarScale barScale_0;
        private System.DateTime dateTime_0;
        private System.DateTime dateTime_1;
        private int int_0;
        private int int_1;
        private List<string> list_0 = new List<string>();
        private List<string> list_1 = new List<string>();
        private string string_0;
        private string string_1;
        private string string_2;
        private string string_3;
        private string string_4;

        public string GetCachePath(string fileName)
        {
            string str = fileName.Substring(0, 1);
            string path = Path.Combine(Config.CachePath, str);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return Path.Combine(path, fileName);
        }

        private void method_0(Bars bars_0)
        {
            using (BinaryWriter writer = new BinaryWriter(File.Create(this.GetCachePath(this.string_0))))
            {
                writer.Write(bars_0.Count);
                for (int i = 0; i < bars_0.Count; i++)
                {
                    writer.Write(bars_0.Date[i].ToBinary());
                    writer.Write(bars_0.Open[i]);
                    writer.Write(bars_0.High[i]);
                    writer.Write(bars_0.Low[i]);
                    writer.Write(bars_0.Close[i]);
                    writer.Write(bars_0.Volume[i]);
                }
            }
        }

        private void method_1(DataSeries dataSeries_0, string string_5)
        {
            using (BinaryWriter writer = new BinaryWriter(File.Create(this.GetCachePath(string_5))))
            {
                writer.Write(dataSeries_0.Count);
                for (int i = 0; i < dataSeries_0.Count; i++)
                {
                    writer.Write(dataSeries_0[i]);
                }
            }
        }

        private Bars method_2()
        {
            Bars bars = new Bars(this.string_1, this.barScale_0, this.int_1);
            using (BinaryReader reader = new BinaryReader(File.Open(this.GetCachePath(this.string_0), FileMode.Open, FileAccess.Read, FileShare.Read)))
            {
                int num = reader.ReadInt32();
                for (int i = 0; i < num; i++)
                {
                    bars.Add(System.DateTime.FromBinary(reader.ReadInt64()), reader.ReadDouble(), reader.ReadDouble(), reader.ReadDouble(), reader.ReadDouble(), reader.ReadDouble());
                }
            }
            return bars;
        }

        private void method_3(Bars bars_0, string string_5, string string_6)
        {
            DataSeries series = bars_0.RegisterNamedSeries(string_5, false);
            using (BinaryReader reader = new BinaryReader(File.Open(this.GetCachePath(string_6), FileMode.Open, FileAccess.Read, FileShare.Read)))
            {
                int num = reader.ReadInt32();
                for (int i = 0; i < num; i++)
                {
                    series[i] = reader.ReadDouble();
                }
            }
        }

        public Bars ReadCache()
        {
            Bars bars = this.method_2();
            for (int i = 0; i < this.list_1.Count; i++)
            {
                this.method_3(bars, this.list_0[i], this.list_1[i]);
            }
            return bars;
        }

        public void WriteCache(Bars bars, string sourceFileName, string dataSet)
        {
            this.string_2 = bars.SecurityName;
            this.int_1 = bars.BarInterval;
            this.barScale_0 = bars.Scale;
            this.string_1 = bars.Symbol;
            this.int_0 = bars.Count;
            this.string_3 = sourceFileName;
            this.dateTime_1 = System.DateTime.Now;
            this.dateTime_0 = File.GetLastWriteTime(sourceFileName);
            this.string_4 = dataSet;
            this.string_0 = Guid.NewGuid() + ".cache";
            this.method_0(bars);
            foreach (DataSeries series in bars.NamedSeries)
            {
                this.list_0.Add(series.Description);
                string item = Guid.NewGuid() + ".dscache";
                this.list_1.Add(item);
                this.method_1(series, item);
            }
        }

        public int BarInterval
        {
            get
            {
                return this.int_1;
            }
            set
            {
                this.int_1 = value;
            }
        }

        public int BarsCount
        {
            get
            {
                return this.int_0;
            }
            set
            {
                this.int_0 = value;
            }
        }

        public List<string> DataSeriesFiles
        {
            get
            {
                return this.list_1;
            }
            set
            {
                this.list_1 = value;
            }
        }

        public List<string> DataSeriesNames
        {
            get
            {
                return this.list_0;
            }
            set
            {
                this.list_0 = value;
            }
        }

        public string DataSet
        {
            get
            {
                return this.string_4;
            }
            set
            {
                this.string_4 = value;
            }
        }

        public System.DateTime DateTime
        {
            get
            {
                return this.dateTime_1;
            }
            set
            {
                this.dateTime_1 = value;
            }
        }

        public string FileName
        {
            get
            {
                return this.string_0;
            }
            set
            {
                this.string_0 = value;
            }
        }

        public BarScale Scale
        {
            get
            {
                return this.barScale_0;
            }
            set
            {
                this.barScale_0 = value;
            }
        }

        public string SecurityName
        {
            get
            {
                return this.string_2;
            }
            set
            {
                this.string_2 = value;
            }
        }

        public string SourceFileName
        {
            get
            {
                return this.string_3;
            }
            set
            {
                this.string_3 = value;
            }
        }

        public System.DateTime SourceLastWriteTime
        {
            get
            {
                return this.dateTime_0;
            }
            set
            {
                this.dateTime_0 = value;
            }
        }

        public string Symbol
        {
            get
            {
                return this.string_1;
            }
            set
            {
                this.string_1 = value;
            }
        }
    }
}

