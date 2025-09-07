using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Сумма за услугу с единицами измерения

namespace ToSobesHz.BillingContext.Domain.ValueObjects
{
    public record Amount(decimal Value, string Unit)
    {
        public static readonly Amount CubicMeters = new(1, "м^3");
        public static readonly Amount KilowattHours = new(1, "кВт/ч");
        public static readonly Amount Gigacalories = new(1, "Гкал");

        public override string ToString() => $"{Value} {Unit}";
    }
}
