namespace Automata.Application.Assets.Dtos;

public class MaintenanceRecordDto
{
    public DateTime ScheduledDate { get; set; }
    public string? Technician { get; set; }
    public string? Notes { get; set; }
    public string Type { get; set; } = null!;
}
