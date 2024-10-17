using SachkovTech.Core.Abstractions;
using SachkovTech.Files.Contracts.Dtos;

namespace SachkovTech.Files.Application.Commands.UploadFiles;

public record UploadFilesCommand(string OwnerTypeName, Guid OwnerId, IEnumerable<UploadFileDto> Files) : ICommand;