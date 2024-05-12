using System.Drawing;

namespace WhatInBus
{
    public class Cropper : ICropper
    {
        public byte[] CropImage(byte[] image, Rectangle area)
        {
            if (image == null) throw new ArgumentNullException("image");

            Bitmap source;

            try
            {
                using (Stream s = new MemoryStream(image))
                {
                    source = new Bitmap(s);
                }

                var result = new Bitmap(area.Width, area.Height);

                using (var g = Graphics.FromImage(result))
                {
                    g.DrawImage(source, 0, 0, area, GraphicsUnit.Pixel);
                }

                ImageConverter converter = new ImageConverter();
                return (byte[])converter.ConvertTo(result, typeof(byte[]));
            }
            catch (Exception ex)
            {
                throw ex;
            }            
        }
    }
}
