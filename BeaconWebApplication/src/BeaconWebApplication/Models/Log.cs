using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeaconWebApplication.Models
{
    public class Log
    {
        public int Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public Proximity Proximity { get; set; }

        // Navigation properties
        public virtual Beacon Beacon { get; set; }

    }
}
