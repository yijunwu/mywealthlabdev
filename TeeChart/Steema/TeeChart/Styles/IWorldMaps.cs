namespace Steema.TeeChart.Styles
{
    using System;

    public interface IWorldMaps
    {
        string[] GetWorldMapTypeNames();
        object ParseWorldMapTypeName(string name);

        bool InGallery { get; set; }
    }
}

