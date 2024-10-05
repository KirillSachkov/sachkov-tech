using SachkovTech.Core.Abstractions;
using SachkovTech.Files.Application.Modles;

namespace SachkovTech.Files.Application.Commands.UploadFiles
{
    public record UploadFilesCommand(string ownerTypeName, Guid ownerId, IEnumerable<UploadFileData> Files) : ICommand;
}
