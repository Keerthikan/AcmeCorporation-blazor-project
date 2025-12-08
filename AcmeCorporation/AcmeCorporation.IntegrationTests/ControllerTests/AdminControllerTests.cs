using AcmeCorporation.Data.Databases;
using AcmeCorporation.Data.Entities;
using AcmeCorporation.IntegrationTests.Fixtures;
using Microsoft.Extensions.DependencyInjection;

namespace AcmeCorporation.IntegrationTests.ControllerTests;

public class AdminControllerTests : IClassFixture<PostgresTestFixture>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory _factory;

    public AdminControllerTests(PostgresTestFixture fixture)
    {
        _factory = new CustomWebApplicationFactory(
            fixture.BusinessConnectionString,
            fixture.AuthConnectionString);

        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task DownloadCsv_ReturnsCsvFile()
    {
        // Use _factory.Services to seed data
        using var scope = _factory.Services.CreateScope();
        var businessCtx = scope.ServiceProvider.GetRequiredService<BusinessDbContext>();
        businessCtx.Participants.Add(new Participant(FirstName: "Alice", LastName: "Wonderland",
            EmailAddress: "alice@test.com", Id: 0, ProductSerialNumber: "Win+11"));
        await businessCtx.SaveChangesAsync();

        // Use _client to call your endpoint
        var response = await _client.GetAsync("/admin/download-submissions-csv");

        response.EnsureSuccessStatusCode();
        Assert.Equal("text/csv", response.Content.Headers.ContentType?.MediaType);

        var csv = await response.Content.ReadAsStringAsync();
        Assert.Contains("Alice", csv);
    }
}