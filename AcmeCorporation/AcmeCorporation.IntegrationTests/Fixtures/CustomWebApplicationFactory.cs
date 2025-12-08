using AcmeCorporation.Data.Databases;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace AcmeCorporation.IntegrationTests.Fixtures;

public class CustomWebApplicationFactory(string businessConn, string authConn) : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.RemoveAll<DbContextOptions<BusinessDbContext>>();
            services.RemoveAll<DbContextOptions<AuthDbContext>>();

            services.AddDbContext<BusinessDbContext>(options =>
                options.UseNpgsql(businessConn));

            services.AddDbContext<AuthDbContext>(options =>
                options.UseNpgsql(authConn));
        });
    }
}