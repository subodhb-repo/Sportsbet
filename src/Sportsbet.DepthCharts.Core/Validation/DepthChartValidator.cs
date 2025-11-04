using Sportsbet.DepthCharts.Domain.Configuration;

namespace Sportsbet.DepthCharts.Domain.Validation;

public static class DepthChartValidator
{
    public static void ValidatePosition(string? position, ISportsConfiguration sportsConfig)
    {
        if (!sportsConfig.IsValidPosition(position))
        {
            throw new ArgumentException(
                $"Position {position} is not valid for {sportsConfig.SportsName}"
            );
        }
    }
}
