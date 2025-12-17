using Automata.Domain.Common;

namespace Automata.Domain.Aggregates.Assets;

public class Asset : IAggregateRoot
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    private readonly List<MaintenanceRecord> _maintenanceRecords = new();
    public IReadOnlyCollection<MaintenanceRecord> MaintenanceRecords => _maintenanceRecords.AsReadOnly();


    public Asset()
    {
        CreatedAt = DateTime.UtcNow;
    }

    public void ScheduleMaintenance(MaintenanceRecord record)
    {
        if (record.Type == MaintenanceType.Preventive &&
            _maintenanceRecords.Any(r => r.Type == MaintenanceType.Preventive && r.IsPending))
        {
            throw new InvalidOperationException("Cannot schedule more than one pending preventive maintenance.");
        }

        _maintenanceRecords.Add(record);
    }

    public void CompleteMaintenance(int recordId, DateTime completedAt)
    {
        var record = _maintenanceRecords.FirstOrDefault(r => r.Id == recordId)
                     ?? throw new InvalidOperationException("Maintenance record not found.");

        record.Complete(completedAt);
    }
}