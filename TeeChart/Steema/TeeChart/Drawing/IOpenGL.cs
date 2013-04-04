namespace Steema.TeeChart.Drawing
{
    using Steema.TeeChart;
    using System;

    public interface IOpenGL
    {
        void SetChart(Chart c);

        bool Active { get; set; }

        int AmbientLight { get; set; }

        SurfaceStyle DrawStyle { get; set; }

        int FontExtrusion { get; set; }

        bool FontOutlines { get; set; }

        IGLLightsource Light0 { get; }

        IGLLightsource Light1 { get; }

        IGLLightsource Light2 { get; }

        bool ShadeQuality { get; set; }

        double Shininess { get; set; }
    }
}

