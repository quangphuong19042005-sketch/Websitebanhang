using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebsiteBanHang.Models;
using WebsiteBanHang.Repositories; // 1. Nhớ thêm dòng này để dùng Repository

namespace WebsiteBanHang.Controllers;

public class HomeController : Controller
{
    // 2. Khai báo biến để chứa kho hàng
    private readonly IProductRepository _productRepository;

    // 3. Constructor: "Tiêm" kho hàng vào Controller
    public HomeController(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public IActionResult Index()
    {
        // 4. Lấy tất cả sản phẩm từ kho và gửi sang View
        var products = _productRepository.GetAll();
        return View(products); 
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