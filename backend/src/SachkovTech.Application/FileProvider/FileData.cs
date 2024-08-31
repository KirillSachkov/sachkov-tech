namespace SachkovTech.Application.FileProvider;

public record FileData(IEnumerable<FileContent> Files, string BucketName);

public record FileContent(Stream Stream, string ObjectName);