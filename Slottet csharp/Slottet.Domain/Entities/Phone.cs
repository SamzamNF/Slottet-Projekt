using System;
using System.Collections.Generic;
using System.Text;

namespace Slottet.Domain.Entities;

public class Phone
{
    public int Id { get; set; } // Primary key
    public int PhoneNumber { get; set; } 

    private Phone() { }

    public static Phone Create(int phoneNumber)
    {
        // Sat requirement to between 8 and 14, can be changed to specifically 8 for denmark if needed
        if (phoneNumber.ToString().Length < 8 || phoneNumber.ToString().Length > 14 )
        {
            throw new ArgumentException("Dit telefonnummer skal være mellem 8 og 14 tegn.");
        }

        return new Phone
        {
            PhoneNumber = phoneNumber
        };
    }

    public void Update(int phoneNumber)
    {
        // Sat requirement to between 8 and 14, can be changed to specifically 8 for denmark if needed
        if (phoneNumber.ToString().Length < 8 || phoneNumber.ToString().Length > 14 )
        {
            throw new ArgumentException("Dit telefonnummer skal være mellem 8 og 14 tegn.");
        }

        PhoneNumber = phoneNumber;
    }

}
