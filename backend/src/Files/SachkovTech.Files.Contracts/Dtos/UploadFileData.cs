namespace SachkovTech.Files.Contracts.Dtos
{
    public record UploadFileData(Stream Stream, string BucketName, string FileName, string Prefix = "");
}