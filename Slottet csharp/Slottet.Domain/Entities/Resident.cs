namespace Slottet.Domain.Entities;

// The core Domain model representing a resident in the system.
// Holds the absolute truth about what a resident is in the business.

public class Resident
{
    public int Id { get; set; }
    public string Initial { get; set; }

    // Private constructor to enforce the use of the Create method for validation
    private Resident(string initial)
    {
        Initial = initial;
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
}