using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Paylocity.Models;

namespace Paylocity.Service
{
    public class PaycheckCalc
    {
        public List<FamilyList> CalculateBenefitsCost(string employeeName, List<string> familyNames)
        {
            var famList = new List<FamilyList>();
            var employeeFirstInitial = employeeName.Substring(0, 1);

            var employeeCost = 0.0;
            if(employeeFirstInitial.ToUpper() == "A")
                employeeCost = 1000 * 0.1;
            else
                employeeCost = 1000;

            famList.Add(new FamilyList { Name = employeeName, Cost = employeeCost });

            foreach(string name in familyNames)
            {
                var dependentCost = 0.0;
                var dependentFirstInitial = name.Substring(0, 1);

                if (dependentFirstInitial.ToUpper() == "A")
                    dependentCost = 500 * 0.1;
                else
                    dependentCost = 500;

                famList.Add(new FamilyList { Name = name, Cost = dependentCost });
            }

            return famList;
        }

        public Double NetPay(List<FamilyList> familyList)
        {
            var totalCost = 0.0;
            foreach(FamilyList record in familyList)
            {
                totalCost = totalCost + record.Cost;
            }

            return totalCost;
        }

    }
}