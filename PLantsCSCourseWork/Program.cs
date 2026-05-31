using Gardener.Database;
using Gardener.Endpoints;
using Gardener.services;
using Gardener.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new()
    {
        Title = "Gardener API",
        Version = "v1",
        Description = "API для каталога растений, избранного и рекомендаций"
    });
});

var connectionString = builder.Configuration.GetConnectionString("Postgres")
    ?? throw new InvalidOperationException("Строка подключения не найдена.");
builder.Services.AddDbContext<GardenerDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/api/auth/login";
        options.LogoutPath = "/api/auth/logout";
        options.Events.OnRedirectToLogin = context =>
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return Task.CompletedTask;
        };
    });
builder.Services.AddAuthorization();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IPlantService, PlantService>();
builder.Services.AddScoped<IFavoriteService, FavoriteService>();
builder.Services.AddScoped<IUserService, UserService>();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<GardenerDbContext>();
    await db.Database.EnsureCreatedAsync();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

var api = app.MapGroup("/api");
api.MapAuthEndpoints();
api.MapPlantEndpoints();
api.MapFavoriteEndpoints();
api.MapUserEndpoints();

app.MapGet("/", () => Results.Ok(new
{
    message = "Gardener API v1",
    description = "API для управления растениями, избранным, садом пользователя",
    resources = new
    {
        auth = new { register = "/api/auth/register", login = "/api/auth/login", logout = "/api/auth/logout" },
        plants = new { list = "/api/plants", top3 = "/api/plants/top3", detail = "/api/plants/{id}" },
        user = new { profile = "/api/user/profile", preferences = "/api/user/preferences", recommendation = "/api/user/recommendation" },
        favorites = new { list = "/api/favorites", add = "/api/favorites/{plantId}", remove = "/api/favorites/{plantId}", check = "/api/favorites/check/{plantId}" },
        garden = new { myGarden = "/api/garden", plant = "/api/garden/plant/{plantId}" }
    }
}));

await app.RunAsync();