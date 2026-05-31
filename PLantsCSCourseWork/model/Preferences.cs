namespace Gardener.entities;

public class Preferences
{
    public int Id { get; set; }          
    public User User { get; set; } = null!;
    public string? Climate { get; set; }
    public string? Soil { get; set; }
    public int? Space { get; set; }
    public string? Water { get; set; }
    public string? LandScape { get; set; }
}