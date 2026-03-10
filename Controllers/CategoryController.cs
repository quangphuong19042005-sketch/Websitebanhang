using Microsoft.AspNetCore.Mvc;
using WebsiteBanHang.Models;
using WebsiteBanHang.Repositories;

namespace WebsiteBanHang.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        // 1. Hiển thị danh sách danh mục
        public async Task<IActionResult> Index()
        {
            var categories = await _categoryRepository.GetAllCategoriesAsync();
            return View(categories);
        }

        // 2. Thêm danh mục mới (GET)
        public IActionResult Add()
        {
            return View();
        }

        // 3. Thêm danh mục mới (POST)
        [HttpPost]
        public async Task<IActionResult> Add(Category category)
        {
            if (ModelState.IsValid)
            {
                await _categoryRepository.AddAsync(category);
                TempData["Success"] = "Thêm danh mục thành công!";
                return RedirectToAction("Index");
            }
            return View(category);
        }

        // 4. Cập nhật danh mục (GET)
        public async Task<IActionResult> Update(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // 5. Cập nhật danh mục (POST)
        [HttpPost]
        public async Task<IActionResult> Update(Category category)
        {
            if (ModelState.IsValid)
            {
                await _categoryRepository.UpdateAsync(category);
                TempData["Success"] = "Cập nhật danh mục thành công!";
                return RedirectToAction("Index");
            }
            return View(category);
        }

        // 6. Xóa danh mục (POST)
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _categoryRepository.DeleteAsync(id);
            TempData["Success"] = "Xóa danh mục thành công!";
            return RedirectToAction("Index");
        }
    }
}
