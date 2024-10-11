using CSharpFunctionalExtensions;
using SachkovTech.Files.Domain;
using SachkovTech.SharedKernel;
using SachkovTech.SharedKernel.ValueObjects.Ids;

namespace SachkovTech.Files.Application.Interfaces;

public interface IFilesRepository
{
    public Task<Result<FileId, Error>> Add(FileData fileData, CancellationToken cancellationToken);
}