using Microsoft.AspNetCore.Mvc;
using TechTools.Services;
using TechTools.WebShop.Extensions;
using TechTools.WebShop.Models;

namespace TechTools.WebShop.Controllers
{
    public class CartController(GPUService gpuService) : Controller
    {
        private const string CartSessionKey = "ShoppingCart";

        public IActionResult Index()
        {
            return View(GetCart());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(int id)
        {
            var gpu = gpuService.Read(id);
            if (gpu == null)
            {
                TempData["ErrorMessage"] = $"Geen GPU gevonden met id {id}.";
                return RedirectToAction("Index", "GPUs");
            }

            var cart = GetCart();
            var cartItem = cart.FirstOrDefault(item => item.GPUId == id);

            if (cartItem == null)
            {
                cart.Add(new CartItem
                {
                    GPUId = gpu.Id,
                    Brand = gpu.Brand,
                    Model = gpu.Model,
                    Price = gpu.Price,
                    Picture = gpu.Picture,
                    Quantity = 1
                });
            }
            else
            {
                cartItem.Quantity++;
            }

            SaveCart(cart);
            TempData["SuccessMessage"] = $"{gpu.Brand} {gpu.Model} toegevoegd aan winkelwagen.";

            return RedirectToAction("Index", "GPUs");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateQuantity(int id, int quantity)
        {
            var cart = GetCart();
            var cartItem = cart.FirstOrDefault(item => item.GPUId == id);

            if (cartItem != null)
            {
                if (quantity <= 0)
                {
                    cart.Remove(cartItem);
                }
                else
                {
                    cartItem.Quantity = quantity;
                }

                SaveCart(cart);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Remove(int id)
        {
            var cart = GetCart();
            var cartItem = cart.FirstOrDefault(item => item.GPUId == id);

            if (cartItem != null)
            {
                cart.Remove(cartItem);
                SaveCart(cart);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Clear()
        {
            SaveCart(new List<CartItem>());
            return RedirectToAction("Index");
        }

        private List<CartItem> GetCart()
        {
            return HttpContext.Session.GetObject<List<CartItem>>(CartSessionKey) ?? [];
        }

        private void SaveCart(List<CartItem> cart)
        {
            HttpContext.Session.SetObject(CartSessionKey, cart);
        }
    }
}
