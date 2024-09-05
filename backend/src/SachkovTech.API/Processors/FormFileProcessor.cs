using SachkovTech.Application.Modules.AddIssue;

namespace SachkovTech.API.Processors;

public class FormFileProcessor : IAsyncDisposable
{
    private readonly List<CreateFileCommand> _fileCommands = [];

    public List<CreateFileCommand> Process(IFormFileCollection files)
    {
        foreach (var file in files)
        {
            var stream = file.OpenReadStream();
            var fileDto = new CreateFileCommand(stream, file.FileName);
            _fileCommands.Add(fileDto);
        }

        return _fileCommands;
    }

    public async ValueTask DisposeAsync()
    {
        foreach (var file in _fileCommands)
        {
            await file.Content.DisposeAsync();
        }
    }
}