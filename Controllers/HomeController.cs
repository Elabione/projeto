using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using projeto.Models;
using projeto.Repositories.Interfaces;
using projeto.ViewModel;

namespace projeto.Controllers;

public class HomeController : Controller
{
    private readonly IItemRepository _itemRepository;

    public HomeController(IItemRepository itemRepository)
    {
        _itemRepository = itemRepository;
    }

    public IActionResult Index()
    {
        var homeViewModel = new HomeViewModel{
            ItensEmDestaque = _itemRepository.ItensEmDestaque
        };
        return View(homeViewModel);
    }

    public IActionResult Privacy()
    {
        
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

