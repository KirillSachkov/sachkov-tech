// using CSharpFunctionalExtensions;
// using Microsoft.Extensions.Logging;
// using Minio;
// using Minio.DataModel.Args;
// using SachkovTech.Application.Files;
// using SachkovTech.Core;
// using SachkovTech.IssuesManagement.Domain.ValueObjects;
// using FileInfo = SachkovTech.Application.Files.FileInfo;
//
// namespace SachkovTech.IssuesManagement.Infrastructure.Providers;
//
// public class MinioProvider : IFileProvider
// {
//     private const int MAX_DEGREE_OF_PARALLELISM = 10;
//
//     private readonly IMinioClient _minioClient;
//     private readonly ILogger<MinioProvider> _logger;
//
//     public MinioProvider(IMinioClient minioClient, ILogger<MinioProvider> logger)
//     {
//         _minioClient = minioClient;
//         _logger = logger;
//     }
//
//     public async Task<Result<IReadOnlyList<FilePath>, Error>> UploadFiles(
//         IEnumerable<FileData> filesData,
//         CancellationToken cancellationToken = default)
//     {
//         var semaphoreSlim = new SemaphoreSlim(MAX_DEGREE_OF_PARALLELISM);
//         var filesList = filesData.ToList();
//
//         try
//         {
//             await IfBucketsNotExistCreateBucket(filesList.Select(file => file.Info.BucketName), cancellationToken);
//
//             var tasks = filesList.Select(async file =>
//                 await PutObject(file, semaphoreSlim, cancellationToken));
//
//             var pathsResult = await Task.WhenAll(tasks);
//
//             if (pathsResult.Any(p => p.IsFailure))
//                 return pathsResult.First().Error;
//
//             var results = pathsResult.Select(p => p.Value).ToList();
//
//             _logger.LogInformation("Uploaded files: {files}", results.Select(f => f.Path));
//
//             return results;
//         }
//         catch (Exception ex)
//         {
//             _logger.LogError(ex,
//                 "Fail to upload files in minio, files amount: {amount}", filesList.Count);
//
//             return Error.Failure("file.upload", "Fail to upload files in minio");
//         }
//     }
//
//     public async Task<UnitResult<Error>> RemoveFile(
//         FileInfo fileInfo,
//         CancellationToken cancellationToken = default)
//     {
//         try
//         {
//             await IfBucketsNotExistCreateBucket([fileInfo.BucketName], cancellationToken);
//
//             var statArgs = new StatObjectArgs()
//                 .WithBucket(fileInfo.BucketName)
//                 .WithObject(fileInfo.FilePath.Path);
//
//             var objectStat = await _minioClient.StatObjectAsync(statArgs, cancellationToken);
//             if (objectStat is null)
//                 return Result.Success<Error>();
//
//             var removeArgs = new RemoveObjectArgs()
//                 .WithBucket(fileInfo.BucketName)
//                 .WithObject(fileInfo.FilePath.Path);
//
//             await _minioClient.RemoveObjectAsync(removeArgs, cancellationToken);
//         }
//         catch (Exception ex)
//         {
//             _logger.LogError(ex,
//                 "Fail to remove file in minio with path {path} in bucket {bucket}",
//                 fileInfo.FilePath.Path,
//                 fileInfo.BucketName);
//
//             return Error.Failure("file.upload", "Fail to upload file in minio");
//         }
//
//         return Result.Success<Error>();
//     }
//
//     private async Task<Result<FilePath, Error>> PutObject(
//         FileData fileData,
//         SemaphoreSlim semaphoreSlim,
//         CancellationToken cancellationToken)
//     {
//         await semaphoreSlim.WaitAsync(cancellationToken);
//
//         var putObjectArgs = new PutObjectArgs()
//             .WithBucket(fileData.Info.BucketName)
//             .WithStreamData(fileData.Stream)
//             .WithObjectSize(fileData.Stream.Length)
//             .WithObject(fileData.Info.FilePath.Path);
//
//         try
//         {
//             await _minioClient
//                 .PutObjectAsync(putObjectArgs, cancellationToken);
//
//             return fileData.Info.FilePath;
//         }
//         catch (Exception ex)
//         {
//             _logger.LogError(ex,
//                 "Fail to upload file in minio with path {path} in bucket {bucket}",
//                 fileData.Info.FilePath.Path,
//                 fileData.Info.BucketName);
//
//             return Error.Failure("file.upload", "Fail to upload file in minio");
//         }
//         finally
//         {
//             semaphoreSlim.Release();
//         }
//     }
//
//     private async Task IfBucketsNotExistCreateBucket(
//         IEnumerable<string> buckets,
//         CancellationToken cancellationToken)
//     {
//         HashSet<string> bucketNames = [..buckets];
//
//         foreach (var bucketName in bucketNames)
//         {
//             var bucketExistArgs = new BucketExistsArgs()
//                 .WithBucket(bucketName);
//
//             var bucketExist = await _minioClient
//                 .BucketExistsAsync(bucketExistArgs, cancellationToken);
//
//             if (bucketExist == false)
//             {
//                 var makeBucketArgs = new MakeBucketArgs()
//                     .WithBucket(bucketName);
//
//                 await _minioClient.MakeBucketAsync(makeBucketArgs, cancellationToken);
//             }
//         }
//     }
// }