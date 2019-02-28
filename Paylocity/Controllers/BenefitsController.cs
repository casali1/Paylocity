using System.Collections.Generic;
using System.Web.Http;
using Paylocity.Models;
using Paylocity.Service;
using System.Web.Http.Cors;

namespace Paylocity.Controllers
{
    [EnableCors("*", "*", "*")]
    public class BenefitsController : ApiController
    {
        public static List<Employee> employees = new List<Employee>();
        PaycheckCalc paycheckCalc = new PaycheckCalc();

        public List<string> GetAllEmployees()
        {
            var employeeList = new List<string>();

            employees = paycheckCalc.GetAllEmployee();

            foreach (Employee employee in employees)
            {
                employeeList.Add(employee.EmployeeName);
            }

            return employeeList;
        }

        public EmployeeViewModel GetEmployee(int ID)
        {
            var employeeRec = paycheckCalc.GetEmployeeById(ID);

            var employee = new EmployeeViewModel
            {
                EmployeeName = employeeRec.EmployeeName,
                YearlyEmployeeBenefitCost = employeeRec.BenefitCost,

                WeeklySalary = employeeRec.Salary,              
                WeeklyTotalBenefitCosts = employeeRec.TotalBenefitCosts,
                WeeklyNetPay = employeeRec.NetPay,

                YearlySalary = employeeRec.YearlySalary,
                YearlyTotalBenefitsCost = employeeRec.YearlyTotalBenefitsCost,
                YearlyNetPay = employeeRec.YearlyNetPay
            };

            return employee;
        }

        //public IHttpActionResult Put(Employee employee)
        //{
        //    return Ok();
        //}

        public IHttpActionResult Post(List<string> familyMembers)
        {
            var employee = familyMembers[0];
            familyMembers.RemoveAt(0);

            paycheckCalc.CalculateEmployeeBenefitsCost(employee);


            paycheckCalc.CalculateDependentBenefitsCost(employee, familyMembers);

            paycheckCalc.CalculateTotalBenefitsCost(employee);

            paycheckCalc.CalculateSalaryInfo(employee);

            paycheckCalc.CalculateYearlySalaryInfo(employee);
            return Ok();
        }
    }
}
