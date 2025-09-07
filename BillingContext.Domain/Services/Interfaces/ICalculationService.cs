using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToSobesHz.BillingContext.Domain.Entities;
using ToSobesHz.BillingContext.Domain.ValueObjects;

// Расчетный сервис, реализация в BillingContext.Infrastructure/Services/...

namespace ToSobesHz.BillingContext.Domain.Services.Interfaces
{
    public interface ICalculationService
    {
        decimal CalculateCharge(Account account, ServiceType serviceType, bool hasMeter, DateTime calculationDate);
    }
}
