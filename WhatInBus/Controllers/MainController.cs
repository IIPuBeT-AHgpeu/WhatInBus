using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using static System.Collections.Specialized.BitVector32;

namespace WhatInBus.Controllers
{
    [ApiController]
    [Route("test")]
    public class MainController : ControllerBase
    {
        [HttpGet("get")]
        public async Task<IActionResult> Get([FromQuery] string path)
        {
            ImageFileManager m = new ImageFileManager();
            List<ImageInDataset> images;

            try
            {
                var result = await m.GetFilesAsync(path);

                images = result.ToList();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
            
            List<string> names = new List<string>();

            foreach(var img in images)
            {
                names.Add(img.FileName);
            }

            return Ok(names);
        }

        [HttpGet("test")]
        public async Task<IActionResult> Test([FromQuery] string path, [FromQuery] string pathToSave)
        {
            ImageFileManager m = new ImageFileManager();
            List<ImageInDataset> images;

            try
            {
                var result = await m.GetFilesAsync(path);

                images = result.ToList();

                ImageInDataset toSave = images[0];
                toSave.Path = pathToSave;

                Bitmap btm;

                using (Stream s = new MemoryStream(toSave.Data))
                {
                    btm = new Bitmap(s);
                }

                Rectangle rec = new Rectangle(new Point(0, 0), new Size(100, 100));

                var bitmap = new Bitmap(rec.Width, rec.Height);

                using (var g = Graphics.FromImage(bitmap))
                {
                    g.DrawImage(btm, 0, 0, rec, GraphicsUnit.Pixel);
                }

                ImageConverter converter = new ImageConverter();
                toSave.Data = (byte[])converter.ConvertTo(bitmap, typeof(byte[]));

                m.SaveFileAsync(toSave);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

            return Ok("Файл успешно сохранен.");
        }
    }
}
