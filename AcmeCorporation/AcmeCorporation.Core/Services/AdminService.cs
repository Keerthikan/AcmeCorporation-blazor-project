using System.Text;
using AcmeCorporation.Data.Services;

namespace AcmeCorporation.Core.Services;

public class AdminService(ISystemSettingRepository systemRepo, IParticipantRepository participantRepo ) : IAdminService
{
    public async Task<bool> IsWinModeActiveAsync()
        => (await systemRepo.GetSettingsAsync()).WinModeActive;

    public async Task ActivateWinModeAsync()
    {
        await systemRepo.SetWinModeAsync(true);
    }

    public async Task DeactivateWinModeAsync()
    {
        await systemRepo.SetWinModeAsync(false);
    }

    public async Task<byte[]> ExportSubmissionsCsvAsync()
    {
        var submissions = await participantRepo.GetAllAsync();
        var csv = new StringBuilder();
        csv.AppendLine("Id,FirstName,LastName,Email,SerialNumber");
        foreach (var s in submissions)
            csv.AppendLine($"{s.Id},{s.FirstName},{s.LastName},{s.EmailAddress},{s.ProductSerialNumber}");
        return Encoding.UTF8.GetBytes(csv.ToString());
    }
}