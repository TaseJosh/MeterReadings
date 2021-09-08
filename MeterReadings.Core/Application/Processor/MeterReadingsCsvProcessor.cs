using CsvHelper;
using CsvHelper.TypeConversion;
using MeterReadings.Core.Application.CsvHandlers;
using MeterReadings.Core.Domain.Entities;
using MeterReadings.Core.Domain.Interface;
using MeterReadings.Core.Infrastructure;
using Microsoft.AspNetCore.Http;
using System;
using System.Globalization;
using System.IO;

namespace MeterReadings.Core.Application.Processor
{
    public class MeterReadingsCsvProcessor : IMeterReadingsCsvProcessor
    {
        private readonly ICsvCustomErrorHandler _csvCustomIntErrorHandler;
        private readonly IMeterReadingsToSave _readingsToSave;

        public MeterReadingsCsvProcessor(ICsvCustomErrorHandler csvCustomIntErrorHandler,
            IMeterReadingsToSave readingsToSave)
        {
            _csvCustomIntErrorHandler = csvCustomIntErrorHandler ??
                                        throw new ArgumentNullException(nameof(csvCustomIntErrorHandler));
            _readingsToSave = readingsToSave ?? throw new ArgumentNullException(nameof(readingsToSave));
        }

        /// <summary>
        ///     Implements a new meter reading csv processor.
        /// </summary>
        public IMeterReadingsToSave ParseCsvFile(IFormFile file)
        {
            if (file == null) throw new ArgumentNullException(nameof(file));
            TextReader textReader = new StreamReader(file.OpenReadStream());
            using var csv = new CsvReader(textReader, CultureInfo.InvariantCulture);
            {
                try
                {
                    return ParseCsv(csv);
                }
                catch (Exception ex)
                {
                    throw new ApplicationException
                        ("File is not correct format to process Meter Readings", ex);
                }
            }
        }

        private IMeterReadingsToSave ParseCsv(IReader csv)
        {
            csv.Read();
            csv.ReadHeader();
            while (csv.Read())
            {
                var reading = ReadOneValue(csv, _csvCustomIntErrorHandler);
                if (reading == null) _readingsToSave.FailedReadings++;
                else _readingsToSave.MeterReadings.Add(reading);
            }

            return _readingsToSave;
        }

        private static MeterReading ReadOneValue(IReaderRow csv, ITypeConverter customInt32Converter)
        {
            var reading = new MeterReading
            {
                AccountId = csv.GetField<int>("AccountId"),
                MeterReadingDateTime =
                    DateTime.ParseExact(csv.GetField<string>
                            ("MeterReadingDateTime"), "dd/MM/yyyy HH:mm",
                        CultureInfo.InvariantCulture),
                MeterReadValue = csv.GetField<int>("MeterReadValue", customInt32Converter)
            };
            if (reading.MeterReadValue < 1 || reading.MeterReadValue >= 100000) return null;
            return reading;
        }
    }
}
