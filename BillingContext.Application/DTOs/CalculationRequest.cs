using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToSobesHz.BillingContext.Application.DTOs
{
	public class CalculationRequest
	{
		public string AccountNumber { get; set; } = string.Empty;
		public DateTime Period { get; set; }

		// Эквивалентное количество проживающих за месяц (с учётом смены в середине месяца)
		public decimal OccupantsEquivalent { get; set; }

		// ХВС
		public bool HasColdWaterMeter { get; set; }
		public decimal? ColdWaterCurrent { get; set; }
		public decimal? ColdWaterPrevious { get; set; }

		// ГВС
		public bool HasHotWaterMeter { get; set; }
		public decimal? HotWaterCurrent { get; set; }
		public decimal? HotWaterPrevious { get; set; }

		// ЭЭ (двухтарифный)
		public bool HasElectricityMeter { get; set; }
		public decimal? ElectricityDayCurrent { get; set; }
		public decimal? ElectricityDayPrevious { get; set; }
		public decimal? ElectricityNightCurrent { get; set; }
		public decimal? ElectricityNightPrevious { get; set; }
	}
}
