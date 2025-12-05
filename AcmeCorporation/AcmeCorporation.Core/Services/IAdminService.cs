namespace AcmeCorporation.Core.Services;

public interface IAdminService
{
    Task<bool> IsWinModeActiveAsync();
    Task ActivateWinModeAsync();
    Task DeactivateWinModeAsync();

    Task<byte[]> ExportSubmissionsCsvAsync();
}