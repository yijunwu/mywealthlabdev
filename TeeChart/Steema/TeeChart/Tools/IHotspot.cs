namespace Steema.TeeChart.Tools
{
    using System;
    using System.Drawing;

    public interface IHotspot
    {
        string GenerateMap();
        string GenerateMap(Rectangle bounds);

        string MapElements { get; set; }
    }
}

