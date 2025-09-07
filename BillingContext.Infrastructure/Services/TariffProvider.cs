using System;
using ToSobesHz.BillingContext.Domain.Services.Interfaces;
using ToSobesHz.BillingContext.Domain.ValueObjects;

namespace ToSobesHz.BillingContext.Infrastructure.Services
{
	public class TariffProvider : ITariffProvider
	{
		private const decimal ColdWaterTariff = 35.78m;               // ХВС
		private const decimal HotWaterHeatCarrierTariff = 35.78m;     // ГВС Теплоноситель
		private const decimal HotWaterHeatEnergyTariff = 998.69m;     // ГВС Тепловая энергия
		private const decimal ElectricityGeneralTariff = 4.28m;       // ЭЭ общий (без диф. учёта)
		private const decimal ElectricityDayTariff = 4.90m;           // ЭЭ день
		private const decimal ElectricityNightTariff = 2.31m;         // ЭЭ ночь

		public Tariff GetTariff(ServiceType serviceType)
		{
			return serviceType switch
			{
				ServiceType.ColdWater => new Tariff(ColdWaterTariff),
				ServiceType.HotWaterHeatCarrier => new Tariff(HotWaterHeatCarrierTariff),
				ServiceType.HotWaterHeatEnergy => new Tariff(HotWaterHeatEnergyTariff),
				ServiceType.ElectricityDay => new Tariff(ElectricityDayTariff),
				ServiceType.ElectricityNight => new Tariff(ElectricityNightTariff),
				_ => new Tariff(ElectricityGeneralTariff)
			};
		}
	}
}
