namespace WealthLab.DataProviders.AsciiFilesStatic
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using WealthLab;
    using WealthLab.DataProviders.Helper;

    [Serializable]
    public class AsciiFilesDataSetList : DataSetSettings
    {
        public ArrayList Items = new ArrayList();
        public int Version = 2;

        public void CreateDefaultFormats(int version)
        {
            if (version < 1)
            {
                AsciiFilesDataSet set = new AsciiFilesDataSet("Default");
                set.Fields.Items.Add(new Field(FieldType.Date));
                set.Fields.Items.Add(new Field(FieldType.Open));
                set.Fields.Items.Add(new Field(FieldType.High));
                set.Fields.Items.Add(new Field(FieldType.Low));
                set.Fields.Items.Add(new Field(FieldType.Close));
                set.Fields.Items.Add(new Field(FieldType.Volume));
                this.Items.Add(set);
                set = new AsciiFilesDataSet("Yahoo! Finance");
                set.Fields.Items.Add(new Field(FieldType.Date));
                set.Fields.Items.Add(new Field(FieldType.Open));
                set.Fields.Items.Add(new Field(FieldType.High));
                set.Fields.Items.Add(new Field(FieldType.Low));
                set.Fields.Items.Add(new Field(FieldType.Close));
                set.Fields.Items.Add(new Field(FieldType.Volume));
                set.Options.DateFormat = "yyyy-MM-dd";
                set.Options.IgnoreFirstLines = 1;
                this.Items.Add(set);
                set = new AsciiFilesDataSet("MSN");
                set.Fields.Items.Add(new Field(FieldType.Date));
                set.Fields.Items.Add(new Field(FieldType.Open));
                set.Fields.Items.Add(new Field(FieldType.High));
                set.Fields.Items.Add(new Field(FieldType.Low));
                set.Fields.Items.Add(new Field(FieldType.Close));
                set.Fields.Items.Add(new Field(FieldType.Volume));
                set.Options.DateFormat = "M/d/yyyy";
                set.Options.IgnoreFirstLines = 7;
                this.Items.Add(set);
                set = new AsciiFilesDataSet("Google");
                set.Fields.Items.Add(new Field(FieldType.Date));
                set.Fields.Items.Add(new Field(FieldType.Open));
                set.Fields.Items.Add(new Field(FieldType.High));
                set.Fields.Items.Add(new Field(FieldType.Low));
                set.Fields.Items.Add(new Field(FieldType.Close));
                set.Fields.Items.Add(new Field(FieldType.Volume));
                set.Options.DateFormat = "d-MMM-yy";
                set.Options.IgnoreFirstLines = 1;
                this.Items.Add(set);
            }
        }

        public void Delete(string name)
        {
            for (int i = this.Items.Count - 1; i >= 0; i--)
            {
                if ((this.Items[i] as AsciiFilesDataSet).Name == name)
                {
                    this.Items.RemoveAt(i);
                }
            }
        }

        public AsciiFilesDataSetList GetFormatFromDataSets(string string_0)
        {
            AsciiFilesDataSetList list = new AsciiFilesDataSetList();
            foreach (string str in Directory.GetFiles(string_0, "*.xml", SearchOption.TopDirectoryOnly))
            {
                DataSource source = DataSource.FromFile(str);
                if (source.ProviderName == "AsciiFilesStaticProvider")
                {
                    AsciiFilesDataSet set = (AsciiFilesDataSet) DataSetSettings.DeserializeFromString(source.DSString);
                    set.Name = source.Name;
                    list.Items.Add(set);
                }
            }
            return list;
        }

        public List<string> GetNames()
        {
            List<string> list = new List<string>();
            foreach (object obj2 in this.Items)
            {
                list.Add((obj2 as AsciiFilesDataSet).Name);
            }
            return list;
        }

        public AsciiFilesDataSet Search(string name)
        {
            AsciiFilesDataSet set2;
            using (IEnumerator enumerator = this.Items.GetEnumerator())
            {
                AsciiFilesDataSet current;
                while (enumerator.MoveNext())
                {
                    current = (AsciiFilesDataSet) enumerator.Current;
                    if (current.Name == name)
                    {
                        goto Label_0033;
                    }
                }
                return null;
            Label_0033:
                set2 = current;
            }
            return set2;
        }
    }
}

