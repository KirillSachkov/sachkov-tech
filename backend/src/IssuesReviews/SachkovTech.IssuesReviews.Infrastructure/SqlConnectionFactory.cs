using System.Data;
using Microsoft.Extensions.Configuration;
using Npgsql;
using SachkovTech.Core.Abstractions;

namespace SachkovTech.IssuesReviews.Infrastructure;

public class SqlConnectionFactory : ISqlConnectionFactory
{
    private const string DATABASE = "Database";
    private readonly IConfiguration _configuration;

    public SqlConnectionFactory(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public IDbConnection Create() =>
        new NpgsqlConnection(_configuration.GetConnectionString(DATABASE));
}