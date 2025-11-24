using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AcmeCorporation.Data.Databases;

public class AuthDbContext : IdentityDbContext<ApplicationUser>
{
    public AuthDbContext(DbContextOptions<AuthDbContext> options)
        : base(options) { }
}

public sealed class ApplicationUser : IdentityUser
{
    public ApplicationUser(string email) : base(email)
    {
        Email = email;
    }
}