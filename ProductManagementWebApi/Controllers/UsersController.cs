using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductManagement.DataAccess.Data;
using ProductManagement.Models;
using ProductManagement.Repository.IRepository;
using ProductManagementWebApi.Helpers.Common;
using ProductManagementWebApi.Helpers.Interfaces;
using ProductManagementWebApi.Helpers.Request;

namespace ProductManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public UsersController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // Get simplified information of all users
        // Supports paging, sorting, searching (By name, by status, etc)
        [HttpGet]
        public async Task<IActionResult> GetAllUsers([FromQuery] GetRequest req)
        {
            if (req == null) return BadRequest();
            return Ok(_unitOfWork.User.GetAll(req));
        }

        // Sign in (Get detailed information about a user with a specific id)
        [HttpGet("{Id}")]
        public async Task<IActionResult> SignIn(int? Id)
        {
            if (Id == null) return BadRequest();
            var user = _unitOfWork.User.GetFirstOrDefault(u => u.Id == Id);
            return Ok(user);
        }

        // Sign Up (Create a new User)
        [HttpPost]
        public async Task<IActionResult> SignUp([FromForm] UserPostRequest userPostRequest)
        {
            if (userPostRequest == null) return BadRequest();
            await _unitOfWork.User.Add(userPostRequest);
            await _unitOfWork.SaveAsync();
            return StatusCode(StatusCodes.Status201Created);
        }

        // Update a user information 
        [HttpPut("{Id}")]
        public async Task<IActionResult> Update(int? Id, [FromForm] UserPostRequest userPostRequest)
        {
            if (Id == null) return BadRequest();
            if (userPostRequest == null) return BadRequest();
            User user = _unitOfWork.User.GetFirstOrDefault(u => u.Id == Id);
            if (user == null) return NotFound();
            _unitOfWork.User.Update(user, userPostRequest);
            await _unitOfWork.SaveAsync();
            return StatusCode(StatusCodes.Status202Accepted);
        }

        // Delete a user
        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(int? Id)
        {
            if (Id == null) return BadRequest();
            var User = _unitOfWork.User.GetFirstOrDefault(u => u.Id == Id);
            if (User == null) return NotFound();
            _unitOfWork.User.Remove(User);
            _unitOfWork.SaveAsync();
            return StatusCode(StatusCodes.Status202Accepted);
        }

        // Forgot password request handler
        // How it works: the request comes with the user's email. The request handler will check if there is any user in the database
        // having that email. If there is no one, then the response will be false. Conversely, the response will be true.
        [HttpGet("[action]")]
        public async Task<IActionResult> ForgotPassword(string? Email)
        {
            if (Email == null) return BadRequest();
            User user = _unitOfWork.User.GetFirstOrDefault(u => u.Email == Email);
            if (user == null) return NotFound();
            // Send email to the user
            return Ok();
        }

        // Change password of a user given his/her email and a new password
        [HttpPut("[action]")]
        public async Task<IActionResult> ChangePassword(string Email, string NewPassword)
        {
            if (Email == null || NewPassword == null) return BadRequest();
            var user = _unitOfWork.User.GetFirstOrDefault(u => u.Email == Email);
            user.Password = NewPassword;
            _unitOfWork.SaveAsync();
            return StatusCode(StatusCodes.Status202Accepted);
        }

    }
}
