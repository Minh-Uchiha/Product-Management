using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductManagement.DataAccess.Data;
using ProductManagement.Models;
using ProductManagementWebApi.Helpers;

namespace ProductManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        public ProductsController(ApplicationDbContext db)
        {
            _db = db;
        }

        // Get the information of all the products
        [HttpGet]
        public async Task<IActionResult> GetAll(int? PageSize, int? PageNumber)
        {
            var CurrPageSize = PageSize ?? 10;
            var CurrPageNumber = PageNumber ?? 1;
            var products = await (from product in _db.Products
                                  select new
                                  {
                                      product.Id,
                                      product.Name,
                                      product.Price,
                                      Image = product.ImageUrl
                                  }).ToListAsync();
            return Ok(products.Skip(CurrPageSize * (CurrPageNumber - 1)).Take(CurrPageSize));
        }

        // Get the information of all the products in ascending order (by price)
        [HttpGet("[action]")]
        public async Task<IActionResult> GetSortedAscending()
        {
            var products = await (from product in _db.Products
                                  select new
                                  {
                                      product.Id,
                                      product.Name,
                                      product.Price,
                                      Image = product.ImageUrl
                                  }).OrderBy(p => p.Price).ToListAsync();
            return Ok(products);
        }

        // Get the information of all the products in descending order (by price)
        [HttpGet("[action]")]
        public async Task<IActionResult> GetSortedDescending()
        {
            var products = await (from product in _db.Products
                                  select new
                                  {
                                      product.Id,
                                      product.Name,
                                      product.Price,
                                      Image = product.ImageUrl
                                  }).OrderByDescending(p => p.Price).ToListAsync();
            return Ok(products);
        }

        // Get the detailed information of a single product
        [HttpGet("[action]/{Id}")]
        public async Task<IActionResult> GetDetails(int? Id)
        {
            if (Id == null) return BadRequest();
            var Product = await _db.Products.FirstOrDefaultAsync(x => x.Id == Id);
            return Ok(Product);
        }

        // Create a new product
        [HttpPost]
        public async Task<IActionResult> CreateNewProduct([FromForm] Product product)
        {
            if (product == null) return BadRequest();
            product.ImageUrl = await FileHelper.UploadProductImage(product.Image);
            await _db.Products.AddAsync(product);
            await _db.SaveChangesAsync();
            return StatusCode(StatusCodes.Status201Created);
        }

        // Update a product given an Id
        [HttpPut("{Id}")]
        public async Task<IActionResult> UpdateProductById(int? Id, [FromForm] Product product)
        {
            if (Id == null) return BadRequest();
            product.Id = (int)Id;
            _db.Products.Update(product);
            await _db.SaveChangesAsync();
            return StatusCode(StatusCodes.Status202Accepted);
        }

        // Delete a product by Id
        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteProductById(int? Id)
        {
            if (Id == null) return BadRequest();
            var product = await _db.Products.FirstOrDefaultAsync(x => x.Id == Id);
            _db.Products.Remove(product);
            await _db.SaveChangesAsync();
            return StatusCode(StatusCodes.Status200OK);
        }

        // Search for products whose names begin with string query
        [HttpGet("[action]")]
        public async Task<IActionResult> Search(string? query)
        {
            if (query == null) query = "";
            var products = await (from product in _db.Products
                                  where product.Name.StartsWith(query)
                                  select new
                                  {
                                      product.Id,
                                      product.Name,
                                      product.Price,
                                      Image = product.ImageUrl
                                  }).ToListAsync();
            return Ok(products);
        }

    }
}
