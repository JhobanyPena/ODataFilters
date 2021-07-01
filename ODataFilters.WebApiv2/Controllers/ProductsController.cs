using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using ODataFilters.Model.Data;

namespace ODataFilters.WebApiv2.Controllers
{
    public class ProductsController : ODataController
    {
        private readonly NorthwindDbContext context;

        public ProductsController(NorthwindDbContext context)
        {
            this.context = context;
        }

        [EnableQuery]
        public IActionResult Get() => Ok(context.Products);
    }
}
