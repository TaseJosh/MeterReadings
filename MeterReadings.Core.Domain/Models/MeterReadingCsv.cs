using System;

namespace MeterReadings.Core.Domain.Models
{
    public class MeterReadingCsv
    {
        public string AccountId { get; set; }
        public DateTime MeterReadingDateTime { get; set; }
        public string MeterReadValue { get; set; }
    }
}
