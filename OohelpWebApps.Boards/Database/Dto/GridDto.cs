namespace OohelpWebApps.Boards.Database.Dto;
public class GridDto
{
    public Guid Id { get; set; }
    public int Status { get; set; }    
    public DateTime Downloaded { get; set; }    
    public int Language { get; set; }    
    public int Provider { get; set; }   
    public int BoardsCount { get; set; }
}
