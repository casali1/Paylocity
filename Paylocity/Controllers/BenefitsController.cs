using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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

        public List<string> GetAllProducts()
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
                BenefitCost = employeeRec.BenefitCost,

                Salary = employeeRec.Salary,
                NetPay = employeeRec.NetPay,
                TotalBenefitCosts = employeeRec.TotalBenefitCosts,

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

            paycheckCalc.CalculateBenefitsCost(context, employee, familyMembers);
            paycheckCalc.NetPay(context, employee);
            paycheckCalc.YearlySalary(context, employee);
            return Ok();
        }
    }
}
