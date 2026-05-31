using Gardener.Database;
using Gardener.entities;
using Microsoft.EntityFrameworkCore;

namespace Gardener.services;

public class FavoriteService(GardenerDbContext db) : IFavoriteService
{
    public async Task<bool> IsFavoriteAsync(int userId, int plantId)
    {
        return await db.Favorites.AnyAsync(f => f.UserId == userId && f.PlantId == plantId);
    }

    public async Task AddFavoriteAsync(int userId, int plantId)
    {
        if (await IsFavoriteAsync(userId, plantId)) return;
        var favorite = new Favorite
        {
            UserId = userId,
            PlantId = plantId,
            User = await db.Users.FindAsync(userId) ?? throw new Exception("User not found"),
            Plant = await db.Plants.FindAsync(plantId) ?? throw new Exception("Plant not found")
        };
        db.Favorites.Add(favorite);

        var rating = await db.PlantRatings.FindAsync(plantId);
        if (rating != null) rating.FavoriteCount++;
    
        await db.SaveChangesAsync();
    }

    public async Task RemoveFavoriteAsync(int userId, int plantId)
    {
        var favorite = await db.Favorites.FirstOrDefaultAsync(f => f.UserId == userId && f.PlantId == plantId);
        if (favorite != null)
        {
            db.Favorites.Remove(favorite);

            var rating = await db.PlantRatings.FindAsync(plantId);
            if (rating != null && rating.FavoriteCount > 0) rating.FavoriteCount--;

            await db.SaveChangesAsync();
        }
    }

    public async Task<List<int>> GetUserFavoritesAsync(int userId)
    {
        return await db.Favorites
            .Where(f => f.UserId == userId)
            .Select(f => f.PlantId)
            .ToListAsync();
    }
}