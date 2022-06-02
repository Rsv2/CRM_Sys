using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace Site
{
    public static class ResizePic
    {
        public static byte[] Resize(IFormFile img, double width, double height)
        {
            using var image = Image.Load(img.OpenReadStream());
            if (image.Width > width || image.Height > height)
            {
                double h = image.Height / height;
                double w = image.Width / width;
                double factor = 1.00;
                if (h > w) { factor = h; }
                else if (w >= h) { factor = w; }
                image.Mutate(x => x.Resize(Convert.ToInt32(image.Width / factor), Convert.ToInt32(image.Height / factor)));
            }  
            using var imageStream = new MemoryStream();
            image.SaveAsPng(imageStream);
            return imageStream.ToArray();
        }
    }
}
