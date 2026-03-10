using Microsoft.EntityFrameworkCore;
using WebsiteBanHang.Data;
using WebsiteBanHang.Models;

namespace WebsiteBanHang.Repositories
{
    public class EFProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public EFProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Product> GetAll()
        {
            return _context.Products.Include(p => p.Category).Include(p => p.Images).ToList();
        }

        public Product GetById(int id)
        {
            return _context.Products.Include(p => p.Category).Include(p => p.Images).FirstOrDefault(p => p.Id == id);
        }

        public void Add(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
        }

        public void Update(Product product)
        {
            _context.Products.Update(product);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var product = _context.Products.Find(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
            }
        }

        public ProductImage GetImageById(int imageId)
        {
            return _context.ProductImages.FirstOrDefault(i => i.Id == imageId);
        }

        public void DeleteImage(int imageId)
        {
            var image = _context.ProductImages.Find(imageId);
            if (image != null)
            {
                _context.ProductImages.Remove(image);
                _context.SaveChanges();
            }
        }
    }
}