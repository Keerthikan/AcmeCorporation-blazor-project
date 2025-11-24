using AcmeCorporation.Core.Services;
using AcmeCorporation.Data.Databases;
using AcmeCorporation.Data.Entities;
using AcmeCorporation.Data.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AcmeCorporation.Web.Components;
using AcmeCorporation.Web.Components.Account;
using MudBlazor.Services;

namespace AcmeCorporation.Web;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        if (builder.Environment.IsDevelopment())
        {
            builder.Host.UseDefaultServiceProvider(options =>
            {
                options.ValidateScopes = true;
                options.ValidateOnBuild = true;
            });
        }

        // --- Services ---
        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();

        builder.Services.AddCascadingAuthenticationState();
        builder.Services.AddScoped<IdentityRedirectManager>();
        builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

        builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = IdentityConstants.ApplicationScheme;
                options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
            })
            .AddIdentityCookies();

        var connectionStringBusinessDb = builder.Configuration.GetConnectionString("BusinessDb")
                                   ?? throw new InvalidOperationException("Connection string 'BusinessDb' not found.");
        var connectionStringIdentityDb = builder.Configuration.GetConnectionString("IdentityDb")
                                    ?? throw new InvalidOperationException("Connection string 'IdentityDb' not found.");

        builder.Services.AddDbContext<BusinessDbContext>(options =>
            options.UseNpgsql(connectionStringBusinessDb));

        builder.Services.AddDbContext<AuthDbContext>(options =>
            options.UseNpgsql(connectionStringIdentityDb));

        builder.Services.AddScoped<ISerialRepository, SerialRepository>();
        builder.Services.AddScoped<IParticipantRepository, ParticipantRepository>();
        builder.Services.AddScoped<ISerialService, SerialService>();
        builder.Services.AddScoped<IFormService, FormService>();

        builder.Services.AddMudServices();
        builder.Services.AddDatabaseDeveloperPageExceptionFilter();

        builder.Services.AddIdentityCore<ApplicationUser>(options =>
            {
                options.SignIn.RequireConfirmedAccount = true;
                options.Stores.SchemaVersion = IdentitySchemaVersions.Version3;
            })
            .AddEntityFrameworkStores<AuthDbContext>()
            .AddSignInManager()
            .AddDefaultTokenProviders();

        builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();
        builder.Services.AddRazorPages();

        var app = builder.Build();

        // --- Dev environment database setup ---
        if (app.Environment.IsDevelopment())
        {
            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;

            // Business DB
            var businessDb = services.GetRequiredService<BusinessDbContext>();
            businessDb.Database.Migrate();

            if (!businessDb.EligibleDrawSerials.Any())
            {
                var serials = Enumerable.Range(1, 100).Select(i =>
                    new EligibleDrawSerial($"Win+{i}", false, null)
                );
                businessDb.EligibleDrawSerials.AddRange(serials);
                businessDb.SaveChanges();
            }

            // Identity DB
            var authDb = services.GetRequiredService<AuthDbContext>();
            authDb.Database.Migrate();

            // Seed admin user
            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
            await SeedRoadRunnerUser(userManager);

            app.UseMigrationsEndPoint();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        // --- Middleware pipeline ---
        app.UseHttpsRedirection();

        // 1. Routing
        app.UseRouting();

        // 2. Authentication & Authorization
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseAntiforgery();

        // 3. Endpoint mappings
        app.MapStaticAssets();
        app.MapRazorPages();                   // Identity Razor Pages
        app.MapRazorComponents<App>()          // Blazor app
            .AddInteractiveServerRenderMode();
        app.MapAdditionalIdentityEndpoints();  // /Account/Login etc.

        // 4. StatusCodePages (last)
        app.Run();
    }

    private static async Task SeedRoadRunnerUser(UserManager<ApplicationUser> userManager)
    {
        const string email = "roadrunner@acme.com";
        const string password = "SuperFast123!";

        if (await userManager.FindByEmailAsync(email) == null)
        {
            var user = new ApplicationUser(email)
            {
                EmailConfirmed = true
            };

            await userManager.CreateAsync(user, password);
        }
    }
}
