using System;

namespace Slottet.Domain.Entities;

public class Medicine
{
    public int Id { get; private set; }
    public string Description { get; private set; } = string.Empty;
    public DateTime TimeStamp { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public bool IsFromToday { get; private set; }

    // Foreign keys
    public int ResidentId { get; private set; }
    public Resident? Resident { get; private set; }
    
    public int StaffId { get; private set; }
    public Staff? Staff { get; private set; }

    // Required by EF Core
    private Medicine() { }

    public static Medicine Create(int residentId, int staffId, string description, DateTime timeStamp)
    {
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Beskrivelse er påkrævet");

        var now = DateTime.UtcNow;
        // Check if the provided timeStamp is from today
        bool isFromToday = timeStamp.Date == now.Date;

        return new Medicine
        {
            ResidentId = residentId,
            StaffId = staffId,
            Description = description,
            TimeStamp = timeStamp,
            CreatedAt = now,
            IsFromToday = isFromToday
        };
    }

    public void Update(string newDescription)
    {
        if (!CanBeEditedOrDeleted())
            throw new InvalidOperationException("Redigering/sletning er kun tilladt samme dag");

        if (string.IsNullOrWhiteSpace(newDescription))
            throw new ArgumentException("Beskrivelse er påkrævet");

        Description = newDescription;
    }

    // Edit/delete rule validation - only allow if the TimeStamp is from today
    public bool CanBeEditedOrDeleted()
    {
        var now = DateTime.UtcNow;
        return TimeStamp.Date == now.Date;
    }
}