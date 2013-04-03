namespace WealthLab.Extensions.Attribute
{
    using System;

    [Serializable]
    public enum ExtensionType
    {
        Addin = 2,
        Indicator = 4,
        Provider = 1,
        Strategy = 3
    }
}

