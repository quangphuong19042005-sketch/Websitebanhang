using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebsiteBanHang.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class OrderController : Controller
    {
        public IActionResult Index()
        {
            // Placeholder: Trong thực tế sẽ lấy danh sách đơn hàng từ DB
            return View();
        }
    }
}
