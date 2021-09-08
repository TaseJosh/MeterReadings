using System;

namespace MeterReadings.Core.Domain.Models
{
    public class MeterReadingUpdateDto
    {
        public DateTime MeterReadingDateTime { get; set; }
        public string MeterReadValue { get; set; }
    }
}
