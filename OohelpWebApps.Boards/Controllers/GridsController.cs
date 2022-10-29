using GridsDownloader.Mapping;
using Microsoft.AspNetCore.Mvc;
using OohelpWebApps.Boards.Configurations.Exporting;
using OohelpWebApps.Boards.Contracts.Requests;
using OohelpWebApps.Boards.Mapping;
using OohelpWebApps.Boards.Services;

namespace OohelpWebApps.Boards.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GridsController : Controller
{
    private readonly GridsService gridsService;

    public GridsController(GridsService gridsService)
    {
        this.gridsService = gridsService;
    }

    [HttpPost("Update")]
    public async Task<ActionResult> Update(UpdateGridRequest request)
    {
        var result = await gridsService.UpdateGrid(request);
        return result.Success ? Json(result.Value.ToResponse()) : Json(result.Error.ToResponse());
    }

    [HttpGet("Actual")]
    public async Task<ActionResult> GetNewest()
    {
        var multiResult = await gridsService.GetActual();
        return multiResult.Success ? Json(multiResult.Value.ToResponse()) : Json(multiResult.Error.ToResponse());
    }

    [HttpGet("Download")]
    public async Task<ActionResult> Download()
    {
        var boardsResult = await gridsService.CollectBoards();        

        if (!boardsResult.Success)
            return Json(boardsResult.Error.ToResponse());

        if (boardsResult.Value.Length == 0)
            return NotFound();

        var sheetSchema = ExportToExcelConfigurationBuilder.Build(boardsResult.Value);
        var excelBoards = boardsResult.Value.Select(a => a.ToExcelBoard(sheetSchema)).ToList();

        using var stream = new MemoryStream();

        OutOfHome.Excel.Services.ExcelBoardService service = new OutOfHome.Excel.Services.ExcelBoardService();
        await service.Export(excelBoards, stream, sheetSchema).ConfigureAwait(false);

        const string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        HttpContext.Response.ContentType = contentType;
        HttpContext.Response.Headers.Add("Access-Control-Expose-Headers", "Content-Disposition");

        var fileContentResult = new FileContentResult(stream.ToArray(), contentType)
        {
            FileDownloadName = $"gridExport_{DateTime.Now:yyyy_MM_dd}.xlsx"
        };

        return fileContentResult;
    }

}
