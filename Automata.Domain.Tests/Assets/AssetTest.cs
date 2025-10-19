using Automata.Domain.Aggregates.Assets;

namespace Automata.Domain.Tests.Assets;

public class AssetTest
{
    [Fact]
    public void ScheduleMaintenance_SecondPendingPreventive_ThrowsInvalidOperationException()
    {
        // Arrange
        var asset = new Asset();
        var first = new MaintenanceRecord(DateTime.UtcNow, MaintenanceType.Preventive, null, null);
        asset.ScheduleMaintenance(first);
        var second = new MaintenanceRecord(DateTime.UtcNow.AddDays(1), MaintenanceType.Preventive, null, null);

        // Act
        var act = () => asset.ScheduleMaintenance(second);

        // Assert
        var ex = Assert.Throws<InvalidOperationException>(act);
        Assert.Equal("Cannot schedule more than one pending preventive maintenance.", ex.Message);
    }

    [Fact]
    public void ScheduleMaintenance_PreventiveAfterCompletion_Succeeds()
    {
        // Arrange
        var asset = new Asset();
        var preventive = new MaintenanceRecord(DateTime.UtcNow, MaintenanceType.Preventive, "Tech", "Initial service");
        asset.ScheduleMaintenance(preventive);

        var completionTime = DateTime.UtcNow.AddHours(1);
        asset.CompleteMaintenance(preventive.Id, completionTime);

        var secondPreventive = new MaintenanceRecord(DateTime.UtcNow.AddDays(1), MaintenanceType.Preventive, "Tech 2", "Follow-up");

        // Act
        asset.ScheduleMaintenance(secondPreventive);

        // Assert
        Assert.Equal(2, asset.MaintenanceRecords.Count);
        Assert.Contains(secondPreventive, asset.MaintenanceRecords);
    }

    [Fact]
    public void ScheduleMaintenance_CorrectiveWhilePreventivePending_Succeeds()
    {
        // Arrange
        var asset = new Asset();
        var preventive = new MaintenanceRecord(DateTime.UtcNow, MaintenanceType.Preventive, null, null);
        asset.ScheduleMaintenance(preventive);

        var corrective = new MaintenanceRecord(DateTime.UtcNow.AddHours(2), MaintenanceType.Corrective, null, null);

        // Act
        asset.ScheduleMaintenance(corrective);

        // Assert
        Assert.Equal(2, asset.MaintenanceRecords.Count);
        Assert.Contains(corrective, asset.MaintenanceRecords);
    }    

    [Theory]
    [InlineData(MaintenanceType.Corrective)]
    [InlineData(MaintenanceType.Predictive)]
    public void ScheduleMaintenance_NonPreventiveWhilePreventivePending_Succeeds(MaintenanceType overlappingType)
    {
        // Arrange
        var asset = new Asset();
        var preventive = new MaintenanceRecord(DateTime.UtcNow, MaintenanceType.Preventive, null, null);
        asset.ScheduleMaintenance(preventive);
        var overlapping = new MaintenanceRecord(DateTime.UtcNow.AddHours(2), overlappingType, null, null);

        // Act
        asset.ScheduleMaintenance(overlapping);

        // Assert
        Assert.Equal(2, asset.MaintenanceRecords.Count);
        Assert.Contains(overlapping, asset.MaintenanceRecords);
        Assert.Equal(MaintenanceStatus.Pending, overlapping.Status);
        Assert.Null(overlapping.CompletedDate);
        Assert.True(preventive.IsPending);
    }

    [Fact]
    public void ScheduleMaintenance_PreventiveWhileNonPreventivePending_Succeeds()
    {
        // Arrange
        var asset = new Asset();
        var corrective = new MaintenanceRecord(DateTime.UtcNow, MaintenanceType.Corrective, null, null);
        asset.ScheduleMaintenance(corrective);
        var preventive = new MaintenanceRecord(DateTime.UtcNow.AddHours(2), MaintenanceType.Preventive, null, null);

        // Act
        asset.ScheduleMaintenance(preventive);

        // Assert
        Assert.Equal(2, asset.MaintenanceRecords.Count);
        Assert.Contains(preventive, asset.MaintenanceRecords);
        Assert.True(corrective.IsPending);
        Assert.True(preventive.IsPending);
    }

    [Fact]
    public void CompleteMaintenance_UnknownRecord_ThrowsInvalidOperationException()
    {
        // Arrange
        var asset = new Asset();
        var preventive = new MaintenanceRecord(DateTime.UtcNow, MaintenanceType.Preventive, null, null);
        asset.ScheduleMaintenance(preventive);
        var missingId = preventive.Id + 1;

        // Act
        var act = () => asset.CompleteMaintenance(missingId, DateTime.UtcNow);

        // Assert
        var ex = Assert.Throws<InvalidOperationException>(act);
        Assert.Equal("Maintenance record not found.", ex.Message);
    }

    [Fact]
    public void CompleteMaintenance_ExistingRecord_UpdatesStatusAndDate()
    {
        // Arrange
        var asset = new Asset();
        var preventive = new MaintenanceRecord(DateTime.UtcNow, MaintenanceType.Preventive, null, null);
        var corrective = new MaintenanceRecord(DateTime.UtcNow.AddHours(2), MaintenanceType.Corrective, null, null);
        asset.ScheduleMaintenance(preventive);
        asset.ScheduleMaintenance(corrective);
        var completionTime = DateTime.UtcNow.AddHours(1);

        // Act
        asset.CompleteMaintenance(preventive.Id, completionTime);

        // Assert
        Assert.Equal(MaintenanceStatus.Completed, preventive.Status);
        Assert.Equal(completionTime, preventive.CompletedDate);
        Assert.False(preventive.IsPending);

        Assert.Equal(MaintenanceStatus.Pending, corrective.Status);
        Assert.Null(corrective.CompletedDate);
    }
}