using WebsiteBanHang.Models;

namespace WebsiteBanHang.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<IEnumerable<Product>> GetFilteredAsync(int? categoryId, decimal? minPrice, decimal? maxPrice, string? size, string? color, string? sortOrder);
        Task<Product?> GetByIdAsync(int id);
        Task AddAsync(Product product);
        Task UpdateAsync(Product product);
        Task DeleteAsync(int id);
        Task<ProductImage?> GetImageByIdAsync(int imageId);
        Task DeleteImageAsync(int imageId);
    }
}