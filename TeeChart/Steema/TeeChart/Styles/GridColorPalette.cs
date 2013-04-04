namespace Steema.TeeChart.Styles
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;

    public class GridColorPalette
    {
        private Custom3DPalette iseries;

        public GridColorPalette(Custom3DPalette series)
        {
            this.iseries = series;
        }

        [DefaultValue(typeof(Color), "White")]
        public Color EndColor
        {
            get
            {
                return this.iseries.EndColor;
            }
            set
            {
                this.iseries.EndColor = value;
            }
        }

        [DefaultValue(typeof(Color), "Transparent")]
        public Color MidColor
        {
            get
            {
                return this.iseries.MidColor;
            }
            set
            {
                this.iseries.MidColor = value;
            }
        }

        public List<GridPalette> Palette
        {
            get
            {
                return this.iseries.Palette;
            }
        }

        public double PaletteMin
        {
            get
            {
                return this.iseries.PaletteMin;
            }
            set
            {
                this.iseries.PaletteMin = value;
            }
        }

        public double PaletteStep
        {
            get
            {
                return this.iseries.PaletteStep;
            }
            set
            {
                this.iseries.PaletteStep = value;
            }
        }

        [DefaultValue(0x20)]
        public int PaletteSteps
        {
            get
            {
                return this.iseries.PaletteSteps;
            }
            set
            {
                this.iseries.PaletteSteps = value;
            }
        }

        [DefaultValue(0)]
        public PaletteStyles PaletteStyle
        {
            get
            {
                return this.iseries.PaletteStyle;
            }
            set
            {
                this.iseries.PaletteStyle = value;
            }
        }

        [DefaultValue(typeof(Color), "Navy")]
        public Color StartColor
        {
            get
            {
                return this.iseries.StartColor;
            }
            set
            {
                this.iseries.StartColor = value;
            }
        }

        [DefaultValue(true)]
        public bool UseColorRange
        {
            get
            {
                return this.iseries.UseColorRange;
            }
            set
            {
                this.iseries.UseColorRange = value;
            }
        }

        [DefaultValue(false)]
        public bool UsePalette
        {
            get
            {
                return this.iseries.UsePalette;
            }
            set
            {
                this.iseries.UsePalette = value;
            }
        }

        [DefaultValue(false)]
        public bool UsePaletteMin
        {
            get
            {
                return this.iseries.UsePaletteMin;
            }
            set
            {
                this.iseries.UsePaletteMin = value;
            }
        }
    }
}

