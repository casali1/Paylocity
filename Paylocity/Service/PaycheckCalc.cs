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

        public List<Employee> GetAllEmployee(BenefitsContext context)
        {
            //List<Employee>();
            var employees = context.Employees;

            return employees.ToList();
        }

        public void CalculateEmployeeBenefitsCost(BenefitsContext context, string employeeName)
        {
            //Employee
            var employeeFirstInitial = employeeName.Substring(0, 1);

            var employeeCost = 0.0;
            if (employeeFirstInitial.ToUpper() == "A")
                employeeCost = YearlyEmployBenefitCosts - YearlyEmployBenefitCosts * 0.1;
            else
                employeeCost = YearlyEmployBenefitCosts;

            context.Employees.Add(new Employee { EmployeeName = employeeName, BenefitCost = employeeCost });
            context.SaveChanges();
        }

        public void CalculateDependentBenefitsCost(BenefitsContext context, string employeeName, List<string> familyNames)
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

                var employeeRec = context.Employees.Where(e => e.EmployeeName == employeeName).SingleOrDefault();
                context.Dependents.Add(new Dependent { DependentName = name, BenefitCost = dependentCost, Employee = employeeRec });
                context.SaveChanges();
            }
        }

        public void CalculateTotalBenefitsCost(BenefitsContext context, string employeeName)
        {
            var yearlyTotalBenefitsCost = 0.0;

            var employeeRec = context.Employees.Where(e => e.EmployeeName == employeeName).SingleOrDefault();
            if (employeeRec != null)
            {
                yearlyTotalBenefitsCost = employeeRec.BenefitCost;  //Initializing cost...starting with the employee benefit's cost.

                var dependentRecords = context.Dependents.Where(e => e.Employee.EmployeeId == employeeRec.EmployeeId);
                foreach (Dependent record in dependentRecords.ToList())
                {
                    yearlyTotalBenefitsCost = yearlyTotalBenefitsCost + record.BenefitCost;
                }

                var totalBenefitsCost = Convert.ToInt32(yearlyTotalBenefitsCost / 26);

                employeeRec.TotalBenefitCosts = totalBenefitsCost;
                employeeRec.YearlyTotalBenefitsCost = yearlyTotalBenefitsCost;

                context.SaveChanges();
            }
        }

        public void CalculateSalaryInfo(BenefitsContext context, string employeeName)
        {
            var employeeRec = context.Employees.Where(e => e.EmployeeName == employeeName).SingleOrDefault();
            if (employeeRec != null)
            {
                employeeRec.Salary = Salary;
                employeeRec.NetPay = Salary - employeeRec.TotalBenefitCosts;

                context.SaveChanges();
            }
        }

        public void CalculateYearlySalaryInfo(BenefitsContext context, string employeeName)
        {
            var yearlySalary = Salary * 26;

            var employeeRec = context.Employees.Where(e => e.EmployeeName == employeeName).SingleOrDefault();
            if (employeeRec != null)
            {
                employeeRec.YearlySalary = yearlySalary;
                employeeRec.YearlyNetPay = yearlySalary - employeeRec.YearlyTotalBenefitsCost;
                context.SaveChanges();
            }
        }

        //public List<Employee> GetAllEmployee()
        //{
        //    //List<Employee>();
        //    return paycheckData.GetEmployees();
        //}

        //public void CalculateEmployeeBenefitsCost(string employeeName)
        //{
        //    //Employee
        //    var employeeFirstInitial = employeeName.Substring(0, 1);

        //    var employeeCost = 0.0;
        //    if (employeeFirstInitial.ToUpper() == "A")
        //        employeeCost = YearlyEmployBenefitCosts - YearlyEmployBenefitCosts * 0.1;
        //    else
        //        employeeCost = YearlyEmployBenefitCosts;

        //    paycheckData.SaveEmployee(employeeName, employeeCost);
        //}

        //public void CalculateDependentBenefitsCost(string employeeName, List<string> familyNames)
        //{
        //    //Dependents
        //    foreach (string name in familyNames)
        //    {
        //        var dependentCost = 0.0;
        //        var dependentFirstInitial = name.Substring(0, 1);

        //        if (dependentFirstInitial.ToUpper() == "A")
        //            dependentCost = YearlyDependBenefitCosts - YearlyDependBenefitCosts * 0.1;
        //        else
        //            dependentCost = YearlyDependBenefitCosts;

        //        paycheckData.SaveDependent(employeeName, name, dependentCost);
        //    }
        //}

        //public void CalculateTotalBenefitsCost(string employeeName)
        //{
        //    var yearlyTotalBenefitsCost = 0.0;

        //    var employeeRec = paycheckData.GetEmployee(employeeName);
        //    if (employeeRec != null)
        //    {
        //        yearlyTotalBenefitsCost = employeeRec.BenefitCost;  //Initializing cost...starting with the employee benefit's cost.

        //        var dependentRecords = paycheckData.GetDependents(employeeName);
        //        foreach (Dependent record in dependentRecords)
        //        {
        //            yearlyTotalBenefitsCost = yearlyTotalBenefitsCost + record.BenefitCost;
        //        }

        //        var totalBenefitsCost = Convert.ToInt32(yearlyTotalBenefitsCost / 26);

        //        paycheckData.SaveTotalBenefitsCost(employeeName, totalBenefitsCost, yearlyTotalBenefitsCost);
        //    }
        //}

        //public void CalculateSalaryInfo(string employeeName)
        //{
        //    var netPay = Salary - paycheckData.GetTotalBenefitCosts(employeeName);

        //    paycheckData.SaveSalaryInfo(employeeName, Salary, netPay);
        //}

        //public void CalculateYearlySalaryInfo(BenefitsContext context, string employeeName)
        //{
        //    var yearlySalary = Salary * 26;

        //    var yearlyNetPay = yearlySalary - paycheckData.GetYearlyTotalBenefitsCost(employeeName);
        //    paycheckData.SaveYearlySalaryInfo(employeeName, yearlySalary, yearlyNetPay);
        //}
    }
}