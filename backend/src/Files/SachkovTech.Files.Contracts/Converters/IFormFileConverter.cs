using Microsoft.AspNetCore.Http;
using SachkovTech.Files.Contracts.Dtos;

namespace SachkovTech.Files.Contracts.Converters;

public interface IFormFileConverter : IAsyncDisposable
{
    IEnumerable<UploadFileDto> ToUploadFileDtos(IFormFileCollection formFiles);
}