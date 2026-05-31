namespace Gardener.entities;

public class Favorite
{
    public int UserId { get; set; }
    public int PlantId { get; set; }
    public User User { get; set; } = null!;
    public Plant Plant { get; set; } = null!;
}