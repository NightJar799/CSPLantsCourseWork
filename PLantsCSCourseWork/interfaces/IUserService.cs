using Gardener.dto.request;
using Gardener.dto.response;

namespace Gardener.Services;

public interface IUserService
{
    Task<UserResponse?> GetUserByIdAsync(int id);
    Task<bool> UpdatePreferencesAsync(int userId, UpdatePreferencesRequest request);
    Task<int?> GetRecommendedPlantIdAsync(int userId);
}