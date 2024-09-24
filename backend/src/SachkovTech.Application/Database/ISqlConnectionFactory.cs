using System.Data;

namespace SachkovTech.Application.Database;

public interface ISqlConnectionFactory
{
    IDbConnection Create();
}