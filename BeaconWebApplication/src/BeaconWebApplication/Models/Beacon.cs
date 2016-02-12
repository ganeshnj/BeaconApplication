using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeaconWebApplication.Models
{
    public class Beacon
    {
        public int Id { get; set; }
        public String UUID { get; set; }
        public String Major { get; set; }
        public String Minor { get; set; }
        public String NearMessage { get; set; }
        public String FarMessage { get; set; }
        public String ImmediateMessage { get; set; }

    }
}
