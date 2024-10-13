using Microsoft.AspNetCore.Http;
using SachkovTech.Files.Contracts.Converters;
using SachkovTech.Files.Contracts.Dtos;

namespace SachkovTech.Files.Presentation;

public class FormFileConverter : IFormFileConverter
{
    private readonly List<Stream> _fileStreams = [];

    public IEnumerable<UploadFileDto> ToUploadFileDtos(IFormFileCollection formFiles)
    {
        List<UploadFileDto> fileDtos = [];

        foreach (var file in formFiles)
        {
            var stream = file.OpenReadStream();
            var fileDto = new UploadFileDto(stream, file.FileName);
            fileDtos.Add(fileDto);
            _fileStreams.Add(fileDto.Content);
        }

        return fileDtos;
    }

    public async ValueTask DisposeAsync()
    {
        foreach (var item in _fileStreams)
            await item.DisposeAsync();
    }
}