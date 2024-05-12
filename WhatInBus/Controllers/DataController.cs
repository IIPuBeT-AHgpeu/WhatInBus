using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WhatInBus.Controllers
{
    [ApiController]
    [Route("data")]
    public class DataController : Controller
    {
        private IFileManager<ImageInDataset> _fileManager;
        private ICropper _cropper;

        public DataController()
        {
            _fileManager = new ImageFileManager();
            _cropper = new Cropper();
        }

        [HttpGet("crop_all")]
        public async Task<IActionResult> CropAll([FromQuery] string path, [FromQuery] uint x, [FromQuery] uint y, [FromQuery] string pathToSave)
        {
            if (path == null) return BadRequest("path is null");
            if (pathToSave == null) return BadRequest("pathToSave is null");
            if (x == null) return BadRequest("x is null");
            if (y == null) return BadRequest("y is null");

            try
            {
                List<ImageInDataset> images;
                var files = await _fileManager.GetFilesAsync(path);
                images = files.ToList();

                List<ImageInDataset> croppedImages = new List<ImageInDataset>();
                foreach (var image in images)
                {
                    ImageInDataset im = new ImageInDataset() { FileName = x.ToString() + "_" + y.ToString() + "_" + image.FileName, Path = pathToSave };
                    im.Data = _cropper.CropImage(image.Data, new System.Drawing.Rectangle((int)x, (int)y, 500, 500));

                    croppedImages.Add(im);
                }

                _fileManager.SaveFilesAsync(croppedImages);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
