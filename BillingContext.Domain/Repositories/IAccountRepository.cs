using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToSobesHz.BillingContext.Domain.Entities;

// Основное БД приложения

namespace ToSobesHz.BillingContext.Domain.Repositories
{
    public interface IAccountRepository
    {
        Task<Account> GetByIdAsync(int id);
        Task SaveAsync(Account account);
    }
}
