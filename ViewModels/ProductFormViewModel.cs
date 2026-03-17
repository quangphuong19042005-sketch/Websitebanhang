using Microsoft.AspNetCore.Mvc.Rendering;
using WebsiteBanHang.Models;

namespace WebsiteBanHang.ViewModels
{
    public class ProductFormViewModel
    {
        public Product Product { get; set; } = null!;
        public SelectList Categories { get; set; } = null!;
    }
}
