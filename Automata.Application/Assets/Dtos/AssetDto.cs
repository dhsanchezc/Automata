namespace Automata.Application.Assets.Dtos;

public class AssetDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<MaintenanceRecordDto> MaintenanceRecords { get; set; } = new();
}