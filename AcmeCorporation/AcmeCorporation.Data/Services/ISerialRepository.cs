namespace AcmeCorporation.Data.Services;

/// <summary>
/// Repository abstraction for eligible draw serials. Allows Core to remain independent of EF/Data project.
/// </summary>
public interface ISerialRepository
{
    /// <summary>
    /// Returns true if a serial with given key exists.
    /// </summary>
    Task<bool> ExistsAsync(string serial);

    /// <summary>
    /// Returns true if serial is already used.
    /// </summary>
    Task<bool> IsUsedAsync(string serial);

    /// <summary>
    /// Attempts to mark the serial as used. Returns true if marking succeeded (i.e. it existed and wasn't already used).
    /// </summary>
    Task<bool> TryMarkAsUsedAsync(string serial);

    /// <summary>
    /// Returns all eligible draw serials.
    /// </summary>
    Task<IEnumerable<AcmeCorporation.Data.Entities.EligibleDrawSerial>> GetAllAsync();
}