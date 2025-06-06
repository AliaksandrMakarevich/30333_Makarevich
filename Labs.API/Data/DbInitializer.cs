using Labs.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Labs.API.Data
{
    public class DbInitializer
    {
        public static async Task SeedData(WebApplication app)
        {
            // Uri проекта
            var uri = "https://localhost:7002/";

            // Получение контекста БД
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            // Применение миграции
            await context.Database.MigrateAsync();

            // Заполнение данными
            if (!context.Categories.Any() && !context.PetFoods.Any())
            {
                var categories = new Category[]
                {
                    new Category { Name = "Корм для кошек", NormalizedName = "cats" },
                    new Category { Name = "Корм для собак", NormalizedName = "dogs" },
                    new Category { Name = "Корм для грызунов", NormalizedName = "rodents" },
                    new Category { Name = "Корм для птиц", NormalizedName = "birds" },
                    new Category { Name = "Корм для рыб", NormalizedName = "fishes" },
                    new Category { Name = "Корм для рептилий", NormalizedName = "reptiles" }
                };

                await context.Categories.AddRangeAsync(categories);
                await context.SaveChangesAsync();

                // Добавляем товары
                var petFoods = new List<PetFood>
                {
                    // Кошки
                    new PetFood()
                    {
                        Name = "Whiskas с курицей",
                        Description = "Сбалансированный корм с витаминами для взрослых кошек.",
                        Price = 7.10m,
                        Category = categories.FirstOrDefault(c => c.NormalizedName.Equals("cats")),
                        Image = uri + "Images/whiskas.jpg"
                    },
                    new PetFood()
                    {
                        Name = "Purina One с говядиной",
                        Description = "Питательный корм для кошек с высоким содержанием белка.",
                        Price = 8.68m,
                        Category = categories.FirstOrDefault(c => c.NormalizedName.Equals("cats")),
                        Image = uri + "Images/purina.jpg"
                    },

                    // Собаки
                    new PetFood() 
                    {
                        Name = "Pedigree с ягнёнком",
                        Description = "Питательный корм для собак всех пород, с натуральными ингредиентами.",
                        Price = 11.68m,
                        Category = categories.FirstOrDefault(c => c.NormalizedName.Equals("dogs")),
                        Image = uri + "Images/pedigree.jpg"
                    },
                    new PetFood()
                    {
                        Name = "Royal Canin Medium",
                        Description = "Профессиональный корм для собак средних пород.",
                        Price = 15.20m,
                        Category = categories.FirstOrDefault(c => c.NormalizedName.Equals("dogs")),
                        Image = uri + "Images/royalcanin.jpg"
                    },

                    // Грызуны
                    new PetFood()
                    {
                        Name = "Vitakraft Emotion Beauty",
                        Description = "Корм для хомяков с витаминами и клетчаткой.",
                        Price = 3.55m,
                        Category = categories.FirstOrDefault(c => c.NormalizedName.Equals("rodents")),
                        Image = uri + "Images/vitakraft.jpg"
                    },
                    new PetFood()
                    {
                        Name = "Little One для морских свинок",
                        Description = "Микс с фруктами и овощами для морских свинок.",
                        Price = 2.02m,
                        Category = categories.FirstOrDefault(c => c.NormalizedName.Equals("rodents")),
                        Image = uri + "Images/littleone.jpg"
                    },              
                
                    // Птицы
                    new PetFood()
                    {
                        Name = "RIO Daily Feed",
                        Description = "Ежедневный рацион для волнистых попугаев.",
                        Price = 4.10m,
                        Category = categories.FirstOrDefault(c => c.NormalizedName.Equals("birds")),
                        Image = uri + "Images/rio.jpg"
                    },
                    new PetFood()
                    {
                        Name = "Versele-Laga Prestige",
                        Description = "Смесь зерен для канареек и экзотических птиц.",
                        Price = 3.37m,
                        Category = categories.FirstOrDefault(c => c.NormalizedName.Equals("birds")),
                        Image = uri + "Images/versele.jpg"
                    },               
                
                    // Рыбы               
                    new PetFood()
                    {
                        Name = "TetraMin Flakes",
                        Description = "Хлопья для всех декоративных рыб, включая экзотических.",
                        Price = 17.62m,
                        Category = categories.FirstOrDefault(c => c.NormalizedName.Equals("fishes")),
                        Image = uri + "Images/tetra.jpg"
                    },
                    new PetFood()
                    {
                        Name = "Sera Goldy",
                        Description = "Корм для золотых рыбок с натуральными добавками.",
                        Price = 16.42m,
                        Category = categories.FirstOrDefault(c => c.NormalizedName.Equals("fishes")),
                        Image = uri + "Images/sera.jpg"
                    }, 

                    // Рептилии
                    new PetFood()
                    {
                        Name = "ReptiGourmet Turtle",
                        Description = "Сушёные креветки для водных черепах, с высоким содержанием белка.",
                        Price = 17.32m,
                        Category = categories.FirstOrDefault(c => c.NormalizedName.Equals("reptiles")),
                        Image = uri + "Images/reptigourmet.jpg"
                    },
                    new PetFood()
                    {
                        Name = "Exo Terra Mealworms",
                        Description = "Сушеные мучные черви для ящериц и гекконов.",
                        Price = 13.26m,
                        Category = categories.FirstOrDefault(c => c.NormalizedName.Equals("reptiles")),
                        Image = uri + "Images/exoterra.jpg"
                    }
                };
                await context.PetFoods.AddRangeAsync(petFoods);
                await context.SaveChangesAsync();
            }
        }
    }
}
