using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace projeto.Areas.Admin.Controllers
{
    [Authorize(Roles="Admin")]
    public class AdminController : Controller
    {
        [Area("Admin")]
        public IActionResult Index(){
            return View();
        }        
    }
}