using Sportsbet.DepthCharts.Domain.Configuration;

namespace Sportsbet.DepthCharts.Tests.Configuration;

public class MLBConfigTests
{
    [Fact]
    public void IsValidPosition_ShouldReturnTrue_WhenPositionIsValid()
    {
        // Arrange
        var validPositions = new[]
        {
            "SP",
            "RP",
            "C",
            "1B",
            "2B",
            "3B",
            "SS",
            "LF",
            "RF",
            "CF",
            "DH",
        };

        foreach (var position in validPositions)
        {
            // Act
            var result = new MLBConfiguration().IsValidPosition(position);

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
        var result = new MLBConfiguration().IsValidPosition(invalidPosition);

        // Assert
        Assert.False(result);
    }
}
