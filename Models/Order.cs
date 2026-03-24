using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace WebsiteBanHang.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string UserId { get; set; } = null!;
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
        
        [Required(ErrorMessage = "Vui lòng nhập họ tên")]
        public string FullName { get; set; } = null!;
        
        [Required(ErrorMessage = "Vui lòng nhập địa chỉ")]
        public string Address { get; set; } = null!;
        
        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        [Phone]
        public string PhoneNumber { get; set; } = null!;

        public string? Notes { get; set; }

        public string Status { get; set; } = "Pending"; // Pending, Processing, Shipped, Completed, Cancelled

        [ValidateNever]
        public ApplicationUser User { get; set; } = null!;
        
        [ValidateNever]
        public List<OrderDetail> OrderDetails { get; set; } = new();
    }
}
