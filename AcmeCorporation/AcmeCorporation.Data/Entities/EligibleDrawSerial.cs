using System.ComponentModel.DataAnnotations;

namespace AcmeCorporation.Data.Entities;

public record EligibleDrawSerial(
    [property: Key] string SerialNumber,
    bool IsUsed,
    DateTime? UsedAt
);