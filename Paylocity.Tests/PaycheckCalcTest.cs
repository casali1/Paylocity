using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public void CalculateBenefitsCostTest()
        {
            var context = new BenefitsContext();
                  
            var familyList = new List<string>();
            familyList.Add("Az");
            familyList.Add("Ju");
            familyList.Add("Ay");

            var list = paycheckCalc.CalculateBenefitsCost(context, "Al", familyList);
            Assert.AreEqual(500, list[2].Cost);

            var netPay = paycheckCalc.NetPay(context, "Al");
            Assert.AreEqual(1912, netPay);

            var yearlySalary = paycheckCalc.YearlySalary(context, "Al");
            Assert.AreEqual(52000, Convert.ToInt32(yearlySalary));
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

