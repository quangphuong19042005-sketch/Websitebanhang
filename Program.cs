using WebsiteBanHang.Repositories; 

var builder = WebApplication.CreateBuilder(args);

// Đăng ký các dịch vụ
builder.Services.AddControllersWithViews();

// Dùng Singleton để giữ dữ liệu trong RAM khi chạy web
builder.Services.AddSingleton<IProductRepository, MockProductRepository>(); 
builder.Services.AddScoped<ICategoryRepository, MockCategoryRepository>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
// QUAN TRỌNG: Cho phép truy cập ảnh trong wwwroot
app.UseStaticFiles(); 

app.UseRouting();
app.UseAuthorization();

// ĐỊNH TUYẾN CHUẨN: Home là mặc định
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"); 

app.Run();