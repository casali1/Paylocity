using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Paylocity.Models;
using Paylocity.Service;
using Paylocity.Context;
using System.Web.Http.Cors;

namespace Paylocity.Controllers
{
    [EnableCors("*", "*", "*")]
    public class BenefitsController : ApiController
    {
        public static List<Employee> employees = new List<Employee>();
        public static BenefitsContext context = new BenefitsContext();
        PaycheckCalc paycheckCalc = new PaycheckCalc();

        public List<string> GetAllEmployees()
        {
            var employeeList = new List<string>();

            employees = paycheckCalc.GetAllEmployee(context);

            foreach (Employee employee in employees)
            {
                employeeList.Add(employee.EmployeeName);
            }

            return employeeList;
        }

        public EmployeeViewModel GetEmployee(int id)
        {
            var employeeRec = context.Employees.Where(e => e.EmployeeId == id).SingleOrDefault();

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

            paycheckCalc.CalculateEmployeeBenefitsCost(context, employee);


            paycheckCalc.CalculateDependentBenefitsCost(context, employee, familyMembers);

            paycheckCalc.CalculateTotalBenefitsCost(context, employee);

            paycheckCalc.CalculateSalaryInfo(context, employee);

            paycheckCalc.CalculateYearlySalaryInfo(context, employee);
            return Ok();
        }
    }
}
