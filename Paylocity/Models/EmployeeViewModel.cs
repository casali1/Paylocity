using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Paylocity.Models
{
    public class EmployeeViewModel
    {
        public string EmployeeName { get; set; }
        public double BenefitCost { get; set; }

        public double NetPay { get; set; }
        public double Salary { get; set; }
        public double TotalBenefitCosts { get; set; }

        public double YearlySalary { get; set; }
        public double YearlyNetPay { get; set; }      
        public double YearlyTotalBenefitsCost { get; set; }       
    }
}