using ArchGen.Domain;
using Microsoft.EntityFrameworkCore;

namespace ArchGen.InfraStructure;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }
    public DbSet<TemplateStructure> Template { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<TemplateStructure>(entity =>
        {
            entity.HasKey(x => x.ID);
            entity.OwnsOne(x => x.paths, paths =>
            {
               paths.OwnsOne(x => x.Domain);
               paths.OwnsOne(x => x.Application);
               paths.OwnsOne(x => x.InfraStructure);
               paths.OwnsOne(x => x.Tests);
               paths.OwnsOne(x => x.Console_OR_API);
            });
        });
    }
}
