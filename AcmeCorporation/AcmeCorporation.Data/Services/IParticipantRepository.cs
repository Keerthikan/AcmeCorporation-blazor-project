using System.Linq.Expressions;
using AcmeCorporation.Data.Entities;

namespace AcmeCorporation.Data.Services;

public interface IParticipantRepository
{
    /// <summary>
    /// Returns all participants who submitted a form and has a valid serial number.
    /// </summary>
    Task<IEnumerable<AcmeCorporation.Data.Entities.Participant>> GetAllAsync();

    /// <summary>
    ///  Adds a new participant entity.
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task AddAsync(Participant entity);

    /// <summary>
    ///  Saves changes to the data store.
    /// </summary>
    /// <returns></returns>
    Task SaveChangesAsync();

    /// <summary>
    ///  Returns true if any participant exists matching the given predicate.
    /// </summary>
    /// <param name="func"></param>
    /// <returns></returns>
    Task<int> CountAsync(Expression<Func<Participant, bool>> predicate);

    /// <summary>
    ///  Returns true if any participant exists matching the given predicate.
    /// </summary>
    /// <param name="func"></param>
    /// <returns></returns>
    Task<bool> ExistsAsync(Expression<Func<Participant, bool>> func);
}