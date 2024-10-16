using Testcontainers.PostgreSql;
using Xunit;
using Microsoft.EntityFrameworkCore;
using MakApi;
using MakApi.Data;

public class DatabaseFixture : IAsyncLifetime
{
    public PostgreSqlContainer _PostgresContainer { get; set; }
    public MakDbContext _DbContext { get; set; }

    public async Task InitializeAsync()
    {
        // Starte den PostgreSQL-Container
        _PostgresContainer = new PostgreSqlBuilder()
            .WithDatabase("testdb")
            .WithUsername("testuser")
            .WithPassword("testpassword")
            .Build();

        await _PostgresContainer.StartAsync();

        // Konfiguriere den DbContext mit der Test-PostgreSQL-Datenbank
        var options = new DbContextOptionsBuilder<MakDbContext>()
            .UseNpgsql(_PostgresContainer.GetConnectionString())
            .Options;

        _DbContext = new MakDbContext(options);

        // Führe die Migrations aus, um alle Entitäten als Tabellen zu erstellen
        await _DbContext.Database.MigrateAsync();
    }

    public async Task DisposeAsync()
    {
        await _DbContext.DisposeAsync();
        await _PostgresContainer.StopAsync();
    }
}