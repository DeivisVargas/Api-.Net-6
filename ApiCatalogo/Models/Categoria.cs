using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiCatalogo.Models;
//iniciando o projeto no git e enviando

[Table("Categorias")]
public class Categoria
{

    //inicialisando a coleção de  produtos no construtor da classe 
    public Categoria() {
        Produtos = new Collection<Produto>();
    }

    [Key]
    public int CategoriaId { get; set; }

    //data Anotations para alterar os padrões dp EF core 
    [Required]
    [StringLength(80)]
    public string? Nome { get; set; }

    [Required]
    [StringLength(300)]
    public string? ImageUrl { get; set; }

    //definindo a relação de produto com categoria onde uma categoria pode ter muitos 
    //produtos
    public ICollection<Produto>? Produtos { get; set; }

}
