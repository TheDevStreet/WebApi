using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiAutores.Entidades;

namespace WebApiAutores.Controllers
{
    [ApiController]
    [Route("api/libros")]
    public class LibrosController : ControllerBase
    {
        private readonly ApplicationDBContext _dbContext;

        public LibrosController(ApplicationDBContext dbContext)
        {
            this._dbContext = dbContext;
        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Libro>> Get(int id)
        {
            return await _dbContext.Libros.Include(x=>x.Autor).FirstOrDefaultAsync(x => x.Id == id);
        }
        [HttpPost]
        public async Task<ActionResult> Post(Libro libro)
        {
            var existsAutor = await _dbContext.Autores.AnyAsync(x => x.Id ==  libro.AutorId);
            if (!existsAutor)
            {
                return BadRequest($"No existe el autor id: {libro.AutorId}");
            }
            _dbContext.Add(libro);
            await _dbContext.SaveChangesAsync();
            return Ok();
        }
    }
}
