using System;

namespace Slottet.Domain.Entities;

public class Staff
{
    public int Id { get; set; }
    public string Initials { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? Email { get; set; }

    // Navigation properties/foreign keys
    public int DepartmentId { get; set; }
    public string? Role { get; set; }
}
