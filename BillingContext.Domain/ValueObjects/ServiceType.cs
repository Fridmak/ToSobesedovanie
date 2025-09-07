using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Тип предоставляемой услуги

namespace ToSobesHz.BillingContext.Domain.ValueObjects
{
    public enum ServiceType
    {
        ColdWater,              // ХВС
        HotWaterHeatCarrier,    // ГВС Теплоноситель
        HotWaterHeatEnergy,     // ГВС Тепловая энергия
        ElectricityDay,         // ЭЭ день
        ElectricityNight        // ЭЭ ночь
    }
}
