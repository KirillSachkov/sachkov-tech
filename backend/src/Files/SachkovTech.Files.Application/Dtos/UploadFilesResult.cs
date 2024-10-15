using SachkovTech.Files.Domain.ValueObjects;

namespace SachkovTech.Files.Application.Dtos;

public record UploadFilesResult(string BucketName, string FileName, FilePath FilePath, FileSize FileSize);