using MakApi;
using MakApi.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Testcontainers.PostgreSql;

namespace MakTest.Fixtures;

public class WebApplicationFactoryFixture : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly HttpClient _client;
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder().Build();


    public int InitialProjectCount { get; set; } = 3;

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
        await InitializeDatabaseAsync();
    }

    public async Task DisposeAsync()
    {
        await ClearDatabaseAsync();
        await _dbContainer.StopAsync();
    }


    public async Task InitializeDatabaseAsync()
    {
        using (var scope = Services.CreateScope())
        {
            var scopedServices = scope.ServiceProvider;
            var cntx = scopedServices.GetRequiredService<MakDbContext>();

            await cntx.Database.EnsureCreatedAsync();
            await cntx.Projects.AddRangeAsync(DataFixture.GetProjects(InitialProjectCount));
            await cntx.SaveChangesAsync();
        }
    }

    public async Task ClearDatabaseAsync()
    {
        using (var scope = Services.CreateScope())
        {
            var scopedServices = scope.ServiceProvider;
            var cntx = scopedServices.GetRequiredService<MakDbContext>();

            cntx.Projects.RemoveRange(cntx.Projects);
            await cntx.SaveChangesAsync();
        }
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        var connectionString = _dbContainer.GetConnectionString();
        base.ConfigureWebHost(builder);
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll(typeof(DbContextOptions<MakDbContext>));
            services.AddDbContext<MakDbContext>(options => { options.UseNpgsql(connectionString); });
        });
    }
}