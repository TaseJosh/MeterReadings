using MeterReadings.Core.Domain.Entities;
using System.Collections.Generic;
using MeterReadings.Core.Domain.Interface;

namespace MeterReadings.Core.Domain.Models
{
    public class MeterReadingsToSave : IMeterReadingsToSave
    {
        public List<MeterReading> MeterReadings { get; set; } = new List<MeterReading>();
        public int FailedReadings { get; set; }
        public int SuccessfulReadings { get; set; }
    }
}