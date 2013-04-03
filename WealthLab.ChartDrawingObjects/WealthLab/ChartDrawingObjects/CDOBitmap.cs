namespace WealthLab.ChartDrawingObjects
{
    using Fidelity.Components;
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Windows.Forms;
    using WealthLab;
    using WealthLab.ChartControl;

    public class CDOBitmap : CDOPointBased
    {
        private BitmapSettings bitmapSettings_0;
        private bool bool_2;
        private static DrawingObjectHelper drawingObjectHelper_0 = new BitmapHelper();
        private Image image_0;
        private int int_0;
        private string string_2;

        public CDOBitmap()
        {
        }

        public CDOBitmap(ChartPane pane, DateTime dateTime_0, double value) : base(pane, dateTime_0, value)
        {
            this.method_0();
        }

        public override void ChangeSettings(UserControl userControl_0)
        {
            BitmapSettings settings = userControl_0 as BitmapSettings;
            this.ImagePath = settings.ImagePath;
            this.Transparent = settings.Transparent;
            this.Transparency = settings.Transparency;
            this.theImage = Image.FromFile(this.ImagePath);
        }

        public override UserControl GetSettingsUI()
        {
            if (this.bitmapSettings_0 == null)
            {
                this.bitmapSettings_0 = new BitmapSettings();
            }
            this.bitmapSettings_0.ImagePath = this.ImagePath;
            this.bitmapSettings_0.Transparent = this.bool_2;
            this.bitmapSettings_0.Transparency = this.Transparency;
            return this.bitmapSettings_0;
        }

        protected override bool IsMouseOver(int int_1, int int_2)
        {
            if (this.theImage != null)
            {
                Rectangle rectangle = new Rectangle(this._origin.X - Chart.PixelSensitivity, this._origin.Y - Chart.PixelSensitivity, this.theImage.Width + Chart.PixelSensitivity, this.theImage.Height + Chart.PixelSensitivity);
                if (rectangle.Contains(int_1, int_2))
                {
                    return true;
                }
            }
            return false;
        }

        private void method_0()
        {
            Stream stream = null;
            OpenFileDialog dialog = new OpenFileDialog {
                InitialDirectory = Path.GetDirectoryName(Application.ExecutablePath),
                Filter = "Image Files(*.BMP;*.JPG;*.JPEG;*.GIF;*.PNG;*.TIF)|*.BMP;*.JPG;*.JPEG;*.GIF;*.PNG;*.TIF|All files (*.*)|*.*",
                FilterIndex = 1,
                RestoreDirectory = true
            };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    stream = dialog.OpenFile();
                    if (stream == null)
                    {
                        return;
                    }
                    this.string_2 = dialog.FileName;
                    using (stream)
                    {
                        this.image_0 = Image.FromStream(stream);
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            throw new Exception("No file selected.");
        }

        protected override void OnSelected(int int_1, int int_2)
        {
        }

        protected override void Read(BinaryReader binaryReader_0)
        {
            try
            {
                base.Read(binaryReader_0);
                this.ImagePath = binaryReader_0.ReadString();
                this.Transparent = binaryReader_0.ReadBoolean();
                this.Transparency = binaryReader_0.ReadInt32();
                base._mover = base.Handles[0];
            }
            catch
            {
            }
        }

        public override void ReadSettings(ISettingsHost host)
        {
            string str = "DrawObj." + base.GetType().Name + ".";
            if (this.ImagePath == null)
            {
                this.ImagePath = host.Get(str + "ImagePath", "");
            }
            this.Transparent = host.Get(str + "Transparent", false);
            this.Transparency = host.Get(str + "Transparency", 0);
        }

        protected override void Render(Graphics graphics_0)
        {
            if ((base._mover.Bar != -1) && (this.theImage != null))
            {
                if (((!(this.theImage.RawFormat.Guid == ImageFormat.Bmp.Guid) && !(this.theImage.RawFormat.Guid == ImageFormat.Gif.Guid)) && (!(this.theImage.RawFormat.Guid == ImageFormat.Jpeg.Guid) && !(this.theImage.RawFormat.Guid == ImageFormat.Png.Guid))) && !(this.theImage.RawFormat.Guid == ImageFormat.Tiff.Guid))
                {
                    graphics_0.DrawImage(this.theImage, base._mover.X, base._mover.Y);
                }
                else
                {
                    Bitmap image = new Bitmap(this.theImage);
                    if (this.Transparent)
                    {
                        Color pixel = image.GetPixel(0, 0);
                        image.MakeTransparent(pixel);
                        graphics_0.DrawImage(image, base._mover.X, base._mover.Y);
                    }
                    else if (this.Transparency != 100)
                    {
                        float[][] numArray = new float[5][];
                        float[] numArray2 = new float[5];
                        numArray2[0] = 1f;
                        numArray[0] = numArray2;
                        float[] numArray3 = new float[5];
                        numArray3[1] = 1f;
                        numArray[1] = numArray3;
                        float[] numArray4 = new float[5];
                        numArray4[2] = 1f;
                        numArray[2] = numArray4;
                        float[] numArray5 = new float[5];
                        numArray5[3] = 1f;
                        numArray[3] = numArray5;
                        float[] numArray6 = new float[5];
                        numArray6[4] = 1f;
                        numArray[4] = numArray6;
                        float[][] newColorMatrix = numArray;
                        ColorMatrix matrix = new ColorMatrix(newColorMatrix);
                        int num = (int) ((100 - this.Transparency) * 2.55M);
                        matrix.Matrix33 = ((float) num) / 255f;
                        ImageAttributes imageAttr = new ImageAttributes();
                        imageAttr.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Default);
                        graphics_0.DrawImage(image, new Rectangle(base._mover.X, base._mover.Y, image.Width, image.Height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, imageAttr);
                    }
                }
                this._origin.X = base._mover.X;
                this._origin.Y = base._mover.Y;
            }
        }

        protected override void Write(BinaryWriter binaryWriter_0)
        {
            base.Write(binaryWriter_0);
            binaryWriter_0.Write(this.ImagePath);
            binaryWriter_0.Write(this.Transparent);
            binaryWriter_0.Write(this.Transparency);
        }

        public override void WriteSettings(ISettingsHost host)
        {
            string str = "DrawObj." + base.GetType().Name + ".";
            host.Set(str + "ImagePath", this.ImagePath);
            host.Set(str + "Transparent", this.bool_2);
            host.Set(str + "Transparency", this.Transparency);
        }

        protected override DrawingObjectHelper Helper
        {
            get
            {
                return drawingObjectHelper_0;
            }
        }

        public string ImagePath
        {
            get
            {
                return this.string_2;
            }
            set
            {
                this.string_2 = value;
            }
        }

        public Image theImage
        {
            get
            {
                if ((this.image_0 == null) && (this.string_2 != null))
                {
                    this.image_0 = Image.FromFile(this.string_2);
                }
                return this.image_0;
            }
            set
            {
                this.image_0 = value;
            }
        }

        public int Transparency
        {
            get
            {
                return this.int_0;
            }
            set
            {
                this.int_0 = value;
            }
        }

        public bool Transparent
        {
            get
            {
                return this.bool_2;
            }
            set
            {
                this.bool_2 = value;
            }
        }
    }
}

