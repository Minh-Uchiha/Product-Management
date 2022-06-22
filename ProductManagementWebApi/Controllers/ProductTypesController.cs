using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductManagement.DataAccess.Data;
using ProductManagement.Helpers.Helpers.Request;
using ProductManagement.Models;
using ProductManagement.Repository.IRepository;
using ProductManagementWebApi.Helpers.Request;

namespace ProductManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductTypesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProductTypesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        // Get the information of all the product types
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] GetRequest req)
        {
            return Ok(_unitOfWork.ProductType.GetAll(req));
        }
    
        // Get the detailed information of a single product type
        [HttpGet("{Id}")]
        public async Task<IActionResult> GetDetails(int? Id)
        {
            if (Id == null) return BadRequest();
            return Ok(_unitOfWork.ProductType.GetDetails((int)Id));
        }

        // Create a new product type
        [HttpPost]
        public async Task<IActionResult> CreateNewProductType([FromForm] ProductTypePostRequest ProductType)
        {
            if (ProductType == null) return BadRequest();
            await _unitOfWork.ProductType.Add(ProductType);
            await _unitOfWork.SaveAsync();
            return StatusCode(StatusCodes.Status201Created);
        }

        // Update a product type given an Id
        [HttpPut("{Id}")]
        public async Task<IActionResult> UpdateProductTypeById(int? Id, [FromForm] ProductTypePostRequest ProductType)
        {
            if (Id == null) return BadRequest();
            _unitOfWork.ProductType.Update((int)Id, ProductType);
            await _unitOfWork.SaveAsync();
            return StatusCode(StatusCodes.Status202Accepted);
        }

        // Delete a product type by Id
        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteProductTypeById(int? Id)
        {
            if (Id == null) return BadRequest();
            ProductType productType = _unitOfWork.ProductType.GetFirstOrDefault(pt => pt.Id == Id);
            _unitOfWork.ProductType.Remove(productType);
            await _unitOfWork.SaveAsync();
            return StatusCode(StatusCodes.Status200OK);
        }

    }
}
