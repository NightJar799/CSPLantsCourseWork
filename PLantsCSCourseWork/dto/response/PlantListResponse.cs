namespace Gardener.dto.response;

public record PlantListResponse(int PlantId, string Name, string ScientificName,
    string? Climate, int Space, string Soil, string? Water, string? LandScape);