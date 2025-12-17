using MediatR;
using Automata.Application.Assets.Commands;
using Automata.Application.Assets.Queries;
using Automata.Application.Assets.Dtos;

namespace Automata.Api.ApiHandlers;

public static class AssetApi
{
    // TODO: return RouteGroupBuilder for Fluent API usage instead of void
    public static void MapAssets(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/", async (IMediator mediator) =>
            Results.Ok(await mediator.Send(new GetAllAssetsQuery())));

        endpoints.MapGet("/{id:int}", async (IMediator mediator, int id) =>
        {
            var asset = await mediator.Send(new GetAssetByIdQuery(id));
            return asset is not null ? Results.Ok(asset) : Results.NotFound();
        });

        endpoints.MapPost("/", async (IMediator mediator, CreateAssetCommand command) =>
        {
            var assetId = await mediator.Send(command);
            return Results.Created($"/api/assets/{assetId}", new { Id = assetId });
        });

        endpoints.MapPut("/{id:int}", async (IMediator mediator, int id, UpdateAssetCommand command) =>
        {
            var success = await mediator.Send(command with { Id = id });
            return success ? Results.NoContent() : Results.NotFound();
        });

        endpoints.MapDelete("/{id:int}", async (IMediator mediator, int id) =>
        {
            var success = await mediator.Send(new DeleteAssetCommand(id));
            return success ? Results.NoContent() : Results.NotFound();
        });

        endpoints.MapPost("/{id:int}/maintenance", async (
            IMediator mediator,
            int id,
            MaintenanceRecordDto dto) =>
            {
                var command = new AddMaintenanceRecordCommand(id, dto);
                var success = await mediator.Send(command);
                return success ? Results.NoContent() : Results.NotFound();
            });
    }
}