using Gardener.dto.request;
using Gardener.dto.response;
using Gardener.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Gardener.Endpoints;

public static class PlantEndpoints
{
    public static RouteGroupBuilder MapPlantEndpoints(this RouteGroupBuilder api)
    {
        var group = api.MapGroup("/plants").WithTags("Plants").RequireAuthorization();

        group.MapGet("/", async (string? sortBy, IPlantService service) =>
        {
            var plants = await service.GetAllPlantsAsync(sortBy);
            return Results.Ok(plants);
        });

        group.MapGet("/top3", async (IPlantService service) =>
        {
            var top3 = await service.GetTop3PopularPlantNamesAsync();
            return Results.Ok(top3);
        });

        group.MapGet("/{id:int}", async (int id, IPlantService service) =>
        {
            var plant = await service.GetPlantDetailsAsync(id);
            return plant is null
                ? Results.NotFound(new ErrorResponse("Plant not found"))
                : Results.Ok(plant);
        });

        group.MapPost("/", async (CreatePlantRequest request, IPlantService service, ClaimsPrincipal user) =>
        {
            if (!user.IsInRole("ROLE_ADMIN"))
                return Results.Forbid();
            var plant = await service.AddPlantAsync(request);
            return Results.Created($"/api/plants/{plant.Id}", plant);
        }).RequireAuthorization();

        group.MapPut("/{id:int}", async (int id, UpdatePlantRequest request, IPlantService service, ClaimsPrincipal user) =>
        {
            if (!user.IsInRole("ROLE_ADMIN"))
                return Results.Forbid();
            var ok = await service.UpdatePlantAsync(id, request);
            return ok ? Results.Ok() : Results.NotFound(new ErrorResponse("Plant not found"));
        }).RequireAuthorization();

        group.MapDelete("/{id:int}", async (int id, IPlantService service, ClaimsPrincipal user) =>
        {
            if (!user.IsInRole("ROLE_ADMIN"))
                return Results.Forbid();
            var ok = await service.DeletePlantAsync(id);
            return ok ? Results.NoContent() : Results.NotFound(new ErrorResponse("Plant not found"));
        }).RequireAuthorization();

        return api;
    }
}