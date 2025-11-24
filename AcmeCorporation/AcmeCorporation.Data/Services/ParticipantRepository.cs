using System.Linq.Expressions;
using AcmeCorporation.Data.Databases;
using AcmeCorporation.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace AcmeCorporation.Data.Services;

public class ParticipantRepository : IParticipantRepository
{
    private readonly BusinessDbContext _db;
    public ParticipantRepository(BusinessDbContext db)
    {
        _db = db;
    }
    
    public async Task<IEnumerable<Participant>> GetAllAsync()
    {
        return await _db.Participants.OrderBy(s => s.ProductSerialNumber).ToListAsync();
    }

    public async Task AddAsync(Participant participant)
    {
        await _db.Participants.AddAsync(participant);
    }

    public async Task SaveChangesAsync()
    {
        await _db.SaveChangesAsync();
    }

    public Task<int> CountAsync(Expression<Func<Participant, bool>> predicate)
    {
        return _db.Participants.CountAsync(predicate);
    }

    public Task<bool> ExistsAsync(Expression<Func<Participant, bool>> predicate)
    {
        return _db.Participants.AnyAsync(predicate);
    }
}