using Microsoft.EntityFrameworkCore;
using WebsiteBanHang.Data;
using WebsiteBanHang.Models;

namespace WebsiteBanHang.Repositories
{
    public class EFSettingRepository : ISettingRepository
    {
        private readonly ApplicationDbContext _context;

        public EFSettingRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<string?> GetValueAsync(string key)
        {
            var setting = await _context.WebsiteSettings.FirstOrDefaultAsync(s => s.Key == key);
            return setting?.Value;
        }

        public async Task SetValueAsync(string key, string value)
        {
            var setting = await _context.WebsiteSettings.FirstOrDefaultAsync(s => s.Key == key);
            if (setting == null)
            {
                setting = new WebsiteSetting { Key = key, Value = value };
                _context.WebsiteSettings.Add(setting);
            }
            else
            {
                setting.Value = value;
            }
            await _context.SaveChangesAsync();
        }

        public async Task<Dictionary<string, string>> GetAllSettingsAsync()
        {
            return await _context.WebsiteSettings.ToDictionaryAsync(s => s.Key, s => s.Value);
        }
    }
}
