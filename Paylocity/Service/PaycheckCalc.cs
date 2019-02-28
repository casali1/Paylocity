using System;
using System.Collections.Generic;
using System.Linq;
using Paylocity.Models;
using Paylocity.Context;
using Paylocity.DataLayer;

namespace Paylocity.Service
{
    public class PaycheckCalc
    {
        double Salary = 2000;
        double YearlyEmployBenefitCosts = 1000;
        double YearlyDependBenefitCosts = 500;
        PaycheckData paycheckData = new PaycheckData();

        public List<Employee> GetAllEmployee()
        {
            //List<Employee>();
            return paycheckData.GetEmployees();
        }

        public Employee GetEmployeeById(int ID)
        {
            return paycheckData.GetEmployeeByID(ID);
        }

        public void CalculateEmployeeBenefitsCost(string employeeName)
        {
            //Employee
            var employeeFirstInitial = employeeName.Substring(0, 1);

            var employeeCost = 0.0;
            if (employeeFirstInitial.ToUpper() == "A")
                employeeCost = YearlyEmployBenefitCosts - YearlyEmployBenefitCosts * 0.1;
            else
                employeeCost = YearlyEmployBenefitCosts;

            paycheckData.SaveEmployee(employeeName, employeeCost);
        }

        public void CalculateDependentBenefitsCost(string employeeName, List<string> familyNames)
        {
            //Dependents
            foreach (string name in familyNames)
            {
                var dependentCost = 0.0;
                var dependentFirstInitial = name.Substring(0, 1);

                if (dependentFirstInitial.ToUpper() == "A")
                    dependentCost = YearlyDependBenefitCosts - YearlyDependBenefitCosts * 0.1;
                else
                    dependentCost = YearlyDependBenefitCosts;

                paycheckData.SaveDependent(employeeName, name, dependentCost);
            }
        }

        public void CalculateTotalBenefitsCost(string employeeName)
        {
            var yearlyTotalBenefitsCost = 0.0;

            var employeeRec = paycheckData.GetEmployee(employeeName);
            if (employeeRec != null)
            {
                yearlyTotalBenefitsCost = employeeRec.BenefitCost;  //Initializing cost...starting with the employee benefit's cost.

                var dependentRecords = paycheckData.GetDependents(employeeName);
                foreach (Dependent record in dependentRecords)
                {
                    yearlyTotalBenefitsCost = yearlyTotalBenefitsCost + record.BenefitCost;
                }

                var totalBenefitsCost = Convert.ToInt32(yearlyTotalBenefitsCost / 26);

                paycheckData.SaveTotalBenefitsCost(employeeName, totalBenefitsCost, yearlyTotalBenefitsCost);
            }
        }

        public void CalculateSalaryInfo(string employeeName)
        {
            var netPay = Salary - paycheckData.GetTotalBenefitCosts(employeeName);

            paycheckData.SaveSalaryInfo(employeeName, Salary, netPay);
        }

        public void CalculateYearlySalaryInfo(string employeeName)
        {
            var yearlySalary = Salary * 26;

            var yearlyNetPay = yearlySalary - paycheckData.GetYearlyTotalBenefitsCost(employeeName);
            paycheckData.SaveYearlySalaryInfo(employeeName, yearlySalary, yearlyNetPay);
        }
    }
}