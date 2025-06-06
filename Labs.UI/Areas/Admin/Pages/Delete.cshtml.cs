using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Labs.Domain.Entities;
using Labs.UI.Data;
using Labs.UI.Services;

namespace Labs.UI.Areas.Admin.Pages
{
    public class DeleteModel(IProductService productService) : PageModel
    {
        public PetFood PetFood { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var result = await productService.GetProductByIdAsync(id);
            if (!result.Success || result.Data is null)
            {
                return RedirectToPage("./Index");
            }                

            PetFood = result.Data;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            await productService.DeleteProductAsync(id);
            return RedirectToPage("./Index");
        }
    }
}
