using MeterReadings.Core.Domain.Entities;
using System.Collections.Generic;

namespace MeterReadings.Core.Data.Interfaces
{
    public interface IMeterReadingRepository
    {
        void Add(MeterReading entity);
        void AddMeterReading(int accountId, MeterReading reading);
        void Delete(MeterReading entity);
        bool FindDuplicate(MeterReading reading);
        MeterReading GetMeterReading(int accountId, int readingId);
        IEnumerable<MeterReading> GetMeterReadings(int accountId);
        void SaveAll(IEnumerable<MeterReading> entity);
        void SaveChanges();
        MeterReading Update(MeterReading entity);
    }
}