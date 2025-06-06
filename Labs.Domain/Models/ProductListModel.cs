using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labs.Domain.Models
{
    public class ProductListModel<T>
    {        
        public List<T> Items { get; set; } = new(); // Запрошенный список объектов       
        public int CurrentPage { get; set; } = 1;  // Номер текущей страницы        
        public int TotalPages { get; set; } = 1; // Общее количество страниц
    }
}
