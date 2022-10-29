using Microsoft.EntityFrameworkCore;
using OohelpWebApps.Boards.Database.Dto;

namespace OohelpWebApps.Boards.Database;
public class AppDbContext : DbContext
{
    public DbSet<GridDto> Grids { get; set; }
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        //Database.EnsureDeleted();
        //Database.EnsureCreated();
    }
}
