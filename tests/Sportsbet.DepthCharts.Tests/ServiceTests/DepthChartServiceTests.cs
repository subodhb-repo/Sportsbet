using Sportsbet.DepthCharts.Domain.Configuration;
using Sportsbet.DepthCharts.Domain.Model;
using Sportsbet.DepthCharts.Domain.Services;

namespace Sportsbet.DepthCharts.Tests.ServiceTests;

public class DepthChartServiceTests
{
    [Fact]
    public void GetFullDepthChart_ShouldReturnFullDepthChart()
    {
        // Arrange
        var service = new DepthChartService(new NFLConfiguration());

        var bob = new Player(PlayerId: 1, Name: "Bob");
        var alice = new Player(PlayerId: 2, Name: "Alice");
        var charlie = new Player(PlayerId: 3, Name: "Charlie");
        var position1 = "WR";
        var position2 = "KR";

        service.AddPlayerToDepthChart(bob, position1, 0);
        service.AddPlayerToDepthChart(alice, position1, 0);
        service.AddPlayerToDepthChart(charlie, position1, 2);

        service.AddPlayerToDepthChart(bob, position2);

        // Act
        var depthChart = service.GetFullDepthChart();

        // Assert
        Assert.Equal(2, depthChart.Count);

        Assert.Equal(3, depthChart[position1].Count);
        Assert.Equal(
            new[] { alice.PlayerId, bob.PlayerId, charlie.PlayerId },
            depthChart[position1]
        );

        Assert.Single(depthChart[position2]);
        Assert.Equal(new[] { bob.PlayerId }, depthChart[position2]);
    }

    [Fact]
    public void AddPlayerToDepthChart_ShouldAddToEnd_WhenPositionDepthIsNotProvided()
    {
        // Arrange
        var nflConfig = new NFLConfiguration();
        var service = new DepthChartService(nflConfig);

        var bob = new Player(PlayerId: 1, Name: "Bob");
        var alice = new Player(PlayerId: 2, Name: "Alice");
        var position = "WR";

        // Act
        service.AddPlayerToDepthChart(bob, position);

        var playersAtWRPosition = service.AddPlayerToDepthChart(alice, position);

        // Assert
        Assert.Equal(2, playersAtWRPosition.Count);
        Assert.Equal(bob.PlayerId, playersAtWRPosition[0].PlayerId);
        Assert.Equal(alice.PlayerId, playersAtWRPosition[1].PlayerId);
    }

    [Fact]
    public void AddPlayerToDepthChart_ShouldAddAndBumpDown_WhenSameSlotIsProvided()
    {
        // Arrange
        var service = new DepthChartService(new NFLConfiguration());

        var bob = new Player(PlayerId: 1, Name: "Bob");
        var alice = new Player(PlayerId: 2, Name: "Alice");
        var charlie = new Player(PlayerId: 3, Name: "Charlie");
        var position = "WR";

        // Act
        service.AddPlayerToDepthChart(bob, position, 0);
        service.AddPlayerToDepthChart(charlie, position, 1);
        var playersAtWRPosition = service.AddPlayerToDepthChart(alice, position, 0); // adding alice at same spot as bob

        // Assert
        Assert.Equal(3, playersAtWRPosition.Count);
        Assert.Equal(alice.PlayerId, playersAtWRPosition[0].PlayerId);
        Assert.Equal(bob.PlayerId, playersAtWRPosition[1].PlayerId);
        Assert.Equal(charlie.PlayerId, playersAtWRPosition[2].PlayerId);
    }

    [Fact]
    public void AddPlayerToDepthChart_ShouldRearrangePlayers()
    {
        // Arrange
        var service = new DepthChartService(new NFLConfiguration());

        var bob = new Player(PlayerId: 1, Name: "Bob");
        var alice = new Player(PlayerId: 2, Name: "Alice");
        var position = "WR";

        // Act
        service.AddPlayerToDepthChart(bob, position, 0); // [bob]
        service.AddPlayerToDepthChart(alice, position, 1); // [bob, alice]
        var playersAtWRPosition = service.AddPlayerToDepthChart(bob, position, 2); // [alice, bob]

        // Assert
        Assert.Equal(2, playersAtWRPosition.Count);
        Assert.Equal(alice.PlayerId, playersAtWRPosition[0].PlayerId);
        Assert.Equal(bob.PlayerId, playersAtWRPosition[1].PlayerId);
    }

    [Fact]
    public void AddPlayerToDepthChart_ShouldAddAtEnd_WhenDepthIsTooHigh()
    {
        // Arrange
        var service = new DepthChartService(new NFLConfiguration());

        var bob = new Player(PlayerId: 1, Name: "Bob");
        var alice = new Player(PlayerId: 2, Name: "Alice");
        var position = "WR";

        // Act
        service.AddPlayerToDepthChart(bob, position, 0);

        // position 5 is requested but squad is not that deep, so add to end
        var playersAtWRPosition = service.AddPlayerToDepthChart(alice, position, 5);

        // Assert
        Assert.Equal(2, playersAtWRPosition.Count);
        Assert.Equal(bob.PlayerId, playersAtWRPosition[0].PlayerId);
        Assert.Equal(alice.PlayerId, playersAtWRPosition[1].PlayerId);
    }

