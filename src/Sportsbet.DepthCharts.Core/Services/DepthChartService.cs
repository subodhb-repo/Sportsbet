using Sportsbet.DepthCharts.Domain.Configuration;
using Sportsbet.DepthCharts.Domain.Model;
using Sportsbet.DepthCharts.Domain.Validation;

namespace Sportsbet.DepthCharts.Domain.Services;

public class DepthChartService(ISportsConfiguration sportsConfig) : IDepthChartService
{
    private readonly ISportsConfiguration _sportsConfig =
        sportsConfig ?? throw new ArgumentNullException(nameof(sportsConfig));

    // Depth chart state: Dictionary<Position, List<Player>>
    private readonly Dictionary<string, List<Player>> _depthChart = [];

    public List<Player> AddPlayerToDepthChart(
        Player player,
        string position,
        int? positionDepth = null
    )
    {
        var playerList = GetPlayersByPosition(position);

        // Try remove as same player can't have multiple entry in same position
        _ = playerList.RemoveAll(p => p.PlayerId == player.PlayerId);

        var depth = positionDepth ?? playerList.Count;
        var insertAt = Math.Min(depth, playerList.Count);
        playerList.Insert(index: insertAt, player);
        return playerList;
    }

    public List<Player> RemovePlayerFromDepthChart(int playerId, string position)
    {
        var playerList = GetPlayersByPosition(position);

        var removedCount = playerList.RemoveAll(p => p.PlayerId == playerId);
        if (removedCount == 0) // Player not found in depth chart
        {
            // handle: players to be removed not found eg log warning
        }

        if (removedCount > 1)
        {
            // Best to log as this could mean bug in AddPlayerToDepthChart usecase
            // as it should remove then add to mantain uniqueness
        }

        return playerList;
    }

    public Dictionary<string, List<int>> GetFullDepthChart() =>
        _depthChart
            .Where(d => d.Value.Count > 0)
            .ToDictionary(
                d => d.Key, //position
                d => d.Value.Select(v => v.PlayerId).ToList() // list of player id
            );

    public List<int> GetPlayersUnderPlayerInDepthChart(int playerId, string position)
    {
        var playerList = GetPlayersByPosition(position);
        var playerIndex = playerList.FindIndex(p => p.PlayerId == playerId);
        if (playerIndex == -1) // Player not found in depth chart
        {
            return [];
        }

        var startIndex = playerIndex + 1;
        var totalPlayers = playerList.Count - startIndex;
        if (totalPlayers <= 0) // No player to return
        {
            return [];
        }

        return [.. playerList.GetRange(startIndex, totalPlayers).Select(p => p.PlayerId)];
    }

    private List<Player> GetPlayersByPosition(string position)
    {
        var normalisedPosition = position?.Trim()?.ToUpper();
        DepthChartValidator.ValidatePosition(normalisedPosition, _sportsConfig);

        if (!_depthChart.TryGetValue(normalisedPosition!, out var value))
        {
            value = [];
            _depthChart[normalisedPosition!] = value;
        }

        return value;
    }
}
