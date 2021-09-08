using MeterReadings.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using MeterReadings.Core.Data.Interfaces;

namespace MeterReadings.Core.Data.Repositories
{
    public class AccountRepository : GenericRepository<Account>, IAccountRepository
    {
        public AccountRepository(MeterReadingDbContext context) : base(context)
        {
        }

        public void AddAccount(Account account)
        {
            if (account == null) throw new ArgumentNullException(nameof(account));
            Context.Add(account);
        }

        public bool AccountExists(int accountId)
        {
            return Context.Set<Account>().AsQueryable().Any(a => a.Id == accountId);
        }

        public void DeleteAccount(Account account)
        {
            if (account == null) throw new ArgumentNullException(nameof(account));
            Context.Remove(account);
        }

        public Account GetAccount(int accountId)
        {
            return Context.Set<Account>().AsQueryable().FirstOrDefault(a => a.Id == accountId);
        }

        public Account GetAccountUsingId(int accountId)
        {
            return Context.Set<Account>().AsQueryable().FirstOrDefault(a => a.AccountId == accountId);
        }

        public override Account Update(Account entity)
        {
            return base.Update(entity);
        }

        public override void SaveChanges()
        {
            base.SaveChanges();
        }

        public override void Add(Account entity)
        {
            base.Add(entity);
        }

        public override IEnumerable<Account> All()
        {
            return base.All();
        }

        public override void SaveAll(IEnumerable<Account> entity)
        {
            base.SaveAll(entity);
        }
    }
}
