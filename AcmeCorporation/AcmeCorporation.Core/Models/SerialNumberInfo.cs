namespace AcmeCorporation.Core.Models;

public record SerialNumberInfo
{
    public string SerialNumber { get; set; } = string.Empty;
    public bool IsUsed { get; set; }
    public DateTime? UsedAt { get; set; }
}