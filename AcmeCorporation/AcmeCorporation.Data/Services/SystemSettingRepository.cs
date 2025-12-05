using AcmeCorporation.Data.Databases;
using AcmeCorporation.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace AcmeCorporation.Data.Services;

public class SystemSettingRepository : ISystemSettingRepository
{
    private readonly BusinessDbContext _db;

    public SystemSettingRepository(BusinessDbContext db)
    {
        _db = db;
    }

    public async Task<SystemSetting> GetSettingsAsync()
    {
        return await _db.SystemSettings
                   .OrderByDescending(s => s.ActivatedAt)
                   .FirstOrDefaultAsync()
               ?? new SystemSetting { WinModeActive = false, ActivatedAt = null };
    }

    public async Task SetWinModeAsync(bool active)
    {
        Console.WriteLine($"Persisting WinMode={active} at {DateTime.UtcNow}");

        var newEntry = new SystemSetting
        {
            WinModeActive = active,
            ActivatedAt = DateTime.UtcNow
        };

        _db.SystemSettings.Add(newEntry);
        await _db.SaveChangesAsync();
    }

    public async Task<List<SystemSetting>> GetHistoryAsync()
    {
        return await _db.SystemSettings
            .OrderByDescending(s => s.ActivatedAt)
            .ToListAsync();
    }
}