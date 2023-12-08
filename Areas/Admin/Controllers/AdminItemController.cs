
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using projeto.Context;
using projeto.Models;
using ReflectionIT.Mvc.Paging;

namespace projeto.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class AdminItemController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ConfiguraImagem _confImg;
        private readonly IWebHostEnvironment _hostingEnvireoment;

        // Constructor
        public AdminItemController(AppDbContext context,
            IWebHostEnvironment hostEnvironment, IOptions<ConfiguraImagem> confImg)
        {
            _context = context;
            _confImg = confImg.Value;
            _hostingEnvireoment = hostEnvironment;
        }

        // GET: Admin/AdminItem
        public async Task<IActionResult> Index(string filtro, int pageindex = 1, string sort = "Nome")

        {
            var itenslist =

            _context.Itens.AsNoTracking().AsQueryable();

            if (filtro != null)
            {
                itenslist = itenslist.Where(p => p.Nome.ToLower().Contains(filtro.ToLower()));

            }
            var model = await PagingList.CreateAsync(itenslist, 5,

            pageindex, sort, "Name");

            model.RouteValue = new RouteValueDictionary{{"filtro", filtro

}};

            return View(model);
        }

        // GET: Admin/AdminItem/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            // Check if id is null or if there are no items in the database
            if (id == null || _context.Itens == null)
            {
                return NotFound();
            }

            // Get the item with the specified id
            var item = await _context.Itens
                .Include(i => i.Categoria)
                .FirstOrDefaultAsync(m => m.ItemId == id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // GET: Admin/AdminItem/Create
        public IActionResult Create()
        {
            // Populate the dropdown list with categories
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "CategoriaId", "Nome");
            return View();
        }

        // POST: Admin/AdminItem/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ItemId,Nome,DescricaoCurta,DescricaoDetalhada,Preco,ImagemPequenaUrl,ImagemUrl,Ativo,Destaque,CategoriaId")] Item item, IFormFile Imagem, IFormFile Imagemcurta)
        {
            // Check if the uploaded image is valid and save it to the specified folder
            if (Imagem != null)
            {
                string imagemr = await SalvarArquivo(Imagem);
                item.ImagemUrl = imagemr;
            }
            if (Imagemcurta != null)
            {
                string imagemcr = await SalvarArquivo(Imagemcurta);
                item.ImagemPequenaUrl = imagemcr;
            }

            // Check if the model is valid
            if (ModelState.IsValid)
            {
                _context.Add(item);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "CategoriaId", "Nome", item.CategoriaId);
            return View(item);
        }

        // GET: Admin/AdminItem/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            // Check if id is null or if there are no items in the database
            if (id == null || _context.Itens == null)
            {
                return NotFound();
            }

            // Get the item with the specified id
            var item = await _context.Itens.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "CategoriaId", "Nome", item.CategoriaId);
            return View(item);
        }

        // POST: Admin/AdminItem/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ItemId,Nome,DescricaoCurta,DescricaoDetalhada,Preco,ImagemPequenaUrl,ImagemUrl,Ativo,Destaque,CategoriaId")] Item item, IFormFile Imagem, IFormFile Imagemcurta)
        {
            // Check if the id is valid and if the item exists in the database
            if (id != item.ItemId)
            {
                return NotFound();
            }

            // Check if the uploaded image is valid and save it to the specified folder
            if (Imagem != null)
            {
                Deletefile(item.ImagemUrl);
                string imagemr = await SalvarArquivo(Imagem);
                item.ImagemUrl = imagemr;
            }
            if (Imagemcurta != null)
            {
                Deletefile(item.ImagemPequenaUrl);
                string imagemcr = await SalvarArquivo(Imagemcurta);
                item.ImagemPequenaUrl = imagemcr;
            }

            // Check if the model is valid
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(item);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ItemExists(item.ItemId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "CategoriaId", "Nome", item.CategoriaId);
            return View(item);
        }

        // GET: Admin/AdminItem/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            // Check if id is null or if there are no items in the database
            if (id == null || _context.Itens == null)
            {
                return NotFound();
            }

            // Get the item with the specified id
            var item = await _context.Itens
                .Include(i => i.Categoria)
                .FirstOrDefaultAsync(m => m.ItemId == id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // POST: Admin/AdminItem/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Check if the items set in the database is null
            if (_context.Itens == null)
            {
                return Problem("Entity set 'AppDbContext.Itens' is null.");
            }

            // Get the item with the specified id
            var item = await _context.Itens.FindAsync(id);
            if (item != null)
            {
                try
                {
                    _context.Itens.Remove(item);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException ex)
                {
                    if (ex.InnerException.ToString().Contains("FOREIGN KEY"))
                    {
                        ViewData["Erro"] = "Esse item não pode ser deletada pois está sendo utilizada em um item";
                        return View();
                    }
                    else
                    {
                        ViewData["Erro"] = "Erro desconhecido!";
                        return View();
                    }
                }

                // Delete the image files from the specified folder
                Deletefile(item.ImagemPequenaUrl);
                Deletefile(item.ImagemUrl);
                _context.Itens.Remove(item);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ItemExists(int id)
        {
            // Check if the item with the specified id exists in the database
            return (_context.Itens?.Any(e => e.ItemId == id)).GetValueOrDefault();
        }

        // Save the uploaded image to the specified folder
        public async Task<string> SalvarArquivo(IFormFile Imagem)
        {
            var filePath = Path.Combine(_hostingEnvireoment.WebRootPath,
                _confImg.NomePastaImagemItem);

            if (Imagem.FileName.Contains(".jpg") || Imagem.FileName.Contains(".gif")
                || Imagem.FileName.Contains(".svg") || Imagem.FileName.Contains(".png"))
            {
                string novoNome =
                    $"{Guid.NewGuid()}.{Path.GetExtension(Imagem.FileName)}";

                var fileNameWithPath = string.Concat(filePath, "\\", novoNome);
                using (var stream = new FileStream(fileNameWithPath,
                    FileMode.Create))
                {
                    await Imagem.CopyToAsync(stream);
                }
                return "~/" + _confImg.NomePastaImagemItem + "/" + novoNome;
            }
            return null;
        }

        // Delete the specified image file from the specified folder
        public void Deletefile(string fname)
        {
            if (fname != null)
            {
                int pi = fname.LastIndexOf("/") + 1;
                int pf = fname.Length - pi;
                string nomearquivo = fname.Substring(pi, pf);
                try
                {
                    string _imagemDeleta = Path.Combine(_hostingEnvireoment.WebRootPath,
                        _confImg.NomePastaImagemItem + "\\", nomearquivo);
                    if ((System.IO.File.Exists(_imagemDeleta)))
                    {
                        System.IO.File.Delete(_imagemDeleta);
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
    }
}
//
//This code is a controller for managing items in an e-commerce application. It includes methods for creating, editing, deleting, and displaying items. The code also includes methods for handling image uploads and deletions.