using System.ComponentModel.DataAnnotations;

namespace WebsiteBanHang.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required, StringLength(50)]
        public string Name { get; set; }

        [StringLength(50)]
        public string? FolderName { get; set; }

        public ICollection<Product> Products {get; set;} = new List<Product>();
    }
}