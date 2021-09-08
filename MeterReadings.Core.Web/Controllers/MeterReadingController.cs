using AutoMapper;
using MeterReadings.Core.Data.Interfaces;
using MeterReadings.Core.Domain.Entities;
using MeterReadings.Core.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace MeterReadings.Core.Web.Controllers
{
    [ApiController]
    [Route("api/accounts/{accountId}/readings")]
    public class MeterReadingController : ControllerBase
    {
        private readonly IMeterReadingRepository _readingDbContext;
        private readonly IAccountRepository _accountDbContext;
        private readonly IMapper _mapper;

        public MeterReadingController(IMeterReadingRepository readingDbContext,
            IAccountRepository accountDbContext, IMapper mapper)
        {
            _accountDbContext = accountDbContext;
            _readingDbContext = readingDbContext;
            _mapper = mapper;
        }

        [HttpGet()]
        [HttpHead]
        public ActionResult<IEnumerable<MeterReadingDto>> GetReadings(int accountId)
        {
            var readingsFromRepo = _readingDbContext.GetMeterReadings(accountId);

            if (!readingsFromRepo.Any())
                return NotFound("No meter readings for that account");

            return Ok(_mapper.Map<IEnumerable<MeterReadingDto>>(readingsFromRepo));
        }

        [HttpGet("{readingId}", Name = "GetReading")]
        public ActionResult<MeterReadingDto> GetReading(int accountId, int readingId)
        {
            var readingFromRepo = _readingDbContext.GetMeterReading(accountId, readingId);

            if (readingFromRepo == null) return NotFound();

            return Ok(_mapper.Map<MeterReadingDto>(readingFromRepo));
        }

        [HttpPost]
        public ActionResult<MeterReadingDto> CreateReading(int accountId,
            MeterReadingCreationDto reading)
        {
            var accountFromRepo = _accountDbContext.GetAccount(accountId);

            if (accountFromRepo == null)
                return NotFound();


            var readingEntity = _mapper.Map<MeterReading>(reading);
            readingEntity.AccountId = accountFromRepo.Id;

            var duplicateReading = _readingDbContext.FindDuplicate(readingEntity);
            if (duplicateReading)
                return BadRequest("This reading has been added for this account");

            _readingDbContext.Add(readingEntity);
            _readingDbContext.SaveChanges();

            var meterReadingToReturn = _mapper.Map<MeterReadingDto>(readingEntity);

            return CreatedAtRoute($"GetReading", new
            {
                accountId,
                readingId = meterReadingToReturn.Id
            }, meterReadingToReturn);
        }

        [HttpPut("{readingId}")]
        public ActionResult UpdateReading(int accountId, int readingId,
            MeterReadingUpdateDto readingUpdateDto)
        {
            var readingFromRepo = _readingDbContext.GetMeterReading(accountId, readingId);

            if (readingFromRepo == null)
            {
                var readingToAdd = _mapper.Map<MeterReading>(readingUpdateDto);
                readingToAdd.AccountId = accountId;

                _readingDbContext.Add(readingToAdd);
                _readingDbContext.SaveChanges();

                var readingToReturn = _mapper.Map<MeterReadingDto>(readingToAdd);

                return CreatedAtRoute("GetReading", new
                {
                    accountId,
                    readingId = readingToReturn.Id
                }, readingToReturn);
            }

            _mapper.Map(readingUpdateDto, readingFromRepo);

            _readingDbContext.Update(readingFromRepo);

            _readingDbContext.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{readingId}")]
        public ActionResult DeleteReading(int accountId, int readingId)
        {
            var accountFromRepo = _accountDbContext.GetAccount(accountId);

            if (accountFromRepo == null)
                return NotFound();

            var readingFromAccountFromRepo = _readingDbContext
                .GetMeterReading(accountId, readingId);

            if (readingFromAccountFromRepo == null)
                return NotFound();

            _readingDbContext.Delete(readingFromAccountFromRepo);
            _readingDbContext.SaveChanges();

            return NoContent();
        }
    }
}

