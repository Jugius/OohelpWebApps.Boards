using OohelpWebApps.Boards.Contracts.Common;
using OohelpWebApps.Boards.Contracts.Common.Enums;
using OohelpWebApps.Boards.Database.Dto;

namespace OohelpWebApps.Boards.Mapping;
public static class DomainToDto
{
    //public static GridDto ToDto(this GridInfo gridInfo) =>
    //    new GridDto
    //    {
    //        Id = gridInfo.Id,
    //        Downloaded = gridInfo.Downloaded,
    //        Provider = (int)gridInfo.Provider,
    //        BoardsCount = gridInfo.BoardsCount,
    //        Language = (int)gridInfo.Language,
    //        Status = (int)gridInfo.Status
    //    };

    public static GridDto ToDto(this OutOfHome.DataProviders.Boards.Grids.Interfaces.IResponse response) =>
        new GridDto {
            Downloaded = response.Downloaded,
            Provider = (int)response.Provider,
            BoardsCount = response.BoardsCount,
            Language = (int)response.Language,
            Status = (int)GridStatus.Actual           
        };
}
