using CSharpFunctionalExtensions;
using SachkovTech.SharedKernel;

namespace SachkovTech.Core.Abstractions;

public interface IQueryHandler<TResponse, in TQuery> where TQuery : IQuery
{
    public Task<TResponse> Handle(TQuery query, CancellationToken cancellationToken = default);
}

public interface IQueryHandlerWithResult<TResponse, in TQuery> where TQuery : IQuery
{
    public Task<Result<TResponse, ErrorList>> Handle(TQuery command, CancellationToken cancellationToken = default);
}