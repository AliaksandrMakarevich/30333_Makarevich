using Labs.Domain.Entities;
using Labs.Domain.Models;

namespace Labs.UI.Services
{
    public class ApiCategoryService(HttpClient httpClient) : ICategoryService
    {
        public async Task<ResponseData<List<Category>>> GetCategoryListAsync()
        {
            // Отправка GET-запроса к базовому адресу API (например: https://localhost:7002/api/categories)
            var result = await httpClient.GetAsync(httpClient.BaseAddress);

            // Если ответ успешный (статус 200 OK) — читаем тело ответа как ResponseData<List<Category>>
            if (result.IsSuccessStatusCode)
            {
                return await result.Content.ReadFromJsonAsync<ResponseData<List<Category>>>();
            }

            // Если запрос не удался — возвращаем объект с ошибкой
            var response = new ResponseData<List<Category>>
            { 
                Success = false, 
                ErrorMessage = "Ошибка чтения API" 
            };

            return response;
        }
    }
}
