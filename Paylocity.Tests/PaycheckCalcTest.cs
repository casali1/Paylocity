using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Paylocity.Service;
using Paylocity.Models;
using Paylocity.Context;

namespace Paylocity.Tests
{
    [TestFixture]
    public class PaycheckCalcTest
    {
        PaycheckCalc paycheckCalc = new PaycheckCalc();

        [Test]
        public void CalculateEmployeeBenefitsCostTest()
        {
            var context = new BenefitsContext();
            var paycheckCalc = new PaycheckCalc();

            paycheckCalc.CalculateEmployeeBenefitsCost(context, "Ali");

            var employeeRec = context.Employees.Where(e => e.EmployeeName == "Ali").SingleOrDefault();
            Assert.AreEqual(900, employeeRec.BenefitCost);
        }

        [Test]
        public void CalculateDependentBenefitsCostTest()
        {
            var context = new BenefitsContext();
            var paycheckCalc = new PaycheckCalc();

            context.Employees.Add(new Employee { EmployeeName = "Ali", BenefitCost = 900 });
            context.SaveChanges();

            var familyList = new List<string>();
            familyList.Add("Az");
            familyList.Add("JJ");
            paycheckCalc.CalculateDependentBenefitsCost(context, "Ali", familyList);

            var dependentRecs = context.Dependents.Where(e => e.Employee.EmployeeName == "Ali");
            var dependentsArray = dependentRecs.ToArray();
            Assert.AreEqual(450, dependentsArray[0].BenefitCost);
            Assert.AreEqual(500, dependentsArray[1].BenefitCost);
        }

        [Test]
        public void CalculateTotalBenefitsCostTest()
        {
            var context = new BenefitsContext();
            var paycheckCalc = new PaycheckCalc();

            context.Employees.Add(new Employee { EmployeeName = "Ali", BenefitCost = 900 });
            context.SaveChanges();
            var employeeRec = context.Employees.Where(e => e.EmployeeName == "Ali").SingleOrDefault();
            context.Dependents.Add(new Dependent { DependentName = "Az", BenefitCost = 450, Employee = employeeRec });
            context.Dependents.Add(new Dependent { DependentName = "JJ", BenefitCost = 500, Employee = employeeRec });
            context.SaveChanges();

            paycheckCalc.CalculateTotalBenefitsCost(context, "Ali");
            employeeRec = context.Employees.Where(e => e.EmployeeName == "Ali").SingleOrDefault();

            Assert.AreEqual(71, employeeRec.TotalBenefitCosts);
            Assert.AreEqual(1850, employeeRec.YearlyTotalBenefitsCost);
        }

        [Test]
        public void CalculateSalaryInfoTest()
        {
            var context = new BenefitsContext();
            var paycheckCalc = new PaycheckCalc();

            context.Employees.Add(new Employee { EmployeeName = "Ali", TotalBenefitCosts = 71 });
            context.SaveChanges();

            paycheckCalc.CalculateSalaryInfo(context, "Ali");
            var employeeRec = context.Employees.Where(e => e.EmployeeName == "Ali").SingleOrDefault();

            Assert.AreEqual(2000, employeeRec.Salary);
            Assert.AreEqual(2000 - 71, employeeRec.NetPay);
        }

        [Test]
        public void CalculateYearlySalaryInfo()
        {
            var context = new BenefitsContext();
            var paycheckCalc = new PaycheckCalc();

            context.Employees.Add(new Employee { EmployeeName = "Ali", YearlyTotalBenefitsCost = 1850 });
            context.SaveChanges();

            paycheckCalc.CalculateYearlySalaryInfo(context, "Ali");
            var employeeRec = context.Employees.Where(e => e.EmployeeName == "Ali").SingleOrDefault();

            Assert.AreEqual(52000, employeeRec.YearlySalary);
            Assert.AreEqual(52000 - 1850, employeeRec.YearlyNetPay);
        }

        [Test]
        public void Seed_Database()
        {
            using (BenefitsContext db = new BenefitsContext())
            {
                var employeeFamily = new Employee()
                {
                    EmployeeName = "Bob",
                    BenefitCost = 1000,
                    Dependents = new List<Dependent>
                    {
                        new Dependent { DependentName = "Judy", BenefitCost = 500 },
                        new Dependent { DependentName = "Alfred", BenefitCost = 450 },
                        new Dependent { DependentName = "Susan", BenefitCost = 500 }
                    }
                };

                // save data to db
                db.SaveChanges();
                db.Dispose();
            }
        }
    }
}

