using ArchGen.Application.Interfaces;
using ArchGen.Application.UseCases;
using Microsoft.Extensions.DependencyInjection;

namespace ArchGen.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IGenerateProjectUseCase, GenerateProjectUseCase>();
        services.AddScoped<ITemplateUseCase, TemplateUseCase>();
        services.AddScoped<IProjectHistoryUseCase, ProjectHistoryUseCase>();
        services.AddScoped<IHelpUseCase, HelpUseCase>();

        return services;
    }
}
