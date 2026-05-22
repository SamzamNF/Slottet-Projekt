using System;

namespace Slottet.Shared.DTO;

public class MedicineDto
{
    public int Id { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateTime TimeStamp { get; set; }
    public DateTime CreatedAt { get; set; }

}