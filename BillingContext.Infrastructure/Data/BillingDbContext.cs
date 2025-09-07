using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ToSobesHz.BillingContext.Infrastructure.Data
{
	public class BillingDbContext : DbContext
	{
		public DbSet<AccountEntity> Accounts { get; set; } = null!;
		public DbSet<CalculationEntryEntity> Calculations { get; set; } = null!;

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlite("Data Source=billing.db");
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<AccountEntity>(e =>
			{
				e.HasKey(x => x.Id);
				e.Property(x => x.AccountNumber).IsRequired();
				e.HasMany(x => x.Calculations).WithOne().HasForeignKey(x => x.AccountId);
			});

			modelBuilder.Entity<CalculationEntryEntity>(e =>
			{
				e.HasKey(x => x.Id);
				e.Property(x => x.Period).IsRequired();
				e.Property(x => x.ServiceType).IsRequired();
				e.Property(x => x.Volume).HasColumnType("decimal(18,4)");
				e.Property(x => x.Tariff).HasColumnType("decimal(18,4)");
				e.Property(x => x.Charge).HasColumnType("decimal(18,2)");
			});
		}
	}

	public class AccountEntity
	{
		public Guid Id { get; set; }
		public string AccountNumber { get; set; } = string.Empty;
		public List<CalculationEntryEntity> Calculations { get; set; } = new();
	}

	public class CalculationEntryEntity
	{
		public Guid Id { get; set; }
		public Guid AccountId { get; set; }
		public DateTime Period { get; set; }
		public string ServiceType { get; set; } = string.Empty;
		public decimal Volume { get; set; }
		public string Unit { get; set; } = string.Empty;
		public decimal Tariff { get; set; }
		public decimal Charge { get; set; }
	}
}
