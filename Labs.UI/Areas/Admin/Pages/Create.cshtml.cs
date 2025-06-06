using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Labs.Domain.Entities;
using Labs.UI.Data;
using Labs.UI.Services;

namespace Labs.UI.Areas.Admin.Pages
{    
    public class CreateModel(ICategoryService categoryService, IProductService productService) : PageModel
    {       
        public async Task<IActionResult> OnGetAsync()
        {
            var categoryListData = await categoryService.GetCategoryListAsync();
            ViewData["CategoryId"] = new SelectList(categoryListData.Data, "Id", "Name");
            return Page();
        }

        [BindProperty]
        public PetFood PetFood { get; set; } = default!;

        [BindProperty]
        public IFormFile? Image { get; set; }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            
            await productService.CreateProductAsync(PetFood, Image);

            return RedirectToPage("./Index");
        }
    }
}
