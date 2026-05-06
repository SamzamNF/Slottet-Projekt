using System;
using System.Collections.Generic;
using System.Text;

namespace Slottet.Domain.Entities;

public class Risk
{
    public int Id { get; set; }
    // Set as Neutral until actual assessment is made
    public string? RiskAssessment { get; set; } = "Neutral";
}
