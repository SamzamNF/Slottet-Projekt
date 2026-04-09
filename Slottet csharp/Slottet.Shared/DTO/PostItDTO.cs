using System;
using System.Collections.Generic;
using System.Text;

namespace Slottet.Shared.DTO
{
    internal class PostItDTO
    {
        public DateTime Date { get; set; }
        public string Payment { get; set; } = string.Empty;
        public string ShoppingDay { get; set; } = string.Empty;
        public string Mood { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string Events { get; set; } = string.Empty;
        public string RelativesContact { get; set; } = string.Empty;
    }
}
