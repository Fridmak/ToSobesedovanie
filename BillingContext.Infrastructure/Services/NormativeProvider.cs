using System;
using ToSobesHz.BillingContext.Domain.Services.Interfaces;
using ToSobesHz.BillingContext.Domain.ValueObjects;

namespace ToSobesHz.BillingContext.Infrastructure.Services
{
	public class NormativeProvider : INormativeProvider
	{
		private static readonly Normative ColdWaterNorm = new(4.85m, Amount.CubicMeters);
		private static readonly Normative HotWaterHeatCarrierNorm = new(4.01m, Amount.CubicMeters);
		private static readonly Normative HotWaterHeatEnergyNorm = new(0.05349m, Amount.Gigacalories);
		private static readonly Normative ElectricityGeneralNorm = new(164m, Amount.KilowattHours);

		public Normative GetNormative(ServiceType serviceType)
		{
			return serviceType switch
			{
				ServiceType.ColdWater => ColdWaterNorm,
				ServiceType.HotWaterHeatCarrier => HotWaterHeatCarrierNorm,
				ServiceType.HotWaterHeatEnergy => HotWaterHeatEnergyNorm,
				ServiceType.ElectricityDay => ElectricityGeneralNorm,
				ServiceType.ElectricityNight => ElectricityGeneralNorm,
				_ => ElectricityGeneralNorm
			};
		}
	}
}
