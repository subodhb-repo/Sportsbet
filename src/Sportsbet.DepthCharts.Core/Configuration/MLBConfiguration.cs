namespace Sportsbet.DepthCharts.Domain.Configuration;

public class MLBConfiguration : ISportsConfiguration
{
    public string SportsName => Sports.MLB;

    public IReadOnlyList<string> SupportedPositions =>
        ["SP", "RP", "C", "1B", "2B", "3B", "SS", "LF", "RF", "CF", "DH"];

    public bool IsValidPosition(string? position) =>
        !string.IsNullOrWhiteSpace(position) && SupportedPositions.Contains(position);
}
