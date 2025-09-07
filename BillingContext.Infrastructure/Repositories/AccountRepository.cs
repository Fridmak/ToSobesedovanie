using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ToSobesHz.BillingContext.Application.DTOs;
using ToSobesHz.BillingContext.Infrastructure.Data;

namespace ToSobesHz.BillingContext.Infrastructure.Repositories
{
	public class AccountRepository
	{
		public async Task<Guid> UpsertAccountAsync(string accountNumber)
		{
			await using var db = new BillingDbContext();
			db.Database.Migrate();
			var account = await db.Accounts.FirstOrDefaultAsync(a => a.AccountNumber == accountNumber);
			if (account == null)
			{
				account = new AccountEntity { Id = Guid.NewGuid(), AccountNumber = accountNumber };
				db.Accounts.Add(account);
				await db.SaveChangesAsync();
			}
			return account.Id;
		}

		public async Task SaveCalculationAsync(Guid accountId, DateTime period, IEnumerable<ServiceConsumptionDto> services)
		{
			await using var db = new BillingDbContext();
			db.Database.Migrate();
			
			var existing = db.Calculations.Where(c => c.AccountId == accountId && c.Period == period);
			db.Calculations.RemoveRange(existing);

			foreach (var s in services)
			{
				db.Calculations.Add(new CalculationEntryEntity
				{
					Id = Guid.NewGuid(),
					AccountId = accountId,
					Period = period,
					ServiceType = s.ServiceType.ToString(),
					Volume = s.Volume,
					Unit = s.Unit,
					Tariff = s.Tariff,
					Charge = s.Charge
				});
			}
			await db.SaveChangesAsync();
		}
	}
}
