using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNetCore.Mvc;
using ODataFilters.Model.Data;
using ODataFilters.Model.Models;
using System.Collections.Generic;
using System.Linq;

namespace ODataFilters.WebApi.Controllers.v2
{
    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly NorthwindDbContext _context;

        public ProductsController(NorthwindDbContext context)
        {
            _context = context;
        }

        // GET: api/Products
        //[HttpGet]
        //[EnableQuery(PageSize = 10)]
        //public ActionResult<IEnumerable<Product>> GetProducts()
        //{
        //    return _context.Products;
        //}

        // Se usa OData sin el Encabezado "EnableQuery"
        [HttpGet]
        public IActionResult GetProducts(ODataQueryOptions<Product> ops)
        {
            var query = _context.Products;

            var items = ops.ApplyTo(query, new ODataQuerySettings() { PageSize = 10 });
            var count = ops.Count?.GetEntityCount(ops.Filter?.ApplyTo(query, new ODataQuerySettings()) ?? query);

            return Ok(new
            {
                Count = count,
                Items = items
            });
        }
    }
}
