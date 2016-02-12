using BeaconWebApplication.Models;
using Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BeaconWebApplication.ViewModels
{
    public class LogViewModel
    {
        public int Id { get; set; }
        public DateTime CreatedOn { get; set; }

        [Required]
        public Proximity Proximity { get; set; }

        // Navigation properties
        public virtual Beacon Beacon { get; set; }
    }
}
