using System;
using System.Collections.Generic;
using System.Text;

namespace Slottet.Domain.Entities
{
    public class Medicine
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string TimeStamp { get; set; }

        public PostIt PostIts { get; set; }
    }
}
