using System;
using System.Collections.Generic;
using ToSobesHz.BillingContext.Application.DTOs;
using ToSobesHz.BillingContext.Domain.Services.Interfaces;
using ToSobesHz.BillingContext.Domain.ValueObjects;
using ToSobesHz.BillingContext.Infrastructure.Services;

namespace ToSobesHz.BillingContext.Application.Handlers
{
	public class CalculateBillingCommandHandler
	{
		private readonly CalculationService _calculationService;
		private readonly ITariffProvider _tariffProvider;
		private readonly INormativeProvider _normativeProvider;

		public CalculateBillingCommandHandler(CalculationService calculationService, ITariffProvider tariffProvider, INormativeProvider normativeProvider)
		{
			_calculationService = calculationService;
			_tariffProvider = tariffProvider;
			_normativeProvider = normativeProvider;
		}

		public CalculationResult Handle(CalculationRequest request)
		{
			var result = new CalculationResult();

			// ХВС
			decimal coldVolume;
			if (request.HasColdWaterMeter)
			{
				coldVolume = CalculationService.CalculateMeteredVolume(request.ColdWaterCurrent ?? 0m, request.ColdWaterPrevious);
			}
			else
			{
				var norm = _normativeProvider.GetNormative(ServiceType.ColdWater).ValuePerPerson;
				coldVolume = norm * request.OccupantsEquivalent;
			}
			var coldTariff = _tariffProvider.GetTariff(ServiceType.ColdWater).Value;
			var coldCharge = Math.Round(coldVolume * coldTariff, 2, MidpointRounding.AwayFromZero);
			result.Services.Add(new ServiceConsumptionDto
			{
				ServiceType = ServiceType.ColdWater,
				Volume = coldVolume,
				Unit = "м^3",
				Tariff = coldTariff,
				Charge = coldCharge
			});

			// ГВС
			decimal hotVolume;
			if (request.HasHotWaterMeter)
			{
				hotVolume = CalculationService.CalculateMeteredVolume(request.HotWaterCurrent ?? 0m, request.HotWaterPrevious);
			}
			else
			{
				var norm = _normativeProvider.GetNormative(ServiceType.HotWaterHeatCarrier).ValuePerPerson;
				hotVolume = norm * request.OccupantsEquivalent;
			}
			var (gvsTnCharge, gvsTeCharge, gvsTotal) = _calculationService.CalculateHotWaterCharges(hotVolume);
			result.Services.Add(new ServiceConsumptionDto
			{
				ServiceType = ServiceType.HotWaterHeatCarrier,
				Volume = hotVolume,
				Unit = "м^3",
				Tariff = _tariffProvider.GetTariff(ServiceType.HotWaterHeatCarrier).Value,
				Charge = gvsTnCharge
			});
			var teNorm = _normativeProvider.GetNormative(ServiceType.HotWaterHeatEnergy).ValuePerPerson;
			result.Services.Add(new ServiceConsumptionDto
			{
				ServiceType = ServiceType.HotWaterHeatEnergy,
				Volume = hotVolume * teNorm,
				Unit = "Гкал",
				Tariff = _tariffProvider.GetTariff(ServiceType.HotWaterHeatEnergy).Value,
				Charge = gvsTeCharge
			});

			// Электроэнергия
			decimal eeDayVolume;
			decimal eeNightVolume;
			if (request.HasElectricityMeter)
			{
				eeDayVolume = CalculationService.CalculateMeteredVolume(request.ElectricityDayCurrent ?? 0m, request.ElectricityDayPrevious);
				eeNightVolume = CalculationService.CalculateMeteredVolume(request.ElectricityNightCurrent ?? 0m, request.ElectricityNightPrevious);
			}
			else
			{
				var norm = _normativeProvider.GetNormative(ServiceType.ElectricityDay).ValuePerPerson;
				var totalNorm = norm * request.OccupantsEquivalent;
				
				// Без счётчика: считаем всё как дневное
				eeDayVolume = totalNorm;
				eeNightVolume = 0m;
			}
			var eeCharge = _calculationService.CalculateElectricityCharge(eeDayVolume, eeNightVolume);
			result.Services.Add(new ServiceConsumptionDto
			{
				ServiceType = ServiceType.ElectricityDay,
				Volume = eeDayVolume,
				Unit = "кВт/ч",
				Tariff = _tariffProvider.GetTariff(ServiceType.ElectricityDay).Value,
				Charge = Math.Round(eeDayVolume * _tariffProvider.GetTariff(ServiceType.ElectricityDay).Value, 2, MidpointRounding.AwayFromZero)
			});
			result.Services.Add(new ServiceConsumptionDto
			{
				ServiceType = ServiceType.ElectricityNight,
				Volume = eeNightVolume,
				Unit = "кВт/ч",
				Tariff = _tariffProvider.GetTariff(ServiceType.ElectricityNight).Value,
				Charge = Math.Round(eeNightVolume * _tariffProvider.GetTariff(ServiceType.ElectricityNight).Value, 2, MidpointRounding.AwayFromZero)
			});

			// Итого
			decimal total = 0m;
			foreach (var s in result.Services)
			{
				total += s.Charge;
			}
			result.Total = total;
			return result;
		}
	}
}
