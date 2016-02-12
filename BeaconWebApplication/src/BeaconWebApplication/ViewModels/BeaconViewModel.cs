using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BeaconWebApplication.ViewModels
{
    public class BeaconViewModel
    {
        public int Id { get; set; }

        [Required]
        public String UUID { get; set; }

        [Required]
        public String Major { get; set; }

        [Required]
        public String Minor { get; set; }
        public String NearMessage { get; set; }
        public String FarMessage { get; set; }
        public String ImmediateMessage { get; set; }
    }
}
