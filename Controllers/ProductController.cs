using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebsiteBanHang.Models;
using WebsiteBanHang.Repositories;
using WebsiteBanHang.ViewModels;

namespace WebsiteBanHang.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(IProductRepository productRepository, 
                                 ICategoryRepository categoryRepository, 
                                 IWebHostEnvironment webHostEnvironment)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _webHostEnvironment = webHostEnvironment;
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
        public async Task<IActionResult> Add(ProductFormViewModel viewModel, List<IFormFile> ImageFiles)
        {
            var product = viewModel.Product;
            // Xóa lỗi validation của ImageUrl vì chúng ta sẽ tự gán giá trị ở dưới
            ModelState.Remove("Product.ImageUrl");
            ModelState.Remove("Categories"); // ViewModel UI Property

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
                            product.ImageUrl = url; // Lưu ảnh đầu tiên vào ImageUrl để giữ tương thích
                        }
                        product.Images.Add(new ProductImage { Url = url });
                    }
                }

                await _productRepository.AddAsync(product);
                return RedirectToAction("Index");
            }

            // Nếu không hợp lệ, nạp lại Dropdown
            var categories = await _categoryRepository.GetAllCategoriesAsync();
            viewModel.Categories = new SelectList(categories, "Id", "Name");
            return View(viewModel);
        }

        // 4. Xem chi tiết
        public async Task<IActionResult> Display(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null) return NotFound();
            return View(product);
        }

        // 5. Form cập nhật
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

        // 6. Xử lý cập nhật
        [HttpPost]
        public async Task<IActionResult> Update(ProductFormViewModel viewModel, List<IFormFile> ImageFiles)
        {
            var product = viewModel.Product;
            ModelState.Remove("ImageFiles"); // Tránh lỗi validation file nếu không chọn ảnh mới
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
                // Nếu không chọn ảnh mới, ImageUrl sẽ giữ giá trị từ input hidden trong View

                await _productRepository.UpdateAsync(product);
                return RedirectToAction("Index");
            }

            var categories = await _categoryRepository.GetAllCategoriesAsync();
            viewModel.Categories = new SelectList(categories, "Id", "Name", product.CategoryId);
            return View(viewModel);
        }

        // 7. Form xác nhận xóa
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null) return NotFound();
            return View(product);
        }

        // 8. Thực hiện xóa
        [HttpPost, ActionName("DeleteConfirmed")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _productRepository.DeleteAsync(id);
            return RedirectToAction("Index");
        }

        // --- HÀM HỖ TRỢ ---

        [HttpPost]
        public async Task<IActionResult> DeleteImage(int imageId)
        {
            var image = await _productRepository.GetImageByIdAsync(imageId);
            if (image == null)
            {
                return Json(new { success = false, message = "Không tìm thấy ảnh" });
            }

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
                product.ImageUrl = remainingImage?.Url; // Gán lại thành ảnh đầu tiên hoặc null
                await _productRepository.UpdateAsync(product);
            }

            return Json(new { success = true });
        }

        private async Task<string> SaveImageAsync(IFormFile image, string folderName)
        {
            // Tạo đường dẫn: wwwroot/images/ao hoặc wwwroot/images/quan
            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", folderName);
            
            if (!Directory.Exists(uploadsFolder)) 
                Directory.CreateDirectory(uploadsFolder);

            string uniqueFileName = Guid.NewGuid().ToString() + "_" + image.FileName;
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
            // Nếu foldername trống thì tạo tên ngẫu nhiên hoặc "default"
            return !string.IsNullOrWhiteSpace(category?.FolderName) ? category.FolderName : "others";
        }
    }
}