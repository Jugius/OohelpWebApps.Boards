using System.IO.Compression;

namespace OohelpWebApps.Boards.Services;

public static class FilesService
{
    public static async Task<OperationResult<bool>> CompressFile(string sourceFile, string archiveFile)
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
    public static async Task<OperationResult<bool>> CompressResponseById(Guid responseId)
    {
        string sourceFile = GetFilePath(responseId);
        string destinationFile = $"{sourceFile}.gz";

        var compressResult = await CompressFile(sourceFile, destinationFile);

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

    public static string GetFilePath(Guid responseId) => System.IO.Path.Combine("DownloadedGrids", $"{responseId}.resp");
}
