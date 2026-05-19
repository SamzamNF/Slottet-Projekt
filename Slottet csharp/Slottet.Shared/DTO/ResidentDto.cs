namespace Slottet.Shared.DTO;

// Data Transfer Object used to safely transport resident data between the API and the Frontend.
// Contains no business logic or database dependencies.

public class ResidentDto
{
    public int Id { get; set; }
    public string Initial { get; set; } = string.Empty;
    public string? Department { get; set; } // Til dummy data, hov, fy fy, dansk kommentar
    // bool IsArchived included for display purposes in the frontend
    public bool IsArchived { get; set; }
}