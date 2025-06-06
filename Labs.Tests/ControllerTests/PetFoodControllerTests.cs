using Labs.API.Data;
using Labs.API.Controllers;
using Labs.Domain.Entities;
using Labs.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Microsoft.AspNetCore.Hosting;
using System.Data.Common;
using Microsoft.Data.Sqlite;

namespace Labs.Tests.ControllerTests
{
    public class PetFoodControllerTests : IDisposable
    {
        private readonly DbConnection _connection;
        private readonly DbContextOptions<AppDbContext> _contextOptions;
        private readonly IWebHostEnvironment _environment;
        public PetFoodControllerTests()
        {
            _environment = Substitute.For<IWebHostEnvironment>();
            _connection = new SqliteConnection("Filename=:memory:");
            _connection.Open();
            _contextOptions = new DbContextOptionsBuilder<AppDbContext>().UseSqlite(_connection).Options;

            using var context = new AppDbContext(_contextOptions);
            context.Database.EnsureCreated();

            var categories = new Category[]
            {
                new Category {Name= "Корм для кошек", NormalizedName="cats"},
                new Category {Name= "Корм для собак", NormalizedName="dogs"}
            };

            context.Categories.AddRange(categories);
            context.SaveChanges();

            var petFoods = new List<PetFood>
            {
                new PetFood { Name = "Whiskas", Description = "Корм для кошек", Category = categories.First(c => c.NormalizedName == "cats") },
                new PetFood { Name = "Purina", Description = "Корм для кошек", Category = categories.First(c => c.NormalizedName == "cats") },
                new PetFood { Name = "Pedigree", Description = "Корм для собак", Category = categories.First(c => c.NormalizedName == "dogs") },
                new PetFood { Name = "Royal Canin", Description = "Корм для собак", Category = categories.First(c => c.NormalizedName == "dogs") },
                new PetFood { Name = "Nutra Nuggets", Description = "Корм для собак", Category = categories.First(c => c.NormalizedName == "dogs") }
            };

            context.AddRange(petFoods);
            context.SaveChanges();
        }
        public void Dispose() => _connection?.Dispose();
        AppDbContext CreateContext() => new AppDbContext(_contextOptions);

        // Проверка фильтра по категории
        [Fact]
        public async Task ControllerFiltersCategory()
        {
            // arrange
            using var context = CreateContext();
            var category = context.Categories.First();
            var controller = new PetFoodsController(context, _environment);

            // act
            var response = await controller.GetPetFoods(category.NormalizedName);

            Assert.NotNull(response.Value);
            ResponseData<ProductListModel<PetFood>> responseData = response.Value;

            Assert.NotNull(responseData.Data);
            var petFoodsList = responseData.Data.Items;         // полученный список объектов

            //assert
            Assert.True(petFoodsList.All(d => d.CategoryId == category.Id));
        }

        // Проверка подсчета количества страниц
        // Первый параметр - размер страницы
        // Второй параметр - ожидаемое количество страниц (при условии, что всего объектов 5)
        [Theory]
        [InlineData(2, 3)]
        [InlineData(3, 2)]
        public async Task ControllerReturnsCorrectPagesCount(int size, int quantity)
        {
            using var context = CreateContext();
            var controller = new PetFoodsController(context, _environment);

            // act
            var response = await controller.GetPetFoods(null, 1, size);

            Assert.NotNull(response.Value);
            ResponseData<ProductListModel<PetFood>> responseData = response.Value;

            Assert.NotNull(responseData.Data);
            var totalPages = responseData.Data.TotalPages;     // полученное количество страниц

            //assert
            Assert.Equal(quantity, totalPages);                // количество страниц совпадает
        }
        [Fact]
        public async Task ControllerReturnsCorrectPage()
        {
            using var context = CreateContext();
            var controller = new PetFoodsController(context, _environment);

            // Первый объект на второй странице
            PetFood firstItem = context.PetFoods.ToArray()[3];

            // act
            // Получить данные 2-й страницы
            var response = await controller.GetPetFoods(null, 2);

            Assert.NotNull(response.Value);
            ResponseData<ProductListModel<PetFood>> responseData = response.Value;

            Assert.NotNull(responseData.Data);
            var petFoodsList = responseData.Data.Items;         // полученный список объектов
            var currentPage = responseData.Data.CurrentPage;    // полученный номер текущей страницы

            // assert
            Assert.Equal(2, currentPage);                       // номер страницы совпадает
            Assert.Equal(2, petFoodsList.Count);                // количество объектов на странице равно 2
            Assert.Equal(firstItem.Id, petFoodsList[0].Id);     // 1-й объект в списке правильный
        }
    }
}
