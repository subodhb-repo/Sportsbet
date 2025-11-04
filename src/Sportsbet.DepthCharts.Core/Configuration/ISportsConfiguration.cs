namespace Sportsbet.DepthCharts.Domain.Configuration;

/// <summary>
/// Defines the contract for sport specific
/// congiguration, currently used only for defining
/// valid positions for a sports.
/// </summary>
public interface ISportsConfiguration
{
    /// <summary>
    /// Gets name of the sports
    /// </summary>
    public string SportsName { get; }

    /// <summary>
    /// Gets a list of supported positions.
    /// </summary>
    public IReadOnlyList<string> SupportedPositions { get; }

    /// <summary>
    /// Check if a given position is valid
    /// </summary>
    public bool IsValidPosition(string? position);
}
