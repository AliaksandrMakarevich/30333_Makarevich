using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labs.Domain.Models
{
    public class ResponseData<T>
    {       
        public T? Data { get; set; }  // Запрашиваемые данные
        public bool Success { get; set; } = true; // Признак успешного завершения запроса
        public string? ErrorMessage { get; set; } // Сообщение в случае неуспешного завершения
    }
}
