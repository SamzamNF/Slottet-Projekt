using System;

namespace Slottet.Shared.DTO;

public class MedicineDto
{
    public int Id { get; set; }
    public int ResidentId { get; set; }
    public int StaffId { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateTime TimeStamp { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsFromToday { get; set; }

    // UI helper property - checks if the medicine entry can be edited or deleted based on its timestamp and creation date
    public bool CanBeEditedOrDeleted { get; set; }
}