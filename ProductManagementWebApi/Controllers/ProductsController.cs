using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductManagement.DataAccess.Data;
using ProductManagement.Helpers.Helpers.Request;
using ProductManagement.Models;
using ProductManagement.Repository.IRepository;
using ProductManagementWebApi.Helpers.Common;
using ProductManagementWebApi.Helpers.Request;

namespace ProductManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProductsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // Get the information of all the products
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] GetRequest req)
        {
            return Ok(_unitOfWork.Product.GetAll(req));
        }

        // Get the detailed information of a single product
        [HttpGet("[action]/{Id}")]
        public async Task<IActionResult> GetDetails(int? Id)
        {
            if (Id == null) return BadRequest();
            var Product = _unitOfWork.Product.GetFirstOrDefault(p => p.Id == Id);
            return Ok(Product);
        }

        // Create a new product
        [HttpPost]
        public async Task<IActionResult> CreateNewProduct([FromForm] ProductPostRequest productPostRequest)
        {
            if (productPostRequest == null) return BadRequest();
            await _unitOfWork.Product.Add(productPostRequest);
            await _unitOfWork.SaveAsync();
            return StatusCode(StatusCodes.Status201Created);
        }

        // Update a product given an Id
        [HttpPut("{Id}")]
        public async Task<IActionResult> UpdateProductById(int? Id, [FromForm] ProductPostRequest productPostRequest)
        {
            if (Id == null || productPostRequest == null) return BadRequest();
            _unitOfWork.Product.Update((int)Id, productPostRequest);
            await _unitOfWork.SaveAsync();
            return StatusCode(StatusCodes.Status202Accepted);
        }

        // Delete a product by Id
        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteProductById(int? Id)
        {
            if (Id == null) return BadRequest();
            Product product = _unitOfWork.Product.GetFirstOrDefault(p => p.Id == Id);
            _unitOfWork.Product.Remove(product);
            await _unitOfWork.SaveAsync();
            return StatusCode(StatusCodes.Status200OK);
        }

    }
}
