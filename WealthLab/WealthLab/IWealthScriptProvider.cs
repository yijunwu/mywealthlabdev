namespace WealthLab
{
    using System;

    public interface IWealthScriptProvider
    {
        bool SaveStrategy();

        bool ParametersNeedSave { get; set; }

        WealthLab.Strategy Strategy { get; set; }

        WealthLab.WealthScript WealthScript { get; set; }
    }
}

