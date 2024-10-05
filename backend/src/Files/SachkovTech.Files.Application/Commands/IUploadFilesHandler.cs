using SachkovTech.Core.Abstractions;
using SachkovTech.Files.Application.Modles;

namespace SachkovTech.Files.Application.Commands
{
    public interface IUploadFilesHandler : ICommandHandler<UploadFilesResult, UploadFilesCommand>;
}
