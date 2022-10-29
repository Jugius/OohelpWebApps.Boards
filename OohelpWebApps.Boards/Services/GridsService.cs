using Microsoft.EntityFrameworkCore;
using OohelpWebApps.Boards.Configurations.Downloading;
using OohelpWebApps.Boards.Configurations.Reading;
using OohelpWebApps.Boards.Contracts.Common;
using OohelpWebApps.Boards.Contracts.Common.Enums;
using OohelpWebApps.Boards.Contracts.Requests;
using OohelpWebApps.Boards.Database;
using OohelpWebApps.Boards.Database.Dto;
using OohelpWebApps.Boards.Mapping;
using OutOfHome.DataProviders.Boards.Grids.Common;
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
                var archiveResult = await FilesService.CompressResponseById(currentActualGrid.Id);

                if (archiveResult.Success)
                {
                    currentActualGrid.Status = (int)GridStatus.Archived;
                }
                else
                {
                    return OperationResult<GridInfo>.FromError(archiveResult.Error);
                }
            }
            GridDto newGridGto = response.ToDto();

            dbContext.Grids.Add(newGridGto);
            await dbContext.SaveChangesAsync();

            string file = FilesService.GetFilePath(newGridGto.Id);

            await ResponseService.SaveAsync(response, new FileInfo(file));
            
            return OperationResult<GridInfo>.FromResult(newGridGto.ToDomain());
        }
        catch (Exception ex)
        {
            return OperationResult<GridInfo>.FromError($"Ошибка загрузки {request.Provider}: {ex.Message}");
        }        
    }
    public async Task<OperationResult<GridInfo[]>> GetActual()
    {
        var dtos = await dbContext.Grids.Where(a => a.Status == (int)GridStatus.Actual).ToArrayAsync();

        if(dtos?.Length > 0)
            return OperationResult<GridInfo[]>.FromResult(dtos.Select(a=>a.ToDomain()).ToArray());

        return OperationResult<GridInfo[]>.FromResult(Array.Empty<GridInfo>());
    }

    public async Task<OperationResult<GridBoard[]>> CollectBoards()
    {
        var dtos = await dbContext.Grids.Where(a => a.Status == (int)GridStatus.Actual).ToArrayAsync();

        if (dtos.Length == 0)
            return OperationResult<GridBoard[]>.FromResult(Array.Empty<GridBoard>());


        var grids = await Task.WhenAll(dtos.Select(a => LoadGridAsync(a.Id, (GridProvider)a.Provider)));
        var boards = grids.SelectMany(a => a.Boards).ToArray();

        return OperationResult<GridBoard[]>.FromResult(boards);
    }
    private async Task<Grid> LoadGridAsync(Guid responseId, GridProvider provider)
    {
        string file = FilesService.GetFilePath(responseId);
        var response = await ResponseService.LoadAsync(new FileInfo(file), provider);

        var readProperties = new ReadGridConfigurationBuilder(response.Provider).Build();
        return await GridService.LoadGrid(response, readProperties);
    }

}
