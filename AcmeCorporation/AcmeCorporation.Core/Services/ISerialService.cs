using AcmeCorporation.Core.Models;

namespace AcmeCorporation.Core.Services;

public enum SerialCheckResult
{
    NotFound,
    AlreadyUsed,
    Success
}

public interface ISerialService
{
    /// <summary>
    /// Checks whether the serial exists and (if eligible) marks it as used.
    /// </summary>
    Task<SerialCheckResult> CheckAndMarkAsync(string serial);

    Task<IEnumerable<SerialNumberInfo>> GetAllValidSerialNumbers();
    Task MarkAsUsedAsync(string serialNumber);
}