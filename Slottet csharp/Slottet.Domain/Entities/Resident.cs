using System;
using System.Collections.Generic;
using System.Text;

namespace Slottet.Domain.Entities
{
    public class Resident
    {
        public int Id { get; set; }

        public List<PostIt> PostIts { get; set; }
    }
}
