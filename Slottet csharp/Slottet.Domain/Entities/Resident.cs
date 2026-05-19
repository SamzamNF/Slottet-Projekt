namespace Slottet.Domain.Entities;

// The core Domain model representing a resident in the system.
// Holds the absolute truth about what a resident is in the business.

public class Resident
{
    public int Id { get; set; }
    public string? Initial { get; set; }
    public bool IsArchived { get; set; }
    public DateTime? ArchivedAt { get; set; }

    public Department? Department { get; private set; }
    public int DepartmentId { get; private set; }

    // Private constructor to enforce the use of the Create method for validation
    private Resident()
    {
        IsArchived = false;
    }

    // Method to create a new staff object with validation
    public static Resident Create(string initial, int departmentId)
    {
        if (string.IsNullOrWhiteSpace(initial))
            throw new ArgumentException("Initialer skal udfyldes");
        if (departmentId <= 0)
            throw new ArgumentException("Department skal være valgt");

        return new Resident
        {
            Initial = initial,
            DepartmentId = departmentId
        };
    }

    // Update existing resident data with validation
    public void Update(string newInitial, int newDepartmentId)
    {
        if (string.IsNullOrWhiteSpace(newInitial))
            throw new ArgumentException("Initialer skal udfyldes");
        if (newDepartmentId <= 0)
            throw new ArgumentException("Department skal være valgt");
            
        Initial = newInitial;
        DepartmentId = newDepartmentId;
    }

    // Archive resident and set timestamp
    public void Archive()
    {
        IsArchived = true;
        ArchivedAt = DateTime.UtcNow;
    }
}