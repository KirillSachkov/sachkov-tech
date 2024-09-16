using System.Data;

namespace SachkovTech.Infrastructure;

public interface ISqlConnectionFactory
{
    IDbConnection Create();
}