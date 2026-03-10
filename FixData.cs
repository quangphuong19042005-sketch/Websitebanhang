using System;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebsiteBanHang.Data;
using WebsiteBanHang.Models;

namespace FixData
{
    class Program
    {
        static void Main(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlServer("Server=10PC274\\SQLEXPRESS;Database=WebsiteBanHang;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true");

            using (var context = new ApplicationDbContext(optionsBuilder.Options))
            {
                var products = context.Products.Include(p => p.Images).ToList();
                int fixedCount = 0;
                foreach (var product in products)
                {
                    bool imageExists = false;
                    if (!string.IsNullOrEmpty(product.ImageUrl))
                    {
                        var path = Path.Combine("c:\\wwwwwww\\Websitebanhang\\wwwroot\\images", product.ImageUrl);
                        imageExists = File.Exists(path);
                    }

                    if (!imageExists)
                    {
                        Console.WriteLine($"Product {product.Id} has broken ImageUrl: {product.ImageUrl}");
                        var firstValidImage = product.Images.FirstOrDefault(i => File.Exists(Path.Combine("c:\\wwwwwww\\Websitebanhang\\wwwroot\\images", i.Url)));
                        if (firstValidImage != null)
                        {
                            product.ImageUrl = firstValidImage.Url;
                            Console.WriteLine($"  -> Fixed with: {product.ImageUrl}");
                            fixedCount++;
                        }
                        else
                        {
                            product.ImageUrl = "default.png"; // Or leave null if you want
                            Console.WriteLine($"  -> Fixed with default.png or null");
                            fixedCount++;
                        }
                    }
                }
                context.SaveChanges();
                Console.WriteLine($"Fixed {fixedCount} products.");
            }
        }
    }
}
