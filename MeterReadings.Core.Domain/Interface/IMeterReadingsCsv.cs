using System.Collections.Generic;
using MeterReadings.Core.Domain.Models;

namespace MeterReadings.Core.Domain.Interface
{
    public interface IMeterReadingsCsv
    {
        List<MeterReadingCsv> MeterReadings { get; set; }
    }
}