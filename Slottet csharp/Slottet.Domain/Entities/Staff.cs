using System;

namespace Slottet.Domain.Entities;

public class Staff
{
    public int Id { get; private set; } // Primary key
    public string? Initials { get; private set; }
    public string? FirstName { get; private set; }
    public string? LastName { get; private set; }
    public string Email { get; private set; } = string.Empty;
    public int DepartmentId { get; private set; }
    public int RoleId { get; private set; }


    // Private constructor to enforce the use of the Create method for validation
    private Staff () { }


    // Method to create a new staff object with validation
    public static Staff Create(
        string initials,
        string firstName,
        string lastName,
        string email,
        int departmentId,
        int roleId
    )
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("FirstName er påkrævet");
        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("LastName er påkrævet");
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email er påkrævet");
        if (string.IsNullOrWhiteSpace(initials))
            throw new ArgumentException("Initialer er påkrævet");
        if (departmentId <= 0)
            throw new ArgumentException("Department skal være valgt");
        if (roleId <= 0)
            throw new ArgumentException("Rolle skal være valgt");

        return new Staff
        {
            Initials = initials,
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            DepartmentId = departmentId,
            RoleId = roleId
        };
    }
        





    
}
