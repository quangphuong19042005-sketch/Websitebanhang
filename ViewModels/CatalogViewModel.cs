using Microsoft.AspNetCore.Mvc.Rendering;
using WebsiteBanHang.Models;

namespace WebsiteBanHang.ViewModels
{
    public class CatalogViewModel
    {
        public IEnumerable<Product> Products { get; set; } = new List<Product>();
        public SelectList? Categories { get; set; }
        
        // Filter states
        public int? CategoryId { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public string? Size { get; set; }
        public string? Color { get; set; }
        public string? SortOrder { get; set; }
    }
}
