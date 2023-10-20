using System.ComponentModel.DataAnnotations;

namespace projeto.Models
{
    public class Item
    {
        public int ItemId {get;set;}
        [Display(Name = "Nome do item")]
        [Required(ErrorMessage ="Informe o nome do item")]
        [MinLength(4, ErrorMessage ="Nome deve ter no minimo {1} caracteres")]
        [MaxLength(30, ErrorMessage ="Nome deve ter no máximo {1} caracteres")]
        public string Nome {get;set;}
        [Display(Name = "Descrição curta")]
        [Required(ErrorMessage ="Informe a descrição curta")]
        [MinLength(4, ErrorMessage ="Descrição deve ter no minimo {1} caracteres")]
        [MaxLength(50, ErrorMessage ="Descrição deve ter no máximo {1} caracteres")]
        public string DescricaoCurta{get;set;}
        [Display(Name = "Descrição detalhada")]
        [Required(ErrorMessage ="Informe a descrição detalhada")]
        [MinLength(20, ErrorMessage ="Descrição deve ter no minimo {1} caracteres")]
        [MaxLength(200, ErrorMessage ="Descrição deve ter no máximo {1} caracteres")]
        public string DescricaoDetalhada{get;set;}
        [Display(Name = "Preço")]
        [Required(ErrorMessage ="Informe o Preço")]
        public double Preco{get;set;}
        [Display(Name ="Imagem pequena")]
        public string ImagemPequenaUrl{get;set;}
        [Display(Name ="Imagem normal")]
        public string ImagemUrl{get;set;}
        public bool Ativo{get;set;}
        public bool Destaque{get;set;}
        [Display(Name ="Categoria")]
        public int CategoriaId{get;set;}
        public virtual Categoria Categoria{get;set;}
    }
}