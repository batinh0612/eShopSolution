using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace eShopSolution.Data.Entities
{
    public class Product
    {
        public int Id { get; set; }
        [Column(TypeName = "decimal(18, 0)")]
        public decimal Price { get; set; }
        [Column(TypeName = "decimal(18, 0)")]
        public decimal OriginalPrice { get; set; }
        public int Stock { get; set; }
        public int ViewCount { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool? IsFeatured { get; set; }


        public List<ProductInCategory> ProductInCategories { get; set; }

        public List<OrderDetail> OrderDetails { get; set; }

        public List<Cart> Carts { get; set; }

        public List<ProductTranslation> ProductTranslations { get; set; }

        public List<ProductImage> ProductImages { get; set; }


    }
}
