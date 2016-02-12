using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeaconWebApplication.Models
{
    public class BeaconsContextSeedData
    {
        private BeaconsContext _context;

        public BeaconsContextSeedData(BeaconsContext context)
        {
            _context = context;
        }
        public void EnsureSeedData()
        {
            if (!_context.Beacons.Any())
            {
                // Add new data
                var beacon = new Beacon()
                {
                    UUID = "f7826da6-4fa2-4e98-8024-bc5b71e0893e",
                    Major = "5262",
                    Minor = "65087",
                    FarMessage = "Hi, you are far!",
                    NearMessage = "Hi, you are near!",
                    ImmediateMessage = "Hi, you are immediate!"
                };

                _context.Beacons.Add(beacon);
                _context.SaveChanges();
            }
        }
    }
}
