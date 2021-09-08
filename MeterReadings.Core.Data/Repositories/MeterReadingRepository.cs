using MeterReadings.Core.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using MeterReadings.Core.Data.Interfaces;

namespace MeterReadings.Core.Data.Repositories
{
    public class MeterReadingRepository : GenericRepository<MeterReading>, IMeterReadingRepository
    {
        public MeterReadingRepository(MeterReadingDbContext context) : base(context)
        {
        }

        public void AddMeterReading(int accountId, MeterReading reading)
        {
            reading.AccountId = accountId;
            Context.Add(reading);
        }

        public MeterReading GetMeterReading(int accountId, int readingId)
        {
            return Context
                .Set<MeterReading>()
                .AsQueryable()
                .FirstOrDefault(m => m.AccountId == accountId && m.Id == readingId);
        }

        public IEnumerable<MeterReading> GetMeterReadings(int accountId)
        {
            return Context.Set<MeterReading>()
                .AsQueryable()
                .Where(m => m.AccountId == accountId)
                .OrderBy(m => m.MeterReadingDateTime)
                .ToList();
        }
        public override MeterReading Update(MeterReading entity)
        {
            return base.Update(entity);
        }

        public override void Delete(MeterReading entity)
        {
            base.Delete(entity);
        }

        public override void SaveChanges()
        {
            base.SaveChanges();
        }

        public override void Add(MeterReading entity)
        {
            base.Add(entity);
        }

        public override void SaveAll(IEnumerable<MeterReading> entity)
        {
            base.SaveAll(entity);
        }

        public bool FindDuplicate(MeterReading reading)
        {
            return base
                .Find(x => x.AccountId == reading.AccountId)
                .Any(i => i.MeterReadingDateTime == reading.MeterReadingDateTime &&
                          i.MeterReadValue == reading.MeterReadValue);
        }

    }
}
