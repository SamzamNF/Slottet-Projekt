using System;
using System.Collections.Generic;
using System.Text;

namespace Slottet.Domain.Entities
{
    public class PostIt
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string? Payment { get; set; }
        public string? DayOfAction { get; set; }
        public string? Status { get; set; }
        public string? Mood { get; set; }

        public int StaffId { get; set; }
        public int MedicineId { get; set; }
        public int ResidentId { get; set; }
        public int RiskId { get; set; }
        


        /*public Staff Staff { get; set; }
        public Medicine Medicine { get; set; }
        public Resident Residents { get; set; }
        public Risk Risks { get; set; }
        */

    }
}
