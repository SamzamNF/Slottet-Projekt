using System;
using System.Collections.Generic;
using System.Text;

namespace Slottet.Domain.Entities
{
    public class Department
    {
        public int Id { get; set; }
        public string? DepartmentName { get; set; }

        
        public static Department Create(string departmentName)
        {
            if (string.IsNullOrWhiteSpace(departmentName))
                throw new ArgumentException("Afdelingsnavn er påkrævet.");

            return new Department
            {
                DepartmentName = departmentName
            };
        }

        public void Update(string departmentName)
        {
            if (string.IsNullOrWhiteSpace(departmentName))
                throw new ArgumentException("Afdelingsnavn er påkrævet.");

            DepartmentName = departmentName;
        }
    }
}
