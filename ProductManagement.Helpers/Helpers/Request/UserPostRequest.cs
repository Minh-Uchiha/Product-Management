using Microsoft.AspNetCore.Http;
using ProductManagement.Models;
using ProductManagementWebApi.Helpers.Interfaces;

namespace ProductManagementWebApi.Helpers.Request
{
    public class UserPostRequest
    {
        public string UserName { get; set; }
        public string? Password { get; set; }
        public bool? IsInActive { get; set; } = false;
        public string? Email { get; set; }
        public IFormFile? Avatar { get; set; }
    }
}
