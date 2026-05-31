namespace Gardener.dto.request;

public class CreatePlantRequest
{
    public string Name { get; set; } = string.Empty;
    public string ScienceName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string LeafType { get; set; } = string.Empty;
    public string Root { get; set; } = string.Empty;
    public string Fruit { get; set; } = string.Empty;
    public string? AmmFruit { get; set; }
    public string Morphology { get; set; } = string.Empty;
    public string Ppfd { get; set; } = string.Empty;
    public double Humidity { get; set; }
    public double Ph { get; set; }
    public int Space { get; set; }
    public string Soil { get; set; } = string.Empty;
    public string Survivability { get; set; } = string.Empty;
    public string GrowthSpeed { get; set; } = string.Empty;
    public string? Climate { get; set; }
    public string? Water { get; set; }
    public string? LandScape { get; set; }
}