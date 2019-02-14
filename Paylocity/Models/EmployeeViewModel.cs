using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Paylocity.Models
{
    public class EmployeeViewModel
    {
        public string EmployeeName { get; set; }
        public double YearlyEmployeeBenefitCost { get; set; }

        public double WeeklySalary { get; set; }
        public double WeeklyTotalBenefitCosts { get; set; }
        public double WeeklyNetPay { get; set; }

        public double YearlySalary { get; set; }           
        public double YearlyTotalBenefitsCost { get; set; }
        public double YearlyNetPay { get; set; }
    }
}