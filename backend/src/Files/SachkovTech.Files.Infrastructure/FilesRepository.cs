using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using SachkovTech.Files.Application.Interfaces;
using SachkovTech.Files.Domain;
using SachkovTech.Files.Infrastructure.Database;
using SachkovTech.SharedKernel;
using SachkovTech.SharedKernel.ValueObjects.Ids;

namespace SachkovTech.Files.Infrastructure;

internal class FilesRepository : IFilesRepository
{
    private readonly FilesWriteDbContext _dbContext;
    private readonly ILogger<FilesRepository> _logger;

    public FilesRepository(FilesWriteDbContext dbContext, ILogger<FilesRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Result<FileId, Error>> Add(FileData fileData, CancellationToken cancellationToken)
    {
        try
        {
            await _dbContext.FileData.AddAsync(fileData, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return fileData.Id;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Failed to save file data with id \"{fileId}\" to the database.", fileData.Id.Value.ToString());

            return Error.Failure("file.upload", "Fail to upload file in minio");
        }
    }
}