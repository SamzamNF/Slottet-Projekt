using System;

namespace Slottet.Domain.Entities;

public class Medicine
{
    public int Id { get; private set; }
    public string Description { get; private set; } = string.Empty;
    public DateTime TimeStamp { get; private set; }
    public DateTime CreatedAt { get; private set; }


    // Foreign keys

    public int PostItId { get; set; }
    public PostIt? PostIt { get; set; }

    // Required by EF Core
    private Medicine() { }

    public static Medicine Create(string description, DateTime timeStamp)
    {
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Beskrivelse er påkrævet");

        var now = DateTime.UtcNow;
        // Check if timeStamp is from today
        bool isFromToday = timeStamp.Date == now.Date;

        return new Medicine
        {
            Description = description,
            TimeStamp = timeStamp,
            CreatedAt = now,
        };
    }

    public void Update(string newDescription)
    {
        if (string.IsNullOrWhiteSpace(newDescription))
            throw new ArgumentException("Beskrivelse er påkrævet");

        Description = newDescription;
    }

}