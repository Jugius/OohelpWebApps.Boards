using OohelpWebApps.Boards.Contracts.Common;

namespace OohelpWebApps.Boards.Mapping;
public static class DomainToResponse
{
    public static Contracts.Responses.UpdateGridResponse ToResponse(this GridInfo info) =>
        new Contracts.Responses.UpdateGridResponse
        {
            Info = info,
            Status = Contracts.Common.Enums.ResponseStatus.Ok
        };

    public static Contracts.Responses.GridsInfoResponse ToResponse(this GridInfo[] infos) =>
        new Contracts.Responses.GridsInfoResponse
        {
            Grids = infos,
            Status = Contracts.Common.Enums.ResponseStatus.Ok
        };
}
