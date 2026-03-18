using System;
using System.Collections.Generic;
using System.Text;

namespace Slottet.Domain.Entities
{
    public class PostIt
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Payment { get; set; }
        public string DayOfAction { get; set; }
        public string Status { get; set; }

        public Staff Staff { get; set; }
        public Medicine Medicine { get; set; }
        public Resident Residents { get; set; }
        public Risk Risks { get; set; }

    }
}
