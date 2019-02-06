using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Paylocity.Service;
using Paylocity.Models;

namespace Paylocity.Tests
{
    [TestFixture]
    public class PaycheckCalcTest
    {
        [Test]
        public void CalculateBenefitsCostTest()
        {
            var paycheckCalc = new PaycheckCalc();

            var familyList = new List<string>();
            familyList.Add("Al");
            familyList.Add("Judy");
            familyList.Add("Allison");

            var list = paycheckCalc.CalculateBenefitsCost("Alfred", familyList);

            Assert.AreEqual(500, list[2].Cost);
        }

        [Test]
        public void NetPayTest()
        {
            var familyList = new List<FamilyList>();
            familyList.Add(new FamilyList { Name = "Al", Cost = 900 });
            familyList.Add(new FamilyList { Name = "Ally", Cost = 450 });
            familyList.Add(new FamilyList { Name = "Judy", Cost = 500 });
            familyList.Add(new FamilyList { Name = "Allison", Cost = 450 });

            var paycheckCalc = new PaycheckCalc();
            var netPay = paycheckCalc.NetPay(familyList);

            Assert.AreEqual(1912, Convert.ToInt32(netPay));
        }
    }
}
