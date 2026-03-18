using System;
using System.Collections.Generic;
using System.Text;

namespace Slottet.Domain.Entities
{
    public class Phone
    {
        public int PhoneNumber { get; set; } // Primary key
        public string Initials { get; set; } // Ikke sikker

        public Staff Staff { get; set; }
    }
}
