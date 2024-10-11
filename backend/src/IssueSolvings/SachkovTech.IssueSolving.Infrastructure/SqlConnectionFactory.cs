using Microsoft.Extensions.Configuration;
using Npgsql;
using SachkovTech.Core.Abstractions;
using System.Data;

namespace SachkovTech.IssueSolving.Infrastructure;

public class SqlConnectionFactory : ISqlConnectionFactory
{
    private readonly IConfiguration _configuration;

    public SqlConnectionFactory(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public IDbConnection Create() =>
        new NpgsqlConnection(_configuration.GetConnectionString("Database"));
}