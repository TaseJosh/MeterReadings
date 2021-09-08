using MeterReadings.Core.Domain.Entities;
using System.Collections.Generic;

namespace MeterReadings.Core.Data.Interfaces
{
    public interface IAccountRepository
    {
        bool AccountExists(int accountId);
        void Add(Account entity);
        void AddAccount(Account account);
        IEnumerable<Account> All();
        void DeleteAccount(Account account);
        Account GetAccount(int accountId);
        Account GetAccountUsingId(int accountId);
        void SaveAll(IEnumerable<Account> entity);
        void SaveChanges();
        Account Update(Account entity);
    }
}