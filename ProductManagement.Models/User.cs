using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool? IsInActive { get; set; } = false;
        public string Email { get; set; }
        [NotMapped]
        public IFormFile? Avatar { get; set; }
        public string? AvatarUrl { get; set; }
    }
}
