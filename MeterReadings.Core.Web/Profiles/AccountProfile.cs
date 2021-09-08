using AutoMapper;
using MeterReadings.Core.Domain.Entities;
using MeterReadings.Core.Domain.Models;

namespace MeterReadings.Core.Web.Profiles
{
    public class AccountProfile : Profile
    {
        public AccountProfile()
        {
            CreateMap<Account, AccountDto>();
            CreateMap<AccountForCreationDto, Account>();
            CreateMap<AccountUpdateDto, Account>();
        }
    }
}
