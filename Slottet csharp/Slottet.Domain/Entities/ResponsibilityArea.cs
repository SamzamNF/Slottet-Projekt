using System;
using System.Collections.Generic;
using System.Text;

namespace Slottet.Domain.Entities;

public class ResponsibilityArea
{
    public int Id { get; set; }
    public string? Description { get; set; }
    public DateTime Date { get; set; }

    
    public int StaffId { get; set; }
    public int PhoneId { get; set; }
    public Staff? Staff { get; set; }
    public Phone? Phone { get; set; }


    private ResponsibilityArea() { }

    public static ResponsibilityArea Create(
        string description, 
        DateTime date, 
        int staffId, 
        int phoneId
        )
    {
        if (staffId <= 0)
            throw new ArgumentException("Medarbejder skal være valgt.");
        if (phoneId <= 0)            
            throw new ArgumentException("Telefon skal være valgt.");
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Beskrivelse er påkrævet.");
        if (date == default)
            throw new ArgumentException("Dato er påkrævet.");

        return new ResponsibilityArea
        {
            Description = description,
            Date = date,
            StaffId = staffId,
            PhoneId = phoneId
        };
    }
    public void Update(
        string description,
        DateTime date,
        int staffId,
        int phoneId
        )
    {
        if (staffId <= 0)
            throw new ArgumentException("Medarbejder skal være valgt.");
        if (phoneId <= 0)
            throw new ArgumentException("Telefon skal være valgt.");
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Beskrivelse er påkrævet.");
        if (date == default)
            throw new ArgumentException("Dato er påkrævet.");

        Description = description;
        Date = date;
        StaffId = staffId;
        PhoneId = phoneId;
    }
}
