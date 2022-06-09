using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductManagement.DataAccess.Data;
using ProductManagement.Models;

namespace ProductManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductTypesController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        public ProductTypesController(ApplicationDbContext db)
        {
            _db = db;
        }
        // Get the information of all the product types
        [HttpGet]
        public async Task<IActionResult> GetAll(int? PageSize, int? PageNumber)
        {
            var CurrPageSize = PageSize ?? 10;
            var CurrPageNumber = PageNumber ?? 1;
            var productTypes = await (from productType in _db.ProductTypes
                                      select new
                                      {
                                          Id = productType.Id,
                                          Name = productType.Name,
                                      }).ToListAsync();
            return Ok(productTypes.Skip(CurrPageSize * (CurrPageNumber - 1)).Take(CurrPageSize));
        }

        // Get the information of all the product types in ascending order (by price)
        [HttpGet("[action]")]
        public async Task<IActionResult> GetSortedAscending()
        {
            var productTypes = await (from productType in _db.ProductTypes
                                      select new
                                      {
                                          Id = productType.Id,
                                          Name = productType.Name,
                                      }).OrderBy(pt => pt.Name).ToListAsync();
            return Ok(productTypes);
        }

        // Get the information of all the product types in descending order (by price)
        [HttpGet("[action]")]
        public async Task<IActionResult> GetSortedDescending()
        {
            var productTypes = await (from productType in _db.ProductTypes
                                      select new
                                      {
                                          Id = productType.Id,
                                          Name = productType.Name,
                                      }).OrderByDescending(pt => pt.Name).ToListAsync();
            return Ok(productTypes);
        }

        // Get the detailed information of a single product type
        [HttpGet("[action]/{Id}")]
        public async Task<IActionResult> GetDetails(int? Id)
        {
            if (Id == null) return BadRequest();
            var Product = await _db.ProductTypes.Where(pt => pt.Id == Id).Include(pt => pt.products).ToListAsync();
            return Ok(Product);
        }

        // Create a new product type
        [HttpPost]
        public async Task<IActionResult> CreateNewProductType([FromForm] ProductType ProductType)
        {
            if (ProductType == null) return BadRequest();
            await _db.ProductTypes.AddAsync(ProductType);
            await _db.SaveChangesAsync();
            return StatusCode(StatusCodes.Status201Created);
        }

        // Update a product type given an Id
        [HttpPut("{Id}")]
        public async Task<IActionResult> UpdateProductTypeById(int? Id, [FromForm] ProductType ProductType)
        {
            if (Id == null) return BadRequest();
            ProductType.Id = (int)Id;
            _db.ProductTypes.Update(ProductType);
            await _db.SaveChangesAsync();
            return StatusCode(StatusCodes.Status202Accepted);
        }

        // Delete a product type by Id
        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteProductTypeById(int? Id)
        {
            if (Id == null) return BadRequest();
            var ProductType = await _db.ProductTypes.FirstOrDefaultAsync(x => x.Id == Id);
            _db.ProductTypes.Remove(ProductType);
            await _db.SaveChangesAsync();
            return StatusCode(StatusCodes.Status200OK);
        }

        // Search for product types whose names begin with string query
        [HttpGet("[action]")]
        public async Task<IActionResult> Search(string? query)
        {
            query = query ?? "";
            var ProductTypes = await (from productType in _db.ProductTypes
                                      select new
                                      {
                                          Id = productType.Id,
                                          Name = productType.Name,
                                      }).Where(pt => pt.Name.StartsWith(query)).OrderBy(pt => pt.Name).ToListAsync();
            return Ok(ProductTypes);
        }
    }
}
