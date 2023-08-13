using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiAutores.Entidades;

namespace WebApiAutores.Controllers
{
    [ApiController]
    [Route("api/autores")]
    public class AutoresController : ControllerBase
    {
        private readonly ApplicationDBContext _dbContext;

        public AutoresController(ApplicationDBContext context)
        {

            this._dbContext = context;
        }

        [HttpGet]
        [HttpGet("listado")]
        [HttpGet("/listado")]
        public async Task<ActionResult<List<Autor>>> Get()
        {
            return await _dbContext.Autores.Include(x => x.Libros).ToListAsync();
        }
        [HttpGet("{id:int}/{param2=persona}")]
        public async Task<ActionResult<Autor>> Get(int id, string param2)
        {
            var autor = await _dbContext.Autores.FirstOrDefaultAsync(x => x.Id == id);
            if (autor == null)
            {
                return NotFound();
            }
            return autor;
        }
        //[HttpGet("{id:int}/{param2=persona}")]
        //public ActionResult<Autor> Get(int id, string param2)
        //{
        //    var autor =  _dbContext.Autores.FirstOrDefault(x => x.Id == id);
        //    if (autor == null)
        //    {
        //        return NotFound();
        //    }
        //    return autor;
        //}
        [HttpGet("{nombre}")]
        public async Task<ActionResult<Autor>> Get(string nombre)
        {
            var autor = await _dbContext.Autores.FirstOrDefaultAsync(x => x.Name.Contains(nombre));

            if (autor == null)
            {
                return NotFound();
            }
            return autor;
        }
        [HttpGet("primerAutor")]//api/autores/primerAutor
        public async Task<ActionResult<Autor>> PrimerAutor()
        {
            return await _dbContext.Autores.FirstOrDefaultAsync();
        }
        [HttpPost]
        public async Task<ActionResult> Post(Autor autor)
        {
            _dbContext.Add(autor);
            await _dbContext.SaveChangesAsync();
            return Ok();
        }
        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(Autor autor, int id)
        {
            if (autor.Id != id)
            {
                return BadRequest("El id del autor no coincide con el id de la URL");
               
            }
            _dbContext.Update(autor);
            await _dbContext.SaveChangesAsync();
            return Ok();
        }
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var exists = await _dbContext.Autores.AnyAsync(x => x.Id == id);

            if(!exists) 
            { 
                return NotFound();
            }
            _dbContext.Remove(new Autor() { Id = id });
            await _dbContext.SaveChangesAsync();
            return Ok();
        }
    }
}
