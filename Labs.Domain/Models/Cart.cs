using Labs.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labs.Domain.Models
{
    public class Cart
    {
        public int Id { get; set; }
        /// <summary>
        /// Список объектов в корзине
        /// key - идентификатор объекта
        /// </summary>
        public Dictionary<int, CartItem> CartItems { get; set; } = new();
        /// <summary>
        /// Добавить объект в корзину
        /// </summary>
        /// <param name="petFood">Добавляемый объект</param>
        public virtual void AddToCart(PetFood petFood)
        {
            if (CartItems.ContainsKey(petFood.Id))
            {
                CartItems[petFood.Id].Quantity++;
            }
            else
            {
                CartItems.Add(petFood.Id, new CartItem
                {
                    Item = petFood,
                    Quantity = 1
                });
            }           
        }
        /// <summary>
        /// Удалить объект из корзины
        /// </summary>
        /// <param name="petFood">удаляемый объект</param>
        public virtual void RemoveItem(int id)
        {
            CartItems.Remove(id);
        }

        /// <summary>
        /// Увеличить количество объекта в корзине
        /// </summary>
        public void IncreaseQuantity(int id)
        {
            if (CartItems.ContainsKey(id))
            {
                CartItems[id].Quantity++;
            }
        }

        /// <summary>
        /// Уменьшить количество объекта в корзине (и удалить, если стало 0)
        /// </summary>
        public void DecreaseQuantity(int id)
        {
            if (CartItems.ContainsKey(id))
            {
                CartItems[id].Quantity--;
                if (CartItems[id].Quantity <= 0)
                {
                    CartItems.Remove(id);
                }
            }
        }

        /// <summary>
        /// Очистить корзину
        /// </summary>
        public virtual void ClearAll()
        {
            CartItems.Clear();
        }
        /// <summary>
        /// Количество объектов в корзине
        /// </summary>
        public int Count { get => CartItems.Sum(item => item.Value.Quantity); }
        /// <summary>
        /// Общая стоимость всех объектов в корзине
        /// </summary>        
        public decimal TotalPrice => CartItems.Sum(item => item.Value.Item.Price * item.Value.Quantity);

        
    }
}
