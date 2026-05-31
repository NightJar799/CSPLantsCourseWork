namespace Gardener.dto.response;

public record PlantDetailsResponse(
    int PlantId, string Name, string ScienceName, string Description,
    string LeafType, string Root, string Fruit, string? AmmFruit, string Morphology,
    string Ppfd, double Humidity, double Ph, int Space, string Soil,
    string Survivability, string GrowthSpeed, string? Climate, string? Water, string? LandScape);