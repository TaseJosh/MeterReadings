using AutoMapper;
using MeterReadings.Core.Data.Interfaces;
using MeterReadings.Core.Domain.Entities;
using MeterReadings.Core.Domain.Helpers;
using MeterReadings.Core.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace MeterReadings.Core.Web.Controllers
{
    [ApiController]
    [Route("api/accounts")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository _accountContext;
        private readonly IMapper _mapper;

        public AccountController(IAccountRepository accountContext, IMapper mapper)
        {
            _accountContext = accountContext;
            _mapper = mapper;
        }

        [HttpGet()]
        [HttpHead]
        public ActionResult<IEnumerable<AccountDto>> GetAccounts()
        {
            var accountsFromRepo = _accountContext.All();
            if (accountsFromRepo.Any())
                return Ok(_mapper.Map<IEnumerable<AccountDto>>(accountsFromRepo));
            LoadSampleData();
            accountsFromRepo = _accountContext.All();
            return Ok(_mapper.Map<IEnumerable<AccountDto>>(accountsFromRepo));
        }

        [HttpGet("{accountId}", Name = "GetAccount")]
        public ActionResult<AccountDto> GetAccount(int accountId)
        {
            var accountFromRepo = _accountContext.GetAccount(accountId);

            if (accountFromRepo == null)
                return NotFound();

            return Ok(_mapper.Map<AccountDto>(accountFromRepo));
        }

        [HttpPost]
        public ActionResult<AccountDto> CreateAccount(AccountForCreationDto account)
        {
            var accountEntity = _mapper.Map<Account>(account);

            _accountContext.Add(accountEntity);
            _accountContext.SaveChanges();

            var accountToReturn = _mapper.Map<AccountDto>(accountEntity);
            return CreatedAtRoute("GetAccount",
                new { accountId = accountToReturn.AccountId }, accountToReturn);
        }

        [HttpPut("{accountId}")]
        public IActionResult UpdateAccount(int accountId, AccountUpdateDto accountUpdateDto)
        {
            var readingFromRepo = _accountContext.GetAccount
                (accountId);

            if (readingFromRepo == null)
            {
                var accountToAdd = _mapper.Map<Account>(accountUpdateDto);
                accountToAdd.AccountId = accountId;

                _accountContext.Add(accountToAdd);
                _accountContext.SaveChanges();

                var accountToReturn = _mapper.Map<AccountDto>(accountToAdd);

                return CreatedAtRoute("GetAccount",
                    new { accountId }, accountToReturn);
            }

            _mapper.Map(accountUpdateDto,
                readingFromRepo);

            _accountContext.Update(readingFromRepo);

            _accountContext.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{accountId}")]
        public ActionResult DeleteAccount(int accountId)
        {
            var readingFromRepo = _accountContext.GetAccount
                (accountId);

            if (readingFromRepo == null)
                return NotFound();

            _accountContext.DeleteAccount(readingFromRepo);
            _accountContext.SaveChanges();
            return NoContent();
        }


        private void LoadSampleData()
        {
            if (_accountContext.All().Count() != 0) return;
            var testAccountFile = System.IO.File.ReadAllText(Constants.TestAccountsJsonFIle);
            var accounts = JsonSerializer.Deserialize<List<Account>>(testAccountFile);
            _accountContext.SaveAll(accounts);
            _accountContext.SaveChanges();
        }
    }
}
