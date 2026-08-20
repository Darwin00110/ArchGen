using ArchGen.Domain;
using Microsoft.EntityFrameworkCore;

namespace ArchGen.Infrastructure;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    public DbSet<ArchGenEntity> DataUser { get; set; }
}
