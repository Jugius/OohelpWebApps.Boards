using Microsoft.EntityFrameworkCore;
using OohelpWebApps.Boards.Configurations.Downloading;
using OohelpWebApps.Boards.Contracts.Common;
using OohelpWebApps.Boards.Contracts.Common.Enums;
using OohelpWebApps.Boards.Contracts.Requests;
using OohelpWebApps.Boards.Database;
using OohelpWebApps.Boards.Database.Dto;
using OohelpWebApps.Boards.Exceptions;
using OohelpWebApps.Boards.Mapping;
using OutOfHome.DataProviders.Boards.Grids.Common.Enums;
using OutOfHome.DataProviders.Boards.Grids.Properties.Downloading;
using OutOfHome.DataProviders.Boards.Grids.Services;

namespace OohelpWebApps.Boards.Services;
public class GridsService
{
    private readonly DownloadConfigurationBuilder downloadConfigurationBuilder;
    private readonly AppDbContext dbContext;

    public GridsService(DownloadConfigurationBuilder downloadConfigurationBuilder, AppDbContext dbContext)
    {
        this.downloadConfigurationBuilder = downloadConfigurationBuilder;
        this.dbContext = dbContext;
    }

    public async Task<OperationResult<GridInfo>> UpdateGrid(UpdateGridRequest request)
    {
        DownloadProperties config = downloadConfigurationBuilder.Build(request.Provider);

        try
        {
            var response = await ResponseService.GetResponse(config);

            var currentActualGrid = await dbContext.Grids.FirstOrDefaultAsync(a => a.Provider == (int)response.Provider && a.Status == (int)GridStatus.Actual);
            if (currentActualGrid != null)
            {
                var archiveResult = await ArchiveService.Archive(currentActualGrid.ToDomain());

                if (archiveResult.Success)
                {
                    currentActualGrid.Status = (int)GridStatus.Archived;
                }
                else
                {
                    return OperationResult<GridInfo>.FromError(archiveResult.Error);
                }
            }
            GridDto newGridGto = new GridDto
            {
                Status = (int)GridStatus.Actual,
                BoardsCount = response.BoardsCount,
                Downloaded = response.Downloaded,
                Language = (int)response.Language,
                Provider = (int)response.Provider
            };

            dbContext.Grids.Add(newGridGto);
            await dbContext.SaveChangesAsync();

            string file = System.IO.Path.Combine("DownloadedGrids", $"{newGridGto.Id}.resp");

            await ResponseService.SaveAsync(response, new FileInfo(file));
            
            return OperationResult<GridInfo>.FromResult(newGridGto.ToDomain());
        }
        catch (Exception ex)
        {
            return OperationResult<GridInfo>.FromError($"Ошибка загрузки {request.Provider}: {ex.Message}");
        }        
    }
    public async Task<OperationResult<GridInfo>> GetNewest(GridProvider provider)
    {
        var dto = await dbContext.Grids.FirstOrDefaultAsync(a => a.Provider == (int)provider && a.Status == (int)GridStatus.Actual);
        if (dto == null)
            return OperationResult<GridInfo>.FromError(new ApiException(Contracts.Common.Enums.ResponseStatus.NotFound));
        return OperationResult<GridInfo>.FromResult(dto.ToDomain());
    }
    public async Task<OperationResult<GridInfo[]>> GetNewest()
    {
        var dtos = await dbContext.Grids.Where(a => a.Status == (int)GridStatus.Actual).ToArrayAsync();

        if(dtos?.Length > 0)
            return OperationResult<GridInfo[]>.FromResult(dtos.Select(a=>a.ToDomain()).ToArray());

        return OperationResult<GridInfo[]>.FromResult(Array.Empty<GridInfo>());
    }    
}
