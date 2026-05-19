using System;
using System.Collections.Generic;
using System.Text;

namespace Slottet.Domain.Entities;

public class Phone
{
    public int Id { get; set; } // Primary key
    public string PhoneNumber { get; private set; } = string.Empty;

    private Phone() { }

    public static Phone Create(string phoneNumber)
    {
        // Sat requirement to between 8 and 14, can be changed to specifically 8 for denmark if needed
        if (string.IsNullOrWhiteSpace(phoneNumber) || phoneNumber.Length < 8 || phoneNumber.Length > 14)
        {
            throw new ArgumentException("Dit telefonnummer skal være mellem 8 og 14 tegn.");
        }

        return new Phone
        {
            PhoneNumber = phoneNumber
        };
    }

    public void Update(string phoneNumber)
    {
        // Sat requirement to between 8 and 14, can be changed to specifically 8 for denmark if needed
        if (string.IsNullOrWhiteSpace(phoneNumber) || phoneNumber.Length < 8 || phoneNumber.Length > 14)
        {
            throw new ArgumentException("Dit telefonnummer skal være mellem 8 og 14 tegn.");
        }

        PhoneNumber = phoneNumber;
    }

}
