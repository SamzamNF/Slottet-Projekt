using System;
using System.Collections.Generic;
using System.Text;

namespace Slottet.Domain.Entities;

public class PostIt
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public string Payment { get; set; } = string.Empty;
    public string ShoppingDay { get; set; } = string.Empty;
    public string Mood { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Events { get; set; } = string.Empty;
    public string RelativesContact { get; set; } = string.Empty;

    public int StaffId { get; set; }
    public int MedicineId { get; set; }
    public int ResidentId { get; set; }
    public int RiskId { get; set; }

    // Navigation properties
    public Risk? Risk { get; set; }

    private PostIt() { }

    // Service
    public static PostIt Create(
        DateTime date,
        string payment,
        string shoppingDay,
        string mood,
        string status,
        string events,
        string relativesContact
    )
    {
        if (date == default)
            throw new ArgumentException("Dato er påkrævet");
        if (string.IsNullOrWhiteSpace(payment))
            throw new ArgumentException("Betaling er påkrævet");
        if (string.IsNullOrWhiteSpace(shoppingDay))
            throw new ArgumentException("Indkøbsdag er påkrævet");
        if (string.IsNullOrWhiteSpace(mood))
            throw new ArgumentException("Humør er påkrævet");
        if (string.IsNullOrWhiteSpace(status))
            throw new ArgumentException("Status er påkrævet");

        return new PostIt
        {
            Date = date,
            Payment = payment,
            ShoppingDay = shoppingDay,
            Mood = mood,
            Status = status,
            Events = events,
            RelativesContact = relativesContact,
            // Attach new Risk entity with default value
            Risk = new Risk()
        };
    }
}
