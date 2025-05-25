namespace WealthLab.DataProviders.AsciiFilesStatic
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Text;
    using System.Windows.Forms;
    using WealthLab;
    using WealthLab.DataProviders.Helper;

    [Serializable]
    public class AsciiFilesDataSet : DataSetSettings
    {
        public string Extension;
        public FieldList Fields;
        public string Folder;
        public string Name;
        [NonSerialized]
        private NumberFormatInfo numberFormatInfo_0;
        public FormatOptions Options;
        public bool ParseError;
        [NonSerialized]
        public List<object[]> Rows;

        public AsciiFilesDataSet()
        {
            this.Rows = new List<object[]>();
            this.numberFormatInfo_0 = new NumberFormatInfo();
            this.Fields = new FieldList();
            this.Options = new FormatOptions();
            this.Name = "";
            this.Folder = "";
            this.Extension = "";
        }

        public AsciiFilesDataSet(string name) : this()
        {
            this.Name = name;
        }

        public string GetFileName(string symbol)
        {
            string str = this.Folder + Path.DirectorySeparatorChar + symbol;
            string str2 = this.Extension.Remove(0, 1);
            return (str + str2);
        }

        public static List<string> GetFilesFromDir(string string_0, string extension, bool fullPath, bool withExtension)
        {
            List<string> list = new List<string>();
            if (Directory.Exists(string_0))
            {
                string[] collection = Directory.GetFiles(string_0, extension, SearchOption.TopDirectoryOnly);
                if (!fullPath)
                {
                    foreach (string str in collection)
                    {
                        if (!withExtension)
                        {
                            list.Add(Path.GetFileNameWithoutExtension(str));
                        }
                        else
                        {
                            list.Add(Path.GetFileName(str));
                        }
                    }
                    return list;
                }
                list.AddRange(collection);
            }
            return list;
        }

        private void method_0()
        {
            this.numberFormatInfo_0.NumberDecimalSeparator = this.Options.DecimalSeparator;
            this.numberFormatInfo_0.NumberGroupSeparator = this.Options.ThousandsSeparator;
        }

        private int method_1(StreamReader streamReader_0)
        {
            int num = 0;
            streamReader_0.BaseStream.Seek(0L, SeekOrigin.Begin);
            while (streamReader_0.ReadLine() != null)
            {
                num++;
            }
            streamReader_0.BaseStream.Seek(0L, SeekOrigin.Begin);
            return num;
        }

        private void method_2()
        {
            if (this.Rows == null)
            {
                this.Rows = new List<object[]>();
            }
            if (this.numberFormatInfo_0 == null)
            {
                this.numberFormatInfo_0 = new NumberFormatInfo();
            }
        }

        public void Parse(string fileName)
        {
            this.ParseError = false;
            this.method_2();
            this.Rows.Clear();
            this.method_0();
            StreamReader reader = new StreamReader(new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read), Encoding.Default);
            int num = this.method_1(reader);
            int count = this.Fields.Items.Count;
            string str = "";
            bool flag = this.Options.TimeFormat.StartsWith("Hmm");
            for (int i = 0; i < num; i++)
            {
                str = reader.ReadLine();
                if (((i >= this.Options.IgnoreFirstLines) && (i <= (num - this.Options.IgnoreLastLines))) && (str != ""))
                {
                    string[] separator = this.Options.FieldSeparator.Split(new char[] { '&', '+' });
                    string[] strArray = str.Split(separator, StringSplitOptions.None); //Keep empty entries
                    object[] item = new object[count];
                    if (this.Fields.Items.Count > strArray.Length)
                    {
                        string text = string.Format("{0}\r\n\r\nFile: {1}\r\nLine: {2}", "Incorrect field separator or the number of fields specified.", fileName, i.ToString());
                        Class5.smethod_4(TraceEventType.Error, "Parse\r\n" + text);
                        MessageBox.Show(text, "Parse Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        this.Rows.Clear();
                        reader.Close();
                        this.ParseError = true;
                        return;
                    }
                    for (int j = 0; j < this.Fields.Items.Count; j++)
                    {
                        Field field = (Field) this.Fields.Items[j];
                        string str2 = strArray[j];
                        try
                        {
                            switch (field.Type)
                            {
                                case FieldType.Unknow:
                                case FieldType.Filler:
                                {
                                    item[j] = "";
                                    continue;
                                }
                                case FieldType.Date:
                                {
                                    item[j] = DateTime.ParseExact(strArray[j], this.Options.DateFormat, CultureInfo.InvariantCulture);
                                    continue;
                                }
                                case FieldType.Time:
                                {
                                    string s = strArray[j];
                                    if (flag && ((s.Length == 3) || (s.Length == 5)))
                                    {
                                        s = "0" + s;
                                    }
                                    item[j] = DateTime.ParseExact(s, this.Options.TimeFormat, CultureInfo.InvariantCulture);
                                    continue;
                                }
                                case FieldType.Open:
                                case FieldType.High:
                                case FieldType.Low:
                                case FieldType.Close:
                                {
                                    item[j] = Convert.ToDouble(strArray[j], this.numberFormatInfo_0);
                                    if (this.Options.ImpliedDecimals != 0)
                                    {
                                        item[j] = ((double) item[j]) / Math.Pow(10.0, (double) this.Options.ImpliedDecimals);
                                    }
                                    continue;
                                }
                                case FieldType.Volume:
                                {
                                    item[j] = Convert.ToDouble(strArray[j], this.numberFormatInfo_0) * this.Options.VolumeMultiple;
                                    continue;
                                }
                                case FieldType.OpenInterest:
                                case FieldType.Custom:
                                {
                                    item[j] = Convert.ToDouble(strArray[j], this.numberFormatInfo_0);
                                    continue;
                                }
                                case FieldType.SecurityName:
                                {
                                    item[j] = strArray[j];
                                    continue;
                                }
                            }
                        }
                        catch (Exception exception)
                        {
                            string str5 = string.Format("{0}\r\n\r\nFile: {1}\r\nLine: {2}\r\nField: {3}\r\nField Name: {4}\r\nField Type: {5}", new object[] { exception.Message, fileName, i.ToString(), str2, field.Name, field.Type });
                            Class5.smethod_4(TraceEventType.Error, "Parse\r\n" + str5);
                            MessageBox.Show(str5, "Parse Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                            this.Rows.Clear();
                            reader.Close();
                            this.ParseError = true;
                            return;
                        }
                    }
                    this.Rows.Add(item);
                }
            }
            reader.Close();
        }

        public void Parse(string fileName, Bars bars)
        {
            this.Parse(fileName);
            if (!this.ParseError)
            {
                int index = this.Fields.GetIndex(FieldType.Date);
                if (((index > -1) && (this.Rows.Count > 0)) && (((DateTime) this.Rows[0][index]) > ((DateTime) this.Rows[this.Rows.Count - 1][index])))
                {
                    this.Rows.Reverse();
                }
                Dictionary<int, DataSeries> dictionary = new Dictionary<int, DataSeries>();
                for (int i = 0; i < this.Fields.Items.Count; i++)
                {
                    Field field = this.Fields.Items[i] as Field;
                    if ((field.Type == FieldType.OpenInterest) || (field.Type == FieldType.Custom))
                    {
                        bars.RegisterNamedSeries(field.Name, false);
                        dictionary.Add(i, bars.FindNamedSeries(field.Name));
                    }
                }
                foreach (object[] objArray in this.Rows)
                {
                    double num3;
                    double num4;
                    double num6;
                    double num9;
                    DateTime minValue = DateTime.MinValue;
                    double open = num4 = num6 = num3 = num9 = 0.0;
                    Dictionary<int, double> dictionary2 = new Dictionary<int, double>();
                    for (int j = 0; j < this.Fields.Items.Count; j++)
                    {
                        object obj2 = objArray[j];
                        Field field2 = (Field) this.Fields.Items[j];
                        switch (field2.Type)
                        {
                            case FieldType.Date:
                                minValue = (DateTime) obj2;
                                break;

                            case FieldType.Time:
                            {
                                DateTime time = (DateTime) obj2;
                                minValue = minValue.Add(time.TimeOfDay);
                                break;
                            }
                            case FieldType.Open:
                                open = (double) obj2;
                                break;

                            case FieldType.High:
                                num4 = (double) obj2;
                                break;

                            case FieldType.Low:
                                num6 = (double) obj2;
                                break;

                            case FieldType.Close:
                                num3 = (double) obj2;
                                break;

                            case FieldType.Volume:
                                num9 = (double) obj2;
                                break;

                            case FieldType.OpenInterest:
                            case FieldType.Custom:
                                dictionary2.Add(j, (double) obj2);
                                break;

                            case FieldType.SecurityName:
                                bars.SecurityName = (string) obj2;
                                break;
                        }
                    }
                    if (open == 0.0)
                    {
                        open = num3;
                    }
                    if (num4 == 0.0)
                    {
                        num4 = num3;
                    }
                    if (num6 == 0.0)
                    {
                        num6 = num3;
                    }
                    bars.Add(minValue, open, num4, num6, num3, num9);
                    foreach (int num5 in dictionary.Keys)
                    {
                        dictionary[num5][bars.Count - 1] = dictionary2[num5];
                    }
                }
            }
        }
    }
}

