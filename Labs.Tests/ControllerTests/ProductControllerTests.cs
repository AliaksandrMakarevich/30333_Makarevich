using Labs.Domain.Entities;
using Labs.Domain.Models;
using Labs.UI.Controllers;
using Labs.UI.Services;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labs.Tests.ControllerTests
{
    public class ProductControllerTests
    {
        IProductService _productService;
        ICategoryService _categoryService;
        public ProductControllerTests()
        {
            _categoryService = Substitute.For<ICategoryService>();
            _productService = Substitute.For<IProductService>();
            SetupData();
        }

        // Список категорий сохраняется во ViewData
        [Fact]
        public async Task IndexPutsCategoriesToViewData()
        {
            //arrange
            var controller = new ProductController(_categoryService, _productService);
            //act
            var response = await controller.Index(null);
            //assert
            var view = Assert.IsType<ViewResult>(response);
            var categories = Assert.IsType<List<Category>>(view.ViewData["categories"]);
            Assert.Equal(6, categories.Count);
            Assert.Equal("Все", view.ViewData["currentCategory"]);
        }
        // Имя текущей категории сохраняется во ViewData
        [Fact]
        public async Task IndexSetsCorrectCurrentCategory()
        {
            //arrange
            var categories = await _categoryService.GetCategoryListAsync();
            Assert.NotNull(categories.Data);
            var currentCategory = categories.Data[0];
            var controller = new ProductController(_categoryService, _productService);
            //act
            var response = await controller.Index(currentCategory.NormalizedName);
            //assert
            var view = Assert.IsType<ViewResult>(response);
            Assert.Equal(currentCategory.Name, view.ViewData["currentCategory"]);
        }
        // В случае ошибки возвращается NotFoundObjectResult
        [Fact]
        public async Task IndexReturnsNotFound()
        {
            //arrange
            string errorMessage = "Test error";
            var categoriesResponse = new ResponseData<List<Category>>();
            categoriesResponse.Success = false;
            categoriesResponse.ErrorMessage = errorMessage;
            _categoryService.GetCategoryListAsync().Returns(Task.FromResult(categoriesResponse));
            var controller = new ProductController(_categoryService, _productService);
            //act
            var response = await controller.Index(null);
            //assert
            var result = Assert.IsType<NotFoundObjectResult>(response);
            Assert.NotNull(result.Value);
            Assert.Equal(errorMessage, result.Value.ToString());
        }

        // Настройка имитации ICategoryService и IProductService
        void SetupData()
        {
            _categoryService = Substitute.For<ICategoryService>();
            var categoriesResponse = new ResponseData<List<Category>>();
            categoriesResponse.Data = new List<Category>
        {
            new Category { Id = 1, Name = "Корм для кошек", NormalizedName = "cats" },
            new Category { Id = 2, Name = "Корм для собак", NormalizedName = "dogs" },
            new Category { Id = 3, Name = "Корм для грызунов", NormalizedName = "rodents" },
            new Category { Id = 4, Name = "Корм для птиц", NormalizedName = "birds" },
            new Category { Id = 5, Name = "Корм для рыб", NormalizedName = "fishes" },
            new Category { Id = 6, Name = "Корм для рептилий", NormalizedName = "reptiles" }
        };
            _categoryService.GetCategoryListAsync().Returns(Task.FromResult(categoriesResponse));
            _productService = Substitute.For<IProductService>();
            var petfood = new List<PetFood>
        {
            new PetFood { Id = 1 },
            new PetFood { Id = 2 },
            new PetFood { Id = 3 },
            new PetFood { Id = 4 },
            new PetFood { Id = 5 }
        };
            var productResponse = new ResponseData<ProductListModel<PetFood>>();
            productResponse.Data = new ProductListModel<PetFood> { Items = petfood };
            _productService.GetProductListAsync(Arg.Any<string?>(), Arg.Any<int>())
            .Returns(productResponse);
        }
    }
}