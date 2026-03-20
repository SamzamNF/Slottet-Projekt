using System;
using System.Collections.Generic;
using System.Text;

namespace Slottet.Domain.Entities
{
    public class Department
    {
        public int Id { get; set; }
        public string? DepartmentName { get; set; }

        // Skal ikke bruges
        //public List<Staff> Staff { get; set; } = new List<Staff>();
    }
}
