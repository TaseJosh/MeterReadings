using MeterReadings.Core.Domain.Interface;
using MeterReadings.Core.Domain.Models;
using Microsoft.AspNetCore.Http;

namespace MeterReadings.Core.Infrastructure
{
    public interface IMeterReadingsCsvProcessor
    {
        /// <summary>
        /// Parse an IFormFile CSV file into a MeterReading class
        /// </summary>
        IMeterReadingsToSave ParseCsvFile(IFormFile file);
    }
}