    [Fact]
    public void AddPlayerToDepthChart_ShouldAddSamePLayer_WhenDifferentPositionIsSelected()
    {
        // Arrange
        var service = new DepthChartService(new NFLConfiguration());

        var bob = new Player(PlayerId: 1, Name: "Bob");
        var alice = new Player(PlayerId: 2, Name: "Alice");
        var position1 = "QB";
        var position2 = "WR";

        // Act
        service.AddPlayerToDepthChart(bob, position1, 0);
        var playersAtPosition1 = service.AddPlayerToDepthChart(alice, position1, 1);
        var playersAtPosition2 = service.AddPlayerToDepthChart(bob, position2, 0);

        // Assert
        Assert.Equal(2, playersAtPosition1.Count);
        Assert.Equal(bob.PlayerId, playersAtPosition1[0].PlayerId);
        Assert.Equal(alice.PlayerId, playersAtPosition1[1].PlayerId);

        Assert.Single(playersAtPosition2);
        Assert.Equal(bob.PlayerId, playersAtPosition2[0].PlayerId);
    }

    [Theory]
    [InlineData("invalid")]
    [InlineData("")]
    [InlineData(null)]
    public void AddPlayerToDepthChart_ShouldThrow_WhenPositionIsNotSupportedForASports(
        string invalidPosition
    )
    {
        // Arrange
        var service = new DepthChartService(new NFLConfiguration());

        var bob = new Player(PlayerId: 1, Name: "Bob");

        // Act & Assert
        Assert.Throws<ArgumentException>(() => service.AddPlayerToDepthChart(bob, invalidPosition));
    }

    [Fact]
    public void RemovePlayerFromDepthChart_ShouldRemovePlayerAndBumpUp()
    {
        // Arrange
        var service = new DepthChartService(new NFLConfiguration());

        var bob = new Player(PlayerId: 1, Name: "Bob");
        var alice = new Player(PlayerId: 2, Name: "Alice");
        var charlie = new Player(PlayerId: 3, Name: "Charlie");
        var position = "WR";

        service.AddPlayerToDepthChart(bob, position, 0);
        service.AddPlayerToDepthChart(alice, position, 1);
        service.AddPlayerToDepthChart(charlie, position, 2);

        // Act
        var playersAtWRPosition = service.RemovePlayerFromDepthChart(alice.PlayerId, position);

        // Assert
        Assert.Equal(2, playersAtWRPosition.Count);
        Assert.Equal(bob.PlayerId, playersAtWRPosition[0].PlayerId);
        Assert.Equal(charlie.PlayerId, playersAtWRPosition[1].PlayerId);
    }

    [Fact]
    public void RemovePlayerFromDepthChart_ShouldHandlePlayerNotFound()
    {
        // Arrange
        var service = new DepthChartService(new NFLConfiguration());

        var bob = new Player(PlayerId: 1, Name: "Bob");
        var alice = new Player(PlayerId: 2, Name: "Alice");
        var charlie = new Player(PlayerId: 3, Name: "Charlie");
        var position = "WR";

        service.AddPlayerToDepthChart(bob, position, 0);
        service.AddPlayerToDepthChart(alice, position, 1);

        // Act
        var playersAtWRPosition = service.RemovePlayerFromDepthChart(charlie.PlayerId, position); // charlier was never added in the chart

        // Assert - Depth chart does not change
        Assert.Equal(2, playersAtWRPosition.Count);
        Assert.Equal(bob.PlayerId, playersAtWRPosition[0].PlayerId);
        Assert.Equal(alice.PlayerId, playersAtWRPosition[1].PlayerId);
    }

    [Fact]
    public void GetPlayersUnderPlayerInDepthChart_ShouldHandlePlayerNotFound()
    {
        // Arrange
        var service = new DepthChartService(new NFLConfiguration());

        var bob = new Player(PlayerId: 1, Name: "Bob");
        var alice = new Player(PlayerId: 2, Name: "Alice");
        var position = "WR";

        service.AddPlayerToDepthChart(bob, position, 0);

        // Act
        var playerAfterAlice = service.GetPlayersUnderPlayerInDepthChart(alice.PlayerId, position);

        // Assert
        Assert.Empty(playerAfterAlice);
    }

    [Fact]
    public void GetPlayersUnderPlayerInDepthChart_WhenPlayerIsLastInTheDepthChart()
    {
        // Arrange
        var service = new DepthChartService(new NFLConfiguration());

        var bob = new Player(PlayerId: 1, Name: "Bob");
        var alice = new Player(PlayerId: 2, Name: "Alice");
        var position = "WR";

        service.AddPlayerToDepthChart(bob, position, 0);
        service.AddPlayerToDepthChart(alice, position, 1);

        // Act
        var playerAfterAlice = service.GetPlayersUnderPlayerInDepthChart(alice.PlayerId, position);

        // Assert
        Assert.Empty(playerAfterAlice);
    }

    [Fact]
    public void GetPlayersUnderPlayerInDepthChart_ReturnsAllPlayersUnderPLayer()
    {
        // Arrange
        var service = new DepthChartService(new NFLConfiguration());

        var bob = new Player(PlayerId: 1, Name: "Bob");
        var alice = new Player(PlayerId: 2, Name: "Alice");
        var charlie = new Player(PlayerId: 3, Name: "Charlie");
        var position = "WR";

        service.AddPlayerToDepthChart(bob, position, 0);
        service.AddPlayerToDepthChart(alice, position, 1);
        service.AddPlayerToDepthChart(charlie, position, 2);

        // Act
        var playerAfterBob = service.GetPlayersUnderPlayerInDepthChart(bob.PlayerId, position);

        // Assert
        Assert.Equal(2, playerAfterBob.Count);
        Assert.Equal(alice.PlayerId, playerAfterBob[0]);
        Assert.Equal(charlie.PlayerId, playerAfterBob[1]);
    }
}
