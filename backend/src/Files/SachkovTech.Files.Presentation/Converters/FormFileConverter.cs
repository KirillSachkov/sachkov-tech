
using Microsoft.AspNetCore.Http;
using SachkovTech.Core.Dtos;

namespace SachkovTech.Files.Presentation.Converters
{
    public class FormFileConverter : IAsyncDisposable
    {
        public readonly IFormFileCollection _formFileCollection;
        public readonly List<Stream> _fileStreams;

        public FormFileConverter(IFormFileCollection formFileCollection)
        {
            _formFileCollection = formFileCollection;
            _fileStreams = new();
        }

        public IEnumerable<UploadFileDto> ToUploadFileDtos()
        {
            List<UploadFileDto> fileDtos = [];

            foreach (var file in _formFileCollection)
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
}
