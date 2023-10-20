using Microsoft.AspNetCore.Mvc;
using projeto.Models;


namespace projeto.ViewModel
{
    public class HomeViewModel 
    {
        public IEnumerable<Item> ItensEmDestaque {get; set;}
    }
}