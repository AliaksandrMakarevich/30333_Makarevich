using Labs.UI.Services;
using Microsoft.AspNetCore.Mvc;

namespace Labs.UI.Controllers
{
    public class ProductController(ICategoryService categoryService, IProductService productService) : Controller
    {
        [Route("Catalog")]
        [Route("Catalog/{category}")]
        [Route("Product")]
        [Route("Product/{category}")]
        public async Task<IActionResult> Index(string? category, int pageNo = 1)
        {
            // Получение списка категорий
            var categoriesResponse = await categoryService.GetCategoryListAsync();
            if (!categoriesResponse.Success) return NotFound(categoriesResponse.ErrorMessage);

            ViewData["categories"] = categoriesResponse.Data;

            // Имя текущей категории (или "Все")
            var currentCategory = category == null || categoriesResponse.Data == null ? "Все" : categoriesResponse.Data.FirstOrDefault(c => c.NormalizedName == category)?.Name ?? "Все";

            ViewData["currentCategory"] = currentCategory;

            // Получение товаров
            var productResponse = await productService.GetProductListAsync(category, pageNo);
            if (!productResponse.Success) ViewData["Error"] = productResponse.ErrorMessage;

            return View(productResponse.Data);  
        }
    }
}
