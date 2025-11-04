using System.ComponentModel.DataAnnotations;

namespace Sportsbet.DepthCharts.App.Models;

/// <summary>
/// Request model for removing a player
/// from a depth chart
/// </summary>
/// <param name="PlayerId">Id of this player</param>
/// <param name="Position">Position depth of this player</param>
public record RemovePlayerRequest([Required] int PlayerId, [Required] string Position);
