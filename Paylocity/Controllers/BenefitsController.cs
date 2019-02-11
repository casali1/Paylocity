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
        public static int PageLoadFlag = 1;

        // GET: api/Product
        public List<Employee> GetAllProducts()
        {
            if (PageLoadFlag == 1)
            {
                var data = new PaycheckCalc();
                PageLoadFlag++;
                employees = data.GetAllEmployee(context);
                return employees;
            }

            return employees;
        }

        //// GET: api/Product/5
        //public IHttpActionResult GetEmployee(int id)
        //{
        //    var product = products.FirstOrDefault(x => x.ID == id);

        //    if (product == null)
        //        return NotFound();

        //    return Ok(product);
        //}
    }
}
