using System.Data;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using notion.dal.models;
using notion.migrations;
using Npgsql;
using Testcontainers.PostgreSql;

namespace notion.dal.tests;

public class UserTests : IAsyncLifetime
{
    private const string defaultContainerName = "test-pg";
    private const string defaultHost = "localhost";
    private const string defaultUsername = "postgres";
    private const string defaultDatabase = "public";
    private const string defaultPort = "5432";
    private const string ConnectionString = $"host={defaultHost}; UserName={defaultUsername};Database={defaultDatabase}";
    private readonly PostgreSqlContainer _postgres = new PostgreSqlBuilder()
        .WithImage("postgres")
        .WithName(defaultContainerName)
        .WithEnvironment(new Dictionary<string, string> {
                {"POSTGRES_HOST_AUTH_METHOD","trust"},
                {"POSTGRES_USERNAME", "postgres"},
                {$"POSTGRES_DB", "public"}
            })
        .WithPortBinding(defaultPort, defaultPort)
        .Build();

    private readonly IDbConnection _connection = NpgsqlDataSource.Create(ConnectionString).CreateConnection();

    public async Task DisposeAsync()
    {
        _connection.Close();
        await _postgres.StopAsync();
    }

    public async Task InitializeAsync()
    {
        await _postgres.StartAsync();
        _connection.Open();

        Migrator.New(_connection, new NullLogger<Migrator>()).Migrate();
    }

    [Fact]
    public async Task TestCreateAsync()
    {
        var u = new User { Email = "test@test.com" };
        var s = new UsersDAL(this._connection);
        var user = await s.CreateUser(u);

        Assert.Equal(u.Email, user.Email);
        Assert.NotEqual(user.ID, Guid.Empty);
        Assert.NotEqual(user.creation_date, DateTime.MinValue);
    }
}