using OutOfHome.DataProviders.Boards.Grids.Common.Enums;


namespace OohelpWebApps.Boards.Contracts.Requests;
public class UpdateGridRequest : Request
{
    public GridProvider Provider { get; set; }
}
