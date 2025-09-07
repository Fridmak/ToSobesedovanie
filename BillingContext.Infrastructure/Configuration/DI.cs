using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToSobesHz.BillingContext.Application.Handlers;
using ToSobesHz.BillingContext.Infrastructure.Services;
using ToSobesHz.BillingContext.Presentation.Services;

namespace ToSobesHz.BillingContext.Infrastructure.Configuration
{
	public static class DI
	{
		public static ConsoleUserInputService BuildConsoleApp()
		{
			var tariffProvider = new TariffProvider();
			var normativeProvider = new NormativeProvider();
			var calcService = new CalculationService(tariffProvider, normativeProvider);
			var handler = new CalculateBillingCommandHandler(calcService, tariffProvider, normativeProvider);
			return new ConsoleUserInputService(handler);
		}
	}
}
