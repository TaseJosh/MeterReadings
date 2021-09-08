using AutoMapper;
using MeterReadings.Core.Data.Interfaces;
using MeterReadings.Core.Domain.Entities;
using MeterReadings.Core.Domain.Helpers;
using MeterReadings.Core.Domain.Models;
using MeterReadings.Core.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using MeterReadings.Core.Domain.Interface;

namespace MeterReadings.Core.Web.Controllers
{
    [ApiController]
    [Route("api/file-upload")]
    public class FileUploadController : ControllerBase
    {
        private readonly IMeterReadingsCsvProcessor _readingsCsvProcessor;
        private IMeterReadingsToSave _result;
        private readonly IMapper _mapper;
        private readonly IMeterReadingRepository _readingDbContext;
        private readonly IAccountRepository _accountDbContext;
        private readonly IMeterReadingsCsv _meterReadingsCsv;

        public FileUploadController(IMeterReadingsCsvProcessor readingsCsvProcessor,
            IMeterReadingsToSave result, IMapper mapper, IMeterReadingRepository readingDbContext,
            IAccountRepository accountDbContext, IMeterReadingsCsv meterReadingsCsv)
        {
            _readingsCsvProcessor = readingsCsvProcessor;
            _result = result;
            _mapper = mapper;
            _readingDbContext = readingDbContext;
            _accountDbContext = accountDbContext;
            _meterReadingsCsv = meterReadingsCsv;
        }

        [HttpPost]
        [Route("upload", Name = "upload")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public IActionResult UploadFile(IFormFile file)
        {
            if (!file.FileName.Contains("csv")) return BadRequest("Please Upload only CSV files");
            if (file.Length < Constants.MinimumCharsForValidCsvFile) return BadRequest("File is empty");
            try
            {
                _result = _readingsCsvProcessor.ParseCsvFile(file);
            }
            catch (Exception ex)
            {
                return BadRequest($"File could not be processed {ex}");
            }

            VerifyMeterReadings();

            var readingEntities = _mapper.Map<IEnumerable<MeterReading>>(_meterReadingsCsv.MeterReadings);
            foreach (var reading in readingEntities)
            {
                _result.SuccessfulReadings++;
                _readingDbContext.Add(reading);
            }

            _readingDbContext.SaveChanges();
            return Ok($"Entries Saved: {_result.SuccessfulReadings}  {Environment.NewLine}" +
                      $"Failed Entries: {_result.FailedReadings} ");
        }

        private void VerifyMeterReadings()
        {
            foreach (var meterReading in _result.MeterReadings)
            {
                var accountFromRepo = _accountDbContext.GetAccountUsingId(meterReading.AccountId);
                if (accountFromRepo == null)
                {
                    _result.FailedReadings++;
                    continue;
                }

                meterReading.AccountId = accountFromRepo.Id;
                var duplicateReading = _readingDbContext.FindDuplicate(meterReading);
                if (duplicateReading)
                {
                    _result.FailedReadings++;
                    continue;
                }

                var readingEntities = _mapper.Map<MeterReadingCsv>(meterReading);
                _meterReadingsCsv.MeterReadings.Add(readingEntities);
            }

        }
    }
}
