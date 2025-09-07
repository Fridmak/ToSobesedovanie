using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToSobesHz.BillingContext.Domain.ValueObjects;

// Получение тарифов к услуге

namespace ToSobesHz.BillingContext.Domain.Services.Interfaces
{
    public interface ITariffProvider
    {
        Tariff GetTariff(ServiceType serviceType);
    }
}
