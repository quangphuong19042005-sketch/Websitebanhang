using System.ComponentModel.DataAnnotations;

namespace WebsiteBanHang.Models
{
    public class WebsiteSetting
    {
        [Key]
        public string Key { get; set; } = null!;
        public string Value { get; set; } = null!;
        public string? Description { get; set; }
    }
}
