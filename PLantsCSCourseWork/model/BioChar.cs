namespace Gardener.entities;

public class BioChar
{
    public int Id { get; set; }          
    public Plant Plant { get; set; } = null!;
    public required string LeafType { get; set; }
    public required string Root { get; set; }
    public required string Fruit { get; set; }
    public char? AmmFruit { get; set; }
    public required string Morphology { get; set; }
}