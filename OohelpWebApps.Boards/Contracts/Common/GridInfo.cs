using OohelpWebApps.Boards.Contracts.Common.Enums;
using OutOfHome.DataProviders;
using OutOfHome.DataProviders.Boards.Grids.Common.Enums;
using System.Text.Json.Serialization;

namespace OohelpWebApps.Boards.Contracts.Common;
public class GridInfo
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
    
    
    [JsonPropertyName("status")]
    public GridStatus Status { get; set; }


    [JsonPropertyName("downloaded")]
    public DateTime Downloaded { get; set; }


    [JsonPropertyName("language")]
    public Language Language { get; set; }


    [JsonPropertyName("provider")]
    public GridProvider Provider { get; set; }


    [JsonPropertyName("count")]
    public int BoardsCount { get; set; }
}
