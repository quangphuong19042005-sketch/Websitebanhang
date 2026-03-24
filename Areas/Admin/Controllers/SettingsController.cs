using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebsiteBanHang.Repositories;

namespace WebsiteBanHang.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class SettingsController : Controller
    {
        private readonly ISettingRepository _settingRepository;

        public SettingsController(ISettingRepository settingRepository)
        {
            _settingRepository = settingRepository;
        }

        public async Task<IActionResult> Index()
        {
            var settings = await _settingRepository.GetAllSettingsAsync();
            return View(settings);
        }

        [HttpPost]
        public async Task<IActionResult> Update(Dictionary<string, string> settings)
        {
            foreach (var setting in settings)
            {
                await _settingRepository.SetValueAsync(setting.Key, setting.Value);
            }
            TempData["Success"] = "Cài đặt đã được cập nhật thành công!";
            return RedirectToAction("Index");
        }
    }
}
