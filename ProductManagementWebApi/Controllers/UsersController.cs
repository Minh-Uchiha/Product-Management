using Azure.Storage.Blobs;
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
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        public UsersController(ApplicationDbContext db)
        {
            _db = db;
        }

        // Get simplified information of all users
        [HttpGet]
        public async Task<IActionResult> GetAllUsers(int? PageSize, int? PageNumber)
        {
            var CurrPageSize = PageSize ?? 10;
            var CurrPageNumber = PageNumber ?? 1;
            var users = await (from user in _db.Users
                               select new
                               {
                                   user.Id,
                                   user.UserName,
                                   Avatar = user.AvatarUrl
                               }).ToListAsync();
            return Ok(users.Skip(CurrPageSize * (CurrPageNumber - 1)).Take(CurrPageSize));
        }

        // Sign in (Get detailed information about a user with a specific id)
        [HttpGet("{Id}")]
        public async Task<IActionResult> SignIn(int? Id)
        {
            if (Id == null) return BadRequest();
            var user = await _db.Users.FirstOrDefaultAsync(x => x.Id == Id);
            return Ok(user);
        }

        // Get the information of all the users in ascending order (by name)
        [HttpGet("[action]")]
        public async Task<IActionResult> GetSortedAscending()
        {
            var users = await (from user in _db.Users
                               select new
                               {
                                   user.Id,
                                   user.UserName,
                                   Avatar = user.AvatarUrl
                               }).OrderBy(u => u.UserName).ToListAsync();
            return Ok(users);
        }

        // Get the information of all the users in descending order (by name)
        [HttpGet("[action]")]
        public async Task<IActionResult> GetSortedDescending()
        {
            var users = await (from user in _db.Users
                               select new
                               {
                                   user.Id,
                                   user.UserName,
                                   Avatar = user.AvatarUrl
                               }).OrderByDescending(u => u.UserName).ToListAsync();
            return Ok(users);
        }

        // Sign Up (Create a new User)
        [HttpPost]
        public async Task<IActionResult> SignUp([FromForm] User User)
        {
            if (User == null) return BadRequest();
            User.AvatarUrl = await FileHelper.UploadUserImage(User.Avatar);
            await _db.Users.AddAsync(User);
            await _db.SaveChangesAsync();
            return StatusCode(StatusCodes.Status201Created);
        }

        // Update a user information given his/her id
        [HttpPut("{Id}")]
        public async Task<IActionResult> Update(int? Id, [FromForm] User user)
        {
            if (Id == null || user == null) return BadRequest();
            user.Id = (int)Id;
            _db.Users.Update(user);
            await _db.SaveChangesAsync();
            return StatusCode(StatusCodes.Status202Accepted);
        }

        // Delete a user
        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(int? Id)
        {
            if (Id == null) return BadRequest();
            var User = await _db.Users.FirstOrDefaultAsync(u => u.Id == Id);
            _db.Users.Remove(User);
            await _db.SaveChangesAsync();
            return StatusCode(StatusCodes.Status202Accepted);
        }

        // Get the users who are inactive
        [HttpGet("[action]")]
        public async Task<IActionResult> InActive()
        {
            var users = await _db.Users.Where(u => u.IsInActive == true).ToListAsync();
            return Ok(users);
        }

        // Search for users whose names start with a given text
        [HttpGet("[action]")]
        public async Task<IActionResult> Search(string? query)
        {
            query = query ?? "";
            var users = await _db.Users.Where(u => u.UserName.StartsWith(query)).ToListAsync();
            return Ok(users);
        }

        // Forgot password request handler
        // How it works: the request comes with the user's email. The request handler will check if there is any user in the database
        // having that email. If there is no one, then the response will be false. Conversely, the response will be true.
        [HttpGet("[action]")]
        public async Task<IActionResult> ForgotPassword(string? Email)
        {
            if (Email == null) return BadRequest();
            bool EmailExisted = _db.Users.Any(u => u.Email == Email);
            return Ok(EmailExisted);
        }

        // Change password of a user knowing the his/her id
        [HttpPut("[action]/{Id}")]
        public async Task<IActionResult> ChangePassword(int? Id, string? NewPassword)
        {
            if (NewPassword == null || Id == null) return BadRequest();
            var user = _db.Users.FirstOrDefault(u => u.Id == Id);
            user.Password = NewPassword;
            await _db.SaveChangesAsync();
            return StatusCode(StatusCodes.Status202Accepted);
        }



    }
}
