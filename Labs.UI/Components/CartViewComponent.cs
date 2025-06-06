using Labs.Domain.Models;
using Labs.UI.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Labs.UI.Components
{
    public class CartViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var cart = HttpContext.Session.Get<Cart>("cart");
            return View(cart);
        }
    }
}
