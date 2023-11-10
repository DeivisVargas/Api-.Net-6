﻿using ApiCatalogo.Context;
using ApiCatalogo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        [HttpGet("{id:int}" , Name ="ObterProduto")] // rota nomeada 
        public ActionResult<Produto> Get(int id)
        {
            var produto = _context.Produtos.FirstOrDefault( p => p.ProdutoId == id);
            if (produto is null)
            {
                return NotFound("Produto não encontrado");
            }
            return produto;
        }


        //rota POST para inserir o produto 
        [HttpPost]
        public ActionResult Post(Produto produto)
        {
            if (produto is null)
                return BadRequest();

            //espera receber os dados no padrão do modelo
            _context.Produtos.Add(produto); //inclui no contexto sem inserir no banco
            _context.SaveChanges(); // sauva na tabela do banco 

            //retorna status 201 e chama a rota de consultar ObterProduto por id
            //para devolver os dados do produto 
            return new CreatedAtRouteResult
                (
                    "ObterProduto",
                    new { id = produto.ProdutoId } ,
                    produto
                );



        }

        //Atualiza produto aponas uma atualização completa , se passar somente uma informação por exemplo
        //não vai funcionar 
        [HttpPut("{id:int}", Name = "AlterarProduto")]
        public ActionResult Put(int id ,Produto produto)
        {
            if ( id != produto.ProdutoId)
            {
                return BadRequest();
            }

            //fala ao contexto que o produto vai ser alterado 
            //no contexto desconectado
            _context.Entry(produto).State = EntityState.Modified;
            _context.SaveChanges(); //salva os dados

            //return NoContent(); retorna um ok mas sem os dados do produto

            //retorna os dados do produto
            return Ok(produto);

        }

        [HttpDelete("{id:int}", Name ="DeletarProduto")]
        public ActionResult Delete(int id)
        {
            var produto = _context.Produtos.FirstOrDefault(p => p.ProdutoId == id);
            if (produto is null)
                return NotFound("Produto não localizado...");

            _context.Produtos.Remove(produto);
            _context.SaveChanges();

            return Ok(produto);
        
        
        }
        
    }
}
