using System.Data;

namespace SachkovTech.Core;

public interface ISqlConnectionFactory
{
    IDbConnection Create();
}