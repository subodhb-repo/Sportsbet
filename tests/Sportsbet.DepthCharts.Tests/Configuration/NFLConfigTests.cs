using Sportsbet.DepthCharts.Domain.Configuration;

namespace Sportsbet.DepthCharts.Tests.Configuration;

public class NFLConfigTests
{
    [Fact]
    public void IsValidPosition_ShouldReturnTrue_WhenPositionIsValid()
    {
        // Arrange
        var validPositions = new[] { "QB", "WR", "RB", "TE", "K", "P", "KR", "PR" };

        foreach (var position in validPositions)
        {
            // Act
            var result = new NFLConfiguration().IsValidPosition(position);

            // Assert
            Assert.True(result);
        }
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("invalid")]
    public void IsValidPosition_ShouldReturnFalse_WhenPositionIsInValid(string invalidPosition)
    {
        // Act
        var result = new NFLConfiguration().IsValidPosition(invalidPosition);

        // Assert
        Assert.False(result);
    }
}
