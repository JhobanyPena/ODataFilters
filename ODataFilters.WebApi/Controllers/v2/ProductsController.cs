using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ODataFilters.Model.Data;
using ODataFilters.Model.Models;

namespace ODataFilters.WebApi.Controllers.v2
{
    [ApiController]
    [Route("api/v2/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly NorthwindDbContext _context;

        public ProductsController(NorthwindDbContext context)
        {
            _context = context;
        }

        // GET: api/Products1
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return await _context.Products.ToListAsync();
        }

    }
}
