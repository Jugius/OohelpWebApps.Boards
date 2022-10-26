
using OohelpWebApps.Boards.Contracts.Common;

namespace OohelpWebApps.Boards.Contracts.Responses;
public class GridsInfoResponse : Response
{
    public GridInfo[] Grids { get; set; }
}
