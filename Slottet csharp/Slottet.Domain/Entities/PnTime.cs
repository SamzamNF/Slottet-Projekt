using System;

namespace Slottet.Domain.Entities;

public class PnTime
{
    public int Id { get; private set; }
    public DateTime Time { get; private set; }
    public string Description { get; private set; } = string.Empty;

    // EF Mapping property
    public PostIt? PostIt { get; set; }
    public int PostItId { get; set; }

    private PnTime() { }

    public static PnTime Create(DateTime time, string description)
    {
        if (time == default)
            throw new ArgumentException("Tidspunktet skal være gyldigt");

        return new PnTime
        {
            Time = time,
            Description = description
        };
    }

    public void Update(DateTime time, string description)
    {
        if (time == default)
            throw new ArgumentException("Tidspunktet skal være gyldigt");

        Time = time;
        Description = description;
    }
}
