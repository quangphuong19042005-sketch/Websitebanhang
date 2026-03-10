using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebsiteBanHang.Models;
using WebsiteBanHang.Repositories; // 1. Nhớ thêm dòng này để dùng Repository
using WebsiteBanHang.ViewModels; // Thêm namespace ViewModel

namespace WebsiteBanHang.Controllers;

public class HomeController : Controller
{
    // 2. Khai báo biến để chứa kho hàng
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;

    // 3. Constructor: "Tiêm" kho hàng vào Controller
    public HomeController(IProductRepository productRepository, ICategoryRepository categoryRepository)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<IActionResult> Index()
    {
        // 4. Lấy 4 sản phẩm mới nhất/nổi bật cho trang chủ
        var products = await _productRepository.GetAllAsync();
        return View(products.Take(4)); 
    }

    public async Task<IActionResult> Products(int? categoryId, decimal? minPrice, decimal? maxPrice, string? size, string? color, string? sortOrder)
    {
        var products = await _productRepository.GetFilteredAsync(categoryId, minPrice, maxPrice, size, color, sortOrder);
        var categories = await _categoryRepository.GetAllCategoriesAsync();

        var viewModel = new CatalogViewModel
        {
            Products = products,
            Categories = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(categories, "Id", "Name", categoryId),
            CategoryId = categoryId,
            MinPrice = minPrice,
            MaxPrice = maxPrice,
            Size = size,
            Color = color,
            SortOrder = sortOrder
        };

        return View(viewModel); 
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}