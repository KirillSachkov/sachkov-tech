using System.Data;

namespace SachkovTech.Core.Abstractions;

public interface ISqlConnectionFactory
{
    IDbConnection Create();
}