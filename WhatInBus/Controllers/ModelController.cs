using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using WhatInBus.Croppers;
using WhatInBus.Repository;
using WhatInBus.Database;

namespace WhatInBus.Controllers
{
    [ApiController]
    [Route("model")]
    public class ModelController : ControllerBase
    {
        private ICropper<Rectangle> _cropper;
        private IRepository<Recognize> _rep;

        public ModelController(ICropper<Rectangle> cropper, IRepository<Recognize> repository) 
        {
            _cropper = cropper; 
            _rep = repository;
        }

        [HttpPost("use")]
        public async Task<IActionResult> Use(IFormFile file)
        {
            if (file == null) return BadRequest("file is null");

            byte[] bytes;

            try
            {
                using (var ms = new MemoryStream())
                {
                    file.CopyTo(ms);

                    bytes = ms.ToArray();
                }

                FullCroppedClassificationModel.ModelInput sampleData = new FullCroppedClassificationModel.ModelInput()
                {
                    ImageSource = bytes,
                };

                var sortedScoresWithLabel = FullCroppedClassificationModel.PredictAllLabels(sampleData);

                Recognize record = new Recognize();

                record.Image = bytes;
                record.ModelName = "FullCroppedClassificationModel";
                record.Result = GetMaxInKeyValuePair(sortedScoresWithLabel);
                record.Date = DateOnly.FromDateTime(DateTime.Now);

                bool isSuccesfully = _rep.Create(record);

                if (isSuccesfully) return Ok(sortedScoresWithLabel);
                else return StatusCode(500, "Error, can not save recognize in history...");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error on the server side: " + ex.Message);
            }        
        }

        [HttpPost("use/cropping")]
        public async Task<IActionResult> UseWithCropping(IFormFile file, [FromQuery] int x, [FromQuery] int y)
        {
            if (file == null) return BadRequest("file is null");

            if (x < 0 || y < 0) return BadRequest("X and Y need to be a non-negative numbers");

            byte[] image;

            try
            {
                using (var ms = new MemoryStream())
                {
                    file.CopyTo(ms);

                    image = ms.ToArray();
                }

                byte[] cropped_image = _cropper.CropImage(image, new Rectangle(new Point(x, y), new Size(500, 500)));

                FullCroppedClassificationModel.ModelInput sampleData = new FullCroppedClassificationModel.ModelInput()
                {
                    ImageSource = cropped_image,
                };

                var sortedScoresWithLabel = FullCroppedClassificationModel.PredictAllLabels(sampleData);

                Recognize record = new Recognize();

                record.Image = cropped_image;
                record.ModelName = "FullCroppedClassificationModel";
                record.Result = GetMaxInKeyValuePair(sortedScoresWithLabel);
                record.Date = DateOnly.FromDateTime(DateTime.Now);

                bool isSuccesfully = _rep.Create(record);

                if (isSuccesfully) return Ok(sortedScoresWithLabel);
                else return StatusCode(500, "Error, can not save recognize in history...");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error on the server side: " + ex.Message);
            }
        }

        private int GetMaxInKeyValuePair(IOrderedEnumerable<KeyValuePair<string, float>> data)
        {
            string category = "";
            float max = 0;

            foreach (var score in data)
            {
                if (score.Value > max)
                {
                    max = score.Value;
                    category = score.Key;
                }
            }

            switch (category)
            {
                case "High":
                    {
                        return 3;
                    }
                case "Medium":
                    {
                        return 2;
                    }
                case "Low":
                    {
                        return 1;
                    }
                default:
                    {
                        return 0;
                    }
            }
        }
    }
}
