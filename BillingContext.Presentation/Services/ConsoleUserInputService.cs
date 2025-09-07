using System;
using System.Globalization;
using ToSobesHz.BillingContext.Application.DTOs;
using ToSobesHz.BillingContext.Application.Handlers;
using ToSobesHz.BillingContext.Infrastructure.Repositories;

namespace ToSobesHz.BillingContext.Presentation.Services
{
	public class ConsoleUserInputService
	{
		private readonly CalculateBillingCommandHandler _handler;
		private readonly AccountRepository _repo = new();

		public ConsoleUserInputService(CalculateBillingCommandHandler handler)
		{
			_handler = handler;
		}

		public void Run()
		{
			while (true)
			{
				Console.WriteLine("Расчёт коммунальных услуг");
				var req = new CalculationRequest();
				req.Period = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
				Console.Write("Номер лицевого счёта: ");
				req.AccountNumber = Console.ReadLine() ?? string.Empty;

				Console.WriteLine("Количество проживающих может меняться в месяце. Введите два периода (или одинаковое значение для простого случая).");
				var daysInMonth = DateTime.DaysInMonth(req.Period.Year, req.Period.Month);
				var firstPeriodDays = ReadInt($"Дней по первому количеству (1..{daysInMonth}): ");
				if (firstPeriodDays < 1) firstPeriodDays = 1;
				if (firstPeriodDays > daysInMonth) firstPeriodDays = daysInMonth;
				var firstOccupants = ReadInt("Проживающих в этот период: ");
				var secondOccupants = ReadInt("Проживающих в оставшиеся дни: ");
				var secondPeriodDays = daysInMonth - firstPeriodDays;
				var equivalent = ((decimal)firstPeriodDays * firstOccupants + (decimal)secondPeriodDays * secondOccupants) / daysInMonth;
				req.OccupantsEquivalent = Math.Round(equivalent, 4, MidpointRounding.AwayFromZero);

				Console.WriteLine("Есть счётчик ХВС? (y/n)");
				req.HasColdWaterMeter = ReadYesNo();
				if (req.HasColdWaterMeter)
				{
					req.ColdWaterPrevious = ReadDecimal("ХВС пред. показание (если нет, введите 0): ");
					req.ColdWaterCurrent = ReadDecimal("ХВС текущее показание: ");
				}

				Console.WriteLine("Есть счётчик ГВС? (y/n)");
				req.HasHotWaterMeter = ReadYesNo();
				if (req.HasHotWaterMeter)
				{
					req.HotWaterPrevious = ReadDecimal("ГВС пред. показание (если нет, введите 0): ");
					req.HotWaterCurrent = ReadDecimal("ГВС текущее показание: ");
				}

				Console.WriteLine("Есть счётчик ЭЭ (двухтарифный)? (y/n)");
				req.HasElectricityMeter = ReadYesNo();
				if (req.HasElectricityMeter)
				{
					req.ElectricityDayPrevious = ReadDecimal("ЭЭ день пред.: ");
					req.ElectricityDayCurrent = ReadDecimal("ЭЭ день текущее: ");
					req.ElectricityNightPrevious = ReadDecimal("ЭЭ ночь пред.: ");
					req.ElectricityNightCurrent = ReadDecimal("ЭЭ ночь текущее: ");
				}

				var res = _handler.Handle(req);

				Console.WriteLine();
				Console.WriteLine("Результаты расчёта:");
				foreach (var s in res.Services)
				{
					Console.WriteLine($"- {s.ServiceType}: V={s.Volume} {s.Unit}, T={s.Tariff}, P={s.Charge}");
				}
				Console.WriteLine($"Итого к оплате: {res.Total} руб");

				Console.WriteLine();
				Console.WriteLine("Сохранить результат и перейти к следующему месяцу? (y = сохранить и продолжить, n = повторить ввод текущего месяца, q = выйти)");
				var key = Console.ReadKey(intercept: true);
				Console.WriteLine();
				if (key.KeyChar == 'q' || key.KeyChar == 'Q') break;
				if (key.KeyChar == 'y' || key.KeyChar == 'Y')
				{
					var accountId = _repo.UpsertAccountAsync(req.AccountNumber).GetAwaiter().GetResult();
					_repo.SaveCalculationAsync(accountId, req.Period, res.Services).GetAwaiter().GetResult();
					Console.WriteLine("Результат сохранён. Переход к следующему месяцу.\n");
					continue;
				}
			}
		}

		private static bool ReadYesNo()
		{
			while (true)
			{
				var key = Console.ReadKey(intercept: true);
				if (key.KeyChar == 'y' || key.KeyChar == 'Y') { Console.WriteLine("y"); return true; }
				if (key.KeyChar == 'n' || key.KeyChar == 'N') { Console.WriteLine("n"); return false; }
			}
		}

		private static int ReadInt(string prompt)
		{
			Console.Write(prompt);
			int parsedValue;
			while (!int.TryParse(Console.ReadLine(), out parsedValue) || parsedValue < 0)
			{
				Console.Write("Введите корректное число: ");
			}
			return parsedValue;
		}

		private static decimal ReadDecimal(string prompt)
		{
			Console.Write(prompt);
			decimal parsedValue;
			while (!decimal.TryParse(Console.ReadLine(), NumberStyles.Number, System.Globalization.CultureInfo.InvariantCulture, out parsedValue) || parsedValue < 0m)
			{
				Console.Write("Введите корректное неотрицательное число: ");
			}
			return parsedValue;
		}
	}
}
