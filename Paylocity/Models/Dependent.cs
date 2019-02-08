using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Paylocity.Models
{
    public class Dependent
    {
        public int DependentId { get; set; }
        public string DependentName { get; set; }
        public double BenefitCost { get; set; }

        public Employee Employee { get; set; }
    }
}