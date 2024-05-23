using Microsoft.AspNetCore.Mvc;
using WhatInBus.Croppers;
using WhatInBus.Database;
using WhatInBus.Repository;

namespace WhatInBus.Controllers
{
    [ApiController]
    [Route("history")]
    public class HistoryController : Controller
    {
        private IRepository<Recognize> _rep;

        public HistoryController(IRepository<Recognize> repository)
        {
            _rep = repository;
        }

        [HttpGet("{id}")]
        public IActionResult GetOne([FromRoute] int id)
        {
            try
            {
                var result = _rep.GetOne(id);

                if (result == null) return NotFound();
                else return Json(result);
            }
            catch (Exception)
            {
                return StatusCode(500, "База данных недоступна.");
            }
        }

        [HttpGet("all")]
        public IActionResult GetAll()
        {
            try
            {
                var results = _rep.GetAll();

                List<HistoryWithBase64> list = new List<HistoryWithBase64>();
                foreach (var result in results)
                {
                    list.Add(new HistoryWithBase64
                    {
                        Id = (int)result.Id!,
                        Result = result.Result,
                        Image = Convert.ToBase64String(result.Image)
                    });
                }

                return Json(list);
            }
            catch (Exception)
            {
                return StatusCode(500, "База данных недоступна.");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var result = _rep.Delete(id);

                if (result) return Ok();
                else return NotFound();
            }
            catch (Exception)
            {
                return StatusCode(500, "База данных недоступна.");
            }
        }

        [HttpPost]
        public IActionResult Create([FromBody] Recognize recognize)
        {
            if (recognize == null) return BadRequest("Не удалось корректно распарсить объект в теле запроса.");

            try
            {
                var result = _rep.Create(recognize);

                if (result) return Ok();
                else throw new Exception("Ошибка создания записи в БД. Возможные причины: Id добавляемой записи уже существует или БД недоступна.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpPut("{id}")]
        public IActionResult Update([FromBody] Recognize recognize, [FromRoute] int id)
        {
            if (recognize == null) return BadRequest("Не удалось корректно распарсить объект в теле запроса.");

            try
            {
                var record = _rep.GetOne(id);

                if (record == null) return NotFound();

                record.Image = recognize.Image;
                record.Result = recognize.Result;

                var result = _rep.Update(record);

                if (result) return Ok();
                else return BadRequest();
            }
            catch (Exception)
            {
                return StatusCode(500, "Внутренняя ошибка.");
            }
        }

        public class HistoryWithBase64
        {
            public int Id { get; set; }
            public string Image { get; set; }
            public int Result { get; set; }
        }
    }
}
