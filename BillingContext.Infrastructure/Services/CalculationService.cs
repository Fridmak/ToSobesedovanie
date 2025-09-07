using System;
using ToSobesHz.BillingContext.Domain.Entities;
using ToSobesHz.BillingContext.Domain.Services.Interfaces;
using ToSobesHz.BillingContext.Domain.ValueObjects;

namespace ToSobesHz.BillingContext.Infrastructure.Services
{
	public class CalculationService : ICalculationService
	{
		private readonly ITariffProvider _tariffProvider;
		private readonly INormativeProvider _normativeProvider;

		public CalculationService(ITariffProvider tariffProvider, INormativeProvider normativeProvider)
		{
			_tariffProvider = tariffProvider;
			_normativeProvider = normativeProvider;
		}

		// Общая формула: P = V * T
		public decimal CalculateCharge(Account account, ServiceType serviceType, bool hasMeter, DateTime calculationDate)
		{
			var tariff = _tariffProvider.GetTariff(serviceType).Value;

			decimal volume = 0m;

			if (hasMeter)
			{
				throw new NotImplementedException("Показания счётчика должны быть переданы извне для расчёта по приборам учёта.");
			}
			else
			{
				// По нормативу: V = n * N
				var normative = _normativeProvider.GetNormative(serviceType);
				int occupants = 1; // заглушка
				volume = occupants * normative.ValuePerPerson;
			}

			return Math.Round(volume * tariff, 2, MidpointRounding.AwayFromZero);
		}

		public static decimal CalculateMeteredVolume(decimal currentReading, decimal? previousReading)
		{
			var prev = previousReading ?? 0m;
			var diff = currentReading - prev;
			if (diff < 0)
			{
				throw new ArgumentException("Текущие показания меньше предыдущих.");
			}
			return diff;
		}

		// Тепловая энергия = Vгвстн * Nгвстэ
		public (decimal heatCarrierCharge, decimal heatEnergyCharge, decimal total) CalculateHotWaterCharges(decimal heatCarrierVolume)
		{
			var heatCarrierTariff = _tariffProvider.GetTariff(ServiceType.HotWaterHeatCarrier).Value;
			var heatEnergyTariff = _tariffProvider.GetTariff(ServiceType.HotWaterHeatEnergy).Value;
			var heatEnergyNorm = _normativeProvider.GetNormative(ServiceType.HotWaterHeatEnergy).ValuePerPerson; // Норматив на 1 м3

			var heatCarrierCharge = Math.Round(heatCarrierVolume * heatCarrierTariff, 2, MidpointRounding.AwayFromZero);
			var heatEnergyVolume = heatCarrierVolume * heatEnergyNorm;
			var heatEnergyCharge = Math.Round(heatEnergyVolume * heatEnergyTariff, 2, MidpointRounding.AwayFromZero);
			return (heatCarrierCharge, heatEnergyCharge, heatCarrierCharge + heatEnergyCharge);
		}

		public decimal CalculateElectricityCharge(decimal dayVolume, decimal nightVolume)
		{
			var dayTariff = _tariffProvider.GetTariff(ServiceType.ElectricityDay).Value;
			var nightTariff = _tariffProvider.GetTariff(ServiceType.ElectricityNight).Value;
			var day = Math.Round(dayVolume * dayTariff, 2, MidpointRounding.AwayFromZero);
			var night = Math.Round(nightVolume * nightTariff, 2, MidpointRounding.AwayFromZero);
			return day + night;
		}
	}
}
