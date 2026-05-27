using Morpheus.Identity.Data;

namespace Morpheus.Tests.UnitTests;

public class ApplicationUserTests
{
    [Fact]
    public void ApplicationUser_ShouldSetCreatedAtToUtcNow()
    {
        // Arrange & Act
        var user = new ApplicationUser
        {
            FirstName = "Test",
            LastName = "User",
            Email = "test@example.com",
            UserName = "test@example.com"
        };

        // Assert
        var diff = DateTime.UtcNow - user.CreatedAt;
        Assert.True(diff.TotalSeconds < 5);
    }
}
