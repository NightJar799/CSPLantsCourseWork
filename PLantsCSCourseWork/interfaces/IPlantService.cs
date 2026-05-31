using Gardener.dto.request;
using Gardener.dto.response;
using Gardener.entities;

namespace Gardener.Services;

public interface IPlantService
{
    Task<List<PlantListResponse>> GetAllPlantsAsync(string? sortBy);
    Task<PlantDetailsResponse?> GetPlantDetailsAsync(int id);
    Task<Plant> AddPlantAsync(CreatePlantRequest request);
    Task<bool> UpdatePlantAsync(int id, UpdatePlantRequest request);
    Task<bool> DeletePlantAsync(int id);
    Task<List<string>> GetTop3PopularPlantNamesAsync();
}