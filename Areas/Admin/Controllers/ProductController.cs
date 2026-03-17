using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebsiteBanHang.Models;
using WebsiteBanHang.Repositories;
using WebsiteBanHang.ViewModels;

namespace WebsiteBanHang.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<ProductController> _logger;

        public ProductController(IProductRepository productRepository, 
                                 ICategoryRepository categoryRepository, 
                                 IWebHostEnvironment webHostEnvironment,
                                 ILogger<ProductController> logger)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
        }

        // 1. Danh sách sản phẩm
        public async Task<IActionResult> Index()
        {
            var products = await _productRepository.GetAllAsync();
            return View(products);
        }

        // 2. Form thêm mới
        public async Task<IActionResult> Add()
        {
            var categories = await _categoryRepository.GetAllCategoriesAsync();
            var viewModel = new ProductFormViewModel
            {
                Product = new Product(),
                Categories = new SelectList(categories, "Id", "Name")
            };
            return View(viewModel);
        }

        // 3. Xử lý thêm mới
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(ProductFormViewModel viewModel, List<IFormFile> ImageFiles)
        {
            var product = viewModel.Product;
            ModelState.Remove("Product.ImageUrl");
            ModelState.Remove("Categories");

            if (ModelState.IsValid)
            {
                if (ImageFiles != null && ImageFiles.Count > 0)
                {
                    string folderName = await GetFolderNameByCategoryIdAsync(product.CategoryId);
                    product.Images = new List<ProductImage>();

                    for (int i = 0; i < ImageFiles.Count; i++)
                    {
                        var file = ImageFiles[i];
                        string url = await SaveImageAsync(file, folderName);
                        if (i == 0)
                        {
                            product.ImageUrl = url;
                        }
                        product.Images.Add(new ProductImage { Url = url });
                    }
                }

                await _productRepository.AddAsync(product);
                _logger.LogInformation("Admin {User} created product: {ProductName}", User.Identity?.Name, product.Name);
                TempData["Success"] = "Thêm sản phẩm thành công!";
                return RedirectToAction("Index");
            }

            var categories = await _categoryRepository.GetAllCategoriesAsync();
            viewModel.Categories = new SelectList(categories, "Id", "Name");
            return View(viewModel);
        }

        // 4. Form cập nhật
        public async Task<IActionResult> Update(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null) return NotFound();

            var categories = await _categoryRepository.GetAllCategoriesAsync();
            var viewModel = new ProductFormViewModel
            {
                Product = product,
                Categories = new SelectList(categories, "Id", "Name", product.CategoryId)
            };
            return View(viewModel);
        }

        // 5. Xử lý cập nhật
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(ProductFormViewModel viewModel, List<IFormFile> ImageFiles)
        {
            var product = viewModel.Product;
            ModelState.Remove("ImageFiles");
            ModelState.Remove("Product.ImageUrl");
            ModelState.Remove("Categories");

            if (ModelState.IsValid)
            {
                if (ImageFiles != null && ImageFiles.Count > 0)
                {
                    string folderName = await GetFolderNameByCategoryIdAsync(product.CategoryId);
                    product.Images = new List<ProductImage>();

                    for (int i = 0; i < ImageFiles.Count; i++)
                    {
                        var file = ImageFiles[i];
                        string url = await SaveImageAsync(file, folderName);
                        if (i == 0)
                        {
                            product.ImageUrl = url;
                        }
                        product.Images.Add(new ProductImage { Url = url });
                    }
                }

                await _productRepository.UpdateAsync(product);
                _logger.LogInformation("Admin {User} updated product ID: {ProductId}", User.Identity?.Name, product.Id);
                TempData["Success"] = "Cập nhật sản phẩm thành công!";
                return RedirectToAction("Index");
            }

            var categories = await _categoryRepository.GetAllCategoriesAsync();
            viewModel.Categories = new SelectList(categories, "Id", "Name", product.CategoryId);
            return View(viewModel);
        }

        // 6. Form xác nhận xóa
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null) return NotFound();
            return View(product);
        }

        // 7. Thực hiện xóa
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _logger.LogInformation("Admin {User} deleted product ID: {ProductId}", User.Identity?.Name, id);
            await _productRepository.DeleteAsync(id);
            TempData["Success"] = "Xóa sản phẩm thành công!";
            return RedirectToAction("Index");
        }

        // --- HÀM HỖ TRỢ ---

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteImage(int imageId)
        {
            var image = await _productRepository.GetImageByIdAsync(imageId);
            if (image == null) return Json(new { success = false, message = "Không tìm thấy ảnh" });

            int productId = image.ProductId;
            string deletedUrl = image.Url;

            // Xóa file vật lý
            var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", image.Url);
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }

            // Xóa trong database
            await _productRepository.DeleteImageAsync(imageId);

            // Cập nhật ImageUrl của Product nếu xóa ảnh hiển thị chính
            var product = await _productRepository.GetByIdAsync(productId);
            if (product != null && product.ImageUrl == deletedUrl)
            {
                var remainingImage = product.Images?.FirstOrDefault(i => i.Id != imageId);
                product.ImageUrl = remainingImage?.Url;
                await _productRepository.UpdateAsync(product);
            }

            return Json(new { success = true });
        }

        private static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
        private const long MaxFileSize = 5 * 1024 * 1024; // 5MB

        private async Task<string> SaveImageAsync(IFormFile image, string folderName)
        {
            // Validate file
            var ext = Path.GetExtension(image.FileName).ToLowerInvariant();
            if (!AllowedExtensions.Contains(ext))
                throw new InvalidOperationException($"Loại file '{ext}' không được hỗ trợ.");
            if (image.Length > MaxFileSize)
                throw new InvalidOperationException("File quá lớn (tối đa 5MB).");

            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", folderName);
            if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

            string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName).ToLowerInvariant();
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }
            return folderName + "/" + uniqueFileName;
        }

        private async Task<string> GetFolderNameByCategoryIdAsync(int categoryId)
        {
            var category = await _categoryRepository.GetByIdAsync(categoryId);
            return !string.IsNullOrWhiteSpace(category?.FolderName) ? category.FolderName : "others";
        }
    }
}
