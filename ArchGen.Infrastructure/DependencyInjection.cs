using ArchGen.Domain.Interfaces;
using ArchGen.InfraStructure.Data;
using ArchGen.InfraStructure.Repositories;
using ArchGen.InfraStructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ArchGen.InfraStructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfraStructure(this IServiceCollection services, string? databasePath = null)
    {
        var dbFile = databasePath ?? "archgen.db";

        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite($"Data Source={dbFile}"));

        services.AddScoped<IProjectHistoryRepository, ProjectHistoryRepository>();
        services.AddScoped<ITemplateRepository, TemplateRepository>();
        services.AddSingleton<IDefaultTemplateProvider, DefaultTemplateProvider>();
        services.AddSingleton<IFileService, PhysicalFileService>();
        services.AddSingleton<ITerminalService, TerminalService>();

        return services;
    }
}
