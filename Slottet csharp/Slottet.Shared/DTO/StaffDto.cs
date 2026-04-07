using System;

namespace Slottet.Shared.DTO;

public class StaffDto
{
    public int Id { get; set; }
    public string Initials { get; set; } = string.Empty;
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public int DepartmentId { get; set; }
    public int RoleId { get; set; }


    public string? Department { get; set; }
    public string? RoleName { get; set; }

    
}
