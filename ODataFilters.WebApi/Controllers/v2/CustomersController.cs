using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using ODataFilters.Model.Data;
using ODataFilters.Model.Models;
using System.Collections.Generic;

namespace ODataFilters.WebApi.Controllers.v2
{
    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly NorthwindDbContext context;

        public CustomersController(NorthwindDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        [EnableQuery(PageSize = 10)]
        public ActionResult<IEnumerable<Customer>> Get() {
            return context.Customers; 
        }
    }
}
