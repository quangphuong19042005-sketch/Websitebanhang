using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebsiteBanHang.Models;
using WebsiteBanHang.Repositories;

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
        public IActionResult Index()
        {
            var products = _productRepository.GetAll();
            return View(products);
        }

        // 2. Form thêm mới
        public IActionResult Add()
        {
            var categories = _categoryRepository.GetAllCategories();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            return View();
        }

        // 3. Xử lý thêm mới
        [HttpPost]
        public async Task<IActionResult> Add(Product product, List<IFormFile> ImageFiles)
        {
            // Xóa lỗi validation của ImageUrl vì chúng ta sẽ tự gán giá trị ở dưới
            ModelState.Remove("ImageUrl");

            if (ModelState.IsValid)
            {
                if (ImageFiles != null && ImageFiles.Count > 0)
                {
                    string folderName = GetFolderNameByCategoryId(product.CategoryId);
                    product.Images = new List<ProductImage>();

                    for (int i = 0; i < ImageFiles.Count; i++)
                    {
                        var file = ImageFiles[i];
                        string url = await SaveImage(file, folderName);
                        if (i == 0)
                        {
                            product.ImageUrl = url; // Lưu ảnh đầu tiên vào ImageUrl để giữ tương thích
                        }
                        product.Images.Add(new ProductImage { Url = url });
                    }
                }

                _productRepository.Add(product);
                return RedirectToAction("Index");
            }

            // Nếu không hợp lệ, nạp lại Dropdown
            var categories = _categoryRepository.GetAllCategories();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            return View(product);
        }

        // 4. Xem chi tiết
        public IActionResult Display(int id)
        {
            var product = _productRepository.GetById(id);
            if (product == null) return NotFound();
            return View(product);
        }

        // 5. Form cập nhật
        public IActionResult Update(int id)
        {
            var product = _productRepository.GetById(id);
            if (product == null) return NotFound();

            var categories = _categoryRepository.GetAllCategories();
            ViewBag.Categories = new SelectList(categories, "Id", "Name", product.CategoryId);
            return View(product);
        }

        // 6. Xử lý cập nhật
        [HttpPost]
        public async Task<IActionResult> Update(Product product, List<IFormFile> ImageFiles)
        {
            ModelState.Remove("ImageFiles"); // Tránh lỗi validation file nếu không chọn ảnh mới
            ModelState.Remove("ImageUrl");

            if (ModelState.IsValid)
            {
                if (ImageFiles != null && ImageFiles.Count > 0)
                {
                    string folderName = GetFolderNameByCategoryId(product.CategoryId);
                    product.Images = new List<ProductImage>();

                    for (int i = 0; i < ImageFiles.Count; i++)
                    {
                        var file = ImageFiles[i];
                        string url = await SaveImage(file, folderName);
                        if (i == 0)
                        {
                            product.ImageUrl = url;
                        }
                        product.Images.Add(new ProductImage { Url = url });
                    }
                }
                // Nếu không chọn ảnh mới, ImageUrl sẽ giữ giá trị từ input hidden trong View

                _productRepository.Update(product);
                return RedirectToAction("Index");
            }

            var categories = _categoryRepository.GetAllCategories();
            ViewBag.Categories = new SelectList(categories, "Id", "Name", product.CategoryId);
            return View(product);
        }

        // 7. Form xác nhận xóa
        public IActionResult Delete(int id)
        {
            var product = _productRepository.GetById(id);
            if (product == null) return NotFound();
            return View(product);
        }

        // 8. Thực hiện xóa
        [HttpPost, ActionName("DeleteConfirmed")]
        public IActionResult DeleteConfirmed(int id)
        {
            _productRepository.Delete(id);
            return RedirectToAction("Index");
        }

        // --- HÀM HỖ TRỢ ---

        [HttpPost]
        public IActionResult DeleteImage(int imageId)
        {
            var image = _productRepository.GetImageById(imageId);
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
            _productRepository.DeleteImage(imageId);

            // Cập nhật ImageUrl của Product nếu xóa ảnh hiển thị chính
            var product = _productRepository.GetById(productId);
            if (product != null && product.ImageUrl == deletedUrl)
            {
                var remainingImage = product.Images?.FirstOrDefault(i => i.Id != imageId);
                product.ImageUrl = remainingImage?.Url; // Gán lại thành ảnh đầu tiên hoặc null
                _productRepository.Update(product);
            }

            return Json(new { success = true });
        }

        private async Task<string> SaveImage(IFormFile image, string folderName)
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

        private string GetFolderNameByCategoryId(int categoryId)
        {
            // Logic: ID 1 là Áo, các ID khác mặc định là Quần
            return categoryId == 1 ? "ao" : "quan";
        }
    }
}