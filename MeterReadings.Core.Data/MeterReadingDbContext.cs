using MeterReadings.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MeterReadings.Core.Data
{
    public class MeterReadingDbContext : DbContext
    {
        public MeterReadingDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<MeterReading> MeterReadings { get; set; }
    }
}
