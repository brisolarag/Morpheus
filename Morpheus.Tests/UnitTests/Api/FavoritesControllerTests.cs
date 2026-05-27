using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Morpheus.Api.Controllers;
using Morpheus.Api.Data;
using Morpheus.Shareds.Entities;
using Moq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Morpheus.Tests.UnitTests.Api;

public class FavoritesControllerTests
{
    private readonly AppDbContext _dbContext;
    private readonly FavoritesController _controller;

    public FavoritesControllerTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        
        _dbContext = new AppDbContext(options);

        _controller = new FavoritesController(_dbContext);
        
        // Mock User Claims
        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.NameIdentifier, "test-user-id")
        }, "mock"));

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };
    }

    [Fact]
    public async Task AddFavorite_ReturnsOk_WhenJobExists()
    {
        // Arrange
        var jobId = Guid.NewGuid();
        _dbContext.Jobs.Add(new Job { Id = jobId, ExternalJobId = "123", Title = "Dev", OriginalDescription = "Desc", SeniorityLevel = "Mid" });
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _controller.AddFavorite(jobId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(okResult.Value);
    }

    [Fact]
    public async Task AddFavorite_ReturnsNotFound_WhenJobDoesNotExist()
    {
        // Act
        var result = await _controller.AddFavorite(Guid.NewGuid());

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }
}
