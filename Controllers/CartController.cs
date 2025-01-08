using garage.Areas.Identity.Data;
using garage.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace garage.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _context;

        public CartController(UserManager<IdentityUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(int productId)
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
            return RedirectToAction("Cart", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var cartItem = await _context.Carts
                .Include(c => c.Product)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (cartItem == null)
            {
                return NotFound();
            }
            return View(cartItem);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cartItem = await _context.Carts.FindAsync(id);
            if (cartItem == null)
            {
                return NotFound();
            }
            _context.Carts.Remove(cartItem);
            await _context.SaveChangesAsync();
            return RedirectToAction("Cart", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateQuantity(int cartId, string action)
        {
            var cartItem = await _context.Carts.FindAsync(cartId);
            if (cartItem != null)
            {
                if (action == "increase")
                {
                    cartItem.Qty += 1;
                }
                else if (action == "decrease" && cartItem.Qty > 1)
                {
                    cartItem.Qty -= 1;
                }
                await _context.SaveChangesAsync();
                return Ok();
            }

            return BadRequest("Cart item not found");
        }

        [HttpPost]
        public async Task<IActionResult> SaveCart()
        {
            var userId = _userManager.GetUserId(User);
            var cartItems = await _context.Carts
                .Where(c => c.UserId == userId)
                .ToListAsync();

            // Implement logic to save the cart to the database if needed
            // For example, you can update the status of the cart items or create an order

            await _context.SaveChangesAsync();
            return Ok();
        }

        public async Task<IActionResult> ViewCart()
        {
            var userId = _userManager.GetUserId(User);
            var cartItems = await _context.Carts
                .Include(c => c.Product)
                .Where(c => c.UserId == userId)
                .ToListAsync();

            return View(cartItems);
        }
    }
}







