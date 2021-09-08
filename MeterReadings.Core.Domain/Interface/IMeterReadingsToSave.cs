using System.Collections.Generic;
using MeterReadings.Core.Domain.Entities;

namespace MeterReadings.Core.Domain.Interface
{
    public interface IMeterReadingsToSave
    {
        int FailedReadings { get; set; }
        List<MeterReading> MeterReadings { get; set; }
        int SuccessfulReadings { get; set; }
    }
}