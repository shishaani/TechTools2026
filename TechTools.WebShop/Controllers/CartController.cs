using Microsoft.AspNetCore.Mvc;
using TechTools.Services;
using TechTools.WebShop.Extensions;
using TechTools.WebShop.Models;

namespace TechTools.WebShop.Controllers
{
    public class CartController(GPUService gpuService, CPUService cpuService) : Controller
    {
        private const string CartSessionKey = "ShoppingCart";
        private const string DiscountSessionKey = "DiscountCode";
        private static readonly Dictionary<string, decimal> ActiveDiscountCodes = new(StringComparer.OrdinalIgnoreCase)
        {
            ["Discount5"] = 5m,
            ["Tech10"] = 10m,
            ["Student15"] = 15m
        };

        public IActionResult Index()
        {
            string? discountCode = GetDiscountCode();

            return View(new CartViewModel
            {
                Items = GetCart(),
                DiscountCode = discountCode,
                DiscountAmount = GetDiscountAmount(discountCode)
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(int id)
        {
            var gpu = gpuService.Read(id);
            var cpu = cpuService.Read(id);
            if (gpu == null && cpu == null)
            {
                TempData["ErrorMessage"] = $"Geen GPU of CPU gevonden met id {id}.";
                return RedirectToAction("Index", "GPUs, CPUs");
            }

            var cart = GetCart();
            var cartItem = cart.FirstOrDefault(item => item.GPUId == id || item.CPUId == id);

            if (cartItem == null)
            {
                cart.Add(new CartItem
                {
                    // If GPU is not null, use its properties
                    GPUId = gpu.Id,
                    Brand = gpu.Brand,
                    Model = gpu.Model,
                    Price = gpu.Price,
                    Picture = gpu.Picture,

                    // If CPU is not null, use its properties
                    CPUId = cpu.Id,

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
            ClearDiscountCode();
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ApplyDiscount(string discountCode)
        {
            if (string.IsNullOrWhiteSpace(discountCode))
            {
                TempData["ErrorMessage"] = "Geef een kortingscode in.";
                return RedirectToAction("Index");
            }

            discountCode = discountCode.Trim();

            if (!ActiveDiscountCodes.ContainsKey(discountCode))
            {
                TempData["ErrorMessage"] = "Deze kortingscode is niet geldig.";
                return RedirectToAction("Index");
            }

            SaveDiscountCode(discountCode);
            TempData["SuccessMessage"] = $"Kortingscode {discountCode} toegepast.";

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RemoveDiscount()
        {
            ClearDiscountCode();
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

        private string? GetDiscountCode()
        {
            return HttpContext.Session.GetString(DiscountSessionKey);
        }

        private void SaveDiscountCode(string discountCode)
        {
            HttpContext.Session.SetString(DiscountSessionKey, discountCode);
        }

        private void ClearDiscountCode()
        {
            HttpContext.Session.Remove(DiscountSessionKey);
        }

        private static decimal GetDiscountAmount(string? discountCode)
        {
            return discountCode != null && ActiveDiscountCodes.TryGetValue(discountCode, out decimal discountAmount)
                ? discountAmount
                : 0m;
        }
    }
}
