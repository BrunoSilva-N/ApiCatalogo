using ApiCatalogo.Context;
using ApiCatalogo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalogo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CategoriasController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet ("produtos")]
        public ActionResult<IEnumerable<Categoria>> GetCategoriasProdutos()
        {
            // return  _context.Categorias.Include(p=> p.Produtos).ToList();
            //Nuca retorndar objetos relacionados sem aplicar um filtro
            return _context.Categorias.Include(p => p.Produtos).Where(c=> c.CategoriaId <=5).ToList();
        }


        [HttpGet]
        //AsNotracking é usado em consultas Get para otimizar o desempenho
        public ActionResult<IEnumerable<Categoria>> Get()
        {
            try
            {
                //throw new DataMisalignedException();
                 var categoria = _context.Categorias.AsNoTracking().ToList();
                 if (categoria is null)
                 {
                     return NotFound();
                 }
                return categoria;
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Ocorreu um problema ao tratar a sua solicitação");
            }
        }
        [HttpGet("{id:int}", Name = "ObtterCategoria")]
        public ActionResult<Categoria> Get(int id)
        {
            try
            {
                var categoria = _context.Produtos.FirstOrDefault(p => p.CategoriaId == id);
                if (categoria is null)
                {
                    return NotFound( $"Categoria com id={id}  nao encontrada");
                }
                return Ok(categoria);

            }
            catch (Exception)
            {


                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Ocorreu um problema ao tratar a sua solicitação");
            }
        }

        [HttpPost]
        public ActionResult Post(Categoria categoria)
        {
            if (categoria is null)
                return BadRequest("Dados invalidos");
            _context.Categorias.Add(categoria);
            _context.SaveChanges();

            return new CreatedAtRouteResult("ObterCategoria",
                 new { id = categoria.CategoriaId }, categoria);
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Categoria categoria)
        {
            if (id != categoria.CategoriaId)
            {
                return BadRequest("Dados invalidos");
            }

            _context.Entry(categoria).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok(categoria);
        }

        [HttpDelete("{id:int}")]
        public ActionResult<Categoria> Delete(int id)
        {
            var categoria = _context.Categorias.FirstOrDefault(p => p.CategoriaId == id);
            // var produto = _context.Produtos.Find(id);

            if (categoria is null)
            {
                return NotFound($"Categoria com id={id}  nao encontrada");

            }
            _context.Categorias.Remove(categoria);
            _context.SaveChanges();

            return Ok(categoria);

        }


    }

}
