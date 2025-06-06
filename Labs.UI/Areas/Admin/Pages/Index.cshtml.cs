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
using Microsoft.AspNetCore.Authorization;
using Azure;

namespace Labs.UI.Areas.Admin.Pages
{
    [Authorize(Policy = "admin")]
    public class IndexModel : PageModel
    {
        private readonly IProductService productService;

        public IndexModel(IProductService productService)
        {
            this.productService = productService;
        }

        public List<PetFood> PetFood { get;set; } = default!;
        public int CurrentPage { get; set; } = 1;
        public int TotalPages { get; set; } = 1;

        public async Task OnGetAsync(int? pageNo = 1)
        {
            int page = pageNo ?? 1;
            var response = await productService.GetProductListAsync(null, page);
            if (response.Success && response.Data != null)
            {
                PetFood = response.Data.Items;
                CurrentPage = response.Data.CurrentPage;
                TotalPages = response.Data.TotalPages;
            }
        }
    }
}
