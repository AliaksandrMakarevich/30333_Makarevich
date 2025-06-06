using Labs.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labs.Domain.Models
{
    public class CartItem
    {
        public PetFood Item { get; set; } = null!;
        public int Quantity { get; set; }
    }
}
