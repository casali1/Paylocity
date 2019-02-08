using System;
using System.Data.Entity;
using Paylocity.Models;
using System.Collections.Generic;

namespace Paylocity.Context
{
    public class BenefitsContext : DbContext
    {
        static BenefitsContext()
        {
            // Not initialize database
            //  Database.SetInitializer<ProjectDatabase>(null);
            // Database initialize

            Database.SetInitializer<BenefitsContext>(new DbInitializer());

            using (BenefitsContext db = new BenefitsContext())
                db.Database.Initialize(false);
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Dependent> Dependents { get; set; }
    }

    class DbInitializer : DropCreateDatabaseAlways<BenefitsContext>
    {
        protected override void Seed(BenefitsContext context)
        {
            var employ = new List<Employee> { new Employee { EmployeeName = "Bob", BenefitCost = 1000, Dependents = new List<Dependent>() } };
            employ.ForEach(e => context.Employees.Add(e));
            context.SaveChanges();

            var depend = new List<Dependent>
            {
            new Dependent { DependentName = "Judy", BenefitCost = 500, Employee = employ.Find(e => e.EmployeeId == 1) },
            new Dependent { DependentName = "Alfred", BenefitCost = 450 , Employee = employ.Find(e => e.EmployeeId == 1) },
            new Dependent { DependentName = "Susan", BenefitCost = 500 , Employee = employ.Find(e => e.EmployeeId == 1) },
            };
            depend.ForEach(d => context.Dependents.Add(d));
            context.SaveChanges();

            employ[0].Dependents.Add(depend[0]);
            employ[0].Dependents.Add(depend[1]);
            employ[0].Dependents.Add(depend[2]);
            context.SaveChanges();

            base.Seed(context);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}