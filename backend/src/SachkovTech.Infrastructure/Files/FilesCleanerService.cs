using Microsoft.Extensions.Logging;
using SachkovTech.Application.Files;
using SachkovTech.Application.Messaging;
using FileInfo = SachkovTech.Application.Files.FileInfo;

namespace SachkovTech.Infrastructure.Files;

public class FilesCleanerService : IFilesCleanerService
{
    private readonly IFileProvider _fileProvider;
    private readonly ILogger<FilesCleanerService> _logger;
    private readonly IMessageQueue<IEnumerable<FileInfo>> _messageQueue;


    public FilesCleanerService(
        IFileProvider fileProvider,
        ILogger<FilesCleanerService> logger,
        IMessageQueue<IEnumerable<FileInfo>> messageQueue)
    {
        _fileProvider = fileProvider;
        _logger = logger;
        _messageQueue = messageQueue;
    }
    
    public async Task Process(CancellationToken cancellationToken)
    {
        var fileInfos = await _messageQueue.ReadAsync(cancellationToken);
            
        foreach (var fileInfo in fileInfos)
        {
            await _fileProvider.RemoveFile(fileInfo, cancellationToken);
        }
    } 
}