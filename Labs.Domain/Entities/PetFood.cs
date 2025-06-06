using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Labs.Domain.Entities
{
    public class PetFood: Entity
    {        
        public string? Description { get; set; } // Описание корма для животных
        public decimal Price { get; set; } // Цена корма для животных
        public string? Image { get; set; } // Путь к файлу изображения

        // Навигационные свойства
        /// <summary>
        /// группа блюд (Например, корм для кошек, корм для собак и т.д.)
        /// </summary>
        public int CategoryId { get; set; }        
        public Category? Category { get; set; }
    }
}
