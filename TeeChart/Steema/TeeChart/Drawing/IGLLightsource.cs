namespace Steema.TeeChart.Drawing
{
    using System;
    using System.Drawing;

    public interface IGLLightsource
    {
        float[] GLColor();

        System.Drawing.Color Color { get; set; }

        bool FixedPosition { get; set; }

        IGLPosition Position { get; set; }

        bool Visible { get; set; }
    }
}

