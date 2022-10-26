using OohelpWebApps.Boards.Contracts.Common;

namespace OohelpWebApps.Boards.Contracts.Responses;
public class UpdateGridResponse : Response
{
    public GridInfo Info { get; set; }
}
