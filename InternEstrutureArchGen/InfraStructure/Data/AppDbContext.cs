using Domain;
using Microsoft.EntityFrameworkCore;
namespace InfraStructure;
public class AppDbContext : DbContext       
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    public DbSet<User> Users { get; set; }
}
