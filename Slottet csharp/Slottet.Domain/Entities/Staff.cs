using System;

namespace Slottet.Domain.Entities;

public class Staff
{
    public int Id { get; set; } // Primary key
    public string? Initials { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string Email { get; set; } = string.Empty;
    public int DepartmentId { get; set; }
    public int RoleId { get; set; }

    
}
