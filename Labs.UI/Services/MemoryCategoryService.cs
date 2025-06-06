using Labs.Domain.Entities;
using Labs.Domain.Models;

namespace Labs.UI.Services
{
    public class MemoryCategoryService : ICategoryService
    {
        public Task<ResponseData<List<Category>>> GetCategoryListAsync()
        {
            var categories = new List<Category>
            {
                new Category { Id = 1, Name = "Корм для кошек", NormalizedName = "cats" },
                new Category { Id = 2, Name = "Корм для собак", NormalizedName = "dogs" },
                new Category { Id = 3, Name = "Корм для грызунов", NormalizedName = "rodents" },
                new Category { Id = 4, Name = "Корм для птиц", NormalizedName = "birds" },
                new Category { Id = 5, Name = "Корм для рыб", NormalizedName = "fishes" },
                new Category { Id = 6, Name = "Корм для рептилий", NormalizedName = "reptiles" }                
            };
            var result = new ResponseData<List<Category>>();
            result.Data = categories;
            return Task.FromResult(result);
        }
    }
}