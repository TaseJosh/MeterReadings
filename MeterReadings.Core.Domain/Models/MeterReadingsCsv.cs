using System.Collections.Generic;
using MeterReadings.Core.Domain.Interface;

namespace MeterReadings.Core.Domain.Models
{
    public class MeterReadingsCsv : IMeterReadingsCsv
    {
        public List<MeterReadingCsv> MeterReadings { get; set; } = new List<MeterReadingCsv>();
    }
}
