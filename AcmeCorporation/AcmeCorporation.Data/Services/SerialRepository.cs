using AcmeCorporation.Data.Databases;
using Microsoft.EntityFrameworkCore;

namespace AcmeCorporation.Data.Services;

public class SerialRepository : ISerialRepository
{
    private readonly BusinessDbContext _db;

    public SerialRepository(BusinessDbContext db) => _db = db;

    public async Task<bool> ExistsAsync(string serial)
    {
        return await _db.EligibleDrawSerials.AnyAsync(s => s.SerialNumber == serial);
    }

    public async Task<bool> IsUsedAsync(string serial)
    {
        var item = await _db.EligibleDrawSerials.FindAsync(serial);
        return item != null && item.IsUsed;
    }

    public async Task<bool> TryMarkAsUsedAsync(string serial)
    {
        var item = await _db.EligibleDrawSerials
            .Where(e => e.SerialNumber == serial)
            .FirstOrDefaultAsync();

        if (item is null)
            return false; 

        if (item.IsUsed)
            return false; 
    
        item = item with { IsUsed = true };
        item = item with { UsedAt = DateTime.UtcNow };

        try
        {
            await _db.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateConcurrencyException)
        {
            return false;
        }
    }

    public async Task<IEnumerable<AcmeCorporation.Data.Entities.EligibleDrawSerial>> GetAllAsync()
    {
        return await _db.EligibleDrawSerials
            .OrderBy(s => s.SerialNumber)
            .ToListAsync();
    }

}