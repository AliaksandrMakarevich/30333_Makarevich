using Labs.Domain.Entities;
using Labs.Domain.Models;
using System.Text.Json;

namespace Labs.UI.Services
{
    public class ApiProductService(HttpClient httpClient) : IProductService
    {
        public async Task<ResponseData<PetFood>> CreateProductAsync(PetFood product, IFormFile? formFile)
        {
            var serializerOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            // Подготовить объект, возвращаемый методом
            var responseData = new ResponseData<PetFood>();

            // Послать запрос к API для сохранения объекта
            var response = await httpClient.PostAsJsonAsync(httpClient.BaseAddress, product);

            if (!response.IsSuccessStatusCode)
            {
                responseData.Success = false;
                responseData.ErrorMessage = $"Не удалось создать объект: {response.StatusCode}";
                return responseData;
            }            

            // Если файл изображения передан клиентом
            if (formFile != null)       
            {
                // Получить созданный объект из ответа Api-сервиса
                var petFood = await response.Content.ReadFromJsonAsync<PetFood>();
                responseData.Data = petFood!;

                // Создать объект запроса
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri($"{httpClient.BaseAddress!.AbsoluteUri}{petFood!.Id}")
                };

                // Создать контент типа multipart form-data
                var content = new MultipartFormDataContent();

                // Создать потоковый контент из переданного файла
                var streamContent = new StreamContent(formFile.OpenReadStream());

                // Добавить потоковый контент в общий контент по именем "image"
                content.Add(streamContent, "image", formFile.FileName);

                // Поместить контент в запрос
                request.Content = content;

                // Послать запрос к Api-сервису
                response = await httpClient.SendAsync(request);
                if (!response.IsSuccessStatusCode)
                {
                    responseData.Success = false;
                    responseData.ErrorMessage = $"Не удалось сохранить изображение: {response.StatusCode}";
                }
            }
            return responseData;
        }
        public async Task DeleteProductAsync(int id)
        {
            await httpClient.DeleteAsync($"{httpClient.BaseAddress}{id}");
        }

        public async Task<ResponseData<PetFood>> GetProductByIdAsync(int id)
        {
            var responseData = new ResponseData<PetFood>();

            // Отправка запроса к API
            var response = await httpClient.GetAsync($"{httpClient.BaseAddress}{id}");

            // Проверка кода ответа
            if (!response.IsSuccessStatusCode)
            {
                responseData.Success = false;
                responseData.ErrorMessage = $"Ошибка при получении объекта: {response.StatusCode}";
                return responseData;
            }

            // Чтение объекта из тела ответа
            var petFood = await response.Content.ReadFromJsonAsync<PetFood>();
            if (petFood == null)
            {
                responseData.Success = false;
                responseData.ErrorMessage = "Объект не найден";
                return responseData;
            }

            // Возврат результата
            responseData.Data = petFood;
            return responseData;
        }

        public async Task<ResponseData<ProductListModel<PetFood>>> GetProductListAsync(string? categoryNormalizedName, int pageNo = 1)
        {
            // Получение базового URI API-клиента (например: https://localhost:7002/api/petfoods)
            var uri = httpClient.BaseAddress;

            // Создание словаря с параметрами запроса
            var queryData = new Dictionary<string, string?>();
            queryData.Add("pageNo", pageNo.ToString()); // номер страницы

            // Если указана категория — добавляем её в параметры
            if (!String.IsNullOrEmpty(categoryNormalizedName))
            {
                queryData.Add("category", categoryNormalizedName); // фильтрация по категории
            }

            // Формирование строки запроса (например: ?pageNo=1&category=cats)
            var query = QueryString.Create(queryData);

            // Отправка GET-запроса к API с параметрами
            var result = await httpClient.GetAsync(uri + query.Value);

            // Если запрос выполнен успешно — читаем тело ответа как ResponseData<ProductListModel<PetFood>>
            if (result.IsSuccessStatusCode)
            {
                return await result.Content.ReadFromJsonAsync<ResponseData<ProductListModel<PetFood>>>()
                    ?? new ResponseData<ProductListModel<PetFood>> { Success = false, ErrorMessage = "Пустой ответ от API" };
            }

            // В случае ошибки возвращаем объект с флагом ошибки
            var response = new ResponseData<ProductListModel<PetFood>> 
            { 
                Success = false, 
                ErrorMessage = "Ошибка чтения API" 
            };

            return response;
        }

        public async Task UpdateProductAsync(int id, PetFood product, IFormFile? formFile)
        {           
            // Сначала отправим PUT-запрос для обновления объекта
            var uri = new Uri(httpClient.BaseAddress!, $"{id}");
            var putResponse = await httpClient.PutAsJsonAsync(uri, product);

            if (!putResponse.IsSuccessStatusCode)
            {
                throw new Exception($"Ошибка при обновлении объекта: {putResponse.StatusCode}");
            }

            // Если передано новое изображение — отправим его отдельно
            if (formFile != null)
            {
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri($"{httpClient.BaseAddress}{id}")
                };

                var content = new MultipartFormDataContent();
                var streamContent = new StreamContent(formFile.OpenReadStream());

                content.Add(streamContent, "image", formFile.FileName);
                request.Content = content;

                var imageResponse = await httpClient.SendAsync(request);

                if (!imageResponse.IsSuccessStatusCode)
                {
                    throw new Exception($"Ошибка при загрузке изображения: {imageResponse.StatusCode}");
                }
            }
        }
    }
}
