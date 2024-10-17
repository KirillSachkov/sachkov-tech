using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Minio;
using Minio.DataModel.Args;
using SachkovTech.Files.Application.Dtos;
using SachkovTech.Files.Application.Interfaces;
using SachkovTech.Files.Domain.ValueObjects;
using SachkovTech.SharedKernel;
using System.Runtime.CompilerServices;

namespace SachkovTech.Files.Infrastructure.Providers;

internal class MinioProvider : IFileProvider
{
    private const int MAX_DEGREE_OF_PARALLELISM = 10;
    private const int LINK_EXPIRY = 604800;

    private readonly IMinioClient _minioClient;
    private readonly ILogger<MinioProvider> _logger;

    public MinioProvider(IMinioClient minioClient, ILogger<MinioProvider> logger)
    {
        _minioClient = minioClient;
        _logger = logger;
    }

    public async IAsyncEnumerable<Result<UploadFilesResult, Error>> UploadFiles(
        IEnumerable<UploadFileData> filesData,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var semaphoreSlim = new SemaphoreSlim(MAX_DEGREE_OF_PARALLELISM);

        List<UploadFilesResult> results = [];

        var uploadFileDatas = filesData.ToList();

        var bucketsExistResult = await IfBucketsNotExistCreateBucket(
            uploadFileDatas.Select(file => file.BucketName).Distinct(),
            cancellationToken);

        if (bucketsExistResult.IsFailure)
            yield break;

        var tasks = uploadFileDatas.Select(async file =>
            await PutObject(file, semaphoreSlim, cancellationToken)).ToList();

        while (tasks.Count != 0)
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

    public async Task<IEnumerable<GetLinkFileResult>> GetLinks(
            IEnumerable<FilePath> filePaths,
            CancellationToken cancellationToken = default)
    {
        var semaphoreSlim = new SemaphoreSlim(MAX_DEGREE_OF_PARALLELISM);

        var bucketsExistResult = await IfBucketsNotExistCreateBucket(
            filePaths.Select(file => file.BucketName).Distinct(),
            cancellationToken);

        if (bucketsExistResult.IsFailure)
            return [];

        var tasks = filePaths.Select(async file =>
                await GetLink(file, semaphoreSlim, cancellationToken)).ToList();

        await Task.WhenAll(tasks);

        var results = tasks.Select(t => t.Result).ToList();

        _logger.LogInformation("Received links to files: {links}", results.Select(r => r.FilePath));

        return results;
    }

    public async Task<UnitResult<Error>> RemoveFile(
        FilePath filePath,
        CancellationToken cancellationToken = default)
    {
        var storagePath = filePath.Prefix + "/" + filePath.FileName;

        var bucketsExistResult = await IfBucketsNotExistCreateBucket([filePath.BucketName], cancellationToken);

        if (bucketsExistResult.IsFailure)
            return bucketsExistResult.Error;

        try
        {

            var statArgs = new StatObjectArgs()
                .WithBucket(filePath.BucketName)
                .WithObject(storagePath);

            var objectStat = await _minioClient.StatObjectAsync(statArgs, cancellationToken);
            if (objectStat.ContentType == null)
                return Result.Success<Error>();

            var removeArgs = new RemoveObjectArgs()
                .WithBucket(filePath.BucketName)
                .WithObject(storagePath);

            await _minioClient.RemoveObjectAsync(removeArgs, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Fail to remove file in minio with path {path} in bucket {bucket}",
                storagePath,
                filePath.BucketName);

            return Error.Failure("file.upload", "Fail to upload file in minio");
        }

        return Result.Success<Error>();
    }


    private async Task<Result<UploadFilesResult, Error>> PutObject(
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
            .WithObject(fileData.Prefix + "/" + fileNameGuid + fileExtension);

        try
        {
            var response = await _minioClient
                .PutObjectAsync(putObjectArgs, cancellationToken);

            _logger.LogInformation("Uploaded file with path {path}", response.ObjectName);

            var filePath = FilePath.Create(fileData.BucketName + "/" + response.ObjectName).Value;
            var fileSize = FileSize.Create(response.Size).Value;

            var result = new UploadFilesResult(
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

    private async Task<GetLinkFileResult> GetLink(
            FilePath filePath,
            SemaphoreSlim semaphoreSlim,
            CancellationToken cancellationToken)
    {
        var objectName = filePath.Prefix + "/" + filePath.FileName;

        await semaphoreSlim.WaitAsync(cancellationToken);


        var statArgs = new StatObjectArgs()
            .WithBucket(filePath.BucketName)
            .WithObject(objectName);

        var getLinkArgs = new PresignedGetObjectArgs()
            .WithBucket(filePath.BucketName)
            .WithObject(objectName)
            .WithExpiry(LINK_EXPIRY);

        try
        {
            var statFile = await _minioClient.StatObjectAsync(statArgs);

            if (statFile.ContentType == null)
            {
                _logger.LogError(
                    "The file with the bucket \"{BucketName}\" and the name \"{FileName}\" was not found",
                    filePath.BucketName,
                    objectName);

                return new GetLinkFileResult(filePath, "");
            }


            var getLinkResult = await _minioClient.PresignedGetObjectAsync(getLinkArgs);

            if (getLinkResult is null)
            {
                _logger.LogError(
                    "The file with the bucket \"{BucketName}\" and the name \"{FileName}\" was not found",
                    filePath.BucketName,
                    objectName);

                return new GetLinkFileResult(filePath, "");
            }


            return new GetLinkFileResult(filePath, getLinkResult);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Fail to get link file from minio with path {path} in bucket {bucket}",
                filePath.Value,
                filePath.BucketName);

            return new GetLinkFileResult(filePath, "");
        }
        finally
        {
            semaphoreSlim.Release();
        }
    }

    private async Task<UnitResult<Error>> IfBucketsNotExistCreateBucket(
        IEnumerable<string> buckets,
        CancellationToken cancellationToken)
    {
        HashSet<string> bucketNames = [.. buckets];

        foreach (var bucketName in bucketNames)
        {
            try
            {
                var bucketExistArgs = new BucketExistsArgs()
                    .WithBucket(bucketName);

                var bucketExist = await _minioClient
                    .BucketExistsAsync(bucketExistArgs, cancellationToken);

                if (bucketExist) continue;

                var makeBucketArgs = new MakeBucketArgs()
                    .WithBucket(bucketName);

                await _minioClient.MakeBucketAsync(makeBucketArgs, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error checking the existence of a bucket named : {bucketName}", bucketName);

                return Error.Failure("file.bucket.not.exists",
                    $"Error checking the existence of a bucket named : {bucketName}");
            }
        }

        return UnitResult.Success<Error>();
    }
}