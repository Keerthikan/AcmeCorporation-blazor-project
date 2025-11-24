using AcmeCorporation.Core.Models;
using AcmeCorporation.Data.Services;

namespace AcmeCorporation.Core.Services;

public class SerialService : ISerialService
{
    private readonly ISerialRepository _repo;

    public SerialService(ISerialRepository repo)
    {
        _repo = repo;
    }

    public async Task<SerialCheckResult> CheckAndMarkAsync(string serial)
    {
        if (string.IsNullOrWhiteSpace(serial))
            return SerialCheckResult.NotFound;

        var sn = serial.Trim();

        var exists = await _repo.ExistsAsync(sn);
        if (!exists)
            return SerialCheckResult.NotFound;

        var used = await _repo.IsUsedAsync(sn);
        if (used)
            return SerialCheckResult.AlreadyUsed;

        var marked = await _repo.TryMarkAsUsedAsync(sn);
        return marked ? SerialCheckResult.Success : SerialCheckResult.AlreadyUsed;
    }

    public async Task<SerialCheckResult> TryMarkAsUsedAsync(string serial)
    {
        return await _repo.TryMarkAsUsedAsync(serial) ? SerialCheckResult.Success : SerialCheckResult.AlreadyUsed;
    }

    public async Task<IEnumerable<SerialNumberInfo>> GetAllValidSerialNumbers()
    {
        var items = await _repo.GetAllAsync();
        return items.Select(e => new SerialNumberInfo
        {
            SerialNumber = e.SerialNumber,
            IsUsed = e.IsUsed,
            UsedAt = e.UsedAt
        });
    }

    public Task MarkAsUsedAsync(string serialNumber)
    {
        throw new NotImplementedException();
    }
}