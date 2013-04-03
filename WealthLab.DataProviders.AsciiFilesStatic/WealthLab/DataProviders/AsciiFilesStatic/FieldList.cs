namespace WealthLab.DataProviders.AsciiFilesStatic
{
    using System;
    using System.Collections;

    [Serializable]
    public class FieldList
    {
        public ArrayList Items = new ArrayList();

        public int GetIndex(FieldType type)
        {
            for (int i = 0; i < this.Items.Count; i++)
            {
                if ((this.Items[i] as Field).Type == type)
                {
                    return i;
                }
            }
            return -1;
        }
    }
}

