namespace Automata.Domain.Aggregates.Assets;

public class MaintenanceRecord
{
    public int Id { get; private set; }
    public DateTime ScheduledDate { get; private set; }
    public DateTime? CompletedDate { get; private set; }
    public string? Technician { get; private set; }
    public string? Notes { get; private set; }
    public MaintenanceType Type { get; private set; }
    public MaintenanceStatus Status { get; private set; }

    public MaintenanceRecord(DateTime scheduledDate, MaintenanceType type, string? technician, string? notes)
    {
        ScheduledDate = scheduledDate;
        Type = type;
        Technician = technician;
        Notes = notes;
        Status = MaintenanceStatus.Pending;
    }

    public void Complete(DateTime completedDate)
    {
        CompletedDate = completedDate; // change to DateTime.UtcNow ?
        Status = MaintenanceStatus.Completed;
    }

    public bool IsPending => Status == MaintenanceStatus.Pending;
}

public enum MaintenanceType
{
    Preventive,
    Corrective,
    Predictive
}

public enum MaintenanceStatus
{
    Pending,
    Completed
}
