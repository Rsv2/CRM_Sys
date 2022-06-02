using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace Desktop
{
    /// <summary>
    /// Изменение размера изображения.
    /// </summary>
    public static class PicsResizer
    {
        /// <summary>
        /// Результат: массив байт.
        /// </summary>
        private static byte[] Output { get; set; }
        /// <summary>
        /// Ширина
        /// </summary>
        public static int Width { get; set; }
        /// <summary>
        /// Высота
        /// </summary>
        public static int Height { get; set; }
        /// <summary>
        /// Размер (байт)
        /// </summary>
        public static int Size { get; set; }

        /// <summary>
        /// Сократить размер изображения до 1280x720.
        /// </summary>
        /// <param name="file">Исходное изображение</param>
        /// <returns>Изменённое изображение (массив байт)</returns>
        public static byte[] GetDatas(string file, double width, double height)
        {
            Bitmap bm = (Bitmap)Image.FromFile(file);
            Width = bm.Width;
            Height = bm.Height;
            Size = File.ReadAllBytes(file).Length;

            if (bm.Width > width || bm.Height > height)
            {
                double h = bm.Height / height;
                double w = bm.Width / width;
                double factor = 1.00;
                if (h > w) { factor = h; }
                else if (w >= h) { factor = w; }
                Width = Convert.ToInt32(bm.Width / factor);
                Height = Convert.ToInt32(bm.Height / factor);
                Bitmap resized = ResizeImage(bm, Width, Height);
                Graphics g = Graphics.FromImage(resized);
                g.DrawImage(bm, new Rectangle(0, 0, Width, Height), 0, 0, bm.Width, bm.Height, GraphicsUnit.Pixel);
                g.Dispose();
                using (var stream = new MemoryStream())
                {
                    resized.Save(stream, ImageFormat.Png);
                    Output = stream.ToArray();
                    Size = Output.Length;
                }
            }
            else { Output = File.ReadAllBytes(file); }
            return Output;
        }

        /// <summary>
        /// Resize the image to the specified width and height.
        /// </summary>
        /// <param name="image">The image to resize.</param>
        /// <param name="width">The width to resize to.</param>
        /// <param name="height">The height to resize to.</param>
        /// <returns>The resized image.</returns>
        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }
    }
}
