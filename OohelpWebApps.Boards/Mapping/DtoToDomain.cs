using OohelpWebApps.Boards.Contracts.Common;
using OohelpWebApps.Boards.Database.Dto;

namespace OohelpWebApps.Boards.Mapping;

public static class DtoToDomain
{
    public static GridInfo ToDomain(this GridDto gridDto) =>
        new GridInfo
        {            
            Provider = (OutOfHome.DataProviders.Boards.Grids.Common.Enums.GridProvider)gridDto.Provider,            
            Downloaded = gridDto.Downloaded,
            Language = (OutOfHome.DataProviders.Language)gridDto.Language,
            BoardsCount = gridDto.BoardsCount                
        };
}
