using System;

namespace Slottet.Shared.DTO;

public class StaffDto
{
    public int Initials { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string Email { get; set; } = string.Empty;
    public int DepartmentId { get; set; }
    public int? RoleId { get; set; }


    public string? Department { get; set; }
    public string? RoleName { get; set; }

    
}
