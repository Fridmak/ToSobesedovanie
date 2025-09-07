using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToSobesHz.BillingContext.Domain.ValueObjects;

namespace ToSobesHz.BillingContext.Application.DTOs
{
	public class ServiceConsumptionDto
	{
		public ServiceType ServiceType { get; set; }
		public decimal Volume { get; set; }
		public string Unit { get; set; } = string.Empty;
		public decimal Tariff { get; set; }
		public decimal Charge { get; set; }
	}
}
