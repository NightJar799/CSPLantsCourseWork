namespace Gardener.services;

public interface IFavoriteService
{
    Task<bool> IsFavoriteAsync(int userId, int plantId);
    Task AddFavoriteAsync(int userId, int plantId);
    Task RemoveFavoriteAsync(int userId, int plantId);
    Task<List<int>> GetUserFavoritesAsync(int userId);
}