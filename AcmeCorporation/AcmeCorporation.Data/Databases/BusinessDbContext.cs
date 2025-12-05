using AcmeCorporation.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace AcmeCorporation.Data.Databases;

public class BusinessDbContext(DbContextOptions<BusinessDbContext> options) : DbContext(options)
{
    public DbSet<Participant> Participants { get; set; }
    public DbSet<EligibleDrawSerial>  EligibleDrawSerials { get; set; }
    public DbSet<SystemSetting> SystemSettings { get; set; } 
}