namespace Gardener.dto.request;

public class UpdatePreferencesRequest
{
    public string? Climate { get; set; }
    public string? Soil { get; set; }
    public int? Space { get; set; }
    public string? Water { get; set; }
    public string? LandScape { get; set; }
}