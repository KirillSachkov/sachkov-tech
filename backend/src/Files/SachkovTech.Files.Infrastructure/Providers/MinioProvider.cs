using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Minio;
using Minio.DataModel.Args;
using SachkovTech.Files.Application.Modles;
using SachkovTech.Files.Domain.ValueObjects;
using SachkovTech.Files.Infrastructure.Interfaces;
using SachkovTech.Files.Infrastructure.Models;
using SachkovTech.SharedKernel;
using System.Runtime.CompilerServices;

namespace SachkovTech.Files.Infrastructure.Providers
{
    internal class MinioProvider : IFileProvider
    {
        private const int MAX_DEGREE_OF_PARALLELISM = 10;

        private readonly IMinioClient _minioClient;
        private readonly ILogger<MinioProvider> _logger;

        public MinioProvider(IMinioClient minioClient, ILogger<MinioProvider> logger)
        {
            _minioClient = minioClient;
            _logger = logger;
        }

        public async IAsyncEnumerable<Result<UploadFilesResponse, Error>> UploadFiles(
        IEnumerable<UploadFileData> filesData,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            SemaphoreSlim semaphoreSlim = new SemaphoreSlim(MAX_DEGREE_OF_PARALLELISM);

            List<UploadFilesResponse> results = new();

            try
            {
                await IfBucketsNotExistCreateBucket(filesData.Select(file => file.BucketName).Distinct(), cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Fail to upload files in minio, files amount: {amount}", filesData.Count());

                yield break;
            }

            var tasks = filesData.Select(async file =>
                    await PutObject(file, semaphoreSlim, cancellationToken)).ToList();

            while (tasks.Count() > 0)
            {
                var task = await Task.WhenAny(tasks);

                var result = await task;

                tasks.Remove(task);

                if (result.IsSuccess)
                    results.Add(result.Value);

                yield return result;
            }

            _logger.LogInformation("Uploaded files: {files}", results.Select(f => f.FilePath.Value));
        }

        public async Task<UnitResult<Error>> RemoveFile(
            FileLocation filesLocation,
            CancellationToken cancellationToken = default)
        {
            try
            {
                await IfBucketsNotExistCreateBucket([filesLocation.BucketName], cancellationToken);

                var statArgs = new StatObjectArgs()
                    .WithBucket(filesLocation.BucketName)
                    .WithObject(filesLocation.FilePath.Value);

                var objectStat = await _minioClient.StatObjectAsync(statArgs, cancellationToken);
                if (objectStat is null)
                    return Result.Success<Error>();

                var removeArgs = new RemoveObjectArgs()
                    .WithBucket(filesLocation.BucketName)
                    .WithObject(filesLocation.FilePath.Value);

                await _minioClient.RemoveObjectAsync(removeArgs, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Fail to remove file in minio with path {path} in bucket {bucket}",
                    filesLocation.FilePath.Value,
                    filesLocation.BucketName);

                return Error.Failure("file.upload", "Fail to upload file in minio");
            }

            return Result.Success<Error>();
        }



        private async Task<Result<UploadFilesResponse, Error>> PutObject(
            UploadFileData fileData,
            SemaphoreSlim semaphoreSlim,
            CancellationToken cancellationToken)
        {
            await semaphoreSlim.WaitAsync(cancellationToken);

            var fileNameGuid = Guid.NewGuid().ToString();
            var fileExtension = Path.GetExtension(fileData.FileName);

            var putObjectArgs = new PutObjectArgs()
                .WithBucket(fileData.BucketName)
                .WithStreamData(fileData.Stream)
                .WithObjectSize(fileData.Stream.Length)
                .WithObject(fileData.Preffix + fileNameGuid + fileExtension);

            try
            {
                var response = await _minioClient
                    .PutObjectAsync(putObjectArgs, cancellationToken);

                _logger.LogInformation("Uploaded file with path {path}", response.ObjectName);

                var filePath = FilePath.Create(response.ObjectName).Value;
                var fileSize = FileSize.Create(response.Size).Value;

                var result = new UploadFilesResponse(
                    fileData.BucketName,
                    fileData.FileName,
                    filePath,
                    fileSize);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Fail to upload file in minio with path {path} in bucket {bucket}",
                    fileData.FileName,
                    fileData.BucketName);

                return Error.Failure("file.upload", "Fail to upload file in minio");
            }
            finally
            {
                semaphoreSlim.Release();
            }
        }

        private async Task IfBucketsNotExistCreateBucket(
            IEnumerable<string> buckets,
            CancellationToken cancellationToken)
        {
            HashSet<string> bucketNames = [.. buckets];

            foreach (var bucketName in bucketNames)
            {
                var bucketExistArgs = new BucketExistsArgs()
                    .WithBucket(bucketName);

                var bucketExist = await _minioClient
                    .BucketExistsAsync(bucketExistArgs, cancellationToken);

                if (bucketExist == false)
                {
                    var makeBucketArgs = new MakeBucketArgs()
                        .WithBucket(bucketName);

                    await _minioClient.MakeBucketAsync(makeBucketArgs, cancellationToken);
                }
            }
        }
    }
}
