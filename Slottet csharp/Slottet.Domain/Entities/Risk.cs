using System;
using System.Collections.Generic;
using System.Text;

namespace Slottet.Domain.Entities
{
    public class Risk
    {
        public int Id { get; set; }
        public string RiskAssessment { get; set; }

        public PostIt PostIts { get; set; }
    }
}
