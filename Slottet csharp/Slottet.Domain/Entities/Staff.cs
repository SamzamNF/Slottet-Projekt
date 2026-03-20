using System;

namespace Slottet.Domain.Entities;

public class Staff
{
    public int Initials { get; set; } // Primary key
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string Email { get; set; } = string.Empty; // If�lge de nye svar
    public int DepartmentId { get; set; }
    public string? Role { get; set; }

    

    /*public Department Department { get; set; }
    public List<Phone> PhoneNumbers { get; set; }
    public List<ResponsibilityArea> ResponsibilityAreas { get; set; }
    */
}
