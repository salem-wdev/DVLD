using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace DVLD.Infrastructure.Persistence.Context;

/// <summary>
/// Manages the database connection lifecycle and holds the active transaction scope
/// across repositories and the unit of work during a single HTTP request lifetime.
/// </summary>
public sealed class DbSession : IDisposable
{
    private const string ConnectionStringName = "DefaultConnection";

    /// <summary>
    /// Gets the active underlying SQL Server connection.
    /// </summary>
    public SqlConnection Connection { get; }

    /// <summary>
    /// Gets or sets the active SQL Server transaction participating in the current execution scope.
    /// </summary>
    public SqlTransaction? Transaction { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="DbSession"/> class with connection strings configured.
    /// </summary>
    /// <param name="configuration">Application configuration to read database connection strings.</param>
    /// <exception cref="InvalidOperationException">Thrown when connection string is missing or empty.</exception>
    public DbSession(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString(ConnectionStringName);

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException($"Connection string '{ConnectionStringName}' not found in configuration.");
        }

        Connection = new SqlConnection(connectionString);
    }

    /// <summary>
    /// Disposes the active transaction and releases connection resources.
    /// </summary>
    public void Dispose()
    {
        Transaction?.Dispose();
        Transaction = null;

        if (Connection.State != ConnectionState.Closed)
        {
            Connection.Close();
        }

        Connection.Dispose();
    }
}