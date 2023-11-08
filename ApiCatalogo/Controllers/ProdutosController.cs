using ApiCatalogo.Context;
using ApiCatalogo.Models;
using Microsoft.AspNetCore.Mvc;

namespace ApiCatalogo.Controllers
{
    [ApiController]
    [Route("[controller]")] //define que a rota terá o mesmo nome do controller sem a parte controller
    public class ProdutosController : Controller
    {

        //chamando a conexao
        private readonly AppDbContext _context;

        //construtor injetando dependência
        public ProdutosController(AppDbContext context)
        {
            _context = context;
        }


        //IEnumerable interface que permite apenas leitura 
        [HttpGet]
        public ActionResult<IEnumerable<Produto>> Get()
        {
            var produtos = _context.Produtos.ToList();
            if(produtos is null)
            {
                return NotFound("Produtos não encontrados");
            }
            return produtos;
        }

        //rota de pesquisa de produtos por id definindo inteiro 
        [HttpGet("{id:int}")]
        public ActionResult<Produto> Get(int id)
        {
            var produto = _context.Produtos.FirstOrDefault( p => p.ProdutoId == id);
            if (produto is null)
            {
                return NotFound("Produto não encontrado");
            }
            return produto;
        }
    }
}
