using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Тариф к услуге

namespace ToSobesHz.BillingContext.Domain.ValueObjects
{
    public record Tariff(decimal Value, string Currency = "руб")
    {
        public override string ToString() => $"{Value} {Currency}";
    }
}
