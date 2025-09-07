using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Норматив потребления

namespace ToSobesHz.BillingContext.Domain.ValueObjects
{
    public record Normative(decimal ValuePerPerson, Amount Unit)
    {
        public override string ToString() => $"{ValuePerPerson} {Unit.Unit}";
    }
}
