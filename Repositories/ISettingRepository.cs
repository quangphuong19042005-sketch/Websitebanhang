using WebsiteBanHang.Models;

namespace WebsiteBanHang.Repositories
{
    public interface ISettingRepository
    {
        Task<string?> GetValueAsync(string key);
        Task SetValueAsync(string key, string value);
        Task<Dictionary<string, string>> GetAllSettingsAsync();
    }
}
