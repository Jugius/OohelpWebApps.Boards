using OohelpWebApps.Boards.Contracts.Common;
using System.IO.Compression;

namespace OohelpWebApps.Boards.Services;

public static class ArchiveService
{
    public static async Task<OperationResult<bool>> Archive(string sourceFile, string archiveFile)
    {
        try
        {
            using FileStream originalFileStream = System.IO.File.OpenRead(sourceFile);
            using FileStream compressedFileStream = System.IO.File.Create(archiveFile);
            using var compressor = new GZipStream(compressedFileStream, CompressionMode.Compress);
            await originalFileStream.CopyToAsync(compressor);
            return OperationResult<bool>.FromResult(true);
        }
        catch (Exception ex)
        {
            return OperationResult<bool>.FromError(ex);
        }
    }
    public static async Task<OperationResult<bool>> Archive(GridInfo gridInfo)
    {
        string sourceFile = GetFilePath(gridInfo);
        string destinationFile = $"{sourceFile}.gz";

        var compressResult = await Archive(sourceFile, destinationFile);

        if (!compressResult.Success)
            return compressResult;

        try
        {
            System.IO.File.Delete(sourceFile);
            return OperationResult<bool>.FromResult(true);
        }
        catch (Exception ex)
        {
            return OperationResult<bool>.FromError(ex);
        }    
    }

    public static string GetFilePath(GridInfo gridInfo) => gridInfo.Status switch
    {
        Contracts.Common.Enums.GridStatus.Actual => System.IO.Path.Combine("DownloadedGrids", $"{gridInfo.Id}.resp"),
        Contracts.Common.Enums.GridStatus.Archived => System.IO.Path.Combine("DownloadedGrids", $"{gridInfo.Id}.resp.gz"),
    };
}
