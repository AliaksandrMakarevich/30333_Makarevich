using Labs.Domain.Entities;
using Labs.Domain.Models;

namespace Labs.Blazor.Services
{
    public class ApiProductService(HttpClient Http) : IProductService<PetFood>
    {
        private List<PetFood> _petFoods = [];
        private int _currentPage = 1;
        private int _totalPages = 1;
        public IEnumerable<PetFood> Products => _petFoods;
        public int CurrentPage => _currentPage;
        public int TotalPages => _totalPages;
        public event Action? ListChanged;
        public async Task GetProducts(int pageNo, int pageSize)
        {            
            // Url сервиса API
            if (Http.BaseAddress == null) throw new InvalidOperationException("BaseAddress не задан в HttpClient");

            var uri = Http.BaseAddress.AbsoluteUri;

            // Данные для Query запроса
            var queryData = new Dictionary<string, string?>
            {
                { "pageNo", pageNo.ToString() },
                { "pageSize", pageSize.ToString() }
            };

            var query = QueryString.Create(queryData);

            // Отправить запрос http
            var result = await Http.GetAsync(uri + query.Value);

            // В случае успешного ответа
            if (result.IsSuccessStatusCode)
            {
                // Получить данные из ответа
                var responseData = await result.Content.ReadFromJsonAsync<ResponseData<ProductListModel<PetFood>>>();

                // Обновить параметры
                if (responseData?.Data != null)
                {
                    _currentPage = responseData.Data.CurrentPage;
                    _totalPages = responseData.Data.TotalPages;
                    _petFoods = responseData.Data.Items;
                    ListChanged?.Invoke();
                }
                else
                {                    
                    _petFoods = [];
                    _currentPage = 1;
                    _totalPages = 1;
                }
            }

            // В случае ошибки
            else
            {
                _petFoods = [];
                _currentPage = 1;
                _totalPages = 1;
            }
        }
    }
}