using ArchGen.Domain.Entities;
using ArchGen.InfraStructure.Data;
using ArchGen.InfraStructure.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ArchGen.Tests.InfraStructure;

public class TemplateRepositoryTests
{
    private AppDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    [Fact]
    public async Task AddAsync_And_GetByNameAsync_Should_Persist_Template_With_Files()
    {
        using var context = CreateDbContext();
        var repo = new TemplateRepository(context);

        var template = new TemplateDefinition("CustomMicroservice", "Template para microsserviços");
        template.AddOrUpdateFile("src/Domain/Order.cs", "// Order entity", "Domain");
        template.AddOrUpdateFile("src/API/Program.cs", "// API Program", "API");

        await repo.AddAsync(template);

        var retrieved = await repo.GetByNameAsync("CustomMicroservice");

        retrieved.Should().NotBeNull();
        retrieved!.Name.Should().Be("CustomMicroservice");
        retrieved.Files.Should().HaveCount(2);
    }

    [Fact]
    public async Task ExistsByNameAsync_Should_Return_True_When_Exists()
    {
        using var context = CreateDbContext();
        var repo = new TemplateRepository(context);

        var template = new TemplateDefinition("Existente", "Desc");
        await repo.AddAsync(template);

        var exists = await repo.ExistsByNameAsync("existente");
        exists.Should().BeTrue();

        var notExists = await repo.ExistsByNameAsync("NaoExiste");
        notExists.Should().BeFalse();
    }

    [Fact]
    public async Task UpdateAsync_Should_Update_Files_And_Metadata()
    {
        using var context = CreateDbContext();
        var repo = new TemplateRepository(context);

        var template = new TemplateDefinition("Editavel", "Desc");
        template.AddOrUpdateFile("file.cs", "// v1", "Domain");
        await repo.AddAsync(template);

        template.AddOrUpdateFile("file.cs", "// v2", "Domain");
        await repo.UpdateAsync(template);

        var updated = await repo.GetByNameAsync("Editavel");
        updated!.GetFile("file.cs")!.Content.Should().Be("// v2");
    }

    [Fact]
    public async Task DeleteAsync_Should_Remove_Template_And_Cascade_Files()
    {
        using var context = CreateDbContext();
        var repo = new TemplateRepository(context);

        var template = new TemplateDefinition("Deletavel", "Desc");
        template.AddOrUpdateFile("file1.cs", "//", "Domain");
        template.AddOrUpdateFile("file2.cs", "//", "API");
        await repo.AddAsync(template);

        await repo.DeleteAsync(template);

        var retrieved = await repo.GetByNameAsync("Deletavel");
        retrieved.Should().BeNull();

        var filesCount = await context.TemplateFiles.CountAsync();
        filesCount.Should().Be(0);
    }
}
