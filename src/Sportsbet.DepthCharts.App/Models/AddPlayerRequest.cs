using System.ComponentModel.DataAnnotations;

namespace Sportsbet.DepthCharts.App.Models;

/// <summary>
/// Request model for adding a player
/// to a depth chart
/// </summary>
/// <param name="PlayerId">Id of this player</param>
/// <param name="Name">Name of this player</param>
/// <param name="Position">Position to which this player should be added</param>
/// <param name="PositionDepth">Position depth of this player</param>
public record AddPlayerRequest(
    [Required] int PlayerId,
    string Name,
    [Required] string Position,
    int? PositionDepth
);
