using Microsoft.AspNetCore.Mvc;
using projeto.Models;

namespace projeto.ViewModel
{
    public class CarrinhoViewModel
    {
        public Carrinho Carrinho { get; set; }
        public double CarrinhoTotal { get; set; }
    }
}