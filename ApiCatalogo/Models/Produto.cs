using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ApiCatalogo.Models;


[Table("Produtos")]
public class Produto
{
    [Key] 
    public int ProdutoId { get; set; }

    [Required]
    [StringLength(80)]
    public string? Nome { get; set; }

    [Required]
    [StringLength(300)]
    public string? Descricao { get; set; }


    //especifica a precisão da coluna 
    [Column(TypeName ="decimal(10,2)")]
    [Required]
    public decimal? Preco { get; set;}

    [Required]
    [StringLength(300)]
    public string? ImagemUrl { get; set;}

    public float? Estoque { get; set; }
    public DateTime DataCadastro { get; set; }

    //cria a chave estrangeira da tabela Categoria
    public int? CategoriaId { get; set; }

    //Propriedade de navegação
    // [JsonIgnore] apenas para vc ignorar essa propriedade na serialização 
    // do json 

    public Categoria? Categoria { get; set; }   

}
