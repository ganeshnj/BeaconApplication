using Microsoft.Data.Entity;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeaconWebApplication.Models
{
    public class BeaconsRepository : IBeaconsRepository
    {
        private BeaconsContext _context;

        public BeaconsRepository(BeaconsContext context)
        {
            _context = context;
        }

        public IEnumerable<Beacon> GetAllBeacons()
        {
            return _context.Beacons.ToList();
        }

        public Beacon GetBeaconById(int? id)
        {
            return _context.Beacons.Where(b => b.Id == id).FirstOrDefault();
        }

        public Beacon GetBeaconByUuidAndMajorAndMinor(String uuid, String major, String minor)
        {
            var beacon = _context.Beacons.Where(b => b.UUID.ToLower().Equals(uuid.ToLower())
                                                   && b.Major.ToLower().Equals(major.ToLower())
                                                   && b.Minor.ToLower().Equals(minor.ToLower())).FirstOrDefault();
            return beacon;
        }

        public IEnumerable<Log> GetAllLogs()
        {
            return _context.Logs.Include(l => l.Beacon).OrderByDescending(l => l.CreatedOn).ToList();
        }

        public void AddLog(Log log)
        {
            _context.Logs.Add(log);
            _context.SaveChanges();
        }

        public void AddBeacon(Beacon beacon)
        {
            _context.Beacons.Add(beacon);
            _context.SaveChanges();
        }

        public void Edit(Beacon beacon)
        {
            _context.Entry<Beacon>(beacon).State = EntityState.Modified;
            var beaconToUpdate = _context.Beacons.Where(b => b.Id == beacon.Id).FirstOrDefault();
            _context.SaveChanges();
        }

        public void Delete(Beacon beacon)
        {
            _context.Beacons.Remove(beacon);
            _context.SaveChanges();
        }

        public string GetMessageByUuidAndMajorAndMinorAndProximity(string uuid, string major, string minor, string proximity)
        {
            var beacon = _context.Beacons.Where(b => b.UUID.ToLower().Equals(uuid.ToLower())
                                                   && b.Major.ToLower().Equals(major.ToLower())
                                                   && b.Minor.ToLower().Equals(minor.ToLower())).FirstOrDefault();
            if (beacon != null)
            {
                if (String.IsNullOrEmpty(proximity))
                {
                    var newLog = new Log() { Beacon = beacon, CreatedOn = DateTime.Now };
                    AddLog(newLog);

                    return beacon.FarMessage;
                }
                else
                {
                    Proximity p = StringToProximityConverter(proximity);
                    var newLog = new Log() { Beacon = beacon, CreatedOn = DateTime.Now, Proximity = p };
                    AddLog(newLog);

                    switch (p)
                    {
                        case Proximity.Near:
                            return beacon.NearMessage;
                        case Proximity.Immediate:
                            return beacon.ImmediateMessage;
                        default:
                            return beacon.FarMessage;
                    }
                }
            }

            return "Beacon not found in the database!";
        }
        private Proximity StringToProximityConverter(string proximity)
        {
            switch (proximity.ToLower())
            {
                case "proximitynear":
                    return Proximity.Near;
                case "proximityimmediate":
                    return Proximity.Immediate;
                default:
                    return Proximity.Far;
            }
        }
    }
}
