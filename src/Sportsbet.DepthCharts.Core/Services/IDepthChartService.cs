using Sportsbet.DepthCharts.Domain.Model;

namespace Sportsbet.DepthCharts.Domain.Services;

/// <summary>
/// Defines the contract for the service to
/// manage a depth chart.
/// </summary>
public interface IDepthChartService
{
    /// <summary>
    /// Adds a player to a specified position in depth chart
    /// </summary>
    List<Player> AddPlayerToDepthChart(Player player, string position, int? positionDepth = null);

    /// <summary>
    /// Removed a player from the depth chart
    /// </summary>
    List<Player> RemovePlayerFromDepthChart(int playerId, string position);

    /// <summary>
    /// Gets full depth chart with position and player id
    /// </summary>
    Dictionary<string, List<int>> GetFullDepthChart();

    /// <summary>
    /// Finds all player ids ranked below a given player
    /// </summary>
    List<int> GetPlayersUnderPlayerInDepthChart(int playerId, string position);
}
