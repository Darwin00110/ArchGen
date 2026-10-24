using ArchGen.Application.UseCases;
using ArchGen.Domain.Entities;
using ArchGen.Domain.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;

namespace ArchGen.Tests.Application;

public class ProjectHistoryUseCaseTests
{
    private readonly Mock<IProjectHistoryRepository> _historyRepoMock;
    private readonly ProjectHistoryUseCase _useCase;

    public ProjectHistoryUseCaseTests()
    {
        _historyRepoMock = new Mock<IProjectHistoryRepository>();
        _useCase = new ProjectHistoryUseCase(_historyRepoMock.Object);
    }

    [Fact]
    public async Task GetAllHistoryAsync_Should_Return_Mapped_Dtos_Ordered_Descending()
    {
        var h1 = new ProjectHistory("Proj1", @"C:\Proj1", "API", "Default") { Id = 1, CreatedAt = DateTime.UtcNow.AddMinutes(-10) };
        var h2 = new ProjectHistory("Proj2", @"C:\Proj2", "CONSOLE", "Custom") { Id = 2, CreatedAt = DateTime.UtcNow };

        _historyRepoMock.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<ProjectHistory> { h1, h2 });

        var list = await _useCase.GetAllHistoryAsync();

        list.Should().HaveCount(2);
        list[0].ProjectName.Should().Be("Proj2");
        list[1].ProjectName.Should().Be("Proj1");
    }

    [Fact]
    public async Task GetHistoryByIdAsync_Should_Return_History_When_Found()
    {
        var h = new ProjectHistory("Proj1", @"C:\Proj1", "API", "Default") { Id = 1 };
        _historyRepoMock.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(h);

        var result = await _useCase.GetHistoryByIdAsync(1);

        result.Should().NotBeNull();
        result!.ProjectName.Should().Be("Proj1");
    }

    [Fact]
    public async Task GetHistoryByIdAsync_Should_Return_Null_When_Not_Found()
    {
        _historyRepoMock.Setup(r => r.GetByIdAsync(999, It.IsAny<CancellationToken>()))
            .ReturnsAsync((ProjectHistory?)null);

        var result = await _useCase.GetHistoryByIdAsync(999);
        result.Should().BeNull();
    }
}
