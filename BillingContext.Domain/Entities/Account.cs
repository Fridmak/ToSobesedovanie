using System;
using System.Collections.Generic;

namespace ToSobesHz.BillingContext.Domain.Entities
{
	public class Account
	{
		public Guid Id { get; set; }
		public string AccountNumber { get; set; } = string.Empty;
	}
}
