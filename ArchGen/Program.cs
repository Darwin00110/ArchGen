using ArchGen.Application;
using ArchGen.Cli;
using ArchGen.InfraStructure;
using ArchGen.InfraStructure.Data;
using Microsoft.Extensions.DependencyInjection;

// Configure Dependency Injection Container
var services = new ServiceCollection();

// Add Layers
services.AddApplication();
services.AddInfraStructure("archgen.db");

// Add CLI Runner
services.AddTransient<CliRunner>();

var serviceProvider = services.BuildServiceProvider();

// Initialize SQLite database automatically
using (var scope = serviceProvider.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await dbContext.Database.EnsureCreatedAsync();
}

// Run CLI
var runner = serviceProvider.GetRequiredService<CliRunner>();
return await runner.RunAsync(args);
