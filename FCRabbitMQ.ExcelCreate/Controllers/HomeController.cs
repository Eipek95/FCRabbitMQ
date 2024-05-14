using FCRabbitMQ.ExcelCreate.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace FCRabbitMQ.ExcelCreate.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AdventureWorks2019Context _context;

        public HomeController(ILogger<HomeController> logger, AdventureWorks2019Context context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Privacy()
        {


            var subCategory = await _context.ProductSubcategories.Include(x => x.ProductCategory).Include(x => x.Products).ToListAsync();


            var categoryCount = _context.Products.GroupBy(p => p.ProductSubcategory).Select(group => new ViewModel
            {
                ProductSubcategory = group.Key,
                Count = group.Count()
            }).ToList();



            return View(subCategory);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        public class ViewModel()
        {
            public ProductSubcategory ProductSubcategory { get; set; }
            public int Count { get; set; }
        }
    }
}
