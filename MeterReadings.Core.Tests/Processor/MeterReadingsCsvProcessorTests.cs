using MeterReadings.Core.Application.CsvHandlers;
using MeterReadings.Core.Application.Processor;
using MeterReadings.Core.Domain.Entities;
using MeterReadings.Core.Domain.Interface;
using MeterReadings.Core.Domain.Models;
using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;

namespace MeterReadings.Core.Processor
{
    [TestFixture]
    public class MeterReadingsCsvProcessorTests
    {
        private MeterReadingsCsvProcessor _processor;
        private IMeterReadingsToSave _readingsToSave;
        private ICsvCustomErrorHandler _csvCustomIntErrorHandler;
        private IFormFile _mockIFormFile;

        [SetUp]
        public void Setup()
        {
            _readingsToSave = new MeterReadingsToSave();
            _csvCustomIntErrorHandler = new CsvCustomErrorHandler<int>();
            _processor = new MeterReadingsCsvProcessor(_csvCustomIntErrorHandler, _readingsToSave);
        }

        private static IFormFile CreateMockIFormFile(string filePath)
        {
            var directory =
                Environment.CurrentDirectory
                    [..Environment.CurrentDirectory.IndexOf("bin", StringComparison.Ordinal)];
            var meterReadingCsvFile = Path.Combine(directory, filePath);
            var csvFileContent = File.ReadAllText(meterReadingCsvFile);
            var formFile = new Mock<IFormFile>();
            var memory = new MemoryStream();
            var writer = new StreamWriter(memory);
            writer.Write(csvFileContent);
            writer.Flush();
            memory.Position = 0;
            formFile.Setup(_ => _.FileName).Returns(meterReadingCsvFile);
            formFile.Setup(_ => _.Length).Returns(memory.Length);
            formFile.Setup(_ => _.OpenReadStream()).Returns(memory);
            formFile.Verify();
            return formFile.Object;
        }

        [Test]
        [TestCase(8, 27, @"Data\Meter_Reading.csv")]
        [TestCase(0, 1, @"Data\One_Reading.csv")]
        [TestCase(0, 0, @"Data\Error_File.csv")]
        public void ShouldReturnMeterReadingParseResultUsingUploadedFileContent
                 (int failed, int parsed, string testValue)
        {
            _mockIFormFile = CreateMockIFormFile(testValue);

            _readingsToSave = _processor.ParseCsvFile(_mockIFormFile);

            Assert.IsNotNull(_readingsToSave.MeterReadings);
            Assert.AreEqual(failed, _readingsToSave.FailedReadings);
            Assert.AreEqual(parsed, _readingsToSave.MeterReadings.Count);
            Assert.IsInstanceOf<List<MeterReading>>(_readingsToSave.MeterReadings);
        }

        [Test]
        public void ShouldThrowArgumentExceptionIfFileIsEmpty()
        {
            const string filePath = @"Data\Error_File.csv";
            _mockIFormFile = CreateMockIFormFile(filePath);
            _readingsToSave = _processor.ParseCsvFile(_mockIFormFile);
            Assert.Throws<ArgumentException>(() => _processor.ParseCsvFile(_mockIFormFile));
        }

        [Test]
        public void ShouldThrowExceptionIfRequestIsNull()
        {
            var actual = Assert.Throws<ArgumentNullException>(() => _processor.ParseCsvFile(null));
        }

    }
}
