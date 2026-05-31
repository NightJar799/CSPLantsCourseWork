namespace Gardener.entities;

public class Plant
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public required string ScienceName { get; set; }
    public required string Description { get; set; }
    public string? Photo { get; set; }
}