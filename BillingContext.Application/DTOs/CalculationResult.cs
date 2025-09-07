using System.Collections.Generic;
using ToSobesHz.BillingContext.Domain.ValueObjects;

namespace ToSobesHz.BillingContext.Application.DTOs
{
	public class CalculationResult
	{
		public List<ServiceConsumptionDto> Services { get; set; } = new();
		public decimal Total { get; set; }
	}
}


