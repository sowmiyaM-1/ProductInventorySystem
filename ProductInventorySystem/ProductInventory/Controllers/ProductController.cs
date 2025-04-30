using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductInventory.Data;
using ProductInventory.Models;

namespace ProductInventory.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;
        
        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Categories
        public async Task<IActionResult> Index()
        {
            var _Product = await _context.Product.OrderByDescending(x => x.Id).ToListAsync();
            if (_Product.Count < 1) await CreateTestData();
            return _context.Product != null ? View(_Product) : Problem("Entity set 'ApplicationDbContext.Category'  is null.");
        }
        


        // GET: Categories/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null || _context.Product == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Categories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Quantity,Price")] Product product )
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null || _context.Product == null)
            {
                return NotFound();
            }

            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Name,Description,Quantity,Price")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
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
            return View(product);
        }

        [HttpDelete]
public async Task<IActionResult> Delete(long id)
{
    var product = await _context.Product.FindAsync(id);
    if (product == null)
    {
        return NotFound(new { success = false, message = "Product not found" });
    }

    _context.Product.Remove(product);
    await _context.SaveChangesAsync();

    return Json(new { success = true, id = id });
}


        

    

    
        private bool ProductExists(long id)
        {
            return (_context.Product?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private async Task CreateTestData()
        {
            foreach (var item in GetProductList())
            {
                _context.Product.Add(item);
                await _context.SaveChangesAsync();
            }
        }

        private IEnumerable<Product> GetProductList()
        {
            return new List<Product>
            {
                new Product { Name = "Books", Description = "Stationaries" },
                new Product { Name = "Jewels", Description = "Ornaments" }
               
            };
        }

    }
}
