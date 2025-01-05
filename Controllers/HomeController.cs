using garage.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace garage.Controllers
{
    public class HomeController : Controller
    {

        public IActionResult Index()
        {
            GarageContext db = new GarageContext();
            var cats = db.Categories.ToList();
            var products = db.Products.ToList();

            var model = new Tuple<List<Category>, List<Product>>(cats, products);
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Cart() {
            return View();
        }

        public IActionResult Categories() {

            GarageContext db = new GarageContext();
           var cats = db.Categories.ToList();
            return View(cats);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
