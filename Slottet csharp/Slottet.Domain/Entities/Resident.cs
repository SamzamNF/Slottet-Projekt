namespace Slottet.Domain.Entities;

// The core Domain model representing a resident in the system.
// Holds the absolute truth about what a resident is in the business.

public class Resident
{
    public int Id { get; set; }
    public string Initial { get; set; }
    public bool IsArchived { get; set; }
    public DateTime? ArchivedAt { get; set; }

    // Private constructor to enforce the use of the Create method for validation
    private Resident(string initial)
    {
        Initial = initial;
        IsArchived = false;
    }

    // Method to create a new staff object with validation
    public static Resident Create(string initial)
    {
        if (string.IsNullOrWhiteSpace(initial))
        {
            throw new ArgumentException("Initialer skal udfyldes");
        }
        return new Resident(initial);
    }

    // Update existing resident data with validation
    public void Update(string newInitial)
    {
        if (string.IsNullOrWhiteSpace(newInitial))
        {
            throw new ArgumentException("Initialer skal udfyldes");
        }
        Initial = newInitial;
    }

    // Archive resident and set timestamp
    public void Archive()
    {
        IsArchived = true;
        ArchivedAt = DateTime.UtcNow;
    }
}