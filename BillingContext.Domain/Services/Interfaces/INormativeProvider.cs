using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToSobesHz.BillingContext.Domain.ValueObjects;

// Получение нормативов

namespace ToSobesHz.BillingContext.Domain.Services.Interfaces
{
    public interface INormativeProvider
    {
        Normative GetNormative(ServiceType serviceType);
    }
}
