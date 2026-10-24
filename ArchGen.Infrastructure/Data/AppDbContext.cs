using ArchGen.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ArchGen.InfraStructure.Data;

public class AppDbContext : DbContext
{
    public DbSet<ProjectHistory> ProjectHistories => Set<ProjectHistory>();
    public DbSet<TemplateDefinition> Templates => Set<TemplateDefinition>();
    public DbSet<TemplateFileDefinition> TemplateFiles => Set<TemplateFileDefinition>();

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ProjectHistory
        modelBuilder.Entity<ProjectHistory>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.ProjectName).IsRequired().HasMaxLength(200);
            entity.Property(e => e.ProjectPath).IsRequired().HasMaxLength(1000);
            entity.Property(e => e.ProjectType).IsRequired().HasMaxLength(50);
            entity.Property(e => e.TemplateUsed).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Status).IsRequired().HasMaxLength(50);
            entity.Property(e => e.CreatedAt).IsRequired();
        });

        // TemplateDefinition
        modelBuilder.Entity<TemplateDefinition>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.HasIndex(e => e.Name).IsUnique();
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.IsDefault).IsRequired();
            entity.Property(e => e.CreatedAt).IsRequired();

            entity.HasMany(e => e.Files)
                  .WithOne(f => f.Template)
                  .HasForeignKey(f => f.TemplateDefinitionId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // TemplateFileDefinition
        modelBuilder.Entity<TemplateFileDefinition>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.RelativePath).IsRequired().HasMaxLength(500);
            entity.Property(e => e.Content).IsRequired();
            entity.Property(e => e.Layer).IsRequired().HasMaxLength(100);
            entity.Property(e => e.IsForApi).IsRequired();
            entity.Property(e => e.IsForConsole).IsRequired();
        });
    }
}
