namespace Gardener.entities;

public class PlantRating
{
    public int Id { get; set; }           
    public int ViewCount { get; set; }    
    public int FavoriteCount { get; set; } 

    public Plant Plant { get; set; } = null!;
}