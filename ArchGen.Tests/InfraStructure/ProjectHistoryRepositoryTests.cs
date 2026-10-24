using ArchGen.Domain.Entities;
using ArchGen.InfraStructure.Data;
using ArchGen.InfraStructure.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ArchGen.Tests.InfraStructure;

public class ProjectHistoryRepositoryTests
{
    private AppDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    [Fact]
    public async Task AddAsync_And_GetAllAsync_Should_Persist_And_Retrieve_Histories()
    {
        using var context = CreateDbContext();
        var repo = new ProjectHistoryRepository(context);

        var h1 = new ProjectHistory("App1", @"C:\App1", "API", "Default");
        var h2 = new ProjectHistory("App2", @"C:\App2", "CONSOLE", "Default");

        await repo.AddAsync(h1);
        await repo.AddAsync(h2);

        var all = await repo.GetAllAsync();

        all.Should().HaveCount(2);
        all.Select(h => h.ProjectName).Should().Contain(new[] { "App1", "App2" });
    }

    [Fact]
    public async Task GetByIdAsync_Should_Return_Correct_Entity()
    {
        using var context = CreateDbContext();
        var repo = new ProjectHistoryRepository(context);

        var h = new ProjectHistory("AppX", @"C:\AppX", "API", "Default");
        await repo.AddAsync(h);

        var retrieved = await repo.GetByIdAsync(h.Id);

        retrieved.Should().NotBeNull();
        retrieved!.ProjectName.Should().Be("AppX");
        retrieved.ProjectPath.Should().Be(@"C:\AppX");
    }
}
