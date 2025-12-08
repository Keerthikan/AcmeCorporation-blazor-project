using AcmeCorporation.Data.Databases;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Testcontainers.PostgreSql;

namespace AcmeCorporation.IntegrationTests.Fixtures;

public class PostgresTestFixture : IAsyncLifetime
{
    private readonly PostgreSqlContainer _container;

    internal string BusinessConnectionString => BuildConn("businessdb");
    internal string AuthConnectionString => BuildConn("authdb");

    private string BuildConn(string dbName) =>
        new NpgsqlConnectionStringBuilder(_container.GetConnectionString())
        {
            Database = dbName
        }.ToString();

    public PostgresTestFixture()
    {
        // Build single container (initial DB is 'postgres')
        _container = new PostgreSqlBuilder()
            .WithDatabase("postgres")
            .WithUsername("postgres")
            .WithPassword("postgres")
            .Build();
    }

    public async Task InitializeAsync()
    {
        await _container.StartAsync();

        // Execute SQL script to create additional databases
        await ExecuteSqlScript(_container.GetConnectionString(), "/Users/thisayinibaskaran/Code/AcmeCorporation/AcmeCorporation.IntegrationTests/Fixtures/Scripts/initdb.sql");

        // Apply migrations
        await ApplyMigrations(new BusinessDbContext(
            new DbContextOptionsBuilder<BusinessDbContext>()
                .UseNpgsql(BusinessConnectionString)
                .Options));

        await ApplyMigrations(new AuthDbContext(
            new DbContextOptionsBuilder<AuthDbContext>()
                .UseNpgsql(AuthConnectionString)
                .Options));

        // Seed Identity user
        await SeedIdentityUser();
    }

    public async Task DisposeAsync()
    {
        await _container.DisposeAsync();
    }

    // -----------------------------
    // Helpers
    // -----------------------------
    private static async Task ExecuteSqlScript(string connStr, string path)
    {
        if (!File.Exists(path))
            throw new FileNotFoundException("SQL script not found", path);

        var sql = await File.ReadAllTextAsync(path);

        await using var conn = new NpgsqlConnection(connStr);
        await conn.OpenAsync();

        await using var cmd = new NpgsqlCommand(sql, conn);
        await cmd.ExecuteNonQueryAsync();
    }

    private static async Task ApplyMigrations(DbContext context)
    {
        var pending = await context.Database.GetPendingMigrationsAsync();
        if (pending.Any())
            await context.Database.MigrateAsync();

        await context.DisposeAsync();
    }

    private async Task SeedIdentityUser()
    {
        var services = new ServiceCollection();

        services.AddDbContext<AuthDbContext>(opts =>
            opts.UseNpgsql(AuthConnectionString));

        services.AddIdentityCore<ApplicationUser>()
            .AddEntityFrameworkStores<AuthDbContext>()
            .AddDefaultTokenProviders();

        await using var provider = services.BuildServiceProvider();
        using var scope = provider.CreateScope();

        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        await SeedTestUser(userManager);
    }

    private static async Task SeedTestUser(UserManager<ApplicationUser> userManager)
    {
        const string email = "testuser@acme.com";
        const string password = "Test123!";

        if (await userManager.FindByEmailAsync(email) == null)
        {
            var user = new ApplicationUser(email) { EmailConfirmed = true };
            await userManager.CreateAsync(user, password);
        }
    }
}
