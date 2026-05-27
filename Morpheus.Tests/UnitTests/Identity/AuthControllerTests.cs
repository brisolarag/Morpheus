using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Morpheus.Identity.Controllers;
using Morpheus.Identity.Data;
using Morpheus.Identity.DTOs;
using Moq;
using Microsoft.AspNetCore.Identity;

namespace Morpheus.Tests.UnitTests.Identity;

public class AuthControllerTests
{
    private readonly Mock<UserManager<ApplicationUser>> _mockUserManager;
    private readonly Mock<IConfiguration> _mockConfiguration;
    private readonly AuthController _controller;

    public AuthControllerTests()
    {
        var store = new Mock<IUserStore<ApplicationUser>>();
        _mockUserManager = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);
        
        _mockConfiguration = new Mock<IConfiguration>();
        _mockConfiguration.Setup(c => c.GetSection("Jwt")["Key"]).Returns("ThisIsAVerySecureKeyForTestingMorpheus");
        _mockConfiguration.Setup(c => c.GetSection("Jwt")["Issuer"]).Returns("TestIssuer");
        _mockConfiguration.Setup(c => c.GetSection("Jwt")["Audience"]).Returns("TestAudience");

        _controller = new AuthController(_mockUserManager.Object, _mockConfiguration.Object);
    }

    [Fact]
    public async Task Register_ReturnsBadRequest_WhenEmailAlreadyExists()
    {
        // Arrange
        var dto = new RegisterRequest { Email = "test@example.com", Password = "Password123!", FirstName = "Test", LastName = "User" };
        var existingUser = new ApplicationUser { FirstName = "Test", LastName = "User" };
        _mockUserManager.Setup(u => u.FindByEmailAsync(dto.Email)).ReturnsAsync(existingUser);

        // Act
        var result = await _controller.Register(dto);

        // Assert
        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Contains("Email already in use", badRequest.Value?.ToString() ?? "");
    }
}
