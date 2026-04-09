using System;
using System.Collections.Generic;
using System.Text;

namespace Slottet.Domain.Entities
{
    public class PostIt
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Payment { get; set; } = string.Empty;
        public string ShoppingDay { get; set; } = string.Empty;
        public string Mood { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string Events { get; set; } = string.Empty;
        public string RelativesContact { get; set; } = string.Empty;

        public int StaffId { get; set; }
        public int MedicineId { get; set; }
        public int ResidentId { get; set; }
        public int RiskId { get; set; }
    }
}
