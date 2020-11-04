using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace eShopSolution.Data.Entities
{
    public class Cart
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        [Column(TypeName = "decimal(18, 0)")]
        public decimal Price { get; set; }
        public DateTime DateCreated { get; set; }

        public Guid UserId { get; set; }
        public AppUser AppUser { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
