using System.Data;
using EvolveDb;
using Microsoft.Extensions.Logging;

namespace notion.migrations;
public class Migrator
{
    private readonly IDbConnection _conn;
    private readonly ILogger _logger;
    private Migrator(IDbConnection conn, ILogger logger)
    {
        this._conn = conn;
        this._logger = logger;
    }

    public static Migrator New(IDbConnection conn, ILogger<Migrator> logger)
    {
        return new Migrator(conn, logger);
    }

    public void Migrate()
    {
        var evolve = new Evolve((System.Data.Common.DbConnection)this._conn, msg => this._logger.LogInformation(msg))
        {
            Locations = new[] { "sql" },
            IsEraseDisabled = true,

        };

        evolve.Migrate();
    }
}
