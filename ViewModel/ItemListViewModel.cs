using Microsoft.AspNetCore.Mvc;
using projeto.Models;

namespace projeto.ViewModel
{
    public class ItemListViewModel
    {
        public IEnumerable<Item> Itens {get;set;}
        public string CategoriaAtual {get;set;}
    }
}