namespace SachkovTech.Files.Application.Modles
{
    public record UploadFileData(Stream Stream, string BucketName, string FileName, string Preffix = "");
}
