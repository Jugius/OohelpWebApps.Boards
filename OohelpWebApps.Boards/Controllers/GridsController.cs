using Microsoft.AspNetCore.Mvc;
using OohelpWebApps.Boards.Contracts;
using OohelpWebApps.Boards.Contracts.Requests;
using OohelpWebApps.Boards.Exceptions;
using OohelpWebApps.Boards.Mapping;
using OohelpWebApps.Boards.Services;
using OutOfHome.DataProviders.Boards.Grids.Common.Enums;

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

    [HttpGet("Download")]
    public IActionResult Index()
    {
        return Json("Message is OK");
    }


    [HttpPost("Update")]
    public async Task<ActionResult> Update(UpdateGridRequest request)
    {
        var result = await gridsService.UpdateGrid(request);
        return result.Success ? Json(result.Value.ToResponse()) : Json(result.Error.ToResponse());
    }

    [HttpGet("Newest")]
    public async Task<ActionResult> GetNewest(string provider)
    {
        if (string.IsNullOrEmpty(provider))
        {
            var multiResult = await gridsService.GetNewest();
            return multiResult.Success ? Json(multiResult.Value.ToResponse()) : Json(multiResult.Error.ToResponse());
        }

        if (Enum.TryParse<GridProvider>(provider, out GridProvider prov))
        {
            var result = await gridsService.GetNewest(prov);
            return result.Success ? Json(result.Value.ToResponse()) : Json(result.Error.ToResponse());
        }

        return Json(new ApiException(Contracts.Common.Enums.ResponseStatus.InvalidRequest).ToResponse());
    }
}
