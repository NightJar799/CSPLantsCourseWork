using Gardener.dto.request;
using Gardener.dto.response;
using Gardener.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Gardener.Endpoints;

public static class AuthEndpoints
{
    public static RouteGroupBuilder MapAuthEndpoints(this RouteGroupBuilder api)
    {
        var group = api.MapGroup("/auth").WithTags("Auth");

        group.MapPost("/register", async (RegisterRequest request, IAuthService auth, HttpContext ctx) =>
        {
            var user = await auth.RegisterAsync(request);
            if (user == null)
                return Results.BadRequest(new ErrorResponse("Email or phone already exists"));
            await ctx.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, user.ToClaimsPrincipal());
            return Results.Ok(new { message = "Registered successfully", user.Id });
        });

        group.MapPost("/login", async (LoginRequest request, IAuthService auth, HttpContext ctx) =>
        {
            var user = await auth.LoginAsync(request);
            if (user == null)
                return Results.UnprocessableEntity(new ErrorResponse("Invalid credentials"));
            await ctx.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, user.ToClaimsPrincipal());
            return Results.Ok(new { message = "Logged in", user.Id });
        });

        group.MapPost("/logout", async (HttpContext ctx) =>
        {
            await ctx.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Results.Ok(new { message = "Logged out" });
        }).RequireAuthorization();

        return api;
    }
}