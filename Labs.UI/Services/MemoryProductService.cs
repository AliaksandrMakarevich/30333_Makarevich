using Labs.Domain.Entities;
using Labs.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace Labs.UI.Services
{
    public class MemoryProductService : IProductService
    {
        private List<PetFood> _petfood = new();
        private readonly IConfiguration _config;
        private List<Category> _categories = new();
        public MemoryProductService([FromServices] IConfiguration config, ICategoryService categoryService)
        {
            _config = config; 
            _categories = categoryService.GetCategoryListAsync().Result.Data ?? new List<Category>();
            SetupData();
        }        

        private void SetupData()
        {
            _petfood = new List<PetFood>
            {
                // Кошки
                new PetFood { Id = 1, Name = "Whiskas с курицей", Description = "Сбалансированный сухой корм для взрослых кошек.", Price = 7.10m, Image = "Images/whiskas.jpg", CategoryId = _categories.Find(c => c.NormalizedName == "cats")?.Id ?? throw new InvalidOperationException("Категория 'cats' не найдена") },
                new PetFood { Id = 2, Name = "Purina One с говядиной", Description = "Полнорационный корм с высоким содержанием белка.", Price = 8.68m, Image = "Images/purina.jpg", CategoryId = _categories.Find(c => c.NormalizedName == "cats")?.Id ?? throw new InvalidOperationException("Категория 'cats' не найдена") },

                // Собаки
                new PetFood { Id = 3, Name = "Pedigree с ягненком", Description = "Питательный корм для собак всех пород, с натуральными ингредиентами.", Price = 11.68m, Image = "Images/pedigree.jpg", CategoryId = _categories.Find(c => c.NormalizedName == "dogs")?.Id ?? throw new InvalidOperationException("Категория 'dogs' не найдена")},
                new PetFood { Id = 4, Name = "Royal Canin Medium", Description = "Профессиональный корм для собак средних пород.", Price = 15.20m, Image = "Images/royalcanin.jpg", CategoryId = _categories.Find(c => c.NormalizedName == "dogs")?.Id ?? throw new InvalidOperationException("Категория 'dogs' не найдена") },

                // Грызуны
                new PetFood { Id = 5, Name = "Vitakraft Emotion Beauty", Description = "Корм для хомяков с витаминами и клетчаткой.", Price = 3.55m, Image = "Images/vitakraft.jpg", CategoryId = _categories.Find(c => c.NormalizedName == "rodents")?.Id ?? throw new InvalidOperationException("Категория 'rodents' не найдена") },
                new PetFood { Id = 6, Name = "Little One для морских свинок", Description = "Микс с фруктами и овощами для морских свинок.", Price = 2.02m, Image = "Images/littleone.jpg", CategoryId = _categories.Find(c => c.NormalizedName == "rodents")?.Id ?? throw new InvalidOperationException("Категория 'rodents' не найдена") },
                
                // Птицы
                new PetFood { Id = 7, Name = "RIO Daily Feed", Description = "Ежедневный рацион для волнистых попугаев.", Price = 4.10m, Image = "Images/rio.jpg", CategoryId = _categories.Find(c => c.NormalizedName == "birds") ?.Id ?? throw new InvalidOperationException("Категория 'birds' не найдена") },
                new PetFood { Id = 8, Name = "Versele-Laga Prestige", Description = "Смесь зерен для канареек и экзотических птиц.", Price = 3.37m, Image = "Images/versele.jpg", CategoryId = _categories.Find(c => c.NormalizedName == "birds") ?.Id ?? throw new InvalidOperationException("Категория 'birds' не найдена") },
                
                // Рыбы
                new PetFood { Id = 9, Name = "TetraMin Flakes", Description = "Хлопья для всех декоративных рыб, включая экзотических.", Price = 17.62m, Image = "Images/tetra.jpg", CategoryId = _categories.Find(c => c.NormalizedName == "fishes") ?.Id ?? throw new InvalidOperationException("Категория 'fishes' не найдена") },
                new PetFood { Id = 10, Name = "Sera Goldy", Description = "Корм для золотых рыбок с натуральными добавками.", Price = 16.42m, Image = "Images/sera.jpg", CategoryId = _categories.Find(c => c.NormalizedName == "fishes") ?.Id ?? throw new InvalidOperationException("Категория 'fishes' не найдена") },

                // Рептилии
                new PetFood { Id = 11, Name = "ReptiGourmet Turtle", Description = "Сушёные креветки для водных черепах, с высоким содержанием белка.", Price = 17.32m, Image = "Images/reptigourmet.jpg", CategoryId = _categories.Find(c => c.NormalizedName == "reptiles") ?.Id ?? throw new InvalidOperationException("Категория 'reptiles' не найдена") },
                new PetFood { Id = 12, Name = "Exo Terra Mealworms", Description = "Сушеные мучные черви для ящериц и гекконов.", Price = 13.26m, Image = "Images/exoterra.jpg", CategoryId = _categories.Find(c => c.NormalizedName == "reptiles") ?.Id ?? throw new InvalidOperationException("Категория 'reptiles' не найдена") }
            };  
        }

        public Task<ResponseData<PetFood>> CreateProductAsync(PetFood product, IFormFile? formFile)
        {
            throw new NotImplementedException();
        }

        public Task DeleteProductAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseData<PetFood>> GetProductByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseData<ProductListModel<PetFood>>> GetProductListAsync(string? categoryNormalizedName, int pageNo = 1)
        {
            var result = new ResponseData<ProductListModel<PetFood>>();
            int? categoryId = _categories.FirstOrDefault(c => c.NormalizedName == categoryNormalizedName)?.Id ?? null;
            var data = _petfood.Where(p => categoryId == null || p.CategoryId == categoryId).ToList();
            
            // Получить размер страницы из конфигурации
            int pageSize = _config.GetSection("ItemsPerPage").Get<int>();
           
            // Получить общее количество страниц
            int totalPages = (int)Math.Ceiling(data.Count / (double)pageSize);
            
            // Получить данные страницы
            var listData = new ProductListModel<PetFood>()
            {
                Items = data.Skip((pageNo - 1) * pageSize).Take(pageSize).ToList(),
                CurrentPage = pageNo,
                TotalPages = totalPages
            };

            // Поместить данные в объект результата
            result.Data = listData;

            // Если список пустой
            if (data.Count == 0)
            {
                result.Success = false;
                result.ErrorMessage = "Нет объектов в выбранной категории";
            }

            // Вернуть результат
            return Task.FromResult(result);
        }

        public Task UpdateProductAsync(int id, PetFood product, IFormFile? formFile)
        {
            throw new NotImplementedException();
        }
    }
}
