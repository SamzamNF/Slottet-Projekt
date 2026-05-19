using System;

namespace Slottet.Shared.DTO;

public class ResponsibilityAreaDTO
{
    public int Id { get; set; }
    public string? Description { get; set; }
    public DateTime Date { get; set; }

    
    public int StaffId { get; set; }
    public string? StaffInitials { get; set; }
    public int PhoneId { get; set; }
    public string? PhoneNumber { get; set; }

}
