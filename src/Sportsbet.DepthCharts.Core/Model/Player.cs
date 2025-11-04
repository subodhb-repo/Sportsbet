namespace Sportsbet.DepthCharts.Domain.Model;

/// <summary>
/// Represents a player
/// </summary>
/// <param name="PlayerId">Id of a player</param>
/// <param name="Name">Name of the player</param>
public record Player(int PlayerId, string Name);
