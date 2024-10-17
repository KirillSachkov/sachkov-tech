namespace SachkovTech.Files.Application.Dtos;

public record UploadFileData(Stream Stream, string BucketName, string FileName, string Prefix = "");