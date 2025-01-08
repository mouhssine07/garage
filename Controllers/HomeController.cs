using garage.Areas.Identity.Data;
using garage.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace garage.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, UserManager<IdentityUser> userManager, ApplicationDbContext context)
        {
            _logger = logger;
            _userManager = userManager;
            _context = context;
        }

        public IActionResult Index()
        {
            var cats = _context.Categories.ToList();
            var products = _context.Products.ToList();

            var model = new Tuple<List<Category>, List<Product>>(cats, products);
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> Cart()
        {
            var userId = _userManager.GetUserId(User);
            var cartItems = await _context.Carts
                .Include(c => c.Product)
                .Where(c => c.UserId == userId)
                .ToListAsync();

            return View(cartItems);
        }

        public IActionResult Categories()
        {
            var cats = _context.Categories.ToList();
            return View(cats);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId)
        {
            var userId = _userManager.GetUserId(User);
            var cartItem = await _context.Carts
                .FirstOrDefaultAsync(c => c.UserId == userId && c.ProductId == productId);

            if (cartItem == null)
            {
                cartItem = new Cart
                {
                    UserId = userId,
                    ProductId = productId,
                    Qty = 1
                };
                _context.Carts.Add(cartItem);
            }
            else
            {
                cartItem.Qty += 1;
            }

            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}


