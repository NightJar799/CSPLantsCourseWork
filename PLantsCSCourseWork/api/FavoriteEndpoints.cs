using Gardener.dto.response;
using Gardener.services;
using System.Security.Claims;

namespace Gardener.Endpoints;

public static class FavoriteEndpoints
{
    public static RouteGroupBuilder MapFavoriteEndpoints(this RouteGroupBuilder api)
    {
        var group = api.MapGroup("/favorites").WithTags("Favorites").RequireAuthorization();

        group.MapGet("/", async (ClaimsPrincipal user, IFavoriteService service) =>
        {
            var userId = int.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var favorites = await service.GetUserFavoritesAsync(userId);
            return Results.Ok(favorites);
        });

        group.MapPost("/{plantId:int}", async (int plantId, ClaimsPrincipal user, IFavoriteService service) =>
        {
            var userId = int.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier)!);
            await service.AddFavoriteAsync(userId, plantId);
            return Results.Ok(new { message = "Added to favorites" });
        });

        group.MapDelete("/{plantId:int}", async (int plantId, ClaimsPrincipal user, IFavoriteService service) =>
        {
            var userId = int.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier)!);
            await service.RemoveFavoriteAsync(userId, plantId);
            return Results.Ok(new { message = "Removed from favorites" });
        });

        group.MapGet("/check/{plantId:int}", async (int plantId, ClaimsPrincipal user, IFavoriteService service) =>
        {
            var userId = int.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var isFavorite = await service.IsFavoriteAsync(userId, plantId);
            return Results.Ok(new { isFavorite });
        });

        return api;
    }
}