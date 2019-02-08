using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Paylocity.Models;
using Paylocity.Context;

namespace Paylocity.Service
{
    public class PaycheckCalc
    {
        Double Salary = 2000;
        Double YearlyEmployBenefitCosts = 1000;
        Double YearlyDependBenefitCosts = 500;

        public List<FamilyList> CalculateBenefitsCost(BenefitsContext context, string employeeName, List<string> familyNames)
        {
            var famList = new List<FamilyList>();

            //Employee
            var employeeFirstInitial = employeeName.Substring(0, 1);

            var employeeCost = 0.0;
            if (employeeFirstInitial.ToUpper() == "A")
                employeeCost = YearlyEmployBenefitCosts - YearlyEmployBenefitCosts * 0.1;
            else
                employeeCost = YearlyEmployBenefitCosts;

            famList.Add(new FamilyList { Name = employeeName, Cost = employeeCost });

            context.Employees.Add(new Employee { EmployeeName = employeeName, BenefitCost = employeeCost });
            context.SaveChanges();

            //Dependents
            foreach (string name in familyNames)
            {
                var dependentCost = 0.0;
                var dependentFirstInitial = name.Substring(0, 1);

                if (dependentFirstInitial.ToUpper() == "A")
                    dependentCost = YearlyDependBenefitCosts - YearlyDependBenefitCosts * 0.1;
                else
                    dependentCost = YearlyDependBenefitCosts;

                famList.Add(new FamilyList { Name = name, Cost = dependentCost });

                var employeeRec = context.Employees.Where(e => e.EmployeeName == employeeName).SingleOrDefault();
                context.Dependents.Add(new Dependent { DependentName = name, BenefitCost = dependentCost, Employee = employeeRec });
                context.SaveChanges();
            }

            return famList;
        }

        public Double NetPay(BenefitsContext context, string employeeName)
        {
            var yearlyTotalBenefitsCost = 0.0;
            var netPay = 0.0;

            var employeeRec = context.Employees.Where(e => e.EmployeeName == employeeName).SingleOrDefault();
            if (employeeRec != null)
            {
                yearlyTotalBenefitsCost = employeeRec.BenefitCost;

                var dependentRecords = context.Dependents.Where(e => e.Employee.EmployeeId == employeeRec.EmployeeId);
                foreach (Dependent record in dependentRecords.ToList())
                {
                    yearlyTotalBenefitsCost = yearlyTotalBenefitsCost + record.BenefitCost;
                }

                var totalBenefitsCost = Convert.ToInt32(yearlyTotalBenefitsCost / 26);

                netPay = Salary - totalBenefitsCost;

                employeeRec.Salary = Salary;
                employeeRec.NetPay = netPay;
                employeeRec.YearlyTotalBenefitsCost = yearlyTotalBenefitsCost;
                employeeRec.TotalBenefitCosts = totalBenefitsCost;

                context.SaveChanges();
            }
            return netPay;
        }

        public Double YearlySalary(BenefitsContext context, string employeeName)
        {
            var yearlySalary = Salary * 26;

            var employeeRec = context.Employees.Where(e => e.EmployeeName == employeeName).SingleOrDefault();
            if (employeeRec != null)
            {
                employeeRec.YearlySalary = yearlySalary;
                employeeRec.YearlyNetPay = yearlySalary - employeeRec.YearlyTotalBenefitsCost;
                context.SaveChanges();
            }

            return yearlySalary;
        }
    }
}