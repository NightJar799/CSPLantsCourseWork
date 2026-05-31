using Gardener.dto.request;
using Gardener.dto.response;
using Gardener.Services;
using System.Security.Claims;

namespace Gardener.Endpoints;

public static class UserEndpoints
{
    public static RouteGroupBuilder MapUserEndpoints(this RouteGroupBuilder api)
    {
        var group = api.MapGroup("/user").WithTags("User").RequireAuthorization();

        group.MapGet("/profile", async (ClaimsPrincipal user, IUserService service) =>
        {
            var userId = int.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var profile = await service.GetUserByIdAsync(userId);
            return profile is null ? Results.NotFound() : Results.Ok(profile);
        });

        group.MapGet("/recommendation", async (ClaimsPrincipal user, IUserService service, IPlantService plantService) =>
        {
            var userId = int.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var plantId = await service.GetRecommendedPlantIdAsync(userId);
            if (plantId == null) return Results.Ok(new { message = "No recommendation" });
            var plant = await plantService.GetPlantDetailsAsync(plantId.Value);
            return Results.Ok(plant);
        });

        group.MapPut("/preferences", async (UpdatePreferencesRequest request, ClaimsPrincipal user, IUserService service) =>
        {
            var userId = int.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var ok = await service.UpdatePreferencesAsync(userId, request);
            return ok ? Results.Ok() : Results.NotFound(new ErrorResponse("User preferences not found"));
        });

        return api;
    }
}