using AcmeCorporation.Data.Entities;

namespace AcmeCorporation.Data.Services;

public interface ISystemSettingRepository
{
    Task<SystemSetting> GetSettingsAsync();
    Task SetWinModeAsync(bool active);
}