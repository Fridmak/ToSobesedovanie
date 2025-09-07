using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// За какой период делать расчет

namespace ToSobesHz.BillingContext.Domain.ValueObjects
{
    public class OccupancyPeriod
    {
        public int Occupants { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }

        private OccupancyPeriod() { }

        public OccupancyPeriod(int occupants, DateTime startDate, DateTime endDate)
        {
            if (occupants <= 0) throw new ArgumentException("Количество проживающих должно быть больше 0");
            if (startDate > endDate) throw new ArgumentException("Дата начала не может быть позже даты окончания");

            Occupants = occupants;
            StartDate = startDate;
            EndDate = endDate;
        }

        public bool Contains(DateTime date) => date >= StartDate && date <= EndDate;
    }
}
