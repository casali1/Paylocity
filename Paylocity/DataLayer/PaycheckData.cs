using System.Collections.Generic;
using System.Linq;
using Paylocity.Models;
using Paylocity.Context;
using Paylocity.Service;

namespace Paylocity.DataLayer
{
    public class PaycheckData
    {
        public static List<Employee> employees = new List<Employee>();
        public static BenefitsContext context = new BenefitsContext();
        PaycheckCalc paycheckCalc = new PaycheckCalc();

        public Employee GetEmployee(string employeeName)
        {
            return context.Employees.Where(e => e.EmployeeName == employeeName.ToUpper()).SingleOrDefault();
        }

        public List<Employee> GetEmployees()
        {
            return context.Employees.ToList();
        }

        public List<Dependent> GetDependents(string employeeName)
        {
            var employeeRec = GetEmployee(employeeName);
            return context.Dependents.Where(e => e.Employee.EmployeeId == employeeRec.EmployeeId).ToList();
        }

        public void SaveEmployee(string employeeName, double employeeCost)
        {
            context.Employees.Add(new Employee { EmployeeName = employeeName.ToUpper(), BenefitCost = employeeCost });
            context.SaveChanges();
        }

        public void SaveDependent(string employeeName, string dependentName, double dependentCost)
        {
            var employeeRec = GetEmployee(employeeName);
            context.Dependents.Add(new Dependent { DependentName = dependentName.ToUpper(), BenefitCost = dependentCost, Employee = employeeRec });
            context.SaveChanges();
        }

        public void SaveTotalBenefitsCost(string employeeName, double totalBenefitsCost, double yearlyTotalBenefitsCost)
        {
            var employeeRec = GetEmployee(employeeName);

            if (employeeRec != null)
            {
                employeeRec.TotalBenefitCosts = totalBenefitsCost;
                employeeRec.YearlyTotalBenefitsCost = yearlyTotalBenefitsCost;

                context.SaveChanges();
            }
        }

        public void SaveSalaryInfo(string employeeName, double salary, double netPay)
        {
            var employeeRec = GetEmployee(employeeName);

            if (employeeRec != null)
            {
                employeeRec.Salary = salary;
                employeeRec.NetPay = netPay;

                context.SaveChanges();
            }
        }

        public void SaveYearlySalaryInfo(string employeeName, double yearlySalary, double yearlyNetPay)
        {
            var employeeRec = GetEmployee(employeeName);
            if (employeeRec != null)
            {
                employeeRec.YearlySalary = yearlySalary;
                employeeRec.YearlyNetPay = yearlyNetPay;
                context.SaveChanges();
            }
        }

        public double GetTotalBenefitCosts(string employeeName)
        {
            var employeeRec = GetEmployee(employeeName);
            return employeeRec.TotalBenefitCosts;
        }

        public double GetYearlyTotalBenefitsCost(string employeeName)
        {
            var employeeRec = GetEmployee(employeeName);
            return employeeRec.YearlyTotalBenefitsCost;
        }
    }
}