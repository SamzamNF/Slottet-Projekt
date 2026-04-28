using System;
using System.Collections.Generic;
using System.Text;

namespace Slottet.Domain.Entities;

public class Medicine
{
    public int Id { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateTime TimeStamp { get; set; }
}
