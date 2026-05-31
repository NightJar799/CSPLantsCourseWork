namespace Gardener.entities;

public class Growth
{
    public int Id { get; set; }          
    public Plant Plant { get; set; } = null!;
    public required string Ppfd { get; set; }
    public double Humidity { get; set; }
    public double Ph { get; set; }
    public int Space { get; set; }
    public required string Soil { get; set; }
    public required string Survivability { get; set; }
    public required string GrowthSpeed { get; set; }
    public string? Climate { get; set; }
    public string? Water { get; set; }
    public string? LandScape { get; set; }
}