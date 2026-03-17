using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;
namespace WebsiteBanHang.Models // Đổi namespace theo tên dự án của bạn
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên sản phẩm")]
        [Display(Name = "Tên quần áo")]
        public string Name { get; set; } = null!;

        [Range(1000, 10000000)]
        [Display(Name = "Giá bán")]
        public decimal Price { get; set; }

        [Display(Name = "Mô tả")]
        public string? Description { get; set; }

        public int CategoryId { get; set; }

        // --- PHẦN THÊM MỚI CHO QUẦN ÁO ---
        [Display(Name = "Kích cỡ (S, M, L)")]
        public string? Size { get; set; } // Ví dụ: S, M, L, XL

        [Display(Name = "Màu sắc")]
        public string? Color { get; set; } // Ví dụ: Xanh, Đỏ, Trắng
        [ValidateNever]
        public string? ImageUrl { get; set; } // Chứa tên file ảnh

        [ForeignKey(nameof(CategoryId))]
        public Category? Category {get; set;}

        public List<ProductImage>? Images { get; set; }
    }
}