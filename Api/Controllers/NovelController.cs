using Api.Data.Collections;
using Api.Models;
using Microsoft.AspNetCore.Mvc;

using MongoDB.Driver;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NovelController : ControllerBase
    {
        Data.MongoDB _mongoDB;
        IMongoCollection<Novel> _novelCollection;
        
        public NovelController(Data.MongoDB mongoDB)
        {
            _mongoDB = mongoDB;
            _novelCollection = _mongoDB.DB.GetCollection<Novel>(typeof(Novel).Name.ToLower());

        }

        [HttpPost]
        public ActionResult Biblioteca([FromBody] NovelDto dto)
        {
            var novel = new Novel(dto.Id, dto.Autor, dto.Genero, dto.Nome);

            _novelCollection.InsertOne(novel);

            return StatusCode(201, "Novel adicionado com sucesso");
        }

        [HttpGet]
        public ActionResult ObterNovels()
        {
            var novel = _novelCollection.Find(Builders<Novel>.Filter.Empty).ToList();

            return Ok(novel);
        }

        [HttpPut]
        public ActionResult AtualizarNovel([FromBody] NovelDto dto)
        {
            var novel = new Novel(dto.Id, dto.Autor, dto.Genero, dto.Nome);
                
            _novelCollection.UpdateOne(Builders<Novel>.Filter.Where(_ => _.Id == dto.Id), Builders<Novel>.Update.Set("Id", dto.Id));

            return Ok("Atualizado com sucesso");
        }
    }
}