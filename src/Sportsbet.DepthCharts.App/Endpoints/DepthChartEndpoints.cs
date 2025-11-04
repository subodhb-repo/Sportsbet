using Microsoft.AspNetCore.Mvc;
using Sportsbet.DepthCharts.App.Models;
using Sportsbet.DepthCharts.Domain.Model;
using Sportsbet.DepthCharts.Domain.Services;

namespace Sportsbet.DepthCharts.App.Endpoints;

public static class DepthChartEndpoints
{
    public static WebApplication MapDepthChartEndpoints(this WebApplication app)
    {
        var api = app.MapGroup("/api/{sports:alpha}").WithOpenApi();

        // Use case 1: Add player /api/{sports}/player
        api.MapPost(
            "/player",
            (
                [FromRoute] string sports,
                [FromBody] AddPlayerRequest request,
                [FromServices] IServiceProvider serviceProvider
            ) =>
            {
                try
                {
                    var service = GetServiceForSports(sports, serviceProvider);

                    var player = new Player(request.PlayerId, request.Name);

                    service.AddPlayerToDepthChart(player, request.Position, request.PositionDepth);
                    return Results.Created();
                }
                catch (ArgumentException e)
                {
                    return Results.BadRequest(new { errorMessage = e.Message });
                }
            }
        );

        // Use case 2: remove player /api/{sports}/player
        api.MapDelete(
            "/player",
            (
                [FromRoute] string sports,
                [FromBody] RemovePlayerRequest request,
                [FromServices] IServiceProvider serviceProvider
            ) =>
            {
                try
                {
                    var service = GetServiceForSports(sports, serviceProvider);

                    service.RemovePlayerFromDepthChart(request.PlayerId, request.Position);
                    return Results.Ok(
                        new
                        {
                            Message = $"Player Id {request.PlayerId} is removed from Position {request.Position} in sports {sports}",
                        }
                    );
                }
                catch (ArgumentException e)
                {
                    return Results.BadRequest(new { errorMessage = e.Message });
                }
            }
        );

        // Use case 3: get full depth chart /api/{sports}/depth-chart
        api.MapGet(
            "/depth-chart",
            ([FromRoute] string sports, [FromServices] IServiceProvider serviceProvider) =>
            {
                try
                {
                    var service = GetServiceForSports(sports, serviceProvider);
                    var fullDepthChart = service.GetFullDepthChart();
                    return Results.Ok(fullDepthChart);
                }
                catch (ArgumentException e)
                {
                    return Results.BadRequest(new { errorMessage = e.Message });
                }
            }
        );

        // Use case 4: get players under player for a position /api/{sports}/players-under/{playerId}/{position}
        api.MapGet(
            "/players-under/{playerId:int}/{position}",
            (
                [FromRoute] string sports,
                [FromRoute] int playerId,
                [FromRoute] string position,
                [FromServices] IServiceProvider serviceProvider
            ) =>
            {
                try
                {
                    var service = GetServiceForSports(sports, serviceProvider);
                    var playersUnder = service.GetPlayersUnderPlayerInDepthChart(
                        playerId,
                        position
                    );
                    return Results.Ok(playersUnder);
                }
                catch (ArgumentException e)
                {
                    return Results.BadRequest(new { errorMessage = e.Message });
                }
            }
        );

        return app;
    }

    private static IDepthChartService GetServiceForSports(
        string sportsName,
        IServiceProvider serviceProvider
    ) => serviceProvider.GetRequiredKeyedService<IDepthChartService>(sportsName.Trim().ToUpper());
}
