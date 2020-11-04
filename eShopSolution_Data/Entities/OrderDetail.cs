using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace eShopSolution.Data.Entities
{
    public class OrderDetail
    {
        public int Quantity { get; set; }
        [Column(TypeName = "decimal(18, 0)")]
        public decimal Price { get; set; }

        public int OrderId { get; set; }
        public Order Order { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
