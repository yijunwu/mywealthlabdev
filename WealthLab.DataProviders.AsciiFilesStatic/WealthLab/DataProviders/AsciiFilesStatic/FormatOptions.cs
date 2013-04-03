namespace WealthLab.DataProviders.AsciiFilesStatic
{
    using System;

    [Serializable]
    public class FormatOptions
    {
        public string DateFormat = "d/M/yyyy";
        public string DecimalSeparator = ".";
        public string FieldSeparator = ",";
        public int IgnoreFirstLines = 0;
        public int IgnoreLastLines = 0;
        public int ImpliedDecimals = 0;
        public string ThousandsSeparator = "";
        public string TimeFormat = "H:mm:ss";
        public int VolumeMultiple = 1;
    }
}

