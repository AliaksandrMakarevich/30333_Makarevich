using Labs.Domain.Models;
using Labs.UI.Extensions;
using Labs.UI.Services;
using Microsoft.AspNetCore.Mvc;

namespace Labs.UI.Controllers
{
    public class CartController : Controller
    {
        private readonly IProductService _productService;
        private Cart _cart;
        public CartController(IProductService productService)
        {
            _productService = productService;
        }
        // GET: CartController
        public ActionResult Index()
        {
            _cart = HttpContext.Session.Get<Cart>("cart") ?? new();
            return View(_cart.CartItems);
        }
        [Route("[controller]/add/{id:int}")]
        public async Task<ActionResult> Add(int id, string returnUrl)
        {
            var data = await _productService.GetProductByIdAsync(id);
            if (data.Success)
            {
                _cart = HttpContext.Session.Get<Cart>("cart") ?? new();
                _cart.AddToCart(data.Data);
                HttpContext.Session.Set<Cart>("cart", _cart);
            }
            return Redirect(returnUrl);
        }

        [Route("[controller]/remove/{id:int}")]
        public ActionResult Remove(int id)
        {
            _cart = HttpContext.Session.Get<Cart>("cart") ?? new();
            _cart.RemoveItem(id);
            HttpContext.Session.Set<Cart>("cart", _cart);
            return RedirectToAction("index");
        }

        [HttpPost]
        public IActionResult Increase(int id)
        {
            _cart = HttpContext.Session.Get<Cart>("cart") ?? new();
            _cart.IncreaseQuantity(id);
            HttpContext.Session.Set<Cart>("cart", _cart);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Decrease(int id)
        {
            _cart = HttpContext.Session.Get<Cart>("cart") ?? new();
            _cart.DecreaseQuantity(id);
            HttpContext.Session.Set<Cart>("cart", _cart);
            return RedirectToAction("Index");
        }

    }
}