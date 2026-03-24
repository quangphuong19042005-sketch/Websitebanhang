using Microsoft.EntityFrameworkCore;
using WebsiteBanHang.Repositories;
using WebsiteBanHang.Data;
using Microsoft.AspNetCore.Identity;
using WebsiteBanHang.Models;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllersWithViews();

builder.Services.AddScoped<ICategoryRepository, EFCategoryRepository>();
builder.Services.AddScoped<IProductRepository, EFProductRepository>();
builder.Services.AddScoped<IOrderRepository, EFOrderRepository>();
builder.Services.AddScoped<ISettingRepository, EFSettingRepository>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.MaxAge = TimeSpan.FromDays(7);
});

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddRazorPages();

var app = builder.Build();

// --- ĐOẠN TỰ ĐỘNG MIGRATION ---
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        context.Database.Migrate(); 
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Lỗi xảy ra khi cập nhật Database.");
    }
}
// ------------------------------

// --- SEED ADMIN ROLE & USER ---
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

    if (!await roleManager.RoleExistsAsync("Admin"))
        await roleManager.CreateAsync(new IdentityRole("Admin"));

    var adminUser = await userManager.FindByEmailAsync("admin@fashion.com");
    if (adminUser == null)
    {
        adminUser = new ApplicationUser 
        { 
            UserName = "admin@fashion.com", 
            Email = "admin@fashion.com", 
            EmailConfirmed = true,
            FullName = "Administrator"
        };
        await userManager.CreateAsync(adminUser, "Admin@123");
        await userManager.AddToRoleAsync(adminUser, "Admin");
    }
    else if (string.IsNullOrEmpty(adminUser.FullName))
    {
        adminUser.FullName = "Administrator";
        await userManager.UpdateAsync(adminUser);
    }
}

// --- SEED WEBSITE SETTINGS ---
using (var scope = app.Services.CreateScope())
{
    var settingRepo = scope.ServiceProvider.GetRequiredService<ISettingRepository>();
    var settings = await settingRepo.GetAllSettingsAsync();
    
    if (!settings.ContainsKey("StoreName")) await settingRepo.SetValueAsync("StoreName", "Fashion Store");
    if (!settings.ContainsKey("StoreEmail")) await settingRepo.SetValueAsync("StoreEmail", "support@fashion.com");
    if (!settings.ContainsKey("StorePhone")) await settingRepo.SetValueAsync("StorePhone", "+84 909 123 456");
    if (!settings.ContainsKey("StoreAddress")) await settingRepo.SetValueAsync("StoreAddress", "HUTECH KHU E1.0809");
}
// ------------------------------

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles(); 

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
