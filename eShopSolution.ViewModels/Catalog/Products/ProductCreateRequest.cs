using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace eShopSolution.ViewModels.Catalog.Products
{
    public class ProductCreateRequest
    {
        [Display(Name ="Giá bán")]
        [Range(1000, int.MaxValue, ErrorMessage ="Bạn phải nhập {0} trong khoảng {1} đến {2}")]
        public decimal Price { get; set; }

        [Display(Name = "Giá nhập")]
        [Range(1000, int.MaxValue, ErrorMessage = "Bạn phải nhập {0} trong khoảng {1} đến {2}")]
        public decimal OriginalPrice { get; set; }

        [Display(Name = "Tồn kho")]
        public int Stock { get; set; }

        [Display(Name = "Tên")]
        [Required(ErrorMessage = "Bạn phải nhập tên sản phẩm")]
        public string Name { get; set; }

        [Display(Name = "Mô tả")]
        [Required(ErrorMessage = "Bạn phải nhập mô tả sản phẩm")]
        public string Description { get; set; }

        [Display(Name = "Chi tiết")]
        [Required(ErrorMessage = "Bạn phải nhập chi tiết sản phẩm")]
        public string Details { get; set; }

        [Display(Name = "Mô tả seo")]
        [Required(ErrorMessage = "Bạn phải nhập mô tả seo")]
        public string SeoDescription { get; set; }

        [Display(Name = "Tiêu đề seo")]
        [Required(ErrorMessage = "Bạn phải nhập tiêu đề seo")]
        public string SeoTitle { get; set; }

        [Display(Name = "Bí danh seo")]
        [Required(ErrorMessage = "Bạn phải nhập bí danh seo")]
        public string SeoAlias { get; set; }

        public string LanguageId { get; set; }

        [Display(Name = "Ảnh")]
        [Required(ErrorMessage = "Bạn phải chọn ảnh")]
        public IFormFile ThumnailImage { get; set; }
    }
}
