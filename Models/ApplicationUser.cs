using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace WebsiteBanHang.Models
{
    public class ApplicationUser : IdentityUser
    {
        [StringLength(100)]
        [Display(Name = "Họ và tên")]
        public string? FullName { get; set; }
    }
}
