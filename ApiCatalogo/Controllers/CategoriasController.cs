
using ApiCatalogo.Context;
using ApiCatalogo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {

        private readonly AppDbContext _context;

        public CategoriasController(AppDbContext context)
        {
            _context = context;

        }

        [HttpGet]
        [Route("produtos")]
        public ActionResult<IEnumerable<Categoria>> GetCategoriasProdutos()
        {
            try
            {
                //.AsNoTracking() melhora a perfomace mas so posso fazer se tiver a certeza 
                //que não vai existir alteração, somente consulta 
                return _context.Categorias.Include(p => p.Produtos).AsNoTracking().ToList();
            }
            
            catch (Exception)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "Problema ao tratar solicitação");

            }
        }
        //IEnumerable interface que permite apenas leitura 
        [HttpGet]
        public ActionResult<IEnumerable<Categoria>> Get()
        {
            try
            {

                var categorias = _context.Categorias.ToList();
                if (categorias is null)
                {
                    return NotFound("Crodutos não encontrados");
                }
                return categorias;
            }
            catch (Exception)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "Problema ao tratar solicitação");
                
            }
           

            
            
        }

        //rota de pesquisa de categoria por id definindo inteiro 
        [HttpGet("{id:int}", Name = "ObterCategoria")] // rota nomeada 
        public ActionResult<Categoria> Get(int id)
        {
            try
            {
                var categoria = _context.Categorias.FirstOrDefault(p => p.CategoriaId == id);
                if (categoria is null)
                {
                    return NotFound($"Categoria de id ={id} não encontrado");
                }
                return Ok(categoria);
            }
                
            catch (Exception)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "Problema ao tratar solicitação");

            }
        }

        //rota POST para inserir o categoria 
        [HttpPost]
        public ActionResult Post(Categoria categoria)
        {
            try
            {
                if (categoria is null)
                    return BadRequest();

                //espera receber os dados no padrão do modelo
                _context.Categorias.Add(categoria); //inclui no contexto sem inserir no banco
                _context.SaveChanges(); // salva na tabela do banco 

                //retorna status 201 e chama a rota de consultar ObterProduto por id
                //para devolver os dados do produto 
                return new CreatedAtRouteResult
                    (
                        "ObterCategoria",
                        new { id = categoria.CategoriaId },
                        categoria
                    );
            }
            catch (Exception)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "Problema ao tratar solicitação");

            }



        }

        //Atualiza produto aponas uma atualização completa , se passar somente uma informação por exemplo
        //não vai funcionar 
        [HttpPut("{id:int}", Name = "AlterarCategoria")]
        public ActionResult Put(int id, Categoria categoria)
        {
            try
            {
                if (id != categoria.CategoriaId)
                {
                    return BadRequest();
                }

                //fala ao contexto que o produto vai ser alterado 
                //no contexto desconectado
                _context.Entry(categoria).State = EntityState.Modified;
                _context.SaveChanges(); //salva os dados

                //return NoContent(); retorna um ok mas sem os dados do produto

                //retorna os dados do produto
                return Ok(categoria);
            }
            catch (Exception)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "Problema ao tratar solicitação");

            }


        }

        [HttpDelete("{id:int}", Name = "DeletarCategoria")]
        public ActionResult Delete(int id)
        {
            try
            {
                var categoria = _context.Categorias.FirstOrDefault(p => p.CategoriaId == id);
                if (categoria is null)
                    return NotFound($"Categoria de id= {id} não localizada...");

                _context.Categorias.Remove(categoria);
                _context.SaveChanges();

                return Ok(categoria);
            }
            
            catch (Exception)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "Problema ao tratar solicitação");

            }


        }

    }
}
