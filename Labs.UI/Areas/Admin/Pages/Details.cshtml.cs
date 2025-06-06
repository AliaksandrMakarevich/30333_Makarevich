using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Labs.Domain.Entities;
using Labs.UI.Services;

namespace Labs.UI.Areas.Admin.Pages
{
    public class DetailsModel(IProductService productService) : PageModel
    {
        public PetFood PetFood { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }   
               
            var result = await productService.GetProductByIdAsync(id.Value);
            if (!result.Success || result.Data == null)
            {
                return NotFound();
            }                

            PetFood = result.Data;
            return Page();
        }
    }
}
