using System;
using System.Collections.Generic;
using System.Text;

namespace Slottet.Domain.Entities;

public class ResponsibilityArea
{
    public int Id { get; set; }
    public string? Description { get; set; }
    public string? EmployeeInitials { get; set; }
    public DateTime Date { get; set; }

    
    public int StaffId { get; set; }
    
    // Skal ikke bruges
    //public Staff Staff { get; set; }
}
