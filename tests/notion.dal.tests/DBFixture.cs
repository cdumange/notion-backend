using System.Data;
using Microsoft.Extensions.Logging.Abstractions;
using notion.migrations;
using Npgsql;
using Testcontainers.PostgreSql;

namespace notion.dal.tests
{
    public class DBFixture : IDisposable
    {
        private const string defaultHost = "localhost";
        private const string defaultUsername = "postgres";
        private const string defaultDatabase = "public";
        private const string defaultPort = "5432";
        private const string ConnectionString = $"host={defaultHost}; UserName={defaultUsername};Database={defaultDatabase}";

        private readonly IDbConnection _connection = NpgsqlDataSource.Create(ConnectionString).CreateConnection();
        private readonly PostgreSqlContainer _postgres = new PostgreSqlBuilder()
        .WithImage("postgres")
        .WithEnvironment(new Dictionary<string, string> {
                {"POSTGRES_HOST_AUTH_METHOD","trust"},
                {"POSTGRES_USERNAME", "postgres"},
                {$"POSTGRES_DB", "public"}
            })
        .WithPortBinding(defaultPort, defaultPort)
        .Build();
        public void Dispose()
        {
            _connection.Close();
            _postgres.StopAsync().Wait();
        }

        public DBFixture()
        {
            _postgres.StartAsync().Wait();
            _connection.Open();

            Migrator.New(_connection, new NullLogger<Migrator>()).Migrate();
        }

        public IDbConnection GetConnection()
        {
            return _connection;
        }
    }

    [CollectionDefinition("Database collection")]
    public class DatabaseCollection : ICollectionFixture<DBFixture>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
}