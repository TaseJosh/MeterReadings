using System;

namespace MeterReadings.Core.Domain.Models
{
    public class MeterReadingDto
    {
        public string Id { get; set; }
        public string AccountId { get; set; }
        public DateTime MeterReadingDateTime { get; set; }
        public string MeterReadValue { get; set; }
    }
}
