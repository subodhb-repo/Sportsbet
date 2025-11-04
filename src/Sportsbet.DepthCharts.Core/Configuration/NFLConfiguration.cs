namespace Sportsbet.DepthCharts.Domain.Configuration;

public class NFLConfiguration : ISportsConfiguration
{
    public string SportsName => Sports.NFL;

    public IReadOnlyList<string> SupportedPositions =>
        ["QB", "WR", "RB", "TE", "K", "P", "KR", "PR"];

    public bool IsValidPosition(string? position) =>
        !string.IsNullOrWhiteSpace(position) && SupportedPositions.Contains(position);
}
