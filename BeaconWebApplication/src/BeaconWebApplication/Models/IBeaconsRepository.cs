using System.Collections.Generic;

namespace BeaconWebApplication.Models
{
    public interface IBeaconsRepository
    {
        IEnumerable<Beacon> GetAllBeacons();
        Beacon GetBeaconById(int? id);
        IEnumerable<Log> GetAllLogs();
        void AddLog(Log log);
        void AddBeacon(Beacon beacon);
        Beacon GetBeaconByUuidAndMajorAndMinor(string uuid, string major, string minor);
        string GetMessageByUuidAndMajorAndMinorAndProximity(string uuid, string major, string minor, string proximity);
        void Edit(Beacon beacon);
        void Delete(Beacon beacon);
    }
}