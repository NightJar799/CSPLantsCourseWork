using Gardener.Database;
using Gardener.dto.request;
using Gardener.dto.response;
using Microsoft.EntityFrameworkCore;

namespace Gardener.Services;

public class UserService(GardenerDbContext db) : IUserService
{
    public async Task<UserResponse?> GetUserByIdAsync(int id)
    {
        var user = await db.Users.FindAsync(id);
        if (user == null) return null;
        return new UserResponse(user.Id, user.Login, user.NickName, user.Role.ToString());
    }

    public async Task<bool> UpdatePreferencesAsync(int userId, UpdatePreferencesRequest request)
    {
        var prefs = await db.Preferences.FindAsync(userId);
        if (prefs == null) return false;

        if (request.Climate != null) prefs.Climate = request.Climate;
        if (request.Soil != null) prefs.Soil = request.Soil;
        if (request.Space.HasValue) prefs.Space = request.Space;
        if (request.Water != null) prefs.Water = request.Water;
        if (request.LandScape != null) prefs.LandScape = request.LandScape;

        await db.SaveChangesAsync();
        return true;
    }

    public async Task<int?> GetRecommendedPlantIdAsync(int userId)
    {
        var prefs = await db.Preferences.FindAsync(userId);
        if (prefs == null) return null;

        var allGrowths = await db.Growths.ToListAsync();
        int? bestPlantId = null;
        int maxMatches = -1;

        foreach (var g in allGrowths)
        {
            int matches = 0;
            if (prefs.Climate != null && g.Climate == prefs.Climate) matches++;
            if (prefs.Soil != null && g.Soil == prefs.Soil) matches++;
            if (prefs.Space.HasValue && g.Space == prefs.Space) matches++;
            if (prefs.Water != null && g.Water == prefs.Water) matches++;
            if (prefs.LandScape != null && g.LandScape == prefs.LandScape) matches++;

            if (matches > maxMatches)
            {
                maxMatches = matches;
                bestPlantId = g.Id;
            }
        }
        return bestPlantId;
    }
}